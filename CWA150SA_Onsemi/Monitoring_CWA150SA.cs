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
using SpiralLab.Sirius;
using SerialCommChiller;
using SerialCommLaserPowerMeter1;
using SerialCommLaserPowerMeter2;
using SerialCommSpectraPhysics;
using static QMC.Common.Modules.WaferProbeAlign;
using QMC.Common.Motion.ACS.Motions;
using ACS.SPiiPlusNET;
using static QMC.Common.Equipment;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using netDxf.Entities;
using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;
using MessageBox = System.Windows.Forms.MessageBox;
using static QMC.Common.Parts.WaferProbeAlignParameter;
using QMC.Common.UI;
using Point = System.Drawing.Point;
using MessageBoxOk = QMC.Core.MessageBoxOk;
using MessageBoxYesNo = QMC.Core.MessageBoxYesNo;

namespace CWA150SA_Onsemi300
{
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

            WAFER_LOADING_READY = 10,                //  웨이퍼 투입 대기 위치로 이동 중...
            PROBE_PACKING = 11,                     //  프로브 카드 패킹 중...

            PROBE_UNPACKING = 12,                   //  웨이퍼, 프로브 카드 언패킹 중...
            PROBE_UNPACKING_READY = 13,             //  웨이퍼, 프로브 카드 언패킹 위치로 이동 중...

            PROBE_CARD_LOADING_READY = 14,          //  프로브 카드 투입 대기 위치로 이동 중...
            PROBE_CARD_LOCKING = 15,                //  프로브 카드 고정 진행 중...
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

        static WaferProbeAlign waferProbeAlign;

        public ProgressForm m_FormProgress;                             //  장비 초기화 시 진행창 표시

        MotionFunction MC_Func = new MotionFunction();

        private Thread m_mainDrillingCycleThread;
        private bool m_bMainDrillingCycleExit;

        private Thread m_mainUpdateCycleThread;
        private bool m_bMainUpdateCycleExit;

        private Thread m_mainAlignVisionCycleThread;                    //  Align Vision 만 돌리는 Thread
        private bool m_bMainAlignVisionCycleExit;

        private bool m_bStartBtn_Clicked { get; set; }
        private bool m_bStopBtn_Clicked { get; set; }

        System.Diagnostics.Stopwatch sw_Test = new System.Diagnostics.Stopwatch();

        static int marginWidth = 10;
        static int marginHeight = 10;
        static int btnWidth = 50;
        static int btnHeight = 50;

        private double m_dCurPackingPos_X {  set; get; }
        private double m_dCurPackingPos_Y { set; get; }
        private double m_dTargetPackingPos_X { set; get; }
        private double m_dTargetPackingPos_Y { set; get; }

        public string m_strWorkingStatus_Message { get; set; }

        //  Test용 변수
        bool m_bRun;
        string m_strTemp;
        int m_nBlink;
        bool m_bBlink;

        int m_nLedBar_Blink;
        int m_nLedBar_Blink_Step;

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

            //InitColorStatus();          //  Color 세팅
            //InitCarrierArray();         //  PCB 상태 보여주는 버튼 배열 초기화

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }


            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Lower.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Lower.SuspendDisplay();

            this.m_visionImageViewer_Upper.Camera = waferProbeAlign.Camera_Upper;
            this.m_visionImageViewer_Lower.Camera = waferProbeAlign.Camera_Lower;

            this.VisibleChanged += Monitoring_CWA150SA_VisibleChanged;

            //  Jog Control
            //m_JogControl = new JogControl(waferProbeAlign.Stage);
            ////m_JogControl.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 10, m_ModuleStateControl.Location.Y);
            //m_JogControl.Location = new Point(baseLabel_WaferChuck_Camera.Location.X + baseLabel_WaferChuck_Camera.Size.Width + 30, 285);
            //this.Controls.Add(m_JogControl);

            m_nMachineStatus = (int)MachineStatus.STATUS_NONE;

            timer_DIO_Status.Enabled = true;        //  메인 화면 IO 갱신 타이머

            if (waferProbeAlign.m_bAlignVisionThread_Use && !Equipment.m_bAlignVisionThread_1time)
            {
                Equipment.m_bAlignVisionThread_1time = true;

                ThreadStart();
            }

            //  라이브러리 초기화
            //SpiralLab.Core.Initialize();
            //            SpiralLab.Core.Ini

            //SiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();

            // 문서 생성후 뷰어에 지정
            //var doc = new DocumentDefault();
            //SiriusViewer.Document = doc;

            //Equipment.EqpSiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();

            //InitComm();

            m_nBlink = 0;
            m_bBlink = false;
            m_nLedBar_Blink = 0;
            m_nLedBar_Blink_Step = 0;

            m_bStartBtn_Clicked = false;
            m_bStopBtn_Clicked = false;

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

            //  Thread Start
            //ThreadStop();
            //ThreadStart();

            m_strWorkingStatus_Message = "";

            //  현재 등록되어 있는 Pattern Image 보여주기
            if (waferProbeAlign.jigAligner_Upper != null)
            {
                pictureBoxTrainImage_Upper.Image = waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage.GetImage();
            }

            if (waferProbeAlign.jigAligner_Lower != null)
            {
                pictureBoxTrainImage_Lower.Image = waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage.GetImage();
            }

            //  패킹 오프셋 변경용 변수
            m_dCurPackingPos_X = 0.0;
            m_dCurPackingPos_Y = 0.0;
            m_dTargetPackingPos_X = 0.0;
            m_dTargetPackingPos_Y = 0.0;
        }

        private void Monitoring_CWA150SA_VisibleChanged(object sender, EventArgs e)
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
            //if (waferProbeAlign.m_spectraPhysicsLaserComm == null)
            //{
            //    waferProbeAlign.SpectraPhysicsLaserComm_Init();
            //}
            //else
            //{
            //    if (!waferProbeAlign.m_spectraPhysicsLaserComm.IsOpen)
            //        waferProbeAlign.SpectraPhysicsLaserComm_Init();
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

            int width = waferProbeAlign.Camera_Upper.Resolution.Width;
            int height = waferProbeAlign.Camera_Upper.Resolution.Height;

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

            int width = waferProbeAlign.Camera_Upper.Resolution.Width;
            int height = waferProbeAlign.Camera_Upper.Resolution.Height;

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
            ////  Drilling Cycle Thread
            //m_bMainDrillingCycleExit = false;
            //m_mainDrillingCycleThread = new Thread(new ThreadStart(OnMainDrillingCycle));
            //m_mainDrillingCycleThread.Start();

            ////  Update Cycle Thread
            //m_bMainUpdateCycleExit = false;
            //m_mainUpdateCycleThread = new Thread(new ThreadStart(OnMainUpdateCycle));
            //m_mainUpdateCycleThread.Start();

            //  Align Vision Cycle Thread
            m_bMainAlignVisionCycleExit = false;
            m_mainAlignVisionCycleThread = new Thread(new ThreadStart(OnMainAlignVisionCycle));
            m_mainAlignVisionCycleThread.Start();
        }

        public void ThreadStop()
        {
            //m_bMainDrillingCycleExit = true;
            //m_bMainUpdateCycleExit = true;
            m_bMainAlignVisionCycleExit = true;

            //if (m_mainDrillingCycleThread != null)
            //    m_mainDrillingCycleThread.Join();

            //if (m_mainUpdateCycleThread != null)
            //    m_mainUpdateCycleThread.Join();

            if (m_mainAlignVisionCycleThread != null)
            {
                m_mainAlignVisionCycleThread.Join();
            }
        }

        protected void OnMainDrillingCycle()
        {
            while (true)
            {
                if (m_bMainDrillingCycleExit)
                {
                    break;
                }
                if (OnDrillingRun() != 0) break;
                Thread.Sleep(50);
            }
        }
        
        protected int OnDrillingRun()
        {
            int ret = 0;

            //  Laser Drilling
            waferProbeAlign.forThread_WaferProbeAlignCycle();

            return ret;
        }

        protected void OnMainUpdateCycle()
        {
            while (true)
            {
                if (m_bMainUpdateCycleExit)
                {
                    break;
                }
                if (OnUpdateRun() != 0) break;
                Thread.Sleep(1);
            }
        }

        protected int OnUpdateRun()
        {
            int ret = 0;

            //  Laser Drilling
            waferProbeAlign.forThread_UpdateCycle();

            MainUI_DIO_Status();

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

                Thread.Sleep(1);
            }
        }

        protected int OnAlignVisionRun()
        {
            int ret = 0;

            //conveyor.forThread_ConveyorCycle();
            //Thread.Sleep(1);
            ////  Inspection Vision
            //inspVision.forThread_VisionCycle();
            //Thread.Sleep(1);
            ////  Reel Feeder and Mounter
            //reelFeederAndMounter.forThread_MounterCycle();
            //Thread.Sleep(1);

            waferProbeAlign.forThread_AlignVisionCycle();
            Thread.Sleep(1);

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
            //ledStatus_BCR_Connected.Image = global::CWA150SA_Onsemi300.Properties.Resources.StopOff;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                DioPoint dioPoint = point;
            }
        }

        private void ledDispenser_ContactSensorAir_Off_Click(object sender, EventArgs e)
        {
            //ledStatus_BCR_Connected.Image = global::CWA150SA_Onsemi300.Properties.Resources.DioEllipseOn;
        }

        private void timer_DIO_Status_Tick(object sender, EventArgs e)
        {
            //  테스트 : 메인 화면 DIO 상태 갱신

            MainUI_DIO_Status();

            //  장비 상태 변경
            if (!waferProbeAlign.m_bHomeOK && (m_nMachineStatus != (int)MachineStatus.INITIALIZE_REQUIRED) &&
                (waferProbeAlign.m_nHomeStep == (int)WaferProbeAlign.Home_Step.None))
            {
                m_nMachineStatus = (int)MachineStatus.INITIALIZE_REQUIRED;
                Set_Machine_Status((int)MachineStatus.INITIALIZE_REQUIRED);
            }
            else if (!waferProbeAlign.m_bHomeOK && (m_nMachineStatus != (int)MachineStatus.MACHINE_INITIALIZING) &&
                (waferProbeAlign.m_nHomeStep >= (int)WaferProbeAlign.Home_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.MACHINE_INITIALIZING;
                Set_Machine_Status((int)MachineStatus.MACHINE_INITIALIZING);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nWaferProbeAlign_MainStep >= (int)WaferProbeAlign.WaferProbeAlign_Step.Start))
            {
                if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step >= (int)WaferProbeAlign.WaferProbeAlignErrorCheck_Step.Start)
                {
                    m_nMachineStatus = (int)MachineStatus.WAFER_ALIGN_ERROR_CHECK;
                    Set_Machine_Status((int)MachineStatus.WAFER_ALIGN_ERROR_CHECK);
                }
                else
                {
                    m_nMachineStatus = (int)MachineStatus.WAFER_ALIGN;
                    Set_Machine_Status((int)MachineStatus.WAFER_ALIGN);
                }
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nReticleCheck_UpperCam_Step >= (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.RETICLE_CHECK_UPPER;
                Set_Machine_Status((int)MachineStatus.RETICLE_CHECK_UPPER);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nReticleCheck_LowerCam_Step >= (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.RETICLE_CHECK_LOWER;
                Set_Machine_Status((int)MachineStatus.RETICLE_CHECK_LOWER);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nSafetyPos_Move_Step >= (int)WaferProbeAlign.SafetyPos_Move_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.SAFETY_POSITION_MOVE;
                Set_Machine_Status((int)MachineStatus.SAFETY_POSITION_MOVE);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nWafer_Loading_Ready_Step >= (int)WaferProbeAlign.WaferLoading_Ready_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.WAFER_LOADING_READY;
                Set_Machine_Status((int)MachineStatus.WAFER_LOADING_READY);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step >= (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.PROBE_PACKING;
                Set_Machine_Status((int)MachineStatus.PROBE_PACKING);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step >= (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.PROBE_UNPACKING;
                Set_Machine_Status((int)MachineStatus.PROBE_UNPACKING);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step >= (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.PROBE_UNPACKING_READY;
                Set_Machine_Status((int)MachineStatus.PROBE_UNPACKING_READY);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nProbeCard_Loading_Ready_Step >= (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.PROBE_CARD_LOADING_READY;
                Set_Machine_Status((int)MachineStatus.PROBE_CARD_LOADING_READY);
            }
            else if (waferProbeAlign.m_bHomeOK && (waferProbeAlign.m_nProbeCard_Locking_Step >= (int)WaferProbeAlign.ProbeCard_Locking_Step.Start))
            {
                m_nMachineStatus = (int)MachineStatus.PROBE_CARD_LOCKING;
                Set_Machine_Status((int)MachineStatus.PROBE_CARD_LOCKING);
            }
            else if (waferProbeAlign.m_bHomeOK)// && (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None))
            {
                m_nMachineStatus = (int)MachineStatus.MACHINE_READY;
                Set_Machine_Status((int)MachineStatus.MACHINE_READY);
            }

            //  버튼 활성화 여부
            if (waferProbeAlign.m_bHomeOK)
            {
                //btnMainWork_Start.Enabled = true;

                if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None)
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

            ////  가공 상태 메세지 표시
            //if (waferProbeAlign.m_nWaferProbeAlign_MainStep >= (int)WaferProbeAlign.WaferProbeAlign_Step.Start)
            //{
            //    if ( waferProbeAlign.m_nMyWaferAlign_MainStep >= (int)WaferProbeAlign.MyWaferAlign_Step.Start)
            //    {
            //        m_strWorkingStatus_Message = "웨이퍼 얼라인 Cycle 진행중...";
            //    }
            //    else if (waferProbeAlign.m_nPEGProcess_MainStep >= (int)WaferProbeAlign.PEGProcess_Step.Start)
            //    {
            //        m_strWorkingStatus_Message = "Stage 기준 가공 Cycle 진행중...";
            //    }
            //    else if (waferProbeAlign.m_nRTCProcess_MainStep >= (int)WaferProbeAlign.RTCProcess_Step.Start)
            //    {
            //        m_strWorkingStatus_Message = "Scanner 기준 가공 Cycle 진행중...";
            //    }
            //}
            //else
            //{
            //    m_strWorkingStatus_Message = "";
            //}


            ////  메인 화면 도면 갱신 (요상스럽도다... 메인 화면에 도면을 불러온 후 다른 화면으로 넘어갔다가 돌아오면, 메인 화면의 Viewer 에 도면이 사라진다. 보이기만 안보이는 게 아니라 데이터도 사라진다. 
            ////                      그래서 Equipment 에 SiriusView 를 하나 임시로 두고, 서로 데이터가 다를 경우(로드된 파일명) 임시 Viewer 의 데이터를 메인 화면의 Viewer 로 가져온다.
            //if ((SiriusViewer.Document != null) && (Equipment.EqpSiriusViewer.Document != null))
            //{
            //    if ((SiriusViewer.Document.FileName != Equipment.EqpSiriusViewer.Document.FileName) &&
            //        (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None))            //  자동운전이 아닐 때만 데이터를 Copy 하도록
            //    {
            //        SiriusViewer.Document = Equipment.EqpSiriusViewer.Document;
            //        waferProbeAlign.MainSiriusViewer.Document = Equipment.EqpSiriusViewer.Document;
            //    }
            //}

            //  서보 알람 상태 표시
            bool m_bAxisServoAlarm = false;
            
            for ( int i = 0; i < 7; i++ )           //  Ajin 모션 축 수 가져오기 (Enum Count 확인)
            {
                if (waferProbeAlign.MC_Func.MC_IsAlarm(i))
                    m_bAxisServoAlarm = true;
            }


            //  패턴 이미지 다시 보여주기
            //if (waferProbeAlign.m_bUpperCam_AlignPattern_Reset)
            if (waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage != null)
            {
                //waferProbeAlign.m_bUpperCam_AlignPattern_Reset = false;

                if (waferProbeAlign.jigAligner_Upper != null)
                {
                    pictureBoxTrainImage_Upper.Image = waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage.GetImage();
                }
            }

            //if (waferProbeAlign.m_bLowerCam_AlignPattern_Reset)
            if (waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage != null)
            {
                //waferProbeAlign.m_bLowerCam_AlignPattern_Reset = false;

                if (waferProbeAlign.jigAligner_Lower != null)
                {
                    pictureBoxTrainImage_Lower.Image = waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage.GetImage();
                }
            }

            //  웨이퍼, 프로브 카드 틀어진 각도 보여주기
            lblUpperCamera_AlignData.Text = waferProbeAlign.m_strProbeCard_TiltData_for_Display;
            lblLowerCamera_AlignData.Text = waferProbeAlign.m_strWafer_TiltData_for_Display;


            Equipment.Packing_AutoStart_Mode = waferProbeAlign.Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage;
            Equipment.AlignErrorCheck_AutoStart_Mode = waferProbeAlign.Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage;
            Equipment.PackingOffset_Use = waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Usage;
            Equipment.Pak_AirLineCheck_Use = waferProbeAlign.Config.ParamConfig.Pak_AirLineCheck_Usage;
            Equipment.ProbeCard_ClampType = waferProbeAlign.Config.ParamConfig.ProbeCard_ClampType;

            //  ProbeCard Clamp Type 에 따라 UI 변경
            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
            {
                if (tabControl_ProbeCard_ClampType.SelectedIndex != (int)WaferProbeAlign.nProbeClampType.Type_A)
                {
                    tabControl_ProbeCard_ClampType.SelectTab((int)WaferProbeAlign.nProbeClampType.Type_A);
                }
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
            {
                if (tabControl_ProbeCard_ClampType.SelectedIndex != (int)WaferProbeAlign.nProbeClampType.Type_B)
                {
                    tabControl_ProbeCard_ClampType.SelectTab((int)WaferProbeAlign.nProbeClampType.Type_B);
                }
            }


            //  웨이퍼 얼라인 에러 상태 보여주기
            //  패킹을 완료했으니, 얼라인 상태 값들은 초기화 한다.
            if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].X == 0.0) ||
                (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].Y == 0.0) ||
                (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].X == 0.0) ||
                (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].Y == 0.0) ||
                (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].X == 0.0) ||
                (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].Y == 0.0) )
            {
                baseLabelPosition_Top.BackColor = Color.Black;
                baseLabelPosition_Top.ForeColor = Color.Yellow;
                baseLabelPosition_Mid.BackColor = Color.Black;
                baseLabelPosition_Mid.ForeColor = Color.Yellow;
                baseLabelPosition_Bot.BackColor = Color.Black;
                baseLabelPosition_Bot.ForeColor = Color.Yellow;

                //lblUpperCamera_Top_ErrorData_X.Text = "- - -";
                //lblUpperCamera_Top_ErrorData_Y.Text = "- - -";

                //lblUpperCamera_Mid_ErrorData_X.Text = "- - -";
                //lblUpperCamera_Mid_ErrorData_Y.Text = "- - -";

                //lblUpperCamera_Bot_ErrorData_X.Text = "- - -";
                //lblUpperCamera_Bot_ErrorData_Y.Text = "- - -";

                lblLowerCamera_Top_ErrorData_X.Text = "- - -";
                lblLowerCamera_Top_ErrorData_Y.Text = "- - -";

                lblLowerCamera_Mid_ErrorData_X.Text = "- - -";
                lblLowerCamera_Mid_ErrorData_Y.Text = "- - -";

                lblLowerCamera_Bot_ErrorData_X.Text = "- - -";
                lblLowerCamera_Bot_ErrorData_Y.Text = "- - -";
            }
            else
            {
                //lblUpperCamera_Top_ErrorData_X.Text = waferProbeAlign.AlignmentErrorCheck_Delta[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Upper].X.ToString();
                //lblUpperCamera_Top_ErrorData_Y.Text = waferProbeAlign.AlignmentErrorCheck_Delta[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Upper].Y.ToString();

                //lblUpperCamera_Mid_ErrorData_X.Text = waferProbeAlign.AlignmentErrorCheck_Delta[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Upper].X.ToString();
                //lblUpperCamera_Mid_ErrorData_Y.Text = waferProbeAlign.AlignmentErrorCheck_Delta[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Upper].Y.ToString();

                //lblUpperCamera_Bot_ErrorData_X.Text = waferProbeAlign.AlignmentErrorCheck_Delta[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Upper].X.ToString();
                //lblUpperCamera_Bot_ErrorData_Y.Text = waferProbeAlign.AlignmentErrorCheck_Delta[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Upper].Y.ToString();

                if (waferProbeAlign.m_bWaferProbeAlign_ErrorCheck_Complete)
                {
                    if ((waferProbeAlign.AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Upper] == true) &&
                        (waferProbeAlign.AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Lower] == true))
                    {
                        baseLabelPosition_Top.BackColor = Color.Lime;
                        baseLabelPosition_Top.ForeColor = Color.Black;
                    }
                    else
                    {
                        baseLabelPosition_Top.BackColor = Color.Red;
                        baseLabelPosition_Top.ForeColor = Color.Yellow;
                    }

                    if ((waferProbeAlign.AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Upper] == true) &&
                        (waferProbeAlign.AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Lower] == true))
                    {
                        baseLabelPosition_Mid.BackColor = Color.Lime;
                        baseLabelPosition_Mid.ForeColor = Color.Black;
                    }
                    else
                    {
                        baseLabelPosition_Mid.BackColor = Color.Red;
                        baseLabelPosition_Mid.ForeColor = Color.Yellow;
                    }

                    if ((waferProbeAlign.AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Upper] == true) &&
                        (waferProbeAlign.AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Lower] == true))
                    {
                        baseLabelPosition_Bot.BackColor = Color.Lime;
                        baseLabelPosition_Bot.ForeColor = Color.Black;
                    }
                    else
                    {
                        baseLabelPosition_Bot.BackColor = Color.Red;
                        baseLabelPosition_Bot.ForeColor = Color.Yellow;
                    }


                    lblLowerCamera_Top_ErrorData_X.Text = (waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Upper].X -
                                                            waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Lower].X).ToString();
                    lblLowerCamera_Top_ErrorData_Y.Text = (waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Upper].Y -
                                                            waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Lower].Y).ToString();

                    lblLowerCamera_Mid_ErrorData_X.Text = (waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Upper].X -
                                                            waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Lower].X).ToString();
                    lblLowerCamera_Mid_ErrorData_Y.Text = (waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Upper].Y -
                                                            waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Lower].Y).ToString();

                    lblLowerCamera_Bot_ErrorData_X.Text = (waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Upper].X -
                                                            waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Lower].X).ToString();
                    lblLowerCamera_Bot_ErrorData_Y.Text = (waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Upper].Y -
                                                            waferProbeAlign.AlignmentErrorCheck_MarkPosition[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Lower].Y).ToString();
                }
            }
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
            //    pictureBoxGantryStatus.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxGantryStatus.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            //
            ///////////////////////////////////////////////////////////////////////////////////////
        }

        private void MainUI_DIO_Status()
        {
            if (waferProbeAlign.m_nHomeStep == (int)WaferProbeAlign.Home_Step.Complete)
            {
                m_FormProgress.Hide();
            }

            //  도면 Load 했는지?
            //if ( waferProbeAlign.DrillingData_Loaded )
            //{
            //    waferProbeAlign.DrillingData_Loaded = false;

            //    SiriusViewer.Document = waferProbeAlign.MainSiriusViewer.Document;
            //}

            //  Blink
            m_nBlink++;
            if ((m_nBlink > 0) && (m_nBlink <= 20))
            {
                m_bBlink = true;
            }
            else if ((m_nBlink > 20) && (m_nBlink <= 40))
            {
                m_bBlink = false;
            }
            else
            {
                m_nBlink = 0;
            }

            //  알람 발생 시 빨간색 LED Bar Blink
            if (Equipment.MachineStop_byAlarm == true)
            {
                if (m_bBlink)
                {
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Red_On();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Green_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
                    }
                }
                else
                {
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Red_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Green_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
                    }
                }
            }
            else
            {
                //  작업 중인지?
                if ((waferProbeAlign.m_nWaferProbeAlign_MainStep > (int)WaferProbeAlign.WaferProbeAlign_Step.None) &&
                    (waferProbeAlign.m_nWaferProbeAlign_MainStep < (int)WaferProbeAlign.WaferProbeAlign_Step.Complete))
                {
                    Equipment.Start();

                    if (m_bBlink)
                    {
                        btnRunStatus_Drilling.BackColor = Color.Lime;
                        btnRunStatus_Drilling2.BackColor = Color.Gray;
                    }
                    else
                    {
                        btnRunStatus_Drilling.BackColor = Color.Gray;
                        btnRunStatus_Drilling2.BackColor = Color.Lime;
                    }

                    if (!waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Start())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Start(true);
                    }
                    if (waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Stop())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Stop(false);
                    }
                    if (waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Reset())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Stop(false);
                    }

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

                    //  LED Bar
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Red_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Green_On();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
                    }
                }
                else if (((waferProbeAlign.m_nWafer_ProbeCard_Packing_Step > (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None) &&
                        (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step < (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Complete)) ||
                        ((waferProbeAlign.m_nWaferProbeCard_Unpacking_Step > (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.None) &&
                        (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step < (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.Complete)))
                {
                    Equipment.Start();

                    if (!waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Start())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Start(true);
                    }
                    if (waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Stop())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Stop(false);
                    }
                    if (waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Reset())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Stop(false);
                    }

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

                    //  LED Bar
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Red_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Green_Off();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Blue_On();
                    }
                }
                else if ((waferProbeAlign.m_nHomeStep > (int)WaferProbeAlign.Home_Step.None) &&
                        (waferProbeAlign.m_nHomeStep < (int)WaferProbeAlign.Home_Step.Complete))                     //  홈 실행 중이면? 흰색 깜빡이도록l
                {
                    if (m_bBlink)
                    {
                        if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() == 0)
                        {
                            CommonModule.Instance.TowerLamp.LedBar_Red_On();
                        }
                        if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() == 0)
                        {
                            CommonModule.Instance.TowerLamp.LedBar_Green_On();
                        }
                        if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() == 0)
                        {
                            CommonModule.Instance.TowerLamp.LedBar_Blue_On();
                        }
                    }
                    else
                    {
                        if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() != 0)
                        {
                            CommonModule.Instance.TowerLamp.LedBar_Red_Off();
                        }
                        if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() != 0)
                        {
                            CommonModule.Instance.TowerLamp.LedBar_Green_Off();
                        }
                        if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
                        {
                            CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
                        }
                    }

                    ////  LED Bar Blink
                    //m_nLedBar_Blink++;
                    //if ((m_nLedBar_Blink > 0) && (m_nLedBar_Blink <= 20))
                    //{
                    //    m_nLedBar_Blink_Step = 0;
                    //}
                    //else if ((m_nLedBar_Blink > 20) && (m_nLedBar_Blink <= 40))
                    //{
                    //    m_nLedBar_Blink_Step = 1;
                    //}
                    //else if ((m_nLedBar_Blink > 40) && (m_nLedBar_Blink <= 60))
                    //{
                    //    m_nLedBar_Blink_Step = 2;
                    //}
                    //else
                    //{
                    //    m_nLedBar_Blink = 0;
                    //}

                    ////  LED Bar Step
                    //switch(m_nLedBar_Blink_Step)
                    //{
                    //    case 0:                             //  Green + Red
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() == 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Red_On();
                    //        }
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() == 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Green_On();
                    //        }
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
                    //        }
                    //        break;


                    //    case 1:                             //  Green + Blue
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() != 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Red_Off();
                    //        }
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() == 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Green_On();
                    //        }
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() == 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Blue_On();
                    //        }
                    //        break;


                    //    case 2:                             //  Blue + Red
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() == 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Red_On();
                    //        }
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() != 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Green_Off();
                    //        }
                    //        if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() == 0)
                    //        {
                    //            CommonModule.Instance.TowerLamp.LedBar_Blue_On();
                    //        }
                    //        break;
                    //}    
                }
                else
                {
                    Equipment.Stop();

                    btnRunStatus_Drilling.BackColor = Color.Maroon;
                    btnRunStatus_Drilling2.BackColor = Color.Maroon;

                    if (waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Start())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Start(false);
                    }
                    if (!waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Stop())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Stop(true);
                    }
                    if (waferProbeAlign.waferProbeAlignParameter.IsDO_OpLamp_Reset())
                    {
                        waferProbeAlign.waferProbeAlignParameter.DO_OpLamp_Stop(false);
                    }

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

                    //  LED Bar
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Red_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Red_On();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Green_On() == 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Green_On();
                    }
                    if (CommonModule.Instance.TowerLamp.Is_LedBar_Blue_On() != 0)
                    {
                        CommonModule.Instance.TowerLamp.LedBar_Blue_Off();
                    }
                }
            }

            //  얼라인 Flag 리셋 조건
            //if (waferProbeAlign.m_bWafer_ThetaAlign_OK)                       //  얼라인 Flag 가 true 인 상태에서, Wafer 나 Thin-Chuck Vacuum 을 해제하면 얼라인을 다시 하도록 한다.
            //{
            //    if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck() ||
            //        !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            //    {
            //        waferProbeAlign.m_bWafer_ThetaAlign_OK = false;
            //    }
            //}

            //if (waferProbeAlign.m_bWafer_XYAlign_OK)                         //  얼라인 Flag 가 true 인 상태에서, Wafer 나 Thin-Chuck Vacuum 을 해제하면 얼라인을 다시 하도록 한다.
            //{
            //    if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck() ||
            //        !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            //    {
            //        waferProbeAlign.m_bWafer_XYAlign_OK = false;
            //    }
            //}

            ////  Laser Power 표시
            //baseLabel_LaserPower.Text = waferProbeAlign.m_dDrilling_Power.ToString();
            //if (waferProbeAlign.Config.ParamConfig.LaserPowerAutoChange || waferProbeAlign.Config.ParamConfig.LaserPowerChange_byPowerMeter)        //  Power 값에 대한 % 
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
                ledStatus_Light_Connected.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            }
            else
            {
                ledStatus_Light_Connected.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOn;
            }
            //
            ///////////////////////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Input
            //   
            
            if (CommonModule.Instance.OperationButtons.IsStop() ||
                CommonModule.Instance.OperationButtons.IsReset())
            {
                Equipment.MachineStop_byAlarm = false;

                //  동작도 정지시킬지는.... 좀 보자...
            }

            if (waferProbeAlign.waferProbeAlignParameter.DI_Main_CDACheck())
                pictureBoxMainAirCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxMainAirCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

            if (waferProbeAlign.waferProbeAlignParameter.DI_Main_VacuumCheck())
                pictureBoxMainVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxMainVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

            if (waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
                pictureBoxThinChuckVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxThinChuckVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

            if (waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
                pictureBoxWaferVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxWaferVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

            if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_PackingCheck())
                pictureBoxProbePackingCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxProbePackingCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

            if (waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
                pictureBoxThinChuckDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxThinChuckDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)                                       //  1호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
                    pictureBoxProbeBWDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeBWDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up())
                    pictureBoxTopCoverUp.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxTopCoverUp.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
                    pictureBoxTopCoverDown.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxTopCoverDown.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)                                  //  2 ~ 6호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_LeftClampModule_FW())
                    pictureBoxProbeLeftClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeLeftClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_LeftClampModule_BW())
                    pictureBoxProbeLeftClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeLeftClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_RightClampModule_FW())
                    pictureBoxProbeRightClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeRightClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_RightClampModule_BW())
                    pictureBoxProbeRightClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeRightClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    pictureBoxProbeUnpackingCyl_Down.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeUnpackingCyl_Down.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up())
                    pictureBoxProbeUnpackingCyl_Up.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeUnpackingCyl_Up.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            }


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Output
            //

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_StageCleaning())
                baseButtonThinChuckStageCleaning.BackColor = Color.Lime;
            else
                baseButtonThinChuckStageCleaning.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_Vacuum())
                baseButtonThinChuckVacuum.BackColor = Color.Lime;
            else
                baseButtonThinChuckVacuum.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Wafer_Vacuum())
                baseButtonWaferVacuum.BackColor = Color.Lime;
            else
                baseButtonWaferVacuum.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Packing())
                baseButtonProbePacking.BackColor = Color.Lime;
            else
                baseButtonProbePacking.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Unpacking())
                baseButtonProbeUnpacking.BackColor = Color.Lime;
            else
                baseButtonProbeUnpacking.BackColor = Color.DimGray;

            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)                                       //  1호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Up())
                    baseButtonTopCoverUp.BackColor = Color.Lime;
                else
                    baseButtonTopCoverUp.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Down())
                    baseButtonTopCoverDown.BackColor = Color.Lime;
                else
                    baseButtonTopCoverDown.BackColor = Color.DimGray;
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)                                  //  2 ~ 6호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
                    baseButtonProbeClamp_Down.BackColor = Color.Lime;
                else
                    baseButtonProbeClamp_Down.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_FW())
                    baseButtonProbeClamp_FW.BackColor = Color.Lime;
                else
                    baseButtonProbeClamp_FW.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_BW())
                    baseButtonProbeClamp_BW.BackColor = Color.Lime;
                else
                    baseButtonProbeClamp_BW.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Down())
                    baseButtonProbeUnpackingCyl_Down.BackColor = Color.Lime;
                else
                    baseButtonProbeUnpackingCyl_Down.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Up())
                    baseButtonProbeUnpackingCyl_Up.BackColor = Color.Lime;
                else
                    baseButtonProbeUnpackingCyl_Up.BackColor = Color.DimGray;
            }
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
            //if (m_nMachineStatus == nStatus)
            //    return;

            switch ( nStatus )
            {
                case (int)MachineStatus.INITIALIZE_REQUIRED:
                    lblMachine_Status.Text = "장비 초기화가 필요합니다.";
                    break;


                case (int)MachineStatus.MACHINE_INITIALIZING:
                    lblMachine_Status.Text = "장비 초기화 중...";
                    break;


                case (int)MachineStatus.MACHINE_READY:
                    lblMachine_Status.Text = "작업 대기";
                    break;


                case (int)MachineStatus.WAFER_ALIGN:
                    lblMachine_Status.Text = "웨이퍼 정렬 작업 진행 중...";
                    break;


                case (int)MachineStatus.WAFER_ALIGN_ERROR_CHECK:
                    lblMachine_Status.Text = "웨이퍼 정렬 오차 검증 작업 진행 중...";
                    break;


                case (int)MachineStatus.RETICLE_CHECK_UPPER:
                    lblMachine_Status.Text = "Reticle 정렬 위치 이동 중... (상부 카메라)";
                    break;


                case (int)MachineStatus.RETICLE_CHECK_LOWER:
                    lblMachine_Status.Text = "Reticle 정렬 위치 이동 중... (하부 카메라)";
                    break;


                case (int)MachineStatus.SAFETY_POSITION_MOVE:
                    lblMachine_Status.Text = "안전 위치로 이동 중...";
                    break;


                case (int)MachineStatus.WAFER_LOADING_READY:
                    lblMachine_Status.Text = "웨이퍼 투입 대기 위치로 이동 중...";
                    break;


                case (int)MachineStatus.PROBE_PACKING:
                    lblMachine_Status.Text = "웨이퍼 && 프로브 카드 패킹 작업 진행 중...";
                    break;


                case (int)MachineStatus.PROBE_UNPACKING:
                    lblMachine_Status.Text = "웨이퍼 && 프로브 카드 언패킹 작업 중...";
                    break;


                case (int)MachineStatus.PROBE_UNPACKING_READY:
                    lblMachine_Status.Text = "웨이퍼 && 프로브 카드 언패킹 준비 위치로 이동 중...";
                    break;


                case (int)MachineStatus.PROBE_CARD_LOADING_READY:
                    lblMachine_Status.Text = "프로브 카드 투입 대기 위치로 이동 중...";
                    break;


                case (int)MachineStatus.PROBE_CARD_LOCKING:
                    lblMachine_Status.Text = "프로브 카드 고정 작업 진행 중...";
                    break;
            }
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
            //                fd.CustomPlaces.Add(CWA150SA_Onsemi.Properties.Settings.Default.JobFolder);
            ////                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
            //                fd.Filter = "sirius files (*.sirius)|*.sirius|All files (*.*)|*.*"; //필터 설정
            //                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

            //                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "작업 파일 불러오기");

            //                //  Arc 를 Polyline 으로 만들 경우
            //                if (waferProbeAlign.Config.ParamConfig.ConvertArcToPolyline)
            //                {
            //                    Config.LwPolylineBulgeToLines = true;

            //                    if (waferProbeAlign.Config.ParamConfig.ArcToPolyline_Resolution < 1)
            //                        Config.LwPolylineBulgePrecision = 15;
            //                    else
            //                        Config.LwPolylineBulgePrecision = waferProbeAlign.Config.ParamConfig.ArcToPolyline_Resolution;
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

            //                    if (waferProbeAlign.MainSiriusViewer == null)
            //                    {
            //                        waferProbeAlign.MainSiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();
            //                    }

            //                    waferProbeAlign.MainSiriusViewer.Document = SiriusViewer.Document;
            //                    Equipment.EqpSiriusViewer.Document = SiriusViewer.Document;

            //                    //  가공 시간 초기화
            //                    Equipment.WorkElapsedTick = 0;
            //                    Equipment.WorkElapsedTick_Outline = 0;
            //                    Equipment.WorkElapsedTick_Thruhole = 0;
            //                    Equipment.WorkElapsedTick_Drilling = 0;
            //                    Equipment.WorkElapsedTick_Marking = 0;

            //                    //  Get Data
            //                    m_nReturn = waferProbeAlign.GetDrillingData();
            //                    switch (m_nReturn)
            //                    {
            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_SUCCESS:
            //                            var mb0 = new MessageBoxOk();
            //                            mb0.ShowDialog("Information !", "데이터가 정상적으로 로드 되었습니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_FAIL:
            //                            var mb1 = new MessageBoxOk();
            //                            mb1.ShowDialog("Information !", "데이터가 정상적으로 로드 되지 않았습니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_NOT_GROUP:
            //                            var mb2 = new MessageBoxOk();
            //                            mb2.ShowDialog("Information !", "데이터가 Group 이 아닙니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_UNGROUP:
            //                            var mb3 = new MessageBoxOk();
            //                            mb3.ShowDialog("Information !", "데이터를 Group 해제 해야 합니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_LAYERNAME_NG:
            //                            var mb4 = new MessageBoxOk();
            //                            mb4.ShowDialog("Information !", "Layer Name 은 '쓰루홀', '외곽선', '드릴링', '마킹' 4가지만 가능합니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_MOTIONTYPE_NG:
            //                            var mb5 = new MessageBoxOk();
            //                            mb5.ShowDialog("Information !", "Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_DRILDATA_NG:
            //                            var mb6 = new MessageBoxOk();
            //                            mb6.ShowDialog("Information !", "Drilling Data 는 Polyline, Line, Circle 중 한 가지 데이터로만 구성되어야 합니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_DRILDATA_LINECNT:
            //                            var mb7 = new MessageBoxOk();
            //                            mb7.ShowDialog("Information !", "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED:
            //                            var mb8 = new MessageBoxOk();
            //                            mb8.ShowDialog("Information !", "Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다.");
            //                            break;

            //                        case (int)WaferProbeAlign.nGetDataResult.GETDATA_DRILDATA_NOT_GROUP:
            //                            var mb9 = new MessageBoxOk();
            //                            mb9.ShowDialog("Information !", "Drilling 데이터가 Group 이 아닙니다.");
            //                            break;
            //                    }
            //                }
            //            }
        }

        static void SyncAxisViewer(IRtcSyncAxis rtcSyncAxis)
        {
            var exeFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncaxis", "Tools", "syncAXIS_Viewer", "syncAXIS_Viewer.exe");
            string simulatedFileName = Path.Combine(exeFileName, rtcSyncAxis.SimulationFileName);
            if (File.Exists(simulatedFileName))
            {
                Task.Run(() =>
                {
                    // Notice
                    // syncAxisViewer v1.6 의 버그로 인해 아래와 같이 외부에서 파일을 인자로 하여 뷰어 프로세스를 생성하면 일부 데이타 누락이 발생되기도 함
                    // 이때는 재차 open 을 하면 해결되며, SCANLAB 에 버그 리포트 된 사항임
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncaxis", "Tools", "syncAXIS_Viewer");
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = Config.ConfigSyncAxisViewerFileName;
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    startInfo.Arguments = "-a";//string.Empty;
                    if (!string.IsNullOrEmpty(simulatedFileName))
                        startInfo.Arguments = Path.Combine(Config.ConfigSyncAxisSimulateFilePath, simulatedFileName);
                    try
                    {
                        using (var proc = Process.Start(startInfo))
                        {
                            proc.WaitForExit();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.Log(Logger.Type.Error, ex.Message);
                    }
                });
            }
        }

        private void btnMainWork_Start_Click(object sender, EventArgs e)
        {
            //  Main Work Start            

            MessageBoxOk mb2 = new MessageBoxOk();
            mb2.ShowDialog("Information !", "사용하지 않는 버튼.");
            return;

            string m_strTemp = "";

            Log.Write("MonitoringCWA150SA", "Start 버튼");

            return;

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Scanner Mode 를 선택해야 합니다.");
                return;
            }

            //if (!waferProbeAlign.waferProbeAlignParameter.DI_FrontDoor())
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Front Door 가 열려있습니다.");
            //    return;
            //}

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "가공을 시작하시겠습니까?"))
                    return;

                ////  Job List
                //if (listView_JobList.Items.Count > 0)
                //{
                //    waferProbeAlign.m_stJobList.m_nContinuousJobCount = listView_JobList.Items.Count;

                //    for (int i = 0; i < listView_JobList.Items.Count; i++)
                //    {
                //        waferProbeAlign.m_stJobList.m_strJobFullPath[i] = listView_JobList.Items[i].SubItems[2].Text;
                //    }
                //}

                //  가공 시간 초기화
                Equipment.WorkElapsedTick = 0;
                Equipment.WorkElapsedTick_Outline = 0;
                Equipment.WorkElapsedTick_Thruhole = 0;
                Equipment.WorkElapsedTick_Drilling = 0;
                Equipment.WorkElapsedTick_Marking = 0;

                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;

                WorkStartTick = Environment.TickCount;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "가공을 중지하시겠습니까?"))
                    return;

                WorkStartTick = 0;
                WorkStartTick_Outline = 0;
                WorkStartTick_Thruhole = 0;
                WorkStartTick_Drilling = 0;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.None;
            }
        }

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

            //if (!waferProbeAlign.waferProbeAlignParameter.DI_OpSwitch_Reset() || !waferProbeAlign.waferProbeAlignParameter.DI_OpSwitch_EMG())
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "전면의 Reset 버튼을 눌러야 합니다.");
            //    return;
            //}

            if (waferProbeAlign.m_nHomeStep == (int)Home_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "장비를 초기화 하시겠습니까?"))
                    return;

                //if (!waferProbeAlign.waferProbeAlignParameter.DI_FrontDoor() && waferProbeAlign.Config.ParamConfig.DoorInterlock_Enable)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Front Door 가 열려있습니다.");
                //    return;
                //}

                //if (!waferProbeAlign.waferProbeAlignParameter.DI_LeftDoor() && waferProbeAlign.Config.ParamConfig.DoorInterlock_Enable)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Left Door 가 열려있습니다.");
                //    return;
                //}

                //if (!waferProbeAlign.waferProbeAlignParameter.DI_RightDoor() && waferProbeAlign.Config.ParamConfig.DoorInterlock_Enable)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Right Door 가 열려있습니다.");
                //    return;
                //}

                //if (!waferProbeAlign.waferProbeAlignParameter.DI_RearDoor() && waferProbeAlign.Config.ParamConfig.DoorInterlock_Enable)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Rear Door 가 열려있습니다.");
                //    return;
                //}

                //  여기서 카메라 초기화 해 줌
                //waferProbeAlign.Camera.BeginInitialize();
                //waferProbeAlign.Camera_LowRes.BeginInitialize();
                //waferProbeAlign.Camera_Upper.Initialize();
                //waferProbeAlign.Camera_Lower.Initialize();

                //Task<int> task = dispenserAndScale.Camera.BeginInitialize();
                //ProgressForm progressForm = new ProgressForm("Camera Initialize", "Working...", task);
                //progressForm.StopProcess += ProgressForm_StopProcess;
                //progressForm.ShowDialog();

                waferProbeAlign.m_bHomeOK = false;

                waferProbeAlign.m_nHomeStep = (int)Home_Step.Start;

                //  Motion 홈 실행 타이머
                waferProbeAlign.timer_Motion_Home.Enabled = true;

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
                    waferProbeAlign.timer_Motion_Home.Enabled = false;

                    waferProbeAlign.m_nHomeStep = (int)Home_Step.None;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
                    //    ACSSPiiPlusMotionBoard.Api.Halt((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);
                    //}

                    if (Equipment.AjinBoard_Opened)
                    {
                        for( int i = 0; i < 7; i++ )
                        {
                            MC_Func.MC_JogStop(i);
                        }
                        //MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.StageTH);
                        //MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ);
                        //MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.ScannerZ);
                        //MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.RvaX);
                        //MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.RvaZ);
                    }

                    //if (Equipment.AjinBoard_Opened)
                    //    MC_Func.MC_JogStop(0);
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
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "레시피 변경 시작");

            RecipeInfoCollection recipes = DataManager.Instance.Recipe;
            FormRecipeList formRecipeList = new FormRecipeList(recipes);
            formRecipeList.BringToFront();
            formRecipeList.StartPosition = FormStartPosition.CenterScreen;
            if (formRecipeList.ShowDialog() == DialogResult.OK)
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "레시피 선택 완료");

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
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                DataManager.Instance.UpdateConfigData(waferProbeAlign);

                //  Config 화면에 데이터가 갱신되도록 하기 위해서. (각 Form 의 Timer 가 1초에 한번씩 이 변수를 체크해서 갱신해준다.) 크게 부하 받지는 않으니.... 꼼수..
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

                if (CurrentRecipe != null)
                {
                    Equipment.UpdateRecipeData();
                }
                Equipment.SetCurrentRecipe(CurrentRecipe);
                Equipment.SaveRecipe();

                waferProbeAlign.m_bUpperCam_AlignPattern_Reset = true;
                waferProbeAlign.m_bLowerCam_AlignPattern_Reset = true;


                waferProbeAlign.Machine_Parameter_Load();
            }
            else
            {

            }
        }

        //private void btnJobFile_AddToJobList_Click(object sender, EventArgs e)
        //{
        //    //  Job List 에 추가


        //    ////  테스트 (가공 도면 이동)
        //    //waferProbeAlign.DrillingData_RotationOffset_Move(-17.0, 2.0, 5.0, 30.0, 30.0);
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

        private void btnTest_EngineerMode_Click(object sender, EventArgs e)
        {

        }

        private void btnTest_AlarmReset_Click(object sender, EventArgs e)
        {
            //  Servo Alarm Reset

            for (int i = 0; i <= 5; i++)
            {
                if (i != 3)                         //  4번째 축은 사용하지 않음. (0, 1, 2, 4, 5) 
                {
                    if (waferProbeAlign.MC_Func.MC_IsAlarm(i))
                    {
                        waferProbeAlign.MC_Func.MC_SetServoOnOff(i, false);
                        waferProbeAlign.MC_Func.MC_AlarmReset(i, true);
                        Thread.Sleep(300);
                        waferProbeAlign.MC_Func.MC_AlarmReset(i, false);
                    }
                    else if (!waferProbeAlign.MC_Func.MC_IsServoOn(i))
                    {
                        waferProbeAlign.MC_Func.MC_SetServoOnOff(i, true);
                    }
                }
            }
        }

        private void btnMainWork_Start_Click_1(object sender, EventArgs e)
        {
            //  Wafer Align            

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            CommonModule.Instance.TowerLamp.Lamp0_Off();
            CommonModule.Instance.TowerLamp.Lamp1_Off();

            waferProbeAlign.m_bProbeCard_TiltCheck_Only = false;               //  ProbeCard Tilt Check Only
            waferProbeAlign.m_bWafer_Align_Only = false;                       //  Wafer Align Only

            //  얼라인 버튼을 누르면 수동 패킹 탭으로 변경 (얼라인 후 수동으로 패킹 해야 하기 때문에, 미리 페이지를 수동패킹 페이지로 변경한다.)
            tabControl_User.SelectedIndex = 0;                                  //  0: 수동 패킹        1: 패킹 오프셋 변경        2: 레티클 글래스 위치 변경

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Wafer 유무 확인
            if (waferProbeAlign.Config.ParamConfig.Wafer_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (waferProbeAlign.Config.ParamConfig.ThinChuck_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Inter-Lock
            //if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
            //    return;
            //}
            //if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
            //    return;
            //}
            if ((waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign_Step.None) && (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None))
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

            //  이미지 저장소 용량 체크
            if (waferProbeAlign.Config.ParamConfig.AlignImageSaveFolder_FreeSpaceCheck_Usage)
            {
                double m_dSpace = 0.0;
                double m_dWarningSpace = 0.0;
                m_dSpace = waferProbeAlign.GetDriveSpace("D");

                m_dWarningSpace = waferProbeAlign.Config.ParamConfig.AlignImageSaveFolder_WarningSpace <= 0 ? 10.0 : waferProbeAlign.Config.ParamConfig.AlignImageSaveFolder_WarningSpace;
                if (m_dSpace <= m_dWarningSpace)
                {
                    string m_strWarningMessage;
                    m_strWarningMessage = string.Format("D 드라이브 남은 용량이 {0:0.00} GB 이하입니다. \r\n과거 얼라인 이미지 또는 불필요한 데이터를 삭제하여 공간을 확보하십시오.", m_dWarningSpace);

                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", m_strWarningMessage);
                }
            }

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 정렬을 시작하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 & 웨이퍼 얼라인 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 정렬을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 & 웨이퍼 얼라인 중지");
            }
        }

        private void btnUpperCamera_Init_Click(object sender, EventArgs e)
        {
            //MC_Func.MC_SetEncPos(3, 0.0);
            //MC_Func.MC_SetCmdPos(3, 0.0);
            //return;

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

        private void baseButtonThinChuckStageCleaning_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_StageCleaning())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "IO 신호, 씬-척 스테이지 클리닝 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_StageCleaning(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "IO 신호, 씬-척 스테이지 클리닝 On");

                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_StageCleaning(true);
            }
        }

        private void baseButtonThinChuckVacuum_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_Vacuum())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "씬-척 공압 On");

                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_Vacuum(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "씬-척 공압 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_Vacuum(true);
            }
        }

        private void baseButtonWaferVacuum_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Wafer_Vacuum())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "웨이퍼 공압 On");

                waferProbeAlign.waferProbeAlignParameter.DO_Wafer_Vacuum(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "웨이퍼 공압 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Wafer_Vacuum(true);
            }
        }

        private void baseButtonTopCoverUp_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Up())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 올림 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Up(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 올림");

                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Up(true);
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Down(false);
            }
        }

        private void baseButtonTopCoverDown_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 트레이가 로딩 위치에 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Down())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 내림 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Down(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 내림");

                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Down(true);
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Up(false);
            }
        }

        private void baseButtonProbePacking_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Packing())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "IO 신호, 프로브 패킹 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "IO 신호, 프로브 패킹 신호 On");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnPacking(false);
            }
        }

        private void baseButtonProbeUnpacking_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Unpacking())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "IO 신호, 프로브 언패킹 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnPacking(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "IO 신호, 프로브 언패킹 신호 On");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnPacking(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(false);
            }
        }

        private void btnUpperCamera_StartLive_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
            }
        }

        private void btnLoadingPos_Click(object sender, EventArgs e)
        {
            //  Wafer Loading 위치로 이동                       

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
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

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "씬-척 & 웨이퍼 로딩 위치 이동 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 로딩 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nWafer_Loading_Ready_Step = (int)WaferProbeAlign.WaferLoading_Ready_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "씬-척 & 웨이퍼 로딩 위치 이동 중지");
            }
        }

        private void btnPacking_Click(object sender, EventArgs e)
        {
            //  Wafer - ProbeCard Packing

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Wafer 유무 확인
            if (waferProbeAlign.Config.ParamConfig.Wafer_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (waferProbeAlign.Config.ParamConfig.ThinChuck_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
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
            //if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
            //    return;
            //}
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

            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None)
            {
                var mb = new MessageBoxYesNo();

                if (waferProbeAlign.m_bWafer_ThetaAlign_OK && waferProbeAlign.m_bWafer_XYAlign_OK)
                {
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard Packing 을 시작하시겠습니까?"))
                        return;
                }
                else
                {
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "###  먼저 Wafer Align 을 진행해야 합니다.  ###\r\n\r\nWafer Align 을 하지 않고 Wafer - ProbeCard Packing 을 시작하시겠습니까?"))
                        return;
                }

                waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 & 웨이퍼 패킹 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard Packing 을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 & 웨이퍼 패킹 중지");
            }
        }

        private void btnProbeCardLoadingReady_Click(object sender, EventArgs e)
        {
            //  Probe-Card Loading 위치로 이동            

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
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

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 투입 위치로 이동 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 로딩 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nProbeCard_Loading_Ready_Step = (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 투입 위치로 이동 중지");
            }
        }

        private void btnProbeCardLocking_Click(object sender, EventArgs e)
        {
            //  Probe-Card Locking            

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
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

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 작업 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 고정 작업을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nProbeCard_Locking_Step = (int)WaferProbeAlign.ProbeCard_Locking_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 작업 중지");
            }
        }

        private void btnWaferProbeCardUnpackingReady_Click(object sender, EventArgs e)
        {
            //  Wafer, Probe-Card Unpacking 대기 위치로 이동

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
            //if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
            //    return;
            //}
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

            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step == (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 대기 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.None;
            }
        }

        private void btnWaferProbeCardUnpacking_Click(object sender, EventArgs e)
        {
            //  Wafer, Probe-Card Unpacking

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
            //if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
            //    return;
            //}
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

            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step == (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 작업을 진행하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 작업을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.None;
            }
        }

        private void btnReticlePos_UpperCam_GO_Click(object sender, EventArgs e)
        {
            //  상부 카메라로 Reticle Glass 를 확인하는 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
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
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동하시겠습니까?\r\n\r\n[Upper Camera]"))
                    return;

                waferProbeAlign.m_nReticleCheck_UpperCam_Step = (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.Start;
                waferProbeAlign.timer_ReticleGlass_Check.Enabled = true;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "[상부 카메라] 레티클 글래스 센터 확인 위치 이동 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동을 중지하시겠습니까?\r\n\r\n[Upper Camera]"))
                    return;

                waferProbeAlign.timer_ReticleGlass_Check.Enabled = false;
                waferProbeAlign.m_nReticleCheck_UpperCam_Step = (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "[상부 카메라] 레티클 글래스 센터 확인 위치 이동 중지");
            }
        }

        private void btnReticlePos_LowerCam_GO_Click(object sender, EventArgs e)
        {
            //  하부 카메라로 Reticle Glass 를 확인하는 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
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

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "[하부 카메라] 레티클 글래스 센터 확인 위치 이동 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동을 중지하시겠습니까?\r\n\r\n[Lower Camera]"))
                    return;

                waferProbeAlign.timer_ReticleGlass_Check.Enabled = false;
                waferProbeAlign.m_nReticleCheck_LowerCam_Step = (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "[하부 카메라] 레티클 글래스 센터 확인 위치 이동 중지");
            }
        }

        private void baseButton_Y_Pos_GO1_Click(object sender, EventArgs e)
        {
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            //if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].X == 0.0) ||
            //    (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].Y == 0.0) )
            //{
            //    var mb = new MessageBoxOk();
            //    mb.ShowDialog("Information !", "얼라인 에러 검사를 진행하지 않았습니다.");
            //    return;
            //}
            //else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 TOP 위치로 보내시겠습니까?"))
                    return;

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                lfTargetPos_X = waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].X;
                lfTargetPos_Y = waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].Y;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

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
                    if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].X == 0.0) ||
                        (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top].Y == 0.0))
                    {
                        MessageBox.Show("얼라인 에러 검사를 진행하지 않아 기본 TOP 위치로 보냅니다.", "Information!!");

                        lfTargetPos_X = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        lfTargetPos_Y = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];
                    }

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
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
        }

        private void baseButton_Y_Pos_GO2_Click(object sender, EventArgs e)
        {
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            //if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].X == 0.0) ||
            //    (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].Y == 0.0))
            //{
            //    var mb = new MessageBoxOk();
            //    mb.ShowDialog("Information !", "얼라인 에러 검사를 진행하지 않았습니다.");
            //    return;
            //}
            //else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 MID 위치로 보내시겠습니까?"))
                    return;

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Middle");

                lfTargetPos_X = waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].X;
                lfTargetPos_Y = waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].Y;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

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
                    if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].X == 0.0) ||
                        (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid].Y == 0.0))
                    {
                        MessageBox.Show("얼라인 에러 검사를 진행하지 않아 기본 MID 위치로 보냅니다.", "Information!!");

                        lfTargetPos_X = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        lfTargetPos_Y = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];
                    }

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
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
        }

        private void baseButton_Y_Pos_GO3_Click(object sender, EventArgs e)
        {
            double lfTargetPos_X = 0.0;
            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            //if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].X == 0.0) ||
            //    (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].Y == 0.0))
            //{
            //    var mb = new MessageBoxOk();
            //    mb.ShowDialog("Information !", "얼라인 에러 검사를 진행하지 않았습니다.");
            //    return;
            //}
            //else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 BOT 위치로 보내시겠습니까?"))
                    return;

                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");

                lfTargetPos_X = waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].X;
                lfTargetPos_Y = waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].Y;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

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
                    if ((waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].X == 0.0) ||
                        (waferProbeAlign.AlignmentErrorCheck_Position[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot].Y == 0.0))
                    {
                        MessageBox.Show("얼라인 에러 검사를 진행하지 않아 기본 BOT 위치로 보냅니다.", "Information!!");

                        lfTargetPos_X = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        lfTargetPos_Y = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];
                    }

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);
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
        }

        private void baseButton_Elev_PitchMove_Up_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            double dVelocity = 50.0;
            double dPitch = 0.1;
            double dDirection = 1.0;

            MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, dPitch * dDirection, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
        }

        private void baseButton_Elev_PitchMove_Down_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            double dVelocity = 50.0;
            double dPitch = 0.1;
            double dDirection = -1.0;
            
            MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, dPitch * dDirection, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
        }

        private void baseButton_Elev_JogMove_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            double dVelocity = 5.0;
            double dDirection = 1.0;

            MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, dVelocity * dDirection, dVelocity * 5.0, dVelocity * 5.0);
        }

        private void baseButton_Elev_JogMove_Up_MouseUp(object sender, MouseEventArgs e)
        {
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

            double dVelocity = 1.0;
            double dDirection = 1.0;

            MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);
        }

        private void baseButton_Elev_JogMove_Down_MouseDown(object sender, MouseEventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            double dVelocity = 5.0;
            double dDirection = -1.0;

            MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, dVelocity * dDirection, dVelocity * 5.0, dVelocity * 5.0);
        }

        private void buttonAxisXDownYUp_Click(object sender, EventArgs e)
        {
            //  Left - Top

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = -1;
                nDirection_Y = -1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisYUp_Click(object sender, EventArgs e)
        {
            //  Top

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                //double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                //int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                //nDirection_X = -1;
                nDirection_Y = -1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                //MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXUpYUp_Click(object sender, EventArgs e)
        {
            //  Right - Top

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = 1;
                nDirection_Y = -1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXUp_Click(object sender, EventArgs e)
        {
            //  Right

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                //double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                //int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = 1;
                //nDirection_Y = -1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                //MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXUpYDown_Click(object sender, EventArgs e)
        {
            //  Right - Bottom

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = 1;
                nDirection_Y = 1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisYDown_Click(object sender, EventArgs e)
        {
            //  Bottom

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                //double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                //int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                //nDirection_X = -1;
                nDirection_Y = 1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                //MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXDownYDown_Click(object sender, EventArgs e)
        {
            //  Left - Bottom

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = -1;
                nDirection_Y = 1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXDown_Click(object sender, EventArgs e)
        {
            //  Left

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Step.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                //double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                //int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = -1;
                //nDirection_Y = -1;

                //  이동량 
                if (radioButton_Movement_05mm.Checked)
                {
                    m_dStep = 0.05;
                }
                else if (radioButton_Movement_01mm.Checked)
                {
                    m_dStep = 0.01;
                }
                else
                {
                    m_dStep = 0.001;
                }

                //  이동 속도
                dVelocity = 50.0;

                MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, m_dStep * (double)nDirection_X, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
                //MC_Func.MC_MoveRelPosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, m_dStep * (double)nDirection_Y, dVelocity, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXDownYUp_MouseDown(object sender, MouseEventArgs e)
        {
            //  Jog : Left - Top

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = -1;
                nDirection_Y = -1;

                //  이동 속도
                dVelocity = 2.0;

                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisYUp_MouseDown(object sender, MouseEventArgs e)
        {
            //  Jog : Top

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                //double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                //int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                //nDirection_X = 1;
                nDirection_Y = -1;

                //  이동 속도
                dVelocity = 2.0;

                //MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXUpYUp_MouseDown(object sender, MouseEventArgs e)
        {
            //  Jog : Right - Top

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = 1;
                nDirection_Y = -1;

                //  이동 속도
                dVelocity = 2.0;

                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXUp_MouseDown_1(object sender, MouseEventArgs e)
        {
            //  Jog : Right

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                //double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                //int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = 1;
                //nDirection_Y = -1;

                //  이동 속도
                dVelocity = 2.0;

                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                //MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXUpYDown_MouseDown(object sender, MouseEventArgs e)
        {
            //  Jog : Right - Bottom

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = 1;
                nDirection_Y = 1;

                //  이동 속도
                dVelocity = 2.0;

                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisYDown_MouseDown(object sender, MouseEventArgs e)
        {
            //  Jog : Bottom

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                //double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                //int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                //nDirection_X = 1;
                nDirection_Y = 1;

                //  이동 속도
                dVelocity = 2.0;

                //MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXDownYDown_MouseDown(object sender, MouseEventArgs e)
        {
            //  Jog : Left - Bottom

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = -1;
                nDirection_Y = 1;

                //  이동 속도
                dVelocity = 2.0;

                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXDown_MouseDown_1(object sender, MouseEventArgs e)
        {
            //  Jog : Left

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (checkBox_Jog.Checked)
            {
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

                double lfTargetPos_X = 0.0;
                //double lfTargetPos_Y = 0.0;
                double m_dStep = 0;
                int nDirection_X = 1;
                //int nDirection_Y = 1;
                double dVelocity = 0;

                //  이동 방향
                nDirection_X = -1;
                //nDirection_Y = -1;

                //  이동 속도
                dVelocity = 2.0;

                MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.X, dVelocity * (double)nDirection_X, dVelocity * 5.0, dVelocity * 5.0);
                //MC_Func.MC_JogMove((int)WaferProbeAlignParameter.AxisAjinEnum.Y, dVelocity * (double)nDirection_Y, dVelocity * 5.0, dVelocity * 5.0);
            }
        }

        private void buttonAxisXDownYUp_MouseUp(object sender, MouseEventArgs e)
        {
            //  //  Jog : Stop

            if (checkBox_Jog.Checked)
            {

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

                double dVelocity = 5.0;
                double dDirection = 1.0;

                MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                MC_Func.MC_JogStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y);
            }
        }

        private void checkBox_Jog_Click(object sender, EventArgs e)
        {
            //  Jog Mode

            if (checkBox_Jog.Checked == false)
            {
                checkBox_Jog.Checked = true;
                checkBox_Reticle_Jog.Checked = true;
            }

            checkBox_Step.Checked = false;
            checkBox_Reticle_Step.Checked = false;

            radioButton_Movement_05mm.Enabled = false;
            radioButton_Movement_01mm.Enabled = false;
            radioButton_Movement_001mm.Enabled = false;
            radioButton_Movement_05mm.Checked = false;
            radioButton_Movement_01mm.Checked = false;
            radioButton_Movement_001mm.Checked = false;

            radioButton_Reticle_Movement_05mm.Enabled = false;
            radioButton_Reticle_Movement_01mm.Enabled = false;
            radioButton_Reticle_Movement_001mm.Enabled = false;
            radioButton_Reticle_Movement_05mm.Checked = false;
            radioButton_Reticle_Movement_01mm.Checked = false;
            radioButton_Reticle_Movement_001mm.Checked = false;
        }

        private void checkBox_Step_Click(object sender, EventArgs e)
        {
            //  Step Mode

            if (checkBox_Step.Checked == false)
            {
                checkBox_Step.Checked = true;
                checkBox_Reticle_Step.Checked = true;
            }

            checkBox_Jog.Checked = false;
            checkBox_Reticle_Jog.Checked = false;

            radioButton_Movement_05mm.Enabled = true;
            radioButton_Movement_01mm.Enabled = true;
            radioButton_Movement_001mm.Enabled = true;
            radioButton_Movement_05mm.Checked = false;
            radioButton_Movement_01mm.Checked = true;
            radioButton_Movement_001mm.Checked = false;

            radioButton_Reticle_Movement_05mm.Enabled = true;
            radioButton_Reticle_Movement_01mm.Enabled = true;
            radioButton_Reticle_Movement_001mm.Enabled = true;
            radioButton_Reticle_Movement_05mm.Checked = false;
            radioButton_Reticle_Movement_01mm.Checked = true;
            radioButton_Reticle_Movement_001mm.Checked = false;
        }

        private void baseButton_VisionXY_Pos_Get1_Click(object sender, EventArgs e)
        {
            //  현재 컨택 위치 가져오기

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            //  버튼 컬러 변경
            baseButton_VisionXY_Pos_Get1.BackColor = Color.LightGreen;
            baseButton_VisionXY_Pos_Get2.BackColor = Color.LightGray;

            m_dCurPackingPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
            m_dCurPackingPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

            var mb = new MessageBoxOk();
            mb.ShowDialog("Information !", "현재 프로브 핀 컨택 위치 확인.");
            return;
        }

        private void baseButton_VisionXY_Pos_Get2_Click(object sender, EventArgs e)
        {
            //  이동하려는 컨택 위치 가져오기

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            if ((m_dCurPackingPos_X == 0.0) || (m_dCurPackingPos_Y == 0.0))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "현재 프로브 핀 컨택 위치를 설정하지 않았습니다.\r\n\r\n[ 1 단계 ] 진행");
                return;
            }

            m_dTargetPackingPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
            m_dTargetPackingPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

            //  컨택 오차 확인
            double m_dDiff_X = m_dTargetPackingPos_X - m_dCurPackingPos_X;
            double m_dDiff_Y = m_dTargetPackingPos_Y - m_dCurPackingPos_Y;

            if ((m_dDiff_X >= 2.0) || (m_dDiff_Y >= 2.0))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "변경하려는 패킹 오프셋 값이 너무 큽니다. (2mm 이상)\r\n\r\n[ 1 단계 ] 부터 재 진행");
                return;
            }

            //  버튼 컬러 변경
            baseButton_VisionXY_Pos_Get2.BackColor = Color.LightGreen;

            //  계산된 오차값을 설정값에 반영
            double m_dBeforeDiff_X = 0.0;
            double m_dBeforeDiff_Y = 0.0;
            double m_dAfterDiff_X = 0.0;
            double m_dAfterDiff_Y = 0.0;

            string m_strTemp;

            m_dBeforeDiff_X = waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X;
            m_dBeforeDiff_Y = waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y;
            m_dAfterDiff_X = m_dBeforeDiff_X + (m_dDiff_X * -1.0);
            m_dAfterDiff_Y = m_dBeforeDiff_Y + m_dDiff_Y;

            m_strTemp = "패킹 오프셋을 변경하시겠습니까?\r\n\r\n" + "[ 기존 -  X: " + m_dBeforeDiff_X.ToString() + ",  Y: " + m_dBeforeDiff_Y.ToString() + " ]\r\n" +
                                                           "[ 변경 -  X: " + m_dAfterDiff_X.ToString() + ",  Y: " + m_dAfterDiff_Y.ToString() + " ]";

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", m_strTemp))
                return;

            waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X = m_dAfterDiff_X;
            waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y = m_dAfterDiff_Y;

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

            m_strTemp = "패킹 오프셋 변경 완료.\r\n\r\n" + "[ 기존 -  X: " + m_dBeforeDiff_X.ToString() + ",  Y: " + m_dBeforeDiff_Y.ToString() + " ]\r\n" +
                                                           "[ 변경 -  X: " + m_dAfterDiff_X.ToString() + ",  Y: " + m_dAfterDiff_Y.ToString() + " ]";

            var mb2 = new MessageBoxOk();
            mb2.ShowDialog("Information !", m_strTemp);

            //  버튼 컬러 변경
            baseButton_VisionXY_Pos_Get1.BackColor = Color.LightGray;
            baseButton_VisionXY_Pos_Get2.BackColor = Color.LightGray;
        }

        private void baseButton_Elev_GoPos_Wafer_ProbeCard_Packing_Click(object sender, EventArgs e)
        {
            //  Elevator Z 축, Packing 위치 아래 30mm 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

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

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");
            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam2 = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("Ready");

            lfTargetPos_EZ = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] - 30.0;

            if (Equipment.AjinBoard_Opened)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Elevator Z축을 Wafer && ProbeCard 패킹 위치 아래 30mm 로 보내시겠습니까?"))
                    return;

                lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Velocity;
                lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.EZ.ToString()].Configuration.Acceleration;

                //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= waferProbeAlign.Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                {
                    MessageBox.Show("카메라가 씬-척 엘리베이터와 충돌 위치에 있습니다.\r\n\r\n카메라를 안전 위치로 이동합니다.\r\n이동이 완료되면 확인 후 다시 시도하십시오.", "Warning!!");

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam2.dTarget[(int)nAxis.Y],
                                        lfVelocity, lfAccDec, lfAccDec) ;
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

        private void btnManualPacking_Click(object sender, EventArgs e)
        {
            //  Wafer - ProbeCard Manual Packing (패킹 위치까지 엘리베이터가 올라가 있는 상태 다음부터 진행)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Wafer 유무 확인
            if (waferProbeAlign.Config.ParamConfig.Wafer_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (waferProbeAlign.Config.ParamConfig.ThinChuck_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
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
            //if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
            //    return;
            //}
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

            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None)
            {
                var mb = new MessageBoxYesNo();

                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 수동 Packing 을 시작하시겠습니까?\r\n\r\n[ 씬-척이 프로브 카드와 접촉한 상태인지 확인하십시오. ]"))
                    return;

                waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Probe_Packing;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 수동 Packing 을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None;
            }
        }

        private void btnPAK_Leak_Check_Click(object sender, EventArgs e)
        {
            //  PAK Air Line Leak Check

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
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
            //if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
            //    return;
            //}

            if (waferProbeAlign.m_nPAK_AirLine_Check_Step == (int)WaferProbeAlign.PAK_AirLine_Check_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "PAK 공압 관로 상태 확인 작업을 진행하시겠습니까?"))
                    return;

                waferProbeAlign.m_nPAK_AirLine_Check_Step = (int)WaferProbeAlign.PAK_AirLine_Check_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "PAK 공압 관로 상태 확인 작업 시작");
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "PAK 공압 관로 상태 확인 작업을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nPAK_AirLine_Check_Step = (int)WaferProbeAlign.PAK_AirLine_Check_Step.None;

                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "PAK 공압 관로 상태 확인 작업 중지");
            }
        }

        private void tabControl_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  레티클 글래스 위치 변경 탭을 클릭하면, 상부 카메라 레티클 위치로 이동하는 버튼 색깔 변경되도록

            if (tabControl_User.SelectedTab == tabPage_ReticlePositionChange)
            {
                btnReticlePos_UpperCam_GO.BackColor = Color.Lime;
                btnReticlePos_UpperCam_GO.ForeColor = Color.Black;
            }
            else
            {
                btnReticlePos_UpperCam_GO.BackColor = Color.LightGray;
                btnReticlePos_UpperCam_GO.ForeColor = Color.DarkRed;
            }
        }

        private void radioButton_Movement_05mm_Click(object sender, EventArgs e)
        {
            radioButton_Movement_05mm.Checked = true;
            radioButton_Movement_01mm.Checked = false;
            radioButton_Movement_001mm.Checked = false;

            radioButton_Reticle_Movement_05mm.Checked = true;
            radioButton_Reticle_Movement_01mm.Checked = false;
            radioButton_Reticle_Movement_001mm.Checked = false;
        }

        private void radioButton_Movement_01mm_Click(object sender, EventArgs e)
        {
            radioButton_Movement_05mm.Checked = false;
            radioButton_Movement_01mm.Checked = true;
            radioButton_Movement_001mm.Checked = false;

            radioButton_Reticle_Movement_05mm.Checked = false;
            radioButton_Reticle_Movement_01mm.Checked = true;
            radioButton_Reticle_Movement_001mm.Checked = false;
        }

        private void radioButton_Movement_001mm_Click(object sender, EventArgs e)
        {
            radioButton_Movement_05mm.Checked = false;
            radioButton_Movement_01mm.Checked = false;
            radioButton_Movement_001mm.Checked = true;

            radioButton_Reticle_Movement_05mm.Checked = false;
            radioButton_Reticle_Movement_01mm.Checked = false;
            radioButton_Reticle_Movement_001mm.Checked = true;
        }

        private void baseButton_ReticleGlass_Pos_Get_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "프로브 카드 카메라(상부) 의 현재 레티클 글래스 위치를 저장하시겠습니까?\r\n\r\n[ 저장 후에, 하부 카메라 센터 위치확인 및 조정이 필요합니다. ]"))
                return;

            double m_dUVW_U = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
            double m_dUVW_V = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
            double m_dUVW_W = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);

            double m_dVision_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
            double m_dVision_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);
            double m_dVision_Z = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

            //  위치 저장 (Config)
            double m_dOldPos = 0.0;
            double m_dNewPos = 0.0;
            string m_strTemp = "";

            int m_nIndex_UpperCam = -1;
            int m_nIndex_LowerCam = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
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

                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].U = m_dUVW_U;
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].V = m_dUVW_V;
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].W = m_dUVW_W;

                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].X = m_dVision_X;
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].Y = m_dVision_Y;
                waferProbeAlign.Config.Positions[m_nIndex_UpperCam].VZ = m_dVision_Z;


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
                mb1.ShowDialog("Information !", "Config 창에 \"ReticleGlass_UpperCamera\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }


            //  Lower Cam 좌표 변경
            if (m_nIndex_LowerCam != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].U = m_dUVW_U;
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].V = m_dUVW_V;
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].W = m_dUVW_W;

                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].X = m_dVision_X;
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].Y = m_dVision_Y;
                waferProbeAlign.Config.Positions[m_nIndex_LowerCam].VZ = m_dVision_Z;


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


            waferProbeAlign.Machine_Parameter_Save();
        }

        private void baseLabelPosition_Top_Click(object sender, EventArgs e)
        {
            return;

            //  테스트 (TOP 이미지 저장)

            Equipment.AlignStart_Time = DateTime.Now.ToString("hh_mm_ss");
            waferProbeAlign.ResultImage_Save(Equipment.User_Name, Equipment.AlignStart_Time, "TOP");
        }

        private void baseLabelPosition_Mid_Click(object sender, EventArgs e)
        {
            return;

            //  테스트 (MID 이미지 저장)

            Equipment.AlignStart_Time = DateTime.Now.ToString("hh_mm_ss");
            waferProbeAlign.ResultImage_Save(Equipment.User_Name, Equipment.AlignStart_Time, "MID");
        }

        private void baseLabelPosition_Bot_Click(object sender, EventArgs e)
        {
            return;

            //  테스트 (BOT 이미지 저장)

            Equipment.AlignStart_Time = DateTime.Now.ToString("hh_mm_ss");
            waferProbeAlign.ResultImage_Save(Equipment.User_Name, Equipment.AlignStart_Time, "BOT");
        }

        private void baseButtonProbeClamp_Down_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 올림");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_Down(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 내림");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_Down(true);
            }
        }

        private void baseButtonProbeClamp_FW_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 클램프가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down() || !waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 언패킹 실린더가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_FW())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 닫기 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_FW(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 닫기");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_FW(true);
                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_BW(false);
            }
        }

        private void baseButtonProbeClamp_BW_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 클램프가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down() || !waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 언패킹 실린더가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_BW())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 열기 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_BW(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 열기");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_BW(true);
                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_FW(false);
            }
        }

        private void baseButtonProbeUnpackingCyl_Up_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Up())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 올림 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 올림");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);
            }
        }

        private void baseButtonProbeUnpackingCyl_Down_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Down())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 내림 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 내림");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(false);
            }
        }
    }
}
