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
    public class BdsConfig
    {
        #region Define

        public enum PositionKeys
        {
            None_Mask,                          //  마스크 없는 위치
            Mask_1,                             //  마스크 1 위치
            Mask_2,                             //  마스크 2 위치
            Mask_3,                             //  마스크 3 위치
            Mask_4,                             //  마스크 4 위치

            //Ready,                              //  대기 위치
            //Picker_MovablePos_Z,                //  Loader 가 충돌 없이 움직일 수 있는 위치 (Z축)
            //Picker_PNL_PutDown,                 //  PNL 을 내려놓는 위치 (Work Table)
            //Picker_Stack0_PickUp_Ready,         //  Stacker0 에서 제품 PickUp 대기 위치
            //Picker_Stack0_PickUp,               //  Stacker0 에서 제품 PickUp 위치
            //Picker_Stack1_PickUp_Ready,         //  Stacker1 에서 제품 PickUp 대기 위치
            //Picker_Stack1_PickUp,               //  Stacker1 에서 제품 PickUp 위치
            //Stacker0_Top,                       //  제품이 비어있는 상태 (Stacker0 이 위로 올라간 위치)
            //Stacker0_Bottom,                    //  제품이 가득 찬 상태 (Stacker0 이 아래로 내려간 위치)
            //Stacker1_Top,                       //  제품이 비어있는 상태 (Stacker1 이 위로 올라간 위치)
            //Stacker1_Bottom,                    //  제품이 가득 찬 상태 (Stacker1 이 아래로 내려간 위치)
            //Aligner_Product_PickUp,             //  얼라인 완료된 자재를 PickUp 하는 위치
            //Aligner_Product_PutDown,            //  얼라인을 위해 자재를 내려놓는 위치
            //Aligner_FullOpen,                   //  얼라이너 전체 Open 위치
            //Aligner_Close100mm,                 //  얼라이너 전체 Close 위치
        }
        #endregion

        #region Property
        public RealTimeScannerConfig RealTimeScannerConfig { set; get; }
        //public List<XyztPositionData> Positions { set; get; }
        //public List<XyzztPositionData> Positions { set; get; }
        //public List<UvwzxyzPositionData> Positions { set; get; }
        public List<YPositionData> Positions { set; get; }
        static public YPositionDataCollection BdsPositions { set; get; }
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
        public BdsParameterConfig ParamConfig { set; get; }
        public LaserPitchMoveShotterConfig LaserPitchMoveShotterConfig { set; get; }
        
        public YStageConfig StageConfig { set; get; }
        #endregion

        #region Consturctor
        public BdsConfig()
        {
            Positions = new List<YPositionData>();

            if (Positions.Count != Enum.GetValues(typeof(PositionKeys)).Length)
            {
                foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
                {
                    bool bFind = false;

                    foreach (YPositionData position in Positions)
                    {
                        if (position.Name == key.ToString())
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        YPositionData positionBase = new YPositionData();

                        positionBase.Name = key.ToString();
                        //To DO : positiondata에 tag추가 ? (목적 : base, offset에 이름 표시는 안하고 Tag로 구분을 위해)

                        Positions.Add(positionBase);

                        YPositionData positionTarget = new YPositionData();

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
                ParamConfig = new BdsParameterConfig();

            if (StageConfig == null)
            {
                StageConfig = new YStageConfig();
            }
            StageConfig.Init();

            //if (JigAlignerConfig == null)
            //    JigAlignerConfig = new JigAlignerConfig();
            //JigAlignerConfig.Init();

            ParamConfig.ConfigBdsPositions = Positions;
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

        public void SetPositionData(string strPosition, TargetType targetType, YCoordinate coordinate)
        {
            foreach (YPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.Y = coordinate.Y;
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

        public YCoordinate GetPositionData(string strPosition, TargetType targetType)
        {
            YCoordinate coordinate = new YCoordinate();

            foreach (YPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    coordinate.Y = position.Y;
                    break;
                }
            }

            return coordinate;
        }
        #endregion
    }
}
