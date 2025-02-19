using QMC.Common;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;




namespace QMC.Common.Vision.EureSys
#region EuresysCoaxlinkCamera
#endregion

{
    public class CoaxlinkCamera : Camera
    {

        #region Define
        [Serializable]
        public new enum AlarmKeys
        {
            DeviceError
        }

        [Serializable]
        public enum OnOff
        {
            On,
            Off
        }

        [Description("\n Immediate : Normal Mode. \n Hard : Hard Trigger Mode. \n Soft : Soft Trigger Mode.")]
        public enum TriggerMode
        {
            Immediate,
            Hard,
            Soft
        }


        #endregion

        #region Field
        //private Euresys.EGrabberCallbackOnDemand m_Grabber;
        private Euresys.EGrabberCallbackMultiThread m_Grabber;
        private volatile bool m_AcquisitionFinished;
        private volatile bool m_ExposureFinished;


        #endregion
        //private const int RetryCount = 3;

        #region Constructor
        public CoaxlinkCamera(string strName)
             : base(strName)
        {
            this.Config = new CoaxlinkCameraConfig();
            this.AcquisitionFinished = false;
            this.ExposureFinished = false;

        }
        public CoaxlinkCamera() : this("Camera") { }




        #endregion

        #region Property
        //protected Euresys.EGrabberCallbackOnDemand Grabber
        //{
        //    get { return this.m_Grabber; }
        //    private set { this.m_Grabber = value; }
        //}

        public Euresys.EGrabberCallbackMultiThread Grabber
        {
            get { return this.m_Grabber; }
            set { this.m_Grabber = value; }
        }

        public bool AcquisitionFinished
        {
            get { return this.m_AcquisitionFinished; }
            private set { this.m_AcquisitionFinished = value; }
        }

        public bool ExposureFinished
        {
            get { return this.m_ExposureFinished; }
            private set { this.m_ExposureFinished = value; }
        }



        //public new CoaxlinkCameraConfig Config { get; set; }
        public CoaxlinkCameraConfig CoaxlinkCameraConfig
        {
            get
            {
                return Config as CoaxlinkCameraConfig;
            }
        }
        #endregion

        #region Event Handlers

/*
        private void NewBufferEvent(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            VisionImage image = null;
            IntPtr imgPtr;

            try
            {
                this.ExposureFinished = true;

                using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer(g, data))
                {
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);

                    this.CreateVisionImage(imgPtr, out image);

                    this.AcquisitionFinished = true;
                }
            }
            catch (System.Exception e)
            {
                Log.WriteLog(this.Name, e.Message);
            }
        }*/

        private void CallBackCIC(Euresys.EGrabberCallbackMultiThread g, Euresys.CicData data)
        {

            if (data.numid == 0x8042)
            {
                this.ExposureFinished = true;
            }
        }
        private void NewBufferEvent(Euresys.EGrabberCallbackMultiThread g, Euresys.NewBufferData data)
        {
            VisionImage image = null;
            IntPtr imgPtr;

            //this.WriteLog(LogLevel.Highest, "New Buffer");

            try
            {
                this.ExposureFinished = true;

                using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer(g, data))
                {
                    buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);

                    this.CreateVisionImage(imgPtr, out image);

                    this.LatestImage = image;

                    this.AcquisitionFinished = true;
                }
            }
            catch (System.Exception e)
            {
                Log.Write(this.Name, e.Message);
            }
        }


        #endregion

        #region Method
        public int CheckConnect()
        {
            int ret = 0;
            //string channelState = string.Empty;
            //try
            //{
            //    if (GenICam.SetParameter(this.Channel, (int)GenICam.GetParameter.ChannelState, out channelState) == 0) return ret;
            //}
            //catch (Exception)
            //{
            //}

            //for (int i = 0; i < RetryCount; i++)
            //{
            //    if ((ret = this.Reconnect()) != 0) continue;

            //    break;
            //}

            return ret;
        }

        #endregion

        #region Camera Members

        public override int Initialize()
        {
            int ret = 0;
            if((ret = base.Initialize()) != 0)
            {
                return ret;
            }

            if((ret = this.Open()) == 0)
            {
                StartLive();
            }
            
            return ret;;
        }
        protected override int OnGetFrameRate(ref double frameRate)
        {
            int ret = 0;

            this.Grabber.setStringRemoteModule(GenICam.SetRemoteParameter.AcquisitionFrameRateEnable.ToString(), OnOff.On.ToString());
            this.Grabber.setStringRemoteModule(GenICam.SetRemoteParameter.TriggerMode.ToString(), OnOff.Off.ToString());
            frameRate = this.Grabber.getFloatRemoteModule(GenICam.GetParameter.AcquisitionFrameRate.ToString());

            return ret;
        }

        protected override int OnSetFrameRate(double frameRate)
        {
            int ret = 0;

            this.Grabber.setStringRemoteModule(GenICam.SetRemoteParameter.AcquisitionFrameRateEnable.ToString(), OnOff.On.ToString());
            this.Grabber.setFloatRemoteModule(GenICam.SetRemoteParameter.AcquisitionFrameRate.ToString(), frameRate);


            return ret;
        }

        protected override int OnGetMaxFrameRate(ref RangeD frameRate)
        {
            int ret = 0;

            return ret;
        }

        protected override int OnGetExposureTime(ref double exposureTime)
        {
            int ret = 0;

            exposureTime = this.Grabber.getFloatRemoteModule(GenICam.GetParameter.ExposureTime.ToString());

            return ret;
        }

        protected override int OnSetExposureTime(double exposureTime)
        {
            int ret = 0;

            this.Grabber.setFloatRemoteModule(GenICam.SetRemoteParameter.ExposureTime.ToString(),exposureTime);// this.Config.ExposureTime);

            return ret;
        }

        protected override int OnReconnect()
        {
            int ret = 0;

            this.Grabber.setIntegerRemoteModule("DeviceConnectionSelector", 1);

            return ret;
        }

        protected override int OnGrab(out VisionImage image)
        {
            int ret = 0;

            TimeoutChecker timeout = null;

            image = null;
            IsLiveOn = false;
            try
            {
                this.Grabber.start(1);

                #region ProcessingCallback Check
                this.AcquisitionFinished = false;

                timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.CoaxlinkCameraConfig.SignalWatingTime), true);

                while (timeout.IsCompleted == false)
                {
                    if (this.AcquisitionFinished == true)
                    {
                        break;
                    }
                }
                if (timeout.IsCompleted == true)
                    return -1;
                #endregion

                image = this.LatestImage;
            }
            catch (Exception ex)
            {
                Log.Write(this.Name, "ongrab" + ex.Message);
            }


            return ret;
        }

        protected override int OnExpose()
        {
            int ret = 0;
            TimeoutChecker timeout = null;

            //if (this.simulation.issimulatedwithoutresource() == true)
            //    return ret;
            this.AcquisitionFinished = false;
            //this.Trigger = null;

            ////Trigger
            //if (this.Trigger != null)
            //{
            //    if ((ret = this.Trigger.OnSync()) != 0) return ret;
            //}
            //else
            {
                this.ExposureFinished = false;
                this.Grabber.start(1);

            }

            #region ExposureFinished Check 
            timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.CoaxlinkCameraConfig.SignalWatingTime), true);

            while (timeout.IsCompleted == false)
            {
                if (this.ExposureFinished == true)
                    break;
                // SafeThread.sleep(1);
            }


            if (timeout.IsCompleted == true)
                //   if ((ret = this.Alarms[CoaxlinkCamera.AlarmKeys.ExposeEndTimeout].post(this)) != 0)
                return ret;
            #endregion

            Log.Write(this.Name, string.Format("OnExpose ExposureFinished"));
            
            return ret;
        }

        protected override int OnReadout(out VisionImage image)
        {

            int ret = 0;
            TimeoutChecker timeout = null;
            image = null;


            try
            {
                #region ProcessingCallback Check 


                timeout = new TimeoutChecker(TimeSpan.FromMilliseconds(this.CoaxlinkCameraConfig.SignalWatingTime), true);

                while (timeout.IsCompleted == false)
                {
                    if (this.AcquisitionFinished == true)
                    {
                        break;
                    }
                    //   SafeThread.Sleep(1);
                }

                if (timeout.IsCompleted == true)
                    return -1;
                #endregion
                if (image is null)
                {
                    byte[] bytes = new byte[this.Resolution.Width * this.Resolution.Height];
                    Buffer.BlockCopy(LatestImage.RawData, 0, bytes, 0, bytes.Length);
                    this.CreateVisionImage(bytes, out image);
                }
                else
                {
                    Buffer.BlockCopy(LatestImage.RawData, 0, image.RawData, 0, image.RawData.Length);
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.Name, "OnReadout :" + ex.Message);
            }

            return ret;


        }

        protected override int OnStartLive()
        {
            int ret = 0;

            this.Grabber.start();

            return ret;
        }

        protected override int OnStopLive()
        {
            int ret = 0;

            this.Grabber.stop();

            return ret;
        }


        protected override int OnOpen()
        {
            int ret = 0;

            this.Grabber = new Euresys.EGrabberCallbackMultiThread(GenICam.Instance, CoaxlinkCameraConfig.BoardIndex, CoaxlinkCameraConfig.ChannelIndex);


            GenICam.Execute(this.Grabber, GenICam.ExecuteRemoteCommand.AcquisitionStop);
            GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.Width, CoaxlinkCameraConfig.CameraResolution.Width);
            GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.Height, CoaxlinkCameraConfig.CameraResolution.Height);

            switch (CoaxlinkCameraConfig.TriggerMode)
            {
                case TriggerMode.Immediate:
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.CameraControlMethod, CoaxlinkCameraConfig.CameraControlMethod);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.CycleTriggerSource, CoaxlinkCameraConfig.CycleTriggerSource);

                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.ReverseX, CoaxlinkCameraConfig.ImageFlipX);
                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.ReverseY, CoaxlinkCameraConfig.ImageFlipY);

                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.AcquisitionFrameRateEnable, CoaxlinkCameraConfig.AcquisitionFrameRateEnable);

                    if (CoaxlinkCameraConfig.AcquisitionFrameRateEnable == GenICam.AcquisitionFrameRateEnableType.On)
                        GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.AcquisitionFrameRateEnable, GenICam.AcquisitionFrameRateEnableType.Off);

                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.TriggerMode, GenICam.TriggerMode.Off);
                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.ExposureTime, CoaxlinkCameraConfig.ExposureTime);
                    GenICam.SetParameter(this.Grabber, GenICam.SetStreamParameter.ErrorSelector, CoaxlinkCameraConfig.ErrorSelector);
                    break;
                case TriggerMode.Hard:
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineSelector, CoaxlinkCameraConfig.LineSelector);
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineMode, GenICam.LineMode.Output);
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineSource, CoaxlinkCameraConfig.LineSource);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.StrobeDelay, 10);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.StrobeDuration, 100);

                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineSelector, GenICam.LineSelector.IIN11);
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineInputToolSelector, GenICam.LineInputToolSelector.LIN1);
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineInputToolSource, GenICam.LineInputToolSource.IIN11);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.ExposureTime, CoaxlinkCameraConfig.ExposureTime);

                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.CameraControlMethod, GenICam.CameraControlMethodType.RG);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.CycleTriggerSource, GenICam.CycleTriggerSourceType.LIN1);

                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.TriggerMode, GenICam.TriggerMode.On);
                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.ExposureMode, GenICam.ExposureMode.TriggerWidth);
                    break;
                case TriggerMode.Soft:
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineSelector, GenICam.LineSelector.IIN11);
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineInputToolSelector, GenICam.LineInputToolSelector.LIN1);
                    GenICam.SetParameter(this.Grabber, GenICam.SetInterfaceParameter.LineInputToolSource, GenICam.LineInputToolSource.IIN11);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.ExposureTime, CoaxlinkCameraConfig.ExposureTime);

                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.CameraControlMethod, GenICam.CameraControlMethodType.RG);
                    GenICam.SetParameter(this.Grabber, GenICam.SetDeviceParameter.CycleTriggerSource, GenICam.CycleTriggerSourceType.LIN1);

                    this.Grabber.setStringDeviceModule("EventNotification[CameraTriggerRisingEdge]", "true");
                    this.Grabber.setStringDeviceModule("EventNotification[CameraTriggerFallingEdge]", "true");

                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.TriggerMode, GenICam.TriggerMode.On);
                    GenICam.SetParameter(this.Grabber, GenICam.SetRemoteParameter.ExposureMode, GenICam.ExposureMode.TriggerWidth);

                    GenICam.Execute(this.Grabber, GenICam.ExecuteDeviceCommand.StartCycle);
                    break;
            }

            this.Grabber.enableAllEvent();

            this.Grabber.enableCicDataEvent();

            this.Grabber.reallocBuffers(CoaxlinkCameraConfig.BufferCount);

            this.Grabber.onNewBufferEvent = this.NewBufferEvent;

            this.Grabber.onCicEvent = this.CallBackCIC;

            this.Resolution = this.CameraResolution = CoaxlinkCameraConfig.CameraResolution;



            return ret;
        }

        protected override int OnClose()
        {
            int ret = 0;

            try
            {
                this.Grabber.stop();
                this.Grabber.disableAllEvent();
                this.Grabber.Dispose();
                this.Grabber = null;
            }
            catch (Exception ex)
            {
                Log.Write(this.Name, "OnClose : " + ex.Message);
            }

            return ret;
        }

        #endregion





    }
}
