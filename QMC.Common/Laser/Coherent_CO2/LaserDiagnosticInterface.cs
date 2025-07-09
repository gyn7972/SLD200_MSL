using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Laser.Coherent_CO2.LaserTcpClient;

namespace QMC.Common.Laser.Coherent_CO2
{
    public class LaserDiagnosticInterface
    {
        private readonly LaserTcpClient _client;

        public LaserDiagnosticInterface(string ip, int port = 23)
        {
            _client = new LaserTcpClient(ip, port);
            _client.OnAlarmRaised += (msg) => RaiseAlarm?.Invoke(msg);
        }

        public event Action<string> RaiseAlarm;

        public bool Connect() => _client.Connect();
        public void Disconnect() => _client.Disconnect();

        public string GetStatus()
        {
            if (!_client.SendCommand(LaserCommand.StatusRequest))
                return "Failed to send StatusRequest command.";

            var response = _client.ReadResponse();
            if (!LaserResponseParser.ValidateChecksum(response))
            {
                RaiseAlarm?.Invoke("Status response checksum error.");
                return "Invalid status response (checksum error).";
            }

            return LaserResponseParser.ParseStatusResponse(response);
        }

        public string GetTemperature()
        {
            if (!_client.SendCommand(LaserCommand.ReadTemperature))
                return "Failed to send ReadTemperature command.";

            var response = _client.ReadResponse();
            if (!LaserResponseParser.ValidateChecksum(response))
            {
                RaiseAlarm?.Invoke("Temperature response checksum error.");
                return "Invalid temperature response (checksum error).";
            }

            return LaserResponseParser.ParseTemperatureResponse(response);
        }

        public string GetFaultStatus()
        {
            if (!_client.SendCommand(LaserCommand.ReadErrorLog))
                return "Failed to send ReadErrorLog command.";

            var response = _client.ReadResponse();
            if (!LaserResponseParser.ValidateChecksum(response))
            {
                RaiseAlarm?.Invoke("Fault response checksum error.");
                return "Invalid fault response (checksum error).";
            }

            return LaserResponseParser.ParseFaultResponse(response);
        }

        public bool ClearFault()
        {
            if (!_client.SendCommand(LaserCommand.ClearFault))
            {
                RaiseAlarm?.Invoke("ClearFault command failed to send.");
                return false;
            }

            var response = _client.ReadResponse();
            if (!LaserResponseParser.ValidateChecksum(response))
            {
                RaiseAlarm?.Invoke("ClearFault response checksum error.");
                return false;
            }

            return true;
        }
    }
}
