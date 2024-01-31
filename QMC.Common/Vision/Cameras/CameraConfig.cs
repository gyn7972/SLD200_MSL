using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using QMC.Common.Vision;
using System.ComponentModel;

namespace QMC.Common.Vision.Cameras
{
    [Serializable]
    public enum ParamCameraConfigKey
    {
        Resolution,
        AutoSleepEnable,
        EnableExposure,
        SuspendedImageDisplay,
        AutoSleepLimitMin,
        DelayBeforeGrab,
        DelayAfterGrab,
        GrabRetryCount,
        SignalWatingTime,
        //PixelResolution,
        ImageFlipX,
        ImageFlipY,
        ImageRotate,
        UseCutImage,
        CutImageWidth,
        CutImageHeight,
        ScaleX,
        ScaleY,
        InvertedX,
        InvertedY,
    }


    [Serializable]
    public class CameraConfig
    {
        #region Define
        public enum AlarmKeys
        {
            OpenFailed,
            CloseFailed,
            CreateFailed,
            PrepareFailed,
            InitializeFailed,
            TerminateFailed,
            GrabFailed,
            StartLiveFailed,
            StopLiveFailed,
            NotOpened,
            GetFrameRateFailed,
            SetFrameRateFailed,
            SendCommandFailed,
            ReciveCommandFailed,
        }
        [Serializable]
        public enum BitPerPixelInfo
        {
            Gray8bpp = 8,
            Color24bpp = 24,
            Color32bpp = 32,
        }

        [Serializable]
        public enum ImageFlip
        {
            On,
            Off,
        }
        #endregion

        #region Property
        public Size Resolution 
        {
            //set
            //{ 
            //    CameraResolution = value; 
            //}
            //get
            //{
            //    return CameraResolution;
            //}

            set
            {
                if (ImageRotate == Camera.ImageRotateInfo.None)
                {
                    CameraResolution = value;
                }
                else
                {
                    CameraResolution = new Size(value.Height, value.Width);
                }

            }
            get
            {
                if (ImageRotate == Camera.ImageRotateInfo.None)
                {
                    return CameraResolution;
                }
                else
                {
                    return new Size(CameraResolution.Height, CameraResolution.Width);
                }

            }
        }
        
        public bool AutoSleepEnable
        {
            get;
            set;
        }
        
        public bool EnableExposure { set; get; }

        public bool SuspendedImageDisplay
        {
            get;
            set;
        }

        #region ConstructConfiguration
        [Category("Camera")]
        public TimeSpanInfo AutoSleepLimitMin
        {
            get;
            set;
        }
        [Category("Delay")]
        public int DelayBeforeGrab { set; get; }
        [Category("Delay")]
        public int DelayAfterGrab { set; get; }
        [Category("Camera")]
        public int GrabRetryCount { set; get; }
        [Category("Camera")]
        public Size CameraResolution
        {
            get;
            set;
        }
        [Category("Camera")]
        public int SignalWatingTime
        {
            get;
            set;
        }
        [Category("Camera")]
        public SizeD PixelResolution { set; get; }
        [Category("Camera")]
        public Camera.ImageFlip ImageFlipX
        {
            get;
            set;
        }
        [Category("Camera")]
        public Camera.ImageFlip ImageFlipY
        {
            get;
            set;
        }
        //[Category("Camera")]
        public Camera.ImageRotateInfo ImageRotate
        {
            get;
            set;
        }
        //[Category("Camera")]
        //public double MaxFrameRate
        //{
        //    get;
        //    set;
        //}
        [Category("Camera")]
        public TimeSpanInfo WaitToGrabTimeout { set; get; }
        #endregion
        #endregion

        #region Consturctor
        public CameraConfig()
        {

            this.Resolution = new Size(0, 0);
            this.AutoSleepEnable = false;

            this.AutoSleepLimitMin = TimeSpanInfo.FromMinutes(5);

            this.DelayAfterGrab = 0;
            this.DelayBeforeGrab = 0;

            this.GrabRetryCount = 1;
            this.SignalWatingTime = 300;

            this.PixelResolution = new SizeD();

            this.ImageFlipX = Camera.ImageFlip.Off;
            this.ImageFlipY = Camera.ImageFlip.Off;

            this.ImageRotate = Camera.ImageRotateInfo.None;
        }
        #endregion
    }
}

