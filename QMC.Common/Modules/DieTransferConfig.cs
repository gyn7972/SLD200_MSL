using QMC.Common.Hmi;
using QMC.Common.Parts;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
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
    public class DieTransferConfig
    {
        [Browsable(false)]
        public LoadZConfig LoadZConfig { get; set; }
        [Browsable(false)]
        public UnloadZConfig UnloadZConfig { get; set; }
        [Browsable(false)]
        public TurretConfig TurretConfig { get; set; }
        [Browsable(false)]
        public ZPositionDataCollection LoadZPositions { set; get; }
        [Browsable(false)]
        public ZPositionDataCollection UnloadZPositions { set; get; }
        [Browsable(false)]
        public List<CalibrationPitch> LoadZCalPitch { set; get; }
        [Browsable(false)]
        public List<CalibrationPitch> UnloadZCalPitch { get; set; }
        [Browsable(false)]
        public GrabLinkMultiCamCameraConfig GrabLinkMultiCamCameraConfig { get; set; }
        [Browsable(false)]
        public List<IlluminationChannel> ListIlluminationChannel { set; get; }
        //public ColletXYPositionCalibratorConfig XYCalibratorConfig { set; get; }

        //[Browsable(false)]
        //public HIKGigECameraConfig HIKGigECameraConfig { set; get; }
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public AutoFocuserConfig AutoFocuerConfig { set; get; }

        public List<XyCoordinate> ColletXYOffset { set; get; }
        public List<XyCoordinate> ColletXYUserOffset { set; get; }
        public List<double> AutoCalzPos { set; get; }

        public int TurretArmCount 
        { 
            set
            {
                TurretConfig.TurretArmCount = value;
            }
            get
            {
                return TurretConfig.TurretArmCount;
            }
        }
        public int TurretMovementAngle 
        { 
            get
            {
                return TurretConfig.TurretMovementAngle;
            }
            set
            {
                TurretConfig.TurretMovementAngle = value;
            }
        }

        public List<int> CollectChannel { set; get; }
        public List<string> listTurretPositionProperty 
        { 
            set
            {
                TurretConfig.listTurretPositionProperty = value;
            }
            get
            {
                return TurretConfig.listTurretPositionProperty;
            }
        }

        [Serializable]
        public enum PositionLoadZ
        {
            GoBack,
            Pick,
        }
        [Serializable]
        public enum PositionUnloadZ
        {
            GoBack,
            Place,
        }

        public DieTransferConfig()
        {
            Init();
            TurretArmCount = 6;
            LoadZPositions.Clear();
            foreach (PositionLoadZ key in Enum.GetValues(typeof(PositionLoadZ)))
            {
                ZPositionData zpositionBase = new ZPositionData();
                zpositionBase.Name = key.ToString();
                LoadZPositions.Add(zpositionBase);

                ZPositionData zpositionTarget = new ZPositionData();
                zpositionTarget.Name = key.ToString();
                zpositionTarget.Type = TargetType.Offset;
                LoadZPositions.Add(zpositionTarget);
            }

            UnloadZPositions.Clear();
            foreach (PositionUnloadZ key in Enum.GetValues(typeof(PositionUnloadZ)))
            {
                ZPositionData zpositionBase = new ZPositionData();
                zpositionBase.Name = key.ToString();
                UnloadZPositions.Add(zpositionBase);

                ZPositionData zpositionTarget = new ZPositionData();
                zpositionTarget.Name = key.ToString();
                zpositionTarget.Type = TargetType.Offset;
                UnloadZPositions.Add(zpositionTarget);
            }

            for (int i = 0; i < TurretArmCount; i++)
            {
                CollectChannel.Add(i);
            }

            

            AutoCalzPos = new List<double>();
            for (int i = 0; i < 6; i++)
            {
                AutoCalzPos.Add(0);
            }
        }
        public void Init()
        {
            if(LoadZConfig == null)
                LoadZConfig = new LoadZConfig();

            if (UnloadZConfig == null)
                UnloadZConfig = new UnloadZConfig();

            if (TurretConfig == null)
                TurretConfig = new TurretConfig();

            if (LoadZPositions == null)
                LoadZPositions = new ZPositionDataCollection();

            if (UnloadZPositions == null)
                UnloadZPositions = new ZPositionDataCollection();

            if (ListIlluminationChannel == null)
                ListIlluminationChannel = new List<IlluminationChannel>();

            //if (XYCalibratorConfig == null)
            //    XYCalibratorConfig = new ColletXYPositionCalibratorConfig();

            if (GrabLinkMultiCamCameraConfig == null)
                GrabLinkMultiCamCameraConfig = new GrabLinkMultiCamCameraConfig();

            //if (HIKGigECameraConfig == null)
            //    HIKGigECameraConfig = new HIKGigECameraConfig();

            if (CollectChannel == null)
                CollectChannel = new List<int>();
            if (AutoFocuerConfig == null)
                AutoFocuerConfig = new AutoFocuserConfig();

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

        public void SetLoadZPosition(string strPositionKey, TargetType targetType, double dPosition)
        {
            foreach (ZPositionData position in LoadZPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.Z = dPosition;
                    break;
                }
            }
        }

        public void SetUnloadZPosition(string strPositionKey, TargetType targetType, double dPosition)
        {
            foreach (ZPositionData position in UnloadZPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.Z = dPosition;
                    break;
                }
            }
        }

        public double GetLoadZPosition(string strPositionKey, TargetType targetType)
        {
            double dPosition = 0;
            foreach (ZPositionData position in LoadZPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    dPosition = position.Z;
                    break;
                }
            }
            return dPosition;
        }

        public double GetUnloadZPosition(string strPositionKey, TargetType targetType)
        {
            double dPosition = 0;
            foreach (ZPositionData position in UnloadZPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    dPosition = position.Z;
                    break;
                }
            }
            return dPosition;
        }
    }
}
