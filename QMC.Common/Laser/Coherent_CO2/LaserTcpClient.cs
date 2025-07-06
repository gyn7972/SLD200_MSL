using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Laser.Coherent_CO2
{
    public class LaserTcpClient
    {
        public enum LaserCommand : byte
        {
            StatusRequest = 0xA1,
            ReadTemperature = 0xB0,
            ReadErrorLog = 0xC1,
            ClearFault = 0xC2,
            // Only diagnostic/status commands are implemented here
        }

        private TcpClient _client;
        private NetworkStream _stream;
        private readonly string _ip;
        private readonly int _port;
        private readonly object _lock = new object();
        private readonly string _logFilePath = "LaserCommLog.txt";

        public bool IsConnected => _client?.Connected ?? false;

        public event Action<string> OnAlarmRaised;

        public LaserTcpClient(string ip, int port = 23)
        {
            _ip = ip;
            _port = port;
        }

        public bool Connect()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(_ip, _port);
                _stream = _client.GetStream();
                Log("Connected to laser.");
                return true;
            }
            catch (Exception ex)
            {
                RaiseAlarm("Laser connection failed: " + ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
                Log("Disconnected from laser.");
            }
            catch { }
        }

        public bool SendCommand(LaserCommand command, byte[] data = null)
        {
            if (_stream == null || !_stream.CanWrite)
            {
                RaiseAlarm("Stream is not writable");
                return false;
            }

            lock (_lock)
            {
                try
                {
                    int dataLen = data?.Length ?? 0;
                    byte[] packet = new byte[3 + dataLen + 1];
                    packet[0] = 0x02; // STX
                    packet[1] = (byte)command;
                    packet[2] = (byte)dataLen;
                    if (dataLen > 0)
                        Array.Copy(data, 0, packet, 3, dataLen);

                    packet[packet.Length - 1] = CalculateChecksum(packet, packet.Length - 1);

                    _stream.Write(packet, 0, packet.Length);
                    Log($"SendCommand: {command}, Length: {dataLen}");
                    return true;
                }
                catch (Exception ex)
                {
                    RaiseAlarm("Command send failed: " + ex.Message);
                    return false;
                }
            }
        }

        public byte[] ReadResponse(int timeoutMs = 1000)
        {
            if (_stream == null || !_stream.CanRead)
            {
                RaiseAlarm("Stream is not readable");
                return null;
            }

            byte[] buffer = new byte[256];
            try
            {
                _stream.ReadTimeout = timeoutMs;
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                byte[] result = new byte[bytesRead];
                Array.Copy(buffer, result, bytesRead);
                Log("ReadResponse: " + BitConverter.ToString(result));
                return result;
            }
            catch (IOException ex)
            {
                RaiseAlarm("Response timeout or read error: " + ex.Message);
                return null;
            }
        }

        public bool SendAndCheck(LaserCommand cmd, byte[] data, Func<byte[], bool> validateResponse)
        {
            if (!SendCommand(cmd, data)) return false;
            var response = ReadResponse();
            if (response == null)
            {
                RaiseAlarm($"No response for command: {cmd}");
                return false;
            }
            return validateResponse(response);
        }

        private byte CalculateChecksum(byte[] data, int length)
        {
            byte sum = 0;
            for (int i = 0; i < length; i++)
                sum ^= data[i];
            return sum;
        }

        private void Log(string message)
        {
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(_logFilePath, line + Environment.NewLine);
            Console.WriteLine(line);
        }

        private void RaiseAlarm(string message)
        {
            Log("[ALARM] " + message);
            OnAlarmRaised?.Invoke(message);
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
