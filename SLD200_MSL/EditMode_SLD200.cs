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
using System.Windows;
using System.Numerics;
using QMC.Core;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using ACS.SPiiPlusNET;
using System.Security.Permissions;
using QMC.Common.Motion.ACS.Motions;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Equipment;
using OpenCvSharp;
using System.Xml;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.AxHost;
//using netDxf.Entities;

//  Sirius1
using SpiralLab.Sirius;

//  Sirius2
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.PowerMap;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms ;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;
//using SpiralLab.Sirius2.Mathematics;

//using Vector3 = System.Numerics.Vector3;


namespace SLD200_MSL
{
    public partial class EditMode_SLD200 : UserControl
    {
        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }

        static WorkStage workStage;

        MotionFunction MC_Func = new MotionFunction();

        public System.Windows.Forms.Timer timer_ScannerMode_Change;

        //  분할 영역 확인용 구조체
        public struct stGroupDataForDivide
        {
            public PointD dCenter;                      //  가공 객체 Center 좌표
            public int nDivideIndex;                    //  가공 객체의 분할 영역 Index
            public bool bAssigned;                      //  가공 객체의 영역 할당 여부
        }
        public stGroupDataForDivide[] m_stGroupDataForDivide;      //  Layer 에 Group 2개 이상 사용하기 위해서...
        public int m_nGroupCount;                                   //  Group 개수 Count

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
        #endregion

        static int marginWidth = 10;
        static int marginHeight = 10;
        static int btnWidth = 50;
        static int btnHeight = 50;

        //  Test용 변수
        bool m_bRun;

        IRtc Rtc;


        public EditMode_SLD200()
        {
            InitializeComponent();
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
            }

            //  Scanner Mode 를 변경하는 타이머
            timer_ScannerMode_Change = new System.Windows.Forms.Timer();
            timer_ScannerMode_Change.Interval = 50;
            timer_ScannerMode_Change.Tick += new System.EventHandler(Timer_ScannerModeChange);
            timer_ScannerMode_Change.Enabled = true;

            m_nGroupCount = 0;
        }

        void Timer_ScannerModeChange(object sender, EventArgs e)
        {
            //if (Equipment.ScannerMode_Change_byUser == (int)RtcMode.RTC_RTC6)
            //{
            //    Equipment.ScannerMode_Change_byUser = (int)RtcMode.RTC_NONE;

            //    if (btnTest_RTC6.BackColor == Color.GreenYellow)
            //        return;

            //    if (btnTest_syncAxis.BackColor == Color.GreenYellow)
            //    {
            //        var mb = new MessageBoxYesNo();
            //        if (DialogResult.Yes != mb.ShowDialog("Question ?", "RTC6 모드로 변경하시겠습니까?"))
            //            return;
            //    }

            //    btnTest_RTC6.BackColor = Color.GreenYellow;
            //    btnTest_syncAxis.BackColor = Color.Gray;
            //    //btnTest_syncAxis.Enabled = false;

            //    Equipment.RtcMode_syncAxis = (int)Equipment.RtcMode.RTC_RTC6;

            //    Log.Write("SLD100", "ScannerModeChange", "RTC6 모드로 변경");

            //    RtcOpenMode_syncAxis(false);
            //}

            //if (Equipment.ScannerMode_Change_byUser == (int)RtcMode.RTC_SYNCAXIS)
            //{
            //    Equipment.ScannerMode_Change_byUser = (int)RtcMode.RTC_NONE;

            //    if (btnTest_syncAxis.BackColor == Color.GreenYellow)
            //        return;

            //    if (btnTest_RTC6.BackColor == Color.GreenYellow)
            //    {
            //        var mb = new MessageBoxYesNo();
            //        if (DialogResult.Yes != mb.ShowDialog("Question ?", "syncAxis 모드로 변경하시겠습니까?"))
            //            return;
            //    }                

            //    btnTest_syncAxis.BackColor = Color.GreenYellow;
            //    btnTest_RTC6.BackColor = Color.Gray;
            //    //btnTest_RTC6.Enabled = false;

            //    Equipment.RtcMode_syncAxis = (int)Equipment.RtcMode.RTC_SYNCAXIS;

            //    Log.Write("SLD100", "ScannerModeChange", "syncAxis 모드로 변경");

            //    RtcOpenMode_syncAxis(true);
            //}
        }

        private void SiriusEditorForm_OnPowerMapSourceChanged(object sender, IPowerMap powerMap)
        {
            //SiriusEditor.PowerMap = powerMap;
        }

        private void SiriusEditorForm_OnDocumentSourceChanged(object sender, IDocument doc)
        {
            //SiriusEditor.Document = doc;
        }


        public void RtcOpenMode_syncAxis( bool m_bTrue )
        {
            //if ( m_bTrue )                      //  syncAxis 모드
            //{
            //    SpiralLab.Core.Initialize();

            //    this.SiriusEditor.EnablePens = true;

            //    // create document
            //    // 신규 문서 생성
            //    var doc = new DocumentDefault();
            //    // assign document into editor
            //    // 문서 지정
            //    SiriusEditor.Document = doc;

            //    // EnablePens option is true  (default)
            //    // enabled pens 옵션은 기본적으로 true 임
            //    if (!SiriusEditor.EnablePens)
            //    {
            //        // 기본 펜 생성후 문서에 추가
            //        var pen = new PenDefault();
            //        doc.Action.ActEntityAdd(pen);
            //    }

            //    // assign document source changed event handler
            //    // 내부 데이타(IDocument) 가 변경될경우 이를 이벤트 통지를 받는 핸들러 등록
            //    SiriusEditor.OnDocumentSourceChanged += SiriusEditorForm_OnDocumentSourceChanged;
            //    //  SiriusEditor.OnPowerMapSourceChanged += SiriusEditorForm_OnPowerMapSourceChanged;           //  2022. 08. 24.  SCH : 이거 없어짐.

            //    //var xmlConfigFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncAxis", "syncAXISConfig.xml");
            //    //var xmlConfigFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncAxis", "syncAXISConfig2.xml");
            //    //string xmlConfigFileName = "D:\\SLD-100_Parameter\\syncAXISConfig.SLD100.xml";
            //    string xmlConfigFileName = Equipment.XmlFile_forSyncAxis;

            //    if (!File.Exists(xmlConfigFileName))
            //    {
            //        MessageBox.Show($"XML configuration file is not founded : {xmlConfigFileName}");
            //        return;
            //    }

            //    if (workStage.rtcSyncAxis != null)
            //    {
            //        workStage.rtcSyncAxis.Dispose();
            //    }

            //    #region SyncAxis 초기화
            //    bool success = true;
            //    //var rtc = new Rtc6SyncAxis();
            //    workStage.rtcSyncAxis = new Rtc6SyncAxis();
            //    workStage.rtcSyncAxis.Name = "SyncAxis";
            //    success &= workStage.rtcSyncAxis.Initialize(xmlConfigFileName);
            //    success &= workStage.rtcSyncAxis.CtlFrequency(50 * 1000, (float)0.5); // laser frequency : 50KHz, pulse width : 2usec
            //    success &= workStage.rtcSyncAxis.CtlSpeed(100, 100); // default scanner jump and mark speed : 100mm/s

            //    //스테이지 이동시 기본 값 설정
            //    workStage.rtcSyncAxis.StageMoveSpeed = 10;

            //    workStage.rtcSyncAxis.StageMoveTimeOut = 5;

            //    //workStage.rtcSyncAxis.BufferMax = 65535;
            //    workStage.rtcSyncAxis.BufferMaxSize = 65535;            //  이건 RTC 모드에서만 사용된다. syncAxis 모드에서는 사용되지 않지만, 그냥 해두자...

            //    #endregion

            //    #region 레이저 소스 초기화
            //    // virtual laser source with max 20W power (최대 출력 20W 의 가상 레이저 소스 생성)
            //    workStage.laser = new LaserVirtual(0, "virtual", 20);
            //    //var laser = new IPGYLPTypeD(0, "IPG YLP D", 1, 20);
            //    //var laser = new IPGYLPTypeE(0, "IPG YLP E", 1, 20);
            //    //var laser = new IPGYLPN(0, "IPG YLP N", 1, 100);
            //    //var laser = new JPTTypeE(0, "JPT Type E", 1, 20);
            //    //var laser = new SPIG4(0, "SPI G3/4", 1, 20);
            //    //var laser = new PhotonicsIndustryDX(0, "DX", 1, 20);
            //    //var laser = new PhotonicsIndustryRGHAIO(0, "RGHAIO", 1, 20);
            //    //var laser = new AdvancedOptoWaveFotia(0, "Fotia", 1, 20);
            //    //var laser = new AdvancedOptoWaveAOPico(0, "AOPico", 1, 20);
            //    //var laser = new CoherentAviaLX(0, "Avia LX", 1, 20);
            //    //var laser = new CoherentDiamondJSeries(0, "Diamond JSeries", "10.0.0.1", 200.0f);
            //    //var laser = new CoherentDiamondCSeries(0, "Diamond CSeries", 1, 100.0f);
            //    //var laser = new SpectraPhysicsHippo(0, "Hippo", 1, 30);
            //    //var laser = new SpectraPhysicsTalon(0, "Talon", 1, 30);

            //    // assign RTC instance at laser 
            //    workStage.laser.Rtc = workStage.rtcSyncAxis;
            //    // initialize laser source
            //    workStage.laser.Initialize();
            //    // set basic power output to 2W
            //    workStage.laser.CtlPower(2);
            //    #endregion

            //    #region 마커 지정
            //    workStage.marker = new MarkerDefault(0, " SyncAxis Marker ");
            //    #endregion

            //    #region RTC 확장 IO 
            //    ////this.RtcExt1DInput = new RtcDInput(rtc, 0, "DIN RTC EXT1");
            //    ////this.RtcExt1DInput.Initialize();
            //    //this.RtcExt1DOutput = new RtcDOutputExt1(rtc, 0, "DOUT RTC EXT1");
            //    //this.RtcExt1DOutput.Initialize();
            //    ////this.RtcExt2DOutput = new RtcDOutputExt2(rtc, 0, "DIN RTC EXT2");
            //    ////this.RtcExt2DOutput.Initialize();
            //    ////this.siriusEditorForm1.RtcExtension1Input = this.RtcExt1DInput;
            //    //this.siriusEditorForm1.RtcExtension1Output = this.RtcExt1DOutput;
            //    ////this.siriusEditorForm1.RtcExtension2Output = this.RtcExt2DOutput;
            //    #endregion

            //    //this.Rtc = rtc;
            //    //this.Laser = laser;
            //    //this.siriusEditorForm1.Rtc = rtc;
            //    //this.siriusEditorForm1.Laser = laser;
            //    //this.siriusEditorForm1.Marker = marker;

            //    #region RTC 초기화
            //    ////create Rtc for dummy (가상 RTC 카드)
            //    ////var rtc = new RtcVirtual(0);
            //    //workStage.rtc = new RtcVirtual(0);

            //    ////create Rtc6 controller
            //    ////var rtc = new Rtc6(0); 
            //    //workStage.rtcSyncAxis = new Rtc6SyncAxis();

            //    //// theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
            //    //float fov = 60.0f;
            //    //// k factor (bits/mm) = 2^20 / fov
            //    //float kfactor = (float)Math.Pow(2, 20) / fov;
            //    //// full path of correction file
            //    ////var correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
            //    //workStage.correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
            //    //// initialize rtc controller
            //    //workStage.rtc.Initialize(kfactor, LaserMode.Yag1, workStage.correctionFile);
            //    //// basic frequency and pulse width
            //    //// laser frequency : 50KHz, pulse width : 2usec (주파수 50KHz, 펄스폭 2usec)
            //    //workStage.rtc.CtlFrequency(50 * 1000, 2);
            //    //// basic sped
            //    //// jump and mark speed : 500mm/s (점프, 마크 속도 500mm/s)
            //    //workStage.rtc.CtlSpeed(500, 500);
            //    //// basic delays
            //    //// scanner and laser delays (스캐너/레이저 지연값 설정)
            //    //workStage.rtc.CtlDelay(10, 100, 200, 200, 0);
            //    #endregion
            //    this.SiriusEditor.Rtc = workStage.rtcSyncAxis;

            //    #region 레이저 소스 초기화
            //    //// virtual laser source with max 20W power (최대 출력 20W 의 가상 레이저 소스 생성)
            //    //workStage.laser = new LaserVirtual(0, "virtual", 20);
            //    ////  laser = new SpectraPhysicsTalon(0, "Talon", 1, 20);

            //    //// assign RTC instance at laser 
            //    ////workStage.laser.Rtc = workStage.rtc;
            //    //workStage.laser.Rtc = workStage.rtcSyncAxis;
            //    //// initialize laser source
            //    //workStage.laser.Initialize();
            //    //// 2W output 
            //    //workStage.laser.CtlPower(2);
            //    #endregion
            //    this.SiriusEditor.Laser = workStage.laser;

            //    #region 마커 지정
            //    // create default marker 
            //    //workStage.marker = new MarkerDefault(0);
            //    #endregion
            //    this.SiriusEditor.Marker = workStage.marker;

            //    #region RTC extension IO 
            //    ////  Create RTC IO 
            //    //workStage.rtcExt1DInput = new RtcDInputExt1(workStage.rtc, 0, "DIN RTC EXT1");
            //    //workStage.rtcExt1DInput.Initialize();
            //    //workStage.rtcExt1DOutput = new RtcDOutputExt1(workStage.rtc, 0, "DOUT RTC EXT1");
            //    //workStage.rtcExt1DOutput.Initialize();
            //    //workStage.rtcExt2DOutput = new RtcDOutputExt2(workStage.rtc, 0, "DIN RTC EXT2");
            //    //workStage.rtcExt2DOutput.Initialize();

            //    ////  RTC 5,6 only
            //    //workStage.rtcPin2DInput = new RtcDInput2Pin(workStage.rtc, 0, "DIN RTC PIN2");
            //    //workStage.rtcPin2DInput.Initialize();
            //    //workStage.rtcPin2DOutput = new RtcDOutput2Pin(workStage.rtc, 0, "DOUT RTC PIN2");
            //    //workStage.rtcPin2DOutput.Initialize();

            //    //this.SiriusEditor.RtcExtension1Input = workStage.rtcExt1DInput;
            //    //this.SiriusEditor.RtcExtension1Output = workStage.rtcExt1DOutput;
            //    //this.SiriusEditor.RtcExtension2Output = workStage.rtcExt2DOutput;
            //    //this.SiriusEditor.RtcPin2Input = workStage.rtcPin2DInput;
            //    //this.SiriusEditor.RtcPin2Output = workStage.rtcPin2DOutput;
            //    #endregion

            //    #region XYZ 모터
            //    //workStage.motorX = new MotorVirtual(2, "X");
            //    //workStage.motorX.Initialize();
            //    //workStage.motorY = new MotorVirtual(0, "Y");
            //    //workStage.motorY.Initialize();
            //    //workStage.motorZ = new MotorVirtual(3, "Z");
            //    //workStage.motorZ.Initialize();
            //    ////var motorR = new MotorVirtual(2, "R");
            //    ////motorR.Initialize();

            //    //workStage.motorArray = new IMotor[] {
            //    //                            workStage.motorX,
            //    //                            workStage.motorY,
            //    //                            workStage.motorZ,
            //    //                            //motorR,
            //    //                        };

            //    //workStage.motors = new MotorsDefault(0, "Group", workStage.motorArray);
            //    //this.SiriusEditor.Motors = workStage.motors;
            //    ////this.siriusEditorForm1.MotorZ = motorZ;

            //    var motorZ = new MotorVirtual(0, "Scanner Z");
            //    motorZ.Initialize();
            //    this.SiriusEditor.MotorZ = motorZ;

            //    #endregion

            //    #region PowerMeter
            //    //// 파워메터
            //    //workStage.pm = new PowerMeterVirtual(0, "Virtual", workStage.laser.MaxPowerWatt);
            //    ////var powerMeter = new PowerMeterCoherentPowerMax(0, "CoherentPM", 1);

            //    //workStage.pm.Initialize();
            //    ////powerMeter.Initialize();
            //    //this.SiriusEditor.PowerMeter = workStage.pm;
            //    #endregion

            //    #region Powermap
            //    //workStage.powerMap = new PowerMapDefault(0, "Virtual", "Watt");
            //    ////var powerMapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "powermap", "test.pmap");
            //    ////var pmap = PowerMapSerializer.Open(powerMapFile);                 //  2022. 08. 24.  SCH : 이거 없어짐.
            //    //this.SiriusEditor.PowerMap = workStage.powerMap;
            //    #endregion

            //    //set compensated power output to 4.5W 
            //    //workStage.laser.PowerMap = workStage.powerMap;
            //    //workStage.laser.CtlPower(2, "Default");

            //    //  Arc 를 Polyline 으로 만들 경우
            //    if (workStage.Config.ParamConfig.ConvertArcToPolyline)
            //    {
            //        Config.LwPolylineBulgeToLines = true;

            //        if (workStage.Config.ParamConfig.ArcToPolyline_Resolution < 1)
            //            Config.LwPolylineBulgePrecision = 10;
            //        else
            //            Config.LwPolylineBulgePrecision = workStage.Config.ParamConfig.ArcToPolyline_Resolution;
            //    }
            //    else        //  Arc 를 Bulge 값을 이용해서 Arc 처럼 만들 경우
            //    {
            //        Config.LwPolylineBulgeToLines = false;
            //    }

            //    //  Spot Distance Control 설정 체크
            //    workStage.m_bSpotDistanceControl_On = workStage.GetSpotDistanceControl_Status(Equipment.XmlFile_forSyncAxis);

            //    //  Spot Distance Value 체크
            //    workStage.m_dSpot_Distance = workStage.GetSpotDistance(Equipment.XmlFile_forSyncAxis);

            //    SiriusEditor.Enabled = true;
            //}
            //else                                //  RTC6 모드
            {
                SpiralLab.Core.Initialize();                            //  Sirius1
                //SpiralLab.Sirius2.Core.Initialize();                  //  Sirius2

                //this.SiriusEditor.EnablePens = true;

                // create document
                // 신규 문서 생성
                var doc = new DocumentDefault();                        //  Sirius1
                //var doc = new DocumentBase();                         //  Sirius2
                // assign document into editor

                // 문서 지정
                //this.SiriusEditor.Document = doc;

                // assign document source changed event handler
                // 내부 데이타(IDocument) 가 변경될경우 이를 이벤트 통지를 받는 핸들러 등록
                //this.SiriusEditor.OnDocumentSourceChanged += SiriusEditorForm_OnDocumentSourceChanged;

                #region RTC 초기화
                //create Rtc for dummy (가상 RTC 카드)
                //var rtc = new RtcVirtual(0); 
                //create Rtc5 controller
                //var rtc = new Rtc5(0);
                //create Rtc6 controller
                workStage.rtc6 = new Rtc6(0);
                //Rtc6 Ethernet
                //var rtc = new Rtc6Ethernet(0, "192.168.0.100", "255.255.255.0"); 

                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                float fov = 15.0f;
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov;
                //float kfactor = (float)workStage.Config.ParamConfig.Scanner_KFactor;
                if (kfactor == 0)
                    kfactor = (float)18830.1889;
                //kfactor = (float)18830.1889;
                // full path of correction file
                //var correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
                string correctionFile = "D:\\SLD-200_Parameter\\Cor_1to1.ct5";

                // initialize rtc controller
                workStage.rtc6.Initialize(kfactor, LaserMode.Yag1, correctionFile);                                                                         //  Sirius1
                //var rtc = ScannerFactory.CreateRtc6(0, kfactor, LaserModes.Yag1, RtcSignalLevels.ActiveHigh, RtcSignalLevels.ActiveHigh, correctionFile);     //  Sirius2
                // basic frequency and pulse width
                // laser frequency : 50KHz, pulse width : 2usec (주파수 50KHz, 펄스폭 2usec)
                workStage.rtc6.CtlFrequency(50 * 1000, 2);
                // basic sped
                // jump and mark speed : 500mm/s (점프, 마크 속도 500mm/s)
                workStage.rtc6.CtlSpeed(500, 500);
                // basic delays
                // scanner and laser delays (스캐너/레이저 지연값 설정)
                workStage.rtc6.CtlDelay(10, 100, 200, 200, 0);
                #endregion

                //this.SiriusEditor.Rtc = workStage.rtc6;

                #region 레이저 소스 초기화
                // virtual laser source with max 20W power (최대 출력 20W 의 가상 레이저 소스 생성)
                //var laser = new LaserVirtual(0, "virtual", 20);
                workStage.laser = new LaserVirtual(0, "virtual", 20);
                //var laser = new IPGYLPTypeD(0, "IPG YLP D", 1, 20);
                //var laser = new IPGYLPTypeE(0, "IPG YLP E", 1, 20);
                //var laser = new IPGYLPN(0, "IPG YLP N", 1, 100);
                //var laser = new JPTTypeE(0, "JPT Type E", 1, 20);
                //var laser = new SPIG4(0, "SPI G3/4", 1, 20);
                //var laser = new PhotonicsIndustryDX(0, "DX", 1, 20);
                //var laser = new PhotonicsIndustryRGHAIO(0, "RGHAIO", 1, 20);
                //var laser = new AdvancedOptoWaveFotia(0, "Fotia", 1, 20);
                //var laser = new AdvancedOptoWaveAOPico(0, "AOPico", 1, 20);
                //var laser = new CoherentAviaLX(0, "Avia LX", 1, 20);
                //var laser = new CoherentDiamondJSeries(0, "Diamond JSeries", "10.0.0.1", 200.0f);
                //var laser = new CoherentDiamondCSeries(0, "Diamond CSeries", 1, 100.0f);
                //var laser = new SpectraPhysicsHippo(0, "Hippo", 1, 30);
                //var laser = new SpectraPhysicsTalon(0, "Talon", 1, 20);

                // assign RTC instance at laser 
                workStage.laser.Rtc = workStage.rtc6;                   //  Sirius1
                //workStage.laser.Scanner = workStage.rtc6;               //  Sirius2

                // initialize laser source
                workStage.laser.Initialize();

                // set basic power output to 2W
                workStage.laser.CtlPower(2);
                #endregion

                //this.SiriusEditor.Laser = workStage.laser;                  //  Sirius1

                #region 마커 지정
                // create default marker 
                //var marker = new MarkerDefault(0);

                workStage.marker = new MarkerDefault(0, " RTC6 Marker ");           //  Sirius1
                //workStage.marker = new MarkerRtc(0, " RTC6 Marker ");             //  Sirius2

                //workStage.marker.Laser.Scanner.ScannerRotateAngle = 90.0;                     //  2022. 10. 12.  SCH : Scanner 가공 Field 를 CCW 방향으로 90도 회전
                                                                                                    //  (SLD-100 은 Scanner 와 Stage 방향이 일치하지 않음. Scanner 가 CW 방향으로 90도 돌아가 있음)
                // If scanner rotate at 90 deg
                //rtc.MatrixStack.BaseMatrix = Matrix4x4.CreateRotationZ((float)(90 * Math.PI / 180.0));                //  Sirius2

                #endregion

                //this.SiriusEditor.Marker = workStage.marker;                        //  Sirius1


                #region RTC extension IO 
                //// create RTC io 
                //var rtcExt1DInput = new RtcDInputExt1(rtc, 0, "DIN RTC EXT1");
                //rtcExt1DInput.Initialize();
                //var rtcExt1DOutput = new RtcDOutputExt1(rtc, 0, "DOUT RTC EXT1");
                //rtcExt1DOutput.Initialize();
                //var rtcExt2DOutput = new RtcDOutputExt2(rtc, 0, "DIN RTC EXT2");
                //rtcExt2DOutput.Initialize();

                ////rtc 5,6 only
                //var rtcPin2DInput = new RtcDInput2Pin(rtc, 0, "DIN RTC PIN2");
                //rtcPin2DInput.Initialize();
                //var rtcPin2DOutput = new RtcDOutput2Pin(rtc, 0, "DOUT RTC PIN2");
                //rtcPin2DOutput.Initialize();

                //this.siriusEditorForm1.RtcExtension1Input = rtcExt1DInput;
                //this.siriusEditorForm1.RtcExtension1Output = rtcExt1DOutput;
                //this.siriusEditorForm1.RtcExtension2Output = rtcExt2DOutput;
                //this.siriusEditorForm1.RtcPin2Input = rtcPin2DInput;
                //this.siriusEditorForm1.RtcPin2Output = rtcPin2DOutput;
                #endregion

                #region XYZ 모터
                //var motorX = new MotorVirtual(0, "X");
                //motorX.Initialize();
                //var motorY = new MotorVirtual(1, "Y");
                //motorY.Initialize();
                //var motorZ = new MotorVirtual(2, "Z");
                //motorZ.Initialize();
                //var motorR = new MotorVirtual(2, "R");
                //motorR.Initialize();

                //var motorArray = new IMotor[]
                //{
                //motorX,
                //motorY,
                //motorZ,
                //motorR,
                //};
                //var motors = new MotorsDefault(0, "Group", motorArray);
                //this.siriusEditorForm1.Motors = motors;

                //var motorZ = new MotorVirtual(0, "Z");
                //this.siriusEditorForm1.MotorZ = motorZ;
                #endregion

                #region PowerMeter
                //// 파워메터
                //var powerMeter = new PowerMeterVirtual(0, "Virtual", laser.MaxPowerWatt);
                ////var powerMeter = new PowerMeterOphir(0, "OphirJuno", "3040875");
                ////var powerMeter = new PowerMeterCoherentPowerMax(0, "CoherentPM", 1);
                ////var powerMeter = new PowerMeterThorLabsPMSeries(0, "PM100USB", "SERIALNO");
                //powerMeter.Initialize();
                //this.siriusEditorForm1.PowerMeter = powerMeter;
                #endregion

                #region Powermap
                //var powerMap = new PowerMapDefault(0, "Virtual", "Watt");
                ////var powerMapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "powermap", "default.map");
                ////PowerMapSerializer.Open(powerMap, powerMapFile);
                //this.siriusEditorForm1.PowerMap = powerMap;
                //laser.PowerMap = powerMap;
                #endregion

                ////  Arc 를 Polyline 으로 만들 경우
                //if (workStage.Config.ParamConfig.ConvertArcToPolyline)
                //{
                //    Config.LwPolylineBulgeToLines = true;

                //    if (workStage.Config.ParamConfig.ArcToPolyline_Resolution < 1)
                //        Config.LwPolylineBulgePrecision = 10;
                //    else
                //        Config.LwPolylineBulgePrecision = workStage.Config.ParamConfig.ArcToPolyline_Resolution;
                //}
                //else        //  Arc 를 Bulge 값을 이용해서 Arc 처럼 만들 경우
                //{
                //    Config.LwPolylineBulgeToLines = false;
                //}

                ////  Spot Distance Control 설정 Off
                //workStage.m_bSpotDistanceControl_On = false;

                ////  Spot Distance Value
                //workStage.m_dSpot_Distance = 0.0;

                //SiriusEditor.Enabled = true;
            }
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


        private bool Get_DI_Status(string strInputName)
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;
                if (dioPoint == null)
                    return false;

                if ((dioPoint.IoType == IoType.Input) && (dioPoint.Name == strInputName))
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
                    if (bSet)
                        dioPoint.Write(DioValue.On);
                    else
                        dioPoint.Write(DioValue.Off);

                    break;
                }
            }

            return bRet;
        }

        private void EditMode_SLD200_Load(object sender, EventArgs e)
        {

        }
  

        private void btnTest_RTC6_Click(object sender, EventArgs e)
        {
            //  RTC6 모드

            if (btnTest_RTC6.BackColor == Color.Lime)
                return;

            btnTest_RTC6.BackColor = Color.Lime;

            Equipment.RtcMode_syncAxis = (int)Equipment.RtcMode.RTC_RTC6 ;

            RtcOpenMode_syncAxis(false);
        }

        private void btnScannerOffset_Set_Click(object sender, EventArgs e)
        {
            Vector3 ScannerOffset = new Vector3(0, 0, 0);

            ScannerOffset.X = (float)Convert.ToDouble(tb_ScannerOffset_X.Text);
            ScannerOffset.Y = (float)Convert.ToDouble(tb_ScannerOffset_Y.Text);
            ScannerOffset.Z = (float)Convert.ToDouble(tb_ScannerOffset_Angle.Text);

            //if ( Equipment.RtcMode_syncAxis != (int)Equipment.RtcMode.RTC_RTC6)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "SyncAxis 모드에서만 동작합니다.");

            //    workStage.rtcSyncAxis.CtlHeadOffset(ScanDevice.ScanDevice1, 0, 0, 0);

            //    return;
            //}


            workStage.rtc6.PrimaryHeadBaseOffset = ScannerOffset;

            //Offset offset = new Offset();
            //offset.Dx = ScannerOffset.X;
            //offset.Dy = ScannerOffset.Y;
            //offset.Dz = 0;
            //offset.AngleZ = ScannerOffset.Z;

            //workStage.rtc6.PrimaryHeadBaseOffset = offset;
        }
        

        //public void Reverse_Entity(bool m_bDirHor)
        //{
        //    int m_nGroupCount = 0;
        //    bool success = true;

        //    if (SiriusEditor.Document == null)
        //    {
        //        var mb = new MessageBoxOk();
        //        mb.ShowDialog("Information !", "도면 데이터를 불러올 Document 가 준비되지 않았습니다.");
        //        return;
        //    }

        //    //  전체 가공 객체 개수 체크
        //    int m_nTotalCount = 0;
        //    foreach (var layer in SiriusEditor.Document.Layers)
        //    {
        //        m_nTotalCount += layer.Count;

        //        foreach (var entity in layer)
        //        {
        //            switch (entity.EntityType)
        //            {
        //                case EType.Group:
        //                    m_nGroupCount++;
        //                    break;
        //            }
        //        }
        //    }

        //    if (m_nTotalCount == 0) 
        //    {
        //        var mb = new MessageBoxOk();
        //        mb.ShowDialog("Information !", "도면 데이터가 없습니다.");
        //        return;
        //    }

        //    if (m_nGroupCount > 0)
        //    {
        //        var mb = new MessageBoxOk();
        //        mb.ShowDialog("Information !", "모든 데이터는 Group 해제 상태여야 합니다.");
        //        return;
        //    }

        //    //  전체 가공 객체 개수만큼 리스트 생성
        //    var list = new List<IEntity>(m_nTotalCount);

        //    //  가공 데이터 뒤집기
        //    foreach (var layer in SiriusEditor.Document.Layers)
        //    {
        //        foreach (var entity in layer)
        //        {
        //            switch (entity.EntityType)
        //            {
        //                case EType.Point:
        //                    var point = entity as SpiralLab.Sirius.Point;

        //                    if (m_bDirHor)
        //                    {
        //                        var Reverse_point = new SpiralLab.Sirius.Point((float)(point.Location.X * -1.0), (float)point.Location.Y);
        //                        Reverse_point.DwellTime = point.DwellTime;
        //                        list.Add(Reverse_point);
        //                    }
        //                    else
        //                    {
        //                        var Reverse_point = new SpiralLab.Sirius.Point((float)point.Location.X, (float)(point.Location.Y * -1.0));
        //                        Reverse_point.DwellTime = point.DwellTime;
        //                        list.Add(Reverse_point);
        //                    }
        //                    break;


        //                case EType.Points:
        //                    var points = entity as Points;
        //                    foreach (var vertex in points)
        //                    {
        //                        //vertex.X
        //                        //vertex.Y
        //                    }
        //                    list.Add(points);
        //                    break;


        //                case EType.Line:
        //                    var line = entity as Line;

        //                    if (m_bDirHor)
        //                    {
        //                        var Reverse_line = new SpiralLab.Sirius.Line((float)(line.Start.X * -1.0), (float)line.Start.Y, (float)(line.End.X * -1.0), (float)line.End.Y);
        //                        list.Add(Reverse_line);
        //                    }
        //                    else
        //                    {
        //                        var Reverse_line = new SpiralLab.Sirius.Line((float)line.Start.X, (float)(line.Start.Y * -1.0), (float)line.End.X, (float)(line.End.Y * -1.0));
        //                        list.Add(Reverse_line);
        //                    }
        //                    break;


        //                case EType.Arc:
        //                    var arc = entity as Arc;

        //                    list.Add(arc);

        //                    //arc.Radius
        //                    //arc.Center
        //                    //arc.StartAngle
        //                    //arc.SweepAngle
        //                    //success &= arc.Mark(markerArg);
        //                    break;


        //                case EType.Circle:
        //                    var circle = entity as Circle;

        //                    if (m_bDirHor)
        //                    {
        //                        var Reverse_Circle = new SpiralLab.Sirius.Circle((float)(circle.Center.X * -1.0), (float)circle.Center.Y, (float)circle.Radius);
        //                        list.Add(Reverse_Circle);
        //                    }
        //                    else
        //                    {
        //                        var Reverse_Circle = new SpiralLab.Sirius.Circle((float)(circle.Center.X), (float)(circle.Center.Y * -1.0), (float)circle.Radius);
        //                        list.Add(Reverse_Circle);
        //                    }
        //                    break;


        //                case EType.Rectangle:
        //                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

        //                    if (m_bDirHor)
        //                    {
        //                        var Reverse_rectangle = new SpiralLab.Sirius.Rectangle((float)(rectangle.Center.X * -1.0), (float)rectangle.Center.Y, (float)rectangle.Width, (float)rectangle.Height);
        //                        list.Add(Reverse_rectangle);
        //                    }
        //                    else
        //                    {
        //                        var Reverse_rectangle = new SpiralLab.Sirius.Rectangle((float)rectangle.Center.X, (float)(rectangle.Center.Y * -1.0), (float)rectangle.Width, (float)rectangle.Height);
        //                        list.Add(Reverse_rectangle);
        //                    }
        //                    break;


        //                case EType.LWPolyline:
        //                    var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;

        //                    var Reverse_lwPolyline = new SpiralLab.Sirius.LwPolyline();

        //                    foreach (var vertex in lwPolyline)
        //                    {
        //                        if (m_bDirHor)
        //                        {
        //                            Reverse_lwPolyline.Add(new LwPolyLineVertex((float)(vertex.X * -1.0), (float)vertex.Y, vertex.Bulge));
        //                        }
        //                        else
        //                        {
        //                            Reverse_lwPolyline.Add(new LwPolyLineVertex((float)vertex.X, (float)(vertex.Y * -1.0), vertex.Bulge));
        //                        }
        //                    }
        //                    Reverse_lwPolyline.IsClosed = lwPolyline.IsClosed;

        //                    list.Add(Reverse_lwPolyline);
        //                    break;


        //                case EType.Spiral:
        //                    var spiral = entity as Spiral;

        //                    list.Add(spiral);

        //                    //spiral.OutterDiameter 
        //                    //spiral.InnerDiameter
        //                    //spiral.RadialPitch
        //                    //spiral.Revolutions
        //                    //spiral.Center
        //                    //success &= spiral.Mark(markerArg);
        //                    break;


        //                case EType.Group:
        //                default:
        //                    var group = entity as Group;

        //                        //foreach (var subEntity in group)
        //                        //{
        //                        //    Type t = subEntity.GetType();

        //                        //    if (t.Name == "LwPolyline")
        //                        //    {
        //                        //        var pl = subEntity as SpiralLab.Sirius.LwPolyline;

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

        //                        //        //  객체 Type
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

        //                        //        //  객체 Edge 좌표 개수
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

        //                        //        //  객체 Edge 좌표 데이터 저장
        //                        //        for (int n_pl = 0; n_pl < pl.Count; n_pl++)
        //                        //        {
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
        //                        //        }

        //                        //        //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
        //                        //        if (pl.IsClosed)
        //                        //        {
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
        //                        //        }

        //                        //        //  Jump 데이터 길이 누적
        //                        //        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
        //                        //        {
        //                        //            m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
        //                        //            m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
        //                        //            m_ptTo.X = (double)m_ptLast.X;
        //                        //            m_ptTo.Y = (double)m_ptLast.Y;

        //                        //            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
        //                        //            }
        //                        //        }

        //                        //        //  마지막 좌표 위치 저장
        //                        //        m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
        //                        //        m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;

        //                        //        //  가공 데이터 길이 누적
        //                        //        for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
        //                        //        {
        //                        //            m_ptFrom.X = (double)pl.Items[n_pl].X;
        //                        //            m_ptFrom.Y = (double)pl.Items[n_pl].Y;
        //                        //            m_ptTo.X = (double)pl.Items[n_pl + 1].X;
        //                        //            m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

        //                        //            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
        //                        //            {
        //                        //                m_dTotal_DrillingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
        //                        //            }
        //                        //        }

        //                        //        //  영역 객체 개수 +1
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
        //                        //    }
        //                        //    else if (t.Name == "Circle")
        //                        //    {
        //                        //        var pl = subEntity as SpiralLab.Sirius.Circle;

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

        //                        //        //  객체 Type
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

        //                        //        //  객체 Edge 좌표 개수
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = 1;

        //                        //        //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

        //                        //        //  Circle 의 경우, 두 번째 데이터는 Radius 값
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

        //                        //        //  Jump 데이터 길이 누적
        //                        //        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
        //                        //        {
        //                        //            m_ptFrom.X = (double)pl.Center.X;
        //                        //            m_ptFrom.Y = (double)pl.Center.Y;
        //                        //            m_ptTo.X = (double)m_ptLast.X;
        //                        //            m_ptTo.Y = (double)m_ptLast.Y;

        //                        //            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
        //                        //            }
        //                        //        }

        //                        //        //  마지막 좌표 위치 저장
        //                        //        m_ptLast.X = (double)pl.Center.X;
        //                        //        m_ptLast.Y = (double)pl.Center.Y;

        //                        //        //  가공 데이터 길이 누적
        //                        //        m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI;

        //                        //        //  영역 객체 개수 +1
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
        //                        //    }
        //                        //    else if (t.Name == "Rectangle")
        //                        //    {
        //                        //        var pl = subEntity as SpiralLab.Sirius.Rectangle;

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

        //                        //        //  객체 Type
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

        //                        //        //  객체 Edge 좌표 개수
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = 5;

        //                        //        //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

        //                        //        //  Jump 데이터 길이 누적
        //                        //        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
        //                        //        {
        //                        //            m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
        //                        //            m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
        //                        //            m_ptTo.X = (double)m_ptLast.X;
        //                        //            m_ptTo.Y = (double)m_ptLast.Y;

        //                        //            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
        //                        //            }
        //                        //        }

        //                        //        //  마지막 좌표 위치 저장
        //                        //        m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
        //                        //        m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;

        //                        //        //  가공 데이터 길이 누적
        //                        //        m_dTotal_DrillingDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

        //                        //        //  영역 객체 개수 +1
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
        //                        //    }
        //                        //    else if (t.Name == "Line")
        //                        //    {
        //                        //        var pl = subEntity as SpiralLab.Sirius.Line;

        //                        //        //  Line 데이터를 Polyline 처럼 변경한다. (나열된 Line 으로는 드릴홀인지 아닌지 확인이 안됨)

        //                        //        if (m_nEachDrillHole_LineCount == 0)              //  사각형을 이루는 Line 의 시작
        //                        //        {
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  사각형은 좌표 개수가 5개 (시작 위치에서 다시 시작 위치로 와야 함)
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  사각형은 좌표 개수가 5개 (시작 위치에서 다시 시작 위치로 와야 함)   - 내부 가공에 사용                                                          

        //                        //            //  객체 Type
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

        //                        //            //  객체 Edge 좌표 개수
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = 5;

        //                        //            //  1 번째 Edge 좌표 저장 (Start)
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
        //                        //            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
        //                        //            m_nEachDrillHole_LineCount++;

        //                        //            //  2 번째 Edge 좌표 저장 (End)
        //                        //            //  1 번째 Edge 좌표의 End 좌표와 거리가 먼 Start or End 좌표가 진짜 End 좌표이다.
        //                        //            if (((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X - 0.002) <= (double)pl.End.X) &&
        //                        //                ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X + 0.002) >= (double)pl.End.X) &&

        //                        //                ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y - 0.002) <= (double)pl.End.Y) &&
        //                        //                ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y + 0.002) >= (double)pl.End.Y))
        //                        //            {
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.End.X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.End.Y;
        //                        //            }
        //                        //            m_nEachDrillHole_LineCount++;
        //                        //        }
        //                        //        else
        //                        //        {
        //                        //            //  3 ~ 5 번째 Edge 좌표 저장 (End 위치만 사용)
        //                        //            //  앞 Line 의 End 좌표와 거리가 먼 Start or End 좌표가 진짜 End 좌표이다.
        //                        //            //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.End.X;
        //                        //            //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.End.Y;
        //                        //            if (((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X - 0.002) <= (double)pl.End.X) &&
        //                        //                ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X + 0.002) >= (double)pl.End.X) &&

        //                        //                ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y - 0.002) <= (double)pl.End.Y) &&
        //                        //                ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
        //                        //                m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y + 0.002) >= (double)pl.End.Y))
        //                        //            {
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.End.X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.End.Y;
        //                        //            }
        //                        //            m_nEachDrillHole_LineCount++;

        //                        //            //  마지막 Edge 좌표(5 번째) 였으면? --> Object 개수 증가, Line Count 0 세팅
        //                        //            if (m_nEachDrillHole_LineCount == 5)
        //                        //            {
        //                        //                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                        //                //  데이터 좌표계 정규화

        //                        //                //  Min, Max 좌표
        //                        //                m_dLine_Min_X = double.MaxValue;
        //                        //                m_dLine_Max_X = double.MinValue;
        //                        //                m_dLine_Min_Y = double.MaxValue;
        //                        //                m_dLine_Max_Y = double.MinValue;

        //                        //                for (int i = 0; i < 4; i++)
        //                        //                {
        //                        //                    if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X >= m_dLine_Max_X)
        //                        //                        m_dLine_Max_X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X;
        //                        //                    if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X <= m_dLine_Min_X)
        //                        //                        m_dLine_Min_X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X;

        //                        //                    if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y >= m_dLine_Max_Y)
        //                        //                        m_dLine_Max_Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y;
        //                        //                    if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y <= m_dLine_Min_Y)
        //                        //                        m_dLine_Min_Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y;
        //                        //                }

        //                        //                //  객체 Edge 좌표 데이터 저장 (LT --> LB --> RB --> RT --> LT)
        //                        //                //  Left Top
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X = m_dLine_Min_X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y = m_dLine_Max_Y;

        //                        //                //  Left Bottom
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].X = m_dLine_Min_X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].Y = m_dLine_Min_Y;

        //                        //                //  Right Bottom
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].X = m_dLine_Max_X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].Y = m_dLine_Min_Y;

        //                        //                //  Right Top
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[3].X = m_dLine_Max_X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[3].Y = m_dLine_Max_Y;

        //                        //                //  Left Top
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[4].X = m_dLine_Min_X;
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[4].Y = m_dLine_Max_Y;
        //                        //                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                        //                ///

        //                        //                if ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X !=
        //                        //                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X) ||
        //                        //                    (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y !=
        //                        //                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y))
        //                        //                {
        //                        //                    return (int)nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED;        //  "Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다."
        //                        //                }

        //                        //                m_nEachDrillHole_LineCount = 0;

        //                        //                //  Jump 데이터 길이 누적
        //                        //                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
        //                        //                {
        //                        //                    m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                                        m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
        //                        //                    m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                                        m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
        //                        //                    m_ptTo.X = (double)m_ptLast.X;
        //                        //                    m_ptTo.Y = (double)m_ptLast.Y;

        //                        //                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
        //                        //                    {
        //                        //                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
        //                        //                    }
        //                        //                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
        //                        //                    {
        //                        //                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
        //                        //                    }
        //                        //                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
        //                        //                    {
        //                        //                        m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
        //                        //                    }
        //                        //                }

        //                        //                //  마지막 좌표 위치 저장
        //                        //                m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                            m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
        //                        //                m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
        //                        //                            m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;

        //                        //                //  가공 데이터 길이 누적
        //                        //                m_dTotal_DrillingDataLength += ((m_dLine_Max_X - m_dLine_Min_X) * 2.0) + ((m_dLine_Max_Y - m_dLine_Min_Y) * 2.0);

        //                        //                //  영역 객체 개수 +1
        //                        //                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
        //                        //            }
        //                        //        }
        //                        //    }
        //                        //    else if (t.Name == "Arc")
        //                        //    {
        //                        //        var pl = subEntity as SpiralLab.Sirius.Arc;

        //                        //        //  객체 Type
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

        //                        //        //  객체 Radius
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dRadius = (double)pl.Radius;

        //                        //        //  객체 Center 좌표
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dCenter.X = (double)pl.Center.X;
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dCenter.Y = (double)pl.Center.Y;

        //                        //        //  객체 Start Angle
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dStartAngle = (double)pl.StartAngle;

        //                        //        //  객체 Sweep Angle
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dSweepAngle = (double)pl.SweepAngle;

        //                        //        //  Jump 데이터 길이 누적
        //                        //        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
        //                        //        {
        //                        //            m_ptFrom.X = (double)pl.Center.X;
        //                        //            m_ptFrom.Y = (double)pl.Center.Y;
        //                        //            m_ptTo.X = (double)m_ptLast.X;
        //                        //            m_ptTo.Y = (double)m_ptLast.Y;

        //                        //            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
        //                        //            }
        //                        //            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
        //                        //            {
        //                        //                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
        //                        //            }
        //                        //        }

        //                        //        //  마지막 좌표 위치 저장
        //                        //        m_ptLast.X = (double)pl.Center.X;
        //                        //        m_ptLast.Y = (double)pl.Center.Y;

        //                        //        //  가공 데이터 길이 누적
        //                        //        if ((double)pl.SweepAngle > 0.0)
        //                        //            m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI * ((double)pl.SweepAngle / 360.0);

        //                        //        //  영역 객체 개수 +1
        //                        //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
        //                        //    }
        //                        //    else        //  또 뭐가 있나...
        //                        //    {

        //                        //    }
        //                        //}
        //                    //}



        //                    //list.Add(group);

        //                    //success &= group.Mark(markerArg);
        //                    break;
        //                    // case EType....
        //                    // ...

        //                    //default:
        //                    //    if (entity is IMarkerable markerable)
        //                    //    {
        //                    //        // mark entity
        //                    //        // 해당 개체(Entity) 가공 
        //                    //        //success &= markerable.Mark(markerArg);
        //                    //    }
        //                    //    break;
        //            }

        //            if (!success)
        //                break;
        //        }
        //    }

        //    //  EditView 객체 Clear
        //    SiriusEditor.Document.New();

        //    //  Reverse 객체를 다시 EditView 에 넣기
        //    foreach (var Reverse_entity in list)
        //    {
        //        if (Reverse_entity != null)
        //        {
        //            switch (Reverse_entity.EntityType)
        //            {
        //                case EType.Point:
        //                    var point = Reverse_entity as SpiralLab.Sirius.Point;

        //                    SiriusEditor.Document.Action.ActEntityAdd(point);
        //                    break;

        //                case EType.Points:
        //                    var points = Reverse_entity as Points;

        //                    SiriusEditor.Document.Action.ActEntityAdd(points);
        //                    break;

        //                case EType.Line:
        //                    var line = Reverse_entity as Line;

        //                    SiriusEditor.Document.Action.ActEntityAdd(line);
        //                    break;

        //                case EType.Arc:
        //                    var arc = Reverse_entity as Arc;

        //                    SiriusEditor.Document.Action.ActEntityAdd(arc);
        //                    break;

        //                case EType.Circle:
        //                    var circle = Reverse_entity as Circle;

        //                    SiriusEditor.Document.Action.ActEntityAdd(circle);
        //                    break;

        //                case EType.Rectangle:
        //                    var rectangle = Reverse_entity as SpiralLab.Sirius.Rectangle;

        //                    SiriusEditor.Document.Action.ActEntityAdd(rectangle);
        //                    break;

        //                case EType.LWPolyline:
        //                    var lwPolyline = Reverse_entity as SpiralLab.Sirius.LwPolyline;

        //                    SiriusEditor.Document.Action.ActEntityAdd(lwPolyline);
        //                    break;

        //                case EType.Spiral:
        //                    var spiral = Reverse_entity as Spiral;

        //                    SiriusEditor.Document.Action.ActEntityAdd(spiral);
        //                    break;

        //                case EType.Group:
        //                default:
        //                    var group = Reverse_entity as Group;

        //                    SiriusEditor.Document.Action.ActEntityAdd(group);

        //                    //success &= group.Mark(markerArg);
        //                    break;
        //                    // case EType....
        //                    // ...

        //                    //default:
        //                    //    if (entity is IMarkerable markerable)
        //                    //    {
        //                    //        // mark entity
        //                    //        // 해당 개체(Entity) 가공 
        //                    //        //success &= markerable.Mark(markerArg);
        //                    //    }
        //                    //    break;
        //            }
        //            if (!success)
        //                break;
        //        }
        //    }

        //    var mb1 = new MessageBoxOk();
        //    mb1.ShowDialog("Information !", "가공 도면 반전 완료.");
        //    return;
        //}
        
        
        //private void btnTest_AlignRun_Click(object sender, EventArgs e)
        //{
        //    if (workStage.m_nWorkStage_MainStep == (int)WorkStage.WorkStage_Step.None)
        //    {
        //        var mb = new MessageBoxYesNo();
        //        if (DialogResult.Yes != mb.ShowDialog("Question ?", "Align 을 시작하시겠습니까?"))
        //            return;

        //        if (cb_Test_2PointAlign.Checked)
        //        {
        //            workStage.m_bFindFirstAlignMarkOnly = false;
        //        }
        //        else
        //        {
        //            workStage.m_bFindFirstAlignMarkOnly = true;
        //        }

        //        workStage.m_nProductAlign_MainStep = (int)WorkStage.ProductAlign_Step.Start;

        //        if (!workStage.m_bAlignVisionThread_Use)
        //        {
        //            workStage.timer_VisionAlign.Enabled = true;
        //        }
        //    }
        //    else
        //    {
        //        workStage.timer_VisionAlign.Enabled = false;
        //        workStage.m_nProductAlign_MainStep = (int)WorkStage.ProductAlign_Step.None;
        //    }
        //}

        private void btnTest_RotOffsetMove_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Align 보정량을 적용하시겠습니까?"))
                return;

            //DrillingData_RotationOffset_Move(workStage.m_forAlign_Data[(int)AlignParam.POS_ROTCENTER].X, workStage.m_forAlign_Data[(int)AlignParam.POS_ROTCENTER].Y, 
            //                                workStage.m_forAlign_Data[(int)AlignParam.RESULTTHETA].X,
            //                                workStage.m_forAlign_Data[(int)AlignParam.POS_OFFSET].X, workStage.m_forAlign_Data[(int)AlignParam.POS_OFFSET].Y);

            var mb1 = new MessageBoxOk();
            mb1.ShowDialog("Result", "Align 보정 적용 완료.");
        }        
    }
}
