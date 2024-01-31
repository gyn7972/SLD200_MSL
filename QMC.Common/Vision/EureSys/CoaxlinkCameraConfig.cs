using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Vision.EureSys.GrabLinkMultiCamCamera;
using Euresys.ge;
using Euresys.gc;
using System.Collections.ObjectModel;

namespace QMC.Common.Vision.EureSys

#region EuresysCoaxlinkCameraConfig
{
    #region CoaxlinkCameraConstructConfiguration

    [Serializable]
    //[TypeConverter(typeof(CoaxlinkCameraConfig))]
    public class CoaxlinkCameraConfig : CameraConfig
    {
        #region Field
        public uint m_BufferCount;
        public int m_BoardIndex;
        public int m_ChannelIndex;
        public GenICam.CameraControlMethodType m_CameraControlMethod;
        public GenICam.CycleTriggerSourceType m_CycleTriggerSource;
        public GenICam.AcquisitionFrameRateEnableType m_AcquisitionFrameRateEnable;
        public double m_AcquisitionFrameRate;
        public GenICam.ErrorSelectorType m_ErrorSelector;
        public double m_ExposureTime;
        public CoaxlinkCamera.TriggerMode m_TriggerMode;
        public GenICam.LineSource m_LineSource;
        public GenICam.LineSelector m_LineSelector;




        #endregion


        #region Constructor
        /*
        public CoaxlinkCameraConfig(PartConstructMethod constructMethod)
            : base(constructMethod)
        {
        }
        public CoaxlinkCameraConfig() : this(PartConstructMethod.Static) { }
        */
        #endregion


        #region Property
        public uint BufferCount
        {
            get { return this.m_BufferCount; }
            set { this.m_BufferCount = value; }
        }

        public int BoardIndex
        {
            get { return this.m_BoardIndex; }
            set { this.m_BoardIndex = value; }
        }

        public int ChannelIndex
        {
            get { return this.m_ChannelIndex; }
            set { this.m_ChannelIndex = value; }
        }

        public GenICam.CameraControlMethodType CameraControlMethod
        {
            get { return this.m_CameraControlMethod; }
            set { this.m_CameraControlMethod = value; }
        }

        [Category("Trigger")]
        public GenICam.CycleTriggerSourceType CycleTriggerSource
        {
            get { return this.m_CycleTriggerSource; }
            set { this.m_CycleTriggerSource = value; }
        }

        public GenICam.AcquisitionFrameRateEnableType AcquisitionFrameRateEnable
        {
            get { return this.m_AcquisitionFrameRateEnable; }
            set { this.m_AcquisitionFrameRateEnable = value; }
        }

        public double AcquisitionFrameRate
        {
            get { return this.m_AcquisitionFrameRate; }
            set { this.m_AcquisitionFrameRate = value; }
        }

        public GenICam.ErrorSelectorType ErrorSelector
        {
            get { return this.m_ErrorSelector; }
            set { this.m_ErrorSelector = value; }
        }

        [Category("Trigger")]
        public CoaxlinkCamera.TriggerMode TriggerMode
        {
            get { return this.m_TriggerMode; }
            set { this.m_TriggerMode = value; }
        }

        [Category("Trigger")]
        public GenICam.LineSource LineSource
        {
            get { return this.m_LineSource; }
            set { this.m_LineSource = value; }
        }

        [Category("Trigger")]
        public GenICam.LineSelector LineSelector
        {
            get { return this.m_LineSelector; }
            set { this.m_LineSelector = value; }
        }

        public double ExposureTime
        {
            get { return this.m_ExposureTime; }
            set { this.m_ExposureTime = value; }
        }

        public string ChannelName { get; set; }


        #endregion


        #region ConstructorConfig Members


        public  CoaxlinkCameraConfig() : base()
        
        {
            //base.SetDefaultValues();

            this.BufferCount = 50;
            this.BoardIndex = 0;
            this.ChannelIndex = 0;
            this.Resolution = this.CameraResolution = new System.Drawing.Size(2464, 2056);
            this.CameraControlMethod = GenICam.CameraControlMethodType.RG;
            this.CycleTriggerSource = GenICam.CycleTriggerSourceType.Immediate;
           // this.CycleTriggerSource = GenICam.CycleTriggerSourceType.LIN1;
            this.AcquisitionFrameRateEnable = GenICam.AcquisitionFrameRateEnableType.Off;
            this.AcquisitionFrameRate = 116;
            this.ErrorSelector = GenICam.ErrorSelectorType.All;
            this.ExposureTime = 5000;
            this.TriggerMode = CoaxlinkCamera.TriggerMode.Immediate;
            this.LineSource = GenICam.LineSource.Device0Strobe;
            this.LineSelector = GenICam.LineSelector.IIN11;
        }
        #endregion


    }

    public class CoaxlinkCameraConfigCollection : Collection<CoaxlinkCameraConfig>
    {
        public CoaxlinkCameraConfigCollection()
        {

        }
    }


    #endregion

    #endregion
}
