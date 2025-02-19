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
using QMC.Common.UI;

namespace SLD200_MSL
{
    public partial class FormNew_Logout : Form
    {
        //private Monitoring_CWA150SA m_Monitoring_CWA150SA;
        private FormLogIn m_formLogIn;
        
        public FormNew_Logout()
        {
            InitializeComponent();

            m_formLogIn = new FormLogIn();
            m_formLogIn.bLogin = false;
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            //  프로그램 종료

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "프로그램을 종료하시겠습니까?"))
                return;

            //  Lamp 다 끄기
            CommonModule.Instance.TowerLamp.AllLamp_Off();

            //m_Monitoring_CWA150SA.ThreadStop();
            //m_Monitoring_CWA150SA.Device_Close();                       //  2024. 07. 11.  SCH : Close 함수가 호출되지 않아서, 프로그램 종료할 때 카메라가 닫히지 않는 문제가 있었음.

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
