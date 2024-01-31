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
    public class WaferProbeAlignConfig
    {
        #region Define

        public enum PositionKeys
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
        #endregion

        #region Property
        //public RealTimeScannerConfig RealTimeScannerConfig { set; get; }
        //public List<XyztPositionData> Positions { set; get; }
        //public List<XyzztPositionData> Positions { set; get; }
        public List<UvwzxyzPositionData> Positions { set; get; }
        static public XyzPositionDataCollection DispenserPositions { set; get; }
        public HIKGigECameraConfig CameraConfig_Upper { set; get; }
        public HIKGigECameraConfig CameraConfig_Lower { set; get; }
        //public GrabLinkMultiCamCameraConfig CameraConfig { set; get; }
        //public GrabLinkMultiCamCameraConfig CameraConfig_LowRes { set; get; }
        public List<IlluminationChannel> ListIlluminationChannel { set; get; }
        public VisionCalibratorConfig VisonCalibratorConfig_Upper { set; get; }
        public VisionCalibratorConfig VisonCalibratorConfig_Lower { set; get; }
        public VisionCompensatorConfig VisionCompensatorConfig { set; get; }
        public ScannerCompensatorConfig ScannerCompensatorConfig { set; get; }
        //public JigAlignerConfig JigAlignerConfig { set; get; }
        public AutoFocuserConfig AutoFocuserConfig_Upper { set; get; }
        public AutoFocuserConfig AutoFocuserConfig_Lower { set; get; }
        public WaferProbeAlignParameterConfig ParamConfig { set; get; }
        public LaserPitchMoveShotterConfig LaserPitchMoveShotterConfig { set; get; }
        //public XyztStageConfig StageConfig { set; get; }
        //public XyzztStageConfig StageConfig { set; get; }
        public UvwzxyzStageConfig StageConfig { set; get; }
        #endregion

        #region Consturctor
        public WaferProbeAlignConfig()
        {
            //Positions = new List<XyztPositionData>();
            //Positions = new List<XyzztPositionData>();
            Positions = new List<UvwzxyzPositionData>();

            if (Positions.Count != Enum.GetValues(typeof(PositionKeys)).Length)
            {
                foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
                {
                    bool bFind = false;
                    //foreach (XyztPositionData position in Positions)
                    //foreach (XyzztPositionData position in Positions)
                    foreach (UvwzxyzPositionData position in Positions)
                    {
                        if (position.Name == key.ToString())
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        //XyztPositionData positionBase = new XyztPositionData();
                        //XyzztPositionData positionBase = new XyzztPositionData();
                        UvwzxyzPositionData positionBase = new UvwzxyzPositionData();

                        positionBase.Name = key.ToString();
                        //To DO : positiondata에 tag추가 ? (목적 : base, offset에 이름 표시는 안하고 Tag로 구분을 위해)

                        Positions.Add(positionBase);

                        //XyztPositionData positionTarget = new XyztPositionData();
                        //XyzztPositionData positionTarget = new XyzztPositionData();
                        UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();

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
            if (CameraConfig_Upper == null)
                CameraConfig_Upper = new HIKGigECameraConfig();
                //CameraConfig = new GrabLinkMultiCamCameraConfig();

            if (CameraConfig_Lower == null)                                        //  저해상도 카메라 추가
                CameraConfig_Lower = new HIKGigECameraConfig();
                //CameraConfig_LowRes = new GrabLinkMultiCamCameraConfig();

            if (ListIlluminationChannel == null)
                ListIlluminationChannel = new List<IlluminationChannel>();

            if (VisonCalibratorConfig_Upper == null)
                VisonCalibratorConfig_Upper = new VisionCalibratorConfig();
                VisonCalibratorConfig_Upper.Init();

            if (VisonCalibratorConfig_Lower == null)
                VisonCalibratorConfig_Lower = new VisionCalibratorConfig();
                VisonCalibratorConfig_Lower.Init();

            if (VisionCompensatorConfig == null)
                VisionCompensatorConfig = new VisionCompensatorConfig();

            if (AutoFocuserConfig_Upper == null)
                AutoFocuserConfig_Upper = new AutoFocuserConfig();
                AutoFocuserConfig_Upper.Init();

            if (AutoFocuserConfig_Lower == null)
                AutoFocuserConfig_Lower = new AutoFocuserConfig();
                AutoFocuserConfig_Lower.Init();

            if (ParamConfig == null)
                ParamConfig = new WaferProbeAlignParameterConfig();

            if (ScannerCompensatorConfig == null)
                ScannerCompensatorConfig = new ScannerCompensatorConfig();
                ScannerCompensatorConfig.Init();

            if (LaserPitchMoveShotterConfig == null)
                LaserPitchMoveShotterConfig = new LaserPitchMoveShotterConfig();
                LaserPitchMoveShotterConfig.Init();

            if (StageConfig == null)
            {
                //StageConfig = new XyztStageConfig();
                //StageConfig = new XyzztStageConfig();
                StageConfig = new UvwzxyzStageConfig();
            }
            StageConfig.Init();

            //if (JigAlignerConfig == null)
            //    JigAlignerConfig = new JigAlignerConfig();
            //JigAlignerConfig.Init();

            ParamConfig.ConfigWaferProbeAlignPositions = Positions;
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

        public void SetPositionData(string strPosition, TargetType targetType, UvwzxyzCoordinate coordinate)
        {
            foreach (UvwzxyzPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.U = coordinate.U;
                    position.V = coordinate.V;
                    position.W = coordinate.W;
                    position.EZ = coordinate.EZ;
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.VZ = coordinate.VZ;
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

        public UvwzxyzCoordinate GetPositionData(string strPosition, TargetType targetType)
        {
            UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();

            foreach (UvwzxyzPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    coordinate.U = position.U;
                    coordinate.V = position.V;
                    coordinate.W = position.W;
                    coordinate.EZ = position.EZ;
                    coordinate.X = position.X;
                    coordinate.Y = position.Y;
                    coordinate.VZ = position.VZ;
                    break;
                }
            }

            return coordinate;
        }
        #endregion
    }
}
