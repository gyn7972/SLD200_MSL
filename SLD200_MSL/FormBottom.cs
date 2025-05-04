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
using static SLD200_MSL.Monitoring_CWA150SA;
using System.Linq.Expressions;
using static QMC.Common.Equipment;
//using SpiralLab.Sirius;
using Timer = System.Windows.Forms.Timer;
using Point = System.Drawing.Point;

namespace SLD200_MSL
{
    #region Define
    public enum ButtonBottomType_Ext
    {
        Configuration,
        Maint,
        Recipe,
        SetUp,
        IO,
    }

    public enum ButtonBottomType
    {
        Main,
        Recipe,
        Config,
        Setup,
        Log,
        Lock,
        Logout
    }

    public enum ButtonControl
    {
        Start,
        Stop
    }

    public delegate void EventHandlerBottom(ButtonBottomType type);
    public delegate void EventHandlerBottomExt(ButtonBottomType_Ext type);
    public delegate void ControlBUttonClickEventHandler(ButtonControl type);
    public delegate void ExitClickHandler();
    //public delegate void LogOutClickHandler();
    #endregion
    public partial class FormBottom : Form
    {
        #region Field
        public event EventHandlerBottom m_FormBottomClickEvent;
        public event EventHandlerBottomExt m_FormBottomExtClickEvent;
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


        int m_nBlink;
        bool m_bBlink;

        private bool m_b1Time = false;


        #region Constructor

        public FormBottom()
        {
            Configuration = new FormBaseConfiguration();
            m_formLogIn = new FormLogIn();
            m_formTop = new FormTop();
            //m_Monitoring_CWA150SA = new Monitoring_CWA150SA(); 
            InitializeComponent();
            FlowPanelBottom();
            //FlowControPanelBottom();                              //  똥글배기 버튼 보일 필요 없지

            m_formLogIn.bLogin = false;

            #region ControlConstuctor
            //this.BackColor = Configuration.BaseBackColor;
            this.BackColor = System.Drawing.SystemColors.Control;

            this.flowLayoutPanelStartAndStop.BackColor = Configuration.BaseBackColor;

            this.flowLayoutPanelBottom.Size = new System.Drawing.Size(Configuration.BottomSize.Width - Configuration.BottomButtonSize.Width - this.flowLayoutPanelStartAndStop.Width * 2, Configuration.BottomSize.Height + 5);
            this.flowLayoutPanelBottom.Location = new Point(0, -10);
            //this.flowLayoutPanelBottom.BackColor = Color.Yellow;
            this.flowLayoutPanelStartAndStop.Size = new Size(Configuration.BottomButtonSize.Width * 2, Configuration.BottomSize.Height - 5);
            this.flowLayoutPanelStartAndStop.Location = new Point(this.flowLayoutPanelBottom.Size.Width, this.flowLayoutPanelBottom.Location.Y + 15);

            //this.buttonExit.Location = new Point(Configuration.BottomSize.Width - Configuration.BottomButtonSize.Width, -5);
            //this.buttonExit.Size = Configuration.BottomButtonSize;

            //new Size(Configuration.ButtonSize.Width, (int)FormSize.BottomHeight);
            flowLayoutPanelBottom.BackColor = Configuration.BaseBackColor;

            flowLayoutPanelBottom.Visible = false;
            flowLayoutPanelStartAndStop.Visible = false;

            //this.buttonExit.Size = new Size((int)(Configuration.BottomButtonSize.Width * 2.5) - 8, Configuration.BottomButtonSize.Height - 10 - 8);
            //this.buttonExit.Font = new System.Drawing.Font("Tahoma", 15.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.buttonExit.BackColor = System.Drawing.Color.White;
            //this.buttonExit.ForeColor = System.Drawing.Color.Black;
            //this.buttonExit.Location = new Point(this.Width - buttonExit.Size.Width - 30 + 4, 5 + 4);
            //this.buttonExit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.buttonExit.Image = null;
            //this.buttonExit.TextAlign = ContentAlignment.MiddleCenter;
            //this.buttonExit.FlatStyle = FlatStyle.Standard;
            //this.buttonExit.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            //this.buttonExit.FlatAppearance.BorderSize = 1;
            //this.buttonExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            //this.buttonExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;


            #endregion

            m_nBlink = 0;
            m_bBlink = false;

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

            ////  프로그램 실행 시 Operator 화면으로 시작하기 위함
            //for (int i = 0; i < control.Length; i++)
            //{
            //    if (control[i].Name == ButtonBottomType.Recipe.ToString())
            //    {
            //        control[i].Image = SLD200.Properties.Resources.Recipea;
            //    }
            //    else if (control[i].Name == ButtonBottomType.Operation.ToString())
            //    {
            //        control[i].Image = SLD200.Properties.Resources.Operation;
            //    }
            //    else if (control[i].Name == ButtonBottomType.Configuration.ToString())
            //    {
            //        control[i].Image = SLD200.Properties.Resources.Configurationa;
            //    }
            //    else if (control[i].Name == ButtonBottomType.Log.ToString())
            //    {
            //        control[i].Image = SLD200.Properties.Resources.Loga;
            //    }
            //    else if (control[i].Name == ButtonBottomType.Maint.ToString())
            //    {
            //        control[i].Image = SLD200.Properties.Resources.Mainta;
            //    }
            //    else if (control[i].Name == ButtonBottomType.SetUp.ToString())
            //    {
            //        control[i].Image = SLD200.Properties.Resources.SetUpa;
            //    }
            //    //else if (control[i].Name == ButtonBottomType.IO.ToString())
            //    //{
            //    //    control[i].Image = SLD200.Properties.Resources.IOa;
            //    //}
            //}
        }

        #endregion


        #region Method
        private Button[] control = new Button[Enum.GetValues(typeof(ButtonBottomType)).Length];
        private Button[] control_Ext = new Button[Enum.GetValues(typeof(ButtonBottomType_Ext)).Length];
        private Button[] StartAndStopButton = new Button[Enum.GetValues(typeof(ButtonControl)).Length];
        //private string path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;


        private void FlowPanelBottom()
        {
            foreach (ButtonBottomType ButtonBottomType in Enum.GetValues(typeof(ButtonBottomType)))
            {
                int nIndex = (int)ButtonBottomType;
                control[nIndex] = new Button();
                control[nIndex].Parent = this;
                //control[nIndex].Size = new Size(Configuration.BottomButtonSize.Width * 2, Configuration.BottomButtonSize.Height);
                control[nIndex].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);
                //control[nIndex].Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[nIndex].Font = new System.Drawing.Font("Tahoma", 15.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[nIndex].BackColor = System.Drawing.Color.White;
                control[nIndex].ForeColor = System.Drawing.Color.Black;
                control[nIndex].Location = new Point(10 + ((control[nIndex].Size.Width + 33) * nIndex) + 4, 5 + 4);
                control[nIndex].Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
                control[nIndex].Text = ButtonBottomType.ToString();
                control[nIndex].Name = ButtonBottomType.ToString();
                control[nIndex].Image = null;
                control[nIndex].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;       // ContentAlignment.MiddleCenter;
                control[nIndex].Click += Button_Click;
                control[nIndex].FlatStyle = FlatStyle.Standard;
                control[nIndex].FlatAppearance.BorderColor = System.Drawing.Color.Gray;
                control[nIndex].FlatAppearance.BorderSize = 1;
                control[nIndex].FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                control[nIndex].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
                //control[nIndex].UseVisualStyleBackColor = true;
                //flowLayoutPanelBottom.Controls.Add(control[(int)ButtonBottomType]);
                this.Controls.Add(control[nIndex]);

                control[nIndex].BackgroundImageLayout = ImageLayout.None;
            }

            //  기존 버튼들 (엔지니어 모드일 때만 보이는 버튼들)
            foreach (ButtonBottomType_Ext ButtonBottomType_Ext in Enum.GetValues(typeof(ButtonBottomType_Ext)))
            {
                int nIndex_Ext = (int)ButtonBottomType_Ext;
                control_Ext[nIndex_Ext] = new Button();
                control_Ext[nIndex_Ext].Parent = this;
                control_Ext[nIndex_Ext].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);
                control_Ext[nIndex_Ext].Font = new System.Drawing.Font("Tahoma", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control_Ext[nIndex_Ext].BackColor = System.Drawing.Color.White;
                control_Ext[nIndex_Ext].ForeColor = System.Drawing.Color.Black;
                control_Ext[nIndex_Ext].Location = new Point(10 + ((control[nIndex_Ext].Size.Width + 33) * nIndex_Ext) + 4, 5 + 4);
                control_Ext[nIndex_Ext].Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
                control_Ext[nIndex_Ext].Text = ButtonBottomType_Ext.ToString();
                control_Ext[nIndex_Ext].Name = ButtonBottomType_Ext.ToString();
                control_Ext[nIndex_Ext].Image = null;
                control_Ext[nIndex_Ext].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;       // ContentAlignment.MiddleCenter;
                control_Ext[nIndex_Ext].Click += ButtonExt_Click;
                control_Ext[nIndex_Ext].FlatStyle = FlatStyle.Standard;
                control_Ext[nIndex_Ext].FlatAppearance.BorderColor = System.Drawing.Color.Gray;
                control_Ext[nIndex_Ext].FlatAppearance.BorderSize = 1;
                control_Ext[nIndex_Ext].FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                control_Ext[nIndex_Ext].FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
                control_Ext[nIndex_Ext].Visible = false;
                //control[nIndex_Ext].UseVisualStyleBackColor = true;
                //flowLayoutPanelBottom.Controls.Add(control[(int)ButtonBottomType]);
                this.Controls.Add(control_Ext[nIndex_Ext]);

                control_Ext[nIndex_Ext].BackgroundImageLayout = ImageLayout.None;
            }
        }

        private void FlowControPanelBottom()
        {
            foreach (ButtonControl ButtonBottomType in Enum.GetValues(typeof(ButtonControl)))
            {
                int nIndex = (int)ButtonBottomType;
                StartAndStopButton[nIndex] = new Button();
                StartAndStopButton[nIndex].Parent = this;
                StartAndStopButton[nIndex].Size = new Size(70, 70);
                StartAndStopButton[nIndex].Text = null;
                StartAndStopButton[nIndex].Name = ButtonBottomType.ToString();

                StartAndStopButton[nIndex].Parent = flowLayoutPanelStartAndStop;
                StartAndStopButton[nIndex].Click += ControlButton_Click;
                StartAndStopButton[nIndex].FlatStyle = FlatStyle.Flat;
                StartAndStopButton[nIndex].FlatAppearance.BorderSize = 1;
                StartAndStopButton[nIndex].FlatAppearance.BorderColor = Configuration.BaseBackColor;
                StartAndStopButton[nIndex].BackColor = Configuration.BaseBackColor;
                StartAndStopButton[nIndex].Margin = new System.Windows.Forms.Padding(5);


                if (ButtonBottomType == ButtonControl.Start)
                {
                    StartAndStopButton[nIndex].BackgroundImage = SLD200.Properties.Resources.StartOff;
                }
                else
                {
                    StartAndStopButton[nIndex].BackgroundImage = SLD200.Properties.Resources.StopOff;
                }
                StartAndStopButton[nIndex].BackgroundImageLayout = ImageLayout.Zoom;
                StartAndStopButton[nIndex].ForeColor = Color.Black;

                flowLayoutPanelStartAndStop.Controls.Add(StartAndStopButton[(int)ButtonBottomType]);
                flowLayoutPanelStartAndStop.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            foreach (ButtonBottomType item in Enum.GetValues(typeof(ButtonBottomType)))
            {
                if (button.Name == item.ToString())
                {
                    if (button.Name == "Lock")
                    {
                        if (button.Text == "Unlock")
                        {
                            button.Text = "Lock";
                        }
                        else
                        {
                            button.Text = "Unlock";
                        }
                        //control[(int)item].Text = "Unlock";
                    }
                    else
                    {
                        m_FormBottomClickEvent(item);
                    }
                    break;
                }
            }
        }
        private void ButtonExt_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            foreach (ButtonBottomType_Ext item in Enum.GetValues(typeof(ButtonBottomType_Ext)))
            {
                if (button.Name == item.ToString())
                {
                    m_FormBottomExtClickEvent(item);
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
                        StartAndStopButton[i].BackgroundImage = SLD200.Properties.Resources.StartOff;
                    }
                    else if (StartAndStopButton[i].Name == ButtonControl.Stop.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = SLD200.Properties.Resources.StopOff;
                    }
                }
                if (button.Name == ButtonControl.Start.ToString())
                {
                    button.BackgroundImage = SLD200.Properties.Resources.StartOn;

                    //  Start
                    Equipment.Start();
                }
                else if (button.Name == ButtonControl.Stop.ToString())
                {
                    button.BackgroundImage = SLD200.Properties.Resources.StopOn;

                    Equipment.MachineStop_byAlarm = false;

                    Equipment.MachineStop_byUser = true;                    //  테스트 : Stop 버튼을 누를 때, 얼라인 마크 검출하던 Thread 도 종료시키기 위해 "true" 로 만들어 줌.

                    ////  안전센서로 인한 Stop 인지 확인하는 Flag 초기화
                    Equipment.AreaSensorDetectFlag_Reset = true;
                    //workStage.m_bInManualMoving_SafetySensor_Detected = false;
                    //workStage.m_bInCycleMoving_SafetySensor_Detected = false;

                    //  Stop
                    Equipment.Stop();
                }
            }
        }                
        #endregion

        #region EvnetHandler
        private void buttonExit_Click(object sender, EventArgs e)
        {
            //if (ExitClick != null)
            //    ExitClick();
        }

        private void buttonExit_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                button.Image = SLD200.Properties.Resources.ExitPressed;
            }
        }
        private void buttonExit_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                button.Image = SLD200.Properties.Resources.ExitNormal;
            }
        }


        //private void buttonLogin_Click(object sender, EventArgs e)
        //{
        //    m_formLogIn.StartPosition = FormStartPosition.CenterScreen;
        //    m_formLogIn.bLoginReady = true;
        //    UpdateButtonOkImage();

        //    ////  아으... 비번 치기 구찮어
        //    //this.buttonLogin.Visible = false;
        //    //this.buttonLogin.Enabled = false;
        //    //this.buttonLogOut.Visible = true;
        //    //this.buttonLogOut.Enabled = true;
        //    //m_formLogIn.bLogin = true;
        //    //LogInInfo();
        //    //return;

        //    if (m_formLogIn.ShowDialog() == DialogResult.OK)
        //    {
        //        this.buttonLogin.Visible = false;
        //        this.buttonLogin.Enabled = false;
        //        this.buttonLogOut.Visible = true;
        //        this.buttonLogOut.Enabled = true;

        //        if (Equipment.User_Mode == "관리자")
        //        {
        //            m_formLogIn.bLogin = true;
        //        }

        //        LogInInfo();

        //        Equipment.LogIn_Status = true;
        //        Equipment.AutoLogOut_Executed = false;
        //    }
        //    else if (m_formLogIn.DialogResult == DialogResult.Cancel)
        //    {
        //        UpdateButtonOkImage();
        //    }
        //}

        //private void buttonLogOut_Click(object sender, EventArgs e)
        //{
        //    this.buttonLogin.Visible = true;
        //    this.buttonLogin.Enabled = true;
        //    this.buttonLogOut.Visible = false;
        //    this.buttonLogOut.Enabled = false;
        //    m_formLogIn.bLoginReady = false;
        //    UpdateButtonOkImage();
        //    m_formLogIn.bLogin = false;
        //    LogInInfo();

        //    Equipment.User_Mode = null;
        //    Equipment.User_Name = null;
        //    Equipment.User_LogOut_1time = true;
        //    Equipment.User_AdminMode = false;
        //    Equipment.User_QMC_Engineer = false;

        //    if (LogOutClick != null)
        //        LogOutClick();

        //    //  Operation 화면 버튼으로 변경하기 위함
        //    for (int i = 0; i < control.Length; i++)
        //    {
        //        if (control[i].Name == ButtonBottomType.Recipe.ToString())
        //        {
        //            control[i].Image = SLD200.Properties.Resources.Recipea;
        //        }
        //        else if (control[i].Name == ButtonBottomType.Operation.ToString())
        //        {
        //            control[i].Image = SLD200.Properties.Resources.Operation;
        //        }
        //        else if (control[i].Name == ButtonBottomType.Configuration.ToString())
        //        {
        //            control[i].Image = SLD200.Properties.Resources.Configurationa;
        //        }
        //        else if (control[i].Name == ButtonBottomType.Maint.ToString())
        //        {
        //            control[i].Image = SLD200.Properties.Resources.Mainta;
        //        }
        //        else if (control[i].Name == ButtonBottomType.SetUp.ToString())
        //        {
        //            control[i].Image = SLD200.Properties.Resources.SetUpa;
        //        }
        //        else if (control[i].Name == ButtonBottomType.IO.ToString())
        //        {
        //            control[i].Image = SLD200.Properties.Resources.IOa;
        //        }
        //    }

        //    Equipment.LogIn_Status = false;
        //}
        #endregion

        public void updateAutoRun()
        {
            //run 상태시에 Main 및 Recipe 
            //Equipment.AutoManualStatus //AutoRunStatus
            if (!Equipment.AutoRunStatus)
            {
                control[0].Enabled = true;                  //  Operation 버튼
                control[1].Enabled = true;                  //  Configuration 버튼
                control[2].Enabled = true;                  //  Maint 버튼
                control[3].Enabled = true;                  //  Recipe 버튼
                control[4].Enabled = true;                  //  Setup 버튼
                control[5].Enabled = true;                  //  IO 버튼
            }
            else
            {
                control[0].Enabled = true;                  //  Operation 버튼
                control[1].Enabled = true;                  //  Configuration 버튼
                control[2].Enabled = false;                 //  Maint 버튼
                control[3].Enabled = false;                 //  Recipe 버튼
                control[4].Enabled = false;                 //  Setup 버튼
                control[5].Enabled = false;                 //  IO 버튼
            }
        }
        public void UpdateLogInInfo()
        {
            //if (m_formLogIn.bLogin == true)
            if (Equipment.LogIn_Status == true)
            {
                //if (Equipment.User_AdminMode)               //  관리자 (전체 버튼 활성화)
                if ((Equipment.User_LoginMode == (int)UserMode.USER_ADMIN) || (Equipment.User_LoginMode == (int)UserMode.USER_ENGINEER))               //  관리자 (전체 버튼 활성화)
                {
                    control[0].Enabled = true;                  //  Operation 버튼
                    control[1].Enabled = true;                  //  Configuration 버튼
                    control[2].Enabled = true;                  //  Maint 버튼
                    control[3].Enabled = true;                  //  Recipe 버튼
                    control[4].Enabled = true;                  //  Setup 버튼
                    control[5].Enabled = true;                  //  IO 버튼

                    //Equipment.Machine_LogIn = true;
                }
                else                                        //  작업자 (Maint 버튼만 활성화)
                {
                    control[0].Enabled = true;                  //  Operation 버튼
                    control[1].Enabled = false;                 //  Configuration 버튼
                    control[2].Enabled = true;                  //  Maint 버튼
                    control[3].Enabled = false;                 //  Recipe 버튼
                    control[4].Enabled = false;                 //  Setup 버튼
                    control[5].Enabled = true;                  //  IO 버튼

                    //Equipment.Machine_LogIn = false;
                }

                m_b1Time = true;
            }
            //else
            //{
            //    //Equipment.Machine_LogIn = false;

            //    //  Operation 화면 버튼으로 변경하기 위함 (Login 상태에서 Logout 상태로 바뀔 때 1번만 실행되도록)
            //    if (m_b1Time)
            //    {
            //        m_b1Time = false;

            //        for (int i = 0; i < control.Length; i++)
            //        {
            //            if (control[i].Name == ButtonBottomType.Recipe.ToString())
            //            {
            //                control[i].Image = SLD200.Properties.Resources.Recipea;
            //            }
            //            else if (control[i].Name == ButtonBottomType.Operation.ToString())
            //            {
            //                control[i].Image = SLD200.Properties.Resources.Operation;
            //            }
            //            else if (control[i].Name == ButtonBottomType.Configuration.ToString())
            //            {
            //                control[i].Image = SLD200.Properties.Resources.Configurationa;
            //            }
            //            else if (control[i].Name == ButtonBottomType.Log.ToString())
            //            {
            //                control[i].Image = SLD200.Properties.Resources.Loga;
            //            }
            //            else if (control[i].Name == ButtonBottomType.Maint.ToString())
            //            {
            //                control[i].Image = SLD200.Properties.Resources.Mainta;
            //            }
            //            else if (control[i].Name == ButtonBottomType.SetUp.ToString())
            //            {
            //                control[i].Image = SLD200.Properties.Resources.SetUpa;
            //            }
            //            //else if (control[i].Name == ButtonBottomType.IO.ToString())
            //            //{
            //            //    control[i].Image = SLD200.Properties.Resources.IOa;
            //            //}
            //        }
            //    }
            //}
        }

        private void UpdateUI_Tick(object sender, EventArgs e)
        {
            m_Timer.Stop();
            //UpdateStartStopButton();                      //  Start, Stop 버튼 상태 업데이트 (숨김)

            //  Blink
            m_nBlink++;
            if ((m_nBlink > 0) && (m_nBlink <= 4))
            {
                m_bBlink = true;
            }
            else if ((m_nBlink > 4) && (m_nBlink <= 8))
            {
                m_bBlink = false;
            }
            else
            {
                m_nBlink = 0;
            }

            //  자동 로그아웃을 위해 추가됨
            //if (Equipment.AutoLogOut_Execute)
            //{
            //    Equipment.AutoLogOut_Execute = false;
            //    Equipment.AutoLogOut_Executed = true;

            //    buttonLogOut.PerformClick();
            //}

            //if ( Equipment.BottomButtonPanelStatus)
            //{
            //    flowLayoutPanelBottom.Enabled = true;
            //}
            //else
            //{
            //    flowLayoutPanelBottom.Enabled = false;
            //}

            //UpdateLogInInfo();                                                            //  OP 와 관리자에 따라 버튼을 다르게 할 경우에 수정해서 사용

            updateAutoRun();    //장비 AutoRun 상태 시 버튼 제한.

            //  로그인 모드에 따라 보이는 버튼을 다르게
            if (Equipment.User_LoginMode == (int)UserMode.USER_ENGINEER)                    //  엔지니어 모드일 경우 기존 버튼도 보이기
            {
                //  새로운 버튼 중 Recipe, Config, Setup, Log 버튼은 크기 절반으로 줄이기
                control[(int)ButtonBottomType.Recipe].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) + 2);
                control[(int)ButtonBottomType.Config].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) + 2);
                control[(int)ButtonBottomType.Setup].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) + 2);
                control[(int)ButtonBottomType.Log].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) + 2);
                control[(int)ButtonBottomType.Lock].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) + 2);

                //  기존 버튼 보이기
                control_Ext[(int)ButtonBottomType_Ext.Recipe].Visible = true;
                control_Ext[(int)ButtonBottomType_Ext.Configuration].Visible = true;
                control_Ext[(int)ButtonBottomType_Ext.SetUp].Visible = true;
                control_Ext[(int)ButtonBottomType_Ext.Maint].Visible = true;
                control_Ext[(int)ButtonBottomType_Ext.IO].Visible = true;

                //  기존 버튼 크기 조절
                control_Ext[(int)ButtonBottomType_Ext.Recipe].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) - 6);
                control_Ext[(int)ButtonBottomType_Ext.Configuration].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) - 6);
                control_Ext[(int)ButtonBottomType_Ext.SetUp].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) - 6);
                control_Ext[(int)ButtonBottomType_Ext.Maint].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) - 6);
                control_Ext[(int)ButtonBottomType_Ext.IO].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), ((Configuration.BottomButtonSize.Height - 10 - 8) / 2) - 6);

                //  기존 버튼 위치
                control_Ext[(int)ButtonBottomType_Ext.Recipe].Location = new Point(control[(int)ButtonBottomType.Recipe].Location.X, control[(int)ButtonBottomType.Recipe].Location.Y + control[(int)ButtonBottomType.Recipe].Size.Height + 4);
                control_Ext[(int)ButtonBottomType_Ext.Configuration].Location = new Point(control[(int)ButtonBottomType.Config].Location.X, control[(int)ButtonBottomType.Config].Location.Y + control[(int)ButtonBottomType.Config].Size.Height + 4);
                control_Ext[(int)ButtonBottomType_Ext.SetUp].Location = new Point(control[(int)ButtonBottomType.Setup].Location.X, control[(int)ButtonBottomType.Setup].Location.Y + control[(int)ButtonBottomType.Setup].Size.Height + 4);
                control_Ext[(int)ButtonBottomType_Ext.Maint].Location = new Point(control[(int)ButtonBottomType.Log].Location.X, control[(int)ButtonBottomType.Log].Location.Y + control[(int)ButtonBottomType.Log].Size.Height + 4);
                control_Ext[(int)ButtonBottomType_Ext.IO].Location = new Point(control[(int)ButtonBottomType.Lock].Location.X, control[(int)ButtonBottomType.Lock].Location.Y + control[(int)ButtonBottomType.Lock].Size.Height + 4);
            }
            else                                                                            //  그 외에는 새로운 메뉴만 보이도록
            {
                //  새로운 버튼들 크기 원복
                control[(int)ButtonBottomType.Recipe].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);
                control[(int)ButtonBottomType.Config].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);
                control[(int)ButtonBottomType.Setup].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);
                control[(int)ButtonBottomType.Log].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);
                control[(int)ButtonBottomType.Lock].Size = new Size((int)((Configuration.BottomButtonSize.Width * 2.5) - 8), Configuration.BottomButtonSize.Height - 10 - 8);

                //  기존 버튼 숨기기
                control_Ext[(int)ButtonBottomType_Ext.Recipe].Visible = false;
                control_Ext[(int)ButtonBottomType_Ext.Configuration].Visible = false;
                control_Ext[(int)ButtonBottomType_Ext.SetUp].Visible = false;
                control_Ext[(int)ButtonBottomType_Ext.Maint].Visible = false;
                control_Ext[(int)ButtonBottomType_Ext.IO].Visible = false;
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
                        StartAndStopButton[i].BackgroundImage = SLD200.Properties.Resources.StartOn;
                    }
                    else if (StartAndStopButton[i].Name == ButtonControl.Stop.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = SLD200.Properties.Resources.StopOff;
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
                        StartAndStopButton[i].BackgroundImage = SLD200.Properties.Resources.StartOff;
                    }
                    else if (StartAndStopButton[i].Name == ButtonControl.Stop.ToString())
                    {
                        StartAndStopButton[i].BackgroundImage = SLD200.Properties.Resources.StopOn;
                    }
                }

                //CommonModule.Instance.OperationButtons.Start(false);
                //CommonModule.Instance.OperationButtons.Stop(true);
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
}
