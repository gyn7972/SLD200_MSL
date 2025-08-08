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


    public class VisionRecipeData
    {
        
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

            //기존코드
            {
                //if (PrePatternMatching != null)
                //{
                //    NativeMethods.WritePrivateProfileString("PatternMatching", "MinScore", PrePatternMatching.MinScore.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("PatternMatching", "MaxInstance", PrePatternMatching.MaxInstance.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("PatternMatching", "MaxTolerance", PrePatternMatching.MaxTolerance.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("PatternMatching", "DuplicateChecked", PrePatternMatching.DuplicateChecked.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("PatternMatching", "UseMaskImage", PrePatternMatching.UseMaskImage.ToString(), path);

                //    NativeMethods.WritePrivateProfileString("TrainROI", "StartX", pointPreTrainRoiStartLocation.X.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("TrainROI", "StartY", pointPreTrainRoiStartLocation.Y.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("TrainROI", "EndX", pointPreTrainRoiEndLocation.X.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("TrainROI", "EndY", pointPreTrainRoiEndLocation.Y.ToString(), path);

                //    NativeMethods.WritePrivateProfileString("InspectROI", "StartX", pointPreInspectRoiStartLocation.X.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("InspectROI", "StartY", pointPreInspectRoiStartLocation.Y.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("InspectROI", "EndX", pointPreInspectRoiEndLocation.X.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("InspectROI", "EndY", pointPreInspectRoiEndLocation.Y.ToString(), path);

                //    NativeMethods.WritePrivateProfileString("Vision", "AlgorithmType", ((int)ePreAlgorithmType).ToString(), path);
                //    NativeMethods.WritePrivateProfileString("Vision", "PatternShape", ((int)ePreMarkType).ToString(), path);

                //    NativeMethods.WritePrivateProfileString("PreAlign_llumination", "IR", nPreIlluminationIR.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("PreAlign_llumination", "Red", nPreIlluminationRed.ToString(), path);

                //    NativeMethods.WritePrivateProfileString("CircleDetection", "Color", nPreCircleColor.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("CircleDetection", "SizeW", dPreCircleMarkRadius.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("CircleDetection", "Spec", dPreCircleMarkSpec.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("CircleDetection", "Score", dPreCircleMarkScore.ToString(), path);
                //    NativeMethods.WritePrivateProfileString("PreAlign_llumination", "ExposureTime", dPreAlignIlluminationExposureTime.ToString(), path);

                //    string folderName = Path.GetFileNameWithoutExtension(path);

                //    if (folderName == "")
                //        return bRet = false;

                //    string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                //    Directory.CreateDirectory(folderPath); // 없으면 생성
                //    string bmpPath = Path.Combine(folderPath, "PreAlign.bmp");  // BMP 저장

                //    //pointPreTrainImagePath = bmpPath;
                //    PreAlignMarkList[0].TrainImagePath = bmpPath;

                //    if (!string.IsNullOrWhiteSpace(pointPreTrainImagePath))
                //        NativeMethods.WritePrivateProfileString("TrainImage", "Path", pointPreTrainImagePath, path);

                //    bRet = true;
                //}
                //else
                //{
                //    bRet = false;
                //    //var mb = new MessageBoxOk();
                //    //mb.ShowDialog("Error!", "Data가 저장되지 않았습니다. 레시피를 불러온 후 진행 바랍니다.");
                //}
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
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "Aligntype", "1", sb, sb.Capacity, path);
                    //data.nSocketAlignType = 1;  // Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "MarkType", "0", sb, sb.Capacity, path);
                    //data.nSocketMarkType = 0; // Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "MarkColor", "true", sb, sb.Capacity, path);
                    //data.nSocketCircleColor = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSize", "0.5", sb, sb.Capacity, path);
                    //data.dSocketCircleMarkRadius = Equipment.ToDouble(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSpec", "0.05", sb, sb.Capacity, path);
                    //data.dSocketCircleMarkSpec = Equipment.ToDouble(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "MarkScore", "0.7", sb, sb.Capacity, path);
                    //data.dSocketCircleMarkScore = Equipment.ToDouble(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "IR", "250", sb, sb.Capacity, path);
                    //data.nSocketIlluminationIR = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "Red", "0", sb, sb.Capacity, path);
                    //data.nSocketIlluminationRed = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "IRUse", "True", sb, sb.Capacity, path);
                    //data.bSocketIlluminationIRUse = Equipment.ToBoolean(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "RedUse", "True", sb, sb.Capacity, path);
                    //data.bSocketIlluminationRedUse = Equipment.ToBoolean(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "ExposureTime", "20000", sb, sb.Capacity, path);
                    //data.dSocketIlluminationExposureTime = Equipment.ToDouble(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("SocketAlign", "AxisZ_Offset", "0.0", sb, sb.Capacity, path);
                    //data.dSocketAxisZ_Offset = Equipment.ToDouble(sb.ToString());
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

                    // 첫 번째 마크는 단일 프로퍼티에도 복사
                    //if (i == 0)
                    //{
                    //    data.PrePatternMatching = mark.PatternMatching;
                    //    data.pointPreTrainRoiStartLocation = mark.TrainRoiStart;
                    //    data.pointPreTrainRoiEndLocation = mark.TrainRoiEnd;
                    //    data.pointPreInspectRoiStartLocation = mark.InspectRoiStart;
                    //    data.pointPreInspectRoiEndLocation = mark.InspectRoiEnd;
                    //    data.nPreIlluminationIR = mark.IllumIR;
                    //    data.nPreIlluminationRed = mark.IllumRed;
                    //    data.dPreAlignIlluminationExposureTime = mark.ExposureTime;
                    //    data.nPreCircleColor = mark.CircleColor;
                    //    data.dPreCircleMarkRadius = mark.CircleMarkRadius;
                    //    data.dPreCircleMarkSpec = mark.CircleMarkSpec;
                    //    data.dPreCircleMarkScore = mark.CircleMarkScore;
                    //    data.ePreAlgorithmType = mark.AlgorithmType;
                    //    data.ePreMarkType = mark.MarkType;
                    //    data.pointPreTrainImagePath = mark.TrainImagePath;
                    //}
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


                //if (preAlignCount == 0)
                //{
                //    PreAlignMarkInfo mark = new PreAlignMarkInfo();

                //    // 기존 단일 구조에서 값을 복사
                //    mark.PatternMatching = data.PrePatternMatching;

                //    mark.TrainRoiStart = data.pointPreTrainRoiStartLocation;
                //    mark.TrainRoiEnd = data.pointPreTrainRoiEndLocation;
                //    mark.InspectRoiStart = data.pointPreInspectRoiStartLocation;
                //    mark.InspectRoiEnd = data.pointPreInspectRoiEndLocation;

                //    mark.IllumIR = data.nPreIlluminationIR;
                //    mark.IllumRed = data.nPreIlluminationRed;
                //    mark.ExposureTime = data.dPreAlignIlluminationExposureTime;

                //    mark.CircleColor = data.nPreCircleColor;
                //    mark.CircleMarkRadius = data.dPreCircleMarkRadius;
                //    mark.CircleMarkSpec = data.dPreCircleMarkSpec;
                //    mark.CircleMarkScore = data.dPreCircleMarkScore;

                //    mark.AlgorithmType = data.ePreAlgorithmType;
                //    mark.MarkType = data.ePreMarkType;

                //    mark.TrainImagePath = data.pointPreTrainImagePath;
                //    if (string.IsNullOrEmpty(mark.TrainImagePath))
                //    {
                //        string folderName = Path.GetFileNameWithoutExtension(path);
                //        string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                //        Directory.CreateDirectory(folderPath);
                //        mark.TrainImagePath = Path.Combine(folderPath, "PreAlign.bmp");
                //    }

                //    data.PreAlignMarkList.Add(mark);
                //}

                //기존 코드
                {
                    //PreAlign
                    //NativeMethods.GetPrivateProfileString("PatternMatching", "MinScore", "0.7", sb, sb.Capacity, path);
                    //data.PrePatternMatching.MinScore = Equipment.ToDouble(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PatternMatching", "MaxInstance", "1", sb, sb.Capacity, path);
                    //data.PrePatternMatching.MaxInstance = Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PatternMatching", "MaxTolerance", "45", sb, sb.Capacity, path);
                    //data.PrePatternMatching.MaxTolerance = Equipment.ToDouble(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PatternMatching", "DuplicateChecked", "False", sb, sb.Capacity, path);
                    //data.PrePatternMatching.DuplicateChecked = Equipment.ToBoolean(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PatternMatching", "UseMaskImage", "False", sb, sb.Capacity, path);
                    //data.PrePatternMatching.UseMaskImage = Equipment.ToBoolean(sb.ToString());

                    //// ROI
                    //NativeMethods.GetPrivateProfileString("TrainROI", "StartX", "0", sb, sb.Capacity, path);
                    //data.pointPreTrainRoiStartLocation.X = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("TrainROI", "StartY", "0", sb, sb.Capacity, path);
                    //data.pointPreTrainRoiStartLocation.Y = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("TrainROI", "EndX", "0", sb, sb.Capacity, path);
                    //data.pointPreTrainRoiEndLocation.X = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("TrainROI", "EndY", "0", sb, sb.Capacity, path);
                    //data.pointPreTrainRoiEndLocation.Y = Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("InspectROI", "StartX", "0", sb, sb.Capacity, path);
                    //data.pointPreInspectRoiStartLocation.X = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("InspectROI", "StartY", "0", sb, sb.Capacity, path);
                    //data.pointPreInspectRoiStartLocation.Y = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("InspectROI", "EndX", "0", sb, sb.Capacity, path);
                    //data.pointPreInspectRoiEndLocation.X = Equipment.ToInt(sb.ToString());
                    //NativeMethods.GetPrivateProfileString("InspectROI", "EndY", "0", sb, sb.Capacity, path);
                    //data.pointPreInspectRoiEndLocation.Y = Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("Vision", "AlgorithmType", "1", sb, sb.Capacity, path);
                    //data.ePreAlgorithmType = (VisionAlgorithmType)Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("Vision", "PatternShape", "1", sb, sb.Capacity, path);
                    //data.ePreMarkType = (MarkTypeList)Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PreAlign_llumination", "IR", "3000", sb, sb.Capacity, path);
                    //data.nPreIlluminationIR = Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PreAlign_llumination", "Red", "0", sb, sb.Capacity, path);
                    //data.nPreIlluminationRed = Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("CircleDetection", "Color", "true", sb, sb.Capacity, path);
                    //data.nPreCircleColor = Equipment.ToInt(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("CircleDetection", "SizeW", "1.0", sb, sb.Capacity, path);
                    //data.dPreCircleMarkRadius = Equipment.ToDouble(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("CircleDetection", "Spec", "0.1", sb, sb.Capacity, path);
                    //data.dPreCircleMarkSpec = Equipment.ToDouble(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("CircleDetection", "Score", "0.7", sb, sb.Capacity, path);
                    //data.dPreCircleMarkScore = Equipment.ToDouble(sb.ToString());

                    //NativeMethods.GetPrivateProfileString("PreAlign_llumination", "ExposureTime", "20000", sb, sb.Capacity, path);
                    //data.dPreAlignIlluminationExposureTime = Equipment.ToDouble(sb.ToString());


                    //NativeMethods.GetPrivateProfileString("TrainImage", "Path", "", sb, sb.Capacity, path);
                    //data.pointPreTrainImagePath = sb.ToString();

                    //if (data.pointPreTrainImagePath == "")
                    //{
                    //    string folderName = Path.GetFileNameWithoutExtension(path);
                    //    string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                    //    Directory.CreateDirectory(folderPath); // 없으면 생성
                    //    string bmpPath = Path.Combine(folderPath, "PreAlign.bmp");  // BMP 저장
                    //    data.pointPreTrainImagePath = bmpPath;
                    //}
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
