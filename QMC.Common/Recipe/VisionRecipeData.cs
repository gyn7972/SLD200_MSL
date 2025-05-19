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
        public int nSocketAlignType;                 //  Fiducial Align Type (0:Circle Find, 2:Pattern Matching)
        public int nSocketMarkType;                  //  Fiducial Mark Type (0:Circle, 1:Gold Powder)

        /// <summary>
        /// Mark Color  0: White, 1: Black, 2: Ignore
        /// </summary>
        public int nSocketCircleColor;               //  0: White, 1: Black, 2: Ignore
        public double dSocketCircleMarkRadius;                  //  Fiducial Mark Size (mm)
        public double dSocketCircleMarkSpec;                  //  Fiducial Mark Spec
        public double dSocketCircleMarkScore;         //circle score
        public int nSocketIlluminationIR;
        public int nSocketIlluminationRed;
        public bool bSocketIlluminationIRUse;
        public bool bSocketIlluminationRedUse;
        public double dSocketIlluminationExposureTime;
        public double dSocketAxisZ_Offset;

        //PreAlign
        public PatternMatchingParameters PrePatternMatching;
        public System.Drawing.Point pointPreTrainRoiStartLocation;
        public System.Drawing.Point pointPreTrainRoiEndLocation;
        public System.Drawing.Point pointPreInspectRoiStartLocation;
        public System.Drawing.Point pointPreInspectRoiEndLocation;
        public int nPreIlluminationIR;
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

            NativeMethods.WritePrivateProfileString("SocketAlign", "Aligntype", nSocketAlignType.ToString(), path);
            NativeMethods.WritePrivateProfileString("SocketAlign", "MarkType", nSocketMarkType.ToString(), path);

            NativeMethods.WritePrivateProfileString("SocketAlign", "MarkColor", nSocketCircleColor.ToString(), path);
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
                // SocketAlign
                NativeMethods.GetPrivateProfileString("SocketAlign", "Aligntype", "1", sb, sb.Capacity, path);
                data.nSocketAlignType = 1;  // Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("SocketAlign", "MarkType", "0", sb, sb.Capacity, path);
                data.nSocketMarkType = 0; // Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("SocketAlign", "MarkColor", "true", sb, sb.Capacity, path);
                data.nSocketCircleColor = Equipment.ToInt(sb.ToString());

                NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSize", "0.5", sb, sb.Capacity, path);
                data.dSocketCircleMarkRadius = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("SocketAlign", "MarkSpec", "0.05", sb, sb.Capacity, path);
                data.dSocketCircleMarkSpec = Equipment.ToDouble(sb.ToString());
                NativeMethods.GetPrivateProfileString("SocketAlign", "MarkScore", "0.7", sb, sb.Capacity, path);
                data.dSocketCircleMarkScore = Equipment.ToDouble(sb.ToString());

                NativeMethods.GetPrivateProfileString("SocketAlign", "IR", "250", sb, sb.Capacity, path);
                data.nSocketIlluminationIR = Equipment.ToInt(sb.ToString());
                NativeMethods.GetPrivateProfileString("SocketAlign", "Red", "0", sb, sb.Capacity, path);
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
