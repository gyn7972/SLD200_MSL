    using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Core;
using QMC.Common.Modules;
//using QMC.Common.UI;

namespace CWA150SA_Onsemi300
{
    #region Define

    //public enum FormSize
    //{
    //    MainWidth = 1500,
    //    MainHeight = 1100,
    //    TopWidth = 1500,
    //    TopHeight = 100,
    //    ContentWidth = 1500,
    //    ContentHeight = (int)FormSize.MainHeight-Configuration.TopSize.Height- (int)FormSize.BottomHeight,
    //    BottomWidth = 1500,
    //    BottomHeight = 100
    //}
    //public enum ButtonSize
    //{
    //    Width = 100,
    //    Height = 30
    //}
    //public enum PanelSize
    //{
    //    flowLayoutPanelButtonHeight = Configuration.ButtonSize.Height+3,
    //   // PanelHeight = 840
    //}    

    #endregion

    public partial class FormMain : Form
    {
        #region Field

        private FormSelectSetUp m_FormSelectSetUp;
        private FormMaintMain m_FormMaint;
        private FormConfiguration m_FormConfiguration;
        private FormOperation m_FormOperation;
        private FormRecipeMain m_FormRecipe;
        private FormAlarm m_FormAlarm;
        private FormOperationMonitering m_FormOperationMornitor;
        private FormSelectIO m_FormSelectIO;

        private Form1 m_Form1;           //  2022. 04. 04.  SCH : Form Test
        public  FormBottom m_formBottom;

        private Monitoring_CWA150SA m_Monitoring_CWA150SA;
        //private SingleMode_LPM100 m_SingleMode_LPM100;

        #endregion


        #region Constructer

        public FormMain()
        {
            InitializeComponent();
            Configuration = new FormBaseConfiguration();

            this.Size = new Size((Point)Configuration.MainSize);
            this.ClientSize = new Size((Point)Configuration.MainSize);

            panelBottom.Location = new Point(0, Configuration.TopSize.Height + Configuration.ContentSize.Height);
            panelBottom.Size = new Size((Point)Configuration.BottomSize);

            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size((Point)Configuration.TopSize);

            panelContent.Location = new Point(0, Configuration.TopSize.Height);
            panelContent.Size = new Size(Configuration.MainSize.Width, Configuration.ContentSize.Height);
            panelContent.BackColor = Color.FromArgb(38, 38, 38);

            this.BackColor = Color.DarkGray;

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            //m_Form1 = new Form1();

            this.FormSelectSetUp = new FormSelectSetUp();
            this.FormMaint = new FormMaintMain();
            this.FormRecipe = new FormRecipeMain();
            this.FormOperation = new FormOperation();
            this.FormConfiguration = new FormConfiguration();
            this.FormAlarm = new FormAlarm();
            this.Monitoring_CWA150SA = new Monitoring_CWA150SA();
            //this.SingleMode_LPM100 = new SingleMode_LPM100();
            this.m_FormSelectIO = new FormSelectIO();

            FormTopShow();
            FormBottomShow();

            FormMonitoringShow();
            
            //AlarmManager.Instance.PostAlarm += AlarmManager_PostAlarm;

            //m_Form1.ShowDialog();
        }
        
        private void AlarmManager_PostAlarm(Alarm alarm)
        {
            BeginInvoke(new Action(() =>
            {
                this.FormAlarm.Alarms = AlarmManager.Instance.Alarms;
                this.ShowAlarmForm(FormAlarm);
            }));
        }

        private void FormMain_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcessesByName("qmc.cepheus");
            if (processList.Length > 0)
            {
                processList[0].Kill();
            }

            Application.ExitThread();
            Environment.Exit(0);
        }

        private void FormMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcessesByName("qmc.cepheus");
            if (processList.Length > 0)
            {
                processList[0].Kill();
            }

            Application.ExitThread();
            Environment.Exit(0);
        }

        #endregion

        #region Property
        public FormBaseConfiguration Configuration { get; set; }

        public FormAlarm FormAlarm
        {
            get { return this.m_FormAlarm; }
            set { this.m_FormAlarm = value; }
        }

        public FormConfiguration FormConfiguration
        {
            get { return this.m_FormConfiguration; }
            set { this.m_FormConfiguration = value; }
        }
        public FormOperation FormOperation 
        {
            get { return this.m_FormOperation; }
            set { this.m_FormOperation = value; }
        }
        public FormRecipeMain FormRecipe
        {
            get { return this.m_FormRecipe; }
            set { this.m_FormRecipe = value; }
        }
        public FormMaintMain FormMaint
        {
            get { return m_FormMaint; }
            set { this.m_FormMaint = value; }
        }
        public FormSelectSetUp FormSelectSetUp
        {
            get { return m_FormSelectSetUp; }
            set { m_FormSelectSetUp = value; }
        }
        public FormSelectIO FormSelectIO
        {
            get { return m_FormSelectIO; }
            set { m_FormSelectIO = value; }
        }

        public Form1 Form1Test           //  2022. 04. 04.  SCH : Form Test
        {
            get { return m_Form1; }
            set { m_Form1 = value; }
        }

        public FormTop FormTop { get; set; }

        public Monitoring_CWA150SA Monitoring_CWA150SA
        {
            get { return this.m_Monitoring_CWA150SA; }
            set { this.m_Monitoring_CWA150SA = value; }
        }

        /*public SingleMode_LPM100 SingleMode_LPM100
        {
            get { return this.m_SingleMode_LPM100; }
            set { this.m_SingleMode_LPM100 = value; }
        }*/

        #endregion

        #region Method
        public void FormTopShow()
        {
            FormTop formTop = new FormTop();
            FormTop = formTop;
            FormTop.FormBorderStyle = FormBorderStyle.None;
            FormTop.TopLevel = false;
            FormTop.TopButtonClick += TopButton_Click;
            FormTop.Dock = DockStyle.Fill;
            
            panelTop.Controls.Add(FormTop);
            FormTop.BringToFront();
            FormTop.Show();
        }
        public void FormBottomShow()
        {
            FormBottom FormBottom = new FormBottom();
            FormBottom.FormBorderStyle = FormBorderStyle.None;
            FormBottom.TopLevel = false;
            FormBottom.m_FormBottomClickEvent += BottomButton_Click;
            FormBottom.LogOutClick += LogOutClick;
            FormBottom.ExitClick += ExitButton_Click;
            FormBottom.Dock = DockStyle.Fill;


            panelBottom.Controls.Add(FormBottom);
            FormBottom.BringToFront();
            FormBottom.Show();
        }

        public void FormMonitoringShow()
        {
            FormBottom FormBottom = new FormBottom();

            this.ShowForm(FormOperation);
            m_FormOperation.ShowMainForm();

            //FormBottom.OperationButtonStatus(true);
        }

        public void ExitButton_Click()
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "프로그램을 종료하시겠습니까?"))
                return;

            //  Lamp 다 끄기
            CommonModule.Instance.TowerLamp.AllLamp_Off();

            m_Monitoring_CWA150SA.ThreadStop();

            this.Close();
        }
        public void TopButton_Click(TopButtons button)
        {
            return;

            Button button1 = FormTop.Controls[1].Controls[(int)TopButtons.Alarm] as Button;
            switch (button)
            {
                case TopButtons.Alarm:

                    Form form = null;
                    foreach (Form openForm in Application.OpenForms)
                    {
                        if (openForm.Name == FormAlarm.Name)
                        {
                            if (openForm.Visible == false)
                            {
                                form = openForm;
                                if (FormAlarm.Alarms != null && FormAlarm.Alarms.Count > 0)
                                {
                                    button1.Image = CWA150SA_Onsemi.Properties.Resources.AlarmOn;

                                }
                                this.ShowAlarmForm(FormAlarm);
                                return;
                            }
                            form = openForm;
                            form.Hide();
                            if(FormAlarm.Alarms !=null && FormAlarm.Alarms.Count > 0)
                            {
                                button1.Image = CWA150SA_Onsemi.Properties.Resources.AlarmOn;

                            }
                            return;

                        }

                    }
                    if (form == null)
                    {
                        this.ShowAlarmForm(FormAlarm);

                    }


                    break;
                case TopButtons.Buzzer:

                    break;
                //case TopButtons.Module:

                //    break;
            }
        }

        public void LogOutClick()
        {
            //TODO : OperationMonitoring Button Image 바꿔주기.
            this.ShowForm(m_FormOperation);
        }

        public void BottomButton_Click(ButtonBottomType button)
        {
            switch (button)
            {
                case ButtonBottomType.Operation:
                    this.ShowForm(FormOperation);
                    break;
                case ButtonBottomType.Configuration:
                    this.ShowForm(FormConfiguration);
                    break;
                case ButtonBottomType.Maint:
                    this.ShowForm(FormMaint);
                    break;
                case ButtonBottomType.Recipe:
                    this.ShowForm(FormRecipe);
                    break;
                case ButtonBottomType.SetUp:
                    this.ShowForm(FormSelectSetUp);
                    break;
                case ButtonBottomType.IO:
                    this.ShowForm(FormSelectIO);
                    break;
                default:
                    break;
            }
        }
        public void ShowForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        public void ShowButtonForm(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.TopLevel = false;
            form.TopMost = true;
            form.BringToFront();
            form.Show();
        }
        public void ShowAlarmForm(Form form)
        {
            form.TopLevel = false;
            panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        #endregion

        #region 화면 깜빡임 제거
        //화면
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion
    }
}
