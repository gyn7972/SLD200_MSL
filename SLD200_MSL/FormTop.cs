using ACS.SPiiPlusNET;
using QMC.Common.Motion.ACS.Motions;
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
using System.IO;
using System.Reflection;
using static QMC.Common.Equipment;

namespace SLD200_MSL
{
    public enum TopButtons
    {
        Alarm,
        Buzzer,
        //Module
    }
    public enum TopButtonSize
    {
        LogoWidth = 250,
        Timewidth = 130
    }
    public delegate void ButtonClickHandler(TopButtons sender);
    public delegate void LogOutClickHandler();

    public partial class FormTop : Form
    {        
        public ButtonClickHandler TopButtonClick { set; get; }
        public FormBaseConfiguration Configuration { get; set; }
        public LogOutClickHandler LogOutClick { get; set; }

        private FormLogIn m_formLogIn;

        protected Timer m_Timer;

        bool m_bBlink;
        int m_nBlink;

        private string m_strRecipeName_Now;
        private string m_strRecipeName_Before;


        public FormTop()
        {
            InitializeComponent();
            Configuration = new FormBaseConfiguration();

            m_formLogIn = new FormLogIn();

            //this.BackColor = Configuration.BaseBackColor;
            this.BackColor = System.Drawing.SystemColors.Control;


            //  C 드라이브 이름 가져오기 (Title 에 쓰기 위함)
            DriveInfo[] drive = DriveInfo.GetDrives();
            string m_strDriveName = drive[0].VolumeLabel;
            //this.label_Title.Text = string.Format("{0}  (CO₂ )", m_strDriveName);

            if (Equipment.Machine_LaserType_CO2)
            {
                this.label_Title.Text = string.Format("{0}  (CO₂ )", Equipment.Machine_Name);
            }
            else
            {
                this.label_Title.Text = string.Format("{0}  (UV)", Equipment.Machine_Name);
            }

            //  파일 수정 날짜 표시하기
            string file = Path.GetFileName(Assembly.GetEntryAssembly().Location);


            label_Ver.Text = string.Format("Ver 1.0.0.1");


            m_bBlink = false;
            m_nBlink = 0;

            m_Timer = new Timer();
            m_Timer.Interval = 100;
            m_Timer.Tick += UpdateUI_Tick;
            m_Timer.Start();

            m_strRecipeName_Before = "";
        }


        Button[] control = new Button[Enum.GetValues(typeof(TopButtons)).Length];
        //string path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
        public void CreateButton()
        {
            foreach (TopButtons item in Enum.GetValues(typeof(TopButtons)))
            {
                int nIndex = (int)item;
                control[nIndex] = new Button();
                control[nIndex].Parent = this;
                control[nIndex].Size = new Size(Configuration.ButtonSize.Width / 2, Configuration .TopSize.Height);
                control[nIndex].Name = item.ToString();
                control[nIndex].Text = item.ToString();
                //flowLayoutPanelButton.Controls.Add(control[(int)item]);
                control[(int)(item)].Click += Button_Click;
                control[nIndex].MouseDown += ButtonDownImageChange;
                control[nIndex].MouseUp += ButtonUpImageChange;
                control[nIndex].FlatStyle = FlatStyle.Flat;
                control[nIndex].FlatAppearance.BorderSize = 1;
                control[nIndex].FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);
                control[nIndex].BackgroundImageLayout = ImageLayout.Center;
                control[nIndex].ImageAlign = ContentAlignment.MiddleCenter;
                control[nIndex].TextAlign = ContentAlignment.BottomCenter;
                control[nIndex].ForeColor = Color.White;
                control[nIndex].TabStop = false;
             
                if(item == TopButtons.Alarm)
                {
                    control[nIndex].Image = SLD200.Properties.Resources.Alarma;
                }
                else if(item == TopButtons.Buzzer)
                {
                    control[nIndex].Image = SLD200.Properties.Resources.Buzzera;
                }
                //else if(item == TopButtons.Module)
                //{
                //    control[nIndex].Image = Properties.Resources.Modulea;
                //}
            }

        }


        public void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            foreach (TopButtons item in Enum.GetValues(typeof(TopButtons)))
            {
                if (button.Name == item.ToString())
                {
                    TopButtonClick(item);
                }
            }
        }
        public void ButtonDownImageChange(object sender, MouseEventArgs e)
        {
            Button botton = sender as Button;
            for (int i = 0; i < control.Length; i++)
            {
                if (e.Button == MouseButtons.Left && botton.Name == control[i].Name)
                {
                    if (botton.Name == TopButtons.Alarm.ToString())
                    {
                        control[i].Image = SLD200.Properties.Resources.Alarma;
                    }
                    else if (botton.Name == TopButtons.Buzzer.ToString())
                    {
                        control[i].Image = SLD200.Properties.Resources.Buzzera;
                    }
                    //else if (botton.Name == TopButtons.Module.ToString())
                    //{
                    //    control[i].Image = Properties.Resources.Modulea;
                    //}
                }
            }
        }
        public void ButtonUpImageChange(object sender, MouseEventArgs e)
        {
            //Button botton = sender as Button;
            
            //for (int i = 0; i < control.Length; i++)
            //{
            //    if (e.Button == MouseButtons.Left && botton.Name == control[i].Name)
            //    {
            //        if (botton.Name == TopButtons.Alarm.ToString())
            //        {
            //            if(control[i].Image == Properties.Resources.AlarmOn)
            //            {
            //                return;
            //            }
            //            control[i].Image = Properties.Resources.Alarma;
            //        }
            //        else if (botton.Name == TopButtons.Buzzer.ToString())
            //        {
            //            control[i].Image = Properties.Resources.Buzzera;
            //        }
            //        else if (botton.Name == TopButtons.Module.ToString())
            //        {
            //            control[i].Image = Properties.Resources.Modulea;
            //        }
            //    }
            //}
        }

        public void EnableButton(bool bEnable)
        {
            //this.flowLayoutPanelButton.Enabled = bEnable;
        }


        private void UpdateUI_Tick(object sender, EventArgs e)
        {
            m_Timer.Stop();
            UpdateUI();
            m_Timer.Start();
        }

        private void UpdateUI()
        {
            //  Blink
            m_nBlink++;
            if ((m_nBlink > 0) && (m_nBlink <= 10))
            {
                m_bBlink = true;
            }
            else if ((m_nBlink > 10) && (m_nBlink <= 13))
            {
                m_bBlink = false;
            }
            else
            {
                m_nBlink = 0;
            }

            //  시간 표시
            label_DateTime.Text = DateTime.Now.ToString("yyyy－MM－dd\r\ntt hh:mm:ss");

            //  로그인 정보
            switch(Equipment.User_LoginMode)
            {
                case (int)UserMode.USER_LOGOUT:
                    label_LoginMode.Text = "Logout";
                    break;

                case (int)UserMode.USER_ADMIN:
                    label_LoginMode.Text = "Administrator";
                    break;

                case (int)UserMode.USER_ENGINEER:
                    label_LoginMode.Text = "Engineer";
                    break;

                case (int)UserMode.USER_OPERATOR:
                    label_LoginMode.Text = "Operator";
                    break;

                default:
                    label_LoginMode.Text = "Logout";
                    break;
            }


            //  Recipe
            if (Equipment.Current_Recipe.Length > 0)
            {
                m_strRecipeName_Now = System.IO.Path.GetFileName(Equipment.Current_Recipe);

                if (m_strRecipeName_Before != m_strRecipeName_Now)
                {
                    label_Title_Recipe.Text = m_strRecipeName_Now;
                }

                m_strRecipeName_Before = m_strRecipeName_Now;
            }
            else
            {
                label_Title_Recipe.Text = "Recipe not loaded.";
            }


            //  System Message
            if (Equipment.CycleStop && 

                Equipment.CycleStopped_LoaderTransfer &&
                Equipment.CycleStopped_UnloaderTransfer && 
                Equipment.CycleStopped_MainWork)
            {
                label_Title_SystemMessage.Text = "Cycle Stopped";
            }
            else if (Equipment.CycleStop && 
                
                (!Equipment.CycleStopped_LoaderTransfer ||
                !Equipment.CycleStopped_UnloaderTransfer ||
                !Equipment.CycleStopped_MainWork))
            {
                label_Title_SystemMessage.Text = "Cycle Stop in Progress...";
            }
            else if (Equipment.AutoRunStatus)
            {
                label_Title_SystemMessage.Text = "Auto Run";
            }
            else if (!Equipment.AutoRunStatus)
            {
                label_Title_SystemMessage.Text = "Ready";
            }
        }

        public void LogInInfo()
        {
            if (m_formLogIn.bLogin == true)
            {
                //m_formTop.EnableButton(m_formLogIn.bLogin);
                Equipment.BottomButtonPanelStatus = true;

                if (m_formLogIn.bLogin_Admin)               //  관리자 (전체 버튼 활성화)
                {
                    //control[0].Enabled = true;                  //  Operation 버튼
                    //control[1].Enabled = true;                  //  Configuration 버튼
                    //control[2].Enabled = true;                  //  Maint 버튼
                    //control[3].Enabled = true;                  //  Recipe 버튼
                    //control[4].Enabled = true;                  //  Setup 버튼
                    //control[5].Enabled = true;                  //  IO 버튼

                    Equipment.Machine_LogIn = true;
                }
                else                                        //  작업자 (Maint 버튼만 활성화)
                {
                    //control[0].Enabled = true;                  //  Operation 버튼
                    //control[1].Enabled = false;                 //  Configuration 버튼
                    //control[2].Enabled = true;                  //  Maint 버튼
                    //control[3].Enabled = false;                 //  Recipe 버튼
                    //control[4].Enabled = false;                 //  Setup 버튼
                    //control[5].Enabled = true;                  //  IO 버튼

                    Equipment.Machine_LogIn = false;
                }
            }
            else
            {
                //m_formTop.EnableButton(m_formLogIn.bLogin);
                Equipment.BottomButtonPanelStatus = false;

                Equipment.Machine_LogIn = false;
            }
        }
    }
}
