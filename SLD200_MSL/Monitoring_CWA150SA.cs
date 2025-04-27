using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using QMC.Core;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Hmi;
using QMC.Common.Vision;
using SerialCommChiller;
using SerialCommLaserPowerMeter1;
using SerialCommLaserPowerMeter2;
using SerialCommSpectraPhysics;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Modules.Loader;
using QMC.Common.Motion.ACS.Motions;
using ACS.SPiiPlusNET;
using static QMC.Common.Equipment;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using netDxf.Entities;
using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;
using MessageBox = System.Windows.Forms.MessageBox;
using static QMC.Common.Parts.WorkStageParameter;
using QMC.Common.UI;
using Point = System.Drawing.Point;
using MessageBoxOk = QMC.Core.MessageBoxOk;
using MessageBoxYesNo = QMC.Core.MessageBoxYesNo;
using Bitmap = System.Drawing.Bitmap;
using QMC.Common.Vision.Cameras;
using System.IO.Ports;

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
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    public partial class Monitoring_CWA150SA : UserControl
    {
        #region Machine Status

        public enum MachineStatus : int
        {
            STATUS_NONE = 0,

            INITIALIZE_REQUIRED = 1,                //  장비 초기화가 필요합니다.
            MACHINE_INITIALIZING = 2,               //  장비 초기화 중...
            MACHINE_READY = 3,                      //  대기

            WAFER_ALIGN = 5,                        //  Wafer Align 중...

            WAFER_ALIGN_ERROR_CHECK = 6,            //  Wafer Align 오차 확인중...

            RETICLE_CHECK_UPPER = 7,                //  Reticle Align 중... (상부 카메라)
            RETICLE_CHECK_LOWER = 8,                //  Reticle Align 중... (하부 카메라)

            SAFETY_POSITION_MOVE = 9,               //  안전 위치로 이동 중...

            WAFER_LOADING_READY = 10,               //  웨이퍼 투입 대기 위치로 이동 중...
            PROBE_PACKING = 11,                     //  프로브 카드 패킹 중...

            PROBE_UNPACKING = 12,                   //  웨이퍼, 프로브 카드 언패킹 중...
            PROBE_UNPACKING_READY = 13,             //  웨이퍼, 프로브 카드 언패킹 위치로 이동 중...

            PROBE_CARD_LOADING_READY = 14,          //  프로브 카드 투입 대기 위치로 이동 중...
            PROBE_CARD_LOCKING = 15,                //  프로브 카드 고정 진행 중...

            LOGIN_REQUIRED = 16,                    //  로그인이 필요합니다.
            AUTO_LOGOUT = 17,                       //  자동으로 로그아웃 되었습니다.

            EMERGENCY_STOP = 18,                    //  비상정지 상태입니다.

            SAFETY_SENSOR_PAUSE = 19,               //  안전센서 감지로 인한 일시정지 상태입니다.
            SAFETY_SENSOR_STOP = 20,                //  안전센서 감지로 인한 장비 정지 !!

            ELEV_Z_OVER_TORQUE = 21,                //  Z축 엘리베이터 오버토크 발생    

            PAK_LATCH_CHECK = 22,                   //  PAK Latch Check 중...    
        }
            


        
        //m_nProbeCard_Locking_Step = (int) ProbeCard_Locking_Step.None;
        //m_nProbeCard_Loading_Ready_Step = (int) ProbeCard_Loading_Ready_Step.None;


        public int m_nMachineStatus { get; set; }

        #endregion


        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }

        //public JogControl m_JogControl;
        public MotionAxis thetaValue { set; get; }

        static WorkStage workStage;
        static Loader loader;

        public ProgressForm m_FormProgress;                             //  장비 초기화 시 진행창 표시
        public bool m_bHomeProgress_Show;

        InterpolatorMotionFunction MC_Func = new InterpolatorMotionFunction();


        private Thread m_MainWorkCycleThread;                           //  Main-Work Cycle. (Wafer Align, Packing, Unpacking, Safely Unpacking)
        private bool m_bMainWorkCycleExit;

        private Thread m_SubWorkCycleThread;                            //  Sub-Work Cycle. (Safety Position Move, Wafer Loading Ready, ProbeCard Loading Ready, ProbeCard Locking, WaferProbeCard Unpacking Ready, Align Error Check, PAK AirLine Check, Align_9Point_Verification)
        private bool m_bSubWorkCycleExit;

        private Thread m_MainAlignVisionCycleThread;                    //  Align Vision 만 돌리는 Thread
        private bool m_bMainAlignVisionCycleExit;

        private Thread m_StatusCheckCycleThread;                        //  상태 확인 Cycle. (IO)
        private bool m_bStatusCheckCycleExit;

        //Run_Safety_Position_Move_Func();
        //Run_WaferLoading_Ready_Func();
        //Run_ProbeCard_Loading_Ready_Func();
        //Run_ProbeCard_Locking_Func();
        //Run_WaferProbeCard_Unpacking_Ready_Func();

        //Run_WorkStage_ErrorCheck_Func();
        //Run_PAK_AirLine_Check_Func();

        //Run_WorkStage_9Point_Verification_Func();


        private bool m_bStartBtn_Clicked { get; set; }
        private bool m_bStopBtn_Clicked { get; set; }
        private bool m_bEmgBtn_Clicked { get; set; }

        System.Diagnostics.Stopwatch sw_Test = new System.Diagnostics.Stopwatch();

        static int marginWidth = 10;
        static int marginHeight = 10;
        static int btnWidth = 50;
        static int btnHeight = 50;

        public string m_strWorkingStatus_Message { get; set; }

        public bool m_bStartBtn_Status;
        public bool m_bStartBtn_Status_Before;
        public int m_nStartBtn_Ignore_Time;
        public bool m_bStopBtn_Status;
        public bool m_bStopBtn_Status_Before;
        public int m_nStopBtn_Ignore_Time;
        public bool m_bResetBtn_Status;
        public bool m_bResetBtn_Status_Before;
        public int m_nResetBtn_Ignore_Time;


        //  자동 로그아웃 관련 변수
        bool m_bAutoLogOut_Counting;
        double m_dAutoLogOut_Total;
        int m_nAutoLogOut_TickStart;
        bool m_bAutoLogOut_TickStart_1time;


        //  안전센서 감지로 인한 Pause 인지
        bool m_bPause_by_SafetySensor = false;
        bool m_bMotionStopped_by_SafetySensor = false;              //  단일 동작중 안전센서 터치로 모터가 정지했을 경우
        bool m_bMotionRunning = false;
        bool m_bDetected_SafetySensor_In_MotionRunning = false;     //  단일 동작중 모터가 구동중일 때 안전센서를 터치하는지 체크
        bool m_bCycleStopped_by_SafetySensor = false;               //  Cycle 동작


        //  Test용 변수
        bool m_bRun;
        string m_strTemp;
        int m_nMonitoring_Blink;
        bool m_bMonitoring_Blink;
        int m_nMonitoring_Blink_Fast;
        bool m_bMonitoring_Blink_Fast;

        int m_nLedBar_Blink;
        int m_nLedBar_Blink_Step;       //  0:All,  1 ~ 3

        #region Recipe List Property
        public RecipeInfo CurrentRecipe { get; set; }
        public FormRecipePart m_FormRecipePart;
        #endregion

        //  집진기 동작 관련 변수
        bool m_bDustCollector_On { get; set; }
        int m_nDustCollector_RunCount { get; set; }
        double m_dDustCollector_AutoShutdown { get; set; }

        public string m_strFullPath { get; set; }
               


        #region Recipe List Method
        public void Recipe_Init()
        {
            if (Equipment.GetCurrentRecipe() != null)
            {
                this.baseTextBoxCurrentRecipe.Text = Equipment.GetCurrentRecipe().Name;
            }

            FormManager.UpdateRecipe += OnnUpdateRecipe;
        }
        private void OnnUpdateRecipe()
        {
            if (Equipment.GetCurrentRecipe() != null)
            {
                CurrentRecipe = Equipment.GetCurrentRecipe();
                this.baseTextBoxCurrentRecipe.Text = Equipment.GetCurrentRecipe().Name;
            }
        }
        private void OnChangedCurrentRecipe()
        {
            Equipment.SetCurrentRecipe(CurrentRecipe);
            FormManager.FireUpdateRecipeEvent();
        }
        public void SetEnable(bool enable)
        {
            this.baseButtonChangeRecipe.Enabled = enable;
        }
        #endregion


        public Monitoring_CWA150SA()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();

            this.MouseMove += Monitoring_CWA150SA_MouseMove;

            //InitColorStatus();          //  Color 세팅
            //InitCarrierArray();         //  PCB 상태 보여주는 버튼 배열 초기화

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
            }

            //  배경화면 색상 변경
            this.BackColor = Color.FromArgb(220, 220, 220);

            //workStage.Machine_Parameter_Load();

            this.m_visionImageViewer_LowRes.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_LowRes.SuspendDisplay();
            this.m_visionImageViewer_HighRes.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_HighRes.SuspendDisplay();

            this.m_visionImageViewer_LowRes.Camera = workStage.Camera_LowRes;
            this.m_visionImageViewer_HighRes.Camera = workStage.Camera_HighRes;

            //  카메라 초기화 시 이미지 Offset 설정
            workStage.Camera_LowRes.MyConfig.OffsetX = (uint)workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
            workStage.Camera_LowRes.MyConfig.OffsetY = (uint)workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;

            this.VisibleChanged += Monitoring_CWA150SA_VisibleChanged;

            //  Jog Control
            //m_JogControl = new JogControl(workStage.Stage);
            ////m_JogControl.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 10, m_ModuleStateControl.Location.Y);
            //m_JogControl.Location = new Point(baseLabel_WaferChuck_Camera.Location.X + baseLabel_WaferChuck_Camera.Size.Width + 30, 285);
            //this.Controls.Add(m_JogControl);

            m_nMachineStatus = (int)MachineStatus.STATUS_NONE;

            //timer_DIO_Status.Enabled = true;        //  메인 화면 IO 갱신 타이머

            //if (workStage.m_bAlignVisionThread_Use && !Equipment.m_bAlignVisionThread_1time)              //  Thread 한번만 실행
            //{
            //    Equipment.m_bAlignVisionThread_1time = true;

            //    ThreadStart();
            //}


            //  요거는 나중에 주석 해제한다. (자꾸 이것저것 뜸)

            ////  라이브러리 초기화
            //SpiralLab.Core.Initialize();
            ////            SpiralLab.Core.Ini

            ////SiriusViewer_Main = new SpiralLab.Sirius.SiriusViewerForm();

            //// 문서 생성후 뷰어에 지정
            //var doc = new DocumentDefault();
            //SiriusViewer_Main.Document = doc;

            ////Equipment.EqpSiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();


            m_nMonitoring_Blink = 0;
            m_bMonitoring_Blink = false;
            m_nMonitoring_Blink_Fast = 0;
            m_bMonitoring_Blink_Fast = false;
            m_nLedBar_Blink = 0;
            m_nLedBar_Blink_Step = 0;

            m_bStartBtn_Clicked = false;
            m_bStopBtn_Clicked = false;

            m_bStartBtn_Status = false ;
            m_bStartBtn_Status_Before = false;
            m_nStartBtn_Ignore_Time = 0;
            m_bStopBtn_Status = false;
            m_bStopBtn_Status_Before = false;
            m_nStopBtn_Ignore_Time = 0;
            m_bResetBtn_Status = false;
            m_bResetBtn_Status_Before = false;
            m_nResetBtn_Ignore_Time = 0;

            m_dDustCollector_AutoShutdown = 10.0;

            FormManager.UpdateRecipe += FormManager_UpdateRecipe;

            if (Equipment.GetCurrentRecipe() != null)
            {
                CurrentRecipe = Equipment.GetCurrentRecipe();
                this.baseTextBoxCurrentRecipe.Text = Equipment.GetCurrentRecipe().Name;
            }

            //listView_JobList.BeginUpdate();
            ListViewItem item;

            m_strFullPath = "";

            //item = new ListViewItem("Order");
            //item.SubItems.Add("Job File Name");
            //listView_JobList.Items.Add(item);

            SecondToTime(0);

            ////  컬럼명과 컬럼사이즈 지정
            //listView_JobList.Columns.Add("Order", 60, HorizontalAlignment.Center);
            //listView_JobList.Columns.Add("파일명", 325, HorizontalAlignment.Center);
            //listView_JobList.Columns.Add("파일위치", 0, HorizontalAlignment.Center);

            //listView_JobList.EndUpdate();

            m_FormProgress = new ProgressForm("Initialize", "장비 초기화 진행중...");
            m_bHomeProgress_Show = false;

            //  Thread Start
            //ThreadStop();
            //ThreadStart();

            m_strWorkingStatus_Message = "";


            //  자동 로그아웃 타임
            m_bAutoLogOut_Counting = false;
            m_dAutoLogOut_Total = workStage.Config.ParamConfig.Auto_LogOut_Time <= 0 ? 10 : workStage.Config.ParamConfig.Auto_LogOut_Time;
            m_dAutoLogOut_Total = m_dAutoLogOut_Total * 60 * 1000;
            m_nAutoLogOut_TickStart = 0;
            m_bAutoLogOut_TickStart_1time = false;


            //  2024. 10. 14.  SCH : 바코드 사용할 때 살리자
            //if (!Equipment.m_bBarcodeReaderComm_1time)
            //{
            //    Equipment.m_bBarcodeReaderComm_1time = true;

            //    //  바코드 리더기 (COM4)
            //    if (workStage.m_BarcodeReader_Comm == null)
            //    {
            //        workStage.BarcodeReaderComm_Init();
            //    }
            //    else
            //    {
            //        if (!workStage.m_BarcodeReader_Comm.IsOpen)
            //            workStage.BarcodeReaderComm_Init();
            //    }
            //}


            m_bEmgBtn_Clicked = false;
        }

        private void Monitoring_CWA150SA_MouseMove(object sender, MouseEventArgs e)
        {
            //  마우스를 움직이면 발생하는 이벤트

            int a = 0;
        }



        private void Monitoring_CWA150SA_VisibleChanged(object sender, EventArgs e)
        {
            if (m_visionImageViewer_LowRes != null)
            {
                if (this.Visible == true)
                {
                    m_visionImageViewer_LowRes.StartUpdateTask();
                    //AddOverlay(visionImageViewer);
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

        private void FormManager_UpdateRecipe()
        {
            if (Equipment.GetCurrentRecipe() != null)
            {
                CurrentRecipe = Equipment.GetCurrentRecipe();
                this.baseTextBoxCurrentRecipe.Text = Equipment.GetCurrentRecipe().Name;
            }
        }

        public void InitComm()
        {
            //  Laser Source Comm 연결은 "SingleMode" 창에서....
            ////  Laser Source (COM2)
            //if (workStage.m_spectraPhysicsLaserComm == null)
            //{
            //    workStage.SpectraPhysicsLaserComm_Init();
            //}
            //else
            //{
            //    if (!workStage.m_spectraPhysicsLaserComm.IsOpen)
            //        workStage.SpectraPhysicsLaserComm_Init();
            //}

        }

        #region Method

        private void AddOverlay(VisionImageViewer viewer)
        {
            if (viewer == null) return;
            if (viewer.NormalOverlays != null)
                viewer.NormalOverlays.Clear();

            DrawCrossLine(viewer);
            DrawEllipse(viewer);
        }

        private void DrawCrossLine(VisionImageViewer viewer)
        {
            EllipseFrameVisionImageOverlay overlay = new EllipseFrameVisionImageOverlay();

            int width = workStage.Camera_HighRes.Resolution.Width;
            int height = workStage.Camera_HighRes.Resolution.Height;

            overlay.Width = 50;
            overlay.Height = 50;
            overlay.StartLocation = new System.Drawing.Point(width / 2 - overlay.Width / 2, height / 2 - overlay.Height / 2);
            overlay.Color = Color.LimeGreen;
            overlay.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            overlay.Thickness = 1;
            overlay.Visible = true;

            viewer.NormalOverlays.Add(overlay);
        }

        private void DrawEllipse(VisionImageViewer viewer)
        {
            LineFrameVisionImageOverlay horizontalOverlay = new LineFrameVisionImageOverlay();
            LineFrameVisionImageOverlay verticalOverlay = new LineFrameVisionImageOverlay();

            int width = workStage.Camera_HighRes.Resolution.Width;
            int height = workStage.Camera_HighRes.Resolution.Height;

            horizontalOverlay.StartLocation = new System.Drawing.Point(0, height / 2);
            horizontalOverlay.EndLocation = new System.Drawing.Point(width, height / 2);
            horizontalOverlay.Color = Color.Red;
            horizontalOverlay.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            horizontalOverlay.Thickness = 1;
            horizontalOverlay.Visible = true;

            viewer.NormalOverlays.Add(horizontalOverlay);

            verticalOverlay.StartLocation = new System.Drawing.Point(width / 2, 0);
            verticalOverlay.EndLocation = new System.Drawing.Point(width / 2, height);
            verticalOverlay.Color = Color.Red;
            verticalOverlay.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            verticalOverlay.Thickness = 1;
            verticalOverlay.Visible = true;

            viewer.NormalOverlays.Add(verticalOverlay);
        }
        #endregion


        #region Main Cycle Thread

        public void ThreadStart()
        {
            ////  Main-Work Cycle Thread
            //m_bMainWorkCycleExit = false;
            //m_MainWorkCycleThread = new Thread(new ThreadStart(OnMainWorkCycle));
            //m_MainWorkCycleThread.Start();

            ////  Sub-Work Cycle Thread
            //m_bSubWorkCycleExit = false;
            //m_SubWorkCycleThread = new Thread(new ThreadStart(OnSubWorkCycle));
            //m_SubWorkCycleThread.Start();

            //  Align Vision Cycle Thread
            m_bMainAlignVisionCycleExit = false;
            m_MainAlignVisionCycleThread = new Thread(new ThreadStart(OnMainAlignVisionCycle));
            m_MainAlignVisionCycleThread.Start();

            ////  Status Check Cycle Thread
            //m_bStatusCheckCycleExit = false;
            //m_StatusCheckCycleThread = new Thread(new ThreadStart(OnStatusCheckCycle));
            //m_StatusCheckCycleThread.Start();
        }

        public void ThreadStop()
        {
            //m_bMainWorkCycleExit = true;
            //m_bSubWorkCycleExit = true;
            m_bMainAlignVisionCycleExit = true;

            //if (m_MainWorkCycleThread != null)
            //{
            //    m_MainWorkCycleThread.Join();
            //}

            //if (m_SubWorkCycleThread != null)
            //{
            //    m_SubWorkCycleThread.Join();
            //}

            if (m_MainAlignVisionCycleThread != null)
            {
                m_MainAlignVisionCycleThread.Join();
            }

            //if (m_StatusCheckCycleThread != null)
            //{
            //    m_StatusCheckCycleThread.Join();
            //}
        }

        public void Device_Close()
        {
            if (workStage.Stage != null)
            {
                workStage.Stage.Close();
            }

            if (workStage.Camera_HighRes != null)
            {
                workStage.Camera_HighRes.Close();
            }

            if (workStage.Camera_LowRes != null)
            {
                workStage.Camera_LowRes.Close();
            }

            if (workStage.m_powerMeter_ExitPos_Comm != null)
            {
                workStage.m_powerMeter_ExitPos_Comm.CloseComm();
                workStage.m_powerMeter_ExitPos_Comm.Close();
            }

            if (workStage.m_powerMeter_TargetPos_Comm != null)
            {
                workStage.m_powerMeter_TargetPos_Comm.CloseComm();
                workStage.m_powerMeter_TargetPos_Comm.Close();
            }

            if (workStage.m_beamExpander_Comm != null)
            {
                workStage.m_beamExpander_Comm.CloseComm();
                workStage.m_beamExpander_Comm.Close();
            }

            if (workStage.m_dustCollector_UpperPos_Comm != null)
            {
                workStage.m_dustCollector_UpperPos_Comm.CloseComm();
                workStage.m_dustCollector_UpperPos_Comm.Close();
            }

            if (workStage.m_dustCollector_LowerPos_Comm != null)
            {
                workStage.m_dustCollector_LowerPos_Comm.CloseComm();
                workStage.m_dustCollector_LowerPos_Comm.Close();
            }

            if (workStage.m_electroRegulator_Comm != null)
            {
                workStage.m_electroRegulator_Comm.CloseComm();
                workStage.m_electroRegulator_Comm.Close();
            }
        }

        protected void OnMainWorkCycle()
        {
            while (true)
            {
                if (m_bMainWorkCycleExit)
                {
                    break;
                }
                if (OnMainWorkRun() != 0) break;

                Thread.Sleep(10);
            }
        }
        
        protected int OnMainWorkRun()
        {
            int ret = 0;

            //  Main-Work
            workStage.forThread_MainWorkCycle();

            return ret;
        }

        protected void OnSubWorkCycle()
        {
            while (true)
            {
                if (m_bSubWorkCycleExit)
                {
                    break;
                }
                if (OnSubWorkRun() != 0) break;

                Thread.Sleep(10);
            }
        }

        protected int OnSubWorkRun()
        {
            int ret = 0;

            //  Sub-Work
            workStage.forThread_SubWorkCycle();

            return ret;
        }


        protected void OnMainAlignVisionCycle()
        {
            while (true)
            {
                if (m_bMainAlignVisionCycleExit)
                {
                    break;
                }
                if (OnAlignVisionRun() != 0)
                {
                    break;
                }

                Thread.Sleep(10);
            }
        }

        protected int OnAlignVisionRun()
        {
            int ret = 0;

            workStage.forThread_AlignVisionCycle();
            //Thread.Sleep(1);

            return ret;
        }

        protected void OnStatusCheckCycle()
        {
            while (true)
            {
                if (m_bStatusCheckCycleExit)
                {
                    break;
                }
                if (OnStatusCheckRun() != 0) break;

                Thread.Sleep(10);
            }
        }

        protected int OnStatusCheckRun()
        {
            int ret = 0;

            //  Status Check
            MainUI_Status_Check();

            return ret;
        }

        #endregion

        #region SetButtonName
        public void SetButtonName(string name)
        {
            //this.baseLabelTheta.Text = name;
        }
        #endregion

        #region buttonClickEvent

        private void buttonCW_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonCCW_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion

        #region buttonDownEvent

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #region buttonUpEvent

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {

        }

        #endregion


        private void ledDispenser_ContactSensorAir_On_Click(object sender, EventArgs e)
        {
            //ledStatus_BCR_Connected.Image = global::SLD200.Properties.Resources.StopOff;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                DioPoint dioPoint = point;
            }
        }

        private void ledDispenser_ContactSensorAir_Off_Click(object sender, EventArgs e)
        {
            //ledStatus_BCR_Connected.Image = global::SLD200.Properties.Resources.DioEllipseOn;
        }

        private void timer_DIO_Status_Tick(object sender, EventArgs e)
        {
            timer_DIO_Status.Enabled = false;

            //  테스트 : 메인 화면 DIO 상태 갱신

            //  동작 Cyc. 중에 한군데라도 안전센서가 감지되면 Pause 처리        --> Pause 안함. 무조건 장비 Stop 으로 처리하기로 함. (온세미 이윤우 대리)
            //if (workStage.m_bReticleCheck_Pause_Start ||
            //    workStage.m_bSafetyPos_Pause_Start ||
            //    workStage.m_bWaferLoadingReady_Pause_Start ||
            //    workStage.m_bProbeCardLoadingReady_Pause_Start ||
            //    workStage.m_bWaferProbeCardUnpackingReady_Pause_Start ||
            //    workStage.m_bWaferProbeCardUnpacking_Pause_Start ||
            //    workStage.m_bProbeCardLocking_Pause_Start ||
            //    workStage.m_bWaferProbeCardPacking_Pause_Start ||
            //    workStage.m_bWorkStage_Pause_Start ||
            //    workStage.m_bWorkStageErrorCheck_Pause_Start)
            //{
            //    m_bPause_by_SafetySensor = true;
            //}
            //else
            //{
            //    m_bPause_by_SafetySensor = false;
            //}

            MainUI_DIO_Status();


            //  2025. 01. 13.  SCH : TOP 의 상태 Message 에 표시하도록 변경

            //if (!workStage.workStageParameter.DI_OpSwitch_EMG() &&
            //    //!m_bPause_by_SafetySensor &&
            //    !workStage.m_bInManualMoving_SafetySensor_Detected &&
            //    !workStage.m_bInCycleMoving_SafetySensor_Detected &&
            //    !workStage.m_bInCycleMoving_ElevZOverTorque_Detected)
            //{
            //    lblMachine_Status.BackColor = Color.Black;
            //    lblMachine_Status.ForeColor = Color.Yellow;
            //}

            ////  장비 상태 변경
            //if (workStage.workStageParameter.DI_OpSwitch_EMG())
            //{
            //    m_nMachineStatus = (int)MachineStatus.EMERGENCY_STOP;
            //    Set_Machine_Status((int)MachineStatus.EMERGENCY_STOP);

            //    if (m_bMonitoring_Blink)
            //    {
            //        lblMachine_Status.BackColor = Color.Black;
            //        lblMachine_Status.ForeColor = Color.Yellow;
            //    }
            //    else
            //    {
            //        lblMachine_Status.BackColor = Color.Yellow;
            //        lblMachine_Status.ForeColor = Color.Black;
            //    }
            //}
            //else if (!Equipment.LogIn_Status)
            //{
            //    if (Equipment.AutoLogOut_Executed)
            //    {
            //        m_nMachineStatus = (int)MachineStatus.AUTO_LOGOUT;
            //        Set_Machine_Status((int)MachineStatus.AUTO_LOGOUT);
            //    }
            //    else
            //    {
            //        m_nMachineStatus = (int)MachineStatus.LOGIN_REQUIRED;
            //        Set_Machine_Status((int)MachineStatus.LOGIN_REQUIRED);
            //    }
            //}
            //else if (!workStage.m_bHomeOK && (m_nMachineStatus != (int)MachineStatus.INITIALIZE_REQUIRED) &&
            //        (workStage.m_nHomeStep == (int)WorkStage.Home_Step.None))
            //{
            //    m_nMachineStatus = (int)MachineStatus.INITIALIZE_REQUIRED;
            //    Set_Machine_Status((int)MachineStatus.INITIALIZE_REQUIRED);
            //}
            //else if (!workStage.m_bHomeOK && (m_nMachineStatus != (int)MachineStatus.MACHINE_INITIALIZING) &&
            //        (workStage.m_nHomeStep >= (int)WorkStage.Home_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.MACHINE_INITIALIZING;
            //    Set_Machine_Status((int)MachineStatus.MACHINE_INITIALIZING);
            //}
            //else if (workStage.m_bHomeOK && 
            //    (workStage.m_bInManualMoving_SafetySensor_Detected || workStage.m_bInCycleMoving_SafetySensor_Detected))
            //{
            //    m_nMachineStatus = (int)MachineStatus.SAFETY_SENSOR_STOP;
            //    Set_Machine_Status((int)MachineStatus.SAFETY_SENSOR_STOP);

            //    if (m_bMonitoring_Blink)
            //    {
            //        lblMachine_Status.BackColor = Color.Black;
            //        lblMachine_Status.ForeColor = Color.Yellow;
            //    }
            //    else
            //    {
            //        lblMachine_Status.BackColor = Color.Yellow;
            //        lblMachine_Status.ForeColor = Color.Black;
            //    }
            //}
            //else if (workStage.m_bHomeOK && workStage.m_bInCycleMoving_ElevZOverTorque_Detected)
            //{
            //    m_nMachineStatus = (int)MachineStatus.ELEV_Z_OVER_TORQUE;
            //    Set_Machine_Status((int)MachineStatus.ELEV_Z_OVER_TORQUE);

            //    if (m_bMonitoring_Blink)
            //    {
            //        lblMachine_Status.BackColor = Color.Black;
            //        lblMachine_Status.ForeColor = Color.Yellow;
            //    }
            //    else
            //    {
            //        lblMachine_Status.BackColor = Color.Yellow;
            //        lblMachine_Status.ForeColor = Color.Black;
            //    }
            //}
            //else if (workStage.m_nWafer_ProbeCard_Packing_Step == (int)WorkStage.WaferProbeCard_Packing_Step.LatchStatus_OP_Wait)
            //{
            //    m_nMachineStatus = (int)MachineStatus.PAK_LATCH_CHECK;
            //    Set_Machine_Status((int)MachineStatus.PAK_LATCH_CHECK);

            //    if (m_bMonitoring_Blink)
            //    {
            //        lblMachine_Status.BackColor = Color.Black;
            //        lblMachine_Status.ForeColor = Color.Yellow;
            //    }
            //    else
            //    {
            //        lblMachine_Status.BackColor = Color.Yellow;
            //        lblMachine_Status.ForeColor = Color.Black;
            //    }
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nWorkStage_MainStep >= (int)WorkStage.WorkStage_Step.Start))
            //{
            //    if (workStage.m_nWorkStage_ErrorCheck_Step >= (int)WorkStage.WorkStageErrorCheck_Step.Start)
            //    {
            //        m_nMachineStatus = (int)MachineStatus.WAFER_ALIGN_ERROR_CHECK;
            //        Set_Machine_Status((int)MachineStatus.WAFER_ALIGN_ERROR_CHECK);
            //    }
            //    else
            //    {
            //        m_nMachineStatus = (int)MachineStatus.WAFER_ALIGN;
            //        Set_Machine_Status((int)MachineStatus.WAFER_ALIGN);
            //    }
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nReticleCheck_UpperCam_Step >= (int)WorkStage.ReticleCheck_UpperCam_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.RETICLE_CHECK_UPPER;
            //    Set_Machine_Status((int)MachineStatus.RETICLE_CHECK_UPPER);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nReticleCheck_LowerCam_Step >= (int)WorkStage.ReticleCheck_LowerCam_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.RETICLE_CHECK_LOWER;
            //    Set_Machine_Status((int)MachineStatus.RETICLE_CHECK_LOWER);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nSafetyPos_Move_Step >= (int)WorkStage.SafetyPos_Move_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.SAFETY_POSITION_MOVE;
            //    Set_Machine_Status((int)MachineStatus.SAFETY_POSITION_MOVE);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nWafer_Loading_Ready_Step >= (int)WorkStage.WaferLoading_Ready_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.WAFER_LOADING_READY;
            //    Set_Machine_Status((int)MachineStatus.WAFER_LOADING_READY);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nWafer_ProbeCard_Packing_Step >= (int)WorkStage.WaferProbeCard_Packing_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.PROBE_PACKING;
            //    Set_Machine_Status((int)MachineStatus.PROBE_PACKING);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nWaferProbeCard_Unpacking_Step >= (int)WorkStage.WaferProbeCard_Unpacking_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.PROBE_UNPACKING;
            //    Set_Machine_Status((int)MachineStatus.PROBE_UNPACKING);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nWaferProbeCard_Unpacking_Ready_Step >= (int)WorkStage.WaferProbeCard_Unpacking_Ready_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.PROBE_UNPACKING_READY;
            //    Set_Machine_Status((int)MachineStatus.PROBE_UNPACKING_READY);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nProbeCard_Loading_Ready_Step >= (int)WorkStage.ProbeCard_Loading_Ready_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.PROBE_CARD_LOADING_READY;
            //    Set_Machine_Status((int)MachineStatus.PROBE_CARD_LOADING_READY);
            //}
            //else if (workStage.m_bHomeOK && (workStage.m_nProbeCard_Locking_Step >= (int)WorkStage.ProbeCard_Locking_Step.Start))
            //{
            //    m_nMachineStatus = (int)MachineStatus.PROBE_CARD_LOCKING;
            //    Set_Machine_Status((int)MachineStatus.PROBE_CARD_LOCKING);
            //}
            //else if (workStage.m_bHomeOK)// && (workStage.m_nWorkStage_MainStep == (int)WorkStage.WorkStage_Step.None))
            //{
            //    m_nMachineStatus = (int)MachineStatus.MACHINE_READY;
            //    Set_Machine_Status((int)MachineStatus.MACHINE_READY);
            //}

            //  버튼 활성화 여부
            if (workStage.m_bHomeOK)
            {
                //btnMainWork_Start.Enabled = true;

                if (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
                {
                    if (!btnHomeAll.Enabled)        btnHomeAll.Enabled = true;
                }
                else
                {
                    if (btnHomeAll.Enabled)         btnHomeAll.Enabled = false;
                }
            }
            else
            {
                //if (btnJobFile_Load.Enabled)        btnJobFile_Load.Enabled = false;
                if (!btnHomeAll.Enabled)            btnHomeAll.Enabled = true;
                //if (btnMainWork_Start.Enabled)      btnMainWork_Start.Enabled = false;
            }

            ////  메인 화면 도면 갱신 (요상스럽도다... 메인 화면에 도면을 불러온 후 다른 화면으로 넘어갔다가 돌아오면, 메인 화면의 Viewer 에 도면이 사라진다. 보이기만 안보이는 게 아니라 데이터도 사라진다. 
            ////                      그래서 Equipment 에 SiriusView 를 하나 임시로 두고, 서로 데이터가 다를 경우(로드된 파일명) 임시 Viewer 의 데이터를 메인 화면의 Viewer 로 가져온다.
            //if ((SiriusViewer.Document != null) && (Equipment.EqpSiriusViewer.Document != null))
            //{
            //    if ((SiriusViewer.Document.FileName != Equipment.EqpSiriusViewer.Document.FileName) &&
            //        (workStage.m_nWorkStage_MainStep == (int)WorkStage.WorkStage_Step.None))            //  자동운전이 아닐 때만 데이터를 Copy 하도록
            //    {
            //        SiriusViewer.Document = Equipment.EqpSiriusViewer.Document;
            //        workStage.MainSiriusViewer.Document = Equipment.EqpSiriusViewer.Document;
            //    }
            //}

            //  서보 알람 상태 표시
            bool m_bAxisServoAlarm = false;
            
            //for ( int i = (int)WorkStageParameter.AxisAjinEnum.X ; i < (int)WorkStageParameter.AxisAjinEnum.Max ; i++ )         //  Laser Drilling 모듈의 Ajin 모션 축 수 가져오기 (Enum Count 확인)
            //{
            //    if (workStage.MC_Func.MC_IsAlarm(i))
            //    {
            //        m_bAxisServoAlarm = true;
            //    }
            //}

            //for (int i = (int)LoaderParameter.AxisAjinEnum.Z0 ; i < (int)LoaderParameter.AxisAjinEnum.Max ; i++)                        //  Loader 모듈의 Ajin 모션 축 수 가져오기 (Enum Count 확인)
            //{
            //    if (loader.MC_Func.MC_IsAlarm(i))
            //    {
            //        m_bAxisServoAlarm = true;
            //    }
            //}

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

            //  자동 로그아웃 타임
            //  어떤 Cycle 도 동작하지 않으면 자동 종료 카운트 진행
            //if (workStage.Config.ParamConfig.Auto_LogOut_Usage)
            //{
            //    if (Equipment.LogIn_Status &&
            //        //m_bAutoLogOut_Counting == false &&
            //        workStage.m_nHomeStep == (int)WorkStage.Home_Step.None &&
            //        workStage.m_nWorkStage_MainStep == (int)WorkStage.WorkStage_Step.None &&
            //        workStage.m_nWorkStage_ErrorCheck_Step == (int)WorkStageErrorCheck_Step.None &&
            //        workStage.m_nFindAlignMark_Step == (int)FindAlignMark_Step.None &&
            //        workStage.m_nReticleCheck_UpperCam_Step == (int)ReticleCheck_UpperCam_Step.None &&
            //        workStage.m_nReticleCheck_LowerCam_Step == (int)ReticleCheck_LowerCam_Step.None &&
            //        workStage.m_nSafetyPos_Move_Step == (int)WorkStage.SafetyPos_Move_Step.None &&
            //        workStage.m_nWafer_Loading_Ready_Step == (int)WaferLoading_Ready_Step.None &&
            //        workStage.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeCard_Packing_Step.None &&
            //        workStage.m_nWaferProbeCard_Unpacking_Step == (int)WaferProbeCard_Unpacking_Step.None &&
            //        workStage.m_nWaferProbeCard_Unpacking_Ready_Step == (int)WaferProbeCard_Unpacking_Ready_Step.None &&
            //        workStage.m_nProbeCard_Locking_Step == (int)ProbeCard_Locking_Step.None &&
            //        workStage.m_nProbeCard_Loading_Ready_Step == (int)ProbeCard_Loading_Ready_Step.None &&
            //        workStage.m_nPAK_AirLine_Check_Step == (int)PAK_AirLine_Check_Step.None &&
            //        workStage.m_nElevZ_PackingPos_Checking_Step == (int)ElevZ_PackingPos_Checking_Step.None &&
            //        workStage.m_nManualPacking_Step == (int)WorkStage.ManualPackingStep.NONE)
            //    {
            //        m_bAutoLogOut_Counting = true;

            //        if (!m_bAutoLogOut_TickStart_1time)
            //        {
            //            m_bAutoLogOut_TickStart_1time = true;
            //            m_nAutoLogOut_TickStart = Environment.TickCount;
            //        }
            //    }
            //    else
            //    {
            //        m_bAutoLogOut_Counting = false;
            //        m_bAutoLogOut_TickStart_1time = false;
            //        m_nAutoLogOut_TickStart = 0;

            //        Equipment.AutoLogOut_Execute = false;
            //    }

            //    if (m_bAutoLogOut_Counting && Equipment.LogIn_Status)
            //    {
            //        if ((Environment.TickCount - m_nAutoLogOut_TickStart) >= m_dAutoLogOut_Total)
            //        {
            //            m_bAutoLogOut_Counting = false;

            //            Equipment.AutoLogOut_Execute = true;                        
            //        }
            //    }
            //}

            timer_DIO_Status.Enabled = true;
        }

        private string SecondToTime(int m_nTotalSecond)
        {
            string m_strTime = "--h --m --s";

            //  시, 분, 초 선언
            int hours, minute, second;

            //시간공식
            hours = m_nTotalSecond / 3600;          //  시 공식
            minute = m_nTotalSecond % 3600 / 60;    //  분을 구하기 위해서 입력되고 남은값에서 또 60을 나눈다.
            second = m_nTotalSecond % 3600 % 60;    //  마지막 남은 시간에서 분을 뺀 나머지 시간을 초로 계산함

            m_strTime = string.Format("{0:00}h {1:00}m {2:00}s", hours, minute, second);

            return m_strTime;
        }

        private void MainUI_Machine_Status()
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            //  Gantry Status
            //
            //if (MC_Func.MC_GetGantryStatus(0))      //  Gantry 의 Master 축 번호 : 임시로 축 번호 고정. 
            //    pictureBoxGantryStatus.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxGantryStatus.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            //
            ///////////////////////////////////////////////////////////////////////////////////////
        }

        private void MainUI_DIO_Status()
        {
            //if (workStage.m_nHomeStep == (int)WorkStage.Home_Step.Complete)
            if (m_bHomeProgress_Show && workStage.m_bHomeOK)
            {
                m_bHomeProgress_Show = false;
                m_FormProgress.Hide();
            }

            //  도면 Load 했는지?
            //if ( workStage.DrillingData_Loaded )
            //{
            //    workStage.DrillingData_Loaded = false;

            //    SiriusViewer.Document = workStage.MainSiriusViewer.Document;
            //}

            //  Blink
            m_nMonitoring_Blink++;
            if ((m_nMonitoring_Blink > 0) && (m_nMonitoring_Blink <= 20))
            {
                m_bMonitoring_Blink = true;                
            }
            else if ((m_nMonitoring_Blink > 20) && (m_nMonitoring_Blink <= 40))
            {
                m_bMonitoring_Blink = false;
            }
            else
            {
                m_nMonitoring_Blink = 0;
            }

            //  Blink_Fast
            m_nMonitoring_Blink_Fast++;
            if ((m_nMonitoring_Blink_Fast > 0) && (m_nMonitoring_Blink_Fast <= 10))
            {
                m_bMonitoring_Blink_Fast = true;
            }
            else if ((m_nMonitoring_Blink_Fast > 10) && (m_nMonitoring_Blink_Fast <= 20))
            {
                m_bMonitoring_Blink_Fast = false;
            }
            else
            {
                m_nMonitoring_Blink_Fast = 0;
            }

            workStage.m_bBlink = m_bMonitoring_Blink;

            //  LED Bar
            m_nLedBar_Blink++;
            if ((m_nLedBar_Blink > 0) && (m_nLedBar_Blink <= 20))
            {
                m_nLedBar_Blink_Step = 1;
            }
            else if ((m_nLedBar_Blink > 20) && (m_nLedBar_Blink <= 40))
            {
                m_nLedBar_Blink_Step = 2;
            }
            else if ((m_nLedBar_Blink > 40) && (m_nLedBar_Blink <= 60))
            {
                m_nLedBar_Blink_Step = 3;
            }
            else
            {
                m_nLedBar_Blink = 0;
                m_nLedBar_Blink_Step = 1;
            }


            //  알람 발생 시 빨간색 LED Bar Blink
            if (Equipment.MachineStop_byAlarm == true)
            {
                //  TowerLamp
                if (CommonModule.Instance.TowerLamp.Is_Green_On() != 0)
                {
                    CommonModule.Instance.TowerLamp.Green_Off();
                }
                if (CommonModule.Instance.TowerLamp.Is_Yellow_On() != 0)
                {
                    CommonModule.Instance.TowerLamp.Yellow_Off();
                }
                if (CommonModule.Instance.TowerLamp.Is_Red_On() == 0)
                {
                    CommonModule.Instance.TowerLamp.Red_On();
                }
                if (CommonModule.Instance.TowerLamp.Is_Buzzer_On() != 0)
                {
                    CommonModule.Instance.TowerLamp.Buzzer_Off();
                }
            }
            //else if ((m_nMachineStatus == (int)MachineStatus.EMERGENCY_STOP) ||                 //  비상정지 상태입니다.
            //        (m_nMachineStatus == (int)MachineStatus.SAFETY_SENSOR_STOP) ||              //  안전센서 감지로 인한 장비 정지 !!
            //        (m_nMachineStatus == (int)MachineStatus.ELEV_Z_OVER_TORQUE))                //  Z축 엘리베이터 오버토크 발생
            //{
            //    if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() == 0)
            //    {
            //        CommonModule.Instance.TowerLamp.LedBar_Red_On();
            //    }
            //    if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() != 0)
            //    {
            //        CommonModule.Instance.TowerLamp.LedBar_Green_Off();
            //    }
            //    if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
            //    {
            //        CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
            //    }
            //}
            else
            {
                //  작업 중인지?
                if ((workStage.m_nLaserDrilling_MainStep > (int)WorkStage.LaserDrilling_Step.None) &&
                    (workStage.m_nLaserDrilling_MainStep < (int)WorkStage.LaserDrilling_Step.Complete))
                {
                    Equipment.Start();

                    //  TowerLamp
                    if (CommonModule.Instance.TowerLamp.Is_Green_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.Green_On();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_Yellow_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.Yellow_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_Red_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.Red_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_Buzzer_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.Buzzer_Off();
                    }
                }
                else if ((workStage.m_nHomeStep > (int)WorkStage.Home_Step.None) &&
                        (workStage.m_nHomeStep < (int)WorkStage.Home_Step.Complete))                     //  홈 실행 중이면? 흰색 깜빡이도록l
                {
                    
                }
                else
                {
                    Equipment.Stop();

                    //  TowerLamp
                    if (CommonModule.Instance.TowerLamp.Is_Green_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.Green_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_Yellow_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.Yellow_On();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_Red_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.Red_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_Buzzer_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.Buzzer_Off();
                    }
                }
            }


            ////  Laser Power 표시
            //baseLabel_LaserPower.Text = workStage.m_dDrilling_Power.ToString();
            //if (workStage.Config.ParamConfig.LaserPowerAutoChange || workStage.Config.ParamConfig.LaserPowerChange_byPowerMeter)        //  Power 값에 대한 % 
            //{
            //    lblLaserPower_Unit.Text = "W";
            //}
            //else                                                                                                                                //  Attenuator 위치값에 대한 %
            //{
            //    lblLaserPower_Unit.Text = "％";
            //}

            ///////////////////////////////////////////////////////////////////////////////////////
            //  Link Status
            //
            if (CommonModule.Instance.Illuminator == null)
            {
                //ledStatus_Light_Connected.Image = global::SLD200.Properties.Resources.StopOn;
            }
            else
            {
                //ledStatus_Light_Connected.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            }
            //
            ///////////////////////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////////////////////
            //  비상 정지 시
            //
            //if (CommonModule.Instance.OperationButtons.IsEMG())                --> 나중에 주석 해제해야 함
            //{
            //    //  Main Work 타이머
            //    workStage.timer_MainWork.Enabled = false;

            //    //  Sub Work 타이머
            //    workStage.timer_SubWork.Enabled = false;

            //    //  Product Align 타이머
            //    workStage.timer_VisionAlign.Enabled = false;

            //    //  Motion 홈 실행 타이머
            //    workStage.timer_Motion_Home.Enabled = false;

            //    //  Reticle Glass check 타이머
            //    workStage.timer_ReticleGlass_Check.Enabled = false;


            //    workStage.m_nHomeStep = (int)WorkStage.Home_Step.None;
            //    workStage.m_nWorkStage_MainStep = (int)WorkStage.WorkStage_Step.None;
            //    workStage.m_nFindAlignMark_Step = (int)WorkStage.FindAlignMark_Step.None;
            //    workStage.m_nReticleCheck_HighResCam_Step = (int)WorkStage.ReticleCheck_HighResCam_Step.None;
            //    workStage.m_nReticleCheck_LowResCam_Step = (int)WorkStage.ReticleCheck_LowResCam_Step.None;
            //    workStage.m_nSafetyPos_Move_Step = (int)WorkStage.SafetyPos_Move_Step.None;


            //    Equipment.MachineStop_byUser = true;

            //    for (int i = (int)WorkStageParameter.AxisAjinEnum.X; i < (int)WorkStageParameter.AxisAjinEnum.Max; i++)         //  Laser Drilling 모듈의 Ajin 모션 축 수 가져오기 (Enum Count 확인)
            //    {
            //        MC_Func.MC_MotorStop(i, 2000);
            //    }

            //    for (int i = (int)LoaderParameter.AxisAjinEnum.Z0; i < (int)LoaderParameter.AxisAjinEnum.Max; i++)                      //  Loader 모듈의 Ajin 모션 축 수 가져오기 (Enum Count 확인)
            //    {
            //        MC_Func.MC_MotorStop(i, 2000);
            //    }

            //    workStage.m_bHomeOK = false;


            //    if (m_bEmgBtn_Clicked == false)
            //    {
            //        m_bEmgBtn_Clicked = true;

            //        var mb = new MessageBoxOk();
            //        mb.ShowDialog("Information !", "비상정지 버튼이 눌렸습니다!!\r\n\r\n비상정지 해제 후 장비를 초기화 하십시오.");
            //    }
            //}
            //else
            //{
            //    m_bEmgBtn_Clicked = false;
            //}
            //
            ///////////////////////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Input
            //   

            //if (CommonModule.Instance.OperationButtons.IsStop() ||
            //    CommonModule.Instance.OperationButtons.IsReset())
            //{
            //    Equipment.MachineStop_byAlarm = false;

            //    //  동작도 정지시킬지는.... 좀 보자...
            //}


            MainUI_Status_Check();              //  버튼입력 시 동작 (아래에서 쓰레드로 바꿨던 것으로 다시 타이머로 한다.


            //if (workStage.workStageParameter.DI_Main_CDACheck())
            //    pictureBoxMainAirCheck.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxMainAirCheck.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Main_VacuumCheck())
            //    pictureBoxMainVacuumCheck.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxMainVacuumCheck.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_ThinChuck_VacuumCheck())
            //    pictureBoxThinChuckVacuumCheck.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxThinChuckVacuumCheck.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Wafer_VacuumCheck())
            //    pictureBoxWaferVacuumCheck.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxWaferVacuumCheck.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_PackingCheck())
            //    pictureBoxProbePackingCheck.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbePackingCheck.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_ThinChuck_Detect())
            //    pictureBoxThinChuckDetect.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxThinChuckDetect.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_LeftClampModule_FW())
            //    pictureBoxProbeLeftClamp_FW.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbeLeftClamp_FW.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_LeftClampModule_BW())
            //    pictureBoxProbeLeftClamp_BW.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbeLeftClamp_BW.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_RightClampModule_FW())
            //    pictureBoxProbeRightClamp_FW.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbeRightClamp_FW.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_RightClampModule_BW())
            //    pictureBoxProbeRightClamp_BW.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbeRightClamp_BW.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_UnpackingCyl_Down())
            //    pictureBoxProbeUnpackingCyl_Down.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbeUnpackingCyl_Down.Image = global::SLD200.Properties.Resources.StopOn;

            //if (workStage.workStageParameter.DI_Probe_UnpackingCyl_Up())
            //    pictureBoxProbeUnpackingCyl_Up.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxProbeUnpackingCyl_Up.Image = global::SLD200.Properties.Resources.StopOn;


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Output
            //

            //if (workStage.workStageParameter.IsDO_ThinChuck_StageCleaning())
            //    baseButtonThinChuckStageCleaning.BackColor = Color.Lime;
            //else
            //    baseButtonThinChuckStageCleaning.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_ThinChuck_Vacuum())
            //    baseButtonThinChuckVacuum.BackColor = Color.Lime;
            //else
            //    baseButtonThinChuckVacuum.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Wafer_Vacuum())
            //    baseButtonWaferVacuum.BackColor = Color.Lime;
            //else
            //    baseButtonWaferVacuum.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_Packing())
            //    baseButtonProbePacking.BackColor = Color.Lime;
            //else
            //    baseButtonProbePacking.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_Unpacking())
            //    baseButtonProbeUnpacking.BackColor = Color.Lime;
            //else
            //    baseButtonProbeUnpacking.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_ClampModule_Down())
            //    baseButtonProbeClamp_Down.BackColor = Color.Lime;
            //else
            //    baseButtonProbeClamp_Down.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_ClampModule_FW())
            //    baseButtonProbeClamp_FW.BackColor = Color.Lime;
            //else
            //    baseButtonProbeClamp_FW.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_ClampModule_BW())
            //    baseButtonProbeClamp_BW.BackColor = Color.Lime;
            //else
            //    baseButtonProbeClamp_BW.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_UnpackingCyl_Down())
            //    baseButtonProbeUnpackingCyl_Down.BackColor = Color.Lime;
            //else
            //    baseButtonProbeUnpackingCyl_Down.BackColor = Color.DimGray;

            //if (workStage.workStageParameter.IsDO_Probe_UnpackingCyl_Up())
            //    baseButtonProbeUnpackingCyl_Up.BackColor = Color.Lime;
            //else
            //    baseButtonProbeUnpackingCyl_Up.BackColor = Color.DimGray;
        }

        private void MainUI_Status_Check()
        {
            //  Start, Stop, Reset 버튼 상태 표시
            //CommonModule.Instance.OperationButtons.Start(true);
            //CommonModule.Instance.OperationButtons.Stop(true);
            //CommonModule.Instance.OperationButtons.Reset(true);
        }

        private void pictureBoxDispenser_ContactSensorAir_On_Click(object sender, EventArgs e)
        {
            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                DioPoint dioPoint = point;
            }
        }

        private bool Get_DI_Status(string strInputName )
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;
                if (dioPoint == null)
                    return false;

                if ( (dioPoint.IoType == IoType.Input) && (dioPoint.Name == strInputName) )
                {
                    if (dioPoint.GetValue() == DioValue.On)
                        bRet = true;

                    break;
                }
            }

            return bRet;
        }

        private bool Get_DO_Status(string strInputName)
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;
                if (dioPoint == null)
                    return false;

                if ((dioPoint.IoType == IoType.Output) && (dioPoint.Name == strInputName))
                {
                    if (dioPoint.GetValue() == DioValue.On)
                        bRet = true;

                    break;
                }
            }

            return bRet;
        }

        private bool Set_DO_Status(string strInputName, bool bSet)
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;

                if (dioPoint == null)
                    return false;

                if ((dioPoint.IoType != IoType.Input) && (dioPoint.Name == strInputName))
                {
                    if ( bSet )
                        dioPoint.Write(DioValue.On);
                    else 
                        dioPoint.Write(DioValue.Off);

                    break;
                }
            }

            return bRet;
        }

        private void Set_Machine_Status(int nStatus)
        {
            //  2025. 01. 03.  SCH : TOP 상태 Message 에 표시하도록 한다.

            //switch ( nStatus )
            //{
            //    case (int)MachineStatus.LOGIN_REQUIRED:
            //        lblMachine_Status.Text = "장비 로그인이 필요합니다.";
            //        break;


            //    case (int)MachineStatus.AUTO_LOGOUT:
            //        lblMachine_Status.Text = "자동으로 로그아웃 되었습니다.";
            //        break;


            //    case (int)MachineStatus.INITIALIZE_REQUIRED:
            //        lblMachine_Status.Text = "장비 초기화가 필요합니다.";
            //        break;


            //    case (int)MachineStatus.MACHINE_INITIALIZING:
            //        lblMachine_Status.Text = "장비 초기화 중...";
            //        break;


            //    case (int)MachineStatus.MACHINE_READY:
            //        lblMachine_Status.Text = "작업 대기";
            //        break;


            //    case (int)MachineStatus.WAFER_ALIGN:
            //        lblMachine_Status.Text = "웨이퍼 정렬 작업 진행 중...";
            //        break;


            //    case (int)MachineStatus.WAFER_ALIGN_ERROR_CHECK:
            //        lblMachine_Status.Text = "웨이퍼 정렬 오차 검증 작업 진행 중...";
            //        break;


            //    case (int)MachineStatus.RETICLE_CHECK_UPPER:
            //        lblMachine_Status.Text = "Reticle 정렬 위치 이동 중... (상부 카메라)";
            //        break;


            //    case (int)MachineStatus.RETICLE_CHECK_LOWER:
            //        lblMachine_Status.Text = "Reticle 정렬 위치 이동 중... (하부 카메라)";
            //        break;


            //    case (int)MachineStatus.SAFETY_POSITION_MOVE:
            //        lblMachine_Status.Text = "안전 위치로 이동 중...";
            //        break;


            //    case (int)MachineStatus.WAFER_LOADING_READY:
            //        lblMachine_Status.Text = "웨이퍼 && 씬-척 투입 대기 위치로 이동 중...";
            //        break;


            //    case (int)MachineStatus.PROBE_PACKING:
            //        lblMachine_Status.Text = "웨이퍼 && 프로브 카드 패킹 작업 진행 중...";
            //        break;


            //    case (int)MachineStatus.PROBE_UNPACKING:
            //        lblMachine_Status.Text = "웨이퍼 && 프로브 카드 언패킹 작업 중...";
            //        break;


            //    case (int)MachineStatus.PROBE_UNPACKING_READY:
            //        lblMachine_Status.Text = "웨이퍼 && 프로브 카드 언패킹 준비 위치로 이동 중...";
            //        break;


            //    case (int)MachineStatus.PROBE_CARD_LOADING_READY:
            //        lblMachine_Status.Text = "프로브 카드 투입 대기 위치로 이동 중...";
            //        break;


            //    case (int)MachineStatus.PROBE_CARD_LOCKING:
            //        lblMachine_Status.Text = "프로브 카드 고정 작업 진행 중...";
            //        break;


            //    case (int)MachineStatus.EMERGENCY_STOP:
            //        lblMachine_Status.Text = "비상정지 상태입니다.  (비상정지 해제 후 초기화)";
            //        break;


            //    case (int)MachineStatus.SAFETY_SENSOR_PAUSE:
            //        lblMachine_Status.Text = "안전센서 감지로 인한 일시 정지!!  (해제시 이어서 진행)";
            //        break;


            //    case (int)MachineStatus.SAFETY_SENSOR_STOP:
            //        lblMachine_Status.Text = "안전센서  감지로  인한  장비  정지 !!!";
            //        break;


            //    case (int)MachineStatus.ELEV_Z_OVER_TORQUE:
            //        lblMachine_Status.Text = "엘리베이터 Z 축 과부하 감지로 인한 정지 !!!";
            //        break;


            //    case (int)MachineStatus.PAK_LATCH_CHECK:
            //        lblMachine_Status.Text = "Latch 가 Lock 상태입니다. !!!    (Unlock 후 확인 버튼)";
            //        break;
            //}
        }


        private void btnTest_Main_Start_Click(object sender, EventArgs e)
        {
            //  Main Start

            /*if (!chkBoxMainUIControl_byUser.Checked)
                return;

            dispenserAndScale.m_bDrawingForWeightCheck = false;

            reelFeederAndMounter.m_bReelCoverTapeMotor_Run = true;

            if (reelFeederAndMounter.Config.ParamConfig.AlwaysReelTapeVacuumOn)
                reelFeederAndMounter.reelFeederAndMounterParameter.DO_ReelFeederVacuum(true);

            chkBoxDispenserConvCombi.Checked = true;
            chkBoxRivetVisionConvCombi.Checked = true;
            chkBoxMounterConvCombi.Checked = true;
            chkBoxMountVisionConvCombi.Checked = true;

            conveyor.m_nLoadingStep = (int)Conveyor.Loading_Step.Start;
            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.Start;
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.Start;
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.Start;
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.Start;

            CommonModule.Instance.TowerLamp.Buzzer_Off();
            CommonModule.Instance.TowerLamp.Red_Off();
            CommonModule.Instance.TowerLamp.Green_On();
            CommonModule.Instance.TowerLamp.Yellow_Off();*/
        }

        private void btnTest_Main_Stop_Click(object sender, EventArgs e)
        {
            //  Main Stop

            /*chkBoxDispenserConvCombi.Checked = false;
            chkBoxRivetVisionConvCombi.Checked = false;
            chkBoxMounterConvCombi.Checked = false;
            chkBoxMountVisionConvCombi.Checked = false;

            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.None;
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.None;
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.None;
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.None;

            Set_DO_Status("LOADING CONVEYOR DIRECTION", false);
            Set_DO_Status("LOADING CONVEYOR RUN", false);
            conveyor.m_nLoadingStep = (int)Conveyor.Loading_Step.None;

            Set_DO_Status("LOADING CONVEYOR DIRECTION", false);
            Set_DO_Status("LOADING CONVEYOR RUN", false);
            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.None;
            dispenserAndScale.m_nDispenser_MainStep = (int)DispenserAndScale.Dispensing_Step.None;
            dispenserAndScale.m_nDispenser_DrawingFormStep = (int)DispenserAndScale.Dispenser_DrawingForm_Step.None;

            Set_DO_Status("R INSPECTION CONVEYOR DIRECTION", false);
            Set_DO_Status("R INSPECTION CONVEYOR RUN", false);
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.None;
            inspVision.m_nRivetInspection_MainStep = (int)InspVision.RivetInspection_Step.None;

            Set_DO_Status("TARGET STAGE CONVEYOR DIRECTION", false);
            Set_DO_Status("TARGET STAGE CONVEYOR RUN", false);
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.None;
            reelFeederAndMounter.m_nMounter_MainStep = (int)ReelFeederAndMounter.Mounter_Step.None;

            Set_DO_Status("M INSPECTION CONVEYOR DIRECTION", false);
            Set_DO_Status("M INSPECTION CONVEYOR RUN", false);
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.None;
            inspVision.m_nMountVision_MainStep = (int)InspVision.MountVision_Step.None;

            //reelFeederAndMounterParameter.DO_ReelFeederVacuum(false);
            conveyor.OpticonBarcodeReaderComm_ReadStop();

            reelFeederAndMounter.m_bReelCoverTapeMotor_Run = false;

            reelFeederAndMounter.reelFeederAndMounterParameter.DO_ReelFeederVacuum(false);

            inspVision.inspVisionParameter.DO_VisionReset((int)InspVisionParameter.VisionUnit.RivetVision, false);
            inspVision.inspVisionParameter.DO_VisionTrigger((int)InspVisionParameter.VisionUnit.RivetVision, (int)InspVisionParameter.RivetInspPos.Rivet1, false);
            inspVision.inspVisionParameter.DO_VisionTrigger((int)InspVisionParameter.VisionUnit.RivetVision, (int)InspVisionParameter.RivetInspPos.Rivet2, false);
            inspVision.inspVisionParameter.DO_VisionTrigger((int)InspVisionParameter.VisionUnit.RivetVision, (int)InspVisionParameter.RivetInspPos.Etching, false);
            inspVision.inspVisionParameter.DO_VisionInspDataRequest((int)InspVisionParameter.VisionUnit.RivetVision, false);

            inspVision.inspVisionParameter.DO_VisionReset((int)InspVisionParameter.VisionUnit.MountVision, false);
            inspVision.inspVisionParameter.DO_VisionTrigger((int)InspVisionParameter.VisionUnit.MountVision, 0, false);
            inspVision.inspVisionParameter.DO_VisionInspDataRequest((int)InspVisionParameter.VisionUnit.MountVision, false);

            reelFeederAndMounter.m_nReelTapeMotor_Step = 0;
            reelFeederAndMounter.m_nCoverTapeMotor_Step = 0;

            dispenserAndScale.dispenserParameter.DO_DigitalContactSensorAirOnOff(false);
            dispenserAndScale.dispenserParameter.DO_Dispenser((int)DispenserAndScaleParameter.DispenserSignal.DIS, false);

            conveyor.conveyorParameter.DO_SMEMA_WorkReady(false);
            conveyor.conveyorParameter.DO_SMEMA_WorkEnd(false);

            for ( int i = 0; i < 14; i++ )
            {
                if ((i != 1) && (i != 10))
                    MC_Func.MC_MotorStop(i, 1000);
            }

            CommonModule.Instance.TowerLamp.Buzzer_Off();
            CommonModule.Instance.TowerLamp.Red_Off();
            CommonModule.Instance.TowerLamp.Green_Off();
            CommonModule.Instance.TowerLamp.Yellow_On();*/
        }

        private void btnTest_Main_Reset_Click(object sender, EventArgs e)
        {
            /*//  Main Reset
            conveyor.SetCarrierFlag((int)ConveyorParameter.ConveyorPart.LoadingZone, (int)Conveyor.FlagType.CARRIER_NONE) ;
            conveyor.SetStatusFlag((int)ConveyorParameter.ConveyorPart.LoadingZone, (int)Conveyor.FlagType.WORK_NONE);

            conveyor.SetCarrierFlag((int)ConveyorParameter.ConveyorPart.DispenseZone, (int)Conveyor.FlagType.CARRIER_NONE);
            conveyor.SetStatusFlag((int)ConveyorParameter.ConveyorPart.DispenseZone, (int)Conveyor.FlagType.WORK_NONE);
            
            conveyor.SetCarrierFlag((int)ConveyorParameter.ConveyorPart.RivetInspZone, (int)Conveyor.FlagType.CARRIER_NONE);
            conveyor.SetStatusFlag((int)ConveyorParameter.ConveyorPart.RivetInspZone, (int)Conveyor.FlagType.WORK_NONE);

            conveyor.SetCarrierFlag((int)ConveyorParameter.ConveyorPart.MountingZone, (int)Conveyor.FlagType.CARRIER_NONE);
            conveyor.SetStatusFlag((int)ConveyorParameter.ConveyorPart.MountingZone, (int)Conveyor.FlagType.WORK_NONE);

            conveyor.SetCarrierFlag((int)ConveyorParameter.ConveyorPart.MountInspZone, (int)Conveyor.FlagType.CARRIER_NONE);
            conveyor.SetStatusFlag((int)ConveyorParameter.ConveyorPart.MountInspZone, (int)Conveyor.FlagType.WORK_NONE);

            //  Dispenser Part Data Reset
            //  Vision Pos. Check --> false
            //  Height Check --> false
            //  Needle Clean --> false
            //  Dummy Shot --> false
            //  Work Count --> 0
            dispenserAndScale.SetDispVisionCheck_OK(false);
            dispenserAndScale.SetDispHeightCheck_OK(false);
            dispenserAndScale.SetNeedleClean_OK(false);
            dispenserAndScale.SetDummyShot_OK(false);
            dispenserAndScale.ResetDispensingCount();

            inspVision.ResetRivetVisionCount();
            inspVision.ResetMountVisionCount();

            reelFeederAndMounter.ResetChipMountingCount();
            reelFeederAndMounter.SetMounterModuleChipExist(false);


            //  Conveyor.cs
            conveyor.m_nLoadingStep = (int)Conveyor.Loading_Step.None;
            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.None;
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.None;
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.None;
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.None;
            conveyor.m_nRollBackStep = (int)Conveyor.RollBack_Step.None;

            conveyor.Inlet_CarrierPCBStatus = (int)PCBSatus.NoExist;

            for (int nPosNum = 0; nPosNum < 4; nPosNum++)
            {
                for (int nPCBNum = 0; nPCBNum < 10; nPCBNum++)
                {
                    //Array_DispPos_ContactSensorValue[nPosNum, nPCBNum] = 0.0;
                    conveyor.CarrierArray_DispPos[nPosNum, nPCBNum].m_dContactSensorValue = 0.0;
                    conveyor.CarrierArray_DispPos[nPosNum, nPCBNum].m_bToWork = false;
                }
            }


            //  DispenserAndScale.cs
            dispenserAndScale.SetDispenserWork((int)DispenserAndScale.DispenserWorkStatus.WORK_NONE);

            dispenserAndScale.DispenserWorkIndex = -1;

            dispenserAndScale.m_bDispenserMainCyc_Running = false;
            dispenserAndScale.m_bDispenserPosCheckVisionCyc_Running = false;
            dispenserAndScale.m_bDispenserHeightCheckSensorCyc_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_NeedleHeightCalibration_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_NeedleXYCalibration_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_WeighCellCheck_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_Dispensing_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_DispensingAndUp_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_VisionCheck_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_HeightCheck_Running = false;

            dispenserAndScale.m_nDispenser_MainStep = (int)DispenserAndScale.Dispensing_Step.None;
            dispenserAndScale.m_nDispenser_DrawingFormStep = (int)DispenserAndScale.Dispenser_DrawingForm_Step.None;
            dispenserAndScale.m_nDispenser_PosCheckVision_Step = (int)DispenserAndScale.Dispensing_PosCheckVision_Step.None;
            dispenserAndScale.m_nDispenser_HeightCheckSensor_Step = (int)DispenserAndScale.Dispensing_HeightCheckSensor_Step.None;
            dispenserAndScale.m_nSub_NeedleHeightCalStep = (int)DispenserAndScale.NeedleHeightCal_Step.None;
            dispenserAndScale.m_nSub_NeedleCleanStep = (int)DispenserAndScale.NeedleClean_Step.None;
            dispenserAndScale.m_nSub_DummyShotStep = (int)DispenserAndScale.DummyShot_Step.None;
            dispenserAndScale.m_nSub_NeedleChangeStep = (int)DispenserAndScale.NeedleChange_Step.None;
            dispenserAndScale.m_nSub_PurgeShotStep = (int)DispenserAndScale.PurgeShot_Step.None;
            dispenserAndScale.m_nSub_NeedleHeightCalStep = (int)DispenserAndScale.NeedleHeightCal_Step.None;
            dispenserAndScale.m_nSub_NeedleXYCalStep = (int)DispenserAndScale.NeedleXYCal_Step.None;
            dispenserAndScale.m_nSub_1ShotWeighCellCheckStep = (int)DispenserAndScale.WeighCell1ShotCheck_Step.None;
            dispenserAndScale.m_nDispensing_1Cyc_Step = (int)DispenserAndScale.Dispensing_1Cyc_Step.None;
            dispenserAndScale.m_nDispensingAndUp_Cyc_Step = (int)DispenserAndScale.DispensingAndUp_Cyc_Step.None;
            dispenserAndScale.m_nDispensing_PosCheckVision_1Cyc_Step = (int)DispenserAndScale.PosCheckVision_1Cyc_Step.None;
            dispenserAndScale.m_nContactSensor_1Cyc_Step = (int)DispenserAndScale.ContactSensor_1Cyc_Step.None;

            for (int i = 0; i < 10; i++)
            {
                dispenserAndScale.CarrierPCBStatus[i] = (int)PCBSatus.NoExist;

                dispenserAndScale.m_dContactSensorValue_TouchedEtchingPosition[i] = 0.0;
                dispenserAndScale.m_dDispZPos_ContactSensor_TouchedEtchingPosition[i] = 0.0;
            }


            //  InspVision.cs
            inspVision.RivetVisionWorkIndex = -1;
            inspVision.MountVisionWorkIndex = -1;

            inspVision.m_bRivetVisionMainCyc_Running = false;
            inspVision.m_bMountVisionMainCyc_Running = false;
            inspVision.m_bVisionSingleCyc_RivetInspection_Running = false;
            inspVision.m_bVisionSingleCyc_MountInspection_Running = false;

            inspVision.m_nRivetInspection_MainStep = (int)InspVision.RivetInspection_Step.None;
            inspVision.m_nMountVision_MainStep = (int)InspVision.MountVision_Step.None;
            inspVision.m_nRivetInspection_1Cyc_Step = (int)InspVision.RivetInspection_1Cyc_Step.None;
            inspVision.m_nMountVision_1Cyc_Step = (int)InspVision.MountVision_1Cyc_Step.None;
            inspVision.m_nMountInspection_1Cyc_Step = (int)InspVision.MountInspection_1Cyc_Step.None;


            //  ReelFeederAndMounter.cs
            reelFeederAndMounter.MounterWorkIndex = -1;

            reelFeederAndMounter.m_bMounterMainCyc_Running = false;
            reelFeederAndMounter.m_bMounterSingleCyc_Running = false;

            reelFeederAndMounter.m_nMounter_MainStep = (int)ReelFeederAndMounter.Mounter_Step.None;
            reelFeederAndMounter.m_nSub_ReelFeederStep = (int)ReelFeederAndMounter.ReelFeeder_Step.None;
            reelFeederAndMounter.m_nSub_ReelTapeStep = (int)ReelFeederAndMounter.ReelTape_Step.None;
            reelFeederAndMounter.m_nSub_CoverTapeStep = (int)ReelFeederAndMounter.CoverTape_Step.None;*/
        }

        private void Main_Start()
        {
            /*chkBoxDispenserConvCombi.Checked = true;
            chkBoxRivetVisionConvCombi.Checked = true;
            chkBoxMounterConvCombi.Checked = true;
            chkBoxMountVisionConvCombi.Checked = true;

            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.Start;
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.Start;
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.Start;
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.Start;*/
        }

        private void Main_Stop()
        {
            /*chkBoxDispenserConvCombi.Checked = false;
            chkBoxRivetVisionConvCombi.Checked = false;
            chkBoxMounterConvCombi.Checked = false;
            chkBoxMountVisionConvCombi.Checked = false;

            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.None;
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.None;
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.None;
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.None;

            Set_DO_Status("LOADING CONVEYOR DIRECTION", false);
            Set_DO_Status("LOADING CONVEYOR RUN", false);
            conveyor.m_nLoadingStep = (int)Conveyor.Loading_Step.None;

            Set_DO_Status("LOADING CONVEYOR DIRECTION", false);
            Set_DO_Status("LOADING CONVEYOR RUN", false);
            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.None;
            dispenserAndScale.m_nDispenser_MainStep = (int)DispenserAndScale.Dispensing_Step.None;
            dispenserAndScale.m_nDispenser_DrawingFormStep = (int)DispenserAndScale.Dispenser_DrawingForm_Step.None;

            Set_DO_Status("R INSPECTION CONVEYOR DIRECTION", false);
            Set_DO_Status("R INSPECTION CONVEYOR RUN", false);
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.None;
            inspVision.m_nRivetInspection_MainStep = (int)InspVision.RivetInspection_Step.None;

            Set_DO_Status("TARGET STAGE CONVEYOR DIRECTION", false);
            Set_DO_Status("TARGET STAGE CONVEYOR RUN", false);
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.None;
            reelFeederAndMounter.m_nMounter_MainStep = (int)ReelFeederAndMounter.Mounter_Step.None;

            Set_DO_Status("M INSPECTION CONVEYOR DIRECTION", false);
            Set_DO_Status("M INSPECTION CONVEYOR RUN", false);
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.None;
            inspVision.m_nMountVision_MainStep = (int)InspVision.MountVision_Step.None;

            for (int i = 0; i < 14; i++)
            {
                if ((i != 1) && (i != 10))
                    MC_Func.MC_MotorStop(i, 1000);
            }*/
        }

        private void Main_Reset()
        {
            /*//  Conveyor.cs
            conveyor.m_nLoadingStep = (int)Conveyor.Loading_Step.None;
            conveyor.m_nDispenserStep = (int)Conveyor.Dispenser_Step.None;
            conveyor.m_nRivetInspVisionStep = (int)Conveyor.RivetInsp_Step.None;
            conveyor.m_nMounterStep = (int)Conveyor.Mounter_Step.None;
            conveyor.m_nMountInspVisionStep = (int)Conveyor.MountInsp_Step.None;
            conveyor.m_nRollBackStep = (int)Conveyor.RollBack_Step.None;

            conveyor.Inlet_CarrierPCBStatus = (int)PCBSatus.NoExist;

            for (int nPosNum = 0; nPosNum < 4; nPosNum++)
            {
                for (int nPCBNum = 0; nPCBNum < 10; nPCBNum++)
                {
                    //Array_DispPos_ContactSensorValue[nPosNum, nPCBNum] = 0.0;
                    conveyor.CarrierArray_DispPos[nPosNum, nPCBNum].m_dContactSensorValue = 0.0;
                    conveyor.CarrierArray_DispPos[nPosNum, nPCBNum].m_bToWork = false;
                }
            }


            //  DispenserAndScale.cs
            dispenserAndScale.SetDispenserWork((int)DispenserAndScale.DispenserWorkStatus.WORK_NONE);

            dispenserAndScale.DispenserWorkIndex = -1;

            dispenserAndScale.m_bDispenserMainCyc_Running = false;
            dispenserAndScale.m_bDispenserPosCheckVisionCyc_Running = false;
            dispenserAndScale.m_bDispenserHeightCheckSensorCyc_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_NeedleHeightCalibration_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_NeedleXYCalibration_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_WeighCellCheck_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_Dispensing_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_DispensingAndUp_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_VisionCheck_Running = false;
            dispenserAndScale.m_bDispenserSingleCyc_HeightCheck_Running = false;

            dispenserAndScale.m_nDispenser_MainStep = (int)DispenserAndScale.Dispensing_Step.None;
            dispenserAndScale.m_nDispenser_DrawingFormStep = (int)DispenserAndScale.Dispenser_DrawingForm_Step.None;
            dispenserAndScale.m_nDispenser_PosCheckVision_Step = (int)DispenserAndScale.Dispensing_PosCheckVision_Step.None;
            dispenserAndScale.m_nDispenser_HeightCheckSensor_Step = (int)DispenserAndScale.Dispensing_HeightCheckSensor_Step.None;
            dispenserAndScale.m_nSub_NeedleHeightCalStep = (int)DispenserAndScale.NeedleHeightCal_Step.None;
            dispenserAndScale.m_nSub_NeedleCleanStep = (int)DispenserAndScale.NeedleClean_Step.None;
            dispenserAndScale.m_nSub_DummyShotStep = (int)DispenserAndScale.DummyShot_Step.None;
            dispenserAndScale.m_nSub_NeedleChangeStep = (int)DispenserAndScale.NeedleChange_Step.None;
            dispenserAndScale.m_nSub_PurgeShotStep = (int)DispenserAndScale.PurgeShot_Step.None;
            dispenserAndScale.m_nSub_NeedleHeightCalStep = (int)DispenserAndScale.NeedleHeightCal_Step.None;
            dispenserAndScale.m_nSub_NeedleXYCalStep = (int)DispenserAndScale.NeedleXYCal_Step.None;
            dispenserAndScale.m_nSub_1ShotWeighCellCheckStep = (int)DispenserAndScale.WeighCell1ShotCheck_Step.None;
            dispenserAndScale.m_nDispensing_1Cyc_Step = (int)DispenserAndScale.Dispensing_1Cyc_Step.None;
            dispenserAndScale.m_nDispensingAndUp_Cyc_Step = (int)DispenserAndScale.DispensingAndUp_Cyc_Step.None;
            dispenserAndScale.m_nDispensing_PosCheckVision_1Cyc_Step = (int)DispenserAndScale.PosCheckVision_1Cyc_Step.None;
            dispenserAndScale.m_nContactSensor_1Cyc_Step = (int)DispenserAndScale.ContactSensor_1Cyc_Step.None;

            for (int i = 0; i < 10; i++)
            {
                dispenserAndScale.CarrierPCBStatus[i] = (int)PCBSatus.NoExist;

                dispenserAndScale.m_dContactSensorValue_TouchedEtchingPosition[i] = 0.0;
                dispenserAndScale.m_dDispZPos_ContactSensor_TouchedEtchingPosition[i] = 0.0;
            }


            //  InspVision.cs
            inspVision.RivetVisionWorkIndex = -1;
            inspVision.MountVisionWorkIndex = -1;

            inspVision.m_bRivetVisionMainCyc_Running = false;
            inspVision.m_bMountVisionMainCyc_Running = false;
            inspVision.m_bVisionSingleCyc_RivetInspection_Running = false;
            inspVision.m_bVisionSingleCyc_MountInspection_Running = false;

            inspVision.m_nRivetInspection_MainStep = (int)InspVision.RivetInspection_Step.None;
            inspVision.m_nMountVision_MainStep = (int)InspVision.MountVision_Step.None;
            inspVision.m_nRivetInspection_1Cyc_Step = (int)InspVision.RivetInspection_1Cyc_Step.None;
            inspVision.m_nMountVision_1Cyc_Step = (int)InspVision.MountVision_1Cyc_Step.None;
            inspVision.m_nMountInspection_1Cyc_Step = (int)InspVision.MountInspection_1Cyc_Step.None;


            //  ReelFeederAndMounter.cs
            reelFeederAndMounter.MounterWorkIndex = -1;

            reelFeederAndMounter.m_bMounterMainCyc_Running = false;
            reelFeederAndMounter.m_bMounterSingleCyc_Running = false;

            reelFeederAndMounter.m_nMounter_MainStep = (int)ReelFeederAndMounter.Mounter_Step.None;
            reelFeederAndMounter.m_nSub_ReelFeederStep = (int)ReelFeederAndMounter.ReelFeeder_Step.None;
            reelFeederAndMounter.m_nSub_ReelTapeStep = (int)ReelFeederAndMounter.ReelTape_Step.None;
            reelFeederAndMounter.m_nSub_CoverTapeStep = (int)ReelFeederAndMounter.CoverTape_Step.None;*/
        }

        private void btnTest_Thread_Start_Click(object sender, EventArgs e)
        {
            ThreadStart();
        }

        private void btnTest_Thread_Stop_Click(object sender, EventArgs e)
        {
            ThreadStop();
        }

        private void btnTest_LoadingZone_CarrierInfo_Show_Click(object sender, EventArgs e)
        {
            /*string m_strCarrierID, m_strTemp;
            List<UnitStatus> m_ListCarrierInfo;
            m_strCarrierID = "";
            m_strTemp = "";

            if ( conveyor.m_bOpticonBarcodeReaderData_Received )
            {
                conveyor.CarrierInfo = new UnitInfoManager();
                conveyor.CarrierInfo.Open();

                conveyor.CarrierInfo.GetCarrierID(conveyor.m_strOpticonBarcodeReader_ReceivedData, ref m_strCarrierID, ref m_strTemp);

                m_ListCarrierInfo = conveyor.CarrierInfo.GetUnitList(m_strCarrierID);

                string m_strContactSensorValue = "Carrier Information\n\n";

                m_strContactSensorValue += "Carrier ID : " + m_strCarrierID + "\n" ;

                for (int i = 0; i < 10; i++)
                {
                    m_strContactSensorValue += (i).ToString() + " : " +
                        m_ListCarrierInfo[i].Status.ToString() + " [" +
                        m_ListCarrierInfo[i].Barcode + "] " + "\n";
                }

                MessageBox.Show(m_strContactSensorValue);

                conveyor.CarrierInfo.Close();
            }*/
        }

        private void lblPreSmema_Pause_Click(object sender, EventArgs e)
        {
            //conveyor.m_bLoadingZone_PreSmema_Pause = !conveyor.m_bLoadingZone_PreSmema_Pause;
        }

        private void lblPostSmema_Pause_Click(object sender, EventArgs e)
        {
            //conveyor.m_bMountInspZone_PostSmema_Pause = !conveyor.m_bMountInspZone_PostSmema_Pause;            
        }

        private void btnJobFile_Load_Click(object sender, EventArgs e)
        {
            //  Load Project File (Sirius)

            int m_nReturn = -1;

            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Scanner Mode 를 선택해야 합니다.");
                return;
            }

            //            using (OpenFileDialog fd = new OpenFileDialog())
            //            {
            //                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
            ////                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
            //                fd.Filter = "sirius files (*.sirius)|*.sirius|All files (*.*)|*.*"; //필터 설정
            //                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

            //                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "작업 파일 불러오기");

            //                //  Arc 를 Polyline 으로 만들 경우
            //                if (workStage.Config.ParamConfig.ConvertArcToPolyline)
            //                {
            //                    Config.LwPolylineBulgeToLines = true;

            //                    if (workStage.Config.ParamConfig.ArcToPolyline_Resolution < 1)
            //                        Config.LwPolylineBulgePrecision = 15;
            //                    else
            //                        Config.LwPolylineBulgePrecision = workStage.Config.ParamConfig.ArcToPolyline_Resolution;
            //                }
            //                else        //  Arc 를 Bulge 값을 이용해서 Arc 처럼 만들 경우
            //                {
            //                    Config.LwPolylineBulgeToLines = false;
            //                }

            //                if (fd.ShowDialog() == DialogResult.OK)
            //                {
            //                    filePath = fd.FileName;
            //                    m_strFullPath = filePath;
            //                    lblJobFile_Name.Text = Path.GetFileName(fd.FileName);

            //                    if (Path.GetExtension(filePath).ToUpper() == ".DXF")
            //                    {
            //                        //SiriusViewer.Document.New();
            //                        var doc = DocumentSerializer.OpenDxf(filePath);
            //                        SiriusViewer.Document = doc;
            //                    }
            //                    else
            //                    {
            //                        //SiriusViewer.Document.New();
            //                        var doc = DocumentSerializer.OpenSirius(filePath);
            //                        SiriusViewer.Document = doc;
            //                    }

            //                    if (workStage.MainSiriusViewer == null)
            //                    {
            //                        workStage.MainSiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();
            //                    }

            //                    workStage.MainSiriusViewer.Document = SiriusViewer.Document;
            //                    Equipment.EqpSiriusViewer.Document = SiriusViewer.Document;

            //                    //  가공 시간 초기화
            //                    Equipment.WorkElapsedTick = 0;
            //                    Equipment.WorkElapsedTick_Outline = 0;
            //                    Equipment.WorkElapsedTick_Thruhole = 0;
            //                    Equipment.WorkElapsedTick_Drilling = 0;
            //                    Equipment.WorkElapsedTick_Marking = 0;

            //                    //  Get Data
            //                    m_nReturn = workStage.GetDrillingData();
            //                    switch (m_nReturn)
            //                    {
            //                        case (int)WorkStage.nGetDataResult.GETDATA_SUCCESS:
            //                            var mb0 = new MessageBoxOk();
            //                            mb0.ShowDialog("Information !", "데이터가 정상적으로 로드 되었습니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_FAIL:
            //                            var mb1 = new MessageBoxOk();
            //                            mb1.ShowDialog("Information !", "데이터가 정상적으로 로드 되지 않았습니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_NOT_GROUP:
            //                            var mb2 = new MessageBoxOk();
            //                            mb2.ShowDialog("Information !", "데이터가 Group 이 아닙니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_UNGROUP:
            //                            var mb3 = new MessageBoxOk();
            //                            mb3.ShowDialog("Information !", "데이터를 Group 해제 해야 합니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_LAYERNAME_NG:
            //                            var mb4 = new MessageBoxOk();
            //                            mb4.ShowDialog("Information !", "Layer Name 은 '쓰루홀', '외곽선', '드릴링', '마킹' 4가지만 가능합니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_MOTIONTYPE_NG:
            //                            var mb5 = new MessageBoxOk();
            //                            mb5.ShowDialog("Information !", "Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NG:
            //                            var mb6 = new MessageBoxOk();
            //                            mb6.ShowDialog("Information !", "Drilling Data 는 Polyline, Line, Circle 중 한 가지 데이터로만 구성되어야 합니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_LINECNT:
            //                            var mb7 = new MessageBoxOk();
            //                            mb7.ShowDialog("Information !", "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED:
            //                            var mb8 = new MessageBoxOk();
            //                            mb8.ShowDialog("Information !", "Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다.");
            //                            break;

            //                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NOT_GROUP:
            //                            var mb9 = new MessageBoxOk();
            //                            mb9.ShowDialog("Information !", "Drilling 데이터가 Group 이 아닙니다.");
            //                            break;
            //                    }
            //                }
            //            }
        }

        //static void SyncAxisViewer(IRtcSyncAxis rtcSyncAxis)
        //{
        //    var exeFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncaxis", "Tools", "syncAXIS_Viewer", "syncAXIS_Viewer.exe");
        //    string simulatedFileName = Path.Combine(exeFileName, rtcSyncAxis.SimulationFileName);
        //    if (File.Exists(simulatedFileName))
        //    {
        //        Task.Run(() =>
        //        {
        //            // Notice
        //            // syncAxisViewer v1.6 의 버그로 인해 아래와 같이 외부에서 파일을 인자로 하여 뷰어 프로세스를 생성하면 일부 데이타 누락이 발생되기도 함
        //            // 이때는 재차 open 을 하면 해결되며, SCANLAB 에 버그 리포트 된 사항임
        //            ProcessStartInfo startInfo = new ProcessStartInfo();
        //            startInfo.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncaxis", "Tools", "syncAXIS_Viewer");
        //            startInfo.CreateNoWindow = false;
        //            startInfo.UseShellExecute = false;
        //            startInfo.FileName = Config.ConfigSyncAxisViewerFileName;
        //            startInfo.WindowStyle = ProcessWindowStyle.Normal;
        //            startInfo.Arguments = "-a";//string.Empty;
        //            if (!string.IsNullOrEmpty(simulatedFileName))
        //                startInfo.Arguments = Path.Combine(Config.ConfigSyncAxisSimulateFilePath, simulatedFileName);
        //            try
        //            {
        //                using (var proc = Process.Start(startInfo))
        //                {
        //                    proc.WaitForExit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //Logger.Log(Logger.Type.Error, ex.Message);
        //            }
        //        });
        //    }
        //}
        
        private void btnHomeAll_Click(object sender, EventArgs e)
        {
            Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "장비 초기화");

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

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            //if (!workStage.workStageParameter.DI_OpSwitch_Reset() || !workStage.workStageParameter.DI_OpSwitch_EMG())
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "전면의 Reset 버튼을 눌러야 합니다.");
            //    return;
            //}

            if (workStage.m_nHomeStep == (int)WorkStage.Home_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "장비를 초기화 하시겠습니까?"))
                    return;

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  이것저것 다 리셋 - 시작
                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
                workStage.m_nFindAlignMark_Step = (int)WorkStage.FindAlignMark_Step.None;
                workStage.m_nReticleCheck_HighResCam_Step = (int)WorkStage.ReticleCheck_HighResCam_Step.None;
                workStage.m_nReticleCheck_LowResCam_Step = (int)WorkStage.ReticleCheck_LowResCam_Step.None;
                workStage.m_nSafetyPos_Move_Step = (int)WorkStage.SafetyPos_Move_Step.None;

                workStage.m_bFindAlignMark_OK = false;

                Equipment.MachineStop_byUser = true;


                //  Main Work 타이머
                workStage.m_btimer_MainWork_Stop = true;
                workStage.timer_MainWork.Enabled = false;

                //  Sub Work 타이머
                workStage.m_btimer_SubWork_Stop = true;
                workStage.timer_SubWork.Enabled = false;

                //  Product Align 타이머
                //workStage.timer_VisionAlign_Stop = true;
                //workStage.timer_VisionAlign.Enabled = false;

                //  Motion 홈 실행 타이머
                workStage.m_btimer_Motion_Home_Stop = true;
                workStage.timer_Motion_Home.Enabled = false;


                MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.X, 2000);
                MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Y, 2000);
                MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Z, 2000);

                if (Equipment.Machine_LaserType_CO2)
                {
                    MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.MASK_Y, 2000);
                }

                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_X, 2000);
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_Y, 2000);
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 500);             // Unloader 것으로 바꿔야 함
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 500);             // Unloader 것으로 바꿔야 함
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 500);           // Unloader 것으로 바꿔야 함
                MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 500);           // Unloader 것으로 바꿔야 함

                //  이것저것 다 리셋 - 끝
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                workStage.m_bHomeOK = false;
                m_bHomeProgress_Show = true;
                workStage.m_nHomeStep = (int)WorkStage.Home_Step.Start;

                //  Motion 홈 실행 타이머
                workStage.timer_Motion_Home.Enabled = true;

                if (!m_FormProgress.HasChildren)            //  Progress 창을 실수로 닫았다면, 다시 메모리 할당하자.
                {
                    m_FormProgress = new ProgressForm("Initialize", "장비 초기화 진행중...");
                }

                m_FormProgress.StartPosition = FormStartPosition.CenterScreen;
                m_FormProgress.TopMost = true;
                m_FormProgress.Show();
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "장비 초기화를 중지 하시겠습니까?"))
                        return;

                    //  Motion 홈 실행 타이머
                    workStage.timer_Motion_Home.Enabled = false;
                    workStage.m_btimer_Motion_Home_Stop = true;

                    workStage.m_nHomeStep = (int)WorkStage.Home_Step.None;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageY);
                    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WorkStageParameter.AxisAcsEnum.StageX);
                    //}

                    if (Equipment.AjinBoard_Opened)
                    {
                        //for (int i = (int)WorkStageParameter.AxisAjinEnum.X; i < (int)WorkStageParameter.AxisAjinEnum.Max; i++)         //  Laser Drilling 모듈의 Ajin 모션 축 수 가져오기 (Enum Count 확인)
                        //{
                        //    MC_Func.MC_MotorStop(i, 2000);
                        //}

                        //for (int i = (int)LoaderParameter.AxisAjinEnum.Z0; i < (int)LoaderParameter.AxisAjinEnum.Max; i++)                      //  Loader 모듈의 Ajin 모션 축 수 가져오기 (Enum Count 확인)
                        //{
                        //    MC_Func.MC_MotorStop(i, 2000);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void baseButtonChangeRecipe_Click(object sender, EventArgs e)
        {
            string m_strBeforeRecipe = null;
            string m_strTemp = null;

            int m_nWaferImageOffset_X = 0;
            int m_nWaferImageOffset_Y = 0;

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "레시피 변경 시작");

            if (CurrentRecipe != null)
            {
                m_strBeforeRecipe = CurrentRecipe.Name;                                                            //  현재 레시피
            }

            RecipeInfoCollection recipes = DataManager.Instance.Recipe;
            FormRecipeList formRecipeList = new FormRecipeList(recipes);
            formRecipeList.BringToFront();
            formRecipeList.StartPosition = FormStartPosition.CenterScreen;
            if (formRecipeList.ShowDialog() == DialogResult.OK)
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "레시피 선택 완료");

                workStage.m_nReticleCheck_Step_forALIGN = (int)ReticleCheck_Step.None;                //  프로그램 시작 시, 레시피 변경 시 레티클 확인 Step 초기화 

                CurrentRecipe = formRecipeList.m_recipe;
                OnChangedCurrentRecipe();
                this.baseTextBoxCurrentRecipe.Text = CurrentRecipe.Name;
                //FormRecipeMain.baseTextBoxRecipeName.Text = CurrentRecipe.Name;
                if (m_FormRecipePart != null)
                {
                    m_FormRecipePart.Close();
                }

                //  Config 파라미터 로드
                Equipment.LoadConfig(CurrentRecipe.Name);
                DataManager.Instance.ApplyConfigData(workStage);
                DataManager.Instance.UpdateConfigData(workStage);

                //  Config 화면에 데이터가 갱신되도록 하기 위해서. (각 Form 의 Timer 가 1초에 한번씩 이 변수를 체크해서 갱신해준다.) 크게 부하 받지는 않으니.... 꼼수..
                Equipment.m_bRedraw_FormWorkStageParameterConfig = true;

                if (CurrentRecipe != null)
                {
                    Equipment.UpdateRecipeData();
                }
                Equipment.SetCurrentRecipe(CurrentRecipe);
                Equipment.SaveRecipe();

                workStage.Machine_Parameter_Load();


                //  패턴 매칭 Train Image 설정
                string m_strFile = "";

                workStage.PatternMatchingImage_Loaded_HighRes = false;
                workStage.PatternMatchingImage_Loaded_LowRes = false;
                workStage.PatternMatchingImage_Reticle_Loaded_HighRes = false;
                workStage.PatternMatchingImage_Reticle_Loaded_LowRes = false;
                

                m_strTemp = string.Format("레시피 변경.    [변경 전 : \"{0}\",   변경 후 : \"{1}\"]",
                                            m_strBeforeRecipe, CurrentRecipe.Name);

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", m_strTemp);


                //  오래된 Backup 파일 & 폴더 삭제 (3일 이전)
                workStage.Delete_Backup(@"C:\Program Files\QMC\SLD200_MSL\Config\Backup");
                workStage.Delete_Backup(@"C:\Program Files\QMC\SLD200_MSL\Recipe\Backup");

                workStage.m_bParameterSetting_PosData_Reload = true;

                //  레시피 변경 시 Offset 재설정
                m_nWaferImageOffset_X = workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
                m_nWaferImageOffset_Y = workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;

                m_strTemp = string.Format("설정값, 웨이퍼 카메라 Offset X : {0:0.000}, Offset Y: {1:0.000}",
                                        workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X,
                                        workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y);
                Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", m_strTemp);


                //public int MAX_IMAGE_WIDTH = 2248;            //  현장에서 조정된 Size (Center Offset X : 100, Offset Y : 84)
                //public int MAX_IMAGE_HEIGHT = 1880;
                if ((workStage.Camera_LowRes != null) &&
                    ((m_nWaferImageOffset_X > 0) && (m_nWaferImageOffset_Y > 0)))             //  Wafer Image Offset 값 모두 0 이상일 때만 적용
                {
                    workStage.Camera_LowRes.MyConfig.OffsetX = (uint)workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
                    workStage.Camera_LowRes.MyConfig.OffsetY = (uint)workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;

                    workStage.Camera_LowRes.OffsetMove((uint)m_nWaferImageOffset_X, (uint)m_nWaferImageOffset_Y);

                    m_strTemp = string.Format("웨이퍼 카메라 Offset 설정 후 X : {0}, Y: {1}, 설정 파일 X : {2}, Y: {3}",
                                            workStage.Camera_LowRes.GetOffsetX(),
                                            workStage.Camera_LowRes.GetOffsetY(),
                                            workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X,
                                            workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y);
                    Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", m_strTemp);
                }
                else
                {
                    Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "카메라 Offset 설정 실패 (카메라가 null 이거나, Offset 값이 0)");

                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Wafer 카메라 - 이미지 Offset 값이 0 입니다.     (0 < X < 200, 0 < Y < 168)\r\n\r\n[ Reticle Glass Center 조정 필요 ]");
                    return;
                }

                if (workStage.Config.ParamConfig.ReticleGlass_CenterCheck_forAlign)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "레티클 글래스 센터 확인 작업을 진행해야 합니다.\r\n\r\n###   PAK 카드를 제거하세요.!!   ###");
                    return;
                }
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "레시피 선택 취소");
            }
        }

        //private void btnJobFile_AddToJobList_Click(object sender, EventArgs e)
        //{
        //    //  Job List 에 추가


        //    ////  테스트 (가공 도면 이동)
        //    //workStage.DrillingData_RotationOffset_Move(-17.0, 2.0, 5.0, 30.0, 30.0);
        //    //return;


        //    string[] arr = new string[3];
        //    ListViewItem itm;

        //    var mb = new MessageBoxYesNo();
        //    if (DialogResult.Yes != mb.ShowDialog("Question ?", "불러온 Job File 을 연속 작업 List 에 추가하시겠습니까?"))
        //        return;

        //    if (lblJobFile_Name.Text.Length > 0)
        //    {
        //        //  현재 리스트에 등록되어 있는 job 인지 체크
        //        for ( int i = 0; i < listView_JobList.Items.Count; i++ )
        //        {
        //            if ( listView_JobList.Items[i].SubItems[1].Text == lblJobFile_Name.Text)
        //            {
        //                var mb1 = new MessageBoxOk();
        //                mb1.ShowDialog("Information !", "이미 List 에 등록되어 있는 Job 입니다.");
        //                return;
        //            }
        //        }

        //        arr[0] = (listView_JobList.Items.Count + 1).ToString();
        //        arr[1] = lblJobFile_Name.Text;
        //        arr[2] = m_strFullPath;
        //        itm = new ListViewItem(arr);

        //        listView_JobList.Items.Add(itm);

        //        lblJobFile_Name.Text = "";
        //        m_strFullPath = "";
        //    }
        //    else
        //    {
        //        var mb1 = new MessageBoxOk();
        //        mb1.ShowDialog("Information !", "먼저 Job file 을 Load 해야 합니다.");
        //    }
        //}

        //private void listView_JobList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        //{
        //    e.NewWidth = listView_JobList.Columns[e.ColumnIndex].Width;
        //    e.Cancel = true;
        //}

        private void btnMainWork_Start_Click(object sender, EventArgs e)
        {
            //  Wafer Align

            Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "Main UI, PAK & Wafer Align 시작 버튼");

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  카메라 연결 확인
            if (!workStage.Camera_HighRes.Opened ||
                !workStage.Camera_LowRes.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            //  레티클 글래스 확인
            if (workStage.Config.ParamConfig.ReticleGlass_CenterCheck_forAlign)
            {
                if (workStage.m_nReticleCheck_Step_forALIGN == (int)ReticleCheck_Step.None)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "PAK 카메라 - 레티클 글래스 센터 확인 작업을 진행해야 합니다.\r\n\r\n###   PAK 카드를 제거하세요.!!   ###");
                    return;
                }
                if (workStage.m_nReticleCheck_Step_forALIGN == (int)ReticleCheck_Step.HighResCam_Complete)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "웨이퍼 카메라 - 레티클 글래스 센터 확인 작업을 진행해야 합니다.");
                    return;
                }
                if (workStage.m_nReticleCheck_Step_forALIGN != (int)ReticleCheck_Step.LowResCam_Complete)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "레티클 글래스 센터 확인 작업을 진행해야 합니다.");
                    return;
                }
            }

            //  패턴 매칭 이미지 로드 확인 (PAK)
            if (!workStage.PatternMatchingImage_Loaded_HighRes)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 얼라인 이미지를 등록해야 합니다.\r\n\r\n[Maint] -> [JigAligner HighRes] -> [Train Image]");
                return;
            }

            //  패턴 매칭 이미지 로드 확인 (Wafer)
            if (!workStage.PatternMatchingImage_Loaded_LowRes)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer 얼라인 이미지를 등록해야 합니다.\r\n\r\n[Maint] -> [JigAligner LowRes] -> [Train Image]");
                return;
            }

            //  패턴 매칭 이미지 로드 확인 (Reticle HighRes)
            if (workStage.Config.ParamConfig.ReticleAutoCal_Usage && !workStage.PatternMatchingImage_Reticle_Loaded_HighRes)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 얼라인 이미지를 등록해야 합니다.\r\n\r\n[Maint] -> [ReticleAligner Upper] -> [Train Image]");
                return;
            }

            //  패턴 매칭 이미지 로드 확인 (Reticle LowRes)
            if (workStage.Config.ParamConfig.ReticleAutoCal_Usage && !workStage.PatternMatchingImage_Reticle_Loaded_LowRes)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 얼라인 이미지를 등록해야 합니다.\r\n\r\n[Maint] -> [ReticleAligner Lower] -> [Train Image]");
                return;
            }
            
            //  Inter-Lock
            //if (workStage.m_nWorkStage_MainStep != (int)WorkStage_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
            //    return;
            //}
            //if (workStage.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
            //    return;
            //}
            if (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (workStage.m_nReticleCheck_HighResCam_Step != (int)ReticleCheck_HighResCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (workStage.m_nReticleCheck_LowResCam_Step != (int)ReticleCheck_LowResCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (workStage.m_nSafetyPos_Move_Step != (int)WorkStage.SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }


            //  TIp Contact 위치 표시용 이미지 삭제
            //workStage.TipContactImage_Delete();

            if (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 정렬을 시작하시겠습니까?"))
                    return;

                //  안전센서로 인한 Stop 인지 확인하는 Flag 초기화
                workStage.m_bInManualMoving_SafetySensor_Detected = false;
                workStage.m_bInCycleMoving_SafetySensor_Detected = false;

                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.Start;
                workStage.timer_MainWork.Enabled = true;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 & 웨이퍼 얼라인 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 정렬을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                workStage.m_btimer_MainWork_Stop = true;
                workStage.timer_MainWork.Enabled = false;
                workStage.m_btimer_SubWork_Stop = true;
                workStage.timer_SubWork.Enabled = false;

                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;

                workStage.m_bFindUpperAlignMark_OK = false;
                workStage.m_bFindLowerAlignMark_OK = false;
                workStage.m_nFindAlignMark_Step = (int)WorkStage.FindAlignMark_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 & 웨이퍼 얼라인 중지");
            }
        }

        private void btnUpperCamera_Init_Click(object sender, EventArgs e)
        {
            //  테스트 코드

            //workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-79.9);

            workStage.DustCollectorComm_Send_Read((int)nDustCollector.DustCollector_Upper, "3000", 1);

            return;

            //workStage.m_bHomeOK = true;
            //workStage.m_nReticleGlassCheck_Cam = (int)ReticleCamType.Lower_Cam;

            Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "Main UI, 카메라 초기화");

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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
                Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "카메라 초기화, Wafer 카메라 해상도 최대");
                //MessageBox.Show("Wafer 카메라 해상도가 최대입니다.\r\n\r\n[레티클 얼라인을 위해서는 Wafer 카메라의 이미지 해상도를 변경해야 합니다.]\r\n[Width : 2248,\tHeight : 1880]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer 카메라 해상도가 최대입니다.\r\n(아래 값으로 변경 요망)\r\n\r\n[Width : 2248, Height : 1880]");
                return;
            }
            else if ((workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X == 0) || (workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y == 0))
            {
                Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "카메라 초기화, Wafer 카메라 이미지 Offset 값 0");

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
            //  테스트 코드

            int m_nStep = 1021;
            double m_dPressure = 0.0;

            workStage.ElectroRegulatorCommReceivedData_To_Pressure_Value(m_nStep, ref m_dPressure);
            return;


            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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
