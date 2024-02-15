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

namespace CWA150SA_Onsemi300
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
    public partial class FormTop : Form
    {        
        public ButtonClickHandler TopButtonClick { set; get; }
        public FormBaseConfiguration Configuration { get; set; }

        protected Timer m_Timer;

        bool m_bBlink;
        int m_nBlink;

        public FormTop()
        {
            InitializeComponent();
            Configuration = new FormBaseConfiguration();

            this.BackColor = Color.FromArgb(78, 78, 78);

            this.panelLogo.Location = new Point(5, 5);
            this.panelLogo.Size = new Size((Point)Configuration.TopButtonLogoSize);
            this.panelLogo.BackColor= Color.FromArgb(78, 78, 78);
            this.panelLogo.BackgroundImage = CWA150SA_Onsemi.Properties.Resources.Logo;
            this.panelLogo.BackgroundImageLayout = ImageLayout.Stretch;     //  ImageLayout.Center;
			
            this.flowLayoutPanelButton.Size = new Size(Configuration.TopSize.Width - Configuration.TopButtonTimeSize.Width - Configuration.TopButtonLogoSize.Width, Configuration.TopSize.Height);
            this.flowLayoutPanelButton.Location = new Point(Configuration.TopButtonLogoSize.Width, -5);

            this.panelTime.Size = new Size(Configuration.TopButtonTimeSize.Width, Configuration.TopSize.Height);
            this.panelTime.Location = new Point(Configuration.TopSize.Width - Configuration.TopButtonTimeSize.Width, -5);
            CreateButton();
            flowLayoutPanelButton.BackColor = Color.FromArgb(78, 78, 78);
            panelTime.BackColor = Color.FromArgb(78, 78, 78);

            this.lbTitle.BackColor = Color.FromArgb(78, 78, 78);
            //lbTitle.Location = new Point(panelLogo.Right + 10, 40);
            this.lbTitle.Location = new Point(panelLogo.Left, panelLogo.Top + panelLogo.Size.Height);
            this.lbTitle.Size = new System.Drawing.Size(panelLogo.Size.Width, 25);
            this.lbTitle.Text = "CWA150SA  (Onsemi)";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;


            m_bBlink = false;
            m_nBlink = 0;

            m_Timer = new Timer();
            m_Timer.Interval = 100;
            m_Timer.Tick += UpdateUI_Tick;
            m_Timer.Start();
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
                control[nIndex].Size = new Size(Configuration.ButtonSize.Width, Configuration .TopSize.Height);
                control[nIndex].Name = item.ToString();
                control[nIndex].Text = item.ToString();
                flowLayoutPanelButton.Controls.Add(control[(int)item]);
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
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.Alarma;
                }
                else if(item == TopButtons.Buzzer)
                {
                    control[nIndex].Image = CWA150SA_Onsemi.Properties.Resources.Buzzera;
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
                        control[i].Image = CWA150SA_Onsemi.Properties.Resources.Alarma;
                    }
                    else if (botton.Name == TopButtons.Buzzer.ToString())
                    {
                        control[i].Image = CWA150SA_Onsemi.Properties.Resources.Buzzera;
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
            this.flowLayoutPanelButton.Enabled = bEnable;
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

            //  얼라인 후 패킹 자동 시작
            if (Equipment.Packing_AutoStart_Mode)
            {
                //if (m_bBlink)
                //{
                //    lblJoyStickMode.Visible = true;
                //}
                //else
                //{
                //    lblJoyStickMode.Visible = false;
                //}

                if (lblWaferAlignAfterPackingAutoStart.BackColor != Color.GreenYellow)
                {
                    lblWaferAlignAfterPackingAutoStart.BackColor = Color.GreenYellow;
                }
            }
            else
            {
                if (lblWaferAlignAfterPackingAutoStart.BackColor != Color.Gray)
                {
                    lblWaferAlignAfterPackingAutoStart.BackColor = Color.Gray;
                }
            }

            //  얼라인 후 얼라인 정밀도 확인
            if (Equipment.AlignErrorCheck_AutoStart_Mode)
            {
                if (lblWaferAlignAfterAlignErrorCheckStart.BackColor != Color.GreenYellow)
                {
                    lblWaferAlignAfterAlignErrorCheckStart.BackColor = Color.GreenYellow;
                }
            }
            else
            {
                if (lblWaferAlignAfterAlignErrorCheckStart.BackColor != Color.Gray)
                {
                    lblWaferAlignAfterAlignErrorCheckStart.BackColor = Color.Gray;
                }
            }

            //  패킹 오프셋 사용 여부
            if (Equipment.PackingOffset_Use)
            {
                if (lblWaferOffsetMoveBeforePacking.BackColor != Color.GreenYellow)
                {
                    lblWaferOffsetMoveBeforePacking.BackColor = Color.GreenYellow;
                }
            }
            else
            {
                if (lblWaferOffsetMoveBeforePacking.BackColor != Color.Gray)
                {
                    lblWaferOffsetMoveBeforePacking.BackColor = Color.Gray;
                }
            }

            //  얼라인 시작 전 PAK 관로 확인
            if (Equipment.Pak_AirLineCheck_Use)
            {
                if (lblPackAirLineCheck_Before_AlignStart.BackColor != Color.GreenYellow)
                {
                    lblPackAirLineCheck_Before_AlignStart.BackColor = Color.GreenYellow;
                }
            }
            else
            {
                if (lblPackAirLineCheck_Before_AlignStart.BackColor != Color.Gray)
                {
                    lblPackAirLineCheck_Before_AlignStart.BackColor = Color.Gray;
                }
            }

            //  프로브 카드 클램프 타입
            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
            {
                if (lbl_MachineType.Text != "Type - A")
                {
                    lbl_MachineType.Text = "Type - A";
                }
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
            {
                if (lbl_MachineType.Text != "Type - B")
                {
                    lbl_MachineType.Text = "Type - B";
                }
            }


            if (Equipment.User_Mode != null)
            {
                lbl_UserMode.Text = Equipment.User_Mode;
            }
            else
            {
                lbl_UserMode.Text = "로그인 필요";
            }

            if (Equipment.User_Name != null)
            {
                lbl_UserName.Text = Equipment.User_Name;
            }
            else
            {
                lbl_UserName.Text = "- - - - -";
            }
        }
    }
}
