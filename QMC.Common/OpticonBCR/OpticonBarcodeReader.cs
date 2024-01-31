using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Opticon
{
    public class OpticonBarcodeReader : Part
    {
        public OpticonBarcodeReaderConfig Config { set; get; }
        protected OpticonBarcodeReaderCommunicator m_BarcodeReaderCommunicator;
        public OpticonBarcodeReader(string strName) : base(strName)
        {
            //m_BarcodeReaderCommunicator = new OpticonBarcodeReaderCommunicator();
            Config = new OpticonBarcodeReaderConfig();
        }

        public override int Create()
        {
            int ret = 0;
            if ((ret = base.Create()) != 0) return ret;

                       
            return ret;
        }

        public override int Initialize()
        {
            int ret = 0;
            if ((ret = base.Initialize()) != 0) return ret;

            try
            {
                if (m_BarcodeReaderCommunicator == null)
                    m_BarcodeReaderCommunicator = new OpticonBarcodeReaderCommunicator();

                m_BarcodeReaderCommunicator.Close();

                m_BarcodeReaderCommunicator.Open();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message);
                ret = -1;
            }

            return ret;
        }

        public override void Close()
        {
            base.Close();
            if(m_BarcodeReaderCommunicator != null)
            {
                m_BarcodeReaderCommunicator.Close();
            }
        }
        public override void UpdateConfigData()
        {
            /*try
            {
                base.UpdateConfigData();
                m_BarcodeReaderCommunicator.Close();
                SetConfig(Config);
                m_BarcodeReaderCommunicator.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        public void SetConfig(OpticonBarcodeReaderConfig config)
        {
            if (config != null)
            {
                m_BarcodeReaderCommunicator.PortName = config.PortName;
                m_BarcodeReaderCommunicator.BaudRate = config.BaudRate;
                m_BarcodeReaderCommunicator.DataBits = config.DataBits;
                m_BarcodeReaderCommunicator.StopBits = config.StopBits;
                m_BarcodeReaderCommunicator.Parity = config.Parity;
                m_BarcodeReaderCommunicator.Handshake = config.Handshake;
                m_BarcodeReaderCommunicator.InterCharacterTimeout = config.InterCharacterTimeout;
                m_BarcodeReaderCommunicator.RetryCount = config.RetryCount;
            }
        }

        public int Read(out string readString)
        {
            int ret = 0;
            readString = string.Empty;
            if(m_BarcodeReaderCommunicator != null)
            {
                ret = m_BarcodeReaderCommunicator.SendAndReceive(OpticonBarcodeReaderCommunicator.OpticonCommands.Start, out readString);
                ret = m_BarcodeReaderCommunicator.Send(OpticonBarcodeReaderCommunicator.OpticonCommands.Stop);
            }
            else
            {
                ret = -1;
            }
            return ret;
        }
    }
}
