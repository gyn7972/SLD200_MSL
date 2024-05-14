using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using System.Windows;
using Microsoft.Win32;
using QMC.Core;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Hmi;
using QMC.Common.Vision;
using SpiralLab.Sirius;
using QMC.Process.WaferProbeAlign.Parts;
using Point = System.Drawing.Point;
using ACS.SPiiPlusNET;
using QMC.Common.Vision.Cameras;
using System.Globalization;
using OpenCvSharp;
using QMC.Common.Motion.ACS.Motions;
using static QMC.Common.Modules.WaferProbeAlign;
using System.Threading;
using System.Diagnostics.Contracts;
using QMC.Common.VisionPart;
using static FASTECH.EziMOTIONPlusELib;
using System.Diagnostics.Eventing.Reader;
using MessageBox = System.Windows.Forms.MessageBox;
using QMC.Vision;
using static QMC.Common.Vision.EureSys.GenICam;
using Size = System.Drawing.Size;

namespace CWA150SA_Onsemi300
{
    public partial class CalibrationMode_CWA150SA : UserControl
    {
        //protected ModuleStateControl m_ModuleStateControl;
        private IlluminatorControl m_IlluminatorControl;
        //public ModulePositionControl m_WaferProbeAlignPosControl;
        public JogControl m_JogControl;

        private FormUserRegistration m_UserRegistration_CWA150SA;

        //protected VisionImageViewer m_visionImageViewer_Upper;
        //protected VisionImageViewer m_visionImageViewer_Lower;

        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        private NeedleCalibrationJogControl m_NeedleCalibrationJogControl;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }

        public Scanner Scanner { get; set; }

        static WaferProbeAlign waferProbeAlign;
        private string itemSelected;

        private Thread m_calibrationModeThread;
        private bool m_bCalibrationModeThreadExit;

        string strRegistryTmp = "SOFTWARE\\";       //  레지스트리 최상위 폴더 지정
        string strAppName = "CWA150SA_Onsemi";

        MotionFunction MC_Func = new MotionFunction();

        public System.Windows.Forms.Timer timer_Laser1Shot;
        //public System.Windows.Forms.Timer timer_ACS_Status;
        public System.Windows.Forms.Timer timer_Motion_Status;

        private object m_objReadVar = null;
        private Array m_arrReadVector = null;
        private const int MAX_UI_LIMIT_CNT = 8;
        private int m_nTotalAxis = 0;

        public bool m_b1time = false;

        private bool m_bRVA_X_Neg_MouseDown = false;
        private bool m_bRVA_Z_Neg_MouseDown = false;
        private bool m_bRVA_X_Pos_MouseDown = false;
        private bool m_bRVA_Z_Pos_MouseDown = false;

        protected XyztStage m_Stage;
        private _2DMappingDataControl m__2DMappingDataControl;


        private int m_nBlink = 0;
        private bool m_bBlink = false;

        #region Tick Count Check
        //System.Diagnostics.Stopwatch sw_User = new System.Diagnostics.Stopwatch();

        public enum TickType : int
        {
            TICK_USER = 0,          //  0 : User
            TICK_USER2 = 1,         //  0 : User2
            TICK_USER3 = 2,         //  0 : User3
        }

        public int[,] TickCount_Cycle = new int[10, 2];          //  0 : User
                                                                 //  1 : 
                                                                 //  2 : 

        public void TickCount_Start(int m_nIndex)
        {
            TickCount_Cycle[m_nIndex, 0] = Environment.TickCount;
        }
        public int TickCount_Elapsed(int m_nIndex)
        {
            int TickCount_Elapsed = 0;
            TickCount_Cycle[m_nIndex, 1] = Environment.TickCount;
            TickCount_Elapsed = TickCount_Cycle[m_nIndex, 1] - TickCount_Cycle[m_nIndex, 0];

            return TickCount_Elapsed;
        }

        public double m_d1ShotTime = 0.0;
        public int m_nTick_1ShotStart = 0;
        public int m_nTick_1Shot_Elapsed = 0;

        #endregion


        #region Registry 읽기 쓰기 삭제

        public bool WriteRegistry(string strAppName, string strSubKey, string strKey, string strValue)
        {
            RegistryKey rkReg = Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName, true);

            //  null 이면 폴더(레지스트리)가 없으므로 만든다.
            if (rkReg == null) rkReg = Registry.CurrentUser.CreateSubKey(strRegistryTmp + strAppName);

            //  OpenSubKey (하위 폴더(레지스트리 이름), 쓰기 선택 True 쓰기 False 및 인자가 없다면 읽기)
            RegistryKey rkSub = Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName + "\\" + strSubKey, true);

            if (rkSub == null) rkSub = Registry.CurrentUser.CreateSubKey(strRegistryTmp + strAppName + "\\" + strSubKey);

            try
            {
                rkSub.SetValue(strKey, strValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }

            return true;
        }

        public string ReadRegistry(string strAppName, string strSubKey, string strKey)
        {
            RegistryKey reg;

            try
            {
                if (Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName).OpenSubKey(strSubKey) == null)
                {
                    WriteRegistry(strAppName, strSubKey, strKey, "0");
                }

                reg = Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName).OpenSubKey(strSubKey);
            }                                                                                                                                      
            catch (Exception ex)
            {
                return "0";
            }

            return reg.GetValue(strKey, "0").ToString();
        }

        public bool DeleteRegistry(string strSubKey)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(strRegistryTmp, true);

            try
            {      
                //  하위 폴더(레즈스트리)가 있으면 삭제 안됨.
                //  if (rk != null) rk.DeleteSubKey( strSubKey ) ;

                //  하위 폴더(레즈스트리)가 있어도 삭제.
                if (rk != null) rk.DeleteSubKeyTree(strSubKey);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion


        //  Test용 변수
        bool m_bRun;


        public FormUserRegistration userRegistration
        {
            get { return this.m_UserRegistration_CWA150SA; }
            set { this.m_UserRegistration_CWA150SA = value; }
        }


        public CalibrationMode_CWA150SA()
        {
            InitializeComponent();

            Scanner = new Scanner("Scanner");

            AxisList = new List<MotionAxis>();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            //m_ModuleStateControl = new ModuleStateControl(waferProbeAlign);

            //m_ModuleStateControl.Location = new Point(5, 0);
            //m_ModuleStateControl.Size = new System.Drawing.Size(175, 500);
            //this.Controls.Add(this.m_ModuleStateControl);

            //m_visionImageViewer_Upper = new VisionImageViewer();
            //m_visionImageViewer_Lower = new VisionImageViewer();

            //m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            //m_visionImageViewer_Upper.SuspendDisplay();
            //m_visionImageViewer_Upper.Location = new Point(m_ModuleStateControl.Location.X + m_ModuleStateControl.Size.Width + 10, m_ModuleStateControl.Location.Y);
            ////Size VisionImageViewerSize = new Size(442, 370);        //  (360, 301)      // (442, 370);     // (400, 336)      //   650, 544);     //  2448 x 2048 비율
            ////m_visionImageViewer_Upper.Size = VisionImageViewerSize;   //  Configuration.VisionImageViewerSize;
            //this.Controls.Add(m_visionImageViewer_Upper);

            // 
            // lblUpperCamera
            // 
            //this.baseLabel_ProbeCard_Camera.Location = new System.Drawing.Point(m_visionImageViewer_Upper.Location.X + m_visionImageViewer_Upper.Size.Width + 1, m_visionImageViewer_Upper.Location.Y);
            //this.baseLabel_ProbeCard_Camera1.Location = new System.Drawing.Point(baseLabel_ProbeCard_Camera.Location.X, baseLabel_ProbeCard_Camera.Location.Y + baseLabel_ProbeCard_Camera.Size.Height - 1);
            //this.lblUpperCamera.Size = new System.Drawing.Size(76, 40);
            //this.Controls.Add(lblUpperCamera);

            //m_visionImageViewer_Lower.SizeMode = PictureBoxSizeMode.CenterImage;
            //m_visionImageViewer_Lower.SuspendDisplay();
            //m_visionImageViewer_Lower.Location = new Point(baseLabel_ProbeCard_Camera.Location.X + baseLabel_ProbeCard_Camera.Size.Width + 10, m_visionImageViewer_Upper.Location.Y);
            //m_visionImageViewer_Lower.Size = VisionImageViewerSize;   //  Configuration.VisionImageViewerSize;
            //this.Controls.Add(m_visionImageViewer_Lower);

            // 
            // lblLowerCamera
            // 
            //this.baseLabel_WaferChuck_Camera.Location = new System.Drawing.Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 1, m_visionImageViewer_Lower.Location.Y);
            //this.baseLabel_WaferChuck_Camera1.Location = new System.Drawing.Point(baseLabel_WaferChuck_Camera.Location.X, baseLabel_WaferChuck_Camera.Location.Y + baseLabel_WaferChuck_Camera.Size.Height - 1);
            //this.lblLowerCamera.Size = new System.Drawing.Size(76, 40);
            //this.Controls.Add(lblLowerCamera);

            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Lower.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Lower.SuspendDisplay();

            this.m_visionImageViewer_Upper.Camera = waferProbeAlign.Camera_Upper;
            this.m_visionImageViewer_Lower.Camera = waferProbeAlign.Camera_Lower;

            this.VisibleChanged += CalibrationMode_CWA150SA_VisibleChanged;


            //  User Position Move
            //groupBoxPosMoveParameter.Location = new Point(m_visionImageViewer_Upper.Location.X, m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Size.Height + 20);

            //  Jog Control
            m_JogControl = new JogControl(waferProbeAlign.Stage);            
            //m_JogControl.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 30, m_visionImageViewer_Lower.Location.Y);
            m_JogControl.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 30, baseLabel_WaferChuck_Camera1.Location.Y);
            this.Controls.Add(m_JogControl);

            //m_NeedleCalibrationJogControl = new NeedleCalibrationJogControl();
            //m_NeedleCalibrationJogControl.Location = new Point(visionImageViewer.Location.X + visionImageViewer.Size.Width + 10, visionImageViewer.Location.Y);

            //  조명
            this.m_IlluminatorControl = new IlluminatorControl(waferProbeAlign.visionCalibrator_Upper.Recipe.IlluminationDataSet.ToList());
            this.m_IlluminatorControl.Location = new Point(this.groupBoxPosMoveParameter.Location.X + groupBoxPosMoveParameter.Size.Width + 22, groupBoxPosMoveParameter.Location.Y);
            this.m_IlluminatorControl.Illuminator = waferProbeAlign.visionCalibrator_Upper.Illuminator;
            this.m_IlluminatorControl.IlluminatorControlButton_Click += m_IlluminatorControl_IlluminatorControlButton_Click;
            this.Controls.Add(m_IlluminatorControl);

            m_nTotalAxis = 4;

            ////  ACS Motion 의 상태를 갱신하는 타이머
            //timer_ACS_Status = new System.Windows.Forms.Timer();
            //timer_ACS_Status.Interval = 30;
            //timer_ACS_Status.Tick += new System.EventHandler(Timer_ACSMotion_StatusFunc);
            //timer_ACS_Status.Enabled = false;

            //  ACS and Ajin Motion 의 상태를 갱신하는 타이머
            timer_Motion_Status = new System.Windows.Forms.Timer();
            timer_Motion_Status.Interval = 50;
            timer_Motion_Status.Tick += new System.EventHandler(Timer_Motion_StatusFunc);
            timer_Motion_Status.Enabled = true;

            //  Laser 1 Shot 을 위한 타이머
            timer_Laser1Shot = new System.Windows.Forms.Timer();
            timer_Laser1Shot.Interval = 10;
            timer_Laser1Shot.Tick += new System.EventHandler(Timer_Laser1ShotFunc);
            timer_Laser1Shot.Enabled = false;

            //  Thread Start
            //ThreadStart();

            //  User Position
            tb_Axis_U1.Text = ReadRegistry("CWA-150SA", "UserPos1", "UVW_U");
            tb_Axis_V1.Text = ReadRegistry("CWA-150SA", "UserPos1", "UVW_V");
            tb_Axis_W1.Text = ReadRegistry("CWA-150SA", "UserPos1", "UVW_W");
            tb_Axis_ElevZ1.Text = ReadRegistry("CWA-150SA", "UserPos1", "ElevZ");
            tb_Axis_VisionX1.Text = ReadRegistry("CWA-150SA", "UserPos1", "Vis_X");
            tb_Axis_VisionY1.Text = ReadRegistry("CWA-150SA", "UserPos1", "Vis_Y");
            tb_Axis_VisionZ1.Text = ReadRegistry("CWA-150SA", "UserPos1", "Vis_Z");

            tb_Axis_U2.Text = ReadRegistry("CWA-150SA", "UserPos2", "UVW_U");
            tb_Axis_V2.Text = ReadRegistry("CWA-150SA", "UserPos2", "UVW_V");
            tb_Axis_W2.Text = ReadRegistry("CWA-150SA", "UserPos2", "UVW_W");
            tb_Axis_ElevZ2.Text = ReadRegistry("CWA-150SA", "UserPos2", "ElevZ");
            tb_Axis_VisionX2.Text = ReadRegistry("CWA-150SA", "UserPos2", "Vis_X");
            tb_Axis_VisionY2.Text = ReadRegistry("CWA-150SA", "UserPos2", "Vis_Y");
            tb_Axis_VisionZ2.Text = ReadRegistry("CWA-150SA", "UserPos2", "Vis_Z");

            tb_Axis_U3.Text = ReadRegistry("CWA-150SA", "UserPos3", "UVW_U");
            tb_Axis_V3.Text = ReadRegistry("CWA-150SA", "UserPos3", "UVW_V");
            tb_Axis_W3.Text = ReadRegistry("CWA-150SA", "UserPos3", "UVW_W");
            tb_Axis_ElevZ3.Text = ReadRegistry("CWA-150SA", "UserPos3", "ElevZ");
            tb_Axis_VisionX3.Text = ReadRegistry("CWA-150SA", "UserPos3", "Vis_X");
            tb_Axis_VisionY3.Text = ReadRegistry("CWA-150SA", "UserPos3", "Vis_Y");
            tb_Axis_VisionZ3.Text = ReadRegistry("CWA-150SA", "UserPos3", "Vis_Z");


            PositionData_Show();


            //m_Stage = waferProbeAlign.Stage;
            //this.m__2DMappingDataControl = new _2DMappingDataControl(m_Stage);
            //this.m__2DMappingDataControl.Location = new Point(200, 200);
            //this.Controls.Add(this.m__2DMappingDataControl);


            this.userRegistration = new FormUserRegistration();
        }


        private void PositionData_Show()
        {
            int m_nIndex_Ver_Top = -1;
            int m_nIndex_Ver_Mid = -1;
            int m_nIndex_Ver_Bot = -1;
            int m_nIndex_Hor_L = -1;
            int m_nIndex_Hor_C = -1;
            int m_nIndex_Hor_R = -1;
            int m_nIndex_Reticle_Upper = -1;
            int m_nIndex_Reticle_Lower = -1;
            int m_nIndex_Packing = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Ver_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Ver_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Ver_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (LEFT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Hor_L = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (CENTER)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Hor_C = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (RIGHT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Hor_R = i;
                }

                //  Reticle 얼라인 상태를 확인하는 위치 (Upper Cam)
                if (waferProbeAlign.Config.Positions[i].Name == "ReticleGlass_UpperCamera")
                {
                    m_nIndex_Reticle_Upper = i;
                }

                //  Reticle 얼라인 상태를 확인하는 위치 (Lower Cam)
                if (waferProbeAlign.Config.Positions[i].Name == "ReticleGlass_LowerCamera")
                {
                    m_nIndex_Reticle_Lower = i;
                }

                //  Packing 할 때의 Elev. Z 위치
                if (waferProbeAlign.Config.Positions[i].Name == "ProbeWafer_Packing")
                {
                    m_nIndex_Packing = i;
                }
            }

            //  좌표 가져오기
            if (m_nIndex_Ver_Top != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                tb_Axis_Y_TOP.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y].ToString();
            }
            if (m_nIndex_Ver_Mid != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Middle");

                tb_Axis_Y_MID.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y].ToString();
            }
            if (m_nIndex_Ver_Bot != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");

                tb_Axis_Y_BOT.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y].ToString();
            }
            if (m_nIndex_Hor_L != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Left");

                tb_Axis_X_LEFT.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X].ToString();
            }
            if (m_nIndex_Hor_C != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Center");

                tb_Axis_X_CENTER.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X].ToString();
            }
            if (m_nIndex_Hor_R != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Right");

                tb_Axis_X_RIGHT.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X].ToString();
            }

            if (m_nIndex_Reticle_Upper != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");

                tb_Axis_U_ReticleCenter.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U].ToString();
                tb_Axis_V_ReticleCenter.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V].ToString();
                tb_Axis_W_ReticleCenter.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W].ToString();

                tb_Axis_X_ReticleCenter.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X].ToString();
                tb_Axis_Y_ReticleCenter.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y].ToString();
                tb_Axis_Z_ReticleCenter.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ].ToString();

                tb_Axis_EZ_ReticleCenter_UpperCam.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ].ToString();
            }

            if (m_nIndex_Reticle_Lower != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                tb_Axis_EZ_ReticleCenter_LowerCam.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ].ToString();
            }

            if (m_nIndex_Packing != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                tb_Axis_EZ_Wafer_ProbeCard_Packing.Text = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ].ToString();
            }
        }


        private void CalibrationMode_CWA150SA_VisibleChanged(object sender, EventArgs e)
        {
            if (m_visionImageViewer_Upper != null)
            {
                if (this.Visible == true)
                {
                    m_visionImageViewer_Upper.StartUpdateTask();
                    //AddOverlay(visionImageViewer);
                }
                else
                {
                    m_visionImageViewer_Upper.StopUpdateTask();
                }
            }

            if (m_visionImageViewer_Lower != null)
            {
                if (this.Visible)
                {
                    m_visionImageViewer_Lower.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer_Lower.StopUpdateTask();
                }
            }
        }

        private void m_IlluminatorControl_IlluminatorControlButton_Click(IlluminatorControl.ButtonType type)
        {
            if (type == IlluminatorControl.ButtonType.Save)
            {
                //Module module = waferProbeAlign m_Owner.Owner as Module;               

                //string m_strRecipe = "";
                //RecipeInfo m_recipeInfo = new RecipeInfo();
                //m_recipeInfo = Equipment.GetCurrentRecipe();

                //if (m_recipeInfo != null)
                //{
                //    m_strRecipe = m_recipeInfo.Name;

                //    DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                //    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                //}
                //else
                //{
                //    DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                //    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //}

                //DataManager.Instance.ApplyConfigData(waferProbeAlign);

                ////  Config 창 데이터 갱신을 위해서
                //Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

                //Equipment.UpdateRecipeData();
                //Equipment.SaveRecipe();
            }
            else if (type == IlluminatorControl.ButtonType.AllOff)
            {

            }
            else { }
        }

        #region Laser 1 Shot (Time) Function

        void Timer_Laser1ShotFunc(object sender, EventArgs e)
        {
            //Run_Laser1ShotTime_Func();
        }

        #endregion


        #region Thread

        public void ThreadStart()
        {
            //  Drilling Cycle Thread
            m_bCalibrationModeThreadExit = false;
            m_calibrationModeThread = new Thread(new ThreadStart(OnCalibrationModeStatusCycle));
            m_calibrationModeThread.Start();
        }

        public void ThreadStop()
        {
            m_bCalibrationModeThreadExit = true;

            if (m_calibrationModeThread != null)
                m_calibrationModeThread.Join();
        }

        protected void OnCalibrationModeStatusCycle()
        {
            while (true)
            {
                if (m_bCalibrationModeThreadExit)
                {
                    break;
                }
                if (OnCalibrationModeStatusRun() != 0) break;
                Thread.Sleep(1);
            }
        }

        protected int OnCalibrationModeStatusRun()
        {
            int ret = 0;

            //ACSMotion_Status();             //  ACS Motion Status

            return ret;
        }
        #endregion
                    
        void Timer_Motion_StatusFunc(object sender, EventArgs e)
        {
            //ACSMotion_Status();
            //AJINMotion_Status();

            //  카메라 라이브 상태인지 표시
            if ((waferProbeAlign.jigAligner_Upper != null) && (waferProbeAlign.jigAligner_Lower != null))
            {
                if (waferProbeAlign.jigAligner_Upper.Camera.IsLiveOn && waferProbeAlign.jigAligner_Lower.Camera.IsLiveOn)
                {
                    btnUpperCamera_StartLive.BackColor = Color.LightGreen;
                }
                else
                {
                    btnUpperCamera_StartLive.BackColor = Color.LightGray;
                }
            }
        }

        //private void ACSMotion_Status()
        //{
        //    double m_dPos = 0.0;
        //    int fault = 0;

        //    if (ACSSPiiPlusMotionBoard.Api.IsConnected)
        //    {
        //        //if (waferProbeAlign.Config.ParamConfig.DoorInterlock_Enable)                                               //  도어 인터락 활성화 상태이고,
        //        //{
        //        //    if (!waferProbeAlign.waferProbeAlignParameter.DI_Safety())                                                    //  문이 열려 있으면? --> RVA 축을 제외한 모든 축 ServoOff
        //        //    {
        //        //        //  ACS Motion
        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)                                             //  축이 Enable(Servo On) 상태이면? --> Disable(Servo Off)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //        }

        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //        }
        //        //    }
        //        //}
        //        //else if (waferProbeAlign.Config.ParamConfig.OpDoorInterlock_Enable)                                        //  OP 패널 도어 인터락 활성화 상태이고,
        //        //{
        //        //    if (!waferProbeAlign.waferProbeAlignParameter.DI_Safety())                                                    //  문이 열려 있으면? --> RVA 축을 제외한 모든 축 ServoOff
        //        //    {
        //        //        //  ACS Motion
        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)                                             //  축이 Enable(Servo On) 상태이면? --> Disable(Servo Off)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //        }

        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //        }
        //        //    }
        //        //}
        //        //else                                                                                                    //  도어 인터락 사용 안할 경우 --> 모든 축 ServoOn
        //        //{
        //        //    //  ACS Motion
        //        //    m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //    if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
        //        //    {
        //        //        ACSSPiiPlusMotionBoard.Api.Enable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        //    }

        //        //    m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //    if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
        //        //    {
        //        //        ACSSPiiPlusMotionBoard.Api.Enable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        //    }
        //        //}



        //        //m_dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].No);
        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        baseLabelX_Enc.Text = String.Format("{0:F3}", m_dPos);

        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        baseLabelY_Enc.Text = String.Format("{0:F3}", m_dPos);

        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
        //        baseLabelX_Ref.Text = String.Format("{0:F3}", m_dPos);

        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
        //        baseLabelY_Ref.Text = String.Format("{0:F3}", m_dPos);

        //        //  Read left/right hardware limits state
        //        //  ACSPL+ Variable : FAULT (integer)
        //        m_objReadVar = ACSSPiiPlusMotionBoard.Api.ReadVariableAsVector("FAULT", ProgramBuffer.ACSC_NONE, 0, m_nTotalAxis - 1, -1, -1);
        //        if (m_objReadVar != null)
        //        {
        //            m_arrReadVector = m_objReadVar as Array;
        //            if (m_arrReadVector != null)
        //            {
        //                for (int i = 0; i < m_nTotalAxis; i++)
        //                {
        //                    fault = (int)m_arrReadVector.GetValue(i);

        //                    if ((fault & (int)SafetyControlMasks.ACSC_SAFETY_LL) != 0)              //  NOT 
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WaferProbeAlignParameter.MotionKey.X: baseLabel_LimitCheck_X_Left.BackColor = Color.Red; break;
        //                            case (int)WaferProbeAlignParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Fwd.BackColor = Color.Red; break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WaferProbeAlignParameter.MotionKey.X: baseLabel_LimitCheck_X_Left.BackColor = Color.Maroon; break;
        //                            case (int)WaferProbeAlignParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Fwd.BackColor = Color.Maroon; break;
        //                        }
        //                    }

        //                    if ((fault & (int)SafetyControlMasks.ACSC_SAFETY_RL) != 0)              //  POT
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WaferProbeAlignParameter.MotionKey.X: baseLabel_LimitCheck_X_Right.BackColor = Color.Red; break;
        //                            case (int)WaferProbeAlignParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Bwd.BackColor = Color.Red; break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WaferProbeAlignParameter.MotionKey.X: baseLabel_LimitCheck_X_Right.BackColor = Color.Maroon; break;
        //                            case (int)WaferProbeAlignParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Bwd.BackColor = Color.Maroon; break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        baseLabelX_Enc.Text = "---.---";
        //        baseLabelY_Enc.Text = "---.---";

        //        baseLabelX_Ref.Text = "---.---";
        //        baseLabelY_Ref.Text = "---.---";
        //    }
        //}

        private void AJINMotion_Status()
        {
            double m_dPos = 0.0;
            int fault = 0;


        }

        #region buttonDownEvent

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion

        #region buttonUpEvent

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion


        private void baseButton_UVWStage_GO1_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            double lfTargetPos_U = 0.0;
            double lfTargetPos_V = 0.0;
            double lfTargetPos_W = 0.0;
            //double lfTargetPos_ElevZ = 0.0;
            //double lfTargetPos_X = 0.0;
            //double lfTargetPos_Y = 0.0;
            //double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_U = Convert.ToDouble(tb_Axis_U1.Text.Trim());
            lfTargetPos_V = Convert.ToDouble(tb_Axis_V1.Text.Trim());
            lfTargetPos_W = Convert.ToDouble(tb_Axis_W1.Text.Trim());
            //lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ1.Text.Trim());
            //lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX1.Text.Trim());
            //lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY1.Text.Trim());
            //lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ1.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "UVW Stage 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_UVWStage_GO2_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            double lfTargetPos_U = 0.0;
            double lfTargetPos_V = 0.0;
            double lfTargetPos_W = 0.0;
            //double lfTargetPos_ElevZ = 0.0;
            //double lfTargetPos_X = 0.0;
            //double lfTargetPos_Y = 0.0;
            //double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_U = Convert.ToDouble(tb_Axis_U2.Text.Trim());
            lfTargetPos_V = Convert.ToDouble(tb_Axis_V2.Text.Trim());
            lfTargetPos_W = Convert.ToDouble(tb_Axis_W2.Text.Trim());
            //lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ2.Text.Trim());
            //lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX2.Text.Trim());
            //lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY2.Text.Trim());
            //lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ2.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "UVW Stage 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_UVWStage_GO3_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            double lfTargetPos_U = 0.0;
            double lfTargetPos_V = 0.0;
            double lfTargetPos_W = 0.0;
            //double lfTargetPos_ElevZ = 0.0;
            //double lfTargetPos_X = 0.0;
            //double lfTargetPos_Y = 0.0;
            //double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_U = Convert.ToDouble(tb_Axis_U3.Text.Trim());
            lfTargetPos_V = Convert.ToDouble(tb_Axis_V3.Text.Trim());
            lfTargetPos_W = Convert.ToDouble(tb_Axis_W3.Text.Trim());
            //lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ3.Text.Trim());
            //lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX3.Text.Trim());
            //lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY3.Text.Trim());
            //lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ3.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "UVW Stage 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }        

        private void baseButton_XYZStage_GO1_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //double lfTargetPos_U = 0.0;
            //double lfTargetPos_V = 0.0;
            //double lfTargetPos_W = 0.0;
            //double lfTargetPos_ElevZ = 0.0;
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;
            double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            //lfTargetPos_U = Convert.ToDouble(tb_Axis_U1.Text.Trim());
            //lfTargetPos_V = Convert.ToDouble(tb_Axis_V1.Text.Trim());
            //lfTargetPos_W = Convert.ToDouble(tb_Axis_W1.Text.Trim());
            //lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ1.Text.Trim());
            lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX1.Text.Trim());
            lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY1.Text.Trim());
            lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ1.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "XYZ Stage 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_XYZStage_GO2_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //double lfTargetPos_U = 0.0;
            //double lfTargetPos_V = 0.0;
            //double lfTargetPos_W = 0.0;
            //double lfTargetPos_ElevZ = 0.0;
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;
            double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            //lfTargetPos_U = Convert.ToDouble(tb_Axis_U2.Text.Trim());
            //lfTargetPos_V = Convert.ToDouble(tb_Axis_V2.Text.Trim());
            //lfTargetPos_W = Convert.ToDouble(tb_Axis_W2.Text.Trim());
            //lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ2.Text.Trim());
            lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX2.Text.Trim());
            lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY2.Text.Trim());
            lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ2.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "XYZ Stage 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_XYZStage_GO3_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //double lfTargetPos_U = 0.0;
            //double lfTargetPos_V = 0.0;
            //double lfTargetPos_W = 0.0;
            //double lfTargetPos_ElevZ = 0.0;
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;
            double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            //lfTargetPos_U = Convert.ToDouble(tb_Axis_U3.Text.Trim());
            //lfTargetPos_V = Convert.ToDouble(tb_Axis_V3.Text.Trim());
            //lfTargetPos_W = Convert.ToDouble(tb_Axis_W3.Text.Trim());
            //lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ3.Text.Trim());
            lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX3.Text.Trim());
            lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY3.Text.Trim());
            lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ3.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "XYZ Stage 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }        

        private void baseLabel7_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel10_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            tb_Axis_ElevZ2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            tb_Axis_VisionX2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_VisionY2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_VisionZ2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            WriteRegistry("CWA-150SA", "UserPos2", "UVW_U", tb_Axis_U2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "UVW_V", tb_Axis_V2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "UVW_W", tb_Axis_W2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "ElevZ", tb_Axis_ElevZ2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "Vis_X", tb_Axis_VisionX2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "Vis_Y", tb_Axis_VisionY2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "Vis_Z", tb_Axis_VisionZ2.Text);
        }

        private void baseLabel13_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            tb_Axis_ElevZ3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            tb_Axis_VisionX3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_VisionY3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_VisionZ3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            WriteRegistry("CWA-150SA", "UserPos3", "UVW_U", tb_Axis_U3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "UVW_V", tb_Axis_V3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "UVW_W", tb_Axis_W3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "ElevZ", tb_Axis_ElevZ3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "Vis_X", tb_Axis_VisionX3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "Vis_Y", tb_Axis_VisionY3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "Vis_Z", tb_Axis_VisionZ3.Text);
        }

        private void btnMoveLoadingPos_Click(object sender, EventArgs e)
        {
            //  

            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            double m_dCurPos_Y = 0.0;
            double m_dCurPos_X = 0.0;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Center 를 카메라가 보고 있는 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X] = m_dCurPos_X + waferProbeAlign.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y] = m_dCurPos_Y + waferProbeAlign.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }

        private void btnMoveUnloadingPos_Click(object sender, EventArgs e)
        {
            //  

            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            double m_dCurPos_Y = 0.0;
            double m_dCurPos_X = 0.0;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 현재 Scanner Center 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X] = m_dCurPos_X - waferProbeAlign.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y] = m_dCurPos_Y - waferProbeAlign.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }

        private void btnCamMoveToScannerPos_Click(object sender, EventArgs e)
        {
            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            double m_dCurPos_Y = 0.0;
            double m_dCurPos_X = 0.0;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 현재 Scanner Center 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X] = m_dCurPos_X + waferProbeAlign.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y] = m_dCurPos_Y + waferProbeAlign.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }

        private void btnScannerMoveToCamPos_Click(object sender, EventArgs e)
        {
            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            double m_dCurPos_Y = 0.0;
            double m_dCurPos_X = 0.0;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Center 를 카메라가 보고 있는 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X] = m_dCurPos_X - waferProbeAlign.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y] = m_dCurPos_Y - waferProbeAlign.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }
     

        private void btnTest_AutoFocus_Click(object sender, EventArgs e)
        {
            if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
                return;
            }

            //  테스트 (오토 포커스)
            waferProbeAlign.autoFocuser_Upper.Work();

            waferProbeAlign.autoFocusResult_Upper = waferProbeAlign.autoFocuser_Upper.Result;

            MessageBox.Show(waferProbeAlign.autoFocusResult_Upper.BestFocusPosition.ToString());
        }

        private void btnStageServoOff_Click(object sender, EventArgs e)
        {
            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "RVA 축을 제외한 모든 축의 서보를 OFF 하시겠습니까?"))
                return;

            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    //  ACS Motion
            //    //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
            //    //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)                                             //  축이 Enable(Servo On) 상태이면? --> Disable(Servo Off)
            //    //{
            //    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
            //    //    ACSSPiiPlusMotionBoard.Api.Disable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
            //    //}

            //    //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //    //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
            //    //{
            //    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //    //    ACSSPiiPlusMotionBoard.Api.Disable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //    //}
            //}

            if (Equipment.AjinBoard_Opened)
            {
                //  Ajin Motion
                //if (MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ))                      //  축이 Servo On 상태이면? --> Servo Off
                //{
                //    MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ, 1000);
                //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ, false);
                //}

                //if (MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.ScannerZ))
                //{
                //    MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.ScannerZ, 1000);
                //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.ScannerZ, false);
                //}

                //if (MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.StageTH))
                //{
                //    MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.StageTH, 1000);
                //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.StageTH, false);
                //}
            }

            waferProbeAlign.m_bRVA_CalibMode = true;

            var mb2 = new MessageBoxOk();
            mb2.ShowDialog("Information !", "RVA 축을 제외한 모든 축의 서보가 OFF 되었습니다.");
            return;
        }

        private void btnStageServoOn_Click(object sender, EventArgs e)
        {
            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            } 

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "모든 축의 서보를 ON 하시겠습니까?"))
                return;

            //  ACS Motion
            //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
            //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
            //{
            //    ACSSPiiPlusMotionBoard.Api.Enable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
            //}

            //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
            //{
            //    ACSSPiiPlusMotionBoard.Api.Enable((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //}

            ////  Ajin Motion
            //if (!MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.ScannerZ))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.ScannerZ, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.StageTH))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.StageTH, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.RvaX))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.RvaX, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WaferProbeAlignParameter.AxisAjinEnum.RvaZ))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WaferProbeAlignParameter.AxisAjinEnum.RvaZ, true);
            //}

            waferProbeAlign.m_bRVA_CalibMode = false;

            var mb2 = new MessageBoxOk();
            mb2.ShowDialog("Information !", "전 축 서보 ON 되었습니다.");
            return;
        }

        private void baseButton_ElevZ_GO1_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //double lfTargetPos_U = 0.0;
            //double lfTargetPos_V = 0.0;
            //double lfTargetPos_W = 0.0;
            double lfTargetPos_ElevZ = 0.0;
            //double lfTargetPos_X = 0.0;
            //double lfTargetPos_Y = 0.0;
            //double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            //lfTargetPos_U = Convert.ToDouble(tb_Axis_U1.Text.Trim());
            //lfTargetPos_V = Convert.ToDouble(tb_Axis_V1.Text.Trim());
            //lfTargetPos_W = Convert.ToDouble(tb_Axis_W1.Text.Trim());
            lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ1.Text.Trim());
            //lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX1.Text.Trim());
            //lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY1.Text.Trim());
            //lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ1.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elev. Z 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_ElevZ_GO2_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //double lfTargetPos_U = 0.0;
            //double lfTargetPos_V = 0.0;
            //double lfTargetPos_W = 0.0;
            double lfTargetPos_ElevZ = 0.0;
            //double lfTargetPos_X = 0.0;
            //double lfTargetPos_Y = 0.0;
            //double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            //lfTargetPos_U = Convert.ToDouble(tb_Axis_U2.Text.Trim());
            //lfTargetPos_V = Convert.ToDouble(tb_Axis_V2.Text.Trim());
            //lfTargetPos_W = Convert.ToDouble(tb_Axis_W2.Text.Trim());
            lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ2.Text.Trim());
            //lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX2.Text.Trim());
            //lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY2.Text.Trim());
            //lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ2.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elev. Z 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_ElevZ_GO3_Click(object sender, EventArgs e)
        {
            //if (waferProbeAlign.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "RVA 조정 모드입니다.");
            //    return;
            //}

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //double lfTargetPos_U = 0.0;
            //double lfTargetPos_V = 0.0;
            //double lfTargetPos_W = 0.0;
            double lfTargetPos_ElevZ = 0.0;
            //double lfTargetPos_X = 0.0;
            //double lfTargetPos_Y = 0.0;
            //double lfTargetPos_VisionZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            //lfTargetPos_U = Convert.ToDouble(tb_Axis_U3.Text.Trim());
            //lfTargetPos_V = Convert.ToDouble(tb_Axis_V3.Text.Trim());
            //lfTargetPos_W = Convert.ToDouble(tb_Axis_W3.Text.Trim());
            lfTargetPos_ElevZ = Convert.ToDouble(tb_Axis_ElevZ3.Text.Trim());
            //lfTargetPos_X = Convert.ToDouble(tb_Axis_VisionX3.Text.Trim());
            //lfTargetPos_Y = Convert.ToDouble(tb_Axis_VisionY3.Text.Trim());
            //lfTargetPos_VisionZ = Convert.ToDouble(tb_Axis_VisionZ3.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elev. Z 를 Target 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity, lfAccDec, lfAccDec);
                MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_ElevZ, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);
                //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_VisionZ, 5, 50, 50);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseLabel_UVW_GetPos1_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 UVW 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            //tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            //tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            //tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            //tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel_UVW_GetPos2_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 UVW 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            //tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            //tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            //tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            //tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            WriteRegistry("CWA-150SA", "UserPos2", "UVW_U", tb_Axis_U2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "UVW_V", tb_Axis_V2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "UVW_W", tb_Axis_W2.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel_UVW_GetPos3_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 UVW 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            //tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            //tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            //tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            //tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            WriteRegistry("CWA-150SA", "UserPos3", "UVW_U", tb_Axis_U3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "UVW_V", tb_Axis_V3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "UVW_W", tb_Axis_W3.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel_XYZ_GetPos1_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 XYZ 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            //tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            //tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            //tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            //tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel_XYZ_GetPos2_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 XYZ 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            //tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            //tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            //tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            //tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            tb_Axis_VisionX2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_VisionY2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_VisionZ2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "Vis_X", tb_Axis_VisionX2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "Vis_Y", tb_Axis_VisionY2.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "Vis_Z", tb_Axis_VisionZ2.Text);
        }

        private void baseLabel_XYZ_GetPos3_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 XYZ 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            //tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            //tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            //tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            //tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            tb_Axis_VisionX3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_VisionY3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_VisionZ3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "Vis_X", tb_Axis_VisionX3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "Vis_Y", tb_Axis_VisionY3.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "Vis_Z", tb_Axis_VisionZ3.Text);
        }

        private void baseLabel_ElevZ_GetPos1_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Elev. Z 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            //tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            //tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            //tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            tb_Axis_ElevZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            //tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            //tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            //tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            WriteRegistry("CWA-150SA", "UserPos1", "ElevZ", tb_Axis_ElevZ1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel_ElevZ_GetPos2_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Elev. Z 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            //tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            //tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            //tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            tb_Axis_ElevZ2.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            //tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            //tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            //tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            WriteRegistry("CWA-150SA", "UserPos2", "ElevZ", tb_Axis_ElevZ2.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void baseLabel_ElevZ_GetPos3_Click(object sender, EventArgs e)
        {
            //  현재 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Elev. Z 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            //tb_Axis_U1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            //tb_Axis_V1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            //tb_Axis_W1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();
            tb_Axis_ElevZ3.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();
            //tb_Axis_VisionX1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            //tb_Axis_VisionY1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            //tb_Axis_VisionZ1.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (레지스트리)
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_U", tb_Axis_U1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_V", tb_Axis_V1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "UVW_W", tb_Axis_W1.Text);
            WriteRegistry("CWA-150SA", "UserPos3", "ElevZ", tb_Axis_ElevZ3.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_X", tb_Axis_VisionX1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Y", tb_Axis_VisionY1.Text);
            //WriteRegistry("CWA-150SA", "UserPos1", "Vis_Z", tb_Axis_VisionZ1.Text);
        }

        private void btnUpperCamera_Init_Click(object sender, EventArgs e)
        {
            //  카메라 초기화

            Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                waferProbeAlign.Camera_Upper.SetRunStatus(Part.RunStatus.Run);
                waferProbeAlign.Camera_Lower.SetRunStatus(Part.RunStatus.Run);
                waferProbeAlign.Camera_Upper.Initialize();
                waferProbeAlign.Camera_Lower.Initialize();
                return 0;
            });

            ProgressForm ProgressForm = new ProgressForm(waferProbeAlign.Name, "Camera Initializing...", task, waferProbeAlign.Camera_Upper);
            ProgressForm.StopProcess += ProgressForm_StopProcess;
            ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            ProgressForm.ShowDialog();

            waferProbeAlign.Camera_Upper.Initialize();
            waferProbeAlign.Camera_Lower.Initialize();
        }

        private void ProgressForm_StopProcess(object target)
        {
            Part part = target as Part;
            if (part != null)
            {
                //part.Stop();
            }
        }

        private void btnUpperCamera_StartLive_Click(object sender, EventArgs e)
        {
            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
            }
        }

        private void btnLoadingPos_Wafer_GO_Click(object sender, EventArgs e)
        {
            //  Wafer Loading 위치로 이동

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWafer_Loading_Ready_Step == (int)WaferProbeAlign.WaferLoading_Ready_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 로딩 대기 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWafer_Loading_Ready_Step = (int)WaferProbeAlign.WaferLoading_Ready_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 로딩 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nWafer_Loading_Ready_Step = (int)WaferProbeAlign.WaferLoading_Ready_Step.None;
            }
        }

        private void btnLoadingPos_ProbeCard_GO_Click(object sender, EventArgs e)
        {
            //  Probe-Card Loading 위치로 이동

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step == (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 로딩 대기 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nProbeCard_Loading_Ready_Step = (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 로딩 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nProbeCard_Loading_Ready_Step = (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.None;
            }
        }

        private void btn_ProbeCard_Locking_Click(object sender, EventArgs e)
        {
            //  Probe-Card Locking

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nProbeCard_Locking_Step == (int)WaferProbeAlign.ProbeCard_Locking_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 고정 작업을 진행하시겠습니까?"))
                    return;

                waferProbeAlign.m_nProbeCard_Locking_Step = (int)WaferProbeAlign.ProbeCard_Locking_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 고정 작업을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nProbeCard_Locking_Step = (int)WaferProbeAlign.ProbeCard_Locking_Step.None;
            }
        }

        private void btnSafetyPos_CamXY_GO_Click(object sender, EventArgs e)
        {
            //  Camera XY 안전위치로 이동

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nSafetyPos_Move_Step == (int)WaferProbeAlign.SafetyPos_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "안전 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nSafetyPos_Move_Step = (int)WaferProbeAlign.SafetyPos_Move_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "안전 위치로 이동 동작을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nSafetyPos_Move_Step = (int)WaferProbeAlign.SafetyPos_Move_Step.None;
            }
        }

        private void btnReticlePos_UpperCam_GO_Click(object sender, EventArgs e)
        {
            //  상부 카메라로 Reticle Glass 를 확인하는 위치로 이동

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step == (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동하시겠습니까?\r\n\r\n[Upper Camera]\r\n\r\n##  프로브 카드는 반드시 제거해야 합니다. [충돌 경고]  ##"))
                    return;

                waferProbeAlign.m_nReticleCheck_UpperCam_Step = (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.Start;
                waferProbeAlign.timer_ReticleGlass_Check.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동을 중지하시겠습니까?\r\n\r\n[Upper Camera]"))
                    return;

                waferProbeAlign.timer_ReticleGlass_Check.Enabled = false;
                waferProbeAlign.m_nReticleCheck_UpperCam_Step = (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.None;
            }
        }

        private void btnReticlePos_LowerCam_GO_Click(object sender, EventArgs e)
        {
            //  하부 카메라로 Reticle Glass 를 확인하는 위치로 이동

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step == (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동하시겠습니까?\r\n\r\n[Lower Camera]"))
                    return;

                waferProbeAlign.m_nReticleCheck_LowerCam_Step = (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.Start;
                waferProbeAlign.timer_ReticleGlass_Check.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동을 중지하시겠습니까?\r\n\r\n[Lower Camera]"))
                    return;

                waferProbeAlign.timer_ReticleGlass_Check.Enabled = false;
                waferProbeAlign.m_nReticleCheck_LowerCam_Step = (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.None;
            }
        }

        private void baseButton_Y_Pos_GO1_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (TOP)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_Y = Convert.ToDouble(tb_Axis_Y_TOP.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision Y축을 TOP 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X], lfVelocity, lfAccDec, lfAccDec);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_Y_Pos_GO2_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (MID)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_Y = Convert.ToDouble(tb_Axis_Y_MID.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision Y축을 MID 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Middle");

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X], lfVelocity, lfAccDec, lfAccDec);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_Y_Pos_GO3_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (BOT)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_Y = Convert.ToDouble(tb_Axis_Y_BOT.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision Y축을 BOT 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam2 = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam2.dTarget[(int)nAxis.X], lfVelocity, lfAccDec, lfAccDec);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_X_Pos_GO1_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (LEFT)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_X = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_X = Convert.ToDouble(tb_Axis_X_LEFT.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision X축을 LEFT 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Left");

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_X_Pos_GO2_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (CENTER)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_X = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_X = Convert.ToDouble(tb_Axis_X_CENTER.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision X축을 CENTER 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Center");

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_X_Pos_GO3_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (RIGHT)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_X = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_X = Convert.ToDouble(tb_Axis_X_RIGHT.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision X축을 RIGHT 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Right");

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseLabelPosition_Top_Click(object sender, EventArgs e)
        {
            //  Top 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "[TOP] 현재 Y축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            double m_dPos_U_TOP = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V_TOP = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W_TOP = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
            double m_dPos_EZ_TOP = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

            double m_dPos_X_TOP = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
            tb_Axis_Y_TOP.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            double m_dPos_VZ_TOP = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Top = -1;
            int m_nIndex_Mid = -1;
            int m_nIndex_Bot = -1;
            int m_nIndex_Left = -1;
            int m_nIndex_Center = -1;
            int m_nIndex_Right = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Left)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Left = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Center)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Center = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Right)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Right = i;
                }
            }

            //  좌표 변경
            if (m_nIndex_Top != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                waferProbeAlign.Config.Positions[m_nIndex_Top].U = m_dPos_U_TOP;
                waferProbeAlign.Config.Positions[m_nIndex_Top].V = m_dPos_V_TOP;
                waferProbeAlign.Config.Positions[m_nIndex_Top].W = m_dPos_W_TOP;
                waferProbeAlign.Config.Positions[m_nIndex_Top].EZ = m_dPos_EZ_TOP;

                waferProbeAlign.Config.Positions[m_nIndex_Top].X = m_dPos_X_TOP;
                waferProbeAlign.Config.Positions[m_nIndex_Top].Y = Convert.ToDouble(tb_Axis_Y_TOP.Text); ;
                waferProbeAlign.Config.Positions[m_nIndex_Top].VZ = m_dPos_VZ_TOP;

                if (m_nIndex_Mid > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Mid].EZ = m_dPos_EZ_TOP;
                    waferProbeAlign.Config.Positions[m_nIndex_Mid].VZ = m_dPos_VZ_TOP;
                }

                if (m_nIndex_Bot > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Bot].EZ = m_dPos_EZ_TOP;
                    waferProbeAlign.Config.Positions[m_nIndex_Bot].VZ = m_dPos_VZ_TOP;
                }

                if (m_nIndex_Left > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Left].EZ = m_dPos_EZ_TOP;
                    waferProbeAlign.Config.Positions[m_nIndex_Left].VZ = m_dPos_VZ_TOP;
                }

                if (m_nIndex_Center > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Center].EZ = m_dPos_EZ_TOP;
                    waferProbeAlign.Config.Positions[m_nIndex_Center].VZ = m_dPos_VZ_TOP;
                }

                if (m_nIndex_Right > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Right].EZ = m_dPos_EZ_TOP;
                    waferProbeAlign.Config.Positions[m_nIndex_Right].VZ = m_dPos_VZ_TOP;
                }


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Ver_Top\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_Mid_Click(object sender, EventArgs e)
        {
            //  Middle 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "[MID] 현재 Y축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            double m_dPos_U_MID = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V_MID = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W_MID = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
            double m_dPos_EZ_MID = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

            double m_dPos_X_MID = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
            tb_Axis_Y_MID.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            double m_dPos_VZ_MID = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Top = -1;
            int m_nIndex_Mid = -1;
            int m_nIndex_Bot = -1;
            int m_nIndex_Left = -1;
            int m_nIndex_Center = -1;
            int m_nIndex_Right = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Left)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Left = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Center)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Center = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Right)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Right = i;
                }
            }

            //  좌표 변경
            if (m_nIndex_Mid != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Middle");

                waferProbeAlign.Config.Positions[m_nIndex_Mid].U = m_dPos_U_MID;
                waferProbeAlign.Config.Positions[m_nIndex_Mid].V = m_dPos_V_MID;
                waferProbeAlign.Config.Positions[m_nIndex_Mid].W = m_dPos_W_MID;
                waferProbeAlign.Config.Positions[m_nIndex_Mid].EZ = m_dPos_EZ_MID;

                waferProbeAlign.Config.Positions[m_nIndex_Mid].X = m_dPos_X_MID;
                waferProbeAlign.Config.Positions[m_nIndex_Mid].Y = Convert.ToDouble(tb_Axis_Y_MID.Text); ;
                waferProbeAlign.Config.Positions[m_nIndex_Mid].VZ = m_dPos_VZ_MID;

                if (m_nIndex_Top > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Top].EZ = m_dPos_EZ_MID;
                }

                if (m_nIndex_Bot > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Bot].EZ = m_dPos_EZ_MID;
                }

                if (m_nIndex_Left > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Left].EZ = m_dPos_EZ_MID;
                }

                if (m_nIndex_Center > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Center].EZ = m_dPos_EZ_MID;
                }

                if (m_nIndex_Right > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Right].EZ = m_dPos_EZ_MID;
                }


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Ver_Middle\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_Bot_Click(object sender, EventArgs e)
        {
            //  Bottom 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "[BOT] 현재 Y축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            double m_dPos_U_BOT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V_BOT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W_BOT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
            double m_dPos_EZ_BOT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

            double m_dPos_X_BOT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
            tb_Axis_Y_BOT.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            double m_dPos_VZ_BOT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Top = -1;
            int m_nIndex_Mid = -1;
            int m_nIndex_Bot = -1;
            int m_nIndex_Left = -1;
            int m_nIndex_Center = -1;
            int m_nIndex_Right = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Left)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Left = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Center)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Center = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Right)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Right = i;
                }
            }

            //  좌표 변경
            if (m_nIndex_Bot != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");

                waferProbeAlign.Config.Positions[m_nIndex_Bot].U = m_dPos_U_BOT;
                waferProbeAlign.Config.Positions[m_nIndex_Bot].V = m_dPos_V_BOT;
                waferProbeAlign.Config.Positions[m_nIndex_Bot].W = m_dPos_W_BOT;
                waferProbeAlign.Config.Positions[m_nIndex_Bot].EZ = m_dPos_EZ_BOT;

                waferProbeAlign.Config.Positions[m_nIndex_Bot].X = m_dPos_X_BOT;
                waferProbeAlign.Config.Positions[m_nIndex_Bot].Y = Convert.ToDouble(tb_Axis_Y_BOT.Text); ;
                waferProbeAlign.Config.Positions[m_nIndex_Bot].VZ = m_dPos_VZ_BOT;

                if (m_nIndex_Top > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Top].EZ = m_dPos_EZ_BOT;
                }

                if (m_nIndex_Mid > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Mid].EZ = m_dPos_EZ_BOT;
                }

                if (m_nIndex_Left > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Left].EZ = m_dPos_EZ_BOT;
                }

                if (m_nIndex_Center > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Center].EZ = m_dPos_EZ_BOT;
                }

                if (m_nIndex_Right > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Right].EZ = m_dPos_EZ_BOT;
                }


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Ver_Bottom\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_Left_Click(object sender, EventArgs e)
        {
            //  Left 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "[LEFT] 현재 X축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            double m_dPos_U_LEFT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V_LEFT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W_LEFT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
            double m_dPos_EZ_LEFT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

            tb_Axis_X_LEFT.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            double m_dPos_Y_LEFT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);
            double m_dPos_VZ_LEFT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Top = -1;
            int m_nIndex_Mid = -1;
            int m_nIndex_Bot = -1;
            int m_nIndex_Left = -1;
            int m_nIndex_Center = -1;
            int m_nIndex_Right = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Left)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Left = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Center)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Center = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Right)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Right = i;
                }
            }

            //  좌표 변경
            if (m_nIndex_Left != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Left");

                waferProbeAlign.Config.Positions[m_nIndex_Left].U = m_dPos_U_LEFT;
                waferProbeAlign.Config.Positions[m_nIndex_Left].V = m_dPos_V_LEFT;
                waferProbeAlign.Config.Positions[m_nIndex_Left].W = m_dPos_W_LEFT;
                waferProbeAlign.Config.Positions[m_nIndex_Left].EZ = m_dPos_EZ_LEFT;

                waferProbeAlign.Config.Positions[m_nIndex_Left].X = Convert.ToDouble(tb_Axis_X_LEFT.Text); ;
                waferProbeAlign.Config.Positions[m_nIndex_Left].Y = m_dPos_Y_LEFT;
                waferProbeAlign.Config.Positions[m_nIndex_Left].VZ = m_dPos_VZ_LEFT;

                if (m_nIndex_Top > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Top].EZ = m_dPos_EZ_LEFT;
                }

                if (m_nIndex_Mid > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Mid].EZ = m_dPos_EZ_LEFT;
                }

                if (m_nIndex_Bot > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Bot].EZ = m_dPos_EZ_LEFT;
                }

                if (m_nIndex_Center > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Center].EZ = m_dPos_EZ_LEFT;
                }

                if (m_nIndex_Right > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Right].EZ = m_dPos_EZ_LEFT;
                }


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Hor_Left\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_Center_Click(object sender, EventArgs e)
        {
            //  Center 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "[CENTER] 현재 X축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            double m_dPos_U_CENTER = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V_CENTER = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W_CENTER = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
            double m_dPos_EZ_CENTER = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

            tb_Axis_X_CENTER.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            double m_dPos_Y_CENTER = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);
            double m_dPos_VZ_CENTER = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Top = -1;
            int m_nIndex_Mid = -1;
            int m_nIndex_Bot = -1;
            int m_nIndex_Left = -1;
            int m_nIndex_Center = -1;
            int m_nIndex_Right = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Left)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Left = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Center)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Center = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Right)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Right = i;
                }
            }

            //  좌표 변경
            if (m_nIndex_Center != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Center");

                waferProbeAlign.Config.Positions[m_nIndex_Center].U = m_dPos_U_CENTER;
                waferProbeAlign.Config.Positions[m_nIndex_Center].V = m_dPos_V_CENTER;
                waferProbeAlign.Config.Positions[m_nIndex_Center].W = m_dPos_W_CENTER;
                waferProbeAlign.Config.Positions[m_nIndex_Center].EZ = m_dPos_EZ_CENTER;

                waferProbeAlign.Config.Positions[m_nIndex_Center].X = Convert.ToDouble(tb_Axis_X_CENTER.Text); ;
                waferProbeAlign.Config.Positions[m_nIndex_Center].Y = m_dPos_Y_CENTER;
                waferProbeAlign.Config.Positions[m_nIndex_Center].VZ = m_dPos_VZ_CENTER;

                if (m_nIndex_Top > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Top].EZ = m_dPos_EZ_CENTER;
                }

                if (m_nIndex_Mid > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Mid].EZ = m_dPos_EZ_CENTER;
                }

                if (m_nIndex_Bot > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Bot].EZ = m_dPos_EZ_CENTER;
                }

                if (m_nIndex_Left > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Left].EZ = m_dPos_EZ_CENTER;
                }

                if (m_nIndex_Right > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Right].EZ = m_dPos_EZ_CENTER;
                }


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Hor_Center\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_RIGHT_Click(object sender, EventArgs e)
        {
            //  Right 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "[RIGHT] 현재 X축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            double m_dPos_U_RIGHT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V_RIGHT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W_RIGHT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
            double m_dPos_EZ_RIGHT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

            tb_Axis_X_RIGHT.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            double m_dPos_Y_RIGHT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);
            double m_dPos_VZ_RIGHT = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Top = -1;
            int m_nIndex_Mid = -1;
            int m_nIndex_Bot = -1;
            int m_nIndex_Left = -1;
            int m_nIndex_Center = -1;
            int m_nIndex_Right = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex_Top = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex_Mid = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex_Bot = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Left)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex_Left = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Center)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex_Center = i;
                }

                //  웨이퍼 얼라인 상태를 확인하는 위치 (Right)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex_Right = i;
                }
            }

            //  좌표 변경
            if (m_nIndex_Right != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Right");

                waferProbeAlign.Config.Positions[m_nIndex_Right].U = m_dPos_U_RIGHT;
                waferProbeAlign.Config.Positions[m_nIndex_Right].V = m_dPos_V_RIGHT;
                waferProbeAlign.Config.Positions[m_nIndex_Right].W = m_dPos_W_RIGHT;
                waferProbeAlign.Config.Positions[m_nIndex_Right].EZ = m_dPos_EZ_RIGHT;

                waferProbeAlign.Config.Positions[m_nIndex_Right].X = Convert.ToDouble(tb_Axis_X_RIGHT.Text); ;
                waferProbeAlign.Config.Positions[m_nIndex_Right].Y = m_dPos_Y_RIGHT;
                waferProbeAlign.Config.Positions[m_nIndex_Right].VZ = m_dPos_VZ_RIGHT;

                if (m_nIndex_Top > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Top].EZ = m_dPos_EZ_RIGHT;
                }

                if (m_nIndex_Mid > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Mid].EZ = m_dPos_EZ_RIGHT;
                }

                if (m_nIndex_Bot > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Bot].EZ = m_dPos_EZ_RIGHT;
                }

                if (m_nIndex_Left > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Left].EZ = m_dPos_EZ_RIGHT;
                }

                if (m_nIndex_Center > -1)
                {
                    waferProbeAlign.Config.Positions[m_nIndex_Center].EZ = m_dPos_EZ_RIGHT;
                }


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Hor_Right\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_StageCamera_ReticleCenter_Click(object sender, EventArgs e)
        {
            //  Reticle 확인 위치 가져오기

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 UVW, Vision XY 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_U_ReticleCenter.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U).ToString();
            tb_Axis_V_ReticleCenter.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V).ToString();
            tb_Axis_W_ReticleCenter.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W).ToString();

            tb_Axis_X_ReticleCenter.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X).ToString();
            tb_Axis_Y_ReticleCenter.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y).ToString();
            tb_Axis_Z_ReticleCenter.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ).ToString();

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_UpperCam = -1;
            int m_nIndex_LowerCam = -1;

            for ( int i = 0; i < waferProbeAlign.Config.Positions.Count; i++ )
            {
                //  Reticle Glass 를 보는 Upper Camera 위치 Index
                if (waferProbeAlign.Config.Positions[i].Name == "ReticleGlass_UpperCamera")
                {
                    m_nIndex_UpperCam = i;
                }

                //  Reticle Glass 를 보는 Lower Camera 위치 Index
                if (waferProbeAlign.Config.Positions[i].Name == "ReticleGlass_LowerCamera")
                {
                    m_nIndex_LowerCam = i;
                }

                if ((m_nIndex_UpperCam != -1) && (m_nIndex_LowerCam != -1))
                {
                    break;
                }
            }

            //  Upper Cam 좌표 변경
            if (m_nIndex_UpperCam != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");
                //m_dOldPos = waferProbeAlign.laserDrillingParameter.stLaserDrillingPosParam.dTarget[(int)LaserDrilling.nAxis.Z];
                //m_dNewPos = waferProbeAlign.MC_Func.MC_GetEncPos(0);

                //m_strTemp = "가공 높이 (WorkStage WorkHeight) 를 변경하시겠습니까?\r\n[ 기존 : " + m_dOldPos.ToString() + " → 변경 : " + m_dNewPos.ToString() + " ]\r\n\r\n(설정파일 변경 후 저장됩니다.)";

                //var mb = new MessageBoxYesNo();
                //if (DialogResult.Yes != mb.ShowDialog("Question ?", m_strTemp))
                //    return;

                ////  StageZ 한계위치 설정되어 있는지 체크
                //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos <= 0.0)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Stage Z축 한계 높이가 설정되어 있지 않습니다.\r\n\r\n(Config -> [17] Interlock  확인)");
                //    return;
                //}

                ////  StageZ 한계위치를 초과하여 이동하는지 체크
                //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos < m_dNewPos)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "변경하려는 가공 높이가 Stage Z축 한계 높이를 초과합니다.\r\n\r\n[ Cancel ]");
                //    return;
                //}

                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].U = Convert.ToDouble(tb_Axis_U_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].V = Convert.ToDouble(tb_Axis_V_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].W = Convert.ToDouble(tb_Axis_W_ReticleCenter.Text);

                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].X = Convert.ToDouble(tb_Axis_X_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].Y = Convert.ToDouble(tb_Axis_Y_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].VZ = Convert.ToDouble(tb_Axis_Z_ReticleCenter.Text);


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                                                                     //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"ReticleGlass_UpperCamera\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }


            //  Lower Cam 좌표 변경
            if (m_nIndex_LowerCam != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].U = Convert.ToDouble(tb_Axis_U_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].V = Convert.ToDouble(tb_Axis_V_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].W = Convert.ToDouble(tb_Axis_W_ReticleCenter.Text);

                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].X = Convert.ToDouble(tb_Axis_X_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].Y = Convert.ToDouble(tb_Axis_Y_ReticleCenter.Text);
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].VZ = Convert.ToDouble(tb_Axis_Z_ReticleCenter.Text);


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                                                                     //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"ReticleGlass_LowerCamera\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseLabelPosition_UpperCamera_ReticleCenter_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Reticle 확인 위치 가져오기 (Upper Cam)

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Elevator Z축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_EZ_ReticleCenter_UpperCam.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_UpperCam = -1;

            for ( int i = 0; i < waferProbeAlign.Config.Positions.Count; i++ )
            {
                //  Reticle Glass 를 보는 Upper Camera 위치 Index
                if (waferProbeAlign.Config.Positions[i].Name == "ReticleGlass_UpperCamera")
                {
                    m_nIndex_UpperCam = i;
                    break;
                }
            }

            //  Upper Cam 좌표 변경
            if (m_nIndex_UpperCam != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");

                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].EZ = Convert.ToDouble(tb_Axis_EZ_ReticleCenter_UpperCam.Text);


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                                                                     //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"ReticleGlass_UpperCamera\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }


            //  조명값 저장
            var mb2 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb2.ShowDialog("Question ?", "현재 조명 밝기값을 Reticle 검사 조명값으로 사용하시겠습니까?\r\n\r\n[Reticle 검사에만 사용됨]"))
            {
                var mb3 = new MessageBoxOk();
                mb3.ShowDialog("Information !", "상부 카메라 Reticle 검사 위치 저장.\r\n\r\n[Elevator. Z]");

                return;
            }

            foreach (IlluminationChannel channel in waferProbeAlign.visionCalibrator_Upper.IlluminationData.Values)
            {
                if (channel.ChannelName == "Upper")
                {
                    waferProbeAlign.Config.ParamConfig.ReticleAlign_UpperVision_LightValue = channel.Value;
                }
                else if (channel.ChannelName == "Lower")
                {
                    waferProbeAlign.Config.ParamConfig.ReticleAlign_LowerVision_LightValue = channel.Value;
                }
            }

            string m_strRecipe1 = "";
            RecipeInfo m_recipeInfo1 = new RecipeInfo();
            m_recipeInfo1 = Equipment.GetCurrentRecipe();

            if (m_recipeInfo1 != null)
            {
                m_strRecipe1 = m_recipeInfo1.Name;

                //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.SaveConfig(m_strRecipe1);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            }
            else
            {
                //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            }

            DataManager.Instance.ApplyConfigData(waferProbeAlign);

            //  Config 창 데이터 갱신을 위해서
            Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

            //visionCalibrator.Illuminator.TurnOnOff(true, 4);       //  Spot 조명
            //visionCalibrator.Illuminator.TurnOnOff(true, 2);       //  Ring 조명

            var mb4 = new MessageBoxOk();
            mb4.ShowDialog("Information !", "Reticle 위치 및 조명값이 저장되었습니다.");
        }

        private void baseLabelPosition_LowerCamera_ReticleCenter_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Reticle 확인 위치 가져오기 (Lower Cam)

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Elevator Z축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_EZ_ReticleCenter_LowerCam.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_LowerCam = -1;

            for ( int i = 0; i < waferProbeAlign.Config.Positions.Count; i++ )
            {
                //  Reticle Glass 를 보는 Lower Camera 위치 Index
                if (waferProbeAlign.Config.Positions[i].Name == "ReticleGlass_LowerCamera")
                {
                    m_nIndex_LowerCam = i;
                    break;
                }
            }

            //  Lower Cam 좌표 변경
            if (m_nIndex_LowerCam != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].EZ = Convert.ToDouble(tb_Axis_EZ_ReticleCenter_LowerCam.Text);


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                                                                     //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"ReticleGlass_LowerCamera\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }


            //  조명값 저장
            var mb2 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb2.ShowDialog("Question ?", "현재 조명 밝기값을 Reticle 검사 조명값으로 사용하시겠습니까?\r\n\r\n[Reticle 검사에만 사용됨]"))
            {
                var mb3 = new MessageBoxOk();
                mb3.ShowDialog("Information !", "하부 카메라 Reticle 검사 위치 저장.\r\n\r\n[Elevator. Z]");

                return;
            }

            foreach (IlluminationChannel channel in waferProbeAlign.visionCalibrator_Upper.IlluminationData.Values)
            {
                if (channel.ChannelName == "Upper")
                {
                    waferProbeAlign.Config.ParamConfig.ReticleAlign_UpperVision_LightValue = channel.Value;
                }
                else if (channel.ChannelName == "Lower")
                {
                    waferProbeAlign.Config.ParamConfig.ReticleAlign_LowerVision_LightValue = channel.Value;
                }
            }

            string m_strRecipe2 = "";
            RecipeInfo m_recipeInfo2 = new RecipeInfo();
            m_recipeInfo2 = Equipment.GetCurrentRecipe();

            if (m_recipeInfo2 != null)
            {
                m_strRecipe2 = m_recipeInfo2.Name;

                //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.SaveConfig(m_strRecipe2);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            }
            else
            {
                //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            }

            DataManager.Instance.ApplyConfigData(waferProbeAlign);

            //  Config 창 데이터 갱신을 위해서
            Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

            //visionCalibrator.Illuminator.TurnOnOff(true, 4);       //  Spot 조명
            //visionCalibrator.Illuminator.TurnOnOff(true, 2);       //  Ring 조명

            var mb4 = new MessageBoxOk();
            mb4.ShowDialog("Information !", "Reticle 위치 및 조명값이 저장되었습니다.");
        }

        private void baseLabelPosition_Wafer_Probecard_Packing_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Wafer & ProbeCard Packing 위치 가져오기
            //  조건 :    1. Thin-Chuck Vacuum On

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Elevator Z축 위치값을 가져오시겠습니까?\r\n\r\n(기존 위치값 변경 및 저장.)"))
                return;

            tb_Axis_EZ_Wafer_ProbeCard_Packing.Text = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ).ToString();

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Packing = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  Probe Card 에 Thin-Chuck 을 Packing 하는 위치 Index
                if (waferProbeAlign.Config.Positions[i].Name == "ProbeWafer_Packing")
                {
                    m_nIndex_Packing = i;
                    break;
                }
            }

            //  Packing 좌표 변경
            if (m_nIndex_Packing != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                waferProbeAlign.Config.Positions[m_nIndex_Packing].EZ = Convert.ToDouble(tb_Axis_EZ_Wafer_ProbeCard_Packing.Text);


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //  2024. 05. 13.  SCH : Config 창 데이터 갱신을 위해서 추가됨.
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"ProbeWafer_Packing\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseButton_GoPos_ReticleCenter_StageCamXY_Click(object sender, EventArgs e)
        {
            //  Reticle Glass 를 보는 위치로 UVW Stage 와 Camera XY Stage 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_U = 0.0;
            double lfTargetPos_V = 0.0;
            double lfTargetPos_W = 0.0;
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;
            double lfTargetPos_Z = 0.0;

            double lfVelocity_UVW = 0.0;
            double lfAccDec_UVW = 0.0f;
            double lfVelocity_XY = 0.0;
            double lfAccDec_XY = 0.0f;
            double lfVelocity_VZ = 0.0;
            double lfAccDec_VZ = 0.0f;

            lfTargetPos_U = Convert.ToDouble(tb_Axis_U_ReticleCenter.Text.Trim());
            lfTargetPos_V = Convert.ToDouble(tb_Axis_V_ReticleCenter.Text.Trim());
            lfTargetPos_W = Convert.ToDouble(tb_Axis_W_ReticleCenter.Text.Trim());
            lfTargetPos_X = Convert.ToDouble(tb_Axis_X_ReticleCenter.Text.Trim());
            lfTargetPos_Y = Convert.ToDouble(tb_Axis_Y_ReticleCenter.Text.Trim());
            lfTargetPos_Z = Convert.ToDouble(tb_Axis_Z_ReticleCenter.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "UVW Stage 와 Vision XYZ 축을 Reticle Glass 확인 위치로 보내시겠습니까?"))
                    return;

                lfVelocity_UVW = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Velocity;
                lfAccDec_UVW = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.U.ToString()].Configuration.Acceleration;
                lfVelocity_XY = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                lfAccDec_XY = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;
                lfVelocity_VZ = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.VZ.ToString()].Configuration.Velocity;
                lfAccDec_VZ = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.VZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                {
                    var mb1 = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb1.ShowDialog("Question ?", "Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.\r\n계속 진행하시겠습니까?"))
                        return;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity_UVW, lfAccDec_UVW, lfAccDec_UVW);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity_UVW, lfAccDec_UVW, lfAccDec_UVW);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity_UVW, lfAccDec_UVW, lfAccDec_UVW);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity_XY, lfAccDec_XY, lfAccDec_XY);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity_XY, lfAccDec_XY, lfAccDec_XY);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_Z, lfVelocity_VZ, lfAccDec_VZ, lfAccDec_VZ);
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, lfTargetPos_U, lfVelocity_UVW, lfAccDec_UVW, lfAccDec_UVW);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, lfTargetPos_V, lfVelocity_UVW, lfAccDec_UVW, lfAccDec_UVW);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, lfTargetPos_W, lfVelocity_UVW, lfAccDec_UVW, lfAccDec_UVW);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity_XY, lfAccDec_XY, lfAccDec_XY);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity_XY, lfAccDec_XY, lfAccDec_XY);
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, lfTargetPos_Z, lfVelocity_VZ, lfAccDec_VZ, lfAccDec_VZ);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_GoPos_ReticleCenter_UpperCam_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Reticle 확인 위치로 이동 (Upper Cam)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            double lfTargetPos_EZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_EZ = Convert.ToDouble(tb_Axis_EZ_ReticleCenter_UpperCam.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elevator Z축을 Reticle Glass 확인 위치로 보내시겠습니까?\r\n\r\n[상부 카메라]"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= waferProbeAlign.Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                {
                    MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_EZ, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_GoPos_ReticleCenter_LowerCam_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Reticle 확인 위치로 이동 (Lower Cam)

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            double lfTargetPos_EZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_EZ = Convert.ToDouble(tb_Axis_EZ_ReticleCenter_LowerCam.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elevator Z축을 Reticle Glass 확인 위치로 보내시겠습니까?\r\n\r\n[하부 카메라]"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= waferProbeAlign.Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                {
                    MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_EZ, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseButton_GoPos_Wafer_ProbeCard_Packing_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Packing 위치로 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            double lfTargetPos_EZ = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            lfTargetPos_EZ = Convert.ToDouble(tb_Axis_EZ_Wafer_ProbeCard_Packing.Text.Trim());

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elevator Z축을 Wafer && ProbeCard 패킹 위치로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                //{
                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                //                            lfTargetPos_Y);

                //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                //                            lfTargetPos_X);
                //}

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= waferProbeAlign.Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                {
                    MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌 위치에 있습니다.", "Warning!!");
                }
                else
                {
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, lfTargetPos_EZ, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                return;
            }
        }

        private void baseLabelUVWPosition_Loading_Click(object sender, EventArgs e)
        {
            //  Ready, Loading Unloading 위치 저장

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_Ready = -1;
            int m_nIndex_Load = -1;
            int m_nIndex_Unload = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  Ready
                if (waferProbeAlign.Config.Positions[i].Name == "Ready")
                {
                    m_nIndex_Ready = i;
                }

                //  Load
                if (waferProbeAlign.Config.Positions[i].Name == "Load")
                {
                    m_nIndex_Load = i;
                }

                //  UnLoad
                if (waferProbeAlign.Config.Positions[i].Name == "UnLoad")
                {
                    m_nIndex_Unload = i;
                }
            }

            double m_dPos_U = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dPos_V = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dPos_W = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 UVW 스테이지 위치를 (Ready / Load / UnLoad) 위치로 저장하시겠습니까?"))
                return;

            //  Ready 좌표 변경
            if (m_nIndex_Ready != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("Ready");

                waferProbeAlign.Config.Positions[m_nIndex_Ready].U = m_dPos_U;
                waferProbeAlign.Config.Positions[m_nIndex_Ready].V = m_dPos_V;
                waferProbeAlign.Config.Positions[m_nIndex_Ready].W = m_dPos_W;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"Ready\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }

            //  Load 좌표 변경
            if (m_nIndex_Load != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("Load");

                waferProbeAlign.Config.Positions[m_nIndex_Load].U = m_dPos_U;
                waferProbeAlign.Config.Positions[m_nIndex_Load].V = m_dPos_V;
                waferProbeAlign.Config.Positions[m_nIndex_Load].W = m_dPos_W;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"Load\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }

            //  Unload 좌표 변경
            if (m_nIndex_Unload != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("UnLoad");

                waferProbeAlign.Config.Positions[m_nIndex_Unload].U = m_dPos_U;
                waferProbeAlign.Config.Positions[m_nIndex_Unload].V = m_dPos_V;
                waferProbeAlign.Config.Positions[m_nIndex_Unload].W = m_dPos_W;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"UnLoad\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }

            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (m_recipeInfo != null)
            {
                m_strRecipe = m_recipeInfo.Name;

                //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            }
            else
            {
                //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            }

            DataManager.Instance.ApplyConfigData(waferProbeAlign);

            //  Config 창 데이터 갱신을 위해서
            Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

            var mb2 = new MessageBoxOk();
            mb2.ShowDialog("Information !", "현재 UVW 스테이지 위치를 (Ready / Load / UnLoad) 위치로 저장하였습니다.");
        }

        private void btnUser_Registration_Click(object sender, EventArgs e)
        {
            //  사용자 등록

            m_UserRegistration_CWA150SA.radioButton_Operator.Checked = true;
            m_UserRegistration_CWA150SA.tb_Password.Text = "";
            m_UserRegistration_CWA150SA.tb_Name.Text = "";
            m_UserRegistration_CWA150SA.tb_ID.Text = "";

            m_UserRegistration_CWA150SA.checkBox_Password_Visible.Checked = false;
            m_UserRegistration_CWA150SA.StartPosition = FormStartPosition.CenterScreen;
            m_UserRegistration_CWA150SA.ShowDialog();
        }
    }
}
