using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.Optics.Leesos
{
    [Serializable]
    public class DigitalIlluminatorConfig
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

        public int TimeOut { get; set; }

        public DigitalIlluminatorConfig()
        {
            PortName = "COM3";
            BaudRate = 9600;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
            Handshake = Handshake.None;
            TimeOut = 1000;
        }
    }
}
