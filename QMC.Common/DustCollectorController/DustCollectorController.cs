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
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
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

        public bool SendRead(string address, out string response)
        {
            return SendRead(address, 1, out response);
        }

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
                else
                {
                    Log.Write("DustCollector", $"[{_position}] Timeout waiting for ACK.");
                    return false;
                }
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string received = _serialPort.ReadExisting();
                _lastReceivedData += received;

                if (_lastReceivedData.Contains(((char)0x04).ToString())) // EOT 도달
                {
                    if (_lastReceivedData.StartsWith(((char)0x06).ToString())) // ACK
                    {
                                    // 데이터는 1바이트 ACK + 실제 응답 본문
                        _lastReceivedData = _lastReceivedData.Trim((char)0x06, (char)0x04); // ACK, EOT 제거
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
            StringBuilder sb = new StringBuilder();
            sb.Append("01");
            sb.Append("W");
            sb.Append(addr);
            sb.Append((char)('0' + addrCount));
            sb.Append(data);
            return AppendChecksumAndEOT(sb.ToString());
        }

        private string BuildReadCommand(string addr, int addrCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("01");
            sb.Append("R");
            sb.Append(addr);
            sb.Append((char)('0' + addrCount));
            return AppendChecksumAndEOT(sb.ToString());
        }

        private string AppendChecksumAndEOT(string coreCommand)
        {
            int checksum = 0;
            foreach (char ch in coreCommand)
                checksum += (byte)ch;

            byte sumByte = (byte)(checksum & 0xFF);
            string checksumHex = sumByte.ToString("X2"); // 대문자 HEX, 항상 2자리

            // 완성된 전체 명령: ENQ + 본문 + 체크섬 + EOT
            return ((char)0x05).ToString() + coreCommand + checksumHex + ((char)0x04).ToString();
        }

        /// <summary>
        /// 수신된 전체 응답 문자열에서 데이터 필드만 추출합니다.
        /// 예: "01R00070002B5" → "0002"
        /// </summary>
        //private string ExtractResponseData(string fullResponse)
        //{
        //    // 예외 처리
        //    if (string.IsNullOrEmpty(fullResponse) || fullResponse.Length < 13)
        //        return "";

        //    // 포맷: 01R + 4자리 주소 + 4자리 데이터 + 2자리 체크섬 → 총 13자리
        //    return fullResponse.Substring(9, 4); // 10번째~13번째 문자 (Data 영역)
        //}

        private string ExtractResponseData(string fullResponse)
        {
            // 예: "01R009682" → "9682" 추출
            if (string.IsNullOrEmpty(fullResponse) || fullResponse.Length < 9)
                return "";

            return fullResponse.Substring(fullResponse.Length - 4, 4);
        }

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

        public bool DustCollector_On() => SendWrite("0006", "0002", 1);
        public bool DustCollector_Off() => SendWrite("0006", "0001", 1);

        public bool SetFrequency(double frequencyHz)
        {
            int freqValue = (int)(frequencyHz * 100.0);
            string asciiData = freqValue.ToString("D4");
            return SendWrite("0005", asciiData, 1);
        }

        public bool GetFrequency(out double frequencyHz)
        {
            frequencyHz = 0.0;
            if (!SendRead("000A", 1, out string freqRaw))
                return false;

            string data = ExtractResponseData(freqRaw);

            if (int.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out int freqVal))
            {
                frequencyHz = freqVal / 10.0;
                return true;
            }

            return false;
        }

        public bool GetCurrentStatus(out string status)
        {
            return SendRead("0007", 1, out status);
        }

        public CollectorRunState GetRunState()
        {
            if (!SendRead("0007", 1, out string statusRaw))
                return CollectorRunState.Unknown;

            string data = ExtractResponseData(statusRaw);
            switch (data)
            {
                case "0001": return CollectorRunState.Stopped;
                case "0002": return CollectorRunState.Running;
                default: return CollectorRunState.Unknown;
            }
        }

        public bool GetStatus(out CollectorRunState runState, out double frequencyHz)
        {
            runState = CollectorRunState.Unknown;
            frequencyHz = 0.0;

            if (!SendRead("0007", 1, out string statusRaw))
                return false;

            string statusData = ExtractResponseData(statusRaw);
            switch (statusData)
            {
                case "0001": runState = CollectorRunState.Stopped; break;
                case "0002": runState = CollectorRunState.Running; break;
            }

            if (!SendRead("000A", 1, out string freqRaw))
                return false;

            string freqData = ExtractResponseData(freqRaw);
            if (int.TryParse(freqData, System.Globalization.NumberStyles.HexNumber, null, out int freqValue))
                frequencyHz = freqValue / 10.0;

            return true;
        }


        public bool GetDetailedStatus(out CollectorRunState runState, out double frequencyHz, out double currentA, out CollectorAlarmState alarmState)
        {
            runState = CollectorRunState.Unknown;
            frequencyHz = 0.0;
            currentA = 0.0;
            alarmState = CollectorAlarmState.Unknown;

            bool ok = true;

            ok &= SendRead("0007", 1, out string statusRaw);
            string statusData = ExtractResponseData(statusRaw);
            switch (statusData)
            {
                case "0001": runState = CollectorRunState.Stopped; break;
                case "0002": runState = CollectorRunState.Running; break;
            }

            ok &= SendRead("000A", 1, out string freqRaw);
            string freqData = ExtractResponseData(freqRaw);
            if (int.TryParse(freqData, System.Globalization.NumberStyles.HexNumber, null, out int freqVal))
                frequencyHz = freqVal / 10.0;

            ok &= SendRead("000B", 1, out string currentRaw);
            string currentData = ExtractResponseData(currentRaw);
            if (int.TryParse(currentData, System.Globalization.NumberStyles.HexNumber, null, out int currentVal))
                currentA = currentVal / 10.0;

            ok &= SendRead("000C", 1, out string warnRaw);
            string warnData = ExtractResponseData(warnRaw);
            switch (warnData)
            {
                case "0000": alarmState = CollectorAlarmState.None; break;
                case "0001": alarmState = CollectorAlarmState.Warning; break;
                case "0002": alarmState = CollectorAlarmState.Alarm; break;
            }

            return ok;
        }

    }
}



//using System;
//using System.Text;
//using System.Threading;
//using System.IO.Ports;

//namespace QMC.Common.Parts
//{
//    public class DustCollectorController : Part
//    {
//        public enum CollectorRunState
//        {
//            Unknown,
//            Stopped,
//            Running
//        }

//        public enum CollectorAlarmState
//        {
//            None,
//            Warning,
//            Alarm,
//            Unknown
//        }

//        public enum CollectorPosition { Upper, Lower }

//        private SerialPort _serialPort;
//        private readonly CollectorPosition _position;
//        private readonly object _lock = new object();

//        private string _lastReceivedData = "";
//        private bool _dataReceived = false;
//        private readonly ManualResetEvent _receiveEvent = new ManualResetEvent(false);

//        public bool IsConnected => _serialPort?.IsOpen ?? false;

//        public DustCollectorController(string name, CollectorPosition position) : base(name)
//        {
//            _position = position;
//        }

//        public bool Connect(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity, Handshake handshake)
//        {
//            try
//            {
//                _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
//                {
//                    Handshake = handshake,
//                    Encoding = Encoding.Default,
//                    ReadTimeout = 1000,
//                    WriteTimeout = 1000
//                };
//                _serialPort.DataReceived += SerialPort_DataReceived;
//                _serialPort.Open();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Log.Write("DustCollector", $"[{_position}] Port Open Fail: {ex.Message}");
//                return false;
//            }
//        }

//        public void Disconnect()
//        {
//            if (_serialPort != null && _serialPort.IsOpen)
//            {
//                _serialPort.Close();
//                _serialPort.Dispose();
//            }
//        }

//        public bool SendWrite(string address, string data, int addressCount = 1)
//        {
//            string command = BuildWriteCommand(address, data, addressCount);
//            return SendAndWaitForAck(command);
//        }

//        public bool SendRead(string address, int addressCount, out string response)
//        {
//            response = "";
//            string command = BuildReadCommand(address, addressCount);
//            bool success = SendAndWaitForAck(command);
//            if (success)
//                response = _lastReceivedData;
//            return success;
//        }

//        // 오버로드: addressCount 기본값 1
//        public bool SendRead(string address, out string response)
//        {
//            return SendRead(address, 1, out response);
//        }

//        private bool SendAndWaitForAck(string command)
//        {
//            if (!IsConnected)
//                return false;

//            lock (_lock)
//            {
//                _receiveEvent.Reset();
//                _lastReceivedData = "";
//                _dataReceived = false;

//                _serialPort.DiscardInBuffer();
//                _serialPort.Write(command);

//                if (_receiveEvent.WaitOne(5000)) // wait 1 second
//                {
//                    return _dataReceived;
//                }
//                else
//                {
//                    Log.Write("DustCollector", $"[{_position}] Timeout waiting for ACK.");
//                    return false;
//                }
//            }
//        }

//        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
//        {
//            try
//            {
//                string received = _serialPort.ReadExisting();
//                _lastReceivedData += received;

//                if (_lastReceivedData.EndsWith(((char)0x04).ToString())) // EOT
//                {
//                    _dataReceived = _lastReceivedData.StartsWith(((char)0x06).ToString()); // ACK
//                    _receiveEvent.Set();
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Write("DustCollector", $"[{_position}] Data Receive Error: {ex.Message}");
//            }
//        }

//        private string BuildWriteCommand(string addr, string data, int addrCount)
//        {
//            // 핵심 명령어: "01W0006" + "1" + "0002" (예시)
//            StringBuilder sb = new StringBuilder();
//            sb.Append("01");          // Station No
//            sb.Append("W");           // Write
//            sb.Append(addr);          // Address (4자리)
//            sb.Append((char)('0' + addrCount)); // Address Count
//            sb.Append(data);          // Data

//            return AppendChecksumAndEOT(sb.ToString());
//        }

//        private string BuildReadCommand(string addr, int addrCount)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append("01");         // Station No
//            sb.Append("R");          // Read
//            sb.Append(addr);         // Address (4자리)
//            sb.Append((char)('0' + addrCount)); // Address Count

//            return AppendChecksumAndEOT(sb.ToString());
//        }

//        /// <summary>
//        /// coreCommand (ENQ 제외, EOT 제외)에 대해 Checksum과 EOT를 붙인 전체 명령 문자열을 반환
//        /// </summary>
//        private string AppendChecksumAndEOT(string coreCommand)
//        {
//            int checksum = 0;
//            foreach (char ch in coreCommand)
//                checksum += ch;

//            byte sumByte = (byte)(checksum & 0xFF);
//            string checksumHex = sumByte.ToString("x2");  // always 2 characters

//            string fullCommand = ((char)0x05).ToString() + coreCommand + checksumHex + ((char)0x04).ToString();
//            return fullCommand;
//        }


//        public bool Connect(Equipment.CommList comm)
//        {
//            string strPortName;
//            int baudRate, dataBits;
//            StopBits stopBits;
//            Parity parity;
//            Handshake handshake;

//            Equipment.GetSerialPortConfig(comm,
//                out strPortName, out baudRate, out dataBits, out stopBits, out parity, out handshake);

//            return Connect(strPortName, baudRate, dataBits, stopBits, parity, handshake);
//            // 내부 기본 설정: 115200, 8N1, No Handshake
//            //return Connect(portName, 115200, 8, StopBits.One, Parity.None, Handshake.None);
//        }

//        public bool DustCollector_On()
//        {
//            return SendWrite("0006", "0002", 1); // Address: 운전명령, Data: 정방향
//        }

//        public bool DustCollector_Off()
//        {
//            return SendWrite("0006", "0001", 1); // Address: 운전명령, Data: 정지
//        }


//        /// <summary>
//        /// 집진기 출력 주파수를 설정합니다 (단위: 0.01Hz → 내부는 100배 값).
//        /// 예: 60.0Hz → "6000" 전송, 주소는 0005
//        /// </summary>
//        public bool SetFrequency(double frequencyHz)
//        {
//            int freqValue = (int)(frequencyHz * 100.0); // 60.0Hz → 6000
//            string asciiData = freqValue.ToString("D4"); // "6000"

//            return SendWrite("0005", asciiData, 1); // 주소는 반드시 0005
//        }


//        public bool ReadFrequency(out string frequencyResponse)
//        {
//            return SendRead("000A", 1, out frequencyResponse); // Address: 출력 주파수 번지
//        }



//        /// <summary>
//        /// 현재 집진기 상태를 조회합니다.
//        /// 반환값: 0001 = 정지, 0002 = 운전 중
//        /// </summary>
//        public bool GetCurrentStatus(out string status)
//        {
//            return SendRead("0007", 1, out status); // 예: 상태 주소 0007
//        }

//        public CollectorRunState GetRunState()
//        {
//            if (GetCurrentStatus(out string status))
//            {
//                switch (status.Trim())
//                {
//                    case "0001": return CollectorRunState.Stopped;
//                    case "0002": return CollectorRunState.Running;
//                }
//            }

//            return CollectorRunState.Unknown;
//        }

//        /// <summary>
//        /// 집진기의 전원 상태와 출력 주파수를 함께 조회합니다.
//        /// </summary>
//        /// <param name="runState">CollectorRunState.Running / Stopped / Unknown</param>
//        /// <param name="frequencyHz">출력 주파수 (단위: Hz)</param>
//        /// <returns>조회 성공 여부</returns>
//        public bool GetStatus(out CollectorRunState runState, out double frequencyHz)
//        {
//            runState = CollectorRunState.Unknown;
//            frequencyHz = 0.0;

//            string statusRaw;
//            if (!SendRead("0007", 1, out statusRaw))
//                return false;

//            switch (statusRaw.Trim())
//            {
//                case "0001": runState = CollectorRunState.Stopped; break;
//                case "0002": runState = CollectorRunState.Running; break;
//                default: runState = CollectorRunState.Unknown; break;
//            }

//            string freqRaw;
//            if (!SendRead("000A", 1, out freqRaw))
//                return false;

//            // 예: freqRaw = "0064" (== 100.0Hz)
//            int freqValue = 0;
//            if (int.TryParse(freqRaw, System.Globalization.NumberStyles.HexNumber, null, out freqValue))
//                frequencyHz = freqValue / 10.0;
//            else if (int.TryParse(freqRaw, out freqValue))
//                frequencyHz = freqValue / 10.0;

//            return true;
//        }

//        /// <summary>
//        /// 집진기의 상태(운전 여부, 주파수, 전류, 경고/알람 상태)를 모두 조회합니다.
//        /// </summary>
//        public bool GetDetailedStatus(out CollectorRunState runState, out double frequencyHz, out double currentA,
//                                      out CollectorAlarmState alarmState)
//        {
//            runState = CollectorRunState.Unknown;
//            frequencyHz = 0.0;
//            currentA = 0.0;
//            alarmState = CollectorAlarmState.Unknown;

//            string statusRaw, freqRaw, currentRaw, warnRaw;
//            bool ok = true;

//            // 운전 상태
//            ok &= SendRead("0007", 1, out statusRaw);
//            switch (statusRaw.Trim())
//            {
//                case "0001": runState = CollectorRunState.Stopped; break;
//                case "0002": runState = CollectorRunState.Running; break;
//                default: runState = CollectorRunState.Unknown; break;
//            }

//            // 출력 주파수
//            ok &= SendRead("000A", 1, out freqRaw);
//            if (int.TryParse(freqRaw, System.Globalization.NumberStyles.HexNumber, null, out int freqVal))
//                frequencyHz = freqVal / 10.0;

//            // 출력 전류
//            ok &= SendRead("000B", 1, out currentRaw); // 예: 000B가 전류 번지
//            if (int.TryParse(currentRaw, System.Globalization.NumberStyles.HexNumber, null, out int currentVal))
//                currentA = currentVal / 10.0;

//            // 경고/알람 상태
//            ok &= SendRead("000C", 1, out warnRaw); // 예: 000C가 경고/알람 상태 번지
//            switch (warnRaw.Trim())
//            {
//                case "0000": alarmState = CollectorAlarmState.None; break;
//                case "0001": alarmState = CollectorAlarmState.Warning; break;
//                case "0002": alarmState = CollectorAlarmState.Alarm; break;
//                default: alarmState = CollectorAlarmState.Unknown; break;
//            }

//            return ok;
//        }


//    }
//}
