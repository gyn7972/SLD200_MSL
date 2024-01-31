/*
 * Purpose
 *      Euresys사의 Frame Grabber에 대해서 정의한다.
 *      
 * Remark
 *      Euresys사의 MultiCam Library를 활용한다.
 *      
 * Reference
 *      
 * Revision
 *      1. Created: 2017.11.15 LEE.SH
 *      2. Modified: 2018.11.07 JUNG.CY
 *          - abstract 추가 : Grabber Series별 클래스 생성 완료.
 *      3. Modified: 2020.04.08 yjbaek
 *          - ExposeCamera apply
 * 
 */

using System;
using System.ComponentModel;
using QMC.Common.Vision.Cameras;

using System.Drawing;
using System.Threading;
using QMC.Common.Modules;
using System.Windows.Forms;

namespace QMC.Common.Vision.EureSys
{
    #region MultiCamCamera
    [Serializable]
    public abstract class MultiCamCamera : Camera
    {
        #region Define
        /// <summary>
        /// Euresys Camera AlarmKeys
        /// </summary>
        [Serializable]
        public delegate int ExposeDelegate();
        public delegate int ReadoutDelegate(out VisionImage image);
        public new enum AlarmKeys
        {
            [Description("Failed to Set the Camera in Snapshot Mode")]
            SetSnapshotModeFailed,

            /// <summary>
            /// Failed to Set the Camera in Video Mode
            /// </summary>
            SetVideoModeFailed,

            /// <summary>
            /// Failed to Process the Camera Callback Function
            /// </summary>
            ProcessingCallbackFailed,

            /// <summary>
            /// Failed to Process the Camera Callback Function by Time-out
            /// </summary>
            ProcessingCallbackTimeOut,

            /// <summary>
            /// Failed to ExposeEnd the Camera Callback Function by Time-out
            /// </summary>
            ExposeEndTimeOut,
        }

        /// <summary>
        /// Path and filename of the error log file
        /// <para>
        /// This parameter specifies the path and the filename of the error log file that is created when the application returns a MC_INVALID_PARAMETER_SETTING (-22) error code.
        /// The incorrect parameters are reported in the log file, including the wrong value and the possible correct values.
        /// When specified, the log file is created and filled during the consistency check.
        /// When unspecified, the consistency check does not produce a log file...
        /// </para>
        /// </summary>
        [Serializable]
        [Flags]
        public enum ErrorLog
        {
            /// <summary>
            /// not create the error log file
            /// </summary>
            None,

            /// <summary>
            /// create the error log file
            /// </summary>
            ErrorLog,
        }

        /// <summary>
        /// Camera Parameter Name (Channel)
        /// When creating a Channel, use an handle designating the connector structure. When creating a Surface, use MC_DEFAULT_SURFACE_HANDLE.
        /// </summary>
        [Serializable]
        private enum Models
        {
            /// <summary>
            /// 
            /// </summary>
            Channel,

            /// <summary>
            /// 
            /// </summary>
            Model,
        }

        /// <summary>
        /// Name of the CAM file
        /// <para>
        /// This parameter specifies a camera configuration file as a character string.
        /// The .cam extension may or may not be included.
        /// The maximum string length is 1024.
        /// </para>
        /// </summary>
        [Serializable]
        [Flags]
        public enum CamFile
        {
            /// <summary>
            /// 
            /// </summary>
            Ntsc,

            /// <summary>
            /// 
            /// </summary>
            Pal,

            /// <summary>
            /// 
            /// </summary>
            BM30C,
        }

        /// <summary>
        /// Designation of color format
        /// <para>
        /// This parameter summarizes all the properties describing how the frame grabber stores pixel data in the destination surface.
        /// </para>
        /// </summary>
        [Serializable]
        public enum ColorFormats
        {
            /// <summary>
            /// Each pixel luminance is stored using Y8 system. ( 8 bits per pixel / Monochrome )
            /// </summary>
            Y8,

            /// <summary>
            /// Each pixel luminance is stored using Y8 system. ( 10 bits per pixel / Monochrome )
            /// </summary>
            Y10,

            /// <summary>
            /// Each pixel luminance is stored using Y8 system. ( 12 bits per pixel / Monochrome )
            /// </summary>
            Y12,

            /// <summary>
            /// Each pixel luminance is stored using Y8 system. ( 14 bits per pixel / Monochrome )
            /// </summary>
            Y14,

            /// <summary>
            /// Each pixel luminance is stored using Y8 system. ( 16 bits per pixel / Monochrome )
            /// </summary>
            Y16,
        }

        /// <summary>
        /// State of the channel
        /// <para>
        /// This parameter gives access to the state of the channel.
        /// </para>
        /// </summary>
        [Serializable]
        public enum ChannelState
        {
            /// <summary>
            /// (Set) Sets the channel's state to IDLE or READY.
            /// (Get) The channel owns the grabber at this moment but does not lock it.
            /// <para>
            /// Values applicable to Grablink, Domino and Picolo boards.
            /// </para>
            /// </summary>
            Idle,

            /// <summary>
            /// (Set) Try to set the channel's state to ORPHAN.
            /// <para>
            /// Values applicable to Grablink and Domino boards
            /// </para>
            /// </summary>
            Free,

            /// <summary>
            /// (Get) The channel has no grabber.
            /// <para>
            /// Values applicable to Grablink and Domino boards
            /// </para>
            /// </summary>
            Orphan,

            /// <summary>
            /// (Set) Try to set the channel's state to READY.
            /// (Get) The channel locks the grabber and is ready to start an acquisition sequence.
            /// <para>
            /// Values applicable to Grablink and Domino boards
            /// </para>
            /// </summary>
            Ready,

            /// <summary>
            /// (Set) Sets the channel's state to ACTIVE.
            /// (Get) The channel uses the grabber.
            /// <para>
            /// Values applicable to Grablink, Domino and Picolo boards.
            /// </para>
            /// </summary>
            Active,
        }

        /// <summary>
        /// Fundamental acquisition mode.
        /// <para>
        /// The user can work with a simplified version of the MultiCam model by selecting an acquisition mode.
        /// </para>
        /// </summary>
        [Serializable]
        public enum AcquisitionMode
        {
            /// <summary>
            /// This mode is intended for recording multiple video sequences from standard area-scan cameras.
            /// ActivityLength specifies the number of sequences within the channel activity period.
            /// </summary>
            Video,

            /// <summary>
            /// This mode is intended for acquisition of snapshot images from area-scan cameras.
            /// The unique sequence is capable to acquire SeqLength_Fr frames within the channel activity period.
            /// </summary>
            Snapshot,

            /// <summary>
            /// This mode is intended for acquisition of snapshot images from high frame rate area-scan cameras.
            /// A single sequence is capable to acquire SeqLength_Fr frames within the channel activity period. 
            /// The sequence is divided into phases, each phase acquiring PhaseLength_Fr frames into a single destination surface.
            /// </summary>
            Hfr,

            /// <summary>
            /// This mode is intended for image acquisition of a continuous object, like a web, from a line-scan camera.
            /// A single sequence acquiring SeqLength_Ln contiguous lines is available within the channel activity period.
            /// The sequence is divided in contiguous phases, each phase acquiring PageLength_Ln lines.
            /// </summary>
            Web,

            /// <summary>
            /// This mode is intended for image acquisition of discrete objects from a line-scan camera.
            /// Each page is constituted of contiguous lines; the page length, expressed in lines, is specified by PageLength_Ln.
            /// A single sequence is capable to acquire SeqLength_Pg pages within the channel activity period.
            /// </summary>
            Page,

            /// <summary>
            /// This mode is intended for image acquisition of long or variable size discrete objects from a line-scan camera.
            /// The parameter ActivityLength specifies the number of sequences within the channel activity period.
            /// Each sequence is capable to acquire SeqLength_Ln contiguous lines.
            /// A sequence is divided in phases, each phase acquiring PageLength_Ln lines.
            /// </summary>
            Longpage,
        }

        /// <summary>
        /// Camera의 Trigger Mode
        /// </summary>
        [Serializable]
        public enum TriggerMode
        {
            /// <summary>
            /// Note for Picolo Diligent. The only possible value is IMMEDIATE.
            /// </summary>
            Immediate,

            /// <summary>
            /// 
            /// </summary>
            Hard,

            /// <summary>
            /// 
            /// </summary>
            Soft,

            /// <summary>
            /// 
            /// </summary>
            Combined,

            /// <summary>
            /// 
            /// </summary>
            Slave,
        }

        /// <summary>
        /// Camera의 Next Trigger Mode
        /// </summary>
        [Serializable]
        public enum NextTriggerMode
        {
            /// <summary>
            /// 
            /// </summary>
            Combined,

            /// <summary>
            /// 
            /// </summary>
            Hard,

            /// <summary>
            /// 
            /// </summary>
            Periodic,

            /// <summary>
            /// 
            /// </summary>
            Repeat,

            /// <summary>
            /// 
            /// </summary>
            Same,

            /// <summary>
            /// 
            /// </summary>
            Soft,

            /// <summary>
            /// 
            /// </summary>
            Slave,
        }

        /// <summary>
        /// Camera의 End Trigger Mode
        /// </summary>
        [Serializable]
        public enum EndTriggerMode
        {
            /// <summary>
            /// 
            /// </summary>
            Auto,

            /// <summary>
            /// 
            /// </summary>
            Hard,

            /// <summary>
            /// 
            /// </summary>
            Slave,
        }

        /// <summary>
        /// Camera의 Break Effect
        /// </summary>
        [Serializable]
        public enum BreakEffect
        {
            /// <summary>
            /// 
            /// </summary>
            Finish,

            /// <summary>
            /// 
            /// </summary>
            Abort,
        }

        /// <summary>
        /// Control of acquisition sequences count.
        /// <para>
        /// An activity period of a channel is made of one or several acquisition sequences.
        /// The ActivityLength parameter establishes the number of acquisition sequences constituting a channel activity period.
        /// </para>
        /// </summary>
        [Serializable]
        public enum ActivitySequenceCount
        {
            /// <summary>
            /// Setting ActivityLength to MC_INDETERMINATE results in indefinitely repeated acquisition sequences. A user break is required to stop the channel activity.
            /// </summary>
            IndefiniteRepeat = MultiCam.SpecificParameterConstants.Indeterminate,

            /// <summary>
            /// An activity period of a channel is made of one acquisition sequences.
            /// </summary>
            One = 1,

            /// <summary>
            /// An activity period of a channel is made of two acquisition sequences.
            /// </summary>
            Two = 2,

            /// <summary>
            /// An activity period of a channel is made of three acquisition sequences.
            /// </summary>
            Three = 3,

            /// <summary>
            /// An activity period of a channel is made of four acquisition sequences.
            /// </summary>
            Four = 4,

            /// <summary>
            /// An activity period of a channel is made of five acquisition sequences.
            /// </summary>
            Five = 5,
        }

        /// <summary>
        /// Number of frames constituting a phase
        /// <para>
        /// The parameter establishes the total number of frames acquired within an acquisition phase.
        /// It is relevant only when AcquisitionMode is HFR.The range of values is [2..256].
        /// </para>
        /// </summary>
        [Serializable]
        public enum FrameNoPerSequence
        {
            /// <summary>
            /// Setting ActivityLength to MC_INDETERMINATE results in indefinitely repeated acquisition sequences. A user break is required to stop the channel activity.
            /// </summary>
            IndefiniteRepeat = MultiCam.SpecificParameterConstants.Indeterminate,

            Zero = 0,

            /// <summary>
            /// 1 Frame / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            One = 1,

            /// <summary>
            /// 2 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Two = 2,

            /// <summary>
            /// 3 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Three = 3,

            /// <summary>
            /// 4 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Four = 4,

            /// <summary>
            /// 5 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Five = 5,
        }

        /// <summary>
        /// Number of frames constituting a phase
        /// <para>
        /// The parameter establishes the total number of frames acquired within an acquisition phase.
        /// It is relevant only when AcquisitionMode is HFR.The range of values is [2..256].
        /// </para>
        /// </summary>
        [Serializable]
        public enum FrameNoPerPhase
        {
            /// <summary>
            /// 1 Frame / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            One = 1,

            /// <summary>
            /// 2 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Two = 2,

            /// <summary>
            /// 3 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Three = 3,

            /// <summary>
            /// 4 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Four = 4,

            /// <summary>
            /// 5 Frames / phase ( the total number of frames acquired within an acquisition phase )
            /// </summary>
            Five = 5,
        }

        /// <summary>
        /// Camera의 Trigger Event
        /// </summary>
        [Serializable]
        public enum ForceTrig
        {
            /// <summary>
            /// 
            /// </summary>
            Trig,

            /// <summary>
            /// 
            /// </summary>
            EndTrig,
        }

        /// <summary>
        /// Camera의 Trigger Edge Status
        /// </summary>
        [Serializable]
        public enum TrigEdge
        {
            /// <summary>
            /// 
            /// </summary>
            GoHigh,

            /// <summary>
            /// 
            /// </summary>
            GoLow,

            /// <summary>
            /// 
            /// </summary>
            GoOpen,
        }

        /// <summary>
        /// Camera의 Channel Trigger Line ( Values applicable to Grablink Full, Grablink Full XR, Grablink DualBase, Grablink Base )
        /// </summary>
        [Serializable]
        public enum TriggerLine
        {
            /// <summary>
            /// This value (default) is applicable when TrigCtl=DIFF or ISO.
            /// <para>
            /// When TrigCtl=DIFF, it designates the DIN2 line. 
            /// When TrigCtl=ISO, it designates the IIN2 line. 
            /// </para>
            /// </summary>
            Nom,

            /// <summary>
            /// Differential high-speed input lines (two lines per camera). This value is applicable only when TrigCtl=DIFF.
            /// </summary>
            Din1,

            /// <summary>
            /// Differential high-speed input lines (two lines per camera). This value is applicable only when TrigCtl=DIFF.
            /// </summary>
            Din2,

            /// <summary>
            /// Isolated current loop input lines (four lines per camera). This value is applicable only when TrigCtl=ISO.
            /// </summary>
            Iin1,

            /// <summary>
            /// Isolated current loop input lines (four lines per camera). This value is applicable only when TrigCtl=ISO.
            /// </summary>
            Iin2,

            /// <summary>
            /// Isolated current loop input lines (four lines per camera). This value is applicable only when TrigCtl=ISO.
            /// </summary>
            Iin3,

            /// <summary>
            /// Isolated current loop input lines (four lines per camera). This value is applicable only when TrigCtl=ISO.
            /// </summary>
            Iin4,
        }

        /// <summary>
        /// Camera의 Channel Trigger Control ( Values applicable to Grablink Full, Grablink Full XR, Grablink DualBase, Grablink Base )
        /// </summary>
        [Serializable]
        public enum TriggerControl
        {
            /// <summary>
            /// Differential high-speed input compatible with EIA/TIA-422 signaling.
            /// </summary>
            Diff,

            /// <summary>
            /// Isolated current loop input compatible with TTL, +12V, +24V signaling. Default value.
            /// </summary>
            Iso,
        }
        #endregion

        #region Field
        /// <summary>
        /// The MultiCam object that controls the acquisition
        /// </summary>
        private int m_Channel;

        /// <summary>
        /// Track if acquisition is ongoing
        /// </summary>
        private volatile bool m_ChannelActive;

        /// <summary>
        /// Track if acquisition is finished
        /// </summary>
        private volatile bool m_AcquisitionFinished;

        /// <summary>
        /// Track if exposure is finished
        /// </summary>
        private volatile bool m_ExposureFinished;

        /// <summary>
        /// MultiCam Callback
        /// </summary>
        private MultiCam.CallBack m_MulticamCallback;
        private string PreviousMode;
        private bool m_GrabNewImageEventEnable;
        private const int RetryCount = 3;
        private MultiCamCamera.FrameNoPerSequence m_SettingFrameCount;
        #endregion

        #region Event
        public event MultiCamCameraEventHandler GrabNewImage;
        #endregion

        #region Constructor
        public MultiCamCamera() : this("") { }
        public MultiCamCameraConfig MultiCamCameraConfig 
        { 
            get
            {
                return Config as MultiCamCameraConfig;
            }
            set
            {
                Config = value;
            }
        }
        public MultiCamCamera(string strName) : base(strName)
        {
            Config = new MultiCamCameraConfig();
            this.m_MulticamCallback = null;
            this.GrabNewImageEventEnable = true;
            this.SettingFrameCount = FrameNoPerSequence.One;
            this.BoardIndex = 0;
            this.ColorFormat = ColorFormats.Y8;

            this.TrigLine = TriggerLine.Nom;
            this.TrigMode = TriggerMode.Immediate;
            this.NextTrigMode = NextTriggerMode.Combined;
            this.SurfaceCount = 3;
        }        
        #endregion

        #region Property
        /// <summary>
        /// Camera에서 생성된 Channel 정보
        /// </summary>
		[DefaultValue(0)]
        public int Channel
        {
            get { return MultiCamCameraConfig.m_Channel; }
            set { MultiCamCameraConfig.m_Channel = value; }
        }

        /// <summary>
        /// EureSys Camera의 Channel Active Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool ChannelActive
        {
            get { return MultiCamCameraConfig.m_ChannelActive; }
            private set { MultiCamCameraConfig.m_ChannelActive = value; }
        }

        /// <summary>
        /// EureSys Camera의 Acquisition 완료 Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool AcquisitionFinished
        {
            get { return MultiCamCameraConfig.m_AcquisitionFinished; }
            private set
            {
                if (MultiCamCameraConfig.m_AcquisitionFinished == value) return;
                //this.WriteLog(LogLevel.Lowest, string.Format("AcquisitionFinished Value :{0} -> {1}", m_AcquisitionFinished, value));
                Console.WriteLine(string.Format("AcquisitionFinished Value :{0} -> {1}", m_AcquisitionFinished, value));
                MultiCamCameraConfig.m_AcquisitionFinished = value;
            }
        }

        /// <summary>
        /// EureSys Camera의 Exposure 완료 Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool ExposureFinished
        {
            get { return MultiCamCameraConfig.m_ExposureFinished; }
            private set
            {
                if (MultiCamCameraConfig.m_ExposureFinished == value) return;
                //this.WriteLog(LogLevel.Lowest, string.Format("ExposureFinished Value :{0} -> {1}", m_ExposureFinished, value));
                Console.WriteLine(string.Format("ExposureFinished Value :{0} -> {1}", m_ExposureFinished, value));
                MultiCamCameraConfig.m_ExposureFinished = value;
            }
        }

        /// <summary>
        /// GrabNewImageEvent 사용 여부에 대해서 가져오거나 설정한다.
        /// </summary>
        public bool GrabNewImageEventEnable
        {
            get { return MultiCamCameraConfig.m_GrabNewImageEventEnable; }
            set { MultiCamCameraConfig.m_GrabNewImageEventEnable = value; }
        }

        public FrameNoPerSequence SettingFrameCount
        {
            get { return MultiCamCameraConfig.m_SettingFrameCount; }
            private set { MultiCamCameraConfig.m_SettingFrameCount = value; }
        }

        #region ConstructConfiguration
        
        
        [Category("Channel")]
        public string CamFilePath
        {
            get { return MultiCamCameraConfig.CamFilePath; }
            set { MultiCamCameraConfig.CamFilePath = value; }
        }

        [Category("Channel")]
        public Enum Connector
        {
            get { return MultiCamCameraConfig.Connector; }
            set { MultiCamCameraConfig.Connector = value; }
        }

        [Category("Channel")]
        public ColorFormats ColorFormat
        {
            get { return MultiCamCameraConfig.ColorFormat; }
            set { MultiCamCameraConfig.ColorFormat = value; }
        }

        [Category("Channel")]
        public TriggerLine TrigLine
        {
            get { return MultiCamCameraConfig.TrigLine; }
            set { MultiCamCameraConfig.TrigLine = value; }
        }
        [Category("Channel")]
        public TriggerMode TrigMode 
        {
            set { MultiCamCameraConfig.TrigMode = value; }
            get { return MultiCamCameraConfig.TrigMode; } 
        }
        [Category("Channel")]
        public NextTriggerMode NextTrigMode
        {
            get { return MultiCamCameraConfig.NextTrigMode; }
            set { MultiCamCameraConfig.NextTrigMode = value; }
        }
        [Category("Board")]
        public Enum BoardTopology 
        {
            set { MultiCamCameraConfig.BoardTopology = value;  }
            get { return MultiCamCameraConfig.BoardTopology; }
        }
        [Category("Board")]
        public uint BoardIndex 
        {
            set { MultiCamCameraConfig.BoardIndex = value; }
            get { return MultiCamCameraConfig.BoardIndex; } 
        }
        [Category("Channel")]
        public int SurfaceCount
        {
            get { return MultiCamCameraConfig.SurfaceCount; }
            set { MultiCamCameraConfig.SurfaceCount = value; }
        }
        
        

       

        #endregion
        #endregion

        #region Method
        public int CheckConnect()
        {
            int ret = 0;
            string channelState = string.Empty;

            try
            {
                if (MultiCam.GetParam(this.Channel, (int)MultiCam.GetParameter.ChannelState, out channelState) == 0) return ret;
            }
            catch(Exception)
            {
            }

            for (int i = 0; i < RetryCount; i++)
            {
                if ((ret = this.Reconnect()) != 0) continue;

                break;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        private int MultiCamCallback(ref MultiCam.SIGNALINFO signalInfo)
        {
            int ret = 0;
            try
            {

                //this.WriteLog(LogLevel.Lowest, string.Format("{0} MultiCamCallback() : {1}", this.Name, signalInfo.Signal));
                //Console.WriteLine(string.Format("{0} MultiCamCallback() : {1}", this.Name, signalInfo.Signal));
               
                switch (signalInfo.Signal)
                {
                    case (int)MultiCam.Signals.Any:
                        break;
                    case (int)MultiCam.Signals.SurfaceProcessing:
                        ret = ProcessingCallback(signalInfo);
                        break;
                    case (int)MultiCam.Signals.SurfaceFilled:
                        break;
                    case (int)MultiCam.Signals.UnrecoverableOverrun:
                        break;
                    case (int)MultiCam.Signals.FrameTriggerViolation:
                        ret = FrameTriggerViolationCallback(signalInfo);
                        break;
                    case (int)MultiCam.Signals.StartExposure:
                        break;
                    case (int)MultiCam.Signals.EndExposure:
                        this.ExposureFinished = true;
                        break;
                    case (int)MultiCam.Signals.AcquisitionFailure:
                        ret = AcqFailureCallback(signalInfo);
                        break;
                    case (int)MultiCam.Signals.ClusterUnavailable:
                        break;
                    case (int)MultiCam.Signals.Release:
                        break;
                    case (int)MultiCam.Signals.EndAcquitionSequence:
                        break;
                    case (int)MultiCam.Signals.StartAcquisitionSequence:
                        break;
                    case (int)MultiCam.Signals.EndChannelActivity:
                        this.ChannelActive = false;
                        break;
                    default:
                        //this.WriteLog(LogLevel.Highest, string.Format("{0} MultiCamCallback() : Unknown signal", this.Name));
                        Console.WriteLine(string.Format("{0} MultiCamCallback() : Unknown signal", this.Name));
                        throw new MultiCamException("Unknown signal");
                }
            }
            catch (MultiCamException ex)
            {
                Console.WriteLine(string.Format("MultiCamException : {0}", ex.Message));
                ret = -1;
            }

            return ret;     // 각각의 case 문에 대한 return 처리는 고민해보자...
        }

        /// <summary>
        /// Callback 함수에서 카메라 원본데이터를 VisionImage 형식으로 변환
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        private int ProcessingCallback(MultiCam.SIGNALINFO signalInfo)
        {
            int ret = 0;

            int currentSurface = 0;
            IntPtr bufferAddress = IntPtr.Zero;
            VisionImage image = null;
            StopWatch stopWatch = new StopWatch();

            try
            {
                stopWatch.Start();

                // The MultiCam object that contains the acquired buffer
                currentSurface = (int)signalInfo.SignalInfo;

                // Get BufferAddress
                if ((ret = this.CheckReturnCode(MultiCam.GetParam(currentSurface, (int)MultiCam.GetParameter.SurfaceMemoryImageBufferAddr, out bufferAddress), MultiCamCamera.AlarmKeys.ProcessingCallbackFailed)) != 0) return ret;

                // image 
                if ((ret = this.CreateVisionImage(bufferAddress, out image)) != 0) return ret;

                this.LatestImage = image;
                this.AcquisitionFinished = true;
                //this.WriteLog(LogLevel.Lowest, string.Format("ProcessingCallback End", this));
                //Console.WriteLine(string.Format("ProcessingCallback End", this));
                if (this.TrigMode != TriggerMode.Immediate)
                    this.OnGrabNewImage(new MultiCamCameraCameraEventArgs());
                stopWatch.Stop();
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        private int AcqFailureCallback(MultiCam.SIGNALINFO signalInfo)
        {
            int ret = 0;
            int currentChannel = (int)signalInfo.Context;
            Console.WriteLine(string.Format("{0} AcqFailureCallback()", this.Name));
            return ret;
        }

        /// <summary>
        /// Trigger를 실패할 경우 발생하는 Callback
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        private int FrameTriggerViolationCallback(MultiCam.SIGNALINFO signalInfo)
        {
            int ret = 0;

            return ret;
        }

        /// <summary>
        /// Camera Acquisition Mode를 Snapshot Mode로 전환한다.
        /// </summary>
        /// <returns></returns>
        private int SetSnapshotMode()
        {
            int ret = 0;

            try
            {
                // Choose the acquisition mode
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionMode, (int)MultiCamCamera.AcquisitionMode.Snapshot), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the number of acquisition sequences constituting a channel activity period
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceCount, (int)MultiCamCamera.ActivitySequenceCount.One), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the total number of frames acquired within an acquisition phase
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionPhaseFrameCount, (int)MultiCamCamera.FrameNoPerPhase.One), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the way the first acquisition is triggered
                //if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, (string)EureSysFrameGrabberCamera.TriggerMode.Immediate.ToString().ToUpper()), EureSysFrameGrabberCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, (string)this.TrigMode.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the triggering mode for subsequent acquisitions
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.SubsequentAcquisitionTriggerMode, (string)MultiCamCamera.NextTriggerMode.Same.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the way the first acquisition is triggered
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.LastAcquisitionTriggerMode, (string)MultiCamCamera.EndTriggerMode.Auto.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the number of images to acquire
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceFrameCount, (int)MultiCamCamera.FrameNoPerSequence.IndefiniteRepeat), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the effect of a user break on the channel
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionChannelBreakEffect, (string)MultiCamCamera.BreakEffect.Finish.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// Camera Acquisition Mode를 Video Mode로 전환한다.
        /// </summary>
        /// <returns></returns>
        private int SetVideoMode()
        {
            int ret = 0;

            try
            {
                // Choose the acquisition mode
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionMode, (string)MultiCamCamera.AcquisitionMode.Video.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;

                // Choose the number of acquisition sequences constituting a channel activity period
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceCount, (int)MultiCamCamera.ActivitySequenceCount.One), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;

                // Choose the total number of frames acquired within an acquisition phase
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionPhaseFrameCount, (int)MultiCamCamera.FrameNoPerPhase.One), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;

                // Choose the way the first acquisition is triggered
                //if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, (string)EureSysFrameGrabberCamera.TriggerMode.Immediate.ToString().ToUpper()), EureSysFrameGrabberCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, (string)this.TrigMode.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetSnapshotModeFailed)) != 0) return ret;

                // Choose the triggering mode for subsequent acquisitions
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.SubsequentAcquisitionTriggerMode, (string)MultiCamCamera.NextTriggerMode.Periodic.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;

                // Choose the way the first acquisition is triggered
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.LastAcquisitionTriggerMode, (string)MultiCamCamera.EndTriggerMode.Auto.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;

                // Choose the number of images to acquire
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceFrameCount, (int)MultiCamCamera.FrameNoPerSequence.IndefiniteRepeat), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;

                // Choose the effect of a user break on the channel
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionChannelBreakEffect, (string)MultiCamCamera.BreakEffect.Finish.ToString().ToUpper()), MultiCamCamera.AlarmKeys.SetVideoModeFailed)) != 0) return ret;
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// Image가 갱신되었을 경우 이벤트가 발생한다.
        /// 단, TriggerMode가 Immediate일 경우에는 발생하지 않음.
        /// </summary>
        /// <param name="e"></param>
        private void OnGrabNewImage(MultiCamCameraCameraEventArgs e)
        {
            try
            {
                if (this.GrabNewImageEventEnable == false) return;
                if (this.GrabNewImage != null)
                    this.GrabNewImage(this, e);
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnGrabNewImage][{this.Alias}] Camera Exception! Message = {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Camera Members
        protected override int OnGetFrameRate(ref double frameRate)
        {
            int ret = 0;
            int value = 0;
            try
            {
                if ((ret = this.CheckReturnCode(MultiCam.GetParam(this.Channel, (int)MultiCam.GetParameter.AcquisitionFramePerSecond, out value), Camera.AlarmKeys.GetFrameRateFailed)) != 0) return ret;
                frameRate = value;
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnGetFrameRate][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnGetFrameRate Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }

        protected override int OnSetFrameRate(double frameRate)
        {
            int ret = 0;

            try
            {
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.GetParameter.AcquisitionFramePerSecond, frameRate), Camera.AlarmKeys.SetFrameRateFailed)) != 0) return ret;
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnSetFrameRate][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnSetFrameRate Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }

        protected override int OnGetMaxFrameRate(ref RangeD frameRate)
        {
            int ret = 0;
            try
            {
                //if ((ret = this.CheckReturnCode(MultiCam.GetParam(this.Channel, (int)MultiCam.GetParameter.AcquisitionFramePerSecond, value), Camera.AlarmKeys.SetFrameRateFailed)) != 0) return;
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnGetMaxFrameRate][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnGetMaxFrameRate Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }

        protected override int OnGetExposureTime(ref double exposureTime)
        {
            int ret = 0;
            try
            {

            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnGetExposureTime][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnGetExposureTime Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }

        protected override int OnSetExposureTime(double exposureTime)
        {
            int ret = 0;
            try
            {

            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnSetExposureTime][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnSetExposureTime Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }

        protected override int OnOpen()
        {
            int ret = 0;
            int channelValue = 0;

            //if (this.Simulation.IsSimulatedWithoutResource() == true) return ret;
            try
            {
                if ((ret = this.CheckReturnCode(MultiCam.Create(MultiCamCamera.Models.Channel.ToString().ToUpper(), out channelValue), Camera.AlarmKeys.OpenFailed)) != 0) return ret;
                else
                    this.Channel = channelValue;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.DriverIndex, (int)this.BoardIndex), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.Connector, this.Connector.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                // Choose the video standard
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.CamFile, this.CamFilePath), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                //// Choose the pixel color format
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ColorFormat, this.ColorFormat.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.HardwareTriggerLine, this.TrigLine.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                this.SetSnapshotMode();

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ImageSizeX, this.CameraResolution.Width), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.Hactive_Px, this.CameraResolution.Width), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ImageSizeY, this.CameraResolution.Height), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.Vactive_Ln, this.CameraResolution.Height), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.BufferPitch, this.CameraResolution.Width), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.BufferSize, this.CameraResolution.Width * this.CameraResolution.Height), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ImageFlipX, this.ImageFlipX.ToString()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ImageFlipY, this.ImageFlipY.ToString()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.SurfaceCount, this.SurfaceCount), Camera.AlarmKeys.GrabFailed)) != 0) return ret;

                if (this.m_MulticamCallback != null)
                {
                    // Disable the signals corresponding to the callback functions
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.SurfaceProcessing, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.AcquisitionFailure, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.EndChannelActivity, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.EndExposure, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.StartExposure, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.FrameTriggerViolation, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;
                    // Unregister the callback function
                    this.m_MulticamCallback = null;
                    MultiCam.RegisterCallback(this.Channel, this.m_MulticamCallback, this.Channel);
                }

                // Register the callback function
                this.m_MulticamCallback = new MultiCam.CallBack(this.MultiCamCallback);
                MultiCam.RegisterCallback(this.Channel, this.m_MulticamCallback, this.Channel);

                // Enable the signals corresponding to the callback functions
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.SurfaceProcessing, MultiCam.Signals.On.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.AcquisitionFailure, MultiCam.Signals.On.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.EndChannelActivity, MultiCam.Signals.On.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.EndExposure, MultiCam.Signals.On.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.StartExposure, MultiCam.Signals.On.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.FrameTriggerViolation, MultiCam.Signals.On.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                //// Prepare the channel in order to minimize the acquisition sequence startup latency
                //if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceFrameCount, (int)EureSysFrameGrabberCamera.FrameNoPerSequence.One), Camera.AlarmKeys.OpenFailed)) != 0) return ret;
                //if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceFrameCount, (int)MultiCamCamera.FrameNoPerSequence.One), Camera.AlarmKeys.GrabFailed)) != 0) return ret;
                //this.SettingFrameCount = FrameNoPerSequence.One;

                //if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, MultiCam.SetParameter.ChannelState.ToString(), EureSysFrameGrabberCamera.ChannelState.Idle.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, MultiCam.SetParameter.ChannelState.ToString(), MultiCamCamera.ChannelState.Idle.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, MultiCam.SetParameter.ChannelState.ToString(), MultiCamCamera.ChannelState.Ready.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, MultiCam.SetParameter.ChannelState.ToString(), MultiCamCamera.ChannelState.Active.ToString().ToUpper()), Camera.AlarmKeys.GrabFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceFrameCount, (int)MultiCamCamera.FrameNoPerSequence.IndefiniteRepeat), Camera.AlarmKeys.GrabFailed)) != 0) return ret;

            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            this.ChannelActive = false;
            this.AcquisitionFinished = false;
            this.ExposureFinished = false;

            return ret;
        }

        protected override int OnClose()
        {
            int ret = 0;
            //TimeoutChecker timeout = null;

            //if (this.Simulation.IsSimulatedWithoutResource() == true) return ret;

            //timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.ConstructConfiguration.SignalWatingTime), true);

            try
            {
                //while (timeout.IsCompleted == false)
                //{
                //    SafeThread.Sleep();
                //    if (this.ChannelActive == false) break;
                //}

                if (this.IsLiveOn == true)
                    this.StopLive();

                //if (timeout.IsCompleted == true)
                //{
                //    this.WriteLog(LogLevel.Highest, "ths channel state is still activated... so it isn't able to close the channel...");
                //}

                if (this.m_MulticamCallback != null)
                {
                    // Disable the signals corresponding to the callback functions
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.SurfaceProcessing, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.CloseFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.AcquisitionFailure, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.CloseFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.EndChannelActivity, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.CloseFailed)) != 0) return ret;

                    //Test Add
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.EndExposure, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.CloseFailed)) != 0) return ret;

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.Signals.Enable + (int)MultiCam.Signals.StartExposure, MultiCam.Signals.Off.ToString().ToUpper()), Camera.AlarmKeys.CloseFailed)) != 0) return ret;

                    // Unregister the callback function
                    this.m_MulticamCallback = null;
                    MultiCam.RegisterCallback(this.Channel, this.m_MulticamCallback, this.Channel);
                }

                if (this.Channel != 0)
                {
                    if ((ret = this.CheckReturnCode(MultiCam.Delete(this.Channel), Camera.AlarmKeys.CloseFailed)) != 0) return ret;
                    else
                        this.Channel = 0;
                }
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        protected override int OnGrab(out VisionImage image)
        {
            int ret = 0;
            TimeoutChecker timeout = null;
            bool isLive = this.IsLiveOn;
            StopWatch stopWatch = new StopWatch();

            image = null;

            //if (this.Simulation.IsSimulatedWithoutResource() == true) return ret;
            
            try
            {
                if (isLive == true)
                    this.StopLive();

                this.CheckConnect();
                this.AcquisitionFinished = false;
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ChannelState, MultiCamCamera.ChannelState.Ready.ToString().ToUpper()), Camera.AlarmKeys.GrabFailed)) != 0) return ret;
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerEventForce, "TRIG"), Camera.AlarmKeys.GrabFailed)) != 0) return ret;
                

                #region ProcessingCallback Check 
                

                timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.SignalWatingTime), true);

                while (timeout.IsCompleted == false)
                {
                    if (this.AcquisitionFinished == true)
                    {
                        break;
                    }
                }

                if (timeout.IsCompleted == true) return -1;
                #endregion

                image = this.LatestImage;

                #region EndChannelActivity Check
                timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.SignalWatingTime), true);

                while (timeout.IsCompleted == false)
                {
                    Thread.Sleep(0);
                    if (this.ChannelActive == false)
                    {
                        break;
                    }
                }
                #endregion

                if (isLive == true)
                    this.StartLive();
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        protected override int OnStartLive()
        {
            int ret = 0;
            string channelState;
            string value = "";

            try
            {
                this.CheckConnect();

                if ((ret = this.CheckReturnCode(MultiCam.GetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, out value), Camera.AlarmKeys.StartLiveFailed)) != 0) return ret;

                this.PreviousMode = value;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, (string)MultiCamCamera.TriggerMode.Immediate.ToString().ToUpper()), Camera.AlarmKeys.StartLiveFailed)) != 0) return ret;

                if (this.SettingFrameCount != FrameNoPerSequence.IndefiniteRepeat)
                {
                    // Choose the number of images to acquire
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionSequenceFrameCount, (int)MultiCamCamera.FrameNoPerSequence.IndefiniteRepeat), Camera.AlarmKeys.StartLiveFailed)) != 0) return ret;

                    this.SettingFrameCount = FrameNoPerSequence.IndefiniteRepeat;
                }

                //Start an acquisition sequence by activating the channel
                if ((ret = this.CheckReturnCode(MultiCam.GetParam(this.Channel, (int)MultiCam.GetParameter.ChannelState, out channelState), Camera.AlarmKeys.StartLiveFailed)) != 0) return ret;

                if (channelState != MultiCamCamera.ChannelState.Active.ToString().ToUpper())
                {
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ChannelState, MultiCamCamera.ChannelState.Active.ToString().ToUpper()), Camera.AlarmKeys.StartLiveFailed)) != 0) return ret;
                }
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            this.ChannelActive = true;

            return ret;
        }

        protected override int OnStopLive()
        {
            int ret = 0;
            try
            {
                this.CheckConnect();

                if (this.Channel != 0)
                {
                    // Stop an acquisition sequence by deactivating the channel
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ChannelState, MultiCamCamera.ChannelState.Idle.ToString().ToUpper()), Camera.AlarmKeys.StopLiveFailed)) != 0) return ret;
                }

                if (this.PreviousMode != MultiCamCamera.TriggerMode.Immediate.ToString().ToUpper())
                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerMode, this.PreviousMode), Camera.AlarmKeys.StopLiveFailed)) != 0) return ret;
            }
            catch (MultiCamException ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        protected override int OnReconnect()
        {
            int ret = 0;

            this.Close();
            if ((ret = this.Open()) != 0) return ret;

            return ret;
        }
        #endregion

        #region ExposeCamera Members
        protected override int OnExpose()
        {
            int ret = 0;
            int checkRet = 0;
            StopWatch sw = new StopWatch();
            TimeoutChecker timeout = null;
            
            this.ExposureFinished = false;
            this.AcquisitionFinished = false;

            try
            {
                //Trigget부터 EndExpose까지의 시간을 측정한다.
                sw.Start();
                for (int i = 0; i < RetryCount; i++)
                {
                    checkRet = this.CheckConnect();
                    if (checkRet != 0 && i < RetryCount)
                    {
                        continue;
                    }
                    else if (checkRet != 0 && i <= RetryCount)
                    {
                        //if ((ret = this.Alarms[Camera.AlarmKeys.OpenFailed].Post(this)) != 0) return ret;
                        ret = -1;
                        return ret;
                    }

                    if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.AcquisitionTriggerEventForce, "TRIG"), Camera.AlarmKeys.GrabFailed)) != 0) return ret;
                    

                    #region ExposureFinished Check 
                    timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.SignalWatingTime), true);
                    while (timeout.IsCompleted == false)
                    {
                        if (this.ExposureFinished == true) break;
                    }


                    if (checkRet == 0 && this.Channel != 0 && this.ExposureFinished == false && i < RetryCount)
                    {
                        continue;
                    }
                    else if (checkRet == 0 && this.Channel != 0 && this.ExposureFinished == false && i <= RetryCount)
                    {
                        ret = -1;
                        return ret;
                    }
                    
                    if (checkRet == 0 && this.Channel != 0 && this.ExposureFinished == true)
                    {
                        if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.ChannelState, MultiCamCamera.ChannelState.Ready.ToString().ToUpper()), Camera.AlarmKeys.GrabFailed)) != 0) return ret;
                        break;
                    }
                    #endregion
                }
                //  this.WriteLog(LogLevel.Lowest, string.Format("OnExpose ExposureFinished"), this);
                sw.Stop();
                //  this.WriteLog(LogLevel.Lowest, string.Format("Expose time : {0}", sw.Elapsed), this);
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnExpose][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnExpose Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }

        protected override int OnReadout(out VisionImage image)
        {
            int ret = 0;
            int checkRet = 0;
            TimeoutChecker timeout = null;
            StopWatch stopWatch = new StopWatch();

            image = null;
            
            try
            {
                stopWatch.Start();
                for (int i = 1; i <= RetryCount; i++)
                {
                    #region AcquisitionFinished Check 
                    timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.SignalWatingTime), true);

                    while (timeout.IsCompleted == false)
                    {
                        if (this.AcquisitionFinished == true) break;
                    }

                    if ((checkRet = this.CheckConnect()) == 0 && this.AcquisitionFinished == true)
                    {
                        this.AcquisitionFinished = false;
                        break;
                    }
                    else if (checkRet == 0 && this.AcquisitionFinished == false && i < RetryCount)
                    {
                        if ((ret = Camera.GrabQuickly(this, out image)) != 0) return ret;
                        continue;
                    }
                    else if (checkRet == 0 && this.AcquisitionFinished == false && i <= RetryCount)
                    {
                        //if ((ret = this.Alarms[AlarmKeys.ProcessingCallbackTimeOut].Post(this)) != 0) return ret;
                        ret = -1;
                        return ret;
                    }
                    else if(checkRet != 0 && i < RetryCount)
                    {
                        continue;
                    }
                    else if (checkRet != 0 && i <= RetryCount)
                    {
                        //if ((ret = this.Alarms[Camera.AlarmKeys.OpenFailed].Post(this)) != 0) return ret;
                        ret = -1;
                        return ret;
                    }
                }
                #endregion

                image = this.LatestImage;

                stopWatch.Stop();
                // this.WriteLog(LogLevel.Lowest, string.Format("Readout time : {0}", stopWatch.Elapsed), this);
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnReadout][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnReadout Error Detected");
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }
        #endregion

        #region IPartConfigurable Members

        public override int Create()
        {
            int ret = 0;
            string boardTopology = "";

            if ((ret = base.Create()) != 0) return ret;

            try
            {
                // Open MultiCam driver
                if ((ret = this.CheckReturnCode(MultiCam.OpenDriver(), Camera.AlarmKeys.CreateFailed)) != 0) return ret;

                // Enable error logging
                if ((ret = this.CheckReturnCode(MultiCam.SetParam((int)MultiCam.DefaultConstants.Configuration, (int)MultiCam.SetParameter.ErrorLog, MultiCamCamera.ErrorLog.ErrorLog.ToString()), Camera.AlarmKeys.CreateFailed)) != 0) return ret;

                if (this is GrabLinkMultiCamCamera)
                {
                    //boardTopology = this.BoardTopology.ToString().ToUpper();
                }
                else
                {
                    throw new InvalidCastException();
                }

                if ((ret = this.CheckReturnCode(MultiCam.SetParam((uint)MultiCam.DefaultConstants.Board + this.BoardIndex, (int)MultiCam.SetParameter.BoardTopology, boardTopology), Camera.AlarmKeys.CreateFailed)) != 0) return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ret;
        }
        public override void Close()
        {
            try
            {
                base.Close();
                // Close MultiCam driver
                this.CheckReturnCode(MultiCam.CloseDriver(), Camera.AlarmKeys.TerminateFailed);
            }
            catch (Exception ex)
            {
                //this.WriteLog(LogLevel.Normal, ex.Message);
                Console.WriteLine(ex.Message);
            }

        }


        #endregion
    }
    #endregion

    #region MultiCamCameraConfiguration
    //[Serializable]
    //public class MultiCamCameraConfiguration : ExposeCameraConfiguration
    //{
    //    #region Constructor
    //    public MultiCamCameraConfiguration(MultiCamCamera owner)
    //        : base(owner)
    //    {

    //    }
    //    public MultiCamCameraConfiguration() : this(null) { }
    //    #endregion

    //    #region PartConfiguration Member
    //    public new MultiCamCameraConfigurationBody Body
    //    {
    //        get { return base.Body as MultiCamCameraConfigurationBody; }
    //        set { base.Body = value; }
    //    }
    //    #endregion

    //    #region Configuration Member
    //    protected override void SetDefaultValues()
    //    {
    //        base.SetDefaultValues();

    //        base.Body = new MultiCamCameraConfigurationBody();
    //    }
    //    #endregion
    //}

    //[Serializable]
    //public class MultiCamCameraConfigurationBody : ExposeCameraConfigurationBody
    //{
    //    #region Define
    //    #endregion

    //    #region Field
    //    #endregion

    //    #region Constructor
    //    public MultiCamCameraConfigurationBody() : base()
    //    {
    //    }
    //    #endregion

    //    #region Property
    //    #endregion
    //}
    #endregion

    #region MultiCamCameraConstructConfiguration
    //[Serializable]
    //public class MultiCamCameraConstructConfiguration : ExposeCameraConstructConfiguration
    //{
    //    #region Field
    //    private string m_CamFile;
    //    private uint m_BoardIndex;
    //    private MultiCamCamera.ColorFormats m_ColorFormat;
    //    private Enum m_Connector;
    //    private MultiCamCamera.TriggerLine m_TriggerLine;
    //    private MultiCamCamera.TriggerMode m_TriggerMode;
    //    private MultiCamCamera.NextTriggerMode m_NextTriggerMode;
    //    private Enum m_BoardTopology;
    //    private int m_SurfaceCount;
    //    #endregion

    //    #region Constructor
    //    public MultiCamCameraConstructConfiguration(PartConstructMethod constructMethod)
    //        : base(constructMethod)
    //    {
    //    }
    //    public MultiCamCameraConstructConfiguration() : this(PartConstructMethod.Static) { }
    //    #endregion

    //    #region Property
    //    /// <summary>
    //    /// camera configuration file 
    //    /// </summary>
    //    [Category("Channel")]
    //    [Editor(typeof(FilePathSelectEditor<string>), typeof(UITypeEditor))]
    //    public string CamFilePath
    //    {
    //        get { return this.m_CamFile; }
    //        set { this.m_CamFile = value; }
    //    }

    //    /// <summary>
    //    /// Indication of connector used by channel.
    //    /// </summary>
    //    [Category("Channel")]
    //    public Enum Connector
    //    {
    //        get { return this.m_Connector; }
    //        set { this.m_Connector = value; }
    //    }

    //    /// <summary>
    //    /// Designation of color format
    //    /// </summary>
    //    [Category("Channel")]
    //    public MultiCamCamera.ColorFormats ColorFormat
    //    {
    //        get { return this.m_ColorFormat; }
    //        set { this.m_ColorFormat = value; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Category("Channel")]
    //    public MultiCamCamera.TriggerLine TriggerLine
    //    {
    //        get { return this.m_TriggerLine; }
    //        set { this.m_TriggerLine = value; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Category("Channel")]
    //    public MultiCamCamera.TriggerMode TriggerMode
    //    {
    //        get { return this.m_TriggerMode; }
    //        set { this.m_TriggerMode = value; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Category("Channel")]
    //    public MultiCamCamera.NextTriggerMode NextTriggerMode
    //    {
    //        get { return this.m_NextTriggerMode; }
    //        set { this.m_NextTriggerMode = value; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Category("Board")]
    //    public Enum BoardTopology
    //    {
    //        get { return this.m_BoardTopology; }
    //        set { this.m_BoardTopology = value; }
    //    }

    //    [Category("Board")]
    //    public uint BoardIndex
    //    {
    //        get { return this.m_BoardIndex; }
    //        set { this.m_BoardIndex = value; }
    //    }


    //    [Category("Channel")]
    //    public int SurfaceCount
    //    {
    //        get { return this.m_SurfaceCount; }
    //        set { this.m_SurfaceCount = value; }
    //    }
    //    #endregion

    //    #region ConstructConfiguration Members
    //    protected override void SetDefaultValues()
    //    {
    //        base.SetDefaultValues();

    //        this.BoardIndex = 0;
    //        this.ColorFormat = MultiCamCamera.ColorFormats.Y8;

    //        this.TriggerLine = MultiCamCamera.TriggerLine.Nom;
    //        this.TriggerMode = MultiCamCamera.TriggerMode.Immediate;
    //        this.NextTriggerMode = MultiCamCamera.NextTriggerMode.Combined;
    //        this.SurfaceCount = 3;
    //    }
    //    #endregion
    //}
    #endregion

    #region MultiCamCameraCameraEventArgs
    [Serializable]
    public class MultiCamCameraCameraEventArgs : EquipmentEventArgs
    {
        #region Field
        #endregion

        #region Constructor

        public MultiCamCameraCameraEventArgs()
        {
 
        }
        #endregion

        #region Property
        #endregion
    }
    public delegate void MultiCamCameraEventHandler(object sender, MultiCamCameraCameraEventArgs e);
    #endregion
}