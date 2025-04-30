using ACS.SPiiPlusNET;
using Microsoft.Win32;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Core;
using QMC.Process.WorkStage.Parts;
using QMC.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Modules.WorkStage;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

namespace SLD200_MSL
{
    public partial class ParameterSetting_SLD200 : UserControl
    {
        //protected ModuleStateControl m_ModuleStateControl;
        private IlluminatorControl m_IlluminatorControl;
        //public ModulePositionControl m_WorkStagePosControl;
        public JogControl m_JogControl_Scanner;
        public JogControl m_JogControl_Loader;
        public JogControl m_JogControl_Unloader;

        private FormUserRegistration m_UserRegistration_SLD200;

        //protected VisionImageViewer m_visionImageViewer_Upper;
        //protected VisionImageViewer m_visionImageViewer_Lower;

        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        private NeedleCalibrationJogControl m_NeedleCalibrationJogControl;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }

        public ScannerParameter Scanner { get; set; }

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        private string itemSelected;

        private Thread m_parameterSettingThread;
        private bool m_bParameterSettingThreadExit;

        string strRegistryTmp = "SOFTWARE\\";       //  레지스트리 최상위 폴더 지정
        string strAppName = "SLD200_MSL";

        MotionFunction MC_Func = new InterpolatorMotionFunction();

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

        private double m_dCurBaseTipPos_X = 0;
        private double m_dCurBaseTipPos_Y = 0;
        private double m_dCurSourceTipPos_X = 0;
        private double m_dCurSourceTipPos_Y = 0;

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
                Log.Write(ex);
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
                Log.Write(ex);
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
                Log.Write(ex);
                return false;
            }

            return true;
        }

        #endregion


        //  Test용 변수
        bool m_bRun;


        public FormUserRegistration userRegistration
        {
            get { return this.m_UserRegistration_SLD200; }
            set { this.m_UserRegistration_SLD200 = value; }
        }


        public ParameterSetting_SLD200()
        {
            InitializeComponent();

            Scanner = new ScannerParameter("Scanner");

            AxisList = new List<MotionAxis>();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
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
            }

            //m_ModuleStateControl = new ModuleStateControl(workStage);

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

            this.m_visionImageViewer_LowRes.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_LowRes.SuspendDisplay();
            this.m_visionImageViewer_HighRes.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_HighRes.SuspendDisplay();

            this.m_visionImageViewer_HighRes.Camera = workStage.Camera_HighRes;
            this.m_visionImageViewer_LowRes.Camera = workStage.Camera_LowRes;

            this.VisibleChanged += ParameterSetting_SLD200_VisibleChanged;


            //  Jog Control
            m_JogControl_Unloader = new JogControl(unloader.Stage);
            //m_JogControl_Scanner.Location = new Point(btnUpperCamera_Init.Location.X, btnUpperCamera_Init.Location.Y + btnUpperCamera_Init.Size.Height + 10);
            m_JogControl_Unloader.Location = new Point(5, 5);
            this.tabControl_Jog.TabPages[0].Controls.Add(m_JogControl_Unloader);

            m_JogControl_Scanner = new JogControl(workStage.Stage);
            //m_JogControl_Scanner.Location = new Point(btnUpperCamera_Init.Location.X, btnUpperCamera_Init.Location.Y + btnUpperCamera_Init.Size.Height + 10);
            m_JogControl_Scanner.Location = new Point(5, 5);
            this.tabControl_Jog.TabPages[1].Controls.Add(m_JogControl_Scanner);

            m_JogControl_Loader = new JogControl(loader.Stage);
            //m_JogControl_Scanner.Location = new Point(btnUpperCamera_Init.Location.X, btnUpperCamera_Init.Location.Y + btnUpperCamera_Init.Size.Height + 10);
            m_JogControl_Loader.Location = new Point(5, 5);
            this.tabControl_Jog.TabPages[2].Controls.Add(m_JogControl_Loader);

            ////  조명
            //this.m_IlluminatorControl = new IlluminatorControl(workStage.visionCalibrator_Upper.Recipe.IlluminationDataSet.ToList());
            //this.m_IlluminatorControl.Location = new Point(this.m_JogControl.Location.X + m_JogControl.Size.Width + 22, m_JogControl.Location.Y);
            //this.m_IlluminatorControl.Illuminator = workStage.visionCalibrator_Upper.Illuminator;
            //this.m_IlluminatorControl.IlluminatorControlButton_Click += m_IlluminatorControl_IlluminatorControlButton_Click;
            //this.Controls.Add(m_IlluminatorControl);

            m_nTotalAxis = 4;

            ////  ACS Motion 의 상태를 갱신하는 타이머
            //timer_ACS_Status = new System.Windows.Forms.Timer();
            //timer_ACS_Status.Interval = 30;
            //timer_ACS_Status.Tick += new System.EventHandler(Timer_ACSMotion_StatusFunc);
            //timer_ACS_Status.Enabled = false;

            //  ACS and Ajin Motion 의 상태를 갱신하는 타이머
            timer_Motion_Status = new System.Windows.Forms.Timer();
            //timer_Motion_Status.Interval = 50;
            timer_Motion_Status.Interval = 1;
            timer_Motion_Status.Tick += new System.EventHandler(Timer_Motion_StatusFunc);
            timer_Motion_Status.Enabled = true;

            //  Laser 1 Shot 을 위한 타이머
            timer_Laser1Shot = new System.Windows.Forms.Timer();
            //timer_Laser1Shot.Interval = 10;
            timer_Laser1Shot.Interval = 1;
            timer_Laser1Shot.Tick += new System.EventHandler(Timer_Laser1ShotFunc);
            timer_Laser1Shot.Enabled = false;

            //  Thread Start
            //ThreadStart();

            //PositionData_Show();


            //m_Stage = workStage.Stage;
            //this.m__2DMappingDataControl = new _2DMappingDataControl(m_Stage);
            //this.m__2DMappingDataControl.Location = new Point(200, 200);
            //this.Controls.Add(this.m__2DMappingDataControl);


            // 문서 생성후 뷰어에 지정
            //var doc = new DocumentDefault();                          //  Sirius1
            //SiriusViewer_Parameter.Document = doc;


            this.userRegistration = new FormUserRegistration();
        }


        private void PositionData_Show()
        {
            //int m_nIndex_Ver_Top = -1;
            //int m_nIndex_Ver_Mid = -1;
            //int m_nIndex_Ver_Bot = -1;
            //int m_nIndex_Hor_L = -1;
            //int m_nIndex_Hor_C = -1;
            //int m_nIndex_Hor_R = -1;
            //int m_nIndex_Reticle_Upper = -1;
            //int m_nIndex_Reticle_Lower = -1;
            //int m_nIndex_Packing = -1;

            //for (int i = 0; i < workStage.Config.Positions.Count; i++)
            //{
            //    //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
            //    if (workStage.Config.Positions[i].Name == "AlignPosition_Ver_Top")
            //    {
            //        m_nIndex_Ver_Top = i;
            //    }

            //    //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
            //    if (workStage.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
            //    {
            //        m_nIndex_Ver_Mid = i;
            //    }

            //    //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
            //    if (workStage.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
            //    {
            //        m_nIndex_Ver_Bot = i;
            //    }

            //    //  웨이퍼 얼라인 상태를 확인하는 위치 (LEFT)
            //    if (workStage.Config.Positions[i].Name == "AlignPosition_Hor_Left")
            //    {
            //        m_nIndex_Hor_L = i;
            //    }

            //    //  웨이퍼 얼라인 상태를 확인하는 위치 (CENTER)
            //    if (workStage.Config.Positions[i].Name == "AlignPosition_Hor_Center")
            //    {
            //        m_nIndex_Hor_C = i;
            //    }

            //    //  웨이퍼 얼라인 상태를 확인하는 위치 (RIGHT)
            //    if (workStage.Config.Positions[i].Name == "AlignPosition_Hor_Right")
            //    {
            //        m_nIndex_Hor_R = i;
            //    }

            //    //  Reticle 얼라인 상태를 확인하는 위치 (Upper Cam)
            //    if (workStage.Config.Positions[i].Name == "ReticleGlass_UpperCamera")
            //    {
            //        m_nIndex_Reticle_Upper = i;
            //    }

            //    //  Reticle 얼라인 상태를 확인하는 위치 (Lower Cam)
            //    if (workStage.Config.Positions[i].Name == "ReticleGlass_LowerCamera")
            //    {
            //        m_nIndex_Reticle_Lower = i;
            //    }

            //    //  Packing 할 때의 Elev. Z 위치
            //    if (workStage.Config.Positions[i].Name == "ProbeWafer_Packing")
            //    {
            //        m_nIndex_Packing = i;
            //    }
            //}

            ////  좌표 가져오기 (세로 방향 얼라인)
            //if (m_nIndex_Ver_Top != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("AlignPosition_Ver_Top");

            //    tb_Axis_Y_TOP.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y].ToString();
            //}
            //if (m_nIndex_Ver_Mid != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("AlignPosition_Ver_Middle");

            //    tb_Axis_Y_MID.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y].ToString();
            //}
            //if (m_nIndex_Ver_Bot != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("AlignPosition_Ver_Bottom");

            //    tb_Axis_Y_BOT.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y].ToString();
            //}

            ////  좌표 가져오기 (가로 방향 얼라인)
            //if (m_nIndex_Hor_L != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("AlignPosition_Hor_Left");

            //    tb_Axis_X_LEFT.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X].ToString();
            //}
            //if (m_nIndex_Hor_C != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("AlignPosition_Ver_Middle");

            //    tb_Axis_X_MID.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X].ToString();
            //}
            //if (m_nIndex_Hor_R != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("AlignPosition_Hor_Right");

            //    tb_Axis_X_RIGHT.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X].ToString();
            //}

            //if (m_nIndex_Reticle_Upper != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("ReticleGlass_UpperCamera");

            //    tb_Axis_U_ReticleCenter.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)nAxis.U].ToString();
            //    tb_Axis_V_ReticleCenter.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)nAxis.V].ToString();
            //    tb_Axis_W_ReticleCenter.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)nAxis.W].ToString();
            //}

            //if (m_nIndex_Reticle_Lower != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("ReticleGlass_LowerCamera");
            //}

            ////  레티클 글래스 위치
            //tb_Axis_X_ReticleCenter.Text = workStage.Config.ParamConfig.ReticleGlass_Vision_X_Pos.ToString();
            //tb_Axis_Y_ReticleCenter.Text = workStage.Config.ParamConfig.ReticleGlass_Vision_Y_Pos.ToString();
            //tb_Axis_Z_ReticleCenter.Text = workStage.Config.ParamConfig.ReticleGlass_Vision_Z_Pos.ToString();
            //tb_Axis_EZ_ReticleCenter_UpperCam.Text = workStage.Config.ParamConfig.ReticleGlass_Elev_Z_PAK_Pos.ToString();
            //tb_Axis_EZ_ReticleCenter_LowerCam.Text = workStage.Config.ParamConfig.ReticleGlass_Elev_Z_Wafer_Pos.ToString();

            //if (m_nIndex_Packing != -1)
            //{
            //    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("ProbeWafer_Packing");

            //    tb_Axis_EZ_Wafer_ProbeCard_Packing.Text = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)nAxis.EZ].ToString();
            //}


            ////  UVW-Stage 제한 위치
            //tb_Axis_U_Limit_Minus.Text = workStage.Config.ParamConfig.AlignLimit_UVW_U_Minus.ToString();
            //tb_Axis_U_Limit_Plus.Text = workStage.Config.ParamConfig.AlignLimit_UVW_U_Plus.ToString();
            //tb_Axis_V_Limit_Minus.Text = workStage.Config.ParamConfig.AlignLimit_UVW_V_Minus.ToString();
            //tb_Axis_V_Limit_Plus.Text = workStage.Config.ParamConfig.AlignLimit_UVW_V_Plus.ToString();
            //tb_Axis_W_Limit_Minus.Text = workStage.Config.ParamConfig.AlignLimit_UVW_W_Minus.ToString();
            //tb_Axis_W_Limit_Plus.Text = workStage.Config.ParamConfig.AlignLimit_UVW_W_Plus.ToString();

            //tb_Axis_UVW_Limit_Range.Text = Math.Abs(Convert.ToDouble(tb_Axis_U_Limit_Plus.Text) - Convert.ToDouble(tb_Axis_U_Limit_Minus.Text)).ToString();


            ////  Latch 검사 Offset (from Packing Pos)
            //tb_Axis_EZ_Latch_Check_Offset.Text = workStage.Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos.ToString();
        }


        private void ParameterSetting_SLD200_VisibleChanged(object sender, EventArgs e)
        {
            if (m_visionImageViewer_LowRes != null)
            {
                if (this.Visible == true)
                {
                    m_visionImageViewer_LowRes.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer_LowRes.StopUpdateTask();
                }
            }

            if (m_visionImageViewer_HighRes != null)
            {
                if (this.Visible)
                {
                    m_visionImageViewer_HighRes.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer_HighRes.StopUpdateTask();
                }
            }
        }

        private void m_IlluminatorControl_IlluminatorControlButton_Click(IlluminatorControl.ButtonType type)
        {
            if (type == IlluminatorControl.ButtonType.Save)
            {
                //Module module = workStage m_Owner.Owner as Module;               

                //string m_strRecipe = "";
                //RecipeInfo m_recipeInfo = new RecipeInfo();
                //m_recipeInfo = Equipment.GetCurrentRecipe();

                //if (m_recipeInfo != null)
                //{
                //    m_strRecipe = m_recipeInfo.Name;

                //    DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                //    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                //}
                //else
                //{
                //    DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                //    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //}

                //DataManager.Instance.ApplyConfigData(workStage);

                ////  Config 창 데이터 갱신을 위해서
                //Equipment.m_bRedraw_FormWorkStageParameterConfig = true;

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
            timer_Laser1Shot.Enabled = false;


            //Run_Laser1ShotTime_Func();


            timer_Laser1Shot.Enabled = true;
        }

        #endregion


        #region Thread

        public void ThreadStart()
        {
            //  Drilling Cycle Thread
            m_bParameterSettingThreadExit = false;
            m_parameterSettingThread = new Thread(new ThreadStart(OnParameterSettingStatusCycle));
            m_parameterSettingThread.Start();
        }

        public void ThreadStop()
        {
            m_bParameterSettingThreadExit = true;

            if (m_parameterSettingThread != null)
                m_parameterSettingThread.Join();
        }

        protected void OnParameterSettingStatusCycle()
        {
            while (true)
            {
                if (m_bParameterSettingThreadExit)
                {
                    break;
                }
                if (OnParameterSettingStatusRun() != 0) break;
                Thread.Sleep(1);
            }
        }

        protected int OnParameterSettingStatusRun()
        {
            int ret = 0;

            //ACSMotion_Status();             //  ACS Motion Status

            return ret;
        }
        #endregion

        void Timer_Motion_StatusFunc(object sender, EventArgs e)
        {
            timer_Motion_Status.Enabled = false;


            //ACSMotion_Status();
            //AJINMotion_Status();

            //  카메라 라이브 상태인지 표시
            if ((workStage.jigAligner_HighRes != null) && (workStage.jigAligner_LowRes != null))
            {
                if (workStage.jigAligner_HighRes.Camera.IsLiveOn && workStage.jigAligner_LowRes.Camera.IsLiveOn)
                {
                    btnUpperCamera_StartLive.BackColor = Color.LightGreen;
                }
                else
                {
                    btnUpperCamera_StartLive.BackColor = Color.LightGray;
                }
            }

            //  레시피 변경 시, 해당 레시피의 위치값으로 변경
            if (workStage.m_bParameterSetting_PosData_Reload)
            {
                workStage.m_bParameterSetting_PosData_Reload = false;

                PositionData_Show();                  //  중요 : 프로젝트명 변경 시, Setup -> Equipment -> Motion 에서 축 다시 등록해야 함.
            }

            timer_Motion_Status.Enabled = true;
        }

        //private void ACSMotion_Status()
        //{
        //    double m_dPos = 0.0;
        //    int fault = 0;

        //    if (ACSSPiiPlusMotionBoard.Api.IsConnected)
        //    {
        //        //if (workStage.Config.ParamConfig.DoorInterlock_Enable)                                               //  도어 인터락 활성화 상태이고,
        //        //{
        //        //    if (!workStage.workStageParameter.DI_Safety())                                                    //  문이 열려 있으면? --> RVA 축을 제외한 모든 축 ServoOff
        //        //    {
        //        //        //  ACS Motion
        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)                                             //  축이 Enable(Servo On) 상태이면? --> Disable(Servo Off)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //        }

        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //        }
        //        //    }
        //        //}
        //        //else if (workStage.Config.ParamConfig.OpDoorInterlock_Enable)                                        //  OP 패널 도어 인터락 활성화 상태이고,
        //        //{
        //        //    if (!workStage.workStageParameter.DI_Safety())                                                    //  문이 열려 있으면? --> RVA 축을 제외한 모든 축 ServoOff
        //        //    {
        //        //        //  ACS Motion
        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)                                             //  축이 Enable(Servo On) 상태이면? --> Disable(Servo Off)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //        }

        //        //        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
        //        //        {
        //        //            ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //            ACSSPiiPlusMotionBoard.Api.Disable((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //        }
        //        //    }
        //        //}
        //        //else                                                                                                    //  도어 인터락 사용 안할 경우 --> 모든 축 ServoOn
        //        //{
        //        //    //  ACS Motion
        //        //    m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //    if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
        //        //    {
        //        //        ACSSPiiPlusMotionBoard.Api.Enable((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        //    }

        //        //    m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //    if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
        //        //    {
        //        //        ACSSPiiPlusMotionBoard.Api.Enable((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        //    }
        //        //}



        //        //m_dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)workStage.workStageParameter.Axes[WorkStageParameter.MotionKey.X.ToString()].No);
        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        baseLabelX_Enc.Text = String.Format("{0:F3}", m_dPos);

        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);
        //        baseLabelY_Enc.Text = String.Format("{0:F3}", m_dPos);

        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);
        //        baseLabelX_Ref.Text = String.Format("{0:F3}", m_dPos);

        //        m_dPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);
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
        //                            case (int)WorkStageParameter.MotionKey.X: baseLabel_LimitCheck_X_Left.BackColor = Color.Red; break;
        //                            case (int)WorkStageParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Fwd.BackColor = Color.Red; break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WorkStageParameter.MotionKey.X: baseLabel_LimitCheck_X_Left.BackColor = Color.Maroon; break;
        //                            case (int)WorkStageParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Fwd.BackColor = Color.Maroon; break;
        //                        }
        //                    }

        //                    if ((fault & (int)SafetyControlMasks.ACSC_SAFETY_RL) != 0)              //  POT
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WorkStageParameter.MotionKey.X: baseLabel_LimitCheck_X_Right.BackColor = Color.Red; break;
        //                            case (int)WorkStageParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Bwd.BackColor = Color.Red; break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        switch (i)
        //                        {
        //                            case (int)WorkStageParameter.MotionKey.X: baseLabel_LimitCheck_X_Right.BackColor = Color.Maroon; break;
        //                            case (int)WorkStageParameter.MotionKey.Y: baseLabel_LimitCheck_Y_Bwd.BackColor = Color.Maroon; break;
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


        private void btnMoveLoadingPos_Click(object sender, EventArgs e)
        {
            //  

            //if (workStage.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
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

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Center 를 카메라가 보고 있는 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] = m_dCurPos_X + workStage.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] = m_dCurPos_Y + workStage.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageY,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageX,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X]);                           //  Target position
        }

        private void btnMoveUnloadingPos_Click(object sender, EventArgs e)
        {
            //  

            //if (workStage.m_bRVA_CalibMode)                  //  RVA 조정 모드일 경우
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

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 현재 Scanner Center 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] = m_dCurPos_X - workStage.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] = m_dCurPos_Y - workStage.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageY,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageX,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X]);                           //  Target position
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

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 현재 Scanner Center 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] = m_dCurPos_X + workStage.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] = m_dCurPos_Y + workStage.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageY,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageX,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X]);                           //  Target position
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

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Center 를 카메라가 보고 있는 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] = m_dCurPos_X - workStage.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] = m_dCurPos_Y - workStage.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageY,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WorkStageParameter.AxisAcsEnum.StageX,
            //                                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X]);                           //  Target position
        }


        private void btnTest_AutoFocus_Click(object sender, EventArgs e)
        {
            //  테스트 (오토 포커스)
            workStage.autoFocuser_HighRes.Work();

            workStage.autoFocusResult_HighRes = workStage.autoFocuser_HighRes.Result;

            MessageBox.Show(workStage.autoFocusResult_HighRes.BestFocusPosition.ToString());
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
            //    //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);
            //    //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)                                             //  축이 Enable(Servo On) 상태이면? --> Disable(Servo Off)
            //    //{
            //    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageX);
            //    //    ACSSPiiPlusMotionBoard.Api.Disable((Axis)WorkStageParameter.AxisAcsEnum.StageX);
            //    //}

            //    //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //    //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
            //    //{
            //    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //    //    ACSSPiiPlusMotionBoard.Api.Disable((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //    //}
            //}

            if (Equipment.AjinBoard_Opened)
            {
                //  Ajin Motion
                //if (MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.InspectionZ))                      //  축이 Servo On 상태이면? --> Servo Off
                //{
                //    MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.InspectionZ, 1000);
                //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.InspectionZ, false);
                //}

                //if (MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.ScannerZ))
                //{
                //    MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.ScannerZ, 1000);
                //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.ScannerZ, false);
                //}

                //if (MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.StageTH))
                //{
                //    MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.StageTH, 1000);
                //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.StageTH, false);
                //}
            }

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
            //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageX);
            //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
            //{
            //    ACSSPiiPlusMotionBoard.Api.Enable((Axis)WorkStageParameter.AxisAcsEnum.StageX);
            //}

            //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) == 0)
            //{
            //    ACSSPiiPlusMotionBoard.Api.Enable((Axis)WorkStageParameter.AxisAcsEnum.StageY);
            //}

            ////  Ajin Motion
            //if (!MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.InspectionZ))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.InspectionZ, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.ScannerZ))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.ScannerZ, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.StageTH))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.StageTH, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.RvaX))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.RvaX, true);
            //}

            //if (!MC_Func.MC_IsServoOn((int)WorkStageParameter.AxisAjinEnum.RvaZ))
            //{
            //    MC_Func.MC_SetServoOnOff((int)WorkStageParameter.AxisAjinEnum.RvaZ, true);
            //}

            var mb2 = new MessageBoxOk();
            mb2.ShowDialog("Information !", "전 축 서보 ON 되었습니다.");
            return;
        }

        private void btnUpperCamera_Init_Click(object sender, EventArgs e)
        {
            //  카메라 초기화

            Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                workStage.Camera_HighRes.SetRunStatus(Part.RunStatus.Run);
                workStage.Camera_LowRes.SetRunStatus(Part.RunStatus.Run);
                workStage.Camera_HighRes.Initialize();
                workStage.Camera_LowRes.Initialize();
                return 0;
            });

            ProgressForm ProgressForm = new ProgressForm(workStage.Name, "Camera Initializing...", task, workStage.Camera_HighRes);
            ProgressForm.StopProcess += ProgressForm_StopProcess;
            ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            ProgressForm.ShowDialog();

            workStage.Camera_HighRes.Initialize();
            workStage.Camera_LowRes.Initialize();


            if ((workStage.Camera_LowRes.Resolution.Width == workStage.MAX_IMAGE_WIDTH) || (workStage.Camera_LowRes.Resolution.Height == workStage.MAX_IMAGE_HEIGHT))
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Calibration 화면", "카메라 초기화, Wafer 카메라 해상도 최대");
                //MessageBox.Show("Wafer 카메라 해상도가 최대입니다.\r\n\r\n[레티클 얼라인을 위해서는 Wafer 카메라의 이미지 해상도를 변경해야 합니다.]\r\n[Width : 2248,\tHeight : 1880]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer 카메라 해상도가 최대입니다.\r\n(아래 값으로 변경 요망)\r\n\r\n[Width : 2248, Height : 1880]");
                return;
            }
            else if ((workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X == 0) || (workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y == 0))
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Calibration 화면", "카메라 초기화, Wafer 카메라 이미지 Offset 값 0");

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer 카메라 - 이미지 Offset 값이 0 입니다.     (0 < X < 200, 0 < Y < 168)\r\n\r\n[ Reticle Glass Center 조정 필요 ]");
                return;
            }
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
            //  카메라 연결 확인
            if (!workStage.Camera_HighRes.Opened ||
                !workStage.Camera_LowRes.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            if (workStage.Camera_HighRes != null)
            {
                workStage.Camera_HighRes.StartLive();
            }

            if (workStage.Camera_LowRes != null)
            {
                workStage.Camera_LowRes.StartLive();
            }
        }        
    }
}
