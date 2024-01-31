using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Vision.Cameras;

namespace QMC.Common.Vision.EureSys
{
    [Serializable]
    public class MultiCamCameraConfig : CameraConfig
    {
        public int m_Channel;
        public bool m_ChannelActive;
        public bool m_AcquisitionFinished;
        public bool m_ExposureFinished;
        public bool m_GrabNewImageEventEnable;
        public MultiCamCamera.FrameNoPerSequence m_SettingFrameCount;
      
        [DefaultValue(0)]
        public int Channel
        {
            get { return this.m_Channel; }
            set { this.m_Channel = value; }
        }

        /// <summary>
        /// EureSys Camera의 Channel Active Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool ChannelActive
        {
            get { return this.m_ChannelActive; }
            private set { this.m_ChannelActive = value; }
        }

        /// <summary>
        /// EureSys Camera의 Acquisition 완료 Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool AcquisitionFinished
        {
            get { return this.m_AcquisitionFinished; }
            private set
            {
                if (this.m_AcquisitionFinished == value) return;
                //this.WriteLog(LogLevel.Lowest, string.Format("AcquisitionFinished Value :{0} -> {1}", m_AcquisitionFinished, value));
                Console.WriteLine(string.Format("AcquisitionFinished Value :{0} -> {1}", m_AcquisitionFinished, value));
                this.m_AcquisitionFinished = value;
            }
        }

        /// <summary>
        /// EureSys Camera의 Exposure 완료 Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool ExposureFinished
        {
            get { return this.m_ExposureFinished; }
            private set
            {
                if (this.m_ExposureFinished == value) return;
                //this.WriteLog(LogLevel.Lowest, string.Format("ExposureFinished Value :{0} -> {1}", m_ExposureFinished, value));
                Console.WriteLine(string.Format("ExposureFinished Value :{0} -> {1}", m_ExposureFinished, value));
                this.m_ExposureFinished = value;
            }
        }

        /// <summary>
        /// GrabNewImageEvent 사용 여부에 대해서 가져오거나 설정한다.
        /// </summary>
        public bool GrabNewImageEventEnable
        {
            get { return this.m_GrabNewImageEventEnable; }
            set { this.m_GrabNewImageEventEnable = value; }
        }

        public MultiCamCamera.FrameNoPerSequence SettingFrameCount
        {
            get { return this.m_SettingFrameCount; }
            private set { this.m_SettingFrameCount = value; }
        }


        [Category("Channel")]
        public string CamFilePath
        {
            get;
            set;
        }

        [Category("Channel")]
        public Enum Connector
        {
            get;
            set;
        }

        [Category("Channel")]
        public MultiCamCamera.ColorFormats ColorFormat
        {
            get;
            set;
        }

        [Category("Channel")]
        public MultiCamCamera.TriggerLine TrigLine
        {
            get;
            set;
        }
        [Category("Channel")]
        public MultiCamCamera.TriggerMode TrigMode { set; get; }
        [Category("Channel")]
        public MultiCamCamera.NextTriggerMode NextTrigMode
        {
            get;
            set;
        }
        [Category("Board")]
        public Enum BoardTopology { set; get; }
        [Category("Board")]
        public uint BoardIndex { set; get; }
        [Category("Channel")]
        public int SurfaceCount
        {
            get;
            set;
        }

        public MultiCamCameraConfig()
        {
            this.GrabNewImageEventEnable = true;
            this.SettingFrameCount = MultiCamCamera.FrameNoPerSequence.One;
            this.BoardIndex = 0;
            this.ColorFormat = MultiCamCamera.ColorFormats.Y8;                    

            this.TrigLine = MultiCamCamera.TriggerLine.Nom;
            this.TrigMode = MultiCamCamera.TriggerMode.Immediate;
            this.NextTrigMode = MultiCamCamera.NextTriggerMode.Combined;
            this.SurfaceCount = 3;

        }
    }
}
