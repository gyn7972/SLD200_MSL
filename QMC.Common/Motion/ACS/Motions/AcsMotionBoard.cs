using ACS.SPiiPlusNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QMC.Common.Motion.ACS.Motion
{
    public class AcsMotionBoard : MotionBoard
    {
		private Api _ACS;
		private bool m_bConnected;
		private int m_nTotalAxis;
		private Axis[] m_arrAxisList;
		private int m_nTotalBuffer;
		private Timer m_Timer;
		public AcsMotionBoard() : base()
	{
			m_bConnected = false;
			m_nTotalAxis = 2;
			m_arrAxisList = null;
			m_nTotalBuffer = 0;
		}
        protected override int OnOpen()
        {
			int ret = 0;

			return ret;
        }
        protected override int OnClose()
        {
			int ret = 0;
            Close();
			return ret;
        }
        public override int Load(FileStream fs)
        {
			int ret = 0;

			return ret;
        }
        public int OpenTCP(string strIP, string strPort)
        {
            int ret = 0;
			string strTemp;
			int i;
			//double lfTemp = 0.0f;
			
			try
			{

                _ACS.OpenCommEthernetTCP(
					strIP,                             // IP Address (Default : 10.0.0.100)
                    Convert.ToInt32(strPort.Trim())    // TCP/IP Port nubmer (default : 701)
                    );
                m_bConnected = true;

				// Get Total number of axes
				// Using Transaction function : return string text from controller, we need to convert to integer value
				strTemp = _ACS.Transaction("?SYSINFO(13)");
				m_nTotalAxis = Convert.ToInt32(strTemp.Trim());

				// Using Sysinfo function
				//_ACS.GetSysInfo(_ACS.ACSC_SYS_NAXES_KEY, out lfTemp);

				// When we are using multi axes command (ex) ToPointM, HaltM, ...), we need to allocate the array size more 1.
				// Because of the last delimeter (-1)
				m_arrAxisList = new Axis[m_nTotalAxis + 1];
				for (i = 0; i < m_nTotalAxis; i++)
				{
					m_arrAxisList[i] = (Axis)i;
				}
				// Insert '-1' at the last
				m_arrAxisList[m_nTotalAxis] = Axis.ACSC_NONE;

				// Update current motion paramter to UI.
				UpdateProfile();

				strTemp = _ACS.Transaction("?SYSINFO(10)");
				m_nTotalBuffer = Convert.ToInt32(strTemp.Trim());
				//for (i = 0; i < m_nTotalBuffer; i++) 
				//{ 
				//	cboBufferNo.Items.Add(i.ToString()); 
				//}
				//cboBufferNo.SelectedIndex = 0;

				//btnOpen.Enabled = false;
				//btnClose.Enabled = true;

				// Set updating timer
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			//catch (COMException comex)
			//{
			//    MessageBox.Show("Connection fail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    System.Diagnostics.Debug.WriteLine("Connection fail" + comex.Message);

			//    m_bConnected = false;
			//    return;
			//}
			return ret;
        }

		public int OpenSimulator()
        {
			int ret = 0;
			string strTemp;
			int i;
            //double lfTemp = 0.0f;

            try
            {

                // Simmulation mode
                _ACS.OpenCommSimulator();

                m_bConnected = true;

                // Get Total number of axes
                // Using Transaction function : return string text from controller, we need to convert to integer value
                strTemp = _ACS.Transaction("?SYSINFO(13)");
				m_nTotalAxis = Convert.ToInt32(strTemp.Trim());

				// Using Sysinfo function
				//_ACS.GetSysInfo(_ACS.ACSC_SYS_NAXES_KEY, out lfTemp);

				// When we are using multi axes command (ex) ToPointM, HaltM, ...), we need to allocate the array size more 1.
				// Because of the last delimeter (-1)
				m_arrAxisList = new Axis[m_nTotalAxis + 1];
				for (i = 0; i < m_nTotalAxis; i++)
				{
					m_arrAxisList[i] = (Axis)i;
				}
				// Insert '-1' at the last
				m_arrAxisList[m_nTotalAxis] = Axis.ACSC_NONE;

				// Update current motion paramter to UI.
				UpdateProfile();

				strTemp = _ACS.Transaction("?SYSINFO(10)");
				m_nTotalBuffer = Convert.ToInt32(strTemp.Trim());
				//for (i = 0; i < m_nTotalBuffer; i++) { cboBufferNo.Items.Add(i.ToString()); }
				//cboBufferNo.SelectedIndex = 0;

				//btnOpen.Enabled = false;
				//btnClose.Enabled = true;

				// Set updating timer
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			//catch (COMException comex)
			//{
			//    MessageBox.Show("Connection fail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    System.Diagnostics.Debug.WriteLine("Connection fail" + comex.Message);

			//    m_bConnected = false;
			//    return;
			//}
			return ret;
        }
        public int Close()
        {
            int ret = 0;
			if (m_bConnected) _ACS.CloseComm();

			m_Timer.Stop();
			return ret;
        }

		private void UpdateProfile()
		{
			//if (m_bConnected)
			//{
			//	txtVel.Text = _ACS.GetVelocity((Axis)cboAxisNo.SelectedIndex).ToString();
			//	txtAcc.Text = _ACS.GetAcceleration((Axis)cboAxisNo.SelectedIndex).ToString();
			//	txtDec.Text = _ACS.GetDeceleration((Axis)cboAxisNo.SelectedIndex).ToString();
			//	txtKdec.Text = _ACS.GetKillDeceleration((Axis)cboAxisNo.SelectedIndex).ToString();
			//	txtJerk.Text = _ACS.GetJerk((Axis)cboAxisNo.SelectedIndex).ToString();
			//}
		}

		private void UpdateUI_Tick(object sender, EventArgs e)
		{
			m_Timer.Stop();
			
			m_Timer.Start();
		}

        public override int Load(MotionBoardConfiguration configuration, FileStream fs)
        {
			//throw new NotImplementedException();
			return 0;
        }
    }
}
