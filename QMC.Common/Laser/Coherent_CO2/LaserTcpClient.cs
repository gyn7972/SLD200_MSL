using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace QMC.Common.Laser.Coherent_CO2
{
    public class LaserTcpClient : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly string _ip;
        private readonly int _port;
        private readonly object _lock = new object();
        private readonly string _logFile = "LaserCommLog.txt";

        public bool IsConnected => _client?.Connected ?? false;
        public event Action<string> OnAlarmRaised;

        public LaserTcpClient(string ip, int port = 5000)
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
                Log($"Connected to laser ({_ip}:{_port}).");
                return true;
            }
            catch (Exception ex)
            {
                RaiseAlarm($"TCP connect failed: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
                Log("Disconnected.");
            }
            catch { }
        }

        public bool SendAscii(string cmd)
        {
            if (!IsConnected || _stream == null)
            {
                RaiseAlarm("Stream not writable.");
                return false;
            }

            lock (_lock)
            {
                try
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(cmd + "\r\n");
                    _stream.Write(bytes, 0, bytes.Length);
                    Log($">> {cmd}");
                    return true;
                }
                catch (Exception ex)
                {
                    RaiseAlarm($"Send failed: {ex.Message}");
                    return false;
                }
            }
        }

        public string ReadAscii(int timeoutMs = 3000)
        {
            if (_stream == null)
                return null;

            try
            {
                _stream.ReadTimeout = timeoutMs;
                byte[] buffer = new byte[2048];
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0)
                    return null;

                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Log($"<< {response}");
                return response;
            }
            catch (IOException ex)
            {
                RaiseAlarm($"Read timeout or error: {ex.Message}");
                return null;
            }
        }

        private void Log(string msg)
        {
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}";
            File.AppendAllText(_logFile, line + Environment.NewLine);
        }

        private void RaiseAlarm(string msg)
        {
            Log("[ALARM] " + msg);
            OnAlarmRaised?.Invoke(msg);
        }

        public void Dispose() => Disconnect();
    }
}
