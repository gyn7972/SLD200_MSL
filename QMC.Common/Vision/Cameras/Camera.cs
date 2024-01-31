using QMC.Common.Hmi;
using QMC.Common.Modules;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace QMC.Common.Vision.Cameras
{
    [Serializable]
    public abstract class Camera : Part
    {
        [Serializable]
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

        public enum ImageRotateInfo
        {
            None,
            RotateCW,
            RotateCCW,
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
        [NonSerialized]
        private System.Timers.Timer Timer;
        [NonSerialized]
        private CycleTimer m_CycleTimer;
        [NonSerialized]
        protected SemaphoreSlim m_GrabSemaphoreSlim;
        protected VisionImage m_latestImage;
        protected object m_ImageLock = new object();
        public bool Opened { get; protected set; }
        public bool IsLiveOn { set; get; }
        public Size Resolution
        {
            set
            {
                m_resolution = value;
            }
            get
            {
                if(this.ImageRotate == ImageRotateInfo.None)
                {
                    return m_resolution;
                }
                else
                {
                    return new Size(m_resolution.Height, m_resolution.Width);
                }
            }
        }
        Size m_resolution;
        public VisionImage LatestImage 
        { 
            set
            {
                lock(m_ImageLock)
                {
                    m_latestImage = value;
                }                    
            }
            get
            {
                lock(m_ImageLock)
                {
                    return m_latestImage;
                }
                
            }
        }
        
        #region Sleep
        /// <summary>
        /// Camera Live 시작된 시간을 가져온다.
        /// </summary>
        public DateTime LiveStartTime
        {
            get;
            private set;
        }
        

        

        public bool AutoSleepEnable
        {
            get { return Config.AutoSleepEnable; }
            set { Config.AutoSleepEnable = value; }
        }

        

        /// <summary>
        /// Camera의 Sleep 여부를 가져온다.
        /// </summary>
        public bool Sleep
        {
            get;
            private set;
        }

        public bool EnableExposure
        { 
            set { Config.EnableExposure = value; }
            get { return Config.EnableExposure; }
        }

        

        public bool SuspendedImageDisplay
        {
            get { return Config.SuspendedImageDisplay; }
            set { Config.SuspendedImageDisplay = value; }
        }

        #region ConstructConfiguration
        [Category("Camera")]
        public TimeSpanInfo AutoSleepLimitMin
        {
            get { return Config.AutoSleepLimitMin; }
            set { Config.AutoSleepLimitMin = value; }
        }
        [Category("Delay")]
        public int DelayBeforeGrab 
        {
            set { Config.DelayBeforeGrab = value;}
            get { return Config.DelayBeforeGrab; } 
        }
        [Category("Delay")]
        public int DelayAfterGrab 
        {
            set { Config.DelayAfterGrab = value; } 
            get { return Config.DelayAfterGrab;} 
        }
        [Category("Camera")]
        public int GrabRetryCount
        {
            set { Config.GrabRetryCount = value;}
            get { return Config.GrabRetryCount; } 
        }
        [Category("Camera")]
        public Size CameraResolution
        {
            get { return Config.CameraResolution;}
            set { Config.CameraResolution = value; }
        }
        [Category("Camera")]
        public int SignalWatingTime
        {
            get { return Config.SignalWatingTime;}
            set { Config.SignalWatingTime = value; }
        }
        [Category("Camera")]
        public SizeD PixelResolution 
        {
            set { Config.PixelResolution = value;}
            get { return Config.PixelResolution;}
        }
        [Category("Camera")]
        public ImageFlip ImageFlipX
        {
            get { return Config.ImageFlipX;}
            set { Config.ImageFlipX = value;}
        }
        [Category("Camera")]
        public ImageFlip ImageFlipY
        {
            get { return Config.ImageFlipY;}
            set { Config.ImageFlipY = value;}
        }
        public ImageRotateInfo ImageRotate
        {
            get { return Config.ImageRotate; }
            set { Config.ImageRotate = value; }

        }
        //[Category("Camera")]
        //public double MaxFrameRate
        //{
        //    get;
        //    set;
        //}
        [Category("Camera")]
        public TimeSpanInfo WaitToGrabTimeout 
        {
            set { Config.WaitToGrabTimeout = value;}
            get { return Config.WaitToGrabTimeout;} 
        }

        #endregion
        #endregion

        public CameraConfig Config { set; get; }
        #region Constructor
        public Camera() :this("Camera") { }
        public Camera(string strName) : base(strName)
        {
            Config = new CameraConfig();

            this.LatestImage = new VisionImage();
            this.LiveStartTime = new DateTime();
            this.AutoSleepEnable = false;
            this.Timer = new System.Timers.Timer(1000);
            this.Timer.Elapsed += Timer_Elapsed;
            this.Timer.Start();
            this.Sleep = false;
            //this.m_SyncRoot = new object();

            m_CycleTimer = new CycleTimer(this);
            m_GrabSemaphoreSlim = new SemaphoreSlim(1, 1);

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

        #region Method

        #region GrabQuickly()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="job">
        /// 조명(Illumination)을 제어하기 위한 VisionJob을 지정한다..
        /// 만약 null을 지정하면 조명을 제어하지 않는다.
        /// </param>
        /// <param name="camera">Grab을 수행하기 위한 카메라를 지정한다.</param>
        /// <param name="image"></param>
        /// <returns></returns>
        //public static int GrabQuickly(VisionJob job, Camera camera, out VisionImage image)
        //{
        //    int ret = 0;

        //    image = null;

        //    // set illumination
        //    if (job != null)
        //    {
        //        VisionPart visionPart = camera.Owner;
        //        if ((ret = visionPart.SetIluminationSync(job)) != 0) return ret;
        //    }

        //    // grab or expose
        //    if (camera.EnableExposure == true)
        //    {
        //        if ((ret = (camera.Expose()) != 0) return ret;
        //    }
        //    else
        //    {
        //        if ((ret = camera.GrabSync(out image)) != 0) return ret;
        //    }


        //    return ret;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static int GrabQuickly(Camera camera, out VisionImage image)
        {
            int ret = 0;

            image = null;

            //if ((ret = Camera.GrabQuickly(null, camera, out image)) != 0) return ret;
            if (camera.EnableExposure == true)
            {
                if ((ret = camera.Expose()) != 0) return ret;
            }
            else
            {
                if ((ret = camera.GrabSync(out image)) != 0) return ret;
            }

            return ret;
        }
        #endregion

        
        protected int CheckReturnCode(int code, Enum alarmKey)
        {
            int ret = 0;
            //Alarm alarm = null;
            //Error error = null;

            if (code == 0) return ret;

            //error = ErrorManager.GetByUid(ErrorManager.Register("", code));

            //alarm = this.Alarms[alarmKey];
            //alarm.Cause += string.Format("\n{0} [{1}]", error.Message, error.Code);
            //if ((ret = alarm.Post(this)) != 0) return ret;

            //this.WriteLog(LogLevel.Highest, "{0}", code);
            Console.WriteLine(string.Format("Error : {0}", alarmKey));

            return ret;
        }

        #region GetFrameRate
        public int GetFrameRate(ref double frameRate)
        {
            int ret = 0;

            if ((ret = this.OnGetFrameRate(ref frameRate)) == 0) return ret;
            return ret;
        }

        protected abstract int OnGetFrameRate(ref double frameRate);
        #endregion

        #region SetFrameRate
        public int SetFrameRate(double frameRate)
        {
            int ret = 0;
            if ((ret = this.OnSetFrameRate(frameRate)) == 0) return ret;
            return ret;
        }

        protected abstract int OnSetFrameRate(double frameRate);
        #endregion

        #region GetMaxFrameRate
        public int GetMaxFrameRate(ref RangeD frameRate)
        {
            int ret = 0;
            if ((ret = this.OnGetMaxFrameRate(ref frameRate)) == 0) return ret;
            return ret;
        }

        protected abstract int OnGetMaxFrameRate(ref RangeD frameRate);
        #endregion

        #region GetExposureTime
        public int GetExposureTime(ref double exposureTime)
        {
            int ret = 0;

            if ((ret = this.OnGetExposureTime(ref exposureTime)) != 0) return ret;

            return ret;
        }

        protected abstract int OnGetExposureTime(ref double exposureTime);
        #endregion

        #region SetExposureTime
        public int SetExposureTime(double exposureTime)
        {
            int ret = 0;

            if ((ret = this.OnSetExposureTime(exposureTime)) != 0) return ret;

            return ret;
        }

        protected abstract int OnSetExposureTime(double exposureTime);
        #endregion

        #region Reconnect
        public int Reconnect()
        {
            int ret = 0;
            if ((ret = OnReconnect()) != 0) return ret;
            return ret;
        }
        protected abstract int OnReconnect();
        #endregion

        #region AutoSleep
        public int WakeUp()
        {
            int ret = 0;
            this.LiveStartTime = DateTime.Now;
            this.Sleep = false;

            return ret;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.AutoSleepLimitMin.TotalSeconds <= 0) return;

            if (this.IsLiveOn == false) return;

            if (this.AutoSleepEnable == false) return;

            if (this.AutoSleepLimitMin < DateTime.Now - this.LiveStartTime)
            {
                this.StopLive();
                this.Sleep = true;
                Console.WriteLine("AutoSleep : Live Off");
            }
        }

        /// <summary>
        /// Camera의 Sleep 상태가 Change 되었을 경우 이벤트가 발생합니다.
        /// </summary>
        /// <param name="e"></param>
        private void OnAutoSleepStatusChange(CameraAutoSleepEventArgs e)
        {
            //if (this.AutoSleepStatusChange != null)
            //    this.AutoSleepStatusChange(this, e);
        }
        #endregion

        #region StartLive()

        public int StartLive()
        {
            return StartLiveProcedure();
        }

        private int StartLiveProcedure()
        {
            int ret = 0;

            if (this.Opened == false)
            {
                ret = -1;
                return ret;
            }

            if (this.IsLiveOn == true) return ret;

            Console.WriteLine("Start OnStartLive()");
            if ((ret = this.OnStartLive()) != 0) return ret;
            Console.WriteLine("End OnStartLive()");

            this.IsLiveOn = true;

            if (this.AutoSleepEnable == true)
            {
                this.LiveStartTime = DateTime.Now;

                this.Sleep = false;
            }

            return ret;
        }

        protected abstract int OnStartLive();
        #endregion

        #region StopLive
        public int StopLive()
        {
            return StopLiveProcedure();
        }

        private int StopLiveProcedure()
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();

            //if (this.Simulation.IsSimulatedWithoutResource() == true) return ret;

            if (this.IsLiveOn == false) return ret;

            try
            {
                if (this.Opened == false)
                {
                    //if ((ret = this.Alarms[AlarmKeys.NotOpened].Post(this)) != 0) return ret;
                    ret = -1;
                    return ret;
                }
                Console.WriteLine("Start OnStopLive()");
                stopWatch.Start();
                if ((ret = this.OnStopLive()) != 0) return ret;
                stopWatch.Stop();
                Console.WriteLine(string.Format("End OnStopLive() : {0}ms", stopWatch.Elapsed.Milliseconds));
            }
            finally
            {
                this.IsLiveOn = false;
            }

            return ret;
        }

        protected abstract int OnStopLive();
        #endregion

        #region CreateVisionImage


        private void RotateImage90(byte[] image, int nWidth, int nHeight, byte[] result, int nResultWidth, int nResultHeight)
        {
            // 1. 회전된 영상의 크기를 구한다.
            // 2. 회전된 영상의 데이터를 구한다.
            // 3. 회전된 영상의 크기를 저장한다.
            // 4. 회전된 영상의 데이터를 저장한다.
            int nResultSize = nResultWidth * nResultHeight;
            for (int i = 0; i < nResultSize; i++)
            {
                int nRow = i / nResultWidth;
                int nCol = i % nResultWidth;
                int nResultIndex = nRow + nCol * nResultHeight;
                int nIndex = nCol + nRow * nWidth;
                result[nResultIndex] = image[nIndex];
            }
        }

        private Task<int> ImageRotateCW(ImageRotateInfoClass imageRotateInfo)
        {
            Task<int> t = Task<int>.Factory.StartNew((obj) =>
            {
                ImageRotateInfoClass IRI = (ImageRotateInfoClass)obj;

                int nWidth = IRI.nWidth;
                int TargetIndex = IRI.nTargetIndex;

                for (int SourceIndex = IRI.nStartIndex; SourceIndex < IRI.nEndIndex; SourceIndex++)
                {
                    //if (TargetIndex >= IRI.nSize)
                    //{
                    //    TargetIndex -= IRI.nWidth;
                    //    TargetIndex--;
                    //}
                    //IRI.dst[TargetIndex] = (byte)(IRI.src[SourceIndex]);
                    //// bytes[SourceIndex] = 128;
                    //TargetIndex += nWidth;
                    IRI.dst[TargetIndex] = (byte)(IRI.src[SourceIndex]);
                    //IRI.dst[SourceIndex] = 128;

                    // bytes[SourceIndex] = 128;
                    TargetIndex += nWidth;
                    if (TargetIndex >= IRI.nSize)
                    {
                        TargetIndex -= IRI.nSize;
                        TargetIndex--;
                    }
                }

                return 0;
            }, imageRotateInfo);
            return t;
        }

        /// <summary>
        /// 취득된 카메라 이미지데이터를 VisionImage 타입으로 생성한다.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        protected int CreateVisionImage(IntPtr pointer, out VisionImage image)
        {
            int ret = 0;
            byte[] bytes = null;
            image = null;

            int TargetIndex = 0;
            int ss = 0;
            //Size sz = new Size(1536, 2048);
            //this.Resolution = sz;

            Size resolution = this.Resolution;


            int nSize = resolution.Width * resolution.Height;

            try
            {
                //bytes = new byte[this.Resolution.Width * this.Resolution.Height];
                //Marshal.Copy(pointer, bytes, 0, bytes.Length);

                //this.CreateVisionImage(bytes, out image);

                //image.Header.Pointer = pointer;

                bytes = new byte[resolution.Width * resolution.Height];

                if (this.ImageRotate == ImageRotateInfo.None)
                {
                    Marshal.Copy(pointer, bytes, 0, bytes.Length);

                }
                else
                {
                    byte[] bytesOrg = new byte[resolution.Width * resolution.Height];
                    Marshal.Copy(pointer, bytesOrg, 0, bytes.Length);


                    if (this.ImageRotate == ImageRotateInfo.RotateCW)
                    {
                        TargetIndex = resolution.Width - 1;
                        int nWidth = resolution.Width;
                        for (int SourceIndex = 0; SourceIndex < nSize; SourceIndex++)
                        {
                            bytes[TargetIndex] = (byte)(bytesOrg[SourceIndex]);
                            // bytes[SourceIndex] = 128;
                            TargetIndex += nWidth;
                            if (TargetIndex >= nSize)
                            {
                                TargetIndex -= nSize;
                                TargetIndex--;
                            }
                            ss = SourceIndex;
                        }
                    }
                    else
                    {
                        TargetIndex = nSize - resolution.Width;
                        int nWidth = resolution.Width;
                        for (int SourceIndex = 0; SourceIndex < nSize; SourceIndex++)
                        {
                            bytes[TargetIndex] = (byte)(bytesOrg[SourceIndex]);

                            TargetIndex -= nWidth;
                            if (TargetIndex < 0)
                            {
                                TargetIndex += nSize;
                                TargetIndex++;

                            }
                        }
                    }

                }

                this.CreateVisionImage(bytes, out image);

                image.Header.Pointer = pointer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// 취득된 카메라 이미지데이터를 VisionImage 타입으로 생성한다.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        protected int CreateVisionImage(byte[] buffer, out VisionImage image)
        {
            int ret = 0;

            image = new VisionImage();
            image.Header = new VisionImageHeader();

            try
            {
                // create vision image
                image.RawData = buffer;
                image.Header.Width = this.Resolution.Width;
                image.Header.Height = this.Resolution.Height;
                image.Header.BufferSize = this.Resolution.Width * this.Resolution.Height;
                image.Header.BitsPerPixel = (int)Camera.BitPerPixelInfo.Gray8bpp;
                image.Header.PixelFormat = PixelFormat.Format8bppIndexed;
                image.Header.Stride = (int)((image.Header.Width * image.Header.BitsPerPixel + 7) / 8);
                image.Header.OwnerName = this.Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ret;
        }
        #endregion

        #region WaitToGrab
        /// <summary>
        /// Grab Procedure 수행하기 위해 lock이 해제될 때까지 기다린다. 
        /// </summary>
        protected int WaitToGrab()
        {
            int ret = 0;
            int retryCount = 0;

            Console.WriteLine("WaitToGrab()");

        Retry:

            if (m_GrabSemaphoreSlim.Wait(this.WaitToGrabTimeout) == true)
            {
                Console.WriteLine("GrabSemaphore Acquire");
            }
            else
            {
                Console.WriteLine(string.Format("GrabSemaphore Denied {0}", retryCount));
                
                if (retryCount < this.GrabRetryCount)
                {
                    retryCount++;
                    goto Retry;
                }

                if ((ret = this.ReleaseToGrab()) != 0) return ret;
            }

            return ret;
        }
        #endregion

        #region ReleaseToGrab
        /// <summary>
        /// Grab Procedue 수행을 완료하여 lock을 해제한다.
        /// </summary>
        protected int ReleaseToGrab()
        {
            int ret = 0;

            if (m_GrabSemaphoreSlim.CurrentCount == 0)
            {
                try
                {
                    m_GrabSemaphoreSlim.Release();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("GrabSemaphore Release");
            }

            return ret;
        }
        #endregion

        #region Grab
        public int GrabSync(Purpose purpose, out VisionImage image)
        {
            return GrabProcedure(purpose, out image);
        }

        public int GrabSync(out VisionImage image)
        {
            return GrabProcedure(Purpose.Processing, out image);
        }

        public int GrabSync(Purpose purpose)
        {
            VisionImage image = null;
            return GrabProcedure(purpose, out image);
        }

        public int GrabSync()
        {
            VisionImage image = null;
            return GrabProcedure(Purpose.Processing, out image);
        }

        private int GrabProcedure(Purpose purpose, out VisionImage image)
        {
            int ret = 0;
            //int index = 0;
            image = null;

            //lock (this.m_SyncRoot)
            try
            {
                if (purpose != Purpose.Display)
                    m_CycleTimer.Start();

                {
                    if (purpose == Purpose.Display/* && this.IsControl() == false*/)
                    {
                        image = this.LatestImage;
                        return ret;
                    }

                    if (this.Opened == false)
                    {
                        //if ((ret = this.Alarms[AlarmKeys.NotOpened].Post(this)) != 0) return ret;
                        ret = -1;
                        return ret;
                    }

                    if (purpose != Purpose.Display)
                        Console.WriteLine("Start OnGrab()");

                    if ((ret = this.WaitToGrab()) != 0) return ret;

                    if (0 < this.DelayBeforeGrab)
                        Thread.Sleep(this.DelayBeforeGrab);

                    // Grab 실패시 재시도
                    for (int i = 0; i < this.GrabRetryCount + 1; i++)
                    {
                        if ((ret = this.OnGrab(out image)) != 0)
                        {
                            Console.WriteLine("Retry OnGrab()");
                            continue;
                        }
                        else break;
                    }

                    if (image == null)
                    {
                        ret = -1;
                        Console.WriteLine("Fail OnGrab()");
                        return ret;
                    }

                    if (0 < this.DelayAfterGrab)
                        Thread.Sleep(this.DelayAfterGrab);

                    if (purpose != Purpose.Display)
                        Console.WriteLine("End OnGrab()");
                }

                if (purpose != Purpose.Display)
                    m_CycleTimer.End();
            }
            finally
            {
                ret = this.ReleaseToGrab();

            }

            return ret;
        }

        protected abstract int OnGrab(out VisionImage image);
        #endregion

        #region Open()
        
        public int Open()
        {
            return OpenProcedure();
        }

        private int OpenProcedure()
        {
            int ret = 0;

            if (this.Opened == true) return ret;

            if ((ret = this.OnOpen()) != 0) return ret;

            this.Opened = true;

            return ret;
        }

        protected abstract int OnOpen();
        #endregion

        #region Close()

        public override void Close()
        {
            base.Close();
            CloseProcedure();
        }

        private int CloseProcedure()
        {
            int ret = 0;

            if (this.Opened == false) return ret;

            Console.WriteLine("Start OnClose()");

            if (this.IsLiveOn == true)
                this.StopLive();

            if ((ret = this.OnClose()) != 0) return ret;
            Console.WriteLine("End OnClose()");
            this.Opened = false;

            return ret;
        }

        protected abstract int OnClose();
        #endregion

        #region Expose()
        public int Expose()
        {
            return this.ExposeProcedure();
        }

        private int ExposeProcedure()
        {
            int ret = 0;

            try
            {
                if ((ret = WaitToGrab()) != 0) return ret;

                Console.WriteLine("Expose Start");

                if ((ret = this.OnExpose()) != 0) return ret;

                Console.WriteLine("Expose End");
            }
            finally
            {
                if (ret != 0)
                    ret = ReleaseToGrab();
            }

            return ret;
        }

        protected abstract int OnExpose();
        #endregion

        #region Readout()
        public int Readout(out VisionImage image)
        {
            return this.ReadoutProcedure(out image);
        }

        private int ReadoutProcedure(out VisionImage image)
        {
            int ret = 0;

            try
            {
                Console.WriteLine("Readout Start");

                if ((ret = this.OnReadout(out image)) != 0) return ret;

                Console.WriteLine("Readout End");
            }
            finally
            {
                ret = ReleaseToGrab();
            }
            return ret;
        }

        protected abstract int OnReadout(out VisionImage image);
        #endregion

        public override int Create()
        {
            if (m_CycleTimer == null)
                m_CycleTimer = new CycleTimer(this);
            if (m_GrabSemaphoreSlim == null)
                m_GrabSemaphoreSlim = new SemaphoreSlim(1, 1);

            return 0;
        }
        //public override int Initialize()
        //{
        //    int ret = 0;

        //    if ((ret = base.Initialize()) != 0) return ret;
        //    if (this.Opened == false)
        //    {
        //        if ((ret = this.Open()) != 0) return ret;
        //    }

        //    return ret;
        //}
        //public override int StopExecute()
        //{
        //    int ret = 0;
        //    if ((ret = base.StopExecute()) != 0) return ret;

        //    return ret;
        //}


        #endregion
    }

    #region CameraAutoSleepEventArgs
    [Serializable]
    public class CameraAutoSleepEventArgs : EquipmentEventArgs
    {
        #region Field

        private bool m_SleepStatus;

        #endregion

        #region Constructor
        public CameraAutoSleepEventArgs(bool sleepStatus)
        {
            this.SleepStatus = sleepStatus;
        }
        public CameraAutoSleepEventArgs() : this(false) { }
        #endregion

        #region Property
        public bool SleepStatus
        {
            get { return this.m_SleepStatus; }
            set { this.m_SleepStatus = value; }
        }
        #endregion
    }

    public class ImageRotateInfoClass
    {
        public int nWidth;
        public int nHeight;
        public byte[] src;
        public byte[] dst;
        public int nStartIndex;
        public int nEndIndex;
        public int nTargetIndex;
        public int nSize;
        public ImageRotateInfoClass()
        {

        }
    }

    #endregion
}
