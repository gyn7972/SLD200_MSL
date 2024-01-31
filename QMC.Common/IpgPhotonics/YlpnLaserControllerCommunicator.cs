using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.IpgPhotonics
{
    public class YlpnLaserControllerCommunicator : SerialComm
    {
        #region Define
        public const string Etx = "\r";
        public const string Stx = "$";

        private readonly object m_SyncRoot;

        [Serializable]
        public enum ReadCommand
        {
            DeviceId = 1,
            DeviceSN = 2,
            FWRevision = 3,
            Vendor = 99,
            DeviceStatus = 4,
            DeviceTemperature = 5,
            DigitalInterfaceStatus = 10,
            ExtendedStatus = 11,
            BackReflectionCounter = 12,
            SessionBrCounter = 13,
            NominalAveragePower = 14,
            NominalPulseDuration = 15,
            NominalPulseEnerge = 16,
            NominalPeakPower = 17,
            PulseRepetitionRateRange = 18,
            HeadTemperature = 19,
            MainSupplyVoltage = 21,
            HousekeepingVoltage24V = 22,
            OperatingMode = 23,
            InstalledOptions = 25,
            StartOperatingMode = 27,
            OperatingPowerWatt = 33,
            OperatingPowerPercent = 34,
            OperatingPulseEnergy = 36,
            PulseRepetitionRaterMonitor = 38,
            AlarmCounters = 70,
            ModuleTemperaturerange = 58,
            NominalFrequency = 59,
            CriticalErrorCounter = 95,
            CriticalErrorCode = 96,
            ReadPulseRepetitionRate = 29,
            ReadTheNumberOfAPDModes = 55,
            ReadAPDModeDescription = 56,
            MaximumPrepump = 63,
            Prepump = 65,
        }

        [Serializable]
        public enum SetCommand
        {
            OperatingMode = 24,
            StartOperatingMode = 26,
            ResetCriticalErrorAlarms = 97,
            SetPulseRepetitionRate = 28,
            LaserEmissionOn = 30,
            LaserEmissionOff = 31,
            OperatingPower = 32,
            GuideLaserOn = 40,
            GuideLaserOff = 41,
            EmissionEnableOn = 42,
            EmissionEnableOff = 43,
            ResetAlarms = 50,
            SetAPDModeIndex = 69,
            SaveAPDModeIndex = 54,
            Prepump = 64,
        }
        #endregion

        #region Constructor
        public YlpnLaserControllerCommunicator(string strName)
        {
            m_SyncRoot = new object();
        }

        public YlpnLaserControllerCommunicator() : this("YlpnLaserControllerCommunicator")
        {

        }
        #endregion

        #region Property
        public int ReplyTimeout { set; get; }

        public bool IsOpen => m_SerialPort.IsOpen;
        #endregion

        #region Method
        public void Init()
        {

        }

        private int Send(string send)
        {
            int ret = 0;

            if (this.m_SerialPort.IsOpen == false)
            {
                //this.WriteLog(LogLevel.Highest, "Failed this port open.");
                //if ((ret = this.Alarms[CommPart.AlarmKeys.Open].Post(this)) != 0) return ret;
            }

            if ((ret = this.SendFrame(send, Stx, Etx)) != 0)
            {
                //this.WriteLog(LogLevel.Highest, "Send failed.");
                //if ((ret = this.Alarms[CommPart.AlarmKeys.Send].Post(this)) != 0) return ret;
            }

            //this.WriteLog(LogLevel.BelowNormal, "[SEND] {0}", send);

            return ret;
        }

        private int Receive(ref string response)
        {
            int ret = 0;
            //byte[] data = null;
            string data = string.Empty;
            string[] token;

            //if (this.Simulation.IsSimulatedWithoutResource() == true) return ret;

            if (this.m_SerialPort.IsOpen == false)
            {
                //this.WriteLog(LogLevel.Highest, "Failed this port open.");
                //if (this.Alarms.ContainsKey(CommPart.AlarmKeys.Open) == true)
                //    if ((ret = this.Alarms[CommPart.AlarmKeys.Open].Post(this)) != 0) return ret;
            }

            //Wait
            if (this.WaitRecv(200) == true)
            {
                if ((ret = this.ReceiveFrame(out data, Etx)) != 0)
                {
                    //if (this.Alarms.ContainsKey(YlpLaserControllerCommunicator.AlarmKeys.ReceiveFail) == true)
                    //    if ((ret = this.Alarms[YlpLaserControllerCommunicator.AlarmKeys.ReceiveFail].Post(this)) != 0) return ret;
                }
            }
            else
            {
                //if (this.Alarms.ContainsKey(CommPart.AlarmKeys.ReplyTimeout) == true)
                //    if ((ret = this.Alarms[CommPart.AlarmKeys.ReplyTimeout].Post(this)) != 0) return ret;
            }

            if (data == null || data.Length == 0) return ret;

            response = data;
            //response = BytesConverter.ToString(data);
            //this.WriteLog(LogLevel.Normal, "[RECV] {0}", response);

            token = response.Split(';');

            if (token[1] == "E")
            {
                //this.Alarms[AlarmKeys.SentInvaliedCommand].Cause = string.Format("Command : {0}", token[0]);
                //if ((ret = this.Alarms[AlarmKeys.SentInvaliedCommand].Post(this)) != 0) return ret;
            }

            return ret;
        }

        private int SendAndRecive(string send, ref string response)
        {
            int ret = 0;
            string[] message;
            string[] token;

            if ((ret = this.Send(send)) != 0) return ret;
            if ((ret = this.Receive(ref response)) != 0) return ret;

            message = send.Split(';');
            token = response.Split(';');

            if (token[0] != message[0])
            {
                //this.Alarms[AlarmKeys.DoesNotReceivedEqualCommand].Cause = string.Format("Sent : {0}, Received : {1}", message[0], token[0]);
                //if ((ret = this.Alarms[AlarmKeys.DoesNotReceivedEqualCommand].Post(this)) != 0) return ret;
            }

            return ret;
        }

        public int OnSetValue(SetCommand command, string settingValue)
        {
            int ret = 0;
            string[] token;
            string send = "";
            string response = "";

            lock (this.m_SyncRoot)
            {
                if (string.IsNullOrEmpty(settingValue))
                    send = string.Format("{0}", (int)command);
                else
                    send = string.Format("{0};{1}", (int)command, settingValue);

                if ((ret = this.SendAndRecive(send, ref response)) != 0) return ret;

                token = response.Split(';');

                //if (token[1] == "N")
                //if ((ret = this.Alarms[AlarmKeys.DoesNotExecuted].Post(this)) != 0) return ret;
            }

            return ret;
        }

        public int OnGetValue(ReadCommand command, ref string result)
        {
            int ret = 0;
            string[] token;
            string send = "";
            string response = "";

            lock (this.m_SyncRoot)
            {
                send = string.Format("{0}", (int)command);

                if ((ret = this.SendAndRecive(send, ref response)) != 0) return ret;

                token = response.Split(';');

                //if (token[1] == "N")
                //    if ((ret = this.Alarms[AlarmKeys.DoesNotExecuted].Post(this)) != 0) return ret;

                result = token[1];
            }

            return ret;
        }
        #endregion

    }
}
