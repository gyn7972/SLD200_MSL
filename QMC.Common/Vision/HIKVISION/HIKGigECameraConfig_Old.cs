using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.HIKVISION
{
    [Serializable]
    public enum ParamHIKGigECameraConfigKey
    {
        SerialNumber,
        ExposureTime,
        Gain,
        OpenDelayTime,
        RetryCount,
        OffsetX,
        OffsetY,
        CameraType,
    }

    [Serializable]
    [TypeConverter(typeof(HIKGigECameraConfig))]
    public class HIKGigECameraConfig : CameraConfig       
    {

        #region Property
        [Category("HIKGigECameraConfig")]
        public string SerialNumber { get; set; }

        [Category("HIKGigECameraConfig")]
        public float ExposureTime { get; set; }

        [Category("HIKGigECameraConfig")]
        public float Gain { get; set; }
        [Category("HIKGigECameraConfig")]
        public int OpenDelayTime { get; set; }
        [Category("HIKGigECameraConfig")]
        public int RetryCount { get; set; }
        #endregion

        public HIKGigECameraConfig()
            :base()
        {
            this.SerialNumber = "";
            this.ExposureTime = 1000.0f;
            this.Gain = 1.0f;
            this.OpenDelayTime = 1000;
            this.RetryCount = 5;
        }


    }
    public class HIKGigECameraConfigCollection : Collection<HIKGigECameraConfig>
    {
        public HIKGigECameraConfigCollection()
        {

        }
    }
   
}
