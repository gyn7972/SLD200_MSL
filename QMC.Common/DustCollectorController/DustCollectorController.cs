using System;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace QMC.Common.Parts
{
    public class DustCollectorController : Part
    {
        public enum CollectorRunState
        {
            Unknown,
            Stopped,
            Running
        }

        public enum CollectorAlarmState
        {
            None,
            Warning,
            Alarm,
            Unknown
        }

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

        // 오버로드: addressCount 기본값 1
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

                if (_receiveEvent.WaitOne(1000)) // wait 1 second
                {
                    return _dataReceived;
                }
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

                if (_lastReceivedData.EndsWith(((char)0x04).ToString())) // EOT
                {
                    _dataReceived = _lastReceivedData.StartsWith(((char)0x06).ToString()); // ACK
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
            byte[] cmd = new byte[12 + 4 * addrCount];
            cmd[0] = 0x05; // ENQ
            cmd[1] = (byte)'0'; cmd[2] = (byte)'1'; // Device No "01"
            cmd[3] = (byte)'W'; // Write

            int checksum = cmd[1] + cmd[2] + cmd[3];
            for (int i = 0; i < 4; i++) { cmd[4 + i] = (byte)addr[i]; checksum += cmd[4 + i]; }

            cmd[8] = (byte)(addrCount + '0'); checksum += cmd[8];

            for (int i = 0; i < data.Length; i++) { cmd[9 + i] = (byte)data[i]; checksum += cmd[9 + i]; }

            byte sum = (byte)(checksum & 0xFF);
            string sumHex = sum.ToString("x2");
            cmd[9 + data.Length] = (byte)sumHex[0];
            cmd[10 + data.Length] = (byte)sumHex[1];
            cmd[11 + data.Length] = 0x04; // EOT

            return Encoding.Default.GetString(cmd);
        }

        private string BuildReadCommand(string addr, int addrCount)
        {
            byte[] cmd = new byte[12];
            cmd[0] = 0x05; // ENQ
            cmd[1] = (byte)'0'; cmd[2] = (byte)'1';
            cmd[3] = (byte)'R';

            int checksum = cmd[1] + cmd[2] + cmd[3];
            for (int i = 0; i < 4; i++) { cmd[4 + i] = (byte)addr[i]; checksum += cmd[4 + i]; }

            cmd[8] = (byte)(addrCount + '0'); checksum += cmd[8];

            byte sum = (byte)(checksum & 0xFF);
            string sumHex = sum.ToString("x2");
            cmd[9] = (byte)sumHex[0];
            cmd[10] = (byte)sumHex[1];
            cmd[11] = 0x04;

            return Encoding.Default.GetString(cmd);
        }


        public bool Connect(Equipment.CommList comm)
        {
            string strPortName;
            int baudRate, dataBits;
            StopBits stopBits;
            Parity parity;
            Handshake handshake;

            Equipment.GetSerialPortConfig(comm,
                out strPortName, out baudRate, out dataBits, out stopBits, out parity, out handshake);

            return Connect(strPortName, baudRate, dataBits, stopBits, parity, handshake);
            // 내부 기본 설정: 115200, 8N1, No Handshake
            //return Connect(portName, 115200, 8, StopBits.One, Parity.None, Handshake.None);
        }

        public bool DustCollector_On()
        {
            return SendWrite("0006", "0002", 1); // Address: 운전명령, Data: 정방향
        }

        public bool DustCollector_Off()
        {
            return SendWrite("0006", "0001", 1); // Address: 운전명령, Data: 정지
        }


        /// <summary>
        /// 집진기 출력 주파수를 설정합니다 (단위: 0.01Hz → 내부는 100배 값).
        /// 예: 60.0Hz → "6000" 전송, 주소는 0005
        /// </summary>
        public bool SetFrequency(double frequencyHz)
        {
            int freqValue = (int)(frequencyHz * 100.0); // 60.0Hz → 6000
            string asciiData = freqValue.ToString("D4"); // "6000"

            return SendWrite("0005", asciiData, 1); // 주소는 반드시 0005
        }


        public bool ReadFrequency(out string frequencyResponse)
        {
            return SendRead("000A", 1, out frequencyResponse); // Address: 출력 주파수 번지
        }



        /// <summary>
        /// 현재 집진기 상태를 조회합니다.
        /// 반환값: 0001 = 정지, 0002 = 운전 중
        /// </summary>
        public bool GetCurrentStatus(out string status)
        {
            return SendRead("0007", 1, out status); // 예: 상태 주소 0007
        }

        public CollectorRunState GetRunState()
        {
            if (GetCurrentStatus(out string status))
            {
                switch (status.Trim())
                {
                    case "0001": return CollectorRunState.Stopped;
                    case "0002": return CollectorRunState.Running;
                }
            }

            return CollectorRunState.Unknown;
        }

        /// <summary>
        /// 집진기의 전원 상태와 출력 주파수를 함께 조회합니다.
        /// </summary>
        /// <param name="runState">CollectorRunState.Running / Stopped / Unknown</param>
        /// <param name="frequencyHz">출력 주파수 (단위: Hz)</param>
        /// <returns>조회 성공 여부</returns>
        public bool GetStatus(out CollectorRunState runState, out double frequencyHz)
        {
            runState = CollectorRunState.Unknown;
            frequencyHz = 0.0;

            string statusRaw;
            if (!SendRead("0007", 1, out statusRaw))
                return false;

            switch (statusRaw.Trim())
            {
                case "0001": runState = CollectorRunState.Stopped; break;
                case "0002": runState = CollectorRunState.Running; break;
                default: runState = CollectorRunState.Unknown; break;
            }

            string freqRaw;
            if (!SendRead("000A", 1, out freqRaw))
                return false;

            // 예: freqRaw = "0064" (== 100.0Hz)
            int freqValue = 0;
            if (int.TryParse(freqRaw, System.Globalization.NumberStyles.HexNumber, null, out freqValue))
                frequencyHz = freqValue / 10.0;
            else if (int.TryParse(freqRaw, out freqValue))
                frequencyHz = freqValue / 10.0;

            return true;
        }

        /// <summary>
        /// 집진기의 상태(운전 여부, 주파수, 전류, 경고/알람 상태)를 모두 조회합니다.
        /// </summary>
        public bool GetDetailedStatus(out CollectorRunState runState, out double frequencyHz, out double currentA,
                                      out CollectorAlarmState alarmState)
        {
            runState = CollectorRunState.Unknown;
            frequencyHz = 0.0;
            currentA = 0.0;
            alarmState = CollectorAlarmState.Unknown;

            string statusRaw, freqRaw, currentRaw, warnRaw;
            bool ok = true;

            // 운전 상태
            ok &= SendRead("0007", 1, out statusRaw);
            switch (statusRaw.Trim())
            {
                case "0001": runState = CollectorRunState.Stopped; break;
                case "0002": runState = CollectorRunState.Running; break;
                default: runState = CollectorRunState.Unknown; break;
            }

            // 출력 주파수
            ok &= SendRead("000A", 1, out freqRaw);
            if (int.TryParse(freqRaw, System.Globalization.NumberStyles.HexNumber, null, out int freqVal))
                frequencyHz = freqVal / 10.0;

            // 출력 전류
            ok &= SendRead("000B", 1, out currentRaw); // 예: 000B가 전류 번지
            if (int.TryParse(currentRaw, System.Globalization.NumberStyles.HexNumber, null, out int currentVal))
                currentA = currentVal / 10.0;

            // 경고/알람 상태
            ok &= SendRead("000C", 1, out warnRaw); // 예: 000C가 경고/알람 상태 번지
            switch (warnRaw.Trim())
            {
                case "0000": alarmState = CollectorAlarmState.None; break;
                case "0001": alarmState = CollectorAlarmState.Warning; break;
                case "0002": alarmState = CollectorAlarmState.Alarm; break;
                default: alarmState = CollectorAlarmState.Unknown; break;
            }

            return ok;
        }


    }
}
