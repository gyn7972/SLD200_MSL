using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Linq;
using netDxf;

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

        private readonly string _stationId;

        private static readonly double[] DustCollector_FrequencyDataArray = new double[]
        {
            59, 58.1, 57.9, 56, 53, 52, 50.9, 50,
            49, 48, 47, 46.1, 45.5, 44, 43,
            41, 40, 39.9, 29, 26.5, 26, 25.9,
            23.6, 23.2, 22, 21.5, 21.2, 20
        };

        public DustCollectorController(string name, CollectorPosition position, string stationId = "01") : base(name)
        {
            _stationId = stationId;
        }

        public void Close()
        {
            Disconnect();
        }

        public bool Connect(Equipment.CommList comm)
        {
            Equipment.GetSerialPortConfig(comm, out string strPortName, out int baudRate, out int dataBits, out StopBits stopBits, out Parity parity, out Handshake handshake);
            return Connect(strPortName, baudRate, dataBits, stopBits, parity, handshake);
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
                Log.Write(ex);
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

        public bool Start()
        {
            bool ok = true;
            Thread.Sleep(10);
            ok &= SendWrite("0006", "0002");
            Thread.Sleep(10);
            ok &= SendWrite("0007", "0001");
            //Thread.Sleep(10);
            //ok &= SetFrequency(60);
            return ok;
        }

        public bool Stop()
        {
            bool ok = true;
            ok &= SendWrite("0006", "0001");
            return ok;
        }

        public bool SetFrequency(double freqHz)
        {
            int val = (int)(freqHz * 100.0);
            string hexVal = val.ToString("X4");
            return SendWrite("0380", hexVal);
        }

        public bool GetFrequency(out double freqHz)
        {
            freqHz = 0.0;
            if (!SendRead("0005", out string raw))
                return false;

            string data = ExtractData(raw);
            if (int.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out int hex))
            {
                freqHz = hex * 0.01;
                return true;
            }
            return false;
        }

        public CollectorRunState GetRunState()
        {
            if (!SendRead("000E", out string raw))
                return CollectorRunState.Unknown;

            string data = ExtractData(raw);
            if (!ushort.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out ushort value))
                return CollectorRunState.Unknown;

            string result = ParseInverterRunState(data);
            //Log.Write("DustCollector", $"운전 상태: {result}");

            bool isRunning = (value & (1 << 0)) != 0;
            return isRunning ? CollectorRunState.Running : CollectorRunState.Stopped;
        }

        private bool SendWrite(string address, string data)
        {
            string cmd = BuildWriteCommand(address, data, 1);

            //Log.Write("DustCollector", $"[TX] {cmd}");

            return SendAndWaitForAck(cmd);
        }

        private bool SendRead(string address, out string response)
        {
            string cmd = BuildReadCommand(address, 1);

            //Log.Write("DustCollector", $"[TX] {cmd}");

            bool result = SendAndWaitForAck(cmd);
            response = result ? _lastReceivedData : "";
            return result;
        }

        private string BuildReadCommand(string addr, int wordCount)
        {
            string body = _stationId + "R" + addr + wordCount.ToString();
            int sum = body.Sum(c => (byte)c);
            string checksum = (sum & 0xFF).ToString("X2");
            return ((char)0x05) + body + checksum + ((char)0x04);
        }

        private string BuildWriteCommand(string addr, string data, int wordCount)
        {
            string body = _stationId + "W" + addr + wordCount.ToString() + data;
            int sum = body.Sum(c => (byte)c);
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
                    {
                        //Log.Write("DustCollector", $"[RX] {_lastReceivedData}");
                        return _dataReceived;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
                return false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                _lastReceivedData += _serialPort.ReadExisting();
                //Log.Write("DustCollector", $"[RAW RX] {_lastReceivedData}");

                // 응답은 항상 STX(0x06)로 시작, EOT(0x04)로 끝남
                int start = _lastReceivedData.IndexOf((char)0x06);
                int end = _lastReceivedData.IndexOf((char)0x04, start + 1);

                if (start >= 0 && end > start)
                {
                    string packet = _lastReceivedData.Substring(start + 1, end - start - 1); // 사이 내용만
                    _lastReceivedData = packet;
                    _dataReceived = true;
                    _receiveEvent.Set();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            //try
            //{
            //    _lastReceivedData += _serialPort.ReadExisting();
            //    if (_lastReceivedData.Contains(((char)0x04).ToString()))
            //    {
            //        _dataReceived = _lastReceivedData.StartsWith(((char)0x06).ToString());
            //        _lastReceivedData = _lastReceivedData.Trim((char)0x06, (char)0x04);
            //        _receiveEvent.Set();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Write("DustCollector", $"[G100] RX Error: {ex.Message}");
            //}
        }

        private string ExtractData(string response)
        {
            if (string.IsNullOrWhiteSpace(response) || response.Length < 4)
                return "";
            return response.Substring(response.Length - 4);
        }

        public string ParseInverterRunState(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || hex.Length != 4)
                return $"[Invalid] 응답값 오류: '{hex}'";

            if (!ushort.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out ushort value))
                return $"[Invalid] 숫자 변환 실패: '{hex}'";

            bool isRunning = (value & (1 << 0)) != 0;
            bool isFreqSet = (value & (1 << 1)) != 0;
            bool isWarning = (value & (1 << 2)) != 0;
            bool isTrip = (value & (1 << 3)) != 0;

            return $"[Raw: {hex}] Run: {(isRunning ? "O" : "X")}, FreqSet: {(isFreqSet ? "O" : "X")}, Warning: {(isWarning ? "O" : "X")}, Trip: {(isTrip ? "O" : "X")}";
        }
    }
}
