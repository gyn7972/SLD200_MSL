using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Parts;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Common.VisionPart;
using QMC.Process.WorkStage.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Process.WorkStage.Modules
{
    [Serializable]
    public class WorkStageProcessorConfig
    {
        public enum PositionKey
        {
            Etching,
            Load,
            Unload,
        }

        protected XyztPositionDataCollection m_EtchingPosition;

        public XyztPositionDataCollection EtchingPosition
        {
            get { return m_EtchingPosition; }
            set { m_EtchingPosition = value; }
        }

        public HIKGigECameraConfig CameraConfig_Upper { set; get; }
        //public GrabLinkMultiCamCameraConfig CameraConfig { set; get; }
        //public ConveyorConfig ConveyorConfig { get; set; }

        public HIKGigECameraConfig CameraConfig_Lower { set; get; }

        public VisionCalibratorConfig VisonCalibratorConfig { set; get; }

        [Category("Illuminator")]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public DigitalIlluminatorConfig IlluminatorConfig { set; get; }
        public List<IlluminationChannel> ListIlluminationChannel { set; get; }

        public ScannerParameterConfig ScannerConfig { set; get; }

        public List<XyCoordinate> OffsetVisionToScanner
        {
            set
            {
                ScannerConfig.OffsetVisionToScanner = value;
            }
            get
            {
                return ScannerConfig.OffsetVisionToScanner;
            }
        }
        public WorkStageProcessorConfig()
        {
            Init();
            m_EtchingPosition.Clear();
            foreach (PositionKey key in Enum.GetValues(typeof(PositionKey)))
            {
                XyztPositionData positionBase = new XyztPositionData();
                positionBase.Name = key.ToString();
                m_EtchingPosition.Add(positionBase);

                XyztPositionData positionTarget = new XyztPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                m_EtchingPosition.Add(positionTarget);
            }

            
        }

        public void Init()
        {
            if (CameraConfig_Upper == null)
                CameraConfig_Upper = new HIKGigECameraConfig();
            //CameraConfig = new GrabLinkMultiCamCameraConfig();

            if (CameraConfig_Lower == null)
                CameraConfig_Lower = new HIKGigECameraConfig();

            /*if(ConveyorConfig == null)
                ConveyorConfig = new ConveyorConfig();*/

            if (IlluminatorConfig == null)
                IlluminatorConfig = new DigitalIlluminatorConfig();

            if (ListIlluminationChannel == null)
                ListIlluminationChannel = new List<IlluminationChannel>();

            if(VisonCalibratorConfig == null)
                VisonCalibratorConfig = new VisionCalibratorConfig();

            if (m_EtchingPosition == null)
                m_EtchingPosition = new XyztPositionDataCollection();

            if(ScannerConfig == null)
                ScannerConfig = new ScannerParameterConfig();
        }

        public List<string> GetInspectionPosition()
        {
            List<string> ret = new List<string>();
            foreach (XyztPositionData position in m_EtchingPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public void SetPositionData(string strPosition, TargetType targetType, XyztCoordinate coordinate)
        {
            foreach (XyztPositionData position in m_EtchingPosition)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.T = coordinate.T;
                    position.Z = coordinate.Z;
                    break;
                }
            }
        }

        public XyztCoordinate GetPositionData(string strPosition)
        {
            XyztCoordinate coordinate = new XyztCoordinate();

            foreach (XyztPositionData position in m_EtchingPosition)
            {
                if (position.Name == strPosition)
                {
                    coordinate += position.Coordinate;
                }
            }
            return coordinate;
        }

        public XyCoordinate GetOffsetVisionToScanner(int nIndex)
        {
            XyCoordinate coordinate = new XyCoordinate();
            if(nIndex >= 0 && nIndex < OffsetVisionToScanner.Count)
            {
                coordinate = OffsetVisionToScanner[nIndex];
                coordinate.Y += ScannerConfig.EtchingOffsetY;
            }

            return coordinate;
        }
    }
}
