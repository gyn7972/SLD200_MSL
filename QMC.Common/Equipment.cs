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
using static QMC.Common.Modules.Loader;

namespace QMC.Common
{
    public static class Equipment
    {
        public static double ToDouble(string str)
        {
            double dValue = 0.0;            
            try
            {
                double.TryParse(str, out dValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return dValue;
        }

        public static int ToInt(string str)
        {
            int nValue = 0;
            try
            {
                int.TryParse(str, out nValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return nValue;
        }

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
        public static void SetWaterLeakStatus(bool m_bLeak)
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
            public double Home_Clear_Time;                  //  Home Clear Time 
            public int Home_ZPhase_Use;                     //  Home Z Phase 사용 여부
            public double Home_Offset;                      //  Home Offset
            public double Home_Acceleration_1st;            //  Home 1st Acceleration
            public double Home_Acceleration_2nd;            //  Home 2nd Acceleration

            public double Common_UnitPerPulse_Unit;         //  Unit Per Pulse (Unit)
            public int Common_UnitPerPulse_Pulse;           //  Unit Per Pulse (Pulse)
            public double Common_Settle_Delay;              //  Settle Delay Time
            public double Common_Acceleration_Min;          //  Acceleration Min
            public double Common_Acceleration_Max;          //  Acceleration Max
            public double Common_Acceleration_Fine;         //  Acceleration Fine
            public double Common_Acceleration_Coarse;       //  Acceleration Coarse
            public double Common_Speed_Min;                 //  Speed Min
            public double Common_Speed_Max;                 //  Speed Max
            public double Common_Speed_Fine;                //  Move Speed Fine
            public double Common_Speed_Coarse;              //  Move Speed Coarse
            public double Common_Position_Min;              //  Position Min
            public double Common_Position_Max;              //  Position Max

            public double Jog_Speed_Fine;                   //  Jog Speed, Fine
            public double Jog_Speed_Coarse;                 //  Jog Speed, Coarse
            public double Jog_StepSize_Min;                 //  Jog StepSize, Min
            public double Jog_StepSize_Max;                 //  Jog StepSize, Max
            public double Jog_StepSize_Fine;                //  Jog StepSize, Fine
            public double Jog_StepSize_Coarse;              //  Jog StepSize, Coarse
        }
        public static stAxisParameter[] stAxisParam = new stAxisParameter[Max_Axis];                  //  총 14개 축. 가변 가능하도록 변경해야 함. (시간 관계상 고정하자)


        //  Communication 장치 파라미터
        public enum CommList : int
        {
            Illuminator = 0,
            PowerMeter_BDS,
            PowerMeter_Stage,
            MotorizedBeamExpander,
            DustCollector_Upper,
            DustCollector_Lower,
            ElectroPneumaticRegulator,
            Laser,
            LaserHeightSensor,
        }

        public struct stCommParameter
        {
            public int Comm_Type;                           //  TCP/IP, RS232

            //  프로그램 시작 시 자동으로 연결할 것인지
            public bool Connect;                            //  Connect or Not

            //  for TCP/IP
            public int TCPIP_PortType;                      //  TCP/IP 의 포트 형식 (Server, Client)
            public string TCPIP_IPAddress;                  //  TCP/IP 의 IP 주소
            public int TCPIP_PortNum;                       //  TCP/IP 의 Port 번호

            //  for RS232
            public int Serial_CommTimeout;                  //  Timeout (ms)
            public int Serial_CommSpacingDelay;             //  Spacing Delay (ms)
            public int Serial_CommPort;                     //  COM Port
            public int Serial_CommBaudRate;                 //  Baud Rate
            public int Serial_CommDataBits;                 //  Data Bits
            public int Serial_CommStopBits;                 //  Stop Bits
            public int Serial_CommParity;                   //  Parity
            public int Serial_CommFlowControl;              //  Flow Control
        }
        public static stCommParameter[] stCommunicationSet = new stCommParameter[System.Enum.GetValues(typeof(CommList)).Length];


        //  Recipe 파라미터
        public enum LayerList : int
        {
            Hole1 = 0,
            Hole2,
            Hole3,
            Hole4,
            Rect,
            Outline,
            Marking,
            Fiducial,
            Thruhole,
        }

        public enum HoleProcessingType : int
        {
            Circle = 0,
            Spiral,
        }

        public struct stLayerRecipeParameter
        {
            public string DrawingFile;                                  //  Drawing File Path and Name

            public int LaserParam_PulseWidth;                           //  Laser Pulse Width (us)
            public int LaserParam_PulsePeriod;                          //  Laser Pulse Period (us)
            public int LaserParam_Frequency;                            //  Laser Frequency (Hz)
            public double LaserParam_DutyCycle;                         //  Laser Duty Cycle (%)
            public bool LaserParam_TriggerMode_External;                //  Laser Trigger Mode (true: External, false: Internal)

            public bool ProcessPriority_P2P;                            //  Process Priority (true: Space of P2P, false: Pulse Period)

            public string Miscellaneous_ReferenceLayer;                 //  Reference Layer
            public double Miscellaneous_DefocusingDistance;             //  Defocusing Distance (mm)
            public double Miscellaneous_Resizing;                       //  Resizing (mm)
            public int Miscellaneous_HoleDrilling_StartPosDivision;     //  Hole Drilling Start Position Division(등분)
            public double Miscellaneous_GroupSplitSize;                 //  Group Split Size (mm)
            public double Miscellaneous_ScannerDrillingSpeed;           //  Scanner Drilling Speed (mm/s)
            public double Miscellaneous_ScannerJumpSpeed;               //  Scanner Jump Speed (mm/s)
            public double Miscellaneous_LaserOnDelay;                   //  Laser On Delay (us)
            public double Miscellaneous_LaserOffDelay;                  //  Laser Off Delay (us)
            public double Miscellaneous_MarkDelay;                      //  Mark Delay (us)
            public double Miscellaneous_JumpDelay;                      //  Jump Delay (us)
            public double Miscellaneous_PolygonDelay;                   //  Polygon Delay (us)
            public double Miscellaneous_Drilling_Power;                 //  Drilling Power
            public int Miscellaneous_DrillingRepetition;                //  Drilling Repetition
            public int Miscellaneous_DrillingRepetitionBundle;          //  Drilling Repetition Bundle
            public double Miscellaneous_RotationAngleArc;               //  Rotation Angle Arc (degree)
            public double Miscellaneous_P2PDistance;                    //  P2P Distance (mm)
            public int Miscellaneous_MaskIndex;                         //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
            public int Miscellaneous_BETPositionIndex;                  //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
            public int Miscellaneous_HoleProcessingType;                //  Hole Processing Type (0:Circle, 1:Spiral)

            public bool ProcessOption_SocketAlign_Use;                  //  Socket Align Use (true: Use, false: Not Use)
            public bool ProcessOption_SocketHeightCheck_Use;            //  Socket Height Check Use Offset (true: Use, false: Not Use)

            public double ModuleInformation_Module_Width;               //  Module Width (mm)
            public double ModuleInformation_Module_Height;              //  Module Height (mm)
            public double ModuleInformation_Silicon_Thickness;          //  Silicon Thickness (mm)

            public double SpiralParam_OuterDiameter;                    //  Spiral Outer Diameter (mm)
            public double SpiralParam_InnerDiameter;                    //  Spiral Inner Diameter (mm)
            public double SpiralParam_Revolutions;                      //  Spiral Revolutions
            public double SpiralParam_AngleFactor;                      //  Spiral Angle Factor

            public bool MAligner_VacuumPos_Center;                      //  M-Aligner Vacuum Position Center (true: Using, false: Not Using)
            public bool MAligner_VacuumPos_Inner;                       //  M-Aligner Vacuum Position Inner (true: Using, false: Not Using)
            public bool MAligner_VacuumPos_Outer;                       //  M-Aligner Vacuum Position Outer (true: Using, false: Not Using)

            public int IlluminatorValue_FineCamRed;                     //  Illuminator Value (Fine Camera, Red)                            //  조명은 0번 index 만 사용
            public int IlluminatorValue_FineCamIR;                      //  Illuminator Value (Fine Camera, IR)                             //  조명은 0번 index 만 사용
            public int IlluminatorValue_CoarseCamIR;                    //  Illuminator Value (Coarse Camera, IR)                           //  조명은 0번 index 만 사용
        }
        public static stLayerRecipeParameter[] stLayerRecipeSet = new stLayerRecipeParameter[System.Enum.GetValues(typeof(LayerList)).Length];


        //  Machine Name
        public static string Machine_Name { set; get; } = "SLD-200";


        //  Laser Type
        public static bool Machine_LaserType_CO2 { set; get; }


        //  Options
        public static bool Machine_Door_Enable { set; get; } = true;                            //  Door Enable (true: Enable, false: Disable)
        public static bool Machine_VacuumSensor_Enable { set; get; } = true;                    //  Vacuum Sensor Enable (true: Enable, false: Disable)
        public static int Machine_SignalHoldTime { set; get; } = 500;                           //  Signal Hold Time (ms)
        public static bool Machine_MAligner_ReleaseType { set; get; } = true;                   //  M-Aligner Release Timing (true: Release -> Up, false: Up -> Release)
        public static double Machine_MAligner_NarrowingDistance { set; get; } = 0.5;            //  M-Aligner Narrowing Distance (mm)
        public static double Machine_MAligner_WidenDistance { set; get; } = 2.0;                //  M-Aligner Widen Distance (mm)
        public static bool Machine_VacuumStableTime_Enable { set; get; } = true;                //  Vacuum Stabilization Time Enable
        public static int Machine_VacuumStableTime { set; get; } = 500;                         //  Vacuum Signal Stabilization Time (ms)
        public static bool Machine_LaserHeightCheckStableTime_Enable { set; get; } = true;      //  Laser Height Check Stabilization Time Enable
        public static int Machine_LaserHeightCheckStableTime { set; get; } = 500;               //  Laser Height Check Stabilization Time (ms)
        public static double Machine_Stacker_TopCheck_OverDistance { set; get; } = 0.2;         //  Full Sensor 감지 후 추가로 이동하는 거리 (mm)
        public static bool Machine_FiducialLaserHeightCheckStableTime_Enable { set; get; } = true;      //  Laser Height Check Stabilization Time Enable
        public static bool Machine_FiducialMarkJudgementRange_Enable { set; get; } = true;      //  FIducial Mark Judgement Enable
        public static double Machine_FiducialMarkJudgementRange { set; get; } = 0.1;            //  Fiducial Mark Judgement Range (mm)
        public static bool Machine_FiducialImageSave_Always { set; get; } = false;              //  Fiducial Image Save Always


        //  Offset Distance
        public struct stOffsetDistanceParameter
        {
            public PointD FromScannerToFineCam;             //  Scanner to Fine Camera
            public PointD FromFineCamToCoarseCam;           //  Fine Camera to Coarse Camera
            public PointD FromFineCamToLaserHeightSensor;   //  Fine Camera to Laser Height Sensor (Keyence)
        }
        public static stOffsetDistanceParameter stOffsetDistance = new stOffsetDistanceParameter();


        //  소켓 얼라인 테스트 확인용
        public static double m_dTest_SocketAlign_OffsetX { set; get; } = 0.0;           //  Socket Align Offset X
        public static double m_dTest_SocketAlign_OffsetY { set; get; } = 0.0;           //  Socket Align Offset Y
        public static double m_dTest_SocketAlign_Theta { set; get; } = 0.0;             //  Socket Align Theta (degree)


        //  Scanner Head Offset
        public static double Scanner_HeadOffset_X { set; get; }
        public static double Scanner_HeadOffset_Y { set; get; }
        public static double Scanner_HeadOffset_Angle { set; get; }


        //  Coordinate System Matching Offset (Stage Origin Pos. to Scanner Center Pos.)
        public static double CoordinateMatchingOffset_X { set; get; }
        public static double CoordinateMatchingOffset_Y { set; get; }


        //  Offset distance from the stage to the scanner position (스테이지와 스캐너 좌표계를 일치시키지 않는다면, 이 값만큼 이동해서 가공해야 함) - 스테이지 스캐너 좌표계를 일치시키면 이 값은 반드시 0 으로 설정해야 함.
        public static double StageOffset_forDrilling_X { set; get; }
        public static double StageOffset_forDrilling_Y { set; get; }


        //  Keyence Laser Height Sensor 기준값 설정
        public static double LaserHeightSensor_ReferenceValue_atVisionFocusPosition { set; get; } = 0.0;         //  Vision Focus 위치에서의 Keyence Laser Height Sensor 기준값
        public static double LaserHeightSensor_ReferenceValue_atScannerFocusPosition { set; get; } = 0.0;        //  Scanner Focus 위치에서의 Keyence Laser Height Sensor 기준값


        //  Scanner Calibration Parameter
        public static double Scanner_Calibration_LaserFrequency { set; get; } = 0.0;            //  Scanner Calibration Laser Frequency
        public static double Scanner_Calibration_LaserEnergy { set; get; } = 0.0;               //  Scanner Calibration Laser Energy
        public static double Scanner_Calibration_CrossMarkLength { set; get; } = 0.0;           //  Scanner Calibration Cross Mark Length
        public static double Scanner_Calibration_LaserMarkSpeed { set; get; } = 0.0;            //  Scanner Calibration Laser Mark Speed (mm/s)
        public static double Scanner_Calibration_LaserJumpSpeed { set; get; } = 0.0;            //  Scanner Calibration Laser Jump Speed (mm/s)
        public static double Scanner_Calibration_LaserOnDelay { set; get; } = 0.0;                //  Scanner Calibration Laser On Delay (us)
        public static double Scanner_Calibration_LaserOffDelay { set; get; } = 0.0;               //  Scanner Calibration Laser Off Delay (us)
        public static double Scanner_Calibration_MarkDelay { set; get; } = 0.0;                  //  Scanner Calibration Mark Delay (us)
        public static double Scanner_Calibration_JumpDelay { set; get; } = 0.0;                  //  Scanner Calibration Jump Delay (us)
        public static double Scanner_Calibration_PolygonDelay { set; get; } = 0.0;               //  Scanner Calibration Polygon Delay (us)


        public static string Scanner_Calibration_srcFilePath { set; get; } = "";            //  Scanner Calibration Source File Path
        public static string Scanner_Calibration_targetFilePath { set; get; } = "";            //  Scanner Calibration Destination File Path
        public static float Scanner_Calibration_FieldSize { set; get; } = 0;            //  Scanner Calibration Field Size (mm)
        public static float Scanner_Calibration_rowInterval { set; get; } = 0;
        public static float Scanner_Calibration_colInterval { set; get; } = 0;
        public static int Scanner_Calibration_rowCount { set; get; } = 0;
        public static int Scanner_Calibration_colCount { set; get; } = 0;



        //  Mapping Data 파일 경로
        public static string MappingData_FilePath_Stage_Scanner { set; get; }
        public static string MappingData_FilePath_Stage_FineCam { set; get; }
        public static string MappingData_FilePath_StageCal_Scanner { set; get; }
        public static string MappingData_FilePath_StageCal_FineCam { set; get; }



        //  현재 Recipe (저장했거나 불러왔거나)
        public static string Current_Recipe { set; get; }


        //  메인 화면에서 Open 하려는 Recipe 이름
        public static bool RecipeOpen_fromMainForm { set; get; }
        public static string RecipeName_fromMainForm { set; get; }


        //  자동운전 상태 확인
        public static bool AutoRunStatus { set; get; }
        public static int DryRun_ProcessingTime { set; get; } = 5;


        //  Sequence Test 일 경우
        public static bool SeqTestMode { set; get; } = false;


        //  Loader Port 투입 일시정지
        public static bool Loader_LPort_Pause { set; get; } = false;
        public static bool Loader_RPort_Pause { set; get; } = false;


        //  Recipe Open 시 열린 도면 파일 경로
        public static string RecipeOpen_DrawingFilePath { set; get; } = "";



        ////  Recipe Data
        //public struct stRecipeParameter
        //{
        //    public string DrawingFile;                                  //  Drawing File Path and Name

        //    public int LaserParam_PulseWidth;                           //  Laser Pulse Width (us)
        //    public int LaserParam_PulsePeriod;                          //  Laser Pulse Period (us)
        //    public int LaserParam_Frequency;                            //  Laser Frequency (Hz)
        //    public bool LaserParam_TriggerMode_External;                //  Laser Trigger Mode (true: External, false: Internal)

        //    public bool ProcessPriority_P2P;                            //  Process Priority (true: Space of P2P, false: Pulse Period)

        //    public string Miscellaneous_ReferenceLayer;                 //  Reference Layer
        //    public double Miscellaneous_DefocusingDistance;             //  Defocusing Distance (mm)
        //    public double Miscellaneous_Resizing;                       //  Resizing (mm)
        //    public int Miscellaneous_HoleDrilling_StartPosDivision;     //  Hole Drilling Start Position Division (등분)
        //    public double Miscellaneous_GroupSplitSize;                 //  Group Split Size (mm)
        //    public double Miscellaneous_ScannerDrillingSpeed;           //  Scanner Drilling Speed (mm/s)
        //    public double Miscellaneous_ScannerJumpSpeed;               //  Scanner Jump Speed (mm/s)
        //    public double Miscellaneous_LaserOnDelay;                   //  Laser On Delay (us)
        //    public double Miscellaneous_LaserOffDelay;                  //  Laser Off Delay (us)
        //    public double Miscellaneous_MarkDelay;                      //  Mark Delay (us)
        //    public double Miscellaneous_JumpDelay;                      //  Jump Delay (us)
        //    public double Miscellaneous_PolygonDelay;                   //  Polygon Delay (us)
        //    public int Miscellaneous_DrillingRepetation;                //  Drilling Repetation
        //    public double Miscellaneous_P2PDistance;                    //  P2P Distance (mm)
        //    public int Miscellaneous_MaskIndex;                         //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
        //    public double Miscellaneous_BETPositionIndex;               //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
        //}
        //public static stRecipeParameter stRecipeSet = new stRecipeParameter();


        //  Vision Popup 창 Open 모드 (true: Scanner FineCam Offset Change)
        public static bool m_bVisionFormOpenMode_ScannerFineCamOffsetChange { set; get; }


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

        public static bool MapDataStatus_Activate { set; get; }                   //  맵 데이터 활성화 / 비활성화

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
        public static bool User_LogOut_1time { set; get; }


        //  자동 로그아웃
        public static bool LogIn_Status { set; get; }
        public static bool AutoLogOut_Execute { set; get; }
        public static bool AutoLogOut_Executed { set; get; }


        //  바코드 리더 Comm 1번만
        public static bool m_bBarcodeReaderComm_1time { set; get; }


        public static SiriusViewerForm EqpSiriusViewer { set; get; }
        public static SiriusViewerForm EqpSiriusViewer_Origin { set; get; }                     //  모듈 생산 완료 후, 다음 모듈이 투입될 때 이 데이터로 재설정
        public static bool m_bAlignVisionThread_1time { set; get; }
        public static bool m_bParamLoadThread_1time { set; get; }

        public static bool m_bDrawingFileOpen_1time { set; get; } = false;                      //  도면 파일 Open 시 1회만 실행하기 위한 Flag


        //  메인화면에 Process 상태를 표시하기 위한 변수
        public static bool m_bMainProcessStatus_LD_LPort_Complete { set; get; } = false;         //  Loader LPort 투입 완료
        public static bool m_bMainProcessStatus_LD_RPort_Complete { set; get; } = false;         //  Loader RPort 투입 완료
        public static bool m_bMainProcessStatus_LD_Module_PortPickUp_Complete { set; get; } = false;        //  Loader Port 에서 Module Pick Up 완료
        public static bool m_bMainProcessStatus_LD_Module_MAlignerPutDown_Complete { set; get; } = false;   //  Loader M-Aligner 에 Module Put Down 완료
        public static bool m_bMainProcessStatus_LD_M_Aligner_Align_Complete { set; get; } = false;          //  Loader M-Align 완료
        public static bool m_bMainProcessStatus_LD_Module_MAlignerPickUp_Complete { set; get; } = false;    //  Loader M-Aligner 에서 Module Pick Up 완료
        public static bool m_bMainProcessStatus_LD_Module_WorkStagePutDown_Complete { set; get; } = false;  //  Loader Work Stage 에 Module Put Down 완료
        public static bool m_bMainProcessStatus_WorkStage_Module_Process_Complete { set; get; } = false;    //  Work Stage Process 완료
        public static bool m_bMainProcessStatus_UL_Module_WorkStagePickUp_Complete { set; get; } = false;   //  Unloader Work Stage 에서 Module Pick Up 완료
        public static bool m_bMainProcessStatus_UL_Module_PortPutDown_Complete { set; get; } = false;       //  Unloader Port 에 Module Put Down 완료



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
                stAxisParam[i].Home_Acceleration_1st = 0;
                stAxisParam[i].Home_Acceleration_2nd = 0;
                stAxisParam[i].Common_UnitPerPulse_Unit = 0;
                stAxisParam[i].Common_UnitPerPulse_Pulse = 0;
                stAxisParam[i].Common_Acceleration_Min = 0;
                stAxisParam[i].Common_Acceleration_Max = 0;
                stAxisParam[i].Common_Acceleration_Fine = 0;
                stAxisParam[i].Common_Acceleration_Coarse = 0;
                stAxisParam[i].Common_Speed_Min = 0;
                stAxisParam[i].Common_Speed_Max = 0;
                stAxisParam[i].Common_Speed_Fine = 0;
                stAxisParam[i].Common_Speed_Coarse = 0;
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

            //  Communication 장치 파라미터 초기화
            for (int i = 0; i < System.Enum.GetValues(typeof(CommList)).Length; i++)
            {
                stCommunicationSet[i].Comm_Type = 0;                        //  0 : TCP/IP,         1 : RS232
                stCommunicationSet[i].Connect = false;                      //  0 : Not Connect,    1 : Connect
                stCommunicationSet[i].TCPIP_PortType = 0;                   //  0 : Server,         1 : Client
                stCommunicationSet[i].TCPIP_IPAddress = "127.0.0.1";
                stCommunicationSet[i].TCPIP_PortNum = 5000;
                stCommunicationSet[i].Serial_CommTimeout = 500;
                stCommunicationSet[i].Serial_CommSpacingDelay = 20;
                stCommunicationSet[i].Serial_CommPort = 0;                  //  0 : COM1,           1 : COM2,       2 : COM3,       3 : COM4 ....
                stCommunicationSet[i].Serial_CommBaudRate = 3;              //  0 : 1200,           1 : 2400,       2 : 4800,       3 : 9600,           4 : 19200,      5 : 38400,      6 : 57600,      7 : 115200
                stCommunicationSet[i].Serial_CommDataBits = 3;              //  0 : 5,              1 : 6,          2 : 7,          3 : 8
                stCommunicationSet[i].Serial_CommStopBits = 0;              //  0 : 1,              1 : 1.5,        2 : 2
                stCommunicationSet[i].Serial_CommParity = 0;                //  0 : None,           1 : Odd,        2 : Even
                stCommunicationSet[i].Serial_CommFlowControl = 0;           //  0 : None,           1 : Xon/Xoff,   2 : RTS/CTS
            }

            //  장비 Laser Type
            Machine_LaserType_CO2 = true;

            //  Offset Distance 
            stOffsetDistance.FromScannerToFineCam.X = 0;
            stOffsetDistance.FromScannerToFineCam.Y = 0;
            stOffsetDistance.FromFineCamToCoarseCam.X = 0;
            stOffsetDistance.FromFineCamToCoarseCam.Y = 0;
            stOffsetDistance.FromFineCamToLaserHeightSensor.X = 0;
            stOffsetDistance.FromFineCamToLaserHeightSensor.Y = 0;


            //  Layer Recipe 파라미터 초기화
            for (int i = 0; i < System.Enum.GetValues(typeof(LayerList)).Length; i++)
            {
                //  Drawing File
                stLayerRecipeSet[i].DrawingFile = "";                                //  Drawing File Path and Name

                //  Laser Parameter
                stLayerRecipeSet[i].LaserParam_PulseWidth = 0;                      //  Laser Pulse Width (us)
                stLayerRecipeSet[i].LaserParam_PulsePeriod = 0;                     //  Laser Pulse Period (us)
                stLayerRecipeSet[i].LaserParam_Frequency = 0;                       //  Laser Frequency (Hz)
                stLayerRecipeSet[i].LaserParam_DutyCycle = 0;                       //  Laser Duty Cycle (%)
                stLayerRecipeSet[i].LaserParam_TriggerMode_External = false;        //  Laser Trigger Mode (true: External, false: Internal)

                //  Process Priority
                stLayerRecipeSet[i].ProcessPriority_P2P = true;                     //  Process Priority (true: Space of P2P, false: Pulse Period)

                //  Miscellaneous
                stLayerRecipeSet[i].Miscellaneous_ReferenceLayer = "";                              //  어떤 Layer 의 데이터를 사용할 것인지
                stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = 0.0;                         //  가공 시 초점 위치에서 얼마나 이동해서 가공할 것인지
                stLayerRecipeSet[i].Miscellaneous_Resizing = 0.0;                                   //  가공 시 데이터를 얼마나 확대/축소할 것인지 (전체 길이를 입력하면 2등분 하여 양방향으로 크기 조정)
                stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = 1;                //  Hole Drilling 가공 시 시작 위치를 몇개로 나눌 것인지 (Only 1, 2, 3, 4, 5, 6, 8, 9, 10, 12)
                stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = 4.0;                             //  Group 분할 크기 (mm, default : 4mm)
                stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = 10;                        //  Hole Drilling 속도 (mm/s)
                stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed = 100;                           //  Jump 속도 (mm/s)  
                stLayerRecipeSet[i].Miscellaneous_LaserOnDelay = 0;                                 //  Laser On Delay (us)
                stLayerRecipeSet[i].Miscellaneous_LaserOffDelay = 0;                                //  Laser Off Delay (us)
                stLayerRecipeSet[i].Miscellaneous_MarkDelay = 0;                                    //  Mark Delay (us)
                stLayerRecipeSet[i].Miscellaneous_JumpDelay = 0;                                    //  Jump Delay (us)
                stLayerRecipeSet[i].Miscellaneous_PolygonDelay = 0;                                 //  Polygon Delay (us)
                stLayerRecipeSet[i].Miscellaneous_Drilling_Power = 1;                               //  Drilling Power (w)
                stLayerRecipeSet[i].Miscellaneous_P2PDistance = 0.1;                                //  P2P Distance (mm)
                stLayerRecipeSet[i].Miscellaneous_DrillingRepetition = 1;                           //  Drilling 반복 횟수
                stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = 50;                    //  Drilling 반복 묶음 횟수
                stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = 360.0;                         //  Rotation Angle Arc (degree)
                stLayerRecipeSet[i].Miscellaneous_MaskIndex = 0;                                    //  Mask Index  
                stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = 0;                             //  BET Index  
                stLayerRecipeSet[i].Miscellaneous_Drilling_Power = 10;                              //  Drilling Power              
                stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = 0;                           //  Hole Processing Type (0:Circle, 1:Spiral)

                //  Process Options
                stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = false;                          //  Socket Align Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = false;                    //  Socket Height Check Use Offset (true: Use, false: Not Use)

                //  Module Information
                stLayerRecipeSet[i].ModuleInformation_Module_Width = 0.0;                           //  Module Width (mm)
                stLayerRecipeSet[i].ModuleInformation_Module_Height = 0.0;                          //  Module Height (mm)
                stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = 0.0;                      //  Silicon Thickness (mm)

                //  Spiral Parameter
                stLayerRecipeSet[i].SpiralParam_OuterDiameter = 0.0;                                //  Spiral Outer Diameter Resizing (mm)
                stLayerRecipeSet[i].SpiralParam_InnerDiameter = 0.0;                                //  Spiral Inner Diameter Resizing (mm)
                stLayerRecipeSet[i].SpiralParam_Revolutions = 10.0;                                 //  Spiral Revolutions
                stLayerRecipeSet[i].SpiralParam_AngleFactor = 10.0;                                 //  Spiral Angle Factor

                //  Mechanical-Alignment Vacuum
                stLayerRecipeSet[i].MAligner_VacuumPos_Center = true;                               //  Mechanical-Alignment Center Vacuum Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].MAligner_VacuumPos_Outer = false;                               //  Mechanical-Alignment Outer Vacuum Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].MAligner_VacuumPos_Inner = false;                               //  Mechanical-Alignment Inner Vacuum Use (true: Use, false: Not Use)
            }


            //  Scanner Head Offset
            Scanner_HeadOffset_X = 0;
            Scanner_HeadOffset_Y = 0;
            Scanner_HeadOffset_Angle = 0;


            //  Coordinate System Matching Offset (Stage Origin Pos. to Scanner Center Pos.)
            CoordinateMatchingOffset_X = 0.0;
            CoordinateMatchingOffset_Y = 0.0;


            //  Offset distance from the stage to the scanner position (스테이지와 스캐너 좌표계를 일치시키지 않는다면, 이 값만큼 이동해서 가공해야 함) - 스테이지 스캐너 좌표계를 일치시키면 이 값은 반드시 0 으로 설정해야 함.
            StageOffset_forDrilling_X = 0.0;
            StageOffset_forDrilling_Y = 0.0;


            //  Keyence Laser Height Sensor 기준값 설정
            LaserHeightSensor_ReferenceValue_atVisionFocusPosition = 0.0;         //  Vision Focus 위치에서의 Keyence Laser Height Sensor 기준값
            LaserHeightSensor_ReferenceValue_atScannerFocusPosition = 0.0;        //  Scanner Focus 위치에서의 Keyence Laser Height Sensor 기준값

            Scanner_Calibration_LaserFrequency = 0.0;            //  Scanner Calibration Laser Frequency
            Scanner_Calibration_LaserEnergy = 0.0;               //  Scanner Calibration Laser Energy
            Scanner_Calibration_CrossMarkLength = 0.0;           //  Scanner Calibration Cross Mark Length
            Scanner_Calibration_LaserMarkSpeed = 0.0;            //  Scanner Calibration Laser Mark Speed (mm/s)
            Scanner_Calibration_LaserJumpSpeed = 0.0;            //  Scanner Calibration Laser Jump Speed (mm/s)


            //  자동운전 상태 확인
            AutoRunStatus = false;


            m_bVisionFormOpenMode_ScannerFineCamOffsetChange = false;

            Current_Recipe = "";

            RecipeOpen_fromMainForm = false;
            RecipeName_fromMainForm = "";

            MapDataStatus_Activate = false;


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
            NewForm_CommParameter_Load();
            NewForm_MachineOption_Load();
            NewForm_MapDataList_Load();
            NewForm_MapDataActivate_Load();


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
                        MotionBoardConfiguration.Load(fs, out configuration);
                        MotionBoard board = null;

                        switch (configuration.BoardType)
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

                switch (config.BoardType)
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
                //  Home Clear Time
                NativeMethods.GetPrivateProfileString(strTemp, "ClearTime", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Clear_Time = Convert.ToDouble(temp.ToString());
                //  Home ZPhase Use (Disable, Dir CW, Dir CCW)
                NativeMethods.GetPrivateProfileString(strTemp, "ZPhase", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_ZPhase_Use = Convert.ToInt16(temp.ToString());
                //  Home Offset
                NativeMethods.GetPrivateProfileString(strTemp, "Offset", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Offset = Convert.ToDouble(temp.ToString());
                //  Home 1st Acceleration
                NativeMethods.GetPrivateProfileString(strTemp, "1stAccel", "300", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Acceleration_1st = Convert.ToDouble(temp.ToString());
                //  Home 2nd Acceleration
                NativeMethods.GetPrivateProfileString(strTemp, "2ndAccel", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Acceleration_2nd = Convert.ToDouble(temp.ToString());

                strTemp = string.Format("Axis_{0}_Common", i);
                //  Unit Per Pulse (Unit)
                NativeMethods.GetPrivateProfileString(strTemp, "Unit", "1.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_UnitPerPulse_Unit = Convert.ToDouble(temp.ToString());
                //  Unit Per Pulse (Pulse)
                NativeMethods.GetPrivateProfileString(strTemp, "Pulse", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse = Convert.ToInt16(temp.ToString());
                //  Acceleration Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinAcc", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Min = Convert.ToDouble(temp.ToString());
                //  Acceleration Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxAcc", "10000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Max = Convert.ToDouble(temp.ToString());
                //  Acceleration Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineAcc", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Fine = Convert.ToDouble(temp.ToString());
                //  Acceleration Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseAcc", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Coarse = Convert.ToDouble(temp.ToString());
                //  Speed Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Min = Convert.ToDouble(temp.ToString());
                //  Speed Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxSpeed", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Max = Convert.ToDouble(temp.ToString());
                //  Move Speed Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Fine = Convert.ToDouble(temp.ToString());
                //  Move Speed Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseSpeed", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Coarse = Convert.ToDouble(temp.ToString());
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

        public static bool NewForm_CommParameter_Load()
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

            //  Comm. Parameter 로드
            for (int i = 0; i < System.Enum.GetValues(typeof(CommList)).Length; i++)
            {
                strTemp = string.Format("CommUnit_{0}", i);


                //  TCP/IP, RS232                                                                                   //  0 : TCP/IP,         1 : RS232
                NativeMethods.GetPrivateProfileString(strTemp, "CommType", "1", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Comm_Type = Convert.ToInt16(temp.ToString());


                //  Not Connect, Connect                                                                            //  0 : Not Connect,    1 : Connect
                NativeMethods.GetPrivateProfileString(strTemp, "ConnectType", "False", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Connect = temp.ToString() == "False" ? false : true;


                //  TCP/IP 의 포트 형식 (Server, Client)                                                            //  0 : Server,         1 : Client
                NativeMethods.GetPrivateProfileString(strTemp, "TCPIP_PortType", "1", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].TCPIP_PortType = Convert.ToInt16(temp.ToString());
                //  TCP/IP 의 IP 주소
                NativeMethods.GetPrivateProfileString(strTemp, "TCPIP_IPAddress", "127.0.0.1", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].TCPIP_IPAddress = temp.ToString();
                //  TCP/IP 의 Port 번호
                NativeMethods.GetPrivateProfileString(strTemp, "TCPIP_PortNum", "5000", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].TCPIP_PortNum = Convert.ToInt16(temp.ToString());


                //  Timeout (ms)
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_Timeout", "500", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommTimeout = Convert.ToInt16(temp.ToString());
                //  Spacing Delay (ms)
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_SpacingDelay", "20", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommSpacingDelay = Convert.ToInt16(temp.ToString());
                //  COM Port                                                                                        //  0 : COM1,           1 : COM2,       2 : COM3,       3 : COM4 ....
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_Port", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommPort = Convert.ToInt16(temp.ToString());
                //  Baud Rate                                                                                       //  0 : 1200,           1 : 2400,       2 : 4800,       3 : 9600,           4 : 19200,      5 : 38400,      6 : 57600,      7 : 115200
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_BaudRate", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommBaudRate = Convert.ToInt16(temp.ToString());
                //  Data Bits                                                                                       //  0 : 5,              1 : 6,          2 : 7,          3 : 8
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_DataBit", "3", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommDataBits = Convert.ToInt16(temp.ToString());
                //  Stop Bits                                                                                       //  0 : 1,              1 : 1.5,        2 : 2
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_StopBit", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommStopBits = Convert.ToInt16(temp.ToString());
                //  Parity                                                                                          //  0 : None,           1 : Odd,        2 : Even
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_Parity", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommParity = Convert.ToInt16(temp.ToString());
                //  Flow Control                                                                                    //  0 : None,           1 : Xon/Xoff,   2 : RTS/CTS
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_FlowControl", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommFlowControl = Convert.ToInt16(temp.ToString());
            }

            return m_bRet;
        }


        public static bool NewForm_MachineOption_Load()
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

            //  Machine Option  로드
            //  Machine Name
            NativeMethods.GetPrivateProfileString("Machine_Option", "Machine_Name", "SLD-200", temp, 255, strFIle);
            Equipment.Machine_Name = temp.ToString();

            //  Laser Type                                                                            //  True : CO₂,    False : UV
            NativeMethods.GetPrivateProfileString("Machine_Option", "Laser_Type", "True", temp, 255, strFIle);
            Equipment.Machine_LaserType_CO2 = temp.ToString() == "False" ? false : true;

            //  Options
            NativeMethods.GetPrivateProfileString("Machine_Option", "Door_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_Door_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumSensor_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_VacuumSensor_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumSignalHoldTime", "500", temp, 255, strFIle);
            Equipment.Machine_SignalHoldTime = Convert.ToInt16(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "MAligner_ReleaseType", "True", temp, 255, strFIle);
            Equipment.Machine_MAligner_ReleaseType = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "MAligner_NarrowingDistance", "0.5", temp, 255, strFIle);
            Equipment.Machine_MAligner_NarrowingDistance = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "MAligner_WidenDistance", "2.0", temp, 255, strFIle);
            Equipment.Machine_MAligner_WidenDistance = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumStableTime_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_VacuumStableTime_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumStableTime", "500", temp, 255, strFIle);
            Equipment.Machine_VacuumStableTime = Convert.ToInt16(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LaserHeightCheckStableTime_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_LaserHeightCheckStableTime_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "LaserHeightCheckStableTime", "500", temp, 255, strFIle);
            Equipment.Machine_LaserHeightCheckStableTime = Convert.ToInt16(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "FiducialMarkJudgementRange_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_FiducialMarkJudgementRange_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "FiducialMarkJudgementRange", "0.1", temp, 255, strFIle);
            Equipment.Machine_FiducialMarkJudgementRange = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "FiducialImageSave_Always", "True", temp, 255, strFIle);
            Equipment.Machine_FiducialImageSave_Always = temp.ToString() == "False" ? false : true;

            //  Offset Distance
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_X", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromScannerToFineCam.X = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_Y", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromScannerToFineCam.Y = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_X", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.X = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_Y", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_X", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_Y", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y = Convert.ToDouble(temp.ToString());

            //  Scanner Head Offset
            NativeMethods.GetPrivateProfileString("ScannerHeadOffset", "Offset_X", "0.0", temp, 255, strFIle);
            Equipment.Scanner_HeadOffset_X = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("ScannerHeadOffset", "Offset_Y", "0.0", temp, 255, strFIle);
            Equipment.Scanner_HeadOffset_Y = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("ScannerHeadOffset", "Offset_Angle", "0.0", temp, 255, strFIle);
            Equipment.Scanner_HeadOffset_Angle = Convert.ToDouble(temp.ToString());

            //  Coordinate System Matching Offset
            NativeMethods.GetPrivateProfileString("MachineCoordinateOffset", "Offset_X", "0.0", temp, 255, strFIle);
            Equipment.CoordinateMatchingOffset_X = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("MachineCoordinateOffset", "Offset_Y", "0.0", temp, 255, strFIle);
            Equipment.CoordinateMatchingOffset_Y = Convert.ToDouble(temp.ToString());

            //  Offset Distance from Stage to Scanner
            NativeMethods.GetPrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_X", "0.0", temp, 255, strFIle);
            Equipment.StageOffset_forDrilling_X = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_Y", "0.0", temp, 255, strFIle);
            Equipment.StageOffset_forDrilling_Y = Convert.ToDouble(temp.ToString());

            //  Keyence Laser Height Sensor 기준값 설정
            NativeMethods.GetPrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Vision_Focus_Position", "0.0", temp, 255, strFIle);
            Equipment.LaserHeightSensor_ReferenceValue_atVisionFocusPosition = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Scanner_Focus_Position", "0.0", temp, 255, strFIle);
            Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition = Convert.ToDouble(temp.ToString());

            //  Scanner Calibration parameter
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Frequency", "50.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserFrequency = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Energy", "2.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserEnergy = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "CrossMark_Length", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_CrossMarkLength = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Marking_Speed", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserMarkSpeed = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Jump_Speed", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserJumpSpeed = Convert.ToDouble(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "LaserOn_Delay", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserOnDelay = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "LaserOff_Delay", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserOffDelay = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Mark_Delay", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_MarkDelay = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Jump_Delay", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_JumpDelay = Convert.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Polygon_Delay", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PolygonDelay = Convert.ToDouble(temp.ToString());


            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "srcFilePath", "", temp, 255, strFIle);
            Equipment.Scanner_Calibration_srcFilePath = temp.ToString();
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "targetFilePath", "", temp, 255, strFIle);
            Equipment.Scanner_Calibration_targetFilePath = temp.ToString();
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "FieldSize", "55", temp, 255, strFIle);
            Equipment.Scanner_Calibration_FieldSize = Convert.ToInt32(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "rowInterval", "2", temp, 255, strFIle);
            Equipment.Scanner_Calibration_rowInterval = Convert.ToInt32(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "colInterval", "2", temp, 255, strFIle);
            Equipment.Scanner_Calibration_colInterval = Convert.ToInt32(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "rowCount", "3", temp, 255, strFIle);
            Equipment.Scanner_Calibration_rowCount = Convert.ToInt32(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "colCount", "3", temp, 255, strFIle);
            Equipment.Scanner_Calibration_colCount = Convert.ToInt32(temp.ToString());


            //  체크 포인트
            if (((Equipment.CoordinateMatchingOffset_X != 0.0) || (Equipment.CoordinateMatchingOffset_Y != 0.0)) &&
                ((Equipment.StageOffset_forDrilling_X != 0.0) || (Equipment.StageOffset_forDrilling_Y != 0.0)))
            {
                MessageBox.Show("\"Offset Distance for Coordinate Matching\" 과\r\n\"Offset Distance to the Center of the Scanner\" 두 그룹 전체에 값이 들어가면 안됩니다.\n\r\n[두 그룹 중 한쪽에만 값이 들어가거나, 모두 0 이어야 합니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return m_bRet;
        }

        //  Mapping Data List Load
        public static bool NewForm_MapDataList_Load()
        {
            string strTemp = "";
            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);
            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Mapping File List 파일이 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //  Mapping Data 리스트 로드

            //  Stage Center 가 Scanner 위치에 있을 경우
            NativeMethods.GetPrivateProfileString("MappingFilePath", "StageCenter_ScannerCenter", "", temp, 255, strFIle);
            Equipment.MappingData_FilePath_Stage_Scanner = temp.ToString();

            //  Stage Center 가 Fine Camera 위치에 있을 경우
            NativeMethods.GetPrivateProfileString("MappingFilePath", "StageCenter_FineCameraCenter", "", temp, 255, strFIle);
            Equipment.MappingData_FilePath_Stage_FineCam = temp.ToString();

            //  Stage Calibration 위치가 Scanner 위치에 있을 경우
            NativeMethods.GetPrivateProfileString("MappingFilePath", "StageCalPos_ScannerCenter", "", temp, 255, strFIle);
            Equipment.MappingData_FilePath_StageCal_Scanner = temp.ToString();

            //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
            NativeMethods.GetPrivateProfileString("MappingFilePath", "StageCalPos_FineCameraCenter", "", temp, 255, strFIle);
            Equipment.MappingData_FilePath_StageCal_FineCam = temp.ToString();

            return m_bRet;
        }

        public static bool NewForm_MapDataActivate_Load()
        {
            string strTemp = "";
            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);
            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Mapping File List 파일이 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            //  활성화 여부
            NativeMethods.GetPrivateProfileString("MapData", "Activate", "False", temp, 255, strFIle);
            Equipment.MapDataStatus_Activate = temp.ToString() == "False" ? false : true;

            return m_bRet;
        }
    }
}
