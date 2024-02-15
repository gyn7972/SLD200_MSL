using ACS.SPiiPlusNET;
using netDxf.Entities;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.VisionPart;
using QMC.Core;
using QMC.Common.Laser;
using SerialCommChiller;
using SerialCommLaserPowerMeter1;
using SerialCommLaserPowerMeter2;
using SerialCommSpectraPhysics;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using QMC.Process.WaferProbeAlign.Parts;
using static QMC.Common.Modules.WaferProbeAlign;
using QMC.Common.Vision.EureSys;
using static QMC.Common.Parts.WaferProbeAlignParameter;
using System.Diagnostics.Eventing.Reader;

namespace QMC.Common.Modules
{
    [Serializable]
    public class WaferProbeAlign : Module
    {
        #region Define

        public enum nAxis
        {
            //X = 0,
            //Y,
            //IZ,
            //SZ,
            //T,
            //RVA_X,
            //RVA_Z,

            U = 0,
            V,
            W,
            EZ,
            X,
            Y,
            VZ,
        }

        public enum nCameraType
        {
            Cam_Upper = 0,
            Cam_Lower,
        }

        public enum nProbeClampType
        {
            Type_A = 0,                         //  1호기
            Type_B,                             //  2 ~ 6호기
        }

        public enum nAlignErrorCheckPos
        {
            Pos_Top = 0,
            Pos_Mid,
            Pos_Bot,
        }

        public enum nEPulseType
        {
            Type_None = -1,
            Type_A = 0,
            Type_B,
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

        public enum nPegMode
        {
            PEG_FullNormal = 0,
            PEG_FullManual = 1,
            PEG_FullManualVisionLine = 2,
            PEG_NgChipLLO = 3,
            PEG_RandomPEG = 4,
        }

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
        public stAlignMark m_stAlignMark;

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
        /// Stage 위치 기준 가공
        /// </summary>
        ///
        public double ProcessOffsetX = 0;
        public double ProcessOffsetY = 0;
        public double ProcessPitchX = 0;
        public double ProcessPitchY = 0;
        public double ProcessBeamX = 0;
        public double ProcessBeamY = 0;

        public double ChipPitchX = 0;
        public double ChipPitchY = 0;

        public double FirstChipVisionPosX = 0.0;
        public double FirstChipVisionPosY = 0.0;

        private int CurrentRow = 0;

        private double Firstpoint_PEG;
        private double Increment_PEG;
        private double Lastpoint_PEG;
        private double StartPos_PEG;
        private double Endpos_PEG;
        private double ProcSpeed;

        //  PEG Trigger 영역을 분할하여 사용할 경우 다시 계산되는 위치값
        private double PEGDiv_Firstpoint_PEG;           //  PEG 시작 위치
        private double PEGDiv_Lastpoint_PEG;            //  PEG 종료 위치
        private double PEGDiv_StartPos_PEG;             //  Stage 시작 위치
        private double PEGDiv_EndPos_PEG;               //  Stage 종료 위치
        private double PEGDiv_ReCalc_Firstpoint_PEG;    //  다시 계산된 PEG 시작 위치
        private double PEGDiv_ReCalc_Lastpoint_PEG;     //  다시 계산된 PEG 종료 위치
        private double PEGDiv_ReCalc_StartPos_PEG;      //  다시 계산된 Stage 시작 위치
        private double PEGDiv_ReCalc_EndPos_PEG;        //  다시 계산된 Stage 종료 위치
        private int PEGDiv_DivArea_TotalCount;          //  몇개의 분할 영역으로 나누어 지는지
        private int PEGDiv_DivArea_Count;               //  몇번째 분할 영역인지
        private int PEGDiv_WorkChipCount_inDivArea;     //  분할 영역 안에 몇개의 Chip 이 있는지
        private bool PEGDiv_MoveDirection_LtoR;         //  가공 방향 (true: 왼쪽에서 오른쪽으로 이동하면서 가공, 스테이지는 오른쪽에서 왼쪽으로 이동)

        public enum LLO_Unit
        {
            LLOUnit_Block = 0,
            LLOUnit_Chip = 1,
        }
        public int Chip_LLO_Unit = 0;           //  2023. 06. 01.  SCH : 0 (Block 단위 가공), 1 (Chip 단위 가공)

        public enum Trigger_Mode
        {
            Trigger_Stage = 0,
            Trigger_Scanner = 1,
            Trigger_LCI = 2,
        }
        public int Chip_Trigger_Mode = 0;           //  2023. 05. 14.  SCH : 0 (Stage 기준 Trigger), 1 (Scanner 가공)

        public enum LLO_Mode
        {
            All_Chip = 0,
            NG_Chip_Spot = 1,
            NG_Chip_RandomPEG = 2,
        }
        public int Chip_LLO_Mode = 0;               //  2023. 02. 15.  SCH : 0 (전체 Chip LLO), 1 (NG Chip Only), 2 (NG Chip Random PEG)

        //  NG Chip LLO 를 위한 변수
        public bool m_bNgChip_Found;
        public int m_nNgChip_Index;
        public double m_dNgChipLLOPos_X;
        public double m_dNgChipLLOPos_Y;
        //public int m_nNgChipLLOPos_Interval_X;      //  X 방향 가공 Interval (몇 개 Chip or Block 마다 가공할 것인지)
        //public int m_nNgChipLLOPos_Interval_Y;      //  X 방향 가공 Interval (몇 개 Chip or Block 마다 가공할 것인지)
        //public int m_nNgChipLLOPos_Interval_X_Count;    //  X 방향 가공 Interval Count
        //public int m_nNgChipLLOPos_Interval_Y_Count;    //  X 방향 가공 Interval Count
        
        public enum LLO_Repetition
        {
            LLORepetition_Chip = 0,                 //  Chip 단위 반복
            LLORepetition_ScanArea = 1,             //  Scanner 영역 단위 반복
        }
        public int m_nNgChipLLO_Repetition;         //  가공 반복 회수
        public int m_nNgChipLLO_Repeat_Count;       //  현재 반복 회수

        private int Ext_Y_LineCnt = 0;

        private int PEG_Mode;

        public int PegTotalCountX;
        public int PegTotalCountY;
        public double PegMarginCount;

        //  PEG 시작 끝 포지션
        private double PegStartPositionX;
        private double PegEndPositionX;

        //  Stage 이동 포지션 (margin 포함)
        private double StageStartPositionX;
        private double StageEndPositionX;

        private double StageStartPositionY;
        private double StageEndPositionY;

        public Vector CurrentPosition = new Vector(0, 0);
        public Vector LastVisionPosition = new Vector(0, 0);
        public double LastVisionPosition_Z = 0.0;
        public double LastScannerPosition_Z = 0.0;

        public int m_nBlockChipMax_X = 0;
        public int m_nBlockChipMax_Y = 0;

        private double scannerOnDelayOffset;

        public int m_nNGChipLLO_Count = 0;                                      //  2023. 02. 14.  SCH : NG LLO Position 화면 List 의 진행상황 갱신을 위해

        public bool RandomPEG_Dir_LeftToRight = true;                           //  2023. 02. 18.  SCH : Random PEG 모드일 경우, Laser 가공 방향 (Toggle 방식으로 
        public int RandomPEG_Row_Count = 1;                                     //  2023. 02. 18.  SCH : Random PEG 모드일 경우, Wafer 가공 Row 회수
        public bool RandomPEG_RowLine_DataExist = false;                        //  2023. 02. 18.  SCH : Random PEG 모드일 경우, 가공해야 할 Row 에 데이터가 있는지 확인. (없으면 RandomPEG_Row_Count 증가)
        public double RandomPEG_RowPosition = 0;
        public int RandomPEG_RowPosition_BlockNum = 0;

        public bool m_Blink_SESAM_Show = false;
        public bool m_Blink_SESAM = false;

        public bool m_bRVA_CalibMode = false;           //  2023. 09. 28.  SCH : RVA 축 조정을 위해 다른 모든 축의 서보를 OFF 시켰는지 확인하는 Flag

        /// <summary>
        /// Scanner 이동 포지션 (Scanner Center 시작, 끝 위치)
        /// </summary>
        ///
        //  위치 계산 : Wafer XY 크기를 Scanner FOV XY 크기로 나누어 이동 영역 개수를 계산한다.
        public int ScannerTotalCountX;                  //  Scanner FOV 만큼 X 방향으로 몇 번 이동해야 하는 지
        public int ScannerTotalCountY;                  //  Scanner FOV 만큼 Y 방향으로 몇 번 이동해야 하는 지

        private double ScannerStartPositionX;
        private double ScannerStartPositionY;
        private double ScannerEndPositionX;
        private double ScannerEndPositionY;

        private int Scanner_CurrentCount_X = 0;         //  Scanner 가공 시 X 방향 이동, 최대 이동 회수는 ScannerTotalCountX
        private int Scanner_CurrentCount_Y = 0;         //  Scanner 가공 시 Y 방향 이동, 최대 이동 회수는 ScannerTotalCountY

        //  Manual 창에서 입력받는 파라미터
        public double ManualPage_WaferSize = 0;
        public double ManualPage_PitchX = 0;
        public double ManualPage_PitchY = 0;
        public double ManualPage_OffsetX = 0;
        public double ManualPage_OffsetY = 0;
        public double ManualPage_MarginCnt = 0;
        public double ManualPage_BeamSizeX = 0;
        public double ManualPage_BeamSizeY = 0;
        public double ManualPage_PitchCount = 0;
        public double ManualPage_StageLineCount = 0;

        //  LLO 시작 Index (사용자에 의해 입력 받음)
        public int m_nLLO_Work_StartIndex_X = 0;
        public int m_nLLO_Work_StartIndex_Y = 0;

        //  LLO 개수 (사용자에 의해 입력 받음)
        public int m_nLLO_Work_Count_X = 0;
        public int m_nLLO_Work_Count_Y = 0;
        public int m_nLLO_Work_Count_withInterval_X = 0;
        public int m_nLLO_Work_Count_withInterval_Y = 0;

        //  Block 단위 LLO 일 때, Set 번호
        public int m_nLLO_Work_Set_Index;           //  Block 단위 가공 시, Set 번호

        //  LLO 반복 회수
        public int m_nLLO_Work_Repetition_Count = 0;
        public int m_nLLO_Work_Current_Count = 0;

        //  LLO Chip Interval (X, Y 방향으로 몇개의 Chip 마다 가공할 것인지)
        public int m_nLLO_Work_Interval_X = 0;
        public int m_nLLO_Work_Interval_Y = 0;
        public int m_nLLO_Work_Interval_Current_X = 0;
        public int m_nLLO_Work_Interval_Current_Y = 0;
        public double m_dLLO_Work_ChipPitch_X = 0.0;
        public double m_dLLO_Work_ChipPitch_Y = 0.0;

        //  NG Chip Position List
        public struct stNgChipPosition              //  NG Chip 위치데이터 구조체
        {
            public int nLLO_Block_X;                //  Block Number (X, 1 ~ 280)
            public int nLLO_Block_Y;                //  Block Number (Y, 1 ~ 220)
            public int nLLO_Set;                    //  Block Index (1 ~ 28)
            public bool bChip_Judge;                //  Chip Judgement (OK, NG)
        }

        public stNgChipPosition[] stChipData;       // = new stNgChipPosition[1];
        public int m_nBlockChipGridData_PageIndex = 0;                                                                   //  GridData 페이지 인덱스
        public stNgChipPosition[,] stWaferPos;      // = new stNgChipPosition[1, 1];
        public stNgChipPosition[] stNgLLOPos;       // = new stNgChipPosition[1];
        public stNgChipPosition[] stNgLLOPos_forGridData;
        public int nLLO_NgChip_TotalCount { get; set; }                                                             //  2023. 02. 22.  SCH : NG Chip 총 개수

        private double RtcProcPosX;
        private double RtcProcPosY;

        public int m_LaserPowerCal_Count { set; get; }
        public bool m_bLaserPowerCal_OK { set; get; }
        public double m_dLaserInitPower { set; get; }

        public double PowerMeterValue { get; set; }

        public bool m_bLaserGetStatus_Run { set; get; }

        public QMC.Common.Stage StageCompenData = new QMC.Common.Stage();
        public bool m_bStage2DMapDataApply_Success { set; get; }

        //  SESAM 교체 주기
        public bool m_bSESAM_ReplacementCycle_Exceed { set; get; }
        public string m_strSESAM_ReplacementCycle_Remained { set; get; }


        public enum MapData_Type
        {
            MapData_Stage = 0,
            MapData_LCI = 1,
        }
        public bool m_bMapDataType_UserChange_Complete {  set; get; }               //  Map Data 변경 되었는지? --> 해당 타입의 ACS Global 변수를 1 로 세팅하고, 변경 성공하면 2로 리턴된다.

        public int m_nLCIMapData_StartIndex_X {  get; set; }                        //  LCI Module 사용 시, Map Data 변경 위치 X Index (시작, Left)
        public int m_nLCIMapData_EndIndex_X { get; set; }                           //  LCI Module 사용 시, Map Data 변경 위치 X Index (종료, Right)
        public int m_nLCIMapData_StageStartIndex_Y { get; set; }                    //  LCI Module 사용 시, Map Data 변경 위치 Y Index (시작)
        public int m_nLCIMapData_StageEndIndex_Y { get; set; }                      //  LCI Module 사용 시, Map Data 변경 위치 Y Index (종료)

        #endregion


        #region DPSS Laser (마곡 LGD)

        //public DPSSLaser Cepheus_laser = null;

        public MyCepheusLaser Cepheus_laser = null;

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

        public const byte chrSTX = 0x02;
        public const byte chrETX = 0x13;
        public int m_nChillerCommRecvData_ETX_Count { set; get; }

        public XyzCoordinate xyzCoord_WaferProbeAlignPos_Align = new XyzCoordinate();
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
        public WaferProbeAlignConfig Config { set; get; }
        public WaferProbeAlignParameterConfig ParamConfig { set; get; }
        public WaferProbeAlignRecipe Recipe { set; get; }

        public bool DrillingData_Loaded { set; get; }

        public double m_dRotationCenter_X;
        public double m_dRotationCenter_Y;

        public VisionScale Scale
        {
            set
            {
                Config.VisonCalibratorConfig_Upper.Scale = value;
            }
            get
            {
                return Config.VisonCalibratorConfig_Upper.Scale;
            }
        }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }                       //  마곡 LGD 의 SLO-400 은 Z 축이 2개다. (Camera Z 축, Scanner Z 축)
        public UvwzxyzStage Stage { set; get; }                         //  신천테크 CWA-150SA 

        //  상부 카메라
        public HIKGigECamera Camera_Upper { set; get; }
        //public GrabLinkMultiCamCamera Camera { set; get; }
        public VisionCalibrator visionCalibrator_Upper { set; get; }
        public VisionCompensator visionCompensator_Upper { set; get; }
        public LaserPitchMoveShotter laserPitchMoveShotter { get; set; }
        public ScannerCompensator scannerCompensator { set; get; }
        public JigAligner jigAligner_Upper { set; get; }

        public AutoFocuser autoFocuser_Upper { set; get; }
        public AutoFocusResult autoFocusResult_Upper;

        //  하부 카메라
        public HIKGigECamera Camera_Lower { set; get; }
        //public GrabLinkMultiCamCamera Camera_LowRes { set; get; }
        public VisionCalibrator visionCalibrator_Lower { set; get; }
        public VisionCompensator visionCompensator_Lower { set; get; }
        public JigAligner jigAligner_Lower { set; get; }
        public AutoFocuser autoFocuser_Lower { set; get; }
        public AutoFocusResult autoFocusResult_Lower;

        public int m_nProductAlign_CameraType {  set; get; }
        //public Api ACS_Motion { set; get; }

        //  가공 도면 Align 을 위한 변수
        public double m_dALIGN_FACTOR_RotationCenter_X { set; get; }                //  전체 가공 도면 회전 중심 X
        public double m_dALIGN_FACTOR_RotationCenter_Y { set; get; }                //  전체 가공 도면 회전 중심 Y
        public double m_dALIGN_FACTOR_Offset_X { set; get; }                        //  전체 가공 도면 이동 Offset X
        public double m_dALIGN_FACTOR_Offset_Y { set; get; }                        //  전체 가공 도면 이동 Offset Y
        public double m_dALIGN_FACTOR_Theta { set; get; }                           //  전체 가공 도면 회전 (Theta,     기준위치 : Align1 (Thruhole 의 Circle 객체, Description 에 Align1 표시)


        //public bool ACS_Motion_isSimulationMode { set; get; }

        private ProgramStates m_nProgramState0;
        private ProgramStates m_nProgramState1;
        private MotorStates m_nMotorState0;
        private MotorStates m_nMotorState1;

        public System.Windows.Forms.Timer timer_MainWork;
        public System.Windows.Forms.Timer timer_SubWork;
        public System.Windows.Forms.Timer timer_Motion_Home;
        //public System.Windows.Forms.Timer timer_Calibration;
        public System.Windows.Forms.Timer timer_VisionAlign;
        public System.Windows.Forms.Timer timer_ReticleGlass_Check;

        public bool m_bAlignVisionThread_Use;                                                       //  2022. 04. 08.  SCH : Align Vision 을 Thread 로 할지 말지?

        public bool m_bWaferProbeAlign_MainCyc_Running;

        public bool[,] AlignmentErrorCheck_Status = new bool[3, 2];                                 //  얼라인 에러 체크 [위치 개수, 상하부]
        public PointD[,] AlignmentErrorCheck_MarkPosition = new PointD[3, 2];                       //  얼라인 마크 위치값 [위치 개수, 상하부] --> 프로브 핀 마크 위치를 기준으로 웨이퍼전극 마크 위치의 차이를 얼라인 오차로 본다.
        public PointD[] AlignmentErrorCheck_Position = new PointD[3];                               //  얼라인 에러 체크 위치 [위치 개수]

        public WaferProbeAlignParameter waferProbeAlignParameter { set; get; }

        //public SerialPowerMeter1Port m_powerMeter_ExitPos_Comm { set; get; }                        //  2023. 04. 12.  SCH : Laser PowerMeter Comm (Exit Position) - COM3
        //public string m_strLaserPowerMeter_ExitPos_Comm_ReceivedData;
        //public bool m_bLaserPowerMeter_ExitPos_CommData_Received { set; get; }

        //public SerialPowerMeter2Port m_powerMeter_TargetPos_Comm { set; get; }                      //  2023. 04. 12.  SCH : Laser PowerMeter Comm (Target Position) - COM2
        //public string m_strLaserPowerMeter_TargetPos_Comm_ReceivedData;
        //public bool m_bLaserPowerMeter_TargetPos_CommData_Received { set; get; }

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
            TICK_LASER_INTERFACE = 3,   //  3 : Laser Interface Set
            TICK_LASER_FOCUS = 4,       //  4 : Laser Focus Check Cycle
            TICK_LASER_COMM = 5,        //  5 : Laser Comm. Cycle
            TICK_POWERMETER_COMM = 6,   //  6 : Power Meter Comm. Cycle
            TICK_ALIGN = 7,             //  7 : Wafer Align Cycle
            TICK_RVA = 8,               //  8 : Beam Size Change Cycle
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

        public int WaferProbeAlignCount { set; get; }
        public int WaferProbeAlignCount_Index { set; get; }

        #endregion


        #region Variable And Function

        //  Manual Packing Step
        public int m_nManualPacking_Step {  set; get; }

        public enum ManualPackingStep : int
        {
            NONE = 0,
            STEP1_OK = 1,
            STEP2_OK = 1,
        }



        public enum VisionType : int
        {
            NONE = 0,
            LOW_VISION = 1,
            HIGH_VISION = 2,
        }

        public int m_nHomeStep { set; get; }                    //  Home Step
        public int m_nHomeAxisCount { set; get; }
        public bool m_bHomeOK { get; set; }
        public enum Home_Step
        {
            None = 0,
            Start,                                              //  시작

            //  알람이 발생한 축이 있을 경우, Servo Off --> Reset --> Servo On 해야 한다.
            AxisAlarmCheck,                                     //  서보 축 알람 체크
            AlarmAxisServoOff,                                  //  알람 축 서보 Off
            AlarmAxisServoOffCheck,                             //  알람 축 서보 Off 확인
            AlarmAxisAlarmResetOn,                              //  알람 축 리셋 신호 On
            AlarmAxisAlarmResetOff,                             //  알람 축 리셋 신호 Off (30ms delay 후 Off)
            AlarmAxisServoOn,                                   //  알람 축 서보 On
            AlarmAxisServoOnCheck,                              //  알람 축 서보 On 확인

            VisionY_HomeStart,                                  //  Vision Y 축 홈 실행
            VisionY_HomeCompleteCheck,                          //  Vision Y 축 홈 완료 체크

            VisionXZ_UVW_EZ_HomeStart,                          //  Vision XZ, UVW EZ 축 홈 실행
            VisionXZ_UVW_EZ_HomeCompleteCheck,                  //  Vision XZ, UVW EZ 축 홈 완료 체크

            ElevZ_Move_ReadyPos,                                //  Elev. Z 축, 대기 위치로 이동 
            ElevZ_Move_ReadyPos_DoneCheck,                      //  Elev. Z 축, 대기 위치로 이동 완료 확인

            UVW_Move_ReadyPos,                                  //  UVW 축, 대기 위치로 이동
            UVW_Move_ReadyPos_DoneCheck,                        //  UVW 축, 대기 위치로 이동 완료 체크

            VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동 
            VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        #endregion


        #region Reticle Glass Check

        public int m_nReticleCheck_UpperCam_Step { set; get; }                    //  Reticle Glass Check Step (Upper Camera)
        public enum ReticleCheck_UpperCam_Step
        {
            None = 0,
            Start,                                              //  시작

            VisionXY_Move_ReadyPos,                             //  Vision XY 축, 대기 위치로 이동
            VisionXY_Move_ReadyPos_DoneCheck,                   //  Vision XY 축, 대기 위치로 이동 완료 확인

            UVW_Move_UpperCam_ReticlePos,                       //  UVW 축, Reticle Glass 확인 위치로 이동
            UVW_Move_UpperCam_ReticlePos_DoneCheck,             //  UVW 축, Reticle Glass 확인 위치로 이동 완료 확인

            ElevZ_Move_UpperCam_ReticlePos,                     //  Elev. Z 축, Reticle Glass 확인 위치로 이동 
            ElevZ_Move_UpperCam_ReticlePos_DoneCheck,           //  Elev. Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionZ_Move_ReticlePos,                            //  Vision Z 축, Reticle Glass 확인 위치로 이동
            VisionZ_Move_ReticlePos_DoneCheck,                  //  Vision Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionXY_Move_ReticlePos,                           //  Vision XY 축, Reticle Glass 확인 위치로 이동
            VisionXY_Move_ReticlePos_DoneCheck,                 //  Vision XY 축, Reticle Glass 확인 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        public int m_nReticleCheck_LowerCam_Step { set; get; }                    //  Reticle Glass Check Step (Lower Camera)
        public enum ReticleCheck_LowerCam_Step
        {
            None = 0,
            Start,                                              //  시작

            VisionXY_Move_ReadyPos,                             //  Vision XY 축, 대기 위치로 이동
            VisionXY_Move_ReadyPos_DoneCheck,                   //  Vision XY 축, 대기 위치로 이동 완료 확인

            UVW_Move_LowerCam_ReticlePos,                       //  UVW 축, Reticle Glass 확인 위치로 이동
            UVW_Move_LowerCam_ReticlePos_DoneCheck,             //  UVW 축, Reticle Glass 확인 위치로 이동 완료 확인

            ElevZ_Move_LowerCam_ReticlePos,                     //  Elev. Z 축, Reticle Glass 확인 위치로 이동 
            ElevZ_Move_LowerCam_ReticlePos_DoneCheck,           //  Elev. Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionZ_Move_ReticlePos,                            //  Vision Z 축, Reticle Glass 확인 위치로 이동
            VisionZ_Move_ReticlePos_DoneCheck,                  //  Vision Z 축, Reticle Glass 확인 위치로 이동 완료 확인

            VisionXY_Move_ReticlePos,                           //  Vision XY 축, Reticle Glass 확인 위치로 이동
            VisionXY_Move_ReticlePos_DoneCheck,                 //  Vision XY 축, Reticle Glass 확인 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        #endregion


        #region Single Action

        public int m_nSafetyPos_Move_Step { set; get; }                 //  Safety Position Move Step
        public enum SafetyPos_Move_Step
        {
            None = 0,
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

        public int m_nPAK_AirLine_Check_Step { set; get; }      //  PAK Air Line Check Step
        public bool m_bPAK_AirLineCheck_Complete {  set; get; }
        public bool m_bPAK_AirLineCheck_OK { set; get; }
        public enum PAK_AirLine_Check_Step
        {
            None = 0,
            Start,                                              //  시작


            _MachineType_Check,                                 //  Machine Type 확인


            _MachineType_A_Start,                               //  Type A 시작

                PAK_Check_Condition_Check,                          //  PAK Check 조건 확인 (Probe Card Exist)

                Top_Cover_Down,                                     //  Top Cover Down
                Top_Cover_Down_Check,                               //  Top Cover Down Check    

            _MachineType_A_Complete,                            //  Type A 완료


            _MachineType_B_Start,                               //  Type B 시작

            _MachineType_B_Complete,                            //  Type B 완료


            Packing_Signal_On,                                  //  Packing Signal On
            Packing_Signal_On_StableTime,                       //  Packing Signal On 후 확인 시간

            Complete                                            //  완료
        }

        public int m_nWafer_Loading_Ready_Step { set; get; }          //  Wafer Loading Position Move Step
        public enum WaferLoading_Ready_Step
        {
            None = 0,
            Start,                                              //  시작

            VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            ElevZ_Move_WaferLoadingPos,                         //  Elev. Z 축, Wafer Loading 위치로 이동 
            ElevZ_Move_WaferLoadingPos_DoneCheck,               //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

            UVW_Move_WaferLoadingPos,                           //  UVW 축, Wafer Loading 위치로 이동
            UVW_Move_WaferLoadingPos_DoneCheck,                 //  UVW 축, Wafer Loading 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        public int m_nProbeCard_Loading_Ready_Step { set; get; }          //  Probe Card Loading Ready Step
        public enum ProbeCard_Loading_Ready_Step
        {
            None = 0,
            Start,                                              //  시작

            VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            ElevZ_Move_WaferLoadingPos,                         //  Elev. Z 축, Wafer Loading 위치로 이동 
            ElevZ_Move_WaferLoadingPos_DoneCheck,               //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

            UVW_Move_WaferLoadingPos,                           //  UVW 축, Wafer Loading 위치로 이동
            UVW_Move_WaferLoadingPos_DoneCheck,                 //  UVW 축, Wafer Loading 위치로 이동 완료 확인


            _MachineType_Check,                                 //  Machine Type 확인


            _MachineType_A_Start,                               //  Type A 시작

                Top_Cover_Up,                                       //  Top Cover Up
                Top_Cover_Up_Check,                                 //  Top Cover Up Check

            _MachineType_A_Complete,                            //  Type A 완료


            _MachineType_B_Start,                               //  Type B 시작

                ProbeCard_UnpackingSignal_Off,                      //  프로브 카드 패킹, 언패킹 신호 Off
                ProbeCard_UnpackingSignal_Off_Check,                //  프로브 카드 패킹, 언패킹 신호 Off 확인

                ProbeCard_UnpackingCyl_Up,                          //  프로브 카드 언패킹 실린더 Up 
                ProbeCard_UnpackingCyl_Up_Check,                    //  프로브 카드 언패킹 실린더 Up 확인

                ProbeCard_Clamp_Up,                                 //  프로브 카드 클램프 Up 
                ProbeCard_Clamp_Up_Check,                           //  프로브 카드 클램프 Up 확인

                ProbeCard_Clamp_BW,                                 //  프로브 카드 클램프 BW 
                ProbeCard_Clamp_BW_Check,                           //  프로브 카드 클램프 BW 확인

            _MachineType_B_Complete,                            //  Type B 완료


            Complete                                            //  완료
        }

        public int m_nWaferProbeCard_Unpacking_Ready_Step { set; get; }          //  Wafer ProbeCard Unpacking Ready Step
        public enum WaferProbeCard_Unpacking_Ready_Step
        {
            None = 0,
            Start,                                              //  시작

            Unpacking_Ready_Condition_Check,                    //  Unpacking Ready 조건 확인 (Thin Chuck None Exist)

            Unpacking_Signal_Off,                               //  Unpacking Signal Off
            Unpacking_Signal_Off_Check,                         //  Unpacking Signal Off 확인


            _MachineType_Check,                                 //  Machine Type 확인


            _MachineType_A_Start,                               //  Type A 시작

                Top_Cover_Up,                                       //  Top Cover Up
                Top_Cover_Up_Check,                                 //  Top Cover Up Check

            _MachineType_A_Complete,                            //  Type A 완료


            _MachineType_B_Start,                               //  Type B 시작

                ProbeCard_UnpackingSignal_Off,                      //  프로브 카드 패킹, 언패킹 신호 Off
                ProbeCard_UnpackingSignal_Off_Check,                //  프로브 카드 패킹, 언패킹 신호 Off 확인

                ProbeCard_UnpackingCyl_Up,                          //  프로브 카드 언패킹 실린더 Up 
                ProbeCard_UnpackingCyl_Up_Check,                    //  프로브 카드 언패킹 실린더 Up 확인

                ProbeCard_Clamp_Up,                                 //  프로브 카드 클램프 Up 
                ProbeCard_Clamp_Up_Check,                           //  프로브 카드 클램프 Up 확인

                ProbeCard_Clamp_BW,                                 //  프로브 카드 클램프 BW 
                ProbeCard_Clamp_BW_Check,                           //  프로브 카드 클램프 BW 확인

            _MachineType_B_Complete,                            //  Type B 완료


            VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            ElevZ_Move_WaferProbeCard_UnpackingReadyPos,             //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 (Unpacking 높이에서 10mm 아래)
            ElevZ_Move_WaferProbeCard_UnpackingReadyPos_DoneCheck,   //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 완료 확인

            UVW_Move_WaferProbeCard_UnpackingPos,               //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동
            UVW_Move_WaferProbeCard_UnpackingPos_DoneCheck,     //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        public int m_nWaferProbeCard_Unpacking_Step { set; get; }          //  Wafer ProbeCard Unpacking Step
        public enum WaferProbeCard_Unpacking_Step
        {
            None = 0,
            Start,                                              //  시작

            Unpacking_Condition_Check,                          //  Unpacking 조건 확인 (Thin Chuck Exist, Probe Card Exist)


            _MachineType_Check,                                 //  Machine Type 확인


            _MachineType_A_Start,                               //  Type A 시작

                Top_Cover_Down,                                     //  Top Cover Down
                Top_Cover_Down_Check,                               //  Top Cover Down Check

            _MachineType_A_Complete,                            //  Type A 완료


            _MachineType_B_Start,                               //  Type B 시작

                ProbeCard_UnpackingSignal_Off,                      //  프로브 카드 패킹, 언패킹 신호 Off
                ProbeCard_UnpackingSignal_Off_Check,                //  프로브 카드 패킹, 언패킹 신호 Off 확인

                ProbeCard_UnpackingCyl_Up,                          //  프로브 카드 언패킹 실린더 Up 
                ProbeCard_UnpackingCyl_Up_Check,                    //  프로브 카드 언패킹 실린더 Up 확인

                ProbeCard_Clamp_Up,                                 //  프로브 카드 클램프 Up 
                ProbeCard_Clamp_Up_Check,                           //  프로브 카드 클램프 Up 확인

                ProbeCard_Clamp_FW,                                 //  프로브 카드 클램프 FW 
                ProbeCard_Clamp_FW_Check,                           //  프로브 카드 클램프 FW 확인

                ProbeCard_Clamp_Down,                               //  프로브 카드 클램프 Down 
                ProbeCard_Clamp_Down_Check,                         //  프로브 카드 클램프 Down 확인

                ProbeCard_UnpackingCyl_Down,                        //  프로브 카드 언패킹 실린더 Down 
                ProbeCard_UnpackingCyl_Down_Check,                  //  프로브 카드 언패킹 실린더 Down 확인

            _MachineType_B_Complete,                            //  Type B 완료


            VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            UVW_Move_WaferProbeCard_UnpackingPos,               //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동
            UVW_Move_WaferProbeCard_UnpackingPos_DoneCheck,     //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동 완료 확인

            ElevZ_Move_WaferProbeCard_UnpackingReadyPos,             //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 (Unpacking 높이에서 10mm 아래, fast)
            ElevZ_Move_WaferProbeCard_UnpackingReadyPos_DoneCheck,   //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 완료 확인

            ElevZ_Move_WaferProbeCard_UnpackingPos,             //  Elev. Z 축, Wafer ProbeCard Unpacking 위치로 이동 (slow)
            ElevZ_Move_WaferProbeCard_UnpackingPos_DoneCheck,   //  Elev. Z 축, Wafer ProbeCard Unpacking 위치로 이동 완료 확인

            ThinChuck_Wafer_Vacuum_On,                          //  Thin-Chuck 과 웨이퍼 공압 On
            ThinChuck_Wafer_Vacuum_On_Check,                    //  Thin-Chuck 과 웨이퍼 공압 On 확인

            Unpacking_Signal_On,                                //  Unpacking Signal On
            Unpacking_Signal_On_Check,                          //  Unpacking Signal On 확인
            Unpacking_Signal_On_StableTime,                     //  Unpacking Signal On 안정화 시간
                    
            ElevZ_Move_WaferProbeCard_UnpackingReadyPos2,            //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 (Unpacking 높이에서 10mm 아래, slow)
            ElevZ_Move_WaferProbeCard_UnpackingReadyPos2_DoneCheck,  //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 완료 확인

            Unpacking_Signal_Off,                               //  Unpacking Signal Off
            Unpacking_Signal_Off_Check,                         //  Unpacking Signal Off 확인

            ElevZ_Move_WaferLoadingPos,                         //  Elev. Z 축, Wafer Loading 위치로 이동 
            ElevZ_Move_WaferLoadingPos_DoneCheck,               //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

            Complete                                            //  완료
        }

        public int m_nProbeCard_Locking_Step { set; get; }          //  Probe Card Locking Step
        public enum ProbeCard_Locking_Step
        {
            None = 0,
            Start,                                              //  시작


            _MachineType_Check,                                 //  Machine Type 확인


            _MachineType_A_Start,                               //  Type A 시작

                ProbeCard_Detect,                                   //  ProbeCard 확인

                TopCover_Down,                                      //  Top Cover Down 
                TopCover_Down_Check,                                //  Top Cover Down 완료 확인

            _MachineType_A_Complete,                            //  Type A 완료


            _MachineType_B_Start,                               //  Type B 시작

                ProbeCard_UnpackingSignal_Off,                      //  프로브 카드 패킹, 언패킹 신호 Off
                ProbeCard_UnpackingSignal_Off_Check,                //  프로브 카드 패킹, 언패킹 신호 Off 확인

                ProbeCard_UnpackingCyl_Up,                          //  프로브 카드 언패킹 실린더 Up 
                ProbeCard_UnpackingCyl_Up_Check,                    //  프로브 카드 언패킹 실린더 Up 확인

                ProbeCard_Clamp_Up,                                 //  프로브 카드 클램프 Up 
                ProbeCard_Clamp_Up_Check,                           //  프로브 카드 클램프 Up 확인

                ProbeCard_Clamp_FW,                                 //  프로브 카드 클램프 FW 
                ProbeCard_Clamp_FW_Check,                           //  프로브 카드 클램프 FW 확인

                ProbeCard_Clamp_Down,                               //  프로브 카드 클램프 Down 
                ProbeCard_Clamp_Down_Check,                         //  프로브 카드 클램프 Down 확인

            _MachineType_B_Complete,                            //  Type B 완료


            Complete                                            //  완료
        }

        public int m_nWafer_ProbeCard_Packing_Step { set; get; }        //  Wafer ProbeCard Packing Step
        public double m_dWafer_ProbeCard_PackingPos_Axis_U { set; get; }        //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
        public double m_dWafer_ProbeCard_PackingPos_Axis_V { set; get; }        //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
        public double m_dWafer_ProbeCard_PackingPos_Axis_W { set; get; }        //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

        public enum WaferProbeCard_Packing_Step
        {
            None = 0,
            Start,                                              //  시작

            Packing_Condition_Check,                            //  Packing 조건 확인 (Thin Chuck Exist, Probe Card Exist. Top Cover Down, Thin Chuck Vacuum Check, Wafer Vacuum Check)

            VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인



            __Wafer_OffsetMove_Start,                                       //  Wafer Offset 이동 시작

            UVW_Move_WaferPackingOffset,                                    //  UVW 축, Wafer Packing Offset 거리 이동
            UVW_Move_WaferPackingOffset_DoneCheck,                          //  UVW 축, Wafer Packing Offset 거리 이동 완료 확인

            __Wafer_OffsetMove_Complete,                                    //  Wafer Offset 이동 완료



            ElevZ_FastMove_PackingReadyPos,                     //  Elev. Z 축, Wafer ProbeCard Packing 대기 높이로 이동 (fast)
            ElevZ_FastMove_PackingReadyPos_DoneCheck,           //  Elev. Z 축, Wafer ProbeCard Packing 대기 높이로 이동 완료 확인

            ElevZ_SlowMove_PackingPos,                          //  Elev. Z 축, Wafer ProbeCard Packing 높이로 이동 (slow)
            ElevZ_SlowMove_PackingPos_DoneCheck,                //  Elev. Z 축, Wafer ProbeCard Packing 높이로 이동 완료 확인

            BeforePacking_StableTime,                           //  Packing 전 안정화 시간

            Probe_Packing,                                      //  Packing Signal On
            Probe_Packing_StableTime,                           //  Packing Signal On 후 안정화 시간

            Wafer_Vacuum_Off,                                   //  Wafer Vacuum Off
            Wafer_Vacuum_Off_Check,                             //  Wafer Vacuum Off 확인
            
            Packing_Vacuum_Off,                                 //  Packing Vacuum Off
            Packing_Vacuum_Off_Check,                           //  Packing Vacuum Off 확인

            ThinChuck_Vacuum_Off,                               //  Thin Chuck Vacuum Off
            ThinChuck_Vacuum_Off_Check,                         //  Thin Chuck Vacuum Off 확인

            ElevZ_Move_WaferLoadingReadyPos,                    //  Elev. Z 축, Wafer Loading 대기 위치로 이동 (slow)
            ElevZ_Move_WaferLoadingReadyPos_DoneCheck,          //  Elev. Z 축, Wafer Loading 대기 위치로 이동 완료 확인

            Packing_Status_Check,                               //  Packing 이 잘 되엇는지 확인. (여기서 Thin Chuck 이 감지되면 Packing 실패)

            ElevZ_Move_WaferLoadingPos,                         //  Elev. Z 축, Wafer Loading 위치로 이동
            ElevZ_Move_WaferLoadingPos_DoneCheck,               //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

            UVW_Move_WaferLoadingPos,                           //  UVW 축, Wafer Loading 위치로 이동
            UVW_Move_WaferLoadingPos_DoneCheck,                 //  UVW 축, Wafer Loading 위치로 이동 완료 확인


            _MachineType_Check,                                 //  Machine Type 확인


            _MachineType_A_Start,                               //  Type A 시작

                Top_Cover_Up,                                       //  Top Cover Up
                Top_Cover_Up_Check,                                 //  Top Cover Up Check

            _MachineType_A_Complete,                            //  Type A 완료


            _MachineType_B_Start,                               //  Type B 시작

                ProbeCard_UnpackingSignal_Off,                      //  프로브 카드 패킹, 언패킹 신호 Off
                ProbeCard_UnpackingSignal_Off_Check,                //  프로브 카드 패킹, 언패킹 신호 Off 확인

                ProbeCard_UnpackingCyl_Up,                          //  프로브 카드 언패킹 실린더 Up 
                ProbeCard_UnpackingCyl_Up_Check,                    //  프로브 카드 언패킹 실린더 Up 확인

                ProbeCard_Clamp_Up,                                 //  프로브 카드 클램프 Up 
                ProbeCard_Clamp_Up_Check,                           //  프로브 카드 클램프 Up 확인

                ProbeCard_Clamp_BW,                                 //  프로브 카드 클램프 BW 
                ProbeCard_Clamp_BW_Check,                           //  프로브 카드 클램프 BW 확인

            _MachineType_B_Complete,                            //  Type B 완료


            Complete                                            //  완료
        }


        public bool m_bWaferProbeAlign_MainCyc_Complete { set; get; }         //  Wafer ProbeCard Align Cycle 완료 확인
                                                                           //  WorkStatus 를 "WORK_DONE" 으로 세팅
        public int m_nWaferProbeAlign_MainStep { set; get; }                  //  Wafer ProbeCard Align Main Cycle Step
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

            RESULT_UpperVISION_FIRSTMARKPOS = 3,
            RESULT_UpperVISION_SECONDMARKPOS = 4,
            RESULT_UpperVISION_THETA = 5,
            RESULT_UpperVISION_POSOFFSET = 6,

            RESULT_LowerVISION_FIRSTMARKPOS = 7,
            RESULT_LowerVISION_SECONDMARKPOS = 8,
            RESULT_LowerVISION_THETA = 9,
            RESULT_LowerVISION_POSOFFSET = 10,

            RESULTPOS_FIRSTMARK_AVG = 11,                                    //  평균 계산을 위한 변수
            RESULTTHETA_AVG = 12,                                            //  평균 계산을 위한 변수
        }


        public int m_nAlignMark_FoundCount { set; get; }                            //  Align Mark 찾은 개수
        public XyCoordinate[] m_forAlign_Data = new XyCoordinate[System.Enum.GetValues(typeof(AlignParam)).Length];        //  Align 용 회전 중심 좌표 (1번 마크)

        public int m_nUnfollow_TryCount { set; get; }                       //  Motion Unfollow 시도 회수
        public bool m_bUnfollow_Ret { set; get; }                           //  Motion Unfollow 함수 리턴값

        public bool m_bProbeCard_TiltCheck_Only { set; get; }               //  ProbeCard Tilt Check Only
        public bool m_bWafer_Align_Only { set; get; }                       //  Wafer Align Only

        public bool m_bUpperCam_AlignPattern_Reset { set; get; }            //  상부 카메라 얼라인 패턴 재등록
        public bool m_bLowerCam_AlignPattern_Reset { set; get; }            //  하부 카메라 얼라인 패턴 재등록

        public bool m_bProbeCard_TiltCheck_OK { set; get; }                 //  ProbeCard Tilt Check OK
        public double m_dProbeCard_TiltData {  set; get; }                  //  Probe Card Tilt Data
        public bool m_bProbeCard_XYAlign_OK { set; get; }                   //  Probe Card XY Align OK
        public bool m_bWafer_ThetaAlign_OK { set; get; }                    //  Wafer Theta Align OK
        public bool m_bWafer_XYAlign_OK { set; get; }                       //  Wafer XY Align OK
        public double m_dProbeCardAlign_CorrectionAngle { set; get; }       //  Wafer Align 시 이 각도로 얼라인 한다.
        public double m_dProbeCard_XYAlign_CorrectionAngle { set; get; }        
        public double m_dWaferAlign_CorrectionAngle { set; get; }
        public string m_strProbeCard_TiltData_for_Display {  set; get; }      //  각도 보여주기
        public string m_strWafer_TiltData_for_Display { set; get; }           //  각도 보여주기
        public double m_dWafer_ProbeCard_AlignPos_Axis_U { set; get; }        //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
        public double m_dWafer_ProbeCard_AlignPos_Axis_V { set; get; }        //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
        public double m_dWafer_ProbeCard_AlignPos_Axis_W { set; get; }        //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
        public enum WaferProbeAlign_Step
        {
            None = 0,
            Start,                                                          //  시작


            Align_Condition_Check,                                          //  Align 조건 확인 (Thin Chuck Exist)


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
            VisionXYZ_Move_WaferAlignPos_TopCenter_StableTime,              //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동 후 안정화 시간

            WaferAlign_TopMarkFind,                                         //  Top 위치 얼라인 마크 찾기
            WaferAlign_TopMarkResultCheck,                                  //  Top 위치 얼라인 마크 찾기 결과 확인


            WaferAlign_BottomMarkFind_Ready,                                //  Bottom 위치 얼라인 마크 찾기 준비

            VisionXYZ_Move_WaferAlignPos_BottomCenter,                      //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동
            VisionXYZ_Move_WaferAlignPos_BottomCenter_DoneCheck,            //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동 완료 확인
            VisionXYZ_Move_WaferAlignPos_BottomCenter_StableTime,           //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동 후 안정화 시간

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
            VisionXYZ_Move_WaferXYAlignPos_TopCenter_StableTime,            //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동 후 안정화 시간

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
        public bool m_bLowerVision_Align { set; get; }                      //  true: Lower Vision       false: Upper Vision
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



        public int m_nWaferProbeAlign_ErrorCheck_Step { set; get; }         //  Wafer ProbeCard Alignment Error Check Cycle Step
        public bool m_bWaferProbeAlign_ErrorCheck_Complete {  set; get; }
        public bool m_bProbeCard_XYAlign_ErrorCheck_OK { set; get; }        //  Probe Card XY Align Error Check OK
        public bool m_bWafer_XYAlign_ErrorCheck_OK { set; get; }            //  Wafer XY Align Error Check OK
        public int m_nWaferProbeAlign_ErrorCheck_Count_Total { set; get; }  //  오차 확인 위치 총 개수
        public int m_nWaferProbeAlign_ErrorCheck_Count {  set; get; }       //  오차 확인 위치 카운트 (3개,   0: TOP, 1: MID, 2: BOT)
        public enum WaferProbeAlignErrorCheck_Step
        {
            None = 0,
            Start,                                                          //  시작


            Align_Condition_Check,                                          //  Align 조건 확인 (Thin Chuck Exist)


            VisionXYZ_Move_ReadyPos,                                        //  Vision XYZ 축, 대기 위치로 이동
            VisionXYZ_Move_ReadyPos_DoneCheck,                              //  Vision XYZ 축, 대기 위치로 이동 완료 확인

            ElevZ_Move_WaferAlignPos,                                       //  Elev. Z 축, Wafer Align 위치로 이동
            ElevZ_Move_WaferAlignPos_DoneCheck,                             //  Elev. Z 축, Wafer Align 위치로 이동 완료 확인



            __ErrorCheck_Start,                                             //  Wafer - ProbeCard Alignment Error Check 시작

            ErrorCheck_Position_Remained_Check,                             //  얼라인 확인 위치가 남아있는지 체크 (Top, Center, Bottom 3군데 체크)



            __ProbeCard_XYAlign_Start,                                      //  ProbeCard XY Align 시작

            ProbeCardXYAlign_MarkFind_Ready,                                //  얼라인 마크 찾기 준비

            VisionXYZ_Move_ProbeCardXYAlignPos,                             //  VisionXYZ 축, ProbeCard XY Align 위치로 이동
            VisionXYZ_Move_ProbeCardXYAlignPos_DoneCheck,                   //  VisionXYZ 축, ProbeCard XY Align 위치로 이동 완료 확인
            VisionXYZ_Move_ProbeCardXYAlignPos_StableTime,                  //  VisionXYZ 축, ProbeCard XY Align 위치로 이동 후 안정화 시간

            ProbeCardXYAlign_MarkFind,                                      //  얼라인 마크 찾기
            ProbeCardXYAlign_MarkResultCheck,                               //  얼라인 마크 찾기 결과 확인

            ProbeCardXYAlignData_Calc,                                      //  XY 얼라인 데이터 계산

            ProbeCardXYAlign_CorrectionMove,                                //  XY 보정 이동 (카메라 Center 로)
            ProbeCardXYAlign_CorrectionMove_DoneCheck,                      //  XY 보정 이동 완료 확인

            ProbeCardXYAlign_Retry,                                         //  Vision XY 값 확인 -> Config 에 설정된 값보다 클 경우 Retry

            __ProbeCard_XYAlign_Complete,                                   //  ProbeCard XY Align 완료



            __Wafer_XYAlign_Start,                                          //  Wafer XY Align 시작

            WaferXYAlign_MarkFind_Ready,                                    //  얼라인 마크 찾기 준비
            WaferXYAlign_MarkFind_Ready_StableTime,                         //  얼라인 마크 찾기 전 안정화 시간

            WaferXYAlign_MarkFind,                                          //  얼라인 마크 찾기
            WaferXYAlign_MarkResultCheck,                                   //  얼라인 마크 찾기 결과 확인

            WaferXYAlignData_ErrorCalc,                                     //  XY 얼라인 데이터 오차 계산

            __Wafer_XYAlign_Complete,                                       //  Wafer XY Align 완료



            Complete                                            //  완료
        }



        #endregion


        #region Constructor
        public WaferProbeAlign(string strName) : base(strName)
        {

            bool ret = true;

            ParamConfig = new WaferProbeAlignParameterConfig();
            Config = new WaferProbeAlignConfig();
            //SetDispenserWork((int)DispenserWorkStatus.WORK_NONE);

            //WaferProbeAlignIndex = -1;
            m_bLaserGetStatus_Run = false;

            Cepheus_laser = new MyCepheusLaser();

            m_nHomeStep = (int)Home_Step.None;
            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
            m_nFindAlignMark_Step = (int)FindAlignMark_Step.None;
            m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
            m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
            m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;
            m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.None;
            m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
            m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
            m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
            m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
            m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
            m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.None;

            m_nManualPacking_Step = (int)ManualPackingStep.NONE;

            m_bProbeCard_TiltCheck_Only = false;               //  ProbeCard Tilt Check Only
            m_bWafer_Align_Only = false;                       //  Wafer Align Only

            m_bFindAlignMark_OK = false;

            m_bProbeCard_XYAlign_ErrorCheck_OK = false;         //  Probe Card XY Align Error Check OK
            m_bWafer_XYAlign_ErrorCheck_OK = false;             //  Wafer XY Align Error Check OK

            m_bPAK_AirLineCheck_Complete = false;
            m_bPAK_AirLineCheck_OK = true;                      //  PAK 공압 라인이 막혔는지 확인

            m_bWaferLowResAlign_OK = false;
            m_bWaferHighResAlign_OK = false;
            m_bWaferLowResAutoFocus_OK = false;
            m_bWaferHighResAutoFocus_OK = false;

            m_bAlignVisionThread_Use = true;                                            //  Align Vision 을 Thread 로 할지 말지?

            m_bSelected_HighResCamera = false;
            m_bMyWaferAlign_fromManualMode = false;

            ScannerTotalCountX = 0;          //  Scanner FOV 만큼 X 방향으로 몇 번 이동해야 하는 지
            ScannerTotalCountY = 0;          //  Scanner FOV 만큼 Y 방향으로 몇 번 이동해야 하는 지

            ScannerStartPositionX = 0;
            ScannerStartPositionY = 0;
            ScannerEndPositionX = 0;
            ScannerEndPositionY = 0;

            //  Main Work 타이머
            timer_MainWork = new System.Windows.Forms.Timer();
            timer_MainWork.Interval = 50;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_MainWork.Tick += new System.EventHandler(Timer_MainWork_Func);

            //  Sub Work 타이머
            timer_SubWork = new System.Windows.Forms.Timer();
            timer_SubWork.Interval = 50;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_SubWork.Tick += new System.EventHandler(Timer_SubWork_Func);

            //  Product Align 타이머
            timer_VisionAlign = new System.Windows.Forms.Timer();
            timer_VisionAlign.Interval = 50;
            timer_VisionAlign.Tick += new System.EventHandler(Timer_ProductAlign_Func);

            //  Motion 홈 실행 타이머
            timer_Motion_Home = new System.Windows.Forms.Timer();
            timer_Motion_Home.Interval = 30;
            timer_Motion_Home.Tick += new System.EventHandler(Timer_MotionHome_Func);

            //  Reticle Glass check 타이머
            timer_ReticleGlass_Check = new System.Windows.Forms.Timer();
            timer_ReticleGlass_Check.Interval = 30;
            timer_ReticleGlass_Check.Tick += new System.EventHandler(Timer_ReticleGlass_Func);

            ////  Laser Status 갱신 실행 타이머
            //timer_Calibration = new System.Windows.Forms.Timer();
            //timer_Calibration.Interval = 50;
            //timer_Calibration.Tick += new System.EventHandler(Timer_LaserCalibration_Func);

            //SpiralLab.Core.Initialize();

            m_nProductAlign_CameraType = (int)CameraType.CAMERA_HIGH;

            m_bHomeOK = false;

            DrillingData_Loaded = false;

            m_stJobList.m_nContinuousJobCount = 0;
            m_stJobList.m_strJobFullPath = new string[10];              //  10개 이상 할 일이 있을까?

            m_bDrillingData_isLine = false;

            m_bUpperCam_AlignPattern_Reset = false;
            m_bLowerCam_AlignPattern_Reset = false;

            m_strProbeCard_TiltData_for_Display = "- - -";
            m_strWafer_TiltData_for_Display = "- - -";

            m_dWafer_ProbeCard_PackingPos_Axis_U = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
            m_dWafer_ProbeCard_PackingPos_Axis_V = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
            m_dWafer_ProbeCard_PackingPos_Axis_W = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

            m_dWafer_ProbeCard_AlignPos_Axis_U = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
            m_dWafer_ProbeCard_AlignPos_Axis_V = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
            m_dWafer_ProbeCard_AlignPos_Axis_W = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)

            m_nWaferProbeAlign_ErrorCheck_Count_Total = 3;              //  오차 확인 위치 총 개수
            m_nWaferProbeAlign_ErrorCheck_Count = 0;                    //  오차 확인 위치 카운트 (3개,   0: TOP, 1: MID, 2: BOT)

            for ( int nPos = 0; nPos < 3; nPos++)
            {
                AlignmentErrorCheck_Position[nPos].X = 0.0;
                AlignmentErrorCheck_Position[nPos].Y = 0.0;

                for (int nSide = 0; nSide < 2; nSide++)
                {
                    AlignmentErrorCheck_Status[nPos, nSide] = false;

                    AlignmentErrorCheck_MarkPosition[nPos, nSide].X = 0.0;
                    AlignmentErrorCheck_MarkPosition[nPos, nSide].Y = 0.0;                    
                }
            }

            //  가공 도면 Align
            m_stAlignMark.dAlignMark1.X = 0.0;
            m_stAlignMark.dAlignMark1.Y = 0.0;
            m_stAlignMark.dAlignMark2.X = 0.0;
            m_stAlignMark.dAlignMark2.Y = 0.0;
            m_stAlignMark.dRotationCenter.X = 0.0;
            m_stAlignMark.dRotationCenter.Y = 0.0;
            m_stAlignMark.dRotationDegree = 0.0;

            m_dALIGN_FACTOR_RotationCenter_X = 0.0;                     //  전체 가공 도면 회전 중심 X
            m_dALIGN_FACTOR_RotationCenter_Y = 0.0;                     //  전체 가공 도면 회전 중심 Y
            m_dALIGN_FACTOR_Offset_X = 0.0;                             //  전체 가공 도면 이동 Offset X
            m_dALIGN_FACTOR_Offset_Y = 0.0;                             //  전체 가공 도면 이동 Offset Y
            m_dALIGN_FACTOR_Theta = 0.0;                               //  전체 가공 도면 회전 (Theta,     기준위치 : Align1 (Thruhole 의 Circle 객체, Description 에 Align1 표시)

            for (int i = 0; i < System.Enum.GetValues(typeof(AlignParam)).Length; i++)
            {
                m_forAlign_Data[i].X = 0.0;
                m_forAlign_Data[i].Y = 0.0;
            }

            //m_bFindFirstAlignMarkOnly = false;
            m_nFindAlignMarkType = (int)AlignMarkType.ALIGN_2POINT;

            TickCount_MainCycle_Start = 0;
            TickCount_MainCycle_Current = 0;
            TickCount_MainCycle_Interval = 0;

            for (int i = 0; i < 2; i++)
            {
                m_dCmdPos[i] = 0.0;
                m_dActPos[i] = 0.0;
            }

            m_nUnfollow_TryCount = 0;
            m_bUnfollow_Ret = false;

            PEG_Mode = (int)nPegMode.PEG_FullNormal;

            //  변수 초기화
            m_nLLO_Work_Interval_X = 0;                     //  몇개의 Chip or Block 마다 가공할 것인지 (X)
            m_nLLO_Work_Interval_Y = 0;                     //  몇개의 Chip or Block 마다 가공할 것인지 (Y)
            m_nLLO_Work_Interval_Current_X = 0;             //  Interval Count X
            m_nLLO_Work_Interval_Current_Y = 0;             //  Interval Count Y

            m_nLLO_Work_Repetition_Count = 1;               //  가공 회수
            m_nLLO_Work_Current_Count = 0;                  //  현재 가공 회수

            m_nLLO_Work_Set_Index = 1;                      //  Block 단위 가공 시 Set Index

            m_bStage2DMapDataApply_Success = false;

            m_bSESAM_ReplacementCycle_Exceed = false;
            m_strSESAM_ReplacementCycle_Remained = "";

            m_bMapDataType_UserChange_Complete = false;

            m_nLCIMapData_StartIndex_X = 0;                 //  LCI Module 사용 시, Map Data 변경 위치 X Index (시작, Left)
            m_nLCIMapData_EndIndex_X = 0;                   //  LCI Module 사용 시, Map Data 변경 위치 X Index (종료, Right)
            m_nLCIMapData_StageStartIndex_Y = 0;            //  LCI Module 사용 시, Map Data 변경 위치 Y Index (시작)
            m_nLCIMapData_StageEndIndex_Y = 0;              //  LCI Module 사용 시, Map Data 변경 위치 Y Index (종료)

            PEGDiv_Firstpoint_PEG = 0.0;                    //  PEG 시작 위치
            PEGDiv_Lastpoint_PEG = 0.0;                     //  PEG 종료 위치
            PEGDiv_StartPos_PEG = 0.0;                      //  Stage 시작 위치
            PEGDiv_EndPos_PEG = 0.0;                        //  Stage 종료 위치
            PEGDiv_ReCalc_Firstpoint_PEG = 0.0;             //  다시 계산된 PEG 시작 위치
            PEGDiv_ReCalc_Lastpoint_PEG = 0.0;              //  다시 계산된 PEG 종료 위치
            PEGDiv_ReCalc_StartPos_PEG = 0.0;               //  다시 계산된 Stage 시작 위치
            PEGDiv_ReCalc_EndPos_PEG = 0.0;                 //  다시 계산된 Stage 종료 위치
            PEGDiv_DivArea_TotalCount = 0;                  //  몇개의 분할 영역으로 나누어지는지
            PEGDiv_DivArea_Count = 0;                       //  몇 번째 분할 영역인지
            PEGDiv_WorkChipCount_inDivArea = 0;             //  분할 영역 안에 몇개의 Chip 이 있는지
            PEGDiv_MoveDirection_LtoR = true;               //  가공 방향 (true: 왼쪽에서 오른쪽으로 이동하면서 가공, 스테이지는 오른쪽에서 왼쪽으로 이동)
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

            //Stage = new XyztStage("Stage");
            //Stage = new XyzztStage("Stage");
            Stage = new UvwzxyzStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            Camera_Lower = new HIKGigECamera("Lower Camera");                                           //  하부 비전 카메라
            //Camera_LowRes = new GrabLinkMultiCamCamera("LaserCamera Low-Res");                          //  하부 비전 카메라
            Camera_Lower.Create();
            Camera_Lower.Owner = this;
            Parts.Add(Camera_Lower);

            Camera_Upper = new HIKGigECamera("Upper Camera");
            //Camera = new GrabLinkMultiCamCamera("LaserCamera");
            Camera_Upper.Create();
            Camera_Upper.Owner = this;
            Parts.Add(Camera_Upper);

            waferProbeAlignParameter = new WaferProbeAlignParameter("WaferProbeAlign Parameter");
            waferProbeAlignParameter.Create();
            waferProbeAlignParameter.Owner = this;
            waferProbeAlignParameter.Axes = Stage.Axes;
            Parts.Add(waferProbeAlignParameter);

            visionCalibrator_Lower = new VisionCalibrator("VisionCalibrator Lower");
            visionCalibrator_Lower.Create();
            visionCalibrator_Lower.Owner = this;
            visionCalibrator_Lower.Camera = Camera_Lower;
            visionCalibrator_Lower.UvwzxyzStage = Stage;
            visionCalibrator_Lower.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(visionCalibrator_Lower);

            visionCalibrator_Upper = new VisionCalibrator("VisionCalibrator Upper");
            visionCalibrator_Upper.Create();
            visionCalibrator_Upper.Owner = this;
            visionCalibrator_Upper.Camera = Camera_Upper;
            visionCalibrator_Upper.UvwzxyzStage = Stage;
            visionCalibrator_Upper.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(visionCalibrator_Upper);

            visionCompensator_Upper = new VisionCompensator("Vision Compensator Upper");
            visionCompensator_Upper.Create();
            visionCompensator_Upper.Owner = this;
            visionCompensator_Upper.Camera = Camera_Upper;
            visionCompensator_Upper.Stage = Stage;
            visionCompensator_Upper.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(visionCompensator_Upper);

            laserPitchMoveShotter = new LaserPitchMoveShotter("LaserPitchMove Shotter");
            laserPitchMoveShotter.Create();
            laserPitchMoveShotter.Owner = this;
            laserPitchMoveShotter.Camera = Camera_Upper;
            laserPitchMoveShotter.Stage = Stage;
            Parts.Add(laserPitchMoveShotter);

            autoFocuser_Lower = new AutoFocuser("AutoFocuser Lower");
            autoFocuser_Lower.Create();
            autoFocuser_Lower.Owner = this;
            autoFocuser_Lower.Camera = Camera_Lower;
            autoFocuser_Lower.Motion = Stage;
            autoFocuser_Lower.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(autoFocuser_Lower);

            autoFocuser_Upper = new AutoFocuser("AutoFocuser Upper");
            autoFocuser_Upper.Create();
            autoFocuser_Upper.Owner = this;
            autoFocuser_Upper.Camera = Camera_Upper;
            autoFocuser_Upper.Motion = Stage;
            autoFocuser_Upper.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(autoFocuser_Upper);

            scannerCompensator = new ScannerCompensator("Scanner Compensator Upper");
            scannerCompensator.Create();
            scannerCompensator.Owner = this;
            scannerCompensator.Camera = Camera_Upper;
            scannerCompensator.Stage = Stage;
            scannerCompensator.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(scannerCompensator);

            jigAligner_Lower = new JigAligner("JigAligner Lower");
            jigAligner_Lower.Create();
            jigAligner_Lower.Owner = this;
            jigAligner_Lower.Camera = Camera_Lower;
            jigAligner_Lower.Stage = Stage;
            jigAligner_Lower.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(jigAligner_Lower);

            jigAligner_Upper = new JigAligner("JigAligner Upper");
            jigAligner_Upper.Create();
            jigAligner_Upper.Owner = this;
            jigAligner_Upper.Camera = Camera_Upper;
            jigAligner_Upper.Stage = Stage;
            jigAligner_Upper.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(jigAligner_Upper);

            //PosParam_Dispenser = GetConfigData();     //  요건 나중에

            //ACS_Motion_isSimulationMode = false;
            //ACS_Motion = new Api();

            Recipe = new WaferProbeAlignRecipe(this);

            return ret;
        }

        public override void SetConfigData(object configData)
        {
            Config = configData as WaferProbeAlignConfig;

            if (Config == null)
                Config = new WaferProbeAlignConfig();

            Config.Init();

            if (Config.ParamConfig != null)
            {
                ParamConfig = Config.ParamConfig;

                waferProbeAlignParameter.Config = ParamConfig;
            }

            Stage.Config = this.Config.StageConfig;

            Camera_Upper.Config = Config.CameraConfig_Upper;                        //  상부 카메라
            Camera_Lower.Config = Config.CameraConfig_Lower;                        //  하부 카메라
            visionCalibrator_Upper.Config = Config.VisonCalibratorConfig_Upper;
            visionCalibrator_Lower.Config = Config.VisonCalibratorConfig_Lower;
            visionCompensator_Upper.Config = Config.VisionCompensatorConfig;
            autoFocuser_Upper.Config = Config.AutoFocuserConfig_Upper;
            autoFocuser_Lower.Config = Config.AutoFocuserConfig_Lower;
            scannerCompensator.Config = Config.ScannerCompensatorConfig;
            laserPitchMoveShotter.Config = Config.LaserPitchMoveShotterConfig;

            Stage.UpdateDirection();
            //jigAligner.Config = Config.JigAlignerConfig;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override void UpdateConfigData()
        {
            Camera_Upper.Config = Config.CameraConfig_Upper;
            Camera_Lower.Config = Config.CameraConfig_Lower;
            visionCalibrator_Upper.Config = Config.VisonCalibratorConfig_Upper;
            visionCalibrator_Lower.Config = Config.VisonCalibratorConfig_Lower;
            visionCompensator_Upper.Config = Config.VisionCompensatorConfig;
            autoFocuser_Upper.Config = Config.AutoFocuserConfig_Upper;
            autoFocuser_Lower.Config = Config.AutoFocuserConfig_Lower;
            scannerCompensator.Config = Config.ScannerCompensatorConfig;
            laserPitchMoveShotter.Config = Config.LaserPitchMoveShotterConfig;
            //jigAligner.Config = Config.JigAlignerConfig;

            base.UpdateConfigData();
        }

        public override void SetRecipeData(object recipeData)
        {
            WaferProbeAlignRecipe recipe = recipeData as WaferProbeAlignRecipe;
            if (recipe == null)
            {
                recipe = new WaferProbeAlignRecipe(this);
            }


            Recipe = recipe;
            Recipe.Init(this);

            visionCalibrator_Upper.Recipe = Recipe.VisionCalibratorRecipe_Upper;
            visionCalibrator_Lower.Recipe = Recipe.VisionCalibratorRecipe_Lower;
            //scannerCompensator.Recipe = Recipe.scannerCompensatorRecipe;
            jigAligner_Upper.Recipe = Recipe.jigAlignerRecipe_Upper;
            jigAligner_Lower.Recipe = Recipe.jigAlignerRecipe_Lower;

            base.SetRecipeData(recipeData);
        }

        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void UpdateRecipeData()
        {
            visionCalibrator_Upper.Recipe = Recipe.VisionCalibratorRecipe_Upper;
            visionCalibrator_Lower.Recipe = Recipe.VisionCalibratorRecipe_Lower;
            //scannerCompensator.Recipe = Recipe.scannerCompensatorRecipe;
            jigAligner_Upper.Recipe = Recipe.jigAlignerRecipe_Upper;
            jigAligner_Lower.Recipe = Recipe.jigAlignerRecipe_Lower;

            base.UpdateRecipeData();
        }

        public override void Close()
        {
            base.Close();

            if (Stage != null)
            {
                Stage.Close();
            }

            if (Camera_Upper != null)
            {
                Camera_Upper.Close();
            }

            if (Camera_Lower != null)
            {
                Camera_Lower.Close();
            }

            //if (ACS_Motion != null)
            //{
            //    ACS_Motion.CloseComm();
            //}
        }
        #endregion


        #region Event Handler

        private void Timer_MainWork_Func(object sender, EventArgs e)
        {
            //PEGIncrementalCycle();

            Run_WaferProbeAlign_Main_Func();
            Run_Wafer_ProbeCard_Packing_Func();
            Run_Wafer_ProbeCard_Unacking_Func();
        }

        private void Timer_SubWork_Func(object sender, EventArgs e)
        {
            //WaferAlignCycle();
            //LaserOneShotCycle();

            timer_SubWork.Enabled = false;

            Run_Safety_Position_Move_Func();
            Run_WaferLoading_Ready_Func();
            Run_ProbeCard_Loading_Ready_Func();
            Run_ProbeCard_Locking_Func();
            Run_WaferProbeCard_Unpacking_Ready_Func();

            Run_WaferProbeAlign_ErrorCheck_Func();
            Run_PAK_AirLine_Check_Func();

            timer_SubWork.Enabled = true;
        }

        private void Timer_ProductAlign_Func(object sender, EventArgs e)
        {
            if (!m_bAlignVisionThread_Use)
            {
                
            }
        }

        private void Timer_MotionHome_Func(object sender, EventArgs e)
        {
            Run_Home_Func();
        }

        private void Timer_ReticleGlass_Func(object sender, EventArgs e)
        {
            Run_ReticleCheck_UpperCam_Func();
            Run_ReticleCheck_LowerCam_Func();
        }        

        public void forThread_WaferProbeAlignCycle()
        {
            //  Main Cycle
            Run_WaferProbeAlign_Main_Func();
        }

        public void forThread_UpdateCycle()
        {
            //  Update Cycle
            //  Run_LaserComm_Func();
            //  Run_PowerMeterComm_Func();
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

            //foreach (XyztPositionData position in Config.Positions)
            //foreach (XyzztPositionData position in Config.Positions)
            foreach (UvwzxyzPositionData position in Config.Positions)
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


        #region Wafer ProbeCard Align Cycle Function

        private void Run_WaferProbeAlign_Main_Func()
        {
            string m_strTemp = "";

            double theta = 0.0;
            double deltaX = 0.0;
            double deltaY = 0.0;
            double dMmPer1Deg = 0.0;
            double dCenterUVWDistance = 0.0;
            int invertAngle = 1;

            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            switch (m_nWaferProbeAlign_MainStep)
            {
                case (int)WaferProbeAlign_Step.Start:
                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 시작");

                    //  얼라인 시작 시간
                    Equipment.AlignStart_Time = DateTime.Now.ToString("hh_mm_ss");

                    Equipment.MachineStop_byAlarm = false;

                    //m_bFindFirstAlignMarkOnly = false;

                    m_nManualPacking_Step = (int)ManualPackingStep.NONE;

                    m_nWaferAlign_Count = 0;

                    m_bWafer_ThetaAlign_OK = false;
                    m_bWafer_XYAlign_OK = false;
                    m_bProbeCard_TiltCheck_OK = false;
                    m_bProbeCard_XYAlign_OK = false;

                    m_dProbeCardAlign_CorrectionAngle = 0.0;
                    m_dWaferAlign_CorrectionAngle = 0.0;

                    m_dMyVisionPos_Distance_X = 0.0;
                    m_dMyVisionPos_Distance_Y = 0.0;

                    m_strProbeCard_TiltData_for_Display = "- - -";
                    m_strWafer_TiltData_for_Display = "- - -";

                    for (int nPos = 0; nPos < 3; nPos++)
                    {
                        AlignmentErrorCheck_Position[nPos].X = 0.0;
                        AlignmentErrorCheck_Position[nPos].Y = 0.0;

                        for (int nSide = 0; nSide < 2; nSide++)
                        {
                            AlignmentErrorCheck_Status[nPos, nSide] = false;

                            AlignmentErrorCheck_MarkPosition[nPos, nSide].X = 0.0;
                            AlignmentErrorCheck_MarkPosition[nPos, nSide].Y = 0.0;
                        }
                    }

                    m_dWafer_ProbeCard_PackingPos_Axis_U = -1;
                    m_dWafer_ProbeCard_PackingPos_Axis_V = -1;
                    m_dWafer_ProbeCard_PackingPos_Axis_W = -1;

                    m_dWafer_ProbeCard_AlignPos_Axis_U = -1;
                    m_dWafer_ProbeCard_AlignPos_Axis_V = -1;
                    m_dWafer_ProbeCard_AlignPos_Axis_W = -1;

                    Equipment.Vision_SpiralMove_Use = true;

                    if (m_bProbeCard_TiltCheck_Only && !m_bWafer_Align_Only)           //  ProbeCard Tilt Check Only
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos;
                    }
                    else if (!m_bProbeCard_TiltCheck_Only && m_bWafer_Align_Only)      //  Wafer Align Only
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.Align_Condition_Check;
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.Align_Condition_Check;
                    }
                    break;


                case (int)WaferProbeAlign_Step.Align_Condition_Check:               //  Align 조건 확인 (Thin Chuck Exist)

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 시작 조건 확인");

                    if (!waferProbeAlignParameter.DI_ThinChuck_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (씬-척이 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Thin Chuck 이 감지되지 않음.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 시작 조건 OK. (씬-척 감지)");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ThinChuck_Vacuum_On;
                    }
                    break;


                case (int)WaferProbeAlign_Step.ThinChuck_Vacuum_On:                                 //  Thin Chuck Vacuum On

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "씬-척 공압 On");

                    waferProbeAlignParameter.DO_ThinChuck_Vacuum(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ThinChuck_Vacuum_On_Check;
                    break;


                case (int)WaferProbeAlign_Step.ThinChuck_Vacuum_On_Check:                           //  Thin Chuck Vacuum On 확인

                    if ((Config.ParamConfig.ThinChuck_VacuumSignal_Usage && waferProbeAlignParameter.DI_ThinChuck_VacuumCheck()) ||
                        (!Config.ParamConfig.ThinChuck_VacuumSignal_Usage && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.ThinChuck_VacuumSignal_Time)))          //  임시로 3초 (옵션 처리 하자)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "씬-척 공압 On 확인");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.Wafer_Vacuum_On;
                    }
                    break;


                case (int)WaferProbeAlign_Step.Wafer_Vacuum_On:                                     //  Wafer Vacuum On

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 공압 On");

                    waferProbeAlignParameter.DO_Wafer_Vacuum(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.Wafer_Vacuum_On_Check;
                    break;


                case (int)WaferProbeAlign_Step.Wafer_Vacuum_On_Check:                               //  Wafer Vacuum On 확인

                    if ((Config.ParamConfig.Wafer_VacuumSignal_Usage && waferProbeAlignParameter.DI_Wafer_VacuumCheck()) ||
                        (!Config.ParamConfig.Wafer_VacuumSignal_Usage && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.Wafer_VacuumSignal_Time)))          //  임시로 3초 (옵션 처리 하자)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 공압 On 확인");

                        if (Config.ParamConfig.Pak_AirLineCheck_Usage)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "PAK 공압 라인 막힘 점검 모드");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__PAK_LeakCheck_Start;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼가 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Wafer 가 감지되지 않음.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__PAK_LeakCheck_Start:                                      //  PAK 공압 라인 막힘 검사 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "PAK 공압 라인 막힘 점검 시작");

                    m_bPAK_AirLineCheck_Complete = false;
                    m_bPAK_AirLineCheck_OK = true;

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Start;
                    timer_SubWork.Enabled = true;

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__PAK_LeakCheck_Complete;
                    break;


                case (int)WaferProbeAlign_Step.__PAK_LeakCheck_Complete:                                      //  PAK 공압 라인 막힘 검사 완료

                    if (m_bPAK_AirLineCheck_Complete && (m_nPAK_AirLine_Check_Step == (int)PAK_AirLine_Check_Step.None))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "PAK 공압 라인 막힘 점검 완료");

                        if (m_bPAK_AirLineCheck_OK)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos;
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (PAK 공압 라인 막힘)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            timer_MainWork.Enabled = false;
                            timer_SubWork.Enabled = false;

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                            m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.None;

                            MessageBox.Show("PAK 공압라인 막힘 검사 완료.\r\n\r\n[ NG ]\r\n\r\n(PAK 관로 막힘)", "Error");
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 20000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (PAK 공압 라인 막힘 점검 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;
                        timer_SubWork.Enabled = false;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.None;

                        MessageBox.Show("PAK 공압라인 막힘 점검 실패. (Time Out)", "Error");
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 안전 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 안전 위치로 이동 완료");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.UVW_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 안전 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.UVW_Move_WaferLoadingPos:                             //  UVW 축, Wafer Loading 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "얼라인 스테이지, 웨이퍼 로딩 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.UVW_Move_WaferLoadingPos_DoneCheck;
                    break;


                case (int)WaferProbeAlign_Step.UVW_Move_WaferLoadingPos_DoneCheck:                   //  UVW 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "얼라인 스테이지, 웨이퍼 로딩 위치로 이동 완료");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ElevZ_Move_WaferAlignPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (얼라인 스테이지, 웨이퍼 로딩 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("UVW 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.ElevZ_Move_WaferAlignPos:                            //  Elev. Z 축, Wafer Align 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "씬-척 엘리베이터, 웨이퍼 얼라인 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  이동 할 Elev. Z 축 높이가 얼라인 한계 높이를 초과하는 지 체크
                    if (waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (씬-척 엘리베이터, 웨이퍼 얼라인 높이 한계 위치를 초과하여 이동하려는 시도)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Align 한계 높이를 초과하여 이동하려는 시도로 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ElevZ_Move_WaferAlignPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.ElevZ_Move_WaferAlignPos_DoneCheck:                  //  Elev. Z 축, Wafer Align 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "씬-척 엘리베이터, 웨이퍼 얼라인 높이로 이동 완료");

                        if (m_bProbeCard_TiltCheck_Only && !m_bWafer_Align_Only)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_Tilt_Check_Start;
                        }
                        else if (!m_bProbeCard_TiltCheck_Only && m_bWafer_Align_Only)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Start;
                        }
                        else
                        {
                            if (Config.ParamConfig.ProbrCard_Align_Use)
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_Tilt_Check_Start;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Start;
                            }
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (씬-척 엘리베이터, 웨이퍼 얼라인 높이로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Align 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__ProbeCard_Tilt_Check_Start:                                  //  Probe Card Tilt Check 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 파트 시작");

                    m_nWaferAlign_Count = 0;

                    m_dProbeCard_TiltData = 0.0;
                    m_bProbeCard_TiltCheck_OK = false;

                    if (Camera_Upper != null)
                    {
                        Camera_Upper.StartLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_UpperVision_LightValue, 1);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
                    }

                    if (Camera_Lower != null)
                    {
                        Camera_Lower.StopLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_LowerVision_LightValue, 2);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(false, 2);       //  Wafer 조명
                    }

                    //  첫번째 얼라인 마크 위치 (최초 이동 좌표, 2회 이상 부터는 계산된 위치를 넣어줌)
                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    m_dMy1stMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                    m_dMy1stMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                    m_dMy2ndMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                    m_dMy2ndMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    //  얼라인 위치 간 거리
                    m_dMyVisionPos_Distance_X = m_dMy2ndMarkVisionPos_X - m_dMy1stMarkVisionPos_X;
                    m_dMyVisionPos_Distance_Y = m_dMy2ndMarkVisionPos_Y - m_dMy1stMarkVisionPos_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_TopMarkFind_Ready;
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_TopMarkFind_Ready:                        //  Top 위치 얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, TOP 위치 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    jigAligner_Upper.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                    jigAligner_Upper.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter:              //  VisionXYZ 축, ProbeCard Tilt Check (Top, Center) 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Upper.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Upper.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 프로브 카드 TOP 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter_DoneCheck:    //  VisionXYZ 축, ProbeCard Tilt Check (Top, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치로 이동 완료");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 프로브 카드 TOP 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Probe Card Top Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter_StableTime:           //  VisionXYZ 축, ProbeCard Tilt Check (Top, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치로 이동 후 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_TopMarkFind;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_TopMarkFind:                                   //  Top 위치 얼라인 마크 찾기

                    //  Probe Card 를 Align 하는 것이므로 Upper Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = false;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_MAIN);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Upper.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bProbeCard_TiltCheck_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_TopMarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_TopMarkResultCheck:                            //  Top 위치 얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkFind_Ready;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 TOP 위치에서 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_MainWork.Enabled = false;

                            MessageBox.Show("상부 카메라, 1번 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_TopCenter;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkFind_Ready;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkFind_Ready:                        //  Bottom 위치 얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, BOTTOM 위치 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                    //jigAligner_Lower.m_AlignPositions[0].X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X + m_dMyVisionPos_Distance_X;
                    //jigAligner_Lower.m_AlignPositions[0].Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y + m_dMyVisionPos_Distance_Y;
                    jigAligner_Upper.m_AlignPositions[0].X = MC_Func.MC_GetEncPos((int)nAxis.X) + m_dMyVisionPos_Distance_X;
                    jigAligner_Upper.m_AlignPositions[0].Y = MC_Func.MC_GetEncPos((int)nAxis.Y) + m_dMyVisionPos_Distance_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter:              //  VisionXYZ 축, ProbeCard Tilt Check (Bottom, Center) 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Upper.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Upper.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 프로브 카드 BOTTOM 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter_DoneCheck:               //  VisionXYZ 축, ProbeCard Tilt Check (Bottom, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치로 이동 완료");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 프로브 카드 BOTTOM 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Bottom Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter_StableTime:           //  VisionXYZ 축, ProbeCard Tilt Check (Bottom, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치로 이동 후 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkFind;
                        }
                        else
                        {                            
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_Tilt_Check_Complete;
                        }                        
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkFind:                                   //  Bottom 위치 얼라인 마크 찾기

                    //  Probe Card 를 Align 하는 것이므로 Upper Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치에서 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = false;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_MAIN);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Upper.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bProbeCard_TiltCheck_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Check_BottomMarkResultCheck:                            //  Bottom 위치 얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치에서 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_SECONDMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_SECONDMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Data_Calc;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 BOTTOM 위치에서 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_MainWork.Enabled = false;

                            MessageBox.Show("상부 카메라, 2번 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치에서 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_SECONDMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_SECONDMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 BOTTOM 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCard_Tilt_CheckPos_BottomCenter;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCard_Tilt_Data_Calc;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCard_Tilt_Data_Calc:                                            //  Tilt 데이터 계산

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 회전량 계산");

                    m_dProbeCardAlign_CorrectionAngle = 0.0;

                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X - m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_SECONDMARKPOS].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y - m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_SECONDMARKPOS].Y;

                    if (Config.ParamConfig.Align_AngleInvert == true)
                    {
                        invertAngle *= -1;
                    }

                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        m_dProbeCardAlign_CorrectionAngle = Math.Atan(deltaY / deltaX) * (180 / Math.PI) * invertAngle;
                    }
                    else
                    {
                        m_dProbeCardAlign_CorrectionAngle = -Math.Atan(deltaX / deltaY) * (180 / Math.PI) * invertAngle;
                    }

                    m_dProbeCardAlign_CorrectionAngle = Math.Truncate(m_dProbeCardAlign_CorrectionAngle * 1000000) / 1000000;

                    //  2023. 07. 18.  SCH : 마곡 LGD SLO-400 장비에는 10배 하도록 코드 변경해야 함. (광주 광기술원 SLO-300 장비에는 주석처리)
                    //m_dWaferAlign_CorrectionAngle *= 10.0;                                            //  엉뚱한 변수에 10배를 했군.... theta 값에 10배를 했어야 했는데... 그래서 LG 담당자가 각도가 별로 안틀어진다고 했군...

                    //  판정. (OK 범위 이내가 아니면 Retry)
                    if (Math.Abs(m_dProbeCardAlign_CorrectionAngle) <= 5.0)                 //  5도 이내이면 각도 계산은 되었다고 본다.
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 프로브 카드 회전량 계산 완료");

                        m_strProbeCard_TiltData_for_Display = m_dProbeCardAlign_CorrectionAngle.ToString();

                        m_bProbeCard_TiltCheck_OK = true;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_Tilt_Check_Complete;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 회전량 계산 결과 5도 이상 틀어짐)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        m_strProbeCard_TiltData_for_Display = "각도 오류 (5˚이상)";
                        m_strWafer_TiltData_for_Display = "- - -";

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bProbeCard_TiltCheck_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MessageBox.Show("Probe Card Tilt 계산 실패\r\n\r\n[틀어진 각도가 너무 큼. (5˚ 이상)]");
                    }
                    break;
                                        

                case (int)WaferProbeAlign_Step.__ProbeCard_Tilt_Check_Complete:                                  //  Probe Card Tilt Check 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 틀어짐 각도 측정, 틀어짐 각도 측정 파트 완료");

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_XYAlign_Start;
                    break;


                case (int)WaferProbeAlign_Step.__ProbeCard_XYAlign_Start:                                  //  ProbeCard XY Align 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, XY 위치 보정 파트 시작");

                    m_nWaferAlign_Count = 0;
                    m_bProbeCard_XYAlign_OK = false;

                    //  첫번째 얼라인 마크 위치 (최초 이동 좌표, 2회 이상 부터는 계산된 위치를 넣어줌)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    //m_dMy1stMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                    //m_dMy1stMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlign_TopMarkFind_Ready;
                    break;


                case (int)WaferProbeAlign_Step.ProbeCardXYAlign_TopMarkFind_Ready:                        //  Top 위치 얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, TOP 위치 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    jigAligner_Upper.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                    jigAligner_Upper.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter:              //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Upper.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Upper.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 프로브 카드 TOP 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter_DoneCheck:    //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 완료");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Top XY Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter_StableTime:           //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 후 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlign_TopMarkFind;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_XYAlign_Complete;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCardXYAlign_TopMarkFind:                                   //  Top 위치 얼라인 마크 찾기

                    //  Probe Card 를 Align 하는 것이므로 Upper Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = false;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_MAIN);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Upper.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bProbeCard_XYAlign_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlign_TopMarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCardXYAlign_TopMarkResultCheck:                            //  Top 위치 얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlignData_Calc;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 TOP 위치에서 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_MainWork.Enabled = false;

                            MessageBox.Show("상부 카메라, 1번 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlignData_Calc;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCardXYAlignData_Calc:                                            //  XY 얼라인 데이터 계산

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 XY 보정량 계산");

                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X - jigAligner_Upper.m_AlignPositions[0].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y - jigAligner_Upper.m_AlignPositions[0].Y;

                    //  2023. 07. 18.  SCH : 마곡 LGD SLO-400 장비에는 10배 하도록 코드 변경해야 함. (광주 광기술원 SLO-300 장비에는 주석처리)
                    //m_dWaferAlign_CorrectionAngle *= 10.0;                                            //  엉뚱한 변수에 10배를 했군.... theta 값에 10배를 했어야 했는데... 그래서 LG 담당자가 각도가 별로 안틀어진다고 했군...

                    //  판정. (OK 범위 이내가 아니면 Retry)
                    if ((Math.Abs(deltaX) <= Config.ParamConfig.Align_Vision_Allowable_XY) &&
                        (Math.Abs(deltaY) <= Config.ParamConfig.Align_Vision_Allowable_XY))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치의 얼라인 마크가 보정 범위 이내이므로 OK");

                        m_bProbeCard_XYAlign_OK = true;

                        //  현재 위치를 첫번째 얼라인 마크 위치로 한다.
                        m_dMy1stMarkVisionPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                        m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__ProbeCard_XYAlign_Complete;
                    }
                    else
                    {
                        m_nWaferAlign_Count++;

                        if (m_nWaferAlign_Count < Config.ParamConfig.Align_Retries)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치의 얼라인 마크가 보정 범위를 초과하므로 NG");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlign_CorrectionMove;
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치의 얼라인 마크 보정 회수 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            //if (!m_bAlignVisionThread_Use)
                            {
                                timer_MainWork.Enabled = false;
                            }

                            m_bMyWaferAlign_fromManualMode = false;
                            m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                            m_bProbeCard_XYAlign_OK = false;
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                            MessageBox.Show("Probe Card 얼라인 실패\r\n\r\n[얼라인 회수 초과]");
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.ProbeCardXYAlign_CorrectionMove:                                      //  Top 위치 XY 보정 이동 (카메라 Center 로)

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치의 얼라인 마크 보정 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    //  Vision XY 방향 이동량 계산
                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X - jigAligner_Upper.m_AlignPositions[0].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y - jigAligner_Upper.m_AlignPositions[0].Y;

                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.X] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X) - deltaX;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.Y] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y) - deltaY;

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  절대 위치 이동
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.X],
                                                                                        m_dSpeed_ElvXY,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.Y],
                                                                                        m_dSpeed_ElvXY,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.ProbeCardXYAlign_CorrectionMove_DoneCheck;
                    break;


                case (int)WaferProbeAlign_Step.ProbeCardXYAlign_CorrectionMove_DoneCheck:                             //  Top 위치 XY 보정 이동 완료 확인
                    if ((TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치의 얼라인 마크 보정 이동 완료");

                        //  1번 마크 위치 다시 계산
                        //  - X 위치 : 2번 마크 위치에서 마크 OffsetX 만큼
                        //  - Y 위치 : 1번 마크 위치와 2번 마크 위치의 중간

                        //m_dMy1stMarkVisionPos_X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X + Config.ParamConfig.AlignMark_2nd_Offset_X;
                        //m_dMy1stMarkVisionPos_Y = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y) / 2.0 + Config.ParamConfig.AlignMark_2nd_Offset_Y;
                        //m_dMy1stMarkVisionPos_X = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X) / 2.0 - m_dMyVisionPos_Distance_X;
                        //m_dMy1stMarkVisionPos_Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y - m_dMyVisionPos_Distance_Y;                        
                        //m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos( (int)nAxis.Y ) ;

                        //m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind_Ready;       //   VisionXYZ_Move_WaferAlignPos_TopCenter;



                        //  마크 1개만 찾기 위한 설정
                        m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                        //  0번 인덱스 위치 (마크 찾을 위치)
                        m_dMy1stMarkVisionPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                        m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                        jigAligner_Upper.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                        jigAligner_Upper.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ProbeCardXYAlignPos_TopCenter;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치의 얼라인 마크 보정 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__ProbeCard_XYAlign_Complete:                                  //  Wafer XY Align 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "프로브 카드 XY 위치 보정, XY 위치 보정 파트 완료");

                    //m_bWaferAlign_OK = true;

                    if (m_bProbeCard_TiltCheck_Only)                                        //  Probe Card tilt 확인만 할 경우, 여기서 종료
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2;
                    }
                    else
                    {
                        if (m_bProbeCard_TiltCheck_OK && m_bProbeCard_XYAlign_OK)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Start;
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (프로브 카드 회전량 확인 또는 XY 위치 보정 실패)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            timer_MainWork.Enabled = false;

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                            m_bProbeCard_TiltCheck_OK = false;
                            m_bProbeCard_XYAlign_OK = false;

                            MessageBox.Show("Probe Card Tilt 및 XY Align 확인 실패.", "Error");
                        }
                    }                    
                    break;


                case (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Start:                                  //  Wafer Theta Align 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 파트 시작");

                    m_nWaferAlign_Count = 0;

                    m_bWafer_ThetaAlign_OK = false;

                    if (Camera_Upper != null)
                    {
                        Camera_Upper.StopLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_UpperVision_LightValue, 1);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(false, 1);       //  Probe Card 조명
                    }

                    if (Camera_Lower != null)
                    {
                        Camera_Lower.StartLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_LowerVision_LightValue, 2);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
                    }

                    if (Config.ParamConfig.ProbrCard_Align_Use && !m_bProbeCard_TiltCheck_Only && !m_bWafer_Align_Only )
                    {
                        //  Probe Card Align 하면서 구해진 위치값
                        //m_dMy1stMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        //m_dMy1stMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                        m_dMy1stMarkVisionPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                        m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                        m_dMy2ndMarkVisionPos_X = m_dMyVisionPos_Distance_X + m_dMy1stMarkVisionPos_X;
                        m_dMy2ndMarkVisionPos_Y = m_dMyVisionPos_Distance_Y + m_dMy1stMarkVisionPos_Y;

                        //  얼라인 위치 간 거리
                        m_dMyVisionPos_Distance_X = m_dMy2ndMarkVisionPos_X - m_dMy1stMarkVisionPos_X;
                        m_dMyVisionPos_Distance_Y = m_dMy2ndMarkVisionPos_Y - m_dMy1stMarkVisionPos_Y;
                    }
                    else
                    {
                        m_dProbeCardAlign_CorrectionAngle = 0.0;

                        //  첫번째 얼라인 마크 위치 (최초 이동 좌표, 2회 이상 부터는 계산된 위치를 넣어줌)
                        waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                        m_dMy1stMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        m_dMy1stMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                        waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                        m_dMy2ndMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        m_dMy2ndMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                        //  얼라인 위치 간 거리
                        m_dMyVisionPos_Distance_X = m_dMy2ndMarkVisionPos_X - m_dMy1stMarkVisionPos_X;
                        m_dMyVisionPos_Distance_Y = m_dMy2ndMarkVisionPos_Y - m_dMy1stMarkVisionPos_Y;
                    }

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind_Ready;
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind_Ready:                        //  Top 위치 얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, TOP 위치 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    //jigAligner_Lower.m_AlignPositions[0].X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                    //jigAligner_Lower.m_AlignPositions[0].Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];
                    jigAligner_Lower.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                    jigAligner_Lower.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter:              //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Lower.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Lower.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 웨이퍼 TOP 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter_DoneCheck:    //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치로 이동 완료");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Top Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter_StableTime:           //  VisionXYZ 축, Wafer Align (Top, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치로 이동 후 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind:                                   //  Top 위치 얼라인 마크 찾기

                    //  Wafer 를 Align 하는 것이므로 Lower Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = true;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_MAIN);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Lower.Work();
                    }                    

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bWafer_ThetaAlign_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_TopMarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_TopMarkResultCheck:                            //  Top 위치 얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_BottomMarkFind_Ready;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 TOP 위치에서 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_MainWork.Enabled = false;

                            MessageBox.Show("하부 카메라, 1번 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_BottomMarkFind_Ready;
                        }
                    }                    
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_BottomMarkFind_Ready:                        //  Bottom 위치 얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, BOTTOM 위치 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                    //jigAligner_Lower.m_AlignPositions[0].X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X + m_dMyVisionPos_Distance_X;
                    //jigAligner_Lower.m_AlignPositions[0].Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y + m_dMyVisionPos_Distance_Y;
                    jigAligner_Lower.m_AlignPositions[0].X = MC_Func.MC_GetEncPos((int)nAxis.X) + m_dMyVisionPos_Distance_X;
                    jigAligner_Lower.m_AlignPositions[0].Y = MC_Func.MC_GetEncPos((int)nAxis.Y) + m_dMyVisionPos_Distance_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter:              //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Lower.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Lower.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 웨이퍼 BOTTOM 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter_DoneCheck:    //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치로 이동 완료");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 BOTTOM 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Bottom Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter_StableTime:           //  VisionXYZ 축, Wafer Align (Bottom, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치로 이동 후 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_BottomMarkFind;
                        }
                        else
                        {                            
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Complete;
                        }                        
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_BottomMarkFind:                                   //  Bottom 위치 얼라인 마크 찾기

                    //  Wafer 를 Align 하는 것이므로 Lower Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치에서 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = true;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_MAIN);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Lower.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bWafer_ThetaAlign_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_BottomMarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_BottomMarkResultCheck:                            //  Bottom 위치 얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치에서 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlignData_Calc;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 BOTTOM 위치에서 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_MainWork.Enabled = false;

                            MessageBox.Show("하부 카메라, 2번 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치에서 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 BOTTOM 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_BottomCenter;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlignData_Calc;
                        }
                    }                    
                    break;


                case (int)WaferProbeAlign_Step.WaferAlignData_Calc:                                            //  얼라인 데이터 계산

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 회전량 계산");

                    m_dWaferAlign_CorrectionAngle = 0.0;

                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X - m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y - m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y;

                    if (Config.ParamConfig.Align_AngleInvert == true)
                    {
                        invertAngle *= -1;
                    }

                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        m_dWaferAlign_CorrectionAngle = Math.Atan(deltaY / deltaX) * (180 / Math.PI) * invertAngle;
                    }
                    else
                    {
                        m_dWaferAlign_CorrectionAngle = -Math.Atan(deltaX / deltaY) * (180 / Math.PI) * invertAngle;
                    }

                    m_dWaferAlign_CorrectionAngle = Math.Truncate(m_dWaferAlign_CorrectionAngle * 1000000) / 1000000;

                    //  2023. 07. 18.  SCH : 마곡 LGD SLO-400 장비에는 10배 하도록 코드 변경해야 함. (광주 광기술원 SLO-300 장비에는 주석처리)
                    //m_dWaferAlign_CorrectionAngle *= 10.0;                                            //  엉뚱한 변수에 10배를 했군.... theta 값에 10배를 했어야 했는데... 그래서 LG 담당자가 각도가 별로 안틀어진다고 했군...

                    //  판정. (OK 범위 이내가 아니면 Retry)
                    //if (Math.Abs(m_dWaferAlign_CorrectionAngle) <= Config.ParamConfig.Align_Vision_Allowable_Angle)
                    if ((m_dWaferAlign_CorrectionAngle >= (m_dProbeCardAlign_CorrectionAngle - Config.ParamConfig.Align_Vision_Allowable_Angle)) &&
                        (m_dWaferAlign_CorrectionAngle <= (m_dProbeCardAlign_CorrectionAngle + Config.ParamConfig.Align_Vision_Allowable_Angle)))                        
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 프로브 카드 회전 각도에서 오차범위 이내에 있음");

                        m_strWafer_TiltData_for_Display = m_dWaferAlign_CorrectionAngle.ToString();

                        m_bWafer_ThetaAlign_OK = true;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Complete;
                    }
                    else
                    {
                        m_nWaferAlign_Count++;

                        if (m_nWaferAlign_Count < Config.ParamConfig.Align_Retries)
                        {
                            m_strWafer_TiltData_for_Display = m_dWaferAlign_CorrectionAngle.ToString();

                            //  계산된 각도가 너무 크면 다시 시도한다. (대충 5도 이상?)
                            if (Math.Abs(m_dWaferAlign_CorrectionAngle) >= 5.0)
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 계산된 각도에 문제가 있음. (5도 이상 틀어짐)");

                                jigAligner_Lower.m_AlignPositions[0].X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X;
                                jigAligner_Lower.m_AlignPositions[0].Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y;

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter;
                            }
                            else
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 프로브 카드 회전 각도에서 오차범위 초과");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_ThetaMove;
                            }
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 틀어짐 각도 보정 회수 초과)");

                            m_strWafer_TiltData_for_Display = "얼라인 회수 초과";

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            //if (!m_bAlignVisionThread_Use)
                            {
                                timer_MainWork.Enabled = false;
                            }

                            m_bMyWaferAlign_fromManualMode = false;
                            m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                            m_bWafer_ThetaAlign_OK = false;
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                            MessageBox.Show("Wafer 얼라인 실패\r\n\r\n[얼라인 회수 초과]");
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_ThetaMove:                                      //  UVW Stage Theta 회전
                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 보정 이동 시작. (Theta)");

                    //  UVW 위치값 계산
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U) + (m_dWaferAlign_CorrectionAngle * -1.0);
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V) + (m_dWaferAlign_CorrectionAngle * -1.0);
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W) - (m_dWaferAlign_CorrectionAngle * -1.0);

                    //  UVW Stage Center 에서 UVW 축 회전체 까지의 거리
                    dCenterUVWDistance = Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance == 0 ? 70 : Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance;

                    //  UVW Stage 가 1도 회전하는 데 필요한 축 이동량
                    dMmPer1Deg = Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg == 0 ? 1.221668451 : Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg;

                    //  UVW Stage 1도 회전 이동량 다시 계산
                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X - m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y - m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y;

                    if (Math.Abs(deltaX) > Math.Abs(deltaY))        //  가로 방향 얼라인
                    {
                        dMmPer1Deg = (Math.Abs(deltaX) * dMmPer1Deg) / dCenterUVWDistance;
                    }
                    else                                            //  세로 방향 얼라인
                    {
                        dMmPer1Deg = (Math.Abs(deltaY) * dMmPer1Deg) / dCenterUVWDistance;
                    }

                    m_dWaferAlign_CorrectionAngle = m_dWaferAlign_CorrectionAngle - m_dProbeCardAlign_CorrectionAngle;

                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U) + ((m_dWaferAlign_CorrectionAngle * -1.0) / (dMmPer1Deg / 2.0));
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V) + ((m_dWaferAlign_CorrectionAngle * -1.0) / (dMmPer1Deg / 2.0));
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W) - ((m_dWaferAlign_CorrectionAngle * -1.0) / (dMmPer1Deg / 2.0));

                    //  Theta 축 속도 변경
                    if (Config.ParamConfig.Align_Theta_Velocity <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U] = 10.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V] = 10.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W] = 10.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Velocity;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Velocity;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Velocity;
                    }

                    if (Config.ParamConfig.Align_Theta_Accel <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W] = 100.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Accel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Accel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Accel;
                    }

                    if (Config.ParamConfig.Align_Theta_Decel <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W] = 100.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Decel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Decel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Decel;

                    }

                    //  절대 위치 이동
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_ThetaMove_DoneCheck;
                    break;


                case (int)WaferProbeAlign_Step.WaferAlign_ThetaMove_DoneCheck:                             //  UVW Stage Theta 회전 완료 확인
                    if ((TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 보정, 웨이퍼 보정 이동 완료. (Theta)");

                        //  1번 마크 위치 다시 계산
                        //  - X 위치 : 2번 마크 위치에서 마크 OffsetX 만큼
                        //  - Y 위치 : 1번 마크 위치와 2번 마크 위치의 중간

                        //m_dMy1stMarkVisionPos_X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X + Config.ParamConfig.AlignMark_2nd_Offset_X;
                        //m_dMy1stMarkVisionPos_Y = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y) / 2.0 + Config.ParamConfig.AlignMark_2nd_Offset_Y;
                        //m_dMy1stMarkVisionPos_X = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X) / 2.0 - m_dMyVisionPos_Distance_X;
                        //m_dMy1stMarkVisionPos_Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y - m_dMyVisionPos_Distance_Y;                        
                        //m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos( (int)nAxis.Y ) ;

                        //m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind_Ready;       //   VisionXYZ_Move_WaferAlignPos_TopCenter;



                        //  마크 1개만 찾기 위한 설정
                        m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                        //  0번 인덱스 위치 (마크 찾을 위치)
                        jigAligner_Lower.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                        jigAligner_Lower.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferAlignPos_TopCenter;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 틀어짐 각도 보정, 웨이퍼 보정 이동 시간 초과. (Theta))");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__Wafer_ThetaAlign_Complete:                                  //  Wafer Theta Align 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 틀어짐 각도 측정, 파트 완료");

                    //m_bWaferAlign_OK = true;

                    //m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2;
                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_XYAlign_Start;                  //  Theta Align 을 끝냈으니 XY Align 을 해야 한다.
                    break;


                case (int)WaferProbeAlign_Step.__Wafer_XYAlign_Start:                                  //  Wafer XY Align 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 파트 시작");

                    m_nWaferAlign_Count = 0;
                    m_bWafer_XYAlign_OK = false;

                    if (Config.ParamConfig.ProbrCard_Align_Use && !m_bProbeCard_TiltCheck_Only && !m_bWafer_Align_Only)
                    {

                    }
                    else
                    {
                        //  첫번째 얼라인 마크 위치 (최초 이동 좌표, 2회 이상 부터는 계산된 위치를 넣어줌)
                        waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                        m_dMy1stMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                        m_dMy1stMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];
                    }

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlign_TopMarkFind_Ready;
                    break;


                case (int)WaferProbeAlign_Step.WaferXYAlign_TopMarkFind_Ready:                        //  Top 위치 얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, TOP 위치 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    //jigAligner_Lower.m_AlignPositions[0].X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                    //jigAligner_Lower.m_AlignPositions[0].Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];
                    jigAligner_Lower.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                    jigAligner_Lower.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter:              //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Lower.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Lower.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 웨이퍼 TOP 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter_DoneCheck:    //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치로 이동 완료");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 TOP 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Top XY Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter_StableTime:           //  VisionXYZ 축, Wafer XY Align (Top, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치로 이동 후 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlign_TopMarkFind;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_XYAlign_Complete;
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferXYAlign_TopMarkFind:                                   //  Top 위치 얼라인 마크 찾기

                    //  Wafer 를 Align 하는 것이므로 Lower Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = true;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_MAIN);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Lower.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bWafer_XYAlign_OK = false;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlign_TopMarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferXYAlign_TopMarkResultCheck:                            //  Top 위치 얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                            //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter;
                            }
                            else
                            {
                                m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlignData_Calc;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 TOP 위치에서 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_MainWork.Enabled = false;

                            MessageBox.Show("하부 카메라, 1번 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 TOP 위치에서 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlignData_Calc;
                        }
                    }                    
                    break;


                case (int)WaferProbeAlign_Step.WaferXYAlignData_Calc:                                            //  XY 얼라인 데이터 계산

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 XY 보정량 계산");

                    m_dWaferAlign_CorrectionAngle = 0.0;

                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X - jigAligner_Lower.m_AlignPositions[0].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y - jigAligner_Lower.m_AlignPositions[0].Y;

                    if (Config.ParamConfig.Align_AngleInvert == true)
                    {
                        invertAngle *= -1;
                    }

                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        m_dWaferAlign_CorrectionAngle = Math.Atan(deltaY / deltaX) * (180 / Math.PI) * invertAngle;
                    }
                    else
                    {
                        m_dWaferAlign_CorrectionAngle = -Math.Atan(deltaX / deltaY) * (180 / Math.PI) * invertAngle;
                    }

                    m_dWaferAlign_CorrectionAngle = Math.Truncate(m_dWaferAlign_CorrectionAngle * 1000000) / 1000000;

                    //  2023. 07. 18.  SCH : 마곡 LGD SLO-400 장비에는 10배 하도록 코드 변경해야 함. (광주 광기술원 SLO-300 장비에는 주석처리)
                    //m_dWaferAlign_CorrectionAngle *= 10.0;                                            //  엉뚱한 변수에 10배를 했군.... theta 값에 10배를 했어야 했는데... 그래서 LG 담당자가 각도가 별로 안틀어진다고 했군...

                    //  판정. (OK 범위 이내가 아니면 Retry)
                    if ((Math.Abs(deltaX) <= Config.ParamConfig.Align_Vision_Allowable_XY) &&
                        (Math.Abs(deltaY) <= Config.ParamConfig.Align_Vision_Allowable_XY))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 XY 위치가 카메라 센터에서 오차범위 이내에 있음");

                        m_bWafer_XYAlign_OK = true;
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_XYAlign_Complete;
                    }
                    else
                    {
                        m_nWaferAlign_Count++;

                        if (m_nWaferAlign_Count < Config.ParamConfig.Align_Retries)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 XY 위치가 카메라 센터에서 오차범위 초과");

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlign_CorrectionMove;
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 XY 위치 보정, 웨이퍼 XY 위치 보정 회수 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            //if (!m_bAlignVisionThread_Use)
                            {
                                timer_MainWork.Enabled = false;
                            }

                            m_bMyWaferAlign_fromManualMode = false;
                            m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                            m_bWafer_XYAlign_OK = false;
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                            MessageBox.Show("Wafer 얼라인 실패\r\n\r\n[얼라인 회수 초과]");
                        }
                    }
                    break;


                case (int)WaferProbeAlign_Step.WaferXYAlign_CorrectionMove:                                      //  Top 위치 XY 보정 이동 (카메라 Center 로)

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 XY 위치 보정 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    //  UVW Stage XY 방향 이동량 계산
                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X - jigAligner_Lower.m_AlignPositions[0].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y - jigAligner_Lower.m_AlignPositions[0].Y;

                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U) + deltaX;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V) - deltaY;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W) - deltaY;

                    //  Theta 축 속도 변경
                    if (Config.ParamConfig.Align_Theta_Velocity <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U] = 10.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V] = 10.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W] = 10.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Velocity;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Velocity;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Velocity;
                    }

                    if (Config.ParamConfig.Align_Theta_Accel <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W] = 100.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Accel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Accel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Accel;
                    }

                    if (Config.ParamConfig.Align_Theta_Decel <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W] = 100.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Decel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Decel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Decel;

                    }

                    //  절대 위치 이동
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferXYAlign_CorrectionMove_DoneCheck;
                    break;


                case (int)WaferProbeAlign_Step.WaferXYAlign_CorrectionMove_DoneCheck:                             //  UVW Stage Theta 회전 완료 확인
                    if ((TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 웨이퍼 XY 위치 보정 이동 완료");

                        //  1번 마크 위치 다시 계산
                        //  - X 위치 : 2번 마크 위치에서 마크 OffsetX 만큼
                        //  - Y 위치 : 1번 마크 위치와 2번 마크 위치의 중간

                        //m_dMy1stMarkVisionPos_X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X + Config.ParamConfig.AlignMark_2nd_Offset_X;
                        //m_dMy1stMarkVisionPos_Y = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y) / 2.0 + Config.ParamConfig.AlignMark_2nd_Offset_Y;
                        //m_dMy1stMarkVisionPos_X = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X) / 2.0 - m_dMyVisionPos_Distance_X;
                        //m_dMy1stMarkVisionPos_Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y - m_dMyVisionPos_Distance_Y;                        
                        //m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos( (int)nAxis.Y ) ;

                        //m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.WaferAlign_TopMarkFind_Ready;       //   VisionXYZ_Move_WaferAlignPos_TopCenter;



                        //  마크 1개만 찾기 위한 설정
                        m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                        //  0번 인덱스 위치 (마크 찾을 위치)
                        jigAligner_Lower.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                        jigAligner_Lower.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_WaferXYAlignPos_TopCenter;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 XY 위치 보정, 웨이퍼 XY 위치 보정 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_MainWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__Wafer_XYAlign_Complete:                                  //  Wafer XY Align 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 보정, 파트 완료");

                    //m_bWaferAlign_OK = true;

                    if (Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage)
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_Align_ErrorCheck_Start;
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__Wafer_Align_ErrorCheck_Start:                                  //  Wafer Align 에러 검증 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 에러 검증, 파트 시작");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        if (m_bProbeCard_TiltCheck_OK && m_bProbeCard_XYAlign_OK && m_bWafer_ThetaAlign_OK && m_bWafer_XYAlign_OK)
                        {
                            m_bWaferProbeAlign_ErrorCheck_Complete = false;
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlign.WaferProbeAlignErrorCheck_Step.Start;
                            timer_SubWork.Enabled = true;

                            TickCount_Start((int)TickType.TICK_MAIN);

                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.__Wafer_Align_ErrorCheck_Complete;
                        }
                        else
                        {
                            m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2;
                        }
                    }
                    else
                    {
                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2;
                    }
                    break;


                case (int)WaferProbeAlign_Step.__Wafer_Align_ErrorCheck_Complete:                                  //  Wafer Align 에러 검증 완료 확인

                    if (m_nWaferProbeAlign_ErrorCheck_Step == (int)WaferProbeAlign.WaferProbeAlignErrorCheck_Step.None)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "웨이퍼 XY 위치 에러 검증, 파트 완료");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 60000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (웨이퍼 XY 위치 에러 검증 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("얼라인 오차 검증 실패. (TimeOut)", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.X],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.X],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.X]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2_DoneCheck;
                    break;


                case (int)WaferProbeAlign_Step.VisionXYZ_Move_ReadyPos2_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (//MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        //MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                        //                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))// &&

                        //MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        //MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                        //                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "비전 카메라, 대기 위치로 이동 완료");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 대기 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;
                    }
                    break;


                case (int)WaferProbeAlign_Step.Complete:

                    timer_MainWork.Enabled = false;

                    m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign_Step.None;

                    //if (Camera_Upper != null)
                    //{
                    //    Camera_Upper.StartLive();
                    //    visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_UpperVision_LightValue, 1);
                    //    visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
                    //}

                    //if (Camera_Lower != null)
                    //{
                    //    Camera_Lower.StartLive();
                    //    visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_LowerVision_LightValue, 2);
                    //    visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
                    //}


                    //  패킹 시 UVW 스테이지 충돌 확인
                    double m_dAxisU_Pos = 0.0;
                    double m_dAxisV_Pos = 0.0;
                    double m_dAxisW_Pos = 0.0;
                    bool m_bConflictCheck_U_OK = false;
                    bool m_bConflictCheck_V_OK = false;
                    bool m_bConflictCheck_W_OK = false;

                    if ((Config.ParamConfig.AlignLimit_UVW_U_Minus != 0) && (Config.ParamConfig.AlignLimit_UVW_U_Plus != 0))
                    {
                        m_dAxisU_Pos = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
                        if ((m_dAxisU_Pos >= Config.ParamConfig.AlignLimit_UVW_U_Minus) && (m_dAxisU_Pos <= Config.ParamConfig.AlignLimit_UVW_U_Plus))
                        {
                            m_bConflictCheck_U_OK = true;
                        }
                        else
                        {
                            m_bConflictCheck_U_OK = false;
                        }
                    }
                    else
                    {
                        m_bConflictCheck_U_OK = true;
                    }

                    if ((Config.ParamConfig.AlignLimit_UVW_V_Minus != 0) && (Config.ParamConfig.AlignLimit_UVW_V_Plus != 0))
                    {
                        m_dAxisV_Pos = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
                        if ((m_dAxisV_Pos >= Config.ParamConfig.AlignLimit_UVW_V_Minus) && (m_dAxisV_Pos <= Config.ParamConfig.AlignLimit_UVW_V_Plus))
                        {
                            m_bConflictCheck_V_OK = true;
                        }
                        else
                        {
                            m_bConflictCheck_V_OK = false;
                        }
                    }
                    else
                    {
                        m_bConflictCheck_V_OK = true;
                    }

                    if ((Config.ParamConfig.AlignLimit_UVW_W_Minus != 0) && (Config.ParamConfig.AlignLimit_UVW_W_Plus != 0))
                    {
                        m_dAxisW_Pos = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
                        if ((m_dAxisW_Pos >= Config.ParamConfig.AlignLimit_UVW_W_Minus) && (m_dAxisW_Pos <= Config.ParamConfig.AlignLimit_UVW_W_Plus))
                        {
                            m_bConflictCheck_W_OK = true;
                        }
                        else
                        {
                            m_bConflictCheck_W_OK = false;
                        }
                    }
                    else
                    {
                        m_bConflictCheck_W_OK = true;
                    }


                    if (m_bProbeCard_TiltCheck_Only)
                    {
                        m_strTemp = "Probe Card Tilt 상태 확인 완료.\r\n\r\n[Theta : " + m_dProbeCardAlign_CorrectionAngle + "˚]";
                        MessageBox.Show(m_strTemp, "Information!");
                    }
                    else if (m_bWafer_Align_Only)
                    {
                        m_strTemp = "Wafer Align 상태 확인 완료.";
                        MessageBox.Show(m_strTemp, "Information!");
                    }
                    else
                    {
                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            if (m_bWafer_ThetaAlign_OK && m_bWafer_XYAlign_OK)
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 완료 (OK)");

                                m_dWafer_ProbeCard_AlignPos_Axis_U = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
                                m_dWafer_ProbeCard_AlignPos_Axis_V = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
                                m_dWafer_ProbeCard_AlignPos_Axis_W = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);

                                if (!Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage)
                                {
                                    if (Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage)
                                    {
                                        if (m_bConflictCheck_U_OK && m_bConflictCheck_V_OK && m_bConflictCheck_W_OK)
                                        {
                                            m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Start;
                                            timer_MainWork.Enabled = true;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 성공]\r\n\r\n[주의 : 패킹 시 얼라인 스테이지와 프로브 카드가 충돌할 수 있음", "Warning!");
                                        }
                                    }
                                    else
                                    {
                                        if (m_bConflictCheck_U_OK && m_bConflictCheck_V_OK && m_bConflictCheck_W_OK)
                                        {
                                            MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 성공]", "Information!");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 성공]\r\n\r\n[주의 : 패킹 시 얼라인 스테이지와 프로브 카드가 충돌할 수 있음", "Warning!");
                                        }                                        
                                    }
                                }
                                else
                                {
                                    if (m_bWaferProbeAlign_ErrorCheck_Complete)
                                    {
                                        if ((AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Upper] == true) &&
                                            (AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Top, (int)WaferProbeAlign.nCameraType.Cam_Lower] == true) &&
                                            (AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Upper] == true) &&
                                            (AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Mid, (int)WaferProbeAlign.nCameraType.Cam_Lower] == true) &&
                                            (AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Upper] == true) &&
                                            (AlignmentErrorCheck_Status[(int)WaferProbeAlign.nAlignErrorCheckPos.Pos_Bot, (int)WaferProbeAlign.nCameraType.Cam_Lower] == true))
                                        {
                                            if (Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage)
                                            {
                                                if (m_bConflictCheck_U_OK && m_bConflictCheck_V_OK && m_bConflictCheck_W_OK)
                                                {
                                                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Start;
                                                    timer_MainWork.Enabled = true;
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 검증 성공]\r\n\r\n[주의 : 패킹 시 얼라인 스테이지와 프로브 카드가 충돌할 수 있음", "Warning!");
                                                }
                                            }
                                            else
                                            {                                                
                                                if (m_bConflictCheck_U_OK && m_bConflictCheck_V_OK && m_bConflictCheck_W_OK)
                                                {
                                                    MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 검증 성공]", "Information!");
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 검증 성공]\r\n\r\n[주의 : 패킹 시 얼라인 스테이지와 프로브 카드가 충돌할 수 있음", "Warning!");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 검증 실패]\r\n\r\n[작업자 확인 후 패킹 진행]", "Information!");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 성공]\r\n\r\n\r\n[얼라인 정확성 검증 : 실패 (메인화면 확인)]", "Information!");
                                    }
                                }
                            }
                            else if (m_bWafer_ThetaAlign_OK && !m_bWafer_XYAlign_OK)
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 완료 (NG)");
                                MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 실패]\r\n\r\n[얼라인 실패]", "Information!");
                            }
                            else if (!m_bWafer_ThetaAlign_OK && m_bWafer_XYAlign_OK)
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 완료 (NG)");
                                MessageBox.Show("Wafer Theta Align 완료. [Theta Align 실패]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 실패]", "Information!");
                            }
                            else
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 완료 (NG)");
                                MessageBox.Show("Wafer Theta Align 완료. [Theta Align 실패]\r\nWafer XY Align 완료. [XY Align 실패]\r\n\r\n[얼라인 실패]", "Information!");
                            }
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "Wafer Align 완료 (Dry Run)");
                            MessageBox.Show("Wafer Align 완료.\r\n\r\n[Dry Run]", "Information!");
                        }
                    }
                    break;
            }
        }

        #endregion



        #region Wafer ProbeCard Alignment Error Check Cycle Function

        private void Run_WaferProbeAlign_ErrorCheck_Func()
        {
            string m_strTemp = "";

            double theta = 0.0;
            double deltaX = 0.0;
            double deltaY = 0.0;
            double dMmPer1Deg = 0.0;
            double dCenterUVWDistance = 0.0;
            int invertAngle = 1;

            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            switch (m_nWaferProbeAlign_ErrorCheck_Step)
            {
                case (int)WaferProbeAlignErrorCheck_Step.Start:
                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "Wafer Align Error Check 시작");

                    Equipment.MachineStop_byAlarm = false;

                    //m_bFindFirstAlignMarkOnly = false;

                    m_bWaferProbeAlign_ErrorCheck_Complete = false;

                    m_nWaferAlign_Count = 0;

                    for (int nPos = 0; nPos < 3; nPos++)
                    {
                        AlignmentErrorCheck_Position[nPos].X = 0.0;
                        AlignmentErrorCheck_Position[nPos].Y = 0.0;

                        for (int nSide = 0; nSide < 2; nSide++)
                        {
                            AlignmentErrorCheck_Status[nPos, nSide] = false;

                            AlignmentErrorCheck_MarkPosition[nPos, nSide].X = 0.0;
                            AlignmentErrorCheck_MarkPosition[nPos, nSide].Y = 0.0;
                        }
                    }

                    m_nWaferProbeAlign_ErrorCheck_Count_Total = 3;              //  오차 확인 위치 총 개수
                    m_nWaferProbeAlign_ErrorCheck_Count = 0;                    //  오차 확인 위치 카운트 (3개,   0: TOP, 1: MID, 2: BOT)

                    m_bProbeCard_XYAlign_ErrorCheck_OK = false;         //  Probe Card XY Align Error Check OK
                    m_bWafer_XYAlign_ErrorCheck_OK = false;             //  Wafer XY Align Error Check OK

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.Align_Condition_Check;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.Align_Condition_Check:               //  Align 조건 확인 (Thin Chuck Exist)

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "시작 조건 확인");

                    if (!waferProbeAlignParameter.DI_ThinChuck_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (씬-척이 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Thin Chuck 이 감지되지 않음.", "Error");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    else
                    {
                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ReadyPos;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "비전 카메라, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "비전 카메라, 대기 위치로 이동 완료");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ElevZ_Move_WaferAlignPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (비전 카메라, 대기 위치로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ElevZ_Move_WaferAlignPos:                            //  Elev. Z 축, Wafer Align 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "씬-척 엘리베이터, 웨이퍼 얼라인 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  이동 할 Elev. Z 축 높이가 얼라인 한계 높이를 초과하는 지 체크
                    if (waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (씬-척 엘리베이터, 웨이퍼 얼라인 한계 높이를 초과하여 이동하려고 하였음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Align 한계 높이를 초과하여 이동하려는 시도로 작업 중지.", "Information");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_SUB);

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ElevZ_Move_WaferAlignPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ElevZ_Move_WaferAlignPos_DoneCheck:                  //  Elev. Z 축, Wafer Align 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "씬-척 엘리베이터, 웨이퍼 얼라인 높이로 이동 완료");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__ErrorCheck_Start;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (씬-척 엘리베이터, 웨이퍼 얼라인 높이로 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Align 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.__ErrorCheck_Start:                                      //  Wafer - ProbeCard Alignment Error Check 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "에러 체크 파트 시작");

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ErrorCheck_Position_Remained_Check;

                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ErrorCheck_Position_Remained_Check:                      //  얼라인 확인 위치가 남아있는지 체크 (Top, Center, Bottom 3군데 체크)

                    if (m_nWaferProbeAlign_ErrorCheck_Count < m_nWaferProbeAlign_ErrorCheck_Count_Total)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "에러 검사 위치가 남아 있음");

                        if (Camera_Upper != null)
                        {
                            Camera_Upper.StartLive();
                            visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_UpperVision_LightValue, 1);
                            visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
                        }

                        if (Camera_Lower != null)
                        {
                            Camera_Lower.StopLive();
                            visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_LowerVision_LightValue, 2);
                            visionCalibrator_Upper.Illuminator.TurnOnOff(false, 2);       //  Wafer 조명
                        }

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__ProbeCard_XYAlign_Start;
                    }    
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "에러 검사 위치가 남아 있지 않음");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.Complete;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.__ProbeCard_XYAlign_Start:                                  //  ProbeCard XY Align 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 얼라인 파트 시작");

                    m_nWaferAlign_Count = 0;
                    m_bProbeCard_XYAlign_ErrorCheck_OK = false;

                    //  첫번째 얼라인 마크 위치 (최초 이동 좌표, 2회 이상 부터는 계산된 위치를 넣어줌)

                    if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 마크 위치 : TOP");

                        waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    }
                    else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 마크 위치 : MID");

                        waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Middle");
                    }
                    else                                                                        //  Bottom
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 마크 위치 : BOT");

                        waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                    }

                    m_dMy1stMarkVisionPos_X = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];
                    m_dMy1stMarkVisionPos_Y = waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_MarkFind_Ready;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_MarkFind_Ready:                        //  얼라인 마크 찾기 준비
                    //  얼라인 마크 위치를 찾기 위한 데이터 세팅

                    if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 TOP 마크 검출을 위한 파라미터 세팅");
                    }
                    else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 MID 마크 검출을 위한 파라미터 세팅");
                    }
                    else                                                                        //  Bottom
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 BOT 마크 검출을 위한 파라미터 세팅");
                    }

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    //waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");
                    jigAligner_Upper.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                    jigAligner_Upper.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos:              //  VisionXYZ 축, ProbeCard XY Align 위치로 이동
                                        
                    if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 시작");
                    }
                    else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 MID 위치로 이동 시작");
                    }
                    else                                                                        //  Bottom
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 BOT 위치로 이동 시작");
                    }

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.X] = jigAligner_Upper.m_AlignPositions[0].X;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlignParameter.AxisAjinEnum.Y] = jigAligner_Upper.m_AlignPositions[0].Y;
                    }

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Vision Y 축이 웨이퍼 위치로 이동할 때, Elev. Z 축 높이가 충돌 한계 높이를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer ProbeCard Align", "작업 중지. (비전 카메라, 웨이퍼 얼라인 위치로 이동 시 씬-척 엘리베이터와 충돌할 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축이 Vision Y 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                        TickCount_Start((int)TickType.TICK_SUB);

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos_DoneCheck:    //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 완료");
                        }
                        else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 MID 위치로 이동 완료");
                        }
                        else                                                                        //  Bottom
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 BOT 위치로 이동 완료");
                        }

                        TickCount_Start((int)TickType.TICK_SUB);

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 시간 초과)");
                        }
                        else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 MID 위치로 이동 시간 초과)");
                        }
                        else                                                                        //  Bottom
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 BOT 위치로 이동 시간 초과)");
                        }

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Probe-Card XY Align 마크 위치로 이동 실패.", "Error");

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos_StableTime:           //  VisionXYZ 축, ProbeCard XY Align (Top, Center) 위치로 이동 후 안정화 시간

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_SUB) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_SUB) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 TOP 위치로 이동 후 안정화 시간 완료");
                        }
                        else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 MID 위치로 이동 후 안정화 시간 완료");
                        }
                        else                                                                        //  Bottom
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 BOT 위치로 이동 후 안정화 시간 완료");
                        }

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_MarkFind;
                        }
                        else
                        {
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__ProbeCard_XYAlign_Complete;
                        }
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_MarkFind:                                   //  얼라인 마크 찾기

                    //  Probe Card 를 Align 하는 것이므로 Upper Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = false;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.Vision_SpiralMove_Use = true;
                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_SUB);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Upper.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_SubWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bProbeCard_XYAlign_ErrorCheck_OK = false;
                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_MarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_MarkResultCheck:                            //  얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 얼라인 마크 검출 완료");

                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos;
                            }
                            else
                            {
                                m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlignData_Calc;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (프로브 카드 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_SubWork.Enabled = false;

                            MessageBox.Show("상부 카메라, 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].X = jigAligner_LowRes.Result;
                        //m_forAlign_Data[(int)AlignParam.RESULT_LOWVISION_THETA].Y = jigAligner_LowRes.Result;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Upper.FirstPosition.X == 0) || (jigAligner_Upper.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos;
                        }
                        else
                        {
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlignData_Calc;
                        }
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlignData_Calc:                                            //  XY 얼라인 데이터 계산

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 XY 보정량 계산");

                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X - jigAligner_Upper.m_AlignPositions[0].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y - jigAligner_Upper.m_AlignPositions[0].Y;

                    ////  얼라인 에러 값
                    //AlignmentErrorCheck_Delta[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper].X = deltaX;
                    //AlignmentErrorCheck_Delta[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper].Y = deltaY;

                    //  얼라인 마크 위치값
                    AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper].X = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X;
                    AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper].Y = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y;                    

                    //  얼라인 확인 위치
                    AlignmentErrorCheck_Position[m_nWaferProbeAlign_ErrorCheck_Count].X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X); 
                    AlignmentErrorCheck_Position[m_nWaferProbeAlign_ErrorCheck_Count].Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                    //  판정. (OK 범위 이내가 아니면 Retry)
                    if ((Math.Abs(deltaX) <= Config.ParamConfig.Align_Vision_Allowable_XY) &&
                        (Math.Abs(deltaY) <= Config.ParamConfig.Align_Vision_Allowable_XY))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 XY 위치가 카메라 센터에서 오차범위 이내에 있음");

                        m_bProbeCard_XYAlign_ErrorCheck_OK = true;

                        //  현재 위치를 첫번째 얼라인 마크 위치로 한다.
                        m_dMy1stMarkVisionPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                        m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                        //  현재 마크 위치, 상부 얼라인 OK
                        AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper] = true;

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__ProbeCard_XYAlign_Complete;
                    }
                    else
                    {
                        m_nWaferAlign_Count++;

                        if (m_nWaferAlign_Count < Config.ParamConfig.Align_Retries)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 XY 위치가 카메라 센터에서 오차범위 초과");

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_CorrectionMove;
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 XY 위치 보정 회수 초과)");

                            m_bProbeCard_XYAlign_ErrorCheck_OK = false;

                            //  현재 마크 위치, 상부 얼라인 NG
                            AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper] = false;

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__ProbeCard_XYAlign_Complete;
                        }
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_CorrectionMove:                                      //  Top 위치 XY 보정 이동 (카메라 Center 로)

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 XY 위치 보정 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    //  Vision XY 방향 이동량 계산
                    deltaX = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X - jigAligner_Upper.m_AlignPositions[0].X;
                    deltaY = m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y - jigAligner_Upper.m_AlignPositions[0].Y;

                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.X] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X) - deltaX;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.Y] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y) - deltaY;

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  절대 위치 이동
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.X],
                                                                                        m_dSpeed_ElvXY,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.Y],
                                                                                        m_dSpeed_ElvXY,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                                                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_CorrectionMove_DoneCheck;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.ProbeCardXYAlign_CorrectionMove_DoneCheck:                             //  Top 위치 XY 보정 이동 완료 확인
                    if ((TickCount_Elapsed((int)TickType.TICK_SUB) >= Config.ParamConfig.WaferAlign_Move_StableTime) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 위치 보정, 프로브 카드 XY 위치 보정 이동 완료");

                        //  1번 마크 위치 다시 계산
                        //  - X 위치 : 2번 마크 위치에서 마크 OffsetX 만큼
                        //  - Y 위치 : 1번 마크 위치와 2번 마크 위치의 중간

                        //m_dMy1stMarkVisionPos_X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X + Config.ParamConfig.AlignMark_2nd_Offset_X;
                        //m_dMy1stMarkVisionPos_Y = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y) / 2.0 + Config.ParamConfig.AlignMark_2nd_Offset_Y;
                        //m_dMy1stMarkVisionPos_X = (m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X + m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].X) / 2.0 - m_dMyVisionPos_Distance_X;
                        //m_dMy1stMarkVisionPos_Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_SECONDMARKPOS].Y - m_dMyVisionPos_Distance_Y;                        
                        //m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos( (int)nAxis.Y ) ;

                        //m_nWaferProbeAlign_MainStep = (int)WaferProbeAlignErrorCheck_Step.WaferAlign_TopMarkFind_Ready;       //   VisionXYZ_Move_WaferAlignPos_TopCenter;



                        //  마크 1개만 찾기 위한 설정
                        m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                        //  0번 인덱스 위치 (마크 찾을 위치)
                        m_dMy1stMarkVisionPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                        m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                        jigAligner_Upper.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                        jigAligner_Upper.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.VisionXYZ_Move_ProbeCardXYAlignPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (프로브 카드 XY 위치 보정, 프로브 카드 XY 위치 보정 이동 시간 초과)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_SubWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.__ProbeCard_XYAlign_Complete:                                  //  Wafer XY Align 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 얼라인 파트 완료");

                    if (m_bProbeCard_XYAlign_ErrorCheck_OK)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 얼라인 OK, 웨이퍼 XY 오차 확인 진행");

                        if (Camera_Upper != null)
                        {
                            Camera_Upper.StopLive();
                            visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_UpperVision_LightValue, 1);
                            visionCalibrator_Upper.Illuminator.TurnOnOff(false, 1);       //  Probe Card 조명
                        }

                        if (Camera_Lower != null)
                        {
                            Camera_Lower.StartLive();
                            visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.Align_LowerVision_LightValue, 2);
                            visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
                        }

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__Wafer_XYAlign_Start;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "프로브 카드 XY 얼라인 NG, 웨이퍼 XY 오차 확인도 NG 처리");

                        //  현재 마크 위치, 상부 얼라인 NG 이므로 하부 얼라인도 NG 처리한다. 
                        AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower] = false;

                        m_nWaferProbeAlign_ErrorCheck_Count++;

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ErrorCheck_Position_Remained_Check;


                        //timer_SubWork.Enabled = false;

                        //m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;

                        //m_bProbeCard_XYAlign_ErrorCheck_OK = false;

                        //MessageBox.Show("Probe Card XY Align 확인 실패.", "Error");
                    }
                    break;
                    

                case (int)WaferProbeAlignErrorCheck_Step.__Wafer_XYAlign_Start:                                  //  Wafer XY Align 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인 파트 시작");

                    m_nWaferAlign_Count = 0;
                    m_bWafer_XYAlign_ErrorCheck_OK = false;

                    //  현재 위치 
                    m_dMy1stMarkVisionPos_X = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                    m_dMy1stMarkVisionPos_Y = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready:                        //  얼라인 마크 찾기 준비
                    //  TOP 위치의 얼라인 마크 위치를 찾기 위한 데이터 세팅

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인 마크 검출을 위한 파라미터 세팅");

                    //  마크 1개만 찾기 위한 설정
                    m_nFindAlignMarkType = (int)WaferProbeAlign.AlignMarkType.ALIGN_1STMARK;                           //  요걸로 하면 마크 1개만 찾고 끝.

                    //  0번 인덱스 위치 (마크 찾을 위치)
                    jigAligner_Lower.m_AlignPositions[0].X = m_dMy1stMarkVisionPos_X;
                    jigAligner_Lower.m_AlignPositions[0].Y = m_dMy1stMarkVisionPos_Y;

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready_StableTime;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready_StableTime:

                    if (((Config.ParamConfig.WaferAlign_Move_StableTime <= 0) && (TickCount_Elapsed((int)TickType.TICK_SUB) >= 3000)) ||                 //  임시로 3초 (옵션 처리 하자)
                        (Config.ParamConfig.WaferAlign_Move_StableTime > 0) && (TickCount_Elapsed((int)TickType.TICK_SUB) >= Config.ParamConfig.WaferAlign_Move_StableTime))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인 마크 검출을 위한 안정화 시간 완료");

                        if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                        {
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind;
                        }
                        else
                        {
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__Wafer_XYAlign_Complete;
                        }
                    }
                    break;

                    
                case (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind:                                   //  얼라인 마크 찾기

                    //  Wafer 를 Align 하는 것이므로 Lower Camera 를 사용해야 한다.

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 얼라인 마크 검출 시작");

                    m_bLowerVision_Align = true;                                               //  true : Lower Vision     false : Upper Vision

                    Equipment.Vision_SpiralMove_Use = false;
                    Equipment.MachineStop_byUser = false;

                    if (m_bAlignVisionThread_Use)
                    {
                        //  Thread 를 사용할 경우
                        m_bFindAlignMark_OK = false;
                        m_nFindAlignMark_Step = (int)FindAlignMark_Step.Start;

                        TickCount_Start((int)TickType.TICK_SUB);
                    }
                    else
                    {
                        //  그냥 타이머를 사용할 경우
                        jigAligner_Lower.Work();
                    }

                    if (Equipment.MachineStop_byUser == true)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (사용자에 의해 작업이 중지됨)");

                        //if (!m_bAlignVisionThread_Use)
                        {
                            timer_SubWork.Enabled = false;
                        }

                        m_bMyWaferAlign_fromManualMode = false;
                        //m_nMyWaferAlign_ManualMode_VisionType = (int)VisionType.NONE;

                        m_bWafer_XYAlign_ErrorCheck_OK = false;
                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                        MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                    }
                    else
                    {
                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkResultCheck;
                    }
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkResultCheck:                            //  얼라인 마크 찾기 결과 확인

                    if (m_bAlignVisionThread_Use)
                    {
                        if (m_nFindAlignMark_Step == (int)FindAlignMark_Step.None)
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 얼라인 마크 검출 완료");

                            Equipment.Vision_SpiralMove_Use = true;

                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                            m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;

                            //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                            if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 얼라인 마크 검출 결과 NG (위치값 0)");

                                m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready;
                            }
                            else
                            {
                                m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlignData_ErrorCalc;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 60000)               //  1분 동안 마크를 찾지 못할 경우
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "작업 중지. (웨이퍼 XY 오차 확인, 웨이퍼 얼라인 마크 검출 시간 초과)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            Equipment.MachineStop_byUser = true;

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);

                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                            MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);

                            timer_SubWork.Enabled = false;

                            MessageBox.Show("하부 카메라, 얼라인 마크 찾기 실패\r\n\r\n[Time Out]", "Error");

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;
                        }
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 얼라인 마크 검출 완료");

                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                        m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;

                        //  마크를 찾긴 했는데, Position 값이 0인 경우가 있다. 
                        if ((jigAligner_Lower.FirstPosition.X == 0) || (jigAligner_Lower.FirstPosition.Y == 0))
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 얼라인 마크 검출 결과 NG (위치값 0)");

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready;
                        }
                        else
                        {
                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlignData_ErrorCalc;
                        }
                    }                    
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.WaferXYAlignData_ErrorCalc:                                            //  XY 얼라인 데이터 오차 계산

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 XY 오차 계산");

                    m_dWaferAlign_CorrectionAngle = 0.0;

                    //  기존
                    //deltaX = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X - jigAligner_Lower.m_AlignPositions[0].X;
                    //deltaY = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y - jigAligner_Lower.m_AlignPositions[0].Y;

                    //  기존
                    ////  얼라인 에러 값
                    //AlignmentErrorCheck_Delta[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower].X = deltaX;
                    //AlignmentErrorCheck_Delta[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower].Y = deltaY;

                    //  얼라인 마크 위치값
                    AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower].X = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X;
                    AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower].Y = m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y;

                    ////  판정. (OK 범위 이내가 아니면 Retry)
                    //if ((Math.Abs(deltaX) <= Config.ParamConfig.Align_Vision_Allowable_XY) &&
                    //    (Math.Abs(deltaY) <= Config.ParamConfig.Align_Vision_Allowable_XY))
                    //{
                    //    m_bWafer_XYAlign_ErrorCheck_OK = true;

                    //    //  현재 마크 위치, 하부 얼라인 OK
                    //    AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower] = true;
                    //}
                    //else
                    //{
                    //    m_bWafer_XYAlign_ErrorCheck_OK = false;

                    //    //  현재 마크 위치, 하부 얼라인 NG
                    //    AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower] = false;
                    //}

                    deltaX = AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper].X - AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower].X;
                    deltaY = AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Upper].Y - AlignmentErrorCheck_MarkPosition[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower].Y;

                    //  판정. (프로브 핀 위치 대비 웨이퍼 전극 위치가 오차범위 이내인지 확인)
                    if ((Math.Abs(deltaX) <= Config.ParamConfig.AlignError_Vision_Allowable_XY) &&
                        (Math.Abs(deltaY) <= Config.ParamConfig.AlignError_Vision_Allowable_XY))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 XY 위치가 카메라 센터에서 오차범위 이내에 있음");

                        m_bWafer_XYAlign_ErrorCheck_OK = true;

                        //  현재 마크 위치, 하부 얼라인 OK
                        AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower] = true;

                        m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__Wafer_XYAlign_Complete;
                    }
                    else
                    {
                        if ((Math.Abs(deltaX) >= 1.0) ||            //  오차가 1mm 이상이면 마크 검출이 이상하게 된 것으로 본다. 마크 검사 Retry
                            (Math.Abs(deltaY) >= 1.0))
                        {
                            m_nWaferAlign_Count++;

                            if (m_nWaferAlign_Count < Config.ParamConfig.Align_Retries)
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 XY 위치 오차가 너무 커서 재시도");

                                m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.WaferXYAlign_MarkFind_Ready;
                            }
                        }
                        else
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인, 웨이퍼 XY 위치가 카메라 센터에서 오차범위 초과");

                            m_bWafer_XYAlign_ErrorCheck_OK = false;

                            //  현재 마크 위치, 하부 얼라인 NG
                            AlignmentErrorCheck_Status[m_nWaferProbeAlign_ErrorCheck_Count, (int)nCameraType.Cam_Lower] = false;

                            m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.__Wafer_XYAlign_Complete;
                        }
                    }                    
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.__Wafer_XYAlign_Complete:                                  //  Wafer XY Align 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "웨이퍼 XY 오차 확인 파트 완료");

                    //  얼라인 이미지 저장
                    if (Config.ParamConfig.AlignImageSave_Usage)
                    {
                        if (m_nWaferProbeAlign_ErrorCheck_Count == 0)                               //  Top
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "TOP 위치 얼라인 이미지 저장");
                            ResultImage_Save(Equipment.User_Name, Equipment.AlignStart_Time, "TOP");
                        }
                        else if (m_nWaferProbeAlign_ErrorCheck_Count == 1)                          //  Middle
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "MIDDLE 위치 얼라인 이미지 저장");
                            ResultImage_Save(Equipment.User_Name, Equipment.AlignStart_Time, "MID");
                        }
                        else                                                                        //  Bottom
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "BOTTOM 위치 얼라인 이미지 저장");
                            ResultImage_Save(Equipment.User_Name, Equipment.AlignStart_Time, "BOT");
                        }                        
                    }

                    m_nWaferProbeAlign_ErrorCheck_Count++;

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.ErrorCheck_Position_Remained_Check;
                    break;


                case (int)WaferProbeAlignErrorCheck_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Align Error Check", "Wafer Align Error Check 완료");

                    Equipment.AlignStart_Time = null;                               //  이미지 저장할 때 사용했으므로 null 로 초기화. (Align 진행하면 시간이 저장된다.)
                    m_bWaferProbeAlign_ErrorCheck_Complete = true;

                    timer_SubWork.Enabled = false;

                    m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlignErrorCheck_Step.None;

                    if (m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign_Step.None)
                    {
                        MessageBox.Show("Wafer Align Error Check 완료.\r\n\r\n[메인화면에서 오차값 확인]", "Information!");
                    }

                    //if (Config.ParamConfig.Wafer_Align_Cam_Usage)
                    //{
                    //    if (m_bWafer_ThetaAlign_OK && m_bWafer_XYAlign_OK)
                    //    {
                    //        Log.Write("CWA150SA", Equipment.User_Name, "Auto Run", "Wafer Align 완료 (OK)");

                    //        if (Config.ParamConfig.Wafer_Align_After_Packing_AutoStart_Usage)
                    //        {
                    //            m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Start;
                    //            timer_MainWork.Enabled = true;
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 성공]", "Information!");
                    //        }
                    //    }
                    //    else if (m_bWafer_ThetaAlign_OK && !m_bWafer_XYAlign_OK)
                    //    {
                    //        Log.Write("CWA150SA", Equipment.User_Name, "Auto Run", "Wafer Align 완료 (NG)");
                    //        MessageBox.Show("Wafer Theta Align 완료. [Theta Align 성공]\r\nWafer XY Align 완료. [XY Align 실패]\r\n\r\n[얼라인 실패]", "Information!");
                    //    }
                    //    else if (!m_bWafer_ThetaAlign_OK && m_bWafer_XYAlign_OK)
                    //    {
                    //        Log.Write("CWA150SA", Equipment.User_Name, "Auto Run", "Wafer Align 완료 (NG)");
                    //        MessageBox.Show("Wafer Theta Align 완료. [Theta Align 실패]\r\nWafer XY Align 완료. [XY Align 성공]\r\n\r\n[얼라인 실패]", "Information!");
                    //    }
                    //    else
                    //    {
                    //        Log.Write("CWA150SA", Equipment.User_Name, "Auto Run", "Wafer Align 완료 (NG)");
                    //        MessageBox.Show("Wafer Theta Align 완료. [Theta Align 실패]\r\nWafer XY Align 완료. [XY Align 실패]\r\n\r\n[얼라인 실패]", "Information!");
                    //    }
                    //}
                    //else
                    //{
                    //    Log.Write("CWA150SA", Equipment.User_Name, "Auto Run", "Wafer Align 완료 (Dry Run)");
                    //    MessageBox.Show("Wafer Align 완료.\r\n\r\n[Dry Run]", "Information!");
                    //}
                    break;
            }
        }

        #endregion



        #region Align Image Save

        public bool ResultImage_Save( string m_strOperator, string m_strStartTime, string m_strAlignPos )
        {
            bool m_bRet = true;
            string m_strDirectory = null;
            string m_strRoot = null;
            string m_strDate = null;
            string m_strImageFile_Upper = null;
            string m_strImageFile_Lower = null;

            //  데이터 확인
            if (m_strOperator == null)                                             //  Align 을 진행하지 않았으면?
            {
                m_strOperator = "UnknownOperator";
            }

            if (m_strStartTime == null)                                             //  Align 을 진행하지 않았으면?
            {
                m_strStartTime = "NoAlign";
            }


            //  이미지 저장 경로 (꼭대기)
            m_strRoot = "D:\\CWA-150SA_AlignImage";
            DirectoryInfo di = new DirectoryInfo(m_strRoot);
            if (!di.Exists)                                                         //  없으면 생성
            {
                di.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜)
            m_strDate = DateTime.Now.ToString("yyyy-MM-dd");
            m_strDirectory = string.Format("{0}\\{1}", m_strRoot, m_strDate);
            DirectoryInfo di2 = new DirectoryInfo(m_strDirectory);
            if (!di2.Exists)                                                         //  없으면 생성
            {
                di2.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜 / 작업자)
            m_strDirectory = string.Format("{0}\\{1}\\{2}", m_strRoot, m_strDate, m_strOperator);
            DirectoryInfo di3 = new DirectoryInfo(m_strDirectory);
            if (!di3.Exists)                                                         //  없으면 생성
            {
                di3.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜 / 작업자 / 작업자_얼라인시작시간_얼라인위치_카메라방향)
            //  작업자 : 작업자 이름
            //  얼라인시작시간 : 얼라인 시작 버튼을 눌렀을 때의 시간 (년-월-일)
            //  얼라인위치 : 얼라인 검사 위치 (TOP, MID, BOT)
            //  카메라 방향 : ProbeCard or Wafer
            m_strImageFile_Upper = string.Format("{0}\\{1}\\{2}\\{3}_{4}_{5}_ProbeCard.jpg", m_strRoot, m_strDate, m_strOperator, m_strOperator, m_strStartTime, m_strAlignPos);
            m_strImageFile_Lower = string.Format("{0}\\{1}\\{2}\\{3}_{4}_{5}_Wafer.jpg", m_strRoot, m_strDate, m_strOperator, m_strOperator, m_strStartTime, m_strAlignPos);

            if (Camera_Upper != null)
            {
                Camera_Upper.LatestImage.Save( m_strImageFile_Upper, Vision.VisionImage.FileFilter.jpg) ;
            }

            if (Camera_Lower != null)
            {
                Camera_Lower.LatestImage.Save(m_strImageFile_Lower, Vision.VisionImage.FileFilter.jpg);
            }

            return m_bRet;
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
                    if (!m_bLowerVision_Align)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "상부 카메라 마크 찾기 시작");

                        jigAligner_Upper.Work();
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "하부 카메라 마크 찾기 시작");

                        jigAligner_Lower.Work();
                    }

                    m_nFindAlignMark_Step = (int)FindAlignMark_Step.FindMark_ResultCheck;
                    break;


                case (int)FindAlignMark_Step.FindMark_ResultCheck:                                        //  Align 결과 확인

                    //if (!m_bLowerVision_Align)
                    //{
                    //    m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].X = jigAligner_Upper.FirstPosition.X;
                    //    m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_FIRSTMARKPOS].Y = jigAligner_Upper.FirstPosition.Y;
                    //    m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_THETA].X = jigAligner_Upper.Result;
                    //    m_forAlign_Data[(int)AlignParam.RESULT_UpperVISION_THETA].Y = jigAligner_Upper.Result;
                    //}
                    //else
                    //{
                    //    m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].X = jigAligner_Lower.FirstPosition.X;
                    //    m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_FIRSTMARKPOS].Y = jigAligner_Lower.FirstPosition.Y;
                    //    m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_THETA].X = jigAligner_Lower.Result;
                    //    m_forAlign_Data[(int)AlignParam.RESULT_LowerVISION_THETA].Y = jigAligner_Lower.Result;
                    //}

                    m_nFindAlignMark_Step = (int)FindAlignMark_Step.Complete;
                    break;


                case (int)FindAlignMark_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Find Align Mark", "마크 찾기 완료");

                    if (!m_bLowerVision_Align)
                    {
                        if ((jigAligner_Upper.FirstPosition.X == 0.0) || (jigAligner_Upper.FirstPosition.Y == 0.0))
                        {
                            m_bFindUpperAlignMark_OK = false;
                        }
                        else
                        {
                            m_bFindUpperAlignMark_OK = true;
                        }
                    }
                    else
                    {
                        if ((jigAligner_Lower.FirstPosition.X == 0.0) || (jigAligner_Lower.FirstPosition.Y == 0.0))
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


        #region Home Function

        void Run_Home_Func()
        {
            bool m_bRet = false;
            string m_strTemp;

            switch (m_nHomeStep)
            {
                case (int)Home_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    //  초기화 하면 얼라인을 다시 해야 하므로, Ailgn Flag 를 false 로 만든다.
                    m_bWafer_ThetaAlign_OK = false;
                    m_bWafer_XYAlign_OK = false;

                    m_nManualPacking_Step = (int)ManualPackingStep.NONE;

                    m_bHomeOK = false;

                    m_dWafer_ProbeCard_PackingPos_Axis_U = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_V = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_W = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

                    m_dWafer_ProbeCard_AlignPos_Axis_U = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
                    m_dWafer_ProbeCard_AlignPos_Axis_V = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
                    m_dWafer_ProbeCard_AlignPos_Axis_W = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)

                    //Camera_Upper.Initialize();
                    //Camera_Lower.Initialize();

                    m_nHomeStep = (int)Home_Step.AxisAlarmCheck;
                    break;


                case (int)Home_Step.AxisAlarmCheck:                                         //  서보 축 알람 체크
                    if (m_nHomeAxisCount <= 6)                                                  //  0 ~ 6 번 축 까지 있음. (0:U, 1:V, 2:W, 3:EZ, 4:X, 5:Y, 6:VZ)
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
                        m_nHomeStep = (int)Home_Step.VisionY_HomeStart;
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


                case (int)Home_Step.AlarmAxisServoOn:               //  알람 축 서보 On
                    if (TickCount_Elapsed((int)TickType.TICK_HOME) > 300)
                    {
                        MC_Func.MC_SetServoOnOff(m_nHomeAxisCount, true);

                        m_nHomeStep = (int)Home_Step.AlarmAxisServoOnCheck;
                    }
                    break;


                case (int)Home_Step.AlarmAxisServoOnCheck:          //  알람 축 서보 On 확인
                    if (MC_Func.MC_IsServoOn(m_nHomeAxisCount))
                    {
                        m_nHomeAxisCount++;

                        m_nHomeStep = (int)Home_Step.AxisAlarmCheck;
                    }
                    break;


                case (int)Home_Step.VisionY_HomeStart:                                    //  Vision Y 축 홈 실행

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "카메라 Y 축 초기화 시작");

                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.Y);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.VisionY_HomeCompleteCheck;
                    break;


                case (int)Home_Step.VisionY_HomeCompleteCheck:                            //  Vision Y 축 홈 완료 체크

                    if ((TickCount_Elapsed((int)TickType.TICK_HOME) > 100) &&                        
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.Y) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.Y))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "카메라 Y 축 초기화 완료");

                        m_nHomeStep = (int)Home_Step.VisionXZ_UVW_EZ_HomeStart;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "카메라 Y 축 초기화 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;

                        MessageBox.Show("Vision Y 축 초기화 실패", "Error");

                        m_nHomeStep = (int)Home_Step.None;
                    }
                    break;


                case (int)Home_Step.VisionXZ_UVW_EZ_HomeStart:                                         //  Vision XZ, UVW EZ 축 홈 실행

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "UVW, 엘리베이터, 카메라 X, 카메라 Z 축 초기화 시작");

                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.U);
                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.V);
                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.W);
                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.EZ);

                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.X);
                    MC_Func.MC_HomeSearch((int)WaferProbeAlignParameter.AxisAjinEnum.VZ);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.VisionXZ_UVW_EZ_HomeCompleteCheck;
                    break;


                case (int)Home_Step.VisionXZ_UVW_EZ_HomeCompleteCheck:                                 //  Vision XZ, UVW EZ 축 홈 완료 체크

                    if ((TickCount_Elapsed((int)TickType.TICK_HOME) > 100) &&
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.U) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.V) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.W) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.X) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        !MC_Func.MC_GetHoming((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) && MC_Func.MC_GetInposition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "UVW, 엘리베이터, 카메라 X, 카메라 Z 축 초기화 완료");

                        m_nHomeStep = (int)Home_Step.ElevZ_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) > 60000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "UVW, 엘리베이터, 카메라 X, 카메라 Z 축 초기화 실패 (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;

                        MessageBox.Show("Vision XZ, UVW, Elev.Z 축 초기화 실패", "Error");

                        m_nHomeStep = (int)Home_Step.None;
                    }
                    break;


                case (int)Home_Step.ElevZ_Move_ReadyPos:                                    //  Elev. Z 축, 대기 위치로 이동 

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "엘리베이터 Z 축, 대기 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.ElevZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Home_Step.ElevZ_Move_ReadyPos_DoneCheck:                          //  Elev. Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "엘리베이터 Z 축, 대기 높이로 이동 완료");

                        m_nHomeStep = (int)Home_Step.UVW_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "엘리베이터 Z 축, 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Ready 위치로 이동 실패.", "Error");

                        m_nHomeStep = (int)Home_Step.None;
                    }
                    break;


                case (int)Home_Step.UVW_Move_ReadyPos:                                      //  UVW 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "UVW Stage, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.U],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.U],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.U]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.U],              //  U 축과 동일한 속도로
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.U],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.U]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.U],              //  U 축과 동일한 속도로
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.U],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.U]);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.UVW_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Home_Step.UVW_Move_ReadyPos_DoneCheck:                            //  UVW 축, 대기 위치로 이동 완료 체크

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "UVW Stage, 대기 위치로 이동 완료");

                        m_nHomeStep = (int)Home_Step.VisionXYZ_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "UVW Stage, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;

                        MessageBox.Show("UVW 축, Ready 위치로 이동 실패.", "Error");

                        m_nHomeStep = (int)Home_Step.None;
                    }
                    break;


                case (int)Home_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동 

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "카메라 XYZ 축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.X],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.X],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.X]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.Y],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.Y],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.Y]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_HOME);

                    m_nHomeStep = (int)Home_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Home_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                            //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "카메라 XYZ 축, 대기 위치로 이동 완료");

                        m_nHomeStep = (int)Home_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_HOME) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "카메라 XYZ 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_Motion_Home.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Ready 위치로 이동 실패.", "Error");

                        m_nHomeStep = (int)Home_Step.None;
                    }
                    break;


                case (int)Home_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "완료");

                    m_bHomeOK = true;

                    timer_Motion_Home.Enabled = false;

                    m_strTemp = "===  장비 초기화 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nHomeStep = (int)Home_Step.None;
                    break;
            }
        }

        #endregion

        #region Reticle Glass Check Func

 
        void Run_ReticleCheck_UpperCam_Func()                   //  Reticle Glass Check Step (Upper Camera)
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nReticleCheck_UpperCam_Step)
            {
                case (int)ReticleCheck_UpperCam_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    if (Camera_Upper != null)
                    {
                        Camera_Upper.StartLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.ReticleAlign_UpperVision_LightValue, 1);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
                    }

                    if (Camera_Lower != null)
                    {
                        Camera_Lower.StartLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.ReticleAlign_LowerVision_LightValue, 2);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
                    }

                    m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReadyPos;
                    break;


                case (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReadyPos:                                      //  Vision XY 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 XY 축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReadyPos_DoneCheck;
                    break;


                case (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReadyPos_DoneCheck:                             //  Vision XY 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))  // &&

                        //MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        //MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                        //                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 XY 축, 대기 위치로 이동 완료");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.UVW_Move_UpperCam_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 XY 축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision XY 축, Ready 위치로 이동 실패.", "Error");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_UpperCam_Step.UVW_Move_UpperCam_ReticlePos:                                      //  UVW 축, Reticle Glass 확인 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "UVW Stage, 상부 카메라 Reticle 확인 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.UVW_Move_UpperCam_ReticlePos_DoneCheck;
                    break;


                case (int)ReticleCheck_UpperCam_Step.UVW_Move_UpperCam_ReticlePos_DoneCheck:                             //  UVW 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "UVW Stage, 상부 카메라 Reticle 확인 위치로 이동 완료");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.ElevZ_Move_UpperCam_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "UVW Stage, 상부 카메라 Reticle 확인 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("UVW 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_UpperCam_Step.ElevZ_Move_UpperCam_ReticlePos:                                      //  Elev. Z 축, Reticle Glass 확인 위치로 이동 

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 Upper Cam 확인 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "엘리베이터 Z축, 상부 카메라 Reticle 확인 높이로 이동 실패. (카메라 Y 축이 충돌 위치에 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "엘리베이터 Z축, 상부 카메라 Reticle 확인 높이로 이동 시작");

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_SUB);

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.ElevZ_Move_UpperCam_ReticlePos_DoneCheck;
                    }
                    break;


                case (int)ReticleCheck_UpperCam_Step.ElevZ_Move_UpperCam_ReticlePos_DoneCheck:                             //  Elev. Z 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "엘리베이터 Z축, 상부 카메라 Reticle 확인 높이로 이동 완료");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.VisionZ_Move_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "엘리베이터 Z축, 상부 카메라 Reticle 확인 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_UpperCam_Step.VisionZ_Move_ReticlePos:                                      //  Vision Z 축, Reticle Glass 확인 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 Z축, 상부 카메라 Reticle 확인 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.VisionZ_Move_ReticlePos_DoneCheck;
                    break;


                case (int)ReticleCheck_UpperCam_Step.VisionZ_Move_ReticlePos_DoneCheck:                             //  Vision Z 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 Z축, 상부 카메라 Reticle 확인 높이로 이동 완료");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 Z축, 상부 카메라 Reticle 확인 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision Z 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReticlePos:                                      //  Vision XY 축, Reticle Glass 확인 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 XY축, 상부 카메라 Reticle 확인 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_UpperCamera");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReticlePos_DoneCheck;
                    break;


                case (int)ReticleCheck_UpperCam_Step.VisionXY_Move_ReticlePos_DoneCheck:                             //  Vision XY 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 XY축, 상부 카메라 Reticle 확인 위치로 이동 완료");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "카메라 XY축, 상부 카메라 Reticle 확인 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision XY 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_UpperCam_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Upper Cam]", "완료");

                    timer_ReticleGlass_Check.Enabled = false;

                    m_strTemp = "===  Reticle 확인 위치로 이동 완료  ===\r\n\r\n\r\n[Upper Camera]";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nReticleCheck_UpperCam_Step = (int)ReticleCheck_UpperCam_Step.None;

                    break;
            }
        }


        void Run_ReticleCheck_LowerCam_Func()                   //  Reticle Glass Check Step (Lower Camera)
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nReticleCheck_LowerCam_Step)
            {
                case (int)ReticleCheck_LowerCam_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    if (Camera_Upper != null)
                    {
                        Camera_Upper.StartLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.ReticleAlign_UpperVision_LightValue, 1);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
                    }

                    if (Camera_Lower != null)
                    {
                        Camera_Lower.StartLive();
                        visionCalibrator_Upper.Illuminator.SetVolume(Config.ParamConfig.ReticleAlign_LowerVision_LightValue, 2);
                        visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
                    }

                    m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReadyPos;
                    break;


                case (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReadyPos:                                      //  Vision XY 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 XY 축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReadyPos_DoneCheck;
                    break;


                case (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReadyPos_DoneCheck:                             //  Vision XY 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))  // &&

                        //MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        //MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                        //                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 XY 축, 대기 위치로 이동 완료");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.UVW_Move_LowerCam_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 XY 축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision XY 축, Ready 위치로 이동 실패.", "Error");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_LowerCam_Step.UVW_Move_LowerCam_ReticlePos:                                      //  UVW 축, Reticle Glass 확인 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "UVW Stage, Reticle 확인 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.UVW_Move_LowerCam_ReticlePos_DoneCheck;
                    break;


                case (int)ReticleCheck_LowerCam_Step.UVW_Move_LowerCam_ReticlePos_DoneCheck:                             //  UVW 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "UVW Stage, Reticle 확인 위치로 이동 완료");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.ElevZ_Move_LowerCam_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "UVW Stage, Reticle 확인 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("UVW 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_LowerCam_Step.ElevZ_Move_LowerCam_ReticlePos:                                      //  Elev. Z 축, Reticle Glass 확인 위치로 이동                     

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 Upper Cam 확인 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "엘리베이터 Z축, Reticle 확인 높이로 이동 실패. (카메라 Y축이 충돌 위치에 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "엘리베이터 Z축, Reticle 확인 높이로 이동 시작");

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_SUB);

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.ElevZ_Move_LowerCam_ReticlePos_DoneCheck;
                    }
                    break;


                case (int)ReticleCheck_LowerCam_Step.ElevZ_Move_LowerCam_ReticlePos_DoneCheck:                             //  Elev. Z 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "엘리베이터 Z축, Reticle 확인 높이로 이동 완료");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.VisionZ_Move_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "엘리베이터 Z축, Reticle 확인 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_LowerCam_Step.VisionZ_Move_ReticlePos:                                      //  Vision Z 축, Reticle Glass 확인 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 Z축, Reticle 확인 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.VisionZ_Move_ReticlePos_DoneCheck;
                    break;


                case (int)ReticleCheck_LowerCam_Step.VisionZ_Move_ReticlePos_DoneCheck:                             //  Vision Z 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 Z축, Reticle 확인 높이로 이동 완료");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReticlePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 Z축, Reticle 확인 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision Z 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReticlePos:                                      //  Vision XY 축, Reticle Glass 확인 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 XY축, Reticle 확인 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ReticleGlass_LowerCamera");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReticlePos_DoneCheck;
                    break;


                case (int)ReticleCheck_LowerCam_Step.VisionXY_Move_ReticlePos_DoneCheck:                             //  Vision XY 축, Reticle Glass 확인 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 XY축, Reticle 확인 위치로 이동 완료");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "카메라 XY축, Reticle 확인 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_ReticleGlass_Check.Enabled = false;

                        MessageBox.Show("Vision XY 축, Reticle 확인 위치로 이동 실패.", "Error");

                        m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;
                    }
                    break;


                case (int)ReticleCheck_LowerCam_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Reticle Glass Check [Lower Cam]", "완료");

                    timer_ReticleGlass_Check.Enabled = false;

                    m_strTemp = "===  Reticle 확인 위치로 이동 완료  ===\r\n\r\n\r\n[Lower Camera]";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nReticleCheck_LowerCam_Step = (int)ReticleCheck_LowerCam_Step.None;

                    break;
            }
        }

        #endregion

        #region Safety Position Move Func

        void Run_Safety_Position_Move_Func()                   //  Safety Position Move Step
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nSafetyPos_Move_Step)
            {
                case (int)SafetyPos_Move_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.VisionXY_Move_ReadyPos;
                    break;


                case (int)SafetyPos_Move_Step.VisionXY_Move_ReadyPos:                                      //  Vision XY 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "카메라 XY축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                    //                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.VisionXY_Move_ReadyPos_DoneCheck;
                    break;


                case (int)SafetyPos_Move_Step.VisionXY_Move_ReadyPos_DoneCheck:                             //  Vision XY 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]))  // &&

                        //MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        //MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                        //                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "카메라 XY축, 대기 위치로 이동 완료");

                        //m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.ElevZ_Move_ReadyPos;
                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.VisionZ_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "카메라 XY축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;
                    }
                    break;


                case (int)SafetyPos_Move_Step.VisionZ_Move_ReadyPos:                                      //  Vision Z 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "카메라 Z축, 대기 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.VisionZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)SafetyPos_Move_Step.VisionZ_Move_ReadyPos_DoneCheck:                             //  Vision Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "카메라 Z축, 대기 높이로 이동 완료");

                        //m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.ElevZ_Move_ReadyPos;
                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "카메라 Z축, 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision Z 축, Safety 위치로 이동 실패.", "Error");

                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;
                    }
                    break;


                case (int)SafetyPos_Move_Step.ElevZ_Move_ReadyPos:                                      //  Elev. Z 축, 대기 위치로 이동 

                    Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "엘리베이터 Z축, 대기 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.ElevZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)SafetyPos_Move_Step.ElevZ_Move_ReadyPos_DoneCheck:                             //  Elev. Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "엘리베이터 Z축, 대기 높이로 이동 완료");

                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.UVW_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "엘리베이터 Z축, 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Safety 위치로 이동 실패.", "Error");

                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;
                    }
                    break;


                case (int)SafetyPos_Move_Step.UVW_Move_ReadyPos:                                      //  UVW 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "UVW Stage, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.UVW_Move_ReadyPos_DoneCheck;
                    break;


                case (int)SafetyPos_Move_Step.UVW_Move_ReadyPos_DoneCheck:                             //  UVW 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "UVW Stage, 대기 위치로 이동 완료");

                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "UVW Stage, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("UVW 축, Safety 위치로 이동 실패.", "Error");

                        m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;
                    }
                    break;


                case (int)SafetyPos_Move_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Safety Position Move", "완료");

                    timer_SubWork.Enabled = false;

                    m_strTemp = "===  Safety 위치로 이동 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nSafetyPos_Move_Step = (int)SafetyPos_Move_Step.None;

                    break;
            }
        }
        #endregion


        #region PAK Air-Line Check Func

        void Run_PAK_AirLine_Check_Func()                   //  PAK Air Line Check Step
        {
            string m_strTemp;

            switch (m_nPAK_AirLine_Check_Step)
            {
                case (int)PAK_AirLine_Check_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_bPAK_AirLineCheck_Complete = false;
                    m_bPAK_AirLineCheck_OK = true;

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step._MachineType_Check;
                    break;


                case (int)PAK_AirLine_Check_Step._MachineType_Check:                                    //  Machine Type 확인

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "Machine Type 확인");

                    if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
                    {
                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step._MachineType_A_Start;
                    }
                    else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
                    {
                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step._MachineType_B_Start;
                    }
                    break;


                //////////////////////////////////////////////////////////////////
                //  Type-A 시작
                ///
                case (int)PAK_AirLine_Check_Step._MachineType_A_Start:                                    //  Type A 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "Machine Type A 시작");

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.PAK_Check_Condition_Check;
                    break;


                case (int)PAK_AirLine_Check_Step.PAK_Check_Condition_Check:                                     //  PAK Check 조건 확인 (Probe Card Exist)

                    if (!waferProbeAlignParameter.DI_Probe_BW_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "프로브 카드 트레이가 패킹 위치에 있지 않아서 동작 취소됨");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;
                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.None;

                        if (m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign_Step.None)
                        {
                            MessageBox.Show("프로브 카드 트레이가 패킹 위치에 있지 않습니다.", "Error");
                        }

                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.None;
                    }
                    else
                    {
                        //m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Top_Cover_Down;
                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step._MachineType_A_Complete;
                    }                    
                    break;


                case (int)PAK_AirLine_Check_Step.Top_Cover_Down:                                     //  Top Cover Down

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "상부 커버 닫기 시작");

                    waferProbeAlignParameter.DO_TopCover_Up(false);
                    waferProbeAlignParameter.DO_TopCover_Down(true);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Top_Cover_Down_Check;
                    break;


                case (int)PAK_AirLine_Check_Step.Top_Cover_Down_Check:                               //  Top Cover Down Check

                    if (!waferProbeAlignParameter.DI_TopCover_Up() && waferProbeAlignParameter.DI_TopCover_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "상부 커버 닫기 완료");

                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Packing_Signal_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "상부 커버 닫기 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        if (m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign_Step.None)
                        {
                            MessageBox.Show("Top Cover Down 실패. (Time Out)", "Error");
                        }

                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.None;
                    }
                    break;


                case (int)PAK_AirLine_Check_Step._MachineType_A_Complete:                                    //  Type A 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "Machine Type A 완료");

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Packing_Signal_On;
                    break;
                ///
                //  Type-A 완료
                //////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////
                //  Type-B 시작
                ///
                case (int)PAK_AirLine_Check_Step._MachineType_B_Start:                                    //  Type B 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "Machine Type B 시작");

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step._MachineType_B_Complete;                                       
                    break;


                //  B 타입 장비 (#2 ~ #6 호기) 는 클램핑 동작 없이 바로 Packing Signal 체크 한다.


                case (int)PAK_AirLine_Check_Step._MachineType_B_Complete:                                    //  Type B 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "Machine Type B 완료");

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Packing_Signal_On;
                    break;
                ///
                //  Type-B 완료
                //////////////////////////////////////////////////////////////////


                case (int)PAK_AirLine_Check_Step.Packing_Signal_On:                                        //  Packing Signal On

                    Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "프로브 카드 & 씬척 패킹 신호 On");

                    waferProbeAlignParameter.DO_Probe_Packing(true);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Packing_Signal_On_StableTime;
                    break;


                case (int)PAK_AirLine_Check_Step.Packing_Signal_On_StableTime:                             //  Packing Signal On 후 확인 시간

                    if (TickCount_Elapsed((int)TickType.TICK_SUB) <= Config.ParamConfig.Pak_AirLineCheck_Time)
                    {
                        if (waferProbeAlignParameter.DI_Probe_PackingCheck())
                        {
                            m_bPAK_AirLineCheck_OK = false;

                            m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Complete;
                        }
                    }
                    else
                    {
                        m_nPAK_AirLine_Check_Step = (int)PAK_AirLine_Check_Step.Complete;
                    }
                    break;


                case (int)PAK_AirLine_Check_Step.Complete:

                    waferProbeAlignParameter.DO_Probe_Packing(false);

                    m_bPAK_AirLineCheck_Complete = true;

                    timer_SubWork.Enabled = false;
                    m_nPAK_AirLine_Check_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;

                    if (m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign_Step.None)
                    {
                        if (waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlignParameter.DI_TopCover_Down())
                        {
                            Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "완료. [프로브 카드 커버가 열려있음]");

                            m_bPAK_AirLineCheck_OK = false;

                            MessageBox.Show("PAK 점검 실패.\r\n\r\n[ 프로브 카드 커버 열려있음 ] ", "Information!");
                        }
                        else
                        {
                            if (m_bPAK_AirLineCheck_OK)
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "완료. [PAK 공압 관로 정상]");

                                m_strTemp = "===  PAK 공압 관로 정상  ===";
                            }
                            else
                            {
                                Log.Write("CWA150SA", Equipment.User_Name, "PAK Air-Line Check", "완료. [PAK 공압 관로 막힘]");

                                m_strTemp = "===  PAK 공압 관로 막힘  ===";
                            }

                            MessageBox.Show(m_strTemp, "Information!");
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Wafer Loading Ready Func
        void Run_WaferLoading_Ready_Func()                   //  Wafer Loading Ready Step
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nWafer_Loading_Ready_Step)
            {
                case (int)WaferLoading_Ready_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.VisionXYZ_Move_ReadyPos;
                    break;


                case (int)WaferLoading_Ready_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "카메라 XYZ축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)WaferLoading_Ready_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "카메라 XYZ축, 대기 위치로 이동 완료");

                        m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.ElevZ_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "카메라 XYZ축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.None;
                    }
                    break;


                case (int)WaferLoading_Ready_Step.ElevZ_Move_WaferLoadingPos:                                      //  Elev. Z 축, Wafer Loading 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.ElevZ_Move_WaferLoadingPos_DoneCheck;
                    break;


                case (int)WaferLoading_Ready_Step.ElevZ_Move_WaferLoadingPos_DoneCheck:                             //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 완료");

                        m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.UVW_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.None;
                    }
                    break;


                case (int)WaferLoading_Ready_Step.UVW_Move_WaferLoadingPos:                                      //  UVW 축, Wafer Loading 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.UVW_Move_WaferLoadingPos_DoneCheck;
                    break;


                case (int)WaferLoading_Ready_Step.UVW_Move_WaferLoadingPos_DoneCheck:                             //  UVW 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 완료");

                        m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("UVW 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.None;
                    }
                    break;


                case (int)WaferLoading_Ready_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer Loading Ready Func", "완료");

                    timer_SubWork.Enabled = false;

                    m_strTemp = "===  Wafer Loading 위치로 이동 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nWafer_Loading_Ready_Step = (int)WaferLoading_Ready_Step.None;

                    break;
            }
        }
        #endregion

        #region ProbeCard Loading Ready Func
        void Run_ProbeCard_Loading_Ready_Func()                   //  Probe Card Loading Ready Step
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nProbeCard_Loading_Ready_Step)
            {
                case (int)ProbeCard_Loading_Ready_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.VisionXYZ_Move_ReadyPos;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "카메라 XYZ축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "카메라 XYZ축, 대기 위치로 이동 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ElevZ_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "카메라 XYZ축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ElevZ_Move_WaferLoadingPos:                                      //  Elev. Z 축, Wafer Loading 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ElevZ_Move_WaferLoadingPos_DoneCheck;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ElevZ_Move_WaferLoadingPos_DoneCheck:                             //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.UVW_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step.UVW_Move_WaferLoadingPos:                                      //  UVW 축, Wafer Loading 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.UVW_Move_WaferLoadingPos_DoneCheck;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.UVW_Move_WaferLoadingPos_DoneCheck:                             //  UVW 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 완료");

                        //m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.Top_Cover_Up;
                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step._MachineType_Check;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("UVW 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step._MachineType_Check:                                    //  Machine Type 확인

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "Machine Type 확인");

                    if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
                    {
                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step._MachineType_A_Start;
                    }
                    else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
                    {
                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step._MachineType_B_Start;
                    }
                    break;


                //////////////////////////////////////////////////////////////////
                //  Type-A 시작
                ///
                case (int)ProbeCard_Loading_Ready_Step._MachineType_A_Start:                                    //  Type A 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "Machine Type A 시작");

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.Top_Cover_Up;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.Top_Cover_Up:                                     //  Top Cover Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "UVW Stage, 상부 커버 Up");

                    waferProbeAlignParameter.DO_TopCover_Up(true);
                    waferProbeAlignParameter.DO_TopCover_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.Top_Cover_Up_Check;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.Top_Cover_Up_Check:                               //  Top Cover Up Check

                    if (waferProbeAlignParameter.DI_TopCover_Up() && !waferProbeAlignParameter.DI_TopCover_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "UVW Stage, 상부 커버 Up 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step._MachineType_A_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "UVW Stage, 상부 커버 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Top Cover Up 실패. (Time Out)", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step._MachineType_A_Complete:                                    //  Type A 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "Machine Type A 완료");

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.Complete;
                    break;
                ///
                //  Type-A 완료
                //////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////
                //  Type-B 시작
                ///
                case (int)ProbeCard_Loading_Ready_Step._MachineType_B_Start:                                    //  Type B 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "Machine Type B 시작");

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingSignal_Off;                                       
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingSignal_Off:                                     //  프로브 카드 패킹, 언패킹 신호 Off

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 패킹, 언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingSignal_Off_Check;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingSignal_Off_Check:                               //  프로브 카드 패킹, 언패킹 신호 Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Packing() && !waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 패킹, 언패킹 신호 Off 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingCyl_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 패킹, 언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 패킹, 언패킹 신호 Off 실패. (Time Out)", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingCyl_Up:                                     //  프로브 카드 언패킹 실린더 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 언패킹 실린더 Up");

                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingCyl_Up_Check;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_UnpackingCyl_Up_Check:                               //  프로브 카드 언패킹 실린더 Up 확인

                    if (waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up() && !waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 언패킹 실린더 Up 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 언패킹 실린더 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 언패킹 실린더 Up 실패. (Time Out)", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_Up:                                     //  프로브 카드 클램프 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 클램프 Up");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_Up_Check;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_Up_Check:                               //  프로브 카드 클램프 Up 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_SUB) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 클램프 Up 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_BW;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 클램프 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Up 실패. (Time Out)", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_BW:                                     //  프로브 카드 클램프 BW

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 클램프 BW");

                    waferProbeAlignParameter.DO_ProbeClampModule_BW(true);
                    waferProbeAlignParameter.DO_ProbeClampModule_FW(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_BW_Check;
                    break;


                case (int)ProbeCard_Loading_Ready_Step.ProbeCard_Clamp_BW_Check:                               //  프로브 카드 클램프 BW 확인

                    if (!waferProbeAlignParameter.DI_Probe_LeftClampModule_FW() && waferProbeAlignParameter.DI_Probe_LeftClampModule_BW() &&
                        !waferProbeAlignParameter.DI_Probe_RightClampModule_FW() && waferProbeAlignParameter.DI_Probe_RightClampModule_BW() )
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 클램프 BW 완료");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step._MachineType_B_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "프로브 카드 클램프 BW 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 BW 실패. (Time Out)", "Error");

                        m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;
                    }
                    break;


                case (int)ProbeCard_Loading_Ready_Step._MachineType_B_Complete:                                    //  Type B 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "Machine Type B 완료");

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.Complete;
                    break;
                ///
                //  Type-B 완료
                //////////////////////////////////////////////////////////////////


                case (int)ProbeCard_Loading_Ready_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Loading Ready Func", "완료");

                    timer_SubWork.Enabled = false;

                    m_strTemp = "===  Probe Card 장착 준비 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nProbeCard_Loading_Ready_Step = (int)ProbeCard_Loading_Ready_Step.None;

                    break;
            }
        }
        #endregion

        #region Wafer ProbeCard Unpacking Ready Func
        void Run_WaferProbeCard_Unpacking_Ready_Func()                   //  Wafer ProbeCard Unpacking Ready Step
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nWaferProbeCard_Unpacking_Ready_Step)
            {
                case (int)WaferProbeCard_Unpacking_Ready_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Unpacking_Ready_Condition_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.Unpacking_Ready_Condition_Check:                                     //  Unpacking Ready 조건 확인 (Thin Chuck None Exist)

                    if (Config.ParamConfig.ThinChuck_DetectSignal_Usage && waferProbeAlignParameter.DI_ThinChuck_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "작업 중지. (씬-척 감지됨)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Thin Chuck 이 감지됨.", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    else
                    {
                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Unpacking_Signal_Off;
                    }                    
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.Unpacking_Signal_Off:                                     //  Unpacking Signal Off

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_UnPacking(false);
                    waferProbeAlignParameter.DO_Probe_Packing(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Unpacking_Signal_Off_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.Unpacking_Signal_Off_Check:                               //  Unpacking Signal Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Unpacking() && !waferProbeAlignParameter.IsDO_Probe_Packing())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "언패킹 신호 Off 완료");

                        //m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Top_Cover_Up;
                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_Check;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Unpacking Signal Off 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_Check:                                    //  Machine Type 확인

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "Machine Type 확인");

                    if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
                    {
                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_A_Start;
                    }
                    else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
                    {
                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_B_Start;
                    }
                    break;


                //////////////////////////////////////////////////////////////////
                //  Type-A 시작
                ///
                case (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_A_Start:                                    //  Type A 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "Machine Type A 시작");

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Top_Cover_Up;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.Top_Cover_Up:                                     //  Top Cover Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "상부 커버 Up");

                    waferProbeAlignParameter.DO_TopCover_Up(true);
                    waferProbeAlignParameter.DO_TopCover_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Top_Cover_Up_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.Top_Cover_Up_Check:                               //  Top Cover Up Check

                    if (waferProbeAlignParameter.DI_TopCover_Up() && !waferProbeAlignParameter.DI_TopCover_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "상부 커버 Up 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_A_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "상부 커버 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Top Cover Up 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_A_Complete:                                    //  Type A 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "Machine Type A 완료");

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.VisionXYZ_Move_ReadyPos;
                    break;
                ///
                //  Type-A 완료
                //////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////
                //  Type-B 시작
                ///
                case (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_B_Start:                                    //  Type B 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "Machine Type B 시작");

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingSignal_Off;                                       
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingSignal_Off:                                     //  프로브 카드 패킹, 언패킹 신호 Off

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 패킹, 언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingSignal_Off_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingSignal_Off_Check:                               //  프로브 카드 패킹, 언패킹 신호 Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Packing() && !waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 패킹, 언패킹 신호 Off 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingCyl_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 패킹, 언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 패킹, 언패킹 신호 Off 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingCyl_Up:                                     //  프로브 카드 언패킹 실린더 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 언패킹 실린더 Up");

                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingCyl_Up_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_UnpackingCyl_Up_Check:                               //  프로브 카드 언패킹 실린더 Up 확인

                    if (waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up() && !waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 언패킹 실린더 Up 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 언패킹 실린더 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 언패킹 실린더 Up 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_Up:                                     //  프로브 카드 클램프 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 클램프 Up");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_Up_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_Up_Check:                               //  프로브 카드 클램프 Up 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_SUB) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 클램프 Up 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_BW;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 클램프 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Up 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_BW:                                     //  프로브 카드 클램프 BW

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 클램프 BW");

                    waferProbeAlignParameter.DO_ProbeClampModule_BW(true);
                    waferProbeAlignParameter.DO_ProbeClampModule_FW(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_BW_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ProbeCard_Clamp_BW_Check:                               //  프로브 카드 클램프 BW 확인

                    if (!waferProbeAlignParameter.DI_Probe_LeftClampModule_FW() && waferProbeAlignParameter.DI_Probe_LeftClampModule_BW() &&
                        !waferProbeAlignParameter.DI_Probe_RightClampModule_FW() && waferProbeAlignParameter.DI_Probe_RightClampModule_BW())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 클램프 BW 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_B_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "프로브 카드 클램프 BW 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 BW 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step._MachineType_B_Complete:                                    //  Type B 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "Machine Type B 완료");

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.VisionXYZ_Move_ReadyPos;
                    break;
                ///
                //  Type-B 완료
                //////////////////////////////////////////////////////////////////


                case (int)WaferProbeCard_Unpacking_Ready_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "카메라 XYZ축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "카메라 XYZ축, 대기 위치로 이동 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "카메라 XYZ축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos:                                      //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 (Unpacking 높이에서 30mm 아래)

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 중지. (카메라 Y 축이 충돌 위치에 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시작");

                        //  Packing 높이에서 30mm 아래로
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= 30.0;

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_SUB);

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos_DoneCheck:                             //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.UVW_Move_WaferProbeCard_UnpackingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.UVW_Move_WaferProbeCard_UnpackingPos:                                      //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "UVW Stage, 언패킹 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.UVW_Move_WaferProbeCard_UnpackingPos_DoneCheck;
                    break;


                case (int)WaferProbeCard_Unpacking_Ready_Step.UVW_Move_WaferProbeCard_UnpackingPos_DoneCheck:                             //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "UVW Stage, 언패킹 위치로 이동 완료");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "UVW Stage, 언패킹 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("UVW 축, Wafer ProbeCard Unpacking 준비 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;
                    }
                    break;                                    


                case (int)WaferProbeCard_Unpacking_Ready_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Unpacking Ready Func", "완료");

                    timer_SubWork.Enabled = false;

                    m_strTemp = "===  Wafer, ProbeCard 분리 준비 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeCard_Unpacking_Ready_Step.None;

                    break;
            }
        }
        #endregion

        #region ProbeCard Locking Func
        void Run_ProbeCard_Locking_Func()                       //  Probe Card Locking Step
        {
            bool m_bRet = false;
            string m_strTemp;

            switch (m_nProbeCard_Locking_Step)
            {
                case (int)ProbeCard_Locking_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Detect;
                    break;


                case (int)ProbeCard_Locking_Step._MachineType_Check:                                    //  Machine Type 확인

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "Machine Type 확인");

                    if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
                    {
                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step._MachineType_A_Start;
                    }
                    else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
                    {
                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step._MachineType_B_Start;
                    }
                    break;


                //////////////////////////////////////////////////////////////////
                //  Type-A 시작
                ///
                case (int)ProbeCard_Locking_Step._MachineType_A_Start:                                    //  Type A 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "Machine Type A 시작");

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Detect;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Detect:              //  ProbeCard 확인

                    if (!waferProbeAlignParameter.DI_Probe_BW_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드가 감지되지 않아 작업 중지");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Probe Card 가 감지되지 않음.", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    else
                    {
                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.TopCover_Down;
                    }
                    break;


                case (int)ProbeCard_Locking_Step.TopCover_Down:                                     //  Top Cover Down 

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "상부 커버 Down");

                    waferProbeAlignParameter.DO_TopCover_Up(false);
                    waferProbeAlignParameter.DO_TopCover_Down(true);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.TopCover_Down_Check;
                    break;


                case (int)ProbeCard_Locking_Step.TopCover_Down_Check:                               //  Top Cover Down 완료 확인

                    if (!waferProbeAlignParameter.DI_TopCover_Up() && waferProbeAlignParameter.DI_TopCover_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "상부 커버 Down 완료");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step._MachineType_A_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "상부 커버 Down 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("Top Cover Down 실패. (Time Out)", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    break;


                case (int)ProbeCard_Locking_Step._MachineType_A_Complete:                                    //  Type A 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "Machine Type A 완료");

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.Complete;
                    break;
                ///
                //  Type-A 완료
                //////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////
                //  Type-B 시작
                ///
                case (int)ProbeCard_Locking_Step._MachineType_B_Start:                                    //  Type B 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "Machine Type B 시작");

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_UnpackingSignal_Off;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_UnpackingSignal_Off:                                     //  프로브 카드 패킹, 언패킹 신호 Off

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 패킹, 언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_UnpackingSignal_Off_Check;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_UnpackingSignal_Off_Check:                               //  프로브 카드 패킹, 언패킹 신호 Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Packing() && !waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 패킹, 언패킹 신호 Off 완료");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_UnpackingCyl_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 패킹, 언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 패킹, 언패킹 신호 Off 실패. (Time Out)", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_UnpackingCyl_Up:                                     //  프로브 카드 언패킹 실린더 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 언패킹 실린더 Up");

                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_UnpackingCyl_Up_Check;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_UnpackingCyl_Up_Check:                               //  프로브 카드 언패킹 실린더 Up 확인

                    if (waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up() && !waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 언패킹 실린더 Up 완료");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 언패킹 실린더 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 언패킹 실린더 Up 실패. (Time Out)", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Up:                                     //  프로브 카드 클램프 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 Up");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Up_Check;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Up_Check:                               //  프로브 카드 클램프 Up 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_SUB) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 Up 완료");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Clamp_FW;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Up 실패. (Time Out)", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Clamp_FW:                                     //  프로브 카드 클램프 FW

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 FW");

                    waferProbeAlignParameter.DO_ProbeClampModule_FW(true);
                    waferProbeAlignParameter.DO_ProbeClampModule_BW(false);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Clamp_FW_Check;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Clamp_FW_Check:                               //  프로브 카드 클램프 FW 확인

                    if (waferProbeAlignParameter.DI_Probe_LeftClampModule_FW() && !waferProbeAlignParameter.DI_Probe_LeftClampModule_BW() &&
                        waferProbeAlignParameter.DI_Probe_RightClampModule_FW() && !waferProbeAlignParameter.DI_Probe_RightClampModule_BW())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 FW 완료");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Down;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 FW 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 FW 실패. (Time Out)", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Down:                                     //  프로브 카드 클램프 Down

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 Down");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(true);

                    TickCount_Start((int)TickType.TICK_SUB);

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Down_Check;
                    break;


                case (int)ProbeCard_Locking_Step.ProbeCard_Clamp_Down_Check:                               //  프로브 카드 클램프 Down 확인

                    if (waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_SUB) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 Down 완료");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step._MachineType_B_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_SUB) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "프로브 카드 클램프 Down 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_SubWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Down 실패. (Time Out)", "Error");

                        m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;
                    }
                    break;


                case (int)ProbeCard_Locking_Step._MachineType_B_Complete:                                    //  Type B 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "Machine Type B 완료");

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.Complete;
                    break;
                ///
                //  Type-B 완료
                //////////////////////////////////////////////////////////////////


                case (int)ProbeCard_Locking_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "ProbeCard Locking Func", "완료");

                    timer_SubWork.Enabled = false;

                    m_strTemp = "===  Probe Card 고정 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nProbeCard_Locking_Step = (int)ProbeCard_Locking_Step.None;

                    break;
            }
        }
        #endregion

        #region Wafer ProbeCard Packing Func
        void Run_Wafer_ProbeCard_Packing_Func()                   //  Wafer ProbeCard Packing Step
        {
            double deltaX = 0.0;
            double deltaY = 0.0;
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            double m_dCurPos_U = 0.0;
            double m_dCurPos_V = 0.0;
            double m_dCurPos_W = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nWafer_ProbeCard_Packing_Step)
            {
                case (int)WaferProbeCard_Packing_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_dWafer_ProbeCard_PackingPos_Axis_U = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_V = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_W = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Packing_Condition_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.Packing_Condition_Check:              //  Packing 조건 확인 (Thin Chuck Exist, Probe Card Exist. Top Cover Down, Thin Chuck Vacuum Check, Wafer Vacuum Check)

                    m_dCurPos_U = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
                    m_dCurPos_V = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
                    m_dCurPos_W = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);

                    if (Config.ParamConfig.ThinChuck_DetectSignal_Usage && !waferProbeAlignParameter.DI_ThinChuck_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (씬-척이 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Thin Chuck 이 감지되지 않음.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else if ((Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A) && !waferProbeAlignParameter.DI_Probe_BW_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (프로브 카드가 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Probe Card 가 감지되지 않음.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else if ((Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A) && (waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlignParameter.DI_TopCover_Down()))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (상부 커버가 Down 상태가 아님)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Top Cover 가 Down 상태가 아님.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else if (Config.ParamConfig.ThinChuck_VacuumSignal_Usage && !waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (씬-척 진공압이 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Thin Chuck Vacuum 이 감지되지 않음.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else if (Config.ParamConfig.Wafer_VacuumSignal_Usage && !waferProbeAlignParameter.DI_Wafer_VacuumCheck())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (웨이퍼 진공압이 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Wafer Vacuum 이 감지되지 않음.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else if ((m_dWafer_ProbeCard_AlignPos_Axis_U == -1) && (m_dWafer_ProbeCard_AlignPos_Axis_V == -1) && (m_dWafer_ProbeCard_AlignPos_Axis_W == -1))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (프로브 카드 얼라인이 진행되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;

                        MessageBox.Show("웨이퍼 - 프로브 카드 얼라인이 진행되지 않았습니다.\r\n\r\n[프로브 카드 패킹 위치 데이터가 없음.]", "Information");
                    }
                    else if ((m_dCurPos_U <= (m_dWafer_ProbeCard_AlignPos_Axis_U - 0.001)) ||
                            (m_dCurPos_U >= (m_dWafer_ProbeCard_AlignPos_Axis_U + 0.001)) ||
                            (m_dCurPos_V <= (m_dWafer_ProbeCard_AlignPos_Axis_V - 0.001)) ||
                            (m_dCurPos_V >= (m_dWafer_ProbeCard_AlignPos_Axis_V + 0.001)) ||
                            (m_dCurPos_W <= (m_dWafer_ProbeCard_AlignPos_Axis_W - 0.001)) ||
                            (m_dCurPos_W >= (m_dWafer_ProbeCard_AlignPos_Axis_W + 0.001)))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "작업 중지. (웨이퍼 얼라인은 완료되었으나, 패킹 위치에서 많이 벗어남)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;

                        MessageBox.Show("얼라인 스테이지가 얼라인 완료 위치에서 1um 이상 벗어났습니다.\r\n\r\n[재 얼라인 필요함.]", "Information");
                    }
                    else
                    {
                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.VisionXYZ_Move_ReadyPos;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "카메라 XYZ축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)WaferProbeCard_Packing_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "카메라 XYZ축, 대기 위치로 이동 완료");

                        if (Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Usage)
                        {
                            m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.__Wafer_OffsetMove_Start;
                        }
                        else
                        {
                            m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.__Wafer_OffsetMove_Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "카메라 XYZ축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.__Wafer_OffsetMove_Start:                                  //  Wafer Offset 이동 시작

                    //Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 패킹 오프셋 이동 시작");

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.UVW_Move_WaferPackingOffset;
                    break;


                case (int)WaferProbeCard_Packing_Step.UVW_Move_WaferPackingOffset:                                      //  UVW 축, Wafer Packing Offset 거리 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 패킹 오프셋 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                    //  UVW Stage XY 방향 옵셋 이동
                    deltaX = Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X;
                    deltaY = Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y;

                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U) + deltaX;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V) + deltaY;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W) + deltaY;

                    //  Theta 축 속도 변경
                    if (Config.ParamConfig.Align_Theta_Velocity <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U] = 10.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V] = 10.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W] = 10.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Velocity;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Velocity;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Velocity;
                    }

                    if (Config.ParamConfig.Align_Theta_Accel <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W] = 100.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Accel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Accel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Accel;
                    }

                    if (Config.ParamConfig.Align_Theta_Decel <= 0.0)
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V] = 100.0;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W] = 100.0;
                    }
                    else
                    {
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U] = Config.ParamConfig.Align_Theta_Decel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V] = Config.ParamConfig.Align_Theta_Decel;
                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W] = Config.ParamConfig.Align_Theta_Decel;

                    }

                    //  절대 위치 이동
                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.U],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.U]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.V],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.V]);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W, waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)WaferProbeAlign.nAxis.W],
                                                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)WaferProbeAlign.nAxis.W]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.UVW_Move_WaferPackingOffset_DoneCheck;
                    break;


                case (int)WaferProbeCard_Packing_Step.UVW_Move_WaferPackingOffset_DoneCheck:                             //  UVW 축, Wafer Packing Offset 거리 이동 완료 확인
                    if ((TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.WaferAlign_Move_StableTime) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 패킹 오프셋 이동 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.__Wafer_OffsetMove_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 패킹 오프셋 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("UVW Stage, 패킹 옵셋 위치로 이동 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.__Wafer_OffsetMove_Complete:                                  //  Wafer Offset 이동 완료

                    //  패킹 시 UVW 스테이지 충돌 확인
                    double m_dAxisU_Pos = 0.0;
                    double m_dAxisV_Pos = 0.0;
                    double m_dAxisW_Pos = 0.0;
                    bool m_bConflictCheck_U_OK = false;
                    bool m_bConflictCheck_V_OK = false;
                    bool m_bConflictCheck_W_OK = false;

                    if ((Config.ParamConfig.AlignLimit_UVW_U_Minus != 0) && (Config.ParamConfig.AlignLimit_UVW_U_Plus != 0))
                    {
                        m_dAxisU_Pos = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.U);
                        if ((m_dAxisU_Pos >= Config.ParamConfig.AlignLimit_UVW_U_Minus) && (m_dAxisU_Pos <= Config.ParamConfig.AlignLimit_UVW_U_Plus))
                        {
                            m_bConflictCheck_U_OK = true;
                        }
                        else
                        {
                            m_bConflictCheck_U_OK = false;
                        }
                    }
                    else
                    {
                        m_bConflictCheck_U_OK = true;
                    }

                    if ((Config.ParamConfig.AlignLimit_UVW_V_Minus != 0) && (Config.ParamConfig.AlignLimit_UVW_V_Plus != 0))
                    {
                        m_dAxisV_Pos = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.V);
                        if ((m_dAxisV_Pos >= Config.ParamConfig.AlignLimit_UVW_V_Minus) && (m_dAxisV_Pos <= Config.ParamConfig.AlignLimit_UVW_V_Plus))
                        {
                            m_bConflictCheck_V_OK = true;
                        }
                        else
                        {
                            m_bConflictCheck_V_OK = false;
                        }
                    }
                    else
                    {
                        m_bConflictCheck_V_OK = true;
                    }

                    if ((Config.ParamConfig.AlignLimit_UVW_W_Minus != 0) && (Config.ParamConfig.AlignLimit_UVW_W_Plus != 0))
                    {
                        m_dAxisW_Pos = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.W);
                        if ((m_dAxisW_Pos >= Config.ParamConfig.AlignLimit_UVW_W_Minus) && (m_dAxisW_Pos <= Config.ParamConfig.AlignLimit_UVW_W_Plus))
                        {
                            m_bConflictCheck_W_OK = true;
                        }
                        else
                        {
                            m_bConflictCheck_W_OK = false;
                        }
                    }
                    else
                    {
                        m_bConflictCheck_W_OK = true;
                    }

                    if (m_bConflictCheck_U_OK && m_bConflictCheck_V_OK && m_bConflictCheck_W_OK)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 패킹 오프셋 이동 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_FastMove_PackingReadyPos;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 패킹 오프셋 이동 완료. (프로브 카드와 충돌 가능성이 있어 작업 중지)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("얼라인 스테이지와 프로브 카드가 충돌할 가능성이 있어 작업이 중지됩니다.", "Warning!!");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_FastMove_PackingReadyPos:                                      //  Elev. Z 축, Wafer ProbeCard Packing 대기 높이로 이동 (fast)

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 대기 높이로 이동 중지. (카메라 Y 축과 충돌 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 대기 높이로 이동 시작. (고속 이동)");

                        //  Packing 높이에서 일정 거리 전까지 이동 (옵션 처리, 임시로 10mm)
                        if (Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance <= 0.0)
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= 10.0;
                        }
                        else
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance;
                        }

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_FastMove_PackingReadyPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_FastMove_PackingReadyPos_DoneCheck:                             //  Elev. Z 축, Wafer ProbeCard Packing 대기 높이로 이동 완료 확인 (fast)

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 대기 높이로 이동 완료. (고속 이동)");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_SlowMove_PackingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer ProbeCard Packing 대기 위치로 이동 (fast) 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_SlowMove_PackingPos:                                      //  Elev. Z 축, Wafer ProbeCard Packing 높이로 이동 (slow)

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_Packing = Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 높이로 이동 중지. (카메라 Y 축과 충돌 가능성이 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 높이로 이동 시작. (저속 이동)");

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_Packing,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_SlowMove_PackingPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_SlowMove_PackingPos_DoneCheck:                             //  Elev. Z 축, Wafer ProbeCard Packing 높이로 이동 완료 확인 (slow)

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 높이로 이동 완료. (저속 이동)");

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.BeforePacking_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer ProbeCard Packing 위치로 이동 (slow) 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.BeforePacking_StableTime:                             //  Packing 전 안정화 시간

                    if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.StableTime_before_PackingSignal_On)          //  임시로 3초 (옵션 처리 하자)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 패킹 높이로 이동 후 안정화 시간");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Probe_Packing;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.Probe_Packing:                                        //  Packing Signal On

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 신호 On");

                    m_dWafer_ProbeCard_PackingPos_Axis_U = MC_Func.MC_GetEncPos((int)nAxis.U);              //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_V = MC_Func.MC_GetEncPos((int)nAxis.V);              //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_W = MC_Func.MC_GetEncPos((int)nAxis.W);              //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

                    m_dWafer_ProbeCard_AlignPos_Axis_U = -1;                                                //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
                    m_dWafer_ProbeCard_AlignPos_Axis_V = -1;                                                //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
                    m_dWafer_ProbeCard_AlignPos_Axis_W = -1;                                                //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)

                    waferProbeAlignParameter.DO_Probe_Packing(true);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Probe_Packing_StableTime;
                    break;


                case (int)WaferProbeCard_Packing_Step.Probe_Packing_StableTime:                             //  Packing Signal On 후 안정화 시간

                    if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 신호 On 후 씬-척과 웨이퍼 진공압 파기하기 전까지 안정화 시간");

                        if (waferProbeAlignParameter.IsDO_Wafer_Vacuum())
                        {
                            waferProbeAlignParameter.DO_Wafer_Vacuum(false);
                        }

                        if (waferProbeAlignParameter.IsDO_ThinChuck_Vacuum())
                        {
                            waferProbeAlignParameter.DO_ThinChuck_Vacuum(false);
                        }
                    }

                    if ((TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.StableTime_after_PackingSignal_On) && 
                        ((Config.ParamConfig.Packing_VacuumSignal_Usage && waferProbeAlignParameter.DI_Probe_PackingCheck()) ||
                        (!Config.ParamConfig.Packing_VacuumSignal_Usage && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.Packing_VacuumSignal_Time))))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 신호 On 후 씬-척과 웨이퍼 진공압 파기. (진공압 신호 Off)");

                        waferProbeAlignParameter.DO_Wafer_Vacuum(false);
                        waferProbeAlignParameter.DO_ThinChuck_Vacuum(false);

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Wafer_Vacuum_Off;
                    }                    
                    break;


                case (int)WaferProbeCard_Packing_Step.Wafer_Vacuum_Off:                                     //  Wafer Vacuum Off

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 진공압 신호 Off");

                    waferProbeAlignParameter.DO_Wafer_Vacuum(false);

                    //  Wafer Vacuum 을 파기하면 얼라인을 다시 해야 하므로, Ailgn Flag 를 false 로 만든다.
                    m_bWafer_ThetaAlign_OK = false;
                    m_bWafer_XYAlign_OK = false;

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Wafer_Vacuum_Off_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.Wafer_Vacuum_Off_Check:                               //  Wafer Vacuum Off 확인

                    if ((TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.StableTime_after_WaferVacuumSignal_Off) && 
                        ((Config.ParamConfig.Wafer_VacuumSignal_Usage && waferProbeAlignParameter.DI_Probe_PackingCheck()) ||
                        (!Config.ParamConfig.Wafer_VacuumSignal_Usage && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.Wafer_VacuumSignal_Time))))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 진공압 신호 Off 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Packing_Vacuum_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "웨이퍼 진공압 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Wafer Vacuum Off 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.Packing_Vacuum_Off:                                     //  Packing Vacuum Off

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Packing_Vacuum_Off_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.Packing_Vacuum_Off_Check:                               //  Packing Vacuum Off 확인

                    if (!waferProbeAlignParameter.DI_Probe_PackingCheck()) 
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 신호 Off 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ThinChuck_Vacuum_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Wafer Vacuum Off 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ThinChuck_Vacuum_Off:                                 //  Thin Chuck Vacuum Off

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "씬-척 진공압 신호 Off");

                    waferProbeAlignParameter.DO_ThinChuck_Vacuum(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ThinChuck_Vacuum_Off_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.ThinChuck_Vacuum_Off_Check:                           //  Thin Chuck Vacuum Off 확인

                    if (!waferProbeAlignParameter.DI_ThinChuck_VacuumCheck() && TickCount_Elapsed((int)TickType.TICK_MAIN) >= Config.ParamConfig.StableTime_after_ThinChuckVacuumSignal_Off)          //  임시로 3초 (옵션 처리 하자)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "씬-척 진공압 신호 Off 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "씬-척 진공압 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Thin Chuck Vacuum Off 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingReadyPos:                      //  Elev. Z 축, Wafer Loading 대기 위치로 이동 (slow)

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_Packing = Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 대기 높이로 이동 중지. (카메라 Y축과 충돌 가능성 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 대기 높이로 이동 시작. (저속 이동)");

                        //  현재 위치에서 일정 거리만큼 천천히 내리기
                        if (Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance <= 0.0)
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) - 10.0;
                        }
                        else
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] = MC_Func.MC_GetEncPos((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) - Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance;
                        }

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_Packing,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingReadyPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingReadyPos_DoneCheck:            //  Elev. Z 축, Wafer Loading 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 대기 높이로 이동 완료. (저속 이동)");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Packing_Status_Check;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Loading 대기 위치로 이동 (slow) 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.Packing_Status_Check:                           //  Packing 이 잘 되엇는지 확인. (여기서 Thin Chuck 이 감지되면 Packing 실패)

                    if (!Config.ParamConfig.ThinChuck_DetectSignal_Usage || 
                        (Config.ParamConfig.ThinChuck_DetectSignal_Usage && !waferProbeAlignParameter.DI_ThinChuck_Detect()))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 상태 OK");

                        //m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingPos;
                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.UVW_Move_WaferLoadingPos;
                    }
                    else 
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "패킹 상태 NG. (씬-척이 분리됨)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Wafer - Probe Card Packing 실패.\r\n\r\n[Thin-Chuck 분리됨]", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingPos:                           //  Elev. Z 축, Wafer Loading 위치로 이동
                    
                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 중지. (카메라 Y축과 충돌 가능성 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시작");

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ElevZ_Move_WaferLoadingPos_DoneCheck:                 //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.UVW_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.UVW_Move_WaferLoadingPos:                             //  UVW 축, Wafer Loading 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.UVW_Move_WaferLoadingPos_DoneCheck;
                    break;


                case (int)WaferProbeCard_Packing_Step.UVW_Move_WaferLoadingPos_DoneCheck:                   //  UVW 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 완료");

                        //m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Top_Cover_Up;
                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step._MachineType_Check;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "UVW Stage, 웨이퍼 로딩 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("UVW 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step._MachineType_Check:                                    //  Machine Type 확인

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "Machine Type 확인");

                    if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
                    {
                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step._MachineType_A_Start;
                    }
                    else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
                    {
                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step._MachineType_B_Start;
                    }
                    break;


                //////////////////////////////////////////////////////////////////
                //  Type-A 시작
                ///
                case (int)WaferProbeCard_Packing_Step._MachineType_A_Start:                                    //  Type A 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "Machine Type A 시작");

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Top_Cover_Up;
                    break;


                case (int)WaferProbeCard_Packing_Step.Top_Cover_Up:                                     //  Top Cover Up

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "상부 커버 Up");

                    waferProbeAlignParameter.DO_TopCover_Up(true);
                    waferProbeAlignParameter.DO_TopCover_Down(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Top_Cover_Up_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.Top_Cover_Up_Check:                               //  Top Cover Up Check

                    if (waferProbeAlignParameter.DI_TopCover_Up() && !waferProbeAlignParameter.DI_TopCover_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "상부 커버 Up 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step._MachineType_A_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "상부 커버 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Top Cover Up 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step._MachineType_A_Complete:                                    //  Type A 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "Machine Type A 완료");

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Complete;
                    break;
                ///
                //  Type-A 완료
                //////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////
                //  Type-B 시작
                ///
                case (int)WaferProbeCard_Packing_Step._MachineType_B_Start:                                    //  Type B 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "Machine Type B 시작");

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step._MachineType_B_Complete;
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingSignal_Off:                                     //  프로브 카드 패킹, 언패킹 신호 Off

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 패킹, 언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingSignal_Off_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingSignal_Off_Check:                               //  프로브 카드 패킹, 언패킹 신호 Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Packing() && !waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 패킹, 언패킹 신호 Off 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingCyl_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 패킹, 언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 패킹, 언패킹 신호 Off 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingCyl_Up:                                     //  프로브 카드 언패킹 실린더 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 언패킹 실린더 Up");

                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingCyl_Up_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_UnpackingCyl_Up_Check:                               //  프로브 카드 언패킹 실린더 Up 확인

                    if (waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up() && !waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 언패킹 실린더 Up 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 언패킹 실린더 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 언패킹 실린더 Up 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_Up:                                     //  프로브 카드 클램프 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 클램프 Up");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_Up_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_Up_Check:                               //  프로브 카드 클램프 Up 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 클램프 Up 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_BW;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 클램프 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Up 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_BW:                                     //  프로브 카드 클램프 BW

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 클램프 BW");

                    waferProbeAlignParameter.DO_ProbeClampModule_FW(true);
                    waferProbeAlignParameter.DO_ProbeClampModule_BW(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_BW_Check;
                    break;


                case (int)WaferProbeCard_Packing_Step.ProbeCard_Clamp_BW_Check:                               //  프로브 카드 클램프 BW 확인

                    if (!waferProbeAlignParameter.DI_Probe_LeftClampModule_FW() && waferProbeAlignParameter.DI_Probe_LeftClampModule_BW() &&
                        !waferProbeAlignParameter.DI_Probe_RightClampModule_FW() && waferProbeAlignParameter.DI_Probe_RightClampModule_BW())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 클램프 BW 완료");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step._MachineType_B_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "프로브 카드 클램프 BW 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 BW 실패. (Time Out)", "Error");

                        m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Packing_Step._MachineType_B_Complete:                                    //  Type B 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "Machine Type B 완료");

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.Complete;
                    break;
                ///
                //  Type-B 완료
                //////////////////////////////////////////////////////////////////


                case (int)WaferProbeCard_Packing_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard Packing Func", "완료");

                    //  패킹을 완료했으니, 얼라인 상태 값들은 초기화 한다.
                    for (int nPos = 0; nPos < 3; nPos++)
                    {
                        AlignmentErrorCheck_Position[nPos].X = 0.0;
                        AlignmentErrorCheck_Position[nPos].Y = 0.0;

                        for (int nSide = 0; nSide < 2; nSide++)
                        {
                            AlignmentErrorCheck_Status[nPos, nSide] = false;

                            AlignmentErrorCheck_MarkPosition[nPos, nSide].X = 0.0;
                            AlignmentErrorCheck_MarkPosition[nPos, nSide].Y = 0.0;                            
                        }
                    }

                    m_nManualPacking_Step = (int)ManualPackingStep.NONE;

                    timer_MainWork.Enabled = false;

                    m_strTemp = "===  Wafer - ProbeCard Packing 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeCard_Packing_Step.None;

                    break;
            }
        }
        #endregion

        #region Wafer ProbeCard Unpacking Func
        void Run_Wafer_ProbeCard_Unacking_Func()                   //  Wafer ProbeCard Unpacking Step
        {
            double m_dSpeed_UVW = 0.0;
            double m_dSpeed_Packing = 0.0;
            double m_dSpeed_ElvXY = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            bool m_bRet = false;
            string m_strTemp;

            switch (m_nWaferProbeCard_Unpacking_Step)
            {
                case (int)WaferProbeCard_Unpacking_Step.Start:
                    //On_LogFile_Add(LOG_OPERATION, "홈 실행 루틴, 시작.");
                    //Display_Event("홈 실행 루틴 : 시작.");

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Unpacking_Condition_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Unpacking_Condition_Check:              //  Unpacking 조건 확인 (Thin Chuck Exist, Probe Card Exist)

                    if (Config.ParamConfig.ThinChuck_DetectSignal_Usage && waferProbeAlignParameter.DI_ThinChuck_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "작업 중지. (씬-척이 감지됨)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;

                        MessageBox.Show("Thin Chuck 이 감지됨.", "Error");
                    }
                    else if ((Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A) && !waferProbeAlignParameter.DI_Probe_BW_Detect())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "작업 중지. (프로브 카드가 감지되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;

                        MessageBox.Show("Probe Card 가 감지되지 않음.", "Error");
                    }
                    else if ((m_dWafer_ProbeCard_PackingPos_Axis_U == -1) && (m_dWafer_ProbeCard_PackingPos_Axis_V == -1) && (m_dWafer_ProbeCard_PackingPos_Axis_W == -1))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "작업 중지. (프로브 카드 패킹이 진행되지 않음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;

                        MessageBox.Show("웨이퍼 - 프로브 카드 패킹이 진행되지 않았습니다.\r\n\r\n[프로브 카드 패킹 위치 없음.]", "Information");
                    }
                    else
                    {
                        //m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Top_Cover_Down;
                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step._MachineType_Check;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step._MachineType_Check:                                    //  Machine Type 확인

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "Machine Type 확인");

                    if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
                    {
                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step._MachineType_A_Start;
                    }
                    else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
                    {
                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step._MachineType_B_Start;
                    }
                    break;


                //////////////////////////////////////////////////////////////////
                //  Type-A 시작
                ///
                case (int)WaferProbeCard_Unpacking_Step._MachineType_A_Start:                                    //  Type A 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "Machine Type A 시작");

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Top_Cover_Down;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Top_Cover_Down:                                     //  Top Cover Down 

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "상부 커버 Down");

                    waferProbeAlignParameter.DO_TopCover_Up(false);
                    waferProbeAlignParameter.DO_TopCover_Down(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Top_Cover_Down_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Top_Cover_Down_Check:                               //  Top Cover Down 완료 확인

                    if (!waferProbeAlignParameter.DI_TopCover_Up() && waferProbeAlignParameter.DI_TopCover_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "상부 커버 Down 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step._MachineType_A_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "상부 커버 Down 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Top Cover Down 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step._MachineType_A_Complete:                                    //  Type A 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "Machine Type A 완료");

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.VisionXYZ_Move_ReadyPos;
                    break;
                ///
                //  Type-A 완료
                //////////////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////////////
                //  Type-B 시작
                ///
                case (int)WaferProbeCard_Unpacking_Step._MachineType_B_Start:                                    //  Type B 시작

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "Machine Type B 시작");

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingSignal_Off;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingSignal_Off:                                     //  프로브 카드 패킹, 언패킹 신호 Off

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 패킹, 언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingSignal_Off_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingSignal_Off_Check:                               //  프로브 카드 패킹, 언패킹 신호 Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Packing() && !waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 패킹, 언패킹 신호 Off 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 패킹, 언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 패킹, 언패킹 신호 Off 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Up:                                     //  프로브 카드 언패킹 실린더 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 언패킹 실린더 Up");

                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Up_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Up_Check:                               //  프로브 카드 언패킹 실린더 Up 확인

                    if (waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up() && !waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 언패킹 실린더 Up 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Up;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 언패킹 실린더 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 언패킹 실린더 Up 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Up:                                     //  프로브 카드 클램프 Up

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 Up");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Up_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Up_Check:                               //  프로브 카드 클램프 Up 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 Up 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_FW;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 Up 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Up 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_FW:                                     //  프로브 카드 클램프 FW

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 FW");

                    waferProbeAlignParameter.DO_ProbeClampModule_FW(true);
                    waferProbeAlignParameter.DO_ProbeClampModule_BW(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_FW_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_FW_Check:                               //  프로브 카드 클램프 FW 확인

                    if (waferProbeAlignParameter.DI_Probe_LeftClampModule_FW() && !waferProbeAlignParameter.DI_Probe_LeftClampModule_BW() &&
                        waferProbeAlignParameter.DI_Probe_RightClampModule_FW() && !waferProbeAlignParameter.DI_Probe_RightClampModule_BW())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 FW 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Down;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 FW 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 FW 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Down:                                     //  프로브 카드 클램프 Down

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 Down");

                    waferProbeAlignParameter.DO_ProbeClampModule_Down(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Down_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_Clamp_Down_Check:                               //  프로브 카드 클램프 Down 확인

                    if (waferProbeAlignParameter.IsDO_Probe_ClampModule_Down() && (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 1000))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 Down 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Down;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 클램프 Down 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 클램프 Down 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Down:                                     //  프로브 카드 언패킹 실린더 Down

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 언패킹 실린더 Down");

                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(false);
                    waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Down_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ProbeCard_UnpackingCyl_Down_Check:                               //  프로브 카드 언패킹 실린더 Down 확인

                    if (!waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up() && waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 언패킹 실린더 Down 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step._MachineType_B_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "프로브 카드 언패킹 실린더 Down 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("프로브 카드 언패킹 실린더 Down 실패. (Time Out)", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step._MachineType_B_Complete:                                    //  Type B 완료

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "Machine Type B 완료");

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.VisionXYZ_Move_ReadyPos;
                    break;
                ///
                //  Type-B 완료
                //////////////////////////////////////////////////////////////////


                case (int)WaferProbeCard_Unpacking_Step.VisionXYZ_Move_ReadyPos:                                      //  Vision XYZ 축, 대기 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "카메라 XYZ축, 대기 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Ready");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y],
                                        m_dSpeed_ElvXY,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                        m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.VisionXYZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.VisionXYZ_Move_ReadyPos_DoneCheck:                             //  Vision XYZ 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.X) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.X,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.Y) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.Y,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.VZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "카메라 XYZ축, 대기 위치로 이동 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.UVW_Move_WaferProbeCard_UnpackingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "카메라 XYZ축, 대기 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision XYZ 축, Safety 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.UVW_Move_WaferProbeCard_UnpackingPos:                                      //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "UVW Stage, 언패킹 위치로 이동 시작");

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  언패킹 위치 (== 마지막 패킹 위치)
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U] = m_dWafer_ProbeCard_PackingPos_Axis_U;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V] = m_dWafer_ProbeCard_PackingPos_Axis_V;
                    waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W] = m_dWafer_ProbeCard_PackingPos_Axis_W;

                    //  속도
                    m_dSpeed_UVW = Config.ParamConfig.Speed_Wafer_Align_Stage <= 0.0 ? 10 : Config.ParamConfig.Speed_Wafer_Align_Stage;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                        waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W],
                                        m_dSpeed_UVW,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec,
                                        m_dSpeed_UVW * m_dSpeedMag_forAccDec);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.UVW_Move_WaferProbeCard_UnpackingPos_DoneCheck;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.UVW_Move_WaferProbeCard_UnpackingPos_DoneCheck:                             //  UVW 축, Wafer ProbeCard Unpacking 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.U) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.U,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.U]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.V) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.V,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.V]) &&

                        MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.W) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.W,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.W]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "UVW Stage, 언패킹 위치로 이동 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "UVW Stage, 언패킹 위치로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("UVW 축, Wafer ProbeCard Unpacking 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos:                                      //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 (Unpacking 높이에서 10mm 아래, fast)

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 중지. (카메라 Y축과 충돌 가능성 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시작. (고속 이동)");

                        //  Packing 높이에서 10mm 아래로
                        if (Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance <= 0.0)
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= 10.0;
                        }
                        else
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance;
                        }

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos_DoneCheck:                             //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 완료. (고속 이동)");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingPos:                                      //  Elev. Z 축, Wafer ProbeCard Unpacking 위치로 이동 (slow)
                    
                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_Packing = Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove <= 0.0 ? 5 : (Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove / 2.0);

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 높이로 이동 중지. (카메라 Y축과 충돌 가능성 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 높이로 이동 시작. (저속 이동)");

                        //  Packing 높이에서 1mm 아래로
                        if (Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance <= -5.0)
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= 1.0;
                        }
                        else
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance;
                        }

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_Packing,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingPos_DoneCheck:                             //  Elev. Z 축, Wafer ProbeCard Unpacking 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 높이로 이동 완료. (저속 이동)");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ThinChuck_Wafer_Vacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer ProbeCard Unpacking 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ThinChuck_Wafer_Vacuum_On:                                        //  Thin-Chuck 과 웨이퍼 공압 On

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "씬-척 진공압 On, 웨이퍼 진공압 On");

                    waferProbeAlignParameter.DO_ThinChuck_Vacuum(true);
                    waferProbeAlignParameter.DO_Wafer_Vacuum(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ThinChuck_Wafer_Vacuum_On_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ThinChuck_Wafer_Vacuum_On_Check:                                  //  Thin-Chuck 과 웨이퍼 공압 On 확인

                    if (waferProbeAlignParameter.DI_ThinChuck_VacuumCheck() && waferProbeAlignParameter.DI_Wafer_VacuumCheck())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "씬-척 진공압 On 완료, 웨이퍼 진공압 On 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "씬-척 진공압 On, 웨이퍼 진공압 On, 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Thin-Chuck 과 Wafer 공압 체크 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_On:                                        //  Unpacking Signal On

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 On");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(true);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_On_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_On_Check:                                  //  Unpacking Signal On 확인

                    if (waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 On 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_On_StableTime;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 On 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Wafer ProbeCard Unpacking Signal On 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_On_StableTime:                             //  Unpacking Signal On 안정화 시간

                    if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 1000)          //  임시로 1초 (옵션 처리 하자)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 On 후 안정화 시간");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos2;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos2:                                      //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 (Unpacking 높이에서 10mm 아래, slow)

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("ProbeWafer_Packing");

                    //  속도
                    m_dSpeed_Packing = Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove <= 0.0 ? 5 : (Config.ParamConfig.Speed_Wafer_ProbeCard_Packing_LastMove / 2.0);

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 중지. (카메라 Y축에 충돌 가능성 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시작. (저속 이동)");

                        //  Packing 높이에서 5mm + 1mm 아래로
                        if (Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance <= 0.0)
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= 6.0;
                        }
                        else
                        {
                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ] -= (Config.ParamConfig.Wafer_ProbeCard_UnPackingOffset_Distance + Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance);
                        }

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_Packing,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Packing * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos2_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferProbeCard_UnpackingReadyPos2_DoneCheck:                             //  Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 완료. (저속 이동)");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 언패킹 대기 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer ProbeCard Unpacking 대기 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_Off:                                        //  Unpacking Signal Off

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 Off");

                    waferProbeAlignParameter.DO_Probe_Packing(false);
                    waferProbeAlignParameter.DO_Probe_UnPacking(false);

                    TickCount_Start((int)TickType.TICK_MAIN);

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_Off_Check;
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Unpacking_Signal_Off_Check:                                  //  Unpacking Signal Off 확인

                    if (!waferProbeAlignParameter.IsDO_Probe_Unpacking())
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 Off 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferLoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "언패킹 신호 Off 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer ProbeCard Unpacking Signal Off 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferLoadingPos:                           //  Elev. Z 축, Wafer Loading 위치로 이동 

                    waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlignParameter.GetPositionInformation("Load");

                    //  속도
                    m_dSpeed_ElvXY = Config.ParamConfig.Speed_Operation_Elev <= 0.0 ? 50 : Config.ParamConfig.Speed_Operation_Elev;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = Config.ParamConfig.Speed_Operation_Mag_forAccDec <= 0.0 ? 5 : Config.ParamConfig.Speed_Operation_Mag_forAccDec;

                    //  Elev. Z 축이 UnPacking 위치로 이동할 때, Vision Y 축 위치가 충돌 한계 위치를 초과하는 지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.Y) >= Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 중지. (카메라 Y축에 충돌 가능성 있음)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Vision Y 축이 Elev. Z 축과 충돌하는 위치에 있어 작업 중지.", "Information");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    else
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시작");

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            m_dSpeed_ElvXY,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec,
                                            m_dSpeed_ElvXY * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_MAIN);

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferLoadingPos_DoneCheck;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.ElevZ_Move_WaferLoadingPos_DoneCheck:                 //  Elev. Z 축, Wafer Loading 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)WaferProbeAlignParameter.AxisAjinEnum.EZ) &&
                        MC_Func.MC_PosTolerance((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ]))
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 완료");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_MAIN) >= 10000)
                    {
                        Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "엘리베이터 Z축, 웨이퍼 로딩 높이로 이동 시간 초과. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        timer_MainWork.Enabled = false;

                        MessageBox.Show("Elev. Z 축, Wafer Loading 위치로 이동 실패.", "Error");

                        m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;
                    }
                    break;


                case (int)WaferProbeCard_Unpacking_Step.Complete:

                    Log.Write("CWA150SA", Equipment.User_Name, "Wafer-ProbeCard UnPacking Func", "완료");

                    m_dWafer_ProbeCard_PackingPos_Axis_U = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_V = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                    m_dWafer_ProbeCard_PackingPos_Axis_W = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

                    timer_MainWork.Enabled = false;

                    m_strTemp = "===  Wafer ProbeCard Unpacking 완료  ===";

                    MessageBox.Show(m_strTemp, "Information!");

                    m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeCard_Unpacking_Step.None;

                    break;
            }
        }
        #endregion


        #region => PEG(Position Event Generation)
        /****       Position Event Generation(PEG)      ****
         * 
         * The UDMhp/ba supports advanced Position Event Generator(referred to also as Position Output Compare) output signals 
         * for synchronous random and incremental timing generation.The two PEG Pulses and eight PEG STATE signals can be associated 
         * with any of the incremental encoders, and can be programmed for polarity and shape.
         * The product supports 3 PEG generators, each of which can be associated with the available encoders. 
         * 
         * s the ability to generate a series of evenly spaced fixed width pulses, starting and ending at predefined start and end points.
         *    
         * 2) Random PEG mode 
         *    It Provides the ability to control a PEG Pulse and a four - bit STATE vector at pre - defined positions, 
         *    out of a 1024 member user - defined array per each PEG - generator.Moreover, since the PEG signals from each engine can be 
         *    'OR'ed so that they all result in a signal coming out of a single interface, a maximal array of 3x1024 position points per 
         *    selected axis(3072 points) is supported.
         ****/

        /****       Non-NT Position Event Generation (PEG) Methods      ****
         * 
         * PegInc     : Sets incremental PEG
         * PegRandom  : Sets random PEG
         * AssignPins : Defines whether a digital output is allocated to the corresponding bit of the OUT array (for general purpose use) or allocated for PEG method use.
         * StopPeg    : Stops PEG
         * 
         ****/

        /****       NT Position Event Generation (PEG) Methods      ****
         * 
         * AssignPegNT          : Assigns engine-to-encoder as well as additional digital outputs for use as PEG State and PEG Pulse outputs.
         * AssignPegOutputsNT   : Sets output pins assignment and mapping between FGP_OUT signals to the bits of the ACSPL+ OUT(x) variable.
         * AssignFastInputsNT   : Used for switching MARK_1 physical inputs to ACSPL+ variables as Fast General Purpose inputs.
         * PegIncNT             : Sets the parameters for the Incremental PEG mode.
         * PegRandomNT          : Sets the parameters for the Random PEG mode.
         * WaitPegReadyNT       : Waits for the all values to be loaded and the PEG engine to be ready to respond to movement.
         * StartPegNT           : Initiates the PEG process on the specified axis.
         * StopPegNT            : Terminates the PEG process immediately on the specified axis.
         * 
         ****/

        public bool RunRandomPEG(double pulsewidth_ms, int[] stateArray, int startIdx, int endIdx, double[] randomposArr,
            double startpos, double endpos, double speed)
        {
            if (randomposArr.Length > 1024)
                return false;

            ACSSPiiPlusMotionBoard.Api.WriteVariable(stateArray, "STAT", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(startIdx, "STARTIDX", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(endIdx, "ENDIDX", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(randomposArr, "ARR", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(startpos, "STARTPOS", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(endpos, "ENDPOS", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(speed, "SPEED", ProgramBuffer.ACSC_BUFFER_4);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(pulsewidth_ms, "PULSEWIDTH", ProgramBuffer.ACSC_BUFFER_4);

            ACSSPiiPlusMotionBoard.Api.RunBuffer(ProgramBuffer.ACSC_BUFFER_4, null);
            return true;
        }

        public bool RunRandomPEGState()
        {
            var RunState = ACSSPiiPlusMotionBoard.Api.GetProgramState(ProgramBuffer.ACSC_BUFFER_4);


            if ((RunState & ProgramStates.ACSC_PST_RUN) == 0)
                return false;
            else
                return true;
        }

        public bool RunIncrementalPEG(double pulsewidth_ms, double firstpoint, double increment,
            double lastpoint, double startpos, double endpos, double speed)
        {
            ACSSPiiPlusMotionBoard.Api.WriteVariable(pulsewidth_ms, "PULSEWIDTH", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(firstpoint, "FIRSTPOINT", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(increment, "INCREMENT", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(lastpoint, "LASTPOINT", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(startpos, "STARTPOS", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(endpos, "ENDPOS", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(speed, "SPEED", ProgramBuffer.ACSC_BUFFER_9);

            ACSSPiiPlusMotionBoard.Api.RunBuffer(ProgramBuffer.ACSC_BUFFER_9, null);

            return true;
        }

        public bool RunIncrementalLCI(double pulsewidth_ms, double firstpoint, double increment,
            double lastpoint, double startpos, double endpos, double speed, double delay_AfterStartPosMoved, double delay_AfterLciInit, double delay_AfterSetMotion, double delay_AfterEndPosMoved)
        {
            if (delay_AfterStartPosMoved <= 0)
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(500, "LCI_STABLETIME_AFTER_STARTPOS_MOVED", ProgramBuffer.ACSC_BUFFER_2);
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(delay_AfterStartPosMoved, "LCI_STABLETIME_AFTER_STARTPOS_MOVED", ProgramBuffer.ACSC_BUFFER_2);
            }

            if (delay_AfterLciInit <= 0)
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(100, "LCI_STABLETIME_AFTER_LCI_INIT", ProgramBuffer.ACSC_BUFFER_2);
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(delay_AfterLciInit, "LCI_STABLETIME_AFTER_LCI_INIT", ProgramBuffer.ACSC_BUFFER_2);
            }

            if (delay_AfterSetMotion <= 0)
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(100, "LCI_STABLETIME_AFTER_SETMOTION", ProgramBuffer.ACSC_BUFFER_2);
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(delay_AfterSetMotion, "LCI_STABLETIME_AFTER_SETMOTION", ProgramBuffer.ACSC_BUFFER_2);
            }

            if (delay_AfterEndPosMoved <= 0)
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(100, "LCI_STABLETIME_AFTER_ENDPOS_MOVED", ProgramBuffer.ACSC_BUFFER_2);
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.WriteVariable(delay_AfterEndPosMoved, "LCI_STABLETIME_AFTER_ENDPOS_MOVED", ProgramBuffer.ACSC_BUFFER_2);
            }

            ACSSPiiPlusMotionBoard.Api.WriteVariable(pulsewidth_ms, "LCI_PULSEWIDTH", ProgramBuffer.ACSC_BUFFER_2);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(firstpoint, "LCI_FIRSTPOINT", ProgramBuffer.ACSC_BUFFER_2);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(increment, "LCI_INCREMENT", ProgramBuffer.ACSC_BUFFER_2);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(lastpoint, "LCI_LASTPOINT", ProgramBuffer.ACSC_BUFFER_2);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(startpos, "LCI_STARTPOS", ProgramBuffer.ACSC_BUFFER_2);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(endpos, "LCI_ENDPOS", ProgramBuffer.ACSC_BUFFER_2);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(speed, "LCI_SPEED", ProgramBuffer.ACSC_BUFFER_2);

            ACSSPiiPlusMotionBoard.Api.RunBuffer(ProgramBuffer.ACSC_BUFFER_2, null);

            return true;
        }

        public bool RunIncrementalPEG(double pulsewidth_ms, double firstpoint, double increment,
            double lastpoint, double startpos, double endpos, double speed, double offset = 0)
        {
            firstpoint += offset;
            startpos += offset;
            lastpoint += offset;
            endpos += offset;

            ACSSPiiPlusMotionBoard.Api.WriteVariable(pulsewidth_ms, "PULSEWIDTH", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(firstpoint, "FIRSTPOINT", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(increment, "INCREMENT", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(lastpoint, "LASTPOINT", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(startpos, "STARTPOS", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(endpos, "ENDPOS", ProgramBuffer.ACSC_BUFFER_9);
            ACSSPiiPlusMotionBoard.Api.WriteVariable(speed, "SPEED", ProgramBuffer.ACSC_BUFFER_9);

            ACSSPiiPlusMotionBoard.Api.RunBuffer(ProgramBuffer.ACSC_BUFFER_9, null);

            return true;
        }

        public bool RunIncrementalPEGState()
        {
            var RunState = ACSSPiiPlusMotionBoard.Api.GetProgramState(ProgramBuffer.ACSC_BUFFER_9);

            if ((RunState & ProgramStates.ACSC_PST_RUN) == 0)
                return false;
            else
                return true;
        }

        public bool RunIncrementalLCIState()
        {
            var RunState = ACSSPiiPlusMotionBoard.Api.GetProgramState(ProgramBuffer.ACSC_BUFFER_2);

            if ((RunState & ProgramStates.ACSC_PST_RUN) == 0)
                return false;
            else
                return true;
        }

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

            if (File.Exists(strFIle))
            {
                m_bRet = true;

                                
                //  구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치
                NativeMethods.GetPrivateProfileString("Drive_Limit", "Elev_Z", "58.0", temp, 255, strFIle);
                Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign = Convert.ToDouble(temp.ToString());

                //  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
                NativeMethods.GetPrivateProfileString("Drive_Limit", "Vision_Y", "20.0", temp, 255, strFIle);
                Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ = Convert.ToDouble(temp.ToString());

                //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
                NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Minus", "-1.915", temp, 255, strFIle);
                Config.ParamConfig.AlignLimit_UVW_U_Minus = Convert.ToDouble(temp.ToString());

                //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
                NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Plus", "0.085", temp, 255, strFIle);
                Config.ParamConfig.AlignLimit_UVW_U_Plus = Convert.ToDouble(temp.ToString());

                //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
                NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Minus", "-0.805", temp, 255, strFIle);
                Config.ParamConfig.AlignLimit_UVW_V_Minus = Convert.ToDouble(temp.ToString());

                //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
                NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Plus", "1.195", temp, 255, strFIle);
                Config.ParamConfig.AlignLimit_UVW_V_Plus = Convert.ToDouble(temp.ToString());

                //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
                NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Minus", "-0.075", temp, 255, strFIle);
                Config.ParamConfig.AlignLimit_UVW_W_Minus = Convert.ToDouble(temp.ToString());

                //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
                NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Plus", "1.925", temp, 255, strFIle);
                Config.ParamConfig.AlignLimit_UVW_W_Plus = Convert.ToDouble(temp.ToString());

                //  비전 스케일 - Manual Scale Usage
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Manual_Scale_Use", "True", temp, 255, strFIle);
                Config.ParamConfig.ManualScale_Usage = temp.ToString() == "False" ? false : true;

                //  비전 스케일 - Lower Vision Scale X (mm)
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X", "0.001726468", temp, 255, strFIle);
                Config.ParamConfig.LowerVision_Scale_X = Convert.ToDouble(temp.ToString());

                //  비전 스케일 - Lower Vision Scale Y (mm)
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y", "0.001726468", temp, 255, strFIle);
                Config.ParamConfig.LowerVision_Scale_Y = Convert.ToDouble(temp.ToString());

                //  비전 스케일 - Lower Vision Scale Invert X
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", "False", temp, 255, strFIle);
                Config.ParamConfig.LowerVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

                //  비전 스케일 - Lower Vision Scale Invert Y
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", "False", temp, 255, strFIle);
                Config.ParamConfig.LowerVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

                //  비전 스케일 - Upper Vision Scale X (mm)
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X", "0.001726468", temp, 255, strFIle);
                Config.ParamConfig.UpperVision_Scale_X = Convert.ToDouble(temp.ToString());

                //  비전 스케일 - Upper Vision Scale Y (mm)
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y", "0.001726468", temp, 255, strFIle);
                Config.ParamConfig.UpperVision_Scale_Y = Convert.ToDouble(temp.ToString());

                //  비전 스케일 - Upper Vision Scale Invert X
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", "False", temp, 255, strFIle);
                Config.ParamConfig.UpperVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

                //  비전 스케일 - Upper Vision Scale Invert Y
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", "False", temp, 255, strFIle);
                Config.ParamConfig.UpperVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

                //  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
                NativeMethods.GetPrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", "602", temp, 255, strFIle);
                Config.ParamConfig.ReticleAlign_UpperVision_LightValue = Convert.ToInt16(temp.ToString());

                //  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
                NativeMethods.GetPrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", "964", temp, 255, strFIle);
                Config.ParamConfig.ReticleAlign_LowerVision_LightValue = Convert.ToInt16(temp.ToString());

                //  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
                NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Speed", "5.0", temp, 255, strFIle);
                Config.ParamConfig.Align_Theta_Velocity = Convert.ToDouble(temp.ToString());

                //  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
                NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Accel", "100.0", temp, 255, strFIle);
                Config.ParamConfig.Align_Theta_Accel = Convert.ToDouble(temp.ToString());

                //  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
                NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Decel", "100.0", temp, 255, strFIle);
                Config.ParamConfig.Align_Theta_Decel = Convert.ToDouble(temp.ToString());

                //  얼라인 - 얼라인 재시도 회수
                NativeMethods.GetPrivateProfileString("Vision_Align", "Align_Retry", "10", temp, 255, strFIle);
                Config.ParamConfig.Align_Retries = Convert.ToInt16(temp.ToString());

                //  얼라인 - 얼라인 각도 Invert
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Angle_Invert", "True", temp, 255, strFIle);
                Config.ParamConfig.Align_AngleInvert = temp.ToString() == "False" ? false : true;

                //  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
                NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", "True", temp, 255, strFIle);
                Config.ParamConfig.Align_ThetaCalcFunction_Atan = temp.ToString() == "False" ? false : true;

                //  얼라인 스테이지 - Theta 회전 반경 (mm)
                NativeMethods.GetPrivateProfileString("Align_Stage", "Turning_Radius", "70.0", temp, 255, strFIle);
                Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance = Convert.ToDouble(temp.ToString());

                //  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
                NativeMethods.GetPrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", "1.221668451", temp, 255, strFIle);
                Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg = Convert.ToDouble(temp.ToString());

                //  얼라인 이미지 저장 여부
                NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_Use", "True", temp, 255, strFIle);
                Config.ParamConfig.AlignImageSave_Usage = temp.ToString() == "False" ? false : true;

                //  얼라인 이미지 저장 위치 용량 부족 경고 여부 (D 드라이브)
                NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Use", "True", temp, 255, strFIle);
                Config.ParamConfig.AlignImageSaveFolder_FreeSpaceCheck_Usage = temp.ToString() == "False" ? false : true;

                //  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
                NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", "10", temp, 255, strFIle);
                Config.ParamConfig.AlignImageSaveFolder_WarningSpace = Convert.ToDouble(temp.ToString());

                //  프로브 카드 클램프 타입 (0:1호기, 1:2~6호기)
                NativeMethods.GetPrivateProfileString("Machine_Type", "ProbeCard_Clamp_Type", "0", temp, 255, strFIle);
                Config.ParamConfig.ProbeCard_ClampType = Convert.ToInt16(temp.ToString());



                /// Position 로드
                /// 
                int m_nIndex_Ready = -1;
                int m_nIndex_Load = -1;
                int m_nIndex_UnLoad = -1;
                int m_nIndex_Reticle_UpperCam = -1;
                int m_nIndex_Reticle_LowerCam = -1;

                for (int i = 0; i < Config.Positions.Count; i++)
                {
                    //  Ready
                    if (Config.Positions[i].Name == "Ready")
                    {
                        m_nIndex_Ready = i;
                    }

                    //  Load
                    if (Config.Positions[i].Name == "Load")
                    {
                        m_nIndex_Load = i;
                    }

                    //  UnLoad
                    if (Config.Positions[i].Name == "UnLoad")
                    {
                        m_nIndex_UnLoad = i;
                    }

                    //  Reticle Glass 를 보는 Upper Camera 위치 Index
                    if (Config.Positions[i].Name == "ReticleGlass_UpperCamera")
                    {
                        m_nIndex_Reticle_UpperCam = i;
                    }

                    //  Reticle Glass 를 보는 Lower Camera 위치 Index
                    if (Config.Positions[i].Name == "ReticleGlass_LowerCamera")
                    {
                        m_nIndex_Reticle_LowerCam = i;
                    }

                    if ((m_nIndex_Ready != -1) && (m_nIndex_Load != -1) && (m_nIndex_UnLoad != -1) &&
                        (m_nIndex_Reticle_UpperCam != -1) && (m_nIndex_Reticle_LowerCam != -1))
                    {
                        break;
                    }
                }

                //  Ready 좌표 로드
                if (m_nIndex_Ready != -1)
                {
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_U", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].U = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_V", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].V = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_W", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].W = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_EZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].EZ = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_X", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].X = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_Y", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].Y = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_VZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Ready].VZ = Convert.ToDouble(temp.ToString());
                }

                //  Load 좌표 로드
                if (m_nIndex_Load != -1)
                {
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_U", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].U = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_V", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].V = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_W", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].W = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_EZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].EZ = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_X", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].X = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_Y", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].Y = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_VZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Load].VZ = Convert.ToDouble(temp.ToString());
                }

                //  UnLoad 좌표 로드
                if (m_nIndex_UnLoad != -1)
                {
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_U", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].U = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_V", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].V = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_W", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].W = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].EZ = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_X", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].X = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_Y", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].Y = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_UnLoad].VZ = Convert.ToDouble(temp.ToString());
                }

                //  Upper Cam 좌표 로드
                if (m_nIndex_Reticle_UpperCam != -1)
                {
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].U = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].V = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].W = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].EZ = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].X = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].Y = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Convert.ToDouble(temp.ToString());
                }

                //  Lower Cam 좌표 로드
                if (m_nIndex_Reticle_LowerCam != -1)
                {
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].U = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].V = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].W = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].EZ = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].X = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].Y = Convert.ToDouble(temp.ToString());
                    NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", "0.0", temp, 255, strFIle);
                    Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Convert.ToDouble(temp.ToString());
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

                DataManager.Instance.ApplyConfigData(this);

                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
            }

            return m_bRet;
        }

        public void Machine_Parameter_Save()
        {
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                //File.Create(strFIle);
                return;
            }


            /// Config 저장
            /// 

            //  구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치
            NativeMethods.WritePrivateProfileString("Drive_Limit", "Elev_Z", Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign.ToString(), strFIle);

            //  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
            NativeMethods.WritePrivateProfileString("Drive_Limit", "Vision_Y", Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ.ToString(), strFIle);

            //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
            NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_U_Minus", Config.ParamConfig.AlignLimit_UVW_U_Minus.ToString(), strFIle);

            //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
            NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_U_Plus", Config.ParamConfig.AlignLimit_UVW_U_Plus.ToString(), strFIle);

            //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
            NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_V_Minus", Config.ParamConfig.AlignLimit_UVW_V_Minus.ToString(), strFIle);

            //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
            NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_V_Plus", Config.ParamConfig.AlignLimit_UVW_V_Plus.ToString(), strFIle);

            //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
            NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_W_Minus", Config.ParamConfig.AlignLimit_UVW_W_Minus.ToString(), strFIle);

            //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
            NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_W_Plus", Config.ParamConfig.AlignLimit_UVW_W_Plus.ToString(), strFIle);

            //  비전 스케일 - Manual Scale Usage
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Manual_Scale_Use", Config.ParamConfig.ManualScale_Usage.ToString(), strFIle);

            //  비전 스케일 - Lower Vision Scale X (mm)
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_X", Config.ParamConfig.LowerVision_Scale_X.ToString(), strFIle);

            //  비전 스케일 - Lower Vision Scale Y (mm)
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_Y", Config.ParamConfig.LowerVision_Scale_Y.ToString(), strFIle);

            //  비전 스케일 - Lower Vision Scale Invert X
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", Config.ParamConfig.LowerVision_ScaleInvert_X.ToString(), strFIle);

            //  비전 스케일 - Lower Vision Scale Invert Y
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", Config.ParamConfig.LowerVision_ScaleInvert_Y.ToString(), strFIle);

            //  비전 스케일 - Upper Vision Scale X (mm)
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_X", Config.ParamConfig.UpperVision_Scale_X.ToString(), strFIle);

            //  비전 스케일 - Upper Vision Scale Y (mm)
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_Y", Config.ParamConfig.UpperVision_Scale_Y.ToString(), strFIle);

            //  비전 스케일 - Upper Vision Scale Invert X
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", Config.ParamConfig.UpperVision_ScaleInvert_X.ToString(), strFIle);

            //  비전 스케일 - Upper Vision Scale Invert Y
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", Config.ParamConfig.UpperVision_ScaleInvert_Y.ToString(), strFIle);

            //  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
            NativeMethods.WritePrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", Config.ParamConfig.ReticleAlign_UpperVision_LightValue.ToString(), strFIle);

            //  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
            NativeMethods.WritePrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", Config.ParamConfig.ReticleAlign_LowerVision_LightValue.ToString(), strFIle);

            //  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
            NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Speed", Config.ParamConfig.Align_Theta_Velocity.ToString(), strFIle);

            //  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
            NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Accel", Config.ParamConfig.Align_Theta_Accel.ToString(), strFIle);

            //  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
            NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Decel", Config.ParamConfig.Align_Theta_Decel.ToString(), strFIle);

            //  얼라인 - 얼라인 재시도 회수
            NativeMethods.WritePrivateProfileString("Vision_Align", "Align_Retry", Config.ParamConfig.Align_Retries.ToString(), strFIle);

            //  얼라인 - 얼라인 각도 Invert
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Align_Angle_Invert", Config.ParamConfig.Align_AngleInvert.ToString(), strFIle);

            //  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
            NativeMethods.WritePrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", Config.ParamConfig.Align_ThetaCalcFunction_Atan.ToString(), strFIle);

            //  얼라인 스테이지 - Theta 회전 반경 (mm)
            NativeMethods.WritePrivateProfileString("Align_Stage", "Turning_Radius", Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance.ToString(), strFIle);

            //  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
            NativeMethods.WritePrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg.ToString(), strFIle);

            //  얼라인 이미지 저장 여부
            NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_Use", Config.ParamConfig.AlignImageSave_Usage.ToString(), strFIle);

            //  얼라인 이미지 저장 위치 용량 부족 경고 여부 (D 드라이브)
            NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Use", Config.ParamConfig.AlignImageSaveFolder_FreeSpaceCheck_Usage.ToString(), strFIle);

            //  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
            NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", Config.ParamConfig.AlignImageSaveFolder_WarningSpace.ToString(), strFIle);

            //  프로브 카드 클램프 타입 (0:1호기, 1:2~6호기)
            NativeMethods.WritePrivateProfileString("Machine_Type", "ProbeCard_Clamp_Type", Config.ParamConfig.ProbeCard_ClampType.ToString(), strFIle);


            /// Position 저장
            /// 
            int m_nIndex_Ready = -1;
            int m_nIndex_Load = -1;
            int m_nIndex_UnLoad = -1;
            int m_nIndex_Reticle_UpperCam = -1;
            int m_nIndex_Reticle_LowerCam = -1;

            for (int i = 0; i < Config.Positions.Count; i++)
            {
                //  Ready
                if (Config.Positions[i].Name == "Ready")
                {
                    m_nIndex_Ready = i;
                }

                //  Load
                if (Config.Positions[i].Name == "Load")
                {
                    m_nIndex_Load = i;
                }

                //  UnLoad
                if (Config.Positions[i].Name == "UnLoad")
                {
                    m_nIndex_UnLoad = i;
                }

                //  Reticle Glass 를 보는 Upper Camera 위치 Index
                if (Config.Positions[i].Name == "ReticleGlass_UpperCamera")
                {
                    m_nIndex_Reticle_UpperCam = i;
                }

                //  Reticle Glass 를 보는 Lower Camera 위치 Index
                if (Config.Positions[i].Name == "ReticleGlass_LowerCamera")
                {
                    m_nIndex_Reticle_LowerCam = i;
                }

                if ((m_nIndex_Ready != -1) && (m_nIndex_Load != -1) && (m_nIndex_UnLoad != -1) && 
                    (m_nIndex_Reticle_UpperCam != -1) && (m_nIndex_Reticle_LowerCam != -1))
                {
                    break;
                }
            }

            //  Ready 좌표 저장
            if (m_nIndex_Ready != -1)
            {
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_U", Config.Positions[m_nIndex_Ready].U.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_V", Config.Positions[m_nIndex_Ready].V.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_W", Config.Positions[m_nIndex_Ready].W.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_EZ", Config.Positions[m_nIndex_Ready].EZ.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_X", Config.Positions[m_nIndex_Ready].X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_Y", Config.Positions[m_nIndex_Ready].Y.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_VZ", Config.Positions[m_nIndex_Ready].VZ.ToString(), strFIle);
            }

            //  Load 좌표 저장
            if (m_nIndex_Load != -1)
            {
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_U", Config.Positions[m_nIndex_Load].U.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_V", Config.Positions[m_nIndex_Load].V.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_W", Config.Positions[m_nIndex_Load].W.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_EZ", Config.Positions[m_nIndex_Load].EZ.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_X", Config.Positions[m_nIndex_Load].X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_Y", Config.Positions[m_nIndex_Load].Y.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_VZ", Config.Positions[m_nIndex_Load].VZ.ToString(), strFIle);
            }

            //  UnLoad 좌표 저장
            if (m_nIndex_UnLoad != -1)
            {
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_U", Config.Positions[m_nIndex_UnLoad].U.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_V", Config.Positions[m_nIndex_UnLoad].V.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_W", Config.Positions[m_nIndex_UnLoad].W.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", Config.Positions[m_nIndex_UnLoad].EZ.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_X", Config.Positions[m_nIndex_UnLoad].X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_Y", Config.Positions[m_nIndex_UnLoad].Y.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", Config.Positions[m_nIndex_UnLoad].VZ.ToString(), strFIle);
            }

            //  Upper Cam 좌표 저장
            if (m_nIndex_Reticle_UpperCam != -1)
            {
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", Config.Positions[m_nIndex_Reticle_UpperCam].U.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", Config.Positions[m_nIndex_Reticle_UpperCam].V.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", Config.Positions[m_nIndex_Reticle_UpperCam].W.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", Config.Positions[m_nIndex_Reticle_UpperCam].EZ.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", Config.Positions[m_nIndex_Reticle_UpperCam].X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", Config.Positions[m_nIndex_Reticle_UpperCam].Y.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", Config.Positions[m_nIndex_Reticle_UpperCam].VZ.ToString(), strFIle);
            }

            //  Lower Cam 좌표 저장
            if (m_nIndex_Reticle_LowerCam != -1)
            {
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", Config.Positions[m_nIndex_Reticle_LowerCam].U.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", Config.Positions[m_nIndex_Reticle_LowerCam].V.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", Config.Positions[m_nIndex_Reticle_LowerCam].W.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", Config.Positions[m_nIndex_Reticle_LowerCam].EZ.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", Config.Positions[m_nIndex_Reticle_LowerCam].X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", Config.Positions[m_nIndex_Reticle_LowerCam].Y.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", Config.Positions[m_nIndex_Reticle_LowerCam].VZ.ToString(), strFIle);
            }
        }
    }
}
