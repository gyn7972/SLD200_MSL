using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Yamatake
{
    public class MCW400A100GaugeCommunicator : SerialComm
    {
        [Serializable]
        public enum MCW400A100Commands
        {
            GetCurrentValue,
            SetZero,
        }

        public const int ChannelCount = 4;
        public const string Etx = "\r\n";
        public const string ReceiveOK = "OK";
        private List<double> m_LatestPresentValues;

        public int RetryCount { set; get; }
        public bool IsOpen => m_SerialPort.IsOpen;

        public MCW400A100GaugeCommunicator()
        {
            m_LatestPresentValues = new List<double>();
        }

        public void SetConfig(MCW400A100GaugeCommunicatorConfig config)
        {
            if (config != null)
            {
                PortName = config.PortName;
                BaudRate = config.BaudRate;
                DataBits = config.DataBits;
                StopBits = config.StopBits;
                Parity = config.Parity;
                Handshake = config.Handshake;
                InterCharacterTimeout = config.InterCharacterTimeout;
                RetryCount = config.RetryCount;
            }
        }

        private string GetCommand(int channel, MCW400A100Commands command, string value, out bool etxDouble)
        {
            string commandString = string.Empty;
            bool combineChannel = false;
            etxDouble = false;

            if (command == MCW400A100Commands.GetCurrentValue)
            {
                /*Received
                1=(Value)\r\n
                2=(Value)\r\n
                3=(Value)\r\n
                4=(Value)\r\n\r\n
                */
                commandString = "A";
                etxDouble = true;
            }
            else if (command == MCW400A100Commands.SetZero)
            {
                //Received OK\r\n
                commandString = "B";
                combineChannel = true;
            }

            if (combineChannel == true)
                commandString += channel.ToString();

            if (value != string.Empty)
                commandString = string.Format("{0} {1}", commandString, value);

            return string.Format("@{0}", commandString);
        }

        protected virtual int Send(int channel, MCW400A100Commands command, string value, out bool etxDouble)
        {
            int ret = 0;
            if (ChannelCount < channel)
                throw new ArgumentOutOfRangeException("Channel is overflow");

            string message = this.GetCommand(channel, command, value, out etxDouble);

            if ((ret = this.SendFrame(message, Etx)) != 0) return ret;


            return ret;
        }

        protected virtual int Receive(bool etxDouble, out string response)
        {
            int ret = 0;
            response = string.Empty;
            string receive = string.Empty;
            string etx = Etx;
            if (etxDouble == true)
                etx += Etx;

            if ((ret = ReceiveFrame(out response, etx)) != 0) return ret;
            
            return ret;
        }

        public virtual int SendAndReceive(int channel, MCW400A100Commands command, string value, out string response)
        {
            int ret = 0;
            response = string.Empty;
            bool etxDouble = false;
            int retryCount = 0;

            for (retryCount = 0; retryCount < this.RetryCount; retryCount++)
            {
                if ((ret = this.Send(channel, command, value, out etxDouble)) != 0) continue;

                if ((ret = this.Receive(etxDouble, out response)) != 0) continue;
            }

            if (string.IsNullOrEmpty(response) == true)
            {
                if (command != MCW400A100Commands.GetCurrentValue)
                {
                    return -1;
                }
            }

            return ret;
        }

        public virtual int Query(int channel, ref double value)
        {
            int ret = 0;
            string response = "";

            if ((ret = this.SendAndReceive(channel, MCW400A100Commands.GetCurrentValue, string.Empty, out response)) != 0) return ret;

            if ((ret = this.UpdatePresentValue(response)) != 0) return ret;

            if (0 < channel)
                value = m_LatestPresentValues[channel - 1];
            return ret;
        }

        protected int UpdatePresentValue(string response)
        {
            int ret = 0;

            try
            {
                int channel = 0;
                double value = 0D;
                string[] data = null;
                string[] token = response.Split(new string[] { Etx }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < m_LatestPresentValues.Count; i++)
                    m_LatestPresentValues[i] = 0;

                for (int i = 0; i < token.Length; i++)
                {
                    data = token[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                    if (data.Length < 2) continue;

                    if (int.TryParse(data[0], out channel) == false || double.TryParse(data[1], out value) == false)
                    {
                        return -1;
                    }
                    channel--;
                    if (channel < 0 || ChannelCount < channel) continue;

                    m_LatestPresentValues[channel] = value;
                }
            }
            catch (Exception) { }

            return ret;
        }

        public int SetZero(int nChannel)
        {
            int ret = 0;
            string response = string.Empty;

            if ((ret = SendAndReceive(nChannel, MCW400A100Commands.SetZero, string.Empty, out response)) != 0) return ret;

            if (response != ReceiveOK)
            {
                //Response Error 
                return -1;
            }

            return ret;
        }

    }
}
