using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.IpgPhotonics
{
    public class YlpnLaserController
    {
        #region Define
        [Serializable]
        public enum LaserOperationStatus
        {
            On,
            Off
        }
        #endregion

        #region Field

        #endregion

        #region Constructor
        public YlpnLaserController(string strName)
        {
            this.Communicator = new YlpnLaserControllerCommunicator();
            this.PowerMeter = new LaserPowerMeter();
            this.Config = new LaserConfig();
            IsOpen = false;

            this.Init();
        }
        public YlpnLaserController() : this("YlpnLaserController")
        {
        }
        #endregion

        #region Property
        public YlpnLaserControllerCommunicator Communicator;
        public LaserPowerMeter PowerMeter;
        public LaserConfig Config;
        public bool IsOpen;
        #endregion

        #region Method
        public void Init()
        {
            int ret = 0;

            try
            {
                if (Communicator == null)
                {
                    Communicator = new YlpnLaserControllerCommunicator();
                }
                if (Communicator.IsOpen)
                {
                    Communicator.Close();
                }

                Communicator.PortName = Config.LaserPortName;
                Communicator.BaudRate = Config.LaserBaudRate;
                Communicator.DataBits = Config.LaserDataBits;
                Communicator.Parity = Config.LaserParity;
                Communicator.StopBits = Config.LaserStopBits;
                Communicator.Handshake = Config.LaserHandshake;
                Communicator.ReplyTimeout = Config.LaserTimeOut;

                Communicator.Open();

                //if (Communicator.IsOpen)
                //{
                //    m_Cts = new CancellationTokenSource();
                //    CancellationToken token = m_Cts.Token;
                //    m_Task = Task.Factory.StartNew(() =>
                //    {
                //        while (true)
                //        {
                //            if (m_bStop)
                //                break;

                //            if (CommandQueueCount() > 0)
                //            {
                //                SendVolumn();
                //            }

                //            token.Register(() =>
                //            {
                //                m_bStop = true;
                //            });
                //            Thread.Sleep(100);
                //        }
                //    });
                //}

                if (PowerMeter.Communicator == null)
                {
                    PowerMeter.Communicator = new LaserPowerMeterCommunicator();
                }
                if (PowerMeter.Communicator.IsOpen)
                {
                    PowerMeter.Communicator.Close();
                }

                PowerMeter.Communicator.PortName = Config.PowerMeterPortName;
                PowerMeter.Communicator.BaudRate = Config.PowerMeterBaudRate;
                PowerMeter.Communicator.DataBits = Config.PowerMeterDataBits;
                PowerMeter.Communicator.Parity = Config.PowerMeterParity;
                PowerMeter.Communicator.StopBits = Config.PowerMeterStopBits;
                PowerMeter.Communicator.Handshake = Config.PowerMeterHandshake;
                PowerMeter.Communicator.ReplyTimeout = Config.PowerMeterTimeOut;

                PowerMeter.Communicator.Open();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);

                ret = -1;
            }
            return;
        }

        public int OnOpen()
        {
            int ret = 0;
            if (this.Communicator.IsOpen == false) return ret;
            if (IsOpen == true) return ret;

            //if ((ret = this.Communicator.SendFrame(YlpnLaserControllerCommunicator.SetCommand.EmissionEnableOn.ToString(), YlpnLaserControllerCommunicator.Stx, YlpnLaserControllerCommunicator.Etx)) != 0) return ret;

            if ((ret = this.Communicator.OnSetValue(YlpnLaserControllerCommunicator.SetCommand.EmissionEnableOn, "")) != 0) return ret;

            IsOpen = true;

            return ret;
        }

        public int OnClose()
        {
            int ret = 0;
            if (this.Communicator.IsOpen == false) return ret;
            if (IsOpen == true) return ret;

            //if ((ret = this.Communicator.SendFrame(YlpnLaserControllerCommunicator.SetCommand.EmissionEnableOn.ToString(), YlpnLaserControllerCommunicator.Stx, YlpnLaserControllerCommunicator.Etx)) != 0) return ret;

            if ((ret = this.Communicator.OnSetValue(YlpnLaserControllerCommunicator.SetCommand.EmissionEnableOff, "")) != 0) return ret;

            IsOpen = true;

            return ret;
        }
        #endregion
    }
}
