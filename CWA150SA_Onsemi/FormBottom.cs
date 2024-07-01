using QMC.Common;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Parts;
using static CWA150SA_Onsemi300.Monitoring_CWA150SA;
using System.Linq.Expressions;

namespace CWA150SA_Onsemi300
{
    #region Define
    public enum ButtonBottomType
    {
        Operation,
        Configuration,
        Maint,
        Recipe,
        SetUp,
        IO
    }

    public enum ButtonControl
    {
        Start,
        Stop
    }

    public delegate void EventHandlerBottom(ButtonBottomType type);
    public delegate void ControlBUttonClickEventHandler(ButtonControl type);
    public delegate void ExitClickHandler();
    public delegate void LogOutClickHandler();
    #endregion
    public partial class FormBottom : Form
    {
        #region Field
        public event EventHandlerBottom m_FormBottomClickEvent;
        private FormLogIn m_formLogIn;
        public FormTop m_formTop;
        public Monitoring_CWA150SA m_Monitoring_CWA150SA;
        public bool m_bEnable;
        public ControlBUttonClickEventHandler m_ControlbuttonClick;
        #endregion


        #region Property

        public FormBaseConfiguration Configuration { get; set; }
        public ExitClickHandler ExitClick { get; set; }
        public LogOutClickHandler LogOutClick { get; set; }
        protected Timer m_Timer;
        #endregion

        #region Constructor

        public FormBottom()
        {
            Configuration = new FormBaseConfiguration();
            m_formLogIn = new FormLogIn();
            m_formTop = new FormTop();
            //m_Monitoring_CWA150SA = new Monitoring_CWA150SA(); 
            InitializeComponent();
            FlowPanelBottom();
            FlowControPanelBottom();

            m_formLogIn.bLogin = false;
            LogInInfo();

            //this.buttonLogin.Hide();
            this.buttonLogOut.Hide();

            #region ControlConstuctor
            this.BackColor = Color.FromArgb(78, 78, 78);

            this.flowLayoutPanelStartAndStop.BackColor = Color.FromArgb(78, 78, 78);

            this.flowLayoutPanelBottom.Size = new System.Drawing.Size(Configuration.BottomSize.Width - Configuration.BottomButtonSize.Width - this.flowLayoutPanelStartAndStop.Width * 2, Configuration.BottomSize.Height);
            this.flowLayoutPanelBottom.Location = new Point(0, -5);
            //this.flowLayoutPanelBottom.BackColor = Color.Yellow;
            this.flowLayoutPanelStartAndStop.Size = new Size(Configuration.BottomButtonSize.Width * 2, Configuration.BottomSize.Height - 5);
            this.flowLayoutPanelStartAndStop.Location = new Point(this.flowLayoutPanelBottom.Size.Width, this.flowLayoutPanelBottom.Location.Y + 10);

            this.buttonExit.Location = new Point(Configuration.BottomSize.Width - Configuration.BottomButtonSize.Width, -5);
            this.buttonExit.Size = Configuration.BottomButtonSize;

            this.buttonLogin.Location = new Point(this.buttonExit.Location.X - this.buttonLogin.Width, -5);
            this.buttonLogin.Size = Configuration.BottomButtonSize;

            this.buttonLogOut.Location = new Point(this.buttonLogin.Location.X, buttonLogin.Location.Y);
            this.buttonLogOut.Size = Configuration.BottomButtonSize;

            //new Size(Configuration.ButtonSize.Width, (int)FormSize.BottomHeight);
            flowLayoutPanelBottom.BackColor = Color.FromArgb(78, 78, 78);
            this.buttonExit.Parent = this;
            this.buttonExit.FlatStyle = FlatStyle.Flat;
            this.buttonExit.FlatAppearance.BorderSize = 1;
            this.buttonExit.FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);
            this.buttonExit.BackColor = Color.FromArgb(78, 78, 78);
            this.buttonExit.ForeColor = Color.White;

            this.buttonLogin.Parent = this;
            this.buttonLogin.FlatStyle = FlatStyle.Flat;
            this.buttonLogin.FlatAppearance.BorderSize = 1;
            this.buttonLogin.FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);
            this.buttonLogin.BackColor = Color.FromArgb(78, 78, 78);
            this.buttonLogin.ForeColor = Color.White;

            this.buttonLogOut.Parent = this;
            this.buttonLogOut.FlatStyle = FlatStyle.Flat;
            this.buttonLogOut.FlatAppearance.BorderSize = 1;
            this.buttonLogOut.FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);
            this.buttonLogOut.BackColor = Color.FromArgb(78, 78, 78);
            this.buttonLogOut.ForeColor = Color.White;
            this.buttonLogOut.Visible = false;
            this.buttonLogOut.Enabled = false;
            this.buttonLogOut.TextAlign = ContentAlignment.BottomCenter;

            #endregion

            m_Timer = new Timer();
            m_Timer.Interval = 100;
            m_Timer.Tick += UpdateUI_Tick;
            m_Timer.Start();


            ////  아으... 비번 치기 구찮어
            //this.buttonLogin.Visible = false;
            //this.buttonLogin.Enabled = false;
            //this.buttonLogOut.Visible = true;
            //this.buttonLogOut.Enabled = true;
            //m_formLogIn.bLogin = true;
            //LogInInfo();
        }
        #endregion


        #region Method
        private Button[] control = new Button[Enum.GetValues(typeof(ButtonBottomType)).Length];
        private Button[] StartAndStopButton = new Button[Enum.GetValues(typeof(ButtonControl)).Length];
        //private string path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;


        private void FlowPanelBottom()
        {
            foreach (ButtonBottomType ButtonBottomType in Enum.GetValues(typeof(ButtonBottomType)))
            {
                int nIndex = (int)ButtonBottomType;
                control[nIndex] = new Button();
                control[nIndex].Parent = this;
                control[nIndex].Size = new Size(Configuration.BottomButtonSize.Width, Configuration.BottomButtonSize.Height);
                control[nIndex].Text = ButtonBottomType.ToString();
                control[nIndex].Name = ButtonBottomType.ToString();

                flowLayoutPanelBottom.Controls.Add(control[(int)ButtonBottomType]);
                control[nIndex].Click += Button_Click;
                control[nIndex].FlatStyle = FlatStyle.Flat;
                control[nIndex].FlatAppearance.BorderSize = 1;
                control[nIndex].FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);

                if (ButtonBottomType == ButtonBottomType.Recipe)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.Recipea;
                }
                else if (ButtonBottomType == ButtonBottomType.Operation)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.Operationa;
                }
                else if (ButtonBottomType == ButtonBottomType.Configuration)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.Configurationa;
                }
                else if (ButtonBottomType == ButtonBottomType.Maint)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.Mainta;
                }
                else if (ButtonBottomType == ButtonBottomType.SetUp)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.SetUpa;
                }
                else if (ButtonBottomType == ButtonBottomType.IO)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.IOa;
                }

                control[nIndex].ImageAlign = ContentAlignment.MiddleCenter;
                control[nIndex].TextAlign = ContentAlignment.BottomCenter;

                control[nIndex].ForeColor = Color.White;
            }
        }

        private void FlowControPanelBottom()
        {
            foreach (ButtonControl ButtonBottomType in Enum.GetValues(typeof(ButtonControl)))
            {
                int nIndex = (int)ButtonBottomType;
                StartAndStopButton[nIndex] = new Button();
                StartAndStopButton[nIndex].Parent = this;
                StartAndStopButton[nIndex].Size = new Size(80, 80);
                StartAndStopButton[nIndex].Text = null;
                StartAndStopButton[nIndex].Name = ButtonBottomType.ToString();

                StartAndStopButton[nIndex].Parent = flowLayoutPanelStartAndStop;
                StartAndStopButton[nIndex].Click += ControlButton_Click;
                StartAndStopButton[nIndex].FlatStyle = FlatStyle.Flat;
                StartAndStopButton[nIndex].FlatAppearance.BorderSize = 1;
                StartAndStopButton[nIndex].FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);
                StartAndStopButton[nIndex].BackColor = Color.FromArgb(78, 78, 78);
                StartAndStopButton[nIndex].Margin = new System.Windows.Forms.Padding(5);


                if (ButtonBottomType == ButtonControl.Start)
                {
                    StartAndStopButton[nIndex].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StartOff;
                }
                else
                {
                    StartAndStopButton[nIndex].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StopOff;
                }
                StartAndStopButton[nIndex].BackgroundImageLayout = ImageLayout.Zoom;
                StartAndStopButton[nIndex].ForeColor = Color.White;

                flowLayoutPanelStartAndStop.Controls.Add(StartAndStopButton[(int)ButtonBottomType]);
                flowLayoutPanelStartAndStop.FlowDirection = FlowDirection.LeftToRight;
            }
        }


        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            for (int i = 0; i < control.Length; i++)
            {
                if (control[i].Name == ButtonBottomType.Recipe.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Recipea;
                }
                else if (control[i].Name == ButtonBottomType.Operation.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Operationa;
                }
                else if (control[i].Name == ButtonBottomType.Configuration.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Configurationa;
                }
                else if (control[i].Name == ButtonBottomType.Maint.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Mainta;
                }
                else if (control[i].Name == ButtonBottomType.SetUp.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.SetUpa;
                }
                else if (control[i].Name == ButtonBottomType.IO.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.IOa;
                }
            }

            if (button.Name == ButtonBottomType.Recipe.ToString())
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.Recipe;
            }
            else if (button.Name == ButtonBottomType.Operation.ToString())
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.Operation;
            }
            else if (button.Name == ButtonBottomType.Configuration.ToString())
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.Configuration;
            }
            else if (button.Name == ButtonBottomType.Maint.ToString())
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.Maint;
            }
            else if (button.Name == ButtonBottomType.SetUp.ToString())
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.SetUp;
            }
            else if (button.Name == ButtonBottomType.IO.ToString())
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.IO;
            }

            foreach (ButtonBottomType item in Enum.GetValues(typeof(ButtonBottomType)))
            {
                if (button.Name == item.ToString())
                {
                    m_FormBottomClickEvent(item);
                    break;
                }
            }
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            foreach (ButtonControl item in Enum.GetValues(typeof(ButtonControl)))
            {
                for (int i = 0; i < StartAndStopButton.Length; i++)
                {
                    if (StartAndStopButton[i].Name == ButtonControl.Start.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StartOff;
                    }
                    else if (StartAndStopButton[i].Name == ButtonControl.Stop.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StopOff;
                    }
                }
                if (button.Name == ButtonControl.Start.ToString())
                {
                    button.BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StartOn;

                    //  Start
                    Equipment.Start();
                }
                else if (button.Name == ButtonControl.Stop.ToString())
                {
                    button.BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StopOn;

                    Equipment.MachineStop_byAlarm = false;

                    Equipment.MachineStop_byUser = true;                    //  테스트 : Stop 버튼을 누를 때, 얼라인 마크 검출하던 Thread 도 종료시키기 위해 "true" 로 만들어 줌.

                    ////  안전센서로 인한 Stop 인지 확인하는 Flag 초기화
                    Equipment.AreaSensorDetectFlag_Reset = true;
                    //waferProbeAlign.m_bInManualMoving_SafetySensor_Detected = false;
                    //waferProbeAlign.m_bInCycleMoving_SafetySensor_Detected = false;

                    //  Stop
                    Equipment.Stop();
                }
            }
        }

        private void UpdateButtonOkImage()
        {
            if (m_formLogIn.bLoginReady)
            {
                this.buttonLogin.Image = CWA150SA_Onsemi.Properties.Resources.LogIn;
            }
            else
            {
                this.buttonLogin.Image = CWA150SA_Onsemi.Properties.Resources.LogIna;
            }
        }

        public void LogInInfo()
        {
            if (m_formLogIn.bLogin == true)
            {
                m_formTop.EnableButton(m_formLogIn.bLogin);
                this.flowLayoutPanelBottom.Enabled = true;

                if (m_formLogIn.bLogin_Admin)               //  관리자 (전체 버튼 활성화)
                {
                    control[0].Enabled = true;                  //  Operation 버튼
                    control[1].Enabled = true;                  //  Configuration 버튼
                    control[2].Enabled = true;                  //  Maint 버튼
                    control[3].Enabled = true;                  //  Recipe 버튼
                    control[4].Enabled = true;                  //  Setup 버튼
                    control[5].Enabled = true;                  //  IO 버튼

                    Equipment.Machine_LogIn = true;
                }
                else                                        //  작업자 (Maint 버튼만 활성화)
                {
                    control[0].Enabled = true;                  //  Operation 버튼
                    control[1].Enabled = false;                 //  Configuration 버튼
                    control[2].Enabled = true;                  //  Maint 버튼
                    control[3].Enabled = false;                 //  Recipe 버튼
                    control[4].Enabled = false;                 //  Setup 버튼
                    control[5].Enabled = true;                  //  IO 버튼

                    Equipment.Machine_LogIn = false;
                }
            }
            else
            {
                m_formTop.EnableButton(m_formLogIn.bLogin);
                this.flowLayoutPanelBottom.Enabled = false;

                Equipment.Machine_LogIn = false;
            }
        }
        #endregion

        #region EvnetHandler
        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (ExitClick != null)
                ExitClick();
        }

        private void buttonExit_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.ExitPressed;
            }
        }
        private void buttonExit_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                button.Image = CWA150SA_Onsemi.Properties.Resources.ExitNormal;
            }
        }


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            m_formLogIn.StartPosition = FormStartPosition.CenterScreen;
            m_formLogIn.bLoginReady = true;
            UpdateButtonOkImage();

            ////  아으... 비번 치기 구찮어
            //this.buttonLogin.Visible = false;
            //this.buttonLogin.Enabled = false;
            //this.buttonLogOut.Visible = true;
            //this.buttonLogOut.Enabled = true;
            //m_formLogIn.bLogin = true;
            //LogInInfo();
            //return;

            if (m_formLogIn.ShowDialog() == DialogResult.OK)
            {
                this.buttonLogin.Visible = false;
                this.buttonLogin.Enabled = false;
                this.buttonLogOut.Visible = true;
                this.buttonLogOut.Enabled = true;

                if (Equipment.User_Mode == "관리자")
                {
                    m_formLogIn.bLogin = true;
                }

                LogInInfo();

                Equipment.LogIn_Status = true;
                Equipment.AutoLogOut_Executed = false;
            }
            else if (m_formLogIn.DialogResult == DialogResult.Cancel)
            {
                UpdateButtonOkImage();
            }
        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            this.buttonLogin.Visible = true;
            this.buttonLogin.Enabled = true;
            this.buttonLogOut.Visible = false;
            this.buttonLogOut.Enabled = false;
            m_formLogIn.bLoginReady = false;
            UpdateButtonOkImage();
            m_formLogIn.bLogin = false;
            LogInInfo();

            Equipment.User_Mode = null;
            Equipment.User_Name = null;
            Equipment.User_LogOut_1time = true;
            Equipment.User_AdminMode = false;
            Equipment.User_QMC_Engineer = false;

            if (LogOutClick != null)
                LogOutClick();

            //  Operation 화면 버튼으로 변경하기 위함
            for (int i = 0; i < control.Length; i++)
            {
                if (control[i].Name == ButtonBottomType.Recipe.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Recipea;
                }
                else if (control[i].Name == ButtonBottomType.Operation.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Operation;
                }
                else if (control[i].Name == ButtonBottomType.Configuration.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Configurationa;
                }
                else if (control[i].Name == ButtonBottomType.Maint.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.Mainta;
                }
                else if (control[i].Name == ButtonBottomType.SetUp.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.SetUpa;
                }
                else if (control[i].Name == ButtonBottomType.IO.ToString())
                {
                    control[i].Image = CWA150SA_Onsemi.Properties.Resources.IOa;
                }
            }

            Equipment.LogIn_Status = false;
        }
        #endregion

        private void UpdateUI_Tick(object sender, EventArgs e)
        {
            m_Timer.Stop();
            UpdateStartStopButton();

            //  자동 로그아웃을 위해 추가됨
            if (Equipment.AutoLogOut_Execute)
            {
                Equipment.AutoLogOut_Execute = false;
                Equipment.AutoLogOut_Executed = true;

                buttonLogOut.PerformClick();
            }

            m_Timer.Start();
        }
        private void UpdateStartStopButton()
        {
            if (Equipment.GetMachineRunStatus())
            {
                for (int i = 0; i < StartAndStopButton.Length; i++)
                {
                    if (StartAndStopButton[i].Name == ButtonControl.Start.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StartOn;
                    }
                    else if (StartAndStopButton[i].Name == ButtonControl.Stop.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StopOff;
                    }
                }

                //CommonModule.Instance.OperationButtons.Start(true);
                //CommonModule.Instance.OperationButtons.Stop(false);
            }
            else
            {
                for (int i = 0; i < StartAndStopButton.Length; i++)
                {
                    if (StartAndStopButton[i].Name == ButtonControl.Start.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StartOff;
                    }
                    else if (StartAndStopButton[i].Name == ButtonControl.Stop.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = CWA150SA_Onsemi.Properties.Resources.StopOn;
                    }
                }

                //CommonModule.Instance.OperationButtons.Start(false);
                //CommonModule.Instance.OperationButtons.Stop(true);
            }


            //  ProbeCard Clamp Type 에 따라 UI 변경
            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
            {
                if (button_Lamp0.Visible)
                {
                    button_Lamp0.Visible = false;
                }

                //if (button_Lamp1.Visible)
                //{
                //    button_Lamp1.Visible = false;
                //}                
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
            {
                if (!button_Lamp0.Visible)
                {
                    button_Lamp0.Visible = true;
                }

                //if (!button_Lamp1.Visible)
                //{
                //    button_Lamp1.Visible = true;
                //}

                //  실내 조명 상태
                if (CommonModule.Instance.TowerLamp.IsLamp0())
                {
                    if (button_Lamp0.BackColor != Color.GreenYellow)
                    {
                        button_Lamp0.BackColor = Color.GreenYellow;
                    }
                }
                else
                {
                    if (button_Lamp0.BackColor != Color.LightGray)
                    {
                        button_Lamp0.BackColor = Color.LightGray;
                    }
                }

                //if (CommonModule.Instance.TowerLamp.IsLamp1())
                //{
                //    if (button_Lamp1.BackColor != Color.GreenYellow)
                //    {
                //        button_Lamp1.BackColor = Color.GreenYellow;
                //    }
                //}
                //else
                //{
                //    if (button_Lamp1.BackColor != Color.LightGray)
                //    {
                //        button_Lamp1.BackColor = Color.LightGray;
                //    }
                //}
            }
        }

        private void button_Lamp1_Click(object sender, EventArgs e)
        {
            //  실내조명 1, 2 켜기(끄기)

            if (CommonModule.Instance.TowerLamp.IsLamp0())
            {
                CommonModule.Instance.TowerLamp.Lamp0_Off();
                CommonModule.Instance.TowerLamp.Lamp1_Off();
            }
            else
            {
                CommonModule.Instance.TowerLamp.Lamp0_On();
                CommonModule.Instance.TowerLamp.Lamp1_On();
            }
        }

        private void button_Lamp2_Click(object sender, EventArgs e)
        {
            //  실내조명 2 켜기

            if (CommonModule.Instance.TowerLamp.IsLamp1())
            {
                CommonModule.Instance.TowerLamp.Lamp1_Off();
            }
            else
            {
                CommonModule.Instance.TowerLamp.Lamp1_On();
            }
        }
    }
}
