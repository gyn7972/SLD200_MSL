/*
 * Purpose
 *      Euresys사의 Frame Grabber중 GrabLink Series에 대해서 정의한다.
 *      
 * Reference
 *      
 * Revision
 *      1. Created: 2018.11.07 JUNG.CY
 * 
 */

using QMC.Common.Modules;
using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Vision.EureSys
{
    #region GrabLinkMultiCamCamera
    [Serializable]
    public class GrabLinkMultiCamCamera : MultiCamCamera
    {
        #region Define
        [Serializable]
        // Values applicable to Grablink series
        public enum eBoardTypes
        {
            VALUE,
            VALUE_CPCI,
            GRABLINK_AVENUE,
            GRABLINK_EXPRESS_PCIe,
            EXPERT_2,
            EXPERT_2_CPCI,
            COLORSCAN,
            QUICKPACK_CFA,
            Grablink_Quickpack_CFA_PCIe,
            GRABLINK_FULL_XR,
            GRABLINK_FULL,
            GRABLINK_DUALBASE,
            GRABLINK_BASE
        }

        [Serializable]
        public enum eConnector
        {
            M,
            A,
            B,
        }

        [Serializable]
        public enum eBoardTopology
        {
            MONO,
            MONO_DECA,
            MONO_SLOW,
            DUO,
        }

        [Serializable]
        public enum eTapConfiguration
        {
            /// <summary>
            /// The camera requires the Base-1T8 tap configuration.
            /// </summary>
            BASE_1T8,

            /// <summary>
            /// The camera requires the Base-1T10 tap configuration.
            /// </summary>
            BASE_1T10,

            /// <summary>
            /// The camera requires the Base-1T12 tap configuration.
            /// </summary>
            BASE_1T12,

            /// <summary>
            /// The camera requires the Base-1T14 tap configuration.
            /// </summary>
            BASE_1T14,

            /// <summary>
            /// The camera requires the Base-1T16 tap configuration.
            /// </summary>
            BASE_1T16,

            /// <summary>
            /// The camera requires the Base-1T24 tap configuration.
            /// </summary>
            BASE_1T24,

            /// <summary>
            /// The camera requires the Base-1T30B2 tap configuration.
            /// </summary>
            BASE_1T30B2,

            /// <summary>
            /// The camera requires the Base-1T30B3 tap configuration.
            /// </summary>
            BASE_1T30B3,

            /// <summary>
            /// The camera requires the Base-1T36B2 tap configuration.
            /// </summary>
            BASE_1T36B2,

            /// <summary>
            /// The camera requires the Base-1T36B3 tap configuration.
            /// </summary>
            BASE_1T36B3,

            /// <summary>
            /// The camera requires the Base-1T42B2 tap configuration.
            /// </summary>
            BASE_1T42B2,

            /// <summary>
            /// The camera requires the Base-1T42B3 tap configuration.
            /// </summary>
            BASE_1T42B3,

            /// <summary>
            /// The camera requires the Base-1T48B2 tap configuration.
            /// </summary>
            BASE_1T48B2,

            /// <summary>
            /// The camera requires the Base-1T48B3 tap configuration.
            /// </summary>
            BASE_1T48B3,

            /// <summary>
            /// The camera requires the Base-2T8 tap configuration.
            /// </summary>
            BASE_2T8,

            /// <summary>
            /// The camera requires the Base-2T10 tap configuration.
            /// </summary>
            BASE_2T10,

            /// <summary>
            /// The camera requires the Base-2T12 tap configuration.
            /// </summary>
            BASE_2T12,

            /// <summary>
            /// The camera requires the Base-2T14B2 tap configuration.
            /// </summary>
            BASE_2T14B2,

            /// <summary>
            /// The camera requires the Base-2T16B2 tap configuration.
            /// </summary>
            BASE_2T16B2,

            /// <summary>
            /// The camera requires the Base-3T8 tap configuration.
            /// </summary>
            BASE_3T8,

            /// <summary>
            /// The camera requires the Medium-1T30 tap configuration.
            /// </summary>
            MEDIUM_1T30,

            /// <summary>
            /// The camera requires the Medium-1T36 tap configuration.
            /// </summary>
            MEDIUM_1T36,

            /// <summary>
            /// The camera requires the Medium-2T24 tap configuration.
            /// </summary>
            MEDIUM_2T24,

            /// <summary>
            /// The camera requires the Medium-3T10 tap configuration.
            /// </summary>
            MEDIUM_3T10,

            /// <summary>
            /// The camera requires the Medium-3T12 tap configuration.
            /// </summary>
            MEDIUM_3T12,

            /// <summary>
            /// The camera requires the Medium-4T8 tap configuration.
            /// </summary>
            MEDIUM_4T8,

            /// <summary>
            /// The camera requires the Medium-4T10 tap configuration.
            /// </summary>
            MEDIUM_4T10,

            /// <summary>
            /// The camera requires the Medium-4T12 tap configuration.
            /// </summary>
            MEDIUM_4T12,

            /// <summary>
            /// The camera requires the Full Camera Link configuration and delivers pixels of 8-bit on 8 taps.
            /// </summary>
            FULL_8T8,

            /// <summary>
            /// The camera requires the 10-tap configuration and delivers pixels of 8-bit on 10 taps.
            /// </summary>
            DECA_10T8,

            /// <summary>
            /// 
            /// </summary>
            LITE_1T8,

            /// <summary>
            /// 
            /// </summary>
            LITE_1T10,

            /// <summary>
            /// 
            /// </summary>
            LITE_2T8,

            /// <summary>
            /// 
            /// </summary>
            LITE_2T10,

        }

        [Serializable]
        public enum eTapGeometry
        {
            /// <summary>
            /// The area-scan camera requires the 1X-1Y tap geometry.
            /// </summary>
            A1X_1Y,

            /// <summary>
            /// The area-scan camera requires the 1X2-1Y tap geometry.
            /// </summary>
            A1X2_1Y,

            /// <summary>
            /// The area-scan camera requires the 1X4-1Y tap geometry.
            /// </summary>
            A1X4_1Y,

            /// <summary>
            /// The area-scan camera requires the 1X8-1Y tap geometry.
            /// </summary>
            A1X8_1Y,

            /// <summary>
            /// The area-scan camera requires the 1X10-1Y tap geometry.
            /// </summary>
            A1X10_1Y,

            /// <summary>
            /// The area-scan camera requires the 2X-1Y tap geometry.
            /// </summary>
            A2X_1Y,

            /// <summary>
            /// The area-scan camera requires the 2XE-1Y tap geometry.
            /// </summary>
            A2XE_1Y,

            /// <summary>
            /// The area-scan camera requires the 2XM-1Y tap geometry.
            /// </summary>
            A2XM_1Y,

            /// <summary>
            /// The area-scan camera requires the 2XR-1Y tap geometry.
            /// </summary>
            A2XR_1Y,

            /// <summary>
            /// The area-scan camera requires the 2X2-1Y tap geometry.
            /// </summary>
            A2X2_1Y,

            /// <summary>
            /// The area-scan camera requires the 2X2E-1Y tap geometry.
            /// </summary>
            A2X2E_1Y,

            /// <summary>
            /// The area-scan camera requires the 2X2M-1Y tap geometry.
            /// </summary>
            A2X2M_1Y,

            /// <summary>
            /// The area-scan camera requires the 2X4-1Y tap geometry.
            /// </summary>
            A2X4_1Y,

            /// <summary>
            /// The area-scan camera requires the 4X-1Y tap geometry.
            /// </summary>
            A4X_1Y,

            /// <summary>
            /// The area-scan camera requires the 4XE-1Y tap geometry.
            /// </summary>
            A4XE_1Y,

            /// <summary>
            /// The area-scan camera requires the 4XR-1Y tap geometry.
            /// </summary>
            A4XR_1Y,

            /// <summary>
            /// The area-scan camera requires the 4X2-1Y tap geometry.
            /// </summary>
            A4X2_1Y,

            /// <summary>
            /// The area-scan camera requires the 4X2E-1Y tap geometry.
            /// </summary>
            A4X2E_1Y,

            /// <summary>
            /// The area-scan camera requires the 8X-1Y tap geometry.
            /// </summary>
            A8X_1Y,

            /// <summary>
            /// The area-scan camera requires the 8XR-1Y tap geometry.
            /// </summary>
            A8XR_1Y,

            /// <summary>
            /// The area-scan camera requires the 1X-1Y2 tap geometry.
            /// </summary>
            A1X_1Y2,

            /// <summary>
            /// The area-scan camera requires the 1X2-1Y2 tap geometry.
            /// </summary>
            A1X2_1Y2,

            /// <summary>
            /// The area-scan camera requires the 1X4-1Y2 tap geometry.
            /// </summary>
            A1X4_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2X-1Y2 tap geometry.
            /// </summary>
            A2X_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2XE-1Y2 tap geometry.
            /// </summary>
            A2XE_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2XR-1Y2 tap geometry.
            /// </summary>
            A2XR_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2XM-1Y2 tap geometry.
            /// </summary>
            A2XM_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2X2-1Y2 tap geometry.
            /// </summary>
            A2X2_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2X2E-1Y2 tap geometry.
            /// </summary>
            A2X2E_1Y2,

            /// <summary>
            /// The area-scan camera requires the 2X2M-1Y2 tap geometry.
            /// </summary>
            A2X2M_1Y2,

            /// <summary>
            /// The area-scan camera requires the 4X-1Y2 tap geometry.
            /// </summary>
            A4X_1Y2,

            /// <summary>
            /// The area-scan camera requires the 4XE-1Y2 tap geometry.
            /// </summary>
            A4XE_1Y2,

            /// <summary>
            /// The area-scan camera requires the 4XR-1Y2 tap geometry.
            /// </summary>
            A4XR_1Y2,

            /// <summary>
            /// The area-scan camera requires the 1X-2YE tap geometry.
            /// </summary>
            A1X_2YE,

            /// <summary>
            /// The area-scan camera requires the 1X2-2YE tap geometry.
            /// </summary>
            A1X2_2YE,

            /// <summary>
            /// The area-scan camera requires the 1X4-2YE tap geometry.
            /// </summary>
            A1X4_2YE,

            /// <summary>
            /// The area-scan camera requires the 2X-2YE tap geometry.
            /// </summary>
            A2X_2YE,

            /// <summary>
            /// The area-scan camera requires the 2XE-2YE tap geometry.
            /// </summary>
            A2XE_2YE,

            /// <summary>
            /// The area-scan camera requires the 2XR-2YE tap geometry.
            /// </summary>
            A2XR_2YE,

            /// <summary>
            /// The area-scan camera requires the 2XM-2YE tap geometry.
            /// </summary>
            A2XM_2YE,

            /// <summary>
            /// The area-scan camera requires the 2X2-2YE tap geometry.
            /// </summary>
            A2X2_2YE,

            /// <summary>
            /// The area-scan camera requires the 2X2E-2YE tap geometry.
            /// </summary>
            A2X2E_2YE,

            /// <summary>
            /// The area-scan camera requires the 2X2M-2YE tap geometry.
            /// </summary>
            A2X2M_2YE,

            /// <summary>
            /// The area-scan camera requires the 4X-2YE tap geometry.
            /// </summary>
            A4X_2YE,

            /// <summary>
            /// The area-scan camera requires the 4XE-2YE tap geometry.
            /// </summary>
            A4XE_2YE,

            /// <summary>
            /// The area-scan camera requires the 4XR-2YE tap geometry.
            /// </summary>
            A4XR_2YE,

            A1X3_1Y,

            A3X_1Y,
        }
        #endregion

        #region Field
        #endregion

        #region Constructor

        public GrabLinkMultiCamCameraConfig GrabLinkMultiCamCameraConfig 
        { 
            set
            {
                Config = value;
            }
            get
            {
                return Config as GrabLinkMultiCamCameraConfig;
            }
        }
        public GrabLinkMultiCamCamera(string strName)
            : base(strName)
        {
            Config = new GrabLinkMultiCamCameraConfig();
            this.Connector = eConnector.M;
            this.BoardTopology = eBoardTopology.MONO;
            this.TapConfiguration = eTapConfiguration.BASE_1T10;
            this.TapGeometry = eTapGeometry.A1X_1Y;
        }
        public GrabLinkMultiCamCamera() : this("GrabLinkMultiCamCamera") { }
        #endregion

        #region Property
        //public eTapConfiguration TapConfiguration { set; get; }
        //public eTapGeometry TapGeometry { set; get; }

        #region ConstructConfiguration

        [Category("Channel")]
        public new eConnector Connector
        {
            get { return (eConnector)GrabLinkMultiCamCameraConfig.Connector; }
            set { GrabLinkMultiCamCameraConfig.Connector = value; }
        }

        [Category("Board")]
        public new eBoardTopology BoardTopology
        {
            get { return (eBoardTopology)GrabLinkMultiCamCameraConfig.BoardTopology; }
            set { GrabLinkMultiCamCameraConfig.BoardTopology = value; }
        }

        [Category("Board")]
        public eTapConfiguration TapConfiguration
        {
            get { return GrabLinkMultiCamCameraConfig.TapConfiguration; }
            set { GrabLinkMultiCamCameraConfig.TapConfiguration = value; }
        }

        [Category("Board")]
        public eTapGeometry TapGeometry
        {
            get { return GrabLinkMultiCamCameraConfig.TapGeometry;}
            set { GrabLinkMultiCamCameraConfig.TapGeometry = value; }
        }
        #endregion
        #endregion

        #region Method
        #endregion

        #region EureSysFrameGrabberCamera Members
        protected override int OnOpen()
        {
            int ret = 0;

            base.OnOpen();

            try
            {
                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.CameraLinkTapConfiguration, this.TapConfiguration.ToString().ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;

                if ((ret = this.CheckReturnCode(MultiCam.SetParam(this.Channel, (int)MultiCam.SetParameter.CameraLinkTapGeometry, this.TapGeometry.ToString().Split('A')[0].ToUpper()), Camera.AlarmKeys.OpenFailed)) != 0) return ret;
            }
            catch (MultiCamException ex)
            {
                //Log.Write("CameraException", $"[OnOpen][{this.Alias}] Camera Exception! Message = {ex.Message}");
                //ret = ErrorManager.Register("OnOpen Error Detected");
                Console.WriteLine(ex.Message);
            }

            return ret;
        }
        #endregion

        #region Part Members
        //protected new GrabLinkMultiCamCameraConstructConfiguration ConstructConfiguration
        //{
        //    get { return base.ConstructConfiguration as GrabLinkMultiCamCameraConstructConfiguration; }
        //}

        //protected override PartConstructConfiguration OnGetDefaultConstructConfiguration()
        //{
        //    return new GrabLinkMultiCamCameraConstructConfiguration();
        //}

        //protected override void OnSetConstructConfiguration(PartConstructConfiguration configuration)
        //{
        //    base.OnSetConstructConfiguration(configuration);

        //    if (this.ConstructConfiguration == null) return;
        //}

        #endregion
        public override void UpdateConfigData() //참고 : Override
        {

            //if (Owner is DieLoader) // 참고 : Loader인지 Unloader인지 parsing
            //{
            //    DieLoader dieLoader = Owner as DieLoader;
            //    Config = dieLoader.Config.GrabLinkMultiCamCameraConfig;//?
            //}
            //else if (Owner is DieUnloader)
            //{
            //    DieUnloader dieUnloader = Owner as DieUnloader;
            //    Config = dieUnloader.Config.GrabLinkMultiCamCameraConfig;
            //}
            //else { }
        }

        public override int Initialize()
        {
            int ret = base.Initialize();
            
            if(Opened)
            {
                Close();
            }

            Open();

            return ret;
        }
    }
    #endregion

    #region GrabLinkMultiCamCameraConstructConfiguration
    //[Serializable]
    //public class GrabLinkMultiCamCameraConstructConfiguration : MultiCamCameraConstructConfiguration
    //{
    //    #region Field
    //    private GrabLinkMultiCamCamera.TapConfiguration m_TapConfiguration;
    //    private GrabLinkMultiCamCamera.TapGeometry m_TapGeometry;
    //    #endregion

    //    #region Constructor
    //    public GrabLinkMultiCamCameraConstructConfiguration(PartConstructMethod constructMethod)
    //        : base(constructMethod)
    //    {
    //    }
    //    public GrabLinkMultiCamCameraConstructConfiguration() : this(PartConstructMethod.Static) { }
    //    #endregion

    //    #region Property
    //    //[Category("EureSysFrameGrabber")]
    //    //public new EureSysGrabLinkCamera.BoardTypes BoardType
    //    //{
    //    //    get { return (EureSysGrabLinkCamera.BoardTypes)base.BoardType; }
    //    //    set { base.BoardType = value; }
    //    //}

    //    [Category("Channel")]
    //    public new GrabLinkMultiCamCamera.Connector Connector
    //    {
    //        get { return (GrabLinkMultiCamCamera.Connector)base.Connector; }
    //        set { base.Connector = value; }
    //    }

    //    [Category("Board")]
    //    public new GrabLinkMultiCamCamera.BoardTopology BoardTopology
    //    {
    //        get { return (GrabLinkMultiCamCamera.BoardTopology)base.BoardTopology; }
    //        set { base.BoardTopology = value; }
    //    }

    //    [Category("Board")]
    //    public GrabLinkMultiCamCamera.TapConfiguration TapConfiguration
    //    {
    //        get { return this.m_TapConfiguration; }
    //        set { this.m_TapConfiguration = value; }
    //    }

    //    [Category("Board")]
    //    public GrabLinkMultiCamCamera.TapGeometry TapGeometry
    //    {
    //        get { return this.m_TapGeometry; }
    //        set { this.m_TapGeometry = value; }
    //    }
    //    #endregion

    //    #region ConstructConfiguration Members
    //    protected override void SetDefaultValues()
    //    {
    //        base.SetDefaultValues();

    //        this.Connector = GrabLinkMultiCamCamera.Connector.M;
    //        //this.BoardType = EureSysGrabLinkCamera.BoardTypes.GRABLINK_BASE;
    //        this.BoardTopology = GrabLinkMultiCamCamera.BoardTopology.MONO;
    //        this.TapConfiguration = GrabLinkMultiCamCamera.TapConfiguration.BASE_1T10;
    //        this.TapGeometry = GrabLinkMultiCamCamera.TapGeometry.A1X_1Y;
    //    }
    //    #endregion
    //}
    #endregion
    
}
