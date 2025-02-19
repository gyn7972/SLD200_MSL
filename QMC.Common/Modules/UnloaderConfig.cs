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
    public class UnloaderConfig
    {
        #region Define

        public enum PositionKeys
        {
            Transfer_To_WorkStage,              //  Transfer 가 Work Stage 의 Module Pickup 위치로 이동
            Transfer_To_F_Port,                 //  Transfer 가 NG Stacker 의 Module Drop 위치로 이동
            Transfer_To_R_Port,                 //  Transfer 가 우측 Stacker 의 Module Pickup 위치로 이동
            Transfer_To_L_Port,                 //  Transfer 가 좌측 Stacker 의 Module Pickup 위치로 이동
            R_Port_Ready,                       //  우측 Stacker 가 내려간 위치
            R_Port_Top,                         //  우측 Stacker 가 올라간 위치
            L_Port_Ready,                       //  좌측 Stacker 가 내려간 위치
            L_Port_Top,                         //  좌측 Stacker 가 올라간 위치


            //Ready,                              //  대기 위치
            //Load,                               //  제품 로딩 위치
            //UnLoad,                             //  제품 언로딩 위치
            //ReticleGlass_UpperCamera,           //  상부 카메라가 Reticle Glass 를 보는 위치
            //ReticleGlass_LowerCamera,           //  하부 카메라가 Reticle Glass 를 보는 위치
            //AlignPosition_Ver_Top,              //  세로 방향 Wafer 얼라인 위치 (TOP)
            //AlignPosition_Ver_Middle,           //  세로 방향 Wafer 얼라인 위치 (MID)
            //AlignPosition_Ver_Bottom,           //  세로 방향 Wafer 얼라인 위치 (BOT)
            //AlignPosition_Hor_Left,             //  가로 방향 Wafer 얼라인 위치 (LEFT)
            //AlignPosition_Hor_Center,           //  가로 방향 Wafer 얼라인 위치 (CENTER)
            //AlignPosition_Hor_Right,            //  가로 방향 Wafer 얼라인 위치 (RIGHT)
            //ProbeWafer_Packing,                 //  Probe Card 에 Wafer 를 Packing 하는 위치 (Elev. Z 값만 사용)
        }
        #endregion

        #region Property
        public RealTimeScannerConfig RealTimeScannerConfig { set; get; }
        //public List<XyztPositionData> Positions { set; get; }
        //public List<XyzztPositionData> Positions { set; get; }
        //public List<UvwzxyzPositionData> Positions { set; get; }
        public List<ZzxzPositionData> Positions { set; get; }
        static public ZzxzPositionDataCollection UnloaderPositions { set; get; }
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
        public PickupAgentConfig PickUpAgentConfig { set; get; }
        public UnloaderParameterConfig ParamConfig { set; get; }
        public LaserPitchMoveShotterConfig LaserPitchMoveShotterConfig { set; get; }
        
        public ZzxzStageConfig StageConfig { set; get; }
        #endregion

        #region Consturctor
        public UnloaderConfig()
        {
            Positions = new List<ZzxzPositionData>();

            if (Positions.Count != Enum.GetValues(typeof(PositionKeys)).Length)
            {
                foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
                {
                    bool bFind = false;

                    foreach (ZzxzPositionData position in Positions)
                    {
                        if (position.Name == key.ToString())
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        ZzxzPositionData positionBase = new ZzxzPositionData();

                        positionBase.Name = key.ToString();
                        //To DO : positiondata에 tag추가 ? (목적 : base, offset에 이름 표시는 안하고 Tag로 구분을 위해)

                        Positions.Add(positionBase);

                        ZzxzPositionData positionTarget = new ZzxzPositionData();

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
            if (ParamConfig == null)
                ParamConfig = new UnloaderParameterConfig();

            if (StageConfig == null)
            {
                StageConfig = new ZzxzStageConfig();
            }
            StageConfig.Init();

            //if (JigAlignerConfig == null)
            //    JigAlignerConfig = new JigAlignerConfig();
            //JigAlignerConfig.Init();

            ParamConfig.ConfigUnloaderPositions = Positions;
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

        public void SetPositionData(string strPosition, TargetType targetType, ZzxzCoordinate coordinate)
        {
            foreach (ZzxzPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.Z0 = coordinate.Z0;
                    position.Z1 = coordinate.Z1;
                    position.TR_X = coordinate.TR_X;
                    position.TR_Z = coordinate.TR_Z;
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

        public ZzxzCoordinate GetPositionData(string strPosition, TargetType targetType)
        {
            ZzxzCoordinate coordinate = new ZzxzCoordinate();

            foreach (ZzxzPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    coordinate.Z0 = position.Z0;
                    coordinate.Z1 = position.Z1;
                    coordinate.TR_X = position.TR_X;
                    coordinate.TR_Z = position.TR_Z;
                    break;
                }
            }

            return coordinate;
        }
        #endregion
    }
}
