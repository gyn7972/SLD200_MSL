using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Motion.Ajin.IO;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Common.VisionPart;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using System.Windows.Forms;
using System.Security.Policy;

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
using MessageBox = System.Windows.Forms.MessageBox;
using QMC.Core;

namespace QMC.Common
{
    public static class Equipment
    {
        private static uint m_nLastDioUID;
        private static uint m_nLastAxisUID;
        private static int m_nLastModuleNo;
        private static RecipeInfo m_CurrentRecipe;
        public delegate void EventLoadModuleCollection();

        public static event EventLoadModuleCollection LoadModules;

        public static string Name { set; get; }
        public static List<MotionBoard> MotionBoards { set; get; }
        public static List<IOBoard> IOBoards { set; get; }

        public static List<IOModule> IOModules { set; get; }

        public static List<IOPoint> IOPoints { set; get; }
        public static ModuleCollection Modules { set; get; }

        public static InitializeSequenceCollection InitializeSequence { set; get; }

        public static LoadingQueue LoadingQueue { set; get; }
        //public static RecipeInfoCollection Recipes { set; get; }

        public static ProductionData ProductData { set; get; }

        public static int RtcMode_syncAxis { set; get; }           //  0 : None     1 : syncAxis    2 : RTC6

        public static string XmlFile_forSyncAxis { set; get; }

        public enum RtcMode : int
        {
            RTC_NONE = 0,           //  0 : None (Not Initialize)
            RTC_SYNCAXIS = 1,       //  1 : syncAxis Mode
            RTC_RTC6 = 2,           //  2 : RTC6 Mode
        }

        public static bool Mode_DryRun { set; get; }
        private static bool Machine_Run;
        public static bool AjinBoard_Opened { set; get; }
        public static bool m_bRedraw_FormWorkStageParameterConfig { set; get; }
        public static bool m_bRedraw_FormLoaderParameterConfig { set; get; }
        public static bool m_bRedraw_FormUnloaderParameterConfig { set; get; }
        public static bool m_bRedraw_FormBdsParameterConfig { set; get; }
        public static bool m_bRedraw_FormUpperCameraConfig { set; get; }
        public static bool m_bRedraw_FormLowerCameraConfig { set; get; }
        public static bool GetMachineRunStatus()
        {
            return Machine_Run;
        }

        private static bool Water_Leak;
        public static bool GetWaterLeakStatus()
        {
            return Water_Leak;
        }
        public static void SetWaterLeakStatus( bool m_bLeak )
        {
            Water_Leak = m_bLeak;
        }

        public static int ScannerMode_Change_byUser { set; get; }           //  0: None         1: Change To RTC6       2: Change to syncAxis

        public static bool m_bWorkTotalTime_Changed { set; get; }
        public static bool m_bWorkElapsedTime_Changed { set; get; }
        public static double WorkTotalTime { set; get; }                     //  총 작업 시간 (sec)
        public static int WorkStartTick { set; get; }                     //  작업 시작 Tick 
        public static int WorkElapsedTick { set; get; }                   //  작업 진행 Tick

        public static double WorkTotalTime_Outline { set; get; }                       //  Outline 총 작업 시간 (sec)
        public static int WorkStartTick_Outline { set; get; }                       //  Outline 작업 시작 Tick 
        public static int WorkElapsedTick_Outline { set; get; }                     //  Outline 작업 진행 Tick
        public static double WorkElapsedTick_Outline_1time { set; get; }               //  Outline 작업 진행 Tick (1회)
        public static double WorkTotalTime_Outline_AdditionalTime { set; get; }        //  Outline 추가 시간 (sec)

        public static double WorkTotalTime_Thruhole { set; get; }                      //  Thruhole 총 작업 시간 (sec)
        public static int WorkStartTick_Thruhole { set; get; }                      //  Thruhole 작업 시작 Tick 
        public static int WorkElapsedTick_Thruhole { set; get; }                    //  Thruhole 작업 진행 Tick
        public static double WorkElapsedTick_Thruhole_1time { set; get; }              //  Thruhole 작업 진행 Tick (1회)
        public static double WorkTotalTime_Thruhole_AdditionalTime { set; get; }       //  Thruhole 추가 시간 (sec)

        public static double WorkTotalTime_Drilling { set; get; }                      //  Drilling 총 작업 시간 (sec)
        public static int WorkStartTick_Drilling { set; get; }                      //  Drilling 작업 시작 Tick 
        public static int WorkElapsedTick_Drilling { set; get; }                    //  Drilling 작업 진행 Tick
        public static double WorkElapsedTick_Drilling_1time { set; get; }              //  Drilling 작업 진행 Tick (1회)
        public static double WorkTotalTime_Drilling_AdditionalTime { set; get; }       //  Drilling 추가 시간 (sec)

        public static double WorkTotalTime_Marking { set; get; }                       //  Marking 총 작업 시간 (sec)
        public static int WorkStartTick_Marking { set; get; }                       //  Marking 작업 시작 Tick 
        public static int WorkElapsedTick_Marking { set; get; }                     //  Marking 작업 진행 Tick
        public static double WorkElapsedTick_Marking_1time { set; get; }               //  Marking 작업 진행 Tick (1회)
        public static double WorkTotalTime_Marking_AdditionalTime { set; get; }        //  Marking 추가 시간 (sec)


        //  전체 가공시간 계산을 위해 사용되는 변수
        public static double MainCycle_Interval { set; get; }                           //  Main Cycle 타이머의 Interval. 


        //  Auto-Focus 에 실패했을 때 사용자가 수동으로 카메라 초점을 조작하기 위한 Flag
        public static bool AutoFocus_Failed { set; get; }


        //  User Stop
        public static bool MachineStop_byUser { set; get; }


        //  Stop by Alarm
        public static bool MachineStop_byAlarm { set; get; }


        //  Area Sensor Detect Flag Reset
        public static bool AreaSensorDetectFlag_Reset { set; get; }


        //  카메라 시리얼 넘버
        public static bool CameraSerialNumberType { set; get; }
        public static string PAKCamera_SerialNumber { set; get; }
        public static int PAKCamera_Width { set; get; }
        public static int PAKCamera_Height { set; get; }
        public static string WaferCamera_SerialNumber { set; get; }
        public static int WaferCamera_Width { set; get; }
        public static int WaferCamera_Height { set; get; }


        //  모터 축 파라미터
        public static int Max_Axis = 14;
        public struct stAxisParameter
        {
            public int LimitSensor_Installed;               //  Limit Sensor 설치 여부 (Not Installed, Installed)
            public int LimitSensor_ActiveLevel;             //  Limit Sensor 동작 레벨 (Low, High)

            public int Home_Sensing;                        //  Home Sensor 형태 (Home, -Limit, +Limit)
            public int Home_Installed;                      //  Home Sensor 설치 여부 (Not Installed, Installed)
            public int Home_ActiveLevel;                    //  Home Sensor 동작 레벨 (Low, High)
            public int Home_Direction;                      //  Home Sensor 동작 방향 (Negative, Positive)
            public double Home_Speed_1st;                   //  Home 1st Speed
            public double Home_Speed_2nd;                   //  Home 2nd Speed
            public double Home_Speed_3rd;                   //  Home 3rd Speed
            public double Home_Speed_Last;                  //  Home Last Speed
            public double Home_Offset;                      //  Home Offset

            public double Common_UnitPerPulse_Unit;         //  Unit Per Pulse (Unit)
            public double Common_UnitPerPulse_Pulse;        //  Unit Per Pulse (Pulse)
            public double Common_Acceleration_Min;          //  Acceleration Min
            public double Common_Acceleration_Max;          //  Acceleration Max
            public double Common_Acceleration;              //  Acceleration
            public double Common_Speed_Min;                 //  Speed Min
            public double Common_Speed_Max;                 //  Speed Max
            public double Common_MoveSpeed;                 //  Move Speed
            public double Common_Position_Min;              //  Position Min
            public double Common_Position_Max;              //  Position Max
            public double Common_Settle_Delay;              //  Settle Delay Time

            public double Jog_Speed_Fine;                   //  Jog Speed, Fine
            public double Jog_Speed_Coarse;                 //  Jog Speed, Coarse
            public double Jog_StepSize_Min;                 //  Jog StepSize, Min
            public double Jog_StepSize_Max;                 //  Jog StepSize, Max
            public double Jog_StepSize_Fine;                //  Jog StepSize, Fine
            public double Jog_StepSize_Coarse;              //  Jog StepSize, Coarse
        }
        public static stAxisParameter[] stAxisParam = new stAxisParameter[Max_Axis];                  //  총 14개 축. 가변 가능하도록 변경해야 함. (시간 관계상 고정하자)

        //  Log In
        public static bool Machine_LogIn { set; get; }

        //  버튼 Panel 활성화 여부
        public static bool BottomButtonPanelStatus { set; get; }


        //  비전 검사 시, Spiral 이동 없이 한번만 검사하도록
        public static bool Vision_SpiralMove_Use { set; get; }


        //  작업자 모드인지 관리자 모드인지?
        public static string User_Mode { set; get; }
        public static string User_Name { set; get; }
        //public static bool User_AdminMode { set; get; }
        //public static bool User_QMC_Engineer { set; get; }
        public static int User_LoginMode { set; get; }                      //  0 : Logout     1 : Admin     2 : Engineer     3 : Operator

        public enum UserMode : int
        {
            USER_LOGOUT = 0,            //  0 : Logout
            USER_ADMIN = 1,             //  1 : Administrator
            USER_ENGINEER = 2,          //  2 : Engineer
            USER_OPERATOR = 3,          //  3 : Operator
        }

        //  얼라인 시작할 때 시간
        public static string AlignStart_Time { set; get; }
        public static string AlignVerificationStart_Time { set; get; }


        //  로그아웃 할 때 메인화면을 보여주도록 하기 위한 Flag
        public static bool User_LogOut_1time {  set; get; }


        //  자동 로그아웃
        public static bool LogIn_Status {  set; get; }
        public static bool AutoLogOut_Execute {  set; get; }
        public static bool AutoLogOut_Executed { set; get; }


        //  바코드 리더 Comm 1번만
        public static bool m_bBarcodeReaderComm_1time { set; get; }


        //public static SiriusViewerForm EqpSiriusViewer { set; get; }
        public static bool m_bAlignVisionThread_1time { set; get; }
        public static bool m_bParamLoadThread_1time { set; get; }


        public static void CreateInstance(string strEquipmentName)
        {
            ScannerMode_Change_byUser = (int)RtcMode.RTC_NONE;
            Mode_DryRun = true;
            Machine_Run = false;
            MachineStop_byUser = false;
            MachineStop_byAlarm = false;
            AreaSensorDetectFlag_Reset = false;

            BottomButtonPanelStatus = false;

            Machine_LogIn = false;
            AutoLogOut_Execute = false;
            AutoLogOut_Executed = false;
            LogIn_Status = false;

            m_bBarcodeReaderComm_1time = false;

            CameraSerialNumberType = false;
            PAKCamera_SerialNumber = "";
            PAKCamera_Width = 0;
            PAKCamera_Height = 0;
            WaferCamera_SerialNumber = "";
            WaferCamera_Width = 0;
            WaferCamera_Height = 0;

            Vision_SpiralMove_Use = true;

            User_Mode = null;
            User_Name = null;
            User_LoginMode = (int)UserMode.USER_LOGOUT;
            //User_AdminMode = false;
            //User_QMC_Engineer = false;

            AlignStart_Time = null;
            User_LogOut_1time = false;

            AjinBoard_Opened = false;
            RtcMode_syncAxis = 0;

            MainCycle_Interval = 54;                //  Main Cycle Timer Interval : 20 인데, Debug 모드에서 가동 시 47 ~ 62 사이 정도 나오는 듯 
            AutoFocus_Failed = false;

            //XmlFile_forSyncAxis = "D:\\SLO-400_Parameter\\syncAXISConfig.SLD100.xml";

            //  총 가공 시간 표시
            m_bWorkTotalTime_Changed = false;
            WorkTotalTime = 0;                      //  총 작업 시간 (sec)

            m_bWorkElapsedTime_Changed = false;
            WorkStartTick = 0;                      //  작업 시작 Tick 
            WorkElapsedTick = 0;                    //  작업 진행 Tick

            WorkStartTick_Outline = 0;                      //  Outline 작업 시작 Tick 
            WorkElapsedTick_Outline = 0;                    //  Outline 작업 진행 Tick
            WorkStartTick_Thruhole = 0;                     //  Thruhole 작업 시작 Tick 
            WorkElapsedTick_Thruhole = 0;                   //  Thruhole 작업 진행 Tick
            WorkStartTick_Drilling = 0;                     //  Drilling 작업 시작 Tick 
            WorkElapsedTick_Drilling = 0;                   //  Drilling 작업 진행 Tick
            WorkStartTick_Marking = 0;                      //  Marking 작업 시작 Tick 
            WorkElapsedTick_Marking = 0;                    //  Marking 작업 진행 Tick

            WorkElapsedTick_Outline_1time = 0;
            WorkElapsedTick_Thruhole_1time = 0;
            WorkElapsedTick_Drilling_1time = 0;
            WorkElapsedTick_Marking_1time = 0;

            m_bRedraw_FormWorkStageParameterConfig = false;
            m_bRedraw_FormLoaderParameterConfig = false;
            m_bRedraw_FormUnloaderParameterConfig = false;
            m_bRedraw_FormBdsParameterConfig = false;
            m_bRedraw_FormUpperCameraConfig = false;
            m_bRedraw_FormLowerCameraConfig = false;

            m_bAlignVisionThread_1time = false;
            m_bParamLoadThread_1time = false;

            //  모터 축 파라미터 초기화
            for (int i = 0; i < Max_Axis; i++)
            {
                stAxisParam[i].LimitSensor_Installed = 0;
                stAxisParam[i].LimitSensor_ActiveLevel = 0;
                stAxisParam[i].Home_Sensing = 0;
                stAxisParam[i].Home_Installed = 0;
                stAxisParam[i].Home_ActiveLevel = 0;
                stAxisParam[i].Home_Direction = 0;
                stAxisParam[i].Home_Speed_1st = 0;
                stAxisParam[i].Home_Speed_2nd = 0;
                stAxisParam[i].Home_Speed_3rd = 0;
                stAxisParam[i].Home_Speed_Last = 0;
                stAxisParam[i].Home_Offset = 0;
                stAxisParam[i].Common_UnitPerPulse_Unit = 0;
                stAxisParam[i].Common_UnitPerPulse_Pulse = 0;
                stAxisParam[i].Common_Acceleration_Min = 0;
                stAxisParam[i].Common_Acceleration_Max = 0;
                stAxisParam[i].Common_Acceleration = 0;
                stAxisParam[i].Common_Speed_Min = 0;
                stAxisParam[i].Common_Speed_Max = 0;
                stAxisParam[i].Common_MoveSpeed = 0;
                stAxisParam[i].Common_Position_Min = 0;
                stAxisParam[i].Common_Position_Max = 0;
                stAxisParam[i].Common_Settle_Delay = 0;
                stAxisParam[i].Jog_Speed_Fine = 0;
                stAxisParam[i].Jog_Speed_Coarse = 0;
                stAxisParam[i].Jog_StepSize_Min = 0;
                stAxisParam[i].Jog_StepSize_Max = 0;
                stAxisParam[i].Jog_StepSize_Fine = 0;
                stAxisParam[i].Jog_StepSize_Coarse = 0;
            }

            m_nLastDioUID = 0;
            m_nLastAxisUID = 0;
            m_nLastModuleNo = 0;
            Name = strEquipmentName;
            MotionBoards = new List<MotionBoard>();
            IOBoards = new List<IOBoard>();
            IOModules = new List<IOModule>();
            IOPoints = new List<IOPoint>();
            Modules = new ModuleCollection();
            InitializeSequence = new InitializeSequenceCollection();
            LoadingQueue = new LoadingQueue();
            ConfigManager.SetEquipmentName(Name);
            CreateModules();
            LoadMotionBoards();
            LoadIOBoards();
            //LoadModuleCollection();
            //LoadInitializeSequence();
            LoadRecipe();

            string strRecipeName = DataManager.Instance.Recipe.Header.CurrentRecipeName;
            for (int i = 0; i < DataManager.Instance.Recipe.Count; i++)
            {
                if (DataManager.Instance.Recipe[i].Name == strRecipeName)
                {
                    m_CurrentRecipe = DataManager.Instance.Recipe[i];
                    ApplyRecipeData();
                    break;
                }
            }

            int m_nBoardOpened = -1;

            LoadConfig();
            foreach (var board in MotionBoards)
            {
                m_nBoardOpened = board.Open();
            }
            foreach (var board in IOBoards)
            {
                board.Open();
            }


            //  2024. 04. 08.  SCH : Pattern Matching Image 저장 폴더 생성
            string strFolderPath = ConfigManager.GetPatternImagePath();
            if (!VerifyFile(strFolderPath))
            {
                Directory.CreateDirectory(ConfigManager.GetPatternImagePath());
            }



            //FunctionManager.Instance.SetModuleCollection(Modules);

            //ApplyConfigData();

            CommonModule.Instance.Initialize();

            //DieLoader dieLoader = Modules[1] as DieLoader;
            //if (dieLoader != null)
            //{
            //
            //}


            NewForm_AxisParameter_Load();


            if (m_nBoardOpened != 0)
            {
                MessageBox.Show("모터 파라미터 폴더가 없거나, 모터 파라미터 파일이 없습니다.\r\n\r\n[D:\\SLD-200_Parameter\\SLD-200.mot]", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private static void CreateModules()
        {
            //  2022. 03. 21.  SCH : 여기에 Module 추가. (Button 생성)
            //StageLoader stageLoader = new StageLoader("SourceLoader");
            //stageLoader.Create();
            //Modules.Add(stageLoader);

            CommonModule common = CommonModule.Instance;
            common.Create();
            Modules.Add(common);

            //WorkStage workStage = new WorkStage("WorkStage");
            WorkStage workStage = new WorkStage("WorkStage");
            workStage.Create();
            Modules.Add(workStage);

            Loader Loader = new Loader("Loader");
            Loader.Create();
            Modules.Add(Loader);

            Unloader unloader = new Unloader("Unloader");
            unloader.Create();
            Modules.Add(unloader);

            Modules.Laser laser = new Modules.Laser("Laser");
            laser.Create();
            Modules.Add(laser);

            Scanner scanner = new Scanner("Scanner");
            scanner.Create();
            Modules.Add(scanner);

            Modules.Vision vision = new Modules.Vision("Vision");
            vision.Create();
            Modules.Add(vision);

            Bds bds = new Bds("BDS");
            bds.Create();
            Modules.Add(bds);
        }

        public static void Start()
        {
            //foreach (var module in Modules)
            //{
            //    module.Start();
            //}

            Machine_Run = true;
        }

        public static void Stop()
        {
            //foreach (var module in Modules)
            //{
            //    module.Stop();
            //}

            //Equipment.MachineStop_byAlarm = false;

            Machine_Run = false;
        }

        public static void Close()
        {
            foreach (Module module in Modules)
            {
                module.Close();
            }
            foreach (var board in MotionBoards)
            {
                board.Close();
            }
        }

        public static void LoadMotionBoards()
        {
            string strFilePath = ConfigManager.GetMotionFilePath();
            FileInfo fi = new FileInfo(strFilePath);
            if (fi.Exists)
            {
                using (FileStream fs = new FileStream(strFilePath, FileMode.Open))
                {
                    MotionBoards.Clear();
                    ResetModuleMotion();

                    while (fs.Position < fs.Length)
                    {
                        MotionBoardConfiguration configuration;
                        MotionBoardConfiguration.Load(fs,out configuration);
                        MotionBoard board = null;

                        switch(configuration.BoardType)
                        {
                            case MotionBoardType.ACS:
                                board = new ACSSPiiPlusMotionBoard();
                                break;
                            case MotionBoardType.Ajin:
                                board = new AjinAxlMotionBoard();
                                break;
                            default:
                                break;
                        }

                        board.Load(configuration, fs);
                        MotionBoards.Add(board);
                        uint nMaxUid = board.GetMaxAxisUid();
                        if (m_nLastAxisUID < nMaxUid)
                            m_nLastAxisUID = nMaxUid;
                    }

                    foreach (MotionBoard board in MotionBoards)
                    {
                        for (int i = 0; i < board.GetAxisCount(); i++)
                        {
                            MotionAxis axis = board.GetAxis(i);
                            Part part = Equipment.GetPart(axis.Configuration.ModuleUid, axis.Configuration.PartUid);
                            if (part != null && axis.Configuration.Tag != null)
                            {
                                part.SetMotion(axis.Configuration.Tag, axis);
                            }
                        }
                    }
                }
            }
        }

        private static void ResetModuleMotion()
        {
            foreach (Module module in Modules)
            {
                module.ClearMotion();
                foreach (Part part in module.Parts)
                {
                    part.ClearMotion();
                }
            }
        }

        private static void ResetModuleDioPoint()
        {
            foreach (Module module in Modules)
            {
                module.ClearDioPoint();
                foreach (Part part in module.Parts)
                {
                    part.ClearDioPoint();
                }
            }
        }

        public static Part GetPart(string strModule, string strPart)
        {
            Part part = null;
            foreach (Module module in Modules)
            {
                if (module.Name == strModule)
                {
                    foreach (Part part2 in module.Parts)
                    {
                        if (part2.Name == strPart)
                        {
                            part = part2;
                            break;
                        }

                    }
                }
            }

            return part;
        }
        public static void SaveMotionBoard(MotionBoardConfigurationColletion configurations)
        {

            string strMotionFilePath = ConfigManager.GetMotionFilePath();
            {
                string strFolderPath = ConfigManager.GetConfigPath();
                if (!VerifyFile(strFolderPath))
                {
                    Directory.CreateDirectory(ConfigManager.GetConfigPath());
                }
            }
            {

                FileInfo fi = new FileInfo(strMotionFilePath);
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }


            MotionBoards.Clear();

            foreach (MotionBoardConfiguration config in configurations)
            {
                MotionBoard board = null;

                switch(config.BoardType)
                {
                    case MotionBoardType.Ajin:
                        board = new AjinAxlMotionBoard();
                        break;
                    case MotionBoardType.ACS:
                        board = new ACSSPiiPlusMotionBoard();
                        break;
                    default:
                        break;
                }

                board.Configuration = config;
                MotionBoards.Add(board);
            }

            using (FileStream fs = new FileStream(strMotionFilePath, FileMode.OpenOrCreate))
            {
                foreach (MotionBoard board in MotionBoards)
                {
                    board.Save(fs);
                }
            }
        }

        public static void LoadIOBoards()
        {
            string strFilePath = ConfigManager.GetIOFilePath();
            FileInfo fi = new FileInfo(strFilePath);
            if (fi.Exists)
            {
                using (FileStream fs = new FileStream(strFilePath, FileMode.Open))
                {
                    ResetModuleDioPoint();
                    IOBoards.Clear();
                    while (fs.Position < fs.Length)
                    {
                        AjinAxlIoBoard board = new AjinAxlIoBoard();
                        board.Load(fs);
                        IOBoards.Add(board);

                        uint nMaxUid = board.GetMaxDioPointUid();
                        if (m_nLastAxisUID < nMaxUid)
                            m_nLastAxisUID = nMaxUid;
                    }

                    foreach (AjinAxlIoBoard board in IOBoards)
                    {
                        foreach (AjinAxlDioModule module in board.Modules)
                        {
                            foreach (DioPoint dioPoint in module.Points)
                            {
                                if (dioPoint.Configuration != null)
                                {
                                    Part part = GetPart(dioPoint.Configuration.ModuleUid, dioPoint.Configuration.PartUid);
                                    if (part != null)
                                    {
                                        part.SetDioPoint(dioPoint.Configuration.Tag, dioPoint);
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveIOPoints()
        {
            List<IOPointConfiguration> ListDigitalIOConfiguration = Equipment.GetIOPointConfigurationList();
            SaveIOPoints(ListDigitalIOConfiguration);
        }

        public static void SaveIOBoard(List<AjinIoAxlBoardConfiguration> configurations)
        {

            string strMotionFilePath = ConfigManager.GetIOFilePath();
            {
                string strFolderPath = ConfigManager.GetConfigPath();
                if (!VerifyFile(strFolderPath))
                {
                    Directory.CreateDirectory(ConfigManager.GetConfigPath());
                }
            }
            {

                FileInfo fi = new FileInfo(strMotionFilePath);
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }

            ValuchangedIOBoard(configurations);


            using (FileStream fs = new FileStream(strMotionFilePath, FileMode.OpenOrCreate))
            {
                foreach (AjinAxlIoBoard board in IOBoards)
                {
                    board.Configuration.ModuleCount = board.Modules.Count;
                    board.Save(fs);
                }
            }
        }
        public static void UpdateMotionAxis(MotionAxis axisTarget)
        {
            foreach (AjinAxlMotionBoard board in MotionBoards)
            {
                for (int i = 0; i < board.GetAxisCount(); i++)
                {
                    AjinAxlAxis axis = board.GetAxis(i) as AjinAxlAxis;
                    if (axisTarget.UID == axis.UID)
                    {
                        axis.Configuration = axisTarget.Configuration as AjinAxlAxisConfiguration;
                        break;
                    }
                }
            }
        }
        public static void SaveMotionAxis()
        {
            MotionAxisConfigurationCollection AxisConfigurations = GetAxisConfigurationList();
            SaveMotionAxis(AxisConfigurations);
        }

        public static void SaveMotionAxis(MotionAxisConfigurationCollection configurations)
        {
            string strMotionFilePath = ConfigManager.GetMotionFilePath();
            if (VerifyFile(strMotionFilePath))
            {
                FileInfo fi = new FileInfo(strMotionFilePath);
                fi.Delete();
            }

            foreach (MotionBoard board in MotionBoards)
            {
                int i = 0;
                while (true)
                {
                    MotionAxis axis = board.GetAxis(i);
                    if (axis != null)
                    {
                        axis.IsDelete = true;
                        foreach (AjinAxlAxisConfiguration config in configurations)
                        {
                            if (board.Configuration.No == config.BoardNo && axis.UID == config.UID)
                            {
                                axis.IsDelete = false;
                                break;
                            }
                        }

                        if (axis.IsDelete)
                        {
                            board.RemoveAtAxis(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (AjinAxlAxisConfiguration config in configurations)
            {
                if (config.BoardNo < MotionBoards.Count)
                {
                    AjinAxlAxis ajinAxis = MotionBoards[config.BoardNo].GetAxis(config.UID) as AjinAxlAxis;
                    if (ajinAxis != null)
                    {
                        ajinAxis.Configuration = config;
                    }
                    else
                    {
                        AjinAxlAxis newAxis = new AjinAxlAxis();
                        newAxis.Configuration = config;
                        MotionBoards[config.BoardNo].AddAxis(newAxis);
                    }
                }

            }

            using (FileStream fs = new FileStream(strMotionFilePath, FileMode.OpenOrCreate))
            {
                foreach (MotionBoard board in MotionBoards)
                {
                    board.Save(fs);
                }
            }
        }

        public static MotionAxis GetAxis(int nAxisNo)
        {
             MotionAxis axis = null;
            foreach (MotionBoard board in MotionBoards)
            {
                axis = board.GetAxis((uint)nAxisNo);
                if (axis != null)
                {
                    break;
                }
            }

            return axis;
        }

        public static IOBoard GetIOBoard(int nBoardNo)
        {
            IOBoard retValue = null;

            foreach (IOBoard board in IOBoards)
            {
                if (board.Configuration.No == nBoardNo)
                {
                    retValue = board;
                }
            }

            return retValue;
        }
        public static void SaveIOModule(List<AjinAxlDioModuleConfiguration> configurations)
        {
            string strIOFilePath = ConfigManager.GetIOFilePath();
            if (VerifyFile(strIOFilePath))
            {
                FileInfo fi = new FileInfo(strIOFilePath);
                fi.Delete();
            }

            ValuchangedIOModule(configurations);

            foreach (AjinAxlDioModuleConfiguration config in configurations)
            {
                IOBoard board = GetIOBoard(config.BoardNo);
                if (board == null)
                {
                    continue;
                }
                AjinAxlDioModule module = board.GetIOModule(config.No) as AjinAxlDioModule;
                if (module != null)
                {
                    module.Configuration = config;
                }
                else
                {
                    AjinAxlDioModule newModule = new AjinAxlDioModule();
                    newModule.Configuration = config;
                    board.Modules.Add(newModule);
                }
            }

            using (FileStream fs = new FileStream(strIOFilePath, FileMode.OpenOrCreate))
            {
                foreach (AjinAxlIoBoard board in IOBoards)
                {
                    board.Configuration.ModuleCount = board.Modules.Count;
                    board.Save(fs);
                }
            }
        }

        public static IOModule GetIOModule(uint nModuleNo)
        {
            IOModule retValue = null;
            foreach (IOBoard board in IOBoards)
            {
                foreach (IOModule module in board.Modules)
                    if (module.Configuration.No == nModuleNo)
                    {
                        retValue = module;
                    }
            }

            return retValue;
        }
        public static void SaveIOPoints(List<IOPointConfiguration> configurations)
        {
            string strIOFilePath = ConfigManager.GetIOFilePath();
            if (VerifyFile(strIOFilePath))
            {
                FileInfo fi = new FileInfo(strIOFilePath);
                fi.Delete();
            }

            foreach (AjinAxlIoBoard board in IOBoards)
            {
                foreach (AjinAxlDioModule module in board.Modules)
                {
                    module.Points.Clear();
                    module.Configuration.PointCount = module.Points.Count;
                }
            }

            foreach (IOPointConfiguration config in configurations)
            {
                AjinAxlDioModule module = GetIOModule(config.ModuleNo) as AjinAxlDioModule;
                if (module != null)
                {
                    DioPoint newDigital = new DioPoint();
                    newDigital.Configuration = config;
                    module.Points.Add(newDigital);
                    module.Configuration.PointCount = module.Points.Count;
                }

            }


            using (FileStream fs = new FileStream(strIOFilePath, FileMode.OpenOrCreate))
            {
                foreach (AjinAxlIoBoard board in IOBoards)
                {
                    board.Save(fs);
                }
            }
        }
        private static void ValuchangedIOBoard(List<AjinIoAxlBoardConfiguration> configurations)
        {
            if (IOBoards.Count > 0)
            {
                List<IOBoard> deleteList = new List<IOBoard>();
                foreach (IOBoard board in IOBoards)
                {
                    bool bFind = false;
                    foreach (AjinIoAxlBoardConfiguration config in configurations)
                    {
                        if (board.Configuration.No == config.No)
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        //삭제 될놈
                        deleteList.Add(board);
                    }
                    //}
                }

                foreach (IOBoard board in deleteList)
                {
                    IOBoards.Remove(board);
                }

                foreach (AjinIoAxlBoardConfiguration config in configurations)
                {
                    bool bFind = false;
                    foreach (IOBoard board in IOBoards)
                    {
                        if (config.No == board.Configuration.No)
                        {
                            bFind = true;
                            board.Configuration = config;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        AjinAxlIoBoard newIOBoard = new AjinAxlIoBoard();
                        newIOBoard.Configuration = config;
                        IOBoards.Add(newIOBoard);
                    }
                }
            }
            else
            {
                foreach (AjinIoAxlBoardConfiguration config in configurations)
                {
                    AjinAxlIoBoard newBoard = new AjinAxlIoBoard();
                    newBoard.Configuration = config;
                    IOBoards.Add(newBoard);
                }
            }
        }
        private static void ValuchangedIOModule(List<AjinAxlDioModuleConfiguration> newList)
        {
            List<IOModule> deleteList = new List<IOModule>();
            foreach (IOBoard board in IOBoards)
            {
                foreach (IOModule module in board.Modules)
                {
                    bool bFind = false;
                    foreach (AjinAxlDioModuleConfiguration newModule in newList)
                    {
                        if (module.No == newModule.No)
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        //삭제 될놈
                        deleteList.Add(module);
                    }
                }
            }
            List<AjinAxlDioModuleConfiguration> addList = new List<AjinAxlDioModuleConfiguration>();
            foreach (AjinAxlDioModuleConfiguration newModule in newList)
            {
                bool bFindNew = false;
                foreach (IOBoard board in IOBoards)
                {
                    foreach (IOModule module in board.Modules)
                    {
                        if (newModule.No == module.No)
                        {
                            bFindNew = true;
                            break;
                        }
                    }
                }
                if (!bFindNew)
                {
                    addList.Add(newModule);
                }
            }
            foreach (IOModule module in deleteList)
            {
                IOBoards[module.Configuration.BoardNo].Modules.Remove(module);
            }
            foreach (AjinAxlDioModuleConfiguration newModule in addList)
            {
                AjinAxlDioModule newIOModule = new AjinAxlDioModule();
                newIOModule.Configuration = newModule;
                IOModules.Add(newIOModule);
            }
        }

        public static Module GetModule(Type type)
        {
            Module RetValue = null;
            foreach (Module module in Modules)
            {
                if (module.GetType() == type)
                {
                    RetValue = module;
                    break;
                }
            }

            return RetValue;
        }

        public static Module GetModule(string strName)
        {
            Module RetValue = null;
            foreach (Module module in Modules)
            {
                if (module.Name == strName)
                {
                    RetValue = module;
                    break;
                }
            }

            return RetValue;
        }

        public static void SetLastAxisUID(uint nUid)
        {
            m_nLastAxisUID = nUid;
        }
        public static uint GetAxisUID()
        {
            return m_nLastAxisUID++;
        }
        public static uint GetDioPointUID()
        {
            return m_nLastDioUID++;
        }

        public static int GetModuleNo()
        {
            return m_nLastModuleNo++;
        }

        public static MotionBoardConfigurationColletion GetMotionConfigurationList()
        {
            MotionBoardConfigurationColletion ConfigurationCollection = new MotionBoardConfigurationColletion();

            foreach (MotionBoard board in MotionBoards)
            {
                ConfigurationCollection.Add(board.Configuration);
            }

            return ConfigurationCollection;
        }
        public static List<AjinIoAxlBoardConfiguration> GetIOConfigurationList()
        {
            List<AjinIoAxlBoardConfiguration> ConfigurationCollection = new List<AjinIoAxlBoardConfiguration>();

            foreach (AjinAxlIoBoard board in IOBoards)
            {
                ConfigurationCollection.Add(board.Configuration);
            }

            return ConfigurationCollection;
        }

        public static MotionAxisConfigurationCollection GetAxisConfigurationList()
        {
            //AjinAxlAxisConfigurationCollection axisConfigurations = new AjinAxlAxisConfigurationCollection();
            MotionAxisConfigurationCollection axisConfigurations = new MotionAxisConfigurationCollection();

            foreach (MotionBoard board in MotionBoards)
            {
                for (int i = 0; i < board.GetAxisCount(); i++)
                {
                    MotionAxis axis = board.GetAxis(i) as MotionAxis;
                    axisConfigurations.Add(axis.Configuration);
                }
            }

            return axisConfigurations;
        }
        public static List<AjinAxlDioModuleConfiguration> GetIOModuleConfigurationList()
        {
            List<AjinAxlDioModuleConfiguration> moduleConfigurations = new List<AjinAxlDioModuleConfiguration>();

            foreach (AjinAxlIoBoard board in IOBoards)
            {
                for (int i = 0; i < board.GetIOModuleCount(); i++)
                {
                    AjinAxlDioModule module = board.GetIOModule(i) as AjinAxlDioModule;
                    AjinAxlDioModuleConfiguration configuration = module.Configuration as AjinAxlDioModuleConfiguration;
                    moduleConfigurations.Add(configuration);
                }
            }

            return moduleConfigurations;
        }

        public static List<IOPointConfiguration> GetIOPointConfigurationList()
        {
            List<IOPointConfiguration> pointConfigurations = new List<IOPointConfiguration>();
            foreach (AjinAxlIoBoard board in IOBoards)
            {
                foreach (AjinAxlDioModule module in board.Modules)
                {
                    for (int i = 0; i < module.GetIOPointCount(); i++)
                    {
                        IOPoint point = module.GetIOPoint(i) as IOPoint;
                        pointConfigurations.Add(point.Configuration);
                    }
                }

            }

            return pointConfigurations;
        }

        public static List<uint> GetAxisUidList()
        {
            List<uint> listUids = new List<uint>();

            foreach (AjinAxlMotionBoard board in MotionBoards)
            {
                for (int i = 0; i < board.GetAxisCount(); i++)
                {
                    AjinAxlAxis axis = board.GetAxis(i) as AjinAxlAxis;
                    listUids.Add(axis.UID);
                }
            }

            return listUids;
        }

        public static List<uint> GetModuleList()
        {
            List<uint> listUids = new List<uint>();

            foreach (MotionBoard board in MotionBoards)
            {
                for (int i = 0; i < board.GetAxisCount(); i++)
                {
                    MotionAxis axis = board.GetAxis(i) as MotionAxis;
                    listUids.Add(axis.UID);
                }
            }

            return listUids;
        }


        private static void CreatePartAxisUid(Part part)
        {
            //part.CreateSaveUidList();
            //foreach (Part subPart in part.Parts)
            //{
            //    CreatePartAxisUid(subPart);
            //}
        }

        private static void CreatePartAxis(Part part, List<MotionAxis> listAxes)
        {
            //part.CreateAxis(listAxes);
            //part.Init();
            //foreach (Part subPart in part.Parts)
            //{
            //    CreatePartAxis(subPart, listAxes);
            //}
        }

        private static void CreatePartDioPointUid(Part part)
        {
            //part.CreateSaveDioPointUidList();
            //foreach (Part subPart in part.Parts)
            //{
            //    CreatePartDioPointUid(subPart);
            //}
        }

        private static void CreatePartDioPoint(Part part, List<DioPoint> listDioPoint)
        {
            //part.CreateDioPoint(listDioPoint);
            //part.Init();
            //foreach (Part subPart in part.Parts)
            //{
            //    CreatePartDioPoint(subPart, listDioPoint);
            //}
        }


        private static bool VerifyFile(string strPath)
        {
            FileInfo fi = new FileInfo(strPath);
            return fi.Exists;
        }

        private static void SetOwner(Part part)
        {
            //foreach (Part child in part.Parts)
            //{
            //    SetOwner(child);
            //    child.SetOwner(part);
            //}
        }

        private static List<string> LoadModuleList()
        {
            string strTitle = "";
            string strModuleListPath = "";
            List<string> listTitle = new List<string>();

            strModuleListPath = ConfigManager.GetModuleListFilePath();

            if (VerifyFile(strModuleListPath))
            {
                using (StreamReader Reader = new StreamReader(strModuleListPath))
                {
                    while (true)
                    {
                        strTitle = Reader.ReadLine();
                        if (strTitle == null)
                            break;
                        listTitle.Add(strTitle);
                    }
                }
            }


            return listTitle;
        }


        public static List<MotionAxis> GetAllMotionAxisList()
        {
            List<MotionAxis> listMotions = new List<MotionAxis>();

            foreach (MotionBoard board in MotionBoards)
            {
                for (int i = 0; i < board.GetAxisCount(); i++)
                {
                    MotionAxis axis = board.GetAxis(i);
                    listMotions.Add(axis);
                }
            }

            return listMotions;
        }


        public static List<DioPoint> GetAllDioPointList()
        {
            List<DioPoint> listPoints = new List<DioPoint>();

            foreach (AjinAxlIoBoard board in IOBoards)
            {
                foreach (AjinAxlDioModule module in board.Modules)
                {
                    for (int i = 0; i < module.GetPointCount(); i++)
                    {
                        DioPoint point = module.GetPoint(i) as DioPoint;
                        listPoints.Add(point);
                    }
                }
            }

            return listPoints;
        }

        #region InitializeSequence
        public static void LoadInitializeSequence()
        {
            InitializeSequenceCollection initializes;
            string strInitializeFilePath = "";
            strInitializeFilePath = ConfigManager.GetInitializeSequenceFilePath();

            if (VerifyFile(strInitializeFilePath))
            {
                using (FileStream fs = new FileStream(strInitializeFilePath, FileMode.Open))
                {
                    SaveManager.BinaryDeserialize<InitializeSequenceCollection>(fs, out initializes);
                    InitializeSequence = initializes;
                }
            }
            else
            {
                string strBeforeModule = string.Empty;
                string strPart = string.Empty;
                InitializeSequence.Clear();

                foreach (Module module in Modules)
                {
                    InitializeSequence initialize = new InitializeSequence(module.Name);
                    if (!string.IsNullOrEmpty(strBeforeModule) && !string.IsNullOrEmpty(strPart))
                    {
                        initialize.Condition.Name = strBeforeModule;
                        initialize.Condition.Sub = strPart;
                    }
                    foreach (Part part in module.Parts)
                    {
                        initialize.Sequence.Add(new SequenceItem(part.Name));
                        strPart = part.Name;
                    }
                    InitializeSequence.Add(initialize);
                    strBeforeModule = module.Name;

                }
            }
        }

        public static void SaveInitializeSequence()
        {
            string strInitializeFilePath = "";
            strInitializeFilePath = ConfigManager.GetInitializeSequenceFilePath();

            FileInfo fi = new FileInfo(strInitializeFilePath);
            if (fi.Exists)
            {
                fi.Delete();
            }

            using (FileStream fs = new FileStream(strInitializeFilePath, FileMode.OpenOrCreate))
            {
                SaveManager.BinarySerialize(fs, InitializeSequence);
            }
        }

        public static List<Part> GetPartList()
        {
            List<Part> listPart = new List<Part>();
            if (listPart != null)
            {
                //listPart.Add(new Module("NewModule"));
                //listPart.Add(new Part());
                //listPart.Add(new GrabLinkMultiCamCamera());
                //listPart.Add(new DigitalIlluminator());
                //listPart.Add(new Stage());
                //listPart.Add(new TwoChipAlinger());
                //listPart.Add(new ChipFinder());
                //listPart.Add(new NeedleBlock());
                //listPart.Add(new Turret());
            }

            return listPart;
        }

        public static int Initialize()
        {
            int ret = 0;

            for (int i = 0; i < InitializeSequence.Count; i++)
            {
                foreach (Module module in Modules)
                {
                    if (InitializeSequence[i].Owner == module.Name)
                    {
                        module.Initialize();
                    }
                }
            }

            return ret;
        }
        #endregion

        #region Config

        public static void LoadConfig()
        {
            ConfigParameters config;
            string strConfigFilePath = "";
            strConfigFilePath = ConfigManager.GetConfigFilePath();

            SaveManager.Load<ConfigParameters>(strConfigFilePath, out config);
            if (config == null)
            {
                config = new ConfigParameters();
            }

            DataManager.Instance.Config = config;
            ApplyConfigData();

        }

        public static void LoadConfig(string m_strRecipeName)
        {
            ConfigParameters config;
            string strConfigFilePath = "";
            strConfigFilePath = ConfigManager.GetConfigFilePath(m_strRecipeName);

            SaveManager.Load<ConfigParameters>(strConfigFilePath, out config);
            if (config == null)
            {
                config = new ConfigParameters();
            }

            DataManager.Instance.Config = config;
            ApplyConfigData();

        }

        //ublic static void LoadConfig()
        //{
        //    ConfigParameters config;
        //    string strConfigFilePath = "";
        //    strConfigFilePath = ConfigManager.GetConfigFilePath();
        //
        //    if (VerifyFile(strConfigFilePath))
        //    {
        //        using (FileStream fs = new FileStream(strConfigFilePath, FileMode.Open))
        //        {
        //            SaveManager.BinaryDeserialize<ConfigParameters>(fs, out config);
        //        }
        //    }
        //    else
        //    {
        //        config = new ConfigParameters();
        //    }

        //    DataManager.Instance.Config = config;
        //    ApplyConfigData();

        //}

        public static void ApplyConfigData()
        {
            DataManager.Instance.UpdateConfigParameters(Modules);
        }

        public static void ApplyRecipeData()
        {
            foreach (Module module in Modules)
            {
                if (m_CurrentRecipe != null)
                {
                    m_CurrentRecipe.ApplyRecipeData(module);
                }
            }
        }

        public static void UpdateRecipeData()
        {
            foreach (Module module in Modules)
            {
                if (m_CurrentRecipe != null)
                {
                    m_CurrentRecipe.SaveRecipeData(module);
                }
            }
        }

        //public static void SaveConfig()
        //{
        //    string strConfigFilePath = "";
        //    strConfigFilePath = ConfigManager.GetConfigFilePath();

        //    FileInfo fi = new FileInfo(strConfigFilePath);
        //    if (fi.Exists)
        //    {
        //        fi.Delete();
        //    }

        //    using (FileStream fs = new FileStream(strConfigFilePath, FileMode.OpenOrCreate))
        //    {
        //        SaveManager.BinarySerialize(fs, DataManager.Instance.Config);
        //    }
        //}
        public static void SaveConfig()
        {
            string strConfigFilePath = "";
            strConfigFilePath = ConfigManager.GetConfigFilePath();

            SaveManager.Save(strConfigFilePath, ConfigManager.GetConfigBackupFilePath(), DataManager.Instance.Config);
        }

        public static void SaveConfig(string m_strRecipeName)
        {
            string strConfigFilePath = "";
            strConfigFilePath = ConfigManager.GetConfigFilePath(m_strRecipeName);

            SaveManager.Save(strConfigFilePath, ConfigManager.GetConfigBackupFilePath(), DataManager.Instance.Config);
        }
        #endregion

        #region Recipe
        //public static void LoadRecipe()
        //{
        //    RecipeInfoCollection recipes;
        //    string strRecipeFilePath = "";
        //    strRecipeFilePath = ConfigManager.GetRecipeFilePath();

        //    if (VerifyFile(strRecipeFilePath))
        //    {
        //        using (FileStream fs = new FileStream(strRecipeFilePath, FileMode.Open))
        //        {
        //            SaveManager.BinaryDeserialize<RecipeInfoCollection>(fs, out recipes);
        //        }
        //    }
        //    else
        //    {
        //        recipes = new RecipeInfoCollection();
        //    }
        //    DataManager.Instance.Recipe = recipes;

        //}
        public static void LoadRecipe()
        {
            RecipeInfoCollection recipes;
            SaveManager.LoadRecipe(out recipes);

            DataManager.Instance.Recipe = recipes;

        }
        public static void SaveRecipe()
        {
            DataManager.Instance.Recipe.Header.CurrentRecipeName = m_CurrentRecipe.Name;
            SaveManager.SaveRecipe(DataManager.Instance.Recipe);
        }


        public static void CreateRecipes(RecipeInfo recipe)
        {
            foreach (Module module in Modules)
            {
                module.CreateRecipe(recipe);
            }
        }

        public static void SetCurrentRecipe(RecipeInfo recipe)
        {
            m_CurrentRecipe = recipe;

            ApplyRecipeData();

        }

        public static RecipeInfo GetCurrentRecipe()
        {
            return m_CurrentRecipe;
        }
        #endregion


        public static bool NewForm_AxisParameter_Load()
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

            //  Axis Parameter 로드
            for (int i = 0; i < Equipment.Max_Axis; i++)
            {
                strTemp = string.Format("Axis_{0}_Limit", i);
                //  Limit Sensor 설치 여부 (Not Installed, Installed)
                NativeMethods.GetPrivateProfileString(strTemp, "Install", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].LimitSensor_Installed = Convert.ToInt16(temp.ToString());
                //  Limit Sensor 동작 레벨 (Low, High)
                NativeMethods.GetPrivateProfileString(strTemp, "ActiveLevel", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].LimitSensor_ActiveLevel = Convert.ToInt16(temp.ToString());

                strTemp = string.Format("Axis_{0}_Home", i);
                //  Home Sensor 형태 (Home, -Limit, +Limit)
                NativeMethods.GetPrivateProfileString(strTemp, "SensingType", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Sensing = Convert.ToInt16(temp.ToString());
                //  Home Sensor 설치 여부 (Not Installed, Installed)
                NativeMethods.GetPrivateProfileString(strTemp, "Install", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Installed = Convert.ToInt16(temp.ToString());
                //  Home Sensor 동작 레벨 (Low, High)
                NativeMethods.GetPrivateProfileString(strTemp, "ActiveLevel", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_ActiveLevel = Convert.ToInt16(temp.ToString());
                //  Home Sensor 동작 방향 (Negative, Positive)
                NativeMethods.GetPrivateProfileString(strTemp, "Direction", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Direction = Convert.ToInt16(temp.ToString());
                //  Home 1st Speed
                NativeMethods.GetPrivateProfileString(strTemp, "1stSpeed", "30", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_1st = Convert.ToDouble(temp.ToString());
                //  Home 2nd Speed
                NativeMethods.GetPrivateProfileString(strTemp, "2ndSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_2nd = Convert.ToDouble(temp.ToString());
                //  Home 3rd Speed
                NativeMethods.GetPrivateProfileString(strTemp, "3rdSpeed", "5", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_3rd = Convert.ToDouble(temp.ToString());
                //  Home Last Speed
                NativeMethods.GetPrivateProfileString(strTemp, "LastSpeed", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_Last = Convert.ToDouble(temp.ToString());
                //  Home Offset
                NativeMethods.GetPrivateProfileString(strTemp, "Offset", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Offset = Convert.ToDouble(temp.ToString());

                strTemp = string.Format("Axis_{0}_Common", i);
                //  Unit Per Pulse (Unit)
                NativeMethods.GetPrivateProfileString(strTemp, "Unit", "1.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_UnitPerPulse_Unit = Convert.ToDouble(temp.ToString());
                //  Unit Per Pulse (Pulse)
                NativeMethods.GetPrivateProfileString(strTemp, "Pulse", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse = Convert.ToDouble(temp.ToString());
                //  Acceleration Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinAcc", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Min = Convert.ToDouble(temp.ToString());
                //  Acceleration Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxAcc", "10000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Max = Convert.ToDouble(temp.ToString());
                //  Acceleration
                NativeMethods.GetPrivateProfileString(strTemp, "Acceleration", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration = Convert.ToDouble(temp.ToString());
                //  Speed Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Min = Convert.ToDouble(temp.ToString());
                //  Speed Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxSpeed", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Max = Convert.ToDouble(temp.ToString());
                //  Move Speed
                NativeMethods.GetPrivateProfileString(strTemp, "MoveSpeed", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_MoveSpeed = Convert.ToDouble(temp.ToString());
                //  Position Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinPos", "-1.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Position_Min = Convert.ToDouble(temp.ToString());
                //  Position Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxPos", "500.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Position_Max = Convert.ToDouble(temp.ToString());
                //  Settle Delay Time
                NativeMethods.GetPrivateProfileString(strTemp, "SettleDelay", "30", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Settle_Delay = Convert.ToDouble(temp.ToString());

                strTemp = string.Format("Axis_{0}_Jog", i);
                //  Jog Speed, Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_Speed_Fine = Convert.ToDouble(temp.ToString());
                //  Jog Speed, Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseSpeed", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_Speed_Coarse = Convert.ToDouble(temp.ToString());
                //  Jog StepSize, Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinStepSize", "0.0001", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Min = Convert.ToDouble(temp.ToString());
                //  Jog StepSize, Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxStepSize", "500.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Max = Convert.ToDouble(temp.ToString());
                //  Jog StepSize, Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineStepSize", "0.001", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Fine = Convert.ToDouble(temp.ToString());
                //  Jog StepSize, Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseStepSize", "0.1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Coarse = Convert.ToDouble(temp.ToString());
            }

            return m_bRet;
        }
    }
}
