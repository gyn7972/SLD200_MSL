/*
 * Purpose
 *      Euresys Frame Grabber 제어를 위한 MultiCam 라이브러리 제어클래스에 대해서 정의한다.
 * 
 * Revision
 *      1. Created: 2017.12.06 LEE.SH
 * 
 */

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace QMC.Common.Vision.EureSys
{
    #region MultiCamException
    internal class MultiCamException : System.Exception
    {
        public MultiCamException(string error) : base(error) { }
    }
    #endregion

    #region MultiCam
    /// <summary>
    /// Class to expose the MultiCam C API in .NET
    /// </summary>
    internal static class MultiCam
    {
        /// <summary>
        /// Imported from the MultiCam DLL
        /// </summary>
        #region Dll Imports
        [DllImport("MultiCam.dll")]
        private static extern int McOpenDriver(IntPtr instanceName);
        [DllImport("MultiCam.dll")]
        private static extern int McCloseDriver();
        [DllImport("MultiCam.dll")]
        private static extern int McCreate(uint modelInstance, out uint instance);
        [DllImport("MultiCam.dll")]
        private static extern int McCreateNm(string modelName, out uint instance);
        [DllImport("MultiCam.dll")]
        private static extern int McDelete(uint instance);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamInt(uint instance, uint parameterId, int value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamNmInt(uint instance, string parameterName, int value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamStr(uint instance, uint parameterId, string value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamNmStr(uint instance, string parameterName, string value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamFloat(uint instance, uint parameterId, double value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamNmFloat(uint instance, string parameterName, double value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamInst(uint instance, uint parameterId, uint value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamNmInst(uint instance, string parameterName, uint value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamPtr(uint instance, uint parameterId, IntPtr value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamNmPtr(uint instance, string parameterName, IntPtr value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamInt64(uint instance, uint parameterId, Int64 value);
        [DllImport("MultiCam.dll")]
        private static extern int McSetParamNmInt64(uint instance, string parameterName, Int64 value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamInt(uint instance, uint parameterId, out int value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamNmInt(uint instance, string parameterName, out int value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamStr(uint instance, uint parameterId, IntPtr value, uint maxLength);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamNmStr(uint instance, string parameterName, IntPtr value, uint maxLength);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamFloat(uint instance, uint parameterId, out double value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamNmFloat(uint instance, string parameterName, out double value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamInst(uint instance, uint parameterId, out uint value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamNmInst(uint instance, string parameterName, out uint value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamPtr(uint instance, uint parameterId, out IntPtr value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamNmPtr(uint instance, string parameterName, out IntPtr value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamInt64(uint instance, uint parameterId, out Int64 value);
        [DllImport("MultiCam.dll")]
        private static extern int McGetParamNmInt64(uint instance, string parameterName, out Int64 value);
        [DllImport("MultiCam.dll")]
        private static extern int McRegisterCallback(uint instance, CallBack callbackFunction, uint context);
        [DllImport("MultiCam.dll")]
        private static extern int McWaitSignal(uint instance, int signal, uint timeout, out SIGNALINFO info);
        [DllImport("MultiCam.dll")]
        private static extern int McGetSignalInfo(uint instance, int signal, out SIGNALINFO info);
        #endregion

        #region Define
        #region Default object instance Constants
        /// <summary>
        /// MultiCam Default Constants
        /// </summary>
        [Serializable]
        [Flags]
        public enum DefaultConstants : uint
        {
            /// <summary>
            /// Maximum number of characters in the read string.
            /// </summary>
            MaxValueLength = 1024,

            /// <summary>
            /// Surface Object Class
            /// </summary>
            SurfaceClass = 0x4,

            /// <summary>
            /// Channel Object Class
            /// </summary>
            ChannelClass = 0x8,

            /// <summary>
            /// Config Object Class
            /// </summary>
            ConfigClass = 0x2,

            /// <summary>
            /// Board Object Class
            /// </summary>
            BoardClass = 0xE,

            /// <summary>
            /// Default Configuration Instance Template
            /// <para>
            /// The Configuration object groups all MultiCam parameters dedicated to the control of system wide features.
            /// The system should be basically understood as the set of Euresys boards installed inside a host computer.
            /// The configuration object also addresses any hardware or software element of the host computer requesting some degree of control for the MultiCam system operation.
            /// The configuration object does not belong to a true class, as it is unique within the system.
            /// There is no need for the user to instantiate a Configuration class object using the McCreate or McCreateNm function.
            /// The Configuration object is natively made available to the application when the MultiCam driver is connected to it.
            /// </para>
            /// </summary>
            //Configuration = 0x20000000,
            Configuration = ((ConfigClass << 28) | 0),

            /// <summary>
            /// Default Board Instance Template
            /// <para>
            /// The Board object groups all MultiCam parameters dedicated to the control of features specific to a board.
            /// The Board object MultiCam parameters also address the access of I/O lines from an application program, implementing the general-purpose I/O functionality.
            /// The Board object does not belong to a true class, as it is unique for each Euresys board installed inside a host computer.
            /// There is no need for the user to instantiate a Board class object using the McCreate or McCreateNm function.
            /// The Board objects are natively made available to the application for each installed Euresys board when the MultiCam driver is opened.
            /// </para>
            /// </summary>
            //Board = 0xE0000000,
            Board = ((BoardClass << 28) | 0),

            /// <summary>
            /// Default Channel Instance Template
            /// <para>
            /// The Channel class groups all MultiCam parameters dedicated to the control of image acquisition related features.
            /// A Channel object is an instance of the Channel class, represented by a dedicated set of such parameters.
            /// </para>
            /// </summary>
            //Channel = 0x8000FFFF,
            Channel = ((ChannelClass << 28) | 0x0000FFFF),

            /// <summary>
            /// Default Surface Handle Instance Template
            /// <para>
            /// The surface is a container where a 2D image can be stored.
            /// In most situations, the surface is a buffer in the host memory.
            /// Other types of surfaces may be defined, such as the hardware frame buffer located inside a frame grabber.
            /// In the particular case of a line-scan camera, the surface can be used as a circular buffer.
            /// This implies that, although the surface is 2D-limited, the incoming data flow is continuous and virtually unlimited.
            /// Regarding the acquisition process, the surface is the destination where the grabbed images from the cameras are recorded.
            /// The overall goal of the MultiCam driver is to provide flexible channels to route images coming from a camera towards a specified surface.
            /// </para>
            /// </summary>
            //DefaultSurfaceHandle = 0x4FFFFFFF,
            DefaultSurfaceHandle = ((SurfaceClass << 28) | 0x0FFFFFFF),
        }
        #endregion

        #region Specific parameter values Constants
        /// <summary>
        /// MultiCam Specific Parameter Values Constants
        /// </summary>
        [Serializable]
        [Flags]
        public enum SpecificParameterConstants
        {
            /// <summary>
            ///
            /// </summary>
            Infinite = -1,

            /// <summary>
            ///
            /// </summary>
            Indeterminate = -1,

            /// <summary>
            /// 
            /// </summary>
            LowPart = 0,

            /// <summary>
            /// 
            /// </summary>
            HighPart = 1,

            /// <summary>
            ///
            /// </summary>
            Disable = 0,

            /// <summary>
            /// 
            /// </summary>
            Unknown = -2,
        }
        #endregion

        #region Signal handling Constants
        /// <summary>
        /// MultiCam Signal Identifier
        /// <para>
        /// The MultiCam signals are not named with a character string as parameters are.
        /// In the body of C functions used to interact with the MultiCam system, they are referred to by a signal identifier defined in the header file MultiCam.h.
        /// Throughout the documentation, the MultiCam signals are designated by a descriptive text.
        /// The signal identifier is an integer value unambiguously designating the signal.
        /// </para>
        /// </summary>
        [Serializable]
        [Flags]
        public enum Signals
        {
            /// <summary>
            /// Signal Enable : using only SetParam() function
            /// <para>
            /// Selection of callback or waiting signalsl1
            /// </para>
            /// </summary>
            Enable = (24 << 14),

            /// <summary>
            /// Signal Event : using only GetParam() function
            /// <para>
            /// Returns the operating system event object associated with a particular MultiCam signal
            /// Linkage of signals to Windows events
            /// This collection parameter holds operating system handles to event objects that are signaled when MultiCam signals occur.
            /// </para>
            /// <para>
            /// The retrieved value may be cast into an operating system handle (HANDLE) and subsequently used in any of the following wait functions:
            /// WaitForSingleObject(), WaitForMultipleObject(), MsgWaitForMultipleObjects().
            /// The operating system event is signaled each time an enabled MultiCam signal occurs.
            /// Enabling a MultiCam signal is done with the SignalEnable parameter.
            /// It is allowed to enable several signals.
            /// When waiting for the Surface Processing signal, it is the application responsibility to reset the SurfaceState parameter of the PROCESSING surface to FREE when done.
            /// Failure to do so would prevent the surface from being used by subsequent acquisition phases.
            /// The MultiCam signal information associated with the event may be retrieved by calling the McGetSignalInfo function.
            /// If the advanced signaling mechanism is used, the callback signaling mechanism cannot be used.
            /// However, the waiting signaling mechanism can be used as long as the waiting function is not used to wait for an event used by the advanced signaling mechanism.
            /// </para>
            /// </summary>
            // Example
            // HANDLE MyHandle;
            // McSetParamInt(hChannel, MC_SignalEnable + MC_SIG_SURFACE_FILLED, MC_SignalEnable_ON);
            // McGetParamInt(hChannel, MC_SignalEvent + MC_SIG_SURFACE_FILLED, (int*)&MyHandle);
            // WaitForSingleObject(MyHandle, INFINITE);
            Event = (25 << 14),

            /// <summary>
            /// Any
            /// <para>
            /// No signaling method has been selected.
            /// </para>
            /// </summary>
            Any = 0,

            /// <summary>
            /// Surface Processing
            /// <para>
            /// A surface of the channel cluster enters the PROCESSING state.
            /// This signal is issued when a surface of the destination cluster enters the state PROCESSING.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_SURFACE_PROCESSING
            /// usage : MC_SignalEvent + MC_SIG_SURFACE_PROCESSING
            /// </code>
            /// </para>
            /// </summary>
            SurfaceProcessing = 1,

            /// <summary>
            /// Surface Filled
            /// <para>
            /// A surface of the channel cluster enters the FILLED state.
            /// This signal is issued when a surface of the destination cluster enters the state FILLED.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_SURFACE_FILLED
            /// usage : MC_SignalEvent + MC_SIG_SURFACE_FILLED
            /// </code>
            /// </para>
            /// </summary>
            SurfaceFilled = 2,

            /// <summary>
            /// Unrecoverable Overrun
            /// <para>
            /// not describe this signal define in MultiCam manual
            /// </para>
            /// </summary>
            UnrecoverableOverrun = 3,

            /// <summary>
            /// Frame Trigger Violation
            /// <para>
            /// A frame trigger has been received that can not be handle because an acquisition is still in progress.
            /// This exception is signaled with a MultiCam signal identified by MC_SIG_FRAMETRIGGER_VIOLATION.
            /// This signal is issued when a frame or page trigger has been received which could not be handled because an acquisition phase was still in progress.
            /// The trigger is lost.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_FRAME_TRIGGER_VIOLATION
            /// usage : MC_SignalEvent + MC_SIG_FRAME_TRIGGER_VIOLATION
            /// </code>
            /// </para>
            /// </summary>
            FrameTriggerViolation = 4,

            /// <summary>
            /// Start Exposure
            /// <para>
            /// Beginning of the exposure phase of the acquisition.
            /// This signal is issued at the beginning of the frame exposure condition.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_START_EXPOSURE
            /// usage : MC_SignalEvent + MC_SIG_START_EXPOSURE
            /// </code>
            /// </para>
            /// </summary>
            StartExposure = 5,

            /// <summary>
            /// End Exposure
            /// <para>
            /// End of the exposure phase of the acquisition and the beginning of a readout.
            /// This signal is issued at the end of the frame exposure condition.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_END_EXPOSURE
            /// usage : MC_SignalEvent + MC_SIG_END_EXPOSURE
            /// </code>
            /// </para>
            /// </summary>
            EndExposure = 6,

            /// <summary>
            /// Acquisition Failure
            /// <para>
            /// The channel acquisition time-out timer expired before the end of the acquisition.
            /// The channel is disabled and must be deleted.
            /// This signal is issued when the channel acquisition time-out timer expires before the end of the acquisition phase. (*)
            /// (*) On Picolo boards, in case of a loss of video signal during an acquisition, the video digitizer does not provide the loss of signal information immediately, 
            /// and the last acquired image may contain invalid data.
            /// This exception is signaled with a MultiCam signal identified by MC_SIG_ACQUISITION_FAILURE.
            /// This signal is issued when the channel acquisition timeout timer expires before the end of the acquisition.
            /// In that case the channel is disabled and must be deleted.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_ACQUISITION_FAILURE
            /// usage : MC_SignalEvent + MC_SIG_ACQUISITION_FAILURE
            /// </code>
            /// </para>
            /// </summary>
            AcquisitionFailure = 7,

            /// <summary>
            /// Cluster Unavailable
            /// <para>
            /// The channel cluster is not available.
            /// This signal is issued when the destination cluster is not able to receive the acquired data.
            /// This exception is signaled with a MultiCam signal identified by MC_SIG_CLUSTER_UNAVAILABLE.
            /// This signal is issued when the cluster mechanism has not been able to designate one of its surface as the destination of frame or page acquisition.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_CLUSTER_UNAVAILABLE
            /// usage : MC_SignalEvent + MC_SIG_CLUSTER_UNAVAILABLE
            /// </code>
            /// </para>
            /// </summary>
            ClusterUnavailable = 8,

            /// <summary>
            /// Release
            /// <para>
            /// Releasing of a waiting thread (*)
            /// (*) This signal is generated only with Domino boards.
            /// This signal is issued when the object may be moved away from the camera, at the end of the exposure.
            /// With interlaced cameras, this signal is issued at the end of the second field's exposure.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_RELEASE
            /// </code>
            /// </para>
            /// </summary>
            Release = 9,

            /// <summary>
            /// End Acquition Sequence
            /// <para>
            /// The acquisition sequence is completed.
            /// This signal is issued when the acquisition sequence terminates.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_END_ACQUISITION_SEQUENCE
            /// </code>
            /// </para>
            /// </summary>
            EndAcquitionSequence = 10,

            /// <summary>
            /// Start Acquisition Sequence
            /// <para>
            /// The acquisition sequence is started.
            /// This signal is issued when the acquisition sequence begins.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_START_ACQUISITION_SEQUENCE
            /// </code>
            /// </para>
            /// </summary>
            StartAcquisitionSequence = 11,

            /// <summary>
            /// End Channel Activity
            /// <para>
            /// The channel has no more task in process.
            /// This signal is issued when the channel leaves the active state.
            /// </para>
            /// <para>
            /// <code>
            /// usage : MC_SignalEnable + MC_SIG_END_CHANNEL_ACTIVITY
            /// </code>
            /// </para>
            /// </summary>
            EndChannelActivity = 12,

            /// <summary>
            ///
            /// </summary>
            GoLow = (1 << 12),

            /// <summary>
            ///
            /// </summary>
            GoHigh = (2 << 12),

            /// <summary>
            ///
            /// </summary>
            GoOpen = (3 << 12),

            /// <summary>
            /// 
            /// </summary>
            MaxBoardEvents = (3 << 12),

            /// <summary>
            /// The signal is included in the selection.
            /// </summary>
            On,

            /// <summary>
            /// The signal is not included in the selection.
            /// </summary>
            Off,

            /// <summary>
            /// The signal is disabled until the end of acquisition sequence.
            /// </summary>
            After_Eas,
        }
        #endregion

        #region Signal handling Type Definitions

        /// <summary>
        /// Declare delegate callback function
        /// <para>
        /// The user should define the callback function in the application code in accordance with this prototype.
        /// The callback function is called by the MultiCam driver when a channel or a processor issues a pre-defined signal.
        /// The pre-defined signal should be enabled with the SignalEnable parameter.
        /// It is allowed to enable several signals.
        /// If more than one enabled signals are issued simultaneously from an object, the callback function is successively called for each signal occurrence.
        /// </para>
        /// <para>
        /// When the signal occurs, the callback dedicated thread is released, and the callback function is automatically invoked.
        /// The thread is restored to an idle condition when the callback function is exited.
        /// The function has a single argument, which is a structure passing information on the signal that caused the callback function. 
        /// This structure has the signal information type.
        /// If the callback signaling mechanism is used, the waiting and advanced signaling mechanisms cannot be used.
        /// </para>
        /// </summary>
        /// <param name="signalInfo">
        /// Argument providing the signal information structure.
        /// </param>
        /// <returns></returns>
        public delegate int CallBack(ref MultiCam.SIGNALINFO signalInfo);

        /// <summary>
        /// The SignalInfo object conveys information about a MultiCam signal.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SIGNALINFO
        {
            public IntPtr Context;
            public uint Instance;
            public int Signal;
            public uint SignalInfo;
            public uint SignalContext;
        };
        #endregion

        #region Status Code
        /// <summary>
        /// Error codes returned by MultiCam DLL functions
        /// </summary>
        [Serializable]
        [Flags]
        public enum StatusCode
        {
            /// <summary>
            /// No Error
            /// </summary>
            OK = 0,

            /// <summary>
            /// No Board Found
            /// </summary>
            NoBoardFound = -1,

            /// <summary>
            /// Bad Parameter
            /// </summary>
            BadParameter = -2,

            /// <summary>
            /// I/O Error
            /// </summary>
            IoError = -3,

            /// <summary>
            /// Internal Error
            /// </summary>
            InternalError = -4,

            /// <summary>
            /// No More Resources
            /// </summary>
            NoMoreResources = -5,

            /// <summary>
            /// Object still in use
            /// </summary>
            InUse = -6,

            /// <summary>
            /// Operation not supported
            /// </summary>
            NotSupported = -7,

            /// <summary>
            /// Parameter database error
            /// </summary>
            DatabaseError = -8,

            /// <summary>
            /// Value out of bound
            /// </summary>
            OutOfBound = -9,

            /// <summary>
            /// Object instance not found
            /// </summary>
            InstanceNotFound = -10,

            /// <summary>
            /// Invalid Handle
            /// </summary>
            InvalidHandle = -11,

            /// <summary>
            /// Timeout
            /// </summary>
            Timeout = -12,

            /// <summary>
            /// Invalid Value
            /// </summary>
            InvalidValue = -13,

            /// <summary>
            /// Value not in range
            /// </summary>
            RangeError = -14,

            /// <summary>
            /// Invalid hardware configuration
            /// </summary>
            BadHwConfig = -15,

            /// <summary>
            /// No Event
            /// </summary>
            NoEvent = -16,

            /// <summary>
            /// License not granted
            /// </summary>
            LicenseNotGranted = -17,

            /// <summary>
            /// Fatal error
            /// </summary>
            FatalError = -18,

            /// <summary>
            /// Hardware event conflict
            /// </summary>
            HwEventConflict = -19,

            /// <summary>
            /// File not found
            /// </summary>
            FileNotFound = -20,

            /// <summary>
            /// Overflow
            /// </summary>
            Overflow = -21,

            /// <summary>
            /// Parameter inconsistency
            /// </summary>
            InvalidParameterSetting = -22,

            /// <summary>
            /// Illegal operation
            /// </summary>
            ParameterIllegalAccess = -23,

            /// <summary>
            /// Cluster busy
            /// </summary>
            ClusterBusy = -24,

            /// <summary>
            /// MultiCam service error
            /// </summary>
            ServiceError = -25,

            /// <summary>
            /// Invalid surface
            /// </summary>
            InvalidSurface = -26,

            /// <summary>
            /// 
            /// </summary>
            MaxErrorsMPF = 3,

            /// <summary>
            /// 
            /// </summary>
            MpfErrorBase = -100,

            /// <summary>
            /// 
            /// </summary>
            BadGrabberConfig = -101,

            /// <summary>
            /// 
            /// </summary>
            IllegalPageLengthValue = -102,
        }
        #endregion

        #region MultiCam DLL Parameter
        /// <summary>
        /// Camera Parameter Name (Setter)
        /// </summary>
        [Serializable]
        public enum SetParameter
        {
            /// <summary>
            /// Path and filename of the error log file
            /// <para>
            /// This parameter specifies the path and the filename of the error log file that is created when the application returns a MC_INVALID_PARAMETER_SETTING (-22) error code.
            /// The incorrect parameters are reported in the log file, including the wrong value and the possible correct values.
            /// When specified, the log file is created and filled during the consistency check.
            /// When unspecified, the consistency check does not produce a log file...
            /// </para>
            /// </summary>
            ErrorLog = (81 << 14),

            /// <summary>
            /// Horizontal size of the transferred images
            /// <para>
            /// This parameter is expressed as a number of columns.
            /// It can be set only with Picolo boards.
            /// It exposes the result of any condition adjustment that could affect the image width during the acquisition process.
            /// The surface in the destination cluster will receive an image, the width of which is that number of columns.
            /// In case of area-scan cameras, the size of the destination surface matches the size of the acquired frame.
            /// In case of line-scan cameras, the size of the destination surface matches the size of the acquired page.
            /// The horizontal size of the image is scaled to the defined ImageSizeX number of pixels per line.
            /// </para>
            /// </summary>
            ImageSizeX = (523 << 14),

            /// <summary>
            /// Vertical size of the transferred images
            /// <para>
            /// This parameter is expressed as a number of lines.
            /// It can be set only with Picolo boards.
            /// It exposes the result of any condition adjustment that could affect the image height during the acquisition process.
            /// The surface in the destination cluster will receive an image the height of which is that number of lines.
            /// In case of area-scan cameras, the size of the destination surface matches the size of the acquired frame.
            /// In case of line-scan cameras, the size of the destination surface matches the size of the acquired page.
            /// The vertical size of the image is scaled to the defined ImageSizeY number of lines.
            /// </para>
            /// </summary>
            ImageSizeY = (524 << 14),

            /// <summary>
            /// Size required to contain one line of the plane
            /// <para>
            /// MultiCam creates the surfaces and automatically allocates the memory buffers, if not done by the application.
            /// The following channel parameters configure the automatic allocation: BufferSize, BufferPitch, ImagePlaneCount and SurfaceCount.
            /// MultiCam decides the adequate number of surfaces for the selected acquisition mode.
            /// This parameter is expressed as a number of bytes.
            /// Getting this parameter gives the minimum size (in bytes) required to contain one line of the plane produced by the channel.
            /// Setting this parameter defines the desired line pitch.
            /// If allowed, this value will be used in the computation of other Cluster category parameters.
            /// The minimum value is reported by parameter MinBufferPitch.
            /// The dimension of this collection parameter is specified by ImagePlaneCount.
            /// The assignment of the planes is returned by SurfacePlaneName.
            /// For a complete description of color formats, see MultiCam Storage Formats.
            /// </para>
            /// </summary>
            BufferPitch = (3336 << 14),

            /// <summary>
            /// Recommended size (in bytes) for the image buffer(s)
            /// <para>
            /// MultiCam creates the surfaces and automatically allocates the memory buffers, if not done by the application.
            /// The following channel parameters configure the automatic allocation: BufferSize, BufferPitch, ImagePlaneCount and SurfaceCount.
            /// MultiCam decides the adequate number of surfaces for the selected acquisition mode.
            /// This parameter is expressed as a number of bytes.
            /// It provides the buffer size needed to contain one image produced by the channel.
            /// If ImagePlaneCount > 1, the channel produces a "multi-plane" image.
            /// In this case, one must allocate ImagePlaneCount buffers.
            /// Each buffer size is given in the BufferSize collection members.
            /// </para>
            /// </summary>
            BufferSize = (3333 << 14),

            /// <summary>
            /// Name of the CAM file
            /// <para>
            /// This parameter specifies a camera configuration file as a character string. The .cam extension may or may not be included. The maximum string length is 1024.
            /// </para>
            /// </summary>
            CamFile = (11 << 14),

            /// <summary>
            /// Designation of color format
            /// <para>
            /// This parameter summarizes all the properties describing how the frame grabber stores pixel data in the destination surface.
            /// </para>
            /// </summary>
            ColorFormat = (2224 << 14),

            /// <summary>
            /// Board index in the list of MultiCam compliant boards returned by the driver
            /// <para>
            /// This parameter gives the index of a particular board in the list returned by the driver.
            /// This parameter is used to access the Board object parameters related to the board.
            /// The MultiCam compliant boards are assigned consecutive integer numbers starting at 0.
            /// The indexing order is system dependent.
            /// </para>
            /// </summary>
            DriverIndex = (0 << 14),

            /// <summary>
            /// Indication of connector used by channel
            /// <para>
            /// The value of this parameter is entered at the channel creation by means of the Connector argument.
            /// The consistency of this parameter should be maintained channel-wide.
            /// </para>
            /// </summary>
            Connector = (682 << 14),

            /// <summary>
            /// State of the channel
            /// <para>
            /// This parameter gives access to the state of the channel.
            /// </para>
            /// </summary>
            ChannelState = (15 << 14),

            /// <summary>
            /// Fundamental acquisition mode
            /// <para>
            /// This parameter gives a fundamental acquisition mode.
            /// </para>
            /// </summary>
            AcquisitionMode = (3396 << 14),

            /// <summary>
            /// Control of acquisition sequences count
            /// <para>
            /// An activity period of a channel is made of one or several acquisition sequences.
            /// The ActivityLength parameter establishes the number of acquisition sequences constituting a channel activity period.
            /// The user is invited to set this parameter when AcquisitionMode is VIDEO or LONGPAGE.
            /// MultiCam sets this parameter to 1 when AcquisitionMode is SNAPSHOT, WEB, PAGE or HFR.
            /// Setting ActivityLength to MC_INDETERMINATE results in indefinitely repeated acquisition sequences.
            /// A user break is required to stop the channel activity.
            /// </para>
            /// </summary>
            //ActivityLength = (3406 << 14),
            AcquisitionSequenceCount = (3406 << 14),

            /// <summary>
            /// Number of frames constituting a phase
            /// <para>
            /// The parameter establishes the total number of frames acquired within an acquisition phase.
            /// It is relevant only when AcquisitionMode is HFR. The range of values is [2..256].
            /// </para>
            /// </summary>
            //PhaseLength_Fr = (3409 << 14),
            AcquisitionPhaseFrameCount = (3409 << 14),

            /// <summary>
            /// Grabber acquisition sequence triggering mode
            /// <para>
            /// The TrigMode parameter establishes the starting conditions of an acquisition sequence.
            /// </para>
            /// </summary>
            //TrigMode = (512 << 14),
            AcquisitionTriggerMode = (512 << 14),

            /// <summary>
            /// Grabber subsequent acquisition phases or slices triggering mode
            /// <para>
            /// The NextTrigMode parameter establishes the starting conditions of the subsequent acquisition phases or slices.
            /// </para>
            /// </summary>
            //NextTrigMode = (663 << 14),
            SubsequentAcquisitionTriggerMode = (663 << 14),

            /// <summary>
            /// Grabber end triggering mode
            /// <para>
            /// The EndTrigMode parameter establishes the conditions of a sequence termination.
            /// </para>
            /// </summary>
            //EndTrigMode = (2916 << 14),
            LastAcquisitionTriggerMode = (2916 << 14),

            /// <summary>
            /// Control of the sequence length by frame count
            /// <para>
            /// The SeqLength_Fr parameter establishes the number of frames constituting a sequence.
            /// </para>
            /// </summary>
            //SeqLength_Fr = (3407 << 14),
            AcquisitionSequenceFrameCount = (3407 << 14),

            /// <summary>
            /// Grabber break effect on the acquisition phase
            /// <para>
            /// The BreakEffect parameter establishes the effect of a user break on the channel.
            /// </para>
            /// </summary>
            //BreakEffect = (2011 << 14),
            AcquisitionChannelBreakEffect = (2011 << 14),

            /// <summary>
            /// Means to force an event trigger from the application
            /// <para>
            /// Setting value TRIG to this parameter force a "software" trigger event.
            /// Setting value ENDTRIG to this parameter force a "software" end trigger event.
            /// </para>
            /// </summary>
            //ForceTrig = (50 << 14),
            AcquisitionTriggerEventForce = (50 << 14),

            /// <summary>
            /// Designation by I/O index of trigger hardware line from outside system
            /// <para>
            /// It designates the hardware line sensed by the channel and aimed at generating the trigger event.
            /// The hardware line is only involved when TrigMode is HARD or COMBINED.
            /// </para>
            /// </summary>
            //TrigLineIndex = (3445 << 14),
            ExternalHardwareTriggeringIoIndex = (3445 << 14),

            /// <summary>
            /// Significant edge of designated trigger hardware line from outside system
            /// <para>
            /// This parameter applies to the hardware line designated by TrigLine or TrigLineIndex.
            /// Along with TrigCtl and TrigFilter, it declares the grabber attributes of the trigger line sensed by the channel and aimed at generating the trigger event.
            /// The TrigEdge parameter determines the significant edge of the end trigger pulse.
            /// This parameter applies when acquisition control settings require a hardware trigger or page trigger (all acquisition modes).
            /// </para>
            /// </summary>
            //TrigEdge = (664 << 14),
            ExternalHardwareEdgeTriggering = (664 << 14),

            /// <summary>
            /// Number of active pixels in the line
            /// <para>
            /// This parameter is expressed as a number of camera sensor pixels.
            /// It is used to characterize digital line-scan or area-scan cameras. 
            /// For analog area-scan cameras, the active line is expressed using time measurement. 
            /// Refer to the Hactive_ns parameter.
            /// For digital area-scan cameras, the manufacturer announces the number of horizontal pixels belonging to the sensor that are effectively available at the camera output. 
            /// This is a measure of the length of the camera active window.
            /// This number is declared by the Hactive_Px parameter.
            /// Values applicable to Grablink Express
            /// 128 to 65535 pixels
            /// </para>
            /// </summary>
            Hactive_Px = (1021 << 14),

            /// <summary>
            /// Number of active video lines in the frame
            /// <para>
            /// This parameter is expressed as a number of video lines.
            /// An active line is, by definition, a video line where useful visual information can appear. 
            /// Blanking lines take no part in the count of active lines.
            /// In case of interlaced scanning, Vactive_Ln represents the number of active lines for both fields altogether. 
            /// This is equivalent to the number of active half-lines per field.
            /// In some cases of dual-tap structure, Vactive_Ln represents the number of active lines for both channels altogether.
            /// This parameter is a measure of the height of the camera active window.
            /// It is used to characterize area-scan cameras. It is meaningless for line-scan cameras.
            /// Values applicable to Grablink Express
            /// 1 to 65535 lines
            /// </para>
            /// </summary>
            Vactive_Ln = (710 << 14),

            /// <summary>
            /// Designation of trigger hardware line from outside system
            /// <para>
            /// It designates the hardware line sensed by the channel and aimed at generating the trigger event.
            /// The hardware line is only involved when TrigMode or NextTrigMode is HARD or COMBINED.
            /// The selection of the electrical style of the hardware line, by means of parameter TrigCtl, is a prerequisite.
            /// </para>
            /// </summary>
            HardwareTriggerLine = (666 << 14),

            /// <summary>
            /// Electrical style of designated trigger hardware line from outside system
            /// <para>
            /// This parameter applies to the hardware line designated by TrigLine or TrigLineIndex.
            /// Along with TrigEdge and TrigFilter, it declares the grabber attributes of the trigger line sensed by the channel and aimed at generating the trigger event.
            /// This parameter applies when acquisition control settings require a hardware trigger or page trigger (all acquisition modes).
            /// </para>
            /// </summary>
            HardwareTriggerControl = (513 << 14),

            /// <summary>
            /// Camera Link tap configuration
            /// <para>
            /// This parameter declares the Camera Link tap configuration used by the camera.
            /// The naming conventions and the detailed description of all Camera Link tap configuration are explained in Camera Link Tap Configuration.
            /// </para>
            /// </summary>
            CameraLinkTapConfiguration = (4268 << 14),

            /// <summary>
            /// Camera Link tap geometry
            /// <para>
            /// This parameter declares the Camera Link tap geometry used by the camera.
            /// Based on this parameter together with TapConfiguration, the frame grabber is able to re-arrange the data in the destination surface.
            /// The naming conventions and the detailed description of all Camera Link tap configurations are explained in Camera Link Tap Geometry.
            /// </para>
            /// </summary>
            CameraLinkTapGeometry = (4273 << 14),

            /// <summary>
            /// Arrangement of the cameras connected to the board
            /// <para>
            /// This parameter defines the arrangement of cameras that can be potentially connected to the frame grabber.
            /// However, some of the camera positions declared by the topology may not be connected.
            /// The application must select and declare the topology before the first assignation of a MultiCam Channel to this board; 
            /// the topology may not be modified while at least one channel is assigned to the board.
            /// </para>
            /// </summary>
            BoardTopology = (59 << 14),

            ImageFlipX = (1340 << 14),
            ImageFlipY = (525 << 14),
            SurfaceCount = (82 << 14),
        }

        /// <summary>
        /// Camera Parameter Name (Getter)
        /// </summary>
        [Serializable]
        public enum GetParameter
        {
            /// <summary>
            /// Type of the board
            /// </summary>
            BoardIdentifier = (3 << 14),

            /// <summary>
            /// Type of the board
            /// </summary>
            BoardType = (6 << 14),

            /// <summary>
            /// State of the channel
            /// <para>
            /// This parameter gives access to the state of the channel.
            /// </para>
            /// </summary>
            ChannelState = (15 << 14),

            /// <summary>
            /// Horizontal size of the transferred images
            /// <para>
            /// This parameter is expressed as a number of columns.
            /// It can be set only with Picolo boards.
            /// It exposes the result of any condition adjustment that could affect the image width during the acquisition process.
            /// The surface in the destination cluster will receive an image, the width of which is that number of columns.
            /// In case of area-scan cameras, the size of the destination surface matches the size of the acquired frame.
            /// In case of line-scan cameras, the size of the destination surface matches the size of the acquired page.
            /// The horizontal size of the image is scaled to the defined ImageSizeX number of pixels per line.
            /// </para>
            /// </summary>
            ImageSizeX = (523 << 14),

            /// <summary>
            /// Vertical size of the transferred images
            /// <para>
            /// This parameter is expressed as a number of lines.
            /// It can be set only with Picolo boards.
            /// It exposes the result of any condition adjustment that could affect the image height during the acquisition process.
            /// The surface in the destination cluster will receive an image the height of which is that number of lines.
            /// In case of area-scan cameras, the size of the destination surface matches the size of the acquired frame.
            /// In case of line-scan cameras, the size of the destination surface matches the size of the acquired page.
            /// The vertical size of the image is scaled to the defined ImageSizeY number of lines.
            /// </para>
            /// </summary>
            ImageSizeY = (524 << 14),

            /// <summary>
            /// Size required to contain one line of the plane
            /// <para>
            /// MultiCam creates the surfaces and automatically allocates the memory buffers, if not done by the application.
            /// The following channel parameters configure the automatic allocation: BufferSize, BufferPitch, ImagePlaneCount and SurfaceCount.
            /// MultiCam decides the adequate number of surfaces for the selected acquisition mode.
            /// This parameter is expressed as a number of bytes.
            /// Getting this parameter gives the minimum size (in bytes) required to contain one line of the plane produced by the channel.
            /// Setting this parameter defines the desired line pitch.
            /// If allowed, this value will be used in the computation of other Cluster category parameters.
            /// The minimum value is reported by parameter MinBufferPitch.
            /// The dimension of this collection parameter is specified by ImagePlaneCount.
            /// The assignment of the planes is returned by SurfacePlaneName.
            /// For a complete description of color formats, see MultiCam Storage Formats.
            /// </para>
            /// </summary>
            BufferPitch = (3336 << 14),

            /// <summary>
            /// Recommended size (in bytes) for the image buffer(s)
            /// <para>
            /// MultiCam creates the surfaces and automatically allocates the memory buffers, if not done by the application.
            /// The following channel parameters configure the automatic allocation: BufferSize, BufferPitch, ImagePlaneCount and SurfaceCount.
            /// MultiCam decides the adequate number of surfaces for the selected acquisition mode.
            /// This parameter is expressed as a number of bytes.
            /// It provides the buffer size needed to contain one image produced by the channel.
            /// If ImagePlaneCount > 1, the channel produces a "multi-plane" image.
            /// In this case, one must allocate ImagePlaneCount buffers.
            /// Each buffer size is given in the BufferSize collection members.
            /// </para>
            /// </summary>
            BufferSize = (3333 << 14),

            /// <summary>
            /// Address of the surface or list of addresses of the surface planes
            /// <para>
            /// This parameter declares the address of the surface for one plane.
            /// If PlaneCount > 1, the parameter is a collection of the starting addresses for every plane constituting the surface.
            /// </para>
            /// </summary>
            //SurfaceAddr = (28 << 14),
            SurfaceMemoryImageBufferAddr = (28 << 14),

            /// <summary>
            /// Means to get informed on the acquisition rate
            /// <para>
            /// This get-only parameter reports the number of frames acquiring during a second.
            /// It is applicable exclusively when AcquisitionMode has one of the following values: VIDEO, HFR or SNAPSHOT.
            /// </para>
            /// </summary>
            //PerSecond_Fr = (3452 << 14),
            AcquisitionFramePerSecond = (3452 << 14),

            /// <summary>
            /// Number of active pixels in the line
            /// <para>
            /// This parameter is expressed as a number of camera sensor pixels.
            /// It is used to characterize digital line-scan or area-scan cameras. 
            /// For analog area-scan cameras, the active line is expressed using time measurement. 
            /// Refer to the Hactive_ns parameter.
            /// For digital area-scan cameras, the manufacturer announces the number of horizontal pixels belonging to the sensor that are effectively available at the camera output. 
            /// This is a measure of the length of the camera active window.
            /// This number is declared by the Hactive_Px parameter.
            /// Values applicable to Grablink Express
            /// 128 to 65535 pixels
            /// </para>
            /// </summary>
            Hactive_Px = (1021 << 14),

            /// <summary>
            /// Number of active video lines in the frame
            /// <para>
            /// This parameter is expressed as a number of video lines.
            /// An active line is, by definition, a video line where useful visual information can appear. 
            /// Blanking lines take no part in the count of active lines.
            /// In case of interlaced scanning, Vactive_Ln represents the number of active lines for both fields altogether. 
            /// This is equivalent to the number of active half-lines per field.
            /// In some cases of dual-tap structure, Vactive_Ln represents the number of active lines for both channels altogether.
            /// This parameter is a measure of the height of the camera active window.
            /// It is used to characterize area-scan cameras. It is meaningless for line-scan cameras.
            /// Values applicable to Grablink Express
            /// 1 to 65535 lines
            /// </para>
            /// </summary>
            Vactive_Ln = (710 << 14),

            /// <summary>
            /// Designation of trigger hardware line from outside system
            /// <para>
            /// It designates the hardware line sensed by the channel and aimed at generating the trigger event.
            /// The hardware line is only involved when TrigMode or NextTrigMode is HARD or COMBINED.
            /// The selection of the electrical style of the hardware line, by means of parameter TrigCtl, is a prerequisite.
            /// </para>
            /// </summary>
            HardwareTriggerLine = (666 << 14),

            /// <summary>
            /// Electrical style of designated trigger hardware line from outside system
            /// <para>
            /// This parameter applies to the hardware line designated by TrigLine or TrigLineIndex.
            /// Along with TrigEdge and TrigFilter, it declares the grabber attributes of the trigger line sensed by the channel and aimed at generating the trigger event.
            /// This parameter applies when acquisition control settings require a hardware trigger or page trigger (all acquisition modes).
            /// </para>
            /// </summary>
            HardwareTriggerControl = (513 << 14),

            /// <summary>
            /// Camera Link tap configuration
            /// <para>
            /// This parameter declares the Camera Link tap configuration used by the camera.
            /// The naming conventions and the detailed description of all Camera Link tap configuration are explained in Camera Link Tap Configuration.
            /// </para>
            /// </summary>
            CameraLinkTapConfiguration = (4268 << 14),

            /// <summary>
            /// Camera Link tap geometry
            /// <para>
            /// This parameter declares the Camera Link tap geometry used by the camera.
            /// Based on this parameter together with TapConfiguration, the frame grabber is able to re-arrange the data in the destination surface.
            /// The naming conventions and the detailed description of all Camera Link tap configurations are explained in Camera Link Tap Geometry.
            /// </para>
            /// </summary>
            CameraLinkTapGeometry = (4273 << 14),

            /// <summary>
            /// Arrangement of the cameras connected to the board
            /// <para>
            /// This parameter defines the arrangement of cameras that can be potentially connected to the frame grabber.
            /// However, some of the camera positions declared by the topology may not be connected.
            /// The application must select and declare the topology before the first assignation of a MultiCam Channel to this board; 
            /// the topology may not be modified while at least one channel is assigned to the board.
            /// </para>
            /// </summary>
            BoardTopologyl12 = (59 << 14),
        }
        #endregion
        #endregion

        #region Field
        #endregion

        #region Constructors
        static MultiCam()
        {
        }
        #endregion

        #region Method

        #region Error handling Methods

        /// <summary>
        /// return MultiCam error message
        /// </summary>
        /// <param name="code">
        /// The code variable can be used for error checking.
        /// </param>
        /// <returns></returns>
        private static string GetErrorMessage(int code)
        {
            const int ErrorDesc = (98 << 14);
            string description;
            int status = Math.Abs(code);
            IntPtr pointer = IntPtr.Zero;

            try
            {
                pointer = Marshal.AllocHGlobal((int)MultiCam.DefaultConstants.MaxValueLength + 1);

                if (MultiCam.McGetParamStr((int)MultiCam.DefaultConstants.Configuration, (uint)(ErrorDesc + status), pointer, (int)MultiCam.DefaultConstants.MaxValueLength) != (int)MultiCam.StatusCode.OK)
                    description = "Unknown error";
                else
                    description = Marshal.PtrToStringAnsi(pointer);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }

            return description;
        }

        /// <summary>
        /// MultiCam.dll API실행시 에러발생에 대한 eFramework Log 작성 및 Exception 처리
        /// </summary>
        /// <param name="status">
        /// The status variable can be used for error checking.
        /// </param>
        /// <param name="action">
        /// MultiCam.dll API 실행실패에 대한 텍스트
        /// </param>
        /// <returns></returns>
        private static int ThrowOnMultiCamError(int status, string action)
        {
            int ret = 0;
            if (status != 0)
            {
                String text = action + ": " + GetErrorMessage(status);
                //Log.Write("EureSys.MultiCam.Error", new LogEntry(LogLevel.Highest, text.ToString()));
                Console.WriteLine("EureSys.MultiCam.Error");
                throw new MultiCamException(text);
            }
            return ret;
        }

        /// <summary>
        /// MultiCam.dll API실행시 경고발생에 대한 eFramework Log 작성
        /// </summary>
        /// <param name="status">
        /// The status variable can be used for error checking.
        /// </param>
        /// <param name="action">
        /// MultiCam.dll API 실행실패에 대한 텍스트
        /// </param>
        /// <returns></returns>
        private static int NotThrowOnMultiCamWarning(int status, string action)
        {
            int ret = 0;
            if (status != 0)
            {
                String text = action + ": " + GetErrorMessage(status);
                //Log.Write("EureSys.MultiCam.Warning", new LogEntry(LogLevel.Highest, text.ToString()));
                Console.WriteLine("EureSys.MultiCam.Warning");
            }
            return ret;
        }
        #endregion

        #region Driver connection Methods
        /// <summary>
        /// Establishes the communication of the application process with the MultiCam driver.
        /// </summary>
        /// <returns></returns>
        public static int OpenDriver()
        {
            int ret = 0;
            MultiCam.ThrowOnMultiCamError(MultiCam.McOpenDriver((IntPtr)null),
                                          "Cannot open MultiCam driver");
            return ret;
        }

        /// <summary>
        /// Terminates the communication of the application process with the MultiCam driver.
        /// <para>
        /// If an application successfully calls McOpenDriver several times, it must call McCloseDriver the same number of times, to adequately close the communication with the MultiCam driver.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public static int CloseDriver()
        {
            int ret = 0;
            MultiCam.ThrowOnMultiCamError(MultiCam.McCloseDriver(),
                                          "Cannot close MultiCam driver");
            return ret;
        }
        #endregion

        #region Object creation/deletion Methods
        /// <summary>
        /// Creates an instance for a MultiCam object according to a model referred to by its handle.
        /// </summary>
        /// <param name="modelInstance">
        /// When creating a Channel, use an handle designating the connector structure.
        /// When creating a Surface, use MC_DEFAULT_SURFACE_HANDLE.
        /// </param>
        /// <param name="channelInstance">
        /// Pointer to the newly created instance.
        /// </param>
        /// <returns></returns>
        public static int Create(int modelInstance, out int channelInstance)
        {
            int ret = 0;
            uint value = 0;
            ret = MultiCam.ThrowOnMultiCamError(MultiCam.McCreate((uint)modelInstance, out value),
                                                string.Format("Cannot create '{0}' instance", modelInstance));
            channelInstance = (int)value;
            return ret;
        }

        /// <summary>
        /// Creates an instance for a MultiCam object according to a model referred to by its name.
        /// </summary>
        /// <param name="modelName">
        /// When creating a Channel, use a string representing the connector structure.
        /// When creating a Surface, use McCreate.
        /// </param>
        /// <param name="channelInstance">
        /// Pointer to the newly created instance.
        /// </param>
        /// <returns></returns>
        public static int Create(string modelName, out int channelInstance)
        {
            int ret = 0;
            uint value = 0;
            ret = MultiCam.ThrowOnMultiCamError(MultiCam.McCreateNm(modelName, out value),
                                                string.Format("Cannot create '{0}' instance", modelName));
            channelInstance = (int)value;
            return ret;
        }

        /// <summary>
        /// Deletes the instance of a MultiCam object.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance to delete.
        /// </param>
        /// <returns></returns>
        public static int Delete(int channelInstance)
        {
            int ret = 0;
            ret = MultiCam.ThrowOnMultiCamError(MultiCam.McDelete((uint)channelInstance),
                                                string.Format("Cannot delete '{0}' instance", channelInstance));
            return ret;
        }
        #endregion

        #region Parameter 'setter' Methods
        /// <summary>
        /// Assigns an integer variable to a MultiCam parameter.
        /// The parameter is referred to by-identifier, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Integer value assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, int parameterId, int value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamInt((uint)channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        /// <summary>
        /// Assigns an integer variable to a MultiCam parameter.
        /// The parameter is referred to by-name, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Integer value assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, string parameterName, int value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamNmInt((uint)channelInstance, parameterName, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            return ret;
        }

        public static int SetParam(int channelInstance, Enum parameterName, int value)
        {
            return MultiCam.SetParam(channelInstance, parameterName.ToString(), value);
        }

        /// <summary>
        /// Assigns an string variable to a MultiCam parameter.
        /// The parameter is referred to by-identifier, and is preferably of the string or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Pointer to the string assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, int parameterId, string value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamStr((uint)channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        public static int SetParam(int channelInstance, int parameterId, Enum value)
        {
            return MultiCam.SetParam(channelInstance, parameterId, value.ToString());
        }

        /// <summary>
        /// Assigns an string variable to a MultiCam parameter.
        /// The parameter is referred to by-identifier, and is preferably of the string or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Pointer to the string assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(uint channelInstance, int parameterId, string value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamStr(channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        public static int SetParam(uint channelInstance, int parameterId, Enum value)
        {
            return MultiCam.SetParam(channelInstance, parameterId, value.ToString());
        }

        /// <summary>
        /// Assigns a string variable to a MultiCam parameter.
        /// The parameter is referred to by-name, and is preferably of the string or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Pointer to the string assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, string parameterName, string value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamNmStr((uint)channelInstance, parameterName, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            return ret;
        }

        public static int SetParam(int channelInstance, Enum parameterName, Enum value)
        {
            return MultiCam.SetParam(channelInstance, parameterName.ToString(), value.ToString());
        }

        /// <summary>
        /// Assigns a floating-point variable to a MultiCam parameter.
        /// The parameter is referred to by-identifier, and is preferably of the floating-point type.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Floating-point value assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, int parameterId, double value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamFloat((uint)channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        /// <summary>
        /// Assigns a floating-point variable to a MultiCam parameter.
        /// The parameter is referred to by-name, and is preferably of the floating-point type.
        /// <para>
        /// If the MultiCam parameter is not of the floating-point type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Floating-point value assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, string parameterName, double value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamNmFloat((uint)channelInstance, parameterName, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            return ret;
        }

        public static int SetParam(int channelInstance, Enum parameterName, double value)
        {
            return MultiCam.SetParam(channelInstance, parameterName.ToString(), value);
        }

        /// <summary>
        /// Assigns an instance variable to a MultiCam parameter.
        /// The parameter is referred to by-identifier, and is of the instance type.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Handle of the instance assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, int parameterId, uint value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamInst((uint)channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        /// <summary>
        /// Assigns an instance variable to a MultiCam parameter.
        /// The parameter is referred to by-name, and is of the instance type.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Handle of the instance assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, string parameterName, uint value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamNmInst((uint)channelInstance, parameterName, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            return ret;
        }

        public static int SetParam(int channelInstance, Enum parameterName, uint value)
        {
            return MultiCam.SetParam(channelInstance, parameterName.ToString(), value);
        }

        /// <summary>
        /// Assigns an string variable to a MultiCam parameter.
        /// The parameter is referred to by-identifier.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Pointer to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, int parameterId, IntPtr value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamPtr((uint)channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        /// <summary>
        /// Assigns a string variable to a MultiCam parameter.
        /// The parameter is referred to by-name.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Pointer to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, string parameterName, IntPtr value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamNmPtr((uint)channelInstance, parameterName, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            return ret;
        }

        public static int SetParam(int channelInstance, Enum parameterName, IntPtr value)
        {
            return MultiCam.SetParam(channelInstance, parameterName.ToString(), value);
        }

        /// <summary>
        /// Assigns a string variable to a MultiCam parameter.
        /// The parameter is referred to by-name.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterId">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Pointer to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, int parameterId, Int64 value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamInt64((uint)channelInstance, (uint)parameterId, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            return ret;
        }

        /// <summary>
        /// Assigns an integer variable to a MultiCam parameter.
        /// The parameter is referred to by-name, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to configure.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to configure.
        /// </param>
        /// <param name="value">
        /// Integer value assigned to the parameter.
        /// </param>
        /// <returns></returns>
        public static int SetParam(int channelInstance, string parameterName, Int64 value)
        {
            int ret = 0;
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McSetParamNmInt64((uint)channelInstance, parameterName, value),
                                                     string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            return ret;
        }

        public static int SetParam(int channelInstance, Enum parameterName, Int64 value)
        {
            return MultiCam.SetParam(channelInstance, parameterName.ToString(), value);
        }
        #endregion

        #region Parameter 'getter' Methods
        /// <summary>
        /// Returns the current value of a MultiCam parameter as an integer variable.
        /// The parameter is referred to by-identifier, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the integer variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, int parameterId, out int value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamInt((uint)channelInstance, (uint)parameterId, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterId));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an integer variable.
        /// The parameter is referred to by-name, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the integer variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, string parameterName, out int value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamNmInt((uint)channelInstance, parameterName, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterName));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as a string variable.
        /// The parameter is referred to by-identifier, and is preferably of the string or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the string variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, int parameterId, out string value)
        {
            int ret = 0;
            IntPtr text = IntPtr.Zero;
            try
            {
                StopWatch stopWatch = new StopWatch();
                stopWatch.Start();
                text = Marshal.AllocHGlobal((int)MultiCam.DefaultConstants.MaxValueLength + 1);
                ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamStr((uint)channelInstance, (uint)parameterId, text, (uint)MultiCam.DefaultConstants.MaxValueLength),
                                                         string.Format("Cannot get parameter '{0}'", parameterId));
                value = Marshal.PtrToStringAnsi(text);
                stopWatch.Stop();

            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as a string variable.
        /// The parameter is referred to by-name, and is preferably of the string or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the string type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the string variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, string parameterName, out string value)
        {
            int ret = 0;
            IntPtr text = IntPtr.Zero;
            try
            {
                StopWatch stopWatch = new StopWatch();
                stopWatch.Start();
                text = Marshal.AllocHGlobal((int)MultiCam.DefaultConstants.MaxValueLength + 1);
                ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamNmStr((uint)channelInstance, parameterName, text, (uint)MultiCam.DefaultConstants.MaxValueLength),
                                                         string.Format("Cannot get parameter '{0}'", parameterName));
                value = Marshal.PtrToStringAnsi(text);
                stopWatch.Stop();

            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an floating-point variable.
        /// The parameter is referred to by-identifier, and is preferably of the floating-point type.
        /// <para>
        /// If the MultiCam parameter is not of the floating-point type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the floating-point variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, int parameterId, out double value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamFloat((uint)channelInstance, (uint)parameterId, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterId));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an floating-point variable.
        /// The parameter is referred to by-name, and is preferably of the floating-point type.
        /// <para>
        /// If the MultiCam parameter is not of the floating-point type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the floating-point variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, string parameterName, out double value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamNmFloat((uint)channelInstance, parameterName, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterName));
            stopWatch.Stop();

            //Log.Write("Camera Time", string.Format("Get Param : {0}", stopWatch.Elapsed.TotalMilliseconds));
            Console.WriteLine("Camera Time : {0}", stopWatch.Elapsed.TotalMilliseconds);
            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an instance variable.
        /// The parameter is referred to by-identifier, and is of the instance type.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the instance variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, int parameterId, out uint value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamInst((uint)channelInstance, (uint)parameterId, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterId));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an instance variable.
        /// The parameter is referred to by-name, and is of the instance type.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the instance variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, string parameterName, out uint value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamNmInst((uint)channelInstance, parameterName, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterName));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as a string variable.
        /// The parameter is referred to by-identifier.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, int parameterId, out IntPtr value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamPtr((uint)channelInstance, (uint)parameterId, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterId));
            stopWatch.Stop();


            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as a string variable.
        /// The parameter is referred to by-name.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, string parameterName, out IntPtr value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamNmPtr((uint)channelInstance, parameterName, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterName));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an integer variable.
        /// The parameter is referred to by-identifier, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterId">
        /// Identifier of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the integer variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, int parameterId, out Int64 value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamInt64((uint)channelInstance, (uint)parameterId, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterId));
            stopWatch.Stop();

            return ret;
        }

        /// <summary>
        /// Returns the current value of a MultiCam parameter as an integer variable.
        /// The parameter is referred to by-name, and is preferably of the integer or enumerated type.
        /// <para>
        /// If the MultiCam parameter is not of the integer type, a type conversion is performed.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the instance of the parameter to read.
        /// </param>
        /// <param name="parameterName">
        /// Pointer to a string containing the name of the parameter to read.
        /// </param>
        /// <param name="value">
        /// Pointer to the integer variable that receives the parameter value.
        /// </param>
        /// <returns></returns>
        public static int GetParam(int channelInstance, string parameterName, out Int64 value)
        {
            int ret = 0;
            StopWatch stopWatch = new StopWatch();
            stopWatch.Start();
            ret = MultiCam.NotThrowOnMultiCamWarning(MultiCam.McGetParamNmInt64((uint)channelInstance, parameterName, out value),
                                                     string.Format("Cannot get parameter '{0}'", parameterName));
            stopWatch.Stop();

            return ret;
        }
        #endregion

        #region Signal handling Methods
        /// <summary>
        /// Registers the callback function to a channel or processor instance.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the channel or processor instance of the callback function to register.
        /// </param>
        /// <param name="callbackFunction">
        /// Pointer to the user-supplied callback function.
        /// To unregister a function, use NULL.
        /// </param>
        /// <param name="context">
        /// Argument to pass to the callback function.
        /// </param>
        /// <returns></returns>
        public static int RegisterCallback(int channelInstance, CallBack callbackFunction, int context)
        {
            int ret = 0;
            ret = MultiCam.ThrowOnMultiCamError(MultiCam.McRegisterCallback((uint)channelInstance, callbackFunction, (uint)context),
                                                "Cannot register callback");
            return ret;
        }

        /// <summary>
        /// Allows a thread to wait for a specific MultiCam signal occurrence
        /// <para>
        /// The main application thread can be forced to wait for a specific MultiCam signal.
        /// The signal should be enabled with the SignalEnable parameter.
        /// Only one signal can be waited for in a given thread at a given time.
        /// The dedicated MultiCam function to control the waiting mechanism is called McWaitSignal.
        /// If the expected signal does not occur within the specified timeout, the function returns MC_TIMEOUT.
        /// When waiting for the Surface Processing signal, it is the application responsibility to reset the SurfaceState parameter of the PROCESSING surface to FREE when done.
        /// Failure to do so will prevent the surface from being used by subsequent acquisitions.
        /// If the waiting signaling mechanism is used, the callback signaling mechanism cannot be used.
        /// However, the advanced signaling mechanism can be used as long as it is not used for the particular event involved in the waiting function.
        /// </para>
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the channel or processor instance that generates the signal.
        /// </param>
        /// <param name="signal">
        /// Identifier of the signal to be waited for.
        /// </param>
        /// <param name="timeout">
        /// Timeout duration expressed in milliseconds.
        /// To disable the timeout, use INFINITE.
        /// </param>
        /// <param name="info">
        /// Signal information structure pointer.
        /// The structure is updated with the signal information when the function completes successfully.
        /// </param>
        /// <returns></returns>
        public static int WaitSignal(int channelInstance, int signal, int timeout, out SIGNALINFO info)
        {
            int ret = 0;
            ret = MultiCam.ThrowOnMultiCamError(MultiCam.McWaitSignal((uint)channelInstance, signal, (uint)timeout, out info),
                                                "WaitSignal error");
            return ret;
        }

        /// <summary>
        /// Returns the information structure associated with the last occurrence of a MultiCam signal.
        /// </summary>
        /// <param name="channelInstance">
        /// Handle of the channel or processor instance that generates the signal.
        /// </param>
        /// <param name="signal">
        /// Identifier of the signal.
        /// </param>
        /// <param name="info">
        /// Signal information structure pointer.
        /// The structure is updated with the signal information when the function completes successfully.
        /// </param>
        /// <returns></returns>
        public static int GetSignalInfo(int channelInstance, int signal, out SIGNALINFO info)
        {
            int ret = 0;
            ret = MultiCam.ThrowOnMultiCamError(MultiCam.McGetSignalInfo((uint)channelInstance, signal, out info),
                                                "Cannot get signal information");
            return ret;
        }
        #endregion
        #endregion
    }
    #endregion
}