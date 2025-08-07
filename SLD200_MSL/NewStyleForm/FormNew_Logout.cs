using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.UI;
using QMC.Common.VisionPart;

namespace SLD200_MSL
{
    public partial class FormNew_Logout : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        //private Monitoring_CWA150SA m_Monitoring_CWA150SA;
        private FormLogIn m_formLogIn;
        
        public FormNew_Logout()
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (QMC.Common.Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
                if (module.Name == "Loader")
                {
                    loader = module as Loader;
                }
                if (module.Name == "Unloader")
                {
                    unloader = module as Unloader;
                }
                if (module.Name == "Vision")
                {
                    vision = module as Vision;
                }
                if (module.Name == "BDS")
                {
                    bds = module as Bds;
                }
            }

            m_formLogIn = new FormLogIn();
            m_formLogIn.bLogin = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!this.Created)
                return;

            if (this.Visible && !m_bFormVisible)
            {
                m_bFormVisible = true;
                // OnShowRecipeForm();
            }
            else if (!this.Visible && m_bFormVisible)
            {
                m_bFormVisible = false;
                //OnHideRecipeForm();
            }
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            //  프로그램 종료

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "프로그램을 종료하시겠습니까?"))
                return;

            //m_Monitoring_CWA150SA.ThreadStop();
            var alarms = AlarmManager.Instance.Alarms;
            if (alarms != null && alarms.Count > 0)
            {
                AlarmManager.Instance.ClearAllAlarms();
                CommonModule.Instance.TowerLamp_BuzzerStop = true;
            }

            workStage.m_MainStatus_Start = false;
            workStage.m_Comm_Start = false;

            workStage.Close();
            //workStage.Device_Close();                           //  2024. 07. 11.  SCH : Close 함수가 호출되지 않아서, 프로그램 종료할 때 카메라가 닫히지 않는 문제가 있었음.

            loader.Close();
            unloader.Close();
            vision.Close();
            bds.Close();

            //  Lamp 다 끄기
            CommonModule.Instance.TowerLamp.AllLamp_Off();
            CommonModule.Instance.TowerLamp.Buzzer_Off();

            //this.Close();
            Application.Exit();
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            m_formLogIn.StartPosition = FormStartPosition.CenterScreen;
            m_formLogIn.bLoginReady = true;
            //UpdateButtonOkImage();

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
                //this.buttonLogin.Visible = false;
                //this.buttonLogin.Enabled = false;
                //this.buttonLogOut.Visible = true;
                //this.buttonLogOut.Enabled = true;

                if (Equipment.User_Mode == "관리자")
                {
                    m_formLogIn.bLogin = true;
                }

                //LogInInfo();

                Equipment.LogIn_Status = true;
                Equipment.AutoLogOut_Executed = false;
            }
            else if (m_formLogIn.DialogResult == DialogResult.Cancel)
            {
                //UpdateButtonOkImage();
            }
        }

        private void button_Logout_Click(object sender, EventArgs e)
        {
            Equipment.User_LoginMode = (int)Equipment.UserMode.USER_LOGOUT;
        }
    }
}
