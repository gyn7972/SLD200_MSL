using QMC.Common.Parts;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    [Serializable]
    public class DieLoaderConfig
    {
        [Serializable]
        public enum PositionKeys
        {
            Load,
            Unload,
            Center,
            ChangeNeedle,
            TouchSensor,
            BackLight,
            FlowSensorCalibration,
        }


        public double Radius { set; get; }
        public XytPositionDataCollection Positions { set; get; } //Stage Positiondata
        public NeedleBlockConfig NeedleBlockConfig { set; get; }
        public PickupAgentConfig PickUpAgentConfig { set; get; }
        public VisionCalibratorConfig VisonCalibratorConfig { set; get; }

        public RealTimeScannerConfig RealTimeScannerConfig { set; get; }

        public List<IlluminationChannel> ListIlluminationChannel { set; get; }

        public GrabLinkMultiCamCameraConfig GrabLinkMultiCamCameraConfig { get; set; }

        //public HIKGigECameraConfig HIKGigECameraConfig { get; set; }

        public ColletXYPositionCalibratorConfig XYCalibratorConfig { set; get; }

        public List<XyCoordinate> ColletXYOffset { set; get; }
        public List<XyCoordinate> ColletXYUserOffset { set; get; }

        public DieLoaderConfig()
        {
            Init();

            Radius = 0;
            Positions.Clear();
            foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
            {
                XytPositionData positionBase = new XytPositionData();
                positionBase.Name = key.ToString();
                Positions.Add(positionBase);

                XytPositionData positionTarget = new XytPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                Positions.Add(positionTarget);
            }

            
        }
        public void Init()
        {
            if (Positions == null)
                Positions = new XytPositionDataCollection();

            if (NeedleBlockConfig == null)
                NeedleBlockConfig = new NeedleBlockConfig();

            if (ListIlluminationChannel == null)
                ListIlluminationChannel = new List<IlluminationChannel>();

            if (PickUpAgentConfig == null)
                PickUpAgentConfig = new PickupAgentConfig();

            if (VisonCalibratorConfig == null)
                VisonCalibratorConfig = new VisionCalibratorConfig();

            if (RealTimeScannerConfig == null)
                RealTimeScannerConfig = new RealTimeScannerConfig();

            if (GrabLinkMultiCamCameraConfig == null)
                GrabLinkMultiCamCameraConfig = new GrabLinkMultiCamCameraConfig();

            //if (HIKGigECameraConfig == null)
            //    HIKGigECameraConfig = new HIKGigECameraConfig();

            if (XYCalibratorConfig == null)
                XYCalibratorConfig = new ColletXYPositionCalibratorConfig();
            if (ColletXYOffset == null)
            {
                ColletXYOffset = new List<XyCoordinate>();
                for (int i = 0; i < 6; i++)
                {
                    ColletXYOffset.Add(new XyCoordinate());
                }
            }
            if (ColletXYUserOffset == null)
            {
                ColletXYUserOffset = new List<XyCoordinate>();
                for (int i = 0; i < 6; i++)
                {
                    ColletXYUserOffset.Add(new XyCoordinate());
                }
            }
        }

        public void SetPositionData(string strPosition, TargetType targetType, XytCoordinate coordinate)
        {
            foreach (XytPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.T = coordinate.T;
                    break;
                }
            }
        }

        public XytCoordinate GetPositionData(string strPosition, TargetType targetType)
        {
            XytCoordinate coordinate = new XytCoordinate();

            foreach (XytPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    coordinate.X = position.X;
                    coordinate.Y = position.Y;
                    coordinate.T = position.T;
                    break;
                }
            }

            return coordinate;
        }

    }
}
