using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Q_Config
{
    public class ScannerCalConfigData
    {
        public string ConfigPath { get; set; } = ScannerCalConfigPath.DefaultPath;

        public PatternMatchingParameters Scanner_Calibration_PatternMatchingParameters;//         //  Scanner Calibration Pattern Matching Parameters
        public BlobVisionToolParameter Scanner_Calibration_BlobVisionToolParameter;      //  Scanner Calibration Blob Vision Tool Parameter
        public double Scanner_Calibration_TrainRoiStartLocation_X;         //  Scanner Calibration Train Roi Start Location
        public double Scanner_Calibration_TrainRoiStartLocation_Y;
        public double Scanner_Calibration_TrainRoiEndLocation_X;           //  Scanner Calibration Train Roi End Location
        public double Scanner_Calibration_TrainRoiEndLocation_Y;
        public double Scanner_Calibration_InspectionRoiStartLocation_X;         //  Scanner Calibration Inspection Roi Location
        public double Scanner_Calibration_InspectionRoiStartLocation_Y;
        public double Scanner_Calibration_InspectionRoiEndLocation_X;           //  Scanner Calibration Inspection Roi Location
        public double Scanner_Calibration_InspectionRoiEndLocation_Y;
        public double Scanner_Calibration_LaserFrequency;            //  Scanner Calibration Laser Frequency
        public double Scanner_Calibration_LaserPulseWidth;          //  Scanner Calibration Laser Pulse Width
        public double Scanner_Calibration_LaserEnergy;               //  Scanner Calibration Laser Energy
        public double Scanner_Calibration_CrossMarkLength;           //  Scanner Calibration Cross Mark Length
        public double Scanner_Calibration_LaserMarkSpeed;            //  Scanner Calibration Laser Mark Speed (mm/s)
        public double Scanner_Calibration_LaserJumpSpeed;            //  Scanner Calibration Laser Jump Speed (mm/s)
        public double Scanner_Calibration_LaserOnDelay;                //  Scanner Calibration Laser On Delay (us)
        public double Scanner_Calibration_LaserOffDelay;               //  Scanner Calibration Laser Off Delay (us)
        public double Scanner_Calibration_MarkDelay;                  //  Scanner Calibration Mark Delay (us)
        public double Scanner_Calibration_JumpDelay;                  //  Scanner Calibration Jump Delay (us)
        public double Scanner_Calibration_PolygonDelay;               //  Scanner Calibration Polygon Delay (us)
        public double Scanner_Calibration_CalAreaWidth;
        public double Scanner_Calibration_CalAreaHeight;
        public double Scanner_Calibration_CalPitch;
        public double Scanner_VerifyCameraOffset_PosX_Last;
        public double Scanner_VerifyCameraOffset_PosY_Last;
        public int Scanner_Calibration_Illumination_Red_Value;
        public int Scanner_Calibration_Illumination_IR_Value;
        public int Scanner_Calibration_ExposureTime_High; //  Scanner Calibration Exposure Time High (ms)
        public bool Scanner_Calibration_UsePatternMatching; //  Scanner Calibration Use Pattern Matching (true: Use, false: Not Use)
        public bool Scanner_Calibration_UseBlobVisionTool;            //  Scanner Calibration Use Blob Vision Tool (true: Use, false: Not Use)
        public bool Scanner_Calibration_MarkType_Cross;
        public bool Scanner_Calibration_MarkType_Circlle;
        //Statu
        //  Scalibration Position Enable true: calPan, false: Stage Center
        public bool Scanner_Calibration_Position_Enable;            //  Scanner Calibration Position Enable (true: Enable, false: Disable)
        public bool Scanner_Calibration_Change;
        public int Scanner_Calibration_Convert;            //  Scanner Calibration Use (true: Use, false: Not Use)
        public bool Scanner_Vision_Offset_Setting_Use;            //  Scanner Calibration Use (true: Use, false: Not Use)
        public double Scanner_Vision_Offset_Setting_X;            //  Scanner Calibration OffsetX(mm) (X축 Offset)
        public double Scanner_Vision_Offset_Setting_Y;            //  Scanner Calibration OffsetY(mm) (Y축 Offset)
        public double Scanner_Calibration_VisionZOffset;
        public double Scanner_Calibration_VarioScanZ;   
        public double Scanner_Calibration_VarioScanZ_Defocus;
        //  Scalibration RTC 및 구동 변수
        public string Scanner_Calibration_srcFilePath;            //  Scanner Calibration Source File Path
        public string Scanner_Calibration_targetFilePath;            //  Scanner Calibration Destination File Path
        public double Scanner_Calibration_FieldSize;            //  Scanner Calibration Field Size (mm)
        public double Scanner_Calibration_rowInterval;
        public double Scanner_Calibration_colInterval;
        public int Scanner_Calibration_rowCount;
        public int Scanner_Calibration_colCount;

        public ScannerCalConfigData()
        {
            PatternMatchingParameters Scanner_Calibration_PatternMatchingParameters = new PatternMatchingParameters();         //  Scanner Calibration Pattern Matching Parameters
            BlobVisionToolParameter Scanner_Calibration_BlobVisionToolParameter = new BlobVisionToolParameter();
            Scanner_Calibration_TrainRoiStartLocation_X = 0.0;         //  Scanner Calibration Train Roi Start Location
            Scanner_Calibration_TrainRoiStartLocation_Y = 0.0;
            Scanner_Calibration_TrainRoiEndLocation_X = 0.0;           //  Scanner Calibration Train Roi End Location
            Scanner_Calibration_TrainRoiEndLocation_Y = 0.0;
            Scanner_Calibration_InspectionRoiStartLocation_X = 0.0;         //  Scanner Calibration Inspection Roi Location
            Scanner_Calibration_InspectionRoiStartLocation_Y = 0.0;
            Scanner_Calibration_InspectionRoiEndLocation_X = 0.0;           //  Scanner Calibration Inspection Roi Location
            Scanner_Calibration_InspectionRoiEndLocation_Y = 0.0;
            Scanner_Calibration_LaserFrequency = 0.0;            //  Scanner Calibration Laser Frequency
            Scanner_Calibration_LaserPulseWidth = 0.0;          //  Scanner Calibration Laser Pulse Width
            Scanner_Calibration_LaserEnergy = 0.0;               //  Scanner Calibration Laser Energy
            Scanner_Calibration_CrossMarkLength = 0.0;           //  Scanner Calibration Cross Mark Length
            Scanner_Calibration_LaserMarkSpeed = 0.0;            //  Scanner Calibration Laser Mark Speed (mm/s)
            Scanner_Calibration_LaserJumpSpeed = 0.0;            //  Scanner Calibration Laser Jump Speed (mm/s)
            Scanner_Calibration_LaserOnDelay = 0.0;                //  Scanner Calibration Laser On Delay (us)
            Scanner_Calibration_LaserOffDelay = 0.0;               //  Scanner Calibration Laser Off Delay (us)
            Scanner_Calibration_MarkDelay = 0.0;                  //  Scanner Calibration Mark Delay (us)
            Scanner_Calibration_JumpDelay = 0.0;                  //  Scanner Calibration Jump Delay (us)
            Scanner_Calibration_PolygonDelay = 0.0;               //  Scanner Calibration Polygon Delay (us)
            Scanner_Calibration_CalAreaWidth = 0.0;
            Scanner_Calibration_CalAreaHeight = 0.0;
            Scanner_Calibration_CalPitch = 0.0;
            Scanner_VerifyCameraOffset_PosX_Last = 0.0;
            Scanner_VerifyCameraOffset_PosY_Last = 0.0;
            Scanner_Calibration_Illumination_Red_Value = 0;
            Scanner_Calibration_Illumination_IR_Value = 0;
            Scanner_Calibration_ExposureTime_High = 0;
            Scanner_Calibration_UsePatternMatching = false; //  Scanner Calibration Use Pattern Matching (true: Use, false: Not Use)
            Scanner_Calibration_UseBlobVisionTool = false;            //  Scanner Calibration Use Blob Vision Tool (true: Use, false: Not Use)
            Scanner_Calibration_MarkType_Cross = false;
            Scanner_Calibration_MarkType_Circlle = false;
            Scanner_Calibration_Position_Enable = false;            //  Scanner Calibration Position Enable (true: Enable, false: Disable)
            Scanner_Calibration_Change = false;
            Scanner_Calibration_Convert = 0;            //  Scanner Calibration Use (true: Use, false: Not Use)
            Scanner_Vision_Offset_Setting_Use = false;            //  Scanner Calibration Use (true: Use, false: Not Use)
            Scanner_Vision_Offset_Setting_X = 0.0;            //  Scanner Calibration OffsetX(mm) (X축 Offset)
            Scanner_Vision_Offset_Setting_Y = 0.0;            //  Scanner Calibration OffsetY(mm) (Y축 Offset)
            Scanner_Calibration_VisionZOffset = 0.0;
            Scanner_Calibration_VarioScanZ = 0.0;
            Scanner_Calibration_VarioScanZ_Defocus = 0.0;
            Scanner_Calibration_srcFilePath = string.Empty;            //  Scanner Calibration Source File Path
            Scanner_Calibration_targetFilePath = string.Empty;            //  Scanner Calibration Destination File Path
            Scanner_Calibration_FieldSize = 0.0;            //  Scanner Calibration Field Size (mm)
            Scanner_Calibration_rowInterval = 0.0;
            Scanner_Calibration_colInterval = 0.0;
            Scanner_Calibration_rowCount = 0;
            Scanner_Calibration_colCount = 0;
        }

        public static class ScannerCalConfigPath
        {
            public static string DefaultPath => System.IO.Path.Combine(ConfigManager.GetConfigPath(), "MachineScannerCalibrationNew (Do not delete or modify).ini");
        }

        public bool SaveToIni(string path = null)
        {
            string savePath = path ?? ConfigPath;

            try
            {
                NativeMethods.WritePrivateProfileString("ScannerCal", "UsePatternMatching", Scanner_Calibration_UsePatternMatching.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "UseBlobTool", Scanner_Calibration_UseBlobVisionTool.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "MarkType_Cross", Scanner_Calibration_MarkType_Cross.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "MarkType_Circle", Scanner_Calibration_MarkType_Circlle.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "CrossMarkLength", Scanner_Calibration_CrossMarkLength.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "TrainRoiStartX", Scanner_Calibration_TrainRoiStartLocation_X.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "TrainRoiStartY", Scanner_Calibration_TrainRoiStartLocation_Y.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "TrainRoiEndX", Scanner_Calibration_TrainRoiEndLocation_X.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "TrainRoiEndY", Scanner_Calibration_TrainRoiEndLocation_Y.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "InspectRoiStartX", Scanner_Calibration_InspectionRoiStartLocation_X.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "InspectRoiStartY", Scanner_Calibration_InspectionRoiStartLocation_Y.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "InspectRoiEndX", Scanner_Calibration_InspectionRoiEndLocation_X.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("ScannerCal", "InspectRoiEndY", Scanner_Calibration_InspectionRoiEndLocation_Y.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("Laser", "Freq", Scanner_Calibration_LaserFrequency.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "PulseWidth", Scanner_Calibration_LaserPulseWidth.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "Energy", Scanner_Calibration_LaserEnergy.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "MarkSpeed", Scanner_Calibration_LaserMarkSpeed.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "JumpSpeed", Scanner_Calibration_LaserJumpSpeed.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "OnDelay", Scanner_Calibration_LaserOnDelay.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "OffDelay", Scanner_Calibration_LaserOffDelay.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "MarkDelay", Scanner_Calibration_MarkDelay.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "JumpDelay", Scanner_Calibration_JumpDelay.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Laser", "PolygonDelay", Scanner_Calibration_PolygonDelay.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("Area", "Width", Scanner_Calibration_CalAreaWidth.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Area", "Height", Scanner_Calibration_CalAreaHeight.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Area", "Pitch", Scanner_Calibration_CalPitch.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("Position", "LastX", Scanner_VerifyCameraOffset_PosX_Last.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Position", "LastY", Scanner_VerifyCameraOffset_PosY_Last.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Position", "VisionOffsetX", Scanner_Vision_Offset_Setting_X.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Position", "VisionOffsetY", Scanner_Vision_Offset_Setting_Y.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Position", "VisionOffsetZ", Scanner_Calibration_VisionZOffset.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Position", "VarioScanZ", Scanner_Calibration_VarioScanZ.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Position", "VarioScanZ_Defocus", Scanner_Calibration_VarioScanZ_Defocus.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("Position", "Enable", Scanner_Calibration_Position_Enable.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("Illumination", "Ch1", Scanner_Calibration_Illumination_Red_Value.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Illumination", "Ch2", Scanner_Calibration_Illumination_IR_Value.ToString(), savePath);
                //Scanner_Calibration_ExposureTime_High
                NativeMethods.WritePrivateProfileString("Illumination", "ExposureTime_High", Scanner_Calibration_ExposureTime_High.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("File", "SourcePath", Scanner_Calibration_srcFilePath, savePath);
                NativeMethods.WritePrivateProfileString("File", "TargetPath", Scanner_Calibration_targetFilePath, savePath);

                NativeMethods.WritePrivateProfileString("Grid", "FieldSize", Scanner_Calibration_FieldSize.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Grid", "RowInterval", Scanner_Calibration_rowInterval.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Grid", "ColInterval", Scanner_Calibration_colInterval.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Grid", "RowCount", Scanner_Calibration_rowCount.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Grid", "ColCount", Scanner_Calibration_colCount.ToString(), savePath);

                NativeMethods.WritePrivateProfileString("Flags", "Calibration_Change", Scanner_Calibration_Change.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Flags", "Calibration_Convert", Scanner_Calibration_Convert.ToString(), savePath);
                NativeMethods.WritePrivateProfileString("Flags", "OffsetSettingUse", Scanner_Vision_Offset_Setting_Use.ToString(), savePath);

                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        public static ScannerCalConfigData LoadFromIni(string path = null)
        {
            string loadPath = path ?? ScannerCalConfigPath.DefaultPath;
            var data = new ScannerCalConfigData();
            StringBuilder sb = new StringBuilder(255);

            string Read(string section, string key, string defaultVal)
            {
                NativeMethods.GetPrivateProfileString(section, key, defaultVal, sb, sb.Capacity, loadPath);
                return sb.ToString();
            }

            try
            {
                // 이미 생성자에서 초기화했기 때문에 중복은 아니지만, 이중 확인 가능
                if (data.Scanner_Calibration_PatternMatchingParameters == null)
                    data.Scanner_Calibration_PatternMatchingParameters = new PatternMatchingParameters();

                if (data.Scanner_Calibration_BlobVisionToolParameter == null)
                    data.Scanner_Calibration_BlobVisionToolParameter = new BlobVisionToolParameter();

                data.Scanner_Calibration_UsePatternMatching = Equipment.ToBoolean(Read("ScannerCal", "UsePatternMatching", "False"));
                data.Scanner_Calibration_UseBlobVisionTool = Equipment.ToBoolean(Read("ScannerCal", "UseBlobTool", "False"));
                data.Scanner_Calibration_MarkType_Cross = Equipment.ToBoolean(Read("ScannerCal", "MarkType_Cross", "False"));
                data.Scanner_Calibration_MarkType_Circlle = Equipment.ToBoolean(Read("ScannerCal", "MarkType_Circle", "False"));
                data.Scanner_Calibration_CrossMarkLength = Equipment.ToDouble(Read("ScannerCal", "CrossMarkLength", "0"));

                data.Scanner_Calibration_TrainRoiStartLocation_X = Equipment.ToDouble(Read("ScannerCal", "TrainRoiStartX", "0"));
                data.Scanner_Calibration_TrainRoiStartLocation_Y = Equipment.ToDouble(Read("ScannerCal", "TrainRoiStartY", "0"));
                data.Scanner_Calibration_TrainRoiEndLocation_X = Equipment.ToDouble(Read("ScannerCal", "TrainRoiEndX", "0"));
                data.Scanner_Calibration_TrainRoiEndLocation_Y = Equipment.ToDouble(Read("ScannerCal", "TrainRoiEndY", "0"));

                data.Scanner_Calibration_InspectionRoiStartLocation_X = Equipment.ToDouble(Read("ScannerCal", "InspectRoiStartX", "0"));
                data.Scanner_Calibration_InspectionRoiStartLocation_Y = Equipment.ToDouble(Read("ScannerCal", "InspectRoiStartY", "0"));
                data.Scanner_Calibration_InspectionRoiEndLocation_X = Equipment.ToDouble(Read("ScannerCal", "InspectRoiEndX", "0"));
                data.Scanner_Calibration_InspectionRoiEndLocation_Y = Equipment.ToDouble(Read("ScannerCal", "InspectRoiEndY", "0"));

                data.Scanner_Calibration_LaserFrequency = Equipment.ToDouble(Read("Laser", "Freq", "0"));
                data.Scanner_Calibration_LaserPulseWidth = Equipment.ToDouble(Read("Laser", "PulseWidth", "0"));
                data.Scanner_Calibration_LaserEnergy = Equipment.ToDouble(Read("Laser", "Energy", "0"));
                data.Scanner_Calibration_LaserMarkSpeed = Equipment.ToDouble(Read("Laser", "MarkSpeed", "0"));
                data.Scanner_Calibration_LaserJumpSpeed = Equipment.ToDouble(Read("Laser", "JumpSpeed", "0"));
                data.Scanner_Calibration_LaserOnDelay = Equipment.ToDouble(Read("Laser", "OnDelay", "0"));
                data.Scanner_Calibration_LaserOffDelay = Equipment.ToDouble(Read("Laser", "OffDelay", "0"));
                data.Scanner_Calibration_MarkDelay = Equipment.ToDouble(Read("Laser", "MarkDelay", "0"));
                data.Scanner_Calibration_JumpDelay = Equipment.ToDouble(Read("Laser", "JumpDelay", "0"));
                data.Scanner_Calibration_PolygonDelay = Equipment.ToDouble(Read("Laser", "PolygonDelay", "0"));

                data.Scanner_Calibration_CalAreaWidth = Equipment.ToDouble(Read("Area", "Width", "0"));
                data.Scanner_Calibration_CalAreaHeight = Equipment.ToDouble(Read("Area", "Height", "0"));
                data.Scanner_Calibration_CalPitch = Equipment.ToDouble(Read("Area", "Pitch", "0"));

                data.Scanner_VerifyCameraOffset_PosX_Last = Equipment.ToDouble(Read("Position", "LastX", "0"));
                data.Scanner_VerifyCameraOffset_PosY_Last = Equipment.ToDouble(Read("Position", "LastY", "0"));
                data.Scanner_Vision_Offset_Setting_X = Equipment.ToDouble(Read("Position", "VisionOffsetX", "0"));
                data.Scanner_Vision_Offset_Setting_Y = Equipment.ToDouble(Read("Position", "VisionOffsetY", "0"));
                data.Scanner_Calibration_VisionZOffset = Equipment.ToDouble(Read("Position", "VisionOffsetZ", "0"));
                data.Scanner_Calibration_VarioScanZ = Equipment.ToDouble(Read("Position", "VarioScanZ", "0"));
                data.Scanner_Calibration_VarioScanZ_Defocus = Equipment.ToDouble(Read("Position", "VarioScanZ_Defocus", "0"));
                data.Scanner_Calibration_Position_Enable = Equipment.ToBoolean(Read("Position", "Enable", "False"));

                data.Scanner_Calibration_Illumination_Red_Value = Equipment.ToInt(Read("Illumination", "Ch1", "0"));
                data.Scanner_Calibration_Illumination_IR_Value = Equipment.ToInt(Read("Illumination", "Ch2", "0"));
                //Scanner_Calibration_ExposureTime_High
                data.Scanner_Calibration_ExposureTime_High = Equipment.ToInt(Read("Illumination", "ExposureTime_High", "0"));

                data.Scanner_Calibration_srcFilePath = Read("File", "SourcePath", "");
                data.Scanner_Calibration_targetFilePath = Read("File", "TargetPath", "");

                data.Scanner_Calibration_FieldSize = Equipment.ToDouble(Read("Grid", "FieldSize", "0"));
                data.Scanner_Calibration_rowInterval = Equipment.ToDouble(Read("Grid", "RowInterval", "0"));
                data.Scanner_Calibration_colInterval = Equipment.ToDouble(Read("Grid", "ColInterval", "0"));
                data.Scanner_Calibration_rowCount = Equipment.ToInt(Read("Grid", "RowCount", "0"));
                data.Scanner_Calibration_colCount = Equipment.ToInt(Read("Grid", "ColCount", "0"));

                data.Scanner_Calibration_Change = Equipment.ToBoolean(Read("Flags", "Calibration_Change", "False"));
                data.Scanner_Calibration_Convert = Equipment.ToInt(Read("Flags", "Calibration_Convert", "0"));
                data.Scanner_Vision_Offset_Setting_Use = Equipment.ToBoolean(Read("Flags", "OffsetSettingUse", "False"));

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return data;
        }

    }
}
