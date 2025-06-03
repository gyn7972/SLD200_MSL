using System;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace QMC.Common.Parts
{
    public class DustCollectorController : Part
    {
        public enum CollectorRunState { Unknown, Stopped, Running }
        public enum CollectorAlarmState { None, Warning, Alarm, Unknown }
        public enum CollectorPosition { Upper, Lower }

        private SerialPort _serialPort;
        private readonly CollectorPosition _position;
        private readonly object _lock = new object();
        private string _lastReceivedData = "";
        private bool _dataReceived = false;
        private readonly ManualResetEvent _receiveEvent = new ManualResetEvent(false);

        public bool IsConnected => _serialPort?.IsOpen ?? false;

        public DustCollectorController(string name, CollectorPosition position) : base(name)
        {
            _position = position;
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
                    Handshake = handshake,
                    Encoding = Encoding.Default,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000
                };
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                Log.Write("DustCollector", $"[{_position}] Port Open Fail: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();

                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        public bool SendWrite(string address, string data, int addressCount = 1)
        {
            string command = BuildWriteCommand(address, data, addressCount);
            return SendAndWaitForAck(command);
        }

        public bool SendRead(string address, int addressCount, out string response)
        {
            response = "";
            string command = BuildReadCommand(address, addressCount);
            bool success = SendAndWaitForAck(command);
            if (success)
                response = _lastReceivedData;
            return success;
        }

        public bool SendRead(string address, out string response) => SendRead(address, 1, out response);

        private bool SendAndWaitForAck(string command)
        {
            if (!IsConnected)
                return false;

            lock (_lock)
            {
                _receiveEvent.Reset();
                _lastReceivedData = "";
                _dataReceived = false;

                _serialPort.DiscardInBuffer();
                _serialPort.Write(command);

                if (_receiveEvent.WaitOne(1000))
                    return _dataReceived;

                Log.Write("DustCollector", $"[{_position}] Timeout waiting for ACK.");
                return false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string received = _serialPort.ReadExisting();
                _lastReceivedData += received;
                Log.Write("DustCollector", $"[{_position}] [RX] {_lastReceivedData}");

                // EOT까지 수신된 경우
                if (_lastReceivedData.Contains(((char)0x04).ToString()))
                {
                    // Write 응답: ACK + EOT
                    if (_lastReceivedData.StartsWith(((char)0x06).ToString()))
                    {
                        _lastReceivedData = _lastReceivedData.Trim((char)0x06, (char)0x04);
                        _dataReceived = true;
                    }
                    // Read 응답: ENQ + 01Rxxxx + CHKSUM + EOT
                    else if (_lastReceivedData.StartsWith(((char)0x05).ToString() + "01R"))
                    {
                        // → "␅01RFE3E␄"
                        string raw = _lastReceivedData.Trim((char)0x05, (char)0x04); // ENQ, EOT 제거
                        _lastReceivedData = raw.Substring(3, raw.Length - 5);        // "01RFE3E" → "FE"
                        _dataReceived = true;
                    }
                    else
                    {
                        _dataReceived = false;
                    }

                    _receiveEvent.Set();
                }
            }
            catch (Exception ex)
            {
                Log.Write("DustCollector", $"[{_position}] Data Receive Error: {ex.Message}");
            }
        }


        private string BuildWriteCommand(string addr, string data, int addrCount)
        {
            var sb = new StringBuilder();
            sb.Append("01W").Append(addr).Append((char)('0' + addrCount)).Append(data);
            return AppendChecksumAndEOT(sb.ToString());
        }

        private string BuildReadCommand(string addr, int addrCount)
        {
            var sb = new StringBuilder();
            sb.Append("01R").Append(addr).Append((char)('0' + addrCount));
            return AppendChecksumAndEOT(sb.ToString());
        }

        private string AppendChecksumAndEOT(string coreCommand)
        {
            int checksum = 0;
            foreach (char ch in coreCommand)
                checksum += (byte)ch;

            // 하위 8비트만 사용
            byte checksumByte = (byte)(checksum & 0xFF);
            string checksumHex = checksumByte.ToString("X2"); // 항상 2자리

            // STX (0x05) + 본문 + 체크섬 + EOT (0x04)
            return ((char)0x05) + coreCommand + checksumHex + ((char)0x04);
        }

        //private string ExtractResponseData(string fullResponse)
        //{
        //    // 예: fullResponse = "FE" ← 이미 전처리된 상태로 들어옴
        //    return fullResponse.Trim();
        //}
        private string ExtractResponseData(string fullResponse)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fullResponse) || fullResponse.Length < 13)
                    return "";

                // 포맷: 01R + Addr(4) + Data(4) + Checksum(2)
                // Index:        3     ~     7     = Data
                return fullResponse.Substring(7, 4);  // 4자리 데이터만 정확히 추출
            }
            catch
            {
                return "";
            }
        }

        private double ConvertHexToDouble(string hex, double divider)
        {
            return int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int val) ? val / divider : 0.0;
        }

        public bool GetFullStatus(out CollectorRunState runState, out double frequencyHz, out double currentA, out CollectorAlarmState alarmState)
        {
            runState = CollectorRunState.Unknown;
            frequencyHz = 0.0;
            currentA = 0.0;
            alarmState = CollectorAlarmState.Unknown;

            if (!SendRead("1000", 20, out string raw))
                return false;

            string data = ExtractResponseData(raw);
            if (data.Length < 40)
                return false;

            try
            {
                runState = data.Substring(20, 4) == "0001" ? CollectorRunState.Running : CollectorRunState.Stopped;
                frequencyHz = ConvertHexToDouble(data.Substring(16, 4), 10.0);
                currentA = ConvertHexToDouble(data.Substring(4, 4), 10.0);
                //alarmState = data.Substring(24, 4) switch
                //{
                //    "0000" => CollectorAlarmState.None,
                //    "0001" => CollectorAlarmState.Warning,
                //    "0002" => CollectorAlarmState.Alarm,
                //    _ => CollectorAlarmState.Unknown
                //};
                string alarmCode = data.Substring(24, 4);
                if (alarmCode == "0000")
                    alarmState = CollectorAlarmState.None;
                else if (alarmCode == "0001")
                    alarmState = CollectorAlarmState.Warning;
                else if (alarmCode == "0002")
                    alarmState = CollectorAlarmState.Alarm;
                else
                    alarmState = CollectorAlarmState.Unknown;
                return true;
            }
            catch (Exception ex)
            {
                Log.Write("DustCollector", $"[Parser Error] FullStatus: {ex.Message}");
                return false;
            }
        }

        public bool DustCollector_On() => SendWrite("0006", "0002", 1);
        public bool DustCollector_Off() => SendWrite("0006", "0001", 1);

        public bool SetFrequency(double frequencyHz)
        {
            int freqValue = (int)(frequencyHz * 100.0);
            return SendWrite("0005", freqValue.ToString("D4"), 1);
        }

        public bool GetFrequency(out double frequencyHz)
        {
            frequencyHz = 0.0;
            if (!SendRead("000A", 1, out string freqRaw))
                return false;

            string data = ExtractResponseData(freqRaw);
            return int.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out int freqVal)
                   && (frequencyHz = freqVal / 10.0) >= 0;
        }

        public bool GetOutputFrequency(out double frequencyHz)
        {
            frequencyHz = 0.0;
            if (!SendRead("1002", 1, out string raw))  // 주소는 예시입니다
                return false;

            string data = ExtractResponseData(raw);
            if (int.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out int val))
            {
                frequencyHz = val / 10.0;
                return true;
            }
            return false;
        }
    }
}
