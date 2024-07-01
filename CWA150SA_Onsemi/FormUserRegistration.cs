using QMC.Common;
using QMC.Core;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using static QMC.Common.Modules.WaferProbeAlign;

namespace CWA150SA_Onsemi300
{
    public partial class FormUserRegistration : Form
    {
        public int m_nOperator_Max = 100;                   //  등록 최대 인원 수
        public int m_nOperator_Count;

        public string[] strUserData_Authority;
        public string[] strUserData_Name;
        public string[] strUserData_ID;
        public string[] strUserData_Password;

        public FormUserRegistration()
        {
            InitializeComponent();

            Init_Load();
        }

        private void Init_Load()
        {
            //  작업자 100명만... 이것도 많다.
            m_nOperator_Count = 0;
            strUserData_Authority = new string[m_nOperator_Max];
            strUserData_Name = new string[m_nOperator_Max];
            strUserData_ID = new string[m_nOperator_Max];
            strUserData_Password = new string[m_nOperator_Max];

            for( int i = 0; i < m_nOperator_Max; i++ )
            {
                strUserData_Authority[i] = "";
                strUserData_Name[i] = "";
                strUserData_ID[i] = "";
                strUserData_Password[i] = "";
            }

            // 리스트뷰 아이템을 업데이트 하기 시작.
            // 업데이트가 끝날 때까지 UI 갱신 중지.
            listView_Operator.BeginUpdate();

            // 뷰모드 지정
            listView_Operator.View = View.Details;
            listView_Operator.GridLines = true;         //  구분선 표시
            listView_Operator.FullRowSelect = true;     //  한줄씩 선택 설정
            
            // 컬럼명과 컬럼사이즈 지정
            //SetHeight(listView_Operator, 24);

            listView_Operator.Columns.Add("        순번", 100, HorizontalAlignment.Center);
            listView_Operator.Columns.Add("권한", 100, HorizontalAlignment.Center);
            listView_Operator.Columns.Add("이름", 100, HorizontalAlignment.Center);
            listView_Operator.Columns.Add("아이디", 200, HorizontalAlignment.Center);
            listView_Operator.Columns.Add("비밀번호", 200, HorizontalAlignment.Center);

            //foreach (var fi in files)
            //{
            //    // 각 파일별로 ListViewItem객체를 하나씩 만듦
            //    // 파일명, 사이즈, 날짜 정보를 추가
            //    ListViewItem lvi = new ListViewItem(fi.Name);
            //    lvi.SubItems.Add(fi.Length.ToString());
            //    lvi.SubItems.Add(fi.LastWriteTime.ToString());
            //    lvi.ImageIndex = 0;

            //    // ListViewItem객체를 Items 속성에 추가
            //    listView_Operator.Items.Add(lvi);
            //}

            UserData_Load();

            // 리스트뷰를 Refresh하여 보여줌
            listView_Operator.EndUpdate();
        }

        public void UserData_Load()
        {
            bool m_bRet = false;
            string strKey = "";
            string strFIle = "";
            string strPW = "";
            int m_nItemCount = 0;

            //string strUserData_Authority = "";
            //string strUserData_Name = "";
            //string strUserData_ID = "";
            //string strUserData_Password = "";

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
                listView_Operator.Items.Clear();

                //  등록 인원 수
                NativeMethods.GetPrivateProfileString("Operator", "Total_Count", "0", temp, 255, strFIle);
                m_nOperator_Count = Convert.ToInt16(temp.ToString());
                
                for ( int i = 0; i < m_nOperator_Count; i++ )
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


                    //  리스트 뷰에 추가
                    ListViewItem lvi = new ListViewItem((i + 1).ToString());                            //  순번
                    lvi.SubItems.Add(strUserData_Authority[i]);                                         //  권한
                    lvi.SubItems.Add(strUserData_Name[i]);                                              //  이름
                    lvi.SubItems.Add(strUserData_ID[i]);                                                //  ID
                    lvi.SubItems.Add(strUserData_Password[i]);                                          //  Password

                    //  ListViewItem 객체를 Items 속성에 추가
                    listView_Operator.Items.Add(lvi);
                }
            }
            else
            {
                listView_Operator.Items.Clear();
            }
        }

        public void UserData_Save()
        {
            string strFIle = "";
            string strKey = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Operator (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
            }


            //  리스트의 데이터 가져오기
            int m_nItemCount = listView_Operator.Items.Count;

            for (int i = 0; i < m_nOperator_Max; i++)
            {
                strUserData_Authority[i] = "";
                strUserData_Name[i] = "";
                strUserData_ID[i] = "";
                strUserData_Password[i] = "";
            }

            for (int i = 0; i < m_nItemCount; i++)
            {
                //  권한
                strUserData_Authority[i] = listView_Operator.Items[i].SubItems[1].Text;

                //  이름
                strUserData_Name[i] = listView_Operator.Items[i].SubItems[2].Text;

                //  ID
                strUserData_ID[i] = listView_Operator.Items[i].SubItems[3].Text;

                //  Password
                strUserData_Password[i] = listView_Operator.Items[i].SubItems[4].Text;
            }


            //  리스트 데이터 저장 파일 초기화
            for (int i = 0; i < m_nOperator_Max; i++)
            {
                //  권한
                strKey = String.Format("Index_{0}_Authority", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);

                //  이름
                strKey = String.Format("Index_{0}_Name", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);

                //  ID
                strKey = String.Format("Index_{0}_ID", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);

                //  Password
                strKey = String.Format("Index_{0}_PW", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);
            }


            //  등록 인원 수
            NativeMethods.WritePrivateProfileString("Operator", "Total_Count", m_nItemCount.ToString(), strFIle);

            for (int i = 0; i < m_nItemCount; i++)
            {
                //  권한
                strKey = String.Format("Index_{0}_Authority", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, listView_Operator.Items[i].SubItems[1].Text, strFIle);

                //  이름
                strKey = String.Format("Index_{0}_Name", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, listView_Operator.Items[i].SubItems[2].Text, strFIle);

                //  ID
                strKey = String.Format("Index_{0}_ID", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, listView_Operator.Items[i].SubItems[3].Text, strFIle);

                //  Password
                strKey = String.Format("Index_{0}_PW", i);
                NativeMethods.WritePrivateProfileString("Operator_Info", strKey, listView_Operator.Items[i].SubItems[4].Text, strFIle);
            }
        }

        public void UserData_Del( int m_nIndex)
        {
            string strFIle = "";
            string strKey = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Operator (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
            }

            //  권한
            strKey = String.Format("Index_{0}_Authority", m_nIndex);
            NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);

            //  이름
            strKey = String.Format("Index_{0}_Name", m_nIndex);
            NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);

            //  ID
            strKey = String.Format("Index_{0}_ID", m_nIndex);
            NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);

            //  Password
            strKey = String.Format("Index_{0}_PW", m_nIndex);
            NativeMethods.WritePrivateProfileString("Operator_Info", strKey, null, strFIle);
        }

        public int UserData_ExistCheck(string strName, string strID)
        {
            int m_nRet = 0;

            int m_nItemCount = 0;

            m_nItemCount = listView_Operator.Items.Count;

            for ( int i = 0; i < m_nItemCount; i++ )
            {
                if (listView_Operator.Items[i].SubItems[2].Text == strName)
                {
                    return 1;
                }

                if (listView_Operator.Items[i].SubItems[3].Text == strID)
                {
                    return 2;
                }
            }

            return m_nRet;
        }

        private void SetHeight(ListView LV, int height)
        {
            // listView 높이 지정

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, height);
            LV.SmallImageList = imgList;
        }

        private void btn_User_Add_Click(object sender, EventArgs e)
        {
            //  추가

            string m_strAuthority = "";
            int m_nItemCount = 0;
            int m_nExistCheck = 0;

            m_nItemCount = listView_Operator.Items.Count;

            m_nExistCheck = UserData_ExistCheck(tb_Name.Text, tb_ID.Text);
            if ( m_nExistCheck == 1)
            {
                MessageBox.Show("이미 등록된 이름입니다.", "Information !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (m_nExistCheck == 2)
            {
                MessageBox.Show("이미 등록된 아이디 입니다.", "Information !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (radioButton_Operator.Checked == false)
            {
                if (DialogResult.No == MessageBox.Show("관리자 아이디로 등록하시겠습니까?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }

                m_strAuthority = "관리자";
            }
            else
            {
                m_strAuthority = "작업자";
            }

            ListViewItem lvi = new ListViewItem((m_nItemCount + 1).ToString());         //  순번
            lvi.SubItems.Add(m_strAuthority);                                           //  권한
            lvi.SubItems.Add(tb_Name.Text);                                             //  이름
            lvi.SubItems.Add(tb_ID.Text);                                               //  ID
            lvi.SubItems.Add(tb_Password.Text);                                         //  Password

            //  ListViewItem 객체를 Items 속성에 추가
            listView_Operator.Items.Add(lvi);

            strUserData_Authority[m_nItemCount] = m_strAuthority;                       //  권한
            strUserData_Name[m_nItemCount] = tb_Name.Text;                              //  이름
            strUserData_ID[m_nItemCount] = tb_ID.Text;                                  //  ID
            strUserData_Password[m_nItemCount] = tb_Password.Text;                      //  Password

            UserData_Save();
            UserData_Load();
        }

        private void btn_User_Del_Click(object sender, EventArgs e)
        {
            //  삭제

            ListView.SelectedListViewItemCollection itemCol = listView_Operator.SelectedItems;

            foreach ( ListViewItem item in itemCol )
            {
                listView_Operator.Items.Remove(item);
            }

            UserData_Save();
            UserData_Load();
        }

        private void checkBox_Password_Visible_CheckedChanged(object sender, EventArgs e)
        {
            //  비밀번호 보이기

            if (checkBox_Password_Visible.Checked)
            {
                tb_Password.PasswordChar = default(char);
            }
            else
            {
                tb_Password.PasswordChar = '*';
            }
        }

        private void listView_Operator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Operator.SelectedItems.Count != 0)
            { 
                int SelectRow = listView_Operator.SelectedItems[0].Index; 
                string m_strAuthority = listView_Operator.Items[SelectRow].SubItems[1].Text;
                string m_strName = listView_Operator.Items[SelectRow].SubItems[2].Text; 
                string m_strID = listView_Operator.Items[SelectRow].SubItems[3].Text;
                string m_strPassword = listView_Operator.Items[SelectRow].SubItems[4].Text;

                if (m_strAuthority == "작업자")
                {
                    radioButton_Operator.Checked = true;
                }
                else
                {
                    radioButton_Administrator.Checked = true;
                }
                tb_Name.Text = m_strName;
                tb_ID.Text = m_strID;
                tb_Password.Text = m_strPassword;
            }
        }
    }
}
