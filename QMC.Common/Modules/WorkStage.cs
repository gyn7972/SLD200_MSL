using ACS.SPiiPlusNET;
using QMC.Common.Laser;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.VisionPart;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using SerialCommHoneywellBarcodeReader;
using static QMC.Common.Parts.WorkStageParameter;
using Point = System.Drawing.Point;
using SerialCommLaserPowerMeter1;                           //  PowerMeter (Source Pos.) - COM2
using SerialCommLaserPowerMeter2;                           //  PowerMeter (Target Pos.) - COM3
using SerialCommBeamExpander;                               //  Motorized Beam Expander - COM4
using SerialCommDustCollector1;                             //  Dust Collector 1 - COM5
using SerialCommDustCollector2;                             //  Dust Collector 2 - COM6
using SerialCommElectroPneumaticRegulator;                  //  Electro Pneumatic Regulator - COM7
using System.IO.Ports;
using MessageBox = System.Windows.Forms.MessageBox;

//  Sirius1
using SpiralLab.Sirius;
using LaserVirtual = SpiralLab.Sirius.LaserVirtual;
using static QMC.Common.Modules.Loader;
//using SpiralLab.Sirius2.Winforms.Marker;

//  Sirius2
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
//using LaserVirtual = SpiralLab.Sirius2.Laser.LaserVirtual;
//using SpiralLab.Sirius2.PowerMap;


namespace QMC.Common.Modules
{
    [Serializable]
    public class WorkStage : Module
    {
        #region Define

        public enum nAxis
        {
            X = 0,
            Y,
            Z,
            MASK_Y,
        }

        public enum nCameraType
        {
            Cam_HighRes = 0,
            Cam_LowRes,
        }
                
        public int m_nReticleGlassCheck_Cam;            //  0:None      1:Upper     2:Lower
        public enum ReticleCamType
        {
            None = 0,
            HighRes_Cam = 1,
            LowRes_Cam = 2,
        }

        public enum nSerialComm
        {
            PowerMeter1 = 0,
            PowerMeter2,
            BeamExpander,
            DustCollector1,
            DustCollector2,
            ElectroPneumaticRegulator,
        }

        public enum nDustCollector
        {
            DustCollector_Upper = 0,
            DustCollector_Lower = 1,
        }

        public enum nPowerMeter
        {
            ExitPos = 0,
            TargetPos,
        }

        public enum nProcessDir
        {
            ZigZag = 0,                         //  지그재그
            LtoR = 1,                           //  왼쪽에서 오른쪽으로 (Stage 는 오른쪽에서 왼쪽으로 이동)
            RtoL = 2,                           //  오른쪽에서 왼쪽으로 (Stage 는 왼쪽에서 오른쪽으로 이동)
        }

        public enum nGetDataResult
        {
            GETDATA_FAIL = -1,                  //  "데이터가 정상적으로 로드 되지 않았습니다."
            GETDATA_SUCCESS = 0,                //  "데이터가 정상적으로 로드 되었습니다."
            GETDATA_NOT_GROUP = 1,              //  "데이터가 Group 이 아닙니다."
            GETDATA_UNGROUP = 2,                //  "데이터를 Group 해제 해야 합니다."
            GETDATA_LAYERNAME_NG = 3,           //  "Layer Name 은 '쓰루홀', '외곽선', '드릴링', '마킹' 4가지만 가능합니다."
            GETDATA_MOTIONTYPE_NG = 4,          //  "Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다."
            GETDATA_DRILDATA_NG = 5,            //  "Drilling Data 는 Polyline, Line, Circle 중 한 가지 데이터로만 구성되어야 합니다."
            GETDATA_DRILDATA_LINECNT = 6,       //  "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다."
            GETDATA_DRILDATA_NOT_CLOSED = 7,    //  "Drilling Data 가 닫힌 도형이 아닙니다."
            GETDATA_DRILDATA_NOT_GROUP = 8,     //  "Drilling Data 가 Group 이 아닙니다."
        }

        //public enum nPegMode
        //{
        //    PEG_FullNormal = 0,
        //    PEG_FullManual = 1,
        //    PEG_FullManualVisionLine = 2,
        //    PEG_NgChipLLO = 3,
        //    PEG_RandomPEG = 4,
        //}

        #endregion


        #region RTC Scanner Config

        /// <summary>
        /// freq : [Hz] set value,
        /// pulsewidth : [usec] set value,
        /// laseron : [usec] delay,
        /// laseroff : [usec] delay,
        /// scannerjump : [usec] delay,
        /// scannermark : [usec] delay,
        /// scannerplygon : [usec] delay,
        /// jump : [mm/s] speed,
        /// mark : [mm/s] speed,
        /// </summary>
        public struct RTCScannerConfig
        {
            //--> Set Value 
            public double freq;         // [Hz]
            public double pulsewidth;   // [usec]

            //--> Delay 
            public double laseron;          // [usec]
            public double laseroff;         // [usec]
            public double scannerjump;      // [usec]
            public double scannermark;      // [usec]
            public double scannerplygon;    // [usec]

            //--> Speed
            public double jump;             // [mm/s]
            public double mark;             // [mm/s]

            //--> Processing Parameter
            public double pixelPeriod;           // [usec]
            public int channel;
            public double pitchX;
            public double pitchY;
            public double pixelOutputTime;
            public double voltage;
            public uint pixelMarginCount;
            public bool zigZag;
            public double FillPercent;

            public void Initialize()
            {
                freq = 20000; // [Hz] :: Fixed 20 kHz
                pulsewidth = 20; // [us] :: Fixed 20 us

                laseron = 30; // Delay [us] 
                laseroff = 60; // Delay [us]
                scannerjump = 99.5; // Delay [us]
                scannermark = 49.5; // Delay [us]
                scannerplygon = 25; // Delay [us]

                jump = 400; // Speed [mm/s]
                mark = 400; // Speed [mm/s]

                pixelPeriod = 200;
                channel = 1;
                pitchX = 0.5;
                pitchY = 0.5;
                pixelOutputTime = 20;
                voltage = 0.0;
                pixelMarginCount = 2;
                zigZag = true;
                FillPercent = 0;
            }
        }

        public RTCScannerConfig stScannerConfig;

        #endregion


        #region Drilling Data Variable

        public enum LayerType : int
        {
            LAYER_DRILLING = 0,
            LAYER_OUTLINE = 1,
            LAYER_THRUHOLE = 2,
            LAYER_MARKING = 3
        }

        public enum ObjectType : int
        {
            OBJECT_POLY = 0,
            OBJECT_RECT = 1,
            OBJECT_CIR = 2,
            OBJECT_LINE = 3,
            OBJECT_ARC = 4
        }

        public enum LaserPowerLevel : int
        {
            EQUAL = 0,
            HIGHER = 1,
            LOWER = 2,
        }

        public enum AlignMarkType : int
        {
            ALIGN_2POINT = 0,               //  2개의 마크를 찾고 보정값 계산
            ALIGN_1STMARK = 1,              //  첫번째 얼라인 마크만 찾기
            ALIGN_2NDMARK = 2,              //  두번째 얼라인 마크만 찾기
        }

        public enum CameraType : int
        {
            CAMERA_HIGH = 0,                //  고해상도 카메라
            CAMERA_LOW = 1,                 //  저해상도 카메라
        }

        public struct stCameraParam
        {
            public string HighRes_SerialNumber;         //  고해상도 카메라 시리얼 넘버
            public int HighRes_Width;
            public int HighRes_Height;

            public string LowRes_SerialNumber;          //  저해상도 카메라 시리얼 넘버
            public int LowRes_Width;
            public int LowRes_Height;
        }
        stCameraParam stCamera = new stCameraParam();

        public struct stLaserPowerParam
        {
            public double dAttenuatorPos_Min;           //  출력 Min 일 때 Attenuator 위치값
            public double dPowerValue_Min;              //  출력 Min 값

            public double dAttenuatorPos_Max;           //  출력 Max 일 때 Attenuator 위치값
            public double dPowerValue_Max;              //  출력 Max 값
        }

        public struct stDrillHole_Line
        {
            public PointD ptStart;                      //  Start 좌표
            public PointD ptEnd;                        //  End 좌표
        }

        public struct stArc
        {
            public double dRadius;                      //  Arc 반지름
            public PointD dCenter;                      //  Arc Center 좌표
            public double dStartAngle;                  //  Arc 시작 각도
            public double dSweepAngle;                  //  Arc 회전 각도
        }

        public struct stAlignMark
        {
            public PointD dAlignMark1;                  //  1번 Align Mark 좌표
            public PointD dAlignMark2;                  //  2번 Align Mark 좌표

            public PointD dRotationCenter;              //  가공 도면 회전 중심 좌표 --> 통상적으로 1번 Align Mark 위치를 회전 중심으로 한다.
            public double dRotationDegree;              //  가공 도면 회전 각도
        }
        //public stAlignMark m_stAlignMark;

        /// <summary>
        /// Job List
        /// </summary>
        /// 
        public struct stJobList
        {
            public string[] m_strJobFullPath;           //  Job File 전체 경로
            public int m_nContinuousJobCount;                     //  Job List 에 추가된 개수
        }
        public stJobList m_stJobList;
        ///
        /// <summary>
        /// Job List - 여기까지
        /// </summary>

        #endregion


        #region Variables

        public double FirstPositionX { set; get; }
        public double FirstPositionY { set; get; }
        public double SecondPositionX { set; get; }
        public double SecondPositionY { set; get; }
        public double FirstMarkPositionX { set; get; }
        public double FirstMarkPositionY { set; get; }
        public double SecondMarkPositionX { set; get; }
        public double SecondMarkPositionY { set; get; }

        public bool LowFocusComplete = false;
        public bool LowAlignComplete = false;
        public bool HighFocusComplete = false;
        public bool HighAlignComplete = false;

        private Vector lowFirstAlignResult = new Vector(0, 0);
        private Vector lowSecondAlignResult = new Vector(0, 0);
        private Vector highFirstAlignResult = new Vector(0, 0);
        private Vector highSecondAlignResult = new Vector(0, 0);
        private Vector FirstMarkPosition = new Vector(0, 0);
        private Vector SecondMarkPosition = new Vector(0, 0);
        private Vector BufferMarkPosition = new Vector(0, 0);

        private int lowVisionAlignCount = 0;
        private int highVisionAlignCount = 0;
        private int alignFailCount = 0;

        private int lowVisionZFocusCount = 0;
        private int highVisionZFocusCount = 0;


        /// <summary>
        /// Scanner 이동 포지션 (Scanner Center 시작, 끝 위치)
        /// </summary>
        ///
        ////  위치 계산 : Wafer XY 크기를 Scanner FOV XY 크기로 나누어 이동 영역 개수를 계산한다.
        //public int ScannerTotalCountX;                  //  Scanner FOV 만큼 X 방향으로 몇 번 이동해야 하는 지
        //public int ScannerTotalCountY;                  //  Scanner FOV 만큼 Y 방향으로 몇 번 이동해야 하는 지

        //private double ScannerStartPositionX;
        //private double ScannerStartPositionY;
        //private double ScannerEndPositionX;
        //private double ScannerEndPositionY;

        //private int Scanner_CurrentCount_X = 0;         //  Scanner 가공 시 X 방향 이동, 최대 이동 회수는 ScannerTotalCountX
        //private int Scanner_CurrentCount_Y = 0;         //  Scanner 가공 시 Y 방향 이동, 최대 이동 회수는 ScannerTotalCountY

        ////  Manual 창에서 입력받는 파라미터
        //public double ManualPage_WaferSize = 0;
        //public double ManualPage_PitchX = 0;
        //public double ManualPage_PitchY = 0;
        //public double ManualPage_OffsetX = 0;
        //public double ManualPage_OffsetY = 0;
        //public double ManualPage_MarginCnt = 0;
        //public double ManualPage_BeamSizeX = 0;
        //public double ManualPage_BeamSizeY = 0;
        //public double ManualPage_PitchCount = 0;
        //public double ManualPage_StageLineCount = 0;

        ////  LLO 시작 Index (사용자에 의해 입력 받음)
        //public int m_nLLO_Work_StartIndex_X = 0;
        //public int m_nLLO_Work_StartIndex_Y = 0;

        ////  LLO 개수 (사용자에 의해 입력 받음)
        //public int m_nLLO_Work_Count_X = 0;
        //public int m_nLLO_Work_Count_Y = 0;
        //public int m_nLLO_Work_Count_withInterval_X = 0;
        //public int m_nLLO_Work_Count_withInterval_Y = 0;

        ////  Block 단위 LLO 일 때, Set 번호
        //public int m_nLLO_Work_Set_Index;           //  Block 단위 가공 시, Set 번호

        ////  LLO 반복 회수
        //public int m_nLLO_Work_Repetition_Count = 0;
        //public int m_nLLO_Work_Current_Count = 0;

        ////  LLO Chip Interval (X, Y 방향으로 몇개의 Chip 마다 가공할 것인지)
        //public int m_nLLO_Work_Interval_X = 0;
        //public int m_nLLO_Work_Interval_Y = 0;
        //public int m_nLLO_Work_Interval_Current_X = 0;
        //public int m_nLLO_Work_Interval_Current_Y = 0;
        //public double m_dLLO_Work_ChipPitch_X = 0.0;
        //public double m_dLLO_Work_ChipPitch_Y = 0.0;

        ////  NG Chip Position List
        //public struct stNgChipPosition              //  NG Chip 위치데이터 구조체
        //{
        //    public int nLLO_Block_X;                //  Block Number (X, 1 ~ 280)
        //    public int nLLO_Block_Y;                //  Block Number (Y, 1 ~ 220)
        //    public int nLLO_Set;                    //  Block Index (1 ~ 28)
        //    public bool bChip_Judge;                //  Chip Judgement (OK, NG)
        //}

        //public stNgChipPosition[] stChipData;       // = new stNgChipPosition[1];
        //public int m_nBlockChipGridData_PageIndex = 0;                                                                   //  GridData 페이지 인덱스
        //public stNgChipPosition[,] stWaferPos;      // = new stNgChipPosition[1, 1];
        //public stNgChipPosition[] stNgLLOPos;       // = new stNgChipPosition[1];
        //public stNgChipPosition[] stNgLLOPos_forGridData;
        //public int nLLO_NgChip_TotalCount { get; set; }                                                             //  2023. 02. 22.  SCH : NG Chip 총 개수

        //private double RtcProcPosX;
        //private double RtcProcPosY;

        //public int m_LaserPowerCal_Count { set; get; }
        //public bool m_bLaserPowerCal_OK { set; get; }
        //public double m_dLaserInitPower { set; get; }

        //public double PowerMeterValue { get; set; }

        //public bool m_bLaserGetStatus_Run { set; get; }

        //public QMC.Common.Stage StageCompenData = new QMC.Common.Stage();
        //public bool m_bStage2DMapDataApply_Success { set; get; }

        ////  SESAM 교체 주기
        //public bool m_bSESAM_ReplacementCycle_Exceed { set; get; }
        //public string m_strSESAM_ReplacementCycle_Remained { set; get; }


        //public enum MapData_Type
        //{
        //    MapData_Stage = 0,
        //    MapData_LCI = 1,
        //}
        //public bool m_bMapDataType_UserChange_Complete { set; get; }               //  Map Data 변경 되었는지? --> 해당 타입의 ACS Global 변수를 1 로 세팅하고, 변경 성공하면 2로 리턴된다.

        //public int m_nLCIMapData_StartIndex_X { get; set; }                        //  LCI Module 사용 시, Map Data 변경 위치 X Index (시작, Left)
        //public int m_nLCIMapData_EndIndex_X { get; set; }                           //  LCI Module 사용 시, Map Data 변경 위치 X Index (종료, Right)
        //public int m_nLCIMapData_StageStartIndex_Y { get; set; }                    //  LCI Module 사용 시, Map Data 변경 위치 Y Index (시작)
        //public int m_nLCIMapData_StageEndIndex_Y { get; set; }                      //  LCI Module 사용 시, Map Data 변경 위치 Y Index (종료)

        #endregion


        #region DPSS Laser (마곡 LGD)

        //public DPSSLaser Cepheus_laser = null;
        //public MyCepheusLaser Cepheus_laser = null;

        #endregion


        #region 스캐너 가공 영역 데이터
        public struct stNgChipPosition_Index        //  NG Chip 위치데이터 구조체
        {
            public int nLLO_Index_X;                //  Index Number (X, 1 ~ )
            public int nLLO_Index_Y;                //  Index Number (Y, 1 ~ )
            public bool bChip_Judge;                //  Chip Judgement (OK, NG)

            public PointD dChipPosition;            //  칩 위치
        }

        public struct stScannerData                 //  스캐너로 가공하기 위한 데이터 구조체
        {
            public PointD dCenter;                  //  스캐너 영역 Center 좌표
            public int nScanArea_StartIndex_X;      //  스캐너 영역 시작 Index X (좌측 상단)
            public int nScanArea_StartIndex_Y;      //  스캐너 영역 시작 Index Y (좌측 상단)
            public int nScanArea_ChipNum_X;         //  스캐너 영역 X 방향 Chip 개수
            public int nScanArea_ChipNum_Y;         //  스캐너 영역 Y 방향 Chip 개수
            public int nScanArea_ChipCount;         //  스캐너 영역 안의 칩 개수

            public stNgChipPosition_Index[] stNgLLOPos;     //  칩 정보 (칩 위치, 가공 여부 등)
            public int nScanArea_NgChipCount;               //  스캐너 영역 안의 불량 칩 개수
        }
        public stScannerData[,] stScannerAreaData;  //  전체 Wafer 에 대한 Scanner 영역별 데이터

        public stNgChipPosition_Index[] stChipData_Index;   // = new stNgChipPosition[1];
        public int m_nIndexChipGridData_PageIndex = 0;                                                                   //  GridData 페이지 인덱스
        public stNgChipPosition_Index[,] stWaferPos_Index;  // = new stNgChipPosition[1, 1];
        public stNgChipPosition_Index[] stNgLLOPos_Index;   // = new stNgChipPosition[1];
        public stNgChipPosition_Index[] stNgLLOPos_Index_forGridData;
        #endregion


        #region Field
        public string m_strLaserComm_ReceivedData;
        public const byte chrESC = 0x1B;
        public const byte chrCR = 0x0D; //\r
        public const byte chrLF = 0x0A; //\n
        public const byte chrExclamation = 0x21;

        //  SLD-200 집진기용 CMD
        public const byte chrENQ = 0x05;    //  ENQ
        public const byte chrACK = 0x06;    //  ACK
        public const byte chrNAK = 0x15;    //  NAK
        public const byte chrEOT = 0x04;    //  EOT
        public const byte chrR = 0x52;      //  Read
        public const byte chrW = 0x57;      //  Write
        public const byte chrX = 0x58;      //  모니터 등록 요구
        public const byte chrY = 0x59;      //  모니터 등록 실행

        public enum EnqType : int
        {
            ENQ_READ = 0,                   //  읽기 요구
            ENQ_WRITE = 1,                  //  쓰기 요구
            ENQ_MON_REGIST_REQ = 2,         //  모니터 등록 요구 (모니터할 필요가 있는 데이터를 미리 지정하여 주기적으로 데이터를 업데이트 하기 위해, n개의 번지를 등록 요구)
            ENQ_MON_READ_REQ = 3            //  모니터 등록 실행 요구 (모니터 등록 요구로 등록된 번지의 데이터 읽기 요구)
        }
        
        public const byte chrSTX = 0x02;
        public const byte chrETX = 0x13;
        public int m_nChillerCommRecvData_ETX_Count { set; get; }

        public XyzCoordinate xyzCoord_WorkStagePos_Align = new XyzCoordinate();
        SettingParameterCollection PosParam_Dispenser;          //  2022. 04. 25.  SCH : 모터 위치 파라미터를 갖다쓰기 위해 선언해봄.
                                                                //static Conveyor conveyor = new Conveyor("");            //  요거 다시해야 함. Conveyor.cs 에 정의된 변수에 접근할 수 있게... 어케 함? -_-
                                                                //  static 으로 선언하면 되긴 헌디.... 맞는건가 -_-
        public MotionFunction MC_Func = new MotionFunction();
        public ACSSPiiPlusAxis ACS_Func = new ACSSPiiPlusAxis();

        public JigAligner m_JigAligner;

        string m_strSendData = "";
        byte[] m_cSendCmd = new byte[11];

        private bool m_bDrillingData_isLine { get; set; }       //  Drilling Hole Data 가 Polyline 구성인지, Line 구성인지?
        #endregion

        #region Property
        public WorkStageConfig Config { set; get; }
        public WorkStageParameterConfig ParamConfig { set; get; }
        public WorkStageRecipe Recipe { set; get; }

        public bool DrillingData_Loaded { set; get; }

        public double m_dRotationCenter_X;
        public double m_dRotationCenter_Y;

        public VisionScale Scale
        {
            set
            {
                Config.VisonCalibratorConfig_HighRes.Scale = value;
            }
            get
            {
                return Config.VisonCalibratorConfig_HighRes.Scale;
            }
        }

        public XyzyStage Stage { set; get; }                         //  MSL SLD-200C, SLD-200U 

        //  저해상도 카메라
        public HIKGigECamera Camera_LowRes { set; get; }
        //public GrabLinkMultiCamCamera Camera { set; get; }
        public VisionCalibrator visionCalibrator_LowRes { set; get; }
        public VisionCompensator visionCompensator_LowRes { set; get; }
        public LaserPitchMoveShotter laserPitchMoveShotter { get; set; }
        public ScannerCompensator scannerCompensator { set; get; }
        public JigAligner jigAligner_LowRes { set; get; }                    //  PAK 검사
        public JigAligner reticleAligner_LowRes { set; get; }                //  Reticle 검사

        public AutoFocuser autoFocuser_LowRes { set; get; }
        public AutoFocusResult autoFocusResult_LowRes;

        //  고해상도 카메라
        public HIKGigECamera Camera_HighRes { set; get; }
        //public GrabLinkMultiCamCamera Camera_LowRes { set; get; }
        public VisionCalibrator visionCalibrator_HighRes { set; get; }
        public VisionCompensator visionCompensator_HighRes { set; get; }
        public JigAligner jigAligner_HighRes { set; get; }                    //  Wafer 검사
        public JigAligner reticleAligner_HighRes { set; get; }                //  Reticle 검사
        public AutoFocuser autoFocuser_HighRes { set; get; }
        public AutoFocusResult autoFocusResult_HighRes;

        public int MAX_IMAGE_WIDTH = 2448;
        public int MAX_IMAGE_HEIGHT = 2048;
        //public int MAX_IMAGE_WIDTH = 2248;            //  현장에서 조정된 Size (Center Offset X : 100, Offset Y : 84)
        //public int MAX_IMAGE_HEIGHT = 1880;
        //public int MAX_IMAGE_WIDTH = 2048;              //  테스트용 카메라
        //public int MAX_IMAGE_HEIGHT = 1536;

        //  패턴 매칭 이미지가 로드 되었는지?
        public bool PatternMatchingImage_Loaded_HighRes = false;
        public bool PatternMatchingImage_Loaded_LowRes = false;
        public bool PatternMatchingImage_Reticle_Loaded_HighRes = false;
        public bool PatternMatchingImage_Reticle_Loaded_LowRes = false;


        //  레시피 변경 시 Calibration Mode 창의 위치값을 변경하기 위해
        public bool m_bParameterSetting_PosData_Reload { set; get; }            //  위치 데이터 다시 로드


        public int m_nProductAlign_CameraType {  set; get; }
        public int m_nProbeCardClamp_TypeB_CylUpDown_StableTime {  set; get; }
        //public Api ACS_Motion { set; get; }

        public bool m_bPAK_Clamp_Handling_byButton {  set; get; }

        //  가공 도면 Align 을 위한 변수
        //public double m_dALIGN_FACTOR_RotationCenter_X { set; get; }                //  전체 가공 도면 회전 중심 X
        //public double m_dALIGN_FACTOR_RotationCenter_Y { set; get; }                //  전체 가공 도면 회전 중심 Y
        //public double m_dALIGN_FACTOR_Offset_X { set; get; }                        //  전체 가공 도면 이동 Offset X
        //public double m_dALIGN_FACTOR_Offset_Y { set; get; }                        //  전체 가공 도면 이동 Offset Y
        //public double m_dALIGN_FACTOR_Theta { set; get; }                           //  전체 가공 도면 회전 (Theta,     기준위치 : Align1 (Thruhole 의 Circle 객체, Description 에 Align1 표시)


        public bool m_bLog_1time;
        public bool m_bLog_1time2;

        //public bool ACS_Motion_isSimulationMode { set; get; }

        private ProgramStates m_nProgramState0;
        private ProgramStates m_nProgramState1;
        private MotorStates m_nMotorState0;
        private MotorStates m_nMotorState1;

        //  쓰레드로 변경 --> 변경 취소. 그냥 타이머 쓴다. Thread 쓰니까 뭐가 막 잘 안됨 ㅡㅡ
        public System.Windows.Forms.Timer timer_MainWork;
        public System.Windows.Forms.Timer timer_SubWork;
        public System.Windows.Forms.Timer timer_Motion_Home;
        public System.Windows.Forms.Timer timer_VisionAlign;
        public System.Windows.Forms.Timer timer_ReticleGlass_Check;

        public bool m_btimer_MainWork_Stop;
        public bool m_btimer_SubWork_Stop;
        public bool m_btimer_Motion_Home_Stop;
        //public bool m_btimer_Calibration_Stop;
        public bool m_btimer_VisionAlign_Stop;
        public bool m_btimer_ReticleGlass_Check_Stop;

        public bool m_bBlink;

        public bool m_bAlignVisionThread_Use;                                                       //  2022. 04. 08.  SCH : Align Vision 을 Thread 로 할지 말지?

        public bool m_bWorkStage_MainCyc_Running;

        public bool m_bInManualMoving_SafetySensor_Detected = false;                                //  단일 동작 중 안전센서를 터치할 경우 모터 Stop                
        public bool m_bInCycleMoving_SafetySensor_Detected = false;                                 //  Cycle 동작 중 안전센서를 터치할 경우 모터 Stop        
        public bool m_bInCycleMoving_ElevZOverTorque_Detected = false;                              //  Cycle 동작 중 엘리베이터 Z축 오버 토크가 발생할 경우 모터 Stop


        public WorkStageParameter workStageParameter { set; get; }
        public LoaderParameter loaderParameter { set; get; }
        public UnloaderParameter unloaderParameter { set; get; }


        //  2025. 01. 22.  SCH : Laser PowerMeter Comm (Exit Position) - COM2
        public SerialCommPowerMeter1Port m_powerMeter_ExitPos_Comm { set; get; }
        public string m_strLaserPowerMeter_ExitPos_Comm_ReceivedData;
        public bool m_bLaserPowerMeter_ExitPos_CommData_Received { set; get; }

        //  2025. 01. 22.  SCH : Laser PowerMeter Comm (Target Position) - COM3
        public SerialCommPowerMeter2Port m_powerMeter_TargetPos_Comm { set; get; }
        public string m_strLaserPowerMeter_TargetPos_Comm_ReceivedData;
        public bool m_bLaserPowerMeter_TargetPos_CommData_Received { set; get; }

        //  2025. 01. 22.  SCH : Motorized Beam Expander Comm - COM4
        public SerialCommBeamExpanderPort m_beamExpander_Comm { set; get; }
        public string m_strBeamExpander_Comm_ReceivedData;
        public bool m_bBeamExpander_CommData_Received { set; get; }

        //  2025. 01. 22.  SCH : Dust Collector Comm (Upper Position) - COM5
        public SerialCommDustCollector1Port m_dustCollector_UpperPos_Comm { set; get; }
        public string m_strDustCollector_UpperPos_Comm_ReceivedData;
        public bool m_bDustCollector_UpperPos_CommData_Received { set; get; }

        //  2025. 01. 22.  SCH : Dust Collector Comm (Lower Position) - COM6
        public SerialCommDustCollector2Port m_dustCollector_LowerPos_Comm { set; get; }
        public string m_strDustCollector_LowerPos_Comm_ReceivedData;
        public bool m_bDustCollector_LowerPos_CommData_Received { set; get; }

        //  2025. 01. 22.  SCH : Electro Pneumatic Regulator Comm - COM7
        //  압력 범위 : -1.3kPa ~ -80kPa (0 ~ 1023)
        public SerialCommElectroPneumaticRegulatorPort m_electroRegulator_Comm { set; get; }
        public string m_strElectroRegulator_Comm_ReceivedData;
        public bool m_bElectroRegulator_CommData_Received { set; get; }

        #endregion



        #region NewForm 을 위한 Teching Position List 변수

        /// <summary>
        /// Config 에서 Teching Position List 가 추가되거나 삭제 되면 여기도 해줘야 함. (이 항목이 Position 배열의 Index 가 되기 때문에)
        /// </summary>
        /// 
        //  Work Stage Teaching Position List
        public enum WorkStage_TeachingPosList : int
        {
            STAGE_OriginPos,
            STAGE_LoadingPos,
            STAGE_LowMagCamPos,
            STAGE_HighMagCamPos,
            STAGE_ProcessingPos,
            Scanner_PMPos,
            Scanner_CalPos,
            HighMagCam_ReticlePos,
            STAGE_UnloadingPos,
            STAGE_SafetyPos,
        }

        public struct stWorkStageAxesPos
        {
            public double Stage_X;                          //  Work Stage X
            public double Stage_Y;                          //  Work Stage Y
        }
        public static stWorkStageAxesPos[] stWorkStageTeachingPos = new stWorkStageAxesPos[System.Enum.GetValues(typeof(WorkStage_TeachingPosList)).Length];

        #endregion



        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            throw new NotImplementedException();
        }

        #region Tick Count Check

        public int TickCount_MainCycle_Start { set; get; }
        public int TickCount_MainCycle_Current { set; get; }
        public int TickCount_MainCycle_Interval { set; get; }

        //System.Diagnostics.Stopwatch sw_DispenserMainCyc = new System.Diagnostics.Stopwatch();
        //System.Diagnostics.Stopwatch sw_DispenserSubCyc = new System.Diagnostics.Stopwatch();

        public enum TickType : int
        {
            TICK_HOME = 0,              //  0 : Initialize
            TICK_MAIN = 1,              //  1 : Main Cycle
            TICK_SUB = 2,               //  2 : Sub Cycle
            TICK_PAUSE = 3,             //  3 : Pause
            TICK_CHECK = 4,             //  4 : 체크용            

            TICK_LDSZ0 = 5,             //  5 : Loader Stacker Z0
            TICK_LDSZ1 = 6,             //  6 : Loader Stacker Z1
            TICK_ULSZ0 = 7,             //  7 : Unloader Stacker Z0
            TICK_ULSZ1 = 8,             //  8 : Unloader Stacker Z1
        }

        public int[,] TickCount_Cycle = new int[System.Enum.GetValues(typeof(TickType)).Length, 2];


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

        #region LaserStatus Status

        private int LaserStatus { set; get; }

        public enum LaserWorkStatus
        {
            WORK_NONE = 0,          //  작업 전
            WORK_ING,               //  작업 중
            WORK_DONE               //  작업 완료
        }

        public void SetLaserWork(int nStatus)
        {
            LaserStatus = nStatus;
        }

        public int GetLaserWork()
        {
            return LaserStatus;
        }

        public int WorkStageCount { set; get; }
        public int WorkStageCount_Index { set; get; }

        #endregion


        //  --> 얘네들 초기화는 "EditMode_SLD100.cs" 의 생성자에서 한다. 여기서는 선언만...
        #region RTC Variable

        public RtcVirtual rtcVirtual { set; get; }
        //public Rtc6SyncAxis rtcSyncAxis { set; get; }
        public Rtc6 rtc6 { set; get; }

        public LaserVirtual laser { set; get; }
        //public SpectraPhysicsTalon laser { set; get; }

        //public MarkerBase marker { set; get; }                    //  Sirius2 꺼
        public MarkerDefault marker { set; get; }                   //  Sirius1 꺼

        public string correctionFile { set; get; }

        ////  Create RTC IO 
        //public RtcDInputExt1 rtcExt1DInput { set; get; }
        //public RtcDOutputExt1 rtcExt1DOutput { set; get; }
        //public RtcDOutputExt2 rtcExt2DOutput { set; get; }

        ////  RTC 5,6 only
        //public RtcDInput2Pin rtcPin2DInput { set; get; }
        //public RtcDOutput2Pin rtcPin2DOutput { set; get; }

        ////  Motor
        //public MotorVirtual motorX { set; get; }
        //public MotorVirtual motorY { set; get; }
        //public MotorVirtual motorZ { set; get; }
        //public IMotor[] motorArray { set; get; }
        //public MotorsDefault motors { set; get; }

        //  Power Meter
        public PowerMeterVirtual pm { set; get; }
        //public PowerMeterCoherentPowerMax powerMeter { set; get; }

        //public PowerMapBase powerMap { set; get; }                        //  Sirius2 꺼

        #endregion


        #region Variable And Function


        public enum VisionType : int
        {
            NONE = 0,
            LOW_VISION = 1,
            HIGH_VISION = 2,
        }



        public int m_nHomeStep { set; get; }                                //  Home Step
        public int m_nHomeAxisCount { set; get; }
        public int m_nTotalAxisCount { set; get; }
        public bool m_bHomeOK { get; set; }
        public enum Home_Step
        {
            None = 0,
            Start,                                                          //  시작

            //  알람이 발생한 축이 있을 경우, Servo Off --> Reset --> Servo On 해야 한다.
            AxisAlarmCheck,                                                 //  서보 축 알람 체크
            AlarmAxisServoOff,                                              //  알람 축 서보 Off
            AlarmAxisServoOffCheck,                                         //  알람 축 서보 Off 확인
            AlarmAxisAlarmResetOn,                                          //  알람 축 리셋 신호 On
            AlarmAxisAlarmResetOff,                                         //  알람 축 리셋 신호 Off (30ms delay 후 Off)
            AlarmAxisServoOn,                                               //  알람 축 서보 On
            AlarmAxisServoOnCheck,                                          //  알람 축 서보 On 확인

            ScannerZ_TransferZ_HomeStart,                                   //  Scanner Z 축, Loader Z 축, Unloader Z 축 홈 실행
            ScannerZ_TransferZ_HomeCompleteCheck,                           //  Scanner Z 축, Loader Z 축, Unloader Z 축 홈 완료 체크

            Remained_AllAxis_HomeStart,                                     //  나머지 축 전체 홈 실행
            Remained_AllAxis_HomeCompleteCheck,                             //  나머지 축 전체 홈 완료 체크

            All_Z_Move_ReadyPos,                                            //  전체 Z 축, 대기 위치로 이동
            All_Z_Move_ReadyPos_DoneCheck,                                  //  전체 Z 축, 대기 위치로 이동 완료 체크

            All_XY_Move_ReadyPos,                                           //  전체 XY 축 대기 위치로 이동 
            All_XY_Move_ReadyPos_DoneCheck,                                 //  전체 XY 축 대기 위치로 이동 완료 확인

            All_StackerZ_Move_ReadyPos,                                     //  전체 Stacker 축 모듈 PickUp, PutDown 높이로 이동 
            All_StackerZ_Move_ReadyPos_DoneCheck,                           //  전체 Stacker 축 모듈 PickUp, PutDown 높이로 이동 완료 확인

            Complete                                                        //  완료
        }



        public int m_nStackerZ_Step { set; get; }                           //  Stacker Z Step
        public enum StackerZ_Step
        {
            None = 0,
            Start,                                                          //  시작

            StackerZ_MoveDown_FullSensorOff,                                //  Full 센서가 Off 되는 위치까지 이동 (고속)
            StackerZ_MoveDown_FullSensorOffCheck,                           //  Full 센서가 Off 되는 위치까지 이동 완료 체크

            StackerZ_MoveUp_FullSensorOn,                                   //  Full 센서가 On 되는 위치까지 이동 (고속)
            StackerZ_MoveUp_FullSensorOnCheck,                              //  Full 센서가 On 되는 위치까지 이동 완료 체크

            StackerZ_MoveSlowDown_FullSensorOff,                            //  Full 센서가 Off 되는 위치까지 이동 (저속)
            StackerZ_MoveSlowDown_FullSensorOffCheck,                       //  Full 센서가 Off 되는 위치까지 이동 완료 체크

            StackerZ_MoveSlowUp_FullSensorOn,                               //  Full 센서가 On 되는 위치까지 이동 (저속 / 2)
            StackerZ_MoveSlowUp_FullSensorOnCheck,                          //  Full 센서가 On 되는 위치까지 이동 완료 체크

            Complete                                                        //  완료
        }



        public int m_nWorkStage_Move_Step { set; get; }                     //  Work Stage Move Step

        public int m_nWorkStageMoveType { set; get; }                       //  Work Stage Move Type
        public enum WorkStageMoveType : int
        {
            MoveTo_LoadingPos = 0,                                          //  Module Loading Pos Move
            MoveTo_UnloadingPos,                                            //  Module Unloading Pos Move
            MoveTo_GlassCleaningPos,                                        //  Scanner Lens Glass Cleaning Pos Move
            MoveTo_CalSheetChangePos,                                       //  Scanner Cal-Sheet Change Pos Move
            MoveTo_ScannerCenterPos,                                        //  Stage and Scanner Center Pos Move
            MoveTo_CameraCenterPos,                                         //  Stage and Camera Center Pos Move
            MoveTo_CameraReticleGlassPos,                                   //  Camera and Reticle Glass Pos Move
            MoveTo_PowerCheckPos,                                           //  Laser Power Check Pos Move
            MoveTo_FrontPos,                                                //  Front Pos Move
            MoveTo_BackwardPos,                                             //  Backward Pos Move
        }

        public enum WorkStage_Move_Step
        {
            None = 0,

            Start,                                                              //  시작


            Process_Type_Check,                                                 //  동작 타입 체크 (Module Loading Pos, Module Unloading Pos, Scanner Lens Glass Cleaning Pos, Scanner CalSheet Change Pos,
                                                                                //                  StageAndScanner Center Pos, StageAndCamera Center Pos, CameraAndReticle Glass Pos, Front Pos, Backward Pos)


            /// <summary>
            /// Module Loading Pos Move - 시작
            /// </summary>
            ToLoadingPos_Condition_Check,                                       //  Work Stage 가 Module Loading 위치로 이동할 수 있는 조건 체크 (Stage Vacuum Off Check [필수 아님], Drilling Cycle : None, Loader Transfer Cycle : None)

            ToLoadingPos_ScannerZ_Move_ReadyPos,                                //  Scanner Z 축, 대기 위치로 이동
            ToLoadingPos_ScannerZ_Move_ReadyPos_DoneCheck,                      //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToLoadingPos_StageXY_Move_LoadingPos,                               //  Stage XY 축, Module Loading 위치로 이동
            ToLoadingPos_StageXY_Move_LoadingPos_DoneCheck,                     //  Stage XY 축, Module Loading 위치로 이동 완료 확인
            /// <summary>
            /// Module Loading Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Module Unloading Pos Move - 시작
            /// </summary>
            ToUnloadingPos_Condition_Check,                                     //  Work Stage 가 Module Unloading 위치로 이동할 수 있는 조건 체크 (Stage Vacuum On Check [필수 아님], Drilling Cycle : None, Unloader Transfer Cycle : None)

            ToUnloadingPos_ScannerZ_Move_ReadyPos,                              //  Scanner Z 축, 대기 위치로 이동
            ToUnloadingPos_ScannerZ_Move_ReadyPos_DoneCheck,                    //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToUnloadingPos_StageXY_Move_UnloadingPos,                           //  Stage XY 축, Module Unloading 위치로 이동
            ToUnloadingPos_StageXY_Move_UnloadingPos_DoneCheck,                 //  Stage XY 축, Module Unloading 위치로 이동 완료 확인
            /// <summary>
            /// Module Unloading Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Scanner Lens Glass Cleaning Pos Move - 시작
            /// </summary>
            ToGlassCleaningPos_Condition_Check,                                 //  Work Stage 가 Scanner Lens Glass Cleaning 위치로 이동할 수 있는 조건 체크 (Drilling Cycle : None)

            ToGlassCleaningPos_ScannerZ_Move_ReadyPos,                          //  Scanner Z 축, 대기 위치로 이동
            ToGlassCleaningPos_ScannerZ_Move_ReadyPos_DoneCheck,                //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToGlassCleaningPos_StageXY_Move_GlassCleaningPos,                   //  Stage XY 축, Scanner Lens Glass Cleaning 위치로 이동
            ToGlassCleaningPos_StageXY_Move_GlassCleaningPos_DoneCheck,         //  Stage XY 축, Scanner Lens Glass Cleaning 위치로 이동 완료 확인
            /// <summary>
            /// Scanner Lens Glass Cleaning Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Scanner Cal-Sheet Change Pos Move - 시작
            /// </summary>
            ToCalSheetChangePos_Condition_Check,                                //  Work Stage 가 Scanner Cal-Sheet Change 위치로 이동할 수 있는 조건 체크 (Drilling Cycle : None)

            ToCalSheetChangePos_ScannerZ_Move_ReadyPos,                         //  Scanner Z 축, 대기 위치로 이동
            ToCalSheetChangePos_ScannerZ_Move_ReadyPos_DoneCheck,               //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToCalSheetChangePos_StageXY_Move_CalSheetChangePos,                 //  Stage XY 축, Scanner Cal-Sheet Change 위치로 이동
            ToCalSheetChangePos_StageXY_Move_CalSheetChangePos_DoneCheck,       //  Stage XY 축, Scanner Cal-Sheet Change 위치로 이동 완료 확인
            /// <summary>
            /// Scanner Cal-Sheet Change Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Stage and Scanner Center Pos Move - 시작
            /// </summary>
            ToScannerCenterPos_Condition_Check,                                 //  Work Stage 가 Scanner Center 위치로 이동할 수 있는 조건 체크 (Drilling Cycle : None)

            ToScannerCenterPos_ScannerZ_Move_ReadyPos,                          //  Scanner Z 축, 대기 위치로 이동
            ToScannerCenterPos_ScannerZ_Move_ReadyPos_DoneCheck,                //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToScannerCenterPos_StageXY_Move_StageScannerCenterPos,              //  Stage XY 축, Scanner Center 위치로 이동
            ToScannerCenterPos_StageXY_Move_StageScannerCenterPos_DoneCheck,    //  Stage XY 축, Scanner Center 위치로 이동 완료 확인
            /// <summary>
            /// Stage and Scanner Center Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Stage and Camera Center Pos Move - 시작
            /// </summary>
            ToCameraCenterPos_Condition_Check,                                  //  Work Stage 가 Camera Center 위치로 이동할 수 있는 조건 체크 (Drilling Cycle : None, 고해상도와 저해상도 선택)

            ToCameraCenterPos_ScannerZ_Move_ReadyPos,                           //  Scanner Z 축, 대기 위치로 이동
            ToCameraCenterPos_ScannerZ_Move_ReadyPos_DoneCheck,                 //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToCameraCenterPos_StageXY_Move_StageCameraCenterPos,                //  Stage XY 축, Camera Center 위치로 이동
            ToCameraCenterPos_StageXY_Move_StageCameraCenterPos_DoneCheck,      //  Stage XY 축, Camera Center 위치로 이동 완료 확인
            /// <summary>
            /// Stage and Camera Center Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Camera and Reticle Glass Center Pos Move - 시작
            /// </summary>
            ToReticleCenterPos_Condition_Check,                                 //  Camera 가 Reticle Glass Center 위치로 이동할 수 있는 조건 체크 (Drilling Cycle : None, 고해상도와 저해상도 선택)

            ToReticleCenterPos_ScannerZ_Move_ReadyPos,                          //  Scanner Z 축, 대기 위치로 이동
            ToReticleCenterPos_ScannerZ_Move_ReadyPos_DoneCheck,                //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToReticleCenterPos_StageXY_Move_CameraReticleCenterPos,             //  Stage XY 축, Camera 가 Reticle Glass Center 위치로 이동
            ToReticleCenterPos_StageXY_Move_CameraReticleCenterPos_DoneCheck,   //  Stage XY 축, Camera 가 Reticle Glass Center 위치로 이동 완료 확인
            /// <summary>
            /// Camera and Reticle Glass Center Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Laser Power Check Pos Move - 시작
            /// </summary>
            ToPowerCheckPos_Condition_Check,                                    //  PowerMeter 가 Scanner Center 위치로 이동할 수 있는 조건 체크 (Drilling Cycle : None)

            ToPowerCheckPos_ScannerZ_Move_ReadyPos,                             //  Scanner Z 축, 대기 위치로 이동
            ToPowerCheckPos_ScannerZ_Move_ReadyPos_DoneCheck,                   //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToPowerCheckPos_StageXY_Move_PowerMeterScannerCenterPos,            //  Stage XY 축, Power Meter 가 Scanner Center 위치로 이동
            ToPowerCheckPos_StageXY_Move_PowerMeterScannerCenterPos_DoneCheck,  //  Stage XY 축, Power Meter 가 Scanner Center 위치로 이동 완료 확인
            /// <summary>
            /// Laser Power Check Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// To Front Pos Move - 시작
            /// </summary>
            ToFrontPos_Condition_Check,                                         //  Work Stage 가 장비 앞쪽으로 이동할 수 있는 조건 체크 (Drilling Cycle : None)

            ToFrontPos_ScannerZ_Move_ReadyPos,                                  //  Scanner Z 축, 대기 위치로 이동
            ToFrontPos_ScannerZ_Move_ReadyPos_DoneCheck,                        //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToFrontPos_StageXY_Move_FrontPos,                                   //  Stage XY 축, Front 위치로 이동
            ToFrontPos_StageXY_Move_FrontPos_DoneCheck,                         //  Stage XY 축, Front 위치로 이동 완료 확인
            /// <summary>
            /// To Front Pos Move - 완료
            /// </summary>
            /// 


            /// <summary>
            /// To Back Pos Move - 시작
            /// </summary>
            ToBackPos_Condition_Check,                                          //  Work Stage 가 장비 뒤쪽으로 이동할 수 있는 조건 체크 (Drilling Cycle : None)

            ToBackPos_ScannerZ_Move_ReadyPos,                                   //  Scanner Z 축, 대기 위치로 이동
            ToBackPos_ScannerZ_Move_ReadyPos_DoneCheck,                         //  Scanner Z 축, 대기 위치로 이동 완료 확인

            ToBackPos_StageXY_Move_FrontPos,                                    //  Stage XY 축, Back 위치로 이동
            ToBackPos_StageXY_Move_FrontPos_DoneCheck,                          //  Stage XY 축, Back 위치로 이동 완료 확인
            /// <summary>
            /// To Back Pos Move - 완료
            /// </summary>
            /// 


            Complete                                                            //  완료
        }



        public int m_nLaserMask_Move_Step { set; get; }                         //  Laser Mask Move Step
        public enum LaserMask_Move_Step
        {
            None = 0,
            Start,                                                          //  시작

            LaserMask_Move_Condition_Check,                                 //  Laser Mask 이동 조건 체크 (Drilling Cycle : None, Laser Shutter Close, Laser Off [필수 아님]) 

            MaskY_Move_MaskPos,                                             //  이동해야 하는 Mask 위치까지 이동
            MaskY_Move_MaskPos_DoneCheck,                                   //  이동해야 하는 Mask 위치까지 이동 완료 체크

            Complete                                                        //  완료
        }

        #endregion


        #region Reticle Glass Check

        public int m_nReticleCheck_Step_forALIGN;               //  레시피 변경 시 무조건 레티클 확인하도록 한다.
        public enum ReticleCheck_Step
        {
            None = 0,
            HighResCam_Complete,
            LowResCam_Complete,
        }
        
        public int m_nReticleCheck_HighResCam_Step { set; get; }                    //  Reticle Glass Check Step (HighRes Camera)

        //  Pause 관련 변수
        public bool m_bReticleCheck_Pause_Start;                //  안전센서를 Touch 하여 Pause 상태가 시작되었는지
        public int m_nReticleCheck_Continue_Step;               //  안전센서 해제 후 다시 진행해야 하는 Step

        public enum ReticleCheck_HighResCam_Step
        {
            None = 0,


            SafetySensorPause_Start,                            //  안전 센서에 의한 Pause 상태일 때 여기로 들어옴
            SafetySensorPause_SafetySensor_Off_Start,           //  안전 센서가 Off 되면 Count 시작
            SafetySensorPause_SafetySensor_Off_Count,           //  안전 센서가 Off 상태에서 Count 진행, 도중에 센서가 On 이 되면 SafetySensorPause_SafetySensor_Off_Start 단계로 다시 감


            Start,                                              //  시작

            ReticleProcess_Condition_Check,                     //  Reticle 확인 Process 조건 확인 (Clamp Open)

            VisionXY_Move_ReadyPos,                             //  Vision XY 축, 대기 위치로 이동
            VisionXY_Move_ReadyPos_DoneCheck,                   //  Vision XY 축, 대기 위치로 이동 완료 확인

            UVW_Move_HighResCam_ReticlePos,                     //  UVW 축, Reticle Glass 확인 위치로 이동
            UVW_Move_HighResCam_ReticlePos_DoneCheck,           //  UVW 축, Reticle Glass 확인 위치로 이동 완료 확인

            ElevZ_Move_HighResCam_ReticlePos,                   //  Elev. Z 축, Reticle Glass 확인 위치로 이동 
            ElevZ_Move_HighResCam_ReticlePos_DoneCheck,         //  Elev. Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionZ_Move_ReticlePos,                            //  Vision Z 축, Reticle Glass 확인 위치로 이동
            VisionZ_Move_ReticlePos_DoneCheck,                  //  Vision Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionXY_Move_ReticlePos,                           //  Vision XY 축, Reticle Glass 확인 위치로 이동
            VisionXY_Move_ReticlePos_DoneCheck,                 //  Vision XY 축, Reticle Glass 확인 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        public int m_nReticleCheck_LowResCam_Step { set; get; }                    //  Reticle Glass Check Step (LowRes Camera)
        public enum ReticleCheck_LowResCam_Step
        {
            None = 0,


            SafetySensorPause_Start,                            //  안전 센서에 의한 Pause 상태일 때 여기로 들어옴
            SafetySensorPause_SafetySensor_Off_Start,           //  안전 센서가 Off 되면 Count 시작
            SafetySensorPause_SafetySensor_Off_Count,           //  안전 센서가 Off 상태에서 Count 진행, 도중에 센서가 On 이 되면 SafetySensorPause_SafetySensor_Off_Start 단계로 다시 감


            Start,                                              //  시작

            ReticleProcess_Condition_Check,                     //  Reticle 확인 Process 조건 확인 (Clamp Open)

            VisionXY_Move_ReadyPos,                             //  Vision XY 축, 대기 위치로 이동
            VisionXY_Move_ReadyPos_DoneCheck,                   //  Vision XY 축, 대기 위치로 이동 완료 확인

            UVW_Move_LowResCam_ReticlePos,                      //  UVW 축, Reticle Glass 확인 위치로 이동
            UVW_Move_LowResCam_ReticlePos_DoneCheck,            //  UVW 축, Reticle Glass 확인 위치로 이동 완료 확인

            ElevZ_Move_LowResCam_ReticlePos,                    //  Elev. Z 축, Reticle Glass 확인 위치로 이동 
            ElevZ_Move_LowResCam_ReticlePos_DoneCheck,          //  Elev. Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionZ_Move_ReticlePos,                            //  Vision Z 축, Reticle Glass 확인 위치로 이동
            VisionZ_Move_ReticlePos_DoneCheck,                  //  Vision Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionXY_Move_ReticlePos,                           //  Vision XY 축, Reticle Glass 확인 위치로 이동
            VisionXY_Move_ReticlePos_DoneCheck,                 //  Vision XY 축, Reticle Glass 확인 위치로 이동 완료 확인

            Complete                                            //  완료
        }
        #endregion


        #region Single Action

        public int m_nSafetyPos_Move_Step { set; get; }                 //  Safety Position Move Step

        //  Pause 관련 변수
        public bool m_bSafetyPos_Pause_Start;                   //  안전센서를 Touch 하여 Pause 상태가 시작되었는지
        public int m_nSafetyPos_Continue_Step;                  //  안전센서 해제 후 다시 진행해야 하는 Step

        public enum SafetyPos_Move_Step
        {
            None = 0,


            SafetySensorPause_Start,                            //  안전 센서에 의한 Pause 상태일 때 여기로 들어옴
            SafetySensorPause_SafetySensor_Off_Start,           //  안전 센서가 Off 되면 Count 시작
            SafetySensorPause_SafetySensor_Off_Count,           //  안전 센서가 Off 상태에서 Count 진행, 도중에 센서가 On 이 되면 SafetySensorPause_SafetySensor_Off_Start 단계로 다시 감


            Start,                                              //  시작

            VisionXY_Move_ReadyPos,                             //  Vision XY 축, 대기 위치로 이동
            VisionXY_Move_ReadyPos_DoneCheck,                   //  Vision XY 축, 대기 위치로 이동 완료 확인

            VisionZ_Move_ReadyPos,                              //  Vision Z 축, 대기 위치로 이동
            VisionZ_Move_ReadyPos_DoneCheck,                    //  Vision Z 축, 대기 위치로 이동 완료 확인

            ElevZ_Move_ReadyPos,                                //  Elev. Z 축, 대기 위치로 이동 
            ElevZ_Move_ReadyPos_DoneCheck,                      //  Elev. Z 축, 대기 위치로 이동 완료 확인

            UVW_Move_ReadyPos,                                  //  UVW 축, 대기 위치로 이동
            UVW_Move_ReadyPos_DoneCheck,                        //  UVW 축, 대기 위치로 이동 완료 확인

            Complete                                            //  완료
        }


        public bool m_bWorkStage_MainCyc_Complete { set; get; }         //  Wafer ProbeCard Align Cycle 완료 확인
                                                                           //  WorkStatus 를 "WORK_DONE" 으로 세팅
        public int m_nWorkStage_MainStep { set; get; }                  //  Wafer ProbeCard Align Main Cycle Step
        public double[] m_dCmdPos = new double[2];
        public double[] m_dActPos = new double[2];

        //public bool m_bFindFirstAlignMarkOnly { set; get; }               //  내부 드릴링 가공할 때, 1번 Align Mark 위치를 검사한 후 드릴링 그룹 Center 위치로 이동하기 위해 추가됨
        public int m_nFindAlignMarkType { set; get; }                       //  얼라인 마크 검출 방식 (0: 2개 찾고 보정값 계산       1: 1번째 마크만 찾기       2: 2번째 마크만 찾기)
        public int m_nAlignMarkSearch_Count { set; get; }                   //  얼라인 마크 찾기 회수

        public enum AlignParam : int
        {
            POS_ALIGNMARK1 = 0,
            POS_ALIGNMARK2 = 1,
            POS_ROTCENTER = 2,

            RESULT_HighResVISION_FIRSTMARKPOS = 3,
            RESULT_HighResVISION_SECONDMARKPOS = 4,
            RESULT_HighResVISION_THETA = 5,
            RESULT_HighResVISION_POSOFFSET = 6,

            RESULT_LowResVISION_FIRSTMARKPOS = 7,
            RESULT_LowResVISION_SECONDMARKPOS = 8,
            RESULT_LowResVISION_THETA = 9,
            RESULT_LowResVISION_POSOFFSET = 10,

            RESULTPOS_FIRSTMARK_AVG = 11,                                    //  평균 계산을 위한 변수
            RESULTTHETA_AVG = 12,                                            //  평균 계산을 위한 변수
        }



        public int m_nAlignMark_FoundCount { set; get; }                            //  Align Mark 찾은 개수
        public XyCoordinate[] m_forAlign_Data = new XyCoordinate[System.Enum.GetValues(typeof(AlignParam)).Length];        //  Align 용 회전 중심 좌표 (1번 마크)

        //public int m_nUnfollow_TryCount { set; get; }                       //  Motion Unfollow 시도 회수
        //public bool m_bUnfollow_Ret { set; get; }                           //  Motion Unfollow 함수 리턴값

        //  얼라인 마크 위치 평균 계산
        public int m_nProbeCard_AlignMarkCount_Max;                         //  프로브 카드 얼라인 마크 검사 최대 회수. (평균 계산용)
        public int m_nWafer_AlignMarkCount_Max;                             //  웨이퍼 얼라인 마크 검사 최대 회수. (평균 계산용)
        public int m_nProbeCard_AlignMark_Count;                            //  프로브 카드 얼라인 마크 개수. (평균 계산용)
        public int m_nWafer_AlignMark_Count;                                //  웨이퍼 얼라인 마크 개수. (평균 계산용)
        public PointD[] m_pProbeCard_AlignMarkPosition_Sum;                 //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pWafer_AlignMarkPosition_Sum;                     //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pProbeCard_AlignMarkPosition_Average;             //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pWafer_AlignMarkPosition_Average;                 //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)

        public PointD[] m_pProbeCard_AlignMark_AddPosition_Sum;             //  추가 위치, 프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pWafer_AlignMark_AddPosition_Sum;                 //  추가 위치, 웨이퍼 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pProbeCard_AlignMark_AddPosition_Average;         //  추가 위치, 프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pWafer_AlignMark_AddPosition_Average;             //  추가 위치, 웨이퍼 얼라인 마크 위치 누적. (평균 계산용)

        public PointD[] m_pProbeCard_AlignMarkPosition_forVerify_Sum;                 //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pWafer_AlignMarkPosition_forVerify_Sum;                     //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pProbeCard_AlignMarkPosition_forVerify_Average;             //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
        public PointD[] m_pWafer_AlignMarkPosition_forVerify_Average;                 //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)


        public double m_dAlignPosition_AverageCheck_Range;                  //  얼라인 마크 위치 평균값 신뢰성 확인

        //  레티클 얼라인 마크 위치 평균 계산
        public int m_nReticle_HighRes_AlignMarkCount_Max;                   //  레티클 글래스 고해상도 카메라 얼라인 마크 검사 최대 회수. (평균 계산용)
        public int m_nReticle_LowRes_AlignMarkCount_Max;                    //  레티클 글래스 저해상도 카메라 얼라인 마크 검사 최대 회수. (평균 계산용)
        public int m_nReticle_HighRes_AlignMark_Count;                      //  레티클 글래스 고해상도 카메라 얼라인 마크 개수. (평균 계산용)
        public int m_nReticle_LowRes_AlignMark_Count;                       //  레티클 글래스 저해상도 카메라 얼라인 마크 개수. (평균 계산용)
        public PointD m_pReticle_HighRes_AlignMarkPosition_Sum;             //  레티클 글래스 고해상도 카메라 얼라인 마크 위치 누적. (평균 계산용)
        public PointD m_pReticle_LowRes_AlignMarkPosition_Sum;              //  레티클 글래스 저해상도 카메라 얼라인 마크 위치 누적. (평균 계산용)
        public PointD m_pReticle_HighRes_AlignMarkPosition_Average;         //  레티클 글래스 고해상도 카메라 얼라인 마크 위치 누적. (평균 계산용)
        public PointD m_pReticle_LowRes_AlignMarkPosition_Average;          //  레티클 글래스 저해상도 카메라 얼라인 마크 위치 누적. (평균 계산용)

        public double m_dReticleAlignPosition_AverageCheck_Range;           //  레티클 글래스 얼라인 마크 위치 평균값 신뢰성 확인

        public int m_nReticleAlign_Count { set; get; }                      //  Reticle Align 시도 회수

        //  Pause 관련 변수
        public bool m_bWorkStage_Pause_Start;                         //  안전센서를 Touch 하여 Pause 상태가 시작되었는지
        public int m_nWorkStage_Continue_Step;                        //  안전센서 해제 후 다시 진행해야 하는 Step

        //  메인 UI 에서 PAK 카메라 초점을 변경했는지 확인하는 변수. 초점을 변경했다면 그 높이값으로 PAK 얼라인 진행. 패킹 완료후에는 다시 원래 초점 높이에서 PAK 얼라인을 진행하도록 한다.
        public int m_nAlign_PosError_RetryCount {  set; get; }              //  위치값 오류로 인해 재시도 되는 회수 (5회 이상 발생하면 장비를 멈추도록 한다.)

        public enum WorkStage_Step
        {
            None = 0,


            SafetySensorPause_Start,                                        //  안전 센서에 의한 Pause 상태일 때 여기로 들어옴
            SafetySensorPause_SafetySensor_Off_Start,                       //  안전 센서가 Off 되면 Count 시작
            SafetySensorPause_SafetySensor_Off_Count,                       //  안전 센서가 Off 상태에서 Count 진행, 도중에 센서가 On 이 되면 SafetySensorPause_SafetySensor_Off_Start 단계로 다시 감


            Start,                                                          //  시작


            Align_Condition_Check,                                          //  Align 조건 확인 (Thin Chuck Exist)


            AlignStart_OP_Check,                                            //  Align 시작할 때 Leak Check Valve 상태를 작업자가 체크
            AlignStart_OP_Check_Confirm,                                    //  Align 시작할 때 Leak Check Valve 상태를 작업자가 체크 완료 확인


            ThinChuck_Vacuum_On,                                            //  Thin Chuck Vacuum On
            ThinChuck_Vacuum_On_Check,                                      //  Thin Chuck Vacuum On 확인

            Wafer_Vacuum_On,                                                //  Wafer Vacuum On
            Wafer_Vacuum_On_Check,                                          //  Wafer Vacuum On 확인



            __PAK_LeakCheck_Start,                                          //  PAK 공압 라인 막힘 검사 시작
            __PAK_LeakCheck_Complete,                                       //  PAK 공압 라인 막힘 검사 완료



            VisionXYZ_Move_ReadyPos,                                        //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                              //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            UVW_Move_WaferLoadingPos,                                       //  UVW 축, Wafer Loading 위치로 이동
            UVW_Move_WaferLoadingPos_DoneCheck,                             //  UVW 축, Wafer Loading 위치로 이동 완료 확인

            ElevZ_Move_WaferAlignPos,                                       //  Elev. Z 축, Wafer Align 위치로 이동
            ElevZ_Move_WaferAlignPos_DoneCheck,                             //  Elev. Z 축, Wafer Align 위치로 이동 완료 확인



            __ProbeCard_Tilt_Check_Start,                                   //  Probe Card Tilt Check 시작

            /// <summary>
            /// 세로 방향 얼라인 - 시작
            /// </summary>
            ProbeCard_Tilt_Check_TopMarkFind_Ready,                         //  Top 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter,               //  VisionXYZ 축, ProbeCard Tilt Check (Top, Center) 위치로 이동
            VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter_DoneCheck,     //  VisionXYZ 축, ProbeCard Tilt Check (Top, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter_StableTime,    //  VisionXYZ 축, ProbeCard Tilt Check (Top, Center) 위치로 이동 후 안정화 시간

            ProbeCard_Tilt_Check_TopMarkFind,                               //  Top 위치 얼라인 마크 찾기
            ProbeCard_Tilt_Check_TopMarkResultCheck,                        //  Top 위치 얼라인 마크 찾기 결과 확인

            ProbeCard_Tilt_Check_BottomMarkFind_Ready,                      //  Bottom 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter,            //  VisionXYZ 축, ProbeCard Tilt Check (Bottom, Center) 위치로 이동
            VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter_DoneCheck,  //  VisionXYZ 축, ProbeCard Tilt Check (Bottom, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter_StableTime, //  VisionXYZ 축, ProbeCard Tilt Check (Bottom, Center) 위치로 이동 후 안정화 시간

            ProbeCard_Tilt_Check_BottomMarkFind,                            //  Bottom 위치 얼라인 마크 찾기
            ProbeCard_Tilt_Check_BottomMarkResultCheck,                     //  Bottom 위치 얼라인 마크 찾기 결과 확인

            ProbeCard_Tilt_Data_Calc,                                       //  Tilt 데이터 계산
            /// <summary>
            /// 세로 방향 얼라인 - 끝
            /// </summary>

            /// <summary>
            /// 가로 방향 얼라인도 추가로 진행하여, 세로 방향 각도와 가로 방향 각도의 평균값을 사용하여 얼라인을 진행한다. - 시작
            /// </summary>
            /// 
            ProbeCard_Hor_Tilt_Check_Start,                                     //  Probe Card Horizontal Tilt Check 시작

            ProbeCard_Hor_Tilt_Check_LeftMarkFind_Ready,                        //  Left 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_ProbeCard_Hor_Tilt_CheckPos_LeftCenter,              //  VisionXYZ 축, ProbeCard Tilt Check (Left, Center) 위치로 이동
            VisionXYZ_Move_ProbeCard_Hor_Tilt_CheckPos_LeftCenter_DoneCheck,    //  VisionXYZ 축, ProbeCard Tilt Check (Left, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_ProbeCard_Hor_Tilt_CheckPos_LeftCenter_StableTime,   //  VisionXYZ 축, ProbeCard Tilt Check (Left, Center) 위치로 이동 후 안정화 시간

            ProbeCard_Hor_Tilt_Check_LeftMarkFind,                              //  Left 위치 얼라인 마크 찾기
            ProbeCard_Hor_Tilt_Check_LeftMarkResultCheck,                       //  Left 위치 얼라인 마크 찾기 결과 확인

            ProbeCard_Hor_Tilt_Check_RightMarkFind_Ready,                       //  Right 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_ProbeCard_Hor_Tilt_CheckPos_RightCenter,             //  VisionXYZ 축, ProbeCard Tilt Check (Right, Center) 위치로 이동
            VisionXYZ_Move_ProbeCard_Hor_Tilt_CheckPos_RightCenter_DoneCheck,   //  VisionXYZ 축, ProbeCard Tilt Check (Right, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_ProbeCard_Hor_Tilt_CheckPos_RightCenter_StableTime,  //  VisionXYZ 축, ProbeCard Tilt Check (Right, Center) 위치로 이동 후 안정화 시간

            ProbeCard_Hor_Tilt_Check_RightMarkFind,                             //  Right 위치 얼라인 마크 찾기
            ProbeCard_Hor_Tilt_Check_RightMarkResultCheck,                      //  Right 위치 얼라인 마크 찾기 결과 확인

            ProbeCard_Hor_Tilt_Data_Calc,                                       //  Tilt 데이터 계산
            /// <summary>
            /// 가로 방향 얼라인도 추가로 진행하여, 세로 방향 각도와 가로 방향 각도의 평균값을 사용하여 얼라인을 진행한다. - 끝
            /// </summary>

            __ProbeCard_Tilt_Check_Complete,                                //  Probe Card Tilt Check 완료



            __ProbeCard_XYAlign_Start,                                      //  ProbeCard XY Align 시작

            ProbeCardXYAlign_TopMarkFind_Ready,                             //  Top 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter,                   //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동
            VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter_DoneCheck,         //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter_StableTime,        //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동 후 안정화 시간

            ProbeCardXYAlign_TopMarkFind,                                   //  Top 위치 얼라인 마크 찾기
            ProbeCardXYAlign_TopMarkResultCheck,                            //  Top 위치 얼라인 마크 찾기 결과 확인

            ProbeCardXYAlignData_Calc,                                      //  XY 얼라인 데이터 계산

            ProbeCardXYAlign_CorrectionMove,                                //  Top 위치 XY 보정 이동 (카메라 Center 로)
            ProbeCardXYAlign_CorrectionMove_DoneCheck,                      //  Top 위치 XY 보정 이동 완료 확인

            ProbeCardXYAlign_Retry,                                         //  Vision XY 값 확인 -> Config 에 설정된 값보다 클 경우 Retry

            __ProbeCard_XYAlign_Complete,                                   //  ProbeCard XY Align 완료



            __Wafer_ThetaAlign_Start,                                       //  Wafer Theta Align 시작

            WaferAlign_TopMarkFind_Ready,                                   //  Top 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_WaferAlignPos_TopCenter,                         //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동
            VisionXYZ_Move_WaferAlignPos_TopCenter_DoneCheck,               //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_WaferAlignPos_TopCenter_StableTime,              //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동 후 안정화 시간 --> 여기서 카메라 Offset 확인 추가

            WaferAlign_TopMarkFind,                                         //  Top 위치 얼라인 마크 찾기
            WaferAlign_TopMarkResultCheck,                                  //  Top 위치 얼라인 마크 찾기 결과 확인


            WaferAlign_BottomMarkFind_Ready,                                //  Bottom 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_WaferAlignPos_BottomCenter,                      //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동
            VisionXYZ_Move_WaferAlignPos_BottomCenter_DoneCheck,            //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_WaferAlignPos_BottomCenter_StableTime,           //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동 후 안정화 시간 --> 여기서 카메라 Offset 확인 추가

            WaferAlign_BottomMarkFind,                                      //  Bottom 위치 얼라인 마크 찾기
            WaferAlign_BottomMarkResultCheck,                               //  Bottom 위치 얼라인 마크 찾기 결과 확인

            WaferAlignData_Calc,                                            //  얼라인 데이터 계산

            WaferAlign_ThetaMove,                                           //  UVW Stage Theta 회전
            WaferAlign_ThetaMove_DoneCheck,                                 //  UVW Stage Theta 회전 완료 확인


            WaferAlign_Retry,                                               //  UVW Stage Theta 값 확인 -> Config 에 설정된 값보다 클 경우 Retry

            __Wafer_ThetaAlign_Complete,                                    //  Wafer Theta Align 완료



            __Wafer_XYAlign_Start,                                          //  Wafer XY Align 시작

            WaferXYAlign_TopMarkFind_Ready,                                 //  Top 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_WaferXYAlignPos_TopCenter,                       //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동
            VisionXYZ_Move_WaferXYAlignPos_TopCenter_DoneCheck,             //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_WaferXYAlignPos_TopCenter_StableTime,            //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동 후 안정화 시간 --> 여기서 카메라 Offset 확인 추가

            WaferXYAlign_TopMarkFind,                                       //  Top 위치 얼라인 마크 찾기
            WaferXYAlign_TopMarkResultCheck,                                //  Top 위치 얼라인 마크 찾기 결과 확인

            WaferXYAlignData_Calc,                                          //  XY 얼라인 데이터 계산

            WaferXYAlign_CorrectionMove,                                    //  Top 위치 XY 보정 이동 (카메라 Center 로)
            WaferXYAlign_CorrectionMove_DoneCheck,                          //  Top 위치 XY 보정 이동 완료 확인

            WaferXYAlign_Retry,                                             //  Vision XY 값 확인 -> Config 에 설정된 값보다 클 경우 Retry

            __Wafer_XYAlign_Complete,                                       //  Wafer XY Align 완료



            __Wafer_Align_ErrorCheck_Start,                                 //  Wafer Align 에러 검증 시작
            __Wafer_Align_ErrorCheck_Complete,                              //  Wafer Align 에러 검증 완료 확인



            //__Wafer_OffsetMove_Start,                                       //  Wafer Offset 이동 시작

            //UVW_Move_WaferPackingOffset,                                    //  UVW 축, Wafer Packing Offset 거리 이동
            //UVW_Move_WaferPackingOffset_DoneCheck,                          //  UVW 축, Wafer Packing Offset 거리 이동 완료 확인

            //__Wafer_OffsetMove_Complete,                                    //  Wafer Offset 이동 완료



            VisionXYZ_Move_ReadyPos2,                                       //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos2_DoneCheck,                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인


            Complete                                            //  완료
        }

        public double m_dMy1stMarkVisionPos_X { set; get; }
        public double m_dMy1stMarkVisionPos_Y { set; get; }
        public double m_dMy2ndMarkVisionPos_X { set; get; }
        public double m_dMy2ndMarkVisionPos_Y { set; get; }
        public double m_dMyVisionPos_Distance_X {  set; get; }
        public double m_dMyVisionPos_Distance_Y { set; get; }
        public bool m_bMyWaferAlign_fromManualMode { set; get; }
        public bool m_bSelected_HighResCamera { set; get; }                 //  OperationManualMode 창에서 카메라를 선택할 때 변경되는 데이터. (어떤 카메라를 선택했는 지)
        public int m_nMyWaferAlign_ManualMode_VisionType { set; get; }      //  1: Low Vision   2: High Vision  (ManualMode 화면에서 Align 을 할 경우, 카메라 타입에 따라 라이브 화면을 변경하기 위한 변수)
        //public bool m_bLowerVision_Align { set; get; }                      //  true: Lower Vision       false: Upper Vision

        public int m_nVisionAligner_Type { set; get; }                      //  1: Wafer Aligner
                                                                            //  2: PAK Aligner
                                                                            //  3: Lower Reticle Aligner
                                                                            //  4: Upper Reticle Aligner

        public enum Aligner_Type
        {
            Aligner_None = 0,

            Aligner_Wafer = 1,
            Aligner_PAK = 2,
            Aligner_Reticle_Lower = 3,
            Aligner_Reticle_Upper = 4
        }

        public bool m_bWaferLowResAlign_OK { set; get; }                    //  Wafer Low Res. Align OK
        public bool m_bWaferHighResAlign_OK { set; get; }                   //  Wafer High Res. Align OK
        public bool m_bWaferLowResAutoFocus_OK { set; get; }                //  Wafer Low Res. Auto Focus OK
        public bool m_bWaferHighResAutoFocus_OK { set; get; }               //  Wafer High Res. Auto Focus OK
        public int m_nWaferAlign_Count { set; get; }                        //  Wafer Align 시도 회수


        public int m_nFindAlignMark_Step { set; get; }                      //  마크 찾기 Step
        public bool m_bFindAlignMark_OK { set; get; }                       //  Find Align Mark OK
        public bool m_bFindUpperAlignMark_OK { set; get; }                  //  Find Upper Camera Align Mark OK
        public bool m_bFindLowerAlignMark_OK { set; get; }                  //  Find Lower Camera Align Mark OK
        public enum FindAlignMark_Step
        {
            None = 0,
            Start,                                                          //  시작

            FindMark_Start,                                                 //  마크 찾기 Start

            FindMark_ResultCheck,                                           //  마크 찾기 결과 확인

            Complete                                                        //  완료
        }

        #endregion


        #region Constructor
        public WorkStage(string strName) : base(strName)
        {

            bool ret = true;

            ParamConfig = new WorkStageParameterConfig();
            Config = new WorkStageConfig();
            //SetDispenserWork((int)DispenserWorkStatus.WORK_NONE);

            //WorkStageIndex = -1;
            //m_bLaserGetStatus_Run = false;

            //Cepheus_laser = new MyCepheusLaser();

            stCamera = Machine_Parameter_Camera_Setting_Load();

            Equipment.PAKCamera_SerialNumber = stCamera.HighRes_SerialNumber;
            Equipment.PAKCamera_Width = stCamera.HighRes_Width;
            Equipment.PAKCamera_Height = stCamera.HighRes_Height;
            Equipment.WaferCamera_SerialNumber = stCamera.LowRes_SerialNumber;
            Equipment.WaferCamera_Width = stCamera.LowRes_Width;
            Equipment.WaferCamera_Height = stCamera.LowRes_Height;

            m_nHomeStep = (int)Home_Step.None;
            m_nWorkStage_MainStep = (int)WorkStage_Step.None;
            m_nFindAlignMark_Step = (int)FindAlignMark_Step.None;
            m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;

            m_bFindAlignMark_OK = false;

            m_bWaferLowResAlign_OK = false;
            m_bWaferHighResAlign_OK = false;
            m_bWaferLowResAutoFocus_OK = false;
            m_bWaferHighResAutoFocus_OK = false;

            m_bAlignVisionThread_Use = true;                                            //  Align Vision 을 Thread 로 할지 말지?

            m_nProbeCard_AlignMarkCount_Max = 0;                                        //  프로브 카드 얼라인 마크 검사 최대 회수. (평균 계산용)
            m_nWafer_AlignMarkCount_Max = 0;                                            //  웨이퍼 얼라인 마크 검사 최대 회수. (평균 계산용)
            m_nProbeCard_AlignMark_Count = 0;                                           //  프로브 카드 얼라인 마크 개수. (평균 계산용)
            m_nWafer_AlignMark_Count = 0;                                               //  웨이퍼 얼라인 마크 개수. (평균 계산용)
            m_pProbeCard_AlignMarkPosition_Sum = new PointD[5];                         //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
            m_pWafer_AlignMarkPosition_Sum = new PointD[5];                             //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)
            m_pProbeCard_AlignMarkPosition_Average = new PointD[5];                     //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
            m_pWafer_AlignMarkPosition_Average = new PointD[5];                         //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)

            m_pProbeCard_AlignMark_AddPosition_Sum = new PointD[5];                     //  추가 위치, 프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
            m_pWafer_AlignMark_AddPosition_Sum = new PointD[5];                         //  추가 위치, 웨이퍼 얼라인 마크 위치 누적. (평균 계산용)
            m_pProbeCard_AlignMark_AddPosition_Average = new PointD[5];                 //  추가 위치, 프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
            m_pWafer_AlignMark_AddPosition_Average = new PointD[5];                     //  추가 위치, 웨이퍼 얼라인 마크 위치 누적. (평균 계산용)

            m_pProbeCard_AlignMarkPosition_forVerify_Sum = new PointD[9];                         //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
            m_pWafer_AlignMarkPosition_forVerify_Sum = new PointD[9];                             //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)
            m_pProbeCard_AlignMarkPosition_forVerify_Average = new PointD[9];                     //  프로브 카드 얼라인 마크 위치 누적. (평균 계산용)
            m_pWafer_AlignMarkPosition_forVerify_Average = new PointD[9];                         //  웨이퍼 얼라인 마크 위치 누적. (평균 계산용)

            m_nReticle_HighRes_AlignMarkCount_Max = 0;                                    //  레티클 글래스 상부 카메라 얼라인 마크 검사 최대 회수. (평균 계산용)
            m_nReticle_LowRes_AlignMarkCount_Max = 0;                                    //  레티클 글래스 하부 카메라 얼라인 마크 검사 최대 회수. (평균 계산용)
            m_nReticle_HighRes_AlignMark_Count = 0;                                       //  레티클 글래스 상부 카메라 얼라인 마크 개수. (평균 계산용)
            m_nReticle_LowRes_AlignMark_Count = 0;                                       //  레티클 글래스 하부 카메라 얼라인 마크 개수. (평균 계산용)
            m_pReticle_HighRes_AlignMarkPosition_Sum = new PointD(0, 0);                  //  레티클 글래스 상부 카메라 얼라인 마크 위치 누적. (평균 계산용)
            m_pReticle_LowRes_AlignMarkPosition_Sum = new PointD(0, 0);                  //  레티클 글래스 하부 카메라 얼라인 마크 위치 누적. (평균 계산용)
            m_pReticle_HighRes_AlignMarkPosition_Average = new PointD(0, 0);              //  레티클 글래스 상부 카메라 얼라인 마크 위치 누적. (평균 계산용)
            m_pReticle_LowRes_AlignMarkPosition_Average = new PointD(0, 0);              //  레티클 글래스 하부 카메라 얼라인 마크 위치 누적. (평균 계산용)

            m_nReticleCheck_Step_forALIGN = (int)ReticleCheck_Step.None;                //  프로그램 시작 시, 레시피 변경 시 레티클 확인 Step 초기화 
            m_bSelected_HighResCamera = false;
            m_bMyWaferAlign_fromManualMode = false;

            m_bInManualMoving_SafetySensor_Detected = false;
            m_bInCycleMoving_SafetySensor_Detected = false;

            m_nReticleGlassCheck_Cam = (int)ReticleCamType.None;


            //  타이머를 쓰레드로 변경 --> 다시 타이머 사용하기로...

            //  Main Work 타이머
            timer_MainWork = new System.Windows.Forms.Timer();
            timer_MainWork.Interval = 1;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_MainWork.Tick += new System.EventHandler(Timer_MainWork_Func);

            //  Sub Work 타이머
            timer_SubWork = new System.Windows.Forms.Timer();
            timer_SubWork.Interval = 1;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_SubWork.Tick += new System.EventHandler(Timer_SubWork_Func);

            //  Product Align 타이머
            timer_VisionAlign = new System.Windows.Forms.Timer();
            timer_VisionAlign.Interval = 1;
            timer_VisionAlign.Tick += new System.EventHandler(Timer_ProductAlign_Func);

            //  Motion 홈 실행 타이머
            timer_Motion_Home = new System.Windows.Forms.Timer();
            timer_Motion_Home.Interval = 1;
            timer_Motion_Home.Tick += new System.EventHandler(Timer_MotionHome_Func);

            //  Reticle Glass check 타이머
            timer_ReticleGlass_Check = new System.Windows.Forms.Timer();
            timer_ReticleGlass_Check.Interval = 1;
            timer_ReticleGlass_Check.Tick += new System.EventHandler(Timer_ReticleGlass_Func);

            m_btimer_MainWork_Stop = false;
            m_btimer_SubWork_Stop = false;
            m_btimer_Motion_Home_Stop = false;
            m_btimer_VisionAlign_Stop = false;
            m_btimer_ReticleGlass_Check_Stop = false;


            ////  Laser Status 갱신 실행 타이머
            //timer_Calibration = new System.Windows.Forms.Timer();
            //timer_Calibration.Interval = 50;
            //timer_Calibration.Tick += new System.EventHandler(Timer_LaserCalibration_Func);

            //SpiralLab.Core.Initialize();

            m_nProductAlign_CameraType = (int)CameraType.CAMERA_HIGH;
            
            m_bHomeOK = false;
            m_nTotalAxisCount = 0;              //  전체 축 수

            DrillingData_Loaded = false;

            m_bDrillingData_isLine = false;

            m_bParameterSetting_PosData_Reload = false;

            m_bLog_1time = false;
            m_bLog_1time2 = false;

            for (int i = 0; i < System.Enum.GetValues(typeof(AlignParam)).Length; i++)
            {
                m_forAlign_Data[i].X = 0.0;
                m_forAlign_Data[i].Y = 0.0;
            }

            //m_bFindFirstAlignMarkOnly = false;
            m_nFindAlignMarkType = (int)AlignMarkType.ALIGN_2POINT;

            m_nVisionAligner_Type = (int)Aligner_Type.Aligner_None;         //  Aligner Type 

            TickCount_MainCycle_Start = 0;
            TickCount_MainCycle_Current = 0;
            TickCount_MainCycle_Interval = 0;

            for (int i = 0; i < 2; i++)
            {
                m_dCmdPos[i] = 0.0;
                m_dActPos[i] = 0.0;
            }


            //  Teaching Data 저장 폴더 생성
            if (Directory.Exists(ConfigManager.GetTeachingDataPath()) == false)
            {
                Directory.CreateDirectory(ConfigManager.GetTeachingDataPath());
            }

            //  Teaching Position List 변수 초기화
            for (int i = 0; i < System.Enum.GetValues(typeof(WorkStage_TeachingPosList)).Length; i++)
            {
                stWorkStageTeachingPos[i].Stage_X = 0;
                stWorkStageTeachingPos[i].Stage_Y = 0;
            }
            


            //  변수 초기화 (Laser 에서 사용)

            ////  가공 도면 Align
            //m_stAlignMark.dAlignMark1.X = 0.0;
            //m_stAlignMark.dAlignMark1.Y = 0.0;
            //m_stAlignMark.dAlignMark2.X = 0.0;
            //m_stAlignMark.dAlignMark2.Y = 0.0;
            //m_stAlignMark.dRotationCenter.X = 0.0;
            //m_stAlignMark.dRotationCenter.Y = 0.0;
            //m_stAlignMark.dRotationDegree = 0.0;

            //m_dALIGN_FACTOR_RotationCenter_X = 0.0;                     //  전체 가공 도면 회전 중심 X
            //m_dALIGN_FACTOR_RotationCenter_Y = 0.0;                     //  전체 가공 도면 회전 중심 Y
            //m_dALIGN_FACTOR_Offset_X = 0.0;                             //  전체 가공 도면 이동 Offset X
            //m_dALIGN_FACTOR_Offset_Y = 0.0;                             //  전체 가공 도면 이동 Offset Y
            //m_dALIGN_FACTOR_Theta = 0.0;                                //  전체 가공 도면 회전 (Theta,     기준위치 : Align1 (Thruhole 의 Circle 객체, Description 에 Align1 표시)

            //m_nUnfollow_TryCount = 0;
            //m_bUnfollow_Ret = false;

            //PEG_Mode = (int)nPegMode.PEG_FullNormal;

            //m_nLLO_Work_Interval_X = 0;                     //  몇개의 Chip or Block 마다 가공할 것인지 (X)
            //m_nLLO_Work_Interval_Y = 0;                     //  몇개의 Chip or Block 마다 가공할 것인지 (Y)
            //m_nLLO_Work_Interval_Current_X = 0;             //  Interval Count X
            //m_nLLO_Work_Interval_Current_Y = 0;             //  Interval Count Y

            //m_nLLO_Work_Repetition_Count = 1;               //  가공 회수
            //m_nLLO_Work_Current_Count = 0;                  //  현재 가공 회수

            //m_nLLO_Work_Set_Index = 1;                      //  Block 단위 가공 시 Set Index

            //m_bStage2DMapDataApply_Success = false;

            //m_bSESAM_ReplacementCycle_Exceed = false;
            //m_strSESAM_ReplacementCycle_Remained = "";

            //m_bMapDataType_UserChange_Complete = false;

            //m_nLCIMapData_StartIndex_X = 0;                 //  LCI Module 사용 시, Map Data 변경 위치 X Index (시작, Left)
            //m_nLCIMapData_EndIndex_X = 0;                   //  LCI Module 사용 시, Map Data 변경 위치 X Index (종료, Right)
            //m_nLCIMapData_StageStartIndex_Y = 0;            //  LCI Module 사용 시, Map Data 변경 위치 Y Index (시작)
            //m_nLCIMapData_StageEndIndex_Y = 0;              //  LCI Module 사용 시, Map Data 변경 위치 Y Index (종료)

            //PEGDiv_Firstpoint_PEG = 0.0;                    //  PEG 시작 위치
            //PEGDiv_Lastpoint_PEG = 0.0;                     //  PEG 종료 위치
            //PEGDiv_StartPos_PEG = 0.0;                      //  Stage 시작 위치
            //PEGDiv_EndPos_PEG = 0.0;                        //  Stage 종료 위치
            //PEGDiv_ReCalc_Firstpoint_PEG = 0.0;             //  다시 계산된 PEG 시작 위치
            //PEGDiv_ReCalc_Lastpoint_PEG = 0.0;              //  다시 계산된 PEG 종료 위치
            //PEGDiv_ReCalc_StartPos_PEG = 0.0;               //  다시 계산된 Stage 시작 위치
            //PEGDiv_ReCalc_EndPos_PEG = 0.0;                 //  다시 계산된 Stage 종료 위치
            //PEGDiv_DivArea_TotalCount = 0;                  //  몇개의 분할 영역으로 나누어지는지
            //PEGDiv_DivArea_Count = 0;                       //  몇 번째 분할 영역인지
            //PEGDiv_WorkChipCount_inDivArea = 0;             //  분할 영역 안에 몇개의 Chip 이 있는지
            //PEGDiv_MoveDirection_LtoR = true;               //  가공 방향 (true: 왼쪽에서 오른쪽으로 이동하면서 가공, 스테이지는 오른쪽에서 왼쪽으로 이동)
        }
        #endregion

        #region IExecuter
        public override int Initialize()
        {
            return base.Initialize();
        }

        public override int OnPrepareToWork()
        {
            return base.OnPrepareToWork();
        }
        public override int OnAfterWork()
        {
            return base.OnAfterWork();
        }
        public override void Stop()
        {
            base.Stop();
        }
        public override int OnWork()
        {
            //dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");


            int ret = 0;
            ret = base.Work();

            return ret;
        }
        #endregion

        #region Module Members

        protected override int OnRun()
        {
            return base.OnRun();
        }

        public override int Create()
        {
            int ret = base.Create();

            Stage = new XyzyStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            Camera_LowRes = new HIKGigECamera("Coarse Vision");                                         //  저해상도 카메라
            //Camera_LowRes = new GrabLinkMultiCamCamera("LaserCamera Low-Res");                          //  하부 비전 카메라
            Camera_LowRes.Create();
            Camera_LowRes.Owner = this;
            Parts.Add(Camera_LowRes);

            Camera_HighRes = new HIKGigECamera("Fine Vision");
            //Camera = new GrabLinkMultiCamCamera("LaserCamera");
            Camera_HighRes.Create();
            Camera_HighRes.Owner = this;
            Parts.Add(Camera_HighRes);

            workStageParameter = new WorkStageParameter("WorkStage Parameter");
            workStageParameter.Create();
            workStageParameter.Owner = this;
            workStageParameter.Axes = Stage.Axes;
            Parts.Add(workStageParameter);

            //visionCalibrator_LowRes = new VisionCalibrator("VisionCalibrator LowRes");
            //visionCalibrator_LowRes.Create();
            //visionCalibrator_LowRes.Owner = this;
            //visionCalibrator_LowRes.Camera = Camera_LowRes;
            //visionCalibrator_LowRes.XyzyStage = Stage;
            //visionCalibrator_LowRes.Illuminator = CommonModule.Instance.Illuminator;
            //Parts.Add(visionCalibrator_LowRes);

            //visionCalibrator_HighRes = new VisionCalibrator("VisionCalibrator HighRes");
            //visionCalibrator_HighRes.Create();
            //visionCalibrator_HighRes.Owner = this;
            //visionCalibrator_HighRes.Camera = Camera_HighRes;
            //visionCalibrator_HighRes.XyzyStage = Stage;
            //visionCalibrator_HighRes.Illuminator = CommonModule.Instance.Illuminator;
            //Parts.Add(visionCalibrator_HighRes);

            //visionCompensator_HighRes = new VisionCompensator("Vision Compensator HighRes");
            //visionCompensator_HighRes.Create();
            //visionCompensator_HighRes.Owner = this;
            //visionCompensator_HighRes.Camera = Camera_HighRes;
            //visionCompensator_HighRes.XyzyStage = Stage;
            //visionCompensator_HighRes.Illuminator = CommonModule.Instance.Illuminator;
            //Parts.Add(visionCompensator_HighRes);

            //laserPitchMoveShotter = new LaserPitchMoveShotter("LaserPitchMove Shotter");
            //laserPitchMoveShotter.Create();
            //laserPitchMoveShotter.Owner = this;
            //laserPitchMoveShotter.Camera = Camera_HighRes;
            //laserPitchMoveShotter.XyzyStage = Stage;
            //Parts.Add(laserPitchMoveShotter);

            autoFocuser_LowRes = new AutoFocuser("AutoFocuser LowRes");
            autoFocuser_LowRes.Create();
            autoFocuser_LowRes.Owner = this;
            autoFocuser_LowRes.Camera = Camera_LowRes;
            autoFocuser_LowRes.Motion = Stage;
            autoFocuser_LowRes.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(autoFocuser_LowRes);

            autoFocuser_HighRes = new AutoFocuser("AutoFocuser HighRes");
            autoFocuser_HighRes.Create();
            autoFocuser_HighRes.Owner = this;
            autoFocuser_HighRes.Camera = Camera_HighRes;
            autoFocuser_HighRes.Motion = Stage;
            autoFocuser_HighRes.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(autoFocuser_HighRes);

            //scannerCompensator = new ScannerCompensator("Scanner Compensator HighRes");
            //scannerCompensator.Create();
            //scannerCompensator.Owner = this;
            //scannerCompensator.Camera = Camera_HighRes;
            //scannerCompensator.XyzyStage = Stage;
            //scannerCompensator.Illuminator = CommonModule.Instance.Illuminator;
            //Parts.Add(scannerCompensator);

            jigAligner_LowRes = new JigAligner("JigAligner (Coarse)");
            jigAligner_LowRes.Create();
            jigAligner_LowRes.Owner = this;
            jigAligner_LowRes.Camera = Camera_LowRes;
            jigAligner_LowRes.XyzyStage = Stage;
            jigAligner_LowRes.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(jigAligner_LowRes);

            jigAligner_HighRes = new JigAligner("JigAligner (Fine)");
            jigAligner_HighRes.Create();
            jigAligner_HighRes.Owner = this;
            jigAligner_HighRes.Camera = Camera_HighRes;
            jigAligner_HighRes.XyzyStage = Stage;
            jigAligner_HighRes.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(jigAligner_HighRes);

            reticleAligner_LowRes = new JigAligner("ReticleAligner (Coarse)");
            reticleAligner_LowRes.Create();
            reticleAligner_LowRes.Owner = this;
            reticleAligner_LowRes.Camera = Camera_LowRes;
            reticleAligner_LowRes.XyzyStage = Stage;
            reticleAligner_LowRes.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(reticleAligner_LowRes);

            reticleAligner_HighRes = new JigAligner("ReticleAligner (Fine)");
            reticleAligner_HighRes.Create();
            reticleAligner_HighRes.Owner = this;
            reticleAligner_HighRes.Camera = Camera_HighRes;
            reticleAligner_HighRes.XyzyStage = Stage;
            reticleAligner_HighRes.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(reticleAligner_HighRes);

            //PosParam_Dispenser = GetConfigData();     //  요건 나중에

            //ACS_Motion_isSimulationMode = false;
            //ACS_Motion = new Api();

            Recipe = new WorkStageRecipe(this);

            return ret;
        }

        public override void SetConfigData(object configData)
        {
            Config = configData as WorkStageConfig;

            if (Config == null)
                Config = new WorkStageConfig();

            Config.Init();

            if (Config.ParamConfig != null)
            {
                ParamConfig = Config.ParamConfig;

                workStageParameter.Config = ParamConfig;
            }

            Stage.Config = this.Config.StageConfig;

            Camera_HighRes.Config = Config.CameraConfig_HighRes;                        //  상부 카메라
            Camera_LowRes.Config = Config.CameraConfig_LowRes;                        //  하부 카메라
            //visionCalibrator_HighRes.Config = Config.VisonCalibratorConfig_HighRes;
            //visionCalibrator_LowRes.Config = Config.VisonCalibratorConfig_LowRes;
            //visionCompensator_HighRes.Config = Config.VisionCompensatorConfig;
            autoFocuser_HighRes.Config = Config.AutoFocuserConfig_HighRes;
            autoFocuser_LowRes.Config = Config.AutoFocuserConfig_LowRes;
            //scannerCompensator.Config = Config.ScannerCompensatorConfig;
            //laserPitchMoveShotter.Config = Config.LaserPitchMoveShotterConfig;

            //Stage.UpdateDirection();                              //  Z 축 방향 바꾸기? (주석 처리)
            //jigAligner.Config = Config.JigAlignerConfig;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override void UpdateConfigData()
        {
            Camera_HighRes.Config = Config.CameraConfig_HighRes;
            Camera_LowRes.Config = Config.CameraConfig_LowRes;
            //visionCalibrator_HighRes.Config = Config.VisonCalibratorConfig_HighRes;
            //visionCalibrator_LowRes.Config = Config.VisonCalibratorConfig_LowRes;
            //visionCompensator_HighRes.Config = Config.VisionCompensatorConfig;
            autoFocuser_HighRes.Config = Config.AutoFocuserConfig_HighRes;
            autoFocuser_LowRes.Config = Config.AutoFocuserConfig_LowRes;
            //scannerCompensator.Config = Config.ScannerCompensatorConfig;
            //laserPitchMoveShotter.Config = Config.LaserPitchMoveShotterConfig;
            //jigAligner.Config = Config.JigAlignerConfig;

            base.UpdateConfigData();
        }

        public override void SetRecipeData(object recipeData)
        {
            WorkStageRecipe recipe = recipeData as WorkStageRecipe;
            if (recipe == null)
            {
                recipe = new WorkStageRecipe(this);
            }


            Recipe = recipe;
            Recipe.Init(this);

            //visionCalibrator_HighRes.Recipe = Recipe.VisionCalibratorRecipe_HighRes;
            //visionCalibrator_LowRes.Recipe = Recipe.VisionCalibratorRecipe_LowRes;
            //scannerCompensator.Recipe = Recipe.scannerCompensatorRecipe;
            jigAligner_HighRes.Recipe = Recipe.jigAlignerRecipe_HighRes;
            jigAligner_LowRes.Recipe = Recipe.jigAlignerRecipe_LowRes;
            reticleAligner_HighRes.Recipe = Recipe.reticleAlignerRecipe_HighRes;
            reticleAligner_LowRes.Recipe = Recipe.reticleAlignerRecipe_LowRes;

            base.SetRecipeData(recipeData);
        }

        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void UpdateRecipeData()
        {
            //visionCalibrator_HighRes.Recipe = Recipe.VisionCalibratorRecipe_HighRes;
            //visionCalibrator_LowRes.Recipe = Recipe.VisionCalibratorRecipe_LowRes;
            //scannerCompensator.Recipe = Recipe.scannerCompensatorRecipe;
            jigAligner_HighRes.Recipe = Recipe.jigAlignerRecipe_HighRes;
            jigAligner_LowRes.Recipe = Recipe.jigAlignerRecipe_LowRes;
            reticleAligner_HighRes.Recipe = Recipe.reticleAlignerRecipe_HighRes;
            reticleAligner_LowRes.Recipe = Recipe.reticleAlignerRecipe_LowRes;

            base.UpdateRecipeData();
        }

        public override void Close()
        {
            base.Close();

            if (Stage != null)
            {
                Stage.Close();
            }

            if (Camera_HighRes != null)
            {
                Camera_HighRes.Close();
            }

            if (Camera_LowRes != null)
            {
                Camera_LowRes.Close();
            }

            if (m_powerMeter_ExitPos_Comm != null)
            {
                m_powerMeter_ExitPos_Comm.CloseComm();
                m_powerMeter_ExitPos_Comm.Close();
            }

            if (m_powerMeter_TargetPos_Comm != null)
            {
                m_powerMeter_TargetPos_Comm.CloseComm();
                m_powerMeter_TargetPos_Comm.Close();
            }

            if (m_beamExpander_Comm != null)
            {
                m_beamExpander_Comm.CloseComm();
                m_beamExpander_Comm.Close();
            }

            if (m_dustCollector_UpperPos_Comm != null)
            {
                m_dustCollector_UpperPos_Comm.CloseComm();
                m_dustCollector_UpperPos_Comm.Close();
            }

            if (m_dustCollector_LowerPos_Comm != null)
            {
                m_dustCollector_LowerPos_Comm.CloseComm();
                m_dustCollector_LowerPos_Comm.Close();
            }

            if (m_electroRegulator_Comm != null)
            {
                m_electroRegulator_Comm.CloseComm();
                m_electroRegulator_Comm.Close();
            }

            //if (m_BarcodeReader_Comm != null)
            //{
            //    m_BarcodeReader_Comm.CloseComm();
            //    m_BarcodeReader_Comm.Close();
            //}

            //if (ACS_Motion != null)
            //{
            //    ACS_Motion.CloseComm();
            //}
        }
        #endregion


        #region Serial Comm. - Power Meter (Exit & Target Pos.)

        public void PowerMeterComm_ExitPos_Init()
        {
            m_powerMeter_ExitPos_Comm = new SerialCommPowerMeter1Port();
            m_powerMeter_ExitPos_Comm.DataReceivedHandler = PowerMeter_ExitPos_DataReceivedHandler;
            m_powerMeter_ExitPos_Comm.DisconnectedHandler = PowerMeter_ExitPos_DisconnectedHandler;
            if (!m_powerMeter_ExitPos_Comm.OpenComm("COM2", 115200, 8, StopBits.One, Parity.None, Handshake.None))
            {
                string text = "COM2 Port Open Failed! (Power Meter, Exit Pos.)";
                MessageBox.Show(text);
            }
        }

        private void PowerMeter_ExitPos_DataReceivedHandler(byte[] receiveData)
        {
            string @string = Encoding.Default.GetString(receiveData);

            m_strLaserPowerMeter_ExitPos_Comm_ReceivedData += @string;

            if (m_strLaserPowerMeter_ExitPos_Comm_ReceivedData.Length >= 1)
            {
                if (m_strLaserPowerMeter_ExitPos_Comm_ReceivedData[m_strLaserPowerMeter_ExitPos_Comm_ReceivedData.Length - 1] == chrCR)
                {
                    m_bLaserPowerMeter_ExitPos_CommData_Received = true;
                }
                else if (m_strLaserPowerMeter_ExitPos_Comm_ReceivedData.Length >= 2)
                {
                    if ((m_strLaserPowerMeter_ExitPos_Comm_ReceivedData[m_strLaserPowerMeter_ExitPos_Comm_ReceivedData.Length - 2] == chrCR) &&
                        (m_strLaserPowerMeter_ExitPos_Comm_ReceivedData[m_strLaserPowerMeter_ExitPos_Comm_ReceivedData.Length - 1] == chrLF))
                    {
                        m_bLaserPowerMeter_ExitPos_CommData_Received = true;
                    }
                }
            }
        }

        private void PowerMeter_ExitPos_DisconnectedHandler()
        {
            Console.WriteLine("Laser Power Meter serial COM2 disconnected");
        }

        public void PowerMeterComm_TargetPos_Init()
        {
            m_powerMeter_TargetPos_Comm = new SerialCommPowerMeter2Port();
            m_powerMeter_TargetPos_Comm.DataReceivedHandler = PowerMeter_TargetPos_DataReceivedHandler;
            m_powerMeter_TargetPos_Comm.DisconnectedHandler = PowerMeter_TargetPos_DisconnectedHandler;
            if (!m_powerMeter_TargetPos_Comm.OpenComm("COM3", 115200, 8, StopBits.One, Parity.None, Handshake.None))
            {
                string text = "COM3 Port Open Failed! (Power Meter, Target Pos.)";
                MessageBox.Show(text);
            }
        }

        private void PowerMeter_TargetPos_DataReceivedHandler(byte[] receiveData)
        {
            string @string = Encoding.Default.GetString(receiveData);

            m_strLaserPowerMeter_TargetPos_Comm_ReceivedData += @string;

            if (m_strLaserPowerMeter_TargetPos_Comm_ReceivedData.Length >= 1)
            {
                if (m_strLaserPowerMeter_TargetPos_Comm_ReceivedData[m_strLaserPowerMeter_TargetPos_Comm_ReceivedData.Length - 1] == chrCR)
                {
                    m_bLaserPowerMeter_TargetPos_CommData_Received = true;
                }
                else if (m_strLaserPowerMeter_TargetPos_Comm_ReceivedData.Length >= 2)
                {
                    if ((m_strLaserPowerMeter_TargetPos_Comm_ReceivedData[m_strLaserPowerMeter_TargetPos_Comm_ReceivedData.Length - 2] == chrCR) &&
                        (m_strLaserPowerMeter_TargetPos_Comm_ReceivedData[m_strLaserPowerMeter_TargetPos_Comm_ReceivedData.Length - 1] == chrLF))
                    {
                        m_bLaserPowerMeter_TargetPos_CommData_Received = true;
                    }
                }
            }
        }

        private void PowerMeter_TargetPos_DisconnectedHandler()
        {
            Console.WriteLine("Laser Power Meter serial COM3 disconnected");
        }

        public bool LaserPowerMeter_GetValue(int m_nPowerMeterPos)
        {
            byte[] m_cSendCmd = new byte[4];
            string m_strSendData = "";

            m_cSendCmd[0] = (byte)'p';
            m_cSendCmd[1] = (byte)'w';
            m_cSendCmd[2] = (byte)'?';
            m_cSendCmd[3] = chrCR;

            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_nPowerMeterPos == (int)nPowerMeter.ExitPos)
            {
                if (m_powerMeter_ExitPos_Comm == null)
                    return false;

                if (m_powerMeter_ExitPos_Comm.IsOpen)
                {
                    m_powerMeter_ExitPos_Comm.Send(m_strSendData);

                    return true;
                }

                return false;
            }
            else
            {
                if (m_powerMeter_TargetPos_Comm == null)
                    return false;

                if (m_powerMeter_TargetPos_Comm.IsOpen)
                {
                    m_powerMeter_TargetPos_Comm.Send(m_strSendData);

                    return true;
                }

                return false;
            }
        }

        #endregion


        #region Serial Comm. - Motorized Beam Expander

        public void BeamExpanderComm_Init()
        {
            m_beamExpander_Comm = new SerialCommBeamExpanderPort();
            m_beamExpander_Comm.DataReceivedHandler = BeamExpander_DataReceivedHandler;
            m_beamExpander_Comm.DisconnectedHandler = BeamExpander_DisconnectedHandler;
            if (!m_beamExpander_Comm.OpenComm("COM4", 115200, 8, StopBits.One, Parity.None, Handshake.None))
            {
                string text = "COM4 Port Open Failed! (Beam Expander)";
                MessageBox.Show(text);
            }
        }

        private void BeamExpander_DataReceivedHandler(byte[] receiveData)
        {
            string @string = Encoding.Default.GetString(receiveData);

            //m_strBeamExpander_Comm_ReceivedData += @string;
            //if (m_strBeamExpander_Comm_ReceivedData.Length >= 1)
            //{
            //    if (m_strBeamExpander_Comm_ReceivedData[m_strBeamExpander_Comm_ReceivedData.Length - 1] == '\r')
            //    {
            //        m_bBeamExpander_CommData_Received = true;
            //    }
            //    else if (m_strBeamExpander_Comm_ReceivedData.Length >= 2 && m_strBeamExpander_Comm_ReceivedData[m_strBeamExpander_Comm_ReceivedData.Length - 2] == '\r' && m_strBeamExpander_Comm_ReceivedData[m_strBeamExpander_Comm_ReceivedData.Length - 1] == '\n')
            //    {
            //        m_bBeamExpander_CommData_Received = true;

            //        MessageBox.Show(m_strBeamExpander_Comm_ReceivedData);

            //        m_strBeamExpander_Comm_ReceivedData = "";
            //    }
            //}
        }

        private void BeamExpander_DisconnectedHandler()
        {
            Console.WriteLine("Beam Expander serial COM4 disconnected");
        }

        #endregion


        #region Serial Comm. - Dust Collector (Upper, Lower Position)

        public void DustCollector_UpperPos_Comm_Init()
        {
            m_dustCollector_UpperPos_Comm = new SerialCommDustCollector1Port();
            m_dustCollector_UpperPos_Comm.DataReceivedHandler = DustCollector_UpperPos_DataReceivedHandler;
            m_dustCollector_UpperPos_Comm.DisconnectedHandler = DustCollector_UpperPos_DisconnectedHandler;
            if (!m_dustCollector_UpperPos_Comm.OpenComm("COM5", 9600, 8, StopBits.One, Parity.None, Handshake.None))
            {
                string text = "COM5 Port Open Failed! (Dust Collector, Upper Pos.)";
                MessageBox.Show(text);
            }
        }

        public void DustCollector_LowerPos_Comm_Init()
        {
            m_dustCollector_LowerPos_Comm = new SerialCommDustCollector2Port();
            m_dustCollector_LowerPos_Comm.DataReceivedHandler = DustCollector_LowerPos_DataReceivedHandler;
            m_dustCollector_LowerPos_Comm.DisconnectedHandler = DustCollector_LowerPos_DisconnectedHandler;
            if (!m_dustCollector_LowerPos_Comm.OpenComm("COM6", 9600, 8, StopBits.One, Parity.None, Handshake.None))
            {
                string text = "COM6 Port Open Failed! (Dust Collector, Lower Pos.)";
                MessageBox.Show(text);
            }
        }

        private void DustCollector_UpperPos_DataReceivedHandler(byte[] receiveData)
        {
            string @string = Encoding.Default.GetString(receiveData);

            //m_strDustCollector_UpperPos_Comm_ReceivedData += @string;
            //if (m_strDustCollector_UpperPos_Comm_ReceivedData.Length >= 1)
            //{
            //    if (m_strDustCollector_UpperPos_Comm_ReceivedData[m_strDustCollector_UpperPos_Comm_ReceivedData.Length - 1] == '\r')
            //    {
            //        m_bDustCollector_UpperPos_CommData_Received = true;
            //    }
            //    else if (m_strDustCollector_UpperPos_Comm_ReceivedData.Length >= 2 && m_strDustCollector_UpperPos_Comm_ReceivedData[m_strDustCollector_UpperPos_Comm_ReceivedData.Length - 2] == '\r' && m_strDustCollector_UpperPos_Comm_ReceivedData[m_strDustCollector_UpperPos_Comm_ReceivedData.Length - 1] == '\n')
            //    {
            //        m_bDustCollector_UpperPos_CommData_Received = true;

            //        MessageBox.Show(m_strDustCollector_UpperPos_Comm_ReceivedData);

            //        m_strDustCollector_UpperPos_Comm_ReceivedData = "";
            //    }
            //}
        }

        private void DustCollector_LowerPos_DataReceivedHandler(byte[] receiveData)
        {
            string @string = Encoding.Default.GetString(receiveData);

            //m_strDustCollector_LowerPos_Comm_ReceivedData += @string;
            //if (m_strDustCollector_LowerPos_Comm_ReceivedData.Length >= 1)
            //{
            //    if (m_strDustCollector_LowerPos_Comm_ReceivedData[m_strDustCollector_LowerPos_Comm_ReceivedData.Length - 1] == '\r')
            //    {
            //        m_bDustCollector_LowerPos_CommData_Received = true;
            //    }
            //    else if (m_strDustCollector_LowerPos_Comm_ReceivedData.Length >= 2 && m_strDustCollector_LowerPos_Comm_ReceivedData[m_strDustCollector_LowerPos_Comm_ReceivedData.Length - 2] == '\r' && m_strDustCollector_LowerPos_Comm_ReceivedData[m_strDustCollector_LowerPos_Comm_ReceivedData.Length - 1] == '\n')
            //    {
            //        m_bDustCollector_LowerPos_CommData_Received = true;

            //        MessageBox.Show(m_strDustCollector_LowerPos_Comm_ReceivedData);

            //        m_strDustCollector_LowerPos_Comm_ReceivedData = "";
            //    }
            //}
        }

        private void DustCollector_UpperPos_DisconnectedHandler()
        {
            Console.WriteLine("Dust Collector serial COM5 disconnected");
        }

        private void DustCollector_LowerPos_DisconnectedHandler()
        {
            Console.WriteLine("Dust Collector serial COM6 disconnected");
        }

        /// <summary>
        /// 
        /// 집진기 인버터 통신 프로토콜 - 시작
        /// 
        /// </summary>

        //  ENQ : 0x15
        //  ACK : 0x06
        //  NAK : 0x15
        //  EOT : 0x04
        //  국번 : 0x01 ~ 0xFA
        //  명령어 : 읽기(0x52), 쓰기(0x57), 모니터 등록 요구(0x58), 모니터 등록 실행 요구(0x59)
        //  번지 : 0x00000000 ~ 0xFFFFFFFF
        //  데이터 : (n X 0x00000000) ~ (n x 0xFFFFFFFF)
        //  번지 개수 : '1'(0x31) ~ '8'(0x38)
        //  SUM : 국번 + 명령어 + 데이터(번지, 번지 개수, 데이터, 에러코드) 의 하위 2바이트


        //  읽기 요구
        //  ENQ(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 번지(4 bytes) + 번지 개수(1 byte) + SUM(2 bytes) + EOT(1 byte)

        //  읽기 응답
        //  ACK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 데이터(n x 4 bytes) + SUM(2 bytes) + EOT(1 byte)      --> 정상
        //  NAK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 에러 코드(2 bytes) + SUM(2 bytes) + EOT(1 byte)       --> 에러


        //  쓰기 요구
        //  ENQ(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 번지(4 bytes) + 번지 개수(1 byte) + 데이터(n x 4 bytes) + SUM(2 bytes) + EOT(1 byte)

        //  쓰기 응답
        //  ACK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 데이터(n x 4 bytes) + SUM(2 bytes) + EOT(1 byte)      --> 정상
        //  NAK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 에러 코드(2 bytes) + SUM(2 bytes) + EOT(1 byte)       --> 에러


        //  모니터 등록 요구
        //  ENQ(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 번지 개수(1 byte) + 번지(n x 4 bytes) + SUM(2 bytes) + EOT(1 byte)

        //  모니터 등록 응답
        //  ACK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + SUM(2 bytes) + EOT(1 byte)                            --> 정상
        //  NAK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 에러 코드(2 bytes) + SUM(2 bytes) + EOT(1 byte)       --> 에러


        //  모니터 등록 실행 요구
        //  ENQ(1 byte) + 국번(2 bytes) + 명령어(1 byte) + SUM(2 bytes) + EOT(1 byte)

        //  모니터 등록 실행 응답
        //  ACK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 데이터(n x 4 bytes) + SUM(2 bytes) + EOT(1 byte)      --> 정상
        //  NAK(1 byte) + 국번(2 bytes) + 명령어(1 byte) + 에러 코드(2 bytes) + SUM(2 bytes) + EOT(1 byte)       --> 에러

        /// <summary>
        /// 
        /// 집진기 인버터 통신 프로토콜 - 끝
        /// 
        /// </summary>

        public bool DustCollectorComm_Send_Read(int m_nDustCollector, string m_strAddr, int m_nAddrCount)
        {
            bool m_bRet = false;

            int m_DataNum = 0;
            int m_nCheckSum = 0;
            byte m_btTemp;
            string m_strSendData = "";
            byte[] m_cSendCmd = null;



            m_DataNum = 12;                                         //  길이 고정
            m_cSendCmd = new byte[m_DataNum];

            m_cSendCmd[0] = chrENQ;                                 //  ENQ 1자리
            m_cSendCmd[1] = (byte)'0';                              //  국번 2자리 (앞)
            m_cSendCmd[2] = (byte)'1';                              //  국번 2자리 (뒤)
            m_cSendCmd[3] = chrR;                                   //  CMD 1자리
            m_nCheckSum = m_cSendCmd[1] + m_cSendCmd[2] + m_cSendCmd[3];    //  CheckSum

            for (int i = 0; i < m_strAddr.Length; i++)
            {
                m_cSendCmd[4 + i] = (byte)m_strAddr[i];             //  번지 4자리
                m_nCheckSum += m_cSendCmd[4 + i];                           //  CheckSum
            }

            m_cSendCmd[8] = (byte)(char)(m_nAddrCount + '0') ;      //  번지 개수 1자리
            m_nCheckSum += m_cSendCmd[8];                                   //  CheckSum

            int m_nTemp = m_nCheckSum & 0xFF;                       //  CheckSum 계산 (하위 1바이트)
            m_btTemp = (byte)m_nTemp;
            string m_strCheckSum = m_btTemp.ToString("x2");

            m_cSendCmd[9] = (byte)m_strCheckSum[0];                 //  CheckSum 2자리 중 앞자리
            m_cSendCmd[10] = (byte)m_strCheckSum[1];                //  CheckSum 2자리 중 뒷자리
            m_cSendCmd[11] = chrEOT;



            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_nDustCollector == (int)nDustCollector.DustCollector_Upper)
            {
                if (m_dustCollector_UpperPos_Comm.IsOpen)
                {
                    m_dustCollector_UpperPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }
            else if (m_nDustCollector == (int)nDustCollector.DustCollector_Lower)
            {
                if (m_dustCollector_LowerPos_Comm.IsOpen)
                {
                    m_dustCollector_LowerPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }

            return m_bRet;
        }

        public bool DustCollectorComm_Send_Write(int m_nDustCollector, string m_strAddr, int m_nAddrCount, string m_strData)
        {
            bool m_bRet = false;

            int m_nIndex = 0;
            int m_DataNum = 0;
            int m_nCheckSum = 0;
            byte m_btTemp;
            string m_strSendData = "";
            byte[] m_cSendCmd = null;



            m_DataNum = 12 + (4 * m_nAddrCount);                    //  데이터 개수에 따라 길이 가변
            m_cSendCmd = new byte[m_DataNum];

            m_cSendCmd[0] = chrENQ;                                 //  ENQ 1자리
            m_cSendCmd[1] = (byte)'0';                              //  국번 2자리 (앞)
            m_cSendCmd[2] = (byte)'1';                              //  국번 2자리 (뒤)
            m_cSendCmd[3] = chrW;                                   //  CMD 1자리
            m_nCheckSum = m_cSendCmd[1] + m_cSendCmd[2] + m_cSendCmd[3];    //  CheckSum

            for (int i = 0; i < m_strAddr.Length; i++)
            {
                m_cSendCmd[4 + i] = (byte)m_strAddr[i];             //  번지 4자리
                m_nCheckSum += m_cSendCmd[4 + i];                           //  CheckSum
            }

            m_cSendCmd[8] = (byte)(char)(m_nAddrCount + '0');       //  번지 개수 1자리
            m_nCheckSum += m_cSendCmd[8];                                   //  CheckSum

            for (int i = 0; i < m_strData.Length; i++)
            {
                m_nIndex = 9 + i;
                m_cSendCmd[m_nIndex] = (byte)m_strData[i];       //  데이터 (번지 개수 * 4자리)
                m_nCheckSum += m_cSendCmd[m_nIndex];                     //  CheckSum
            }

            int m_nTemp = m_nCheckSum & 0xFF;                       //  CheckSum 계산 (하위 1바이트)
            m_btTemp = (byte)m_nTemp;
            string m_strCheckSum = m_btTemp.ToString("x2");

            m_cSendCmd[m_nIndex + 1] = (byte)m_strCheckSum[0];           //  CheckSum 2자리 중 앞자리
            m_cSendCmd[m_nIndex + 2] = (byte)m_strCheckSum[1];           //  CheckSum 2자리 중 뒷자리
            m_cSendCmd[m_nIndex + 3] = chrEOT;



            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_nDustCollector == (int)nDustCollector.DustCollector_Upper)
            {
                if (m_dustCollector_UpperPos_Comm.IsOpen)
                {
                    m_dustCollector_UpperPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }
            else if (m_nDustCollector == (int)nDustCollector.DustCollector_Lower)
            {
                if (m_dustCollector_LowerPos_Comm.IsOpen)
                {
                    m_dustCollector_LowerPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }

            return m_bRet;
        }

        public bool DustCollectorComm_Send_MonitorReg(int m_nDustCollector, int m_nAddrCount, string m_strAddr)
        {
            bool m_bRet = false;

            int m_nIndex = 0;
            int m_DataNum = 0;
            int m_nCheckSum = 0;
            byte m_btTemp;
            string m_strSendData = "";
            byte[] m_cSendCmd = null;



            m_DataNum = 8 + (4 * m_nAddrCount);                     //  데이터 개수에 따라 길이 가변
            m_cSendCmd = new byte[m_DataNum];

            m_cSendCmd[0] = chrENQ;                                 //  ENQ 1자리
            m_cSendCmd[1] = (byte)'0';                              //  국번 2자리 (앞)
            m_cSendCmd[2] = (byte)'1';                              //  국번 2자리 (뒤)
            m_cSendCmd[3] = chrX;                                   //  CMD 1자리
            m_nCheckSum = m_cSendCmd[1] + m_cSendCmd[2] + m_cSendCmd[3];    //  CheckSum

            m_cSendCmd[4] = (byte)(char)(m_nAddrCount + '0');       //  번지 개수 1자리
            m_nCheckSum += m_cSendCmd[4];                                   //  CheckSum

            for (int i = 0; i < m_strAddr.Length; i++)
            {
                m_nIndex = 5 + i;
                m_cSendCmd[m_nIndex] = (byte)m_strAddr[i];          //  번지 (번지 개수 * 4자리)
                m_nCheckSum += m_cSendCmd[m_nIndex];                        //  CheckSum
            }

            int m_nTemp = m_nCheckSum & 0xFF;                       //  CheckSum 계산 (하위 1바이트)
            m_btTemp = (byte)m_nTemp;
            string m_strCheckSum = m_btTemp.ToString("x2");

            m_cSendCmd[m_nIndex + 1] = (byte)m_strCheckSum[0];              //  CheckSum 2자리 중 앞자리
            m_cSendCmd[m_nIndex + 2] = (byte)m_strCheckSum[1];              //  CheckSum 2자리 중 뒷자리
            m_cSendCmd[m_nIndex + 3] = chrEOT;



            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_nDustCollector == (int)nDustCollector.DustCollector_Upper)
            {
                if (m_dustCollector_UpperPos_Comm.IsOpen)
                {
                    m_dustCollector_UpperPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }
            else if (m_nDustCollector == (int)nDustCollector.DustCollector_Lower)
            {
                if (m_dustCollector_LowerPos_Comm.IsOpen)
                {
                    m_dustCollector_LowerPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }

            return m_bRet;
        }

        public bool DustCollectorComm_Send_MonitorRead(int m_nDustCollector)
        {
            bool m_bRet = false;

            int m_nIndex = 0;
            int m_DataNum = 0;
            int m_nCheckSum = 0;
            byte m_btTemp;
            string m_strSendData = "";
            byte[] m_cSendCmd = null;



            m_DataNum = 7;                                          //  데이터 길이 고정
            m_cSendCmd = new byte[m_DataNum];

            m_cSendCmd[0] = chrENQ;                                 //  ENQ 1자리
            m_cSendCmd[1] = (byte)'0';                              //  국번 2자리 (앞)
            m_cSendCmd[2] = (byte)'1';                              //  국번 2자리 (뒤)
            m_cSendCmd[3] = chrY;                                   //  CMD 1자리
            m_nCheckSum = m_cSendCmd[1] + m_cSendCmd[2] + m_cSendCmd[3];    //  CheckSum

            int m_nTemp = m_nCheckSum & 0xFF;                       //  CheckSum 계산 (하위 1바이트)
            m_btTemp = (byte)m_nTemp;
            string m_strCheckSum = m_btTemp.ToString("x2");

            m_cSendCmd[4] = (byte)m_strCheckSum[0];                 //  CheckSum 2자리 중 앞자리
            m_cSendCmd[5] = (byte)m_strCheckSum[1];                 //  CheckSum 2자리 중 뒷자리
            m_cSendCmd[6] = chrEOT;



            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_nDustCollector == (int)nDustCollector.DustCollector_Upper)
            {
                if (m_dustCollector_UpperPos_Comm.IsOpen)
                {
                    m_dustCollector_UpperPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }
            else if (m_nDustCollector == (int)nDustCollector.DustCollector_Lower)
            {
                if (m_dustCollector_LowerPos_Comm.IsOpen)
                {
                    m_dustCollector_LowerPos_Comm.Send(m_strSendData);
                    m_bRet = true;
                }
            }

            return m_bRet;
        }

        #endregion


        #region Serial Comm. - Electro Pneumatic Regulator

        public void ElectroPneumaticRegulator_Comm_Init()
        {
            m_electroRegulator_Comm = new SerialCommElectroPneumaticRegulatorPort();
            m_electroRegulator_Comm.DataReceivedHandler = ElectroPneumaticRegulator_DataReceivedHandler;
            m_electroRegulator_Comm.DisconnectedHandler = ElectroPneumaticRegulator_DisconnectedHandler;
            if (!m_electroRegulator_Comm.OpenComm("COM7", 9600, 8, StopBits.One, Parity.None, Handshake.None))
            {
                string text = "COM7 Port Open Failed! (Electro Pneumatic Regulator)";
                MessageBox.Show(text);
            }
        }

        private void ElectroPneumaticRegulator_DataReceivedHandler(byte[] receiveData)
        {
            //  전공 레귤레이터
            //  종단 코드 : CR.LF

            string @string = Encoding.Default.GetString(receiveData);

            m_strElectroRegulator_Comm_ReceivedData += @string;
            if (m_strElectroRegulator_Comm_ReceivedData.Length >= 1)
            {
                if (m_strElectroRegulator_Comm_ReceivedData.Length >= 2 && m_strElectroRegulator_Comm_ReceivedData[m_strElectroRegulator_Comm_ReceivedData.Length - 2] == '\r' && m_strElectroRegulator_Comm_ReceivedData[m_strElectroRegulator_Comm_ReceivedData.Length - 1] == '\n')
                {
                    m_bElectroRegulator_CommData_Received = true;

                    MessageBox.Show(m_strElectroRegulator_Comm_ReceivedData);

                    m_strElectroRegulator_Comm_ReceivedData = "";
                }
            }
        }

        private void ElectroPneumaticRegulator_DisconnectedHandler()
        {
            Console.WriteLine("Electro Pneumatic Regulator serial COM7 disconnected");
        }

        public bool ElectroPneumaticRegulatorComm_Pressure_Set(double m_dkPa)
        {
            bool m_bRet = false;

            double m_dMinPressure = -1.3;
            double m_dMaxPressure = -80.0;
            double m_dPressureTotalStep = 1023.0;
            double m_dPressurePerStep = (m_dMaxPressure - m_dMinPressure) / m_dPressureTotalStep;

            string m_strStepToString = "";
            int m_nCalcStep = 0;

            if ((m_dkPa > -1.3) || (m_dkPa < -80.0))
            {
                Console.WriteLine("Electro Pneumatic Regulator out of range (Available Range : -1.3kPa ~ -80.0kPa)");
                MessageBox.Show("Electro Pneumatic Regulator out of range\r\n\r\n[Available Range : -1.3kPa ~ -80.0kPa]", "Information!!");
                return m_bRet;
            }

            m_nCalcStep = (int)((m_dkPa - m_dMinPressure) / (m_dMaxPressure - m_dMinPressure) * m_dPressureTotalStep);

            if (m_electroRegulator_Comm == null)
                return m_bRet;

            m_strStepToString = m_nCalcStep.ToString();

            if (m_strStepToString.Length < 1)
            {
                return m_bRet;
            }

            string m_strSendData = "";
            byte[] m_cSendCmd = null;

            m_cSendCmd = new byte[m_strStepToString.Length + 6];            //  6 : 'SET ' + CR + LF

            m_cSendCmd[0] = (byte)'S';
            m_cSendCmd[1] = (byte)'E';
            m_cSendCmd[2] = (byte)'T';
            m_cSendCmd[3] = (byte)' ';

            for ( int i = 0; i < m_strStepToString.Length; i++)
            {
                m_cSendCmd[4 + i] = (byte)m_strStepToString[i];
            }

            m_cSendCmd[4 + m_strStepToString.Length] = chrCR;
            m_cSendCmd[4 + m_strStepToString.Length + 1] = chrLF;

            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_electroRegulator_Comm.IsOpen)
            {
                m_electroRegulator_Comm.Send(m_strSendData);
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool ElectroPneumaticRegulatorComm_Pressure_Inc()
        {
            bool m_bRet = false;

            string m_strSendData = "";
            byte[] m_cSendCmd = null;

            m_cSendCmd = new byte[5];

            m_cSendCmd[0] = (byte)'I';
            m_cSendCmd[1] = (byte)'N';
            m_cSendCmd[2] = (byte)'C';
            m_cSendCmd[3] = chrCR;
            m_cSendCmd[4] = chrLF;

            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_electroRegulator_Comm.IsOpen)
            {
                m_electroRegulator_Comm.Send(m_strSendData);
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool ElectroPneumaticRegulatorComm_Pressure_Dec()
        {
            bool m_bRet = false;

            string m_strSendData = "";
            byte[] m_cSendCmd = null;

            m_cSendCmd = new byte[5];

            m_cSendCmd[0] = (byte)'D';
            m_cSendCmd[1] = (byte)'E';
            m_cSendCmd[2] = (byte)'C';
            m_cSendCmd[3] = chrCR;
            m_cSendCmd[4] = chrLF;

            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_electroRegulator_Comm.IsOpen)
            {
                m_electroRegulator_Comm.Send(m_strSendData);
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool ElectroPneumaticRegulatorComm_SettingPressure_Read()                //  압력 설정값 읽기
        {
            bool m_bRet = false;

            string m_strSendData = "";
            byte[] m_cSendCmd = null;

            m_cSendCmd = new byte[5];

            m_cSendCmd[0] = (byte)'R';
            m_cSendCmd[1] = (byte)'E';
            m_cSendCmd[2] = (byte)'Q';
            m_cSendCmd[3] = chrCR;
            m_cSendCmd[4] = chrLF;

            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_electroRegulator_Comm.IsOpen)
            {
                m_electroRegulator_Comm.Send(m_strSendData);
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool ElectroPneumaticRegulatorComm_Pressure_Read()                //  압력값 읽기
        {
            bool m_bRet = false;

            string m_strSendData = "";
            byte[] m_cSendCmd = null;

            m_cSendCmd = new byte[5];

            m_cSendCmd[0] = (byte)'M';
            m_cSendCmd[1] = (byte)'O';
            m_cSendCmd[2] = (byte)'N';
            m_cSendCmd[3] = chrCR;
            m_cSendCmd[4] = chrLF;

            m_strSendData = Encoding.Default.GetString(m_cSendCmd);

            if (m_electroRegulator_Comm.IsOpen)
            {
                m_electroRegulator_Comm.Send(m_strSendData);
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool ElectroRegulatorCommReceivedData_To_Pressure_Value(int m_nPressureStep, ref double m_dkPa)
        {
            bool m_bRet = false;

            double m_dMinPressure = -1.3;
            double m_dMaxPressure = -80.0;
            double m_dPressureTotalStep = 1023.0;
            double m_dPressurePerStep = (m_dMaxPressure - m_dMinPressure) / m_dPressureTotalStep;

            int m_nCalcStep = 0;

            if ((m_nPressureStep < 0) || (m_nPressureStep > 1023))
            {
                Console.WriteLine("Electro Pneumatic Regulator Value out of range (Available Range : 0 ~ 1023)");
                MessageBox.Show("Electro Pneumatic Regulator Value out of range\r\n\r\n[Available Range : 0 ~ 1023]", "Information!!");
                return m_bRet;
            }

            m_dkPa = m_dMinPressure + ((double)m_nPressureStep / (double)1023) * (m_dMaxPressure - m_dMinPressure);

            if ((m_dkPa <= -1.3) && (m_dkPa >= -80.0))
            {
                m_bRet = true;
            }

            return m_bRet;
        }

        #endregion


        #region Teaching Position List Save / Load

        public bool Teaching_Position_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\WorkStage_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("WorkStage Teaching Position 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Position 데이터 로드
            for (int i = 0; i < System.Enum.GetValues(typeof(WorkStage_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Transfer X
                NativeMethods.GetPrivateProfileString(strTemp, "StageX", "0", temp, 255, strFIle);
                stWorkStageTeachingPos[i].Stage_X = Convert.ToDouble(temp.ToString());
                //  Transfer Z
                NativeMethods.GetPrivateProfileString(strTemp, "StageY", "0", temp, 255, strFIle);
                stWorkStageTeachingPos[i].Stage_Y = Convert.ToDouble(temp.ToString());
            }

            return m_bRet;
        }

        public void Teaching_Position_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\WorkStage_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("WorkStage Teaching Position 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Position Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(WorkStage_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Transfer X
                NativeMethods.WritePrivateProfileString(strTemp, "StageX", stWorkStageTeachingPos[i].Stage_X.ToString(), strFIle);
                //  Transfer Z
                NativeMethods.WritePrivateProfileString(strTemp, "StageY", stWorkStageTeachingPos[i].Stage_Y.ToString(), strFIle);
            }

            MessageBox.Show("Teaching Position 을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #endregion


        #region Event Handler

        private void Timer_MainWork_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            m_btimer_MainWork_Stop = false;
            timer_MainWork.Enabled = false; 

            if (!m_btimer_MainWork_Stop)
            {
                timer_MainWork.Enabled = true;
            }
        }

        private void Timer_SubWork_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            m_btimer_SubWork_Stop = false;
            timer_SubWork.Enabled = false;

            if (!m_btimer_SubWork_Stop)
            {
                timer_SubWork.Enabled = true;
            }
        }

        private void Timer_ProductAlign_Func(object sender, EventArgs e)
        {
            if (!m_bAlignVisionThread_Use)
            {
                
            }
        }

        private void Timer_MotionHome_Func(object sender, EventArgs e)
        {
            m_btimer_Motion_Home_Stop = false;
            timer_Motion_Home.Enabled = false;

            if (!m_btimer_Motion_Home_Stop)
            {
                timer_Motion_Home.Enabled = true;
            }
        }

        private void Timer_ReticleGlass_Func(object sender, EventArgs e)
        {
            m_btimer_ReticleGlass_Check_Stop = false;
            timer_ReticleGlass_Check.Enabled = false;

            if (!m_btimer_ReticleGlass_Check_Stop)
            {
                timer_ReticleGlass_Check.Enabled = true;
            }
        }

        public void forThread_MainWorkCycle()
        {
            //  Main-Work Cycle

        }

        public void forThread_SubWorkCycle()
        {
            //  Sub-Work Cycle

        }

        public void forThread_AlignVisionCycle()
        {
            //Run_ProductAlign_Func();            
            Run_FindAlignMark_Func();
        }


        #endregion

        #region Method

        public List<string> GetPositionList()
        {
            List<string> ret = new List<string>();

            foreach (XyzyPositionData position in Config.Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        #endregion


        //        public XytCoordinate GetCurrentPosition()
        //        {
        //            XytCoordinate current = new XytCoordinate();
        //            if (Stage != null)
        //            {
        //                current.X = Stage.Axes["X"].Motor.ActualPosition;
        //                current.Y = Stage.Axes["Y"].Motor.ActualPosition;
        //                current.T = Stage.Axes["T"].Motor.ActualPosition;
        //            }
        //
        //            return current;
        //        }



        #region Rotation And Offset Move

        //라디안을 각도로 변환
        public static double RadianToDegree(double radian)
        {
            return (radian * 180.0 / Math.PI);
        }

        //각도를 라디안으로 변환
        public static double DegreeToRadian(double degree)
        {
            return (Math.PI / 180.0) * degree;
        }        

        #endregion


        #region 드라이브 용량 확인

        public double GetDriveSpace( string m_strDrive )
        {
            double m_dSpace = 0.0;

            DriveInfo drv = new DriveInfo(m_strDrive);
            m_dSpace = (double)drv.TotalFreeSpace / 1024.0 / 1024.0 / 1024.0;

            return m_dSpace;
        }

        #endregion


        //  원점 기준 회전
        public static PointD RotatePoint(PointD cen, PointD p1, double radian)
        {
            PointD rP = p1;

            //  CCW 회전
            rP.X = ((p1.X - cen.X) * Math.Cos(radian) - (p1.Y - cen.Y) * Math.Sin(radian)) + cen.X;
            rP.Y = ((p1.X - cen.X) * Math.Sin(radian) + (p1.Y - cen.Y) * Math.Cos(radian)) + cen.Y;

            //  CW 회전
            //rP.X = ((p1.X - cen.X) * Math.Cos(radian) + (p1.Y - cen.Y) * Math.Sin(radian)) + cen.X;
            //rP.Y = (-(p1.X - cen.X) * Math.Sin(radian) + (p1.Y - cen.Y) * Math.Cos(radian)) + cen.Y;

            return rP;
        }

        //  제자리 회전
        public static PointD RotatePoint_CurPos(PointD cen, PointD p1, double radian)
        {
            //  현재 Group Center 좌표
            PointD curCenter = cen;

            PointD rP = p1;
            p1.X -= curCenter.X;
            p1.Y -= curCenter.Y;

            p1.X -= curCenter.X;
            p1.Y -= curCenter.Y;

            //  CCW 회전
            rP.X = ((p1.X) * Math.Cos(radian) - (p1.Y) * Math.Sin(radian));
            rP.Y = ((p1.X) * Math.Sin(radian) + (p1.Y) * Math.Cos(radian));

            //  CW 회전
            //rP.X = ((p1.X) * Math.Cos(radian) + (p1.Y) * Math.Sin(radian));
            //rP.Y = (-(p1.X) * Math.Sin(radian) + (p1.Y) * Math.Cos(radian));

            rP.X += curCenter.X;
            rP.Y += curCenter.Y;

            return rP;
        }


        #region Align Mark Find
        void Run_FindAlignMark_Func()
        {
            switch (m_nFindAlignMark_Step)
            {
                case (int)FindAlignMark_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "마크 찾기 시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_bFindAlignMark_OK = false;
                    m_bFindUpperAlignMark_OK = false;
                    m_bFindLowerAlignMark_OK = false;

                    //  어느 쪽 마크를 찾을 것인지... 1번 마크인지 2번 마크인지...
                    if ((m_nFindAlignMarkType == (int)AlignMarkType.ALIGN_2POINT) || (m_nFindAlignMarkType == (int)AlignMarkType.ALIGN_1STMARK))        //  2 Point 찾기나, 1번 마크 찾기일 경우
                    {
                        m_nFindAlignMarkType = (int)AlignMarkType.ALIGN_1STMARK;
                    }
                    else                                                                                                                                //  위 경우가 아니면 2번 마크 찾는 것으로...
                    {
                        m_nFindAlignMarkType = (int)AlignMarkType.ALIGN_2NDMARK;
                    }

                    m_nFindAlignMark_Step = (int)FindAlignMark_Step.FindMark_Start;
                    break;


                case (int)FindAlignMark_Step.FindMark_Start:                                           //  Align Start
                    //if (!m_bLowerVision_Align)
                    if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_PAK)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "PAK 카메라 마크 찾기 시작");

                        //  라이브 상태가 아니면 라이브로 변경
                        if (jigAligner_HighRes.Camera.IsLiveOn == false)
                        {
                            jigAligner_HighRes.Camera.StartLive();
                        }

                        jigAligner_HighRes.Work();
                    }
                    else if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_Wafer)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "Wafer 카메라 마크 찾기 시작");

                        //  라이브 상태가 아니면 라이브로 변경
                        if (jigAligner_LowRes.Camera.IsLiveOn == false)
                        {
                            jigAligner_LowRes.Camera.StartLive();
                        }

                        jigAligner_LowRes.Work();
                    }
                    else if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_Reticle_Upper)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "PAK 카메라 Reticle 마크 찾기 시작");

                        //  라이브 상태가 아니면 라이브로 변경
                        if (jigAligner_HighRes.Camera.IsLiveOn == false)
                        {
                            jigAligner_HighRes.Camera.StartLive();
                        }

                        jigAligner_HighRes.Work();
                    }
                    else if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_Reticle_Lower)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "Wafer 카메라 Reticle 마크 찾기 시작");

                        //  라이브 상태가 아니면 라이브로 변경
                        if (jigAligner_LowRes.Camera.IsLiveOn == false)
                        {
                            jigAligner_LowRes.Camera.StartLive();
                        }

                        jigAligner_LowRes.Work();
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "지정되지 않은 얼라이너");

                        m_bFindUpperAlignMark_OK = false;
                        m_bFindLowerAlignMark_OK = false;
                    }

                    if ((m_nVisionAligner_Type >= (int)Aligner_Type.Aligner_Wafer) && (m_nVisionAligner_Type <= (int)Aligner_Type.Aligner_Reticle_Upper))
                    {
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.FindMark_ResultCheck;
                    }
                    else
                    {
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.None;
                    }
                    break;


                case (int)FindAlignMark_Step.FindMark_ResultCheck:                                        //  Align 결과 확인

                    m_nFindAlignMark_Step = (int)FindAlignMark_Step.Complete;
                    break;


                case (int)FindAlignMark_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "마크 찾기 완료");

                    //if (!m_bLowerVision_Align)
                    if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_PAK)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "PAK 카메라 마크 찾기 완료");

                        if ((jigAligner_HighRes.FirstPosition.X == 0.0) || (jigAligner_HighRes.FirstPosition.Y == 0.0))
                        {
                            m_bFindUpperAlignMark_OK = false;
                        }
                        else
                        {
                            m_bFindUpperAlignMark_OK = true;
                        }
                    }
                    else if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_Wafer)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "Wafer 카메라 마크 찾기 완료");

                        if ((jigAligner_LowRes.FirstPosition.X == 0.0) || (jigAligner_LowRes.FirstPosition.Y == 0.0))
                        {
                            m_bFindLowerAlignMark_OK = false;
                        }
                        else
                        {
                            m_bFindLowerAlignMark_OK = true;
                        }
                    }
                    else if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_Reticle_Upper)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "PAK 카메라 Reticle 마크 찾기 완료");

                        if ((reticleAligner_HighRes.FirstPosition.X == 0.0) || (reticleAligner_HighRes.FirstPosition.Y == 0.0))
                        {
                            m_bFindUpperAlignMark_OK = false;
                        }
                        else
                        {
                            m_bFindUpperAlignMark_OK = true;
                        }
                    }
                    else if (m_nVisionAligner_Type == (int)Aligner_Type.Aligner_Reticle_Lower)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "Wafer 카메라 Reticle 마크 찾기 완료");

                        if ((reticleAligner_LowRes.FirstPosition.X == 0.0) || (reticleAligner_LowRes.FirstPosition.Y == 0.0))
                        {
                            m_bFindLowerAlignMark_OK = false;
                        }
                        else
                        {
                            m_bFindLowerAlignMark_OK = true;
                        }
                    }

                    m_nFindAlignMark_Step = (int)FindAlignMark_Step.None;
                    break;
            }
        }
        #endregion



        #region Home Function

        void Run_Home_Func()
        {
            bool m_bRet = false;
            string m_strTemp;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nHomeStep >= (int)Home_Step.Start)
            {
                //if (Config.ParamConfig.AreaSensor_Usage && (waferProbeAlignParameter.DI_AreaSensor_Detect() || waferProbeAlignParameter.DI_AlignJig_Detect()))
                //{
                //    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "안전 센서 감지로 인한 장비 Stop");

                //    m_bInCycleMoving_SafetySensor_Detected = true;

                //    //  알람 정지 (LED Bar - Red Blink)
                //    Equipment.MachineStop_byAlarm = true;

                //    timer_Motion_Home.Enabled = false;
                //    m_btimer_Motion_Home_Stop = true;

                //    m_nHomeStep = (int)Home_Step.None;

                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, 500);

                //    for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //    {
                //        MC_Func.MC_MotorStop(i, 2000);
                //        //MC_Func.MC_EStop(i);
                //    }

                //    if (!Equipment.User_QMC_Engineer)                   //  QMC 관리자가 아닐 경우에만 Home Flag 를 false 로
                //    {
                //        m_bHomeOK = false;                              //  안전센서 감지 시 무조건 장비 초기화 해야 함
                //    }

                //    if (!Equipment.User_QMC_Engineer && Config.ParamConfig.AreaSensor_ServoOff_Usage)           //  안전센서 감지 시 Servo Off 할 경우
                //    {
                //        for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //        {
                //            MC_Func.MC_SetServoOnOff(i, false);
                //        }
                //    }
                //}
            }            


            switch (m_nHomeStep)
            {
                case (int)Home_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nHomeAxisCount = 0;
                    m_bHomeOK = false;

                    //  모듈들의 축 번호 중 Max 값이 가장 큰 것을 찾아서 사용 (SLD-200 은 Unloader 가 가장 뒤쪽에 배치됨)
                    m_nTotalAxisCount = (int)UnloaderParameter.AxisAjinEnum.Max;

                    //  Unlaoder 모터 전체 Stop
                    for (int m_ul = (int)UnloaderParameter.AxisAjinEnum.Z0; m_ul < (int)UnloaderParameter.AxisAjinEnum.Max; m_ul++)
                    {
                        MC_Func.MC_MotorStop(m_ul, 2000);
                    }

                    //  MainUnit 모터 전체 Stop
                    for (int m_main = (int)WorkStageParameter.AxisAjinEnum.X; m_main < (int)WorkStageParameter.AxisAjinEnum.Max; m_main++)
                    {
                        MC_Func.MC_MotorStop(m_main, 2000);
                    }

                    //  Laoder 모터 전체 Stop
                    for (int m_ld = (int)LoaderParameter.AxisAjinEnum.Z0; m_ld < (int)LoaderParameter.AxisAjinEnum.Max; m_ld++)
                    {
                        MC_Func.MC_MotorStop(m_ld, 2000);
                    }

                    m_nHomeStep = (int)Home_Step.AxisAlarmCheck;
                    break;


                case (int)Home_Step.AxisAlarmCheck:                                         //  서보 축 알람 체크

                    //  0 ~ 13 번 축 까지 있음. (0:Stage X,             1:Stage V,              2:Scanner Z,                3:Mask Y,
                    //                          4:Loader Stacker 0,     5:Loader Stacker 1,     6:Loader Transfer X,        7:Loader Transfer Z,        8:Align X, 9:Align Y,
                    //                          10:Unloader Stacker 0,  11:Unloader Stacker 1,  12:Unloader Transfer X,     13:Unloader Transfer Z)

                    if (m_nHomeAxisCount <= m_nTotalAxisCount)
                    {
                        if (MC_Func.MC_IsAlarm(m_nHomeAxisCount))
                        {
                            m_nHomeStep = (int)Home_Step.AlarmAxisServoOff;
                        }
                        else
                        {
                            MC_Func.MC_SetServoOnOff(m_nHomeAxisCount, true);
                            m_nHomeAxisCount++;
                        }
                    }
                    else
                    {
                        m_nHomeStep = (int)Home_Step.ScannerZ_TransferZ_HomeStart;
                    }
                    break;


                case (int)Home_Step.AlarmAxisServoOff:                                      //  알람 축 서보 Off
                    MC_Func.MC_SetServoOnOff(m_nHomeAxisCount, false);

                    m_nHomeStep = (int)Home_Step.AlarmAxisServoOffCheck;
                    break;


                case (int)Home_Step.AlarmAxisServoOffCheck:                                 //  알람 축 서보 Off 확인
                    if (!MC_Func.MC_IsServoOn(m_nHomeAxisCount))
                    {
                        TickCount_Start((int)TickType.TICK_HOME);

                        m_nHomeStep = (int)Home_Step.AlarmAxisAlarmResetOn;
                    }
                    break;


                case (int)Home_Step.AlarmAxisAlarmResetOn:                                  //  알람 축 리셋 신호 On
                    if (TickCount_Elapsed((int)TickType.TICK_HOME) > 300)
                    {
                        MC_Func.MC_AlarmReset(m_nHomeAxisCount, true);

                        TickCount_Start((int)TickType.TICK_HOME);

                        m_nHomeStep = (int)Home_Step.AlarmAxisAlarmResetOff;
                    }
                    break;


                case (int)Home_Step.AlarmAxisAlarmResetOff:                                 //  알람 축 리셋 신호 Off (30ms delay 후 Off)
                    if (TickCount_Elapsed((int)TickType.TICK_HOME) > 300)
                    {
                        MC_Func.MC_AlarmReset(m_nHomeAxisCount, false);

                        TickCount_Start((int)TickType.TICK_HOME);

                        m_nHomeStep = (int)Home_Step.AlarmAxisServoOn;
                    }
                    break;


                case (int)Home_Step.AlarmAxisServoOn:                                       //  알람 축 서보 On
                    if (TickCount_Elapsed((int)TickType.TICK_HOME) > 300)
                    {
                        MC_Func.MC_SetServoOnOff(m_nHomeAxisCount, true);

                        m_nHomeStep = (int)Home_Step.AlarmAxisServoOnCheck;
                    }
                    break;


                case (int)Home_Step.AlarmAxisServoOnCheck:                                  //  알람 축 서보 On 확인
                    if (MC_Func.MC_IsServoOn(m_nHomeAxisCount))
                    {
                        m_nHomeAxisCount++;

                        m_nHomeStep = (int)Home_Step.AxisAlarmCheck;
                    }
                    break;


                case (int)Home_Step.ScannerZ_TransferZ_HomeStart:                           //  Scanner Z 축, Loader Z 축, Unloader Z 축 홈 실행

                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "Scanner, LD, UL Z 축 초기화 시작");

                    MC_Func.MC_HomeSearch((int)WorkStageParameter.AxisAjinEnum.Z);
                    MC_Func.MC_HomeSearch((int)LoaderParameter.AxisAjinEnum.TR_Z);
                    MC_Func.MC_HomeSearch((int)UnloaderParameter.AxisAjinEnum.TR_Z);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.ScannerZ_TransferZ_HomeCompleteCheck;
                    break;


                case (int)Home_Step.ScannerZ_TransferZ_HomeCompleteCheck:                   //  Scanner Z 축, Loader Z 축, Unloader Z 축 홈 완료 체크

                    if ((TickCount_Elapsed((int)TickType.TICK_HOME) > 100) &&
                        !MC_Func.MC_GetHoming((int)WorkStageParameter.AxisAjinEnum.Z) && MC_Func.MC_GetInposition((int)WorkStageParameter.AxisAjinEnum.Z) &&
                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.TR_Z) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.TR_Z) &&
                        !MC_Func.MC_GetHoming((int)UnloaderParameter.AxisAjinEnum.TR_Z) && MC_Func.MC_GetInposition((int)UnloaderParameter.AxisAjinEnum.TR_Z) )
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "Scanner, LD, UL Z 축 초기화 완료");

                        m_nHomeStep = (int)Home_Step.Remained_AllAxis_HomeStart;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "Scanner, LD, UL Z 축 초기화 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nHomeStep = (int)Home_Step.None;

                        MessageBox.Show("Scanner, LD, UL Z 축 초기화 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Home_Step.Remained_AllAxis_HomeStart:                             //  나머지 축 전체 홈 실행

                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "나머지 축 전체 초기화 시작");

                    MC_Func.MC_HomeSearch((int)WorkStageParameter.AxisAjinEnum.X);
                    MC_Func.MC_HomeSearch((int)WorkStageParameter.AxisAjinEnum.Y);
                    MC_Func.MC_HomeSearch((int)WorkStageParameter.AxisAjinEnum.MASK_Y);

                    MC_Func.MC_HomeSearch((int)LoaderParameter.AxisAjinEnum.Z0);
                    MC_Func.MC_HomeSearch((int)LoaderParameter.AxisAjinEnum.Z1);
                    MC_Func.MC_HomeSearch((int)LoaderParameter.AxisAjinEnum.TR_X);
                    MC_Func.MC_HomeSearch((int)LoaderParameter.AxisAjinEnum.ALN_X);
                    MC_Func.MC_HomeSearch((int)LoaderParameter.AxisAjinEnum.ALN_Y);

                    MC_Func.MC_HomeSearch((int)UnloaderParameter.AxisAjinEnum.Z0);
                    MC_Func.MC_HomeSearch((int)UnloaderParameter.AxisAjinEnum.Z1);
                    MC_Func.MC_HomeSearch((int)UnloaderParameter.AxisAjinEnum.TR_X);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.Remained_AllAxis_HomeCompleteCheck;
                    break;


                case (int)Home_Step.Remained_AllAxis_HomeCompleteCheck:                     //  나머지 축 전체 홈 완료 체크

                    if ((TickCount_Elapsed((int)TickType.TICK_HOME) > 100) &&
                        !MC_Func.MC_GetHoming((int)WorkStageParameter.AxisAjinEnum.X) && MC_Func.MC_GetInposition((int)WorkStageParameter.AxisAjinEnum.X) &&
                        !MC_Func.MC_GetHoming((int)WorkStageParameter.AxisAjinEnum.Y) && MC_Func.MC_GetInposition((int)WorkStageParameter.AxisAjinEnum.Y) &&
                        !MC_Func.MC_GetHoming((int)WorkStageParameter.AxisAjinEnum.MASK_Y) && MC_Func.MC_GetInposition((int)WorkStageParameter.AxisAjinEnum.MASK_Y) &&

                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.Z0) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.Z0) &&
                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.Z1) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.Z1) &&
                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.TR_X) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.TR_X) &&
                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.ALN_X) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.ALN_X) &&
                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.ALN_Y) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.ALN_Y) &&

                        !MC_Func.MC_GetHoming((int)UnloaderParameter.AxisAjinEnum.Z0) && MC_Func.MC_GetInposition((int)UnloaderParameter.AxisAjinEnum.Z0) &&
                        !MC_Func.MC_GetHoming((int)UnloaderParameter.AxisAjinEnum.Z1) && MC_Func.MC_GetInposition((int)UnloaderParameter.AxisAjinEnum.Z1) &&
                        !MC_Func.MC_GetHoming((int)UnloaderParameter.AxisAjinEnum.TR_X) && MC_Func.MC_GetInposition((int)UnloaderParameter.AxisAjinEnum.TR_X))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "나머지 축 전체 초기화 완료");

                        m_bHomeOK = true;

                        m_nHomeStep = (int)Home_Step.All_Z_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "나머지 축 전체 초기화 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nHomeStep = (int)Home_Step.None;

                        MessageBox.Show("나머지 축 전체 초기화 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Home_Step.All_Z_Move_ReadyPos:                                    //  전체 Z 축, 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "전체 Z 축, 대기 위치로 이동 시작");

                    workStageParameter.stWorkStagePosParam = workStageParameter.GetPositionInformation("Ready");
                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Ready");
                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Ready");

                    MC_Func.MC_MovePosition((int)WorkStageParameter.AxisAjinEnum.Z,
                                        workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Z],
                                        workStageParameter.stWorkStagePosParam.dVel[(int)WorkStageParameter.MotionKey.Z],
                                        workStageParameter.stWorkStagePosParam.dAcc[(int)WorkStageParameter.MotionKey.Z],
                                        workStageParameter.stWorkStagePosParam.dDec[(int)WorkStageParameter.MotionKey.Z]);

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.TR_Z,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                        loaderParameter.stLoaderPosParam.dVel[(int)LoaderParameter.MotionKey.TR_Z],
                                        loaderParameter.stLoaderPosParam.dAcc[(int)LoaderParameter.MotionKey.TR_Z],
                                        loaderParameter.stLoaderPosParam.dDec[(int)LoaderParameter.MotionKey.TR_Z]);

                    MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.TR_Z],
                                        unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.TR_Z],
                                        unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.TR_Z]);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.All_Z_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Home_Step.All_Z_Move_ReadyPos_DoneCheck:                          //  전체 Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WorkStageParameter.AxisAjinEnum.Z) && MC_Func.MC_PosTolerance((int)WorkStageParameter.AxisAjinEnum.Z, workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Z]) &&
                        MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.TR_Z) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]) &&
                        MC_Func.MC_GetDone((int)UnloaderParameter.AxisAjinEnum.TR_Z) && MC_Func.MC_PosTolerance((int)UnloaderParameter.AxisAjinEnum.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]) )
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "전체 Z 축, 대기 위치로 이동 완료");

                        m_nHomeStep = (int)Home_Step.All_XY_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) >= 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "전체 Z 축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nHomeStep = (int)Home_Step.None;

                        MessageBox.Show("전체 Z 축, Ready 위치로 이동 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Home_Step.All_XY_Move_ReadyPos:                                    //  전체 XY 축 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "전체 XY 축, 대기 위치로 이동 시작");

                    workStageParameter.stWorkStagePosParam = workStageParameter.GetPositionInformation("Ready");
                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Ready");
                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Ready");

                    MC_Func.MC_MovePosition((int)WorkStageParameter.AxisAjinEnum.X,
                                        workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X],
                                        workStageParameter.stWorkStagePosParam.dVel[(int)WorkStageParameter.MotionKey.X],
                                        workStageParameter.stWorkStagePosParam.dAcc[(int)WorkStageParameter.MotionKey.X],
                                        workStageParameter.stWorkStagePosParam.dDec[(int)WorkStageParameter.MotionKey.X]);

                    MC_Func.MC_MovePosition((int)WorkStageParameter.AxisAjinEnum.Y,
                                        workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y],
                                        workStageParameter.stWorkStagePosParam.dVel[(int)WorkStageParameter.MotionKey.Y],
                                        workStageParameter.stWorkStagePosParam.dAcc[(int)WorkStageParameter.MotionKey.Y],
                                        workStageParameter.stWorkStagePosParam.dDec[(int)WorkStageParameter.MotionKey.Y]);

                    MC_Func.MC_MovePosition((int)WorkStageParameter.AxisAjinEnum.MASK_Y,
                                        workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y],
                                        workStageParameter.stWorkStagePosParam.dVel[(int)WorkStageParameter.MotionKey.MASK_Y],
                                        workStageParameter.stWorkStagePosParam.dAcc[(int)WorkStageParameter.MotionKey.MASK_Y],
                                        workStageParameter.stWorkStagePosParam.dDec[(int)WorkStageParameter.MotionKey.MASK_Y]);

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.TR_X,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                        loaderParameter.stLoaderPosParam.dVel[(int)LoaderParameter.MotionKey.TR_X],
                                        loaderParameter.stLoaderPosParam.dAcc[(int)LoaderParameter.MotionKey.TR_X],
                                        loaderParameter.stLoaderPosParam.dDec[(int)LoaderParameter.MotionKey.TR_X]);

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.ALN_X,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                        loaderParameter.stLoaderPosParam.dVel[(int)LoaderParameter.MotionKey.ALN_X],
                                        loaderParameter.stLoaderPosParam.dAcc[(int)LoaderParameter.MotionKey.ALN_X],
                                        loaderParameter.stLoaderPosParam.dDec[(int)LoaderParameter.MotionKey.ALN_X]);

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.ALN_Y,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                        loaderParameter.stLoaderPosParam.dVel[(int)LoaderParameter.MotionKey.ALN_Y],
                                        loaderParameter.stLoaderPosParam.dAcc[(int)LoaderParameter.MotionKey.ALN_Y],
                                        loaderParameter.stLoaderPosParam.dDec[(int)LoaderParameter.MotionKey.ALN_Y]);

                    MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.TR_X,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X],
                                        unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.TR_X],
                                        unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.TR_X],
                                        unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.TR_X]);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.All_XY_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Home_Step.All_XY_Move_ReadyPos_DoneCheck:                          //  전체 XY 축 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WorkStageParameter.AxisAjinEnum.X) && MC_Func.MC_PosTolerance((int)WorkStageParameter.AxisAjinEnum.X, workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X]) &&
                        MC_Func.MC_GetDone((int)WorkStageParameter.AxisAjinEnum.Y) && MC_Func.MC_PosTolerance((int)WorkStageParameter.AxisAjinEnum.Y, workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y]) &&
                        MC_Func.MC_GetDone((int)WorkStageParameter.AxisAjinEnum.MASK_Y) && MC_Func.MC_PosTolerance((int)WorkStageParameter.AxisAjinEnum.MASK_Y, workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y]) &&
                        MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.TR_X) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]) &&
                        MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.ALN_X) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
                        MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.ALN_Y) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]) &&
                        MC_Func.MC_GetDone((int)UnloaderParameter.AxisAjinEnum.TR_X) && MC_Func.MC_PosTolerance((int)UnloaderParameter.AxisAjinEnum.TR_X, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "전체 XY 축, 대기 위치로 이동 완료");

                        m_nHomeStep = (int)Home_Step.All_StackerZ_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) >= 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "전체 XY 축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nHomeStep = (int)Home_Step.None;

                        MessageBox.Show("전체 XY 축, Ready 위치로 이동 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Home_Step.All_StackerZ_Move_ReadyPos:                             //  전체 Stacker 축 모듈 PickUp, PutDown 높이로 이동 

                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "Stacker Z 축, Module PickUp & PutDown 위치로 이동 시작");

                    //  Cycle 동작 시작

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.All_StackerZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Home_Step.All_StackerZ_Move_ReadyPos_DoneCheck:                   //  전체 Stacker 축 모듈 PickUp, PutDown 높이로 이동 완료 체크

                    if ((TickCount_Elapsed((int)TickType.TICK_HOME) > 100) &&
                        !MC_Func.MC_GetHoming((int)WorkStageParameter.AxisAjinEnum.Z) && MC_Func.MC_GetInposition((int)WorkStageParameter.AxisAjinEnum.Z) &&
                        !MC_Func.MC_GetHoming((int)LoaderParameter.AxisAjinEnum.TR_Z) && MC_Func.MC_GetInposition((int)LoaderParameter.AxisAjinEnum.TR_Z) &&
                        !MC_Func.MC_GetHoming((int)UnloaderParameter.AxisAjinEnum.TR_Z) && MC_Func.MC_GetInposition((int)UnloaderParameter.AxisAjinEnum.TR_Z))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Z 축, Module PickUp & PutDown 위치로 이동 완료");

                        m_nHomeStep = (int)Home_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Z 축, Module PickUp & PutDown 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nHomeStep = (int)Home_Step.None;

                        MessageBox.Show("Stacker Z 축, Module PickUp & PutDown 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Home_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "Machine Initialize", "완료");

                    //  m_bHomeOK = true;

                    timer_Motion_Home.Enabled = false;
                    m_btimer_Motion_Home_Stop = true;

                    m_strTemp = "===  장비 초기화 완료  ===";

                    m_nHomeStep = (int)Home_Step.None;

                    MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
        #endregion


        #region Stacker Move Function (Module PickUp & PutDown 높이로 이동 -> 이건 Loader Unloader 에서 하도록 해야 할듯???)

        void Run_Stacker_WorkPosSet_Func()
        {
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed_Stacker_Fast = 0.0;
            double m_dSpeed_Stacker_Slow = 0.0;
            double m_dSpeed_Stacker_MoreSlow = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nStackerZ_Step >= (int)StackerZ_Step.Start)
            {
                //if (Config.ParamConfig.AreaSensor_Usage && (waferProbeAlignParameter.DI_AreaSensor_Detect() || waferProbeAlignParameter.DI_AlignJig_Detect()))
                //{
                //    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "안전 센서 감지로 인한 장비 Stop");

                //    m_bInCycleMoving_SafetySensor_Detected = true;

                //    //  알람 정지 (LED Bar - Red Blink)
                //    Equipment.MachineStop_byAlarm = true;

                //    timer_Motion_Home.Enabled = false;
                //    m_btimer_Motion_Home_Stop = true;

                //    m_nHomeStep = (int)Home_Step.None;

                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, 500);

                //    for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //    {
                //        MC_Func.MC_MotorStop(i, 2000);
                //        //MC_Func.MC_EStop(i);
                //    }

                //    if (!Equipment.User_QMC_Engineer)                   //  QMC 관리자가 아닐 경우에만 Home Flag 를 false 로
                //    {
                //        m_bHomeOK = false;                              //  안전센서 감지 시 무조건 장비 초기화 해야 함
                //    }

                //    if (!Equipment.User_QMC_Engineer && Config.ParamConfig.AreaSensor_ServoOff_Usage)           //  안전센서 감지 시 Servo Off 할 경우
                //    {
                //        for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //        {
                //            MC_Func.MC_SetServoOnOff(i, false);
                //        }
                //    }
                //}
            }


            switch (m_nStackerZ_Step)
            {
                case (int)StackerZ_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    //  Laoder Stacker Z 축 모터 전체 Stop
                    MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                    //MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);

                    //  Unlaoder Stacker Z 축 모터 전체 Stop
                    //MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);
                    //MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);

                    m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveDown_FullSensorOff;
                    break;


                case (int)StackerZ_Step.StackerZ_MoveDown_FullSensorOff:                            //  Full 센서가 Off 되는 위치까지 이동 (고속)

                    Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (고속)");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Bottom");

                    //  속도 (고속)
                    m_dSpeed_Stacker_Fast = 100.0;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = 2.0;

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.Z0,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                        m_dSpeed_Stacker_Fast,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                    //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                    TickCount_Start((int)TickType.TICK_LDSZ0);

                    m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveDown_FullSensorOffCheck;
                    break;


                case (int)StackerZ_Step.StackerZ_MoveDown_FullSensorOffCheck:                       //  Full 센서가 Off 되는 위치까지 이동 완료 체크

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                    {
                        MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);

                        m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveUp_FullSensorOn;
                    }
                    else if (MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.Z0) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Bottom 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStackerZ_Step = (int)StackerZ_Step.None;

                            MessageBox.Show("Stacker Z 축, 자재가 너무 많거나 Full 수위 감지 센서 점검이 필요합니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveUp_FullSensorOn;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nStackerZ_Step = (int)StackerZ_Step.None;

                        MessageBox.Show("Stacker Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerZ_Step.StackerZ_MoveUp_FullSensorOn:                               //  Full 센서가 On 되는 위치까지 이동 (고속)

                    Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 On 되는 위치까지 이동 시작 (중속)");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                    //  속도 (중속)
                    m_dSpeed_Stacker_Fast = 50.0;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = 2.0;

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.Z0,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                        m_dSpeed_Stacker_Fast,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                    //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                    TickCount_Start((int)TickType.TICK_LDSZ0);

                    m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveUp_FullSensorOnCheck;
                    break;


                case (int)StackerZ_Step.StackerZ_MoveUp_FullSensorOnCheck:                          //  Full 센서가 On 되는 위치까지 이동 완료 체크

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                    {
                        MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);

                        m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveSlowDown_FullSensorOff;
                    }
                    else if (MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.Z0) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Top 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStackerZ_Step = (int)StackerZ_Step.None;

                            MessageBox.Show("Stacker Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveSlowDown_FullSensorOff;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nStackerZ_Step = (int)StackerZ_Step.None;

                        MessageBox.Show("Stacker Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerZ_Step.StackerZ_MoveSlowDown_FullSensorOff:                            //  Full 센서가 Off 되는 위치까지 이동 (저속)

                    Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Bottom");

                    //  속도 (고속)
                    m_dSpeed_Stacker_Fast = 20.0;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = 2.0;

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.Z0,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                        m_dSpeed_Stacker_Fast,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                    //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                    TickCount_Start((int)TickType.TICK_LDSZ0);

                    m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveSlowDown_FullSensorOffCheck;
                    break;


                case (int)StackerZ_Step.StackerZ_MoveSlowDown_FullSensorOffCheck:                       //  Full 센서가 Off 되는 위치까지 이동 완료 체크

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                    {
                        MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);

                        m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveSlowUp_FullSensorOn;
                    }
                    else if (MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.Z0) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Bottom 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStackerZ_Step = (int)StackerZ_Step.None;

                            MessageBox.Show("Stacker Z 축, 자재가 너무 많거나 Full 수위 감지 센서 점검이 필요합니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveSlowUp_FullSensorOn;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nStackerZ_Step = (int)StackerZ_Step.None;

                        MessageBox.Show("Stacker Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerZ_Step.StackerZ_MoveSlowUp_FullSensorOn:                               //  Full 센서가 On 되는 위치까지 이동 (저속 / 2)

                    Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                    //  속도 (중속)
                    m_dSpeed_Stacker_Fast = 10.0;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = 2.0;

                    MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.Z0,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                        m_dSpeed_Stacker_Fast,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                    //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                    TickCount_Start((int)TickType.TICK_LDSZ0);

                    m_nStackerZ_Step = (int)StackerZ_Step.StackerZ_MoveSlowUp_FullSensorOnCheck;
                    break;


                case (int)StackerZ_Step.StackerZ_MoveSlowUp_FullSensorOnCheck:                          //  Full 센서가 On 되는 위치까지 이동 완료 체크

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                    {
                        MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);

                        m_nStackerZ_Step = (int)StackerZ_Step.Complete;
                    }
                    else if (MC_Func.MC_GetDone((int)LoaderParameter.AxisAjinEnum.Z0) && MC_Func.MC_PosTolerance((int)LoaderParameter.AxisAjinEnum.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Top 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStackerZ_Step = (int)StackerZ_Step.None;

                            MessageBox.Show("Stacker Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStackerZ_Step = (int)StackerZ_Step.Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "Stacker Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;
                        m_btimer_Motion_Home_Stop = true;

                        m_nStackerZ_Step = (int)StackerZ_Step.None;

                        MessageBox.Show("Stacker Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerZ_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "Stacker Work Pos. Set", "완료");

                    //  m_bHomeOK = true;

                    timer_Motion_Home.Enabled = false;
                    m_btimer_Motion_Home_Stop = true;

                    m_strTemp = "===  Stacker 작업위치 이동 완료  ===";

                    m_nStackerZ_Step = (int)StackerZ_Step.None;

                    MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
        #endregion


        #region Work Stage Move Function
                
        #endregion


        #region Laser Mask Move Function

        #endregion


        public bool Machine_Parameter_Exist()
        {
            bool m_bRet = false;
            string strFIle = "";

            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle))
            {
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool Machine_Parameter_Load()
        {
            bool m_bRet = false;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            //if (File.Exists(strFIle))
            //{
            //    m_bRet = true;

                                
            //    //  구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치
            //    NativeMethods.GetPrivateProfileString("Drive_Limit", "Elev_Z", "58.0", temp, 255, strFIle);
            //    Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
            //    NativeMethods.GetPrivateProfileString("Drive_Limit", "Vision_Y", "20.0", temp, 255, strFIle);
            //    Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Minus", "-1.915", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_U_Minus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Plus", "0.085", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_U_Plus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Minus", "-0.805", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_V_Minus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Plus", "1.195", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_V_Plus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Minus", "-0.075", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_W_Minus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Plus", "1.925", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_W_Plus = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Manual Scale Usage
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Manual_Scale_Use", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ManualScale_Usage = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Lower Vision Scale X (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_Scale_X = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Lower Vision Scale Y (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_Scale_Y = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Lower Vision Scale Invert X
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Lower Vision Scale Invert Y
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Upper Vision Scale X (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_Scale_X = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Upper Vision Scale Y (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_Scale_Y = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Upper Vision Scale Invert X
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Upper Vision Scale Invert Y
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", "602", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAlign_UpperVision_LightValue = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", "964", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAlign_LowerVision_LightValue = Convert.ToInt16(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Speed", "5.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Velocity = Convert.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Accel", "100.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Accel = Convert.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Decel", "100.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Decel = Convert.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 재시도 회수
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Align_Retry", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Retries = Convert.ToInt16(temp.ToString());

            //    //  얼라인 - 얼라인 각도 Invert
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Angle_Invert", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Align_AngleInvert = temp.ToString() == "False" ? false : true;

            //    //  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Align_ThetaCalcFunction_Atan = temp.ToString() == "False" ? false : true;

            //    //  얼라인 스테이지 - Theta 회전 반경 (mm)
            //    NativeMethods.GetPrivateProfileString("Align_Stage", "Turning_Radius", "70.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance = Convert.ToDouble(temp.ToString());

            //    //  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
            //    NativeMethods.GetPrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", "1.221668451", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg = Convert.ToDouble(temp.ToString());

            //    //  얼라인 이미지 저장 여부
            //    NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_Use", "True", temp, 255, strFIle);
            //    Config.ParamConfig.AlignImageSave_Usage = temp.ToString() == "False" ? false : true;

            //    //  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
            //    NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", "10", temp, 255, strFIle);
            //    Config.ParamConfig.AlignImageSaveFolder_WarningSpace = Convert.ToDouble(temp.ToString());

            //    //  프로브 카드 클램프 타입 1 일 경우, 업다운 실린더 동작 대기 시간
            //    NativeMethods.GetPrivateProfileString("Machine_Type", "ProbeCard_ClampTypeB_CylUpDown_StableTime", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_ClampTypeB_CylUpDown_StableTime = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_CenterCheck_forAlign", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_CenterCheck_forAlign = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 - 레티클 글래스를 확인 시 비전 카메라와 엘리베이터의 충돌 방지를 위한 Vision Y 축 이동 한계 위치. (mm, 대부분 90.0 이내)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Limit", "90", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Y_Limit = Convert.ToDouble(temp.ToString());

            //    //  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X
            //    NativeMethods.GetPrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_X", "0", temp, 255, strFIle);
            //    Config.ParamConfig.PAK_WaferGate_Centering_Offset_X = Convert.ToDouble(temp.ToString());

            //    //  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y
            //    NativeMethods.GetPrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_Y", "0", temp, 255, strFIle);
            //    Config.ParamConfig.PAK_WaferGate_Centering_Offset_Y = Convert.ToDouble(temp.ToString());

            //    //  자동 로그아웃 기능 사용 여부
            //    NativeMethods.GetPrivateProfileString("Auto_LogOut", "Auto_LogOut_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Auto_LogOut_Usage = temp.ToString() == "False" ? false : true;

            //    //  자동 로그아웃 설정 시간
            //    NativeMethods.GetPrivateProfileString("Auto_LogOut", "Auto_LogOut_Time", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Auto_LogOut_Time = Convert.ToDouble(temp.ToString());

            //    //  얼라인 작업 시작 시 & 패킹 작업 완료시 Leak Check 기능 사용 여부
            //    NativeMethods.GetPrivateProfileString("Packing_LeakCheck", "AlignPacking_LeakCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  패킹 작업 완료시 Leak Check 설정 시간 (sec)
            //    NativeMethods.GetPrivateProfileString("Packing_LeakCheck", "Packing_LeakCheck_Time", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_OP_LeakCheck_Time = Convert.ToDouble(temp.ToString());

            //    //  패킹 작업 완료 후, 제품 언로딩을 위해 엘리베이터를 내리는 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("ElevatorZ", "DownDistance_AfterPacking", "20", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_Down_Distance_After_Packing = Convert.ToDouble(temp.ToString());

            //    //  전면 안전센서 사용 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "Area_Sensor_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.AreaSensor_Usage = temp.ToString() == "False" ? false : true;

            //    //  전면 안전센서 감지 시 Servo Off 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "AreaSensor_ServoOff_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.AreaSensor_ServoOff_Usage = temp.ToString() == "False" ? false : true;

            //    ////  전면 안전센서 해제 후 다시 동작시키기 위해 대기하는 시간 (sec)
            //    //NativeMethods.GetPrivateProfileString("Interlock", "Area_Sensor_Release_Pause_Time", "3", temp, 255, strFIle);
            //    //Config.ParamConfig.AreaSensor_Off_Pause_Time = Convert.ToDouble(temp.ToString());

            //    //  수동패킹 모드 사용 권한 설정
            //    NativeMethods.GetPrivateProfileString("Manual_Packing", "Enable_Admin_Only", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ManualPacking_Only_Admin = temp.ToString() == "False" ? false : true;



            //    //  사용 옵션과 안정화 시간

            //    //  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Pak_AirLineCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Pak_AirLineCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Pak_AirLineCheck_Time", "2000", temp, 255, strFIle);
            //    Config.ParamConfig.Pak_AirLineCheck_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Packing 공압 신호 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_Time", "5000", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_AfterTime", "5000", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_AfterTime = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Wafer 공압 체크 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Time", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_VacuumSignal_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Thin-Chuck 감지 센서 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_DetectSignal_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_DetectSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Thin-Chuck 공압 체크 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Time", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_VacuumSignal_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Align_Cam_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Align_Cam_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_AutoStart_After_Wafer_Align_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함) 
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Align_ErrorCheck_After_Wafer_Align_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - 실내 조명을 상시 On 상태로 할 것인지 여부. (false : Align 시 Off 되고, 자재를 Loading 할 때 On)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Indoor_Light_AlwaysOn_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Indoor_Light_AlwaysOn_Usage = temp.ToString() == "False" ? false : true;



            //    //  안정화 시간 - Packing 공압 신호 On 후 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On", "4000", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 공압 신호 On 후 Thin-Chuck 공압을 해제하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 공압 신호 On 후 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off", "0", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_WaferVacuumSignal_Off", "300", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_WaferVacuumSignal_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_before_PackingSignal_On", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_before_PackingSignal_On = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_ThinChuckVacuumSignal_Off", "300", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_ThinChuckVacuumSignal_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "WaferAlign_Move_StableTime", "500", temp, 255, strFIle);
            //    Config.ParamConfig.WaferAlign_Move_StableTime = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Unpacking 작업 중, Unpacking 공압 신호 On 후 Elev. Z 축을 내리기 시작할 때까지 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_UnpackingSignal_On", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_UnpackingSignal_On = Convert.ToInt16(temp.ToString());



            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_PackingOffset_Distance", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnpackingOffset_Distance", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnpackingOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingOffset_Distance", "5", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnPackingOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingStartOffset_Distance", "-1.5", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 이동 방법 (2 Step 이동 or 이동 후 대기)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_OffsetMove_Concept", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_ManualPacking_1st_Step_ElevZ_OffsetMove_Concept = temp.ToString() == "False" ? false : true;

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 안전 분리 동작 시, 씬-척 낙하 방지를 위해 엘리베이터 Z 축을 올리는 위치. (Packing 위치 대비 Offset 거리 (mm, > 0)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance = Convert.ToDouble(temp.ToString());



            //    //  카메라 설정 - 시리얼 넘버 사용 여부 (False : Config 에 세팅된 시리얼 넘버 사용)
            //    NativeMethods.GetPrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Type", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Camera_SerialNumber_Type = temp.ToString() == "False" ? false : true;

            //    //  카메라 설정 - PAK 카메라 시리얼 넘버 (Upper Camera)
            //    NativeMethods.GetPrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_PAK", "", temp, 255, strFIle);
            //    Config.ParamConfig.Camera_SerialNumber_PAK = temp.ToString();

            //    //  카메라 설정 - Wafer 카메라 시리얼 넘버 (Lower Camera)
            //    NativeMethods.GetPrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Wafer", "", temp, 255, strFIle);
            //    Config.ParamConfig.Camera_SerialNumber_Wafer = temp.ToString();



            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion X)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_X_Pos", "164", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_X_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Y)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Pos", "87", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Y_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Z_Pos", "2.73", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Z_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - PAK 카메라)      ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_PAK_Pos", "0.0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Elev_Z_PAK_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - Wafer 카메라)    ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_Wafer_Pos", "0.0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Elev_Z_Wafer_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset X        ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_X", "0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset Y        ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_Y", "0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = Convert.ToInt16(temp.ToString());



            //    //  레티클 글래스 자동 보정 - 사용 여부 (False : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Usage = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_UpperVision_Allowable_XY", "0.005", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_UpperVision_Allowable_XY = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_LowerVision_Allowable_XY", "0.008", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_LowerVision_Allowable_XY = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 상부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Upper_AlignMarkCount_forAverage", "1", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Upper_AlignMarkCount_forAverage = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 하부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Lower_AlignMarkCount_forAverage", "1", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Lower_AlignMarkCount_forAverage = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 마크 위치 평균값 신뢰 공차 (mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Vision_AverageCheck_Range", "0.05", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Vision_AverageCheck_Range = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 얼라인 재시도 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Align_Retries", "10", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Align_Retries = Convert.ToInt16(temp.ToString());



            //    //  PAK 얼라인 - 조명값 조정 크기 1단계
            //    NativeMethods.GetPrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step1", "30", temp, 255, strFIle);
            //    Config.ParamConfig.PAKAlign_LightValue_Step1 = Convert.ToInt16(temp.ToString());

            //    //  PAK 얼라인 - 조명값 조정 크기 2단계
            //    NativeMethods.GetPrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step2", "10", temp, 255, strFIle);
            //    Config.ParamConfig.PAKAlign_LightValue_Step2 = Convert.ToInt16(temp.ToString());



            //    //  인터락 - Z-Slip 기능 - 엘리베이터 Z 축의 Torque 값이 설정치를 초과할 경우 긴급정지 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "ElevZ_ESTOP_by_Torque_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ElevZ_ESTOP_by_Torque_Usage = temp.ToString() == "False" ? false : true;

            //    //  인터락 - Z-Slip 기능 - 엘리베이터 Z 축을 긴급정지시키기 위한 Torque 기준값
            //    NativeMethods.GetPrivateProfileString("Interlock", "ElevZ_ESTOP_Torque_Value", "300", temp, 255, strFIle);
            //    Config.ParamConfig.ElevZ_ESTOP_Torque_Value = Convert.ToDouble(temp.ToString());



            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 오차 비교 여부. (false : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 허용 오차 범위 (mm)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorRange_After_Wafer_Align", "0.05", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Gate_PosMarginErrorRange_After_Wafer_Align = Convert.ToDouble(temp.ToString());



            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사 여부. (false : 검사 안함)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리. (mm, 기준 : Packing 위치)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos", "20", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos = Convert.ToDouble(temp.ToString());

            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 이동 후 대기시간. (ms)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_StableTime", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_StableTime = Convert.ToInt16(temp.ToString());



            //    //  Dummy Wafer Packing 검증 - 메세지 팝업 사용 여부. (False : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_Message_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_Message_Usage = temp.ToString() == "False" ? false : true;

            //    //  Dummy Wafer Packing 검증 - 메세지 팝업 주기 모드 선택. (True : 레시피 변경 시, False : 패킹 작업 누적 회수 도달 시)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Mode", "False", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_MessagePopup_Mode = temp.ToString() == "False" ? false : true;

            //    //  Dummy Wafer Packing 검증 - 패킹 작업 누적 회수. (메세지 팝업 주기를 \"False\" 로 할 경우, 이 회수만큼 패킹을 진행하면 검증 요청 메세지창 팝업)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Count", "100", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_MessagePopup_Count = Convert.ToInt16(temp.ToString());



            //    //  패킹 방법 - 패킹 모드 선택 (True : 패킹 공압 On 후 Elev. Z 를 단계적으로 올려서 패킹, False : 패킹 높이까지 씬-척을 올려서 패킹)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_Mode", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepUp_Mode = temp.ToString() == "False" ? false : true;

            //    //  패킹 방법 - 패킹 모드 True 선택 시, 패킹 시작 오프셋. (mm, 패킹 높이에서 이 값만큼 떨어진 위치까지 Elev. Z 를 올린 후 시작)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StartOffset", "5", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StartOffset = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상숭 시키는 단위 거리. (mm)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_MoveOffset", "0.1", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_MoveOffset = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 Step 만큼 올린 후 패킹 공압을 체크하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPressure_CheckTime", "500", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingPressure_CheckTime = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축에 설정된 패킹 높이보다 추가로 더 올리는 거리. (mm)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPos_AddOffset", "0.1", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingPos_AddOffset = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 방법 선택. (True : 저속으로 연속 이동, False : 상승 단위 거리만큼 이동)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous = temp.ToString() == "False" ? false : true;

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 저속으로 상승 시키면서 패킹 공압을 확인하는 모드일 경우 저속 이동 속도 (mm/s)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed", "0.5", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키면서 패킹 공압을 체크할 때 별도의 공압센서를 사용할 것인지 여부 선택. (True : 사용, False : 사용하지 않음)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage = temp.ToString() == "False" ? false : true;



            //    /// Position 로드
            //    /// 
            //    int m_nIndex_Ready = -1;
            //    int m_nIndex_Load = -1;
            //    int m_nIndex_UnLoad = -1;
            //    int m_nIndex_Reticle_UpperCam = -1;
            //    int m_nIndex_Reticle_LowerCam = -1;

            //    int m_nIndex_AlignPos_Top = -1;
            //    int m_nIndex_AlignPos_Mid = -1;
            //    int m_nIndex_AlignPos_Bottom = -1;
            //    int m_nIndex_AlignPos_Left = -1;
            //    int m_nIndex_AlignPos_Center = -1;
            //    int m_nIndex_AlignPos_Right = -1;

            //    int m_nIndex_PackingPos_ElevZ = -1;


            //    for (int i = 0; i < Config.Positions.Count; i++)
            //    {
            //        //  Ready
            //        if (Config.Positions[i].Name == "Ready")
            //        {
            //            m_nIndex_Ready = i;
            //        }

            //        //  Load
            //        if (Config.Positions[i].Name == "Load")
            //        {
            //            m_nIndex_Load = i;
            //        }

            //        //  UnLoad
            //        if (Config.Positions[i].Name == "UnLoad")
            //        {
            //            m_nIndex_UnLoad = i;
            //        }

            //        //  Reticle Glass 를 보는 Upper Camera 위치 Index
            //        if (Config.Positions[i].Name == "ReticleGlass_UpperCamera")
            //        {
            //            m_nIndex_Reticle_UpperCam = i;
            //        }

            //        //  Reticle Glass 를 보는 Lower Camera 위치 Index
            //        if (Config.Positions[i].Name == "ReticleGlass_LowerCamera")
            //        {
            //            m_nIndex_Reticle_LowerCam = i;
            //        }

            //        //  Align Position (Top)
            //        if (Config.Positions[i].Name == "AlignPosition_Ver_Top")
            //        {
            //            m_nIndex_AlignPos_Top = i;
            //        }

            //        //  Align Position (Mid)
            //        if (Config.Positions[i].Name == "AlignPosition_Ver_Middle")
            //        {
            //            m_nIndex_AlignPos_Mid = i;
            //        }

            //        //  Align Position (Bottom)
            //        if (Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
            //        {
            //            m_nIndex_AlignPos_Bottom = i;
            //        }

            //        //  Align Position (Left)
            //        if (Config.Positions[i].Name == "AlignPosition_Hor_Left")
            //        {
            //            m_nIndex_AlignPos_Left = i;
            //        }

            //        //  Align Position (Center)
            //        if (Config.Positions[i].Name == "AlignPosition_Hor_Center")
            //        {
            //            m_nIndex_AlignPos_Center = i;
            //        }

            //        //  Align Position (Right)
            //        if (Config.Positions[i].Name == "AlignPosition_Hor_Right")
            //        {
            //            m_nIndex_AlignPos_Right = i;
            //        }

            //        //  Wafer Packing (ElevZ)
            //        if (Config.Positions[i].Name == "ProbeWafer_Packing")
            //        {
            //            m_nIndex_PackingPos_ElevZ = i;
            //        }

            //        if ((m_nIndex_Ready != -1) && (m_nIndex_Load != -1) && (m_nIndex_UnLoad != -1) && (m_nIndex_Reticle_UpperCam != -1) && (m_nIndex_Reticle_LowerCam != -1) &&
            //            (m_nIndex_AlignPos_Top != -1) && (m_nIndex_AlignPos_Mid != -1) && (m_nIndex_AlignPos_Bottom != -1) &&
            //            (m_nIndex_AlignPos_Left != -1) && (m_nIndex_AlignPos_Center != -1) && (m_nIndex_AlignPos_Right != -1) &&
            //            (m_nIndex_PackingPos_ElevZ != -1))
            //        {
            //            break;
            //        }
            //    }

            //    //  Ready 좌표 로드
            //    if (m_nIndex_Ready != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].EZ = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].X = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].Y = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].VZ = Convert.ToDouble(temp.ToString());
            //    }

            //    //  Load 좌표 로드
            //    if (m_nIndex_Load != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].EZ = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].X = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].Y = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].VZ = Convert.ToDouble(temp.ToString());
            //    }

            //    //  UnLoad 좌표 로드
            //    if (m_nIndex_UnLoad != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].EZ = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].X = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].Y = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].VZ = Convert.ToDouble(temp.ToString());
            //    }

            //    //  Upper Cam 좌표 로드
            //    if (m_nIndex_Reticle_UpperCam != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].EZ = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].X = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].Y = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Convert.ToDouble(temp.ToString());

            //        Config.Positions[m_nIndex_Reticle_UpperCam].X = Config.ParamConfig.ReticleGlass_Vision_X_Pos;
            //        Config.Positions[m_nIndex_Reticle_UpperCam].Y = Config.ParamConfig.ReticleGlass_Vision_Y_Pos;
            //        Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Config.ParamConfig.ReticleGlass_Vision_Z_Pos;
            //    }

            //    //  Lower Cam 좌표 로드
            //    if (m_nIndex_Reticle_LowerCam != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].EZ = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].X = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].Y = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Convert.ToDouble(temp.ToString());

            //        Config.Positions[m_nIndex_Reticle_LowerCam].X = Config.ParamConfig.ReticleGlass_Vision_X_Pos;
            //        Config.Positions[m_nIndex_Reticle_LowerCam].Y = Config.ParamConfig.ReticleGlass_Vision_Y_Pos;
            //        Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Config.ParamConfig.ReticleGlass_Vision_Z_Pos;
            //    }


            //    //  Top 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Top != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Top].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Top].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Top].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Top].VZ;
            //    }

            //    //  Mid 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Mid != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Mid].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Mid].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Mid].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Mid].VZ;
            //    }

            //    //  Bottom 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Bottom != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Bottom].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Bottom].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Bottom].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Bottom].VZ;
            //    }

            //    //  Left 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Left != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Left].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Left].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Left].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Left].VZ;
            //    }

            //    //  Center 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Center != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Center].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Center].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Center].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Center].VZ;
            //    }

            //    //  Right 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Right != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Right].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Right].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Right].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Right].VZ;
            //    }

            //    //  Packing 시 ElevZ 좌표 로드
            //    if (m_nIndex_PackingPos_ElevZ != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Packing", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_PackingPos_ElevZ].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_PackingPos_ElevZ].EZ;
            //    }


            //    //stCameraParam stCamera = new stCameraParam();
            //    stCamera = Machine_Parameter_Camera_Setting_Load();

            //    Equipment.PAKCamera_SerialNumber = stCamera.Upper_SerialNumber;
            //    Equipment.PAKCamera_Width = stCamera.Upper_Width;
            //    Equipment.PAKCamera_Height = stCamera.Upper_Height;
            //    Equipment.WaferCamera_SerialNumber = stCamera.Lower_SerialNumber;
            //    Equipment.WaferCamera_Width = stCamera.Lower_Width;
            //    Equipment.WaferCamera_Height = stCamera.Lower_Height;

            //    //Camera_Upper.MyConfig.SerialNumber = stCamera.Upper_SerialNumber;
            //    //Camera_Upper.MyConfig.Resolution = new System.Drawing.Size(stCamera.Upper_Width, stCamera.Upper_Height);
            //    //Camera_Upper.MyConfig.CameraResolution = new System.Drawing.Size(stCamera.Upper_Width, stCamera.Upper_Height);

            //    //Camera_Lower.MyConfig.SerialNumber = stCamera.Lower_SerialNumber;
            //    //Camera_Lower.MyConfig.Resolution = new System.Drawing.Size(stCamera.Lower_Width, stCamera.Lower_Height);
            //    //Camera_Lower.MyConfig.CameraResolution = new System.Drawing.Size(stCamera.Lower_Width, stCamera.Lower_Height);


            //    //  Wafer 이미지 Offset 값이 0 이면? --> 최대 해상도에서 현재 해상도 차이의 1/2 로 설정
            //    if (Camera_Lower.Resolution.Width == MAX_IMAGE_WIDTH)
            //    {
            //        Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = 0;
            //    }
            //    //else if (Config.ParamConfig.ReticleGlass_WaferVision_Offset_X == 0)
            //    //{
            //    //    m_nDiffX = MAX_IMAGE_WIDTH - Camera_Lower.Resolution.Width;

            //    //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = m_nDiffX > 0 ? m_nDiffX / 2 : 0;
            //    //}

            //    if (Camera_Lower.Resolution.Height == MAX_IMAGE_HEIGHT)
            //    {
            //        Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = 0;
            //    }
            //    //else if (Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y == 0)
            //    //{
            //    //    m_nDiffY = MAX_IMAGE_HEIGHT - Camera_Lower.Resolution.Height;

            //    //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = m_nDiffY > 0 ? m_nDiffY / 2 : 0;
            //    //}

            //    //public int MAX_IMAGE_WIDTH = 2248;            //  현장에서 조정된 Size (Center Offset X : 100, Offset Y : 84)
            //    //public int MAX_IMAGE_HEIGHT = 1880;
            //    //if ((Camera_Lower.Resolution.Width == MAX_IMAGE_WIDTH) || (Camera_Lower.Resolution.Height == MAX_IMAGE_HEIGHT))
            //    //{
            //    //    Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "레시피 변경, Wafer 카메라 해상도 최대");
            //    //    MessageBox.Show("Wafer 카메라 해상도가 최대입니다.\r\n\r\n[레티클 얼라인을 위해서는 Wafer 카메라의 이미지 해상도를 변경해야 합니다.]\r\n[Width : 2248,\tHeight : 1880]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //}
            //    //else if ((Config.ParamConfig.ReticleGlass_WaferVision_Offset_X == 0) || (Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y == 0))
            //    //{
            //    //    Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "레시피 변경, Wafer 카메라 이미지 Offset 값 0");
            //    //    MessageBox.Show("Wafer 카메라의 이미지 Offset 값은 0 이 될 수 없습니다.\r\n\r\n[default X : 100,\tY : 84]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //}


            //    ////  카메라 초기화 시 이미지 Offset 설정
            //    //Camera_Lower.MyConfig.OffsetX = (uint)Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
            //    //Camera_Lower.MyConfig.OffsetY = (uint)Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;



            //    string m_strRecipe = "";
            //    RecipeInfo m_recipeInfo = new RecipeInfo();
            //    m_recipeInfo = Equipment.GetCurrentRecipe();

            //    if (m_recipeInfo != null)
            //    {
            //        m_strRecipe = m_recipeInfo.Name;

            //        //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
            //        //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            //        Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            //    }
            //    else
            //    {
            //        //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
            //        Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            //    }

            //    DataManager.Instance.ApplyConfigData(this);

            //    //  Config 창 데이터 갱신을 위해서
            //    Equipment.m_bRedraw_FormWorkStageParameterConfig = true;
            //}

            return m_bRet;
        }

        public stCameraParam Machine_Parameter_Camera_Setting_Load()
        {
            stCameraParam stCameraSet = new stCameraParam();

            string m_strUpper_SerialNumber = "";
            int m_nUpper_Width = 0;
            int m_nUpper_Height = 0;

            string m_strLower_SerialNumber = "";
            int m_nLower_Width = 0;
            int m_nLower_Height = 0;

            string m_strTemp = "";
            string strFile = "";
            StringBuilder temp = new StringBuilder(255);

            strFile = ConfigManager.GetConfigPath() + "\\Camera Setting (Do not delete or modify).ini";

            //if (File.Exists(strFile))
            //{
            //    //  PAK 카메라 시리얼 넘버
            //    NativeMethods.GetPrivateProfileString("Upper_Camera", "SerialNumber", "00G97588297", temp, 255, strFile);
            //    stCameraSet.Upper_SerialNumber = temp.ToString();

            //    //  PAK 카메라 Width
            //    NativeMethods.GetPrivateProfileString("Upper_Camera", "Width", "2448", temp, 255, strFile);
            //    stCameraSet.Upper_Width = Convert.ToInt16(temp.ToString());

            //    //  PAK 카메라 Height
            //    NativeMethods.GetPrivateProfileString("Upper_Camera", "Height", "2048", temp, 255, strFile);
            //    stCameraSet.Upper_Height = Convert.ToInt16(temp.ToString());


            //    //  Wafer 카메라 시리얼 넘버
            //    NativeMethods.GetPrivateProfileString("Lower_Camera", "SerialNumber", "00G97588297", temp, 255, strFile);
            //    stCameraSet.Lower_SerialNumber = temp.ToString();

            //    //  Wafer 카메라 Width
            //    NativeMethods.GetPrivateProfileString("Lower_Camera", "Width", "2248", temp, 255, strFile);
            //    stCameraSet.Lower_Width = Convert.ToInt16(temp.ToString());

            //    //  Wafer 카메라 Height
            //    NativeMethods.GetPrivateProfileString("Lower_Camera", "Height", "1880", temp, 255, strFile);
            //    stCameraSet.Lower_Height = Convert.ToInt16(temp.ToString());

            //    if (Camera_Upper != null)
            //    {
            //        Camera_Upper.Resolution = new System.Drawing.Size(stCameraSet.Upper_Width, stCameraSet.Upper_Height); 
            //        Camera_Upper.CameraResolution = new System.Drawing.Size(stCameraSet.Upper_Width, stCameraSet.Upper_Height);
            //    }

            //    if (Camera_Lower != null)
            //    {
            //        Camera_Lower.Resolution = new System.Drawing.Size(stCameraSet.Lower_Width, stCameraSet.Lower_Height);
            //        Camera_Lower.CameraResolution = new System.Drawing.Size(stCameraSet.Lower_Width, stCameraSet.Lower_Height);
            //    }

            //    //  Config 화면에 데이터가 갱신되도록 하기 위해서. (각 Form 의 Timer 가 1초에 한번씩 이 변수를 체크해서 갱신해준다.) 크게 부하 받지는 않으니.... 꼼수..
            //    Equipment.m_bRedraw_FormWorkStageParameterConfig = true;
            //    Equipment.m_bRedraw_FormUpperCameraConfig = true;
            //    Equipment.m_bRedraw_FormLowerCameraConfig = true;

            //    //  카메라 해상도가 4로 나눠지는지 확인
            //    if ((stCameraSet.Upper_Width <= 0) || (stCameraSet.Upper_Height <= 0) || (stCameraSet.Lower_Width <= 0) || (stCameraSet.Lower_Height <= 0))
            //    {
            //        MessageBox.Show("웨이퍼 카메라 해상도는 0 이 되면 안됩니다.", "카메라 세팅", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //    {
            //        if ((stCameraSet.Upper_Width % 4 != 0) || (stCameraSet.Upper_Height % 4 != 0) || (stCameraSet.Lower_Width % 4 != 0) || (stCameraSet.Lower_Height % 4 != 0))
            //        {
            //            m_strTemp = string.Format("카메라 해상도는 4의 배수만 사용 가능합니다.\r\n\r\n[{0}  파일\r\n\r\n==> Width, Height 값 확인]", strFile);
            //            //MessageBox.Show("카메라 해상도는 4의 배수만 사용 가능합니다.", "카메라 세팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            MessageBox.Show(m_strTemp, "카메라 세팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //}
            //else
            //{
            //    if (Camera_Upper != null)
            //    {
            //        Camera_Upper.Resolution = new System.Drawing.Size(MAX_IMAGE_WIDTH, MAX_IMAGE_HEIGHT);
            //        Camera_Upper.CameraResolution = new System.Drawing.Size(MAX_IMAGE_WIDTH, MAX_IMAGE_HEIGHT);
            //    }

            //    if (Camera_Lower != null)
            //    {
            //        Camera_Lower.Resolution = new System.Drawing.Size(MAX_IMAGE_WIDTH, MAX_IMAGE_HEIGHT);
            //        Camera_Lower.CameraResolution = new System.Drawing.Size(MAX_IMAGE_WIDTH, MAX_IMAGE_HEIGHT);
            //    }
            //    //Camera_Lower.Resolution = new System.Drawing.Size(2448, 2048);
            //    //Camera_Lower.CameraResolution = new System.Drawing.Size(2448, 2048);

            //    m_strTemp = string.Format("{0} 파일이 없습니다.", strFile);
            //    MessageBox.Show(m_strTemp, "카메라 세팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            return stCameraSet;
        }

        public void Machine_Parameter_Save()
        {
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            //if (File.Exists(strFIle) == false)
            //{
            //    //File.Create(strFIle);
            //    return;
            //}


            ///// Config 저장
            ///// 

            ////  구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치
            //NativeMethods.WritePrivateProfileString("Drive_Limit", "Elev_Z", Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign.ToString(), strFIle);

            ////  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
            //NativeMethods.WritePrivateProfileString("Drive_Limit", "Vision_Y", Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_U_Minus", Config.ParamConfig.AlignLimit_UVW_U_Minus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_U_Plus", Config.ParamConfig.AlignLimit_UVW_U_Plus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_V_Minus", Config.ParamConfig.AlignLimit_UVW_V_Minus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_V_Plus", Config.ParamConfig.AlignLimit_UVW_V_Plus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_W_Minus", Config.ParamConfig.AlignLimit_UVW_W_Minus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_W_Plus", Config.ParamConfig.AlignLimit_UVW_W_Plus.ToString(), strFIle);

            ////  비전 스케일 - Manual Scale Usage
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Manual_Scale_Use", Config.ParamConfig.ManualScale_Usage.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale X (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_X", Config.ParamConfig.LowerVision_Scale_X.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale Y (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_Y", Config.ParamConfig.LowerVision_Scale_Y.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale Invert X
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", Config.ParamConfig.LowerVision_ScaleInvert_X.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale Invert Y
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", Config.ParamConfig.LowerVision_ScaleInvert_Y.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale X (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_X", Config.ParamConfig.UpperVision_Scale_X.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale Y (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_Y", Config.ParamConfig.UpperVision_Scale_Y.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale Invert X
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", Config.ParamConfig.UpperVision_ScaleInvert_X.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale Invert Y
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", Config.ParamConfig.UpperVision_ScaleInvert_Y.ToString(), strFIle);

            ////  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", Config.ParamConfig.ReticleAlign_UpperVision_LightValue.ToString(), strFIle);

            ////  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", Config.ParamConfig.ReticleAlign_LowerVision_LightValue.ToString(), strFIle);

            ////  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Speed", Config.ParamConfig.Align_Theta_Velocity.ToString(), strFIle);

            ////  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Accel", Config.ParamConfig.Align_Theta_Accel.ToString(), strFIle);

            ////  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Decel", Config.ParamConfig.Align_Theta_Decel.ToString(), strFIle);

            ////  얼라인 - 얼라인 재시도 회수
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Align_Retry", Config.ParamConfig.Align_Retries.ToString(), strFIle);

            ////  얼라인 - 얼라인 각도 Invert
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Align_Angle_Invert", Config.ParamConfig.Align_AngleInvert.ToString(), strFIle);

            ////  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", Config.ParamConfig.Align_ThetaCalcFunction_Atan.ToString(), strFIle);

            ////  얼라인 스테이지 - Theta 회전 반경 (mm)
            //NativeMethods.WritePrivateProfileString("Align_Stage", "Turning_Radius", Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance.ToString(), strFIle);

            ////  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
            //NativeMethods.WritePrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg.ToString(), strFIle);

            ////  얼라인 이미지 저장 여부
            //NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_Use", Config.ParamConfig.AlignImageSave_Usage.ToString(), strFIle);

            ////  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
            //NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", Config.ParamConfig.AlignImageSaveFolder_WarningSpace.ToString(), strFIle);

            ////  프로브 카드 클램프 타입 1 일 경우, 업다운 실린더 동작 대기 시간
            //NativeMethods.WritePrivateProfileString("Machine_Type", "ProbeCard_ClampTypeB_CylUpDown_StableTime", Config.ParamConfig.ProbeCard_ClampTypeB_CylUpDown_StableTime.ToString(), strFIle);

            ////  레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_CenterCheck_forAlign", Config.ParamConfig.ReticleGlass_CenterCheck_forAlign.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스를 확인 시 비전 카메라와 엘리베이터의 충돌 방지를 위한 Vision Y 축 이동 한계 위치. (mm, 대부분 90.0 이내)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Limit", Config.ParamConfig.ReticleGlass_Vision_Y_Limit.ToString(), strFIle);

            ////  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X
            //NativeMethods.WritePrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_X", Config.ParamConfig.PAK_WaferGate_Centering_Offset_X.ToString(), strFIle);

            ////  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y
            //NativeMethods.WritePrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_Y", Config.ParamConfig.PAK_WaferGate_Centering_Offset_Y.ToString(), strFIle);

            ////  자동 로그아웃 기능 사용 여부
            //NativeMethods.WritePrivateProfileString("Auto_LogOut", "Auto_LogOut_Usage", Config.ParamConfig.Auto_LogOut_Usage.ToString(), strFIle);

            ////  자동 로그아웃 설정 시간
            //NativeMethods.WritePrivateProfileString("Auto_LogOut", "Auto_LogOut_Time", Config.ParamConfig.Auto_LogOut_Time.ToString(), strFIle);

            ////  패킹 작업 완료시 Leak Check 기능 사용 여부
            //NativeMethods.WritePrivateProfileString("Packing_LeakCheck", "AlignPacking_LeakCheck_Usage", Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage.ToString(), strFIle);

            ////  패킹 작업 완료시 Leak Check 설정 시간 (sec)
            //NativeMethods.WritePrivateProfileString("Packing_LeakCheck", "Packing_LeakCheck_Time", Config.ParamConfig.Packing_OP_LeakCheck_Time.ToString(), strFIle);

            ////  패킹 작업 완료 후, 제품 언로딩을 위해 엘리베이터를 내리는 거리 (mm)
            //NativeMethods.WritePrivateProfileString("ElevatorZ", "DownDistance_AfterPacking", Config.ParamConfig.Wafer_ProbeCard_Down_Distance_After_Packing.ToString(), strFIle);

            ////  전면 안전센서 사용 여부
            //NativeMethods.WritePrivateProfileString("Interlock", "Area_Sensor_Usage", Config.ParamConfig.AreaSensor_Usage.ToString(), strFIle);

            ////  전면 안전센서 감지 시 Servo Off 여부
            //NativeMethods.WritePrivateProfileString("Interlock", "AreaSensor_ServoOff_Usage", Config.ParamConfig.AreaSensor_ServoOff_Usage.ToString(), strFIle);

            //////  전면 안전센서 해제 후 다시 동작시키기 위해 대기하는 시간 (sec)
            ////NativeMethods.WritePrivateProfileString("Interlock", "Area_Sensor_Release_Pause_Time", Config.ParamConfig.AreaSensor_Off_Pause_Time.ToString(), strFIle);

            ////  수동패킹 모드 사용 권한 설정
            //NativeMethods.WritePrivateProfileString("Manual_Packing", "Enable_Admin_Only", Config.ParamConfig.ManualPacking_Only_Admin.ToString(), strFIle);



            ////  사용 옵션과 안정화 시간

            ////  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Pak_AirLineCheck_Usage", Config.ParamConfig.Pak_AirLineCheck_Usage.ToString(), strFIle);

            ////  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Pak_AirLineCheck_Time", Config.ParamConfig.Pak_AirLineCheck_Time.ToString(), strFIle);

            ////  사용 옵션 - Packing 공압 신호 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_VacuumSignal_Usage", Config.ParamConfig.Packing_VacuumSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_VacuumSignal_Time", Config.ParamConfig.Packing_VacuumSignal_Time.ToString(), strFIle);

            ////  사용 옵션 - Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_VacuumSignal_AfterTime", Config.ParamConfig.Packing_VacuumSignal_AfterTime.ToString(), strFIle);

            ////  사용 옵션 - Wafer 공압 체크 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Usage", Config.ParamConfig.Wafer_VacuumSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Time", Config.ParamConfig.Wafer_VacuumSignal_Time.ToString(), strFIle);

            ////  사용 옵션 - Thin-Chuck 감지 센서 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "ThinChuck_DetectSignal_Usage", Config.ParamConfig.ThinChuck_DetectSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Thin-Chuck 공압 체크 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Usage", Config.ParamConfig.ThinChuck_VacuumSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Time", Config.ParamConfig.ThinChuck_VacuumSignal_Time.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Align_Cam_Usage", Config.ParamConfig.Wafer_Align_Cam_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_AutoStart_After_Wafer_Align_Usage", Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함) 
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Align_ErrorCheck_After_Wafer_Align_Usage", Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage.ToString(), strFIle);

            ////  사용 옵션 - 실내 조명을 상시 On 상태로 할 것인지 여부. (false : Align 시 Off 되고, 자재를 Loading 할 때 On)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Indoor_Light_AlwaysOn_Usage", Config.ParamConfig.Indoor_Light_AlwaysOn_Usage.ToString(), strFIle);



            ////  안정화 시간 - Packing 공압 신호 On 후 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On", Config.ParamConfig.StableTime_after_PackingSignal_On.ToString(), strFIle);

            ////  안정화 시간 - Packing 공압 신호 On 후 Thin-Chuck 공압을 해제하기 위해 대기하는 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off", Config.ParamConfig.StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off.ToString(), strFIle);

            ////  안정화 시간 - Packing 공압 신호 On 후 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off", Config.ParamConfig.StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off.ToString(), strFIle);

            ////  안정화 시간 - Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_WaferVacuumSignal_Off", Config.ParamConfig.StableTime_after_WaferVacuumSignal_Off.ToString(), strFIle);

            ////  안정화 시간 - Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_before_PackingSignal_On", Config.ParamConfig.StableTime_before_PackingSignal_On.ToString(), strFIle);

            ////  안정화 시간 - Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_ThinChuckVacuumSignal_Off", Config.ParamConfig.StableTime_after_ThinChuckVacuumSignal_Off.ToString(), strFIle);

            ////  안정화 시간 - Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "WaferAlign_Move_StableTime", Config.ParamConfig.WaferAlign_Move_StableTime.ToString(), strFIle);

            ////  안정화 시간 - Unpacking 작업 중, Unpacking 공압 신호 On 후 Elev. Z 축을 내리기 시작할 때까지 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_UnpackingSignal_On", Config.ParamConfig.StableTime_after_UnpackingSignal_On.ToString(), strFIle);



            ////  Offset && Delay - 웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_PackingOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnpackingOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_UnpackingOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_UnPackingOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingStartOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance", Config.ParamConfig.Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 이동 방법 (2 Step 이동 or 이동 후 대기)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_OffsetMove_Concept", Config.ParamConfig.Wafer_ProbeCard_ManualPacking_1st_Step_ElevZ_OffsetMove_Concept.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 안전 분리 동작 시, 씬-척 낙하 방지를 위해 엘리베이터 Z 축을 올리는 위치. (Packing 위치 대비 Offset 거리 (mm, > 0)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance", Config.ParamConfig.Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance.ToString(), strFIle);



            ////  카메라 설정 - 시리얼 넘버 사용 여부 (False : Config 에 세팅된 시리얼 넘버 사용)
            //NativeMethods.WritePrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Type", Config.ParamConfig.Camera_SerialNumber_Type.ToString(), strFIle);

            ////  카메라 설정 - PAK 카메라 시리얼 넘버 (Upper Camera)
            //NativeMethods.WritePrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_PAK", Config.ParamConfig.Camera_SerialNumber_PAK.ToString(), strFIle);

            ////  카메라 설정 - Wafer 카메라 시리얼 넘버 (Lower Camera)
            //NativeMethods.WritePrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Wafer", Config.ParamConfig.Camera_SerialNumber_Wafer.ToString(), strFIle);



            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion X)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_X_Pos", Config.ParamConfig.ReticleGlass_Vision_X_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Y)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Pos", Config.ParamConfig.ReticleGlass_Vision_Y_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Z_Pos", Config.ParamConfig.ReticleGlass_Vision_Z_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - PAK 카메라)      ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_PAK_Pos", Config.ParamConfig.ReticleGlass_Elev_Z_PAK_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - Wafer 카메라)    ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_Wafer_Pos", Config.ParamConfig.ReticleGlass_Elev_Z_Wafer_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_X", Config.ParamConfig.ReticleGlass_WaferVision_Offset_X.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_Y", Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y.ToString(), strFIle);




            ////  레티클 글래스 자동 보정 - 사용 여부 (False : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Usage", Config.ParamConfig.ReticleAutoCal_Usage.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_UpperVision_Allowable_XY", Config.ParamConfig.ReticleAutoCal_UpperVision_Allowable_XY.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_LowerVision_Allowable_XY", Config.ParamConfig.ReticleAutoCal_LowerVision_Allowable_XY.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 상부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Upper_AlignMarkCount_forAverage", Config.ParamConfig.ReticleAutoCal_Upper_AlignMarkCount_forAverage.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 하부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Lower_AlignMarkCount_forAverage", Config.ParamConfig.ReticleAutoCal_Lower_AlignMarkCount_forAverage.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 마크 위치 평균값 신뢰 공차 (mm)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Vision_AverageCheck_Range", Config.ParamConfig.ReticleAutoCal_Vision_AverageCheck_Range.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 얼라인 재시도 회수
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Align_Retries", Config.ParamConfig.ReticleAutoCal_Align_Retries.ToString(), strFIle);



            ////  PAK 얼라인 - 조명값 조정 크기 1단계
            //NativeMethods.WritePrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step1", Config.ParamConfig.PAKAlign_LightValue_Step1.ToString(), strFIle);

            ////  PAK 얼라인 - 조명값 조정 크기 2단계
            //NativeMethods.WritePrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step2", Config.ParamConfig.PAKAlign_LightValue_Step2.ToString(), strFIle);



            ////  인터락 - Z-Slip 기능 - 엘리베이터 Z 축의 Torque 값이 설정치를 초과할 경우 긴급정지 여부
            //NativeMethods.WritePrivateProfileString("Interlock", "ElevZ_ESTOP_by_Torque_Usage", Config.ParamConfig.ElevZ_ESTOP_by_Torque_Usage.ToString(), strFIle);

            ////  인터락 - Z-Slip 기능 - 엘리베이터 Z 축을 긴급정지시키기 위한 Torque 기준값
            //NativeMethods.WritePrivateProfileString("Interlock", "ElevZ_ESTOP_Torque_Value", Config.ParamConfig.ElevZ_ESTOP_Torque_Value.ToString(), strFIle);



            ////  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 오차 비교 여부. (false : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage", Config.ParamConfig.Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 허용 오차 범위 (mm)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorRange_After_Wafer_Align", Config.ParamConfig.Wafer_Gate_PosMarginErrorRange_After_Wafer_Align.ToString(), strFIle);



            ////  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사 여부. (false : 검사 안함)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_Usage", Config.ParamConfig.ProbeCard_LatchStatusCheck_Usage.ToString(), strFIle);

            ////  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 (mm, 기준 : Packing 위치)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos", Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos.ToString(), strFIle);

            ////  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 이동 후 대기시간. (ms)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_StableTime", Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_StableTime.ToString(), strFIle);



            ////  Dummy Wafer Packing 검증 - 메세지 팝업 사용 여부. (False : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_Message_Usage", Config.ParamConfig.DummyWaferPacking_Message_Usage.ToString(), strFIle);

            ////  Dummy Wafer Packing 검증 - 메세지 팝업 주기 모드 선택. (True : 레시피 변경 시, False : 패킹 작업 누적 회수 도달 시)
            //NativeMethods.WritePrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Mode", Config.ParamConfig.DummyWaferPacking_MessagePopup_Mode.ToString(), strFIle);

            ////  Dummy Wafer Packing 검증 - 패킹 작업 누적 회수. (메세지 팝업 주기를 \"False\" 로 할 경우, 이 회수만큼 패킹을 진행하면 검증 요청 메세지창 팝업)
            //NativeMethods.WritePrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Count", Config.ParamConfig.DummyWaferPacking_MessagePopup_Count.ToString(), strFIle);



            ////  패킹 방법 - 패킹 모드 선택 (True : 패킹 공압 On 후 Elev. Z 를 단계적으로 올려서 패킹, False : 패킹 높이까지 씬-척을 올려서 패킹)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_Mode", Config.ParamConfig.PackingConcept_StepUp_Mode.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, 패킹 시작 오프셋. (mm, 패킹 높이에서 이 값만큼 떨어진 위치까지 Elev. Z 를 올린 후 시작)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StartOffset", Config.ParamConfig.PackingConcept_StepMode_StartOffset.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상숭 시키는 단위 거리. (mm)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_MoveOffset", Config.ParamConfig.PackingConcept_StepMode_MoveOffset.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 Step 만큼 올린 후 패킹 공압을 체크하기 위해 대기하는 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPressure_CheckTime", Config.ParamConfig.PackingConcept_StepMode_PackingPressure_CheckTime.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축에 설정된 패킹 높이보다 추가로 더 올리는 거리. (mm)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPos_AddOffset", Config.ParamConfig.PackingConcept_StepMode_PackingPos_AddOffset.ToString(), strFIle);

            ////  패킹 방법 - 패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 방법 선택. (True : 저속으로 연속 이동, False : 상승 단위 거리만큼 이동)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous", Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 저속으로 상승 시키면서 패킹 공압을 확인하는 모드일 경우 저속 이동 속도 (mm/s)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed", Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키면서 패킹 공압을 체크할 때 별도의 공압센서를 사용할 것인지 여부 선택. (True : 사용, False : 사용하지 않음)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage", Config.ParamConfig.PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage.ToString(), strFIle);



            ///// Position 저장
            ///// 
            //int m_nIndex_Ready = -1;
            //int m_nIndex_Load = -1;
            //int m_nIndex_UnLoad = -1;
            //int m_nIndex_Reticle_UpperCam = -1;
            //int m_nIndex_Reticle_LowerCam = -1;

            //int m_nIndex_AlignPos_Top = -1;
            //int m_nIndex_Packing_ElevZ = -1;

            //for (int i = 0; i < Config.Positions.Count; i++)
            //{
            //    //  Ready
            //    if (Config.Positions[i].Name == "Ready")
            //    {
            //        m_nIndex_Ready = i;
            //    }

            //    //  Load
            //    if (Config.Positions[i].Name == "Load")
            //    {
            //        m_nIndex_Load = i;
            //    }

            //    //  UnLoad
            //    if (Config.Positions[i].Name == "UnLoad")
            //    {
            //        m_nIndex_UnLoad = i;
            //    }

            //    //  Reticle Glass 를 보는 Upper Camera 위치 Index
            //    if (Config.Positions[i].Name == "ReticleGlass_UpperCamera")
            //    {
            //        m_nIndex_Reticle_UpperCam = i;
            //    }

            //    //  Reticle Glass 를 보는 Lower Camera 위치 Index
            //    if (Config.Positions[i].Name == "ReticleGlass_LowerCamera")
            //    {
            //        m_nIndex_Reticle_LowerCam = i;
            //    }

            //    //  Align 위치 (Top)
            //    if (Config.Positions[i].Name == "AlignPosition_Ver_Top")
            //    {
            //        m_nIndex_AlignPos_Top = i;
            //    }

            //    //  Wafer Packing 높이 (Elevator Z)
            //    if (Config.Positions[i].Name == "ProbeWafer_Packing")
            //    {
            //        m_nIndex_Packing_ElevZ = i;
            //    }


            //    if ((m_nIndex_Ready != -1) && (m_nIndex_Load != -1) && (m_nIndex_UnLoad != -1) && (m_nIndex_Reticle_UpperCam != -1) && (m_nIndex_Reticle_LowerCam != -1) &&
            //        (m_nIndex_AlignPos_Top != -1) && (m_nIndex_Packing_ElevZ != -1))
            //    {
            //        break;
            //    }
            //}

            ////  Ready 좌표 저장
            //if (m_nIndex_Ready != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_U", Config.Positions[m_nIndex_Ready].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_V", Config.Positions[m_nIndex_Ready].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_W", Config.Positions[m_nIndex_Ready].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_EZ", Config.Positions[m_nIndex_Ready].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_X", Config.Positions[m_nIndex_Ready].X.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_Y", Config.Positions[m_nIndex_Ready].Y.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_VZ", Config.Positions[m_nIndex_Ready].VZ.ToString(), strFIle);
            //}

            ////  Load 좌표 저장
            //if (m_nIndex_Load != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_U", Config.Positions[m_nIndex_Load].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_V", Config.Positions[m_nIndex_Load].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_W", Config.Positions[m_nIndex_Load].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_EZ", Config.Positions[m_nIndex_Load].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_X", Config.Positions[m_nIndex_Load].X.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_Y", Config.Positions[m_nIndex_Load].Y.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_VZ", Config.Positions[m_nIndex_Load].VZ.ToString(), strFIle);
            //}

            ////  UnLoad 좌표 저장
            //if (m_nIndex_UnLoad != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_U", Config.Positions[m_nIndex_UnLoad].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_V", Config.Positions[m_nIndex_UnLoad].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_W", Config.Positions[m_nIndex_UnLoad].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", Config.Positions[m_nIndex_UnLoad].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_X", Config.Positions[m_nIndex_UnLoad].X.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_Y", Config.Positions[m_nIndex_UnLoad].Y.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", Config.Positions[m_nIndex_UnLoad].VZ.ToString(), strFIle);
            //}

            ////  Upper Cam 좌표 저장
            //if (m_nIndex_Reticle_UpperCam != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", Config.Positions[m_nIndex_Reticle_UpperCam].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", Config.Positions[m_nIndex_Reticle_UpperCam].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", Config.Positions[m_nIndex_Reticle_UpperCam].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", Config.Positions[m_nIndex_Reticle_UpperCam].EZ.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", Config.Positions[m_nIndex_Reticle_UpperCam].X.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", Config.Positions[m_nIndex_Reticle_UpperCam].Y.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", Config.Positions[m_nIndex_Reticle_UpperCam].VZ.ToString(), strFIle);
            //}

            ////  Lower Cam 좌표 저장
            //if (m_nIndex_Reticle_LowerCam != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", Config.Positions[m_nIndex_Reticle_LowerCam].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", Config.Positions[m_nIndex_Reticle_LowerCam].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", Config.Positions[m_nIndex_Reticle_LowerCam].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", Config.Positions[m_nIndex_Reticle_LowerCam].EZ.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", Config.Positions[m_nIndex_Reticle_LowerCam].X.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", Config.Positions[m_nIndex_Reticle_LowerCam].Y.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", Config.Positions[m_nIndex_Reticle_LowerCam].VZ.ToString(), strFIle);
            //}


            ////  Top 위치의 ElevZ 좌표, VisionZ 좌표 저장
            //if (m_nIndex_AlignPos_Top != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Align", "ElevZ", Config.Positions[m_nIndex_AlignPos_Top].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Align", "VisionZ", Config.Positions[m_nIndex_AlignPos_Top].VZ.ToString(), strFIle);
            //}

            ////  Packing 시 ElevZ 좌표 저장
            //if (m_nIndex_Packing_ElevZ != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Packing", "ElevZ", Config.Positions[m_nIndex_Packing_ElevZ].EZ.ToString(), strFIle);
            //}
        }


        //  로그 자동 삭제
        public void Delete_Backup(string folderDir)
        {
            try
            {
                int deleteDay = 3;

                DirectoryInfo di = new DirectoryInfo(folderDir);
                if (di.Exists)
                {
                    DirectoryInfo[] dirInfo = di.GetDirectories();
                    FileInfo[] fileInfo = di.GetFiles();

                    string IDate = DateTime.Today.AddDays(-deleteDay).ToString("yyyyMMdd");

                    //  폴더가 있으면 삭제
                    foreach( DirectoryInfo dir in dirInfo)
                    {
                        if (IDate.CompareTo(dir.LastWriteTime.ToString("yyyyMMdd")) > 0)
                        {
                            dir.Attributes = FileAttributes.Normal;
                            dir.Delete(true);
                        }
                    }

                    //  파일이 있으면 삭제
                    foreach( FileInfo fi in fileInfo)
                    {
                        if (IDate.CompareTo(fi.LastWriteTime.ToString("yyyyMMdd")) > 0)
                        {
                            fi.Attributes = FileAttributes.Normal;
                            fi.Delete();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public int GetDrillingData()
        {
            string m_strTemp;
            bool success = true;
            bool LayerIsGroup = false;

            //m_bGroupExist_LargerThanDivideSize = false;

            //int m_nUnusableLayerCount = 0;
            //int m_nLayerCount = 0;
            //int m_nLayerThruHole_Count = 0;
            //int m_nLayerOutLine_Count = 0;
            //int m_nLayerDrilling_Count = 0;
            //int m_nGroupData_TotalCount = 0;
            //int m_nGroupData_Count = 0;
            //double m_dGroupSize_Width = 0.0;
            //double m_dGroupSize_Height = 0.0;
            //double m_dDrilling_FOV = 0.0;
            //int m_nGroupIndex_TotalX = 0;
            //int m_nGroupIndex_TotalY = 0;
            //double m_dGroupStartPos_X = 0.0;            //  Group 시작 X 위치. (이 위치를 기준으로 Divide 영역 계산하기 위함)
            //double m_dGroupStartPos_Y = 0.0;            //  Group 시작 Y 위치. (이 위치를 기준으로 Divide 영역 계산하기 위함)
            //int m_nDivCount_X = -1;
            //int m_nDivCount_Y = -1;

            //PointD m_ptFrom = new PointD(0.0, 0.0);
            //PointD m_ptTo = new PointD(0.0, 0.0);
            //PointD m_ptLast = new PointD(0.0, 0.0);

            //int m_nOutlineData_Count = 0;
            //int m_nThruholeData_Count = 0;
            //int m_nDrillingData_Count = 0;
            //int m_nMarkingData_Count = 0;

            ////  글자 개수 카운트
            //int m_nTextCount = 0;

            ////  글자를 구성하는 요소 개수 카운트
            //int m_nTextItemCount = 0;

            //int m_nOtherData_Count = 0;                 //  Drilling Hole 이 Polyline, Line, Circle 이 아닌 경우 개수
            //int m_nPolylineData_Count = 0;              //  Drilling Hole 이 Polyline 으로 구성되어 있을 경우 개수
            //int m_nLineData_Count = 0;                  //  Drilling Hole 이 Line 으로 구성되어 있을 경우 개수
            //int m_nCircleData_Count = 0;                //  Drilling Hole 이 Circle 로 구성되어 있을 경우 개수
            //int m_nEachDrillHole_LineCount = 0;         //  Drill Hole 을 구성하는 Line 개수 카운트

            //int m_nLineData_Count_forDrillHole = 0;
            //stLine[] m_stDrillHole_byLine = new stLine[4];

            //for (int i = 0; i < 4; i++)
            //{
            //    m_stDrillHole_byLine[i].ptStart.X = 0.0;
            //    m_stDrillHole_byLine[i].ptStart.Y = 0.0;
            //    m_stDrillHole_byLine[i].ptEnd.X = 0.0;
            //    m_stDrillHole_byLine[i].ptEnd.Y = 0.0;
            //}

            //m_dTotal_OutlineJumpLength = 0.0;                                   //  전체 Outline 데이터 중 Jump 이동 길이
            //m_dTotal_OutlineDataLength = 0.0;                                   //  전체 Outline 데이터 중 가공 길이
            //m_dTotal_ThruholeJumpLength = 0.0;                                  //  전체 Thruhole 데이터 중 Jump 이동 길이
            //m_dTotal_ThruholeDataLength = 0.0;                                  //  전체 Thruhole 데이터 중 가공 길이
            //m_dTotal_DrillingJumpLength = 0.0;                                  //  전체 Drilling 데이터 중 Jump 이동 길이
            //m_dTotal_DrillingDataLength = 0.0;                                  //  전체 Drilling 데이터 중 가공 길이
            //m_dTotal_MarkingJumpLength = 0.0;                                   //  전체 Thruhole 데이터 중 Jump 이동 길이
            //m_dTotal_MarkingDataLength = 0.0;                                   //  전체 Thruhole 데이터 중 가공 길이

            //m_nThruHole_LayerNum = 0;
            //m_nThruHole_LayerCount = 0;
            //m_nOutLine_LayerNum = 0;
            //m_nOutLine_LayerCount = 0;

            //Equipment.WorkTotalTime_Outline_AdditionalTime = 0;         //  추가 시간 (Cycle Delay 에 의해 추가되는 시간. 현재 Main Cycle Interval 은 20ms 이다)
            //Equipment.WorkTotalTime_Thruhole_AdditionalTime = 0;
            //Equipment.WorkTotalTime_Drilling_AdditionalTime = 0;
            //Equipment.WorkTotalTime_Marking_AdditionalTime = 0;

            ////m_ptFrom.X = 0.0;
            ////m_ptFrom.Y = 0.0;
            ////m_ptTo.X = 0.0;
            ////m_ptTo.Y = 0.0;

            //m_nAlignMark_FoundCount = 0;                                //  Photoveel 제품 Align 을 위한 변수. 최초에 0 이고, Description 에 Align1 or Align2 가 나올 때마다 1씩 증가. 최종적으로 2가 되어야 함. (2 초과이면 마크가 더 많다는 것...)
            //for (int i = 0; i < System.Enum.GetValues(typeof(AlignParam)).Length; i++)
            //{
            //    m_forAlign_Data[i].X = 0.0;
            //    m_forAlign_Data[i].Y = 0.0;
            //}

            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK1].X = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK1].Y = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK2].X = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK2].Y = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK3].X = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK3].Y = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK4].X = 999.999;
            //m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK4].Y = 999.999;

            //double m_dLine_Min_X = double.MaxValue;
            //double m_dLine_Min_Y = double.MaxValue;
            //double m_dLine_Max_X = double.MinValue;
            //double m_dLine_Max_Y = double.MinValue;

            //if (MainSiriusViewer.Document == null)
            //{
            //    MessageBox.Show("도면 데이터를 불러올 Document 가 준비되지 않았습니다.", "Information!!");
            //    return (int)nGetDataResult.GETDATA_FAIL;
            //}

            ////  일단 Layer 는 1개만 사용하기로...

            //m_nGroupCount = 0;

            //if (Config.ParamConfig.Drilling_DivideSize <= 0.0)                //  default : 5mm
            //    m_dDrilling_FOV = 5.0;
            //else
            //    m_dDrilling_FOV = Config.ParamConfig.Drilling_DivideSize;

            ////  Layer 개수 체크
            //m_nLayerCount = 0;
            //foreach (var layer in MainSiriusViewer.Document.Layers)
            //{
            //    m_nLayerCount++;
            //}

            //if (m_nLayerCount == 0)
            //{
            //    return (int)nGetDataResult.GETDATA_FAIL;
            //}

            //m_stLayerType = new stLayerType();
            //m_stLayerType.m_nLayerCount = m_nLayerCount;
            //m_stLayerType.m_nLayerType = new int[m_stLayerType.m_nLayerCount];

            ////  Layer 종류별 Count
            //foreach (var layer in MainSiriusViewer.Document.Layers)
            //{
            //    if (layer.IsMarkerable)
            //    {
            //        if (layer.Name == "쓰루홀")
            //        {
            //            m_nLayerThruHole_Count++;
            //        }
            //        else if (layer.Name == "외곽선")
            //        {
            //            m_nLayerOutLine_Count++;
            //        }
            //        else if (layer.Name == "드릴링")
            //        {
            //            m_nLayerDrilling_Count++;
            //        }
            //    }
            //}

            ////  Outline 의 경우, Layer 가 2개 이상일 수 있다. 여기서 공간 할당.
            //if (m_nLayerOutLine_Count >= 1)
            //{
            //    m_nOutLine_LayerNum = m_nLayerOutLine_Count;
            //    m_stOutLine_LayerData = new WorkStage.stOutLine_LayerData[m_nLayerOutLine_Count];
            //    m_nLayerOutLine_Count = 0;
            //}

            ////  Thruhole 의 경우, Layer 가 2개 이상일 수 있다. 여기서 공간 할당.
            //if (m_nLayerThruHole_Count >= 1)
            //{
            //    m_nThruHole_LayerNum = m_nLayerThruHole_Count;
            //    m_stThruHole_LayerData = new WorkStage.stThruHole_LayerData[m_nLayerThruHole_Count];
            //    m_nLayerThruHole_Count = 0;
            //}

            //m_nLayerCount = 0;
            //foreach (var layer in MainSiriusViewer.Document.Layers)
            //{
            //    if (layer.IsMarkerable)
            //    {
            //        ///////////////////////
            //        ///                 ///
            //        ///     쓰루홀      ///
            //        ///                 ///
            //        ///////////////////////
            //        if (layer.Name == "쓰루홀")
            //        {
            //            if (Equipment.RtcMode_syncAxis != (int)Equipment.RtcMode.RTC_SYNCAXIS)
            //            {
            //                return (int)nGetDataResult.GETDATA_FAIL;
            //            }

            //            m_ptLast.X = 0.0;
            //            m_ptLast.Y = 0.0;

            //            m_stLayerType.m_nLayerType[m_nLayerCount] = (int)LayerType.LAYER_THRUHOLE;

            //            if (layer.MotionType == MotionType.StageAndScanner)
            //            {
            //                int a = 0;
            //            }
            //            else if (layer.MotionType == MotionType.StageOnly)
            //            {
            //                int b = 0;
            //            }
            //            else if (layer.MotionType == MotionType.ScannerOnly)
            //            {
            //                int c = 0;
            //            }

            //            //  Item 이 Group 인지 아닌지 확인 (Group 이면 아래에서 데이터 변수 할당, Group 이 아니면 여기서 할당)
            //            int m_nCount = 0;
            //            foreach (var entity in layer)
            //            {
            //                var group = entity as Group;

            //                if (group == null)
            //                {
            //                    LayerIsGroup = false;

            //                    m_nCount = layer.Count;
            //                }
            //                else
            //                {
            //                    LayerIsGroup = true;

            //                    m_nCount = 1;
            //                }

            //                break;
            //            }

            //            //if ((Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_SYNCAXIS) && (m_nCount == 1))
            //            //{
            //            //    return 2;
            //            //}


            //            if (!LayerIsGroup || (m_nCount > 1))
            //            {
            //                //m_stThruHole_LayerData = new WorkStage.stThruHole_LayerData();

            //                //  전체 Object 개수
            //                m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectTotalNum = layer.Count;

            //                //  Motion Type
            //                if (layer.MotionType == MotionType.ScannerOnly)
            //                {
            //                    m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_MotionType = (int)MotionType.ScannerOnly;

            //                    Equipment.WorkTotalTime_Thruhole_AdditionalTime = (double)(m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                }
            //                else
            //                {
            //                    m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_MotionType = (int)MotionType.StageAndScanner;

            //                    Equipment.WorkTotalTime_Thruhole_AdditionalTime = Config.ParamConfig.Thruhole_Repeat_Count * (double)Equipment.MainCycle_Interval;                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                }

            //                //  Object 별 데이터 공간 메모리 할당
            //                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData = new WorkStage.stThruHole_ObjectData[layer.Count];

            //                //  Thruhole 데이터 개수
            //                m_nThruholeData_Count = m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectTotalNum;
            //            }

            //            //  세부 데이터 저장
            //            m_nGroupData_Count = 0;
            //            foreach (var entity in layer)
            //            {
            //                switch (entity.EntityType)
            //                {
            //                    case EType.Point:
            //                        var point = entity as SpiralLab.Sirius.Point;
            //                        //point.Location 
            //                        //point.DwellTime
            //                        //success &= point.Mark(markerArg);
            //                        break;

            //                    case EType.Points:
            //                        var points = entity as Points;
            //                        foreach (var vertex in points)
            //                        {
            //                            //vertex.X
            //                            //vertex.Y
            //                        }
            //                        //points.DwellTime
            //                        //success &= points.Mark(markerArg);
            //                        break;

            //                    case EType.Line:
            //                        var line = entity as SpiralLab.Sirius.Line;

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End

            //                        //  객체 Type
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                        //  객체 Edge 좌표 개수
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                        //  객체 Center 좌표 데이터 저장
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.X = (double)line.Start.X + (double)line.End.X != 0.0 ? ((double)line.Start.X + (double)line.End.X) / 2.0 : 0.0;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.Y = (double)line.Start.Y + (double)line.End.Y != 0.0 ? ((double)line.Start.Y + (double)line.End.Y) / 2.0 : 0.0;

            //                        //  객체 Edge 좌표 데이터 저장
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X = (double)line.Start.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y = (double)line.Start.Y;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].X = (double)line.End.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].Y = (double)line.End.Y;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)line.Start.X;
            //                            m_ptFrom.Y = (double)line.Start.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)line.End.X;
            //                        m_ptLast.Y = (double)line.End.Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_ThruholeDataLength += (double)line.Length;

            //                        //  영역 객체 개수 +1
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;

            //                        //line.Start
            //                        //line.End
            //                        //success &= line.Mark(markerArg);
            //                        break;

            //                    case EType.Arc:
            //                        var arc = entity as SpiralLab.Sirius.Arc;

            //                        //  객체 Type
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                        //  객체 Radius
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dRadius = (double)arc.Radius;

            //                        //  객체 Center 좌표
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dCenter.X = (double)arc.Center.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dCenter.Y = (double)arc.Center.Y;

            //                        //  객체 Center 좌표 데이터 저장
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.X = (double)arc.Center.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.Y = (double)arc.Center.Y;

            //                        //  객체 Start Angle
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dStartAngle = (double)arc.StartAngle;

            //                        //  객체 Sweep Angle
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dSweepAngle = (double)arc.SweepAngle;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)arc.Center.X;
            //                            m_ptFrom.Y = (double)arc.Center.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)arc.Center.X;
            //                        m_ptLast.Y = (double)arc.Center.Y;

            //                        //  가공 데이터 길이 누적
            //                        if ((double)arc.SweepAngle > 0.0)
            //                            m_dTotal_ThruholeDataLength += (double)arc.Radius * 2.0 * Math.PI * ((double)arc.SweepAngle / 360.0);

            //                        //  영역 객체 개수 +1
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;

            //                        //arc.Radius
            //                        //arc.Center
            //                        //arc.StartAngle
            //                        //arc.SweepAngle
            //                        //success &= arc.Mark(markerArg);
            //                        break;

            //                    case EType.Circle:
            //                        var circle = entity as SpiralLab.Sirius.Circle;

            //                        //if ( circle.Description != null)
            //                        //{
            //                        //    MessageBox.Show(circle.Description);
            //                        //}

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                        //  객체 Type
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                        //  객체 Edge 좌표 개수
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = 1;

            //                        //  객체 Center 좌표 데이터 저장
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.X = (double)circle.Center.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.Y = (double)circle.Center.Y;

            //                        //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X = (double)circle.Center.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y = (double)circle.Center.Y;

            //                        //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].X = (double)circle.Radius;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].Y = (double)circle.Radius;

            //                        //  circle 이 Align Mark 일 경우
            //                        if (circle.Description != null)
            //                        {
            //                            if (circle.Description.ToUpper() == "ALIGN1")
            //                            {
            //                                m_nAlignMark_FoundCount++;

            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK1].X = circle.Center.X;
            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK1].Y = circle.Center.Y;

            //                                m_forAlign_Data[(int)AlignParam.POS_ROTCENTER].X = circle.Center.X;
            //                                m_forAlign_Data[(int)AlignParam.POS_ROTCENTER].Y = circle.Center.Y;

            //                                jigAligner.m_AlignPositions[0].X = circle.Center.X + Config.ParamConfig.OffsetX_fromCamera_toLaser;
            //                                jigAligner.m_AlignPositions[0].Y = -circle.Center.Y + Config.ParamConfig.OffsetY_fromCamera_toLaser;

            //                                //m_stTemp_AlignMark.dRotationCenter = m_stTemp_AlignMark.dAlignMark1;
            //                            }
            //                            else if (circle.Description.ToUpper() == "ALIGN2")
            //                            {
            //                                m_nAlignMark_FoundCount++;

            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK2].X = circle.Center.X;
            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK2].Y = circle.Center.Y;

            //                                jigAligner.m_AlignPositions[1].X = circle.Center.X + Config.ParamConfig.OffsetX_fromCamera_toLaser;
            //                                jigAligner.m_AlignPositions[1].Y = -circle.Center.Y + Config.ParamConfig.OffsetY_fromCamera_toLaser;
            //                            }
            //                            else if (circle.Description.ToUpper() == "ALIGN3")
            //                            {
            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK3].X = circle.Center.X;
            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK3].Y = circle.Center.Y;
            //                            }
            //                            else if (circle.Description.ToUpper() == "ALIGN4")
            //                            {
            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK4].X = circle.Center.X;
            //                                m_forAlign_Data[(int)AlignParam.POS_ALIGNMARK4].Y = circle.Center.Y;
            //                            }
            //                        }

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)circle.Center.X;
            //                            m_ptFrom.Y = (double)circle.Center.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)circle.Center.X;
            //                        m_ptLast.Y = (double)circle.Center.Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_ThruholeDataLength += (double)circle.Radius * 2.0 * Math.PI;

            //                        //  영역 객체 개수 +1
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;
            //                        break;

            //                    case EType.Rectangle:
            //                        var rectangle = entity as SpiralLab.Sirius.Rectangle;

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                        //  객체 Type
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                        //  객체 Edge 좌표 개수
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = 5;

            //                        //  객체 Center 좌표 데이터 저장
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.X = (double)rectangle.Center.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.Y = (double)rectangle.Center.Y;

            //                        //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[2].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[2].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[3].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[3].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[4].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[4].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                            m_ptFrom.Y = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                        m_ptLast.Y = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_ThruholeDataLength += ((double)rectangle.Width * 2.0) + ((double)rectangle.Height * 2.0);

            //                        //  영역 객체 개수 +1
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;

            //                        //rectangle.Width
            //                        //rectangle.Height
            //                        //rectangle.Align
            //                        //rectangle.Location
            //                        //success &= rectangle.Mark(markerArg);
            //                        break;

            //                    case EType.LWPolyline:
            //                        var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                        //lwPolyline.IsClosed

            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                        //  객체 Type
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                        //  객체 Edge 좌표 개수
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count;

            //                        //  객체 Center 좌표 데이터 저장
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.X = (double)lwPolyline.BoundRect.Center.X;
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dObjectCenter.Y = (double)lwPolyline.BoundRect.Center.Y;

            //                        //  객체 Edge 좌표 데이터 저장
            //                        for (int n_pl = 0; n_pl < lwPolyline.Count; n_pl++)
            //                        {
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)lwPolyline.Items[n_pl].X;
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)lwPolyline.Items[n_pl].Y;
            //                        }

            //                        //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                        if (lwPolyline.IsClosed)
            //                        {
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].X = (double)lwPolyline.Items[0].X;
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].Y = (double)lwPolyline.Items[0].Y;
            //                        }

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                            m_ptFrom.Y = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                        m_ptLast.Y = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                        //  가공 데이터 길이 누적
            //                        for (int n_pl = 0; n_pl < lwPolyline.Count - 1; n_pl++)
            //                        {
            //                            m_ptFrom.X = (double)lwPolyline.Items[n_pl].X;
            //                            m_ptFrom.Y = (double)lwPolyline.Items[n_pl].Y;
            //                            m_ptTo.X = (double)lwPolyline.Items[n_pl + 1].X;
            //                            m_ptTo.Y = (double)lwPolyline.Items[n_pl + 1].Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_ThruholeDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_ThruholeDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  영역 객체 개수 +1
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;

            //                        //foreach (var vertex in lwPolyline)
            //                        //{
            //                        //    //vertex.X
            //                        //    //vertex.Y
            //                        //    //vertex.Bulge
            //                        //}
            //                        //success &= lwPolyline.Mark(markerArg);
            //                        break;

            //                    case EType.Spiral:
            //                        var spiral = entity as Spiral;
            //                        //spiral.OutterDiameter 
            //                        //spiral.InnerDiameter
            //                        //spiral.RadialPitch
            //                        //spiral.Revolutions
            //                        //spiral.Center
            //                        //success &= spiral.Mark(markerArg);
            //                        break;

            //                    case EType.Group:
            //                    default:
            //                        var group = entity as Group;

            //                        //m_stThruHole_LayerData = new WorkStage.stThruHole_LayerData();

            //                        //  전체 Object 개수
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectTotalNum = group.Count;

            //                        //  Motion Type
            //                        if (layer.MotionType == MotionType.ScannerOnly)
            //                        {
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_MotionType = (int)MotionType.ScannerOnly;
            //                        }
            //                        else
            //                        {
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_MotionType = (int)MotionType.StageAndScanner;
            //                        }

            //                        //  Object 별 데이터 공간 메모리 할당
            //                        m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData = new WorkStage.stThruHole_ObjectData[group.Count];

            //                        //  세부 데이터 저장
            //                        m_nGroupData_Count = 0;
            //                        foreach (var subEntity in group)
            //                        {
            //                            m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_nGroupData_Count].bAssigned = false;

            //                            Type t = subEntity.GetType();
            //                            if (t.Name == "LwPolyline")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                //  객체 Type
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                //  객체 Edge 좌표 개수
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                //  객체 Edge 좌표 데이터 저장
            //                                for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                {
            //                                    m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;
            //                                    m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                }

            //                                //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                if (pl.IsClosed)
            //                                {
            //                                    m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;
            //                                    m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                }

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                {
            //                                    m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                    m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                    m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                    m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_ThruholeDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  영역 객체 개수 +1
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Circle")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                //  객체 Type
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                //  객체 Edge 좌표 개수
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Center.X;
            //                                    m_ptFrom.Y = (double)pl.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.Center.X;
            //                                m_ptLast.Y = (double)pl.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_ThruholeDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                //  영역 객체 개수 +1
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Rectangle")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                //  객체 Type
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                //  객체 Edge 좌표 개수
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_ThruholeDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                //  영역 객체 개수 +1
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Line")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Line;

            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : Endm_stThruHole_LayerData[m_nLayerThruHole_Count]
            //                                //  객체 Type
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                                //  객체 Edge 좌표 개수
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                                //  객체 Edge 좌표 데이터 저장
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Start.X;
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Start.Y;
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.End.X;
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.End.Y;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Start.X;
            //                                    m_ptFrom.Y = (double)pl.Start.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.End.X;
            //                                m_ptLast.Y = (double)pl.End.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_ThruholeDataLength += (double)pl.Length;

            //                                //  영역 객체 개수 +1
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Arc")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Arc;

            //                                //  객체 Type
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                                //  객체 Radius
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dRadius = (double)pl.Radius;

            //                                //  객체 Center 좌표
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dCenter.X = (double)pl.Center.X;
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dCenter.Y = (double)pl.Center.Y;

            //                                //  객체 Start Angle
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dStartAngle = (double)pl.StartAngle;

            //                                //  객체 Sweep Angle
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].m_stThruHole_ObjectData[m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount].stArcData.dSweepAngle = (double)pl.SweepAngle;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Center.X;
            //                                    m_ptFrom.Y = (double)pl.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_ThruholeJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.Center.X;
            //                                m_ptLast.Y = (double)pl.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                if ((double)pl.SweepAngle > 0.0)
            //                                    m_dTotal_ThruholeDataLength += (double)pl.Radius * 2.0 * Math.PI * ((double)pl.SweepAngle / 360.0);


            //                                //  영역 객체 개수 +1
            //                                m_stThruHole_LayerData[m_nLayerThruHole_Count].nRegion_ObjectCount++;
            //                            }
            //                            else        //  또 뭐가 있나...
            //                            {

            //                            }
            //                        }

            //                        //success &= group.Mark(markerArg);
            //                        break;
            //                        // case EType....
            //                        // ...

            //                        //default:
            //                        //    if (entity is IMarkerable markerable)
            //                        //    {
            //                        //        // mark entity
            //                        //        // 해당 개체(Entity) 가공 
            //                        //        //success &= markerable.Mark(markerArg);
            //                        //    }
            //                        //    break;
            //                }
            //                if (!success)
            //                    break;
            //            }

            //            m_nLayerCount++;
            //            m_nLayerThruHole_Count++;                   //  Thruhole Layer 카운트 +1
            //        }

            //        ///////////////////////
            //        ///                 ///
            //        ///     외곽선      ///
            //        ///                 ///
            //        ///////////////////////
            //        else if (layer.Name == "외곽선")
            //        {
            //            if (Equipment.RtcMode_syncAxis != (int)Equipment.RtcMode.RTC_SYNCAXIS)
            //            {
            //                return (int)nGetDataResult.GETDATA_FAIL;
            //            }

            //            m_ptLast.X = 0.0;
            //            m_ptLast.Y = 0.0;

            //            m_stLayerType.m_nLayerType[m_nLayerCount] = (int)LayerType.LAYER_OUTLINE;

            //            if (layer.MotionType == MotionType.StageAndScanner)
            //            {
            //                int a = 0;
            //            }
            //            else if (layer.MotionType == MotionType.StageOnly)
            //            {
            //                int b = 0;
            //            }
            //            else if (layer.MotionType == MotionType.ScannerOnly)
            //            {
            //                int c = 0;
            //            }

            //            //  Item 이 Group 인지 아닌지 확인 (Group 이면 아래에서 데이터 변수 할당, Group 이 아니면 여기서 할당)
            //            int m_nCount = 0;
            //            foreach (var entity in layer)
            //            {
            //                var group = entity as Group;

            //                if (group == null)
            //                {
            //                    LayerIsGroup = false;

            //                    m_nCount = layer.Count;
            //                }
            //                else
            //                {
            //                    LayerIsGroup = true;

            //                    m_nCount = 1;
            //                }

            //                break;
            //            }

            //            if (!LayerIsGroup || (m_nCount > 1))
            //            {
            //                //m_stOutLine_LayerData = new WorkStage.stOutLine_LayerData();

            //                //  전체 Object 개수
            //                m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectTotalNum = layer.Count;

            //                //  Object 별 데이터 공간 메모리 할당
            //                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData = new WorkStage.stOutLine_ObjectData[layer.Count];

            //                //  Outline 데이터 개수
            //                m_nOutlineData_Count = m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectTotalNum;
            //            }

            //            //if ((Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_SYNCAXIS) && (m_nCount == 1))
            //            //{
            //            //    return 2;
            //            //}

            //            Equipment.WorkTotalTime_Outline_AdditionalTime = Config.ParamConfig.Outline_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.

            //            foreach (var entity in layer)
            //            {
            //                switch (entity.EntityType)
            //                {
            //                    case EType.Point:
            //                        var point = entity as SpiralLab.Sirius.Point;
            //                        //point.Location 
            //                        //point.DwellTime
            //                        //success &= point.Mark(markerArg);
            //                        break;

            //                    case EType.Points:
            //                        var points = entity as Points;
            //                        foreach (var vertex in points)
            //                        {
            //                            //vertex.X
            //                            //vertex.Y
            //                        }
            //                        //points.DwellTime
            //                        //success &= points.Mark(markerArg);
            //                        break;

            //                    case EType.Line:
            //                        var line = entity as SpiralLab.Sirius.Line;

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End

            //                        //  객체 Type
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                        //  객체 Edge 좌표 개수
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                        //  객체 Edge 좌표 데이터 저장
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)line.Start.X;
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)line.Start.Y;
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)line.End.X;
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)line.End.Y;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)line.Start.X;
            //                            m_ptFrom.Y = (double)line.Start.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)line.End.X;
            //                        m_ptLast.Y = (double)line.End.Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_OutlineDataLength += (double)line.Length;

            //                        //  영역 객체 개수 +1
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;

            //                        //line.Start
            //                        //line.End
            //                        //success &= line.Mark(markerArg);
            //                        break;

            //                    case EType.Arc:
            //                        var arc = entity as SpiralLab.Sirius.Arc;

            //                        //  객체 Type
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                        //  객체 Radius
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dRadius = (double)arc.Radius;

            //                        //  객체 Center 좌표
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dCenter.X = (double)arc.Center.X;
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dCenter.Y = (double)arc.Center.Y;

            //                        //  객체 Start Angle
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dStartAngle = (double)arc.StartAngle;

            //                        //  객체 Sweep Angle
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dSweepAngle = (double)arc.SweepAngle;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)arc.Center.X;
            //                            m_ptFrom.Y = (double)arc.Center.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)arc.Center.X;
            //                        m_ptLast.Y = (double)arc.Center.Y;

            //                        //  가공 데이터 길이 누적
            //                        if ((double)arc.SweepAngle > 0.0)
            //                            m_dTotal_OutlineDataLength += (double)arc.Radius * 2.0 * Math.PI * ((double)arc.SweepAngle / 360.0);

            //                        //  영역 객체 개수 +1
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;

            //                        //arc.Radius
            //                        //arc.Center
            //                        //arc.StartAngle
            //                        //arc.SweepAngle
            //                        //success &= arc.Mark(markerArg);
            //                        break;

            //                    case EType.Circle:
            //                        var circle = entity as SpiralLab.Sirius.Circle;

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                        //  객체 Type
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                        //  객체 Edge 좌표 개수
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = 1;

            //                        //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)circle.Center.X;
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)circle.Center.Y;

            //                        //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)circle.Radius;
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)circle.Radius;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)circle.Center.X;
            //                            m_ptFrom.Y = (double)circle.Center.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)circle.Center.X;
            //                        m_ptLast.Y = (double)circle.Center.Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_OutlineDataLength += (double)circle.Radius * 2.0 * Math.PI;

            //                        //  영역 객체 개수 +1
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;

            //                        //circle.Center 
            //                        //circle.Radius
            //                        //success &= circle.Mark(markerArg);
            //                        break;

            //                    case EType.Rectangle:
            //                        var rectangle = entity as SpiralLab.Sirius.Rectangle;

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                        //  객체 Type
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                        //  객체 Edge 좌표 개수
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = 5;

            //                        //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[2].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[2].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[3].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[3].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[4].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[4].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                            m_ptFrom.Y = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                        m_ptLast.Y = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_OutlineDataLength += ((double)rectangle.Width * 2.0) + ((double)rectangle.Height * 2.0);

            //                        //  영역 객체 개수 +1
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;

            //                        //rectangle.Width
            //                        //rectangle.Height
            //                        //rectangle.Align
            //                        //rectangle.Location
            //                        //success &= rectangle.Mark(markerArg);
            //                        break;

            //                    case EType.LWPolyline:
            //                        var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                        //lwPolyline.IsClosed

            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                        //  객체 Type
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                        //  객체 Edge 좌표 개수
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count; ;

            //                        //  객체 Edge 좌표 데이터 저장
            //                        for (int n_pl = 0; n_pl < lwPolyline.Count; n_pl++)
            //                        {
            //                            m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)lwPolyline.Items[n_pl].X;
            //                            m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)lwPolyline.Items[n_pl].Y;
            //                        }

            //                        //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                        if (lwPolyline.IsClosed)
            //                        {
            //                            m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].X = (double)lwPolyline.Items[0].X;
            //                            m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].Y = (double)lwPolyline.Items[0].Y;
            //                        }

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                            m_ptFrom.Y = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                        m_ptLast.Y = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                        //  가공 데이터 길이 누적
            //                        for (int n_pl = 0; n_pl < lwPolyline.Count - 1; n_pl++)
            //                        {
            //                            m_ptFrom.X = (double)lwPolyline.Items[n_pl].X;
            //                            m_ptFrom.Y = (double)lwPolyline.Items[n_pl].Y;
            //                            m_ptTo.X = (double)lwPolyline.Items[n_pl + 1].X;
            //                            m_ptTo.Y = (double)lwPolyline.Items[n_pl + 1].Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_OutlineDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_OutlineDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  영역 객체 개수 +1
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;

            //                        //foreach (var vertex in lwPolyline)
            //                        //{
            //                        //    //vertex.X
            //                        //    //vertex.Y
            //                        //    //vertex.Bulge
            //                        //}
            //                        //success &= lwPolyline.Mark(markerArg);
            //                        break;

            //                    case EType.Spiral:
            //                        var spiral = entity as Spiral;
            //                        //spiral.OutterDiameter 
            //                        //spiral.InnerDiameter
            //                        //spiral.RadialPitch
            //                        //spiral.Revolutions
            //                        //spiral.Center
            //                        //success &= spiral.Mark(markerArg);
            //                        break;

            //                    case EType.Group:
            //                    default:
            //                        var group = entity as Group;

            //                        //m_stOutLine_LayerData = new WorkStage.stOutLine_LayerData();

            //                        //  전체 Object 개수
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectTotalNum = group.Count;

            //                        //  Object 별 데이터 공간 메모리 할당
            //                        m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData = new WorkStage.stOutLine_ObjectData[group.Count];

            //                        //  세부 데이터 저장
            //                        m_nGroupData_Count = 0;
            //                        foreach (var subEntity in group)
            //                        {
            //                            m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_nGroupData_Count].bAssigned = false;

            //                            Type t = subEntity.GetType();
            //                            if (t.Name == "LwPolyline")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                //  객체 Type
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                //  객체 Edge 좌표 개수
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                //  객체 Edge 좌표 데이터 저장
            //                                for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                {
            //                                    m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;
            //                                    m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                }

            //                                //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                if (pl.IsClosed)
            //                                {
            //                                    m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;
            //                                    m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                }

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                {
            //                                    m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                    m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                    m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                    m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_OutlineDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  영역 객체 개수 +1
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Circle")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                //  객체 Type
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                //  객체 Edge 좌표 개수
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Center.X;
            //                                    m_ptFrom.Y = (double)pl.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.Center.X;
            //                                m_ptLast.Y = (double)pl.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_OutlineDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                //  영역 객체 개수 +1
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Rectangle")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                //  객체 Type
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                //  객체 Edge 좌표 개수
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_OutlineDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                //  영역 객체 개수 +1
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Line")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Line;

            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End

            //                                //  객체 Type
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                                //  객체 Edge 좌표 개수
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                                //  객체 Edge 좌표 데이터 저장
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Start.X;
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Start.Y;
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.End.X;
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.End.Y;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Start.X;
            //                                    m_ptFrom.Y = (double)pl.Start.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.End.X;
            //                                m_ptLast.Y = (double)pl.End.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_OutlineDataLength += (double)pl.Length;

            //                                //  영역 객체 개수 +1
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Arc")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Arc;

            //                                //  객체 Type
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                                //  객체 Radius
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dRadius = (double)pl.Radius;

            //                                //  객체 Center 좌표
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dCenter.X = (double)pl.Center.X;
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dCenter.Y = (double)pl.Center.Y;

            //                                //  객체 Start Angle
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dStartAngle = (double)pl.StartAngle;

            //                                //  객체 Sweep Angle
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].m_stOutLine_ObjectData[m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount].stArcData.dSweepAngle = (double)pl.SweepAngle;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Center.X;
            //                                    m_ptFrom.Y = (double)pl.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_OutlineJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.Center.X;
            //                                m_ptLast.Y = (double)pl.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                if ((double)pl.SweepAngle > 0.0)
            //                                    m_dTotal_OutlineDataLength += (double)pl.Radius * 2.0 * Math.PI * ((double)pl.SweepAngle / 360.0);

            //                                //  영역 객체 개수 +1
            //                                m_stOutLine_LayerData[m_nOutLine_LayerCount].nRegion_ObjectCount++;
            //                            }
            //                            else        //  또 뭐가 있나...
            //                            {

            //                            }
            //                        }

            //                        //success &= group.Mark(markerArg);
            //                        break;
            //                        // case EType....
            //                        // ...

            //                        //default:
            //                        //    if (entity is IMarkerable markerable)
            //                        //    {
            //                        //        // mark entity
            //                        //        // 해당 개체(Entity) 가공 
            //                        //        //success &= markerable.Mark(markerArg);
            //                        //    }
            //                        //    break;
            //                }
            //                if (!success)
            //                    break;
            //            }

            //            m_nLayerCount++;
            //            m_nLayerOutLine_Count++;                    //  Outline Layer 카운트 +1
            //        }

            //        ///////////////////////
            //        ///                 ///
            //        ///     드릴링      ///
            //        ///                 ///
            //        ///////////////////////
            //        else if (layer.Name == "드릴링")
            //        {
            //            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            //            {
            //                return (int)nGetDataResult.GETDATA_FAIL;
            //            }

            //            m_ptLast.X = 0.0;
            //            m_ptLast.Y = 0.0;

            //            m_stLayerType.m_nLayerType[m_nLayerCount] = (int)LayerType.LAYER_DRILLING;

            //            //  Item 이 Group 인지 아닌지 확인 (Group 이면 저 아래에서 데이터 변수 할당, Group 이 아니면 여기서 할당)
            //            int m_nCount = 0;

            //            LayerIsGroup = true;

            //            if (layer.MotionType == MotionType.ScannerOnly)             //  ScannerOnly
            //            {
            //                foreach (var entity in layer)
            //                {
            //                    var group = entity as Group;

            //                    if (group == null)
            //                    {
            //                        m_nCount = layer.Count;

            //                        LayerIsGroup = false;
            //                    }
            //                    else
            //                    {
            //                        m_nCount++;
            //                    }
            //                }
            //            }
            //            else if (layer.MotionType == MotionType.StageOnly)          //  StageOnly
            //            {
            //                return (int)nGetDataResult.GETDATA_MOTIONTYPE_NG;       //  "Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다."
            //            }
            //            else                                                        //  StageAndScanner
            //            {
            //                foreach (var entity in layer)
            //                {
            //                    var group = entity as Group;

            //                    if (group == null)
            //                    {
            //                        m_nCount = layer.Count;

            //                        LayerIsGroup = false;
            //                    }
            //                    else
            //                    {
            //                        m_nCount = 1;
            //                    }

            //                    break;
            //                }
            //            }

            //            if (!LayerIsGroup)
            //            {
            //                //  드릴링 데이터는 Group 만 해야 한다.
            //                return (int)nGetDataResult.GETDATA_DRILDATA_NOT_GROUP;        //  "드릴링 데이터가 Group 이 아닙니다."
            //            }

            //            //  정상적인 데이터인지 체크
            //            if ((Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_RTC6) && (m_nCount != 1))
            //            {
            //                return (int)nGetDataResult.GETDATA_NOT_GROUP;        //  "데이터가 Group 이 아닙니다."
            //            }
            //            //else if ((Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_SYNCAXIS) && (m_nCount == 1))
            //            //{
            //            //    //return (int)nGetDataResult.GETDATA_UNGROUP;          //  "데이터를 Group 해제 해야 합니다."
            //            //}

            //            if (m_nCount >= 1)
            //            {
            //                m_stDrilling_LayerData = new WorkStage.stDrilling_LayerData();

            //                if (layer.MotionType == MotionType.ScannerOnly)
            //                {
            //                    m_stDrilling_LayerData.nRegion_MotionType = (int)MotionType.ScannerOnly;

            //                    //  전체 Object 개수
            //                    //m_stDrilling_LayerData.nRegion_ObjectTotalNum = m_nCount;       //   layer.Count;
            //                    m_stDrilling_LayerData.nRegion_GroupTotalNum = m_nCount;

            //                    //  Object 별 데이터 공간 메모리 할당
            //                    //m_stDrilling_LayerData.m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[m_nCount/*layer.Count*/];
            //                    m_stDrilling_LayerData.m_stDrilling_GroupData = new WorkStage.stDrilling_GroupData[m_nCount/*layer.Count*/];
            //                }
            //                else                //  MotionType.StageAndScanner
            //                {
            //                    m_stDrilling_LayerData.nRegion_MotionType = (int)MotionType.StageAndScanner;

            //                    m_stDrilling_LayerData.nRegion_GroupTotalNum = 1;
            //                    m_stDrilling_LayerData.m_stDrilling_GroupData = new WorkStage.stDrilling_GroupData[1];

            //                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectTotalNum = m_nCount;       //   layer.Count;
            //                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[m_nCount/*layer.Count*/];
            //                }
            //            }

            //            ///////////////////////////
            //            ///////////////////////////
            //            ////                   ////
            //            ////  [syncAxis 모드]  ////
            //            ////                   ////
            //            ///////////////////////////
            //            ///////////////////////////
            //            ///
            //            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_SYNCAXIS)              //  syncAxis 모드
            //            {
            //                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                //
            //                //  [드릴링] [syncAxis] [ScannerOnly]
            //                //
            //                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                ///
            //                if (layer.MotionType == MotionType.ScannerOnly)             //  MotionType.ScannerOnly
            //                {
            //                    //  Group 개수
            //                    foreach (var entity in layer)
            //                    {
            //                        m_nGroupCount++;
            //                    }
            //                    m_stDividedRegion_GroupData = new WorkStage.stDividedRegion_GroupData[m_nGroupCount];
            //                    m_stDividedRegion_GroupData[0].nGroup_Num = m_nGroupCount;

            //                    m_nGroupCount = 0;


            //                    //  2023. 05. 23.  SCH : Group 객체들 중에 분할 영역을 넘어가는 부분이 하나라도 있으면 전체를 DividedRegion 으로 처리한다.
            //                    m_bGroupExist_LargerThanDivideSize = false;

            //                    foreach (var entity in layer)
            //                    {
            //                        switch (entity.EntityType)
            //                        {
            //                            case EType.Point: break;
            //                            case EType.Points: break;
            //                            case EType.Line: break;
            //                            case EType.Arc: break;
            //                            case EType.Circle: break;
            //                            case EType.Rectangle: break;
            //                            case EType.LWPolyline: break;
            //                            case EType.Spiral: break;
            //                            case EType.Group:
            //                            default:
            //                                var group = entity as Group;

            //                                //if ((double)group.Width > Config.ParamConfig.Drilling_DivideSize)             //  2023. 11. 24.  SCH : Group 의 가로 크기만 봤었는데,
            //                                if (((double)group.Width > Config.ParamConfig.Drilling_DivideSize) ||
            //                                    ((double)group.Height > Config.ParamConfig.Drilling_DivideSize))            //  2023. 11. 24.  SCH : Group 이 가로가 얇고 세로로 길게 되어 있는 도면이 있어서, 세로 크기도 함께 보도록 한다.
            //                                {
            //                                    m_bGroupExist_LargerThanDivideSize = true;
            //                                }

            //                                break;
            //                        }
            //                    }

            //                    foreach (var entity in layer)
            //                    {
            //                        switch (entity.EntityType)
            //                        {
            //                            case EType.Point:
            //                                var point = entity as SpiralLab.Sirius.Point;
            //                                //point.Location 
            //                                //point.DwellTime
            //                                //success &= point.Mark(markerArg);
            //                                break;

            //                            case EType.Points:
            //                                var points = entity as Points;
            //                                foreach (var vertex in points)
            //                                {
            //                                    //vertex.X
            //                                    //vertex.Y
            //                                }
            //                                //points.DwellTime
            //                                //success &= points.Mark(markerArg);
            //                                break;

            //                            case EType.Line:
            //                                var line = entity as SpiralLab.Sirius.Line;

            //                                //line.Start
            //                                //line.End
            //                                //success &= line.Mark(markerArg);
            //                                break;

            //                            case EType.Arc:
            //                                var arc = entity as SpiralLab.Sirius.Arc;
            //                                //arc.Radius
            //                                //arc.Center
            //                                //arc.StartAngle
            //                                //arc.SweepAngle
            //                                //success &= arc.Mark(markerArg);
            //                                break;

            //                            case EType.Circle:
            //                                var circle = entity as SpiralLab.Sirius.Circle;

            //                                //circle.Center 
            //                                //circle.Radius
            //                                //success &= circle.Mark(markerArg);
            //                                break;

            //                            case EType.Rectangle:
            //                                var rectangle = entity as SpiralLab.Sirius.Rectangle;

            //                                //rectangle.Width
            //                                //rectangle.Height
            //                                //rectangle.Align
            //                                //rectangle.Location
            //                                //success &= rectangle.Mark(markerArg);
            //                                break;

            //                            case EType.LWPolyline:
            //                                var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                                //lwPolyline.IsClosed

            //                                //foreach (var vertex in lwPolyline)
            //                                //{
            //                                //    //vertex.X
            //                                //    //vertex.Y
            //                                //    //vertex.Bulge
            //                                //}
            //                                //success &= lwPolyline.Mark(markerArg);
            //                                break;

            //                            case EType.Spiral:
            //                                var spiral = entity as Spiral;
            //                                //spiral.OutterDiameter 
            //                                //spiral.InnerDiameter
            //                                //spiral.RadialPitch
            //                                //spiral.Revolutions
            //                                //spiral.Center
            //                                //success &= spiral.Mark(markerArg);
            //                                break;

            //                            case EType.Group:
            //                            default:
            //                                var group = entity as Group;

            //                                //m_stDrilling_LayerData = new WorkStage.stDrilling_LayerData();

            //                                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                /// Drilling 데이터가 Polyline 이면 좋지만, Line 일 경우는 Polyline 처럼 Drilling Hole 에 대한 데이터로 정리해줘야 한다. (Line 4개가 1개의 Drilling Hole 이 된다.)
            //                                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                ///
            //                                m_nOtherData_Count = 0;
            //                                m_nPolylineData_Count = 0;
            //                                m_nLineData_Count = 0;

            //                                //  Line 데이터가 몇개 있는지 확인 (4의 배수가 되어야 함. 사각형이기 때문에)
            //                                foreach (var subEntity_forLineCount in group)
            //                                {
            //                                    Type t = subEntity_forLineCount.GetType();

            //                                    if (t.Name == "LwPolyline")
            //                                    {
            //                                        m_nPolylineData_Count++;
            //                                    }
            //                                    else if (t.Name == "Line")
            //                                    {
            //                                        m_nLineData_Count++;
            //                                    }
            //                                    else if (t.Name == "Circle")
            //                                    {
            //                                        m_nCircleData_Count++;
            //                                    }
            //                                    else
            //                                    {
            //                                        m_nOtherData_Count++;
            //                                    }
            //                                }

            //                                if ((m_nOtherData_Count > 0) ||                                         //  "Drilling Data 는 Polyline, Line, Circle 중 한 가지 데이터로만 구성되어야 합니다."
            //                                    ((m_nPolylineData_Count > 0) && (m_nLineData_Count > 0)) ||
            //                                    ((m_nPolylineData_Count > 0) && (m_nCircleData_Count > 0)) ||
            //                                    ((m_nLineData_Count > 0) && (m_nCircleData_Count > 0)))
            //                                {
            //                                    return (int)nGetDataResult.GETDATA_DRILDATA_NG;
            //                                }


            //                                //  2024. 01. 29.  SCH : [테스트] 미세홀 가공 시 무조건 Div 로 하기 위함.
            //                                m_bGroupExist_LargerThanDivideSize = true;

            //                                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                //
            //                                //  [드릴링] [syncAxis] [ScannerOnly]  :  Group Data 가 Scanner FOV 이내인 경우
            //                                //
            //                                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                //if ((double)group.Width <= Config.ParamConfig.Drilling_DivideSize)
            //                                if (!m_bGroupExist_LargerThanDivideSize)
            //                                {
            //                                    m_stLayerType.m_bDrillingGroupSize_withinScannerFOV = true;
            //                                    m_bDrillingData_isLine = false;

            //                                    if (m_nPolylineData_Count > 0)                                          //  데이터가 모두 Polyline 인 경우
            //                                    {
            //                                        m_nDrillingData_Type = (int)ObjectType.OBJECT_POLY;

            //                                        //  전체 Object 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum = group.Count;

            //                                        //  Object 별 데이터 공간 메모리 할당
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[group.Count];

            //                                        //  Drilling 데이터 개수
            //                                        m_nOutlineData_Count += m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum;


            //                                        //  Pre-Drilling 추가 시간
            //                                        if (Config.ParamConfig.bPreDrilling_Use)
            //                                        {
            //                                            if (Config.ParamConfig.bPreDrilling_WorkUnit_Hole)          //  Hole 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                            else                                                        //  Rect 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += Config.ParamConfig.PreDrilling_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                        }

            //                                        //  Drilling 추가 시간
            //                                        if (Config.ParamConfig.Drilling_ProcessingPriority_EachSideFirst)          //  면 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * 4 * Equipment.MainCycle_Interval);    //  4 는 4개의 Line 이 1개의 Rectangle 이 되므로 4를 곱해줌.
            //                                                                                                                                                                                                                                                                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                        else                                                                       //  rect 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                    }
            //                                    else if (m_nLineData_Count > 0)                                         //  데이터가 모두 Line 인 경우
            //                                    {
            //                                        if ((m_nLineData_Count % 4) != 0)                                       //  "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다."
            //                                        {
            //                                            return (int)nGetDataResult.GETDATA_DRILDATA_LINECNT;
            //                                        }

            //                                        m_nDrillingData_Type = (int)ObjectType.OBJECT_LINE;

            //                                        m_bDrillingData_isLine = true;

            //                                        //  Line Data 4개로 사각형을 만들어야 한다. Line 4개 당 1개의 polyline 저장 공간을 할당한다.

            //                                        //  전체 Object 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum = m_nLineData_Count / 4;

            //                                        //  Object 별 데이터 공간 메모리 할당
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[m_nLineData_Count / 4];

            //                                        //  Drilling 데이터 개수
            //                                        m_nOutlineData_Count += m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum;


            //                                        //  Pre-Drilling 추가 시간
            //                                        if (Config.ParamConfig.bPreDrilling_Use)
            //                                        {
            //                                            //  Pre-Drilling 추가 시간
            //                                            if (Config.ParamConfig.bPreDrilling_WorkUnit_Hole)          //  Hole 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                            else                                                        //  Rect 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += Config.ParamConfig.PreDrilling_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                        }

            //                                        //  Drilling 추가 시간
            //                                        if (Config.ParamConfig.Drilling_ProcessingPriority_EachSideFirst)          //  면 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * 4 * Equipment.MainCycle_Interval);    //  4 는 4개의 Line 이 1개의 Rectangle 이 되므로 4를 곱해줌.
            //                                                                                                                                                                                                                                                                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                        else                                                                       //  rect 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                    }
            //                                    else if (m_nCircleData_Count > 0)                                       //  데이터가 모두 Circle 인 경우
            //                                    {
            //                                        m_nDrillingData_Type = (int)ObjectType.OBJECT_CIR;

            //                                        //  전체 Object 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum = group.Count;

            //                                        //  Object 별 데이터 공간 메모리 할당
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[group.Count];

            //                                        //  Drilling 데이터 개수
            //                                        m_nOutlineData_Count += m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum;


            //                                        //  Pre-Drilling 추가 시간
            //                                        if (Config.ParamConfig.bPreDrilling_Use)
            //                                        {
            //                                            //  Pre-Drilling 추가 시간
            //                                            if (Config.ParamConfig.bPreDrilling_WorkUnit_Hole)          //  Hole 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                            else                                                        //  Rect 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += Config.ParamConfig.PreDrilling_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                        }

            //                                        //  Drilling 추가 시간
            //                                        if (Config.ParamConfig.Drilling_ProcessingPriority_EachSideFirst)          //  면 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * 4 * Equipment.MainCycle_Interval);    //  4 는 4개의 Line 이 1개의 Rectangle 이 되므로 4를 곱해줌.
            //                                                                                                                                                                                                                                                                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                        else                                                                       //  rect 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                    }

            //                                    ///
            //                                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //                                    //  Group 별 Center 좌표 저장
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].dGroupCenter.X = (double)group.Location.X;
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].dGroupCenter.Y = (double)group.Location.Y;

            //                                    //  Drilling Hole 이 Line 으로 되어있을 경우, 4개 당 1개의 Hole 로 만들기 위한 카운트 변수
            //                                    m_nEachDrillHole_LineCount = 0;

            //                                    //  세부 데이터 저장
            //                                    m_nGroupData_Count = 0;
            //                                    foreach (var subEntity in group)
            //                                    {
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_nGroupData_Count].bAssigned = false;

            //                                        Type t = subEntity.GetType();
            //                                        if (t.Name == "LwPolyline")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                            //  객체 Type
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                            //  객체 Edge 좌표 개수
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                            //  객체 Edge 좌표 데이터 저장
            //                                            for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                            {
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                            }

            //                                            //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                            if (pl.IsClosed)
            //                                            {
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                            }

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                            m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                            //  가공 데이터 길이 누적
            //                                            for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                            {
            //                                                m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                                m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                                m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                                m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  영역 객체 개수 +1
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
            //                                        }
            //                                        else if (t.Name == "Circle")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                            //  객체 Type
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                            //  객체 Edge 좌표 개수
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                            //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                            //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)pl.Center.X;
            //                                                m_ptFrom.Y = (double)pl.Center.Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = (double)pl.Center.X;
            //                                            m_ptLast.Y = (double)pl.Center.Y;

            //                                            //  가공 데이터 길이 누적
            //                                            m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                            //  영역 객체 개수 +1
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
            //                                        }
            //                                        else if (t.Name == "Rectangle")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                            //  객체 Type
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                            //  객체 Edge 좌표 개수
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                            //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                            //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);              //  왜 이걸로 되어 있는고?
            //                                            //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);             //  왜 이걸로 되어 있는고?
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                            //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);              //  왜 이걸로 되어 있는고?
            //                                            //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);             //  왜 이걸로 되어 있는고?
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                            m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                            //  가공 데이터 길이 누적
            //                                            m_dTotal_DrillingDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                            //  영역 객체 개수 +1
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
            //                                        }
            //                                        else if (t.Name == "Line")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Line;

            //                                            //  Line 데이터를 Polyline 처럼 변경한다. (나열된 Line 으로는 드릴홀인지 아닌지 확인이 안됨)

            //                                            if (m_nEachDrillHole_LineCount == 0)              //  사각형을 이루는 Line 의 시작
            //                                            {
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  사각형은 좌표 개수가 5개 (시작 위치에서 다시 시작 위치로 와야 함)
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  사각형은 좌표 개수가 5개 (시작 위치에서 다시 시작 위치로 와야 함)   - 내부 가공에 사용                                                          

            //                                                //  객체 Type
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                                //  객체 Edge 좌표 개수
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                                //  1 번째 Edge 좌표 저장 (Start)
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
            //                                                m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
            //                                                m_nEachDrillHole_LineCount++;


            //                                                ////  기울어진 사각형에는 사용할 수 없는 코드
            //                                                //{
            //                                                //    //  2 번째 Edge 좌표 저장 (End)
            //                                                //    //  1 번째 Edge 좌표의 End 좌표와 거리가 먼 Start or End 좌표가 진짜 End 좌표이다.
            //                                                //    if (((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X - 0.002) <= (double)pl.End.X) &&
            //                                                //        ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X + 0.002) >= (double)pl.End.X) &&

            //                                                //        ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y - 0.002) <= (double)pl.End.Y) &&
            //                                                //        ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y + 0.002) >= (double)pl.End.Y))
            //                                                //    {
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
            //                                                //    }
            //                                                //    else
            //                                                //    {
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.End.X;
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.End.Y;
            //                                                //    }
            //                                                //}
            //                                                //m_nEachDrillHole_LineCount++;
            //                                            }
            //                                            else
            //                                            {
            //                                                ////  기울어진 사각형에는 사용할 수 없는 코드
            //                                                //{
            //                                                //    //  3 ~ 5 번째 Edge 좌표 저장 (End 위치만 사용)
            //                                                //    //  앞 Line 의 End 좌표와 거리가 먼 Start or End 좌표가 진짜 End 좌표이다.
            //                                                //    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.End.X;
            //                                                //    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.End.Y;
            //                                                //    if (((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X - 0.002) <= (double)pl.End.X) &&
            //                                                //        ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X + 0.002) >= (double)pl.End.X) &&

            //                                                //        ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y - 0.002) <= (double)pl.End.Y) &&
            //                                                //        ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.
            //                                                //        m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y + 0.002) >= (double)pl.End.Y))
            //                                                //    {
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
            //                                                //    }
            //                                                //    else
            //                                                //    {
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.End.X;
            //                                                //        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.End.Y;
            //                                                //    }
            //                                                //}

            //                                                //  라인이 순서대로 되어 있다는 가정 하에 사용할 수 있는 코드
            //                                                {
            //                                                    //  2 ~ 4 번째 Edge 좌표 저장 (Start), 5 번째는 처음 위치의 좌표값을 넣어줌
            //                                                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X = (double)pl.Start.X;
            //                                                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y = (double)pl.Start.Y;
            //                                                }

            //                                                m_nEachDrillHole_LineCount++;

            //                                                //  마지막 Edge 좌표(5 번째) 였으면? --> Object 개수 증가, Line Count 0 세팅
            //                                                //if (m_nEachDrillHole_LineCount == 5)                                          //  기울어진 사각형에는 사용할 수 없는 코드
            //                                                if (m_nEachDrillHole_LineCount == 4)                                            //  라인이 순서대로 되어 있다는 가정 하에 사용할 수 있는 코드
            //                                                {
            //                                                    //  라인이 순서대로 되어 있다는 가정 하에 사용할 수 있는 코드
            //                                                    {
            //                                                        //  5 번째는 처음 위치의 좌표값을 넣어줌
            //                                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].X =
            //                                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount].Y =
            //                                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                    }



            //                                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                    //  데이터 좌표계 정규화

            //                                                    //  Min, Max 좌표
            //                                                    m_dLine_Min_X = double.MaxValue;
            //                                                    m_dLine_Max_X = double.MinValue;
            //                                                    m_dLine_Min_Y = double.MaxValue;
            //                                                    m_dLine_Max_Y = double.MinValue;

            //                                                    //  기울어진 사각형에는 사용할 수 없는 코드
            //                                                    for (int i = 0; i < 4; i++)
            //                                                    {
            //                                                        if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X >= m_dLine_Max_X)
            //                                                            m_dLine_Max_X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X;
            //                                                        if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X <= m_dLine_Min_X)
            //                                                            m_dLine_Min_X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].X;

            //                                                        if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y >= m_dLine_Max_Y)
            //                                                            m_dLine_Max_Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y;
            //                                                        if (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y <= m_dLine_Min_Y)
            //                                                            m_dLine_Min_Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[i].Y;
            //                                                    }

            //                                                    //  직각사각형(?)인 경우에만 적용할 수 있는 코드

            //                                                    ////  객체 Edge 좌표 데이터 저장 (LT --> LB --> RB --> RT --> LT)
            //                                                    ////  Left Top
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X = m_dLine_Min_X;
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y = m_dLine_Max_Y;

            //                                                    ////  Left Bottom
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].X = m_dLine_Min_X;
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[1].Y = m_dLine_Min_Y;

            //                                                    ////  Right Bottom
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].X = m_dLine_Max_X;
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[2].Y = m_dLine_Min_Y;

            //                                                    ////  Right Top
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[3].X = m_dLine_Max_X;
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[3].Y = m_dLine_Max_Y;

            //                                                    ////  Left Top
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[4].X = m_dLine_Min_X;
            //                                                    //m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                    //    m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[4].Y = m_dLine_Max_Y;

            //                                                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                                    ///

            //                                                    if ((m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X !=
            //                                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].X) ||
            //                                                        (m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y !=
            //                                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[m_nEachDrillHole_LineCount - 1].Y))
            //                                                    {
            //                                                        return (int)nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED;        //  "Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다."
            //                                                    }

            //                                                    m_nEachDrillHole_LineCount = 0;

            //                                                    //  Jump 데이터 길이 누적
            //                                                    if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                                    {
            //                                                        m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                                            m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                        m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                                            m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                        m_ptTo.X = (double)m_ptLast.X;
            //                                                        m_ptTo.Y = (double)m_ptLast.Y;

            //                                                        if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                        {
            //                                                            m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                        }
            //                                                        else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                        {
            //                                                            m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                        }
            //                                                        else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                        {
            //                                                            m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                        }
            //                                                    }

            //                                                    //  마지막 좌표 위치 저장
            //                                                    m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                                m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                    m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].
            //                                                                m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                                    //  가공 데이터 길이 누적
            //                                                    m_dTotal_DrillingDataLength += ((m_dLine_Max_X - m_dLine_Min_X) * 2.0) + ((m_dLine_Max_Y - m_dLine_Min_Y) * 2.0);

            //                                                    //  영역 객체 개수 +1
            //                                                    m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
            //                                                }
            //                                            }
            //                                        }
            //                                        else if (t.Name == "Arc")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Arc;

            //                                            //  객체 Type
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                                            //  객체 Radius
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dRadius = (double)pl.Radius;

            //                                            //  객체 Center 좌표
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dCenter.X = (double)pl.Center.X;
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dCenter.Y = (double)pl.Center.Y;

            //                                            //  객체 Start Angle
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dStartAngle = (double)pl.StartAngle;

            //                                            //  객체 Sweep Angle
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount].stArcData.dSweepAngle = (double)pl.SweepAngle;

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)pl.Center.X;
            //                                                m_ptFrom.Y = (double)pl.Center.Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = (double)pl.Center.X;
            //                                            m_ptLast.Y = (double)pl.Center.Y;

            //                                            //  가공 데이터 길이 누적
            //                                            if ((double)pl.SweepAngle > 0.0)
            //                                                m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI * ((double)pl.SweepAngle / 360.0);

            //                                            //  영역 객체 개수 +1
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectCount++;
            //                                        }
            //                                        else        //  또 뭐가 있나...
            //                                        {

            //                                        }
            //                                    }
            //                                }

            //                                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                //
            //                                //  [드릴링] [syncAxis] [ScannerOnly]  :  Group Data 가 Scanner FOV 를 초과하는 경우 --> 분할 가공
            //                                //
            //                                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                else
            //                                {
            //                                    m_stLayerType.m_bDrillingGroupSize_withinScannerFOV = false;
            //                                    m_bDrillingData_isLine = false;

            //                                    //  어떤 데이터로 이루어져 있는지 확인하고 메모리 할당
            //                                    if (m_nPolylineData_Count > 0)                                          //  데이터가 모두 Polyline 인 경우
            //                                    {
            //                                        m_nDrillingData_Type = (int)ObjectType.OBJECT_POLY;

            //                                        //  전체 Object 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum = group.Count;

            //                                        //  Object 별 데이터 공간 메모리 할당
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[group.Count];

            //                                        //  Drilling 데이터 개수
            //                                        m_nOutlineData_Count += m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum;

            //                                        //  밑에 있던건데 요기서 해줌. 
            //                                        //  전체 도형 개수
            //                                        m_nGroupData_TotalCount = group.Count;


            //                                        //  Pre-Drilling 추가 시간
            //                                        if (Config.ParamConfig.bPreDrilling_Use)
            //                                        {
            //                                            //  Pre-Drilling 추가 시간
            //                                            if (Config.ParamConfig.bPreDrilling_WorkUnit_Hole)          //  Hole 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                            else                                                        //  Rect 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += Config.ParamConfig.PreDrilling_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                        }

            //                                        //  Drilling 추가 시간
            //                                        if (Config.ParamConfig.Drilling_ProcessingPriority_EachSideFirst)          //  면 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * 4 * Equipment.MainCycle_Interval);    //  4 는 4개의 Line 이 1개의 Rectangle 이 되므로 4를 곱해줌.
            //                                                                                                                                                                                                                                                                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                        else                                                                       //  rect 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                    }
            //                                    else if (m_nLineData_Count > 0)                                         //  데이터가 모두 Line 인 경우
            //                                    {
            //                                        if ((m_nLineData_Count % 4) != 0)                                       //  "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다."
            //                                        {
            //                                            return (int)nGetDataResult.GETDATA_DRILDATA_LINECNT;
            //                                        }

            //                                        m_nDrillingData_Type = (int)ObjectType.OBJECT_LINE;

            //                                        m_bDrillingData_isLine = true;

            //                                        //  Line Data 4개로 사각형을 만들어야 한다. Line 4개 당 1개의 polyline 저장 공간을 할당한다.

            //                                        //  전체 Object 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum = m_nLineData_Count / 4;

            //                                        //  Object 별 데이터 공간 메모리 할당
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[m_nLineData_Count / 4];

            //                                        //  Drilling 데이터 개수
            //                                        m_nOutlineData_Count += m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum;

            //                                        //  밑에 있던건데 요기서 해줌. 
            //                                        //  전체 도형 개수
            //                                        m_nGroupData_TotalCount = group.Count / 4;


            //                                        //  Pre-Drilling 추가 시간
            //                                        if (Config.ParamConfig.bPreDrilling_Use)
            //                                        {
            //                                            //  Pre-Drilling 추가 시간
            //                                            if (Config.ParamConfig.bPreDrilling_WorkUnit_Hole)          //  Hole 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                            else                                                        //  Rect 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += Config.ParamConfig.PreDrilling_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                        }

            //                                        //  Drilling 추가 시간
            //                                        if (Config.ParamConfig.Drilling_ProcessingPriority_EachSideFirst)          //  면 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * 4 * Equipment.MainCycle_Interval);    //  4 는 4개의 Line 이 1개의 Rectangle 이 되므로 4를 곱해줌.
            //                                                                                                                                                                                                                                                                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                        else                                                                       //  rect 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                    }
            //                                    else if (m_nCircleData_Count > 0)                                       //  데이터가 모두 Circle 인 경우
            //                                    {
            //                                        m_nDrillingData_Type = (int)ObjectType.OBJECT_CIR;

            //                                        //  전체 Object 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum = group.Count;

            //                                        //  Object 별 데이터 공간 메모리 할당
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[group.Count];

            //                                        //  Drilling 데이터 개수
            //                                        m_nOutlineData_Count += m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum;

            //                                        //  밑에 있던건데 요기서 해줌. 
            //                                        //  전체 도형 개수
            //                                        m_nGroupData_TotalCount = group.Count;


            //                                        //  Pre-Drilling 추가 시간
            //                                        if (Config.ParamConfig.bPreDrilling_Use)
            //                                        {
            //                                            //  Pre-Drilling 추가 시간
            //                                            if (Config.ParamConfig.bPreDrilling_WorkUnit_Hole)          //  Hole 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                            else                                                        //  Rect 단위
            //                                            {
            //                                                Equipment.WorkTotalTime_Drilling_AdditionalTime += Config.ParamConfig.PreDrilling_Repeat_Count * (double)Equipment.MainCycle_Interval;           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                            }
            //                                        }

            //                                        //  Drilling 추가 시간
            //                                        if (Config.ParamConfig.Drilling_ProcessingPriority_EachSideFirst)          //  면 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * 4 * Equipment.MainCycle_Interval);    //  4 는 4개의 Line 이 1개의 Rectangle 이 되므로 4를 곱해줌.
            //                                                                                                                                                                                                                                                                //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                        else                                                                       //  rect 단위
            //                                        {
            //                                            Equipment.WorkTotalTime_Drilling_AdditionalTime += (double)(m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount].nRegion_ObjectTotalNum * Equipment.MainCycle_Interval);           //  20 은 Main Cycle 의 Interval. Interval이 변경되면 여기서도 바꿔줘야 한다.
            //                                        }
            //                                    }

            //                                    ///
            //                                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //                                    //  전체 영역 크기 체크
            //                                    m_dGroupSize_Width = (double)group.Width;
            //                                    m_dGroupSize_Height = (double)group.Height;

            //                                    ////  전체 도형 개수
            //                                    //m_nGroupData_TotalCount = group.Count;

            //                                    //  전체 Index 계산
            //                                    m_nGroupIndex_TotalX = (int)(m_dGroupSize_Width / m_dDrilling_FOV);
            //                                    if ((m_dGroupSize_Width % m_dDrilling_FOV) > 0.0)
            //                                        m_nGroupIndex_TotalX++;

            //                                    m_nGroupIndex_TotalY = (int)(m_dGroupSize_Height / m_dDrilling_FOV);
            //                                    if ((m_dGroupSize_Height % m_dDrilling_FOV) > 0.0)
            //                                        m_nGroupIndex_TotalY++;

            //                                    //  전체 영역 시작 위치 (2사분면에서 시작)

            //                                    //           │
            //                                    //   2사분면 │ 1사분면
            //                                    //           │
            //                                    // ───────────
            //                                    //           │
            //                                    //   3사분면 │ 4사분면
            //                                    //           │

            //                                    m_dGroupStartPos_X = (double)group.Location.X - (((double)m_nGroupIndex_TotalX * m_dDrilling_FOV) / 2.0);
            //                                    m_dGroupStartPos_Y = (double)group.Location.Y + (((double)m_nGroupIndex_TotalY * m_dDrilling_FOV) / 2.0);

            //                                    m_stGroupDataForDivide = new stGroupDataForDivide[m_nGroupData_TotalCount];

            //                                    //  Index 계산
            //                                    foreach (var subEntity in group)
            //                                    {
            //                                        m_stGroupDataForDivide[m_nGroupData_Count].bAssigned = false;

            //                                        //  도형 Center 좌표 가져오기
            //                                        Type t = subEntity.GetType();
            //                                        if (t.Name == "LwPolyline")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                            m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)pl.Location.X;
            //                                            m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)pl.Location.Y;
            //                                        }
            //                                        else if (t.Name == "Circle")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                            m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)pl.Center.X;
            //                                            m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)pl.Center.Y;
            //                                        }
            //                                        else if (t.Name == "Rectangle")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                            m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)pl.Location.X;
            //                                            m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)pl.Location.Y;
            //                                        }
            //                                        else if (t.Name == "Line")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Line;

            //                                            //  Line 은 4개가 Drilling Hole 1개이다. (4개씩 데이터를 모아서 Center 좌표 계산하도록 한다)
            //                                            switch (m_nLineData_Count_forDrillHole)
            //                                            {
            //                                                case 0:                 //  1번째 라인
            //                                                case 1:                 //  2번째 라인
            //                                                case 2:                 //  3번째 라인
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.X = (double)pl.Start.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.Y = (double)pl.Start.Y;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.X = (double)pl.End.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.Y = (double)pl.End.Y;
            //                                                    m_nLineData_Count_forDrillHole++;

            //                                                    break;


            //                                                case 3:                 //  4번째 라인 --> 여기까지의 데이터로 사각형 만들기.
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.X = (double)pl.Start.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.Y = (double)pl.Start.Y;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.X = (double)pl.End.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.Y = (double)pl.End.Y;
            //                                                    m_nLineData_Count_forDrillHole++;

            //                                                    //  Min, Max 좌표
            //                                                    m_dLine_Min_X = double.MaxValue;
            //                                                    m_dLine_Max_X = double.MinValue;
            //                                                    m_dLine_Min_Y = double.MaxValue;
            //                                                    m_dLine_Max_Y = double.MinValue;

            //                                                    for (int i = 0; i < 4; i++)
            //                                                    {
            //                                                        if (m_stDrillHole_byLine[i].ptStart.X >= m_dLine_Max_X) m_dLine_Max_X = m_stDrillHole_byLine[i].ptStart.X;
            //                                                        if (m_stDrillHole_byLine[i].ptStart.X <= m_dLine_Min_X) m_dLine_Min_X = m_stDrillHole_byLine[i].ptStart.X;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.X <= m_dLine_Min_X) m_dLine_Min_X = m_stDrillHole_byLine[i].ptEnd.X;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.X >= m_dLine_Max_X) m_dLine_Max_X = m_stDrillHole_byLine[i].ptEnd.X;

            //                                                        if (m_stDrillHole_byLine[i].ptStart.Y >= m_dLine_Max_Y) m_dLine_Max_Y = m_stDrillHole_byLine[i].ptStart.Y;
            //                                                        if (m_stDrillHole_byLine[i].ptStart.Y <= m_dLine_Min_Y) m_dLine_Min_Y = m_stDrillHole_byLine[i].ptStart.Y;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.Y >= m_dLine_Max_Y) m_dLine_Max_Y = m_stDrillHole_byLine[i].ptEnd.Y;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.Y <= m_dLine_Min_Y) m_dLine_Min_Y = m_stDrillHole_byLine[i].ptEnd.Y;
            //                                                    }

            //                                                    //  RTC6 모드에서 사용했던 코드
            //                                                    //m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)(pl.Start.X + pl.End.X) / 2.0;
            //                                                    //m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)(pl.Start.Y + pl.End.Y) / 2.0;

            //                                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)(m_dLine_Max_X + m_dLine_Min_X) / 2.0;
            //                                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)(m_dLine_Max_Y + m_dLine_Min_Y) / 2.0;

            //                                                    break;
            //                                            }
            //                                        }
            //                                        else        //  또 뭐가 있나...
            //                                        {

            //                                        }

            //                                        //  각 도형이 분할 영역의 몇 번째 Index 에 들어가는지 계산 
            //                                        //  4개의 데이터가 모여야 하는 "Line" 은 4개의 데이터가 모인 후에 Index 계산.
            //                                        if (t.Name == "Line")
            //                                        {
            //                                            if (m_nLineData_Count_forDrillHole == 4)
            //                                            {
            //                                                m_nLineData_Count_forDrillHole = 0;

            //                                                //  각 도형이 Divided 영역의 몇 번째 Index 에 들어가는지 계산
            //                                                for (int x = 0; x < m_nGroupIndex_TotalX; x++)
            //                                                {
            //                                                    if ((m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X >= (m_dGroupStartPos_X + (m_dDrilling_FOV * (double)x))) &&
            //                                                        (m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X < (m_dGroupStartPos_X + (m_dDrilling_FOV * (double)x) + m_dDrilling_FOV)))
            //                                                    {
            //                                                        m_nDivCount_X = x;
            //                                                        x = m_nGroupIndex_TotalX;
            //                                                    }
            //                                                }
            //                                                for (int y = 0; y < m_nGroupIndex_TotalY; y++)
            //                                                {
            //                                                    if ((m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y <= (m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)y))) &&
            //                                                        (m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y > (m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)y) - m_dDrilling_FOV)))
            //                                                    {
            //                                                        m_nDivCount_Y = y;
            //                                                        y = m_nGroupIndex_TotalY;
            //                                                    }
            //                                                }

            //                                                //m_stGroupDataForDivide[m_nGroupData_Count++].nDivideIndex = (m_nDivCount_Y * m_nGroupIndex_TotalY) + m_nDivCount_X;
            //                                                m_stGroupDataForDivide[m_nGroupData_Count++].nDivideIndex = (m_nDivCount_Y * m_nGroupIndex_TotalX) + m_nDivCount_X;
            //                                            }
            //                                        }
            //                                        //  1개의 entity 로 도형이 완성되는 "LwPolyline", "Circle", "Rectangle" 일 경우만 여기에서 Index 계산.
            //                                        else        //  "LwPolyline", "Circle", "Rectangle" 일 경우
            //                                        {
            //                                            //  각 도형이 Divided 영역의 몇 번째 Index 에 들어가는지 계산
            //                                            for (int x = 0; x < m_nGroupIndex_TotalX; x++)
            //                                            {
            //                                                if ((m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X >= (m_dGroupStartPos_X + (m_dDrilling_FOV * (double)x))) &&
            //                                                    (m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X < (m_dGroupStartPos_X + (m_dDrilling_FOV * (double)x) + m_dDrilling_FOV)))
            //                                                {
            //                                                    m_nDivCount_X = x;
            //                                                    x = m_nGroupIndex_TotalX;
            //                                                }
            //                                            }
            //                                            for (int y = 0; y < m_nGroupIndex_TotalY; y++)
            //                                            {
            //                                                if ((m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y <= (m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)y))) &&
            //                                                    (m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y > (m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)y) - m_dDrilling_FOV)))
            //                                                {
            //                                                    m_nDivCount_Y = y;
            //                                                    y = m_nGroupIndex_TotalY;
            //                                                }
            //                                            }

            //                                            //m_stGroupDataForDivide[m_nGroupData_Count++].nDivideIndex = (m_nDivCount_Y * m_nGroupIndex_TotalY) + m_nDivCount_X;
            //                                            m_stGroupDataForDivide[m_nGroupData_Count++].nDivideIndex = (m_nDivCount_Y * m_nGroupIndex_TotalX) + m_nDivCount_X;
            //                                        }
            //                                    }

            //                                    //  Divided 영역별 데이터 개수 카운트
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData = new WorkStage.stDividedRegion_RegionData[m_nGroupIndex_TotalX * m_nGroupIndex_TotalY];
            //                                    foreach (var eachObject in m_stGroupDataForDivide)
            //                                    {
            //                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[eachObject.nDivideIndex].nRegion_ObjectTotalNum++;
            //                                    }

            //                                    //  Divided 영역 별 데이터 공간 메모리 할당.
            //                                    for (int nIndex = 0; nIndex < m_nGroupIndex_TotalX * m_nGroupIndex_TotalY; nIndex++)
            //                                    {
            //                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[nIndex].m_stDividedRegion_ObjectData =
            //                                            new WorkStage.stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[nIndex].nRegion_ObjectTotalNum];
            //                                    }

            //                                    //  Divided 영역 별 Center 좌표 계산
            //                                    for (int m_Y = 0; m_Y < m_nGroupIndex_TotalY; m_Y++)
            //                                    {
            //                                        for (int m_X = 0; m_X < m_nGroupIndex_TotalX; m_X++)
            //                                        {
            //                                            //m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[(m_Y * m_nGroupIndex_TotalY) + m_X].dRegionCenter.X = m_dGroupStartPos_X + (m_dDrilling_FOV * (double)m_X) + (m_dDrilling_FOV / 2.0);
            //                                            //m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[(m_Y * m_nGroupIndex_TotalY) + m_X].dRegionCenter.Y = m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)m_Y) - (m_dDrilling_FOV / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[(m_Y * m_nGroupIndex_TotalX) + m_X].dRegionCenter.X = m_dGroupStartPos_X + (m_dDrilling_FOV * (double)m_X) + (m_dDrilling_FOV / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[(m_Y * m_nGroupIndex_TotalX) + m_X].dRegionCenter.Y = m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)m_Y) - (m_dDrilling_FOV / 2.0);
            //                                        }
            //                                    }

            //                                    //  Divided 영역 개수
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[0].nRegion_Num = m_nGroupIndex_TotalX * m_nGroupIndex_TotalY;
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[0].nRegion_Num_X = m_nGroupIndex_TotalX;
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[0].nRegion_Num_Y = m_nGroupIndex_TotalY;

            //                                    //  Group Center 좌표
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].dGroupCenter.X = (double)group.Location.X;
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].dGroupCenter.Y = (double)group.Location.Y;

            //                                    //  카운트 변수 초기화
            //                                    m_nGroupData_Count = 0;
            //                                    m_nLineData_Count_forDrillHole = 0;
            //                                    for (int i = 0; i < 4; i++)
            //                                    {
            //                                        m_stDrillHole_byLine[i].ptStart.X = 0.0;
            //                                        m_stDrillHole_byLine[i].ptStart.Y = 0.0;
            //                                        m_stDrillHole_byLine[i].ptEnd.X = 0.0;
            //                                        m_stDrillHole_byLine[i].ptEnd.Y = 0.0;
            //                                    }

            //                                    //  세부 데이터 저장
            //                                    foreach (var subEntity in group)
            //                                    {
            //                                        //m_stGroupDataForDivide[m_nGroupData_Count].bAssigned = false;         //  안씀

            //                                        //  도형 Center 좌표 가져오기
            //                                        Type t = subEntity.GetType();
            //                                        if (t.Name == "LwPolyline")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                            //  객체 Type
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                            //  객체 Edge 좌표 개수
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                            //  객체 분할영역 Index
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                            //  객체 Edge 좌표 데이터 저장
            //                                            for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                            {
            //                                                m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;

            //                                                m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                            }

            //                                            //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                            if (pl.IsClosed)
            //                                            {
            //                                                m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;

            //                                                m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                            }

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                m_ptFrom.Y = (double)m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                            m_ptLast.Y = m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                            //  가공 데이터 길이 누적
            //                                            for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                            {
            //                                                m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                                m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                                m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                                m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  분할 영역 객체 개수 +1
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                        }
            //                                        else if (t.Name == "Circle")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                            //  객체 Type
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                            //  객체 Edge 좌표 개수
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                            //  객체 분할영역 Index
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                            //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                            //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)pl.Center.X;
            //                                                m_ptFrom.Y = (double)pl.Center.Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = (double)pl.Center.X;
            //                                            m_ptLast.Y = (double)pl.Center.Y;

            //                                            //  가공 데이터 길이 누적
            //                                            m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                            //  분할 영역 객체 개수 +1
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                        }
            //                                        else if (t.Name == "Rectangle")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                            //  객체 Type
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                            //  객체 Edge 좌표 개수
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                            //  객체 분할영역 Index
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                            //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                            //  Jump 데이터 길이 누적
            //                                            if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                            {
            //                                                m_ptFrom.X = (double)m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                m_ptFrom.Y = (double)m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                    m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                    nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                m_ptTo.X = (double)m_ptLast.X;
            //                                                m_ptTo.Y = (double)m_ptLast.Y;

            //                                                if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                }
            //                                                else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                {
            //                                                    m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                }
            //                                            }

            //                                            //  마지막 좌표 위치 저장
            //                                            m_ptLast.X = m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                            m_ptLast.Y = m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                            //  가공 데이터 길이 누적
            //                                            m_dTotal_DrillingDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                            //  분할 영역 객체 개수 +1
            //                                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                        }
            //                                        else if (t.Name == "Line")
            //                                        {
            //                                            var pl = subEntity as SpiralLab.Sirius.Line;

            //                                            //  Drilling Hole 에서 Line 데이터가 들어오는 경우, Line 4개를 묶어서 1개의 POLYLINE 으로 한다. 
            //                                            //  (Drilling Hole 데이터가 Polyline 일 때도 있고, Line 일 때도 있다. 엿장수 맘대로...)

            //                                            switch (m_nLineData_Count_forDrillHole)
            //                                            {
            //                                                case 0:                 //  1번째 라인
            //                                                case 1:                 //  2번째 라인
            //                                                case 2:                 //  3번째 라인
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.X = (double)pl.Start.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.Y = (double)pl.Start.Y;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.X = (double)pl.End.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.Y = (double)pl.End.Y;
            //                                                    m_nLineData_Count_forDrillHole++;

            //                                                    break;


            //                                                case 3:                 //  4번째 라인 --> 여기까지의 데이터로 사각형 만들기.
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.X = (double)pl.Start.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptStart.Y = (double)pl.Start.Y;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.X = (double)pl.End.X;
            //                                                    m_stDrillHole_byLine[m_nLineData_Count_forDrillHole].ptEnd.Y = (double)pl.End.Y;
            //                                                    m_nLineData_Count_forDrillHole++;

            //                                                    //  Min, Max 좌표
            //                                                    m_dLine_Min_X = double.MaxValue;
            //                                                    m_dLine_Max_X = double.MinValue;
            //                                                    m_dLine_Min_Y = double.MaxValue;
            //                                                    m_dLine_Max_Y = double.MinValue;

            //                                                    for (int i = 0; i < 4; i++)
            //                                                    {
            //                                                        if (m_stDrillHole_byLine[i].ptStart.X >= m_dLine_Max_X) m_dLine_Max_X = m_stDrillHole_byLine[i].ptStart.X;
            //                                                        if (m_stDrillHole_byLine[i].ptStart.X <= m_dLine_Min_X) m_dLine_Min_X = m_stDrillHole_byLine[i].ptStart.X;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.X <= m_dLine_Min_X) m_dLine_Min_X = m_stDrillHole_byLine[i].ptEnd.X;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.X >= m_dLine_Max_X) m_dLine_Max_X = m_stDrillHole_byLine[i].ptEnd.X;

            //                                                        if (m_stDrillHole_byLine[i].ptStart.Y >= m_dLine_Max_Y) m_dLine_Max_Y = m_stDrillHole_byLine[i].ptStart.Y;
            //                                                        if (m_stDrillHole_byLine[i].ptStart.Y <= m_dLine_Min_Y) m_dLine_Min_Y = m_stDrillHole_byLine[i].ptStart.Y;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.Y >= m_dLine_Max_Y) m_dLine_Max_Y = m_stDrillHole_byLine[i].ptEnd.Y;
            //                                                        if (m_stDrillHole_byLine[i].ptEnd.Y <= m_dLine_Min_Y) m_dLine_Min_Y = m_stDrillHole_byLine[i].ptEnd.Y;
            //                                                    }

            //                                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  사각형은 좌표 개수가 5개 (시작 위치에서 다시 시작 위치로 와야 함)

            //                                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  사각형은 좌표 개수가 5개 (시작 위치에서 다시 시작 위치로 와야 함)

            //                                                    //  객체 Type
            //                                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                                    //  객체 Edge 좌표 개수
            //                                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].nEdgePointNum = 5;               //  사각형은 좌표 개수가 5개.

            //                                                    //  객체 분할영역 Index
            //                                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;


            //                                                    ////  기울어진 사각형에는 사용할 수 없는 코드
            //                                                    //{
            //                                                    //    //  객체 Edge 좌표 데이터 저장 (LT --> LB --> RB --> RT --> LT)
            //                                                    //    //  Left Top
            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = m_dLine_Min_X;

            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = m_dLine_Max_Y;

            //                                                    //    //  Left Bottom
            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = m_dLine_Min_X;

            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = m_dLine_Min_Y;

            //                                                    //    //  Right Bottom
            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].X = m_dLine_Max_X;

            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].Y = m_dLine_Min_Y;

            //                                                    //    //  Right Top
            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].X = m_dLine_Max_X;

            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].Y = m_dLine_Max_Y;

            //                                                    //    //  Left Top
            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].X = m_dLine_Min_X;

            //                                                    //    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                    //        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                    //        nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].Y = m_dLine_Max_Y;
            //                                                    //}

            //                                                    //  라인이 순서대로 되어 있다는 가정 하에 사용할 수 있는 코드
            //                                                    {
            //                                                        //  객체 Edge 좌표 데이터 저장 (LT --> LB --> RB --> RT --> LT)
            //                                                        //  Left Top
            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = m_stDrillHole_byLine[0].ptStart.X;

            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = m_stDrillHole_byLine[0].ptStart.Y;

            //                                                        //  Left Bottom
            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = m_stDrillHole_byLine[1].ptStart.X;

            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = m_stDrillHole_byLine[1].ptStart.Y;

            //                                                        //  Right Bottom
            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].X = m_stDrillHole_byLine[2].ptStart.X;

            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].Y = m_stDrillHole_byLine[2].ptStart.Y;

            //                                                        //  Right Top
            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].X = m_stDrillHole_byLine[3].ptStart.X;

            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].Y = m_stDrillHole_byLine[3].ptStart.Y;

            //                                                        //  Left Top
            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].X = m_stDrillHole_byLine[0].ptStart.X;

            //                                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].Y = m_stDrillHole_byLine[0].ptStart.Y;
            //                                                    }

            //                                                    //  Jump 데이터 길이 누적
            //                                                    if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                                    {
            //                                                        m_ptFrom.X = (double)m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                        m_ptFrom.Y = (double)m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                                        m_ptTo.X = (double)m_ptLast.X;
            //                                                        m_ptTo.Y = (double)m_ptLast.Y;

            //                                                        if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                                        {
            //                                                            m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                                        }
            //                                                        else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                                        {
            //                                                            m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                                        }
            //                                                        else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                                        {
            //                                                            m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                                        }
            //                                                    }

            //                                                    //  마지막 좌표 위치 저장
            //                                                    m_ptLast.X = m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                                    m_ptLast.Y = m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                                                m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                                                nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                                    //  가공 데이터 길이 누적
            //                                                    m_dTotal_DrillingDataLength += ((m_dLine_Max_X - m_dLine_Min_X) * 2.0) + ((m_dLine_Max_Y - m_dLine_Min_Y) * 2.0);

            //                                                    //  분할 영역 객체 개수 +1
            //                                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;

            //                                                    break;
            //                                            }
            //                                        }
            //                                        else        //  또 뭐가 있나...
            //                                        {

            //                                        }

            //                                        if (t.Name != "Line")
            //                                        {
            //                                            m_nGroupData_Count++;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (m_bDrillingData_isLine && (m_nLineData_Count_forDrillHole == 4))
            //                                            {
            //                                                m_nLineData_Count_forDrillHole = 0;
            //                                                m_nGroupData_Count++;
            //                                            }
            //                                        }
            //                                    }

            //                                    m_nGroupData_Count = 0;
            //                                    int a = 0;
            //                                }

            //                                //success &= group.Mark(markerArg);
            //                                break;
            //                                // case EType....
            //                                // ...

            //                                //default:
            //                                //    if (entity is IMarkerable markerable)
            //                                //    {
            //                                //        // mark entity
            //                                //        // 해당 개체(Entity) 가공 
            //                                //        //success &= markerable.Mark(markerArg);
            //                                //    }
            //                                //    break;
            //                        }

            //                        if (!success)
            //                            break;

            //                        CreateProcessDrillingVectors();
            //                        m_nGroupCount++;
            //                        m_stDrilling_LayerData.nRegion_GroupCount++;


            //                        //foreach (var v in m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount - 1].m_stDrilling_ObjectData)
            //                        //{
            //                        //    Debug.WriteLine("x:" + v.dEdgePoint.Min(tt => tt.X).ToString() + ", Y:" + v.dEdgePoint.Min(tt => tt.Y).ToString());
            //                        //}

            //                        //  데이터 정렬
            //                        if (Config.ParamConfig.Drilling_DataSort_Use)
            //                        {
            //                            SortFastPath(ref m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount - 1], Config.ParamConfig.Drilling_DataSortDir_HorVer, Config.ParamConfig.Drilling_DataSortStep_Size);
            //                        }

            //                        //foreach (var v in m_stDrilling_LayerData.m_stDrilling_GroupData[m_stDrilling_LayerData.nRegion_GroupCount - 1].m_stDrilling_ObjectData)
            //                        //{
            //                        //    Debug.WriteLine("x:" + v.dEdgePoint.Min(tt => tt.X).ToString() + ", Y:" + v.dEdgePoint.Min(tt => tt.Y).ToString());
            //                        //}
            //                    }
            //                }

            //                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                //
            //                //  [드릴링] [syncAxis] [StageAndScanner]
            //                //
            //                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                ///
            //                else                                                //  MotionType.StageAndScanner
            //                {
            //                    foreach (var entity in layer)
            //                    {
            //                        switch (entity.EntityType)
            //                        {
            //                            case EType.Point:
            //                                var point = entity as SpiralLab.Sirius.Point;
            //                                //point.Location 
            //                                //point.DwellTime
            //                                //success &= point.Mark(markerArg);
            //                                break;

            //                            case EType.Points:
            //                                var points = entity as Points;
            //                                foreach (var vertex in points)
            //                                {
            //                                    //vertex.X
            //                                    //vertex.Y
            //                                }
            //                                //points.DwellTime
            //                                //success &= points.Mark(markerArg);
            //                                break;

            //                            case EType.Line:
            //                                var line = entity as SpiralLab.Sirius.Line;

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End

            //                                //  객체 Type
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                                //  객체 Edge 좌표 개수
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                                //  객체 Edge 좌표 데이터 저장
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X = (double)line.Start.X;
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y = (double)line.Start.Y;
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].X = (double)line.End.X;
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].Y = (double)line.End.Y;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)line.Start.X;
            //                                    m_ptFrom.Y = (double)line.Start.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)line.End.X;
            //                                m_ptLast.Y = (double)line.End.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_DrillingDataLength += (double)line.Length;

            //                                //  영역 객체 개수 +1
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;

            //                                //line.Start
            //                                //line.End
            //                                //success &= line.Mark(markerArg);
            //                                break;

            //                            case EType.Arc:
            //                                var arc = entity as SpiralLab.Sirius.Arc;

            //                                //  객체 Type
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                                //  객체 Radius
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dRadius = (double)arc.Radius;

            //                                //  객체 Center 좌표
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dCenter.X = (double)arc.Center.X;
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dCenter.Y = (double)arc.Center.Y;

            //                                //  객체 Start Angle
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dStartAngle = (double)arc.StartAngle;

            //                                //  객체 Sweep Angle
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dSweepAngle = (double)arc.SweepAngle;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)arc.Center.X;
            //                                    m_ptFrom.Y = (double)arc.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)arc.Center.X;
            //                                m_ptLast.Y = (double)arc.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                if ((double)arc.SweepAngle > 0.0)
            //                                    m_dTotal_DrillingDataLength += (double)arc.Radius * 2.0 * Math.PI * ((double)arc.SweepAngle / 360.0);

            //                                //  영역 객체 개수 +1
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;

            //                                //arc.Radius
            //                                //arc.Center
            //                                //arc.StartAngle
            //                                //arc.SweepAngle
            //                                //success &= arc.Mark(markerArg);
            //                                break;

            //                            case EType.Circle:
            //                                var circle = entity as SpiralLab.Sirius.Circle;

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                //  객체 Type
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                //  객체 Edge 좌표 개수
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X = (double)circle.Center.X;
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y = (double)circle.Center.Y;

            //                                //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].X = (double)circle.Radius;
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].Y = (double)circle.Radius;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)circle.Center.X;
            //                                    m_ptFrom.Y = (double)circle.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)circle.Center.X;
            //                                m_ptLast.Y = (double)circle.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_DrillingDataLength += (double)circle.Radius * 2.0 * Math.PI;

            //                                //  영역 객체 개수 +1
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;

            //                                //circle.Center 
            //                                //circle.Radius
            //                                //success &= circle.Mark(markerArg);
            //                                break;

            //                            case EType.Rectangle:
            //                                var rectangle = entity as SpiralLab.Sirius.Rectangle;

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                //  객체 Type
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                //  객체 Edge 좌표 개수
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[2].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[2].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_DrillingDataLength += ((double)rectangle.Width * 2.0) + ((double)rectangle.Height * 2.0);

            //                                //  영역 객체 개수 +1
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;

            //                                //rectangle.Width
            //                                //rectangle.Height
            //                                //rectangle.Align
            //                                //rectangle.Location
            //                                //success &= rectangle.Mark(markerArg);
            //                                break;

            //                            case EType.LWPolyline:
            //                                var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                                //lwPolyline.IsClosed

            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                //  객체 Type
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                //  객체 Edge 좌표 개수
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count;

            //                                //  객체 Edge 좌표 데이터 저장
            //                                for (int n_pl = 0; n_pl < lwPolyline.Count; n_pl++)
            //                                {
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)lwPolyline.Items[n_pl].X;
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)lwPolyline.Items[n_pl].Y;
            //                                }

            //                                //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                if (lwPolyline.IsClosed)
            //                                {
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].X = (double)lwPolyline.Items[0].X;
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].Y = (double)lwPolyline.Items[0].Y;
            //                                }

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                for (int n_pl = 0; n_pl < lwPolyline.Count - 1; n_pl++)
            //                                {
            //                                    m_ptFrom.X = (double)lwPolyline.Items[n_pl].X;
            //                                    m_ptFrom.Y = (double)lwPolyline.Items[n_pl].Y;
            //                                    m_ptTo.X = (double)lwPolyline.Items[n_pl + 1].X;
            //                                    m_ptTo.Y = (double)lwPolyline.Items[n_pl + 1].Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_DrillingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  영역 객체 개수 +1
            //                                m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;

            //                                //foreach (var vertex in lwPolyline)
            //                                //{
            //                                //    //vertex.X
            //                                //    //vertex.Y
            //                                //    //vertex.Bulge
            //                                //}
            //                                //success &= lwPolyline.Mark(markerArg);
            //                                break;

            //                            case EType.Spiral:
            //                                var spiral = entity as Spiral;
            //                                //spiral.OutterDiameter 
            //                                //spiral.InnerDiameter
            //                                //spiral.RadialPitch
            //                                //spiral.Revolutions
            //                                //spiral.Center
            //                                //success &= spiral.Mark(markerArg);
            //                                break;

            //                            case EType.Group:
            //                            default:
            //                                var group = entity as Group;

            //                                //m_stDrilling_LayerData = new WorkStage.stDrilling_LayerData();                                                                    --> 앞에서 할당

            //                                ////  전체 Object 개수
            //                                //m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectTotalNum = group.Count;                                                --> 앞에서 할당

            //                                ////  Object 별 데이터 공간 메모리 할당
            //                                //m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData = new WorkStage.stDrilling_ObjectData[group.Count];      --> 앞에서 할당

            //                                //  세부 데이터 저장
            //                                m_nGroupData_Count = 0;
            //                                foreach (var subEntity in group)
            //                                {
            //                                    m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_nGroupData_Count].bAssigned = false;

            //                                    Type t = subEntity.GetType();
            //                                    if (t.Name == "LwPolyline")
            //                                    {
            //                                        var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                        //  객체 Type
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                        //  객체 Edge 좌표 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                        //  객체 Edge 좌표 데이터 저장
            //                                        for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                        {
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                        }

            //                                        //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                        if (pl.IsClosed)
            //                                        {
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;
            //                                            m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                        }

            //                                        //  Jump 데이터 길이 누적
            //                                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                        {
            //                                            m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                            m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                            m_ptTo.X = (double)m_ptLast.X;
            //                                            m_ptTo.Y = (double)m_ptLast.Y;

            //                                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                            }
            //                                        }

            //                                        //  마지막 좌표 위치 저장
            //                                        m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                        m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                        //  가공 데이터 길이 누적
            //                                        for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                        {
            //                                            m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                            m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                            m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                            m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                            {
            //                                                m_dTotal_DrillingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                            }
            //                                        }

            //                                        //  영역 객체 개수 +1
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;
            //                                    }
            //                                    else if (t.Name == "Circle")
            //                                    {
            //                                        var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                        //  객체 Type
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                        //  객체 Edge 좌표 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                        //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                        //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                        //  Jump 데이터 길이 누적
            //                                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                        {
            //                                            m_ptFrom.X = (double)pl.Center.X;
            //                                            m_ptFrom.Y = (double)pl.Center.Y;
            //                                            m_ptTo.X = (double)m_ptLast.X;
            //                                            m_ptTo.Y = (double)m_ptLast.Y;

            //                                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                            }
            //                                        }

            //                                        //  마지막 좌표 위치 저장
            //                                        m_ptLast.X = (double)pl.Center.X;
            //                                        m_ptLast.Y = (double)pl.Center.Y;

            //                                        //  가공 데이터 길이 누적
            //                                        m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                        //  영역 객체 개수 +1
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;
            //                                    }
            //                                    else if (t.Name == "Rectangle")
            //                                    {
            //                                        var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                        //  객체 Type
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                        //  객체 Edge 좌표 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                        //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                        //  Jump 데이터 길이 누적
            //                                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                        {
            //                                            m_ptFrom.X = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                            m_ptFrom.Y = (double)m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                            m_ptTo.X = (double)m_ptLast.X;
            //                                            m_ptTo.Y = (double)m_ptLast.Y;

            //                                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                            }
            //                                        }

            //                                        //  마지막 좌표 위치 저장
            //                                        m_ptLast.X = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X;
            //                                        m_ptLast.Y = m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                        //  가공 데이터 길이 누적
            //                                        m_dTotal_DrillingDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                        //  영역 객체 개수 +1
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;
            //                                    }
            //                                    else if (t.Name == "Line")
            //                                    {
            //                                        var pl = subEntity as SpiralLab.Sirius.Line;

            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Start      1 : End

            //                                        //  객체 Type
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                                        //  객체 Edge 좌표 개수
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                                        //  객체 Edge 좌표 데이터 저장
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Start.X;
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Start.Y;
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.End.X;
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.End.Y;

            //                                        //  Jump 데이터 길이 누적
            //                                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                        {
            //                                            m_ptFrom.X = (double)pl.Start.X;
            //                                            m_ptFrom.Y = (double)pl.Start.Y;
            //                                            m_ptTo.X = (double)m_ptLast.X;
            //                                            m_ptTo.Y = (double)m_ptLast.Y;

            //                                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                            }
            //                                        }

            //                                        //  마지막 좌표 위치 저장
            //                                        m_ptLast.X = (double)pl.End.X;
            //                                        m_ptLast.Y = (double)pl.End.Y;

            //                                        //  가공 데이터 길이 누적
            //                                        m_dTotal_DrillingDataLength += (double)pl.Length;

            //                                        //  영역 객체 개수 +1
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;
            //                                    }
            //                                    else if (t.Name == "Arc")
            //                                    {
            //                                        var pl = subEntity as SpiralLab.Sirius.Arc;

            //                                        //  객체 Type
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                                        //  객체 Radius
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dRadius = (double)pl.Radius;

            //                                        //  객체 Center 좌표
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dCenter.X = (double)pl.Center.X;
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dCenter.Y = (double)pl.Center.Y;

            //                                        //  객체 Start Angle
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dStartAngle = (double)pl.StartAngle;

            //                                        //  객체 Sweep Angle
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].m_stDrilling_ObjectData[m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount].stArcData.dSweepAngle = (double)pl.SweepAngle;

            //                                        //  Jump 데이터 길이 누적
            //                                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                        {
            //                                            m_ptFrom.X = (double)pl.Center.X;
            //                                            m_ptFrom.Y = (double)pl.Center.Y;
            //                                            m_ptTo.X = (double)m_ptLast.X;
            //                                            m_ptTo.Y = (double)m_ptLast.Y;

            //                                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                            }
            //                                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                            {
            //                                                m_dTotal_DrillingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                            }
            //                                        }

            //                                        //  마지막 좌표 위치 저장
            //                                        m_ptLast.X = (double)pl.Center.X;
            //                                        m_ptLast.Y = (double)pl.Center.Y;

            //                                        //  가공 데이터 길이 누적
            //                                        if ((double)pl.SweepAngle > 0.0)
            //                                            m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI * ((double)pl.SweepAngle / 360.0);

            //                                        //  영역 객체 개수 +1
            //                                        m_stDrilling_LayerData.m_stDrilling_GroupData[0].nRegion_ObjectCount++;
            //                                    }
            //                                    else        //  또 뭐가 있나...
            //                                    {

            //                                    }
            //                                }

            //                                //success &= group.Mark(markerArg);
            //                                break;
            //                                // case EType....
            //                                // ...

            //                                //default:
            //                                //    if (entity is IMarkerable markerable)
            //                                //    {
            //                                //        // mark entity
            //                                //        // 해당 개체(Entity) 가공 
            //                                //        //success &= markerable.Mark(markerArg);
            //                                //    }
            //                                //    break;
            //                        }
            //                        if (!success)
            //                            break;
            //                    }
            //                }
            //            }

            //            ///////////////////////
            //            ///////////////////////
            //            ////               ////
            //            ////  [RTC6 모드]  ////
            //            ////               ////
            //            ///////////////////////
            //            ///////////////////////
            //            ///
            //            else if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_RTC6)             //  RTC6 모드
            //            {
            //                //  Group 개수
            //                foreach (var entity in layer)
            //                {
            //                    m_nGroupCount++;
            //                }
            //                m_stDividedRegion_GroupData = new WorkStage.stDividedRegion_GroupData[m_nGroupCount];
            //                m_stDividedRegion_GroupData[0].nGroup_Num = m_nGroupCount;

            //                m_nGroupCount = 0;
            //                // iterate entities in layer
            //                // 레이어 내의 개체(Entity)들을 순회
            //                foreach (var entity in layer)
            //                {
            //                    switch (entity.EntityType)
            //                    {
            //                        case EType.Point:
            //                            var point = entity as SpiralLab.Sirius.Point;
            //                            //point.Location 
            //                            //point.DwellTime
            //                            //success &= point.Mark(markerArg);
            //                            break;

            //                        case EType.Points:
            //                            var points = entity as Points;
            //                            foreach (var vertex in points)
            //                            {
            //                                //vertex.X
            //                                //vertex.Y
            //                            }
            //                            //points.DwellTime
            //                            //success &= points.Mark(markerArg);
            //                            break;

            //                        case EType.Line:
            //                            var line = entity as SpiralLab.Sirius.Line;
            //                            //line.Start
            //                            //line.End
            //                            //success &= line.Mark(markerArg);
            //                            break;

            //                        case EType.Arc:
            //                            var arc = entity as SpiralLab.Sirius.Arc;
            //                            //arc.Radius
            //                            //arc.Center
            //                            //arc.StartAngle
            //                            //arc.SweepAngle
            //                            //success &= arc.Mark(markerArg);
            //                            break;

            //                        case EType.Circle:
            //                            var circle = entity as SpiralLab.Sirius.Circle;
            //                            //circle.Center 
            //                            //circle.Radius
            //                            //success &= circle.Mark(markerArg);
            //                            break;

            //                        case EType.Rectangle:
            //                            var rectangle = entity as SpiralLab.Sirius.Rectangle;
            //                            //rectangle.Width
            //                            //rectangle.Height
            //                            //rectangle.Align
            //                            //rectangle.Location
            //                            //success &= rectangle.Mark(markerArg);
            //                            break;

            //                        case EType.LWPolyline:
            //                            var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                            //lwPolyline.IsClosed
            //                            foreach (var vertex in lwPolyline)
            //                            {
            //                                //vertex.X
            //                                //vertex.Y
            //                                //vertex.Bulge
            //                            }
            //                            //success &= lwPolyline.Mark(markerArg);
            //                            break;

            //                        case EType.Spiral:
            //                            var spiral = entity as Spiral;
            //                            //spiral.OutterDiameter 
            //                            //spiral.InnerDiameter
            //                            //spiral.RadialPitch
            //                            //spiral.Revolutions
            //                            //spiral.Center
            //                            //success &= spiral.Mark(markerArg);
            //                            break;

            //                        case EType.Group:
            //                        default:
            //                            var group = entity as Group;

            //                            //  전체 영역 크기 체크
            //                            m_dGroupSize_Width = (double)group.Width;
            //                            m_dGroupSize_Height = (double)group.Height;

            //                            //  전체 도형 개수
            //                            m_nGroupData_TotalCount = group.Count;

            //                            //  전체 Index 계산
            //                            m_nGroupIndex_TotalX = (int)(m_dGroupSize_Width / m_dDrilling_FOV);
            //                            if ((m_dGroupSize_Width % m_dDrilling_FOV) > 0.0)
            //                                m_nGroupIndex_TotalX++;

            //                            m_nGroupIndex_TotalY = (int)(m_dGroupSize_Height / m_dDrilling_FOV);
            //                            if ((m_dGroupSize_Height % m_dDrilling_FOV) > 0.0)
            //                                m_nGroupIndex_TotalY++;

            //                            //  전체 영역 시작 위치 (2사분면에서 시작)

            //                            //           │
            //                            //   2사분면 │ 1사분면
            //                            //           │
            //                            // ───────────
            //                            //           │
            //                            //   3사분면 │ 4사분면
            //                            //           │

            //                            m_dGroupStartPos_X = (double)group.Location.X - (((double)m_nGroupIndex_TotalX * m_dDrilling_FOV) / 2.0);
            //                            m_dGroupStartPos_Y = (double)group.Location.Y + (((double)m_nGroupIndex_TotalY * m_dDrilling_FOV) / 2.0);

            //                            m_stGroupDataForDivide = new stGroupDataForDivide[m_nGroupData_TotalCount];

            //                            //  Index 계산
            //                            foreach (var subEntity in group)
            //                            {
            //                                m_stGroupDataForDivide[m_nGroupData_Count].bAssigned = false;

            //                                //  도형 Center 좌표 가져오기
            //                                Type t = subEntity.GetType();
            //                                if (t.Name == "LwPolyline")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)pl.Location.X;
            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)pl.Location.Y;
            //                                }
            //                                else if (t.Name == "Circle")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)pl.Center.X;
            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)pl.Center.Y;
            //                                }
            //                                else if (t.Name == "Rectangle")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)pl.Location.X;
            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)pl.Location.Y;
            //                                }
            //                                else if (t.Name == "Line")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.Line;

            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X = (double)(pl.Start.X + pl.End.X) / 2.0;
            //                                    m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y = (double)(pl.Start.Y + pl.End.Y) / 2.0;
            //                                }
            //                                else        //  또 뭐가 있나...
            //                                {

            //                                }

            //                                //  각 도형이 Divided 영역의 몇 번째 Index 에 들어가는지 계산
            //                                for (int x = 0; x < m_nGroupIndex_TotalX; x++)
            //                                {
            //                                    if ((m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X >= (m_dGroupStartPos_X + (m_dDrilling_FOV * (double)x))) &&
            //                                        (m_stGroupDataForDivide[m_nGroupData_Count].dCenter.X < (m_dGroupStartPos_X + (m_dDrilling_FOV * (double)x) + m_dDrilling_FOV)))
            //                                    {
            //                                        m_nDivCount_X = x;
            //                                        x = m_nGroupIndex_TotalX;
            //                                    }
            //                                }
            //                                for (int y = 0; y < m_nGroupIndex_TotalY; y++)
            //                                {
            //                                    if ((m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y <= (m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)y))) &&
            //                                        (m_stGroupDataForDivide[m_nGroupData_Count].dCenter.Y > (m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)y) - m_dDrilling_FOV)))
            //                                    {
            //                                        m_nDivCount_Y = y;
            //                                        y = m_nGroupIndex_TotalY;
            //                                    }
            //                                }

            //                                m_stGroupDataForDivide[m_nGroupData_Count++].nDivideIndex = (m_nDivCount_Y * m_nGroupIndex_TotalX) + m_nDivCount_X;
            //                            }

            //                            //  Divided 영역별 데이터 개수 카운트
            //                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData = new WorkStage.stDividedRegion_RegionData[m_nGroupIndex_TotalX * m_nGroupIndex_TotalY];
            //                            foreach (var eachObject in m_stGroupDataForDivide)
            //                            {
            //                                m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[eachObject.nDivideIndex].nRegion_ObjectTotalNum++;
            //                            }

            //                            //  Divided 영역 별 데이터 공간 메모리 할당.
            //                            for (int nIndex = 0; nIndex < m_nGroupIndex_TotalX * m_nGroupIndex_TotalY; nIndex++)
            //                            {
            //                                m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[nIndex].m_stDividedRegion_ObjectData =
            //                                    new WorkStage.stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[nIndex].nRegion_ObjectTotalNum];
            //                            }

            //                            //  Divided 영역 별 Center 좌표 계산
            //                            for (int m_Y = 0; m_Y < m_nGroupIndex_TotalY; m_Y++)
            //                            {
            //                                for (int m_X = 0; m_X < m_nGroupIndex_TotalX; m_X++)
            //                                {
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[(m_Y * m_nGroupIndex_TotalX) + m_X].dRegionCenter.X = m_dGroupStartPos_X + (m_dDrilling_FOV * (double)m_X) + (m_dDrilling_FOV / 2.0);
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[(m_Y * m_nGroupIndex_TotalX) + m_X].dRegionCenter.Y = m_dGroupStartPos_Y - (m_dDrilling_FOV * (double)m_Y) - (m_dDrilling_FOV / 2.0);
            //                                }
            //                            }

            //                            //  Divided 영역 개수
            //                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[0].nRegion_Num = m_nGroupIndex_TotalX * m_nGroupIndex_TotalY;
            //                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[0].nRegion_Num_X = m_nGroupIndex_TotalX;
            //                            m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[0].nRegion_Num_Y = m_nGroupIndex_TotalY;

            //                            //  Group Center 좌표
            //                            m_stDividedRegion_GroupData[m_nGroupCount].dGroupCenter.X = (double)group.Location.X;
            //                            m_stDividedRegion_GroupData[m_nGroupCount].dGroupCenter.Y = (double)group.Location.Y;

            //                            //  세부 데이터 저장
            //                            m_nGroupData_Count = 0;
            //                            foreach (var subEntity in group)
            //                            {
            //                                m_stGroupDataForDivide[m_nGroupData_Count].bAssigned = false;

            //                                //  도형 Center 좌표 가져오기
            //                                Type t = subEntity.GetType();
            //                                if (t.Name == "LwPolyline")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                    //  객체 Type
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                    //  객체 Edge 좌표 개수
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                    //  객체 분할영역 Index
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                    //  객체 Edge 좌표 데이터 저장
            //                                    for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                    {
            //                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;

            //                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                    }

            //                                    //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                    if (pl.IsClosed)
            //                                    {
            //                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;

            //                                        m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                            m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                    }

            //                                    //  가공 데이터 길이 누적
            //                                    for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                    {
            //                                        m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                        m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                        m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                        m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                        if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                        {
            //                                            m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                        }
            //                                        else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                        {
            //                                            m_dTotal_DrillingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                        }
            //                                        else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                        {
            //                                            m_dTotal_DrillingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                        }
            //                                    }

            //                                    //  분할 영역 객체 개수 +1
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                }
            //                                else if (t.Name == "Circle")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                    //  객체 Type
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                    //  객체 Edge 좌표 개수
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nEdgePointNum = 1;

            //                                    //  객체 분할영역 Index
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                    //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                    //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                    //  가공 데이터 길이 누적
            //                                    m_dTotal_DrillingDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                    //  분할 영역 객체 개수 +1
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                }
            //                                else if (t.Name == "Rectangle")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                    //  객체 Type
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                    //  객체 Edge 좌표 개수
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nEdgePointNum = 5;

            //                                    //  객체 분할영역 Index
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                    //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - (pl.Width / 2.0);
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + (pl.Height / 2.0);

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + (pl.Width / 2.0);
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + (pl.Height / 2.0);

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + (pl.Width / 2.0);
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - (pl.Height / 2.0);

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - (pl.Width / 2.0);
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - (pl.Height / 2.0);

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - (pl.Width / 2.0);
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + (pl.Height / 2.0);

            //                                    //  가공 데이터 길이 누적
            //                                    m_dTotal_DrillingDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                    //  분할 영역 객체 개수 +1
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                }
            //                                else if (t.Name == "Line")
            //                                {
            //                                    var pl = subEntity as SpiralLab.Sirius.Line;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Start      1 : End

            //                                    //  객체 Type
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                                    //  객체 Edge 좌표 개수
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                                    //  객체 분할영역 Index
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                            nDivideIndex].nRegion_ObjectCount].nDivideIndex = m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex;

            //                                    //  객체 Edge 좌표 데이터 저장
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Start.X;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Start.Y;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.End.X;

            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].
            //                                        m_stDividedRegion_ObjectData[m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].
            //                                        nDivideIndex].nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.End.Y;

            //                                    //  가공 데이터 길이 누적
            //                                    m_dTotal_DrillingDataLength += (double)pl.Length;

            //                                    //  분할 영역 객체 개수 +1
            //                                    m_stDividedRegion_GroupData[m_nGroupCount].m_stDividedRegion_RegionData[m_stGroupDataForDivide[m_nGroupData_Count].nDivideIndex].nRegion_ObjectCount++;
            //                                }
            //                                else        //  또 뭐가 있나...
            //                                {

            //                                }

            //                                m_nGroupData_Count++;
            //                            }

            //                            m_nGroupData_Count = 0;
            //                            int a = 0;

            //                            //success &= group.Mark(markerArg);
            //                            break;
            //                            // case EType....
            //                            // ...

            //                            //default:
            //                            //    if (entity is IMarkerable markerable)
            //                            //    {
            //                            //        // mark entity
            //                            //        // 해당 개체(Entity) 가공 
            //                            //        //success &= markerable.Mark(markerArg);
            //                            //    }
            //                            //    break;
            //                    }
            //                    if (!success)
            //                        break;
            //                    CreateProcessDrillingVectors();
            //                    m_nGroupCount++;
            //                }
            //            }

            //            m_nLayerCount++;
            //        }

            //        ///////////////////////
            //        ///                 ///
            //        ///      마킹       ///
            //        ///                 ///
            //        ///////////////////////
            //        else if (layer.Name == "마킹")
            //        {
            //            if (Equipment.RtcMode_syncAxis != (int)Equipment.RtcMode.RTC_SYNCAXIS)
            //            {
            //                return (int)nGetDataResult.GETDATA_FAIL;
            //            }

            //            m_ptLast.X = 0.0;
            //            m_ptLast.Y = 0.0;

            //            m_stLayerType.m_nLayerType[m_nLayerCount] = (int)LayerType.LAYER_MARKING;

            //            if (layer.MotionType == MotionType.StageAndScanner)
            //            {
            //                int a = 0;
            //            }
            //            else if (layer.MotionType == MotionType.StageOnly)
            //            {
            //                int b = 0;
            //            }
            //            else if (layer.MotionType == MotionType.ScannerOnly)
            //            {
            //                int c = 0;
            //            }

            //            //  Item 이 Group 인지 아닌지 확인 (Group 이면 아래에서 데이터 변수 할당, Group 이 아니면 여기서 할당)
            //            int m_nCount = 0;
            //            foreach (var entity in layer)
            //            {
            //                var group = entity as Group;

            //                if (group == null)
            //                {
            //                    LayerIsGroup = false;

            //                    m_nCount = layer.Count;
            //                }
            //                else
            //                {
            //                    LayerIsGroup = true;

            //                    m_nCount = 1;
            //                }

            //                break;
            //            }

            //            //if ((Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_SYNCAXIS) && (m_nCount == 1))
            //            //{
            //            //    return 2;
            //            //}


            //            if (!LayerIsGroup || (m_nCount > 1))
            //            {
            //                m_stMarking_LayerData = new WorkStage.stMarking_LayerData();

            //                //  전체 Object 개수
            //                m_stMarking_LayerData.nRegion_ObjectTotalNum = layer.Count;

            //                //  Object 별 데이터 공간 메모리 할당
            //                m_stMarking_LayerData.m_stMarking_ObjectData = new WorkStage.stMarking_ObjectData[layer.Count];

            //                //  Marking 데이터 개수
            //                m_nMarkingData_Count = m_stMarking_LayerData.nRegion_ObjectTotalNum;
            //            }

            //            //  세부 데이터 저장
            //            m_nGroupData_Count = 0;
            //            foreach (var entity in layer)
            //            {
            //                switch (entity.EntityType)
            //                {
            //                    case EType.Point:
            //                        var point = entity as SpiralLab.Sirius.Point;
            //                        //point.Location 
            //                        //point.DwellTime
            //                        //success &= point.Mark(markerArg);
            //                        break;

            //                    case EType.Points:
            //                        var points = entity as Points;
            //                        foreach (var vertex in points)
            //                        {
            //                            //vertex.X
            //                            //vertex.Y
            //                        }
            //                        //points.DwellTime
            //                        //success &= points.Mark(markerArg);
            //                        break;

            //                    case EType.Line:
            //                        var line = entity as SpiralLab.Sirius.Line;

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                        //  객체 Edge 좌표 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                        //  객체 Edge 좌표 데이터 저장
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X = (double)line.Start.X;
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y = (double)line.Start.Y;
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].X = (double)line.End.X;
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].Y = (double)line.End.Y;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)line.Start.X;
            //                            m_ptFrom.Y = (double)line.Start.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)line.End.X;
            //                        m_ptLast.Y = (double)line.End.Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_MarkingDataLength += (double)line.Length;

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;

            //                        //line.Start
            //                        //line.End
            //                        //success &= line.Mark(markerArg);
            //                        break;

            //                    case EType.Arc:
            //                        var arc = entity as SpiralLab.Sirius.Arc;

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                        //  객체 Radius
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dRadius = (double)arc.Radius;

            //                        //  객체 Center 좌표
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dCenter.X = (double)arc.Center.X;
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dCenter.Y = (double)arc.Center.Y;

            //                        //  객체 Start Angle
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dStartAngle = (double)arc.StartAngle;

            //                        //  객체 Sweep Angle
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dSweepAngle = (double)arc.SweepAngle;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)arc.Center.X;
            //                            m_ptFrom.Y = (double)arc.Center.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)arc.Center.X;
            //                        m_ptLast.Y = (double)arc.Center.Y;

            //                        //  가공 데이터 길이 누적
            //                        if ((double)arc.SweepAngle > 0.0)
            //                            m_dTotal_MarkingDataLength += (double)arc.Radius * 2.0 * Math.PI * ((double)arc.SweepAngle / 360.0);

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;

            //                        //arc.Radius
            //                        //arc.Center
            //                        //arc.StartAngle
            //                        //arc.SweepAngle
            //                        //success &= arc.Mark(markerArg);
            //                        break;

            //                    case EType.Circle:
            //                        var circle = entity as SpiralLab.Sirius.Circle;

            //                        //if ( circle.Description != null)
            //                        //{
            //                        //    MessageBox.Show(circle.Description);
            //                        //}

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                        //  객체 Edge 좌표 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = 1;

            //                        //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X = (double)circle.Center.X;
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y = (double)circle.Center.Y;

            //                        //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].X = (double)circle.Radius;
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].Y = (double)circle.Radius;

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)circle.Center.X;
            //                            m_ptFrom.Y = (double)circle.Center.Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = (double)circle.Center.X;
            //                        m_ptLast.Y = (double)circle.Center.Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_MarkingDataLength += (double)circle.Radius * 2.0 * Math.PI;

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;
            //                        break;

            //                    case EType.Rectangle:
            //                        var rectangle = entity as SpiralLab.Sirius.Rectangle;

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                        //  객체 Edge 좌표 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = 5;

            //                        //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[2].X = (double)rectangle.Center.X + ((double)rectangle.Width / 2.0);
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[2].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[3].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[3].Y = (double)rectangle.Center.Y - ((double)rectangle.Height / 2.0);

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[4].X = (double)rectangle.Center.X - ((double)rectangle.Width / 2.0);
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[4].Y = (double)rectangle.Center.Y + ((double)rectangle.Height / 2.0);

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                            m_ptFrom.Y = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                        m_ptLast.Y = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;

            //                        //  가공 데이터 길이 누적
            //                        m_dTotal_MarkingDataLength += ((double)rectangle.Width * 2.0) + ((double)rectangle.Height * 2.0);

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;

            //                        //rectangle.Width
            //                        //rectangle.Height
            //                        //rectangle.Align
            //                        //rectangle.Location
            //                        //success &= rectangle.Mark(markerArg);
            //                        break;

            //                    case EType.LWPolyline:
            //                        var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                        //lwPolyline.IsClosed

            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                        //  객체 Edge 좌표 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = lwPolyline.IsClosed ? lwPolyline.Count + 1 : lwPolyline.Count;

            //                        //  객체 Edge 좌표 데이터 저장
            //                        for (int n_pl = 0; n_pl < lwPolyline.Count; n_pl++)
            //                        {
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)lwPolyline.Items[n_pl].X;
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)lwPolyline.Items[n_pl].Y;
            //                        }

            //                        //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                        if (lwPolyline.IsClosed)
            //                        {
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].X = (double)lwPolyline.Items[0].X;
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[lwPolyline.Count].Y = (double)lwPolyline.Items[0].Y;
            //                        }

            //                        //  Jump 데이터 길이 누적
            //                        if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                        {
            //                            m_ptFrom.X = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                            m_ptFrom.Y = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;
            //                            m_ptTo.X = (double)m_ptLast.X;
            //                            m_ptTo.Y = (double)m_ptLast.Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  마지막 좌표 위치 저장
            //                        m_ptLast.X = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                        m_ptLast.Y = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;

            //                        //  가공 데이터 길이 누적
            //                        for (int n_pl = 0; n_pl < lwPolyline.Count - 1; n_pl++)
            //                        {
            //                            m_ptFrom.X = (double)lwPolyline.Items[n_pl].X;
            //                            m_ptFrom.Y = (double)lwPolyline.Items[n_pl].Y;
            //                            m_ptTo.X = (double)lwPolyline.Items[n_pl + 1].X;
            //                            m_ptTo.Y = (double)lwPolyline.Items[n_pl + 1].Y;

            //                            if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                            {
            //                                m_dTotal_MarkingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                            }
            //                            else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                            {
            //                                m_dTotal_MarkingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                            }
            //                        }

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;

            //                        //foreach (var vertex in lwPolyline)
            //                        //{
            //                        //    //vertex.X
            //                        //    //vertex.Y
            //                        //    //vertex.Bulge
            //                        //}
            //                        //success &= lwPolyline.Mark(markerArg);
            //                        break;

            //                    case EType.Spiral:
            //                        var spiral = entity as Spiral;
            //                        //spiral.OutterDiameter 
            //                        //spiral.InnerDiameter
            //                        //spiral.RadialPitch
            //                        //spiral.Revolutions
            //                        //spiral.Center
            //                        //success &= spiral.Mark(markerArg);
            //                        break;

            //                    case EType.Text:                                                        //  True Type Font, Hatch (외곽선 있는 텍스트, 내부 Hatch 는 선택)
            //                        var text = entity as SpiralLab.Sirius.Text;                         //  외곽선은 Polyline, Hatch 는 Line 으로 구성.

            //                        var listText = text.ToOutlineGlyph();

            //                        //  Sirius-Text 와는 다르게, 모든 Text 가 LWPolyline 으로 구성되어 있다.

            //                        //////////////////////
            //                        ///
            //                        /// 글자 외곽선 데이터
            //                        ///
            //                        //////////////////////
            //                        ///
            //                        //  글자 개수 카운트
            //                        m_nTextCount = 0;

            //                        //  글자를 구성하는 요소 개수 카운트
            //                        m_nTextItemCount = 0;

            //                        foreach (var subEntity in listText)
            //                        {
            //                            Type t = subEntity.GetType();

            //                            if (t.Name == "Group")              //  Entity 이름이 "Group" 인 것만 Text 데이터다. (Point 도 있는데, 이건 실제 text 아님)
            //                            {
            //                                m_nTextCount++;
            //                            }
            //                        }

            //                        //  글자 개수만큼 메모리 할당

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_TEXT;

            //                        //  글자 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nTextNum = m_nTextCount;

            //                        //  글자 데이터 메모리 할당
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData = new stMarking_DetailedTextData[m_nTextCount];

            //                        m_nTextCount = 0;

            //                        //  글자별 구성 데이터 넣기
            //                        foreach (var subEntity in listText)
            //                        {
            //                            Type t = subEntity.GetType();

            //                            if (t.Name == "Group")              //  Entity 이름이 "Group" 인 것만 Text 데이터다. (Point 도 있는데, 이건 실제 text 아님)
            //                            {
            //                                var TextGroup = subEntity as Group;

            //                                //  글자 구성요소 개수 확인
            //                                m_nTextItemCount = 0;                               //  글자 구성요소 카운트
            //                                foreach (var subTextEntity in TextGroup)
            //                                {
            //                                    Type t2 = subTextEntity.GetType();

            //                                    if (t2.Name == "LwPolyline")                    //  TrueType-Text 에서 글자는 모두 Polyline 이다. (Hatch 가 들어갈 수 있는 영역으로 구성된 Text 이기 때문에)
            //                                    {
            //                                        m_nTextItemCount++;
            //                                    }
            //                                }

            //                                //  글자 구성요소 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].nTextElementNum = m_nTextItemCount;

            //                                //  글자 구성요소 개수만큼 메모리 할당
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement = new stMarking_TextElement[m_nTextItemCount];

            //                                //  데이터 넣기
            //                                m_nTextItemCount = 0;                       //  글자 구성요소 카운트
            //                                foreach (var subTextEntity in TextGroup)
            //                                {
            //                                    Type t3 = subTextEntity.GetType();

            //                                    if (t3.Name == "LwPolyline")
            //                                    {
            //                                        var pl = subTextEntity as SpiralLab.Sirius.LwPolyline;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].nElementType = (int)ObjectType.OBJECT_POLY;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolylineNum = pl.IsClosed == true ? pl.Count + 1 : pl.Count;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline =
            //                                            new PointD[m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolylineNum];

            //                                        //  Polyline 데이터 넣기
            //                                        //  객체 Edge 좌표 데이터 저장
            //                                        for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                        {
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[n_pl].X = (double)pl.Items[n_pl].X;
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                        }

            //                                        //  닫힌 도형이면, 첫번째 데이터를 마지막 데이터에 추가한다.
            //                                        if (pl.IsClosed)
            //                                        {
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[pl.Count].X = (double)pl.Items[0].X;
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[pl.Count].Y = (double)pl.Items[0].Y;
            //                                        }

            //                                        //  글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Line")
            //                                    {
            //                                        var pl = subTextEntity as SpiralLab.Sirius.Line;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].nElementType = (int)ObjectType.OBJECT_LINE;

            //                                        //  Line 데이터 넣기
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptStart.X = pl.Start.X;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptStart.Y = pl.Start.Y;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptEnd.X = pl.End.X;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptEnd.Y = pl.End.Y;

            //                                        //  글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Arc")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Arc 가 있으면...)

            //                                        //  글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Circle")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Circle 이있으면...)

            //                                        //  글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Rectangle")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Rectangle 이 있으면...)

            //                                        //  글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                }

            //                                //  글자 객체 개수 +1
            //                                m_nTextCount++;
            //                            }
            //                        }


            //                        //  Hatch 가 있을 수 있으니, 영역 개수 증가는 마지막에...
            //                        ////  영역 객체 개수 +1
            //                        //m_stMarking_LayerData.nRegion_ObjectCount++;



            //                        //////////////////////
            //                        ///
            //                        /// 글자 Hatch 데이터
            //                        ///
            //                        //////////////////////
            //                        ///

            //                        bool m_bHatchDataExist = false;

            //                        SpiralLab.Sirius.Group listHatch = text.ToHatchGlyph();

            //                        //  Hatch 로 구성된 글자 개수 카운트
            //                        m_nTextCount = 0;

            //                        //  한 글자를 구성하는 Hatch 라인의 개수 카운트
            //                        m_nTextItemCount = 0;

            //                        foreach (var subEntity in listHatch)
            //                        {
            //                            Type t = subEntity.GetType();

            //                            if (t.Name == "Group")              //  Entity 이름이 "Group" 인 것만 Text 데이터다. (Point 도 있는데, 이건 실제 text 아님)
            //                            {
            //                                var TextGroup = subEntity as Group;

            //                                if (TextGroup.Count == 0)
            //                                {
            //                                    m_bHatchDataExist = false;
            //                                    //break;
            //                                }
            //                                else
            //                                {
            //                                    m_bHatchDataExist = true;
            //                                    m_nTextCount++;
            //                                }
            //                            }
            //                        }

            //                        if (m_bHatchDataExist == false)
            //                        {
            //                            //  영역 객체 개수 +1 (영역 객체 개수 증가 코드가 마지막에 있지만, Hatch 를 하지 않을 경우 여기에서 빠져나가기 때문에...)
            //                            m_stMarking_LayerData.nRegion_ObjectCount++;
            //                            break;
            //                        }

            //                        //  Hatch 글자 개수만큼 메모리 할당

            //                        //  Hatch 글자 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nHatchNum = m_nTextCount;

            //                        //  Hatch 글자 데이터 메모리 할당
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData = new stMarking_DetailedTextData[m_nTextCount];

            //                        m_nTextCount = 0;

            //                        //  Hatch 글자별 구성 데이터 넣기
            //                        foreach (var subEntity in listHatch)
            //                        {
            //                            Type t = subEntity.GetType();

            //                            if (t.Name == "Group")              //  Entity 이름이 "Group" 인 것만 Text 데이터다. (Point 도 있는데, 이건 실제 text 아님)
            //                            {
            //                                var TextGroup = subEntity as Group;

            //                                //  Hatch 글자 구성요소 개수 확인
            //                                m_nTextItemCount = 0;                               //  글자 구성요소 카운트
            //                                foreach (var subTextEntity in TextGroup)
            //                                {
            //                                    Type t2 = subTextEntity.GetType();

            //                                    if (t2.Name == "Line")                    //  TrueType-Text 에서 Hatch 는 모두 Line 이다.
            //                                    {
            //                                        m_nTextItemCount++;
            //                                    }
            //                                }

            //                                //  Hatch 글자 구성요소 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].nTextElementNum = m_nTextItemCount;

            //                                //  Hatch 글자 구성요소 개수만큼 메모리 할당
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement = new stMarking_TextElement[m_nTextItemCount];

            //                                //  데이터 넣기
            //                                m_nTextItemCount = 0;                       //  글자 구성요소 카운트
            //                                foreach (var subTextEntity in TextGroup)
            //                                {
            //                                    Type t3 = subTextEntity.GetType();

            //                                    if (t3.Name == "LwPolyline")
            //                                    {
            //                                        var pl = subTextEntity as SpiralLab.Sirius.LwPolyline;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].nElementType = (int)ObjectType.OBJECT_POLY;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolylineNum = pl.IsClosed == true ? pl.Count + 1 : pl.Count;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline =
            //                                            new PointD[m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolylineNum];

            //                                        //  Polyline 데이터 넣기
            //                                        //  객체 Edge 좌표 데이터 저장
            //                                        for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                        {
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[n_pl].X = (double)pl.Items[n_pl].X;
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                        }

            //                                        //  닫힌 도형이면, 첫번째 데이터를 마지막 데이터에 추가한다.
            //                                        if (pl.IsClosed)
            //                                        {
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[pl.Count].X = (double)pl.Items[0].X;
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[pl.Count].Y = (double)pl.Items[0].Y;
            //                                        }

            //                                        //  Hatch 글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Line")
            //                                    {
            //                                        var pl = subTextEntity as SpiralLab.Sirius.Line;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].nElementType = (int)ObjectType.OBJECT_LINE;

            //                                        //  Line 데이터 넣기
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptStart.X = pl.Start.X;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptStart.Y = pl.Start.Y;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptEnd.X = pl.End.X;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stHatchData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptEnd.Y = pl.End.Y;

            //                                        //  Hatch 글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Arc")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Arc 가 있으면...)

            //                                        //  Hatch 글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Circle")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Circle 이있으면...)

            //                                        //  Hatch 글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                    else if (t3.Name == "Rectangle")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Rectangle 이 있으면...)

            //                                        //  Hatch 글자 구성요소 개수 +1
            //                                        m_nTextItemCount++;
            //                                    }
            //                                }

            //                                //  Hatch 글자 객체 개수 +1
            //                                m_nTextCount++;
            //                            }
            //                        }

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;
            //                        break;

            //                    case EType.SiriusText:                                                  //  Sirius Text (뼈다귀)
            //                        var sirius_text = entity as SpiralLab.Sirius.SiriusText;

            //                        var list = sirius_text.ToOutlineGlyph();

            //                        //  글자 개수 카운트
            //                        m_nTextCount = 0;

            //                        //  글자를 구성하는 요소 개수 카운트
            //                        m_nTextItemCount = 0;

            //                        foreach (var subEntity in list)
            //                        {
            //                            Type t = subEntity.GetType();

            //                            if (t.Name == "Group")              //  Entity 이름이 "Group" 인 것만 Text 데이터다. (Point 도 있는데, 이건 실제 text 아님)
            //                            {
            //                                m_nTextCount++;
            //                            }
            //                        }

            //                        //  글자 개수만큼 메모리 할당

            //                        //  객체 Type
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_SIRIUS_TEXT;

            //                        //  글자 개수
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nTextNum = m_nTextCount;

            //                        //  글자 데이터 메모리 할당
            //                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData = new stMarking_DetailedTextData[m_nTextCount];

            //                        //  글자별 구성 데이터 종류 초기화
            //                        for (int i = 0; i < m_nTextCount; i++)
            //                        {
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[i].nLineNum = 0;
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[i].nPolyLineNum = 0;
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[i].nArcNum = 0;
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[i].nCircleNum = 0;
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[i].nRectNum = 0;
            //                        }

            //                        m_nTextCount = 0;

            //                        //  글자별 구성 데이터 넣기
            //                        foreach (var subEntity in list)
            //                        {
            //                            Type t = subEntity.GetType();

            //                            if (t.Name == "Group")              //  Entity 이름이 "Group" 인 것만 Text 데이터다. (Point 도 있는데, 이건 실제 text 아님)
            //                            {
            //                                var TextGroup = subEntity as Group;

            //                                //  글자 구성요소 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].nTextElementNum = TextGroup.Count;

            //                                //  글자 구성요소 개수만큼 메모리 할당
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement = new stMarking_TextElement[TextGroup.Count];

            //                                m_nTextItemCount = 0;                       //  글자 구성요소 카운트

            //                                foreach (var subTextEntity in TextGroup)
            //                                {
            //                                    Type t2 = subTextEntity.GetType();

            //                                    if (t2.Name == "LwPolyline")
            //                                    {
            //                                        var pl = subTextEntity as SpiralLab.Sirius.LwPolyline;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].nElementType = (int)ObjectType.OBJECT_POLY;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolylineNum = pl.IsClosed == true ? pl.Count + 1 : pl.Count;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline =
            //                                            new PointD[m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolylineNum];

            //                                        //  Polyline 데이터 넣기
            //                                        //  객체 Edge 좌표 데이터 저장
            //                                        for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                        {
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[n_pl].X = (double)pl.Items[n_pl].X;
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                        }

            //                                        //  닫힌 도형이면, 첫번째 데이터를 마지막 데이터에 추가한다.
            //                                        if (pl.IsClosed)
            //                                        {
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[pl.Count].X = (double)pl.Items[0].X;
            //                                            m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elPolyline[pl.Count].Y = (double)pl.Items[0].Y;
            //                                        }
            //                                    }
            //                                    else if (t2.Name == "Line")
            //                                    {
            //                                        var pl = subTextEntity as SpiralLab.Sirius.Line;

            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].nElementType = (int)ObjectType.OBJECT_LINE;

            //                                        //  Line 데이터 넣기
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptStart.X = pl.Start.X;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptStart.Y = pl.Start.Y;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptEnd.X = pl.End.X;
            //                                        m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stTextData[m_nTextCount].stTextElement[m_nTextItemCount].elLine.ptEnd.Y = pl.End.Y;
            //                                    }
            //                                    else if (t2.Name == "Arc")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Arc 가 있으면...)
            //                                    }
            //                                    else if (t2.Name == "Circle")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Circle 이있으면...)
            //                                    }
            //                                    else if (t2.Name == "Rectangle")
            //                                    {
            //                                        //  나중에 추가하자. (Text 를 구성하는 요소 중에 Rectangle 이 있으면...)
            //                                    }

            //                                    //  글자 구성요소 개수 +1
            //                                    m_nTextItemCount++;
            //                                }

            //                                //  글자 객체 개수 +1
            //                                m_nTextCount++;
            //                            }
            //                        }

            //                        //  영역 객체 개수 +1
            //                        m_stMarking_LayerData.nRegion_ObjectCount++;

            //                        int a1 = 0;

            //                        break;

            //                    case EType.Group:
            //                    default:
            //                        var group = entity as Group;

            //                        m_stMarking_LayerData = new WorkStage.stMarking_LayerData();

            //                        //  전체 Object 개수
            //                        m_stMarking_LayerData.nRegion_ObjectTotalNum = group.Count;

            //                        //  Object 별 데이터 공간 메모리 할당
            //                        m_stMarking_LayerData.m_stMarking_ObjectData = new WorkStage.stMarking_ObjectData[group.Count];

            //                        //  세부 데이터 저장
            //                        m_nGroupData_Count = 0;
            //                        foreach (var subEntity in group)
            //                        {
            //                            m_stMarking_LayerData.m_stMarking_ObjectData[m_nGroupData_Count].bAssigned = false;

            //                            Type t = subEntity.GetType();
            //                            if (t.Name == "LwPolyline")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.LwPolyline;

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)
            //                                //m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[pl.IsClosed ? pl.Count + 1 : pl.Count];       //  모든 Edge Point 좌표 (닫힌 도형이면 좌표 1개 더 추가)

            //                                //  객체 Type
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_POLY;

            //                                //  객체 Edge 좌표 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = pl.IsClosed ? pl.Count + 1 : pl.Count;

            //                                //  객체 Edge 좌표 데이터 저장
            //                                for (int n_pl = 0; n_pl < pl.Count; n_pl++)
            //                                {
            //                                    m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[n_pl].X = (double)pl.Items[n_pl].X;
            //                                    m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[n_pl].Y = (double)pl.Items[n_pl].Y;
            //                                }

            //                                //  닫힌 도형일 경우, 시작 좌표 한번 더 추가)
            //                                if (pl.IsClosed)
            //                                {
            //                                    m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[pl.Count].X = (double)pl.Items[0].X;
            //                                    m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[pl.Count].Y = (double)pl.Items[0].Y;
            //                                }

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                for (int n_pl = 0; n_pl < pl.Count - 1; n_pl++)
            //                                {
            //                                    m_ptFrom.X = (double)pl.Items[n_pl].X;
            //                                    m_ptFrom.Y = (double)pl.Items[n_pl].Y;
            //                                    m_ptTo.X = (double)pl.Items[n_pl + 1].X;
            //                                    m_ptTo.Y = (double)pl.Items[n_pl + 1].Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingDataLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingDataLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_MarkingDataLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  영역 객체 개수 +1
            //                                m_stMarking_LayerData.nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Circle")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Circle;

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Center 좌표      1 : Radius 값
            //                                //m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Center 좌표      1 : Radius 값

            //                                //  객체 Type
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_CIR;

            //                                //  객체 Edge 좌표 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = 1;

            //                                //  객체 Edge 좌표 데이터 저장 (Circle Center, Circle 은 1개 고정)
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X;
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y;

            //                                //  Circle 의 경우, 두 번째 데이터는 Radius 값
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Radius;
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Radius;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Center.X;
            //                                    m_ptFrom.Y = (double)pl.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.Center.X;
            //                                m_ptLast.Y = (double)pl.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_MarkingDataLength += (double)pl.Radius * 2.0 * Math.PI;

            //                                //  영역 객체 개수 +1
            //                                m_stMarking_LayerData.nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Rectangle")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Rectangle;

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)
            //                                //m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[5];       //  순서대로 (0 -> 1 -> 2 -> 3 -> 4 -> 0 해야 닫힌 도형이 됨)

            //                                //  객체 Type
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_RECT;

            //                                //  객체 Edge 좌표 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = 5;

            //                                //  객체 Edge 좌표 데이터 저장 (Rectangle 은 4개 고정)
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[2].X = (double)pl.Center.X + ((double)pl.Width / 2.0);
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[2].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[3].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[3].Y = (double)pl.Center.Y - ((double)pl.Height / 2.0);

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[4].X = (double)pl.Center.X - ((double)pl.Width / 2.0);
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[4].Y = (double)pl.Center.Y + ((double)pl.Height / 2.0);

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                                    m_ptFrom.Y = (double)m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X;
            //                                m_ptLast.Y = m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_MarkingDataLength += ((double)pl.Width * 2.0) + ((double)pl.Height * 2.0);

            //                                //  영역 객체 개수 +1
            //                                m_stMarking_LayerData.nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Line")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Line;

            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint = new PointD[2];       //  0 : Start      1 : End
            //                                //m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint_PreDrilling = new PointD[2];       //  0 : Start      1 : End

            //                                //  객체 Type
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_LINE;

            //                                //  객체 Edge 좌표 개수
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nEdgePointNum = 2;               //  Line 데이터는 시작점과 끝 점 2개.

            //                                //  객체 Edge 좌표 데이터 저장
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].X = (double)pl.Start.X;
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[0].Y = (double)pl.Start.Y;
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].X = (double)pl.End.X;
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].dEdgePoint[1].Y = (double)pl.End.Y;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Start.X;
            //                                    m_ptFrom.Y = (double)pl.Start.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.End.X;
            //                                m_ptLast.Y = (double)pl.End.Y;

            //                                //  가공 데이터 길이 누적
            //                                m_dTotal_MarkingDataLength += (double)pl.Length;

            //                                //  영역 객체 개수 +1
            //                                m_stMarking_LayerData.nRegion_ObjectCount++;
            //                            }
            //                            else if (t.Name == "Arc")
            //                            {
            //                                var pl = subEntity as SpiralLab.Sirius.Arc;

            //                                //  객체 Type
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].nObjectType = (int)WorkStage.ObjectType.OBJECT_ARC;

            //                                //  객체 Radius
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dRadius = (double)pl.Radius;

            //                                //  객체 Center 좌표
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dCenter.X = (double)pl.Center.X;
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dCenter.Y = (double)pl.Center.Y;

            //                                //  객체 Start Angle
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dStartAngle = (double)pl.StartAngle;

            //                                //  객체 Sweep Angle
            //                                m_stMarking_LayerData.m_stMarking_ObjectData[m_stMarking_LayerData.nRegion_ObjectCount].stArcData.dSweepAngle = (double)pl.SweepAngle;

            //                                //  Jump 데이터 길이 누적
            //                                if ((m_ptLast.X != 0.0) || (m_ptLast.Y != 0.0))
            //                                {
            //                                    m_ptFrom.X = (double)pl.Center.X;
            //                                    m_ptFrom.Y = (double)pl.Center.Y;
            //                                    m_ptTo.X = (double)m_ptLast.X;
            //                                    m_ptTo.Y = (double)m_ptLast.Y;

            //                                    if ((m_ptFrom.X == m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))                   //  X 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.Y - m_ptTo.Y);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y == m_ptTo.Y))              //  Y 축과 평행인 경우
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Abs(m_ptFrom.X - m_ptTo.X);
            //                                    }
            //                                    else if ((m_ptFrom.X != m_ptTo.X) && (m_ptFrom.Y != m_ptTo.Y))              //  대각선
            //                                    {
            //                                        m_dTotal_MarkingJumpLength += Math.Sqrt(Math.Pow(Math.Abs(m_ptFrom.X - m_ptTo.X), 2) + Math.Pow(Math.Abs(m_ptFrom.Y - m_ptTo.Y), 2));
            //                                    }
            //                                }

            //                                //  마지막 좌표 위치 저장
            //                                m_ptLast.X = (double)pl.Center.X;
            //                                m_ptLast.Y = (double)pl.Center.Y;

            //                                //  가공 데이터 길이 누적
            //                                if ((double)pl.SweepAngle > 0.0)
            //                                    m_dTotal_MarkingDataLength += (double)pl.Radius * 2.0 * Math.PI * ((double)pl.SweepAngle / 360.0);

            //                                //  영역 객체 개수 +1
            //                                m_stMarking_LayerData.nRegion_ObjectCount++;
            //                            }
            //                            else        //  또 뭐가 있나...
            //                            {

            //                            }
            //                        }

            //                        //success &= group.Mark(markerArg);
            //                        break;
            //                        // case EType....
            //                        // ...

            //                        //default:
            //                        //    if (entity is IMarkerable markerable)
            //                        //    {
            //                        //        // mark entity
            //                        //        // 해당 개체(Entity) 가공 
            //                        //        //success &= markerable.Mark(markerArg);
            //                        //    }
            //                        //    break;
            //                }
            //                if (!success)
            //                    break;
            //            }

            //            m_nLayerCount++;
            //        }
            //        else
            //        {
            //            m_nUnusableLayerCount++;
            //        }
            //    }
            //    if (!success)
            //        break;
            //}

            //if (m_nUnusableLayerCount > 0)
            //    return (int)nGetDataResult.GETDATA_LAYERNAME_NG;       //  "Layer Name 은 '쓰루홀', '외곽선', '드릴링', '마킹' 4가지만 가능합니다."

            //Equipment.WorkTotalTime = 0;
            //Equipment.WorkTotalTime_Outline = 0;
            //Equipment.WorkTotalTime_Thruhole = 0;
            //Equipment.WorkTotalTime_Drilling = 0;
            //Equipment.WorkTotalTime_Marking = 0;

            //Equipment.WorkElapsedTick_Outline_1time = 0;
            //Equipment.WorkElapsedTick_Thruhole_1time = 0;
            //Equipment.WorkElapsedTick_Drilling_1time = 0;
            //Equipment.WorkElapsedTick_Marking_1time = 0;


            ////  Outline Jump, 가공 이동 시간
            //if (m_dTotal_OutlineJumpLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Outline += (m_dTotal_OutlineJumpLength / Config.ParamConfig.Outline_Jump_Speed) * (Config.ParamConfig.Outline_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Outline_Repeat_Count);
            //}
            //if (m_dTotal_OutlineDataLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Outline += (m_dTotal_OutlineDataLength / Config.ParamConfig.Outline_Mark_Speed) * (Config.ParamConfig.Outline_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Outline_Repeat_Count);
            //}
            ////  Outline 추가시간
            //Equipment.WorkTotalTime_Outline += Equipment.WorkTotalTime_Outline_AdditionalTime / 1000.0;

            ////  Thruhole Jump, 가공 이동 시간
            //if (m_dTotal_ThruholeJumpLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Thruhole += (m_dTotal_ThruholeJumpLength / Config.ParamConfig.Thruhole_Jump_Speed) * (Config.ParamConfig.Thruhole_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Thruhole_Repeat_Count);
            //}
            //if (m_dTotal_ThruholeDataLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Thruhole += (m_dTotal_ThruholeDataLength / Config.ParamConfig.Thruhole_Mark_Speed) * (Config.ParamConfig.Thruhole_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Thruhole_Repeat_Count);
            //}
            ////  Thruhole 추가시간
            //Equipment.WorkTotalTime_Thruhole += Equipment.WorkTotalTime_Thruhole_AdditionalTime / 1000.0;

            ////  Drilling Jump, 가공 이동 시간
            //if (m_dTotal_DrillingJumpLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Drilling += (m_dTotal_DrillingJumpLength / Config.ParamConfig.Drilling_Jump_Speed) * (Config.ParamConfig.Drilling_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Drilling_Repeat_Count);
            //}
            //if (m_dTotal_DrillingDataLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Drilling += (m_dTotal_DrillingDataLength / Config.ParamConfig.Drilling_Mark_Speed) * (Config.ParamConfig.Drilling_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Drilling_Repeat_Count);
            //}
            ////  Drilling 추가시간
            //Equipment.WorkTotalTime_Drilling += Equipment.WorkTotalTime_Drilling_AdditionalTime / 1000.0;

            ////  Pre-Drilling 모드를 사용할 경우 (길이가 본 Drilling 길이보다 작지만, 다른 part 에서 소요되는 시간이 있어서...)
            //if (Config.ParamConfig.bPreDrilling_Use)
            //{
            //    if (m_dTotal_DrillingJumpLength > 0.0)
            //    {
            //        Equipment.WorkTotalTime_Drilling += (m_dTotal_DrillingJumpLength / Config.ParamConfig.PreDrilling_Jump_Speed) * (Config.ParamConfig.PreDrilling_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.PreDrilling_Repeat_Count);
            //    }
            //    if (m_dTotal_DrillingDataLength > 0.0)
            //    {
            //        Equipment.WorkTotalTime_Drilling += (m_dTotal_DrillingDataLength / Config.ParamConfig.PreDrilling_Mark_Speed) * (Config.ParamConfig.PreDrilling_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.PreDrilling_Repeat_Count);
            //    }
            //}

            ////  Marking Jump, 가공 이동 시간
            //if (m_dTotal_MarkingJumpLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Marking += (m_dTotal_MarkingJumpLength / Config.ParamConfig.Marking_Jump_Speed) * (Config.ParamConfig.Marking_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Marking_Repeat_Count);
            //}
            //if (m_dTotal_MarkingDataLength > 0.0)
            //{
            //    Equipment.WorkTotalTime_Marking += (m_dTotal_MarkingDataLength / Config.ParamConfig.Marking_Mark_Speed) * (Config.ParamConfig.Marking_Repeat_Count == 0 ? 1.0 : Config.ParamConfig.Marking_Repeat_Count);
            //}

            //Equipment.WorkTotalTime = Equipment.WorkTotalTime_Outline + Equipment.WorkTotalTime_Thruhole + Equipment.WorkTotalTime_Drilling + Equipment.WorkTotalTime_Marking;

            return success == true ? (int)nGetDataResult.GETDATA_SUCCESS : (int)nGetDataResult.GETDATA_FAIL;            //   0 : "데이터가 정상적으로 로드 되었습니다."
                                                                                                                        //  -1 : "데이터가 정상적으로 로드 되지 않았습니다."
        }
    }
}