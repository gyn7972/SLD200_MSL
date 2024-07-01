using MvCamCtrl.NET;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.HIKVISION;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using System.Drawing.Imaging;
using System.Diagnostics;
using QMC.Common.Modules;
using SpiralLab.Sirius;

namespace QMC.Common.Vision.HIKVISION
{
    [Serializable]
    public enum GrabMode
    {
        None,
        Live,
        Grab,
        Exporse
    }
    #region HIKGigECamera
    [Serializable]
    public class HIKGigECamera : Camera
    {
        #region Define

        #endregion

        #region Field
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        public static MyCamera.cbEventdelegateEx EventCallback;
        private int m_nRet;
        private string[] m_deviceList;
        private int m_deviceListIndex;
        MyCamera.MV_CC_DEVICE_INFO_LIST stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
        private MyCamera m_MyCamera = new MyCamera();
        private string m_CamLog;
        bool m_bGrabbing = false;
        Thread m_hReceiveThread = null;
        MyCamera.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();
        private GrabMode m_CurrentMode;

        UInt32 m_nBufSizeForDriver = 0;
        IntPtr m_BufforDriver = IntPtr.Zero;
        private static Object BufforDriverLock = new Object();
        private IntPtr m_ViewerHandler;

        static DateTime StartTime { get; set; }
        #endregion

        #region Event

        #endregion

        #region Constructor
        public HIKGigECamera(string strName)
             : base(strName)
        {
            Config = new HIKGigECameraConfig();
            this.ViewerHandler = IntPtr.Zero;
            nRet = new int();
            SerialNumber = "";
            m_CurrentMode = GrabMode.None;
        }
        public HIKGigECamera() : this("Camera") { }//?
        #endregion

        #region Property
        public new HIKGigECameraConfig Config { get; set; }

        public string CamLog
        {
            get { return this.m_CamLog; }
            set { this.m_CamLog = value; }
        }

        public int nRet { get; set; }
        public IntPtr ViewerHandler
        {
            get { return m_ViewerHandler; }
            set { m_ViewerHandler = value; }
        }
        public string SerialNumber { get; set; }

        #endregion

        #region Method
        public override int Create()
        {
            return base.Create();

        }
        public override int Initialize()
        {
            int ret = base.Initialize();
            if (m_Status == RunStatus.Stop) return 1;
            if (m_MyCamera.MV_CC_IsDeviceConnected_NET() == true)
            {
                this.Close();
            }
            else
            {
                if(this.Opened == true)
                {
                    this.Opened = false;
                }
            }

            if (this.Open() == 0)
            {
                this.Live();
            }
            else
            {
                MessageBox.Show("카메라 연결 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = -1;
            }

            return ret;
        }

        public Task<int> BeginInitialize()
        {

            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = Initialize();
                return 0;
            });

            return task;
        }

        public void EventCallbackFunc(ref MyCamera.MV_EVENT_OUT_INFO pEventInfo, IntPtr pUser)
        {
            if (pEventInfo.EventName == "ExposureEnd")
            {
                //카메라 찍는게 끝나면

                //DateTime EndTime = DateTime.Now;
                //TimeSpan timeSpan = EndTime - StartTime;
                //MessageBox.Show(String.Format("Expose Time = {0}", timeSpan.TotalMilliseconds));
            }
        }
        public int DeviceSearch(string strSerialNumber, ref DeviceCollection devices)
        {
            stDeviceList.nDeviceNum = 0;

            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref stDeviceList);

            DeviceInformation deviceInformation = new DeviceInformation();

            for (int i = 0; i < stDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    this.SerialNumber = gigeInfo.chSerialNumber;
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    this.SerialNumber = usbInfo.chSerialNumber;
                }

                if (this.SerialNumber == strSerialNumber)
                {
                    return i;
                }
                else if (strSerialNumber == "")
                {
                    MessageBox.Show(String.Format("Check SerialNumber, Current = {0}", this.SerialNumber), "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                devices.Add(deviceInformation);
            }
            if (stDeviceList.nDeviceNum != 0)
            {
                deviceInformation.Number = 0;
            }
            return -1;
        }

        public void ModeChange(GrabMode grabMode)
        {
            if (grabMode == m_CurrentMode)
            {
                return;
            }
            else
            {
                if (m_CurrentMode == GrabMode.Live)
                {
                    StopLive();
                }
                else if (m_CurrentMode == GrabMode.None)
                {
                }
                else
                {
                    int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();
                    if (MyCamera.MV_OK != nRet)
                    {
                        CamLog += string.Format("MV_CC_StopGrabbing_NET Fail!", nRet);
                    }
                    nRet = m_MyCamera.MV_CC_SetEnumValueByString_NET("EventNotification", "Off");
                    if (MyCamera.MV_OK != nRet)
                    {
                        Console.WriteLine("Set EventNotification failed!");
                        return;
                    }
                }
            }
            if (grabMode == GrabMode.Grab)
            {

                if (m_bGrabbing)
                {
                    StopLive();
                    m_bGrabbing = false;
                }
                nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Grab TriggerMode Fail", nRet);
                }
                LatestImage = null;
                nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Grab TriggerSource Fail", nRet);
                }
                nRet = m_MyCamera.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Grab Fail!", nRet);
                }
            }
            else if (grabMode == GrabMode.Live)
            {
                int nRet = m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                if (MyCamera.MV_OK != nRet)
                {
                    m_bGrabbing = false;
                    CamLog += string.Format(" Live AcquisitionMode Fail!", nRet);
                    return;
                }
                nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                if (MyCamera.MV_OK != nRet)
                {
                    m_bGrabbing = false;
                    CamLog += string.Format(" Live TriggerMode Fail!", nRet);
                    return;
                }
            }
            else
            {
                //Exporce
                nRet = m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Exporce AcquisitionMode Fail!", nRet);
                }
                //nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                //if (MyCamera.MV_OK != nRet)
                //{
                //    CamLog += string.Format("Exporce TriggerMode Fail!", nRet);
                //}


                nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Grab TriggerMode Fail", nRet);
                }
                LatestImage = null;
                nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Grab TriggerSource Fail", nRet);
                }

                nRet = m_MyCamera.MV_CC_SetEnumValueByString_NET("EventSelector", "ExposureEnd");
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Set EventSelector failed!");
                    return;
                }

                nRet = m_MyCamera.MV_CC_SetEnumValueByString_NET("EventNotification", "On");
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Set EventNotification failed!");
                    return;
                }

                EventCallback = new MyCamera.cbEventdelegateEx(EventCallbackFunc);
                nRet = m_MyCamera.MV_CC_RegisterEventCallBackEx_NET("ExposureEnd", EventCallback, IntPtr.Zero);
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Register event callback failed!");
                    return;
                }

                nRet = m_MyCamera.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", nRet);
                    return;
                }
            }
            m_CurrentMode = grabMode;
        }
        public void ReadOut()
        {
            MyCamera.MV_FRAME_OUT stFrameInfo = new MyCamera.MV_FRAME_OUT();

            m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
            if (nRet == MyCamera.MV_OK)
            {
                VisionImage image;
                if (this.Resolution.Width != stFrameInfo.stFrameInfo.nWidth || this.Resolution.Height != stFrameInfo.stFrameInfo.nHeight)
                    this.Resolution = new Size(stFrameInfo.stFrameInfo.nWidth, stFrameInfo.stFrameInfo.nHeight);

                CreateVisionImage(stFrameInfo.pBufAddr, out image);
                LatestImage = image;
                m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
            }
        }
        public int Open(int channel)
        {
            if (channel < 0)
            {
                return -1;
            }
            MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDeviceList.pDeviceInfo[channel], typeof(MyCamera.MV_CC_DEVICE_INFO));

            if (null == m_MyCamera)
            {
                m_MyCamera = new MyCamera();
                if (null == m_MyCamera)
                {
                    return -1;
                }
            }

            int nRet = m_MyCamera.MV_CC_CreateDevice_NET(ref device);
            if (MyCamera.MV_OK != nRet)
            {
                return nRet;
            }

            bool bRet = m_MyCamera.MV_CC_IsDeviceConnected_NET();

            for (int i = 0; i < Config.RetryCount; i++)
            {
                nRet = m_MyCamera.MV_CC_OpenDevice_NET();

                if (MyCamera.MV_OK == nRet)
                {
                    break;
                }
                Thread.Sleep(Config.OpenDelayTime);
            }
            if (MyCamera.MV_OK != nRet)
            {

                m_MyCamera.MV_CC_DestroyDevice_NET();
                CamLog = string.Format("Device Open Fail! : {0}", nRet);
                return nRet;
            }
            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                int nPacketSize = m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = m_MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != MyCamera.MV_OK)
                    {
                        CamLog = string.Format("Set Packet Size failed!", nRet);
                    }
                }
                else
                {
                    CamLog = string.Format("Get Packet Size failed!", nPacketSize);
                }
            }

            nRet = m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);



            MyCamera.MVCC_FLOATVALUE floatValue = new MyCamera.MVCC_FLOATVALUE();
            MyCamera.MVCC_INTVALUE widthValue = new MyCamera.MVCC_INTVALUE();
            MyCamera.MVCC_INTVALUE heightValue = new MyCamera.MVCC_INTVALUE();

            nRet = m_MyCamera.MV_CC_SetFloatValue_NET("ExposureTime", this.Config.ExposureTime);
            if (MyCamera.MV_OK != nRet)
            {
                CamLog = string.Format("Set ExposureTime Failed! : {0}", nRet);
                return nRet;
            }

            nRet = m_MyCamera.MV_CC_SetFloatValue_NET("Gain", this.Config.Gain);
            if (MyCamera.MV_OK != nRet)
            {
                CamLog = string.Format("Set Gain Failed! : {0}", nRet);
                return nRet;
            }

            nRet = m_MyCamera.MV_CC_GetWidth_NET(ref widthValue);
            if (MyCamera.MV_OK != nRet)
            {
                CamLog = string.Format("Get Resolution Width Failed! : {0}", nRet);
                return nRet;
            }

            nRet = m_MyCamera.MV_CC_GetHeight_NET(ref heightValue);
            if (MyCamera.MV_OK != nRet)
            {
                CamLog = string.Format("Get Resolution Height Failed! : {0}", nRet);
                return nRet;
            }

            this.Resolution = this.Config.Resolution = new Size((int)widthValue.nCurValue, (int)heightValue.nCurValue);
            m_CurrentMode = GrabMode.None;

            return nRet;
        }

        public int Grab()
        {
            ModeChange(GrabMode.Grab);
            // DateTime StartTime = DateTime.Now;
            nRet = m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (MyCamera.MV_OK != nRet)
            {
                CamLog += string.Format("Grab Fail!", nRet);
            }
            MyCamera.MV_FRAME_OUT stFrameInfo = new MyCamera.MV_FRAME_OUT();

            nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
            if (nRet == MyCamera.MV_OK)
            {
                VisionImage image;
                if (this.Resolution.Width != stFrameInfo.stFrameInfo.nWidth || this.Resolution.Height != stFrameInfo.stFrameInfo.nHeight)
                    this.Resolution = new Size(stFrameInfo.stFrameInfo.nWidth, stFrameInfo.stFrameInfo.nHeight);
                CreateVisionImage(stFrameInfo.pBufAddr, out image);
                LatestImage = image;
                m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
            }
            return nRet;
        }

        public void CloseCamera()
        {
            m_bGrabbing = false;
            if (m_hReceiveThread != null)
            {
                m_hReceiveThread.Join();
            }

            if (m_MyCamera.GetCameraHandle().ToInt64() != 0x0000000000000000)           //  Camera 가 연결되어 있을 때(Handle 값이 있을 때)만 Close 하자.
            {
                int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();
                m_MyCamera.MV_CC_CloseDevice_NET();
                m_MyCamera.MV_CC_DestroyDevice_NET();
                if (nRet != MyCamera.MV_OK)
                {
                    CamLog += string.Format("Stop Grabbing Fail", nRet);
                }
            }

            this.Opened = false;
        }
        public int Expose()
        {
            int ret = 0;
            try
            {
                ModeChange(GrabMode.Exporse);
                StartTime = DateTime.Now;
                nRet = m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                {
                    CamLog += string.Format("Expose Fail!", nRet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ret = -1;
            }

            return ret;
        }
        public void Live()
        {
            if (m_CurrentMode == GrabMode.Live)
            {
                return;
            }
            m_bGrabbing = true;
            ModeChange(GrabMode.Live);

            if (m_hReceiveThread != null && m_hReceiveThread.IsAlive)
            {
                m_hReceiveThread.Abort();
                m_hReceiveThread.Join();
            }
            m_hReceiveThread = new Thread(ReceiveThreadProcess);
            m_hReceiveThread.Start();
            m_stFrameInfo.nFrameLen = 0;
            m_stFrameInfo.enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;
            nRet = m_MyCamera.MV_CC_StartGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join();
                CamLog = string.Format("Start Live Fail!", nRet);
                Opened = false;
                return;
            }
        }
        public void StopLive()
        {
            m_bGrabbing = false;
            m_hReceiveThread.Join();
            int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();
            if (nRet != MyCamera.MV_OK)
            {
                CamLog = string.Format("Stop Grabbing Fail", nRet);
            }
            IsLiveOn = false;
        }
        public void ReceiveThreadProcess()
        {
            MyCamera.MV_FRAME_OUT stFrameInfo = new MyCamera.MV_FRAME_OUT();
            int nRet = MyCamera.MV_OK;

            while (m_bGrabbing)
            {
                nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
                if (nRet == MyCamera.MV_OK)
                {
                    VisionImage image;
                    if (this.Resolution.Width != stFrameInfo.stFrameInfo.nWidth || this.Resolution.Height != stFrameInfo.stFrameInfo.nHeight)
                        this.Resolution = new Size(stFrameInfo.stFrameInfo.nWidth, stFrameInfo.stFrameInfo.nHeight);

                    CreateVisionImage(stFrameInfo.pBufAddr, out image);
                    LatestImage = image;

                    if (RemoveCustomPixelFormats(stFrameInfo.stFrameInfo.enPixelType))
                    {
                        m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
                        continue;
                    }
                    m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
                }
            }
        }

        private bool RemoveCustomPixelFormats(MyCamera.MvGvspPixelType enPixelFormat)
        {
            Int32 nResult = ((int)enPixelFormat) & (unchecked((Int32)0x80000000));
            if (0x80000000 == nResult)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateConfigData() //참고 : Override
        {
            /*
            if (Owner is DieLoader) // 참고 : Loader인지 Unloader인지 parsing
            {
                DieLoader dieLoader = Owner as DieLoader;
                Config = dieLoader.Config.HIKGigECameraConfig;//?

            }
            else if (Owner is DieUnloader)
            {
                DieUnloader dieUnloader = Owner as DieUnloader;
                Config = dieUnloader.Config.HIKGigECameraConfig;
            }
            else if (Owner is DieTransfer)
            {
                DieTransfer dieTransfer = Owner as DieTransfer;
                Config = dieTransfer.Config.HIKGigECameraConfig;
            }
            else { }

            */
            this.Resolution = Config.Resolution;
        }
        protected override int OnGetFrameRate(ref double frameRate)
        {
            throw new NotImplementedException();
        }

        protected override int OnSetFrameRate(double frameRate)
        {
            throw new NotImplementedException();
        }

        protected override int OnGetMaxFrameRate(ref RangeD frameRate)
        {
            throw new NotImplementedException();
        }

        protected override int OnGetExposureTime(ref double exposureTime)
        {
            throw new NotImplementedException();
        }

        protected override int OnSetExposureTime(double exposureTime)
        {
            throw new NotImplementedException();
        }

        protected override int OnReconnect()
        {
            throw new NotImplementedException();
        }

        protected override int OnStartLive()
        {
            int ret = 0;
            if (m_bGrabbing)
            {
                //라이브중
            }
            else
            {
                this.Live();
            }
            return ret;
        }

        protected override int OnStopLive()
        {
            int ret = 0;
            this.StopLive();
            return ret;
        }

        protected override int OnGrab(out VisionImage image)
        {
            image = null;
            int ret = this.Grab();

            if (LatestImage != null)
            {
                image = LatestImage;
            }
            //image = new VisionImage();
            //? 이미지 어떻게 가져오는지
            return ret;
        }

        protected override int OnOpen()
        {
            int ret = 0;


            //Config에 설정된 시리얼 넘버에 해당하는 Channel 값을 가져온다.

            //Open(Channel) 함수를 호출한다.

            int channel = 0;
            DeviceCollection deviceInformation = new DeviceCollection();
            channel = this.DeviceSearch(Config.SerialNumber, ref deviceInformation);

            ret = this.Open(channel);

            return ret;
            //Channel 값 확인

        }

        protected override int OnClose()
        {
            int ret = 0;
            this.CloseCamera();
            return ret;
        }

        protected override int OnExpose()
        {
            int ret = 0;
            this.Expose();
            return ret;
        }

        protected override int OnReadout(out VisionImage image)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
    [Serializable]
    #endregion
    public class DeviceCollection : Collection<DeviceInformation>
    {
        public DeviceCollection()
        {

        }
    }
    [Serializable]
    public class DeviceInformation
    {
        private string m_Name;
        private int m_Number;

        public DeviceInformation()
        {
            this.Name = "";
            this.Number = 0;
        }

        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        public int Number
        {
            get { return this.m_Number; }
            set { this.m_Number = value; }
        }
    }

}
