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
    public class LoaderConfig
    {
        #region Define

        public enum PositionKeys
        {
            R_Port_Ready,                       //  우측 Stacker 가 내려간 위치
            R_Port_Top,                         //  우측 Stacker 가 올라간 위치
            L_Port_Ready,                       //  좌측 Stacker 가 내려간 위치
            L_Port_Top,                         //  좌측 Stacker 가 올라간 위치
            Transfer_To_R_Port,                 //  Transfer 가 우측 Stacker 의 Module Pickup 위치로 이동
            Transfer_To_L_Port,                 //  Transfer 가 좌측 Stacker 의 Module Pickup 위치로 이동
            Transfer_To_M_Aligner,              //  Transfer 가 Aligner 의 Module Pickup & Putdown 위치로 이동
            Transfer_To_WorkStage,              //  Transfer 가 Work Stage 의 Module Putdown 위치로 이동
            M_Aligner_Open,                     //  Aligner 가 Open 된 위치
            M_Aligner_Close,                    //  Aligner 가 Close 된 위치
            M_Aligner_Gap_100mm,                //  Aligner 의 간격이 100mm 일 때 위치 (Module 크기에 따라 계산해서 사용하기 위한 기준 위치값)  

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
        //public RealTimeScannerConfig RealTimeScannerConfig { set; get; }
        public List<ZzxzxyPositionData> Positions { set; get; }
        static public ZzxzxyPositionDataCollection LoaderPositions { set; get; }
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
        public LoaderParameterConfig ParamConfig { set; get; }
        public LaserPitchMoveShotterConfig LaserPitchMoveShotterConfig { set; get; }
        
        public ZzxzxyStageConfig StageConfig { set; get; }
        #endregion

        #region Consturctor
        public LoaderConfig()
        {
            Positions = new List<ZzxzxyPositionData>();

            if (Positions.Count != Enum.GetValues(typeof(PositionKeys)).Length)
            {
                foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
                {
                    bool bFind = false;

                    foreach (ZzxzxyPositionData position in Positions)
                    {
                        if (position.Name == key.ToString())
                        {
                            bFind = true;
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        ZzxzxyPositionData positionBase = new ZzxzxyPositionData();

                        positionBase.Name = key.ToString();
                        //To DO : positiondata에 tag추가 ? (목적 : base, offset에 이름 표시는 안하고 Tag로 구분을 위해)

                        Positions.Add(positionBase);

                        ZzxzxyPositionData positionTarget = new ZzxzxyPositionData();

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
                ParamConfig = new LoaderParameterConfig();

            if (StageConfig == null)
            {
                StageConfig = new ZzxzxyStageConfig();
            }
            StageConfig.Init();

            //if (JigAlignerConfig == null)
            //    JigAlignerConfig = new JigAlignerConfig();
            //JigAlignerConfig.Init();

            ParamConfig.ConfigLoaderPositions = Positions;
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

        public void SetPositionData(string strPosition, TargetType targetType, ZzxzxyCoordinate coordinate)
        {
            foreach (ZzxzxyPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.Z0 = coordinate.Z0;
                    position.Z1 = coordinate.Z1;
                    position.TR_X = coordinate.TR_X;
                    position.TR_Z = coordinate.TR_Z;
                    position.ALN_X = coordinate.ALN_X;
                    position.ALN_Y = coordinate.ALN_Y;
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

        public ZzxzxyCoordinate GetPositionData(string strPosition, TargetType targetType)
        {
            ZzxzxyCoordinate coordinate = new ZzxzxyCoordinate();

            foreach (ZzxzxyPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    coordinate.Z0 = position.Z0;
                    coordinate.Z1 = position.Z1;
                    coordinate.TR_X = position.TR_X;
                    coordinate.TR_Z = position.TR_Z;
                    coordinate.ALN_X = position.ALN_X;
                    coordinate.ALN_Y = position.ALN_Y;
                    break;
                }
            }

            return coordinate;
        }
        #endregion
    }
}
