using QMC.Common.Vision;
using QMC.Common.VisionPart;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Equipment;

namespace QMC.Common.Recipe
{
    public class SocketMarkInfo
    {
        public int AlignType { get; set; } = 0;
        public int MarkType { get; set; } = 0;
        public int MarkColor { get; set; } = 0;
        public double MarkRadius { get; set; } = 0.0;
        public double MarkSpec { get; set; } = 0.0;
        public double MarkScore { get; set; } = 0.0;
        public int IllumRed { get; set; } = 0;
        public int IllumIR { get; set; } = 0;
        public bool UseRed { get; set; } = false;
        public bool UseIR { get; set; } = false;
        public double ExposureTime { get; set; } = 0.0;
        public double AxisZOffset { get; set; } = 0.0;

        public SocketMarkInfo Clone()
        {
            return new SocketMarkInfo
            {
                AlignType = this.AlignType,
                MarkType = this.MarkType,
                MarkColor = this.MarkColor,
                MarkRadius = this.MarkRadius,
                MarkSpec = this.MarkSpec,
                MarkScore = this.MarkScore,
                UseIR = this.UseIR,
                UseRed = this.UseRed,
                ExposureTime = this.ExposureTime,
                AxisZOffset = this.AxisZOffset,
                IllumIR = this.IllumIR,
                IllumRed = this.IllumRed
            };
        }
    }

    public class PreAlignMarkInfo
    {
        public PatternMatchingParameters PatternMatching { get; set; } = new PatternMatchingParameters();

        public System.Drawing.Point TrainRoiStart { get; set; } = new System.Drawing.Point(0, 0);
        public System.Drawing.Point TrainRoiEnd { get; set; } = new System.Drawing.Point(0, 0);
        public System.Drawing.Point InspectRoiStart { get; set; } = new System.Drawing.Point(0, 0);
        public System.Drawing.Point InspectRoiEnd { get; set; } = new System.Drawing.Point(0, 0);

        public int IllumIR { get; set; } = 0;
        public int IllumRed { get; set; } = 0;
        public bool UseRed { get; set; } = false;
        public bool UseIR { get; set; } = false;
        public string TrainImagePath { get; set; }

        public int CircleColor { get; set; } = 0; // 0: White, 1: Black, 2: Ignore
        public double CircleMarkRadius { get; set; } = 0.0;
        public double CircleMarkSpec { get; set; } = 0.0;
        public double CircleMarkScore { get; set; } = 0.0;

        public VisionAlgorithmType AlgorithmType { get; set; } = VisionAlgorithmType.PatternMatching;
        public MarkTypeList MarkType { get; set; } = MarkTypeList.Circle;
        public double ExposureTime { get; set; } = 0.0;

        public PreAlignMarkInfo Clone()
        {
            return new PreAlignMarkInfo
            {
                PatternMatching = this.PatternMatching,//.Clone(),
                TrainRoiStart = this.TrainRoiStart,
                TrainRoiEnd = this.TrainRoiEnd,
                InspectRoiStart = this.InspectRoiStart,
                InspectRoiEnd = this.InspectRoiEnd,
                IllumIR = this.IllumIR,
                IllumRed = this.IllumRed,
                UseRed = this.UseRed,
                UseIR = this.UseIR,
                TrainImagePath = this.TrainImagePath,
                CircleColor = this.CircleColor,
                CircleMarkRadius = this.CircleMarkRadius,
                CircleMarkSpec = this.CircleMarkSpec,
                CircleMarkScore = this.CircleMarkScore,
                AlgorithmType = this.AlgorithmType,
                MarkType = this.MarkType,
                ExposureTime = this.ExposureTime
            };
        }
    }

    // === (추가) GoldPowder 소켓별 포지션 구조 선언 ===
    public class GoldPowderSocketPos
    {
        // Index: 0~3 => Pos1~Pos4
        public double[] X = new double[4];
        public double[] Y = new double[4];

        public GoldPowderSocketPos Clone()
        {
            var c = new GoldPowderSocketPos();
            for (int i = 0; i < 4; i++)
            {
                c.X[i] = X[i];
                c.Y[i] = Y[i];
            }
            return c;
        }
    }


    public class VisionRecipeData
    {
        // (추가) GoldPowder 소켓별 포지션 리스트
        public List<GoldPowderSocketPos> GoldPowderSocketPosList { get; private set; } = new List<GoldPowderSocketPos>();
        public List<GoldPowderSocketPos> GoldPowderSocketPosListOffset { get; private set; } = new List<GoldPowderSocketPos>();

        public void EnsureGoldPowderSocketPosCount(int count)
        {
            while (GoldPowderSocketPosList.Count < count)
                GoldPowderSocketPosList.Add(new GoldPowderSocketPos());
        }
        public GoldPowderSocketPos GetGoldPowderSocketPos(int socketIndex)
        {
            if (socketIndex < 0 || socketIndex >= GoldPowderSocketPosList.Count)
                return null;
            return GoldPowderSocketPosList[socketIndex];
        }
        public void SetGoldPowderPos(int socketIndex, int posIndex01to04, double x, double y)
        {
            if (posIndex01to04 < 1 || posIndex01to04 > 4) return;
            if (socketIndex < 0) return;
            EnsureGoldPowderSocketPosCount(socketIndex + 1);
            int idx = posIndex01to04 - 1;
            GoldPowderSocketPosList[socketIndex].X[idx] = x;
            GoldPowderSocketPosList[socketIndex].Y[idx] = y;
        }

        public void EnsureGoldPowderSocketPosCount_Offset(int count)
        {
            while (GoldPowderSocketPosListOffset.Count < count)
                GoldPowderSocketPosListOffset.Add(new GoldPowderSocketPos());
        }
        public GoldPowderSocketPos GetGoldPowderSocketPos_Offset(int socketIndex)
        {
            if (socketIndex < 0 || socketIndex >= GoldPowderSocketPosListOffset.Count)
                return null;
            return GoldPowderSocketPosListOffset[socketIndex];
        }
        public void SetGoldPowderPos_Offset(int socketIndex, int posIndex01to04, double x, double y)
        {
            if (posIndex01to04 < 1 || posIndex01to04 > 4) 
                return;
            if (socketIndex < 0) 
                return;

            EnsureGoldPowderSocketPosCount_Offset(socketIndex + 1);
            int idx = posIndex01to04 - 1;
            GoldPowderSocketPosListOffset[socketIndex].X[idx] = x;
            GoldPowderSocketPosListOffset[socketIndex].Y[idx] = y;
        }




        public VisionRecipeData()
        {
            //PrePatternMatching = new PatternMatchingParameters();
            //pointPreTrainRoiStartLocation = new System.Drawing.Point(0, 0);
            //pointPreTrainRoiEndLocation = new System.Drawing.Point(0, 0);
            //pointPreInspectRoiStartLocation = new System.Drawing.Point(0, 0);
            //pointPreInspectRoiEndLocation = new System.Drawing.Point(0, 0);
        }

        //Socket
        public List<SocketMarkInfo> SocketMarkList { get; private set; } = new List<SocketMarkInfo>();
        public List<PreAlignMarkInfo> PreAlignMarkList { get; private set; } = new List<PreAlignMarkInfo>();


        // 기존과의 호환을 위한 속성 매핑 (SocketMarkList[0] 기반)
        public int nSocketAlignType => SocketMarkList.Count > 0 ? SocketMarkList[0].AlignType : 0;
        public int nSocketMarkType => SocketMarkList.Count > 0 ? SocketMarkList[0].MarkType : 0;
        public int nSocketCircleColor => SocketMarkList.Count > 0 ? SocketMarkList[0].MarkColor : 0;
        public double dSocketCircleMarkRadius => SocketMarkList.Count > 0 ? SocketMarkList[0].MarkRadius : 0.0;
        public double dSocketCircleMarkSpec => SocketMarkList.Count > 0 ? SocketMarkList[0].MarkSpec : 0.0;
        public double dSocketCircleMarkScore => SocketMarkList.Count > 0 ? SocketMarkList[0].MarkScore : 0.0;
        public int nSocketIlluminationRed => SocketMarkList.Count > 0 ? SocketMarkList[0].IllumRed : 0;
        public int nSocketIlluminationIR => SocketMarkList.Count > 0 ? SocketMarkList[0].IllumIR : 0;
        public bool bSocketIlluminationRedUse => SocketMarkList.Count > 0 ? SocketMarkList[0].UseRed : false;
        public bool bSocketIlluminationIRUse => SocketMarkList.Count > 0 ? SocketMarkList[0].UseIR : false;
        public double dSocketIlluminationExposureTime => SocketMarkList.Count > 0 ? SocketMarkList[0].ExposureTime : 0.0;
        public double dSocketAxisZ_Offset => SocketMarkList.Count > 0 ? SocketMarkList[0].AxisZOffset : 0.0;

        //PreAlign
        public PatternMatchingParameters PrePatternMatching => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].PatternMatching : new PatternMatchingParameters();
        public System.Drawing.Point pointPreTrainRoiStartLocation => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].TrainRoiStart : new System.Drawing.Point(0, 0);
        public System.Drawing.Point pointPreTrainRoiEndLocation => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].TrainRoiEnd : new System.Drawing.Point(0, 0);
        public System.Drawing.Point pointPreInspectRoiStartLocation => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].InspectRoiStart : new System.Drawing.Point(0, 0);
        public System.Drawing.Point pointPreInspectRoiEndLocation => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].InspectRoiEnd : new System.Drawing.Point(0, 0);
        public int nPreIlluminationIR => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].IllumIR : 0;
        public int nPreIlluminationRed => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].IllumRed : 0;
        public string pointPreTrainImagePath => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].TrainImagePath : "";

        public int nPreCircleColor => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].CircleColor : 0;  //0: White, 1: Black, 2: Ignore
        public double dPreCircleMarkRadius => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].CircleMarkRadius : 0;  //circle size width
        public double dPreCircleMarkSpec => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].CircleMarkSpec : 0;  //circle spec
        public double dPreCircleMarkScore => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].CircleMarkScore : 0;  //circle score
        public VisionAlgorithmType ePreAlgorithmType => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].AlgorithmType : VisionAlgorithmType.PatternMatching;
        public MarkTypeList ePreMarkType => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].MarkType : MarkTypeList.Circle;
        public double dPreAlignIlluminationExposureTime => PreAlignMarkList.Count > 0 ? PreAlignMarkList[0].ExposureTime : 0.0;

        //GoldPowder
        public int dGoldPowderAlignType;                 //  Fiducial Align Type (0:Circle Find, 2:Pattern Matching)
        public int dGoldPowderMarkType;                  //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
        public bool bGoldPowderCircleColor;             //0: White, 1: Black
        public double dGoldPowderCircleMarkRadius;                  //  Fiducial Mark Size (mm)
        public double dGoldPowderCircleMarkSpec;                  //  Fiducial Mark Spec
        public double dGoldPowderCircleMarkScore;         //circle score
        public int nGoldPowderIlluminationIR;
        public int nGoldPowderIlluminationRed;
        public bool bGoldPowderIlluminationIRUse;
        public bool bGoldPowderIlluminationRedUse;
        public double dGoldPowderIlluminationExposureTime;
        public double dGoldPowderAxisZ_Offset;
        public int nGoldPowderCircleMarkMaxInstance;
        public int nGoldPowderCircleMarkFindCount;
        // 추가 : GoldPowder 위치 포지션 4개 (X,Y)
        public double dGoldPowderPos1X; public double dGoldPowderPos1Y;
        public double dGoldPowderPos2X; public double dGoldPowderPos2Y;
        public double dGoldPowderPos3X; public double dGoldPowderPos3Y;
        public double dGoldPowderPos4X; public double dGoldPowderPos4Y;
        public double dGoldPowderPos1XOffset; public double dGoldPowderPos1YOffset;
        public double dGoldPowderPos2XOffset; public double dGoldPowderPos2YOffset;
        public double dGoldPowderPos3XOffset; public double dGoldPowderPos3YOffset;
        public double dGoldPowderPos4XOffset; public double dGoldPowderPos4YOffset;


        public bool SaveToIni(string path)
        {
            bool bRet = false;

            // SocketAlign
            NativeMethods.WritePrivateProfileString("SocketAlign", "Count", SocketMarkList.Count.ToString(), path);

            for (int i = 0; i < SocketMarkList.Count; i++)
            {
                string section = $"SocketMark_{i}";
                var mark = SocketMarkList[i];

                NativeMethods.WritePrivateProfileString(section, "AlignType", mark.AlignType.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MarkType", mark.MarkType.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MarkColor", mark.MarkColor.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MarkRadius", mark.MarkRadius.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MarkSpec", mark.MarkSpec.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MarkScore", mark.MarkScore.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "IllumRed", mark.IllumRed.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "IllumIR", mark.IllumIR.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "UseRed", mark.UseRed.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "UseIR", mark.UseIR.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "ExposureTime", mark.ExposureTime.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "AxisZOffset", mark.AxisZOffset.ToString(), path);
            }

            //PreAlign
            NativeMethods.WritePrivateProfileString("PreAlign", "Count", PreAlignMarkList.Count.ToString(), path);
            for (int i = 0; i < PreAlignMarkList.Count; i++)
            {
                string section = $"PreAlignMark_{i}";
                var mark = PreAlignMarkList[i];

                // Pattern Matching
                NativeMethods.WritePrivateProfileString(section, "MinScore", mark.PatternMatching.MinScore.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MaxInstance", mark.PatternMatching.MaxInstance.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "MaxTolerance", mark.PatternMatching.MaxTolerance.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "DuplicateChecked", mark.PatternMatching.DuplicateChecked.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "UseMaskImage", mark.PatternMatching.UseMaskImage.ToString(), path);

                // ROI
                NativeMethods.WritePrivateProfileString(section, "TrainRoiStartX", mark.TrainRoiStart.X.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "TrainRoiStartY", mark.TrainRoiStart.Y.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "TrainRoiEndX", mark.TrainRoiEnd.X.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "TrainRoiEndY", mark.TrainRoiEnd.Y.ToString(), path);

                NativeMethods.WritePrivateProfileString(section, "InspectRoiStartX", mark.InspectRoiStart.X.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "InspectRoiStartY", mark.InspectRoiStart.Y.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "InspectRoiEndX", mark.InspectRoiEnd.X.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "InspectRoiEndY", mark.InspectRoiEnd.Y.ToString(), path);
                
                // Vision 설정
                NativeMethods.WritePrivateProfileString(section, "AlgorithmType", ((int)mark.AlgorithmType).ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "PatternShape", ((int)mark.MarkType).ToString(), path);

                // Illumination
                NativeMethods.WritePrivateProfileString(section, "Red", mark.IllumRed.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "IR", mark.IllumIR.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "RedUse", mark.UseRed.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "IRUse", mark.UseIR.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "ExposureTime", mark.ExposureTime.ToString(), path);

                // Circle
                NativeMethods.WritePrivateProfileString(section, "Color", mark.CircleColor.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "SizeW", mark.CircleMarkRadius.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "Spec", mark.CircleMarkSpec.ToString(), path);
                NativeMethods.WritePrivateProfileString(section, "Score", mark.CircleMarkScore.ToString(), path);

                // 학습 이미지 경로
                if (!string.IsNullOrWhiteSpace(mark.TrainImagePath))
                    NativeMethods.WritePrivateProfileString(section, "TrainImagePath", mark.TrainImagePath, path);
            }

            //기존코드 (주석 처리 유지)
            {
            }


            // GoldPowder
            NativeMethods.WritePrivateProfileString("GoldPowder", "Aligntype", dGoldPowderAlignType.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "MarkType", dGoldPowderMarkType.ToString(), path);

            NativeMethods.WritePrivateProfileString("GoldPowder", "MarkColor", bGoldPowderCircleColor.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "MarkSize", dGoldPowderCircleMarkRadius.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "MarkSpec", dGoldPowderCircleMarkSpec.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "MarkScore", dGoldPowderCircleMarkScore.ToString(), path);

            NativeMethods.WritePrivateProfileString("GoldPowder", "IR", nGoldPowderIlluminationIR.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Red", nGoldPowderIlluminationRed.ToString(), path);

            NativeMethods.WritePrivateProfileString("GoldPowder", "IRUse", bGoldPowderIlluminationIRUse.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "RedUse", bGoldPowderIlluminationRedUse.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "ExposureTime", dGoldPowderIlluminationExposureTime.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "AxisZ_Offset", dGoldPowderAxisZ_Offset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "CircleMarkMaxInstance", nGoldPowderCircleMarkMaxInstance.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "CircleMarkFindCount", nGoldPowderCircleMarkFindCount.ToString(), path);
            // 위치 4개 저장
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos1X", dGoldPowderPos1X.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos1Y", dGoldPowderPos1Y.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos2X", dGoldPowderPos2X.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos2Y", dGoldPowderPos2Y.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos3X", dGoldPowderPos3X.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos3Y", dGoldPowderPos3Y.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos4X", dGoldPowderPos4X.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowder", "Pos4Y", dGoldPowderPos4Y.ToString(), path);
            // 위치 4개 저장
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos1X", dGoldPowderPos1XOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos1Y", dGoldPowderPos1YOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos2X", dGoldPowderPos2XOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos2Y", dGoldPowderPos2YOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos3X", dGoldPowderPos3XOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos3Y", dGoldPowderPos3YOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos4X", dGoldPowderPos4XOffset.ToString(), path);
            NativeMethods.WritePrivateProfileString("GoldPowderOffset", "Pos4Y", dGoldPowderPos4YOffset.ToString(), path);

            // === (추가) 소켓별 GoldPowder 포지션 저장 ===
            NativeMethods.WritePrivateProfileString("GoldPowderSocketPos", "Count", GoldPowderSocketPosList.Count.ToString(), path);
            for (int s = 0; s < GoldPowderSocketPosList.Count; s++)
            {
                var gp = GoldPowderSocketPosList[s];
                string section = $"GoldPowderSocket_{s}";
                for (int p = 0; p < 4; p++)
                {
                    int posNo = p + 1;
                    NativeMethods.WritePrivateProfileString(section, $"Pos{posNo}X", gp.X[p].ToString(), path);
                    NativeMethods.WritePrivateProfileString(section, $"Pos{posNo}Y", gp.Y[p].ToString(), path);
                }
            }

            // === (추가) 소켓별 GoldPowder Offset 포지션 저장 ===
            NativeMethods.WritePrivateProfileString("GoldPowderSocketPosOffset", "Count", GoldPowderSocketPosListOffset.Count.ToString(), path);
            for (int s = 0; s < GoldPowderSocketPosListOffset.Count; s++)
            {
                var gp = GoldPowderSocketPosListOffset[s];
                string section = $"GoldPowderSocketOffset_{s}";
                for (int p = 0; p < 4; p++)
                {
                    int posNo = p + 1;
                    NativeMethods.WritePrivateProfileString(section, $"Pos{posNo}X", gp.X[p].ToString(), path);
                    NativeMethods.WritePrivateProfileString(section, $"Pos{posNo}Y", gp.Y[p].ToString(), path);
                }
            }


            bRet = true;
            return bRet;
        }

        public static VisionRecipeData LoadFromIni(string path)
        {
            VisionRecipeData data = new VisionRecipeData();
            //data.PrePatternMatching = new PatternMatchingParameters();
            StringBuilder sb = new StringBuilder(255);

            try
            {
                // SocketMarkList.Clear(); // 기존 리스트 초기화
                NativeMethods.GetPrivateProfileString("SocketAlign", "Count", "0", sb, sb.Capacity, path);
                int count = Equipment.ToInt(sb.ToString());

                for (int i = 0; i < count; i++)
                {
                    string section = $"SocketMark_{i}";
                    SocketMarkInfo mark = new SocketMarkInfo();

                    NativeMethods.GetPrivateProfileString(section, "AlignType", "0", sb, sb.Capacity, path);
                    mark.AlignType = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "MarkType", "0", sb, sb.Capacity, path);
                    mark.MarkType = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "MarkColor", "0", sb, sb.Capacity, path);
                    mark.MarkColor = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "MarkRadius", "0", sb, sb.Capacity, path);
                    mark.MarkRadius = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "MarkSpec", "0", sb, sb.Capacity, path);
                    mark.MarkSpec = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "MarkScore", "0", sb, sb.Capacity, path);
                    mark.MarkScore = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "IllumRed", "0", sb, sb.Capacity, path);
                    mark.IllumRed = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "IllumIR", "0", sb, sb.Capacity, path);
                    mark.IllumIR = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "UseRed", "False", sb, sb.Capacity, path);
                    mark.UseRed = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "UseIR", "False", sb, sb.Capacity, path);
                    mark.UseIR = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "ExposureTime", "0.0", sb, sb.Capacity, path);
                    mark.ExposureTime = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString(section, "AxisZOffset", "0.0", sb, sb.Capacity, path);
                    mark.AxisZOffset = Equipment.ToDouble(sb.ToString());

                    data.SocketMarkList.Add(mark);
                }

                // 마이그레이션: SocketAlign 섹션만 존재할 경우 → SocketMarkList[0]에 자동 등록
                if (count == 0)
                {
                    SocketMarkInfo mark = new SocketMarkInfo();

                    NativeMethods.GetPrivateProfileString("SocketAlign", "Aligntype", "0", sb, sb.Capacity, path);
                    mark.AlignType = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkType", "0", sb, sb.Capacity, path);
                    mark.MarkType = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkColor", "0", sb, sb.Capacity, path);
                    mark.MarkColor = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSize", "0.0", sb, sb.Capacity, path);
                    mark.MarkRadius = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSpec", "0.0", sb, sb.Capacity, path);
                    mark.MarkSpec = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkScore", "0.0", sb, sb.Capacity, path);
                    mark.MarkScore = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "Red", "0", sb, sb.Capacity, path);
                    mark.IllumRed = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "IR", "0", sb, sb.Capacity, path);
                    mark.IllumIR = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "RedUse", "False", sb, sb.Capacity, path);
                    mark.UseRed = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "IRUse", "False", sb, sb.Capacity, path);
                    mark.UseIR = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "ExposureTime", "0.0", sb, sb.Capacity, path);
                    mark.ExposureTime = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "AxisZ_Offset", "0.0", sb, sb.Capacity, path);
                    mark.AxisZOffset = Equipment.ToDouble(sb.ToString());

                    data.SocketMarkList.Add(mark);
                }

                //// SocketAlign //기존 코드
                {
                }

                NativeMethods.GetPrivateProfileString("PreAlign", "Count", "0", sb, sb.Capacity, path);
                int preAlignCount = Equipment.ToInt(sb.ToString());
                for (int i = 0; i < preAlignCount; i++)
                {
                    string section = $"PreAlignMark_{i}";
                    PreAlignMarkInfo mark = new PreAlignMarkInfo();

                    // PatternMatching
                    NativeMethods.GetPrivateProfileString(section, "MinScore", "0.7", sb, sb.Capacity, path);
                    mark.PatternMatching.MinScore = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "MaxInstance", "1", sb, sb.Capacity, path);
                    mark.PatternMatching.MaxInstance = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "MaxTolerance", "45", sb, sb.Capacity, path);
                    mark.PatternMatching.MaxTolerance = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "DuplicateChecked", "False", sb, sb.Capacity, path);
                    mark.PatternMatching.DuplicateChecked = Equipment.ToBoolean(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "UseMaskImage", "False", sb, sb.Capacity, path);
                    mark.PatternMatching.UseMaskImage = Equipment.ToBoolean(sb.ToString());

                    // ROI
                    mark.TrainRoiStart = new Point(
                        Equipment.ToInt(ReadIni(section, "TrainRoiStartX", path)),
                        Equipment.ToInt(ReadIni(section, "TrainRoiStartY", path))
                    );
                    mark.TrainRoiEnd = new Point(
                        Equipment.ToInt(ReadIni(section, "TrainRoiEndX", path)),
                        Equipment.ToInt(ReadIni(section, "TrainRoiEndY", path))
                    );
                    mark.InspectRoiStart = new Point(
                        Equipment.ToInt(ReadIni(section, "InspectRoiStartX", path)),
                        Equipment.ToInt(ReadIni(section, "InspectRoiStartY", path))
                    );
                    mark.InspectRoiEnd = new Point(
                        Equipment.ToInt(ReadIni(section, "InspectRoiEndX", path)),
                        Equipment.ToInt(ReadIni(section, "InspectRoiEndY", path))
                    );
                    
                    // Algorithm & MarkType
                    NativeMethods.GetPrivateProfileString(section, "AlgorithmType", "1", sb, sb.Capacity, path);
                    mark.AlgorithmType = (VisionAlgorithmType)Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "PatternShape", "1", sb, sb.Capacity, path);
                    mark.MarkType = (MarkTypeList)Equipment.ToInt(sb.ToString());

                    // Illumination
                    NativeMethods.GetPrivateProfileString(section, "IR", "3000", sb, sb.Capacity, path);
                    mark.IllumIR = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "Red", "0", sb, sb.Capacity, path);
                    mark.IllumRed = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "IRUse", "False", sb, sb.Capacity, path);
                    mark.UseIR = Equipment.ToBoolean(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "RedUse", "False", sb, sb.Capacity, path);
                    mark.UseRed = Equipment.ToBoolean(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "ExposureTime", "20000", sb, sb.Capacity, path);
                    mark.ExposureTime = Equipment.ToDouble(sb.ToString());

                    // Circle
                    NativeMethods.GetPrivateProfileString(section, "Color", "0", sb, sb.Capacity, path);
                    mark.CircleColor = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "SizeW", "1.0", sb, sb.Capacity, path);
                    mark.CircleMarkRadius = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "Spec", "0.1", sb, sb.Capacity, path);
                    mark.CircleMarkSpec = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString(section, "Score", "0.7", sb, sb.Capacity, path);
                    mark.CircleMarkScore = Equipment.ToDouble(sb.ToString());

                    // 이미지 경로
                    NativeMethods.GetPrivateProfileString(section, "TrainImagePath", "", sb, sb.Capacity, path);
                    mark.TrainImagePath = sb.ToString();

                    if (string.IsNullOrEmpty(mark.TrainImagePath))
                    {
                        string folderName = Path.GetFileNameWithoutExtension(path);
                        string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                        Directory.CreateDirectory(folderPath);
                        mark.TrainImagePath = Path.Combine(folderPath, "PreAlign.bmp");
                    }

                    data.PreAlignMarkList.Add(mark);
                }

                // 2. 구버전 ini (다중 마크 없음): 단일값 로드 + PreAlignMarkList[0]로 변환
                if (preAlignCount == 0)
                {
                    // [1] 기존 방식 단일 필드 모두 읽어오기 (임시 변수로 저장)
                    PatternMatchingParameters patternMatching = new PatternMatchingParameters();
                    NativeMethods.GetPrivateProfileString("PatternMatching", "MinScore", "0.7", sb, sb.Capacity, path);
                    patternMatching.MinScore = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString("PatternMatching", "MaxInstance", "1", sb, sb.Capacity, path);
                    patternMatching.MaxInstance = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("PatternMatching", "MaxTolerance", "45", sb, sb.Capacity, path);
                    patternMatching.MaxTolerance = Equipment.ToDouble(sb.ToString());
                    NativeMethods.GetPrivateProfileString("PatternMatching", "DuplicateChecked", "False", sb, sb.Capacity, path);
                    patternMatching.DuplicateChecked = Equipment.ToBoolean(sb.ToString());
                    NativeMethods.GetPrivateProfileString("PatternMatching", "UseMaskImage", "False", sb, sb.Capacity, path);
                    patternMatching.UseMaskImage = Equipment.ToBoolean(sb.ToString());

                    // ROI
                    Point trainRoiStart = new Point();
                    NativeMethods.GetPrivateProfileString("TrainROI", "StartX", "0", sb, sb.Capacity, path);
                    trainRoiStart.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("TrainROI", "StartY", "0", sb, sb.Capacity, path);
                    trainRoiStart.Y = Equipment.ToInt(sb.ToString());
                    Point trainRoiEnd = new Point();
                    NativeMethods.GetPrivateProfileString("TrainROI", "EndX", "0", sb, sb.Capacity, path);
                    trainRoiEnd.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("TrainROI", "EndY", "0", sb, sb.Capacity, path);
                    trainRoiEnd.Y = Equipment.ToInt(sb.ToString());

                    Point inspectRoiStart = new Point();
                    NativeMethods.GetPrivateProfileString("InspectROI", "StartX", "0", sb, sb.Capacity, path);
                    inspectRoiStart.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("InspectROI", "StartY", "0", sb, sb.Capacity, path);
                    inspectRoiStart.Y = Equipment.ToInt(sb.ToString());
                    Point inspectRoiEnd = new Point();
                    NativeMethods.GetPrivateProfileString("InspectROI", "EndX", "0", sb, sb.Capacity, path);
                    inspectRoiEnd.X = Equipment.ToInt(sb.ToString());
                    NativeMethods.GetPrivateProfileString("InspectROI", "EndY", "0", sb, sb.Capacity, path);
                    inspectRoiEnd.Y = Equipment.ToInt(sb.ToString());

                    // Algorithm & MarkType
                    VisionAlgorithmType algoType = VisionAlgorithmType.PatternMatching;
                    NativeMethods.GetPrivateProfileString("Vision", "AlgorithmType", "1", sb, sb.Capacity, path);
                    algoType = (VisionAlgorithmType)Equipment.ToInt(sb.ToString());
                    MarkTypeList markType = MarkTypeList.Circle;
                    NativeMethods.GetPrivateProfileString("Vision", "PatternShape", "1", sb, sb.Capacity, path);
                    markType = (MarkTypeList)Equipment.ToInt(sb.ToString());

                    // Illumination
                    int illumIR = 0;
                    NativeMethods.GetPrivateProfileString("PreAlign_llumination", "IR", "3000", sb, sb.Capacity, path);
                    illumIR = Equipment.ToInt(sb.ToString());
                    int illumRed = 0;
                    NativeMethods.GetPrivateProfileString("PreAlign_llumination", "Red", "0", sb, sb.Capacity, path);
                    illumRed = Equipment.ToInt(sb.ToString());

                    // Circle
                    int circleColor = 0;
                    NativeMethods.GetPrivateProfileString("CircleDetection", "Color", "0", sb, sb.Capacity, path);
                    circleColor = Equipment.ToInt(sb.ToString());
                    double circleMarkRadius = 1.0;
                    NativeMethods.GetPrivateProfileString("CircleDetection", "SizeW", "1.0", sb, sb.Capacity, path);
                    circleMarkRadius = Equipment.ToDouble(sb.ToString());
                    double circleMarkSpec = 0.1;
                    NativeMethods.GetPrivateProfileString("CircleDetection", "Spec", "0.1", sb, sb.Capacity, path);
                    circleMarkSpec = Equipment.ToDouble(sb.ToString());
                    double circleMarkScore = 0.7;
                    NativeMethods.GetPrivateProfileString("CircleDetection", "Score", "0.7", sb, sb.Capacity, path);
                    circleMarkScore = Equipment.ToDouble(sb.ToString());

                    double exposure = 20000.0;
                    NativeMethods.GetPrivateProfileString("PreAlign_llumination", "ExposureTime", "20000", sb, sb.Capacity, path);
                    exposure = Equipment.ToDouble(sb.ToString());

                    string trainImagePath = "";
                    NativeMethods.GetPrivateProfileString("TrainImage", "Path", "", sb, sb.Capacity, path);
                    trainImagePath = sb.ToString();
                    if (string.IsNullOrEmpty(trainImagePath))
                    {
                        string folderName = Path.GetFileNameWithoutExtension(path);
                        string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                        Directory.CreateDirectory(folderPath); // 없으면 생성
                        string bmpPath = Path.Combine(folderPath, "PreAlign.bmp");
                        trainImagePath = bmpPath;
                    }

                    // [2] PreAlignMarkList[0]에 복사
                    PreAlignMarkInfo mark = new PreAlignMarkInfo();
                    mark.PatternMatching = patternMatching;
                    mark.TrainRoiStart = trainRoiStart;
                    mark.TrainRoiEnd = trainRoiEnd;
                    mark.InspectRoiStart = inspectRoiStart;
                    mark.InspectRoiEnd = inspectRoiEnd;
                    mark.IllumIR = illumIR;
                    mark.IllumRed = illumRed;
                    mark.ExposureTime = exposure;
                    mark.CircleColor = circleColor;
                    mark.CircleMarkRadius = circleMarkRadius;
                    mark.CircleMarkSpec = circleMarkSpec;
                    mark.CircleMarkScore = circleMarkScore;
                    mark.AlgorithmType = algoType;
                    mark.MarkType = markType;
                    mark.TrainImagePath = trainImagePath;

                    data.PreAlignMarkList.Add(mark);
                }

                //기존 코드
                {
                }

                // GoldPowder
                NativeMethods.GetPrivateProfileString("GoldPowder", "Aligntype", "1", sb, sb.Capacity, path);
                data.dGoldPowderAlignType = 1;  // Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "MarkType", "0", sb, sb.Capacity, path);
                data.dGoldPowderMarkType = 0; // Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "MarkColor", "true", sb, sb.Capacity, path);
                data.bGoldPowderCircleColor = Equipment.ToBoolean(sb.ToString());

                NativeMethods.GetPrivateProfileString("GoldPowder", "MarkSize", "0.5", sb, sb.Capacity, path);
                data.dGoldPowderCircleMarkRadius = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "MarkSpec", "0.05", sb, sb.Capacity, path);
                data.dGoldPowderCircleMarkSpec = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "MarkScore", "0.7", sb, sb.Capacity, path);
                data.dGoldPowderCircleMarkScore = Equipment.ToDouble(sb.ToString());

                NativeMethods.GetPrivateProfileString("GoldPowder", "IR", "250", sb, sb.Capacity, path);
                data.nGoldPowderIlluminationIR = Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Red", "0", sb, sb.Capacity, path);
                data.nGoldPowderIlluminationRed = Equipment.ToInt(sb.ToString());

                NativeMethods.GetPrivateProfileString("GoldPowder", "IRUse", "True", sb, sb.Capacity, path);
                data.bGoldPowderIlluminationIRUse = Equipment.ToBoolean(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "RedUse", "True", sb, sb.Capacity, path);
                data.bGoldPowderIlluminationRedUse = Equipment.ToBoolean(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "ExposureTime", "20000", sb, sb.Capacity, path);
                data.dGoldPowderIlluminationExposureTime = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "AxisZ_Offset", "0.0", sb, sb.Capacity, path);
                data.dGoldPowderAxisZ_Offset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "CircleMarkMaxInstance", "1", sb, sb.Capacity, path);
                data.nGoldPowderCircleMarkMaxInstance = Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "CircleMarkFindCount", "7", sb, sb.Capacity, path);
                data.nGoldPowderCircleMarkFindCount = Equipment.ToInt(sb.ToString());
                
                // 위치 4개 로드
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos1X", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos1X = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos1Y", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos1Y = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos2X", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos2X = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos2Y", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos2Y = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos3X", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos3X = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos3Y", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos3Y = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos4X", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos4X = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowder", "Pos4Y", "0", sb, sb.Capacity, path); 
                data.dGoldPowderPos4Y = Equipment.ToDouble(sb.ToString());

                // 위치 4개 로드
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos1X", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos1XOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos1Y", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos1YOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos2X", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos2XOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos2Y", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos2YOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos3X", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos3XOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos3Y", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos3YOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos4X", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos4XOffset = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("GoldPowderOffset", "Pos4Y", "0", sb, sb.Capacity, path);
                data.dGoldPowderPos4YOffset = Equipment.ToDouble(sb.ToString());

                // === (추가) 소켓별 GoldPowder 포지션 로드 ===
                NativeMethods.GetPrivateProfileString("GoldPowderSocketPos", "Count", "0", sb, sb.Capacity, path);
                int socketCount = Equipment.ToInt(sb.ToString());
                if (socketCount > 0)
                {
                    for (int s = 0; s < socketCount; s++)
                    {
                        string section = $"GoldPowderSocket_{s}";
                        GoldPowderSocketPos gp = new GoldPowderSocketPos();
                        for (int p = 0; p < 4; p++)
                        {
                            int posNo = p + 1;
                            NativeMethods.GetPrivateProfileString(section, $"Pos{posNo}X", "0", sb, sb.Capacity, path);
                            gp.X[p] = Equipment.ToDouble(sb.ToString());
                            NativeMethods.GetPrivateProfileString(section, $"Pos{posNo}Y", "0", sb, sb.Capacity, path);
                            gp.Y[p] = Equipment.ToDouble(sb.ToString());
                        }
                        data.GoldPowderSocketPosList.Add(gp);
                    }
                }
                else
                {
                    // (마이그레이션) 기존 단일 전역 포지션을 Socket 0 에 넣는다.
                    GoldPowderSocketPos legacy = new GoldPowderSocketPos();
                    legacy.X[0] = data.dGoldPowderPos1X; legacy.Y[0] = data.dGoldPowderPos1Y;
                    legacy.X[1] = data.dGoldPowderPos2X; legacy.Y[1] = data.dGoldPowderPos2Y;
                    legacy.X[2] = data.dGoldPowderPos3X; legacy.Y[2] = data.dGoldPowderPos3Y;
                    legacy.X[3] = data.dGoldPowderPos4X; legacy.Y[3] = data.dGoldPowderPos4Y;
                    data.GoldPowderSocketPosList.Add(legacy);
                }

                // === (추가) 소켓별 GoldPowder 포지션 로드 ===
                NativeMethods.GetPrivateProfileString("GoldPowderSocketPosOffset", "Count", "0", sb, sb.Capacity, path);
                int socketCountOffset = Equipment.ToInt(sb.ToString());
                if (socketCountOffset > 0)
                {
                    for (int s = 0; s < socketCountOffset; s++)
                    {
                        string section = $"GoldPowderSocketOffset_{s}";
                        GoldPowderSocketPos gp = new GoldPowderSocketPos();
                        for (int p = 0; p < 4; p++)
                        {
                            int posNo = p + 1;
                            NativeMethods.GetPrivateProfileString(section, $"Pos{posNo}X", "0", sb, sb.Capacity, path);
                            gp.X[p] = Equipment.ToDouble(sb.ToString());
                            NativeMethods.GetPrivateProfileString(section, $"Pos{posNo}Y", "0", sb, sb.Capacity, path);
                            gp.Y[p] = Equipment.ToDouble(sb.ToString());
                        }
                        data.GoldPowderSocketPosListOffset.Add(gp);
                    }
                }
                else
                {
                    // (마이그레이션) 기존 단일 전역 포지션을 Socket 0 에 넣는다.
                    GoldPowderSocketPos legacy = new GoldPowderSocketPos();
                    legacy.X[0] = data.dGoldPowderPos1XOffset; legacy.Y[0] = data.dGoldPowderPos1YOffset;
                    legacy.X[1] = data.dGoldPowderPos2XOffset; legacy.Y[1] = data.dGoldPowderPos2YOffset;
                    legacy.X[2] = data.dGoldPowderPos3XOffset; legacy.Y[2] = data.dGoldPowderPos3YOffset;
                    legacy.X[3] = data.dGoldPowderPos4XOffset; legacy.Y[3] = data.dGoldPowderPos4YOffset;
                    data.GoldPowderSocketPosListOffset.Add(legacy);
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return data;
        }

        private static string ReadIni(string section, string key, string path, string defaultValue = "0")
        {
            StringBuilder sb = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, path);
            return sb.ToString();
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
}
