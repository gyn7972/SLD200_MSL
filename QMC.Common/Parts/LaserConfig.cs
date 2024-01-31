using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class LaserConfig
    {
        [Category("Laser")]
        public string LaserPortName { set; get; }
        [Category("Laser")]
        public int LaserBaudRate { set; get; }
        [Category("Laser")]
        public int LaserDataBits { set; get; }
        [Category("Laser")]
        public StopBits LaserStopBits { set; get; }
        [Category("Laser")]
        public Parity LaserParity { set; get; }
        [Category("Laser")]
        public Handshake LaserHandshake { set; get; }
        [Category("Laser")]
        public int LaserTimeOut { get; set; }

        [Category("PowerMeter")]
        public string PowerMeterPortName { set; get; }
        [Category("PowerMeter")]
        public int PowerMeterBaudRate { set; get; }
        [Category("PowerMeter")]
        public int PowerMeterDataBits { set; get; }
        [Category("PowerMeter")]
        public StopBits PowerMeterStopBits { set; get; }
        [Category("PowerMeter")]
        public Parity PowerMeterParity { set; get; }
        [Category("PowerMeter")]
        public Handshake PowerMeterHandshake { set; get; }
        [Category("PowerMeter")]
        public int PowerMeterTimeOut { get; set; }

        #region Contructor
        public LaserConfig()
        {
            LaserPortName = "COM1";
            LaserBaudRate = 9600;
            LaserDataBits = 8;
            LaserStopBits = StopBits.One;
            LaserParity = Parity.None;
            LaserHandshake = Handshake.None;
            LaserTimeOut = 1000;

            PowerMeterPortName = "COM2";
            PowerMeterBaudRate = 9600;
            PowerMeterDataBits = 8;
            PowerMeterStopBits = StopBits.One;
            PowerMeterParity = Parity.None;
            PowerMeterHandshake = Handshake.None;
            PowerMeterTimeOut = 1000;
        }
        #endregion

        //#region Laser
        //[Serializable]
        //public class Laser
        //{
        //    [Category("Laser")]
        //    public string LaserPortName { set; get; }
        //    [Category("Laser")]
        //    public int LaserBaudRate { set; get; }
        //    [Category("Laser")]
        //    public int LaserDataBits { set; get; }
        //    [Category("Laser")]
        //    public StopBits LaserStopBits { set; get; }
        //    [Category("Laser")]
        //    public Parity LaserParity { set; get; }
        //    [Category("Laser")]
        //    public Handshake LaserHandshake { set; get; }
        //    [Category("Laser")]
        //    public int LaserTimeOut { get; set; }

        //    #region Constructor
        //    public Laser()
        //    {
        //        LaserPortName = "COM1";
        //        LaserBaudRate = 9600;
        //        LaserDataBits = 8;
        //        LaserStopBits = StopBits.One;
        //        LaserParity = Parity.None;
        //        LaserHandshake = Handshake.None;
        //        LaserTimeOut = 1000;
        //    }
        //    #endregion
        //    #endregion

        //#region PowerMeter

        //    [Serializable]
        //    public class PowerMeter
        //    {


        //        #region Constructor
        //        public PowerMeter()
        //        {
        //        }
        //        #endregion
        //    }
        //    #endregion
    }
}
