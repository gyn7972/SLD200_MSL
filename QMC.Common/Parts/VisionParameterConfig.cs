using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Windows.Forms;

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

namespace QMC.Common.Parts
{
    [Serializable]
    public class VisionParameterConfig
    {
        protected List<XyzyPositionData> m_VisionPosition;

        //  Laser Drilling 장비에서 사용되는 위치
        public enum PositionLaser
        {
            Vision_Image_Focus_AxisZ,           //  비전 카메라의 초점 위치
            Laser_Focus_AxisZ,                  //  레이저의 초점 위치
            Safety_AxisZ,                       //  안전 위치
        }

        public XyzyPositionDataCollection VisionPositions { set; get; }

        #region Laser/ Scanner Parameter
        [Browsable(false)]
        public List<XyzyPositionData> ConfigVisionPositions
        {
            get { return m_VisionPosition; }
            set { m_VisionPosition = value; }
        }



        /// <summary>
        /// Align Parameter
        /// </summary>
        /// 
        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("두번째 얼라인 마크 옵셋 X (mm)"),
        //    DisplayName("공통 - 두번째 얼라인 마크 옵셋 X (mm)")]
        //public double AlignMark_2nd_Offset_X { set; get; }
        
        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("두번째 얼라인 마크 옵셋 Y (mm)"),
        //    DisplayName("공통 - 두번째 얼라인 마크 옵셋 Y (mm)")]
        //public double AlignMark_2nd_Offset_Y { set; get; }

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
            Description("프로브 카드 얼라인 반복 회수 (TOP - BOTTOM 간 얼라인 데이터 계산 과정을 반복하여 평균값 계산)\r\n\r\n[default 0: 1회]"),
            DisplayName("얼라인 - 상부 얼라인 반복 회수 (프로브 카드 얼라인)")]
        public int ProbrCard_Align_Count_Total { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("세로 방향 얼라인(TOP ~ BOTTOM) 을 기본으로 하고, 가로 방향 얼라인을 추가로 진행하여 평균 각도를 계산한다.\r\n\r\n[True : 세로방향 얼라인 결과값과 가로방향 얼라인 결과값의 평균, False : 세로방향 얼라인만 진행]"),
            DisplayName("얼라인 - 상부 얼라인 시 가로 방향 얼라인을 추가로 진행할 것인지 여부 (프로브 카드 얼라인)")]
        public bool ProbrCard_HorAlign_Use { set; get; }

        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("메뉴얼 얼라인 시, 두 얼라인 위치 간격 X (mm)"),
        //    DisplayName("공통 - 메뉴얼 얼라인 시, 두 얼라인 위치 간격 X (mm)")]
        //public double ManualAlign_MarkPos_Distance_X { set; get; }

        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("메뉴얼 얼라인 시, 두 얼라인 위치 간격 Y (mm)"),
        //    DisplayName("공통 - 메뉴얼 얼라인 시, 두 얼라인 위치 간격 Y (mm)")]
        //public double ManualAlign_MarkPos_Distance_Y { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("true : Use MyAlignConcept\r\nfalse : use JigAligner"),
            DisplayName("My Align Process 사용")]
        public bool AlignConcept_MyWaferAligner { set; get; }

        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("얼라인 후 카메라 이동.\r\n[0: 이동 안함, 1: 1번 얼라인 마크 위치, 2: 1번 Chip 위치]"),
        //    DisplayName("얼라인 후 카메라 이동 위치")]
        //public int AfterAlign_StagePosIndex { set; get; }

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
            DisplayName("얼라인 정도 확인 - 얼라인 후 Tag - Chip 위치 확인 여부")]
        public bool Align_EmptyChipOffset_Usage { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("웨이퍼에서 특정 위치에는 Chip 이 없다. 프로브 카드와 웨이퍼의 얼라인이 완료된 후, 해당 위치로 이동해서 육안으로 확인하기 위한 이동 Offset 값.\r\n[Center Chip 기준, Offset X]"),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Tag - Chip 확인 위치 (Center Chip 기준,   Offset X,   + : 오른쪽 위치)")]
        public double Align_EmptyChipOffset_X { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("웨이퍼에서 특정 위치에는 Chip 이 없다. 프로브 카드와 웨이퍼의 얼라인이 완료된 후, 해당 위치로 이동해서 육안으로 확인하기 위한 이동 Offset 값.\r\n[Center Chip 기준, Offset Y]"),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Tag - Chip 확인 위치 (Center Chip 기준,   Offset Y,   + : 위쪽 위치)")]
        public double Align_EmptyChipOffset_Y { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("특정 웨이퍼에서는 Base - Tip 외에 Source - Tip 위치도 확인해야 한다. 프로브 카드와 웨이퍼의 얼라인이 완료된 후, 해당 위치로 이동해서 육안으로 확인하는 기능을 사용할 것인지 여부."),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Source - Tip 위치 확인 여부")]
        public bool Align_SourceTipOffset_Usage { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("특정 웨이퍼에서는 Base - Tip 외에 Source - Tip 위치도 확인해야 한다. 해당 위치로 이동해서 육안으로 확인하기 위한 이동 Offset 값.  [Center-Tip (Left) 기준, Offset X]\r\n[Source-Tip 의 배치가 가로 방향일 경우 왼쪽 Tip 이 기준 Tip 이 된다]\r\n[Source-Tip 의 배치가 세로 방향일 경우 위쪽 Tip 이 기준이 된다] "),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Source - Tip 확인 위치 (Center-Tip (Left) 기준,   Offset X,   + : 오른쪽 위치)")]
        public double Align_SourceTipOffset_X { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("특정 웨이퍼에서는 Base - Tip 외에 Source - Tip 위치도 확인해야 한다. 해당 위치로 이동해서 육안으로 확인하기 위한 이동 Offset 값.  [Center-Tip (Left) 기준, Offset Y]\r\n[Source-Tip 의 배치가 가로 방향일 경우 왼쪽 Tip 이 기준 Tip 이 된다]\r\n[Source-Tip 의 배치가 세로 방향일 경우 위쪽 Tip 이 기준이 된다] "),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Source - Tip 확인 위치 (Center-Tip (Left) 기준,   Offset Y,   + : 위쪽 위치)")]
        public double Align_SourceTipOffset_Y { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("특정 웨이퍼에서는 Base - Tip 외에 Source - Tip 위치도 확인해야 한다. Source - Tip 의 배치 방향이 가로 방향인지 세로 방향인지 설정한다."),
            DisplayName("얼라인 정도 확인 - 얼라인 후 Source - Tip 확인 시, Tip 의 배치 방향 (True : 가로 방향, False : 세로 방향)")]
        public bool Align_SourceTipOffset_Dir_Hor { set; get; }

        [Category("[01] 웨이퍼 얼라인"),
            Description("얼라인 마크의 평균값 계산 후, 평균값이 기본 마크 위치값과 얼마나 차이가 있는지 비교. 너무 크면 기본 마크 위치값을 사용하도록 하기 위함.\r\n[default 0 : 0.05 mm]"),
            DisplayName("얼라인 평균 계산 - 마크 위치 평균값 신뢰 공차 (mm)")]
        public double AlignPosition_AverageCheck_Range { set; get; }



        /// <summary>
        /// 얼라인 정확성 검증
        /// </summary>
        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 좌측 상단 위치 검사"),
            DisplayName("추가 검증 위치 사용[1, ↖] - 좌측 상단")]
        public bool AlignErrorCheckPos_LT_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 상단 위치 검사"),
            DisplayName("추가 검증 위치 사용[2,  ↑] - 상단")]
        public bool AlignErrorCheckPos_TOP_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 우측 상단 위치 검사"),
            DisplayName("추가 검증 위치 사용[3, ↗] - 우측 상단")]
        public bool AlignErrorCheckPos_RT_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 우측 위치 검사"),
            DisplayName("추가 검증 위치 사용[4, →] - 오른쪽")]
        public bool AlignErrorCheckPos_RIGHT_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 우측 하단 위치 검사"),
            DisplayName("추가 검증 위치 사용[5, ↘] - 우측 하단")]
        public bool AlignErrorCheckPos_RB_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 하단 위치 검사"),
            DisplayName("추가 검증 위치 사용[6,  ↓] - 하단")]
        public bool AlignErrorCheckPos_BOTTOM_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 좌측 하단 위치 검사"),
            DisplayName("추가 검증 위치 사용[7, ↙] - 좌측 하단")]
        public bool AlignErrorCheckPos_LB_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 여부 - Tip 간격 만큼 떨어진 좌측 위치 검사"),
            DisplayName("추가 검증 위치 사용[8, ←] - 왼쪽")]
        public bool AlignErrorCheckPos_LEFT_Usage { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 시 Tip 간격 - 가로 방향 (mm)"),
            DisplayName("추가 검증 위치 사용 시 Tip 간격 - 가로 방향 (mm)")]
        public double AlignErrorCheck_TipGap_Hor { set; get; }

        [Category("[02] 얼라인 정확성 검증"),
            Description("추가 검증 위치 사용 시 Tip 간격 - 세로 방향 (mm)"),
            DisplayName("추가 검증 위치 사용 시 Tip 간격 - 세로 방향 (mm)")]
        public double AlignErrorCheck_TipGap_Ver { set; get; }



        /// <summary>
        /// Wafer - Probe Card Packing Position Offset
        /// </summary>
        [Category("[03] 웨이퍼 - 프로브 카드 패킹 옵셋"),
            Description("웨이퍼 얼라인 후 프로브 카드와 합착할 때, 웨이퍼를 XY 옵셋 거리만큼 이동시키는 기능을 사용할 것인지 여부.\r\n\r\n[###  무조건 사용  ###]"),
            DisplayName("웨이퍼 패킹 XY 옵셋 사용 여부                                                                                                       --> ### 무조건 사용하도록 변경 ###")]
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
            Description("웨이퍼의 게이트에 Contact 되는 프로브 카드의 2개 Tip 간격\r\n\r\n[default 0 : 0.4 mm]"),
            DisplayName("PAK 의 Tip 간격 (mm)")]
        public double PAK_GatePin_Gap { set; get; }

        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("웨이퍼의 Source 위치에 Contact 되는 프로브 카드의 2개 Tip 간격\r\n\r\n[default 0 : 0.4 mm]"),
            DisplayName("PAK 의 Source - Tip 간격 (mm)")]
        public double PAK_SourcePin_Gap { set; get; }

        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("웨이퍼의 게이트에 Contact 되는 프로브 카드의 Tip 직경. (이 크기를 이용해 Tip 위치의 사각형을 그린다.)\r\n\r\n[default 0 : 0.11 mm]"),
            DisplayName("PAK 의 Tip 크기 (직경, mm)")]
        public double PAK_GatePin_Diameter { set; get; }

        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("웨이퍼의 게이트에 Contact 되는 프로브 카드의 Tip 위치 표시용 사각형 크기\r\n\r\n[default 0 : 0.07 mm]"),
            DisplayName("웨이퍼 Gate 에 그려지는 PAK Contact 위치 크기 (mm)")]
        public double Wafer_GateContactPosition_OverlaySize { set; get; }

        [Category("[04] 웨이퍼 - Tip Contact 위치 표시"),
            Description("PAK 과 웨이퍼 이미지의 확대 배율. \r\n\r\n[default 0 : 1배]"),
            DisplayName("PAK 과 웨이퍼 이미지의 확대 배율 (n >= 1.0)")]
        public double PAK_Gate_Image_MagRate { set; get; }



        /// <summary>
        /// Offset and Delay
        /// </summary>
        /// 
        //[Category("[05] Offset && Delay"),
        //    Description("Elevator Z 축이 Packing 위치로 이동할 때, Packing 위치에서 이 값만큼 뺀 거리까지 고속으로 이동하고, 이 구간은 저속으로 이동한다.\r\n\r\n[default : 10 mm]"),
        //    DisplayName("웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)")]
        //public double Wafer_ProbeCard_PackingOffset_Distance { set; get; }

        //[Category("[05] Offset && Delay"),
        //    Description("Elevator Z 축이 Unpacking 을 위헤 Packing 위치로 이동할 때, Packing 위치에서 이 값만큼 뺀 거리까지 고속으로 이동하고, 이 구간은 저속으로 이동한다.\r\n\r\n[default : 10 mm]"),
        //    DisplayName("웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)")]
        //public double Wafer_ProbeCard_UnpackingOffset_Distance { set; get; }

        //[Category("[05] Offset && Delay"),
        //    Description("UnPacking 할 때, UnPacking 신호를 주면서 Elevator Z 축을 약간 아래로 내리는 거리\r\n[Packing 완료 후, Leak 로 인해 씬-척이 분리될 경우 안전사고가 발생할 수 있으므로 너무 많이 내리지 않도록 한다.]\r\n[default : 5 mm]     [권장 : 30mm 이내]"),
        //    DisplayName("웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)")]
        //public double Wafer_ProbeCard_UnPackingOffset_Distance { set; get; }

        //[Category("[05] Offset && Delay"),
        //    Description("UnPacking 할 때, Packing 높이에서 얼마나 아래에서 UnPacking 작업을 진행할 것인지.\r\n[Packing 후 씬-척의 높이는 Packing 전보다 높기 때문에, Packing 높이보다 더 올라가야 한다.]\r\n[default : 1 mm],   (입력 값이 0 보다 작을 경우 위로 올라감)"),
        //    DisplayName("웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)")]
        //public double Wafer_ProbeCard_UnPackingStartOffset_Distance { set; get; }

        //[Category("[05] Offset && Delay"),
        //    Description("수동 패킹 시, 패킹 위치에서 몇 mm 아래까지 엘리베이터 Z 축을 올릴 것인지.\r\n\r\n[default 0 : 30 mm]"),
        //    DisplayName("웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)")]
        //public double Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance { set; get; }



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
            Description("자동운전 시 구동 속도 대비 가감속 배율 (mm/s * n -> mm/s² 으로 사용)\r\n\r\n[default : 5 배]"),
            DisplayName("자동운전 시 구동 속도 대비 가감속 배율 (n)")]
        public double Speed_Operation_Mag_forAccDec { set; get; }



        /// <summary>
        /// 사용 옵션
        /// </summary>
        /// 
        //[Category("[80] 사용 옵션"),
        //    Description("프로브 카드 커버가 닫힌 상태에서 Packing 공압을 인가했을 때, Packing 압력 신호가 뜨면 관로가 막힌것으로 본다."),
        //    DisplayName("웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부")]
        //public bool Pak_AirLineCheck_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("프로브 카드 커버가 닫힌 상태에서 Packing 공압을 인가했을 때, Packing 압력 신호가 뜨는지 확인하는 시간. 이 시간 이내에 신호가 뜨면 관로가 막힌것으로 본다."),
        //    DisplayName("웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)")]
        //public int Pak_AirLineCheck_Time { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 && 프로브카드 Packing 시, Packing 공압 신호를 사용할 것인지 여부."),
        //    DisplayName("Packing 공압 신호 사용 여부")]
        //public bool Packing_VacuumSignal_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 && 프로브카드 Packing 시 Packing 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행\r\n\r\n(Packing 공압 신호를 인가하고 Thin-Chuck 이 압력에 의해 프로브 카드에 밀착되는 시간 동안 기다려야 한다. 약 3~5초 소요)"),
        //    DisplayName("Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)")]
        //public int Packing_VacuumSignal_Time { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 && 프로브카드 Packing 시 Packing 공압 신호를 사용할 경우, Packing 공압 신호가 들어온 후 이 시간만큼 대기 후 다음 동작 진행\r\n\r\n[PAK 에 씬-척이 충분히 밀착되도록 기다리는 시간]"),
        //    DisplayName("Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)")]
        //public int Packing_VacuumSignal_AfterTime { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 공압 신호를 사용할 것인지 여부."),
        //    DisplayName("Wafer 공압 체크 사용 여부")]
        //public bool Wafer_VacuumSignal_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
        //    DisplayName("Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        //public int Wafer_VacuumSignal_Time { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("Thin-Chuck 감지 신호를 사용할 것인지 여부."),
        //    DisplayName("Thin-Chuck 감지 센서 사용 여부")]
        //public bool ThinChuck_DetectSignal_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("Thin-Chuck 공압 신호를 사용할 것인지 여부."),
        //    DisplayName("Thin-Chuck 공압 체크 사용 여부")]
        //public bool ThinChuck_VacuumSignal_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("Thin-Chuck 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
        //    DisplayName("Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        //public int ThinChuck_VacuumSignal_Time { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 얼라인 동작 중, 비전 검사 없이 모션 구동만 할 것인지 여부. (false : 사용 안함)"),
        //    DisplayName("Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)")]
        //public bool Wafer_Align_Cam_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 얼라인 완료 후, 바로 Packing 작업을 진행할 것인지 여부. (false : 사용 안함)"),
        //    DisplayName("Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)")]
        //public bool Packing_AutoStart_After_Wafer_Align_Usage { set; get; }

        //[Category("[80] 사용 옵션"),
        //    Description("웨이퍼 얼라인 완료 후, Top - Mid - Bottom 위치에서 프로브 핀 위치 대비 웨이퍼의 위치 오차 검증을 진행할 것인지 여부. (false : 사용 안함)\r\n\r\n[###  무조건 사용  ###]"),
        //    DisplayName("Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함)                                                --> ### 무조건 사용하도록 변경 ###")]
        //public bool Wafer_Align_ErrorCheck_After_Wafer_Align_Usage { set; get; }



        /// <summary>
        /// 안정화 시간
        /// </summary>
        /// 
        //[Category("[81] 안정화 시간"),
        //    //Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후 대기 시간 (ms)\r\n(Packing 공압 신호를 인가하고 Thin-Chuck 이 압력에 의해 프로브 카드에 밀착되는 시간 동안 기다려야 한다. 약 3~5초 소요)"),
        //    Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후 대기 시간 (ms)\r\n(Packing 공압 신호를 사용하거나 사용하지 않거나, 이 시간만큼 기다린 후 진행된다.)"),
        //    DisplayName("Packing 공압 신호 On 후 대기 시간 (ms)")]
        //public int StableTime_after_PackingSignal_On { set; get; }

        //[Category("[81] 안정화 시간"),
        //    Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후, Thin-Chuck 이 프로브 카드에 밀착될 때(딸려 올라갈 때) Thin-Chuck 공압을 해제해야 하는데, 얼마만큼 시간이 지난 후에 Thin-Chuck 공압을 해제할 것인지 (ms)"),
        //    DisplayName("Packing 공압 신호 On 후 Thin-Chuck 과 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)")]
        //public int StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off { set; get; }

        //[Category("[81] 안정화 시간"),
        //    Description("Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)"),
        //    DisplayName("Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)")]
        //public int StableTime_after_WaferVacuumSignal_Off { set; get; }

        //[Category("[81] 안정화 시간"),
        //    Description("Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)"),
        //    DisplayName("Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)")]
        //public int StableTime_before_PackingSignal_On { set; get; }

        //[Category("[81] 안정화 시간"),
        //    Description("Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)"),
        //    DisplayName("Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)")]
        //public int StableTime_after_ThinChuckVacuumSignal_Off { set; get; }


        //[Category("[81] 안정화 시간"),
        //    Description("웨이퍼 얼라인 시, 마크 검사 위치로 이동한 후에 진동 억제를 위한 안정화 시간 (ms)"),
        //    DisplayName("Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)")]
        //public int WaferAlign_Move_StableTime { set; get; }



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
            Description("얼라인 시 Theta 축 회전 속도 (mm/s)\r\n\r\n[default 0: 50 mm/s]"),
            DisplayName("얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)")]
        public double Align_Theta_Velocity { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시 Theta 축 회전 가속도 (mm/s²)\r\n\r\n[default 0: 1000 mm/s²]"),
            DisplayName("얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)")]
        public double Align_Theta_Accel { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시 Theta 축 회전 감속도 (mm/s²)\r\n\r\n[default 0: 1000 mm/s²]"),
            DisplayName("얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)")]
        public double Align_Theta_Decel { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Theta 축이 회전하기 위한 중심 축에서 각 UVW 축 까지의 거리(mm)\r\n[이 거리 값에 따라 1˚회전을 위한 이동량이 달라진다]\r\n[default 0: 70 mm]"),
            DisplayName("얼라인 스테이지 - 회전 반경 (mm)")]
        public double Align_Theta_From_RotCenter_To_UVW_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Theta 축 1˚ 회전하기 위해 필요한 UVW Stage 이동량(mm)\r\n[default 0: 1.221668451 mm        (70mm / 1˚)]\r\n[주의 : Center 기준으로 회전하게 되면 양쪽 마크가 동시에 회전하므로 실제로는 회전량이 약 2배가 됨.]"),
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
            Description("D 드라이브 잔여 용량이 기준값 이하일 경우 경고메세지 출력 (매 얼라인 시작 시 체크)\r\n[C 드라이브는 10 GB 이하이면 경고 메세지 출력]\r\n[default 0 : 10 GB]"),
            DisplayName("얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)")]
        public double AlignImageSaveFolder_WarningSpace { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("클램프 실린더에 업-다운 확인 센서가 없으므로, 실린더를 동작시킨 후 정해진 대기시간만큼 지난 후 다음 동작을 진행하도록 한다.\r\n[default 0: 1000 ms]"),
            DisplayName("프로브 카드 클램프 업-다운 실린더 동작 대기 시간 (ms)")]
        public int ProbeCard_ClampTypeB_CylUpDown_StableTime { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레시피를 변경할 때마다 상하부 카메라의 광축이 일치하는 지 확인 후 작업 진행하도록 한다.\r\n[상부 카메라 레티클 센터 확인 -> 하부 카메라 레티클 센터 확인]"),
            DisplayName("레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능")]
        public bool ReticleGlass_CenterCheck_forAlign { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위해 Vision Y 축이 장비 앞쪽으로 나오게 되는데, 약 90 mm 를 넘어서 나오게 되면 엘리베이터 축과 충돌할 수 있다. 이 한계값 이상으로 움직이지 못하게 한다.\r\n[default 0 : 90 mm]"),
            DisplayName("레티클 글래스 - 레티클 글래스를 확인 시 비전 카메라와 엘리베이터의 충돌 방지를 위한 Vision Y 축 이동 한계 위치. (mm, 대부분 90.0 이내)")]
        public double ReticleGlass_Vision_Y_Limit { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 과 웨이퍼 Gate 의 Center 를 일치시키기 위한 Offset X 값,     [# 조건 : [03] 웨이퍼 패킹 옵셋이 0 일 경우 #]\r\n\r\n[+X : (스테이지가 오른쪽으로 이동, 핀 컨택 위치가 왼쪽으로 이동함),   -X : (스테이지가 왼쪽으로 이동, 핀 컨택 위치가 오른쪽으로 이동함)]"),
            DisplayName("PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X     [ 카메라 광축과 Tip - Contact 위치가 일치하기 위한 오프셋 ] ")]
        public double PAK_WaferGate_Centering_Offset_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 과 웨이퍼 Gate 의 Center 를 일치시키기 위한 Offset Y 값,     [# 조건 : [03] 웨이퍼 패킹 옵셋이 0 일 경우 #]\r\n\r\n[+Y : (스테이지가 뒤쪽으로 이동, 핀 컨택 위치가 아래쪽으로 이동함),   -Y : (스테이지가 앞쪽으로 이동, 핀 컨택 위치가 위쪽으로 이동함)]"),
            DisplayName("PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y     [ 카메라 광축과 Tip - Contact 위치가 일치하기 위한 오프셋 ]")]
        public double PAK_WaferGate_Centering_Offset_Y { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("일정 시간 동안 장비를 사용하지 않을 경우 자동으로 로그아웃 한다."),
            DisplayName("자동 로그아웃 기능 사용 여부")]
        public bool Auto_LogOut_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("일정 시간 동안 장비를 사용하지 않을 경우 자동으로 로그아웃 한다.\r\n\r\n[default 0 : 10 min]"),
            DisplayName("자동 로그아웃 시간 (min)")]
        public double Auto_LogOut_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 시작할 때와 패킹 작업 완료할 때 작업자가 Leak 상태를 확인한 후 진행한다."),
            DisplayName("패킹 작업 완료 시 && 얼라인 작업 시작 시, OP 주관 Leak Check 기능 사용 여부")]
        public bool AlignPacking_OP_LeakCheck_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 동작 후 작업자가 Leak 상태를 확인하는 시간\r\n패킹 공압 신호를 Off 한 후, 이 시간만큼 지난 후에 다음 단계로 진행할 수 있다.\r\n[default 0: 30 sec]"),
            DisplayName("패킹 작업 완료 시, OP 주관 Leak 확인 시간 (sec)")]
        public double Packing_OP_LeakCheck_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 완료 후 작업자가 제품을 꺼내기 위헤 엘리베이터를 내리는 거리 (값이 너무 작으면 씬-척 감지로 인한 오동작이 발생할 수 있음)\r\n기준 위치는 Lip-Seal 이 PAK 과 밀착되는 위치이다.\r\n[default 0: 20 mm]"),
            DisplayName("패킹 작업 완료 후, 제품 언로딩을 위해 엘리베이터를 내리는 거리 (mm)")]
        public double Wafer_ProbeCard_Down_Distance_After_Packing { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            //Description("전면 안전센서에 물체가 감지될 경우, 작업이 중지된다. \r\n감지된 물체가 해제되면 이어서 작업이 진행된다."),
            Description("전면 안전센서에 물체가 감지될 경우, 작업이 중지된다. \r\n\r\n감지 해제 상태를 확인하고 다시 해당 작업을 시작한다."),
            DisplayName("인터락 - 전면 안전센서 사용 여부")]
        public bool AreaSensor_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            //Description("전면 안전센서에 물체가 감지될 경우, 작업이 중지된다. \r\n감지된 물체가 해제되면 이어서 작업이 진행된다."),
            Description("전면 안전센서에 물체가 감지될 경우, 작업이 중지되는데, 이때 모터의 서보를 Off 할 것인지 선택한다. \r\n\r\n[true : 안전센서 감시 시 Servo Off, false : 안전센서 감지 시 Servo On 상태 유지]"),
            DisplayName("인터락 - 전면 안전센서 감지 시 모터 Servo Off 여부")]
        public bool AreaSensor_ServoOff_Usage { set; get; }

        //[Category("[99] 장비 공통 파라미터"),
        //    Description("전면 안전센서에 물체가 감지되어 작업이 중지되었을 경우, 감지된 물체가 해제되면 곧바로 작업이 진행되지 않고 일정 대기시간 이후에 동작시키도록 한다. (채터링 현상 방지 및 안전을 위해)\r\n[default 0: 3 sec]"),
        //    DisplayName("인터락 - 전면 안전센서 해제 후 다시 동작시키기 위해 대기하는 시간 (sec)")]
        //public double AreaSensor_Off_Pause_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("수동패킹 모드의 권한 설정\r\n\r\n[true: 관리자만 수동패킹 사용 가능, false: 관리자와 작업자 모두 수동패킹 모드 사용 가능]"),
            DisplayName("수동패킹 모드 사용 권한 설정.  (true: 관리자 Only)")]
        public bool ManualPacking_Only_Admin { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 커버가 닫힌 상태에서 Packing 공압을 인가했을 때, Packing 압력 신호가 뜨면 관로가 막힌것으로 본다."),
            DisplayName("사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부")]
        public bool Pak_AirLineCheck_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("프로브 카드 커버가 닫힌 상태에서 Packing 공압을 인가했을 때, Packing 압력 신호가 뜨는지 확인하는 시간. 이 시간 이내에 신호가 뜨면 관로가 막힌것으로 본다."),
            DisplayName("사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)")]
        public int Pak_AirLineCheck_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 && 프로브카드 Packing 시, Packing 공압 신호를 사용할 것인지 여부."),
            DisplayName("사용 옵션 - Packing 공압 신호 사용 여부")]
        public bool Packing_VacuumSignal_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 && 프로브카드 Packing 시 Packing 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행\r\n\r\n(Packing 공압 신호를 인가하고 Thin-Chuck 이 압력에 의해 프로브 카드에 밀착되는 시간 동안 기다려야 한다. 약 3~5초 소요)"),
            DisplayName("사용 옵션 - Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)")]
        public int Packing_VacuumSignal_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 && 프로브카드 Packing 시 Packing 공압 신호를 사용할 경우, Packing 공압 신호가 들어온 후 이 시간만큼 대기 후 다음 동작 진행\r\n\r\n[PAK 에 씬-척이 충분히 밀착되도록 기다리는 시간]"),
            DisplayName("사용 옵션 - Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)")]
        public int Packing_VacuumSignal_AfterTime { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 공압 신호를 사용할 것인지 여부."),
            DisplayName("사용 옵션 - Wafer 공압 체크 사용 여부")]
        public bool Wafer_VacuumSignal_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
            DisplayName("사용 옵션 - Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        public int Wafer_VacuumSignal_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Thin-Chuck 감지 신호를 사용할 것인지 여부."),
            DisplayName("사용 옵션 - Thin-Chuck 감지 센서 사용 여부")]
        public bool ThinChuck_DetectSignal_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Thin-Chuck 공압 신호를 사용할 것인지 여부."),
            DisplayName("사용 옵션 - Thin-Chuck 공압 체크 사용 여부")]
        public bool ThinChuck_VacuumSignal_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Thin-Chuck 공압 신호를 사용하지 않을 경우, 이 시간만큼 대기 후 다음 동작 진행"),
            DisplayName("사용 옵션 - Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)")]
        public int ThinChuck_VacuumSignal_Time { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 동작 중, 비전 검사 없이 모션 구동만 할 것인지 여부. (false : 사용 안함)"),
            DisplayName("사용 옵션 - Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)")]
        public bool Wafer_Align_Cam_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 완료 후, 바로 Packing 작업을 진행할 것인지 여부. (false : 사용 안함)"),
            DisplayName("사용 옵션 - Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)")]
        public bool Packing_AutoStart_After_Wafer_Align_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 완료 후, Top - Mid - Bottom 위치에서 프로브 핀 위치 대비 웨이퍼의 위치 오차 검증을 진행할 것인지 여부. (false : 사용 안함)\r\n\r\n[###  무조건 사용  ###]"),
            DisplayName("사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함)                                       --> ### 무조건 사용하도록 변경 ###")]
        public bool Wafer_Align_ErrorCheck_After_Wafer_Align_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 완료 후, Top - Mid - Bottom 위치에서 웨이퍼의 위치 오차 비교를 진행할 것인지 여부. (false : 사용 안함)\r\n\r\n[###  Wafer 의 TOP, MID, BOTTOM 위치의 Gate 가 오차 범위를 벗어나면 다시 얼라인 하도록 한다.  ###]"),
            DisplayName("사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 오차 비교 여부. (false : 사용 안함)")]
        public bool Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 완료 후, Top - Mid - Bottom 위치에서 웨이퍼의 위치 오차 비교시 패킹 허용 오차 범위.\r\n\r\n[default 0: 0.05mm]"),
            DisplayName("사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 허용 오차 범위 (mm)")]
        public double Wafer_Gate_PosMarginErrorRange_After_Wafer_Align { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("True : 상시 On\r\nFalse : Align 시 Off, 자재 투입 시 On"),
            DisplayName("사용 옵션 - 실내 조명을 상시 On 상태로 할 것인지 여부. (false : Align 시 Off 되고, 자재를 Loading 할 때 On)")]
        public bool Indoor_Light_AlwaysOn_Usage { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            //Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후 대기 시간 (ms)\r\n(Packing 공압 신호를 인가하고 Thin-Chuck 이 압력에 의해 프로브 카드에 밀착되는 시간 동안 기다려야 한다. 약 3~5초 소요)"),
            Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후 대기 시간 (ms)\r\n(Packing 공압 신호를 사용하거나 사용하지 않거나, 이 시간만큼 기다린 후 진행된다.)\r\n[default 0: 4000 ms]"),
            DisplayName("안정화 시간 - Packing 공압 신호 On 후 대기 시간 (ms)")]
        public int StableTime_after_PackingSignal_On { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후, Thin-Chuck 이 프로브 카드에 밀착될 때(딸려 올라갈 때) Thin-Chuck 공압을 해제해야 하는데, 얼마만큼 시간이 지난 후에 Thin-Chuck 공압을 해제할 것인지 (ms)\r\n[default 0: 100 ms] ,   [주의 : Thin-Chuck 공압 해제 대기 시간 < Wafer 공압 해제 대기 시간]"),
            DisplayName("안정화 시간 - Packing 공압 신호 On 후 Thin-Chuck 공압을 해제하기 위해 대기하는 시간 (ms)")]
        public int StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후, Thin-Chuck 이 프로브 카드에 밀착될 때(딸려 올라갈 때) Wafer 공압을 해제해야 하는데, 얼마만큼 시간이 지난 후에 Wafer 공압을 해제할 것인지 (ms)\r\n[default 0: \"Packing 공압 신호 On 후 대기 시간 (ms)\"] ,   [주의 : Thin-Chuck 공압 해제 대기 시간 < Wafer 공압 해제 대기 시간]"),
            DisplayName("안정화 시간 - Packing 공압 신호 On 후 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)")]
        public int StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)"),
            DisplayName("안정화 시간 - Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)")]
        public int StableTime_after_WaferVacuumSignal_Off { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)"),
            DisplayName("안정화 시간 - Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)")]
        public int StableTime_before_PackingSignal_On { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)"),
            DisplayName("안정화 시간 - Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)")]
        public int StableTime_after_ThinChuckVacuumSignal_Off { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("웨이퍼 얼라인 시, 마크 검사 위치로 이동한 후에 진동 억제를 위한 안정화 시간 (ms)"),
            DisplayName("안정화 시간 - Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)")]
        public int WaferAlign_Move_StableTime { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            //Description("웨이퍼 && 프로브카드 Packing 공압 신호 On 후 대기 시간 (ms)\r\n(Packing 공압 신호를 인가하고 Thin-Chuck 이 압력에 의해 프로브 카드에 밀착되는 시간 동안 기다려야 한다. 약 3~5초 소요)"),
            Description("웨이퍼 && 프로브카드 Unpacking 공압 신호 On 후 대기 시간 (ms)\r\n(패킹 완료된 상태에서 Unpacking 공압 신호를 On 하면 PAK 이 공압에 의해 위로 밀려 올라오는데, 위로 너무 많이 올라오기 전에 Elev. Z축을 내려야 한다.)\r\n[default 0 : 100ms]"),
            DisplayName("안정화 시간 - Unpacking 작업 중, Unpacking 공압 신호 On 후 Elev. Z 축을 내리기 시작할 때까지 대기 시간 (ms)")]
        public int StableTime_after_UnpackingSignal_On { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Elevator Z 축이 Packing 위치로 이동할 때, Packing 위치에서 이 값만큼 뺀 거리까지 고속으로 이동하고, 이 구간은 저속으로 이동한다.\r\n\r\n[default : 10 mm]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)")]
        public double Wafer_ProbeCard_PackingOffset_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Elevator Z 축이 Packing 위치로 이동할 때, PAK 의 Latch 상태를 검사할 것인지 여부. (false : 검사 안함)"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사 여부. (false : 검사 안함)")]
        public bool ProbeCard_LatchStatusCheck_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Elevator Z 축이 Packing 위치로 이동할 때, Packing 위치에서 이 값만큼 뺀 거리까지 고속으로 이동하고, 이 위치에서 Latch 상태를 검사한다.\r\n\r\n[default : 20 mm]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리. (mm, 기준 : Packing 위치)")]
        public double ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Elevator Z 축이 Latch 상태 검사 위치로 이동한 후, 4개의 센서 앰프에 모두 동작할 때 까지 대기한다.\r\n\r\n[default : 1000 ms]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 이동 후 대기시간. (ms)")]
        public int ProbeCard_LatchStatusCheck_OffsetDistance_StableTime { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Elevator Z 축이 Unpacking 을 위헤 Packing 위치로 이동할 때, Packing 위치에서 이 값만큼 뺀 거리까지 고속으로 이동하고, 이 구간은 저속으로 이동한다.\r\n\r\n[default : 10 mm]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)")]
        public double Wafer_ProbeCard_UnpackingOffset_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("UnPacking 할 때, UnPacking 신호를 주면서 Elevator Z 축을 약간 아래로 내리는 거리\r\n[Packing 완료 후, Leak 로 인해 씬-척이 분리될 경우 안전사고가 발생할 수 있으므로 너무 많이 내리지 않도록 한다.]\r\n[default : 5 mm]     [권장 : 30mm 이내]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)")]
        public double Wafer_ProbeCard_UnPackingOffset_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("UnPacking 할 때, Packing 높이에서 얼마나 아래에서 UnPacking 작업을 진행할 것인지.\r\n[Packing 후 씬-척의 높이는 Packing 전보다 높기 때문에, Packing 높이보다 더 올라가야 한다.]\r\n[default : 1 mm],   (입력 값이 0 보다 작을 경우 위로 올라감)"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)")]
        public double Wafer_ProbeCard_UnPackingStartOffset_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("수동 패킹 시, 패킹 위치에서 몇 mm 아래까지 엘리베이터 Z 축을 올릴 것인지.\r\n\r\n[default 0 : 30 mm]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)")]
        public double Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("수동 패킹 시, 패킹 위치까지 엘리베이터 Z 축을 올리는 Concept.\r\n[True : 패킹 Offset 거리까지 고속으로 이동하고, 패킹 위치까지 저속으로 이동]\r\n[False : 1단계 Offset 거리만큼 이동 후 대기]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 이동 방법 (2 Step 이동 or 이동 후 대기)")]
        public bool Wafer_ProbeCard_ManualPacking_1st_Step_ElevZ_OffsetMove_Concept { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 과 씬-척 분리를 위해 엘리베이터 Z 축을 PAK 하단 어느 위치까지 올릴 것인지.\r\n(사용 조건 : Packing 을 진행했을 때의 얼라인 스테이지 위치값을 모를 때에 한함)\r\n[default 0 : 30 mm]"),
            DisplayName("오프셋 - 웨이퍼 && 프로브카드 안전 분리 동작 시, 씬-척 낙하 방지를 위해 엘리베이터 Z 축을 올리는 위치. (Packing 위치 대비 Offset 거리 (mm, > 0)")]
        public double Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("카메라를 사용하기 위해 시리얼 넘버가 필요한데, 장비 공통 파라미터에 입력한 시리얼 넘버를 사용할 것인지, Config 파일에 세팅된 시리얼 넘버를 사용할 것인지 선택\r\n\r\n[True : 장비 공통 파라미터, False : Config 설정]"),
            DisplayName("카메라 설정 - 시리얼 넘버 사용 여부 (False : Config 에 세팅된 시리얼 넘버 사용)")]
        public bool Camera_SerialNumber_Type { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 카메라의 시리얼 넘버"),
            DisplayName("카메라 설정 - PAK 카메라 시리얼 넘버 (Upper Camera)")]
        public string Camera_SerialNumber_PAK { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Wafer 카메라의 시리얼 넘버"),
            DisplayName("카메라 설정 - Wafer 카메라 시리얼 넘버 (Lower Camera)")]
        public string Camera_SerialNumber_Wafer { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 비전 카메라 위치 (Vision X)\r\n\r\n[Calibration Mode 에서 위치를 설정하므로 여기에서는 변경하지 않도록 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 확인 위치 (VIsion X)                                                                                                 ## 임의 변경 금지 ##")]
        public double ReticleGlass_Vision_X_Pos { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 비전 카메라 위치 (Vision Y)\r\n\r\n[Calibration Mode 에서 위치를 설정하므로 여기에서는 변경하지 않도록 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Y)                                                                                                 ## 임의 변경 금지 ##")]
        public double ReticleGlass_Vision_Y_Pos { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 비전 카메라 위치 (Vision Z)\r\n\r\n[Calibration Mode 에서 위치를 설정하므로 여기에서는 변경하지 않도록 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                                                                                                 ## 임의 변경 금지 ##")]
        public double ReticleGlass_Vision_Z_Pos { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 PAK 비전 카메라 위치 (Elev. Z)\r\n\r\n[Calibration Mode 에서 위치를 설정하므로 여기에서는 변경하지 않도록 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - PAK 카메라)                                                                                ## 임의 변경 금지 ##")]
        public double ReticleGlass_Elev_Z_PAK_Pos { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 Wafer 비전 카메라 위치 (Elev. Z)\r\n\r\n[Calibration Mode 에서 위치를 설정하므로 여기에서는 변경하지 않도록 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - Wafer 카메라)                                                                              ## 임의 변경 금지 ##")]
        public double ReticleGlass_Elev_Z_Wafer_Pos { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 웨이퍼 비전 카메라의  이미지 Offset 위치 (X)\r\n[메인 화면의 \"레티클 글래스 위치 변경\" 에서 Offset 을 설정하므로 여기에서는 변경하지 않도록 한다.]\r\n[이미지 Offset 이동을 위해서는 이미지 최대 크기를 줄여야 한다. 줄인 크기의 1/2 을 초기 Offset 으로 설정해야 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset X                                                                                                ## 임의 변경 금지 ##")]
        public int ReticleGlass_WaferVision_Offset_X { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스를 확인하기 위한 웨이퍼 비전 카메라의  이미지 Offset 위치 (Y)\r\n[메인 화면의 \"레티클 글래스 위치 변경\" 에서 Offset 을 설정하므로 여기에서는 변경하지 않도록 한다.]\r\n[이미지 Offset 이동을 위해서는 이미지 최대 크기를 줄여야 한다. 줄인 크기의 1/2 을 초기 Offset 으로 설정해야 한다.]"),
            DisplayName("레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset Y                                                                                                ## 임의 변경 금지 ##")]
        public int ReticleGlass_WaferVision_Offset_Y { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 기능을 사용할 것인지 선택"),
            DisplayName("레티클 글래스 자동 보정 - 사용 여부 (False : 사용 안함)")]
        public bool ReticleAutoCal_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)"),
            DisplayName("레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)")]
        public double ReticleAutoCal_UpperVision_Allowable_XY { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)"),
            DisplayName("레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)")]
        public double ReticleAutoCal_LowerVision_Allowable_XY { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 - 상부 카메라의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("레티클 글래스 자동 보정 - 상부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수")]
        public int ReticleAutoCal_Upper_AlignMarkCount_forAverage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 - 하부 카메라의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("레티클 글래스 자동 보정 - 하부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수")]
        public int ReticleAutoCal_Lower_AlignMarkCount_forAverage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 - 얼라인 마크의 평균값 계산 후, 평균값이 기본 마크 위치값과 얼마나 차이가 있는지 비교. 너무 크면 기본 마크 위치값을 사용하도록 하기 위함.\r\n[default 0 : 0.05 mm]"),
            DisplayName("레티클 글래스 자동 보정 - 얼라인 마크 위치 평균값 신뢰 공차 (mm)")]
        public double ReticleAutoCal_Vision_AverageCheck_Range { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("레티클 글래스 자동 보정 - 얼라인 재시도 회수"),
            DisplayName("레티클 글래스 자동 보정 - 얼라인 재시도 회수")]
        public int ReticleAutoCal_Align_Retries { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 얼라인 시 메인화면에서 조명값을 조정할 때, 1단계 조정 크기\r\n\r\n[n > 0]"),
            DisplayName("프로브카드 얼라인 - 조명값 조정 크기 1단계")]
        public int PAKAlign_LightValue_Step1 { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("PAK 얼라인 시 메인화면에서 조명값을 조정할 때, 2단계 조정 크기\r\n\r\n[n > 0]"),
            DisplayName("프로브카드 얼라인 - 조명값 조정 크기 2단계")]
        public int PAKAlign_LightValue_Step2 { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 작업 중, PAK 의 Latch 가 Lock 상태로 되어 있을 경우 엘리베이터 Z 축과 충돌하게 되는데, 이때 엘리베이터 축에 인가되는 Torque 값을 확인하여 긴급 정지한다.\r\n\r\n[장비 및 PAK 보호]"),
            DisplayName("인터락 - Z-Slip 기능 - 엘리베이터 Z 축의 Torque 값이 설정치를 초과할 경우 긴급정지 여부")]
        public bool ElevZ_ESTOP_by_Torque_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("엘리베이터 Z 축 구동 시, 이 기준값 이상으로 Torque 가 인가될 경우 긴급정지 시킨다.\r\n\r\n[default: 300]"),
            DisplayName("인터락 - Z-Slip 기능 - 엘리베이터 Z 축을 긴급정지시키기 위한 Torque 기준값")]
        public double ElevZ_ESTOP_Torque_Value { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("얼라인 후 예상되는 Contact 위치와 실제 Contact 위치가 일치하는지 점검하는 작업을 진행하라는 메세지 창 팝업 여부 설정."),
            DisplayName("Dummy Wafer Packing 검증 - 메세지 팝업 사용 여부. (False : 사용 안함)")]
        public bool DummyWaferPacking_Message_Usage { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Dummy Wafer Packing 검증 요청 메세지를 보여주는 시기 설정\r\n[True : 매 레시피를 변경때마다 메세지 창 팝업]\r\n[False : 패킹 작업 회수를 누적하고, 설정한 누적 회수에 도달하면 메세지 창 팝업]"),
            DisplayName("Dummy Wafer Packing 검증 - 메세지 팝업 주기 모드 선택. (True : 레시피 변경 시, False : 패킹 작업 누적 회수 도달 시)")]
        public bool DummyWaferPacking_MessagePopup_Mode { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Dummy Wafer Packing 검증 요청 메세지를 보여주는 시기를 패킹 작업 누적 회수로 설정할 경우, 이 회수만큼 패킹을 진행하면 Dummy Wafer Packing 검증 요청 메세지가 팝업된다.\r\n[default 0: 100 회]"),
            DisplayName("Dummy Wafer Packing 검증 - 패킹 작업 누적 회수. (메세지 팝업 주기를 \"False\" 로 할 경우, 이 회수만큼 패킹을 진행하면 검증 요청 메세지창 팝업)")]
        public int DummyWaferPacking_MessagePopup_Count { set; get; }


        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 높이 아래까지 씬-척을 올린 후 미세하게 올리면서 패킹을 진행할 것인지, 패킹 높이까지 씬-척을 올려서 패킹 할 것인지 선택\r\n[True : 패킹 공압이 체크 될 때 까지 단계적으로 올림, False : 패킹 높이까지 씬-척을 올려서 패킹 진행]"),
            DisplayName("패킹 방법 - 패킹 모드 선택 (True : 패킹 공압 On 후 Elev. Z 를 단계적으로 올려서 패킹, False : 패킹 높이까지 씬-척을 올려서 패킹)")]
        public bool PackingConcept_StepUp_Mode { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 높이 몇 mm 아래까지 씬-척을 올릴 것인지 입력\r\n[default : 5 (mm)]"),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, 패킹 시작 오프셋. (mm, 패킹 높이에서 이 값만큼 떨어진 위치까지 Elev. Z 를 올린 후 시작)")]
        public double PackingConcept_StepMode_StartOffset { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 높이 아래에서 이 값만큼씩 이동하면서 패킹 공압 체크\r\n[default : 0.1 (mm)]"),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 단위 거리. (mm)")]
        public double PackingConcept_StepMode_MoveOffset { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("True : 패킹 높이 아래에서 저속으로 Chuck 을 올리면서 패킹 공압이 들어오는지 확인.\r\nFalse : Step Up 이동 -> 패킹 공압 확인 -> Step Up 이동을반복"),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 방법 선택. (True : 연속 이동(저속), False : 단위 거리 이동)")]
        public bool PackingConcept_StepMode_StepUpMethod_Continuous { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 높이 아래에서 저속으로 Chuck 을 올리면서 패킹 공압이 들어오는지 확인하는 모드일 경우, 저속으로 Chuck 을 올리는 속도 (mm/s)\r\n[default : 0.5 mm/s)"),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 저속으로 상승 시키면서 패킹 공압을 확인하는 모드일 경우 이동 속도 (mm/s)")]
        public double PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("Lip-Seal 이 PAK 에 닿는 위치를 패킹 위치로 설정하면, Lip-Seal 이 낮은 Chuck 은 패킹 공압이 형성되지 않을 수 있다. 그래서 추가로 더 올려서 패킹 공압을 체크하도록 한다.\r\n[default : 0.1 (mm)]"),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축에 설정된 패킹 높이보다 추가로 더 올리는 거리. (mm)")]
        public double PackingConcept_StepMode_PackingPos_AddOffset { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("패킹 높이 아래에서 Step 만큼씩 이동하면서 패킹 공압이 들어오는 지 확인하기 위해 대기하는 시간\r\n[default : 500 (ms)]"),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 Step 만큼 올린 후 패킹 공압을 체크하기 위해 대기하는 시간 (ms)")]
        public double PackingConcept_StepMode_PackingPressure_CheckTime { set; get; }

        [Category("[99] 장비 공통 파라미터"),
            Description("True : 패킹 높이 아래에서 저속으로 Chuck 을 올리면서 별도의 공압 센서에 패킹 공압이 들어올 때 Thin-Chuck 공압 파기.\r\nFalse : 패킹 높이 아래에서 저속으로 Chuck 을 올리면서 기존의 공압 센서에 패킹 공압이 들어올 때 Thin-Chuck 공압 파기."),
            DisplayName("패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키면서 패킹 공압을 체크할 때 별도의 공압센서를 사용할 것인지 여부 선택. (True : 사용, False : 사용하지 않음)")]
        public bool PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage { set; get; }

        #endregion



        #region Constructor
        public VisionParameterConfig()
        {
            Init();

            //Scanner_KFactor = 1000.0;
        }
        //public List<ZPositionData> Positions { set; get; }
        #endregion

        protected void Init()
        {
            if (VisionPositions == null)
                VisionPositions = new XyzyPositionDataCollection();

            m_VisionPosition = new List<XyzyPositionData>();
            m_VisionPosition.Clear();

            VisionPositions.Clear();



            foreach (PositionLaser key in Enum.GetValues(typeof(PositionLaser)))
            {
                XyzyPositionData positionBase = new XyzyPositionData();

                positionBase.Name = key.ToString();
                m_VisionPosition.Add(positionBase);
                VisionPositions.Add(positionBase);

                XyzyPositionData positionTarget = new XyzyPositionData();

                //positionTarget.Name = key.ToString();                     //  Offset 부분까지 Name 이 보이면 Position 별 구분이 잘 안됨.
                positionTarget.Type = TargetType.Offset;
                m_VisionPosition.Add(positionTarget);
                VisionPositions.Add(positionTarget);
            }
        }

        public List<string> GetLaserPositionList()
        {
            List<string> ret = new List<string>();
            foreach (XyzyPositionData position in m_VisionPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            foreach (XyzyPositionData pos in m_VisionPosition)
            {
                //  X
                if (i >= parameters.Count)
                    return i;
                pos.X = parameters[i++].DoubleValue;

                //  Y
                if (i >= parameters.Count)
                    return i;
                pos.Y = parameters[i++].DoubleValue;

                //  Z
                if (i >= parameters.Count)
                    return i;
                pos.Z = parameters[i++].DoubleValue;

                //  Mask Y
                if (i >= parameters.Count)
                    return i;
                pos.MASK_Y = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            foreach (XyzyPositionData pos in m_VisionPosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.X;
                    param.Tag = WorkStageParameter.MotionKey.X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Y;
                    param.Tag = WorkStageParameter.MotionKey.Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Z;
                    param.Tag = WorkStageParameter.MotionKey.Z.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.MASK_Y;
                    param.Tag = WorkStageParameter.MotionKey.MASK_Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
