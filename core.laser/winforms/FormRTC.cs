using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Core.Laser
{
    public partial class FormRTC : UserControl
    {
        string[] targetLaserMode = Enum.GetNames(typeof(LaserMode));
        string[] targetSignalLevel = Enum.GetNames(typeof(SignalLevel));
        string[] targetExtChannel = Enum.GetNames(typeof(ExtensionChannel));        
        private Rtc6 ScannerRTC;

        public FormRTC()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            // combo box Item add 
            for (int i = 0; i < targetLaserMode.Length; i++)
                cbLaserMode.Items.Add(targetLaserMode[i]);

            for (int i = 0; i < targetSignalLevel.Length; i++)
                cbSignalLevel.Items.Add(targetSignalLevel[i]);

            for (int i = 0; i < targetExtChannel.Length; i++)
                cbEXTChannel.Items.Add(targetExtChannel[i]);

            cbLaserMode.SelectedIndex = 0;
            cbSignalLevel.SelectedIndex = 0;
            cbEXTChannel.SelectedIndex = 0;
        }

        private void btnR6Init_Click(object sender, EventArgs e)
        {
            ScannerRTC = new Rtc6(0, "RTC6");
            //LaserMode mylasermode = (LaserMode)Enum.Parse(typeof(LaserMode), cbLaserMode.Text);
            //SignalLevel mysignallevel = (SignalLevel)Enum.Parse(typeof(SignalLevel), cbSignalLevel.Text);
            if (ScannerRTC.Initialize(16382, string.Empty,
                (LaserMode)Enum.Parse(typeof(LaserMode), cbLaserMode.Text),
                (SignalLevel)Enum.Parse(typeof(SignalLevel), cbSignalLevel.Text)
                ))
                btnR6GetStatus.PerformClick();
            else
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Initialize Fail..";
            }
        }

        private void btnScannerMove_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlMove(double.Parse(txtR6ScanX.Text), double.Parse(txtR6ScanY.Text)))
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Scanner Move Command Fail..";
            }
        }

        private void btnSetFreq_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlFrequency(double.Parse(txtR6Freq.Text), double.Parse(txtR6PulseWidth.Text)))
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Set Frequence Command Fail..";
            }
        }

        private void btnSetDelay_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlDelay(double.Parse(txtR6LaserOn.Text), double.Parse(txtR6LaserOff.Text), double.Parse(txtR6ScannerJump.Text),
               double.Parse(txtR6ScannerMark.Text), double.Parse(txtR6ScannerPlygon.Text)))
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Set Delay Command Fail..";
            }
        }

        private void btnSetSpd_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlSpeed(double.Parse(txtR6Jump.Text), double.Parse(txtR6Mark.Text)))
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Set Speed Command Fail..";
            }
        }

        private void btnR6LaserOn_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlLaserOn())
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Laser ON Command Fail..";
            }
        }

        private void btnR6LaserOff_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlLaserOff())
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Laser OFF Command Fail..";
            }
        }

        private void btnR6CtrlAbort_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlAbort())
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Control Abort Command Fail..";
            }
        }

        private void btnR6CtrlReset_Click(object sender, EventArgs e)
        {
            if (!ScannerRTC.CtlReset())
            {
                txtErrMsg.Clear();
                txtErrMsg.Text = $"Control Reset Command Fail..";
            }
        }

        private void btnR6GetStatus_Click(object sender, EventArgs e)
        {
            bool bBusy = ScannerRTC.CtlGetStatus(RtcStatus.Busy);
            bool bNotBusy = ScannerRTC.CtlGetStatus(RtcStatus.NotBusy);
            bool bList1Busy = ScannerRTC.CtlGetStatus(RtcStatus.List1Busy);
            bool bList2Busy = ScannerRTC.CtlGetStatus(RtcStatus.List2Busy);
            bool bNoError = ScannerRTC.CtlGetStatus(RtcStatus.NoError);
            bool bAborted = ScannerRTC.CtlGetStatus(RtcStatus.Aborted);
            bool bPositionAckOk = ScannerRTC.CtlGetStatus(RtcStatus.PositionAckOK);
            bool bPowerOk = ScannerRTC.CtlGetStatus(RtcStatus.PowerOK);
            bool bTempOk = ScannerRTC.CtlGetStatus(RtcStatus.TempOK);

            txtStatus.Text = $" busy  :  {bBusy} \r\n NotBusy  :  {bNotBusy} \r\n List1Busy  :  {bList1Busy} \r\n" +
                $" List2Busy  :  {bList2Busy} \r\n NoError  :  {bNoError} \r\n Aborted  :  {bAborted} \r\n" +
                $" PositionAckOk  :  {bPositionAckOk} \r\n PowerOK  :  {bPowerOk} \r\n bTempOk  :  {bTempOk}";
        }

        private void btnAddList_Click(object sender, EventArgs e)
        {
            this.dgvListExcuteItem.Rows.Add((this.dgvListExcuteItem.Rows.Count - 1).ToString(), txtPeriod.Text, txtOnTime.Text,
                txtPixelCnt.Text, txtRepeatCnt.Text, txtdx.Text, txtdy.Text);
        }

        private void btnDeleteList_Click(object sender, EventArgs e)
        {
            int i = this.dgvListExcuteItem.SelectedRows[0].Index;
            this.dgvListExcuteItem.Rows.Remove(this.dgvListExcuteItem.Rows[i]);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            int listcnt = 0;
            var t = Task.Run(() =>
            {
                ScannerRTC.ListBegin();

                //--> list working /....
                ScannerRTC.ListFrequency(double.Parse(txtR6Freq.Text), double.Parse(txtR6PulseWidth.Text));
                ScannerRTC.ListDelay(double.Parse(txtR6LaserOn.Text), double.Parse(txtR6LaserOff.Text),
                    double.Parse(txtR6ScannerJump.Text), double.Parse(txtR6ScannerMark.Text), double.Parse(txtR6ScannerPlygon.Text));
                ScannerRTC.ListSpeed(double.Parse(txtR6Jump.Text), double.Parse(txtR6Mark.Text));

                // Start Pos 
                ScannerRTC.ListJumpTo(double.Parse(txtStartX.Text), double.Parse(txtStartY.Text));

                for (int k = 0; k < int.Parse(txtListRepeatCnt.Text); k++)
                {
                    foreach (DataGridViewRow row in this.dgvListExcuteItem.Rows)
                    {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            ScannerRTC.ListPixelLine(double.Parse(row.Cells[1].Value.ToString()),
                                (ExtensionChannel)Enum.Parse(typeof(ExtensionChannel), cbEXTChannel.Text),
                                double.Parse(row.Cells[5].Value.ToString()),
                                double.Parse(row.Cells[6].Value.ToString()),
                                UInt32.Parse(row.Cells[3].Value.ToString()));

                            for (int i = 0; i < Int32.Parse(row.Cells[4].Value.ToString()); i++)
                            {//List Pixel 사용 시 Delay 발생 함... List 넣을 때 주의 필요.
                                ScannerRTC.ListPixel(double.Parse(row.Cells[2].Value.ToString()));
                            }

                            listcnt++;
                        });
                    }
                }

                ScannerRTC.ListEnd();
                ScannerRTC.ListExecute();
            }
          );
        }

       
    }
}
