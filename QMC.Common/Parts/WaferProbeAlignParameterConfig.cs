using QMC.Common.VisionPart;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace QMC.Common.Parts
{
    [Serializable]
    public class WaferProbeAlignParameterConfig
    {
        //protected List<XyztPositionData> m_WaferProbeAlignPosition;
        //protected List<XyzztPositionData> m_WaferProbeAlignPosition;
        protected List<UvwzxyzPositionData> m_WaferProbeAlignPosition;

        //  Laser Drilling 장비에서 사용되는 위치
        public enum PositionLaser
        {
            Ready,                              //  대기 위치

            Load,                               //  제품 로딩 위치
            UnLoad,                             //  제품 언로딩 위치

            ReticleGlass_UpperCamera,           //  상부 카메라가 Reticle Glass 를 보는 위치
            ReticleGlass_LowerCamera,           //  하부 카메라가 Reticle Glass 를 보는 위치

            AlignPosition_Ver_Top,              //  세로 방향 Wafer 얼라인 위치 (TOP)
            AlignPosition_Ver_Middle,           //  세로 방향 Wafer 얼라인 위치 (MID)
            AlignPosition_Ver_Bottom,           //  세로 방향 Wafer 얼라인 위치 (BOT)

            AlignPosition_Hor_Left,             //  가로 방향 Wafer 얼라인 위치 (LEFT)
            AlignPosition_Hor_Center,           //  가로 방향 Wafer 얼라인 위치 (CENTER)
            AlignPosition_Hor_Right,            //  가로 방향 Wafer 얼라인 위치 (RIGHT)

            ProbeWafer_Packing,                 //  Probe Card 에 Wafer 를 Packing 하는 위치 (Elev. Z 값만 사용)
        }

        //public XyztPositionDataCollection WaferProbeAlignPositions { set; get; }
        //public XyzztPositionDataCollection WaferProbeAlignPositions { set; get; }
        public UvwzxyzPositionDataCollection WaferProbeAlignPositions { set; get; }

        #region Laser/ Scanner Parameter
        [Browsable(false)]
        //public List<XyztPositionData> ConfigWaferProbeAlignPositions
        //public List<XyzztPositionData> ConfigWaferProbeAlignPositions
        public List<UvwzxyzPositionData> ConfigWaferProbeAlignPositions
        {
            get { return m_WaferProbeAlignPosition; }
            set { m_WaferProbeAlignPosition = value; }
        }



        /// <summary>
        /// Align Parameter
        /// </summary>
        [Category("[01] 웨이퍼 얼라인"),
            Description("두번째 얼라인 마크 옵셋 X (mm)"),
            DisplayName("공통 - 두번째 얼라인 마크 옵셋 X (mm)")]
        public double AlignMark_2nd_Offset_X { set; get; }
        
        [Category("[01] 웨이퍼 얼라인"),
            Description("두번째 얼라인 마크 옵셋 Y (mm)"),
            DisplayName("공통 - 두번째 얼라인 마크 옵셋 Y (mm)")]
        public double AlignMark_2nd_Offset_Y { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 - 비전 허용 오차 (Theta Deg, °)"),
            DisplayName("얼라인 - 비전 허용 오차 (Theta Deg, °)")]
        public double Align_Vision_Allowable_Angle { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 - 비전 허용 오차 (XY, mm)"),
            DisplayName("얼라인 - 비전 허용 오차 (XY, mm)")]
        public double Align_Vision_Allowable_XY { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 위치 평균 계산 - 프로브 카드의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("얼라인 위치 평균 계산 - 프로브 카드 얼라인 마크 측정 회수")]
        public int ProbeCard_AlignMarkCount_forAverage { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 위치 평균 계산 - 웨이퍼의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("얼라인 위치 평균 계산 - 웨이퍼 얼라인 마크 측정 회수")]
        public int Wafer_AlignMarkCount_forAverage { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 후 오차 확인 - 비전 허용 오차 (XY)\r\n\r\n(프로브 핀 위치 기준 웨이퍼 허용 오차 범위"),
            DisplayName("얼라인 후 오차 확인 - 비전 허용 오차 (XY)")]
        public double AlignError_Vision_Allowable_XY { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("프로브 카드가 틀어진 위치로 웨이퍼를 얼라인 할 것인지 여부 선택"),
            DisplayName("얼라인 - 상부 얼라인 사용 여부 (프로브 카드 얼라인)")]
        public bool ProbrCard_Align_Use { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("메뉴얼 얼라인 시, 두 얼라인 위치 간격 X (mm)"),
            DisplayName("공통 - 메뉴얼 얼라인 시, 두 얼라인 위치 간격 X (mm)")]
        public double ManualAlign_MarkPos_Distance_X { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("메뉴얼 얼라인 시, 두 얼라인 위치 간격 Y (mm)"),
            DisplayName("공통 - 메뉴얼 얼라인 시, 두 얼라인 위치 간격 Y (mm)")]
        public double ManualAlign_MarkPos_Distance_Y { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("true : Use MyAlignConcept\r\nfalse : use JigAligner"),
            DisplayName("My Align Process 사용")]
        public bool AlignConcept_MyWaferAligner { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 후 카메라 이동.\r\n[0: 이동 안함, 1: 1번 얼라인 마크 위치, 2: 1번 Chip 위치]"),
            DisplayName("얼라인 후 카메라 이동 위치")]
        public int AfterAlign_StagePosIndex { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("웨이퍼 얼라인 시 조명 밝기값"),
            DisplayName("공통 - 얼라인 조명 밝기값 (웨이퍼, 자동 저장)")]
        public int Align_LowerVision_LightValue { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("프로브 카드 얼라인 시 조명 밝기값"),
            DisplayName("공통 - 얼라인 조명 밝기값 (프로브 카드, 자동 저장)")]
        public int Align_UpperVision_LightValue { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("웨이퍼에서 특정 위치에는 Chip 이 없다. 프로브 카드와 웨이퍼의 얼라인이 완료된 후, 해당 위치로 이동해서 육안으로 확인하는 기능을 사용할 것인지 여부."),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Empty Chip 확인 여부")]
        public bool Align_EmptyChipOffset_Usage { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("웨이퍼에서 특정 위치에는 Chip 이 없다. 프로브 카드와 웨이퍼의 얼라인이 완료된 후, 해당 위치로 이동해서 육안으로 확인하기 위한 이동 Offset 값.\r\n[Center Chip 기준, Offset X]"),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Empty Chip 확인 위치 (Center Chip 기준,   Offset X,   + : 오른쪽 위치)")]
        public double Align_EmptyChipOffset_X { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("웨이퍼에서 특정 위치에는 Chip 이 없다. 프로브 카드와 웨이퍼의 얼라인이 완료된 후, 해당 위치로 이동해서 육안으로 확인하기 위한 이동 Offset 값.\r\n[Center Chip 기준, Offset Y]"),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Empty Chip 확인 위치 (Center Chip 기준,   Offset Y,   + : 위쪽 위치)")]
        public double Align_EmptyChipOffset_Y { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 마크의 평균값 계산 후, 평균값이 기본 마크 위치값과 얼마나 차이가 있는지 비교. 너무 크면 기본 마크 위치값을 사용하도록 하기 위함.\r\n[default 0 : 0.05 mm]"),
            DisplayName("얼라인 평균 계산 - 마크 위치 평균값 신뢰 공차 (mm)")]
        public double AlignPosition_AverageCheck_Range { set; get; }



        /// <summary>
        /// Wafer - Probe Card Packing Position Offset
        /// </summary>
        [Category("[03] 웨이퍼 - 프로브 카드 패킹 옵셋"),
            Description("웨이퍼 얼라인 후 프로브 카드와 합착할 때, 웨이퍼를 XY 옵셋 거리만큼 이동시키는 기능을 사용할 것인지 여부."),
            DisplayName("웨이퍼 패킹 XY 옵셋 사용 여부")]
        public bool Wafer_ProbreCard_PackingPos_Offset_Usage { set; get; }

        [Category("[03] 웨이퍼 - 프로브 카드 패킹 옵셋"),
            Description("웨이퍼 얼라인 후 프로브 카드와 패킹할 때, 웨이퍼를 이동시키는 옵셋 거리 X (mm)\r\n\r\n[+X : (스테이지가 오른쪽으로 이동, 핀 컨택 위치가 왼쪽으로 이동함),   -X : (스테이지가 왼쪽으로 이동, 핀 컨택 위치가 오른쪽으로 이동함)]"),
            DisplayName("웨이퍼 패킹 옵셋 X (mm)")]
        public double Wafer_ProbreCard_PackingPos_Offset_X { set; get; }

        [Category("[03] 웨이퍼 - 프로브 카드 패킹 옵셋"),
            Description("웨이퍼 얼라인 후 프로브 카드와 패킹할 때, 웨이퍼를 이동시키는 옵셋 거리 Y (mm)\r\n\r\n[+Y : (스테이지가 뒤쪽으로 이동, 핀 컨택 위치가 아래쪽으로 이동함),   -Y : (스테이지가 앞쪽으로 이동, 핀 컨택 위치가 위쪽으로 이동함)]"),
            DisplayName("웨이퍼 패킹 옵셋 Y (mm)")]
        public double Wafer_ProbreCard_PackingPos_Offset_Y { set; get; }



        /// <summary>
        /// PAK Tip Contact Position Overlay
        /// </summary>
        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("웨이퍼의 게이트에 Contact 되는 프로브 카드의 2개 Pin 간격\r\n\r\n[default 0 : 0.4 mm]"),
            DisplayName("PAK 의 Pin 간격 (mm)")]
        public double PAK_GatePin_Gap { set; get; }

        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("웨이퍼의 게이트에 Contact 되는 프로브 카드의 Pin 직경. (이 크기를 이용해 Pin 위치의 사각형을 그린다.)\r\n\r\n[default 0 : 0.11mm]"),
            DisplayName("PAK 의 Pin 크기 (직경, mm)")]
        public double PAK_GatePin_Diameter { set; get; }

        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("웨이퍼의 게이트에 Contact 되는 프로브 카드의 Pin 위치 표시용 사각형 크기\r\n\r\n[default 0 : 0.07 mm]"),
            DisplayName("웨이퍼 Gate 에 그려지는 PAK Contact 위치 크기 (mm)")]
        public double Wafer_GateContactPosition_OverlaySize { set; get; }



        /// <summary>
        /// Offset and Delay
        /// </summary>
        /// 
        [Category("[05] Offset && Delay"),
            Description("Elevator Z 축이 Packing 위치로 이동할 때, Packing 위치에서 이 값만큼 뺀 거리까지 고속으로 이동하고, 이 구간은 저속으로 이동한다.\r\n씬-척이 PAK 에 Packing 되고난 후 Elevator Z 축을 내리는 거리에도 사용된다. (값이 너무 작으면 씬-척 감지로 인한 오동작이 발생할 수 있음)\r\n[default : 10]"),
            DisplayName("웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)")]
        public double Wafer_ProbeCard_PackingOffset_Distance { set; get; }

        [Category("[05] Offset && Delay"),
            Description("UnPacking 할 때, UnPacking 신호를 주면서 Elevator Z 축을 살짝 아래로 내리는 거리\r\n[default : 5]"),
            DisplayName("웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)")]
        public double Wafer_ProbeCard_UnPackingOffset_Distance { set; get; }

        [Category("[05] Offset && Delay"),
            Description("UnPacking 할 때, Packing 높이에서 얼마나 아래에서 UnPacking 작업을 진행할 것인지.\r\n[Packing 높이는 Thin-Chuck 이 Probe Card 에 눌리는 높이이기 때문에, 약간 아래에서 작업을 진행한다.]\r\n[default : 1],   (음수를 넣을 경우 위로 올라감)"),
            DisplayName("웨이퍼 && 프로브카드 UnPacking 시, Packing 높이 대비 아래로 내리는 거리 (mm)")]
        public double Wafer_ProbeCard_UnPackingStartOffset_Distance { set; get; }

        [Category("[05] Offset && Delay"),
            Description("수동 패킹 시, 패킹 위치에서 몇 mm 아래까지 엘리베이터 Z 축을 올릴 것인지.\r\n\r\n[default 0 : 30 mm]"),
            DisplayName("웨이퍼 && 프로브카드 수동 패킹 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)")]
        public double Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance { set; get; }



        /// <summary>
        /// Operation Speed
        /// </summary>
        /// 
        [Category("[06] Operation Speed"),
            Description("Elevator Z 축이 Packing 대기 위치로 이동 후, 최종 Packing 위치까지 이동할 때의 속도. 천천히 이동하는 구간\r\n[default : 10 mm/s]"),
            DisplayName("웨이퍼 && 프로브카드 Packing 및 UnPacking 시, 저속 이동 속도 (mm/s)")]
        public double Speed_Wafer_ProbeCard_Packing_LastMove { set; get; }

        [Category("[06] Operation Speed"),
            Description("UVW 스테이지의 이동 속도\r\n[default : 10 mm/s]"),
            DisplayName("UVW 스테이지의 이동 속도 (mm/s)")]
        public double Speed_Wafer_Align_Stage { set; get; }

        [Category("[06] Operation Speed"),
            Description("자동운전 시 구동 속도 (Vision XY)\r\n[default : 50 mm/s]"),
            DisplayName("자동운전 시 구동 속도 (Vision XY, mm/s)")]
        public double Speed_Operation { set; get; }

        [Category("[06] Operation Speed"),
            Description("자동운전 시 구동 속도 (Elevator Z)\r\n[default : 50 mm/s]"),
            DisplayName("자동운전 시 구동 속도 (Elevator Z, mm/s)")]
        public double Speed_Operation_Elev { set; get; }

        [Category("[06] Operation Speed"),
            Description("자동운전 시 구동 속도 대비 가감속 배율 (mm/s * n -> mm/s² 으로 사용)\r\n\r\n[default : 5]"),
            DisplayName("자동운전 시 구동 속도 대비 가감속 배율 (n)")]
        public double Speed_Operation_Mag_forAccDec { set; get; }



        /// <summary>
        /// 사용 옵션
        /// </summary>
        /// 
        [Category("[80] 사용 옵션"),
            Description("프로브 카드 커버가 닫힌 상태에서 Packing 공압을 인가했을 때, Packing 압력 신호가 뜨면 관로가 막힌것으로 본다."),
            DisplayName("웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부")]
        public bool Pak_AirLineCheck_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("프로브 카드 커버가 닫힌 상태에서 Packing 공압을 인가했을 때, Packing 압력 신호가 뜨는지 확인하는 시간. 이 시간 이내에 신호가 뜨면 관로가 막힌것으로 본다."),
            DisplayName("웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)")]
        public int Pak_AirLineCheck_Time { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 && 프로브카드 Packing 시, Packing 공압 신호를 사용할 것인지 여부."),
            DisplayName("Packing 공압 체크 사용 여부")]
        public bool Packing_VacuumSignal_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 && 프로브카드 Packing 시, Packing 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
            DisplayName("Packing 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        public int Packing_VacuumSignal_Time { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 공압 신호를 사용할 것인지 여부."),
            DisplayName("Wafer 공압 체크 사용 여부")]
        public bool Wafer_VacuumSignal_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
            DisplayName("Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        public int Wafer_VacuumSignal_Time { set; get; }

        [Category("[80] 사용 옵션"),
            Description("Thin-Chuck 감지 신호를 사용할 것인지 여부."),
            DisplayName("Thin-Chuck 감지 센서 사용 여부")]
        public bool ThinChuck_DetectSignal_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("Thin-Chuck 공압 신호를 사용할 것인지 여부."),
            DisplayName("Thin-Chuck 공압 체크 사용 여부")]
        public bool ThinChuck_VacuumSignal_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("Thin-Chuck 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
            DisplayName("Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        public int ThinChuck_VacuumSignal_Time { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 얼라인 동작 중, 비전 검사 없이 모션 구동만 할 것인지 여부. (false : 사용 안함)"),
            DisplayName("Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)")]
        public bool Wafer_Align_Cam_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 얼라인 완료 후, 바로 Packing 작업을 진행할 것인지 여부. (false : 사용 안함)"),
            DisplayName("Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)")]
        public bool Packing_AutoStart_After_Wafer_Align_Usage { set; get; }

        [Category("[80] 사용 옵션"),
            Description("웨이퍼 얼라인 완료 후, Top - Mid - Bottom 위치에서 프로브 핀 위치 대비 웨이퍼의 위치 오차 검증을 진행할 것인지 여부. (false : 사용 안함)"),
            DisplayName("Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함)")]
        public bool Wafer_Align_ErrorCheck_After_Wafer_Align_Usage { set; get; }



        /// <summary>
        /// 안정화 시간
        /// </summary>
        /// 
        [Category("[81] 안정화 시간"),
            Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후 대기 시간 (ms)\r\n(Packing 공압 신호를 인가하고 Thin-Chuck 이 압력에 의해 프로브 카드에 밀착되는 시간 동안 기다려야 한다. 약 3~5초 소요)"),
            DisplayName("Packing 공압 신호 On 후 대기 시간 (ms)")]
        public int StableTime_after_PackingSignal_On { set; get; }

        [Category("[81] 안정화 시간"),
            Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후, Thin-Chuck 이 프로브 카드에 밀착될 때(딸려 올라갈 때) Thin-Chuck 공압을 해제해야 하는데, 얼마만큼 시간이 지난 후에 Thin-Chuck 공압을 해제할 것인지 (ms)"),
            DisplayName("Packing 공압 신호 On 후 Thin-Chuck 과 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)")]
        public int StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off { set; get; }

        [Category("[81] 안정화 시간"),
            Description("Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)"),
            DisplayName("Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)")]
        public int StableTime_after_WaferVacuumSignal_Off { set; get; }

        [Category("[81] 안정화 시간"),
            Description("Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)"),
            DisplayName("Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)")]
        public int StableTime_before_PackingSignal_On { set; get; }

        [Category("[81] 안정화 시간"),
            Description("Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)"),
            DisplayName("Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)")]
        public int StableTime_after_ThinChuckVacuumSignal_Off { set; get; }


        [Category("[81] 안정화 시간"),
            Description("웨이퍼 얼라인 시, 마크 검사 위치로 이동한 후에 진동 억제를 위한 안정화 시간 (ms)"),
            DisplayName("Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)")]
        public int WaferAlign_Move_StableTime { set; get; }



        /// <summary>
        /// 2D Mapping Options
        /// </summary>
        /// 
        [Category("[98] 2D Mapping Option"),
            Description("for AJIN - 2D 맵 파일 경로"),
            DisplayName("for AJIN - 2D 맵 파일 경로")]
        public string MapFile_Path { set; get; }

        [Category("[98] 2D Mapping Option"),
            Description("for AJIN - 2D 맵 파일 적용 여부 (프로그램 로딩 시 맵 적용할 것인지)"),
            DisplayName("for AJIN - 2D 맵 파일 적용 여부")]
        public bool MapFileApply_WhenPgmStart { set; get; }



        /// <summary>
        /// 장비 공통 파라미터
        /// </summary>
        /// 
        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 시, 이 높이 이상으로는 Elev. Z 축을 올리지 못하도록 한다."),
            DisplayName("구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치")]
        public double DriveLimit_ElevZ_when_WaferAlign { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Vision Y 축이 장비 앞쪽으로 이동할 때, Elevator Z 축과 충돌하지 않는 최대 위치"),
            DisplayName("구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치")]
        public double DriveLimit_VisionY_NotConflictWithElevZ { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 패킹 시, UVW 스테이지와 프로브 카드의 충돌을 방지하기 위한 제한 위치 (U 축, 마이너스 방향)\r\n\r\n[0 : 사용 안함]"),
            DisplayName("구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)")]
        public double AlignLimit_UVW_U_Minus { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 패킹 시, UVW 스테이지와 프로브 카드의 충돌을 방지하기 위한 제한 위치 (U 축, 플러스 방향)\r\n\r\n[0 : 사용 안함]"),
            DisplayName("구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)")]
        public double AlignLimit_UVW_U_Plus { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 패킹 시, UVW 스테이지와 프로브 카드의 충돌을 방지하기 위한 제한 위치 (V 축, 마이너스 방향)\r\n\r\n[0 : 사용 안함]"),
            DisplayName("구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)")]
        public double AlignLimit_UVW_V_Minus { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 패킹 시, UVW 스테이지와 프로브 카드의 충돌을 방지하기 위한 제한 위치 (V 축, 플러스 방향)\r\n\r\n[0 : 사용 안함]"),
            DisplayName("구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)")]
        public double AlignLimit_UVW_V_Plus { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 패킹 시, UVW 스테이지와 프로브 카드의 충돌을 방지하기 위한 제한 위치 (W 축, 마이너스 방향)\r\n\r\n[0 : 사용 안함]"),
            DisplayName("구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)")]
        public double AlignLimit_UVW_W_Minus { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 패킹 시, UVW 스테이지와 프로브 카드의 충돌을 방지하기 위한 제한 위치 (W 축, 플러스 방향)\r\n\r\n[0 : 사용 안함]"),
            DisplayName("구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)")]
        public double AlignLimit_UVW_W_Plus { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Use this Scale parameter"),
            DisplayName("비전 스케일 - Manual Scale Usage")]
        public bool ManualScale_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Lower Vision Pixel Resolution X (mm)"),
            DisplayName("비전 스케일 - Lower Vision Scale X (mm)")]
        public double LowerVision_Scale_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Lower Vision Pixel Resolution Y (mm)"),
            DisplayName("비전 스케일 - Lower Vision Scale Y (mm)")]
        public double LowerVision_Scale_Y { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Lower Vision Scale Invert X"),
            DisplayName("비전 스케일 - Lower Vision Scale Invert X")]
        public bool LowerVision_ScaleInvert_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Lower Vision Scale Invert Y"),
            DisplayName("비전 스케일 - Lower Vision Scale Invert Y")]
        public bool LowerVision_ScaleInvert_Y { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Upper Vision Pixel Resolution X (mm)"),
            DisplayName("비전 스케일 - Upper Vision Scale X (mm)")]
        public double UpperVision_Scale_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Upper Vision Pixel Resolution Y (mm)"),
            DisplayName("비전 스케일 - Upper Vision Scale Y (mm)")]
        public double UpperVision_Scale_Y { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Upper Vision Scale Invert X"),
            DisplayName("비전 스케일 - Upper Vision Scale Invert X")]
        public bool UpperVision_ScaleInvert_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Upper Vision Scale Invert Y"),
            DisplayName("비전 스케일 - Upper Vision Scale Invert Y")]
        public bool UpperVision_ScaleInvert_Y { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 얼라인 시 조명 밝기값 (프로브 카드 얼라인 카메라)"),
            DisplayName("레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)")]
        public int ReticleAlign_UpperVision_LightValue { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 얼라인 시 조명 밝기값 (웨이퍼 얼라인 카메라)"),
            DisplayName("레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)")]
        public int ReticleAlign_LowerVision_LightValue { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시 Theta 축 회전 속도 (mm/s)\r\n\r\n[default 0: 50]"),
            DisplayName("얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)")]
        public double Align_Theta_Velocity { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시 Theta 축 회전 가속도 (mm/s²)\r\n\r\n[default 0: 1000]"),
            DisplayName("얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)")]
        public double Align_Theta_Accel { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시 Theta 축 회전 감속도 (mm/s²)\r\n\r\n[default 0: 1000]"),
            DisplayName("얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)")]
        public double Align_Theta_Decel { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Theta 축이 회전하기 위한 중심 축에서 각 UVW 축 까지의 거리(mm)\r\n[이 거리 값에 따라 1˚회전을 위한 이동량이 달라진다]\r\n[default : 70 mm]"),
            DisplayName("얼라인 스테이지 - 회전 반경 (mm)")]
        public double Align_Theta_From_RotCenter_To_UVW_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Theta 축 1˚ 회전하기 위해 필요한 UVW Stage 이동량(mm)\r\n[default : 1.221668451 mm        (70mm / 1˚)]\r\n[주의 : Center 기준으로 회전하게 되면 양쪽 마크가 동시에 회전하므로 실제로는 회전량이 약 2배가 됨.]"),
            DisplayName("얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)")]
        public double Align_Theta_Movement_MM_Per_1Deg { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 재시도 회수"),
            DisplayName("얼라인 - 얼라인 재시도 회수")]
        public int Align_Retries { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 각도 Invert"),
            DisplayName("얼라인 - 얼라인 각도 Invert")]
        public bool Align_AngleInvert { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Use \"Atan\" function when calculating alignment angles\r\n(default uses \"Atan2\")"),
            DisplayName("얼라인 - 얼라인 각도 계산 시 Atan 함수 사용")]
        public bool Align_ThetaCalcFunction_Atan { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("저장 위치 : \"D:\\CWA-150SA_AlignImage\"\r\n웨이퍼 얼라인을 완료하고, 얼라인 오차 검증할 때 저장되는 이미지"),
            DisplayName("얼라인 이미지 저장 여부")]
        public bool AlignImageSave_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시작할 때, D 드라이브 용량을 확인하여 경고메세지를 보일 것인지 여부.\r\n\r\n[얼라인 이미지를 저장하기 위해 여유 공간이 필요함]"),
            DisplayName("얼라인 이미지 저장 위치 용량 부족 경고 여부 (D 드라이브)")]
        public bool AlignImageSaveFolder_FreeSpaceCheck_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("D 드라이브 잔여 용량이 기준값 이하일 경우 경고메세지 출력 (매 얼라인 시작 시 체크)\r\n\r\n[default 0 : 10 GB]"),
            DisplayName("얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)")]
        public double AlignImageSaveFolder_WarningSpace { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Type-A : 트레이에 프로브 카드를 넣고 서랍처럼 로딩(언로딩), 업다운 실린더로 고정\r\nType-B : 프로브 카드 정위치에 직접 투입, 좌우 && 업다운 실린더로 고정, 언패킹 실린더 있음\r\n[Type-A: #1 호기,  Type-B: #2 ~ #6 호기]"),
            DisplayName("프로브 카드 클램프 타입 [0: Type-A,  1: Type-B]")]
        public int ProbeCard_ClampType { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Type-B 의 클램프 실린더에 업다운 확인 센서가 없으므로, 실린더를 동작시킨 후 정해진 대기시간만큼 지난 후 다음 동작을 진행하도록 한다.\r\n[default 0: 1000 ms]"),
            DisplayName("프로브 카드 클램프 \"Type-B\" 의 경우, 업다운 실린더 동작 대기 시간 (ms)")]
        public int ProbeCard_ClampTypeB_CylUpDown_StableTime { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레시피를 변경할 때마다 상하부 카메라의 광축이 일치하는 지 확인 후 작업 진행하도록 한다.\r\n[상부 카메라 레티클 센터 확인 -> 하부 카메라 레티클 센터 확인]"),
            DisplayName("레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능")]
        public bool ReticleGlass_CenterCheck_forAlign { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 과 웨이퍼 Gate 의 Center 를 일치시키기 위한 Offset X 값,     [# 조건 : [03] 웨이퍼 패킹 옵셋이 0 일 경우 #]\r\n\r\n[+X : (스테이지가 오른쪽으로 이동, 핀 컨택 위치가 왼쪽으로 이동함),   -X : (스테이지가 왼쪽으로 이동, 핀 컨택 위치가 오른쪽으로 이동함)]"),
            DisplayName("PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X")]
        public double PAK_WaferGate_Centering_Offset_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 과 웨이퍼 Gate 의 Center 를 일치시키기 위한 Offset Y 값,     [# 조건 : [03] 웨이퍼 패킹 옵셋이 0 일 경우 #]\r\n\r\n[+Y : (스테이지가 뒤쪽으로 이동, 핀 컨택 위치가 아래쪽으로 이동함),   -Y : (스테이지가 앞쪽으로 이동, 핀 컨택 위치가 위쪽으로 이동함)]"),
            DisplayName("PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y")]
        public double PAK_WaferGate_Centering_Offset_Y { set; get; }


        #endregion



        #region Constructor
        public WaferProbeAlignParameterConfig()
        {
            Init();

            //Scanner_KFactor = 1000.0;
        }
        //public List<ZPositionData> Positions { set; get; }
        #endregion

        protected void Init()
        {
            if (WaferProbeAlignPositions == null)
                WaferProbeAlignPositions = new UvwzxyzPositionDataCollection();

            m_WaferProbeAlignPosition = new List<UvwzxyzPositionData>();
            m_WaferProbeAlignPosition.Clear();

            WaferProbeAlignPositions.Clear();



            foreach (PositionLaser key in Enum.GetValues(typeof(PositionLaser)))
            {
                //XyztPositionData positionBase = new XyztPositionData();
                //XyzztPositionData positionBase = new XyzztPositionData();
                UvwzxyzPositionData positionBase = new UvwzxyzPositionData();

                positionBase.Name = key.ToString();
                m_WaferProbeAlignPosition.Add(positionBase);
                WaferProbeAlignPositions.Add(positionBase);

                //XyztPositionData positionTarget = new XyztPositionData();
                //XyzztPositionData positionTarget = new XyzztPositionData();
                UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();

                //positionTarget.Name = key.ToString();                     //  Offset 부분까지 Name 이 보이면 Position 별 구분이 잘 안됨.
                positionTarget.Type = TargetType.Offset;
                m_WaferProbeAlignPosition.Add(positionTarget);
                WaferProbeAlignPositions.Add(positionTarget);
            }
        }

        public List<string> GetLaserPositionList()
        {
            List<string> ret = new List<string>();
            //foreach (XyztPositionData position in m_WaferProbeAlignPosition)
            //foreach (XyzztPositionData position in m_WaferProbeAlignPosition)
            foreach (UvwzxyzPositionData position in m_WaferProbeAlignPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            //foreach (XyztPositionData pos in m_WaferProbeAlignPosition)
            //foreach (XyzztPositionData pos in m_WaferProbeAlignPosition)
            foreach (UvwzxyzPositionData pos in m_WaferProbeAlignPosition)
            {
                //  U
                if (i >= parameters.Count)
                    return i;
                pos.U = parameters[i++].DoubleValue;

                //  V
                if (i >= parameters.Count)
                    return i;
                pos.V = parameters[i++].DoubleValue;

                //  W
                if (i >= parameters.Count)
                    return i;
                pos.W = parameters[i++].DoubleValue;

                //  Elevator Z
                if (i >= parameters.Count)
                    return i;
                pos.EZ = parameters[i++].DoubleValue;

                //  X
                if (i >= parameters.Count)
                    return i;
                pos.X = parameters[i++].DoubleValue;

                //  Y
                if (i >= parameters.Count)
                    return i;
                pos.Y = parameters[i++].DoubleValue;

                //  VisionZ
                if (i >= parameters.Count)
                    return i;
                pos.VZ = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            //foreach (XyztPositionData pos in m_WaferProbeAlignPosition)
            //foreach (XyzztPositionData pos in m_WaferProbeAlignPosition)
            foreach (UvwzxyzPositionData pos in m_WaferProbeAlignPosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.U;
                    param.Tag = WaferProbeAlignParameter.MotionKey.U.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.V;
                    param.Tag = WaferProbeAlignParameter.MotionKey.V.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.W;
                    param.Tag = WaferProbeAlignParameter.MotionKey.W.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.EZ;
                    param.Tag = WaferProbeAlignParameter.MotionKey.EZ.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.X;
                    param.Tag = WaferProbeAlignParameter.MotionKey.X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Y;
                    param.Tag = WaferProbeAlignParameter.MotionKey.Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.VZ;
                    param.Tag = WaferProbeAlignParameter.MotionKey.VZ.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
