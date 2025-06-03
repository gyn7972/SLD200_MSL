using System;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace QMC.Common.Parts
{
    public class DustCollectorController : Part
    {
        public enum CollectorPosition { Upper, Lower }

        public enum CollectorRunState { Unknown, Stopped, Running }

        private SerialPort _serialPort;
        private readonly object _lock = new object();
        private string _lastReceivedData = "";
        private bool _dataReceived = false;
        private readonly ManualResetEvent _receiveEvent = new ManualResetEvent(false);

        public bool IsConnected => _serialPort?.IsOpen ?? false;

        public DustCollectorController(string name, DustCollectorController.CollectorPosition lower) : base(name) { }

        public bool Connect(Equipment.CommList comm)
        {
            string strPortName;
            int baudRate, dataBits;
            StopBits stopBits;
            Parity parity;
            Handshake handshake;

            Equipment.GetSerialPortConfig(comm, out strPortName, out baudRate, out dataBits, out stopBits, out parity, out handshake);
            return Connect(strPortName, baudRate, dataBits, stopBits, parity, handshake);
        }

        public bool Connect(Equipment.CommList commList)
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    return true;
            }

            Equipment.GetSerialPortConfig(commList, out string portName, out int baudRate, out int dataBits, out StopBits stopBits, out Parity parity, out Handshake handshake);
            return Connect(portName, baudRate, dataBits, stopBits, parity, handshake);
        }

        public bool Connect(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity, Handshake handshake)
        {
            try
            {
                _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
                {
                    Encoding = Encoding.ASCII,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000
                };
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                Log.Write("DustCollector", $"[G100] Port Open Fail: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            if (_serialPort?.IsOpen == true)
                _serialPort.Close();
            _serialPort?.Dispose();
            _serialPort = null;
        }

        


        public bool Start() => SendWrite("0006", "0002"); // 운전 시작
        public bool Stop() => SendWrite("0006", "0001");  // 운전 정지

        public bool SetFrequency(double freqHz)
        {
            int val = (int)(freqHz * 10.0);
            return SendWrite("0005", val.ToString("D4")); // 주파수 설정
        }

        public bool GetFrequency(out double freqHz)
        {
            freqHz = 0.0;
            if (!SendRead("000A", out string raw))
                return false;

            if (int.TryParse(ExtractData(raw), System.Globalization.NumberStyles.HexNumber, null, out int hex))
            {
                freqHz = hex / 10.0;
                return true;
            }
            return false;
        }

        public CollectorRunState GetRunState()
        {
            if (!SendRead("0007", out string raw))
                return CollectorRunState.Unknown;

            string data = ExtractData(raw);
            switch (data)
            {
                case "0001": return CollectorRunState.Stopped;
                case "0002": return CollectorRunState.Running;
                default: return CollectorRunState.Unknown;
            }
        }

        private bool SendWrite(string address, string data)
        {
            string cmd = BuildCommand('W', address, data);
            return SendAndWaitForAck(cmd);
        }

        private bool SendRead(string address, out string response)
        {
            string cmd = BuildCommand('R', address, "1");
            bool result = SendAndWaitForAck(cmd);
            response = result ? _lastReceivedData : "";
            return result;
        }

        private string BuildCommand(char cmd, string addr, string data)
        {
            string body = "01" + cmd + addr + data;
            int sum = 0;
            foreach (char c in body)
                sum += c;
            string checksum = (sum & 0xFF).ToString("X2");
            return ((char)0x05) + body + checksum + ((char)0x04);
        }

        private bool SendAndWaitForAck(string cmd)
        {
            if (!IsConnected) return false;

            lock (_lock)
            {
                _receiveEvent.Reset();
                _dataReceived = false;
                _lastReceivedData = "";

                try
                {
                    _serialPort.DiscardInBuffer();
                    _serialPort.Write(cmd);
                    if (_receiveEvent.WaitOne(1000))
                        return _dataReceived;
                }
                catch (Exception ex)
                {
                    Log.Write("DustCollector", $"[G100] Comm Error: {ex.Message}");
                }
                return false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                _lastReceivedData += _serialPort.ReadExisting();
                if (_lastReceivedData.Contains(((char)0x04).ToString()))
                {
                    _dataReceived = _lastReceivedData.StartsWith(((char)0x06).ToString());
                    _lastReceivedData = _lastReceivedData.Trim((char)0x06, (char)0x04);
                    _receiveEvent.Set();
                }
            }
            catch (Exception ex)
            {
                Log.Write("DustCollector", $"[G100] RX Error: {ex.Message}");
            }
        }

        private string ExtractData(string response)
        {
            if (string.IsNullOrWhiteSpace(response) || response.Length < 4)
                return "";
            return response.Substring(response.Length - 4);
        }
    }
}
