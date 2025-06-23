using QMC.Common.Vision;
using QMC.Common.VisionPart;
using QMC.Core;
using System;
using System.Collections.Generic;
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
        public List<SocketMarkInfo> SocketMarkList { get; private set; } = new List<SocketMarkInfo>();

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

        //기존코드
        //{
        //public int nSocketAlignType;                 //  Fiducial Align Type (0:Circle Find, 2:Pattern Matching)
        //public int nSocketMarkType;                  //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
        //public int nSocketCircleColor;               //  0: White, 1: Black, 2: Ignore
        //public double dSocketCircleMarkRadius;                  //  Fiducial Mark Size (mm)
        //public double dSocketCircleMarkSpec;                  //  Fiducial Mark Spec
        //public double dSocketCircleMarkScore;         //circle score
        //public int nSocketIlluminationIR;
        //public int nSocketIlluminationRed;
        //public bool bSocketIlluminationIRUse;
        //public bool bSocketIlluminationRedUse;
        //public double dSocketIlluminationExposureTime;
        //public double dSocketAxisZ_Offset;
        //}


        //PreAlign
        public PatternMatchingParameters PrePatternMatching;
        public System.Drawing.Point pointPreTrainRoiStartLocation;
        public System.Drawing.Point pointPreTrainRoiEndLocation;
        public System.Drawing.Point pointPreInspectRoiStartLocation;
        public System.Drawing.Point pointPreInspectRoiEndLocation;
        public int nPreIlluminationIR;
        public int nPreIlluminationRed;
        public string pointPreTrainImagePath;

        public int nPreCircleColor;  //0: White, 1: Black, 2: Ignore
        public double dPreCircleMarkRadius; //circle size width
        public double dPreCircleMarkSpec;  //
        public double dPreCircleMarkScore; //circle score
        public VisionAlgorithmType ePreAlgorithmType;
        public MarkTypeList ePreMarkType;
        public double dPreAlignIlluminationExposureTime;

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
            // 기존 코드 호환을 위한 속성 매핑 (SocketMarkList[0] 기준)
            //NativeMethods.WritePrivateProfileString("SocketAlign", "Aligntype", nSocketAlignType.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "MarkType", nSocketMarkType.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "MarkColor", nSocketCircleColor.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "MarkSize", dSocketCircleMarkRadius.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "MarkSpec", dSocketCircleMarkSpec.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "MarkScore", dSocketCircleMarkScore.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "IR", nSocketIlluminationIR.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "Red", nSocketIlluminationRed.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "IRUse", bSocketIlluminationIRUse.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "RedUse", bSocketIlluminationRedUse.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "ExposureTime", dSocketIlluminationExposureTime.ToString(), path);
            //NativeMethods.WritePrivateProfileString("SocketAlign", "AxisZ_Offset", dSocketAxisZ_Offset.ToString(), path);


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
                NativeMethods.WritePrivateProfileString("PreAlign_llumination", "Red", nPreIlluminationRed.ToString(), path);

                NativeMethods.WritePrivateProfileString("CircleDetection", "Color", nPreCircleColor.ToString(), path);
                NativeMethods.WritePrivateProfileString("CircleDetection", "SizeW", dPreCircleMarkRadius.ToString(), path);
                NativeMethods.WritePrivateProfileString("CircleDetection", "Spec", dPreCircleMarkSpec.ToString(), path);
                NativeMethods.WritePrivateProfileString("CircleDetection", "Score", dPreCircleMarkScore.ToString(), path);
                NativeMethods.WritePrivateProfileString("PreAlign_llumination", "ExposureTime", dPreAlignIlluminationExposureTime.ToString(), path);

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

            return bRet;
        }

        public static VisionRecipeData LoadFromIni(string path)
        {
            VisionRecipeData data = new VisionRecipeData();
            data.PrePatternMatching = new PatternMatchingParameters();
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

                    NativeMethods.GetPrivateProfileString("SocketAlign", "AlignType", "0", sb, sb.Capacity, path);
                    mark.AlignType = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkType", "0", sb, sb.Capacity, path);
                    mark.MarkType = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkColor", "0", sb, sb.Capacity, path);
                    mark.MarkColor = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkRadius", "0.0", sb, sb.Capacity, path);
                    mark.MarkRadius = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSpec", "0.0", sb, sb.Capacity, path);
                    mark.MarkSpec = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "MarkScore", "0.0", sb, sb.Capacity, path);
                    mark.MarkScore = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "IllumRed", "0", sb, sb.Capacity, path);
                    mark.IllumRed = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "IllumIR", "0", sb, sb.Capacity, path);
                    mark.IllumIR = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "UseRed", "False", sb, sb.Capacity, path);
                    mark.UseRed = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "UseIR", "False", sb, sb.Capacity, path);
                    mark.UseIR = Equipment.ToBoolean(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "ExposureTime", "0.0", sb, sb.Capacity, path);
                    mark.ExposureTime = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("SocketAlign", "AxisZOffset", "0.0", sb, sb.Capacity, path);
                    mark.AxisZOffset = Equipment.ToDouble(sb.ToString());

                    data.SocketMarkList.Add(mark);
                }

                //// SocketAlign //기존 코드
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

                NativeMethods.GetPrivateProfileString("PreAlign_llumination", "Red", "0", sb, sb.Capacity, path);
                data.nPreIlluminationRed = Equipment.ToInt(sb.ToString());

                NativeMethods.GetPrivateProfileString("CircleDetection", "Color", "true", sb, sb.Capacity, path);
                data.nPreCircleColor = Equipment.ToInt(sb.ToString());

                NativeMethods.GetPrivateProfileString("CircleDetection", "SizeW", "1.0", sb, sb.Capacity, path);
                data.dPreCircleMarkRadius = Equipment.ToDouble(sb.ToString());

                NativeMethods.GetPrivateProfileString("CircleDetection", "Spec", "0.1", sb, sb.Capacity, path);
                data.dPreCircleMarkSpec = Equipment.ToDouble(sb.ToString());

                NativeMethods.GetPrivateProfileString("CircleDetection", "Score", "0.7", sb, sb.Capacity, path);
                data.dPreCircleMarkScore = Equipment.ToDouble(sb.ToString());

                NativeMethods.GetPrivateProfileString("PreAlign_llumination", "ExposureTime", "20000", sb, sb.Capacity, path);
                data.dPreAlignIlluminationExposureTime = Equipment.ToDouble(sb.ToString());


                NativeMethods.GetPrivateProfileString("TrainImage", "Path", "", sb, sb.Capacity, path);
                data.pointPreTrainImagePath = sb.ToString();

                if (data.pointPreTrainImagePath == "")
                {
                    string folderName = Path.GetFileNameWithoutExtension(path);
                    string folderPath = Path.Combine(Path.GetDirectoryName(path), folderName);
                    Directory.CreateDirectory(folderPath); // 없으면 생성
                    string bmpPath = Path.Combine(folderPath, "PreAlign.bmp");  // BMP 저장
                    data.pointPreTrainImagePath = bmpPath;
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
