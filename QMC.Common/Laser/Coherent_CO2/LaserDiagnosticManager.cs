using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Laser.Coherent_CO2
{
    public class LaserDiagnosticManager : Part
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private string _ip;
        private int _port;
        private object _lock = new object();

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public LaserDiagnosticManager(string name, string ip, int port = 5000) : base(name)
        {
            _ip = ip;
            _port = port;
        }

        public bool Connect()
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_ip, _port);
                _stream = _tcpClient.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                Log.Write("Laser", "Connect", $"TCP 연결 실패: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _tcpClient?.Close();
            }
            catch (Exception ex)
            {
                Log.Write("Laser", "Disconnect", $"연결 해제 중 오류: {ex.Message}");
            }
        }

        public ControllerLongStatus RequestControllerStatus()
        {
            lock (_lock)
            {
                try
                {
                    if (!IsConnected)
                        return null;

                    byte[] request = Encoding.ASCII.GetBytes("085|000|000\r\n"); // Controller + Long Status Request
                    _stream.Write(request, 0, request.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);

                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    return ParseControllerStatus(response);
                }
                catch (Exception ex)
                {
                    Log.Write("Laser", "RequestControllerStatus", $"요청 실패: {ex.Message}");
                    return null;
                }
            }
        }

        private ControllerLongStatus ParseControllerStatus(string response)
        {
            try
            {
                string[] parts = response.Trim().Split('|');
                if (parts.Length < 15 || parts[0] != "085")
                    return null;

                byte[] data = parts.Skip(3).Take(15).Select(p => byte.TryParse(p, out byte b) ? b : (byte)0).ToArray();
                if (data.Length < 15)
                    return null;

                byte status1 = data[4];
                byte status2 = data[5];

                return new ControllerLongStatus
                {
                    Voltage48V = data[0],
                    LaserTempKnife = data[1],
                    ShutterTempBeamDump = data[2],
                    ShutterTempBlade = data[3],

                    VswrLimit = (status1 & (1 << 0)) != 0,
                    TempFault = (status1 & (1 << 1)) != 0,
                    SystemFault = (status1 & (1 << 2)) != 0,
                    Enable = (status1 & (1 << 3)) != 0,
                    DutyCycleLimit = (status1 & (1 << 4)) != 0,
                    ShutterFault = (status1 & (1 << 5)) != 0,
                    ShutterInterlock = (status1 & (1 << 6)) != 0,
                    SystemInterlock = (status1 & (1 << 7)) != 0,

                    ShutterClosed = (status2 & (1 << 0)) != 0,
                    SimmerDischargeLoss = (status2 & (1 << 1)) != 0,
                    SimmerFeedbackLoss = (status2 & (1 << 2)) != 0,
                    ModeSelect = (status2 & (1 << 3)) != 0,
                    FeatureOverride = (status2 & (1 << 4)) != 0,

                    ModulationFreqKHz = data[6],
                    ModulationFreqHz = data[7],
                    DutyCyclePercent = data[8] / 10.0,
                    DewPoint = data[9],
                    LaserPowerWatt = data[10] * 256 + data[11]
                };
            }
            catch (Exception ex)
            {
                Log.Write("Laser", "ParseControllerStatus", $"파싱 실패: {ex.Message}");
                return null;
            }
        }
    }

    public class ControllerLongStatus
    {
        public int Voltage48V { get; set; }
        public int LaserTempKnife { get; set; }
        public int ShutterTempBeamDump { get; set; }
        public int ShutterTempBlade { get; set; }

        public bool VswrLimit { get; set; }
        public bool TempFault { get; set; }
        public bool SystemFault { get; set; }
        public bool Enable { get; set; }
        public bool DutyCycleLimit { get; set; }
        public bool ShutterFault { get; set; }
        public bool ShutterInterlock { get; set; }
        public bool SystemInterlock { get; set; }

        public bool ShutterClosed { get; set; }
        public bool SimmerDischargeLoss { get; set; }
        public bool SimmerFeedbackLoss { get; set; }
        public bool ModeSelect { get; set; }
        public bool FeatureOverride { get; set; }

        public int ModulationFreqKHz { get; set; }
        public int ModulationFreqHz { get; set; }
        public double DutyCyclePercent { get; set; }
        public int DewPoint { get; set; }
        public int LaserPowerWatt { get; set; }
    }
}
