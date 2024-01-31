using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QMC.Vision
{
    [Serializable]
    public enum CameraType
    {
        None,
        GrablinkCamera,
        HikCamera,
        CoaxlinkCamera,
    }

    [Serializable]
    public class InspectionProcessorConfig
    {
        #region Define
        
        #endregion

        #region Field
        private CameraConfig m_CameraConfig;
        #endregion

        #region Property
        public CameraConfig CameraConfig
        {
            get { return this.m_CameraConfig; }
            set { this.m_CameraConfig = value; }
        }
        public VisionCompensatorConfig VisionCompensatorConfig { set; get; }

        public VisionCalibratorConfig VisionCalibratorConfig { set; get; }

        //public XyztStageConfig StageConfig { set; get; }
        //public XyzztStageConfig StageConfig { set; get; }
        public UvwzxyzStageConfig StageConfig { set; get; }
        public List<IlluminationChannel> ListIlluminationChannel { set; get; }

        public CameraType CameraType { set; get; }
        [Browsable(false)]
        public VisionScale VisionScale { set; get; }

        public AutoFocuserConfig AutoFocuserConfig { set; get; }
        #endregion

        #region Constructor
        public InspectionProcessorConfig(string strName)
        {
            CameraType = CameraType.None;
            Init(strName);
        }
        #endregion

        #region Method
        public void Init(string strName)
        {
            if (CameraConfig == null)
                CameraConfig = new CoaxlinkCameraConfig();

            if (ListIlluminationChannel == null)
                ListIlluminationChannel = new List<IlluminationChannel>();

            if(VisionCompensatorConfig == null)
                VisionCompensatorConfig = new VisionCompensatorConfig();
            VisionCompensatorConfig.Init();

            if (StageConfig == null)
            {
                StageConfig = new UvwzxyzStageConfig();
            }
            StageConfig.Init();

            if (VisionScale == null)
                VisionScale = new VisionScale();

            if(VisionCalibratorConfig == null)
            {
                VisionCalibratorConfig = new VisionCalibratorConfig();
            }
            VisionCalibratorConfig.Init();

            if(this.AutoFocuserConfig == null)
            {
                this.AutoFocuserConfig = new AutoFocuserConfig();
            }
        }
        #endregion
    }
}
