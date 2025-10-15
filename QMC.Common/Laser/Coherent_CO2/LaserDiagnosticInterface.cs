using System;
using System.Threading;

namespace QMC.Common.Laser.Coherent_CO2
{
    /// <summary>
    /// 상위 모듈에서 사용되는 레이저 진단 인터페이스.
    /// 내부적으로 LaserDiagnosticManager(Part 상속)를 주기적으로 폴링하여
    /// 상태 및 알람 정보를 이벤트로 전달한다.
    /// </summary>
    public class LaserDiagnosticInterface : IDisposable
    {
        private readonly LaserDiagnosticManager _mgr;
        private Thread _thread;
        private bool _running;
        private readonly object _sync = new object();

        public event Action<string> OnLog;
        public event Action<string> OnAlarm;

        public int PollIntervalMs { get; set; } = 2000;


        /*
         * var laserInterface = new LaserDiagnosticInterface("LaserDiag", "192.168.0.50", 5000);

        laserInterface.OnLog += msg => Console.WriteLine("[STATUS] " + msg);
        laserInterface.OnAlarm += msg => Console.WriteLine("[ALARM] " + msg);

        if (laserInterface.Start())
            Console.WriteLine("Laser monitor started.");

        // ...
        // 종료 시
        laserInterface.Stop();
        */
        /// <summary>
        /// 생성자: 장비명, IP, Port 지정
        /// </summary>
        public LaserDiagnosticInterface(string name, string ip, int port = 5000)
        {
            _mgr = new LaserDiagnosticManager(name, ip, port);
            _mgr.OnAlarmRaised += delegate (string msg)
            {
                if (OnAlarm != null)
                    OnAlarm(msg);
            };
        }

        /// <summary>
        /// 주기적 모니터링 시작
        /// </summary>
        public bool Start()
        {
            lock (_sync)
            {
                if (_running)
                    return false;

                if (!_mgr.Connect())
                {
                    if (OnAlarm != null)
                        OnAlarm("Laser connection failed.");
                    return false;
                }

                _running = true;
                _thread = new Thread(MonitorLoop);
                _thread.IsBackground = true;
                _thread.Start();
                return true;
            }
        }

        /// <summary>
        /// 모니터링 종료
        /// </summary>
        public void Stop()
        {
            lock (_sync)
            {
                _running = false;
            }

            if (_thread != null && _thread.IsAlive)
            {
                try
                {
                    _thread.Join(1000);
                }
                catch { }
            }

            _mgr.Disconnect();
        }

        private void MonitorLoop()
        {
            while (_running)
            {
                try
                {
                    var st = _mgr.GetStatus();
                    if (st != null && OnLog != null)
                        OnLog(st.ToString());

                    var faults = _mgr.GetFaults();
                    if (faults != null)
                    {
                        foreach (var f in faults)
                        {
                            if (OnAlarm != null)
                                OnAlarm(f.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (OnAlarm != null)
                        OnAlarm("MonitorLoop exception: " + ex.Message);

                    // 통신 예외 발생 시 강제 종료 처리
                    _running = false;
                    try { _mgr.Disconnect(); } catch { }
                }

                Thread.Sleep(PollIntervalMs);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
