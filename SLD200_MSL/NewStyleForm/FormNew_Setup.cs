using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Core;
using static QMC.Common.Equipment;

namespace SLD200_MSL
{
    public partial class FormNew_Setup : Form
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Bds Bds;

        //  IO
        private DioPointCollection m_DioPoints;
        private IOListControl Inputlist;
        private IOListControl Outputlist;
        private DioPointCollection InputPoint;
        private DioPointCollection OutputPoint;
        public Size IOGridSize { protected set; get; }

        private FormNew_KeyPad m_keyPad;
        public System.Windows.Forms.Timer timer_Status;

        public FormNew_Setup()
        {
            InitializeComponent();

            m_keyPad = new FormNew_KeyPad();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
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

                if (module.Name == "BDS")
                {
                    Bds = module as Bds;
                }
            }

            IOGridSize = new Size(730, 590);

            InputPoint = new DioPointCollection();
            OutputPoint = new DioPointCollection();
            Inputlist = new IOListControl(InputPoint, "Input List");
            Outputlist = new IOListControl(OutputPoint, "Output List");
            m_DioPoints = new DioPointCollection();
            SetDioPoint();              //  2022. 04. 12.  SCH : 전체 한번만 추가하기 위해서
            this.Inputlist.SetControlName("Input List");
            this.Outputlist.SetControlName("Output List");

            this.Inputlist.Size = new Size(IOGridSize.Width, IOGridSize.Height);
            this.Inputlist.Location = new Point(10, 10);
            this.Inputlist.Font = new Font("Tahoma", 10, FontStyle.Bold);
            this.tabPage_Setup_IO.Controls.Add(this.Inputlist);

            this.Outputlist.Size = new Size(IOGridSize.Width, IOGridSize.Height);
            this.Outputlist.Location = new Point(this.Inputlist.Location.X + this.Inputlist.Width, this.Inputlist.Location.Y);
            this.Outputlist.Font = new Font("Tahoma", 10, FontStyle.Bold);
            this.tabPage_Setup_IO.Controls.Add(this.Outputlist);

            this.VisibleChanged += FormNew_Setup_VisibleChanged;

            Axis_Parameter_Apply();
            Comm_Parameter_Apply();

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 1;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
            timer_Status.Enabled = true;
        }

        private void FormNew_Setup_VisibleChanged(object sender, EventArgs e)
        {
            Inputlist.ShowDataGridView();
            Outputlist.ShowDataGridView();
        }

        private void SetDioPoint()
        {
            m_DioPoints.Clear();
            //foreach (Module module in m_Modules)
            {
                //if (module != null)
                {
                    foreach (DioPoint point in Equipment.GetAllDioPointList())
                    {
                        DioPoint dioPoint = point;
                        m_DioPoints.Add(dioPoint);
                    }
                }
            }
            SetDioPointInfo(m_DioPoints);
            //this.baseLabel.Text = "All";
        }

        private void SetDioPointInfo(DioPointCollection IOList)
        {
            InputPoint.Clear();
            OutputPoint.Clear();
            if (IOList != null)
            {
                foreach (DioPoint one in IOList)
                {
                    if (one.IoType == IoType.Input)
                    {
                        InputPoint.Add(one);
                    }
                    else
                    {
                        OutputPoint.Add(one);
                    }
                }
                Inputlist.UpdateGridInfo(InputPoint);
                Outputlist.UpdateGridInfo(OutputPoint);
            }
            else
            {
                Inputlist.UpdateGridInfo(InputPoint);
                Outputlist.UpdateGridInfo(OutputPoint);
            }
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            timer_Status.Enabled = false;

            //DIO_Status();
            Motor_Position();

            //m_btimer_MainWork_Stop = false;
            //timer_MainWork.Enabled = false;

            //if (!m_btimer_MainWork_Stop)
            //{
            //    timer_MainWork.Enabled = true;
            //}

            timer_Status.Enabled = true;
        }

        private void Motor_Position()
        {
            //  Loader Stacker Position
            label_Setup_EncPosition_LD_Z0.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z0));
            label_Setup_EncPosition_LD_Z1.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z1));

            //  Loader Transfer Position
            label_Setup_EncPosition_LD_TRX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X));
            label_Setup_EncPosition_LD_TRZ.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z));

            //  Loader Mechanic Aligner Position
            label_Setup_EncPosition_LD_ALNX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_X));
            label_Setup_EncPosition_LD_ALNY.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_Y));


            //  Work Stage Position
            label_Setup_EncPosition_STAGE_X.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X));
            label_Setup_EncPosition_STAGE_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y));

            //  Scanner & Camera Position
            label_Setup_EncPosition_SCANNER_Z.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z));

            //  Mask Position
            label_Setup_EncPosition_MASK_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.MASK_Y));


            //  Unloader Stacker Position
            label_Setup_EncPosition_UL_Z0.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0));
            label_Setup_EncPosition_UL_Z1.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z1));

            //  Unloader Transfer Position
            label_Setup_EncPosition_UL_TRX.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X));
            label_Setup_EncPosition_UL_TRZ.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z));
        }

        public bool Axis_Parameter_Exist()
        {
            bool m_bRet = false;
            string strFIle = "";

            strFIle = ConfigManager.GetConfigPath() + "\\Axis Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle))
            {
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool Axis_Parameter_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Axis Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Axis Setting 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  축 파라미터 파일을 로드 하면 맨 첫번째 축 데이터를 표시하도록 한다.
            listBox_Setup_Motion_SelectAxis.SelectedIndex = 0;

            comboBox_Setup_Motion_Limit_Install.SelectedIndex = Equipment.stAxisParam[0].LimitSensor_Installed;
            comboBox_Setup_Motion_Limit_Active.SelectedIndex = Equipment.stAxisParam[0].LimitSensor_ActiveLevel;

            comboBox_Setup_Motion_Home_Sensing.SelectedIndex = Equipment.stAxisParam[0].Home_Sensing;
            comboBox_Setup_Motion_Home_Install.SelectedIndex = Equipment.stAxisParam[0].Home_Installed;
            comboBox_Setup_Motion_Home_Active.SelectedIndex = Equipment.stAxisParam[0].Home_ActiveLevel;
            comboBox_Setup_Motion_Home_Direction.SelectedIndex = Equipment.stAxisParam[0].Home_Direction;
            textBox_Setup_Motion_Home_Speed_1st.Text = Equipment.stAxisParam[0].Home_Speed_1st.ToString();
            textBox_Setup_Motion_Home_Speed_2nd.Text = Equipment.stAxisParam[0].Home_Speed_2nd.ToString();
            textBox_Setup_Motion_Home_Speed_3rd.Text = Equipment.stAxisParam[0].Home_Speed_3rd.ToString();
            textBox_Setup_Motion_Home_Speed_Last.Text = Equipment.stAxisParam[0].Home_Speed_Last.ToString();
            textBox_Setup_Motion_Home_Offset.Text = Equipment.stAxisParam[0].Home_Offset.ToString();

            textBox_Setup_Motion_Common_Unit.Text = Equipment.stAxisParam[0].Common_UnitPerPulse_Unit.ToString();
            textBox_Setup_Motion_Common_Pulse.Text = Equipment.stAxisParam[0].Common_UnitPerPulse_Pulse.ToString();
            textBox_Setup_Motion_Common_MinAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Min.ToString();
            textBox_Setup_Motion_Common_MaxAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Max.ToString();
            textBox_Setup_Motion_Common_FineAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Coarse.ToString();
            textBox_Setup_Motion_Common_MaxSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Max.ToString();
            textBox_Setup_Motion_Common_MinSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Min.ToString();
            textBox_Setup_Motion_Common_FineSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Coarse.ToString();
            textBox_Setup_Motion_Common_MinPos.Text = Equipment.stAxisParam[0].Common_Position_Min.ToString();
            textBox_Setup_Motion_Common_MaxPos.Text = Equipment.stAxisParam[0].Common_Position_Max.ToString();
            textBox_Setup_Motion_Common_SettleDelay.Text = Equipment.stAxisParam[0].Common_Settle_Delay.ToString();

            textBox_Setup_Motion_Jog_FineSpeed.Text = Equipment.stAxisParam[0].Jog_Speed_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseSpeed.Text = Equipment.stAxisParam[0].Jog_Speed_Coarse.ToString();
            textBox_Setup_Motion_Jog_MinStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Min.ToString();
            textBox_Setup_Motion_Jog_MaxStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Max.ToString();
            textBox_Setup_Motion_Jog_FineStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Coarse.ToString();

            return m_bRet;
        }

        public void Axis_Parameter_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Axis Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Axis Setting 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Setup_Motion_SelectAxis.SelectedIndex;

            if (m_nIndex >= 0)
            {
                Equipment.stAxisParam[m_nIndex].LimitSensor_Installed = comboBox_Setup_Motion_Limit_Install.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].LimitSensor_ActiveLevel = comboBox_Setup_Motion_Limit_Active.SelectedIndex;

                Equipment.stAxisParam[m_nIndex].Home_Sensing = comboBox_Setup_Motion_Home_Sensing.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Installed = comboBox_Setup_Motion_Home_Install.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_ActiveLevel = comboBox_Setup_Motion_Home_Active.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Direction = comboBox_Setup_Motion_Home_Direction.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Speed_1st = Convert.ToDouble(textBox_Setup_Motion_Home_Speed_1st.Text);
                Equipment.stAxisParam[m_nIndex].Home_Speed_2nd = Convert.ToDouble(textBox_Setup_Motion_Home_Speed_2nd.Text);
                Equipment.stAxisParam[m_nIndex].Home_Speed_3rd = Convert.ToDouble(textBox_Setup_Motion_Home_Speed_3rd.Text);
                Equipment.stAxisParam[m_nIndex].Home_Speed_Last = Convert.ToDouble(textBox_Setup_Motion_Home_Speed_Last.Text);
                Equipment.stAxisParam[m_nIndex].Home_Offset = Convert.ToDouble(textBox_Setup_Motion_Home_Offset.Text);

                Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Unit = Convert.ToDouble(textBox_Setup_Motion_Common_Unit.Text);
                Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Pulse = Convert.ToDouble(textBox_Setup_Motion_Common_Pulse.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Min = Convert.ToDouble(textBox_Setup_Motion_Common_MinAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Max = Convert.ToDouble(textBox_Setup_Motion_Common_MaxAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Fine = Convert.ToDouble(textBox_Setup_Motion_Common_FineAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Coarse = Convert.ToDouble(textBox_Setup_Motion_Common_CoarseAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Max = Convert.ToDouble(textBox_Setup_Motion_Common_MaxSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Min = Convert.ToDouble(textBox_Setup_Motion_Common_MinSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Fine = Convert.ToDouble(textBox_Setup_Motion_Common_FineSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Coarse = Convert.ToDouble(textBox_Setup_Motion_Common_CoarseSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Position_Min = Convert.ToDouble(textBox_Setup_Motion_Common_MinPos.Text);
                Equipment.stAxisParam[m_nIndex].Common_Position_Max = Convert.ToDouble(textBox_Setup_Motion_Common_MaxPos.Text);
                Equipment.stAxisParam[m_nIndex].Common_Settle_Delay = Convert.ToDouble(textBox_Setup_Motion_Common_SettleDelay.Text);

                Equipment.stAxisParam[m_nIndex].Jog_Speed_Fine = Convert.ToDouble(textBox_Setup_Motion_Jog_FineSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Jog_Speed_Coarse = Convert.ToDouble(textBox_Setup_Motion_Jog_CoarseSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Min = Convert.ToDouble(textBox_Setup_Motion_Jog_MinStepSize.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Max = Convert.ToDouble(textBox_Setup_Motion_Jog_MaxStepSize.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Fine = Convert.ToDouble(textBox_Setup_Motion_Jog_FineStepSize.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Coarse = Convert.ToDouble(textBox_Setup_Motion_Jog_CoarseStepSize.Text);
            }


            //  Axis Parameter 저장
            for (int i = 0; i < Equipment.Max_Axis; i++ )
            {
                strTemp = string.Format("Axis_{0}_Limit", i);
                //  Limit Sensor 설치 여부 (Not Installed, Installed)                
                NativeMethods.WritePrivateProfileString(strTemp, "Install", Equipment.stAxisParam[i].LimitSensor_Installed.ToString(), strFIle);
                //  Limit Sensor 동작 레벨 (Low, High)
                NativeMethods.WritePrivateProfileString(strTemp, "ActiveLevel", Equipment.stAxisParam[i].LimitSensor_ActiveLevel.ToString(), strFIle);

                strTemp = string.Format("Axis_{0}_Home", i);
                //  Home Sensor 형태 (Home, -Limit, +Limit)
                NativeMethods.WritePrivateProfileString(strTemp, "SensingType", Equipment.stAxisParam[i].Home_Sensing.ToString(), strFIle);
                //  Home Sensor 설치 여부 (Not Installed, Installed)
                NativeMethods.WritePrivateProfileString(strTemp, "Install", Equipment.stAxisParam[i].Home_Installed.ToString(), strFIle);
                //  Home Sensor 동작 레벨 (Low, High)
                NativeMethods.WritePrivateProfileString(strTemp, "ActiveLevel", Equipment.stAxisParam[i].Home_ActiveLevel.ToString(), strFIle);
                //  Home Sensor 동작 방향 (Negative, Positive)
                NativeMethods.WritePrivateProfileString(strTemp, "Direction", Equipment.stAxisParam[i].Home_Direction.ToString(), strFIle);
                //  Home 1st Speed
                NativeMethods.WritePrivateProfileString(strTemp, "1stSpeed", Equipment.stAxisParam[i].Home_Speed_1st.ToString(), strFIle);
                //  Home 2nd Speed
                NativeMethods.WritePrivateProfileString(strTemp, "2ndSpeed", Equipment.stAxisParam[i].Home_Speed_2nd.ToString(), strFIle);
                //  Home 3rd Speed
                NativeMethods.WritePrivateProfileString(strTemp, "3rdSpeed", Equipment.stAxisParam[i].Home_Speed_3rd.ToString(), strFIle);
                //  Home Last Speed
                NativeMethods.WritePrivateProfileString(strTemp, "LastSpeed", Equipment.stAxisParam[i].Home_Speed_Last.ToString(), strFIle);
                //  Home Offset
                NativeMethods.WritePrivateProfileString(strTemp, "Offset", Equipment.stAxisParam[i].Home_Offset.ToString(), strFIle);

                strTemp = string.Format("Axis_{0}_Common", i);
                //  Unit Per Pulse (Unit)
                NativeMethods.WritePrivateProfileString(strTemp, "Unit", Equipment.stAxisParam[i].Common_UnitPerPulse_Unit.ToString(), strFIle);
                //  Unit Per Pulse (Pulse)
                NativeMethods.WritePrivateProfileString(strTemp, "Pulse", Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse.ToString(), strFIle);
                //  Acceleration Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinAcc", Equipment.stAxisParam[i].Common_Acceleration_Min.ToString(), strFIle);
                //  Acceleration Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxAcc", Equipment.stAxisParam[i].Common_Acceleration_Max.ToString(), strFIle);
                //  Acceleration Fine
                NativeMethods.WritePrivateProfileString(strTemp, "FineAcc", Equipment.stAxisParam[i].Common_Acceleration_Fine.ToString(), strFIle);
                //  Acceleration Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseAcc", Equipment.stAxisParam[i].Common_Acceleration_Coarse.ToString(), strFIle);
                //  Speed Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinSpeed", Equipment.stAxisParam[i].Common_Speed_Min.ToString(), strFIle);
                //  Speed Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxSpeed", Equipment.stAxisParam[i].Common_Speed_Max.ToString(), strFIle);
                //  Move Speed FIne
                NativeMethods.WritePrivateProfileString(strTemp, "FineSpeed", Equipment.stAxisParam[i].Common_Speed_Fine.ToString(), strFIle);
                //  Move Speed Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseSpeed", Equipment.stAxisParam[i].Common_Speed_Coarse.ToString(), strFIle);
                //  Position Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinPos", Equipment.stAxisParam[i].Common_Position_Min.ToString(), strFIle);
                //  Position Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxPos", Equipment.stAxisParam[i].Common_Position_Max.ToString(), strFIle);
                //  Settle Delay Time
                NativeMethods.WritePrivateProfileString(strTemp, "SettleDelay", Equipment.stAxisParam[i].Common_Settle_Delay.ToString(), strFIle);

                strTemp = string.Format("Axis_{0}_Jog", i);
                //  Jog Speed, Fine
                NativeMethods.WritePrivateProfileString(strTemp, "FineSpeed", Equipment.stAxisParam[i].Jog_Speed_Fine.ToString(), strFIle);
                //  Jog Speed, Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseSpeed", Equipment.stAxisParam[i].Jog_Speed_Coarse.ToString(), strFIle);
                //  Jog StepSize, Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinStepSize", Equipment.stAxisParam[i].Jog_StepSize_Min.ToString(), strFIle);
                //  Jog StepSize, Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxStepSize", Equipment.stAxisParam[i].Jog_StepSize_Max.ToString(), strFIle);
                //  Jog StepSize, Fine
                NativeMethods.WritePrivateProfileString(strTemp, "FineStepSize", Equipment.stAxisParam[i].Jog_StepSize_Fine.ToString(), strFIle);
                //  Jog StepSize, Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseStepSize", Equipment.stAxisParam[i].Jog_StepSize_Coarse.ToString(), strFIle);
            }

            MessageBox.Show("Axis Setting 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Setup_Motion_Save_Click(object sender, EventArgs e)
        {
            Axis_Parameter_Save();
        }

        private void listBox_Setup_Motion_SelectAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  축 선택에 따른 데이터 표시

            int m_nIndex = listBox_Setup_Motion_SelectAxis.SelectedIndex;

            if (m_nIndex < 0)
            {
                return;
            }

            comboBox_Setup_Motion_Limit_Install.SelectedIndex = Equipment.stAxisParam[m_nIndex].LimitSensor_Installed;
            comboBox_Setup_Motion_Limit_Active.SelectedIndex = Equipment.stAxisParam[m_nIndex].LimitSensor_ActiveLevel;

            comboBox_Setup_Motion_Home_Sensing.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_Sensing;
            comboBox_Setup_Motion_Home_Install.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_Installed;
            comboBox_Setup_Motion_Home_Active.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_ActiveLevel;
            comboBox_Setup_Motion_Home_Direction.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_Direction;
            textBox_Setup_Motion_Home_Speed_1st.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_1st.ToString();
            textBox_Setup_Motion_Home_Speed_2nd.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_2nd.ToString();
            textBox_Setup_Motion_Home_Speed_3rd.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_3rd.ToString();
            textBox_Setup_Motion_Home_Speed_Last.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_Last.ToString();
            textBox_Setup_Motion_Home_Offset.Text = Equipment.stAxisParam[m_nIndex].Home_Offset.ToString();

            textBox_Setup_Motion_Common_Unit.Text = Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Unit.ToString();
            textBox_Setup_Motion_Common_Pulse.Text = Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Pulse.ToString();
            textBox_Setup_Motion_Common_MinAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Min.ToString();
            textBox_Setup_Motion_Common_MaxAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Max.ToString();
            textBox_Setup_Motion_Common_FineAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Coarse.ToString();
            textBox_Setup_Motion_Common_MaxSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Max.ToString();
            textBox_Setup_Motion_Common_MinSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Min.ToString();
            textBox_Setup_Motion_Common_FineSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Coarse.ToString();
            textBox_Setup_Motion_Common_MinPos.Text = Equipment.stAxisParam[m_nIndex].Common_Position_Min.ToString();
            textBox_Setup_Motion_Common_MaxPos.Text = Equipment.stAxisParam[m_nIndex].Common_Position_Max.ToString();
            textBox_Setup_Motion_Common_SettleDelay.Text = Equipment.stAxisParam[m_nIndex].Common_Settle_Delay.ToString();

            textBox_Setup_Motion_Jog_FineSpeed.Text = Equipment.stAxisParam[m_nIndex].Jog_Speed_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseSpeed.Text = Equipment.stAxisParam[m_nIndex].Jog_Speed_Coarse.ToString();
            textBox_Setup_Motion_Jog_MinStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Min.ToString();
            textBox_Setup_Motion_Jog_MaxStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Max.ToString();
            textBox_Setup_Motion_Jog_FineStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Coarse.ToString();
        }

        private void comboBox_Setup_Communication_TCPIP_OpenType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int m_nIndex = comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex;

            if (m_nIndex == 0)              //  Server
            {
                label_Setup_Communication_TCPIP_IP.Text = "Host IP :";
            }
            else if (m_nIndex == 1)         //  Client
            {
                label_Setup_Communication_TCPIP_IP.Text = "Remote IP :";
            }
        }

        private void button_Setup_Comm_Save_Click(object sender, EventArgs e)
        {
            Comm_Parameter_Save();
        }

        public bool Comm_Parameter_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Comm Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Comm. Setting 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  통신 모듈 파라미터 파일을 로드 하면 맨 첫번째 축 데이터를 표시하도록 한다.
            listBox_Setup_Communication_SelectUnit.SelectedIndex = 0;

            tabControl_Setup_Communication_Type.SelectedIndex = Equipment.stCommunicationSet[0].Comm_Type;

            if (Equipment.stCommunicationSet[0].Comm_Type == 0)                      //  TCP/IP
            {
                radioButton_Setup_Communication_Comm_TCPIP.Checked = true;

                comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex = Equipment.stCommunicationSet[0].TCPIP_PortType;
                textBox_Setup_Communication_TCPIP_IP.Text = Equipment.stCommunicationSet[0].TCPIP_IPAddress;
                textBox_Setup_Communication_TCPIP_Port.Text = Equipment.stCommunicationSet[0].TCPIP_PortNum.ToString();
            }
            else                                                                            //  RS232
            {
                radioButton_Setup_Communication_Comm_RS232.Checked = true;

                textBox_Setup_Communication_RS232_Timeout.Text = Equipment.stCommunicationSet[0].Serial_CommTimeout.ToString();
                textBox_Setup_Communication_RS232_SpacingDelay.Text = Equipment.stCommunicationSet[0].Serial_CommSpacingDelay.ToString();

                comboBox_Setup_Communication_RS232_ComPort.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommPort;
                comboBox_Setup_Communication_RS232_BaudRate.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommBaudRate;
                comboBox_Setup_Communication_RS232_DataBit.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommDataBits;
                comboBox_Setup_Communication_RS232_StopBit.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommStopBits;
                comboBox_Setup_Communication_RS232_Parity.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommParity;
                comboBox_Setup_Communication_RS232_FlowControl.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommFlowControl;
            }

            return m_bRet;
        }

        public void Comm_Parameter_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Comm Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Comm. Setting 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  선택된 Comm. Unit 에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Setup_Communication_SelectUnit.SelectedIndex;

            if (m_nIndex >= 0)
            {
                Equipment.stCommunicationSet[m_nIndex].Comm_Type = radioButton_Setup_Communication_Comm_TCPIP.Checked ? 0 : 1;

                Equipment.stCommunicationSet[m_nIndex].TCPIP_PortType = comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress = textBox_Setup_Communication_TCPIP_IP.Text;
                Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum = Convert.ToInt16(textBox_Setup_Communication_TCPIP_Port.Text);

                Equipment.stCommunicationSet[m_nIndex].Serial_CommTimeout = Convert.ToInt16(textBox_Setup_Communication_RS232_Timeout.Text); 
                Equipment.stCommunicationSet[m_nIndex].Serial_CommSpacingDelay = Convert.ToInt16(textBox_Setup_Communication_RS232_SpacingDelay.Text);
                Equipment.stCommunicationSet[m_nIndex].Serial_CommPort = comboBox_Setup_Communication_RS232_ComPort.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommBaudRate = comboBox_Setup_Communication_RS232_BaudRate.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommDataBits = comboBox_Setup_Communication_RS232_DataBit.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommStopBits = comboBox_Setup_Communication_RS232_StopBit.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommParity = comboBox_Setup_Communication_RS232_Parity.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommFlowControl = comboBox_Setup_Communication_RS232_FlowControl.SelectedIndex;
            }


            //  Comm. Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(CommList)).Length; i++)
            {
                strTemp = string.Format("CommUnit_{0}", i);


                //  TCP/IP, RS232                                                                                   //  0 : TCP/IP,     1 : RS232
                NativeMethods.WritePrivateProfileString(strTemp, "CommType", Equipment.stCommunicationSet[i].Comm_Type.ToString(), strFIle);


                //  TCP/IP 의 포트 형식 (Server, Client)                                                            //  0 : Server,     1 : Client
                NativeMethods.WritePrivateProfileString(strTemp, "TCPIP_PortType", Equipment.stCommunicationSet[i].TCPIP_PortType.ToString(), strFIle);
                //  TCP/IP 의 IP 주소
                NativeMethods.WritePrivateProfileString(strTemp, "TCPIP_IPAddress", Equipment.stCommunicationSet[i].TCPIP_IPAddress.ToString(), strFIle);
                //  TCP/IP 의 Port 번호
                NativeMethods.WritePrivateProfileString(strTemp, "TCPIP_PortNum", Equipment.stCommunicationSet[i].TCPIP_PortNum.ToString(), strFIle);


                //  Timeout (ms)
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_Timeout", Equipment.stCommunicationSet[i].Serial_CommTimeout.ToString(), strFIle);
                //  Spacing Delay (ms)
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_SpacingDelay", Equipment.stCommunicationSet[i].Serial_CommSpacingDelay.ToString(), strFIle);
                //  COM Port                                                                                        //  0 : COM1,       1 : COM2,       2 : COM3,       3 : COM4 ....
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_Port", Equipment.stCommunicationSet[i].Serial_CommPort.ToString(), strFIle);
                //  Baud Rate                                                                                       //  0 : 9600,       1 : 19200,      2 : 38400,      3 : 57600,      4 : 115200
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_BaudRate", Equipment.stCommunicationSet[i].Serial_CommBaudRate.ToString(), strFIle);
                //  Data Bits                                                                                       //  0 : 5,          1 : 6,          2 : 7,          3 : 8
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_DataBit", Equipment.stCommunicationSet[i].Serial_CommDataBits.ToString(), strFIle);
                //  Stop Bits                                                                                       //  0 : 1,          1 : 1.5,        2 : 2
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_StopBit", Equipment.stCommunicationSet[i].Serial_CommStopBits.ToString(), strFIle);
                //  Parity                                                                                          //  0 : None,       1 : Odd,        2 : Even
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_Parity", Equipment.stCommunicationSet[i].Serial_CommParity.ToString(), strFIle);
                //  Flow Control                                                                                    //  0 : None,       1 : Xon/Xoff,   2 : RTS/CTS
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_FlowControl", Equipment.stCommunicationSet[i].Serial_CommFlowControl.ToString(), strFIle);
            }

            MessageBox.Show("Comm. Setting 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void radioButton_Setup_Communication_Comm_TCPIP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Setup_Communication_Comm_TCPIP.Checked)
            {
                tabControl_Setup_Communication_Type.SelectedIndex = 0;              //  TCP/IP
            }
            else
            {
                tabControl_Setup_Communication_Type.SelectedIndex = 1;              //  RS232
            }
        }

        private void listBox_Setup_Communication_SelectUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  통신 모듈 선택에 따른 데이터 표시

            int m_nIndex = listBox_Setup_Communication_SelectUnit.SelectedIndex;

            if (m_nIndex < 0)
            {
                return;
            }


            tabControl_Setup_Communication_Type.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Comm_Type;

            if (Equipment.stCommunicationSet[m_nIndex].Comm_Type == 0)                      //  TCP/IP
            {
                radioButton_Setup_Communication_Comm_TCPIP.Checked = true;

                comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortType;
                textBox_Setup_Communication_TCPIP_IP.Text = Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress;
                textBox_Setup_Communication_TCPIP_Port.Text = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum.ToString();
            }
            else                                                                            //  RS232
            {
                radioButton_Setup_Communication_Comm_RS232.Checked = true;

                textBox_Setup_Communication_RS232_Timeout.Text = Equipment.stCommunicationSet[m_nIndex].Serial_CommTimeout.ToString();
                textBox_Setup_Communication_RS232_SpacingDelay.Text = Equipment.stCommunicationSet[m_nIndex].Serial_CommSpacingDelay.ToString();

                comboBox_Setup_Communication_RS232_ComPort.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommPort;
                comboBox_Setup_Communication_RS232_BaudRate.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommBaudRate;
                comboBox_Setup_Communication_RS232_DataBit.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommDataBits;
                comboBox_Setup_Communication_RS232_StopBit.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommStopBits;
                comboBox_Setup_Communication_RS232_Parity.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommParity;
                comboBox_Setup_Communication_RS232_FlowControl.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommFlowControl;
            }
        }

        private void button_Test_SocketConnect_Click(object sender, EventArgs e)
        {
            workStage.Laser_Socket_Connect("127.0.0.1", 5000);
        }
    }
}
