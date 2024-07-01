using QMC.Common;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    public enum OperationModuleName
    {
        //  2022. 03. 21.  SCH : 원래 있던 것
        //SourceStage,
        //TargetStage,
        //DieTransfer

        //  2022. 03. 21.  SCH : 변경
        SourceLoader,                   //  요건 없애버릴 것이고...
        WaferProbeAlign                   //  Laser Drilling
    }
    public partial class FormOperation : FormContentBase
    {
        FormOperationOperator m_FormOperationOperator;
        FormOperationMonitering m_FormOperationMonitering;
        //FormOperationSingleMode m_FormOperationSingleMode;
        FormOperationCalibrationMode m_FormOperationCalibrationMode;
        FormOperationManualMode m_FormOperationManualMode;

        public System.Windows.Forms.Timer timer_ButtonBlink;
        public int m_nBlinkCount;
        public bool m_bBlink;

        public enum OperationButton
        {
            //Monitoring,
            Operator,
            //SingleMode,
            CalibrationMode,
            ManualMode,
        }
        public FormOperation()
            :base()
        {
            InitializeComponent();
            m_FormOperationOperator = new FormOperationOperator();
            m_FormOperationMonitering = new FormOperationMonitering();
            //m_FormOperationSingleMode = new FormOperationSingleMode();
            m_FormOperationCalibrationMode = new FormOperationCalibrationMode();
            m_FormOperationManualMode = new FormOperationManualMode();
            CreatFlowPanelButton();

            //  Main Work 타이머
            timer_ButtonBlink = new System.Windows.Forms.Timer();
            timer_ButtonBlink.Interval = 100;
            timer_ButtonBlink.Tick += new System.EventHandler(Timer_ButtonBlink_Func);
            timer_ButtonBlink.Enabled = true;

            m_nBlinkCount = 0;
            m_bBlink = false;
        }


        #region EventHandler

        public void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            switch (button.Tag)
            {
                /*case OperationButton.Monitoring:
                    ShowForm(m_FormOperationMonitering);
                    break;*/

                case OperationButton.Operator:
                    ShowForm(m_FormOperationOperator);
                    break;

                //case OperationButton.SingleMode:
                //    ShowForm(m_FormOperationSingleMode);
                //    break;

                case OperationButton.CalibrationMode:
                    if (Equipment.Machine_LogIn)
                    {
                        ShowForm(m_FormOperationCalibrationMode);
                    }
                    else
                    {
                        MessageBox.Show("관리자 모드가 아닙니다.", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case OperationButton.ManualMode:
                    ShowForm(m_FormOperationManualMode);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Method
        public void CreatFlowPanelButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(OperationButton)).Length];
            foreach (OperationButton item in Enum.GetValues(typeof(OperationButton)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;

                //  2023. 06. 01.  SCH : 광기술원 전용. (필요한 버튼만 색깔을 바꿔준다. 작업자가 알아보기 쉽게...)
                if ((control[(int)item].Name == "Operator") ||
                    (control[(int)item].Name == "SingleMode") ||
                    (control[(int)item].Name == "CalibrationMode") ||
                    (control[(int)item].Name == "ManualMode") )
                {
                    control[(int)item].ForeColor = Color.Yellow;
                    control[(int)item].Font = new System.Drawing.Font("Arial", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }               
                else
                {
                    control[(int)item].Font = new System.Drawing.Font("Arial", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }

                control[(int)(item)].Click += Button_Click;

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }
        public void ShowForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;

            // this.panelContent.BringToFront();

            this.panelContent.Controls.Add(form);

            //form.Dock = DockStyle.Fill;
            form.BringToFront();
            //      form.FormBorderStyle = FormBorderStyle.None;
            form.Show();
        }

        public void ShowMainForm()
        {
            //timer_ButtonBlink.Enabled = true;
            ShowForm(m_FormOperationMonitering);
        }

        private void Timer_ButtonBlink_Func(object sender, EventArgs e)
        {
            //m_bBlink = !m_bBlink;

            //if ( Equipment.AutoFocus_Failed )
            //{
            //    if ( m_bBlink )
            //    {
            //        this.flowLayoutPanelButton.Controls[4].BackColor = Color.Red;
            //        this.flowLayoutPanelButton.Controls[4].ForeColor = Color.White;
            //    }
            //    else
            //    {
            //        this.flowLayoutPanelButton.Controls[4].BackColor = Color.Yellow;
            //        this.flowLayoutPanelButton.Controls[4].ForeColor = Color.Black;
            //    }
            //}
            //else
            //{
            //    this.flowLayoutPanelButton.Controls[4].BackColor = Color.FromArgb(78, 78, 78);
            //    this.flowLayoutPanelButton.Controls[4].ForeColor = Color.White;
            //}


            //  로그아웃 했을 때 메인 화면을 보여주도록 하기 위함
            if ((Equipment.User_LogOut_1time == true) && (Equipment.User_Mode == null))
            {
                Equipment.User_LogOut_1time = false;

                ShowMainForm();
            }
        }

        #endregion
    }
}
