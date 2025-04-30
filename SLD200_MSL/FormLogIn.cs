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
using static QMC.Common.Equipment;

namespace SLD200_MSL
{

    public delegate void OkClickEventHandler();
    public partial class FormLogIn : Form
    {
        #region Field
        private FormBaseConfiguration m_Configuration = new FormBaseConfiguration();
        public event OkClickEventHandler buttonClick;
        public bool bLoginReady { get; set; }
        public bool bLogin { get; set; }                    //  로그인 상태인지? (false : 하부 메뉴 전체 비활성화)
        public bool bLogin_Admin { get; set; }              //  관리자로 로그인 되었는지? (false : 작업자)
        private Dictionary<string, string> m_LoginInfo_maint;           //  기존 메뉴 접속용
        private Dictionary<string, string> m_LoginInfo;
        private Dictionary<string, string> m_LoginInfo_msl;
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
            bLogin = false;
            bLogin_Admin = false;
            m_LoginInfo_maint = new Dictionary<string, string>();
            m_LoginInfo_maint.Add("QMC", "QMC123");
            m_LoginInfo = new Dictionary<string, string>();
            m_LoginInfo.Add("QMC", "123");
            m_LoginInfo_msl = new Dictionary<string, string>();
            m_LoginInfo_msl.Add("MSL", "RHKSFLWK1!");

            #region ControlConstuctor
            this.Text = "Log In";
            this.Size = new Size(m_Configuration.ListBoxSize.Width, m_Configuration.ListBoxSize.Height + 50);
            //this.ForeColor = Color.White;
            //this.BackColor = Color.FromArgb(65, 65, 65);
            //this.BackColor = Color.FromArgb(220, 220, 220);
            //this.BackColor = Color.FromArgb(180, 180, 180);
            this.ControlBox = false;

            this.baseTextBoxMain.Font = new Font(baseTextBoxMain.Font.FontFamily, 25);
            this.baseTextBoxMain.Size = new Size(this.Width - 16, m_Configuration.ButtonSize.Height);
            this.baseTextBoxMain.Location = new Point(0, this.baseTextBoxMain.Height - 10);
            //this.baseTextBoxMain.BackColor = Color.FromArgb(180, 180, 180);

            this.baseTextBoxExplain.Size = new Size(this.Width - 16, m_Configuration.PanelbuttonSize.Height);
            this.baseTextBoxExplain.Location = new Point(0, this.baseTextBoxMain.Location.Y + this.baseTextBoxMain.Height + 10);
            //this.baseTextBoxExplain.BackColor = Color.FromArgb(180, 180, 180);

            this.baseLabelEnterID.Location = new Point(0, this.baseTextBoxExplain.Location.Y + this.baseTextBoxExplain.Height + 30);
            this.baseLabelEnterID.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height + 6);
            this.baseLabelEnterID.TextAlign = ContentAlignment.MiddleRight;

            this.baseLabelEnterPW.Location = new Point(0, this.baseLabelEnterID.Location.Y + this.baseLabelEnterID.Height);
            this.baseLabelEnterPW.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height + 6);
            this.baseLabelEnterPW.TextAlign = ContentAlignment.MiddleRight;

            this.baseTextBoxEnterID.Size = new Size(m_Configuration.ButtonSize.Width * 2 - 130, m_Configuration.ButtonSize.Height * 2);
            this.baseTextBoxEnterID.Location = new Point(this.baseLabelEnterID.Location.X + this.baseLabelEnterID.Width, this.baseLabelEnterID.Location.Y + 6);
            this.baseTextBoxEnterID.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

            this.baseTextBoxEnterPW.Size = new Size(m_Configuration.ButtonSize.Width * 2 - 130, m_Configuration.ButtonSize.Height * 2);
            this.baseTextBoxEnterPW.Location = new Point(this.baseTextBoxEnterID.Location.X, this.baseLabelEnterPW.Location.Y + 8);
            this.baseTextBoxEnterPW.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

            this.baseButtonOk.Size = new Size(m_Configuration.ButtonSize.Width - 15, 50);
            //this.baseButtonOk.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height);
            this.baseButtonOk.Location = new Point(50, this.baseTextBoxEnterPW.Location.Y + m_Configuration.ButtonSize.Height * 3);
            this.baseButtonOk.Text = "OK";

            this.baseButtonCancel.Size = new Size(m_Configuration.ButtonSize.Width - 15, 50);
            //this.baseButtonCancel.Size = new Size(m_Configuration.ButtonSize.Width, m_Configuration.ButtonSize.Height);
            this.baseButtonCancel.Location = new Point(baseButtonOk.Location.X + baseButtonOk.Width + 30, this.baseTextBoxEnterPW.Location.Y + m_Configuration.ButtonSize.Height * 3);
            this.baseButtonCancel.Text = "Cancel";
            #endregion


            //Equipment.User_AdminMode = false;
            Equipment.User_LoginMode = (int)UserMode.USER_LOGOUT;


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

            //Equipment.User_AdminMode = false;

            m_ID = baseTextBoxEnterID.Text;
            m_ID = m_ID.ToUpper();                      //  대문자로 만들자. (대소문자 구별하기 불편함)

            m_PW = baseTextBoxEnterPW.Text;
            m_PW = m_PW.ToUpper();

            if (m_ID != null && m_PW != null && ((m_LoginInfo.ContainsKey(m_ID) == true) || (m_LoginInfo_maint.ContainsKey(m_ID) == true) || (m_LoginInfo_msl.ContainsKey(m_ID) == true)))
            {
                try
                {
                    if (m_LoginInfo[m_ID].ToUpper() == m_PW)
                    {
                        //  관리자 모드
                        Equipment.User_Mode = "관리자";
                        Equipment.User_Name = m_ID;
                        //Equipment.User_AdminMode = true;
                        //Equipment.User_QMC_Engineer = false;
                        Equipment.User_LoginMode = (int)UserMode.USER_ADMIN;

                        DialogResult = DialogResult.OK;
                        bLogin = true;
                        bLogin_Admin = true;
                        bLoginReady = true;

                        if (buttonClick != null)
                        {
                            buttonClick();
                        }
                    }
                    else if (m_LoginInfo_maint[m_ID].ToUpper() == m_PW)
                    {
                        //  관리자 모드
                        Equipment.User_Mode = "QMC 엔지니어";
                        Equipment.User_Name = m_ID;
                        //Equipment.User_AdminMode = true;
                        //Equipment.User_QMC_Engineer = true;
                        Equipment.User_LoginMode = (int)UserMode.USER_ENGINEER;

                        DialogResult = DialogResult.OK;
                        bLogin = true;
                        bLogin_Admin = true;
                        bLoginReady = true;

                        if (buttonClick != null)
                        {
                            buttonClick();
                        }
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        if (m_LoginInfo_msl[m_ID].ToUpper() == m_PW)
                        {
                            //  관리자 모드
                            Equipment.User_Mode = "관리자";
                            Equipment.User_Name = m_ID;
                            //Equipment.User_AdminMode = true;
                            //Equipment.User_QMC_Engineer = false;
                            Equipment.User_LoginMode = (int)UserMode.USER_ADMIN;

                            DialogResult = DialogResult.OK;
                            bLogin = true;
                            bLogin_Admin = true;
                            bLoginReady = true;
                            if (buttonClick != null)
                            {
                                buttonClick();
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        Log.Write(ex1);
                        baseTextBoxExplain.Text = "PW 를 확인해 주세요.";
                    }
                }

                //if (m_LoginInfo[m_ID].ToUpper() == m_PW)
                //{
                //    //  관리자 모드
                //    Equipment.User_Mode = "관리자";
                //    Equipment.User_Name = m_ID ;
                //    Equipment.User_SuperAdminMode = true;

                //    DialogResult = DialogResult.OK;
                //    bLogin = true;
                //    bLogin_Admin = true;
                //    bLoginReady = true;
                    
                //    if (buttonClick != null)
                //    {
                //        buttonClick();
                //    }
                //}
                //else if (m_LoginInfo_onsemi[m_ID].ToUpper() == m_PW)
                //{
                //    //  관리자 모드
                //    Equipment.User_Mode = "관리자";
                //    Equipment.User_Name = m_ID;
                //    Equipment.User_SuperAdminMode = true;

                //    DialogResult = DialogResult.OK;
                //    bLogin = true;
                //    bLogin_Admin = true;
                //    bLoginReady = true;
                //    if (buttonClick != null)
                //    {
                //        buttonClick();
                //    }
                //}
                //else
                //{
                //    baseTextBoxExplain.Text = "PW 를 확인해 주세요.";
                //}
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

                                bLogin = true;
                                bLoginReady = true;

                                //  작업자인지 관리자인지 확인
                                if (strUserData_Authority[i] == "관리자")
                                {
                                    bLogin_Admin = true;
                                    //Equipment.User_AdminMode = true;
                                    Equipment.User_LoginMode = (int)UserMode.USER_ADMIN;
                                }
                                else
                                {
                                    bLogin_Admin = false;
                                    Equipment.User_LoginMode = (int)UserMode.USER_OPERATOR;
                                }

                                //Equipment.User_QMC_Engineer = false;

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
