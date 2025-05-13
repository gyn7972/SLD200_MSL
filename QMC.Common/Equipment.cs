

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
using QMC.Common.Vision.Tools;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;
using QMC.Common.Vision;
using Cognex.VisionPro;
using System.ServiceModel.Syndication;



namespace QMC.Common
{
    public static class Equipment
    {
        public class InitDeviceStatus
        {
            public bool MotionIo { get; set; }
            public bool Laser { get; set; }
            public bool Scanner { get; set; }
            public bool PowerMeter_Bds { get; set; }
            public bool PowerMeter_Stage { get; set; }
            public bool BeamExpander { get; set; }
            public bool DustCollector_Upper { get; set; }
            public bool DustCollector_Lower { get; set; }
            public bool Chiller { get; set; }
            public bool ElectroRegulator { get; set; }
            public bool HeightSensor { get; set; }
            public bool CameraFine { get; set; }
            public bool CameraPre { get; set; }
            public bool Illuminator { get; set; }
        }

        public static double ToDouble(string str)
        {
            double dValue = 0.0;            
            try
            {
                double.TryParse(str, out dValue);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                //Debug.WriteLine(ex.Message);
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
                Log.Write(ex);
                //Debug.WriteLine(ex.Message);
            }
            return nValue;
        }

        public static bool ToBoolean(string str)
        {
            bool bValue = false;
            try
            {
                bool.TryParse(str, out bValue);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return bValue;
        }

        public static float ToFloat(string str)
        {
            float fValue = 0.0f;
            try
            {
                float.TryParse(str, out fValue);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return fValue;
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
        public static Form formMain;
        public enum RtcMode : int
        {
            RTC_NONE = 0,           //  0 : None (Not Initialize)
            RTC_SYNCAXIS = 1,       //  1 : syncAxis Mode
            RTC_RTC6 = 2,           //  2 : RTC6 Mode

            RTC_RTC6_COMPLETE = 3,  //  3 : RTC6 Complete Mode
        }

        public static bool Mode_DryRun { set; get; }
        private static bool Machine_Run;
        public static bool m_bRedraw_FormWorkStageParameterConfig { set; get; }
        public static bool m_bRedraw_FormLoaderParameterConfig { set; get; }
        public static bool m_bRedraw_FormUnloaderParameterConfig { set; get; }
        public static bool m_bRedraw_FormBdsParameterConfig { set; get; }
        public static bool m_bRedraw_FormUpperCameraConfig { set; get; }
        public static bool m_bRedraw_FormLowerCameraConfig { set; get; }

        public static bool AjinBoard_Opened { set; get; }

        //Device 초기화 변수 선언
        public static InitDeviceStatus _InitDeviceStatus = new InitDeviceStatus();
        
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

        public enum Type_Motor_Speed
        {
            Fine = 0,
            Coarse,
        }

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
            Hole5,
            Hole6,
            Hole7,
            Hole8,
            Hole9,
            Hole10,
            Hole11,
            Hole12,
            Hole13,
            Hole14,
            Hole15,
            Hole16,
            Hole17,
            Hole18,
            Hole19,
            Hole20,
            Hole21,
            Hole22,
            Hole23,
            Hole24,
            Hole25,
            Hole26,
            Hole27,
            Hole28,
            Hole29,
            Hole30,
            Hole31,
            Hole32,
            Hole33,
            Hole34,
            Hole35,
            Hole36,
            Hole37,
            Hole38,
            Hole39,
            Hole40,
            Hole41,
            Hole42,
            Hole43,
            Hole44,
            Hole45,
            Hole46,
            Hole47,
            Hole48,
            Hole49,
            Hole50,                 //  설마 50개는 안 넘겠지

            Rect,
            Outline,
            Marking,
            Fiducial,
            Thruhole,
            PreAlign,
        }

        public enum MarkTypeList : int
        {
            Circle = 0,
            GoldPowder,
            Cross
        }

        public enum HoleProcessingType : int
        {
            Circle = 0,
            Spiral_Polyline,
            Spiral_Arc,
            Spiral_Circle,
        }

        public struct stLayerRecipeParameter
        {
            public string DrawingFile;                                  //  Drawing File Path and Name

            public double LaserParam_PulseWidth;                        //  Laser Pulse Width (us)
            public double LaserParam_PulsePeriod;                       //  Laser Pulse Period (us)
            public int LaserParam_Frequency;                            //  Laser Frequency (Hz)
            public double LaserParam_DutyCycle;                         //  Laser Duty Cycle (%)
            public bool LaserParam_TriggerMode_External;                //  Laser Trigger Mode (true: External, false: Internal)

            public bool ProcessPriority_P2P;                            //  Process Priority (true: Space of P2P, false: Pulse Period)

            public string Miscellaneous_ReferenceLayer;                 //  Reference Layer
            public double Miscellaneous_DefocusingDistance;             //  Defocusing Distance (mm)
            public double Miscellaneous_Resizing;                       //  Resizing (mm)
            public int Miscellaneous_HoleDrilling_StartPosDivision;     //  Hole Drilling Start Position Division(등분)
            public double Miscellaneous_GroupSplitSize;                 //  Group Split Size - Width (mm)
            public double Miscellaneous_GroupSplitSize_Height;          //  Group Split Size - Height (mm)
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
            public double Miscellaneous_CircleStartAngleCircle1time;  //  Rotation Start Angle Circle 1 time (degree)
            public double Miscellaneous_P2PDistance;                    //  P2P Distance (mm)
            public int Miscellaneous_MaskIndex;                         //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
            public int Miscellaneous_BETPositionIndex;                  //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
            public int Miscellaneous_HoleProcessingType;                //  Hole Processing Type (0:Circle, 1:Spiral_Polyline, 2:Spiral_Arc, 3:Spiral_Circle)
            public bool Miscellaneous_HoleSortByDistance_Use;               //  Sort By Distance Use (true: Use, false: Not Use)
            public double Miscellaneous_HoleSortingDistance;            //  Hole Sorting Distance (mm)

            public bool ProcessOption_SocketAlign_Use;                  //  Socket Align Use (true: Use, false: Not Use)
            public bool ProcessOption_SocketHeightCheck_Use;            //  Socket Height Check Use Offset (true: Use, false: Not Use)
            public double ProcessOption_SocketHeightCheckPos_OffsetX;   //  Socket Height Check Position Offset X (mm)
            public double ProcessOption_SocketHeightCheckPos_OffsetY;   //  Socket Height Check Position Offset Y (mm)

            public double ModuleInformation_Module_Width;               //  Module Width (mm)
            public double ModuleInformation_Module_Height;              //  Module Height (mm)
            public double ModuleInformation_Silicon_Thickness;          //  Silicon Thickness (mm)

            public double SpiralParam_OuterDiameter;                    //  Spiral Outer Diameter (mm)
            public double SpiralParam_InnerDiameter;                    //  Spiral Inner Diameter (mm)
            public double SpiralParam_Revolutions;                      //  Spiral Revolutions
            public double SpiralParam_AngleFactor;                      //  Spiral Angle Factor

            public double EPRO_ModuleAbsorptionLevel;                   //  EPRO Module Absorption Level

            public bool MAligner_VacuumPos_Center;                      //  M-Aligner Vacuum Position Center (true: Using, false: Not Using)
            public bool MAligner_VacuumPos_Inner;                       //  M-Aligner Vacuum Position Inner (true: Using, false: Not Using)
            public bool MAligner_VacuumPos_Outer;                       //  M-Aligner Vacuum Position Outer (true: Using, false: Not Using)

            //public int IlluminatorValue_FineCamRed;                     //  Illuminator Value (Fine Camera, Red)                            //  조명은 0번 index 만 사용
            //public int IlluminatorValue_FineCamIR;                      //  Illuminator Value (Fine Camera, IR)                             //  조명은 0번 index 만 사용
            //public int IlluminatorValue_CoarseCamIR;                    //  Illuminator Value (Coarse Camera, IR)                           //  조명은 0번 index 만 사용

            public bool DustCollectorRemoteMode_Use;                    //  Dust Collector Remote Mode (true: Remote, false: Local)
            public double DustCollectorFreq_Upper;                      //  Dust Collector Frequency (Upper)
            public double DustCollectorFreq_Lower;                      //  Dust Collector Frequency (Lower)
            public bool DustCollectorLower_Disable;                     //  Dust Collector Lower Disable (true: Disable, false: Enable)
        }
        public static stLayerRecipeParameter[] stLayerRecipeSet = new stLayerRecipeParameter[System.Enum.GetValues(typeof(LayerList)).Length];

        public enum VisionAlgorithmType
        {
            PatternMatching = 0,
            CircleDetection = 1,
            //BlobDetection = 2,
        }

        //  Recipe 파라미터 - PreAlign 
        public class VisionRecipeData
        {
            public VisionRecipeData()
            {
                PrePatternMatching = new PatternMatchingParameters();
                pointPreTrainRoiStartLocation = new System.Drawing.Point(0, 0);
                pointPreTrainRoiEndLocation = new System.Drawing.Point(0, 0);
                pointPreInspectRoiStartLocation = new System.Drawing.Point(0, 0);
                pointPreInspectRoiEndLocation = new System.Drawing.Point(0, 0);
            }
            //Socket
            public int      dSocketAlignType;                 //  Fiducial Align Type (0:Circle Find, 2:Pattern Matching)
            public int      dSocketMarkType;                  //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
            
            public bool     bSocketCircleColor;             //0: White, 1: Black
            public double   dSocketCircleMarkRadius;                  //  Fiducial Mark Size (mm)
            public double   dSocketCircleMarkSpec;                  //  Fiducial Mark Spec
            public double   dSocketCircleMarkScore;         //circle score

            public int      nSocketIlluminationIR;
            public int      nSocketIlluminationRed;

            public bool     bSocketIlluminationIRUse;
            public bool     bSocketIlluminationRedUse;

            public double   dSocketIlluminationExposureTime;
            public double   dSocketAxisZ_Offset;

            //PreAlign
            public PatternMatchingParameters PrePatternMatching;
            public System.Drawing.Point pointPreTrainRoiStartLocation;
            public System.Drawing.Point pointPreTrainRoiEndLocation;
            public System.Drawing.Point pointPreInspectRoiStartLocation;
            public System.Drawing.Point pointPreInspectRoiEndLocation;
            public int nPreIlluminationIR;
            public string pointPreTrainImagePath;

            public bool bPreCircleColor;  //0: White, 1: Black
            public double dPreCircleMarkRadius; //circle size width
            public double dPreCircleMarkSpec;  //
            public double dPreCircleMarkScore; //circle score
            public VisionAlgorithmType ePreAlgorithmType;
            public MarkTypeList ePreMarkType;

            public bool SaveToIni(string path)
            {
                bool bRet = false;

                NativeMethods.WritePrivateProfileString("SocketAlign", "Aligntype", dSocketAlignType.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "MarkType", dSocketMarkType.ToString(), path);

                NativeMethods.WritePrivateProfileString("SocketAlign", "MarkColor", bSocketCircleColor.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "MarkSize", dSocketCircleMarkRadius.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "MarkSpec", dSocketCircleMarkSpec.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "MarkScore", dSocketCircleMarkScore.ToString(), path);

                NativeMethods.WritePrivateProfileString("SocketAlign", "IR", nSocketIlluminationIR.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "Red", nSocketIlluminationRed.ToString(), path);

                NativeMethods.WritePrivateProfileString("SocketAlign", "IRUse", bSocketIlluminationIRUse.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "RedUse", bSocketIlluminationRedUse.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "ExposureTime", dSocketIlluminationExposureTime.ToString(), path);
                NativeMethods.WritePrivateProfileString("SocketAlign", "AxisZ_Offset", dSocketAxisZ_Offset.ToString(), path);


                if (PrePatternMatching != null)
                {
                    NativeMethods.WritePrivateProfileString("PatternMatching", "MinScore", PrePatternMatching.MinScore.ToString(), path);
                    NativeMethods.WritePrivateProfileString("PatternMatching", "MaxInstance", PrePatternMatching.MaxInstance.ToString(), path);
                    NativeMethods.WritePrivateProfileString("PatternMatching", "MaxTolerance", PrePatternMatching.MaxTolerance.ToString(), path);
                    NativeMethods.WritePrivateProfileString("PatternMatching", "DuplicateChecked", PrePatternMatching.DuplicateChecked.ToString(), path);
                    NativeMethods.WritePrivateProfileString("PatternMatching", "UseMaskImage", PrePatternMatching.UseMaskImage.ToString(), path);

                    NativeMethods.WritePrivateProfileString("TrainROI", "StartX", pointPreTrainRoiStartLocation.X.ToString(), path);
                    NativeMethods.WritePrivateProfileString("TrainROI", "StartY", pointPreTrainRoiStartLocation.Y.ToString(), path);
                    NativeMethods.WritePrivateProfileString("TrainROI", "EndX", pointPreTrainRoiEndLocation.X.ToString(), path);
                    NativeMethods.WritePrivateProfileString("TrainROI", "EndY", pointPreTrainRoiEndLocation.Y.ToString(), path);

                    NativeMethods.WritePrivateProfileString("InspectROI", "StartX", pointPreInspectRoiStartLocation.X.ToString(), path);
                    NativeMethods.WritePrivateProfileString("InspectROI", "StartY", pointPreInspectRoiStartLocation.Y.ToString(), path);
                    NativeMethods.WritePrivateProfileString("InspectROI", "EndX", pointPreInspectRoiEndLocation.X.ToString(), path);
                    NativeMethods.WritePrivateProfileString("InspectROI", "EndY", pointPreInspectRoiEndLocation.Y.ToString(), path);

                    NativeMethods.WritePrivateProfileString("Vision", "AlgorithmType", ((int)ePreAlgorithmType).ToString(), path);
                    NativeMethods.WritePrivateProfileString("Vision", "PatternShape", ((int)ePreMarkType).ToString(), path);

                    NativeMethods.WritePrivateProfileString("PreAlign_llumination", "IR", nPreIlluminationIR.ToString(), path);

                    NativeMethods.WritePrivateProfileString("CircleDetection", "Color", bPreCircleColor.ToString(), path);
                    NativeMethods.WritePrivateProfileString("CircleDetection", "SizeW", dPreCircleMarkRadius.ToString(), path);
                    NativeMethods.WritePrivateProfileString("CircleDetection", "Spec", dPreCircleMarkSpec.ToString(), path);
                    NativeMethods.WritePrivateProfileString("CircleDetection", "Score", dPreCircleMarkScore.ToString(), path);

                    
                    string folderName = Path.GetFileNameWithoutExtension(path);

                    if (folderName == "")
                        return bRet = false;

                    string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                    Directory.CreateDirectory(folderPath); // 없으면 생성
                    string bmpPath = Path.Combine(folderPath, "PreAlign.bmp");  // BMP 저장
                    pointPreTrainImagePath = bmpPath;
                    if (!string.IsNullOrWhiteSpace(pointPreTrainImagePath))
                        NativeMethods.WritePrivateProfileString("TrainImage", "Path", pointPreTrainImagePath, path);

                    bRet = true;
                }
                else
                {
                    bRet = false;
                    //var mb = new MessageBoxOk();
                    //mb.ShowDialog("Error!", "Data가 저장되지 않았습니다. 레시피를 불러온 후 진행 바랍니다.");
                }

                return bRet;
            }

            public static VisionRecipeData LoadFromIni(string path)
            {
                VisionRecipeData data = new VisionRecipeData();
                data.PrePatternMatching = new PatternMatchingParameters();
                StringBuilder sb = new StringBuilder(255);

                try
                {
                    // SocketAlign
                    NativeMethods.GetPrivateProfileString("SocketAlign", "Aligntype", "1", sb, sb.Capacity, path);
                    data.dSocketAlignType = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkType", "0", sb, sb.Capacity, path);
                    data.dSocketMarkType = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkColor", "true", sb, sb.Capacity, path);
                    data.bSocketCircleColor = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSize", "0.5", sb, sb.Capacity, path);
                    data.dSocketCircleMarkRadius = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSpec", "0.08", sb, sb.Capacity, path);
                    data.dSocketCircleMarkSpec = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkScore", "0.7", sb, sb.Capacity, path);
                    data.dSocketCircleMarkScore = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "IR", "5", sb, sb.Capacity, path);
                    data.nSocketIlluminationIR = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "Red", "2500", sb, sb.Capacity, path);
                    data.nSocketIlluminationRed = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "IRUse", "True", sb, sb.Capacity, path);
                    data.bSocketIlluminationIRUse = Equipment.ToBoolean(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "RedUse", "True", sb, sb.Capacity, path);
                    data.bSocketIlluminationRedUse = Equipment.ToBoolean(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "ExposureTime", "20000", sb, sb.Capacity, path);
                    data.dSocketIlluminationExposureTime = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString("SocketAlign", "AxisZ_Offset", "0.0", sb, sb.Capacity, path);
                    data.dSocketAxisZ_Offset = Equipment.ToDouble(sb.ToString());


                    //PreAlign
                    NativeMethods.GetPrivateProfileString("PatternMatching", "MinScore", "0.7", sb, sb.Capacity, path);
                    data.PrePatternMatching.MinScore = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("PatternMatching", "MaxInstance", "1", sb, sb.Capacity, path);
                    data.PrePatternMatching.MaxInstance = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("PatternMatching", "MaxTolerance", "45", sb, sb.Capacity, path);
                    data.PrePatternMatching.MaxTolerance = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("PatternMatching", "DuplicateChecked", "False", sb, sb.Capacity, path);
                    data.PrePatternMatching.DuplicateChecked = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("PatternMatching", "UseMaskImage", "False", sb, sb.Capacity, path);
                    data.PrePatternMatching.UseMaskImage = Equipment.ToBoolean(sb.ToString());

                    // ROI
                    NativeMethods.GetPrivateProfileString("TrainROI", "StartX", "0", sb, sb.Capacity, path); 
                    data.pointPreTrainRoiStartLocation.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("TrainROI", "StartY", "0", sb, sb.Capacity, path); 
                    data.pointPreTrainRoiStartLocation.Y = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("TrainROI", "EndX", "0", sb, sb.Capacity, path); 
                    data.pointPreTrainRoiEndLocation.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("TrainROI", "EndY", "0", sb, sb.Capacity, path); 
                    data.pointPreTrainRoiEndLocation.Y = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("InspectROI", "StartX", "0", sb, sb.Capacity, path); 
                    data.pointPreInspectRoiStartLocation.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("InspectROI", "StartY", "0", sb, sb.Capacity, path); 
                    data.pointPreInspectRoiStartLocation.Y = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("InspectROI", "EndX", "0", sb, sb.Capacity, path); 
                    data.pointPreInspectRoiEndLocation.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("InspectROI", "EndY", "0", sb, sb.Capacity, path); 
                    data.pointPreInspectRoiEndLocation.Y = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("Vision", "AlgorithmType", "1", sb, sb.Capacity, path);
                    data.ePreAlgorithmType = (VisionAlgorithmType)Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("Vision", "PatternShape", "1", sb, sb.Capacity, path);
                    data.ePreMarkType = (MarkTypeList)Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("PreAlign_llumination", "IR", "3000", sb, sb.Capacity, path);
                    data.nPreIlluminationIR = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("CircleDetection", "Color", "true", sb, sb.Capacity, path);
                    data.bPreCircleColor = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("CircleDetection", "SizeW", "1.0", sb, sb.Capacity, path);
                    data.dPreCircleMarkRadius = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("CircleDetection", "Spec", "0.1", sb, sb.Capacity, path);
                    data.dPreCircleMarkSpec = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("CircleDetection", "Score", "0.7", sb, sb.Capacity, path);
                    data.dPreCircleMarkScore = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("TrainImage", "Path", "", sb, sb.Capacity, path);
                    data.pointPreTrainImagePath = sb.ToString();

                    if(data.pointPreTrainImagePath == "")
                    {
                        string folderName = Path.GetFileNameWithoutExtension(path);
                        string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                        Directory.CreateDirectory(folderPath); // 없으면 생성
                        string bmpPath = Path.Combine(folderPath, "PreAlign.bmp");  // BMP 저장
                        data.pointPreTrainImagePath = bmpPath;
                    }


                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }

                return data;
            }

            public void SaveTrainImage(VisionImage image)
            {
                if (image == null || string.IsNullOrEmpty(pointPreTrainImagePath))
                    return;

                image.Save(pointPreTrainImagePath, QMC.Common.Vision.VisionImage.FileFilter.bmp);
            }

            public VisionImage LoadTrainImage()
            {
                if (!string.IsNullOrEmpty(pointPreTrainImagePath) && File.Exists(pointPreTrainImagePath))
                {
                    VisionImage img = new VisionImage();
                    img.Load(pointPreTrainImagePath, VisionImage.FileFilter.bmp);
                    return img;
                }

                string strFile = "";
                strFile = string.Format("{0}\\PreAlign.bmp", ConfigManager.GetPatternImagePath());
                if (File.Exists(strFile))
                {
                    try
                    {
                        // 필요한 디렉터리 생성
                        Directory.CreateDirectory(Path.GetDirectoryName(strFile));

                        //File.Copy(strFile, pointPreTrainImagePath, overwrite: true);

                        VisionImage defaultImg = new VisionImage();
                        defaultImg.Load(strFile, VisionImage.FileFilter.bmp);
                        return defaultImg;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }
                }

                return null;
            }
        }
        public static VisionRecipeData stVisionRecipeSet = new VisionRecipeData();

        //PreAlign Data
        public struct PreAlignData
        {
            public double cX;
            public double cY;
            public double Width;
            public double Height;

            public PreAlignData(double x, double y, double width, double height)
            {
                cX = x;
                cY = y;
                Width = width;
                Height = height;
            }
        }
        public static List<PreAlignData> stPreAlignList = new List<PreAlignData>();







        // Laser Process Result struct
        public struct stProcessResultStatus
        {
            public string Layer;
            public int sorket;
            public bool result;
        }


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
        public static bool Machine_VacuumBlowTime_Enable { set; get; } = true;                //  Vacuum Stabilization Time Enable
        public static int Machine_VacuumBlowTime { set; get; } = 500;                         //  Vacuum Signal Stabilization Time (ms)
        public static bool Machine_SocketAlignNG_toNgBox_Enable { set; get; } = true;           //  Vacuum Stabilization Time Enable
        public static int Machine_SocketAlignNG_toNgBox_ReferenceCount { set; get; } = 1;       //  Vacuum Signal Stabilization Time (ms)
        public static bool Machine_LoaderTransfer_Vibration_Enable { set; get; } = false;       //  Loader Transfer Vibration Enable
        public static double Machine_LoaderTransfer_Vibration_AccDecSpeed_Ratio { set; get; } = 2.0;        //  Vibration 시 가감속 속도 비율
        public static int Machine_LoaderTransfer_NumberOfVibrations { set; get; } = 2;                      //  Vibration 횟수
        public static double Machine_LoaderTransfer_Vibration_MoveDistance { set; get; } = 2.0;             //  Vibration 시 이동 거리
        public static int Machine_LoaderTransfer_Vibration_Interval { set; get; } = 500;                    //  Vibration 시 Interval 시간. (한번 털고 나서 대기하는 시간)
        public static bool Machine_LoaderStacker_LiftUp_Enable { set; get; } = true;                        //  Loader Stacker Lift Up Enable
        public static int Machine_LoaderStacker_LiftUpStep { set; get; } = 7;                               //  Loader Stacker Lift Up Step
        public static int Machine_LoaderStacker_LiftUp_StableTime { set; get; } = 1000;                     //  Loader Stacker Lift Up Stable Time
        public static bool Machine_LoaderStacker_Down_afterLoaderPickUp_Enable { set; get; } = true;        //  Loader Stacker Down after Loader Module Pick Up Enable
        public static double Machine_LoaderStacker_DownDistance_afterLoaderPickUp { set; get; } = 5.0;      //  Loader 가 Module Pick Up 후 Stacker 를 내리는 거리
        public static double Machine_LoaderTransfer_ModulePickup_1stDistance { set; get; } = 10.0;          //  Loader 가 Module Pick Up 후 Z축을 올리는 거리 (이 거리만큼 올린 후 바이브레이션을 진행함)
        public static bool Machine_LoaderStacker_NoMaterialDetectTime_Enable { set; get; } = true;          //  Loader Stacker No Material Detect Time Enable
        public static int Machine_LoaderStacker_NoMaterialDetectTime { set; get; } = 10;                    //  Loader Stacker No Material Detect Time
        public static int Machine_PolylineCurve_Resolution { set; get; } = 100;                             //  Polyline Curve Resolution


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


        //  집진기 Run 후 대기 시간 (집진기 동작 IO 는 들어오는데, 동작 안한다는 분기로 빠져서 대기시간 추가함, 디버깅 필요)
        public static double DustCollector_TurnOn_AfterStableTime { set; get; } = 0.0;          //  집진기를 켠 후 대기시간


        //  도면, 레시피 폴더
        public static string DrawingFilePath { set; get; } = "";            //  도면 파일 경로
        public static string RecipeFilePath { set; get; } = "";             //  레시피 파일 경로


        //  도면 렌더링 분해능
        public static int SiriusDrawing_Rendering_Resolution { set; get; } = 50;


        //  Scanner Calibration Parameter
        public static double Scanner_Calibration_LaserFrequency { set; get; } = 0.0;            //  Scanner Calibration Laser Frequency
        public static double Scanner_Calibration_LaserPulseWidth { set; get; } = 0.0;            //  Scanner Calibration Laser Pulse Width
        public static double Scanner_Calibration_LaserEnergy { set; get; } = 0.0;               //  Scanner Calibration Laser Energy
        public static double Scanner_Calibration_CrossMarkLength { set; get; } = 0.0;           //  Scanner Calibration Cross Mark Length
        public static double Scanner_Calibration_LaserMarkSpeed { set; get; } = 0.0;            //  Scanner Calibration Laser Mark Speed (mm/s)
        public static double Scanner_Calibration_LaserJumpSpeed { set; get; } = 0.0;            //  Scanner Calibration Laser Jump Speed (mm/s)
        public static double Scanner_Calibration_LaserOnDelay { set; get; } = 0.0;                //  Scanner Calibration Laser On Delay (us)
        public static double Scanner_Calibration_LaserOffDelay { set; get; } = 0.0;               //  Scanner Calibration Laser Off Delay (us)
        public static double Scanner_Calibration_MarkDelay { set; get; } = 0.0;                  //  Scanner Calibration Mark Delay (us)
        public static double Scanner_Calibration_JumpDelay { set; get; } = 0.0;                  //  Scanner Calibration Jump Delay (us)
        public static double Scanner_Calibration_PolygonDelay { set; get; } = 0.0;               //  Scanner Calibration Polygon Delay (us)
        public static double Scanner_Calibration_CalAreaWidth { set; get; } = 0.0;
        public static double Scanner_Calibration_CalAreaHeight { set; get; } = 0.0;
        public static double Scanner_Calibration_CalPitch   { set; get; } = 0.0;
        public static double Scanner_Calibration_PosX_Last { set; get; } = 0.0;
        public static double Scanner_Calibration_PosY_Last { set; get; } = 0.0;



        public static double Scanner_Calibration_TrainRoiStartLocation_X { set; get; } = 0.0;         //  Scanner Calibration Train Roi Start Location
        public static double Scanner_Calibration_TrainRoiStartLocation_Y { set; get; } = 0.0;
        public static double Scanner_Calibration_TrainRoiEndLocation_X { set; get; } = 0.0;           //  Scanner Calibration Train Roi End Location
        public static double Scanner_Calibration_TrainRoiEndLocation_Y { set; get; } = 0.0;
        public static double Scanner_Calibration_InspectionRoiStartLocation_X { set; get; } = 0.0;         //  Scanner Calibration Inspection Roi Location
        public static double Scanner_Calibration_InspectionRoiStartLocation_Y { set; get; } = 0.0;
        public static double Scanner_Calibration_InspectionRoiEndLocation_X { set; get; } = 0.0;           //  Scanner Calibration Inspection Roi Location
        public static double Scanner_Calibration_InspectionRoiEndLocation_Y { set; get; } = 0.0;

        public static PatternMatchingParameters Scanner_Calibration_PatternMatchingParameters { set; get; } = new PatternMatchingParameters();         //  Scanner Calibration Pattern Matching Parameters
        public static BlobVisionToolParameter Scanner_Calibration_BlobVisionToolParameter { set; get; } = new BlobVisionToolParameter();         //  Scanner Calibration Blob Vision Tool Parameter
        //public static IlluminationDataSet Scanner_Calibration_IlluminationDataSet { set; get; } = new IlluminationDataSet(part);         //  Scanner Calibration Illumination Data Set
        
        public static int Scanner_Calibration_Illumination_channel_01_Value { set; get; } = 0;
        public static int Scanner_Calibration_Illumination_channel_02_Value { set; get; } = 0;
        //public static double Scanner_Calibration_AngleTolerance { set; get; } = 0.0;            //  Scanner Calibration Angle Tolerance (degree)
        //public static double Scanner_Calibration_MaxInstance { set; get; } = 0.0;            //  Scanner Calibration Offset X
        //public static double Scanner_Calibration_MinScore { set; get; } = 0.0;            //  Scanner Calibration Offset Y
        //public static bool Scanner_Calibration_DuplicateCheck { set; get; } = false;            
        //public static bool Scanner_Calibration_UseMaskImage { set; get; } = false;            
        public static bool Scanner_Calibration_UsePatternMatching { set; get; } = false;            //  Scanner Calibration Use Pattern Matching (true: Use, false: Not Use)
        public static bool Scanner_Calibration_UseBlobVisionTool { set; get; } = false;            //  Scanner Calibration Use Blob Vision Tool (true: Use, false: Not Use)
        public static bool Scanner_Calibration_MarkType_Cross { set; get; } = false;
        public static bool Scanner_Calibration_MarkType_Circlle { set; get; } = false;



        //Status로 사용
        public static bool Scanner_Calibration_Change { set; get; } = false;

        public static int Scanner_Calibration_Convert { set; get; } = 0;            //  Scanner Calibration Use (true: Use, false: Not Use)

        public static bool Scanner_Vision_Offset_Setting_Use { set; get; } = false;            //  Scanner Calibration Use (true: Use, false: Not Use)

        public static double Scanner_Vision_Offset_Setting_X { set; get; } = 0.0;            //  Scanner Calibration Use (true: Use, false: Not Use)
        public static double Scanner_Vision_Offset_Setting_Y { set; get; } = 0.0;            //  Scanner Calibration Use (true: Use, false: Not Use)


        //  Scanner Calibration RTC 및 구동 변수
        public static string Scanner_Calibration_srcFilePath { set; get; } = "";            //  Scanner Calibration Source File Path
        public static string Scanner_Calibration_targetFilePath { set; get; } = "";            //  Scanner Calibration Destination File Path
        public static double Scanner_Calibration_FieldSize { set; get; } = 0;            //  Scanner Calibration Field Size (mm)
        public static double Scanner_Calibration_rowInterval { set; get; } = 0;
        public static double Scanner_Calibration_colInterval { set; get; } = 0;
        public static int Scanner_Calibration_rowCount { set; get; } = 0;
        public static int Scanner_Calibration_colCount { set; get; } = 0;



        //  Mapping Data 파일 경로
        public static string MappingData_FilePath_Stage_Scanner { set; get; }
        public static string MappingData_FilePath_Stage_FineCam { set; get; }
        public static string MappingData_FilePath_StageCal_Scanner { set; get; }
        public static string MappingData_FilePath_StageCal_FineCam { set; get; }



        //  현재 Recipe (저장했거나 불러왔거나)
        public static string Current_Recipe { set; get; }


        //  로드된 도면 파일
        public static string Current_DrawingFileName { set; get; } = "";            //  현재 로드된 도면 파일 이름


        //  메인 화면에서 Open 하려는 Recipe 이름
        public static bool RecipeOpen_fromMainForm { set; get; }
        public static string RecipeName_fromMainForm { set; get; }


        //  Loader 에서 Stage 로 Module 을 Loading 할 때 가공 데이터를 Parsing 하기 위한 변수
        public static bool ProcessingData_Parsing_byLoader { set; get; } = false;            //  가공 데이터 Parsing 여부


        //  Auto/Manual 상태 확인
        // 현재 장비의 준비 상태를 관리 할것.! " Auto인 경우에만 시컨스와 같은 동작 가능 하도록 "
        public static bool AutoManualStatus { set; get; }

        // 장비 구동 유/무 변수 : 장비 시컨스 구동 유/무 변수 :: 실제로 장비 구동 확인 
        // true: Auto Run // false : Manual Run
        // 위와 같이 구분하여 장비 관리 할것!
        public static bool AutoRunStatus { set; get; } // 장비 상태: Auto / Manul 상태 표시 

        // Drilling Cycle Stop 예약 변수 : 장비 Stop 시 가공중이던 부분은 완료 되고 Stop 하도록 하기 위함
        // true : Stop 예약
        // _isLaserDrillingWorkRunning 을 false 로 만드는 경우(Stop 하는 경우), 곧바로 false 로 변경하지 않고 Laser 가공이 완료된 후에 false 로 변경
        public static bool LaserDrillingCycStop_Reservation { set; get; } // 장비 Stop 예약

        //  Loading 에 사용하던 Port 를 기억하기 위한 변수
        //  Pick Up 하던 Port 에서만 계속 진행하기 위한 Port Index
        public static int Loader_ActivatePort { set; get; } = 0;            //  Loader Port Activate (0: RPort, 1: LPort)


        public static int DryRun_ProcessingTime { set; get; } = 5;


        public static bool SocketDrilling_Skip { set; get; } = false;            //  Socket Drilling Skip (true: Skip, false: Not Skip)

        public enum SelectedSocketStartModeList : int
        {
            All = 0,
            SelectedSocketOnly,
            SelectedSocketContinue,
        }
        public static int SelectedSocketStartMode { set; get; } = (int)SelectedSocketStartModeList.All;                //  소켓 가공 시작 모드 (0:None, 1:단일 선택 가공,  2:선택 이후 나머지 가공)
        

        public enum LoaderPortList : int
        {
            R_Port = 0,
            L_Port,
        }

        //  자동 운전 중, Loader 의 어떤 Port 에서 Pick Up 했는지
        public static int AUTORUN_Loader_PickUpPort { set; get; } = 0;            //  0: RPort, 1: LPort

        //  자동 운전 중, Work Stage 에 내려놓은 Module 이 어떤 Port 에서 Pick Up 했는지
        public static int AUTORUN_WorkStage_PickUpPort { set; get; } = 0;            //  0: RPort, 1: LPort


        //  Sequence Test 일 경우
        public static bool SeqTestMode { set; get; } = false;


        //  Loader Port 투입 일시정지
        public static bool Loader_Transfer_Pause { set; get; } = false;
        public static bool Loader_LPort_Pause { set; get; } = false;
        public static bool Loader_LPort_Pause_Before { set; get; } = false;             //  L-Port 가 Pause 가 될 때 L-Port 를 아래로 내리기 위한 Flag
        public static bool Loader_RPort_Pause { set; get; } = false;
        public static bool Loader_RPort_Pause_Before { set; get; } = false;             //  R-Port 가 Pause 가 될 때 R-Port 를 아래로 내리기 위한 Flag

        // Loader 자재 상태
        public static bool Loader_LPort_Empty { set; get; } = false;
        public static bool Loader_RPort_Empty { set; get; } = false;

        //  Cycle Stop
        public static bool SocketStop { set; get; } = false;
        public static bool SocketStopped { set; get; } = false;
        public static bool CycleStop { set; get; } = false;
        public static bool CycleStopped_LoaderTransfer { set; get; } = false;
        public static bool CycleStopped_UnloaderTransfer { set; get; } = false;
        public static bool CycleStopped_MainWork { set; get; } = false;


        //  Recipe Open 시 열린 도면 파일 경로
        public static string RecipeOpen_DrawingFilePath { set; get; } = "";


        //  타임아웃으로 인한 Stop
        public static bool MachineStop_byTimeout_Loader { set; get; } = false;
        public static bool MachineStop_byTimeout_Unloader { set; get; } = false;
        public static bool MachineStop_byTimeout_WorkStage { set; get; } = false;


        //  레이저 공정 테스트를 위한 변수
        public static bool LaserDrillingCycleEnable_Manual { set; get; } = false;




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

        //private static Object g_objLock = new object();
        private static SiriusViewerForm EqpSiriusViewer { set;  get; }
        public static IDocument GetEqpSiriusViewerDocument()
        {
            if(EqpSiriusViewer == null)
            {
                return null;
            }
            return EqpSiriusViewer.Document;
        }
        public static void SetEqpSiriusViewerDocument(IDocument doc)
        {
            EqpSiriusViewer.Document.Views = new HashSet<IView>();
            EqpSiriusViewer.Document = doc;
        }

        public static IDocument GetEqpSiriusViewerDocumentOrg()
        {
            return EqpSiriusViewer_Origin.Document;
        }
        public static void SetEqpSiriusViewerDocumentOrg(IDocument doc)
        {
            EqpSiriusViewer_Origin.Document.Views = new HashSet<IView>();
            EqpSiriusViewer_Origin.Document.Views = new HashSet<IView>();
            EqpSiriusViewer_Origin.Document = doc;
        }
        public static void SetEqpSiriusViewer(SiriusViewerForm viewer)
        {
            Equipment.EqpSiriusViewer = viewer;
        }
        public static void SetEqpSiriusViewerOrg(SiriusViewerForm viewer)
        {
            Equipment.EqpSiriusViewer_Origin = viewer;
        }
        private static SiriusViewerForm EqpSiriusViewer_Origin { set; get; }                     //  모듈 생산 완료 후, 다음 모듈이 투입될 때 이 데이터로 재설정
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


        //  평탄도 특정 위치
        public enum FlatMeasureList : int
        {
            Stage = 0,
            CalPos,
            User1,
            User2,
            User3,
        }
        public struct stFlatnessMeasurementParameter
        {
            public PointD[] StagePos;                         //  Stage 위치값
            public double[] LaserHeightValue;                 //  Laser Height Sensor 측정값
        }
        public static stFlatnessMeasurementParameter[] stFlatMeasurePos = new stFlatnessMeasurementParameter[System.Enum.GetValues(typeof(FlatMeasureList)).Length];
        

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
                stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = 3.0;                             //  Group 분할 크기 Width (mm, default : 4mm)
                stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height = 3.0;                      //  Group 분할 크기 Height (mm, default : 4mm)
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
                stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = 100;                   //  Drilling 반복 묶음 횟수
                stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = 360.0;                         //  Rotation Angle Arc (degree)
                stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time = 0.0;                //  Circle Start Angle (degree)
                stLayerRecipeSet[i].Miscellaneous_MaskIndex = 0;                                    //  Mask Index  
                stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = 0;                             //  BET Index  
                stLayerRecipeSet[i].Miscellaneous_Drilling_Power = 10;                              //  Drilling Power              
                stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = 0;                           //  Hole Processing Type (0:Circle, 1:Spiral_Polyline, 2:Spiral_Arc, 3:Spiral_Circle)
                //stLayerRecipeSet[i].Miscellaneous_FiducialAlignType = 0;                            //  Fiducial Align Type (0:Circle Find, 1:Pattern Matching)
                //stLayerRecipeSet[i].Miscellaneous_FiducialMarkType = 0;                             //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
                stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use = false;                   //  Hole Sort By Distance Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance = 0.5;                        //  Hole Data Sorting Distance (mm)

                //  Process Options
                stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = false;                          //  Socket Align Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = false;                    //  Socket Height Check Use Offset (true: Use, false: Not Use)
                stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX = 0.0;               //  Socket Height Check Position Offset X (mm)
                stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY = 0.0;               //  Socket Height Check Position Offset Y (mm)

                //  Module Information
                stLayerRecipeSet[i].ModuleInformation_Module_Width = 0.0;                           //  Module Width (mm)
                stLayerRecipeSet[i].ModuleInformation_Module_Height = 0.0;                          //  Module Height (mm)
                stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = 0.0;                      //  Silicon Thickness (mm)

                //  Spiral Parameter
                stLayerRecipeSet[i].SpiralParam_OuterDiameter = 0.0;                                //  Spiral Outer Diameter Resizing (mm)
                stLayerRecipeSet[i].SpiralParam_InnerDiameter = 0.0;                                //  Spiral Inner Diameter Resizing (mm)
                stLayerRecipeSet[i].SpiralParam_Revolutions = 10.0;                                 //  Spiral Revolutions
                stLayerRecipeSet[i].SpiralParam_AngleFactor = 10.0;                                 //  Spiral Angle Factor

                //  EPRO Module Absorption Level
                stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel = -40.0;                             //  EPRO Module Absorption Level (kPa)

                //  Mechanical-Alignment Vacuum
                stLayerRecipeSet[i].MAligner_VacuumPos_Center = true;                               //  Mechanical-Alignment Center Vacuum Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].MAligner_VacuumPos_Outer = false;                               //  Mechanical-Alignment Outer Vacuum Use (true: Use, false: Not Use)
                stLayerRecipeSet[i].MAligner_VacuumPos_Inner = false;                               //  Mechanical-Alignment Inner Vacuum Use (true: Use, false: Not Use)

                //  Dust Collector
                stLayerRecipeSet[i].DustCollectorRemoteMode_Use = false;                            //  Dust Collector Mode (true: Remote, false: Local)
                stLayerRecipeSet[i].DustCollectorFreq_Upper = 20.0;                                 //  Dust Collector Upper Frequency (Hz)
                stLayerRecipeSet[i].DustCollectorFreq_Lower = 20.0;                                 //  Dust Collector Lower Frequency (Hz)
                stLayerRecipeSet[i].DustCollectorLower_Disable = false;                             //  Dust Collector Lower Disable (true: Disable, false: Enable)
            }


            //  평탄도 측정 위치 초기화
            for (int i = 0; i < System.Enum.GetValues(typeof(FlatMeasureList)).Length; i++)
            {
                stFlatMeasurePos[i].StagePos = new PointD[9];
                stFlatMeasurePos[i].LaserHeightValue = new double[9];
            }

            for (int i = 0; i < System.Enum.GetValues(typeof(FlatMeasureList)).Length; i++)
            {
                for ( int j = 0; j < 9; j++)
                {
                    stFlatMeasurePos[i].StagePos[j] = new PointD(0, 0);
                    stFlatMeasurePos[i].LaserHeightValue[j] = 0.0;
                }
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


            //  집진기 대기 시간
            DustCollector_TurnOn_AfterStableTime = 1000.0;                        //  Dust Collector On 시 안정화 시간 (sec)


            //  파일 저장 위치
            RecipeFilePath = "";
            DrawingFilePath = "";


            //  도면 렌더링  분해능
            SiriusDrawing_Rendering_Resolution = 50;


            Scanner_Calibration_LaserFrequency = 0.0;            //  Scanner Calibration Laser Frequency
            Scanner_Calibration_LaserPulseWidth = 0.0;
            Scanner_Calibration_LaserEnergy = 0.0;               //  Scanner Calibration Laser Energy
            Scanner_Calibration_CrossMarkLength = 0.0;           //  Scanner Calibration Cross Mark Length
            Scanner_Calibration_LaserMarkSpeed = 0.0;            //  Scanner Calibration Laser Mark Speed (mm/s)
            Scanner_Calibration_LaserJumpSpeed = 0.0;            //  Scanner Calibration Laser Jump Speed (mm/s)
            Scanner_Calibration_LaserOnDelay = 0.0;         //  Scanner Calibration Laser On Delay (us)
            Scanner_Calibration_LaserOffDelay = 0.0;        //  Scanner Calibration Laser Off Delay (us)
            Scanner_Calibration_MarkDelay = 0.0;            //  Scanner Calibration Laser Mark Delay (us)
            Scanner_Calibration_JumpDelay = 0.0;           //  Scanner Calibration Laser Jump Delay (us)
            Scanner_Calibration_PolygonDelay = 0.0;        //  Scanner Calibration Laser Polygon Delay (us)
            Scanner_Calibration_CalAreaWidth = 0.0;           //  Scanner Calibration Area Width (mm)
            Scanner_Calibration_CalAreaHeight = 0.0;          //  Scanner Calibration Area Height (mm)
            Scanner_Calibration_CalPitch = 0.0;               //  Scanner Calibration Area Pitch (mm)
            Scanner_Calibration_PosX_Last = 0.0;
            Scanner_Calibration_PosY_Last = 0.0;

            //Scanner_Calibration_TrainRoiStartLocation     //  Scanner Calibration Train ROI Start Location


            //  자동운전 상태 확인
            AutoRunStatus = false;


            m_bVisionFormOpenMode_ScannerFineCamOffsetChange = false;

            Current_Recipe = "";
            Current_DrawingFileName = "";

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
            NewForm_ScannerCalibration_Load();

            NewForm_MapDataList_Load();
            NewForm_MapDataActivate_Load();
            NewForm_FlatMeasurePos_Data_Load();


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

            AlarmSaver alarmSaver = new AlarmSaver();
            alarmSaver.Server = "SLD-200\\SQLEXPRESS";
            alarmSaver.Database = "LASER_DRILLING";
            alarmSaver.UID = "qmc1";
            alarmSaver.Password = "q1234!";
            alarmSaver.Open();
            AlarmManager.Instance.Saver = alarmSaver;
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
                Equipment.stAxisParam[i].LimitSensor_Installed = Equipment.ToInt(temp.ToString());
                //  Limit Sensor 동작 레벨 (Low, High)
                NativeMethods.GetPrivateProfileString(strTemp, "ActiveLevel", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].LimitSensor_ActiveLevel = Equipment.ToInt(temp.ToString());

                strTemp = string.Format("Axis_{0}_Home", i);
                //  Home Sensor 형태 (Home, -Limit, +Limit)
                NativeMethods.GetPrivateProfileString(strTemp, "SensingType", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Sensing = Equipment.ToInt(temp.ToString());
                //  Home Sensor 설치 여부 (Not Installed, Installed)
                NativeMethods.GetPrivateProfileString(strTemp, "Install", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Installed = Equipment.ToInt(temp.ToString());
                //  Home Sensor 동작 레벨 (Low, High)
                NativeMethods.GetPrivateProfileString(strTemp, "ActiveLevel", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_ActiveLevel = Equipment.ToInt(temp.ToString());
                //  Home Sensor 동작 방향 (Negative, Positive)
                NativeMethods.GetPrivateProfileString(strTemp, "Direction", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Direction = Equipment.ToInt(temp.ToString());
                //  Home 1st Speed
                NativeMethods.GetPrivateProfileString(strTemp, "1stSpeed", "30", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_1st = Equipment.ToDouble(temp.ToString());
                //  Home 2nd Speed
                NativeMethods.GetPrivateProfileString(strTemp, "2ndSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_2nd = Equipment.ToDouble(temp.ToString());
                //  Home 3rd Speed
                NativeMethods.GetPrivateProfileString(strTemp, "3rdSpeed", "5", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_3rd = Equipment.ToDouble(temp.ToString());
                //  Home Last Speed
                NativeMethods.GetPrivateProfileString(strTemp, "LastSpeed", "1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Speed_Last = Equipment.ToDouble(temp.ToString());
                //  Home Clear Time
                NativeMethods.GetPrivateProfileString(strTemp, "ClearTime", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Clear_Time = Equipment.ToDouble(temp.ToString());
                //  Home ZPhase Use (Disable, Dir CW, Dir CCW)
                NativeMethods.GetPrivateProfileString(strTemp, "ZPhase", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_ZPhase_Use = Equipment.ToInt(temp.ToString());
                //  Home Offset
                NativeMethods.GetPrivateProfileString(strTemp, "Offset", "0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Offset = Equipment.ToDouble(temp.ToString());
                //  Home 1st Acceleration
                NativeMethods.GetPrivateProfileString(strTemp, "1stAccel", "300", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Acceleration_1st = Equipment.ToDouble(temp.ToString());
                //  Home 2nd Acceleration
                NativeMethods.GetPrivateProfileString(strTemp, "2ndAccel", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Home_Acceleration_2nd = Equipment.ToDouble(temp.ToString());

                strTemp = string.Format("Axis_{0}_Common", i);
                //  Unit Per Pulse (Unit)
                NativeMethods.GetPrivateProfileString(strTemp, "Unit", "1.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_UnitPerPulse_Unit = Equipment.ToDouble(temp.ToString());
                //  Unit Per Pulse (Pulse)
                NativeMethods.GetPrivateProfileString(strTemp, "Pulse", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse = Equipment.ToInt(temp.ToString());
                //  Acceleration Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinAcc", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Min = Equipment.ToDouble(temp.ToString());
                //  Acceleration Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxAcc", "10000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Max = Equipment.ToDouble(temp.ToString());
                //  Acceleration Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineAcc", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Fine = Equipment.ToDouble(temp.ToString());
                //  Acceleration Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseAcc", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Acceleration_Coarse = Equipment.ToDouble(temp.ToString());
                //  Speed Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Min = Equipment.ToDouble(temp.ToString());
                //  Speed Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxSpeed", "1000", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Max = Equipment.ToDouble(temp.ToString());
                //  Move Speed Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Fine = Equipment.ToDouble(temp.ToString());
                //  Move Speed Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseSpeed", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Speed_Coarse = Equipment.ToDouble(temp.ToString());
                //  Position Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinPos", "-1.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Position_Min = Equipment.ToDouble(temp.ToString());
                //  Position Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxPos", "500.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Position_Max = Equipment.ToDouble(temp.ToString());
                //  Settle Delay Time
                NativeMethods.GetPrivateProfileString(strTemp, "SettleDelay", "30", temp, 255, strFIle);
                Equipment.stAxisParam[i].Common_Settle_Delay = Equipment.ToDouble(temp.ToString());

                strTemp = string.Format("Axis_{0}_Jog", i);
                //  Jog Speed, Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineSpeed", "10", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_Speed_Fine = Equipment.ToDouble(temp.ToString());
                //  Jog Speed, Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseSpeed", "100", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_Speed_Coarse = Equipment.ToDouble(temp.ToString());
                //  Jog StepSize, Min
                NativeMethods.GetPrivateProfileString(strTemp, "MinStepSize", "0.0001", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Min = Equipment.ToDouble(temp.ToString());
                //  Jog StepSize, Max
                NativeMethods.GetPrivateProfileString(strTemp, "MaxStepSize", "500.0", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Max = Equipment.ToDouble(temp.ToString());
                //  Jog StepSize, Fine
                NativeMethods.GetPrivateProfileString(strTemp, "FineStepSize", "0.001", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Fine = Equipment.ToDouble(temp.ToString());
                //  Jog StepSize, Coarse
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseStepSize", "0.1", temp, 255, strFIle);
                Equipment.stAxisParam[i].Jog_StepSize_Coarse = Equipment.ToDouble(temp.ToString());
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
                Equipment.stCommunicationSet[i].Comm_Type = Equipment.ToInt(temp.ToString());


                //  Not Connect, Connect                                                                            //  0 : Not Connect,    1 : Connect
                NativeMethods.GetPrivateProfileString(strTemp, "ConnectType", "False", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Connect = temp.ToString() == "False" ? false : true;


                //  TCP/IP 의 포트 형식 (Server, Client)                                                            //  0 : Server,         1 : Client
                NativeMethods.GetPrivateProfileString(strTemp, "TCPIP_PortType", "1", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].TCPIP_PortType = Equipment.ToInt(temp.ToString());
                //  TCP/IP 의 IP 주소
                NativeMethods.GetPrivateProfileString(strTemp, "TCPIP_IPAddress", "127.0.0.1", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].TCPIP_IPAddress = temp.ToString();
                //  TCP/IP 의 Port 번호
                NativeMethods.GetPrivateProfileString(strTemp, "TCPIP_PortNum", "5000", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].TCPIP_PortNum = Equipment.ToInt(temp.ToString());


                //  Timeout (ms)
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_Timeout", "500", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommTimeout = Equipment.ToInt(temp.ToString());
                //  Spacing Delay (ms)
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_SpacingDelay", "20", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommSpacingDelay = Equipment.ToInt(temp.ToString());
                //  COM Port                                                                                        //  0 : COM1,           1 : COM2,       2 : COM3,       3 : COM4 ....
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_Port", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommPort = Equipment.ToInt(temp.ToString());
                //  Baud Rate                                                                                       //  0 : 1200,           1 : 2400,       2 : 4800,       3 : 9600,           4 : 19200,      5 : 38400,      6 : 57600,      7 : 115200
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_BaudRate", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommBaudRate = Equipment.ToInt(temp.ToString());
                //  Data Bits                                                                                       //  0 : 5,              1 : 6,          2 : 7,          3 : 8
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_DataBit", "3", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommDataBits = Equipment.ToInt(temp.ToString());
                //  Stop Bits                                                                                       //  0 : 1,              1 : 1.5,        2 : 2
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_StopBit", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommStopBits = Equipment.ToInt(temp.ToString());
                //  Parity                                                                                          //  0 : None,           1 : Odd,        2 : Even
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_Parity", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommParity = Equipment.ToInt(temp.ToString());
                //  Flow Control                                                                                    //  0 : None,           1 : Xon/Xoff,   2 : RTS/CTS
                NativeMethods.GetPrivateProfileString(strTemp, "RS232_FlowControl", "0", temp, 255, strFIle);
                Equipment.stCommunicationSet[i].Serial_CommFlowControl = Equipment.ToInt(temp.ToString());
            }

            return m_bRet;
        }

        public static bool NewForm_ScannerCalibration_Load()
        {
            bool bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Machine ScannerCalibration (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine ScannerCalibration 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Scanner Calibration parameter
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Frequency", "5000.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserFrequency = Equipment.ToDouble(temp.ToString());
            //Scanner_Calibration_LaserPulseWidth
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Pulse_Width", "1.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserPulseWidth = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Energy", "1.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserEnergy = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "CrossMark_Length", "0.5", temp, 255, strFIle);
            Equipment.Scanner_Calibration_CrossMarkLength = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Marking_Speed", "500.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserMarkSpeed = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Jump_Speed", "500.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserJumpSpeed = Equipment.ToDouble(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "LaserOn_Delay", "10.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserOnDelay = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "LaserOff_Delay", "10.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_LaserOffDelay = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Mark_Delay", "50.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_MarkDelay = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Jump_Delay", "200.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_JumpDelay = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Polygon_Delay", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PolygonDelay = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Width", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_CalAreaWidth = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Height", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_CalAreaHeight = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Cal_Pitch", "2.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_CalPitch = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "PosX_Last", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PosX_Last = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "PosY_Last", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PosY_Last = Equipment.ToDouble(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiStartLocation_X", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_TrainRoiStartLocation_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiStartLocation_Y", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_TrainRoiStartLocation_Y = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiEndLocation_X", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_TrainRoiEndLocation_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiEndLocation_Y", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_TrainRoiEndLocation_Y = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiStartLocation_X", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_InspectionRoiStartLocation_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiStartLocation_Y", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_InspectionRoiStartLocation_Y = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiEndLocation_X", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_InspectionRoiEndLocation_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiEndLocation_Y", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_InspectionRoiEndLocation_Y = Equipment.ToDouble(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "AngleTolerance", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PatternMatchingParameters.MaxTolerance = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MaxInstance", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PatternMatchingParameters.MaxInstance = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MinScore", "0.0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PatternMatchingParameters.MinScore = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "DuplicateCheck", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PatternMatchingParameters.DuplicateChecked = Convert.ToBoolean(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "UseMaskImage", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_PatternMatchingParameters.UseMaskImage = Convert.ToBoolean(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Illumination_channel_01", "0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_Illumination_channel_01_Value = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Illumination_channel_02", "0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_Illumination_channel_02_Value = Equipment.ToInt(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "UsePatternMatching", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_UsePatternMatching = Convert.ToBoolean(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "UseBlobVisionTool", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_UseBlobVisionTool = Convert.ToBoolean(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MarkType_Cross", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_MarkType_Cross = Convert.ToBoolean(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MarkType_Circlle", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_MarkType_Circlle = Convert.ToBoolean(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "HardThreshold", "0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_BlobVisionToolParameter.HardThreshold = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MinPixels", "0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_BlobVisionToolParameter.MinPixels = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Polarity", "0", temp, 255, strFIle);
            int nVel = ToInt(temp.ToString());
            Polarity polarity;
            if (nVel == 0)
                polarity = Polarity.LightBlobs;
            else
                polarity = Polarity.DarkBlobs;
            Equipment.Scanner_Calibration_BlobVisionToolParameter.Polarity = polarity;
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "RepeatCount", "0", temp, 255, strFIle);
            Equipment.Scanner_Calibration_BlobVisionToolParameter.RepeatCount = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "HasChanged", "false", temp, 255, strFIle);
            Equipment.Scanner_Calibration_BlobVisionToolParameter.HasChanged = Convert.ToBoolean(temp.ToString());

            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "srcFilePath", "", temp, 255, strFIle);
            Equipment.Scanner_Calibration_srcFilePath = temp.ToString();
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "targetFilePath", "", temp, 255, strFIle);
            Equipment.Scanner_Calibration_targetFilePath = temp.ToString();
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "FieldSize", "55", temp, 255, strFIle);
            Equipment.Scanner_Calibration_FieldSize = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "rowInterval", "2", temp, 255, strFIle);
            Equipment.Scanner_Calibration_rowInterval = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "colInterval", "2", temp, 255, strFIle);
            Equipment.Scanner_Calibration_colInterval = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "rowCount", "3", temp, 255, strFIle);
            Equipment.Scanner_Calibration_rowCount = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "colCount", "3", temp, 255, strFIle);
            Equipment.Scanner_Calibration_colCount = Equipment.ToInt(temp.ToString());

            return bRet;
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
            Equipment.Machine_SignalHoldTime = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "MAligner_ReleaseType", "True", temp, 255, strFIle);
            Equipment.Machine_MAligner_ReleaseType = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "MAligner_NarrowingDistance", "0.5", temp, 255, strFIle);
            Equipment.Machine_MAligner_NarrowingDistance = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "MAligner_WidenDistance", "2.0", temp, 255, strFIle);
            Equipment.Machine_MAligner_WidenDistance = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumStableTime_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_VacuumStableTime_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumStableTime", "500", temp, 255, strFIle);
            Equipment.Machine_VacuumStableTime = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LaserHeightCheckStableTime_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_LaserHeightCheckStableTime_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "LaserHeightCheckStableTime", "500", temp, 255, strFIle);
            Equipment.Machine_LaserHeightCheckStableTime = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "FiducialMarkJudgementRange_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_FiducialMarkJudgementRange_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "FiducialMarkJudgementRange", "0.1", temp, 255, strFIle);
            Equipment.Machine_FiducialMarkJudgementRange = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "FiducialImageSave_Always", "True", temp, 255, strFIle);
            Equipment.Machine_FiducialImageSave_Always = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumBlowTime_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_VacuumBlowTime_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "VacuumBlowTime", "500", temp, 255, strFIle);
            Equipment.Machine_VacuumBlowTime = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderTransfer_Vibration_Enable", "False", temp, 255, strFIle);
            Equipment.Machine_LoaderTransfer_Vibration_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderTransfer_Vibration_AccDecSpeed_Ratio", "2.0", temp, 255, strFIle);
            Equipment.Machine_LoaderTransfer_Vibration_AccDecSpeed_Ratio = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderTransfer_NumberOfVibrations", "2", temp, 255, strFIle);
            Equipment.Machine_LoaderTransfer_NumberOfVibrations = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderTransfer_Vibration_MoveDistance", "2.0", temp, 255, strFIle);
            Equipment.Machine_LoaderTransfer_Vibration_MoveDistance = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderTransfer_Vibration_Interval", "500", temp, 255, strFIle);
            Equipment.Machine_LoaderTransfer_Vibration_Interval = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_LiftUp_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_LiftUp_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_LiftUp_Step", "7", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_LiftUpStep = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_LiftUp_StableTime", "1000", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_LiftUp_StableTime = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_Down_afterLDPickUp_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_Down_afterLoaderPickUp_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_DownDistance_afterLDPickUp", "5.0", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_DownDistance_afterLoaderPickUp = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderTransfer_ModulePickup_1stDistance", "10.0", temp, 255, strFIle);
            Equipment.Machine_LoaderTransfer_ModulePickup_1stDistance = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_NoMaterialDetectTime_Enable", "True", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_NoMaterialDetectTime_Enable = temp.ToString() == "False" ? false : true;
            NativeMethods.GetPrivateProfileString("Machine_Option", "LoaderStacker_NoMaterialDetectTime", "10", temp, 255, strFIle);
            Equipment.Machine_LoaderStacker_NoMaterialDetectTime = Equipment.ToInt(temp.ToString());
            NativeMethods.GetPrivateProfileString("Machine_Option", "PolylineCurve_Resolution", "100", temp, 255, strFIle);
            Equipment.Machine_PolylineCurve_Resolution = Equipment.ToInt(temp.ToString());

            //  Offset Distance
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_X", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromScannerToFineCam.X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_Y", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromScannerToFineCam.Y = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_X", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_Y", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_X", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_Y", "0.0", temp, 255, strFIle);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y = Equipment.ToDouble(temp.ToString());

            //  Scanner Head Offset
            NativeMethods.GetPrivateProfileString("ScannerHeadOffset", "Offset_X", "0.0", temp, 255, strFIle);
            Equipment.Scanner_HeadOffset_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("ScannerHeadOffset", "Offset_Y", "0.0", temp, 255, strFIle);
            Equipment.Scanner_HeadOffset_Y = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("ScannerHeadOffset", "Offset_Angle", "0.0", temp, 255, strFIle);
            Equipment.Scanner_HeadOffset_Angle = Equipment.ToDouble(temp.ToString());

            //  Coordinate System Matching Offset
            NativeMethods.GetPrivateProfileString("MachineCoordinateOffset", "Offset_X", "0.0", temp, 255, strFIle);
            Equipment.CoordinateMatchingOffset_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("MachineCoordinateOffset", "Offset_Y", "0.0", temp, 255, strFIle);
            Equipment.CoordinateMatchingOffset_Y = Equipment.ToDouble(temp.ToString());

            //  Offset Distance from Stage to Scanner
            NativeMethods.GetPrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_X", "0.0", temp, 255, strFIle);
            Equipment.StageOffset_forDrilling_X = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_Y", "0.0", temp, 255, strFIle);
            Equipment.StageOffset_forDrilling_Y = Equipment.ToDouble(temp.ToString());

            //  Keyence Laser Height Sensor 기준값 설정
            NativeMethods.GetPrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Vision_Focus_Position", "0.0", temp, 255, strFIle);
            Equipment.LaserHeightSensor_ReferenceValue_atVisionFocusPosition = Equipment.ToDouble(temp.ToString());
            NativeMethods.GetPrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Scanner_Focus_Position", "0.0", temp, 255, strFIle);
            Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition = Equipment.ToDouble(temp.ToString());

            //  집진기 동작 후 대기 시간
            NativeMethods.GetPrivateProfileString("Dust_Collector", "After_TurnOn_StableTime", "1000.0", temp, 255, strFIle);
            Equipment.DustCollector_TurnOn_AfterStableTime = Equipment.ToDouble(temp.ToString());

            //  저장 폴더 위치
            NativeMethods.GetPrivateProfileString("File_Path", "RecipeFile", "", temp, 255, strFIle);
            Equipment.RecipeFilePath = temp.ToString();
            NativeMethods.GetPrivateProfileString("File_Path", "DrawingFile", "", temp, 255, strFIle);
            Equipment.DrawingFilePath = temp.ToString();

            //  도면 렌더링 분해능
            NativeMethods.GetPrivateProfileString("Sirius_Drawing", "Rendering_Resolution", "50", temp, 255, strFIle);
            Equipment.SiriusDrawing_Rendering_Resolution = Equipment.ToInt(temp.ToString());


            ////  Scanner Calibration parameter
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Frequency", "5000.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserFrequency = Equipment.ToDouble(temp.ToString());
            ////Scanner_Calibration_LaserPulseWidth
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Pulse_Width", "1.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserPulseWidth = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Laser_Energy", "1.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserEnergy = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "CrossMark_Length", "0.5", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_CrossMarkLength = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Marking_Speed", "500.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserMarkSpeed = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Jump_Speed", "500.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserJumpSpeed = Equipment.ToDouble(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "LaserOn_Delay", "10.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserOnDelay = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "LaserOff_Delay", "10.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_LaserOffDelay = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Mark_Delay", "50.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_MarkDelay = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Jump_Delay", "200.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_JumpDelay = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Polygon_Delay", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PolygonDelay = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Width", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_CalAreaWidth = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Height", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_CalAreaHeight = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Cal_Pitch", "2.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_CalPitch = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "PosX_Last", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PosX_Last = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "PosY_Last", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PosY_Last = Equipment.ToDouble(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiStartLocation_X", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_TrainRoiStartLocation_X = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiStartLocation_Y", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_TrainRoiStartLocation_Y = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiEndLocation_X", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_TrainRoiEndLocation_X = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "TrainRoiEndLocation_Y", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_TrainRoiEndLocation_Y = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiStartLocation_X", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_InspectionRoiStartLocation_X = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiStartLocation_Y", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_InspectionRoiStartLocation_Y = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiEndLocation_X", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_InspectionRoiEndLocation_X = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "InspectionRoiEndLocation_Y", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_InspectionRoiEndLocation_Y = Equipment.ToDouble(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "AngleTolerance", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PatternMatchingParameters.MaxTolerance = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MaxInstance", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PatternMatchingParameters.MaxInstance = Equipment.ToInt(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MinScore", "0.0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PatternMatchingParameters.MinScore = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "DuplicateCheck", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PatternMatchingParameters.DuplicateChecked = Convert.ToBoolean(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "UseMaskImage", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_PatternMatchingParameters.UseMaskImage = Convert.ToBoolean(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Illumination_channel_01", "0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_Illumination_channel_01_Value = Equipment.ToInt(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Illumination_channel_02", "0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_Illumination_channel_02_Value = Equipment.ToInt(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "UsePatternMatching", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_UsePatternMatching = Convert.ToBoolean(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "UseBlobVisionTool", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_UseBlobVisionTool = Convert.ToBoolean(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MarkType_Cross", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_MarkType_Cross = Convert.ToBoolean(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MarkType_Circlle", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_MarkType_Circlle = Convert.ToBoolean(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "HardThreshold", "0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_BlobVisionToolParameter.HardThreshold = Equipment.ToInt(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "MinPixels", "0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_BlobVisionToolParameter.MinPixels = Equipment.ToInt(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "Polarity", "0", temp, 255, strFIle);
            //int nVel = ToInt(temp.ToString());
            //Polarity polarity;
            //if (nVel == 0)
            //    polarity = Polarity.LightBlobs;
            //else
            //    polarity = Polarity.DarkBlobs;
            //Equipment.Scanner_Calibration_BlobVisionToolParameter.Polarity = polarity;
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "RepeatCount", "0", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_BlobVisionToolParameter.RepeatCount = Equipment.ToInt(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "HasChanged", "false", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_BlobVisionToolParameter.HasChanged = Convert.ToBoolean(temp.ToString());

            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "srcFilePath", "", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_srcFilePath = temp.ToString();
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "targetFilePath", "", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_targetFilePath = temp.ToString();
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "FieldSize", "55", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_FieldSize = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "rowInterval", "2", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_rowInterval = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "colInterval", "2", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_colInterval = Equipment.ToDouble(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "rowCount", "3", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_rowCount = Equipment.ToInt(temp.ToString());
            //NativeMethods.GetPrivateProfileString("Scanner_Calibration_Parameter", "colCount", "3", temp, 255, strFIle);
            //Equipment.Scanner_Calibration_colCount = Equipment.ToInt(temp.ToString());



            //  체크 포인트
            if (((Equipment.CoordinateMatchingOffset_X != 0.0) || (Equipment.CoordinateMatchingOffset_Y != 0.0)) &&
                ((Equipment.StageOffset_forDrilling_X != 0.0) || (Equipment.StageOffset_forDrilling_Y != 0.0)))
            {
                MessageBox.Show("\"Offset Distance for Coordinate Matching\" 과\r\n\"Offset Distance to the Center of the Scanner\" 두 그룹 전체에 값이 들어가면 안됩니다.\n\r\n[두 그룹 중 한쪽에만 값이 들어가거나, 모두 0 이어야 합니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return m_bRet;
        }


        public static bool NewForm_FlatMeasurePos_Data_Load()
        {
            string strTemp = "";
            string strTemp2 = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\FlatMeasure_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Flatness Measurement Position Teaching 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }


            //  Flat Measurement Position 로드
            for (int i = 0; i < (int)System.Enum.GetValues(typeof(FlatMeasureList)).Length; i++)
            {
                strTemp = string.Format("MeasureListType_{0}", i);

                for (int j = 0; j < 9; j++)
                {
                    //  Position X
                    strTemp2 = string.Format("Position_{0}_X", j + 1);
                    NativeMethods.GetPrivateProfileString(strTemp, strTemp2, "0.0", temp, 255, strFIle);
                    Equipment.stFlatMeasurePos[i].StagePos[j].X = Equipment.ToDouble(temp.ToString());

                    //  Position Y
                    strTemp2 = string.Format("Position_{0}_Y", j + 1);
                    NativeMethods.GetPrivateProfileString(strTemp, strTemp2, "0.0", temp, 255, strFIle);
                    Equipment.stFlatMeasurePos[i].StagePos[j].Y = Equipment.ToDouble(temp.ToString());
                }
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


        public static bool m_bworkStageVacuumFail = false;
    }
}
