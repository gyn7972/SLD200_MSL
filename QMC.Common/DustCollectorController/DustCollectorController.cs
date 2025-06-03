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

        public bool Start()
        {
            bool ok = true;
            Thread.Sleep(100);
            ok &= SendWrite("0006", "0002");
            Thread.Sleep(100);
            ok &= SendWrite("0007", "0001");
            Thread.Sleep(100);
            ok &= SetFrequency(60);
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
            if (!SendRead("000A", out string raw))
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
            Log.Write("DustCollector", $"운전 상태: {result}");

            bool isRunning = (value & (1 << 0)) != 0;
            return isRunning ? CollectorRunState.Running : CollectorRunState.Stopped;
        }

        private bool SendWrite(string address, string data)
        {
            string cmd = BuildWriteCommand(address, data, 1);
            return SendAndWaitForAck(cmd);
        }

        private bool SendRead(string address, out string response)
        {
            string cmd = BuildReadCommand(address, 1);
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
                        Log.Write("DustCollector", $"[RX] {_lastReceivedData}");
                        return _dataReceived;
                    }
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






//using System;
//using System.Text;
//using System.Threading;
//using System.IO.Ports;
//using System.Linq;
//using netDxf;

//namespace QMC.Common.Parts
//{
//    public class DustCollectorController : Part
//    {
//        public enum CollectorPosition { Upper, Lower }

//        public enum CollectorRunState { Unknown, Stopped, Running }

//        private SerialPort _serialPort;
//        private readonly object _lock = new object();
//        private string _lastReceivedData = "";
//        private bool _dataReceived = false;
//        private readonly ManualResetEvent _receiveEvent = new ManualResetEvent(false);

//        public bool IsConnected => _serialPort?.IsOpen ?? false;

//        private static readonly double[] DustCollector_FrequencyDataArray = new double[]
//        {
//            59, 58.1, 57.9, 56, 53, 52, 50.9, 50,
//            49, 48, 47, 46.1, 45.5, 44, 43,
//            41, 40, 39.9, 29, 26.5, 26, 25.9,
//            23.6, 23.2, 22, 21.5, 21.2, 20
//        };

//        public DustCollectorController(string name, DustCollectorController.CollectorPosition lower) : base(name) { }

//        public bool Connect(Equipment.CommList comm)
//        {
//            string strPortName;
//            int baudRate, dataBits;
//            StopBits stopBits;
//            Parity parity;
//            Handshake handshake;

//            Equipment.GetSerialPortConfig(comm, out strPortName, out baudRate, out dataBits, out stopBits, out parity, out handshake);
//            return Connect(strPortName, baudRate, dataBits, stopBits, parity, handshake);
//        }

//        public bool Connect(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity, Handshake handshake)
//        {
//            try
//            {
//                _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
//                {
//                    Encoding = Encoding.ASCII,
//                    ReadTimeout = 1000,
//                    WriteTimeout = 1000
//                };
//                _serialPort.DataReceived += SerialPort_DataReceived;
//                _serialPort.Open();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Log.Write("DustCollector", $"[G100] Port Open Fail: {ex.Message}");
//                return false;
//            }
//        }

//        public void Disconnect()
//        {
//            if (_serialPort?.IsOpen == true)
//                _serialPort.Close();
//            _serialPort?.Dispose();
//            _serialPort = null;
//        }




//        public bool Start()
//        {
//            bool ok = true;

//            // Step 1: 주파수 설정 (예: 10.0Hz → 0064)
//            //ok &= SendWrite("03A2", "0064");  // 원하는 주파수로 바꿔도 됨

//            // Step 2: 정방향 운전 설정
//            //ok &= SendWrite("03A1", "0002");

//            // Step 3: Run 시작 트리거
//            //ok &= SendWrite("03A0", "0001");
//            //ok &= SendWrite("0006", "0002");

//            //ok &= SendWrite("0005", "0064");   // 10.0Hz 설정
//            //ok &= SendWrite("0005", "00C8");   // 20.0Hz 설정
//            //ok &= SetFrequency(60);
//            //Thread.Sleep(100);
//            Thread.Sleep(100);
//            ok &= SendWrite("0006", "0002");   // 정방향 운전 명령
//            Thread.Sleep(100);
//            ok &= SendWrite("0007", "0001");   // 운전 실행 트리거
//            Thread.Sleep(100);
//            ok &= SetFrequency(60);
            
//            //ok &= SendWrite("03A2","0064");  // 주파수: 10.0Hz
//            //ok &= SendWrite("03A1","0002");  // 정방향
//            //ok &= SendWrite("03A0","0001");  // Run 트리거

//            //return ok;

//            return ok;
//        }

//        public bool Stop()
//        {
//            bool ok = true;

//            // Step 1: Run 트리거 OFF
//            //ok &= SendWrite("03A0", "0000");
//            // Step 2: 정지 명령 설정
//            ok &= SendWrite("0006", "0001");

//            return ok;
//        }



//        public bool SetFrequency(double freqHz)
//        {
//            // 가장 가까운 유효 주파수 찾아서 사용
//            //double closest = DustCollector_FrequencyDataArray.OrderBy(x => Math.Abs(x - freqHz)).First();
//            //int val = (int)(closest * 100.0);

//            //SendWrite("1D04", "0006"); // Int485로 명령 소스 지정
//            //Thread.Sleep(100);
//            //SendWrite("1D03", "0003"); // 운전 명령도 Int485로 설정
//            //Thread.Sleep(100);



//            int val = (int)(freqHz * 100.0);

//            // 16진수 4자리로 변환
//            string hexVal = val.ToString("X4");  // 예: 500 → "01F4"

//            //return SendWrite("0005", hexVal);
//            return SendWrite("0380", hexVal);

//            //return SendWrite("0005", val.ToString("D4"));
//            //return SendWrite("0380", val.ToString("D4"));
//        }

//        public bool GetFrequency(out double freqHz)
//        {
//            freqHz = 0.0;

//            //000A X //0005 : 실제 돌때 ( 설정값과 차이있음) //0380 : 설정값
//            if (!SendRead("000A", out string raw))
//                return false;

//            string data = ExtractData(raw); // 예: "01F4" (16진수로 500)

//            //int value = Convert.ToInt32(data, 16);
//            //ushort swapped = (ushort)((value >> 8) | (value << 8));
//            //freqHz = swapped * 0.01;
//            //return true;

//            if (int.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out int hex))
//            {
//                freqHz = hex * 0.01;  // 예: 0x01F4 → 500 → 5.00 Hz
//                return true;
//            }

//            return false;


//            //freqHz = 0.0;

//            //if (!SendRead("000A", out string raw))
//            //    return false;

//            //string data = ExtractData(raw); // 예: "0500"

//            //if (int.TryParse(ExtractData(raw), System.Globalization.NumberStyles.HexNumber, null, out int hex))
//            //{
//            //    freqHz = hex / 100.0;
//            //    return true;
//            //}
//            //return false;

//            // 10진수로 해석해야 올바르게 동작함 (0.01 Hz 단위)
//            //if (int.TryParse(data, out int value))
//            //{
//            //    freqHz = value * 0.01;  // 예: 500 → 5.00 Hz
//            //    return true;
//            //}
//            //return false;
//            //freqHz = 0.0;
//            //if (!SendRead("000A", out string raw))
//            //    return false;

//            //if (int.TryParse(ExtractData(raw), System.Globalization.NumberStyles.HexNumber, null, out int hex))
//            //{
//            //    freqHz = hex / 100.0;
//            //    return true;
//            //}

//            //return false;
//        }

//        public CollectorRunState GetRunState()
//        {
//            if (!SendRead("000E", out string raw))
//                return CollectorRunState.Unknown;

//            string data = ExtractData(raw);
//            if (!ushort.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out ushort value))
//                return CollectorRunState.Unknown;


//            string hex = data;// "6B42";  // 실제 수신된 응답값
//            string result = ParseInverterRunState(hex);
//            Log.Write("DustCollector", $"운전 상태: {result}");


//            bool isRunning = (value & (1 << 0)) != 0;
//            return isRunning ? CollectorRunState.Running : CollectorRunState.Stopped;
//        }
//        //public CollectorRunState GetRunState()
//        //{
//        //    //"0303" 안됨.
//        //    if (!SendRead("000E", out string raw))
//        //        return CollectorRunState.Unknown;

//        //    string data = ExtractData(raw);
//        //    switch (data)
//        //    {
//        //        case "0001": return CollectorRunState.Stopped;
//        //        case "0002": return CollectorRunState.Running;
//        //        default: return CollectorRunState.Unknown;
//        //    }
//        //}

//        private bool SendWrite(string address, string data)
//        {
//            //string cmd = BuildCommand('W', address, data);
//            string cmd = BuildWriteCommand(address, data, 1);
//            return SendAndWaitForAck(cmd);
//        }

//        private bool SendRead(string address, out string response)
//        {
//            //string cmd = BuildCommand('R', address, "1");
//            string cmd = BuildReadCommand(address, 1);
//            bool result = SendAndWaitForAck(cmd);
//            response = result ? _lastReceivedData : "";
//            return result;
//        }

//        private string BuildReadCommand(string addr, int wordCount)
//        {
//            string body = "01R" + addr + wordCount.ToString();
//            int sum = body.Sum(c => (byte)c);
//            string checksum = (sum & 0xFF).ToString("X2");
//            return ((char)0x05) + body + checksum + ((char)0x04);
//        }

//        private string BuildReadCommand(string addr)
//        {
//            string body = "01R" + addr;
//            int sum = body.Sum(c => (byte)c);
//            string checksum = (sum & 0xFF).ToString("X2");
//            return ((char)0x05) + body + checksum + ((char)0x04);
//        }

//        private string BuildWriteCommand(string addr, string data, int wordCount)
//        {
//            string body = "01W" + addr + wordCount.ToString() + data;
//            int sum = body.Sum(c => (byte)c);
//            string checksum = (sum & 0xFF).ToString("X2");
//            return ((char)0x05) + body + checksum + ((char)0x04);
//        }

//        private string BuildCommand(char cmd, string addr, string data)
//        {
//            //string cleanCmd = cmd.Trim();
//            string cleanAddr = addr.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\0", "").Trim();
//            string cleanData = data.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\0", "").Trim();

//            string body = "01" + cmd.ToString() + cleanAddr + "1" + cleanData;
            
//            Console.WriteLine($"[DEBUG] body.Length = {body.Length}");
//            foreach (char c in body)
//            {
//                string display;
//                if (c == '\r') display = "\\r";
//                else if (c == '\n') display = "\\n";
//                else if (c == '\0') display = "\\0";
//                else if (char.IsControl(c)) display = $"\\x{((int)c):X2}";
//                else display = c.ToString();

//                Console.WriteLine($"char = '{display}', byte = {(byte)c}");
//            }

//            int sum = 0;
//            foreach (char c in body)
//                sum += (byte)c;
//            string checksum = (sum & 0xFF).ToString("X2");

//            return ((char)0x05) + body + checksum + ((char)0x04);
//        }

//        private bool SendAndWaitForAck(string cmd)
//        {
//            if (!IsConnected) return false;

//            lock (_lock)
//            {
//                _receiveEvent.Reset();
//                _dataReceived = false;
//                _lastReceivedData = "";

//                try
//                {
//                    _serialPort.DiscardInBuffer();
//                    _serialPort.Write(cmd);
//                    if (_receiveEvent.WaitOne(1000))
//                    {
//                        Log.Write("DustCollector", $"[RX] {_lastReceivedData}");
//                        return _dataReceived;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Log.Write("DustCollector", $"[G100] Comm Error: {ex.Message}");
//                }
//                return false;
//            }
//        }

//        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
//        {
//            try
//            {
//                _lastReceivedData += _serialPort.ReadExisting();
//                if (_lastReceivedData.Contains(((char)0x04).ToString()))
//                {
//                    _dataReceived = _lastReceivedData.StartsWith(((char)0x06).ToString());
//                    _lastReceivedData = _lastReceivedData.Trim((char)0x06, (char)0x04);
//                    _receiveEvent.Set();
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Write("DustCollector", $"[G100] RX Error: {ex.Message}");
//            }
//        }

//        private string ExtractData(string response)
//        {
//            if (string.IsNullOrWhiteSpace(response) || response.Length < 4)
//                return "";
//            return response.Substring(response.Length - 4);
//        }

//        public string ParseInverterRunState(string hex)
//        {
//            if (string.IsNullOrWhiteSpace(hex) || hex.Length != 4)
//                return $"[Invalid] 응답값 오류: '{hex}'";

//            if (!ushort.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out ushort value))
//                return $"[Invalid] 숫자 변환 실패: '{hex}'";

//            bool isRunning = (value & (1 << 0)) != 0;       // Bit 0: 운전 중
//            bool isFreqSet = (value & (1 << 1)) != 0;       // Bit 1: 주파수 설정
//            bool isWarning = (value & (1 << 2)) != 0;       // Bit 2: 경고
//            bool isTrip = (value & (1 << 3)) != 0;          // Bit 3: 트립

//            return $"[Raw: {hex}] Run: {(isRunning ? "O" : "X")}, FreqSet: {(isFreqSet ? "O" : "X")}, Warning: {(isWarning ? "O" : "X")}, Trip: {(isTrip ? "O" : "X")}";
//        }
//    }
//}
