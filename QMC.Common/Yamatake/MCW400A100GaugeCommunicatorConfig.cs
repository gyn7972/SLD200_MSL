using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Yamatake
{
    [Serializable]
    public class MCW400A100GaugeCommunicatorConfig
    {
        public string PortName
        {
            set;
            get;
        }

        public int BaudRate
        {
            set;
            get;
        }

        public int DataBits
        {
            set;
            get;
        }

        public StopBits StopBits
        {
            set;
            get;
        }

        public Parity Parity
        {
            set;
            get;
        }

        public Handshake Handshake
        {
            set;
            get;
        }

        public int InterCharacterTimeout
        {
            set;
            get;
        }

        public int RetryCount { set; get; }

        public MCW400A100GaugeCommunicatorConfig()
        {
            PortName = "COM1";
            BaudRate = 9600;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
            Handshake = Handshake.None;

            InterCharacterTimeout = 3000;
            RetryCount = 1;
        }
    }
}
