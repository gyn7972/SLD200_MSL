using QMC.Common.Hmi;
using QMC.Common.Parts;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    [Serializable]
    public class VisionConfig
    {
        #region Define

        public enum PositionKeys
        {
            Vision_Image_Focus_AxisZ,           //  비전 카메라의 초점 위치
            Laser_Focus_AxisZ,                  //  레이저의 초점 위치
            Safety_AxisZ,                       //  안전 위치
        }
        #endregion

        #region Property
        //public RealTimeScannerConfig RealTimeScannerConfig { set; get; }
        public List<XyzyPositionData> Positions { set; get; }
        static public XyzPositionDataCollection DispenserPositions { set; get; }
        public HIKGigECameraConfig CameraConfig_LowRes { set; get; }
        public HIKGigECameraConfig CameraConfig_HighRes { set; get; }        
        //public GrabLinkMultiCamCameraConfig CameraConfig { set; get; }
        //public GrabLinkMultiCamCameraConfig CameraConfig_LowRes { set; get; }
        public List<IlluminationChannel> ListIlluminationChannel { set; get; }
        public VisionCalibratorConfig VisonCalibratorConfig_LowRes { set; get; }
        public VisionCalibratorConfig VisonCalibratorConfig_HighRes { set; get; }
        public VisionCompensatorConfig VisionCompensatorConfig { set; get; }
        public ScannerCompensatorConfig ScannerCompensatorConfig { set; get; }
        //public JigAlignerConfig JigAlignerConfig { set; get; }
        public AutoFocuserConfig AutoFocuserConfig_LowRes { set; get; }
        public AutoFocuserConfig AutoFocuserConfig_HighRes { set; get; }
        public VisionParameterConfig ParamConfig { set; get; }
        public LaserPitchMoveShotterConfig LaserPitchMoveShotterConfig { set; get; }

        public XyzyStageConfig StageConfig { set; get; }
        #endregion

        #region Consturctor
        public VisionConfig()
        {
            Positions = new List<XyzyPositionData>();

            if (Positions.Count != Enum.GetValues(typeof(PositionKeys)).Length)
            {
                foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
                {
                    bool bFind = false;
                    foreach (XyzyPositionData position in Positions)
                    {
                        if (position.Name == key.ToString())
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        XyzyPositionData positionBase = new XyzyPositionData();

                        positionBase.Name = key.ToString();
                        //To DO : positiondata에 tag추가 ? (목적 : base, offset에 이름 표시는 안하고 Tag로 구분을 위해)

                        Positions.Add(positionBase);

                        XyzyPositionData positionTarget = new XyzyPositionData();

                        //positionTarget.Name = key.ToString();
                        positionTarget.Type = TargetType.Offset;
                        Positions.Add(positionTarget);
                    }
                }
            }

            Init();
        }
        #endregion

        #region Method
        public void Init()
        {
            if (CameraConfig_LowRes == null)
                CameraConfig_LowRes = new HIKGigECameraConfig();
                //CameraConfig = new GrabLinkMultiCamCameraConfig();

            if (CameraConfig_HighRes == null)                                        //  저해상도 카메라 추가
                CameraConfig_HighRes = new HIKGigECameraConfig();
                //CameraConfig_LowRes = new GrabLinkMultiCamCameraConfig();

            if (ListIlluminationChannel == null)
                ListIlluminationChannel = new List<IlluminationChannel>();

            if (VisonCalibratorConfig_LowRes == null)
                VisonCalibratorConfig_LowRes = new VisionCalibratorConfig();
                VisonCalibratorConfig_LowRes.Init();

            if (VisonCalibratorConfig_HighRes == null)
                VisonCalibratorConfig_HighRes = new VisionCalibratorConfig();
                VisonCalibratorConfig_HighRes.Init();

            if (VisionCompensatorConfig == null)
                VisionCompensatorConfig = new VisionCompensatorConfig();

            if (AutoFocuserConfig_LowRes == null)
                AutoFocuserConfig_LowRes = new AutoFocuserConfig();
                AutoFocuserConfig_LowRes.Init();

            if (AutoFocuserConfig_HighRes == null)
                AutoFocuserConfig_HighRes = new AutoFocuserConfig();
                AutoFocuserConfig_HighRes.Init();

            if (ParamConfig == null)
                ParamConfig = new VisionParameterConfig();

            if (ScannerCompensatorConfig == null)
                ScannerCompensatorConfig = new ScannerCompensatorConfig();
                ScannerCompensatorConfig.Init();

            if (LaserPitchMoveShotterConfig == null)
                LaserPitchMoveShotterConfig = new LaserPitchMoveShotterConfig();
                LaserPitchMoveShotterConfig.Init();

            if (StageConfig == null)
            {
                StageConfig = new XyzyStageConfig();
            }
            StageConfig.Init();

            //if (JigAlignerConfig == null)
            //    JigAlignerConfig = new JigAlignerConfig();
            //JigAlignerConfig.Init();

            ParamConfig.ConfigVisionPositions = Positions;
        }

        //public void SetPositionData(string strPosition, TargetType targetType, XyztCoordinate coordinate)
        //{
        //    foreach (XyztPositionData position in Positions)
        //    {
        //        if (position.Name == strPosition && position.Type == targetType)
        //        {
        //            position.X = coordinate.X;
        //            position.Y = coordinate.Y;
        //            position.Z = coordinate.Z;
        //            position.T = coordinate.T;
        //            break;
        //        }
        //    }
        //}

        //public void SetPositionData(string strPosition, TargetType targetType, UvwzxyzCoordinate coordinate)
        //{
        //    foreach (UvwzxyzPositionData position in Positions)
        //    {
        //        if (position.Name == strPosition && position.Type == targetType)
        //        {
        //            position.U = coordinate.U;
        //            position.V = coordinate.V;
        //            position.W = coordinate.W;
        //            position.EZ = coordinate.EZ;
        //            position.X = coordinate.X;
        //            position.Y = coordinate.Y;
        //            position.VZ = coordinate.VZ;
        //            break;
        //        }
        //    }
        //}

        public void SetPositionData(string strPosition, TargetType targetType, XyzyCoordinate coordinate)
        {
            foreach (XyzyPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.Z = coordinate.Z;
                    position.MASK_Y = coordinate.MASK_Y;
                    break;
                }
            }
        }

        //public XyztCoordinate GetPositionData(string strPosition, TargetType targetType)
        //{
        //    XyztCoordinate coordinate = new XyztCoordinate();

        //    foreach (XyztPositionData position in Positions)
        //    {
        //        if (position.Name == strPosition && position.Type == targetType)
        //        {
        //            coordinate.X = position.X;
        //            coordinate.Y = position.Y;
        //            coordinate.Z = position.Z;
        //            coordinate.T = position.T;
        //            break;
        //        }
        //    }

        //    return coordinate;
        //}

        //public UvwzxyzCoordinate GetPositionData(string strPosition, TargetType targetType)
        //{
        //    UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();

        //    foreach (UvwzxyzPositionData position in Positions)
        //    {
        //        if (position.Name == strPosition && position.Type == targetType)
        //        {
        //            coordinate.U = position.U;
        //            coordinate.V = position.V;
        //            coordinate.W = position.W;
        //            coordinate.EZ = position.EZ;
        //            coordinate.X = position.X;
        //            coordinate.Y = position.Y;
        //            coordinate.VZ = position.VZ;
        //            break;
        //        }
        //    }

        //    return coordinate;
        //}

        public XyzyCoordinate GetPositionData(string strPosition, TargetType targetType)
        {
            XyzyCoordinate coordinate = new XyzyCoordinate();

            foreach (XyzyPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    coordinate.X = position.X;
                    coordinate.Y = position.Y;
                    coordinate.Z = position.Z;
                    coordinate.MASK_Y = position.MASK_Y;
                    break;
                }
            }

            return coordinate;
        }
        #endregion
    }
}
