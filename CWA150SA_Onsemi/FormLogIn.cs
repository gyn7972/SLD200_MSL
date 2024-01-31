using QMC.Common;
using QMC.Core;
using System;
using System.IO;
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

    public delegate void OkClickEventHandler();
    public partial class FormLogIn : Form
    {
        #region Field
        private FormBaseConfiguration m_Configuration = new FormBaseConfiguration();
        public event OkClickEventHandler buttonClick;
        public bool bLoginReady { get; set; }
        public bool bLogin { get; set; }
        private Dictionary<string, string> m_LoginInfo;
        private string m_ID;
        private string m_PW;

        public int m_nOperator_Max = 100;                   //  등록 최대 인원 수
        public int m_nOperator_Count;

        public string[] strUserData_Authority;
        public string[] strUserData_Name;
        public string[] strUserData_ID;
        public string[] strUserData_Password;

        public FormLogIn()
        {
            InitializeComponent();
            this.Load += LoadLogInBox;
            bLoginReady = false;
            bLogin = true;
            m_LoginInfo = new Dictionary<string, string>();
            m_LoginInfo.Add("QMC", "123");

            #region ControlConstuctor
            this.Text = "Log In";
            this.Size = new Size(m_Configuration.ListBoxSize.Width + 250, m_Configuration.ListBoxSize.Height + 20);
            this.ForeColor = Color.White;
            //this.BackColor = Color.FromArgb(38, 38, 38);
            this.BackColor = Color.FromArgb(65, 65, 65);
            this.ControlBox = false;

            this.baseTextBoxMain.Font = new Font(baseTextBoxMain.Font.FontFamily, 25);
            this.baseTextBoxMain.Size = new Size(this.Width - 16, m_Configuration.ButtonSize.Height);
            this.baseTextBoxMain.Location = new Point(0, this.baseTextBoxMain.Height);

            this.baseTextBoxExplain.Size = new Size(this.Width - 16, m_Configuration.PanelbuttonSize.Height);
            this.baseTextBoxExplain.Location = new Point(0, this.baseTextBoxMain.Location.Y + this.baseTextBoxMain.Height);


            this.baseLabelEnterID.Location = new Point(0, this.baseTextBoxExplain.Location.Y + this.baseTextBoxExplain.Height + 20);
            this.baseLabelEnterID.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height);
            this.baseLabelEnterID.TextAlign = ContentAlignment.MiddleRight;


            this.baseLabelEnterPW.Location = new Point(0, this.baseLabelEnterID.Location.Y + this.baseLabelEnterID.Height);
            this.baseLabelEnterPW.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height);
            this.baseLabelEnterPW.TextAlign = ContentAlignment.MiddleRight;

            this.baseTextBoxEnterID.Size = new Size(m_Configuration.ButtonSize.Width * 2 - 30, m_Configuration.ButtonSize.Height * 2);
            this.baseTextBoxEnterID.Location = new Point(this.baseLabelEnterID.Location.X + this.baseLabelEnterID.Width, this.baseLabelEnterID.Location.Y + 4);
            this.baseTextBoxEnterID.Font = new Font(this.baseButtonOk.Font.FontFamily, 15);

            this.baseTextBoxEnterPW.Size = new Size(m_Configuration.ButtonSize.Width * 2 - 30, m_Configuration.ButtonSize.Height * 2);
            this.baseTextBoxEnterPW.Location = new Point(this.baseTextBoxEnterID.Location.X, this.baseLabelEnterPW.Location.Y + 4);
            this.baseTextBoxEnterPW.Font = new Font(this.baseButtonCancel.Font.FontFamily, 15);

            this.baseButtonOk.Size = new Size(m_Configuration.ButtonSize.Width, 50);
            //this.baseButtonOk.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height);
            this.baseButtonOk.Location = new Point(100, this.baseTextBoxEnterPW.Location.Y + m_Configuration.ButtonSize.Height * 2);
            this.baseButtonOk.Text = "OK";


            this.baseButtonCancel.Size = new Size(m_Configuration.ButtonSize.Width, 50);
            //this.baseButtonCancel.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height);
            this.baseButtonCancel.Location = new Point(baseButtonOk.Location.X + baseButtonOk.Width + 20
                , this.baseTextBoxEnterPW.Location.Y + m_Configuration.ButtonSize.Height * 2);
            this.baseButtonCancel.Text = "Cancel";
            #endregion


            //  작업자 100명만... 이것도 많다.
            m_nOperator_Count = 0;
            strUserData_Authority = new string[m_nOperator_Max];
            strUserData_Name = new string[m_nOperator_Max];
            strUserData_ID = new string[m_nOperator_Max];
            strUserData_Password = new string[m_nOperator_Max];

            for (int i = 0; i < m_nOperator_Max; i++)
            {
                strUserData_Authority[i] = "";
                strUserData_Name[i] = "";
                strUserData_ID[i] = "";
                strUserData_Password[i] = "";
            }

            UserData_Load();
        }
        #endregion

        #region EventHandler
        private void baseButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            bLoginReady = false;
            if (buttonClick != null)
            {
                buttonClick();
            }
        }

        private void baseButtonOk_Click(object sender, EventArgs e)
        {
            bool m_bID_Exist = false;
            bool m_bPW_Exist = false;


            m_ID = baseTextBoxEnterID.Text;
            m_ID = m_ID.ToUpper();                      //  대문자로 만들자. (대소문자 구별하기 불편함)

            m_PW = baseTextBoxEnterPW.Text;
            m_PW = m_PW.ToUpper();

            if (m_ID != null && m_PW != null && m_LoginInfo.ContainsKey(m_ID) == true)
            {
                if (m_LoginInfo[m_ID].ToUpper() == m_PW)
                {
                    //  관리자 모드
                    Equipment.User_Mode = "관리자";
                    Equipment.User_Name = m_ID ;

                    DialogResult = DialogResult.OK;
                    bLogin = true;
                    bLoginReady = true;
                    if (buttonClick != null)
                    {
                        buttonClick();
                    }
                }
                else
                {
                    baseTextBoxExplain.Text = "PW 를 확인해 주세요.";
                }
            }
            else
            {
                if (m_ID != null && m_PW != null)
                {
                    for (int i = 0; i < m_nOperator_Count; i++)
                    {
                        if (strUserData_ID[i].ToUpper() == m_ID)
                        {
                            if (strUserData_Password[i].ToUpper() == m_PW)
                            {
                                Equipment.User_Mode = strUserData_Authority[i];
                                Equipment.User_Name = strUserData_Name[i];

                                //  작업자인지 관리자인지 확인
                                if (strUserData_Authority[i] == "관리자")
                                {
                                    bLogin = true;
                                    bLoginReady = true;
                                }

                                DialogResult = DialogResult.OK;
                                if (buttonClick != null)
                                {
                                    buttonClick();
                                }
                            }
                            else
                            {
                                baseTextBoxExplain.Text = "PW 를 확인해 주세요.";
                            }
                        }
                        else
                        {
                            baseTextBoxExplain.Text = "ID 를 확인해 주세요.";
                        }
                    }
                }
                else
                {
                    baseTextBoxExplain.Text = "ID 와 PW 를 확인해 주세요.";
                }
            }
        }
        #endregion
         
        #region Method
        private void LoadInfo()
        {
            this.baseTextBoxEnterID.Text = string.Empty;
            this.baseTextBoxEnterPW.Text = string.Empty;

            this.ActiveControl = baseTextBoxEnterID;                    //  폼 열릴 때 ID 입력하는 텍스트 박스로 포커스 이동

            UserData_Load();
        }
        private void LoadLogInBox(object sender, EventArgs e)
        {
            LoadInfo();
        }
        private void FormLogIn_FormClosed(object sender, FormClosedEventArgs e)
        {
            bLoginReady = false;
        }


        public void UserData_Load()
        {
            bool m_bRet = false;
            string strKey = "";
            string strFIle = "";
            int m_nItemCount = 0;

            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Operator (Do not delete or modify).ini";

            //  초기화
            for (int i = 0; i < m_nOperator_Max; i++)
            {
                strUserData_Authority[i] = "";
                strUserData_Name[i] = "";
                strUserData_ID[i] = "";
                strUserData_Password[i] = "";
            }

            if (File.Exists(strFIle))
            {
                //  등록 인원 수
                NativeMethods.GetPrivateProfileString("Operator", "Total_Count", "0", temp, 255, strFIle);
                m_nOperator_Count = Convert.ToInt16(temp.ToString());

                for (int i = 0; i < m_nOperator_Count; i++)
                {
                    //  등록 인원 정보

                    //  권한
                    strKey = String.Format("Index_{0}_Authority", i);
                    NativeMethods.GetPrivateProfileString("Operator_Info", strKey, "", temp, 255, strFIle);
                    strUserData_Authority[i] = temp.ToString();

                    //  이름
                    strKey = String.Format("Index_{0}_Name", i);
                    NativeMethods.GetPrivateProfileString("Operator_Info", strKey, "", temp, 255, strFIle);
                    strUserData_Name[i] = temp.ToString();

                    //  ID
                    strKey = String.Format("Index_{0}_ID", i);
                    NativeMethods.GetPrivateProfileString("Operator_Info", strKey, "", temp, 255, strFIle);
                    strUserData_ID[i] = temp.ToString();

                    //  Password
                    strKey = String.Format("Index_{0}_PW", i);
                    NativeMethods.GetPrivateProfileString("Operator_Info", strKey, "", temp, 255, strFIle);
                    strUserData_Password[i] = temp.ToString();
                }
            }
        }

        #endregion
    }
}
