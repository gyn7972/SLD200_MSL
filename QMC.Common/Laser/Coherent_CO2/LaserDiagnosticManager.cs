using SpiralLab.Sirius;
using System;
using System.Collections.Generic;

namespace QMC.Common.Laser.Coherent_CO2
{
    /// <summary>
    /// Coherent DIAMOND J-5V-HD Diagnostic Manager
    /// Part 기반 상속 구조 (모션, 알람, DioPoint 통합 관리)
    /// </summary>
    public class LaserDiagnosticManager : Part, IDisposable
    {
        private readonly LaserTcpClient _client;
        private readonly object _sync = new object();

        public event Action<string> OnAlarmRaised;

        public string IpAddress { get; private set; }
        public int Port { get; private set; }

        public bool IsConnected
        {
            get { return _client != null && _client.IsConnected; }
        }

        // ─────────────────────────────────────────────────────────────
        // 생성자
        // ─────────────────────────────────────────────────────────────
        public LaserDiagnosticManager(string name, string ip, int port = 5000)
            : base(name) // ✅ Part(name) 반드시 호출
        {
            IpAddress = ip;
            Port = port;
            _client = new LaserTcpClient(ip, port);
            _client.OnAlarmRaised += delegate (string msg)
            {
                if (OnAlarmRaised != null)
                    OnAlarmRaised(msg);
            };
        }

        // ─────────────────────────────────────────────────────────────
        // 연결 관리
        // ─────────────────────────────────────────────────────────────
        public bool Connect()
        {
            bool ok = _client.Connect();
            if (!ok)
                RaiseAlarm("Laser connection failed");
            return ok;
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }

        // ─────────────────────────────────────────────────────────────
        // 상태 조회
        // ─────────────────────────────────────────────────────────────
        public ControllerStatus GetStatus()
        {
            lock (_sync)
            {
                if (!_client.SendAscii("085|000|000"))
                {
                    RaiseAlarm("Failed to send status request.");
                    return null;
                }

                string resp = _client.ReadAscii();
                if (string.IsNullOrEmpty(resp))
                {
                    //RaiseAlarm("No response for status request.");
                    return null;
                }

                return LaserResponseParser.ParseControllerStatus(resp);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Fault 조회
        // ─────────────────────────────────────────────────────────────
        public List<LaserFault> GetFaults()
        {
            lock (_sync)
            {
                if (!_client.SendAscii("085|001|000"))
                {
                    RaiseAlarm("Failed to send fault request.");
                    return null;
                }

                string resp = _client.ReadAscii();
                if (string.IsNullOrEmpty(resp))
                    return new List<LaserFault>();

                return LaserResponseParser.ParseFaultList(resp);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Fault 초기화
        // ─────────────────────────────────────────────────────────────
        public bool ClearFaults()
        {
            lock (_sync)
            {
                if (!_client.SendAscii("085|002|000"))
                {
                    RaiseAlarm("Failed to send ClearFault command.");
                    return false;
                }

                string resp = _client.ReadAscii();
                bool ok = !string.IsNullOrEmpty(resp) && resp.Contains("OK");
                if (!ok)
                    RaiseAlarm("Failed to clear laser fault.");
                return ok;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // 공통 Part 구조 연동 (Alarm, Close, Dispose)
        // ─────────────────────────────────────────────────────────────
        protected override void InitAlarm()
        {
            base.InitAlarm();
            // Laser 관련 추가 알람 등록 가능
            if (!m_dicAlarms.ContainsKey(1001))
            {
                Alarm commErr = new Alarm();
                commErr.Code = 1001;
                commErr.Title = "Laser Communication Error";
                commErr.Source = Name;
                commErr.Grade = "Error";
                commErr.Cause = "TCP connection lost or invalid response";
                m_dicAlarms.Add(commErr.Code, commErr);
            }
        }

        public override void Close()
        {
            base.Close();
            Disconnect();
        }

        public void Dispose()
        {
            Disconnect();
        }

        // ─────────────────────────────────────────────────────────────
        // 내부 유틸
        // ─────────────────────────────────────────────────────────────
        private void RaiseAlarm(string message)
        {
            LastError = message;
            if (OnAlarmRaised != null)
                OnAlarmRaised(message);
        }
    }
}
