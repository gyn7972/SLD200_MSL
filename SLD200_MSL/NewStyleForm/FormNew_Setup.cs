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
//using OpenTK;
//using OpenTK.Input;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin;
using QMC.Vision;
using QMC.Core;
using static QMC.Common.Equipment;
using QMC.Common.Parts;
using System.Numerics;
using SpiralLab.Sirius;
using Point = System.Drawing.Point;
using System.Windows.Controls;

namespace SLD200_MSL
{
    public partial class FormNew_Setup : Form
    {
        FormNew_VisionPopup m_formVisionPopup = new FormNew_VisionPopup();

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Bds Bds;

        FormNew_CommunicationTerminal m_formCommTerminal = new FormNew_CommunicationTerminal();

        //  IO
        private DioPointCollection m_DioPoints;
        private IOListControl Inputlist;
        private IOListControl Outputlist;
        private DioPointCollection InputPoint;
        private DioPointCollection OutputPoint;
        public Size IOGridSize { protected set; get; }

        private FormNew_KeyPad m_keyPad;
        public System.Windows.Forms.Timer timer_Status;

        protected XyzyStage m_Stage;
        private _2DMappingDataControl m_2DMappingDataControl;
        private _2DMappingFileControl m_2DMappingFileControl;

        // 현재 스캐너 보정 파일
        private string m_srcFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
        // 신규로 생성할 스캐너 보정 파일
        private string m_targetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", $"newfile.ct5");

        // 3x3 (9개) 위치에 대한 보정 테이블 입력용
        private float m_fieldSize = 105;
        private float m_rowInterval = 2; //20;
        private float m_colInterval = 2; //20;
        private int m_row = 5; //3
        private int m_col = 5; //3
        private float m_kfactor = 0; // = (float)Math.Pow(2, 20) / m_fieldSize;
        private Correction2DRtc m_correction2DRtc = null;
        private Correction2DRtcForm m_correction2DRtcForm = null;
        

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
            Machine_Option_Apply();
            MappingData_List_Apply();

            workStage.MapData_Load();

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 20;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
            timer_Status.Enabled = true;


            //  for 2D Mapping
            m_Stage = workStage.Stage;

            this.m_2DMappingDataControl = new _2DMappingDataControl(m_Stage);
            this.m_2DMappingDataControl.Location = new Point(10, 6);
            this.tabPage_Setup_2DMapping.Controls.Add(this.m_2DMappingDataControl);

            this.m_2DMappingFileControl = new _2DMappingFileControl(m_Stage);
            this.m_2DMappingFileControl.Location = new Point(m_2DMappingDataControl.Location.X + m_2DMappingDataControl.Size.Width + 20, m_2DMappingDataControl.Location.Y);
            this.tabPage_Setup_2DMapping.Controls.Add(this.m_2DMappingFileControl);


            //  Scanner Calibration
            textBox_Setup_ScannerCal_LaserFrequency.Text = Equipment.Scanner_Calibration_LaserFrequency.ToString();
            textBox_Setup_ScannerCal_LaserEnergy.Text = Equipment.Scanner_Calibration_LaserEnergy.ToString();
            textBox_Setup_ScannerCal_CrossMarkLength.Text = Equipment.Scanner_Calibration_CrossMarkLength.ToString();
            textBox_Setup_ScannerCal_MarkingSpeed.Text = Equipment.Scanner_Calibration_LaserMarkSpeed.ToString();
            textBox_Setup_ScannerCal_JumpSpeed.Text = Equipment.Scanner_Calibration_LaserJumpSpeed.ToString();





            //m_fieldSize = Equipment.Scanner_Calibration_FieldSize;
            m_srcFile = Equipment.Scanner_Calibration_srcFilePath;
            m_targetFile = Equipment.Scanner_Calibration_targetFilePath;
            m_rowInterval = Equipment.Scanner_Calibration_rowInterval;
            m_colInterval = Equipment.Scanner_Calibration_colInterval;
            m_row = Equipment.Scanner_Calibration_rowCount; 
            m_col = Equipment.Scanner_Calibration_colCount;

            //m_kfactor = (float)Math.Pow(2, 20) / m_fieldSize;
            m_kfactor = (float)18830.1889;

            //m_correction2DRtc.KFactor = m_kfactor;
            //m_correction2DRtc.Rows = m_row;
            //m_correction2DRtc.Cols = m_col;
            //m_correction2DRtc.RowInterval = m_rowInterval;
            //m_correction2DRtc.ColInterval = m_colInterval;
            //m_correction2DRtc.SourceCorrectionFile = m_srcFile;
            //m_correction2DRtc.TargetCorrectionFile = m_targetFile;
            //m_correction2DRtcForm.RefreshData();

            m_correction2DRtc = new Correction2DRtc(m_kfactor, m_row, m_col, m_rowInterval, m_colInterval, m_srcFile, m_targetFile);
            m_correction2DRtcForm = new Correction2DRtcForm(m_correction2DRtc);
            m_correction2DRtcForm.TopLevel = false; // 폼을 최상위 폼이 아니도록 설정
            m_correction2DRtcForm.FormBorderStyle = FormBorderStyle.None; // 폼의 테두리를 제거
            m_correction2DRtcForm.Dock = DockStyle.Fill; // 폼을 패널에 맞게 채움
            this.textBox_ScannerCal_LaserFrequency.Controls.Add(m_correction2DRtcForm);
            m_correction2DRtcForm.Show();
            workStage.scannerCompensator.ActionSaveDone += OnSaveDone;
            workStage.scannerCompensator.ActionSaveDoneAllData += OnSaveDataDone;


        }

        // ActionSaveDone 이벤트 핸들러
        private void OnSaveDone(string message)
        {
            //m_correction2DRtc.ConvertFromDatFile(message);

            //m_correction2DRtcForm.RefreshData();

            //m_correction2DRtc.Convert();

            
        }
        private void OnSaveDataDone(QMCFindLenzCenter list)
        {
            //CorrectionData correctionData;

            //correctionData.
            //List<CorrectionData> listCorrection = new List<CorrectionData>();
            //foreach(var s in list)
            //{
            //    var v = new CorrectionData();
            //    v.Col = s.m_nindexY;
            //    v.Row = s.m_nIndexX;
            //    //v.ReferenceX = s.m_dMeasureX;
            //    //v.ReferenceY = s.m_dMeasureY;
            //    //v.MeasuredX = s.m_dOffsetX;
            //    //v.MeasuredY = s.m_dOffsetY;
            //    v.ReferenceX = s.m_dOffsetX;
            //    v.ReferenceY = s.m_dOffsetY;
            //    v.MeasuredX = s.m_dMeasureX;
            //    v.MeasuredY = s.m_dMeasureY;
            //    listCorrection.Add(v);
            //}

            this.ProcessCorrectionData(list);
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

            //  MapData Status
            if (Equipment.MapDataStatus_Activate)
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Activated";
                button_Setup_2DMapData_Apply.BackColor = Color.Lime;

                workStage.Stage.Config.Use2DMap = true;
            }
            else
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Deactivated";
                button_Setup_2DMapData_Apply.BackColor = Color.LightGray;

                workStage.Stage.Config.Use2DMap = false;
            }

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

            if (Equipment.Machine_LaserType_CO2)
            {
                //  Mask Position
                label_Setup_EncPosition_MASK_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.MASK_Y));
            }
            else
            {
                label_Setup_EncPosition_MASK_Y.Text = "0.0";
            }

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

        public void Axis_Properties_Apply()
        {
            int lAxisNo = 0;
            int lMovePulse = 1;
            uint duModeProfile = 0, duUseAlarm;
            uint duMethodPulse = 0, duMethodEncoder = 0, duModeAbsRel = 0;
            double dVelocityMin = 1.0, dVelocityMax = 100.0, dMoveUnit = 1.0;

            uint duUseInp, duLevelAlmRst, duRetCode;
            uint duLevelLimitN = 0, duLevelLimitP = 0, duLevelZPhase = 0;
            uint duModeStop = 0, duLevelStop = 0, duLevelServoOn = 0;
            uint duTypeEncoder = 0;

            int lHomeDir = 0;
            uint duHomeSignal = 0, duLevelHome = 0, duZphas = 0;
            double dHomeClrTime = 1000.0, dHomeOffset = 0.0;

            uint duUse = 0, duStopMode = 0, duSelection = 0;
            double dPositivePos = 0.0, dNegativePos = 0.0;

            double dInitPos, dInitVel, dInitAccel, dInitDecel;

            for (int i = 0; i < Equipment.Max_Axis; i++)
            {
                //  Limit Install
                if (Equipment.stAxisParam[i].LimitSensor_Installed == 0)            //  Not Installed
                {
                    AXM.SetNegativeLimitNotUse(i);
                    AXM.SetPositiveLimitNotUse(i);
                }
                else if (Equipment.stAxisParam[i].LimitSensor_Installed == 1)       //  Installed
                {
                    AXM.SetNegativeLimitAction(i, MotorEventAction.Stop);
                    AXM.SetPositiveLimitAction(i, MotorEventAction.Stop);
                }

                //  Limit Active Level
                if (Equipment.stAxisParam[i].LimitSensor_ActiveLevel == 0)          //  Active Low
                {
                    AXM.SetNegativeLimitLevel(i, ActiveLevel.Low);
                    AXM.SetPositiveLimitLevel(i, ActiveLevel.Low);
                }
                else if (Equipment.stAxisParam[i].LimitSensor_ActiveLevel == 1)     //  Active High
                {
                    AXM.SetNegativeLimitLevel(i, ActiveLevel.High);
                    AXM.SetPositiveLimitLevel(i, ActiveLevel.High);
                }

                //  Home Direction
                if (Equipment.stAxisParam[i].Home_Direction == 0)                   //  Negative (CCW)
                {
                    lHomeDir = (int)Directions.Ccw;
                }
                else if (Equipment.stAxisParam[i].Home_Direction == 1)              //  Positive (CW)
                {
                    lHomeDir = (int)Directions.Cw;
                }

                //  Home Active Level
                if (Equipment.stAxisParam[i].Home_ActiveLevel == 0)                 //  Active Low
                {
                    AXM.SetHomeSensorLevel(i, ActiveLevel.Low);
                }
                else if (Equipment.stAxisParam[i].Home_ActiveLevel == 1)            //  Active High
                {
                    AXM.SetHomeSensorLevel(i, ActiveLevel.High);
                }

                //  Home Offset
                dHomeOffset = Equipment.stAxisParam[i].Home_Offset;

                //  Home Sensing
                if (Equipment.stAxisParam[i].Home_Sensing == 0)                     //  Using Home Sensor
                {
                    AXM.SetHomeMethod(i, (Directions)lHomeDir, HomeSignals.HomeSensor, (ZPhaseMethods)Equipment.stAxisParam[i].Home_ZPhase_Use, Equipment.stAxisParam[i].Home_Clear_Time, dHomeOffset);
                }
                else if (Equipment.stAxisParam[i].Home_Sensing == 1)                //  Using -Limit Sensor
                {
                    AXM.SetHomeMethod(i, (Directions)lHomeDir, HomeSignals.NegativeLimit, (ZPhaseMethods)Equipment.stAxisParam[i].Home_ZPhase_Use, Equipment.stAxisParam[i].Home_Clear_Time, dHomeOffset);
                }
                else if (Equipment.stAxisParam[i].Home_Sensing == 2)                //  Using +Limit Sensor
                {
                    AXM.SetHomeMethod(i, (Directions)lHomeDir, HomeSignals.PositiveLimit, (ZPhaseMethods)Equipment.stAxisParam[i].Home_ZPhase_Use, Equipment.stAxisParam[i].Home_Clear_Time, dHomeOffset);
                }

                //  Home Speed
                AXM.SetHomeVelocity(i, Equipment.stAxisParam[i].Home_Speed_1st, Equipment.stAxisParam[i].Home_Speed_2nd, Equipment.stAxisParam[i].Home_Speed_3rd, Equipment.stAxisParam[i].Home_Speed_Last, 
                                        Equipment.stAxisParam[i].Home_Acceleration_1st, Equipment.stAxisParam[i].Home_Acceleration_2nd);

                //  Unit Per Pulse
                AXM.AxmMotSetMoveUnitPerPulse(i, Equipment.stAxisParam[i].Common_UnitPerPulse_Unit, Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse);


                //  요것들은 나중에 하자...

                //textBox_Setup_Motion_Common_MinAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Min.ToString();
                //textBox_Setup_Motion_Common_MaxAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Max.ToString();
                //textBox_Setup_Motion_Common_FineAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Fine.ToString();
                //textBox_Setup_Motion_Common_CoarseAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Coarse.ToString();
                //textBox_Setup_Motion_Common_MaxSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Max.ToString();
                //textBox_Setup_Motion_Common_MinSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Min.ToString();
                //textBox_Setup_Motion_Common_FineSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Fine.ToString();
                //textBox_Setup_Motion_Common_CoarseSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Coarse.ToString();
                //textBox_Setup_Motion_Common_MinPos.Text = Equipment.stAxisParam[i].Common_Position_Min.ToString();
                //textBox_Setup_Motion_Common_MaxPos.Text = Equipment.stAxisParam[i].Common_Position_Max.ToString();
                //textBox_Setup_Motion_Common_SettleDelay.Text = Equipment.stAxisParam[i].Common_Settle_Delay.ToString();

                //textBox_Setup_Motion_Jog_FineSpeed.Text = Equipment.stAxisParam[i].Jog_Speed_Fine.ToString();
                //textBox_Setup_Motion_Jog_CoarseSpeed.Text = Equipment.stAxisParam[i].Jog_Speed_Coarse.ToString();
                //textBox_Setup_Motion_Jog_MinStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Min.ToString();
                //textBox_Setup_Motion_Jog_MaxStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Max.ToString();
                //textBox_Setup_Motion_Jog_FineStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Fine.ToString();
                //textBox_Setup_Motion_Jog_CoarseStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Coarse.ToString();
            }            

            //// Pulse/Encoder Method && Move Parameter Setting
            //duMethodPulse = ConvertComboToAxm(ref cboPulse);
            //duMethodEncoder = ConvertComboToAxm(ref cboEncoder);
            //duUseAlarm = ConvertComboToAxm(ref cboAlarm);
            //duModeAbsRel = ConvertComboToAxm(ref cboAbsRel);
            //duModeProfile = ConvertComboToAxm(ref cboProfile);
            //dVelocityMin = Convert.ToDouble(edtMinVel.Text);
            //dVelocityMax = Convert.ToDouble(edtMaxVel.Text);
            //lMovePulse = Convert.ToInt32(edtMovePulse.Text);
            //dMoveUnit = Convert.ToDouble(edtMoveUnit.Text);

            //// Input/Output Signal Setting
            //duUseInp = ConvertComboToAxm(ref cboInp);
            //duLevelAlmRst = ConvertComboToAxm(ref cboAlarmReset);
            //duLevelLimitN = ConvertComboToAxm(ref cboELimitN);
            //duLevelLimitP = ConvertComboToAxm(ref cboELimitP);
            //duLevelZPhase = ConvertComboToAxm(ref cboZPhaseLev);
            //duModeStop = ConvertComboToAxm(ref cboStopMode);
            //duLevelStop = ConvertComboToAxm(ref cboStopLevel);
            //duLevelServoOn = ConvertComboToAxm(ref cboServoOn);
            //duTypeEncoder = ConvertComboToAxm(ref cboEncoderType);

            //// Home Search Setting
            //duLevelHome = ConvertComboToAxm(ref cboHomeLevel);
            //duHomeSignal = ConvertComboToAxm(ref cboHomeSignal);
            //lHomeDir = (int)ConvertComboToAxm(ref cboHomeDir);
            //duZphas = ConvertComboToAxm(ref cboZPhaseUse);
            //dHomeClrTime = Convert.ToDouble(edtHomeClrTime.Text);
            //dHomeOffset = Convert.ToDouble(edtHomeOffset.Text);

            //// Software Limit Setting
            //dNegativePos = Convert.ToDouble(edtSwPosN.Text);
            //dPositivePos = Convert.ToDouble(edtSwPosP.Text);

            //// User Move Parameter Setting
            //dInitPos = Convert.ToDouble(edtPosition.Text);
            //dInitVel = Convert.ToDouble(edtVelocity.Text);
            //dInitAccel = Convert.ToDouble(edtAccel.Text);
            //dInitDecel = Convert.ToDouble(edtDecel.Text);

            //for (lAxisNo = 0; lAxisNo < m_lAxisCounts; lAxisNo++)
            //{
            //    // Pulse/Encoder Method && Move Parameter Setting
            //    if (cboPulse.Enabled == true)
            //    {
            //        //++ 지정 축의 펄스 출력 방식을 설정합니다.
            //        //uMethod : (0)OneHighLowHigh   - 1펄스 방식, PULSE(Active High), 정방향(DIR=Low)  / 역방향(DIR=High)
            //        //          (1)OneHighHighLow   - 1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
            //        //          (2)OneLowLowHigh    - 1펄스 방식, PULSE(Active Low),  정방향(DIR=Low)  / 역방향(DIR=High)
            //        //          (3)OneLowHighLow    - 1펄스 방식, PULSE(Active Low),  정방향(DIR=High) / 역방향(DIR=Low)
            //        //          (4)TwoCcwCwHigh     - 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active High     
            //        //          (5)TwoCcwCwLow      - 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active Low     
            //        //          (6)TwoCwCcwHigh     - 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active High
            //        //          (7)TwoCwCcwLow      - 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active Low
            //        //          (8)TwoPhase         - 2상(90' 위상차),  PULSE lead DIR(CW: 정방향), PULSE lag DIR(CCW:역방향)
            //        //          (9)TwoPhaseReverse  - 2상(90' 위상차),  PULSE lead DIR(CCW: 정방향), PULSE lag DIR(CW:역방향)
            //        CAXM.AxmMotSetPulseOutMethod(lAxisNo, duMethodPulse);
            //    }

            //    if (cboEncoder.Enabled == true)
            //    {
            //        //++ 지정 축의 Encoder 입력 방식을 설정합니다.
            //        // uMethod : (0)ObverseUpDownMode - 정방향 Up/Down
            //        //           (1)ObverseSqr1Mode   - 정방향 1체배
            //        //           (2)ObverseSqr2Mode   - 정방향 2체배
            //        //           (3)ObverseSqr4Mode   - 정방향 4체배
            //        //           (4)ReverseUpDownMode - 역방향 Up/Down
            //        //           (5)ReverseSqr1Mode   - 역방향 1체배
            //        //           (6)ReverseSqr2Mode   - 역방향 2체배
            //        //           (7)ReverseSqr4Mode   - 역방향 4체배
            //        CAXM.AxmMotSetEncInputMethod(lAxisNo, duMethodEncoder);
            //    }

            //    if (cboAlarm.Enabled == true)
            //    {
            //        //++ 지정 축의 비상정지 신호 사용유무/Active Level을 설정합니다. 
            //        CAXM.AxmSignalSetServoAlarm(lAxisNo, duUseAlarm);
            //    }
            //    if (cboAbsRel.Enabled == true)
            //    {
            //        //++ 지정 축의 구동 좌표계를 설정합니다. 
            //        // duAbsRelMode : (0)POS_ABS_MODE - 현재 위치와 상관없이 지정한 위치로 절대좌표 이동합니다.
            //        //                (1)POS_REL_MODE - 현재 위치에서 지정한 양만큼 상대좌표 이동합니다.
            //        CAXM.AxmMotSetAbsRelMode(lAxisNo, duMethodPulse);
            //    }
            //    if (cboProfile.Enabled == true)
            //    {
            //        // duProfileMode : (0)SYM_TRAPEZOID_MODE  - Symmetric Trapezoid
            //        //                 (1)ASYM_TRAPEZOID_MODE - Asymmetric Trapezoid
            //        //                 (2)QUASI_S_CURVE_MODE  - Symmetric Quasi-S Curve
            //        //                 (3)SYM_S_CURVE_MODE    - Symmetric S Curve
            //        //                 (4)ASYM_S_CURVE_MODE   - Asymmetric S Curve
            //        CAXM.AxmMotSetProfileMode(lAxisNo, duModeProfile);
            //    }

            //    //++ 지정 축의 초기 구동속도를 설정합니다.
            //    CAXM.AxmMotSetMinVel(lAxisNo, dVelocityMin);

            //    //++ 지정 축의 최대 구동속도를 설정합니다.
            //    CAXM.AxmMotSetMaxVel(lAxisNo, dVelocityMax);

            //    //++ 지정 축의 거리/속도/가속도의 제어단위를 설정합니다.
            //    CAXM.AxmMotSetMoveUnitPerPulse(lAxisNo, dMoveUnit, lMovePulse);

            //    // Input/Output Signal Setting
            //    //++ 지정 축의 Inposition(위치결정완료) 신호 Active Level/사용유무를 설정합니다.
            //    // - Inposition 신호를 사용안함으로 설정하면 모션제어 칩에서 펄스출력이 완료될 때 즉시구동 종료됩니다.
            //    // ※ [CAUTION] Inposition 신호를 사용함으로 설정하면 모션제어 칩에서 펄스출력이 완료된 후 서보팩으로 부터 
            //    //              Inposition(위치결정완료) 신호가 Active될 때 까지 모션 구동중으로 됩니다.
            //    // ※ [CAUTION] Inposition 신호를 사용할 때 Active Level이 맞지않으면 최초 한번 구동 후 모션구동이 종료되지않아 
            //    //              다음 구동을 할 수 없게 됩니다. 
            //    CAXM.AxmSignalSetInpos(lAxisNo, duUseInp);

            //    //++ 지정 축의 Alarm Reset 신호 Active Level을 설정합니다.
            //    CAXM.AxmSignalSetServoAlarmResetLevel(lAxisNo, duLevelAlmRst);

                

            //    //++ 지정 축의 Z상 Active Level을 설정합니다.
            //    CAXM.AxmSignalSetZphaseLevel(lAxisNo, duLevelZPhase);

            //    //++ 지정 축의 Emergency 신호 Active Level/사용유무를 설정합니다.
            //    CAXM.AxmSignalSetStop(lAxisNo, duModeStop, duLevelStop);

            //    //++ 지정 축의 Servo On/Off 신호 Active Level을 설정합니다.
            //    CAXM.AxmSignalSetServoOnLevel(lAxisNo, duLevelServoOn);

            //    //++ 지정 축의 Encoder Input Type를 설정합니다.
            //    // duEncoderType : (0)TYPE_INCREMENTAL
            //    //                 (1)TYPE_ABSOLUTE
            //    CAXDev.AxmSignalSetEncoderType(lAxisNo, duTypeEncoder);

            //    // Home Search Setting
            //    //++ 지정 축의 원점신호 Active Level을 설정합니다.
            //    CAXM.AxmHomeSetSignalLevel(lAxisNo, duLevelHome);

            //    //++ 지정 축의 원점검색 관련 정보들을 설정합니다.
            //    CAXM.AxmHomeSetMethod(lAxisNo, lHomeDir, duHomeSignal, duZphas, dHomeClrTime, dHomeOffset);

            //    // Software Limit Setting
            //    //++ 지정한 축에 Software Limit기능을 확인합니다.
            //    duRetCode = CAXM.AxmSignalGetSoftLimit(lAxisNo, ref duUse, ref duStopMode, ref duSelection, ref dPositivePos, ref dNegativePos);
            //    if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            //    {
            //        //++ 지정 축의 소프트웨어 리미트를 설정합니다.
            //        // uUse       : (0)DISABLE        - 소프트웨어 리미트 기능을 사용하지 않습니다.
            //        //              (1)ENABLE         - 소프트웨어 리미트 기능을 사용합니다.
            //        // uStopMode  : (0)EMERGENCY_STOP - 소프트웨어 리미트 영역을 벗어날 경우 급정지합니다.
            //        //              (1)SLOWDOWN_STOP  - 소프트웨어 리미트 영역을 벗어날 경우 감속정지합니다.
            //        // uSelection : (0)COMMAND        - 기준위치를 지령위치로 합니다.
            //        //              (1)ACTUAL         - 기준위치를 엔코더 위치로 합니다.
            //        CAXM.AxmSignalSetSoftLimit(lAxisNo, duUse, duStopMode, duSelection, dPositivePos, dNegativePos);
            //    }

            //    //++ 지정 축의 사용자 관련 파라메타들을 설정합니다.
            //    CAXM.AxmMotSetParaLoad(lAxisNo, dInitPos, dInitVel, dInitAccel, dInitDecel);
            //}
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
                Equipment.stAxisParam[m_nIndex].Home_Clear_Time = Convert.ToDouble(textBox_Setup_Motion_Home_ClearTime.Text);
                Equipment.stAxisParam[m_nIndex].Home_ZPhase_Use = comboBox_Setup_Motion_Home_ZPhaseUse.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Offset = Convert.ToDouble(textBox_Setup_Motion_Home_Offset.Text);
                Equipment.stAxisParam[m_nIndex].Home_Acceleration_1st = Convert.ToDouble(textBox_Setup_Motion_Home_Acceleration_1st.Text);
                Equipment.stAxisParam[m_nIndex].Home_Acceleration_2nd = Convert.ToDouble(textBox_Setup_Motion_Home_Acceleration_2nd.Text);

                Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Unit = Convert.ToDouble(textBox_Setup_Motion_Common_Unit.Text);
                Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Pulse = Convert.ToInt16(textBox_Setup_Motion_Common_Pulse.Text);
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
                //  Home Clear Time
                NativeMethods.WritePrivateProfileString(strTemp, "ClearTime", Equipment.stAxisParam[i].Home_Clear_Time.ToString(), strFIle);
                //  Home ZPhase Use (Disable, Dir CCW, Dir CW)
                NativeMethods.WritePrivateProfileString(strTemp, "ZPhase", Equipment.stAxisParam[i].Home_ZPhase_Use.ToString(), strFIle);
                //  Home Offset
                NativeMethods.WritePrivateProfileString(strTemp, "Offset", Equipment.stAxisParam[i].Home_Offset.ToString(), strFIle);
                //  Home 1st Acceleration
                NativeMethods.WritePrivateProfileString(strTemp, "1stAccel", Equipment.stAxisParam[i].Home_Acceleration_1st.ToString(), strFIle);
                //  Home 2nd Acceleration
                NativeMethods.WritePrivateProfileString(strTemp, "2ndAccel", Equipment.stAxisParam[i].Home_Acceleration_2nd.ToString(), strFIle);

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
            textBox_Setup_Motion_Home_ClearTime.Text = Equipment.stAxisParam[m_nIndex].Home_Clear_Time.ToString();
            comboBox_Setup_Motion_Home_ZPhaseUse.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_ZPhase_Use;
            textBox_Setup_Motion_Home_Offset.Text = Equipment.stAxisParam[m_nIndex].Home_Offset.ToString();
            textBox_Setup_Motion_Home_Acceleration_1st.Text = Equipment.stAxisParam[m_nIndex].Home_Acceleration_1st.ToString();
            textBox_Setup_Motion_Home_Acceleration_2nd.Text = Equipment.stAxisParam[m_nIndex].Home_Acceleration_2nd.ToString();

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

            if (Equipment.stCommunicationSet[0].Connect)
            {
                radioButton_Setup_Communication_Comm_Connect.Checked = true;
            }
            else
            {
                radioButton_Setup_Communication_Comm_Disconnect.Checked = true;
            }

            return m_bRet;
        }

        public bool Machine_Option_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine Option 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }


            //  Laser Type                        
            if (Equipment.Machine_LaserType_CO2)                                            //  CO₂Laser
            {
                radioButton_Setup_Option_LaserType_CO2.Checked = true;
            }
            else                                                                            //  UV Laser
            {
                radioButton_Setup_Option_LaserType_UV.Checked = true;
            }


            //  Offset Distance
            textBox_Setup_Option_Offset_ScannerFineCam_X.Text = Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString();
            textBox_Setup_Option_Offset_ScannerFineCam_Y.Text = Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString();
            textBox_Setup_Option_Offset_FineCamCoarseCam_X.Text = Equipment.stOffsetDistance.FromFineCamToCoarseCam.X.ToString();
            textBox_Setup_Option_Offset_FineCamCoarseCam_Y.Text = Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y.ToString();
            textBox_Setup_Option_Offset_FineCamLaserHeightSensor_X.Text = Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X.ToString();
            textBox_Setup_Option_Offset_FineCamLaserHeightSensor_Y.Text = Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y.ToString();


            //  Scanner Head Offset
            textBox_ScannerOffset_X.Text = Equipment.Scanner_HeadOffset_X.ToString();
            textBox_ScannerOffset_Y.Text = Equipment.Scanner_HeadOffset_Y.ToString();
            textBox_ScannerOffset_Angle.Text = Equipment.Scanner_HeadOffset_Angle.ToString();


            //  Machine Coordinate Offset
            textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text = Equipment.CoordinateMatchingOffset_X.ToString();
            textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text = Equipment.CoordinateMatchingOffset_Y.ToString();


            //  Offset Distance
            textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text = Equipment.StageOffset_forDrilling_X.ToString();
            textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text = Equipment.StageOffset_forDrilling_Y.ToString();


            //  Keyence Laser Height Sensor 기준값 설정
            textBox_Setup_Option_ReferenceValue_atVisionFocusPosition.Text = Equipment.LaserHeightSensor_ReferenceValue_atVisionFocusPosition.ToString();
            textBox_Setup_Option_ReferenceValue_atScannerFocusPosition.Text = Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition.ToString();


            //  Options
            checkBox_Setup_Option_DoorEnable.Checked = Equipment.Machine_Door_Enable;
            checkBox_Setup_Option_VacuumSensorEnable.Checked = Equipment.Machine_VacuumSensor_Enable;
            textBox_Setup_Option_SignalHoldTime.Text = Equipment.Machine_SignalHoldTime.ToString();
            checkBox_Setup_Option_MAligner_ReleaseType.Checked = Equipment.Machine_MAligner_ReleaseType;
            textBox_Setup_Option_MAligner_WidenDistance.Text = Equipment.Machine_MAligner_WidenDistance.ToString();
            textBox_Setup_Option_MAligner_NarrowingDistance.Text = Equipment.Machine_MAligner_NarrowingDistance.ToString();
            checkBox_Setup_Option_VacuumStableTime_Enable.Checked = Equipment.Machine_VacuumStableTime_Enable;
            textBox_Setup_Option_VacuumStableTime.Text = Equipment.Machine_VacuumStableTime.ToString();
            checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked = Equipment.Machine_LaserHeightCheckStableTime_Enable;
            textBox_Setup_Option_LaserHeightCheckStableTime.Text = Equipment.Machine_LaserHeightCheckStableTime.ToString();
            checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked = Equipment.Machine_FiducialMarkJudgementRange_Enable;
            textBox_Setup_Option_FiducialMarkJudgementRange.Text = Equipment.Machine_FiducialMarkJudgementRange.ToString();
            if (Equipment.Machine_FiducialImageSave_Always)
            {
                radioButton_Setup_Option_FiducialImageSave_Always.Checked = true;
            }
            else
            {
                radioButton_Setup_Option_FiducialImageSave_FailedToFind.Checked = true;
            }

            return m_bRet;
        }

        public void MappingData_List_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Mapping Data 리스트 파일이 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //  Stage Center 가 Scanner 위치에 있을 경우
                Equipment.MappingData_FilePath_Stage_Scanner = "";

                //  Stage Center 가 Fine Camera 위치에 있을 경우
                Equipment.MappingData_FilePath_Stage_FineCam = "";

                //  Stage Calibration 위치가 Scanner 위치에 있을 경우
                Equipment.MappingData_FilePath_StageCal_Scanner = "";

                //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
                Equipment.MappingData_FilePath_StageCal_FineCam = "";

                return ;
            }


            //  Stage Center 가 Scanner 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCenter_ScannerPosition.Text = Equipment.MappingData_FilePath_Stage_Scanner;

            //  Stage Center 가 Fine Camera 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCenter_FineCameraPosition.Text = Equipment.MappingData_FilePath_Stage_FineCam;

            //  Stage Calibration 위치가 Scanner 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCalPos_ScannerPosition.Text = Equipment.MappingData_FilePath_StageCal_Scanner;

            //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCalPos_FineCameraPosition.Text = Equipment.MappingData_FilePath_StageCal_FineCam;
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

                Equipment.stCommunicationSet[m_nIndex].Connect = radioButton_Setup_Communication_Comm_Connect.Checked ? true : false;

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


                //  Not Connect, Connect                                                                            //  0 : Not Connect,    1 : Connect
                NativeMethods.WritePrivateProfileString(strTemp, "ConnectType", Equipment.stCommunicationSet[i].Connect.ToString(), strFIle);


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

            if (Equipment.stCommunicationSet[m_nIndex].Connect)
            {
                radioButton_Setup_Communication_Comm_Connect.Checked = true;
            }
            else
            {
                radioButton_Setup_Communication_Comm_Disconnect.Checked = true;
            }
        }

        private void button_Test_SocketConnect_Click(object sender, EventArgs e)
        {
            workStage.m_SocketLaser.Close();
        }

        private void button_Setup_Comm_ShowCommTerminal_Click(object sender, EventArgs e)
        {
            int m_nIndex = listBox_Setup_Communication_SelectUnit.SelectedIndex;

            if (m_nIndex < 0)
            {
                return;
            }

            //  선택된 Comm. Terminal 창을 띄운다.

            if (m_formCommTerminal == null)
            {
                m_formCommTerminal.CreateCommTerminal();
            }

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name == m_formCommTerminal.Name)
                {
                    m_formCommTerminal.m_nSelectedUnitIndex = m_nIndex;

                    if (Equipment.stCommunicationSet[m_nIndex].Comm_Type == 0)                      //  TCP/IP
                    {
                        m_formCommTerminal.m_strSocketIP = Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress;
                        m_formCommTerminal.m_nSocketPort = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum;
                    }
                    else                                                                            //  RS232
                    {
                        m_formCommTerminal.m_nCommPort = Equipment.stCommunicationSet[m_nIndex].Serial_CommPort;
                    }

                    m_formCommTerminal.FormNew_CommunicationTerminal_TabRefresh();
                    openForm.BringToFront();
                    openForm.Show();
                    return;
                }
            }

            m_formCommTerminal.m_nSelectedUnitIndex = m_nIndex;

            if (Equipment.stCommunicationSet[m_nIndex].Comm_Type == 0)                      //  TCP/IP
            {
                m_formCommTerminal.m_strSocketIP = Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress;
                m_formCommTerminal.m_nSocketPort = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum;
            }
            else                                                                            //  RS232
            {
                m_formCommTerminal.m_nCommPort = Equipment.stCommunicationSet[m_nIndex].Serial_CommPort;
            }

            m_formCommTerminal.Show();
        }

        private void button_Setup_Option_Save_Click(object sender, EventArgs e)
        {
            //  현재 Laser Type 을 저장한다.
            bool m_bCurrentLaserType = false;
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);


            //  체크 포인트
            if (((Convert.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text) != 0.0) || (Convert.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text) != 0.0)) &&
                ((Convert.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text) != 0.0) || (Convert.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text) != 0.0)))
            {
                MessageBox.Show("\"Offset Distance for Coordinate Matching\" 과\r\n\"Offset Distance to the Center of the Scanner\" 두 그룹 전체에 값이 들어가면 안됩니다.\n\r\n[두 그룹 중 한쪽에만 값이 들어가거나, 모두 0 이어야 합니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine Option 파일이 없습니다.\r\n\r\n[Default 값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }


            //  Machine Option  로드

            //  Laser Type                                                                            //  True : CO₂,    False : UV
            NativeMethods.GetPrivateProfileString("Machine_Option", "Laser_Type", "True", temp, 255, strFIle);
            m_bCurrentLaserType = temp.ToString() == "False" ? false : true;

            //  현재 선택한 Laser Type 을 저장하고,
            Machine_Option_Save();

            //  현재 선택한 Laser Type 과 비교한 후, 다르면 프로그램을 재시작 하도록 한다.
            if (m_bCurrentLaserType != radioButton_Setup_Option_LaserType_CO2.Checked)
            {
                MessageBox.Show("Machine Option 을 변경하였습니다.\r\n\r\n프로그램을 재시작해야 합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Machine_Option_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Machine Option 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //  Machine Name
            Equipment.Machine_Name = textBox_ModelName.Text;
            NativeMethods.WritePrivateProfileString("Machine_Option", "Machine_Name", textBox_ModelName.Text, strFIle);

            //  Laser Type
            Equipment.Machine_LaserType_CO2 = radioButton_Setup_Option_LaserType_CO2.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "Laser_Type", radioButton_Setup_Option_LaserType_CO2.Checked.ToString(), strFIle);

            //  Options
            Equipment.Machine_Door_Enable = checkBox_Setup_Option_DoorEnable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "Door_Enable", checkBox_Setup_Option_DoorEnable.Checked.ToString(), strFIle);
            Equipment.Machine_VacuumSensor_Enable = checkBox_Setup_Option_VacuumSensorEnable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumSensor_Enable", checkBox_Setup_Option_VacuumSensorEnable.Checked.ToString(), strFIle);
            Equipment.Machine_SignalHoldTime = Convert.ToInt16(textBox_Setup_Option_SignalHoldTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumSignalHoldTime", textBox_Setup_Option_SignalHoldTime.Text, strFIle);
            Equipment.Machine_MAligner_ReleaseType = checkBox_Setup_Option_MAligner_ReleaseType.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "MAligner_ReleaseType", checkBox_Setup_Option_MAligner_ReleaseType.Checked.ToString(), strFIle);
            Equipment.Machine_MAligner_NarrowingDistance = Convert.ToDouble(textBox_Setup_Option_MAligner_NarrowingDistance.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "MAligner_NarrowingDistance", textBox_Setup_Option_MAligner_NarrowingDistance.Text, strFIle);
            Equipment.Machine_MAligner_WidenDistance = Convert.ToDouble(textBox_Setup_Option_MAligner_WidenDistance.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "MAligner_WidenDistance", textBox_Setup_Option_MAligner_WidenDistance.Text, strFIle);
            Equipment.Machine_VacuumStableTime_Enable = checkBox_Setup_Option_VacuumStableTime_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumStableTime_Enable", checkBox_Setup_Option_VacuumStableTime_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_VacuumStableTime = Convert.ToInt16(textBox_Setup_Option_VacuumStableTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumStableTime", textBox_Setup_Option_VacuumStableTime.Text, strFIle);
            Equipment.Machine_LaserHeightCheckStableTime_Enable = checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "LaserHeightCheckStableTime_Enable", checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_LaserHeightCheckStableTime = Convert.ToInt16(textBox_Setup_Option_LaserHeightCheckStableTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "LaserHeightCheckStableTime", textBox_Setup_Option_LaserHeightCheckStableTime.Text, strFIle);
            Equipment.Machine_FiducialMarkJudgementRange_Enable = checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "FiducialMarkJudgementRange_Enable", checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_FiducialMarkJudgementRange = Convert.ToDouble(textBox_Setup_Option_FiducialMarkJudgementRange.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "FiducialMarkJudgementRange", textBox_Setup_Option_FiducialMarkJudgementRange.Text, strFIle);
            Equipment.Machine_FiducialImageSave_Always = radioButton_Setup_Option_FiducialImageSave_Always.Checked ? true : false;
            NativeMethods.WritePrivateProfileString("Machine_Option", "FiducialImageSave_Always", radioButton_Setup_Option_FiducialImageSave_Always.Checked.ToString(), strFIle);


            //  Offset Distance
            Equipment.stOffsetDistance.FromScannerToFineCam.X = Convert.ToDouble(textBox_Setup_Option_Offset_ScannerFineCam_X.Text);
            Equipment.stOffsetDistance.FromScannerToFineCam.Y = Convert.ToDouble(textBox_Setup_Option_Offset_ScannerFineCam_Y.Text);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.X = Convert.ToDouble(textBox_Setup_Option_Offset_FineCamCoarseCam_X.Text);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y = Convert.ToDouble(textBox_Setup_Option_Offset_FineCamCoarseCam_Y.Text);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X = Convert.ToDouble(textBox_Setup_Option_Offset_FineCamLaserHeightSensor_X.Text);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y = Convert.ToDouble(textBox_Setup_Option_Offset_FineCamLaserHeightSensor_Y.Text);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_X", textBox_Setup_Option_Offset_ScannerFineCam_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_Y", textBox_Setup_Option_Offset_ScannerFineCam_Y.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_X", textBox_Setup_Option_Offset_FineCamCoarseCam_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_Y", textBox_Setup_Option_Offset_FineCamCoarseCam_Y.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_X", textBox_Setup_Option_Offset_FineCamLaserHeightSensor_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_Y", textBox_Setup_Option_Offset_FineCamLaserHeightSensor_Y.Text, strFIle);

            //  Scanner Head Offset
            Equipment.Scanner_HeadOffset_X = Convert.ToDouble(textBox_ScannerOffset_X.Text);
            Equipment.Scanner_HeadOffset_Y = Convert.ToDouble(textBox_ScannerOffset_Y.Text);
            Equipment.Scanner_HeadOffset_Angle = Convert.ToDouble(textBox_ScannerOffset_Angle.Text);
            NativeMethods.WritePrivateProfileString("ScannerHeadOffset", "Offset_X", textBox_ScannerOffset_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("ScannerHeadOffset", "Offset_Y", textBox_ScannerOffset_Y.Text, strFIle);
            NativeMethods.WritePrivateProfileString("ScannerHeadOffset", "Offset_Angle", textBox_ScannerOffset_Angle.Text, strFIle);

            //  Coordinate System Matching Offset
            Equipment.CoordinateMatchingOffset_X = Convert.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text);
            Equipment.CoordinateMatchingOffset_Y = Convert.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text);
            NativeMethods.WritePrivateProfileString("MachineCoordinateOffset", "Offset_X", textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("MachineCoordinateOffset", "Offset_Y", textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text, strFIle);

            //  Offset Distance from Stage to Scanner
            Equipment.StageOffset_forDrilling_X = Convert.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text);
            Equipment.StageOffset_forDrilling_Y = Convert.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text);
            NativeMethods.WritePrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_X", textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_Y", textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text, strFIle);

            //  Keyence Laser Height Sensor 기준값 설정
            Equipment.LaserHeightSensor_ReferenceValue_atVisionFocusPosition = Convert.ToDouble(textBox_Setup_Option_ReferenceValue_atVisionFocusPosition.Text);
            Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition = Convert.ToDouble(textBox_Setup_Option_ReferenceValue_atScannerFocusPosition.Text);
            NativeMethods.WritePrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Vision_Focus_Position", textBox_Setup_Option_ReferenceValue_atVisionFocusPosition.Text, strFIle);
            NativeMethods.WritePrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Scanner_Focus_Position", textBox_Setup_Option_ReferenceValue_atScannerFocusPosition.Text, strFIle);

            //  Scanner Calibration parameter
            Equipment.Scanner_Calibration_LaserFrequency = Convert.ToDouble(textBox_Setup_ScannerCal_LaserFrequency.Text);
            Equipment.Scanner_Calibration_LaserEnergy = Convert.ToDouble(textBox_Setup_ScannerCal_LaserEnergy.Text);
            Equipment.Scanner_Calibration_CrossMarkLength = Convert.ToDouble(textBox_Setup_ScannerCal_CrossMarkLength.Text);
            Equipment.Scanner_Calibration_LaserMarkSpeed = Convert.ToDouble(textBox_Setup_ScannerCal_MarkingSpeed.Text);
            Equipment.Scanner_Calibration_LaserJumpSpeed = Convert.ToDouble(textBox_Setup_ScannerCal_JumpSpeed.Text);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Frequency", textBox_Setup_ScannerCal_LaserFrequency.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Energy", textBox_Setup_ScannerCal_LaserEnergy.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "CrossMark_Length", textBox_Setup_ScannerCal_CrossMarkLength.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Marking_Speed", textBox_Setup_ScannerCal_MarkingSpeed.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Jump_Speed", textBox_Setup_ScannerCal_JumpSpeed.Text, strFIle);

            Equipment.Scanner_Calibration_srcFilePath = m_correction2DRtc.SourceCorrectionFile; // m_srcFile;
            Equipment.Scanner_Calibration_targetFilePath = m_correction2DRtc.TargetCorrectionFile;  // m_targetFile;
            Equipment.Scanner_Calibration_FieldSize = m_fieldSize;
            Equipment.Scanner_Calibration_rowInterval = m_correction2DRtc.RowInterval;  // m_rowInterval;
            Equipment.Scanner_Calibration_colInterval = m_correction2DRtc.ColInterval; //m_colInterval;
            Equipment.Scanner_Calibration_rowCount = m_correction2DRtc.Rows;   //m_row;
            Equipment.Scanner_Calibration_colCount = m_correction2DRtc.Cols;   //m_col;

            m_srcFile = Equipment.Scanner_Calibration_srcFilePath;
            m_targetFile = Equipment.Scanner_Calibration_targetFilePath;
            m_rowInterval = Equipment.Scanner_Calibration_rowInterval;
            m_colInterval = Equipment.Scanner_Calibration_colInterval;
            m_row = Equipment.Scanner_Calibration_rowCount;
            m_col = Equipment.Scanner_Calibration_colCount;

            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "srcFilePath", m_srcFile, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "targetFilePath", m_targetFile, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "FieldSize", m_fieldSize.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "rowInterval", m_rowInterval.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "colInterval", m_colInterval.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "rowCount", m_row.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "colCount", m_col.ToString(), strFIle);

            MessageBox.Show("Machine Option 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnScannerOffset_Set_Click(object sender, EventArgs e)
        {
            Vector3 ScannerOffset = new Vector3(0, 0, 0);

            ScannerOffset.X = (float)Convert.ToDouble(textBox_ScannerOffset_X.Text);
            ScannerOffset.Y = (float)Convert.ToDouble(textBox_ScannerOffset_Y.Text);
            ScannerOffset.Z = (float)Convert.ToDouble(textBox_ScannerOffset_Angle.Text);

            workStage.rtc.PrimaryHeadBaseOffset = ScannerOffset;
        }

        private void button_Setup_Tab2DMap_StageCenter_ScannerPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCenter_ScannerPosition.Text = filePath;
                }
            }
        }

        private void button_Setup_Tab2DMap_StageCenter_FineCameraPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCenter_FineCameraPosition.Text = filePath;
                }
            }
        }

        private void button_Setup_Tab2DMap_StageCalPos_ScannerPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCalPos_ScannerPosition.Text = filePath;
                }
            }
        }

        private void button_Setup_Tab2DMap_StageCalPos_FineCameraPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCalPos_FineCameraPosition.Text = filePath;
                }
            }
        }

        public void MapData_List_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("2D Mapping 데이터 리스트 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Equipment.MappingData_FilePath_Stage_Scanner = richTextBox_Setup_Tab2DMap_StageCenter_ScannerPosition.Text;
            Equipment.MappingData_FilePath_Stage_FineCam = richTextBox_Setup_Tab2DMap_StageCenter_FineCameraPosition.Text;
            Equipment.MappingData_FilePath_StageCal_Scanner = richTextBox_Setup_Tab2DMap_StageCalPos_ScannerPosition.Text;
            Equipment.MappingData_FilePath_StageCal_FineCam = richTextBox_Setup_Tab2DMap_StageCalPos_FineCameraPosition.Text;
            
            
            //  Stage Center 가 Scanner 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCenter_ScannerCenter", Equipment.MappingData_FilePath_Stage_Scanner, strFIle);

            //  Stage Center 가 Fine Camera 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCenter_FineCameraCenter", Equipment.MappingData_FilePath_Stage_FineCam, strFIle);

            //  Stage Calibration 위치가 Scanner 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCalPos_ScannerCenter", Equipment.MappingData_FilePath_StageCal_Scanner, strFIle);

            //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCalPos_FineCameraCenter", Equipment.MappingData_FilePath_StageCal_FineCam, strFIle);


            MessageBox.Show("2D Mapping 데이터 리스트 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MapDataStatus_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("2D Mapping 데이터 리스트 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //  활성화 여부
            NativeMethods.WritePrivateProfileString("MapData", "Activate", Equipment.MapDataStatus_Activate.ToString(), strFIle);
        }

        private void button_Setup_2DMapping_Save_Click(object sender, EventArgs e)
        {
            MapData_List_Save();
        }

        private void button_Setup_ScannerFineCamOffsetChange_ImageDisplay_Show_Click(object sender, EventArgs e)
        {
            //  Scanner 와 Fine Camera 간의 Offset 값을 변경한다.

            Equipment.m_bVisionFormOpenMode_ScannerFineCamOffsetChange = true;

            if (m_formVisionPopup == null)
            {
                m_formVisionPopup.CreateSiriusEditor();
            }

            foreach (Form openForm in System.Windows.Forms.Application.OpenForms)
            {
                if (openForm.Name == m_formVisionPopup.Name)
                {
                    openForm.BringToFront();
                    openForm.Show();
                    return;
                }
            }

            m_formVisionPopup.Show();
        }

        private void button_Setup_2DMapData_Apply_Click(object sender, EventArgs e)
        {
            //  맵 데이터 적용

            workStage.MapData_Load();

            Equipment.MapDataStatus_Activate = !Equipment.MapDataStatus_Activate;

            if (Equipment.MapDataStatus_Activate)
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Activated";
                button_Setup_2DMapData_Apply.BackColor = Color.Lime;

                workStage.Stage.Config.Use2DMap = true;

                MessageBox.Show("2D Mapping 데이터 활성화", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Deactivated";
                button_Setup_2DMapData_Apply.BackColor = Color.LightGray;

                workStage.Stage.Config.Use2DMap = false;

                MessageBox.Show("2D Mapping 데이터 비활성화", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            MapDataStatus_Save();
        }

        private void FormNew_Setup_Shown(object sender, EventArgs e)
        {
            if (Equipment.Machine_Door_Enable)
            {
                checkBox_Setup_Option_DoorEnable.Checked = true;
            }
            else
            {
                checkBox_Setup_Option_DoorEnable.Checked = false;
            }

            textBox_Setup_Option_SignalHoldTime.Text = Equipment.Machine_SignalHoldTime.ToString();

            if (Equipment.Machine_VacuumSensor_Enable)
            {
                checkBox_Setup_Option_VacuumSensorEnable.Checked = true;
                textBox_Setup_Option_SignalHoldTime.Enabled = false;
            }
            else
            {
                checkBox_Setup_Option_VacuumSensorEnable.Checked = false;
                textBox_Setup_Option_SignalHoldTime.Enabled = true;
            }

            if (Equipment.Machine_MAligner_ReleaseType)
            {
                checkBox_Setup_Option_MAligner_ReleaseType.Checked = true;
            }
            else
            {
                checkBox_Setup_Option_MAligner_ReleaseType.Checked = false;
            }

            textBox_Setup_Option_MAligner_NarrowingDistance.Text = Equipment.Machine_MAligner_NarrowingDistance.ToString();
            textBox_Setup_Option_MAligner_WidenDistance.Text = Equipment.Machine_MAligner_WidenDistance.ToString();

            if (Equipment.Machine_VacuumStableTime_Enable)
            {
                checkBox_Setup_Option_VacuumStableTime_Enable.Checked = true;
                textBox_Setup_Option_VacuumStableTime.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_VacuumStableTime_Enable.Checked = false;
                textBox_Setup_Option_VacuumStableTime.Enabled = false;
            }

            if (Equipment.Machine_LaserHeightCheckStableTime_Enable)
            {
                checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked = true;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked = false;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = false;
            }

            if (Equipment.Machine_FiducialMarkJudgementRange_Enable)
            {
                checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked = true;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked = false;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = false;
            }

            if (Equipment.Machine_FiducialImageSave_Always)
            {
                radioButton_Setup_Option_FiducialImageSave_Always.Checked = true;
            }
            else
            {
                radioButton_Setup_Option_FiducialImageSave_FailedToFind.Checked = true;
            }
        }

        private void checkBox_Setup_Option_VacuumSensorEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_VacuumSensorEnable.Checked)
            {
                Equipment.Machine_VacuumSensor_Enable = true;
                textBox_Setup_Option_SignalHoldTime.Enabled = false;
            }
            else
            {
                Equipment.Machine_VacuumSensor_Enable = false;
                textBox_Setup_Option_SignalHoldTime.Enabled = true;
            }
        }

        private void checkBox_Setup_Option_VacuumStableTime_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_VacuumStableTime_Enable.Checked)
            {
                Equipment.Machine_VacuumStableTime_Enable = true;
                textBox_Setup_Option_VacuumStableTime.Enabled = true;
            }
            else
            {
                Equipment.Machine_VacuumStableTime_Enable = false;
                textBox_Setup_Option_VacuumStableTime.Enabled = false;
            }
        }

        private void checkBox_Setup_Option_LaserHeightCheckStableTime_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked)
            {
                Equipment.Machine_LaserHeightCheckStableTime_Enable = true;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = true;
            }
            else
            {
                Equipment.Machine_LaserHeightCheckStableTime_Enable = false;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = false;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {

            String message = "D:\\SLD-200\\Log\\ScannerCalData\\ScanerCalData2025-04-17 23_46_09.dat";

            m_correction2DRtc.ConvertFromDatFile(message);
            



            m_correction2DRtcForm.RefreshData();

            m_correction2DRtc.Convert();


            //  Scanner Calibration Test

            //m_kfactor = m_correction2DRtc.KFactor;
            //m_row = m_correction2DRtc.Rows;
            //m_col = m_correction2DRtc.Cols;
            //m_rowInterval = m_correction2DRtc.RowInterval;
            //m_colInterval = m_correction2DRtc.ColInterval;
            //m_srcFile = m_correction2DRtc.SourceCorrectionFile;
            //m_targetFile = m_correction2DRtc.TargetCorrectionFile;

            //Vector2 position = new Vector2(0, 0);   //현재 장비 위치값 넣고..
            //Vector2 offset = new Vector2(0, 0);     //마크 찾은 위치값 넣어보자.

            //for (int x = 0; x < m_row; x++)
            //{
            //    for (int y = 0; y < m_col; y++)
            //    {
            //        position.X = 200.0f + (x * m_rowInterval);
            //        position.Y = 400.0f + (y * m_colInterval);
            //        offset.X = 0.12f + x;
            //        offset.Y = 0.23f + y;
            //        m_correction2DRtc.AddAbsolute(x, y, position, offset);
            //        //m_correction2DRtc.AddRelative(x, y, position, offset);
            //        //m_correction2DRtcForm.RefreshData();
            //    }
            //}

            //m_correction2DRtcForm.RefreshData();
            //m_correction2DRtc.Convert();


        }

        private void button_Setup_ScannerCal_Save_Click(object sender, EventArgs e)
        {
            //  Scanner Calibration Parameter 저장

            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);


            //  체크 포인트
            if (((Convert.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text) != 0.0) || (Convert.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text) != 0.0)) &&
                ((Convert.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text) != 0.0) || (Convert.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text) != 0.0)))
            {
                MessageBox.Show("\"Offset Distance for Coordinate Matching\" 과\r\n\"Offset Distance to the Center of the Scanner\" 두 그룹 전체에 값이 들어가면 안됩니다.\n\r\n[두 그룹 중 한쪽에만 값이 들어가거나, 모두 0 이어야 합니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine Option 파일이 없습니다.\r\n\r\n[Default 값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            // Scanner Form Update :: 장비에서 TEST 필요.
            //m_correction2DRtc.Rows = m_row;
            //m_correction2DRtc.Cols = m_col;



            //m_kfactor = (float)Math.Pow(2, 20) / m_fieldSize;
            //m_correction2DRtc = new Correction2DRtc(m_kfactor, m_correction2DRtc.Rows, m_correction2DRtc.Cols,
            //                                        m_correction2DRtc.RowInterval, m_correction2DRtc.ColInterval, 
            //                                        m_correction2DRtc.SourceCorrectionFile, m_correction2DRtc.TargetCorrectionFile);
            //m_correction2DRtcForm = new Correction2DRtcForm(m_correction2DRtc);

            //m_correction2DRtcForm.TopLevel = false; // 폼을 최상위 폼이 아니도록 설정
            //m_correction2DRtcForm.FormBorderStyle = FormBorderStyle.None; // 폼의 테두리를 제거
            //m_correction2DRtcForm.Dock = DockStyle.Fill; // 폼을 패널에 맞게 채움

            //this.textBox_ScannerCal_LaserFrequency.Controls.Add(m_correction2DRtcForm);
            //m_correction2DRtcForm.Show();

            //  Scanner Calibration 관련 파라미터 저장
            Machine_Option_Save();

        }

        private void btnCalStart_Click(object sender, EventArgs e)
        {
            string m_strTemp = "";

            //TEST
            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.EqpSiriusViewer == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.m_nScanner_Calibration_Step == (int)WorkStage.ScannerCalibration_Step.None)
            {
                workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.Start;
                workStage.timer_ScannerCalibration.Enabled = true;
                
                WorkStartTick = Environment.TickCount;
            }
        }

        private void btnCalStop_Click(object sender, EventArgs e)
        {
            Equipment.AutoRunStatus = false;
            workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.None;
            workStage.timer_ScannerCalibration.Enabled = false;

        }

        private void btnCalStart_Vision_Click(object sender, EventArgs e)
        {
            string m_strTemp = "";

            //TEST
            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.EqpSiriusViewer == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.m_nScanner_Calibration_Step == (int)WorkStage.ScannerCalibration_Step.None)
            {
                workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.ScannerCompensation_StartPosition_Set;
                workStage.timer_ScannerCalibration.Enabled = true;

                WorkStartTick = Environment.TickCount;
            }
        }

        private QMCFindLenzCenter m_correctionDataList;
        // CorrectionData를 처리하는 메서드
        public void ProcessCorrectionData(QMCFindLenzCenter correctionDataList)
        {
            int indexX = 0, indexY = 0;
           
            foreach (var data in correctionDataList.SldData)
            {
                // CorrectionData에서 필요한 값을 추출하여 AddAbsolute 호출
                indexX = data.m_nIndexX;
                indexY = data.m_nindexY;
                //indexX = (int)data.m_dX;
                //indexY = (int)data.m_dY;


                //Vector2 position = new Vector2((float)data.m_dMeasureX, (float)data.m_dMeasureY);   //현재 장비 위치값 넣고..
                //Vector2 offset = new Vector2((float)data.m_dOffsetX, (float)data.m_dOffsetY);   //현재 장비 위치값 넣고..


                Vector2 position = new Vector2((float)data.m_dX, (float)data.m_dY);   //현재 장비 위치값 넣고..
                Vector2 offset = new Vector2((float)data.m_dMeasureX, (float)data.m_dMeasureY);   //현재 장비 위치값 넣고..

                m_correction2DRtc.AddAbsolute(indexX, indexY, position, offset);
                //m_correction2DRtc.AddRelative(indexY, indexX, position, offset);
            }


            m_correction2DRtcForm.RefreshData();
            m_correction2DRtc.Convert();
            m_correctionDataList = correctionDataList;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            m_correction2DRtc.Clear();
            
            //if(m_correctionDataList != null)
            //{
            //    ProcessCorrectionData(m_correctionDataList);
            //}

        }

        private void checkBox_Setup_Option_FiducialMarkJudgementRange_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked)
            {
                Equipment.Machine_FiducialMarkJudgementRange_Enable = true;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = true;
            }
            else
            {
                Equipment.Machine_FiducialMarkJudgementRange_Enable = false;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = false;
            }
        }
    }
}
