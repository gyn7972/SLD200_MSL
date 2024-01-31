using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.EureSys

#region EuresysCoaxlinkCamera_GenICam

{
    public static class GenICam
    {
        #region Define    
        #region SetParameter
        [Serializable]
        public enum SetInterfaceParameter
        {
            FirmwareStatus,
            CxpPoCxpHostConnectionSelector,
            CxpHostConnectionSelector,
            CxpHostConnectionTestMode,
            CxpRevisionSelector,
            CxpRevisionSupport,
            CxpUpConnectionSpeedConfig,
            CxpDiscoveryTimingSelector,
            CxpControlParameterSelector,
            LineSelector,
            LineMode,
            LineFilterStrength,
            LineSource,
            IOExtensionModuleLineSelector,
            IOExtensionModuleLineFormat,
            IOExtensionModuleLineMode,
            IOExtensionModuleLineToRepair,
            AddUserAction,
            UserActionsSchedulerReference,
            ScheduledUserActionsPoolStatus,
            LineInputToolSelector,
            LineInputToolSource,
            LineInputToolActivation,
            MultiplierDividerToolSelector,
            MultiplierDividerToolSource,
            MultiplierDividerToolOutputControl,
            QuadratureDecoderToolSources,
            QuadratureDecoderToolActivation,
            QuadratureDecoderToolForwardDirection,
            QuadratureDecoderToolOutputMode,
            QuadratureDecoderToolDirection,
            DividerToolSelector,
            DividerToolSource,
            DividerToolEnableControl,
            DelayToolSelector,
            DelayToolSource1,
            DelayToolSource2,
            DelayToolClockSource,
            EventInputToolSelector,
            EventInputToolSource,
            EventInputToolActivation,
            TemperatureSensorSelector,
            InterfaceEventSelector,
            InterfaceEventNotificationContext1,
            InterfaceEventNotificationContext2,
            InterfaceEventNotificationContext3,
            OemSafetyKeyVerification,
        }

        [Serializable]
        public enum SetDeviceParameter
        {
            CameraControlMethod,
            C2CLinkConfiguration,
            CycleTriggerSource,
            CxpLinkConfiguration,
            CxpLinkConfigurationOption,
            CxpHostConnectionBase,
            CxpTriggerLevel,
            CameraAndIlluminationControllerStream,
            StartOfSequenceTriggerSource,
            EndOfSequenceTriggerSource,
            EventSelector,
            EventNotificationContext1,
            EventNotificationContext2,
            EventNotificationContext3,
            ErrorSelector,
            ExposureTime,
            StrobeDelay,
            StrobeDuration
        }

        [Serializable]
        public enum SetRemoteParameter
        {
            TriggerMode,
            AcquisitionFrameRateEnable,
            AcquisitionFrameRate,
            TiggerMode,
            ExposureTime,
            ExposureMode,
            ReverseX,
            ReverseY,
            Width,
            Height,
        }


        [Serializable]
        public enum SetStreamParameter
        {
            StreamType,
            ErrorSelector,
            PixelFormat,
            PixelFormatNamespace,
            UnpackingMode,
            ImageScaling,
            LUTConfiguration,
            LUTSet,
            LUTEnable,
            StreamBufferHandlingMode,
            StreamAcquisitionModeSelector,
            StartOfScanTriggerSource,
            EndOfScanTriggerSource,
            DmaEngineOptimization,
            StripeArrangement,
            StatisticsSamplingSelector,
            LinearFilterControl,
            ThresholdControl,
            Scan3dExtractionMethod,
            Scan3dOutputMode,
            BayerMethod,
            FfcControl,
            FfcBypass,
        }

        [Serializable]
        public enum InterfaceEventSelector
        {
            [Description("Line Input Tool 1.")]
            LIN1,

            [Description("Line Input Tool 2.")]
            LIN2,

            [Description("Line Input Tool 3.")]
            LIN3,

            [Description("Line Input Tool 4.")]
            LIN4,

            [Description("Line Input Tool 5.")]
            LIN5,

            [Description("Line Input Tool 6.")]
            LIN6,

            [Description("Line Input Tool 7.")]
            LIN7,

            [Description("Line Input Tool 8.")]
            LIN8,

            [Description("Quadrature Decoder Tool 1.")]
            QDC1,

            [Description("Quadrature Decoder Tool 1 Changed Direction.")]
            QDC1Dir,

            [Description("Quadrature Decoder Tool 2.")]
            QDC2,

            [Description("Quadrature Decoder Tool 2 Changed Direction.")]
            QDC2Dir,

            [Description("Quadrature Decoder Tool 3.")]
            QDC3,

            [Description("Quadrature Decoder Tool 3 Changed Direction.")]
            QDC3Dir,

            [Description("Quadrature Decoder Tool 4.")]
            QDC4,

            [Description("Quadrature Decoder Tool 4 Changed Direction.")]
            QDC4Dir,

            [Description("Divider Tool 1.")]
            DIV1,

            [Description("Divider Tool 2.")]
            DIV2,

            [Description("Divider Tool 3.")]
            DIV3,

            [Description("Divider Tool 4.")]
            DIV4,

            [Description("Multiplier/Divider Tool 1.")]
            MDV1,

            [Description("Multiplier/Divider Tool 2.")]
            MDV2,

            [Description("Multiplier/Divider Tool 3.")]
            MDV3,

            [Description("Multiplier/Divider Tool 4.")]
            MDV4,

            [Description("Delay Tool 1 Output 1.")]
            DEL11,

            [Description("Delay Tool 1 Output 2.")]
            DEL12,

            [Description("Delay Tool 2 Output 1.")]
            DEL21,

            [Description("Delay Tool 2 Output 2.")]
            DEL22,

            [Description("Delay Tool 3 Output 1.")]
            DEL31,

            [Description("Delay Tool 3 Output 2.")]
            DEL32,

            [Description("Delay Tool 4 Output 1.")]
            DEL41,

            [Description("Delay Tool 4 Output 2.")]
            DEL42,

            [Description("User Event 1.")]
            UserEvent1,

            [Description("User Event 2.")]
            UserEvent2,

            [Description("User Event 3.")]
            UserEvent3,

            [Description("User Event 4.")]
            UserEvent4,

            [Description("Event Input Tool 1.")]
            EIN1,

            [Description("Event Input Tool 2.")]
            EIN2,

            [Description("Detected CRC error on CXP connector A.")]
            CrcErrorCxpA,

            [Description("Detected CRC error on CXP connector B.")]
            CrcErrorCxpB,

            [Description("Detected CRC error on CXP connector C.")]
            CrcErrorCxpC,

            [Description("Detected CRC error on CXP connector D.")]
            CrcErrorCxpD,

            [Description("Detected CRC error on CXP connector E.")]
            CrcErrorCxpE,

            [Description("Detected CRC error on CXP connector F.")]
            CrcErrorCxpF,

            [Description("Detected CRC error on CXP connector G.")]
            CrcErrorCxpG,

            [Description("Detected CRC error on CXP connector H.")]
            CrcErrorCxpH,

            [Description("Low level connection lock achieved on CXP connector A.")]
            ConnectionDetectedCxpA,

            [Description("Low level connection lock achieved on CXP connector B.")]
            ConnectionDetectedCxpB,

            [Description("Low level connection lock achieved on CXP connector C.")]
            ConnectionDetectedCxpC,

            [Description("Low level connection lock achieved on CXP connector D.")]
            ConnectionDetectedCxpD,

            [Description("Low level connection lock achieved on CXP connector E.")]
            ConnectionDetectedCxpE,

            [Description("Low level connection lock achieved on CXP connector F.")]
            ConnectionDetectedCxpF,

            [Description("Low level connection lock achieved on CXP connector G.")]
            ConnectionDetectedCxpG,

            [Description("Low level connection lock achieved on CXP connector H.")]
            ConnectionDetectedCxpH,

            [Description("Low level connection lock lost on CXP connector A.")]
            ConnectionUndetectedCxpA,

            [Description("Low level connection lock lost on CXP connector B.")]
            ConnectionUndetectedCxpB,

            [Description("Low level connection lock lost on CXP connector C.")]
            ConnectionUndetectedCxpC,

            [Description("Low level connection lock lost on CXP connector D.")]
            ConnectionUndetectedCxpD,

            [Description("Low level connection lock lost on CXP connector E.")]
            ConnectionUndetectedCxpE,

            [Description("Low level connection lock lost on CXP connector F.")]
            ConnectionUndetectedCxpF,

            [Description("Low level connection lock lost on CXP connector G.")]
            ConnectionUndetectedCxpG,

            [Description("Low level connection lock lost on CXP connector H.")]
            ConnectionUndetectedCxpH,

            [Description("CoaXPress link configuration done for Device 0.")]
            Device0Ready,

            [Description("CoaXPress link configuration done for Device 1.")]
            Device1Ready,

            [Description("CoaXPress link configuration done for Device 2.")]
            Device2Ready,

            [Description("CoaXPress link configuration done for Device 3.")]
            Device3Ready,

            [Description("CoaXPress link configuration done for Device 4.")]
            Device4Ready,

            [Description("CoaXPress link configuration done for Device 5.")]
            Device5Ready,

            [Description("CoaXPress link configuration done for Device 6.")]
            Device6Ready,

            [Description("CoaXPress link configuration done for Device 7.")]
            Device7Ready,

            [Description("Device 0 disconnected.")]
            Device0Lost,

            [Description("Device 1 disconnected.")]
            Device1Lost,

            [Description("Device 2 disconnected.")]
            Device2Lost,

            [Description("Device 3 disconnected.")]
            Device3Lost,

            [Description("Device 4 disconnected.")]
            Device4Lost,

            [Description("Device 5 disconnected.")]
            Device5Lost,

            [Description("Device 6 disconnected.")]
            Device6Lost,

            [Description("Device 7 disconnected.")]
            Device7Lost,
        }

        [Serializable]
        public enum InterfaceType
        {
            [Description("This enumeration value indicates CoaXPress transport layer technology.")]
            CXP,
        }

        [Serializable]
        public enum FirmwareStatus
        {
            [Description("OK.")]
            OK,

            [Description("Firmware is too recent.")]
            TooRecent,

            [Description("Firmware is too old.")]
            TooOld,

            [Description("Firmware is in recovery mode.")]
            RecoveryMode,

            [Description("PCIe gen 1 not supported.")]
            PCIeGen1NotSupported,
        }

        [Serializable]
        public enum CxpPoCxpHostConnectionSelector
        {
            [Description("All CoaXPress physical host connections.")]
            All,

            [Description("CoaXPress physical host connection A.")]
            A,

            [Description("CoaXPress physical host connection B.")]
            B,

            [Description("CoaXPress physical host connection C.")]
            C,

            [Description("CoaXPress physical host connection D.")]
            D,

            [Description("CoaXPress physical host connection E.")]
            E,

            [Description("CoaXPress physical host connection F.")]
            F,

            [Description("CoaXPress physical host connection G.")]
            G,

            [Description("CoaXPress physical host connection H.")]
            H,
        }

        [Serializable]
        public enum CxpPoCxpConfigurationStatusl
        {
            [Description("PoCXP is forced off.")]
            Off,

            [Description("Normal automatic PoCXP operation.")]
            Auto,

            [Description("PoCXP configuration is unknown.")]
            Unknown,

            [Description("PoCXP configuration is compound.")]
            Compound,
        }

        [Serializable]
        public enum CxpPoCxpStatus
        {
            [Description("PoCXP is off.")]
            Off,

            [Description("PoCXP is on.")]
            On,

            [Description("PoCXP has shut down because of an over-current trip.")]
            Tripped,

            [Description("PoCXP status is compound.")]
            Compound,
        }

        [Serializable]
        public enum CxpPoCxpPowerInputStatus
        {
            [Description("The 24V Power Converter is not OK.")]
            NotOK,

            [Description("The 24V Power Converter is OK.")]
            OK,
        }

        [Serializable]
        public enum CxpHostConnectionSelector
        {
            [Description("[CoaXPress physical host connection A.")]
            A,

            [Description("[CoaXPress physical host connection B.")]
            B,

            [Description("[CoaXPress physical host connection C.")]
            C,

            [Description("[CoaXPress physical host connection D.")]
            D,

            [Description("[CoaXPress physical host connection E.")]
            E,

            [Description("[CoaXPress physical host connection F.")]
            F,

            [Description("[CoaXPress physical host connection G.")]
            G,

            [Description("[CoaXPress physical host connection H.")]
            H,
        }

        [Serializable]
        public enum CxpConnectionState
        {
            [Description("Undetected.")]
            Undetected,

            [Description("Detected.")]
            Detected,
        }

        [Serializable]
        public enum CxpDownConnectionSpeed
        {
            [Description("1.250 Gbps.")]
            CXP1,

            [Description("2.500 Gbps.")]
            CXP2,

            [Description("3.125 Gbps.")]
            CXP3,

            [Description("5.000 Gbps.")]
            CXP5,

            [Description("6.250 Gbps.")]
            CXP6,

            [Description("10.000 Gbps.")]
            CXP10,

            [Description("12.500 Gbps.")]
            CXP12,
        }

        [Serializable]
        public enum CxpDeviceConnectionID
        {
            [Description("Master Connection of Camera W.")]
            CameraW_Master,

            [Description("Extension 1 of Camera W.")]
            CameraW_Extension1,

            [Description("Extension 2 of Camera W.")]
            CameraW_Extension2,

            [Description("Extension 3 of Camera W.")]
            CameraW_Extension3,

            [Description("Extension 4 of Camera W.")]
            CameraW_Extension4,

            [Description("Extension 5 of Camera W.")]
            CameraW_Extension5,

            [Description("Extension 6 of Camera W.")]
            CameraW_Extension6,

            [Description("Extension 7 of Camera W.")]
            CameraW_Extension7,

            [Description("Master Connection of Camera X.")]
            CameraX_Master,

            [Description("Extension 1 of Camera X.")]
            CameraX_Extension1,

            [Description("Extension 2 of Camera X.")]
            CameraX_Extension2,

            [Description("Extension 3 of Camera X.")]
            CameraX_Extension3,

            [Description("Extension 4 of Camera X.")]
            CameraX_Extension4,

            [Description("Extension 5 of Camera X.")]
            CameraX_Extension5,

            [Description("Extension 6 of Camera X.")]
            CameraX_Extension6,

            [Description("Extension 7 of Camera X.")]
            CameraX_Extension7,

            [Description("Master Connection of Camera Y.")]
            CameraY_Master,

            [Description("Extension 1 of Camera Y.")]
            CameraY_Extension1,

            [Description("Extension 2 of Camera Y.")]
            CameraY_Extension2,

            [Description("Extension 3 of Camera Y.")]
            CameraY_Extension3,

            [Description("Extension 4 of Camera Y.")]
            CameraY_Extension4,

            [Description("Extension 5 of Camera Y.")]
            CameraY_Extension5,

            [Description("Extension 6 of Camera Y.")]
            CameraY_Extension6,

            [Description("Extension 7 of Camera Y.")]
            CameraY_Extension7,

            [Description("Master Connection of Camera Z.")]
            CameraZ_Master,

            [Description("Extension 1 of Camera Z.")]
            CameraZ_Extension1,

            [Description("Extension 2 of Camera Z.")]
            CameraZ_Extension2,

            [Description("Extension 3 of Camera Z.")]
            CameraZ_Extension3,

            [Description("Extension 4 of Camera Z.")]
            CameraZ_Extension4,

            [Description("Extension 5 of Camera Z.")]
            CameraZ_Extension5,

            [Description("Extension 6 of Camera Z.")]
            CameraZ_Extension6,

            [Description("Extension 7 of Camera Z.")]
            CameraZ_Extension7,

            [Description("Master Connection of Camera S.")]
            CameraS_Master,

            [Description("Extension 1 of Camera S.")]
            CameraS_Extension1,

            [Description("Extension 2 of Camera S.")]
            CameraS_Extension2,

            [Description("Extension 3 of Camera S.")]
            CameraS_Extension3,

            [Description("Extension 4 of Camera S.")]
            CameraS_Extension4,

            [Description("Extension 5 of Camera S.")]
            CameraS_Extension5,

            [Description("Extension 6 of Camera S.")]
            CameraS_Extension6,

            [Description("Extension 7 of Camera S.")]
            CameraS_Extension7,

            [Description("Master Connection of Camera T.")]
            CameraT_Master,

            [Description("Extension 1 of Camera T.")]
            CameraT_Extension1,

            [Description("Extension 2 of Camera T.")]
            CameraT_Extension2,

            [Description("Extension 3 of Camera T.")]
            CameraT_Extension3,

            [Description("Extension 4 of Camera T.")]
            CameraT_Extension4,

            [Description("Extension 5 of Camera T.")]
            CameraT_Extension5,

            [Description("Extension 6 of Camera T.")]
            CameraT_Extension6,

            [Description("Extension 7 of Camera T.")]
            CameraT_Extension7,

            [Description("Master Connection of Camera U.")]
            CameraU_Master,

            [Description("Extension 1 of Camera U.")]
            CameraU_Extension1,

            [Description("Extension 2 of Camera U.")]
            CameraU_Extension2,

            [Description("Extension 3 of Camera U.")]
            CameraU_Extension3,

            [Description("Extension 4 of Camera U.")]
            CameraU_Extension4,

            [Description("Extension 5 of Camera U.")]
            CameraU_Extension5,

            [Description("Extension 6 of Camera U.")]
            CameraU_Extension6,

            [Description("Extension 7 of Camera U.")]
            CameraU_Extension7,

            [Description("Master Connection of Camera V.")]
            CameraV_Master,

            [Description("Extension 1 of Camera V.")]
            CameraV_Extension1,

            [Description("Extension 2 of Camera V.")]
            CameraV_Extension2,

            [Description("Extension 3 of Camera V.")]
            CameraV_Extension3,

            [Description("Extension 4 of Camera V.")]
            CameraV_Extension4,

            [Description("Extension 5 of Camera V.")]
            CameraV_Extension5,

            [Description("Extension 6 of Camera V.")]
            CameraV_Extension6,

            [Description("Extension 7 of Camera V.")]
            CameraV_Extension7,

            [Description("Sub-Link Extension 1.")]
            SubLink_Extension1,

            [Description("Sub-Link Extension 2.")]
            SubLink_Extension2,

            [Description("Sub-Link Extension 3.")]
            SubLink_Extension3,

            [Description("Sub-Link Extension 4.")]
            SubLink_Extension4,

            [Description("Sub-Link Extension 5.")]
            SubLink_Extension5,

            [Description("Sub-Link Extension 6.")]
            SubLink_Extension6,

            [Description("Sub-Link Extension 7.")]
            SubLink_Extension7,

            [Description("Not Ready.")]
            NotReady,
        }

        [Serializable]
        public enum CxpHostConnectionTestMode
        {
            [Description("The test mode is disabled on the selected Host connection.")]
            Off,

            [Description("The test mode is one on the selected Host connection.")]
            Mode1,
        }

        [Serializable]
        public enum CxpRevisionSelector
        {
            [Description("CoaXPress Standard Version 1.0.")]
            CXP_1_0,

            [Description("CoaXPress Standard Version 1.1.")]
            CXP_1_1,

            [Description("CoaXPress Standard Version 1.1.1.")]
            CXP_1_1_1,

            [Description("CoaXPress Standard Version 2.0.")]
            CXP_2_0,
        }

        [Serializable]
        public enum CxpRevisionSupport
        {
            [Description("Not supported.")]
            NotSupported,

            [Description("Partially supported.")]
            PartiallySupported,

            [Description("Supported")]
            Supported,
        }

        [Serializable]
        public enum CxpUpConnectionSpeedConfig
        {
            [Description("41.667 Mbps when downconnection speed is above CXP6, 20.833 Mbps otherwise.")]
            Auto,

            [Description("20.833 Mbps.")]
            Use_20Mbps,

            [Description("41.667 Mbps.")]
            Use_40Mbps,

            [Description("Disable upconnection.")]
            Off,
        }

        [Serializable]
        public enum CxpDiscoveryTimingSelector
        {
            [Description("Period of the discovery link resets on undetected connectors (default 1100).")]
            DiscoveryPeriod,

            [Description("Recovery time following an error on a connector before restarting the discovery (default 500).")]
            RecoveryTime,

            [Description("Maximum time for extensions to be discovered by the master (default 6000).")]
            ExtensionSetupMaxTime,

            [Description("Initial delay following a low-level lock before accessing device registers (default 1000).")]
            DiscoveryInitialDelay,

            [Description("Maximum time for link re-configuration (default 1100).")]
            LinkReconfigMaxTime,

            [Description("Delay to allow the device to complete link re-configuration (default 200).")]
            DeviceLinkReconfigDelay,
        }

        [Serializable]
        public enum CxpControlParameterSelector
        {
            [Description("Control transaction timeout (millisecond) (default 300).")]
            TransactionTimeout,

            [Description("Control transaction maximum resend counter (default 10).")]
            TransactionMaxResendCount,

            [Description("Control packet size max (bytes) (default 128).")]
            ControlPacketSizeMax,

            [Description("CoaXPress version 2.0 supported (boolean) (default 0).")]
            CxpVersion20Supported,

            [Description("Enable control command packets with tag (boolean) (default 1).")]
            EnableCommunicationWithTag,

            [Description("Force control command packets with tag (boolean) (default 0).")]
            ForceCommunicationWithTag,
        }

        [Serializable]
        [Description("Euresys Coaxlink Hardware Guide 참조")]
        public enum LineSelector
        {
            [Description("Differential input 1 of Internal I/O connector 1.")]
            DIN11,

            [Description("Differential input 2 of Internal I/O connector 1.")]
            DIN12,

            [Description("Differential input 1 of Internal I/O connector 2.")]
            DIN21,

            [Description("Differential input 2 of Internal I/O connector 2.")]
            DIN22,

            [Description("Isolated input 1 of Internal I/O connector 1.")]
            IIN11,

            [Description("Isolated input 2 of Internal I/O connector 1.")]
            IIN12,

            [Description("Isolated input 3 of Internal I/O connector 1.")]
            IIN13,

            [Description("Isolated input 4 of Internal I/O connector 1.")]
            IIN14,

            [Description("Isolated input 1 of Internal I/O connector 2.")]
            IIN21,

            [Description("Isolated input 2 of Internal I/O connector 2.")]
            IIN22,

            [Description("Isolated input 3 of Internal I/O connector 2.")]
            IIN23,

            [Description("Isolated input 4 of Internal I/O connector 2.")]
            IIN24,

            [Description("Isolated output 1 of Internal I/O connector 1.")]
            IOUT11,

            [Description("Isolated output 2 of Internal I/O connector 1.")]
            IOUT12,

            [Description("Isolated output 1 of Internal I/O connector 2.")]
            IOUT21,

            [Description("Isolated output 2 of Internal I/O connector 2.")]
            IOUT22,

            [Description("TTL input/output 1 of Internal I/O connector 1.")]
            TTLIO11,

            [Description("TTL input/output 2 of Internal I/O connector 1.")]
            TTLIO12,

            [Description("TTL input/output 1 of Internal I/O connector 2.")]
            TTLIO21,

            [Description("TTL input/output 2 of Internal I/O connector 2.")]
            TTLIO22,

            [Description("Input/output 1 of I/O extension module.")]
            MIO1,

            [Description("Input/output 2 of I/O extension module.")]
            MIO2,

            [Description("Input/output 3 of I/O extension module.")]
            MIO3,

            [Description("Input/output 4 of I/O extension module.")]
            MIO4,

            [Description("Input/output 5 of I/O extension module.")]
            MIO5,

            [Description("Input/output 6 of I/O extension module.")]
            MIO6,

            [Description("Input/output 7 of I/O extension module.")]
            MIO7,

            [Description("Input/output 8 of I/O extension module.")]
            MIO8,

            [Description("Input/output 9 of I/O extension module.")]
            MIO9,

            [Description("Input/output 10 of I/O extension module.")]
            MIO10,

            [Description("Input/output 11 of I/O extension module.")]
            MIO11,

            [Description("Input/output 12 of I/O extension module.")]
            MIO12,

            [Description("Input/output 13 of I/O extension module.")]
            MIO13,

            [Description("Input/output 14 of I/O extension module.")]
            MIO14,

            [Description("Input/output 15 of I/O extension module.")]
            MIO15,

            [Description("Input/output 16 of I/O extension module.")]
            MIO16,

            [Description("Input/output 17 of I/O extension module.")]
            MIO17,

            [Description("Input/output 18 of I/O extension module.")]
            MIO18,

            [Description("Input/output 19 of I/O extension module.")]
            MIO19,

            [Description("Input/output 20 of I/O extension module.")]
            MIO20,

            [Description("Input/output 21 of I/O extension module.")]
            MIO21,

            [Description("Input/output 22 of I/O extension module.")]
            MIO22,

            [Description("Input/output 23 of I/O extension module.")]
            MIO23,

            [Description("Input/output 24 of I/O extension module.")]
            MIO24,

            [Description("Input/output 25 of I/O extension module.")]
            MIO25,

            [Description("Input/output 26 of I/O extension module.")]
            MIO26,

            [Description("Input/output 27 of I/O extension module.")]
            MIO27,

            [Description("Input/output 28 of I/O extension module.")]
            MIO28,

            [Description("Input/output 29 of I/O extension module.")]
            MIO29,

            [Description("Input/output 30 of I/O extension module.")]
            MIO30,

            [Description("Input/output 31 of I/O extension module.")]
            MIO31,

            [Description("Input/output 32 of I/O extension module.")]
            MIO32,

            [Description("Input/output 33 of I/O extension module.")]
            MIO33,

            [Description("Input/output 34 of I/O extension module.")]
            MIO34,

            [Description("Input/output 35 of I/O extension module.")]
            MIO35,

            [Description("Input/output 36 of I/O extension module.")]
            MIO36,

            [Description("Input/output 37 of I/O extension module.")]
            MIO37,

            [Description("Input/output 38 of I/O extension module.")]
            MIO38,

            [Description("Input/output 39 of I/O extension module.")]
            MIO39,

            [Description("Input/output 40 of I/O extension module.")]
            MIO40,
        }

        [Serializable]
        public enum LineFormat
        {
            [Description("The I/O line is opto-coupled.")]
            ISO,

            [Description("The differential I/O line is RS-422 compliant.")]
            DIFF,

            [Description("The singled-ended I/O line is TTL compliant.")]
            TTL,
        }

        [Serializable]
        public enum LineMode
        {
            [Description("Input line.")]
            Input,

            [Description("Output line.")]
            Output,

            [Description("Open-collector driver capable of driving low only.")]
            DriveLow,

            [Description("Open-emitter driver capable of driving high only.")]
            DriveHigh,
        }

        [Serializable]
        public enum LineFilterStrength
        {
            //This feature is only available for input-capable GPIO lines.
            [Description("Lowest filter strength.")]
            Lowest,

            [Description("Low filter strength.")]
            Low,

            [Description("Medium filter strength.")]
            Medium,

            [Description("High filter strength.")]
            High,

            [Description("Highest filter strength.")]
            Highest,
        }

        [Serializable]
        public enum LineSource
        {
            [Description("Bit 0 of user output register.")]
            UserOutput0,

            [Description("Bit 1 of user output register.")]
            UserOutput1,

            [Description("Bit 2 of user output register.")]
            UserOutput2,

            [Description("Bit 3 of user output register.")]
            UserOutput3,

            [Description("Bit 4 of user output register.")]
            UserOutput4,

            [Description("Bit 5 of user output register.")]
            UserOutput5,

            [Description("Bit 6 of user output register.")]
            UserOutput6,

            [Description("Bit 7 of user output register.")]
            UserOutput7,

            [Description("Strobe output of device 0.")]
            Device0Strobe,

            [Description("Strobe output of device 1.")]
            Device1Strobe,

            [Description("Strobe output of device 2.")]
            Device2Strobe,

            [Description("Strobe output of device 3.")]
            Device3Strobe,

            [Description("Strobe output of device 4.")]
            Device4Strobe,

            [Description("Strobe output of device 5.")]
            Device5Strobe,

            [Description("Strobe output of device 6.")]
            Device6Strobe,

            [Description("Strobe output of device 7.")]
            Device7Strobe,

            [Description("Camera trigger output of device 0.")]
            Device0CameraTrigger,

            [Description("Camera trigger output of device 1.")]
            Device1CameraTrigger,

            [Description("Camera trigger output of device 2.")]
            Device2CameraTrigger,

            [Description("Camera trigger output of device 3.")]
            Device3CameraTrigger,

            [Description("Camera trigger output of device 4.")]
            Device4CameraTrigger,

            [Description("Camera trigger output of device 5.")]
            Device5CameraTrigger,

            [Description("Camera trigger output of device 6.")]
            Device6CameraTrigger,

            [Description("Camera trigger output of device 7.")]
            Device7CameraTrigger,

            [Description("Start of camera readout on stream 0 of device 0.")]
            Device0Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 0.")]
            Device0Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 0.")]
            Device0Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 0.")]
            Device0Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 0.")]
            Device0Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 0.")]
            Device0Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 0.")]
            Device0Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 0.")]
            Device0Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 1.")]
            Device1Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 1.")]
            Device1Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 1.")]
            Device1Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 1.")]
            Device1Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 1.")]
            Device1Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 1.")]
            Device1Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 1.")]
            Device1Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 1.")]
            Device1Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 2.")]
            Device2Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 2.")]
            Device2Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 2.")]
            Device2Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 2.")]
            Device2Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 2.")]
            Device2Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 2.")]
            Device2Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 2.")]
            Device2Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 2.")]
            Device2Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 3.")]
            Device3Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 3.")]
            Device3Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 3.")]
            Device3Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 3.")]
            Device3Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 3.")]
            Device3Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 3.")]
            Device3Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 3.")]
            Device3Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 3.")]
            Device3Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 4.")]
            Device4Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 4.")]
            Device4Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 4.")]
            Device4Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 4.")]
            Device4Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 4.")]
            Device4Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 4.")]
            Device4Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 4.")]
            Device4Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 4.")]
            Device4Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 5.")]
            Device5Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 5.")]
            Device5Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 5.")]
            Device5Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 5.")]
            Device5Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 5.")]
            Device5Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 5.")]
            Device5Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 5.")]
            Device5Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 5.")]
            Device5Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 6.")]
            Device6Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 6.")]
            Device6Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 6.")]
            Device6Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 6.")]
            Device6Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 6.")]
            Device6Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 6.")]
            Device6Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 6.")]
            Device6Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 6.")]
            Device6Stream7StartOfCameraReadout,

            [Description("Start of camera readout on stream 0 of device 7.")]
            Device7Stream0StartOfCameraReadout,

            [Description("Start of camera readout on stream 1 of device 7.")]
            Device7Stream1StartOfCameraReadout,

            [Description("Start of camera readout on stream 2 of device 7.")]
            Device7Stream2StartOfCameraReadout,

            [Description("Start of camera readout on stream 3 of device 7.")]
            Device7Stream3StartOfCameraReadout,

            [Description("Start of camera readout on stream 4 of device 7.")]
            Device7Stream4StartOfCameraReadout,

            [Description("Start of camera readout on stream 5 of device 7.")]
            Device7Stream5StartOfCameraReadout,

            [Description("Start of camera readout on stream 6 of device 7.")]
            Device7Stream6StartOfCameraReadout,

            [Description("Start of camera readout on stream 7 of device 7.")]
            Device7Stream7StartOfCameraReadout,

            [Description("Low.")]
            Low,

            [Description("high.")]
            High,
        }

        [Serializable]
        public enum IOExtensionModuleConfiguration
        {
            [Description("Enter configuration mode.")]
            Begin,

            [Description("Commit current configuration.")]
            Commit,

            [Description("Cancel current configuration.")]
            Abort,
        }

        [Serializable]
        public enum IOExtensionModuleLineSelector
        {
            [Description("Input/output 1 of I/O extension module.")]
            MIO1,

            [Description("Input/output 2 of I/O extension module.")]
            MIO2,

            [Description("Input/output 3 of I/O extension module.")]
            MIO3,

            [Description("Input/output 4 of I/O extension module.")]
            MIO4,

            [Description("Input/output 5 of I/O extension module.")]
            MIO5,

            [Description("Input/output 6 of I/O extension module.")]
            MIO6,

            [Description("Input/output 7 of I/O extension module.")]
            MIO7,

            [Description("Input/output 8 of I/O extension module.")]
            MIO8,

            [Description("Input/output 9 of I/O extension module.")]
            MIO9,

            [Description("Input/output 10 of I/O extension module.")]
            MIO10,

            [Description("Input/output 11 of I/O extension module.")]
            MIO11,

            [Description("Input/output 12 of I/O extension module.")]
            MIO12,

            [Description("Input/output 13 of I/O extension module.")]
            MIO13,

            [Description("Input/output 14 of I/O extension module.")]
            MIO14,

            [Description("Input/output 15 of I/O extension module.")]
            MIO15,

            [Description("Input/output 16 of I/O extension module.")]
            MIO16,

            [Description("Input/output 17 of I/O extension module.")]
            MIO17,

            [Description("Input/output 18 of I/O extension module.")]
            MIO18,

            [Description("Input/output 19 of I/O extension module.")]
            MIO19,

            [Description("Input/output 20 of I/O extension module.")]
            MIO20,

            [Description("Input/output 21 of I/O extension module.")]
            MIO21,

            [Description("Input/output 22 of I/O extension module.")]
            MIO22,

            [Description("Input/output 23 of I/O extension module.")]
            MIO23,

            [Description("Input/output 24 of I/O extension module.")]
            MIO24,

            [Description("Input/output 25 of I/O extension module.")]
            MIO25,

            [Description("Input/output 26 of I/O extension module.")]
            MIO26,

            [Description("Input/output 27 of I/O extension module.")]
            MIO27,

            [Description("Input/output 28 of I/O extension module.")]
            MIO28,

            [Description("Input/output 29 of I/O extension module.")]
            MIO29,

            [Description("Input/output 30 of I/O extension module.")]
            MIO30,

            [Description("Input/output 31 of I/O extension module.")]
            MIO31,

            [Description("Input/output 32 of I/O extension module.")]
            MIO32,

            [Description("Input/output 33 of I/O extension module.")]
            MIO33,

            [Description("Input/output 34 of I/O extension module.")]
            MIO34,

            [Description("Input/output 35 of I/O extension module.")]
            MIO35,

            [Description("Input/output 36 of I/O extension module.")]
            MIO36,

            [Description("Input/output 37 of I/O extension module.")]
            MIO37,

            [Description("Input/output 38 of I/O extension module.")]
            MIO38,

            [Description("Input/output 39 of I/O extension module.")]
            MIO39,

            [Description("Input/output 40 of I/O extension module.")]
            MIO40,


        }

        [Serializable]
        public enum IOExtensionModuleLineFormat
        {
            [Description("RS-422 compliant")]
            DIFF,

            [Description("TTL compliant.")]
            TTL,
        }

        [Serializable]
        public enum IOExtensionModuleLineMode
        {
            [Description("Input line.")]
            Input,

            [Description("Output line.")]
            Output,
        }

        [Serializable]
        public enum IOExtensionModuleLineToRepair
        {
            [Description("Input/output 1 of I/O extension module.")]
            MIO1,

            [Description("Input/output 2 of I/O extension module.")]
            MIO2,

            [Description("Input/output 3 of I/O extension module.")]
            MIO3,

            [Description("Input/output 4 of I/O extension module.")]
            MIO4,

            [Description("Input/output 5 of I/O extension module.")]
            MIO5,

            [Description("Input/output 6 of I/O extension module.")]
            MIO6,

            [Description("Input/output 7 of I/O extension module.")]
            MIO7,

            [Description("Input/output 8 of I/O extension module.")]
            MIO8,

            [Description("Input/output 9 of I/O extension module.")]
            MIO9,

            [Description("Input/output 10 of I/O extension module.")]
            MIO10,

            [Description("Input/output 11 of I/O extension module.")]
            MIO11,

            [Description("Input/output 12 of I/O extension module.")]
            MIO12,

            [Description("Input/output 13 of I/O extension module.")]
            MIO13,

            [Description("Input/output 14 of I/O extension module.")]
            MIO14,

            [Description("Input/output 15 of I/O extension module.")]
            MIO15,

            [Description("Input/output 16 of I/O extension module.")]
            MIO16,

            [Description("Input/output 17 of I/O extension module.")]
            MIO17,

            [Description("Input/output 18 of I/O extension module.")]
            MIO18,

            [Description("Input/output 19 of I/O extension module.")]
            MIO19,

            [Description("Input/output 20 of I/O extension module.")]
            MIO20,

            [Description("Input/output 21 of I/O extension module.")]
            MIO21,

            [Description("Input/output 22 of I/O extension module.")]
            MIO22,

            [Description("Input/output 23 of I/O extension module.")]
            MIO23,

            [Description("Input/output 24 of I/O extension module.")]
            MIO24,

            [Description("Input/output 25 of I/O extension module.")]
            MIO25,

            [Description("Input/output 26 of I/O extension module.")]
            MIO26,

            [Description("Input/output 27 of I/O extension module.")]
            MIO27,

            [Description("Input/output 28 of I/O extension module.")]
            MIO28,

            [Description("Input/output 29 of I/O extension module.")]
            MIO29,

            [Description("Input/output 30 of I/O extension module.")]
            MIO30,

            [Description("Input/output 31 of I/O extension module.")]
            MIO31,

            [Description("Input/output 32 of I/O extension module.")]
            MIO32,

            [Description("Input/output 33 of I/O extension module.")]
            MIO33,

            [Description("Input/output 34 of I/O extension module.")]
            MIO34,

            [Description("Input/output 35 of I/O extension module.")]
            MIO35,

            [Description("Input/output 36 of I/O extension module.")]
            MIO36,

            [Description("Input/output 37 of I/O extension module.")]
            MIO37,

            [Description("Input/output 38 of I/O extension module.")]
            MIO38,

            [Description("Input/output 39 of I/O extension module.")]
            MIO39,

            [Description("Input/output 40 of I/O extension module.")]
            MIO40,
        }

        [Serializable]
        public enum AddUserAction
        {
            [Description("User Event 1.")]
            UserEvent1,

            [Description("User Event 2.")]
            UserEvent2,

            [Description("User Event 3.")]
            UserEvent3,

            [Description("User Event 4.")]
            UserEvent4,

            [Description("Set User Output Register bit 0 high.")]
            UserOutput0_High,

            [Description("Set User Output Register bit 0 low.")]
            UserOutput0_Low,

            [Description("Toggle User Output Register bit 0.")]
            UserOutput0_Toggle,

            [Description("Set User Output Register bit 1 high.")]
            UserOutput1_High,

            [Description("Set User Output Register bit 1 low.")]
            UserOutput1_Low,

            [Description("Toggle User Output Register bit 1.")]
            UserOutput1_Toggle,

            [Description("Set User Output Register bit 2 high.")]
            UserOutput2_High,

            [Description("Set User Output Register bit 2 low.")]
            UserOutput2_Low,

            [Description("Toggle User Output Register bit 2.")]
            UserOutput2_Toggle,

            [Description("Set User Output Register bit 3 high.")]
            UserOutput3_High,

            [Description("Set User Output Register bit 3 low.")]
            UserOutput3_Low,

            [Description("Toggle User Output Register bit 3.")]
            UserOutput3_Toggle,

            [Description("Set User Output Register bit 4 high.")]
            UserOutput4_High,

            [Description("Set User Output Register bit 4 low.")]
            UserOutput4_Low,

            [Description("Toggle User Output Register bit 4.")]
            UserOutput4_Toggle,

            [Description("Set User Output Register bit 5 high.")]
            UserOutput5_High,

            [Description("Set User Output Register bit 5 low.")]
            UserOutput5_Low,

            [Description("Toggle User Output Register bit 5.")]
            UserOutput5_Toggle,

            [Description("Set User Output Register bit 6 high.")]
            UserOutput6_High,

            [Description("Set User Output Register bit 6 low.")]
            UserOutput6_Low,

            [Description("Toggle User Output Register bit 6.")]
            UserOutput6_Toggle,

            [Description("Set User Output Register bit 7 high.")]
            UserOutput7_High,

            [Description("Set User Output Register bit 7 low.")]
            UserOutput7_Low,

            [Description("Toggle User Output Register bit 7.")]
            UserOutput7_Toggle,
        }

        [Serializable]
        public enum UserActionsSchedulerReference
        {
            [Description("Coaxlink card internal time.")]
            InternalTime,

            [Description("Quadrature Decoder Tool 1 Position.")]
            QDC1Position,

            [Description("Quadrature Decoder Tool 2 Position.")]
            QDC2Position,

            [Description("Quadrature Decoder Tool 3 Position.")]
            QDC3Position,

            [Description("Quadrature Decoder Tool 4 Position.")]
            QDC4Position,
        }

        [Serializable]
        public enum ScheduledUserActionsPoolStatus
        {
            [Description("The pool of scheduled user actions is empty.")]
            Empty,

            [Description("The pool of scheduled user actions is partially filled.")]
            PartiallyFilled,

            [Description("The pool of scheduled user actions almost full.")]
            AlmostFull,
        }

        [Serializable]
        public enum LineInputToolSelector
        {
            [Description("Line Input Tool 1.")]
            LIN1,

            [Description("Line Input Tool 2.")]
            LIN2,

            [Description("Line Input Tool 3.")]
            LIN3,

            [Description("Line Input Tool 4.")]
            LIN4,

            [Description("Line Input Tool 5.")]
            LIN5,

            [Description("Line Input Tool 6.")]
            LIN6,

            [Description("Line Input Tool 7.")]
            LIN7,

            [Description("Line Input Tool 8.")]
            LIN8,
        }

        [Serializable]
        public enum LineInputToolSource
        {
            [Description("Differential input 1 of Internal I/O connector 1.")]
            DIN11,

            [Description("Differential input 2 of Internal I/O connector 1.")]
            DIN12,

            [Description("Differential input 1 of Internal I/O connector 2.")]
            DIN21,

            [Description("Differential input 2 of Internal I/O connector 2.")]
            DIN22,

            [Description("Isolated input 1 of Internal I/O connector 1.")]
            IIN11,

            [Description("Isolated input 2 of Internal I/O connector 1.")]
            IIN12,

            [Description("Isolated input 3 of Internal I/O connector 1.")]
            IIN13,

            [Description("Isolated input 4 of Internal I/O connector 1.")]
            IIN14,

            [Description("Isolated input 1 of Internal I/O connector 2.")]
            IIN21,

            [Description("Isolated input 2 of Internal I/O connector 2.")]
            IIN22,

            [Description("Isolated input 3 of Internal I/O connector 2.")]
            IIN23,

            [Description("Isolated input 4 of Internal I/O connector 2.")]
            IIN24,

            [Description("Isolated output 1 of Internal I/O connector 1.")]
            IOUT11,

            [Description("Isolated output 2 of Internal I/O connector 1.")]
            IOUT12,

            [Description("Isolated output 1 of Internal I/O connector 2.")]
            IOUT21,

            [Description("Isolated output 2 of Internal I/O connector 2.")]
            IOUT22,

            [Description("TTL input/output 1 of Internal I/O connector 1.")]
            TTLIO11,

            [Description("TTL input/output 2 of Internal I/O connector 1.")]
            TTLIO12,

            [Description("TTL input/output 1 of Internal I/O connector 2.")]
            TTLIO21,

            [Description("TTL input/output 2 of Internal I/O connector 2.")]
            TTLIO22,

            [Description("Input/output 1 of I/O extension module.")]
            MIO1,

            [Description("Input/output 2 of I/O extension module.")]
            MIO2,

            [Description("Input/output 3 of I/O extension module.")]
            MIO3,

            [Description("Input/output 4 of I/O extension module.")]
            MIO4,

            [Description("Input/output 5 of I/O extension module.")]
            MIO5,

            [Description("Input/output 6 of I/O extension module.")]
            MIO6,

            [Description("Input/output 7 of I/O extension module.")]
            MIO7,

            [Description("Input/output 8 of I/O extension module.")]
            MIO8,

            [Description("Input/output 9 of I/O extension module.")]
            MIO9,

            [Description("Input/output 10 of I/O extension module.")]
            MIO10,

            [Description("Input/output 11 of I/O extension module.")]
            MIO11,

            [Description("Input/output 12 of I/O extension module.")]
            MIO12,

            [Description("Input/output 13 of I/O extension module.")]
            MIO13,

            [Description("Input/output 14 of I/O extension module.")]
            MIO14,

            [Description("Input/output 15 of I/O extension module.")]
            MIO15,

            [Description("Input/output 16 of I/O extension module.")]
            MIO16,

            [Description("Input/output 17 of I/O extension module.")]
            MIO17,

            [Description("Input/output 18 of I/O extension module.")]
            MIO18,

            [Description("Input/output 19 of I/O extension module.")]
            MIO19,

            [Description("Input/output 20 of I/O extension module.")]
            MIO20,

            [Description("Input/output 21 of I/O extension module.")]
            MIO21,

            [Description("Input/output 22 of I/O extension module.")]
            MIO22,

            [Description("Input/output 23 of I/O extension module.")]
            MIO23,

            [Description("Input/output 24 of I/O extension module.")]
            MIO24,

            [Description("Input/output 25 of I/O extension module.")]
            MIO25,

            [Description("Input/output 26 of I/O extension module.")]
            MIO26,

            [Description("Input/output 27 of I/O extension module.")]
            MIO27,

            [Description("Input/output 28 of I/O extension module.")]
            MIO28,

            [Description("Input/output 29 of I/O extension module.")]
            MIO29,

            [Description("Input/output 30 of I/O extension module.")]
            MIO30,

            [Description("Input/output 31 of I/O extension module.")]
            MIO31,

            [Description("Input/output 32 of I/O extension module.")]
            MIO32,

            [Description("Input/output 33 of I/O extension module.")]
            MIO33,

            [Description("Input/output 34 of I/O extension module.")]
            MIO34,

            [Description("Input/output 35 of I/O extension module.")]
            MIO35,

            [Description("Input/output 36 of I/O extension module.")]
            MIO36,

            [Description("Input/output 37 of I/O extension module.")]
            MIO37,

            [Description("Input/output 38 of I/O extension module.")]
            MIO38,

            [Description("Input/output 39 of I/O extension module.")]
            MIO39,

            [Description("Input/output 40 of I/O extension module.")]
            MIO40,
        }

        [Serializable]
        public enum LineInputToolActivation
        {
            [Description("Activate the output on the rising edge only.")]
            RisingEdge,

            [Description("Activate the output on the falling edge only.")]
            FallingEdge,

            [Description("Activate the output on all edges.")]
            AllEdges,
        }

        [Serializable]
        public enum MultiplierDividerToolSelector
        {
            [Description("Multiplier/Divider Tool 1.")]
            MDV1,

            [Description("Multiplier/Divider Tool 2.")]
            MDV2,

            [Description("Multiplier/Divider Tool 3.")]
            MDV3,

            [Description("Multiplier/Divider Tool 4.")]
            MDV4,
        }

        [Serializable]
        public enum MultiplierDividerToolSource
        {
            [Description("No event stream.")]
            NONE,

            [Description("When an event occurs on Line Input Tool 1.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2.")]
            EIN2,

            [Description("When an event occurs on User Event 1.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4.")]
            UserEvent4,
        }

        [Serializable]
        public enum MultiplierDividerToolOutputControl
        {
            [Description("Output enabled.")]
            Enable,

            [Description("Output disabled.")]
            Disable,
        }

        [Serializable]
        public enum QuadratureDecoderToolSelector
        {
            [Description("Quadrature Decoder Tool 1.")]
            QDC1,
            [Description("Quadrature Decoder Tool 2.")]
            QDC2,
            [Description("Quadrature Decoder Tool 3.")]
            QDC3,
            [Description("Quadrature Decoder Tool 4.")]
            QDC4,
        }

        [Serializable]
        public enum QuadratureDecoderToolSources
        {
            [Description("Differential inputs 1 and 2 of Internal I/O connector 1.")]
            DIN11_DIN12,

            [Description("Differential inputs 1 and 2 of Internal I/O connector 2.")]
            DIN21_DIN22,

            [Description("Isolated inputs 1 and 2 of Internal I/O connector 1.")]
            IIN11_IIN12,

            [Description("Isolated inputs 3 and 4 of Internal I/O connector 1.")]
            IIN13_IIN14,

            [Description("Isolated inputs 1 and 2 of Internal I/O connector 2.")]
            IIN21_IIN22,

            [Description("Isolated inputs 3 and 4 of Internal I/O connector 2.")]
            IIN23_IIN24,

            [Description("TTL inputs 1 and 2 of Internal I/O connector 1.")]
            TTLIO11_TTLIO12,

            [Description("TTL inputs 1 and 2 of Internal I/O connector 2.")]
            TTLIO21_TTLIO22,

            [Description("Inputs 1 and 3 of I/O extension module.")]
            MIO1_MIO3,

            [Description("Inputs 5 and 7 of I/O extension module.")]
            MIO5_MIO7,

            [Description("Inputs 9 and 11 of I/O extension module.")]
            MIO9_MIO11,

            [Description("Inputs 13 and 15 of I/O extension module.")]
            MIO13_MIO15,

            [Description("Inputs 17 and 19 of I/O extension module.")]
            MIO17_MIO19,

            [Description("Inputs 21 and 23 of I/O extension module.")]
            MIO21_MIO23,

            [Description("Inputs 25 and 27 of I/O extension module.")]
            MIO25_MIO27,

            [Description("Inputs 29 and 31 of I/O extension module.")]
            MIO29_MIO31,

            [Description("Inputs 33 and 35 of I/O extension module.")]
            MIO33_MIO35,

            [Description("Inputs 37 and 39 of I/O extension module.")]
            MIO37_MIO39,
        }

        [Serializable]
        public enum QuadratureDecoderToolActivation
        {
            [Description("The event is activated on the rising edge of the A signal.")]
            RisingEdgeA,

            [Description("The event is activated on the falling edge of the A signal.")]
            FallingEdgeA,

            [Description("The event is activated on both edges of the A signal.")]
            AllEdgesA,

            [Description("The event is activated on both edges of all signals.")]
            AllEdgesAB,

            [Description("The event is not activated.")]
            None,
        }

        [Serializable]
        public enum QuadratureDecoderToolForwardDirection
        {
            [Description("A leads B.")]
            A_Leads_B,

            [Description("B_leads_A")]
            B_Leads_A,
        }

        [Serializable]
        public enum QuadratureDecoderToolOutputMode
        {
            [Description("All the quadrature decoder events are delivered.")]
            Unfiltered,

            [Description("Only the events corresponding to the forward motion are delivered.")]
            ForwardOnly,

            [Description("Only the events corresponding to the first pass in the forward direction are delivered.")]
            FirstPassForwardOnly,
        }

        [Serializable]
        public enum QuadratureDecoderToolDirection
        {
            [Description("Forward.")]
            Forward,

            [Description("Backward")]
            Backward,
        }

        [Serializable]
        public enum DividerToolSelector
        {
            [Description("Divider Tool 1.")]
            DIV1,

            [Description("Divider Tool 2.")]
            DIV2,

            [Description("Divider Tool 3.")]
            DIV3,

            [Description("Divider Tool 4.")]
            DIV4,
        }

        [Serializable]
        public enum DividerToolSource
        {
            [Description("No event stream.")]
            NONE,

            [Description("When an event occurs on Line Input Tool 1.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2.")]
            EIN2,

            [Description("When an event occurs on User Event 1.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4.")]
            UserEvent4,
        }

        [Serializable]
        public enum DividerToolEnableControl
        {
            [Description("Output enabled")]
            Enable,

            [Description("Output disabled")]
            Disable,
        }

        [Serializable]
        public enum DelayToolSelector
        {
            [Description("Delay Tool 1.")]
            DEL1,
            [Description("Delay Tool 2.")]
            DEL2,
            [Description("Delay Tool 3.")]
            DEL3,
            [Description("Delay Tool 4.")]
            DEL4,
        }

        [Serializable]
        public enum DelayToolSource1
        {
            [Description("No event stream.")]
            NONE,

            [Description("When an event occurs on Line Input Tool 1.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2.")]
            EIN2,

            [Description("When an event occurs on User Event 1.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4.")]
            UserEvent4,
        }

        [Serializable]
        public enum DelayToolSource2
        {
            [Description("No event stream.")]
            NONE,

            [Description("When an event occurs on Line Input Tool 1.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2.")]
            EIN2,

            [Description("When an event occurs on User Event 1.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4.")]
            UserEvent4,
        }

        [Serializable]
        public enum DelayToolClockSource
        {
            [Description("No event stream.")]
            NONE,

            [Description("Clock input 8 nanoseconds time base.")]
            TIME8NS,

            [Description("Clock input 200 nanoseconds time base.")]
            TIME200NS,

            [Description("Clock input 1 microsecond time base.")]
            TIME1US,

            [Description("When an event occurs on Line Input Tool 1.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4.")]
            QDC4,
        }

        [Serializable]
        public enum EventInputToolSelector
        {
            [Description("Event Input Tool 1.")]
            EIN1,

            [Description("Event Input Tool 2.")]
            EIN2,
        }

        [Serializable]
        public enum EventInputToolSource
        {
            [Description("CoaXPress physical host connection A.")]
            A,
        }

        [Serializable]
        public enum EventInputToolActivation
        {
            [Description("Receipt of start of scan signal.")]
            StartOfScan,

            [Description("Receipt of end of scan signal.")]
            EndOfScan,
        }

        [Serializable]
        public enum PCIeMaxLinkSpeed
        {
            [Description("Not available.")]
            NotAvailable,

            [Description("2.5 GT/s (PCIe Gen 1).")]
            PCIeLinkSpeed2500MTps,

            [Description("5.0 GT/s (PCIe Gen 2).")]
            PCIeLinkSpeed5000MTps,

            [Description("8.0 GT/s (PCIe Gen 3).")]
            PCIeLinkSpeed8000MTps,
        }

        [Serializable]
        public enum PCIeCurrentLinkSpeed
        {
            [Description("Not available.")]
            NotAvailable,

            [Description("2.5 GT/s (PCIe Gen 1).")]
            PCIeLinkSpeed2500MTps,

            [Description("5.0 GT/s (PCIe Gen 2).")]
            PCIeLinkSpeed5000MTps,

            [Description("8.0 GT/s (PCIe Gen 3).")]
            PCIeLinkSpeed8000MTps,
        }

        [Serializable]
        public enum PCIeMaximumLinkWidth
        {
            [Description("Not available.")]
            NotAvailable,

            [Description("1 Lane.")]
            x1,

            [Description("2 Lane.")]
            x2,

            [Description("4 Lane.")]
            x4,

            [Description("8 Lane.")]
            x8,

            [Description("12 Lane.")]
            x12,

            [Description("16 Lane.")]
            x16,

            [Description("32 Lane.")]
            x32,
        }

        [Serializable]
        public enum PCIeNegotiatedLinkWidth
        {
            [Description("Not available.")]
            NotAvailable,

            [Description("1 Lane.")]
            x1,

            [Description("2 Lane.")]
            x2,

            [Description("4 Lane.")]
            x4,

            [Description("8 Lane.")]
            x8,

            [Description("12 Lane.")]
            x12,

            [Description("16 Lane.")]
            x16,

            [Description("32 Lane.")]
            x32,
        }

        [Serializable]
        public enum FanStatus
        {
            [Description("Fan speed is OK.")]
            OK,

            [Description("Fan speed is not OK.")]
            NotOK,
        }

        [Serializable]
        public enum TemperatureSensorSelector
        {
            [Description("Grabber Temperature Sensor.")]
            Grabber,
        }

        [Serializable]
        public enum AuxiliaryPowerInput
        {
            [Description("There is no PEG-compliant power cable connected to the auxiliary power input.")]
            Unconnected,

            [Description("A PEG-compliant power cable is connected to the auxiliary power input.")]
            Connected,
        }

        [Serializable]
        public enum AuxiliaryPower12VInput
        {
            [Description("The 12V auxiliary power input is NOK.")]
            NotOK,

            [Description("The 12V auxiliary power input is OK.")]
            OK,
        }
        #endregion



        [Serializable]
        public enum InterfaceEventNotificationContext1
        {
            [Description("Event-specific context information.")]
            EventSpecific,

            [Description("Low 32-bit part of LineStatusAll.")]
            LineStatusAll,

            [Description("High 32-bit part of LineStatusAll.")]
            LineStatusAllHi,

            [Description("Position of Quadrature Decoder Tool 1.")]
            QDC1Position,

            [Description("Position of Quadrature Decoder Tool 2.")]
            QDC2Position,

            [Description("Position of Quadrature Decoder Tool 3.")]
            QDC3Position,

            [Description("Position of Quadrature Decoder Tool 4.")]
            QDC4Position,

            [Description("Number of LIN1 events.")]
            LIN1EventCount,

            [Description("Number of LIN2 events.")]
            LIN2EventCount,

            [Description("Number of LIN3 events.")]
            LIN3EventCount,

            [Description("Number of LIN4 events.")]
            LIN4EventCount,

            [Description("Number of LIN5 events.")]
            LIN5EventCount,

            [Description("Number of LIN6 events.")]
            LIN6EventCount,

            [Description("Number of LIN7 events.")]
            LIN7EventCount,

            [Description("Number of LIN8 events.")]
            LIN8EventCount,

            [Description("Number of QDC1 events.")]
            QDC1EventCount,

            [Description("Number of QDC1Dir events.")]
            QDC1DirEventCount,

            [Description("Number of QDC2 events.")]
            QDC2EventCount,

            [Description("Number of QDC2Dir events.")]
            QDC2DirEventCount,

            [Description("Number of QDC3 events.")]
            QDC3EventCount,

            [Description("Number of QDC3Dir events.")]
            QDC3DirEventCount,

            [Description("Number of QDC4 events.")]
            QDC4EventCount,

            [Description("Number of QDC4Dir events.")]
            QDC4DirEventCount,

            [Description("Number of DIV1 events.")]
            DIV1EventCount,

            [Description("Number of DIV2 events.")]
            DIV2EventCount,

            [Description("Number of DIV3 events.")]
            DIV3EventCount,

            [Description("Number of DIV4 events.")]
            DIV4EventCount,

            [Description("Number of MDV1 events.")]
            MDV1EventCount,

            [Description("Number of MDV2 events.")]
            MDV2EventCount,

            [Description("Number of MDV3 events.")]
            MDV3EventCount,

            [Description("Number of MDV4 events.")]
            MDV4EventCount,

            [Description("Number of DEL11 events.")]
            DEL11EventCount,

            [Description("Number of DEL12 events.")]
            DEL12EventCount,

            [Description("Number of DEL21 events.")]
            DEL21EventCount,

            [Description("Number of DEL22 events.")]
            DEL22EventCount,

            [Description("Number of DEL31 events.")]
            DEL31EventCount,

            [Description("Number of DEL32 events.")]
            DEL32EventCount,

            [Description("Number of DEL41 events.")]
            DEL41EventCount,

            [Description("Number of DEL42 events.")]
            DEL42EventCount,

            [Description("Number of UserEvent1 events.")]
            UserEvent1EventCount,

            [Description("Number of UserEvent2 events.")]
            UserEvent2EventCount,

            [Description("Number of UserEvent3 events.")]
            UserEvent3EventCount,

            [Description("Number of UserEvent4 events.")]
            UserEvent4EventCount,

            [Description("Number of EIN1 events.")]
            EIN1EventCount,

            [Description("Number of EIN2 events.")]
            EIN2EventCount,

            [Description("Number of CrcErrorCxpA events.")]
            CrcErrorCxpAEventCount,

            [Description("Number of CrcErrorCxpB events.")]
            CrcErrorCxpBEventCount,

            [Description("Number of CrcErrorCxpC events.")]
            CrcErrorCxpCEventCount,

            [Description("Number of CrcErrorCxpD events.")]
            CrcErrorCxpDEventCount,

            [Description("Number of CrcErrorCxpE events.")]
            CrcErrorCxpEEventCount,

            [Description("Number of CrcErrorCxpF events.")]
            CrcErrorCxpFEventCount,

            [Description("Number of CrcErrorCxpG events.")]
            CrcErrorCxpGEventCount,

            [Description("Number of CrcErrorCxpH events.")]
            CrcErrorCxpHEventCount,

            [Description("Number of ConnectionDetectedCxpA events.")]
            ConnectionDetectedCxpAEventCount,

            [Description("Number of ConnectionDetectedCxpB events.")]
            ConnectionDetectedCxpBEventCount,

            [Description("Number of ConnectionDetectedCxpC events.")]
            ConnectionDetectedCxpCEventCount,

            [Description("Number of ConnectionDetectedCxpD events.")]
            ConnectionDetectedCxpDEventCount,

            [Description("Number of ConnectionDetectedCxpE events.")]
            ConnectionDetectedCxpEEventCount,

            [Description("Number of ConnectionDetectedCxpF events.")]
            ConnectionDetectedCxpFEventCount,

            [Description("Number of ConnectionDetectedCxpG events.")]
            ConnectionDetectedCxpGEventCount,

            [Description("Number of ConnectionDetectedCxpH events.")]
            ConnectionDetectedCxpHEventCount,

            [Description("Number of ConnectionUndetectedCxpA events.")]
            ConnectionUndetectedCxpAEventCount,

            [Description("Number of ConnectionUndetectedCxpB events.")]
            ConnectionUndetectedCxpBEventCount,

            [Description("Number of ConnectionUndetectedCxpC events.")]
            ConnectionUndetectedCxpCEventCount,

            [Description("Number of ConnectionUndetectedCxpD events.")]
            ConnectionUndetectedCxpDEventCount,

            [Description("Number of ConnectionUndetectedCxpE events.")]
            ConnectionUndetectedCxpEEventCount,

            [Description("Number of ConnectionUndetectedCxpF events.")]
            ConnectionUndetectedCxpFEventCount,

            [Description("Number of ConnectionUndetectedCxpG events.")]
            ConnectionUndetectedCxpGEventCount,

            [Description("Number of ConnectionUndetectedCxpH events.")]
            ConnectionUndetectedCxpHEventCount,

            [Description("Number of Device0Ready events.")]
            Device0ReadyEventCount,

            [Description("Number of Device1Ready events.")]
            Device1ReadyEventCount,

            [Description("Number of Device2Ready events.")]
            Device2ReadyEventCount,

            [Description("Number of Device3Ready events.")]
            Device3ReadyEventCount,

            [Description("Number of Device4Ready events.")]
            Device4ReadyEventCount,

            [Description("Number of Device5Ready events.")]
            Device5ReadyEventCount,

            [Description("Number of Device6Ready events.")]
            Device6ReadyEventCount,

            [Description("Number of Device7Ready events.")]
            Device7ReadyEventCount,

            [Description("Number of Device0Lost events.")]
            Device0LostEventCount,

            [Description("Number of Device1Lost events.")]
            Device1LostEventCount,

            [Description("Number of Device2Lost events.")]
            Device2LostEventCount,

            [Description("Number of Device3Lost events.")]
            Device3LostEventCount,

            [Description("Number of Device4Lost events.")]
            Device4LostEventCount,

            [Description("Number of Device5Lost events.")]
            Device5LostEventCount,

            [Description("Number of Device6Lost events.")]
            Device6LostEventCount,

            [Description("Number of Device7Lost events.")]
            Device7LostEventCount,
        }

        [Serializable]
        public enum InterfaceEventNotificationContext2
        {
            [Description("Event-specific context information.")]
            EventSpecific,

            [Description("Low 32-bit part of LineStatusAll.")]
            LineStatusAll,

            [Description("High 32-bit part of LineStatusAll.")]
            LineStatusAllHi,

            [Description("Position of Quadrature Decoder Tool 1.")]
            QDC1Position,

            [Description("Position of Quadrature Decoder Tool 2.")]
            QDC2Position,

            [Description("Position of Quadrature Decoder Tool 3.")]
            QDC3Position,

            [Description("Position of Quadrature Decoder Tool 4.")]
            QDC4Position,

            [Description("Number of LIN1 events.")]
            LIN1EventCount,

            [Description("Number of LIN2 events.")]
            LIN2EventCount,

            [Description("Number of LIN3 events.")]
            LIN3EventCount,

            [Description("Number of LIN4 events.")]
            LIN4EventCount,

            [Description("Number of LIN5 events.")]
            LIN5EventCount,

            [Description("Number of LIN6 events.")]
            LIN6EventCount,

            [Description("Number of LIN7 events.")]
            LIN7EventCount,

            [Description("Number of LIN8 events.")]
            LIN8EventCount,

            [Description("Number of QDC1 events.")]
            QDC1EventCount,

            [Description("Number of QDC1Dir events.")]
            QDC1DirEventCount,

            [Description("Number of QDC2 events.")]
            QDC2EventCount,

            [Description("Number of QDC2Dir events.")]
            QDC2DirEventCount,

            [Description("Number of QDC3 events.")]
            QDC3EventCount,

            [Description("Number of QDC3Dir events.")]
            QDC3DirEventCount,

            [Description("Number of QDC4 events.")]
            QDC4EventCount,

            [Description("Number of QDC4Dir events.")]
            QDC4DirEventCount,

            [Description("Number of DIV1 events.")]
            DIV1EventCount,

            [Description("Number of DIV2 events.")]
            DIV2EventCount,

            [Description("Number of DIV3 events.")]
            DIV3EventCount,

            [Description("Number of DIV4 events.")]
            DIV4EventCount,

            [Description("Number of MDV1 events.")]
            MDV1EventCount,

            [Description("Number of MDV2 events.")]
            MDV2EventCount,

            [Description("Number of MDV3 events.")]
            MDV3EventCount,

            [Description("Number of MDV4 events.")]
            MDV4EventCount,

            [Description("Number of DEL11 events.")]
            DEL11EventCount,

            [Description("Number of DEL12 events.")]
            DEL12EventCount,

            [Description("Number of DEL21 events.")]
            DEL21EventCount,

            [Description("Number of DEL22 events.")]
            DEL22EventCount,

            [Description("Number of DEL31 events.")]
            DEL31EventCount,

            [Description("Number of DEL32 events.")]
            DEL32EventCount,

            [Description("Number of DEL41 events.")]
            DEL41EventCount,

            [Description("Number of DEL42 events.")]
            DEL42EventCount,

            [Description("Number of UserEvent1 events.")]
            UserEvent1EventCount,

            [Description("Number of UserEvent2 events.")]
            UserEvent2EventCount,

            [Description("Number of UserEvent3 events.")]
            UserEvent3EventCount,

            [Description("Number of UserEvent4 events.")]
            UserEvent4EventCount,

            [Description("Number of EIN1 events.")]
            EIN1EventCount,

            [Description("Number of EIN2 events.")]
            EIN2EventCount,

            [Description("Number of CrcErrorCxpA events.")]
            CrcErrorCxpAEventCount,

            [Description("Number of CrcErrorCxpB events.")]
            CrcErrorCxpBEventCount,

            [Description("Number of CrcErrorCxpC events.")]
            CrcErrorCxpCEventCount,

            [Description("Number of CrcErrorCxpD events.")]
            CrcErrorCxpDEventCount,

            [Description("Number of CrcErrorCxpE events.")]
            CrcErrorCxpEEventCount,

            [Description("Number of CrcErrorCxpF events.")]
            CrcErrorCxpFEventCount,

            [Description("Number of CrcErrorCxpG events.")]
            CrcErrorCxpGEventCount,

            [Description("Number of CrcErrorCxpH events.")]
            CrcErrorCxpHEventCount,

            [Description("Number of ConnectionDetectedCxpA events.")]
            ConnectionDetectedCxpAEventCount,

            [Description("Number of ConnectionDetectedCxpB events.")]
            ConnectionDetectedCxpBEventCount,

            [Description("Number of ConnectionDetectedCxpC events.")]
            ConnectionDetectedCxpCEventCount,

            [Description("Number of ConnectionDetectedCxpD events.")]
            ConnectionDetectedCxpDEventCount,

            [Description("Number of ConnectionDetectedCxpE events.")]
            ConnectionDetectedCxpEEventCount,

            [Description("Number of ConnectionDetectedCxpF events.")]
            ConnectionDetectedCxpFEventCount,

            [Description("Number of ConnectionDetectedCxpG events.")]
            ConnectionDetectedCxpGEventCount,

            [Description("Number of ConnectionDetectedCxpH events.")]
            ConnectionDetectedCxpHEventCount,

            [Description("Number of ConnectionUndetectedCxpA events.")]
            ConnectionUndetectedCxpAEventCount,

            [Description("Number of ConnectionUndetectedCxpB events.")]
            ConnectionUndetectedCxpBEventCount,

            [Description("Number of ConnectionUndetectedCxpC events.")]
            ConnectionUndetectedCxpCEventCount,

            [Description("Number of ConnectionUndetectedCxpD events.")]
            ConnectionUndetectedCxpDEventCount,

            [Description("Number of ConnectionUndetectedCxpE events.")]
            ConnectionUndetectedCxpEEventCount,

            [Description("Number of ConnectionUndetectedCxpF events.")]
            ConnectionUndetectedCxpFEventCount,

            [Description("Number of ConnectionUndetectedCxpG events.")]
            ConnectionUndetectedCxpGEventCount,

            [Description("Number of ConnectionUndetectedCxpH events.")]
            ConnectionUndetectedCxpHEventCount,

            [Description("Number of Device0Ready events.")]
            Device0ReadyEventCount,

            [Description("Number of Device1Ready events.")]
            Device1ReadyEventCount,

            [Description("Number of Device2Ready events.")]
            Device2ReadyEventCount,

            [Description("Number of Device3Ready events.")]
            Device3ReadyEventCount,

            [Description("Number of Device4Ready events.")]
            Device4ReadyEventCount,

            [Description("Number of Device5Ready events.")]
            Device5ReadyEventCount,

            [Description("Number of Device6Ready events.")]
            Device6ReadyEventCount,

            [Description("Number of Device7Ready events.")]
            Device7ReadyEventCount,

            [Description("Number of Device0Lost events.")]
            Device0LostEventCount,

            [Description("Number of Device1Lost events.")]
            Device1LostEventCount,

            [Description("Number of Device2Lost events.")]
            Device2LostEventCount,

            [Description("Number of Device3Lost events.")]
            Device3LostEventCount,

            [Description("Number of Device4Lost events.")]
            Device4LostEventCount,

            [Description("Number of Device5Lost events.")]
            Device5LostEventCount,

            [Description("Number of Device6Lost events.")]
            Device6LostEventCount,

            [Description("Number of Device7Lost events.")]
            Device7LostEventCount,
        }

        [Serializable]
        public enum InterfaceEventNotificationContext3
        {
            [Description("Event-specific context information.")]
            EventSpecific,

            [Description("Low 32-bit part of LineStatusAll.")]
            LineStatusAll,

            [Description("High 32-bit part of LineStatusAll.")]
            LineStatusAllHi,

            [Description("Position of Quadrature Decoder Tool 1.")]
            QDC1Position,

            [Description("Position of Quadrature Decoder Tool 2.")]
            QDC2Position,

            [Description("Position of Quadrature Decoder Tool 3.")]
            QDC3Position,

            [Description("Position of Quadrature Decoder Tool 4.")]
            QDC4Position,

            [Description("Number of LIN1 events.")]
            LIN1EventCount,

            [Description("Number of LIN2 events.")]
            LIN2EventCount,

            [Description("Number of LIN3 events.")]
            LIN3EventCount,

            [Description("Number of LIN4 events.")]
            LIN4EventCount,

            [Description("Number of LIN5 events.")]
            LIN5EventCount,

            [Description("Number of LIN6 events.")]
            LIN6EventCount,

            [Description("Number of LIN7 events.")]
            LIN7EventCount,

            [Description("Number of LIN8 events.")]
            LIN8EventCount,

            [Description("Number of QDC1 events.")]
            QDC1EventCount,

            [Description("Number of QDC1Dir events.")]
            QDC1DirEventCount,

            [Description("Number of QDC2 events.")]
            QDC2EventCount,

            [Description("Number of QDC2Dir events.")]
            QDC2DirEventCount,

            [Description("Number of QDC3 events.")]
            QDC3EventCount,

            [Description("Number of QDC3Dir events.")]
            QDC3DirEventCount,

            [Description("Number of QDC4 events.")]
            QDC4EventCount,

            [Description("Number of QDC4Dir events.")]
            QDC4DirEventCount,

            [Description("Number of DIV1 events.")]
            DIV1EventCount,

            [Description("Number of DIV2 events.")]
            DIV2EventCount,

            [Description("Number of DIV3 events.")]
            DIV3EventCount,

            [Description("Number of DIV4 events.")]
            DIV4EventCount,

            [Description("Number of MDV1 events.")]
            MDV1EventCount,

            [Description("Number of MDV2 events.")]
            MDV2EventCount,

            [Description("Number of MDV3 events.")]
            MDV3EventCount,

            [Description("Number of MDV4 events.")]
            MDV4EventCount,

            [Description("Number of DEL11 events.")]
            DEL11EventCount,

            [Description("Number of DEL12 events.")]
            DEL12EventCount,

            [Description("Number of DEL21 events.")]
            DEL21EventCount,

            [Description("Number of DEL22 events.")]
            DEL22EventCount,

            [Description("Number of DEL31 events.")]
            DEL31EventCount,

            [Description("Number of DEL32 events.")]
            DEL32EventCount,

            [Description("Number of DEL41 events.")]
            DEL41EventCount,

            [Description("Number of DEL42 events.")]
            DEL42EventCount,

            [Description("Number of UserEvent1 events.")]
            UserEvent1EventCount,

            [Description("Number of UserEvent2 events.")]
            UserEvent2EventCount,

            [Description("Number of UserEvent3 events.")]
            UserEvent3EventCount,

            [Description("Number of UserEvent4 events.")]
            UserEvent4EventCount,

            [Description("Number of EIN1 events.")]
            EIN1EventCount,

            [Description("Number of EIN2 events.")]
            EIN2EventCount,

            [Description("Number of CrcErrorCxpA events.")]
            CrcErrorCxpAEventCount,

            [Description("Number of CrcErrorCxpB events.")]
            CrcErrorCxpBEventCount,

            [Description("Number of CrcErrorCxpC events.")]
            CrcErrorCxpCEventCount,

            [Description("Number of CrcErrorCxpD events.")]
            CrcErrorCxpDEventCount,

            [Description("Number of CrcErrorCxpE events.")]
            CrcErrorCxpEEventCount,

            [Description("Number of CrcErrorCxpF events.")]
            CrcErrorCxpFEventCount,

            [Description("Number of CrcErrorCxpG events.")]
            CrcErrorCxpGEventCount,

            [Description("Number of CrcErrorCxpH events.")]
            CrcErrorCxpHEventCount,

            [Description("Number of ConnectionDetectedCxpA events.")]
            ConnectionDetectedCxpAEventCount,

            [Description("Number of ConnectionDetectedCxpB events.")]
            ConnectionDetectedCxpBEventCount,

            [Description("Number of ConnectionDetectedCxpC events.")]
            ConnectionDetectedCxpCEventCount,

            [Description("Number of ConnectionDetectedCxpD events.")]
            ConnectionDetectedCxpDEventCount,

            [Description("Number of ConnectionDetectedCxpE events.")]
            ConnectionDetectedCxpEEventCount,

            [Description("Number of ConnectionDetectedCxpF events.")]
            ConnectionDetectedCxpFEventCount,

            [Description("Number of ConnectionDetectedCxpG events.")]
            ConnectionDetectedCxpGEventCount,

            [Description("Number of ConnectionDetectedCxpH events.")]
            ConnectionDetectedCxpHEventCount,

            [Description("Number of ConnectionUndetectedCxpA events.")]
            ConnectionUndetectedCxpAEventCount,

            [Description("Number of ConnectionUndetectedCxpB events.")]
            ConnectionUndetectedCxpBEventCount,

            [Description("Number of ConnectionUndetectedCxpC events.")]
            ConnectionUndetectedCxpCEventCount,

            [Description("Number of ConnectionUndetectedCxpD events.")]
            ConnectionUndetectedCxpDEventCount,

            [Description("Number of ConnectionUndetectedCxpE events.")]
            ConnectionUndetectedCxpEEventCount,

            [Description("Number of ConnectionUndetectedCxpF events.")]
            ConnectionUndetectedCxpFEventCount,

            [Description("Number of ConnectionUndetectedCxpG events.")]
            ConnectionUndetectedCxpGEventCount,

            [Description("Number of ConnectionUndetectedCxpH events.")]
            ConnectionUndetectedCxpHEventCount,

            [Description("Number of Device0Ready events.")]
            Device0ReadyEventCount,

            [Description("Number of Device1Ready events.")]
            Device1ReadyEventCount,

            [Description("Number of Device2Ready events.")]
            Device2ReadyEventCount,

            [Description("Number of Device3Ready events.")]
            Device3ReadyEventCount,

            [Description("Number of Device4Ready events.")]
            Device4ReadyEventCount,

            [Description("Number of Device5Ready events.")]
            Device5ReadyEventCount,

            [Description("Number of Device6Ready events.")]
            Device6ReadyEventCount,

            [Description("Number of Device7Ready events.")]
            Device7ReadyEventCount,

            [Description("Number of Device0Lost events.")]
            Device0LostEventCount,

            [Description("Number of Device1Lost events.")]
            Device1LostEventCount,

            [Description("Number of Device2Lost events.")]
            Device2LostEventCount,

            [Description("Number of Device3Lost events.")]
            Device3LostEventCount,

            [Description("Number of Device4Lost events.")]
            Device4LostEventCount,

            [Description("Number of Device5Lost events.")]
            Device5LostEventCount,

            [Description("Number of Device6Lost events.")]
            Device6LostEventCount,

            [Description("Number of Device7Lost events.")]
            Device7LostEventCount,
        }

        [Serializable]
        public enum OemSafetyKeyVerification
        {
            [Description("Only the key written to ProgramOemSafetyKey can be used to verify the OEM safety key.")]
            ProgrammingKey,

            [Description("Only the key read from EncryptedOemSafetyKey can be used to verify the OEM safety key (recommended).")]
            EncryptedKey,

            [Description("Both the key written to ProgramOemSafetyKey and the key read from EncryptedOemSafetyKey can be used to verify the OEM safety key.")]
            ProgrammingKeyOrEncryptedKey,
        }

        [Serializable]
        public enum CameraControlMethodType
        {
            //The NC and the EXTERNAL camera control methods doesn't use the CIC.

            [Description("Not Controlled")]
            NC,

            [Description("Grabber-controlled cycle start, Camera-controlled exposure time.")]
            RC,

            [Description("Grabber-controlled cycle start and exposure time")]
            RG,

            [Description("Externally-controlled cycle start and exposure time.")]
            EXTERNAL,
        }

        [Serializable]
        public enum C2CLinkConfiguration
        {
            [Description("Disconnected from the C2C-Link.")]
            Disconnected,
            [Description("Connected to the C2C-Link as the C2C-Link Master Device.")]
            Master,
            [Description("Connected to the C2C-Link as aC2C-Link Slave Device.")]
            Slave,
        }

        [Serializable]
        public enum CycleTriggerSourceType
        {
            [Description(" Immediately after the start of the sequence and then repeatedly every CycleMinimumPeriod period.")]
            Immediate,

            [Description("On execution of the StartCycle command.")]
            StartCycle,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StartCycle command.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2 or on execution of the StartCycle command.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3 or on execution of the StartCycle command.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4 or on execution of the StartCycle command.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5 or on execution of the StartCycle command.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6 or on execution of the StartCycle command.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7 or on execution of the StartCycle command.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8 or on execution of the StartCycle command.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1 or on execution of the StartCycle command.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2 or on execution of the StartCycle command.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3 or on execution of the StartCycle command.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4 or on execution of the StartCycle command.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1 or on execution of the StartCycle command.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2 or on execution of the StartCycle command.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3 or on execution of the StartCycle command.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4 or on execution of the StartCycle command.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1 or on execution of the StartCycle command.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2 or on execution of the StartCycle command.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3 or on execution of the StartCycle command.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4 or on execution of the StartCycle command.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1 or on execution of the StartCycle command.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2 or on execution of the StartCycle command.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1 or on execution of the StartCycle command.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2 or on execution of the StartCycle command.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1 or on execution of the StartCycle command.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2 or on execution of the StartCycle command.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1 or on execution of the StartCycle command.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2 or on execution of the StartCycle command.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1 or on execution of the StartCycle command.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2 or on execution of the StartCycle command.")]
            EIN2,

            [Description("When an event occurs on User Event 1 or on execution of the StartCycle command.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2 or on execution of the StartCycle command.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3 or on execution of the StartCycle command.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4 or on execution of the StartCycle command.")]
            UserEvent4,
        }

        [Serializable]
        public enum DeviceAccessStatus
        {
            [Description("Unknown access.")]
            Unknown,

            [Description("Available to be opened with full access.")]
            ReadWrite,

            [Description("Available to be opened with read-only access.")]
            ReadOnly,

            [Description("Not reachable.")]
            NoAccess,

            [Description("Already opened by another entity.")]
            Busy,

            [Description("Opened with read-write access.")]
            OpenReadWrite,

            [Description("Opened with read-only access.")]
            OpenReadOnly,
        }

        [Serializable]
        public enum DeviceType
        {
            [Description("This enumeration value indicates CoaXPress transport layer technology.")]
            CXP,
        }

        [Serializable]
        public enum CxpLinkConfiguration
        {
            [Description("1 connection @1.250 Gbps.")]
            CXP1_X1,

            [Description("1 connection @2.500 Gbps.")]
            CXP2_X1,

            [Description("1 connection @3.125 Gbps.")]
            CXP3_X1,

            [Description("1 connection @5.000 Gbps.")]
            CXP5_X1,

            [Description("1 connection @6.250 Gbps.")]
            CXP6_X1,

            [Description("1 connection @10.000 Gbps.")]
            CXP10_X1,

            [Description("1 connection @12.500 Gbps.")]
            CXP12_X1,

            [Description("2 connections @1.250 Gbps.")]
            CXP1_X2,

            [Description("2 connection @2.500 Gbps.")]
            CXP2_X2,

            [Description("2 connection @3.125 Gbps.")]
            CXP3_X2,

            [Description("2 connection @5.000 Gbps.")]
            CXP5_X2,

            [Description("2 connection @6.250 Gbps.")]
            CXP6_X2,

            [Description("2 connection @10.000 Gbps.")]
            CXP10_X2,

            [Description("2 connection @12.500 Gbps.")]
            CXP12_X2,

            [Description("3 connections @1.250 Gbps.")]
            CXP1_X3,

            [Description("3 connection @2.500 Gbps.")]
            CXP2_X3,

            [Description("3 connection @3.125 Gbps.")]
            CXP3_X3,

            [Description("3 connection @5.000 Gbps.")]
            CXP5_X3,

            [Description("3 connection @6.250 Gbps.")]
            CXP6_X3,

            [Description("3 connection @10.000 Gbps.")]
            CXP10_X3,

            [Description("3 connection @12.500 Gbps.")]
            CXP12_X3,

            [Description("4 connections @1.250 Gbps.")]
            CXP1_X4,

            [Description("4 connection @2.500 Gbps.")]
            CXP2_X4,

            [Description("4 connection @3.125 Gbps.")]
            CXP3_X4,

            [Description("4 connection @5.000 Gbps.")]
            CXP5_X4,

            [Description("4 connection @6.250 Gbps.")]
            CXP6_X4,

            [Description("4 connection @10.000 Gbps.")]
            CXP10_X4,

            [Description("4 connection @12.500 Gbps.")]
            CXP12_X4,

            [Description("8 connections @1.250 Gbps.")]
            CXP1_X8,

            [Description("8 connection @2.500 Gbps.")]
            CXP2_X8,

            [Description("8 connection @3.125 Gbps.")]
            CXP3_X8,

            [Description("8 connection @5.000 Gbps.")]
            CXP5_X8,

            [Description("8 connection @6.250 Gbps.")]
            CXP6_X8,

            [Description("8 connection @10.000 Gbps.")]
            CXP10_X8,

            [Description("8 connection @12.500 Gbps.")]
            CXP12_X8,

            [Description("Camera Preferred Configuration adapted to the capabilities of the frame grabber.")]
            Preferred,
        }

        [Serializable]
        public enum CxpLinkConfigurationOption
        {
            [Description("Always write the link configuration to the camera.")]
            AlwaysWrite,

            [Description("Write the link configuration to the camera only if it is different from the current configuration.")]
            WriteIfDifferent,
        }

        [Serializable]
        public enum CxpHostConnectionBase
        {
            [Description("CoaXPress physical host connection A.")]
            A,

            [Description("CoaXPress physical host connection B.")]
            B,

            [Description("CoaXPress physical host connection C.")]
            C,

            [Description("CoaXPress physical host connection D.")]
            D,

            [Description("CoaXPress physical host connection E.")]
            E,

            [Description("CoaXPress physical host connection F.")]
            F,

            [Description("CoaXPress physical host connection G.")]
            G,

            [Description("CoaXPress physical host connection H.")]
            H,
        }

        [Serializable]
        public enum CxpTriggerMessageFormat
        {
            [Description("Rising edge and falling edge CoaXPress trigger messages.")]
            Pulse,

            [Description("Rising edge CoaXPress trigger message.")]
            RisingEdge,

            [Description("Alternating rising edge or falling edge CoaXPress trigger message.")]
            Toggle,
        }

        [Serializable]
        public enum CxpTriggerLevel
        {
            [Description("Next trigger message format will be rising edge CoaXPress trigger message.")]
            Low,

            [Description("Next trigger message format will be falling edge CoaXPress trigger message.")]
            High,
        }

        [Serializable]
        public enum CameraAndIlluminationControllerStream
        {
            [Description("CIC uses camera readout and frame buffer status from Stream0.")]
            Stream0,

            [Description("CIC uses camera readout and frame buffer status from Stream1.")]
            Stream1,

            [Description("CIC uses camera readout and frame buffer status from Stream2.")]
            Stream2,

            [Description("CIC uses camera readout and frame buffer status from Stream3.")]
            Stream3,
        }

        [Serializable]
        public enum StartOfSequenceTriggerSource
        {
            [Description("Immediate")]
            Immediate,

            [Description("StartSequence command.")]
            StartSequence,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StartSequence command.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StartSequence command.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3 or on execution of the StartSequence command.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4 or on execution of the StartSequence command.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5 or on execution of the StartSequence command.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6 or on execution of the StartSequence command.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7 or on execution of the StartSequence command.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8 or on execution of the StartSequence command.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1 or on execution of the StartSequence command.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2 or on execution of the StartSequence command.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3 or on execution of the StartSequence command.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4 or on execution of the StartSequence command.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1 or on execution of the StartSequence command.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2 or on execution of the StartSequence command.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3 or on execution of the StartSequence command.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4 or on execution of the StartSequence command.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1 or on execution of the StartSequence command.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2 or on execution of the StartSequence command.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3 or on execution of the StartSequence command.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4 or on execution of the StartSequence command.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1 or on execution of the StartSequence command.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2 or on execution of the StartSequence command.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1 or on execution of the StartSequence command.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2 or on execution of the StartSequence command.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1 or on execution of the StartSequence command.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2 or on execution of the StartSequence command.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1 or on execution of the StartSequence command.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2 or on execution of the StartSequence command.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1 or on execution of the StartSequence command.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2 or on execution of the StartSequence command.")]
            EIN2,

            [Description("When an event occurs on User Event 1 or on execution of the StartSequence command.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2 or on execution of the StartSequence command.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3 or on execution of the StartSequence command.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4 or on execution of the StartSequence command.")]
            UserEvent4,
        }

        [Serializable]
        public enum EndOfSequenceTriggerSource
        {
            [Description("SequenceLength")]
            SequenceLength,

            [Description("StopSequence command.")]
            StopSequence,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StopSequence command.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StopSequence command.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3 or on execution of the StopSequence command.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4 or on execution of the StopSequence command.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5 or on execution of the StopSequence command.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6 or on execution of the StopSequence command.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7 or on execution of the StopSequence command.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8 or on execution of the StopSequence command.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1 or on execution of the StopSequence command.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2 or on execution of the StopSequence command.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3 or on execution of the StopSequence command.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4 or on execution of the StopSequence command.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1 or on execution of the StopSequence command.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2 or on execution of the StopSequence command.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3 or on execution of the StopSequence command.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4 or on execution of the StopSequence command.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1 or on execution of the StopSequence command.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2 or on execution of the StopSequence command.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3 or on execution of the StopSequence command.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4 or on execution of the StopSequence command.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1 or on execution of the StopSequence command.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2 or on execution of the StopSequence command.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1 or on execution of the StopSequence command.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2 or on execution of the StopSequence command.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1 or on execution of the StopSequence command.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2 or on execution of the StopSequence command.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1 or on execution of the StopSequence command.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2 or on execution of the StopSequence command.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1 or on execution of the StopSequence command.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2 or on execution of the StopSequence command.")]
            EIN2,

            [Description("When an event occurs on User Event 1 or on execution of the StopSequence command.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2 or on execution of the StopSequence command.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3 or on execution of the StopSequence command.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4 or on execution of the StopSequence command.")]
            UserEvent4,
        }

        [Serializable]
        public enum EventSelector
        {
            [Description("Start of camera trigger.")]
            CameraTriggerRisingEdge,

            [Description("End of camera trigger.")]
            CameraTriggerFallingEdge,

            [Description("Start of light strobe.")]
            StrobeRisingEdge,

            [Description("End of light strobe.")]
            StrobeFallingEdge,

            [Description("CIC is ready for next camera cycle.")]
            AllowNextCycle,

            [Description("Ignored CIC trigger because CIC is not ready for next camera cycle.")]
            DiscardedCicTrigger,

            [Description("Delayed CIC trigger until CIC is ready for next camera cycle.")]
            PendingCicTrigger,

            [Description("Received acknowledgement for previous CXP trigger message.")]
            CxpTriggerAck,

            [Description("Resent CXP trigger message (acknowledgement to previous CXP trigger message not received).")]
            CxpTriggerResend,

            [Description("CIC trigger.")]
            Trigger,

            [Description("Stream packet size error.")]
            StreamPacketSizeError,

            [Description("Stream packet FIFO overflow.")]
            StreamPacketFifoOverflow,

            [Description("New trigger sent to remote device even though readout of previous frame has not started yet.")]
            CameraTriggerOverrun,

            [Description(" Trigger ignored because ACK to previous trigger has not been received yet.")]
            DidNotReceiveTriggerAck,

            [Description("Trigger packet resend not successful.")]
            TriggerPacketRetryError,

            [Description("Input stream FIFO half full.")]
            InputStreamFifoHalfFull,

            [Description("Input stream FIFO full.")]
            InputStreamFifoFull,

            [Description("Image header error.")]
            ImageHeaderError,

            [Description("MIG AXI write error.")]
            MigAxiWriteError,

            [Description("MIG AXI read error.")]
            MigAxiReadError,

            [Description("Received a CXP packet with unexpected tag.")]
            PacketWithUnexpectedTag,

            [Description("Start of scan skipped(caused by internal exception frame store almost full).")]
            FillLevelAboveIlSosRejected,

            [Description("End of scan(caused by internal exception frame store almost full).")]
            FillLevelAboveAfEarlyEos,

            [Description("External trigger requests too close together.")]
            ExternalTriggerReqsTooClose,
        }

        [Serializable]
        public enum EventNotificationContext1
        {
            [Description("Event-specific context information.")]
            EventSpecific,

            [Description("Low 32-bit part of LineStatusAll.")]
            LineStatusAll,

            [Description("High 32-bit part of LineStatusAll.")]
            LineStatusAllHi,

            [Description("Position of Quadrature Decoder Tool 1.")]
            QDC1Position,

            [Description("Position of Quadrature Decoder Tool 2.")]
            QDC2Position,

            [Description("Position of Quadrature Decoder Tool 3.")]
            QDC3Position,

            [Description("Position of Quadrature Decoder Tool 4.")]
            QDC4Position,

            [Description("Number of currently pending CIC triggers.")]
            PendingCicTriggerCount,

            [Description("Number of LIN1 events.")]
            LIN1EventCount,

            [Description("Number of LIN2 events.")]
            LIN2EventCount,

            [Description("Number of LIN3 events.")]
            LIN3EventCount,

            [Description("Number of LIN4 events.")]
            LIN4EventCount,

            [Description("Number of LIN5 events.")]
            LIN5EventCount,

            [Description("Number of LIN6 events.")]
            LIN6EventCount,

            [Description("Number of LIN7 events.")]
            LIN7EventCount,

            [Description("Number of LIN8 events.")]
            LIN8EventCount,

            [Description("Number of QDC1 events.")]
            QDC1EventCount,

            [Description("Number of QDC1Dir events.")]
            QDC1DirEventCount,

            [Description("Number of QDC2 events.")]
            QDC2EventCount,

            [Description("Number of QDC2Dir events.")]
            QDC2DirEventCount,

            [Description("Number of QDC3 events.")]
            QDC3EventCount,

            [Description("Number of QDC3Dir events.")]
            QDC3DirEventCount,

            [Description("Number of QDC4 events.")]
            QDC4EventCount,

            [Description("Number of QDC4Dir events.")]
            QDC4DirEventCount,

            [Description("Number of DIV1 events.")]
            DIV1EventCount,

            [Description("Number of DIV2 events.")]
            DIV2EventCount,

            [Description("Number of DIV3 events.")]
            DIV3EventCount,

            [Description("Number of DIV4 events.")]
            DIV4EventCount,

            [Description("Number of MDV1 events.")]
            MDV1EventCount,

            [Description("Number of MDV2 events.")]
            MDV2EventCount,

            [Description("Number of MDV3 events.")]
            MDV3EventCount,

            [Description("Number of MDV4 events.")]
            MDV4EventCount,

            [Description("Number of DEL11 events.")]
            DEL11EventCount,

            [Description("Number of DEL12 events.")]
            DEL12EventCount,

            [Description("Number of DEL21 events.")]
            DEL21EventCount,

            [Description("Number of DEL22 events.")]
            DEL22EventCount,

            [Description("Number of DEL31 events.")]
            DEL31EventCount,

            [Description("Number of DEL32 events.")]
            DEL32EventCount,

            [Description("Number of DEL41 events.")]
            DEL41EventCount,

            [Description("Number of DEL42 events.")]
            DEL42EventCount,

            [Description("Number of UserEvent1 events.")]
            UserEvent1EventCount,

            [Description("Number of UserEvent2 events.")]
            UserEvent2EventCount,

            [Description("Number of UserEvent3 events.")]
            UserEvent3EventCount,

            [Description("Number of UserEvent4 events.")]
            UserEvent4EventCount,

            [Description("Number of EIN1 events.")]
            EIN1EventCount,

            [Description("Number of EIN2 events.")]
            EIN2EventCount,

            [Description("Number of CrcErrorCxpA events.")]
            CrcErrorCxpAEventCount,

            [Description("Number of CrcErrorCxpB events.")]
            CrcErrorCxpBEventCount,

            [Description("Number of CrcErrorCxpC events.")]
            CrcErrorCxpCEventCount,

            [Description("Number of CrcErrorCxpD events.")]
            CrcErrorCxpDEventCount,

            [Description("Number of CrcErrorCxpE events.")]
            CrcErrorCxpEEventCount,

            [Description("Number of CrcErrorCxpF events.")]
            CrcErrorCxpFEventCount,

            [Description("Number of CrcErrorCxpG events.")]
            CrcErrorCxpGEventCount,

            [Description("Number of CrcErrorCxpH events.")]
            CrcErrorCxpHEventCount,

            [Description("Number of CameraTriggerRisingEdge events.")]
            CameraTriggerRisingEdgeEventCount,

            [Description("Number of CameraTriggerFallingEdge events.")]
            CameraTriggerFallingEdgeEventCount,

            [Description("Number of StrobeRisingEdge events.")]
            StrobeRisingEdgeEventCount,

            [Description("Number of StrobeFallingEdge events.")]
            StrobeFallingEdgeEventCount,

            [Description("Number of AllowNextCycle events.")]
            AllowNextCycleEventCount,

            [Description("Number of DiscardedCicTrigger events.")]
            DiscardedCicTriggerEventCount,

            [Description("Number of PendingCicTrigger events.")]
            PendingCicTriggerEventCount,

            [Description("Number of CxpTriggerAck events.")]
            CxpTriggerAckEventCount,

            [Description("Number of CxpTriggerResend events.")]
            CxpTriggerResendEventCount,

            [Description("Number of Trigger events.")]
            TriggerEventCount,

            [Description("Number of StreamPacketSizeError events.")]
            StreamPacketSizeErrorEventCount,
            [Description("Number of StreamPacketFifoOverflow events.")]
            StreamPacketFifoOverflowEventCount,

            [Description("Number of CameraTriggerOverrun events.")]
            CameraTriggerOverrunEventCount,

            [Description("Number of DidNotReceiveTriggerAck events.")]
            DidNotReceiveTriggerAckEventCount,

            [Description("Number of TriggerPacketRetryError events.")]
            TriggerPacketRetryErrorEventCount,

            [Description("Number of InputStreamFifoHalfFull events.")]
            InputStreamFifoHalfFullEventCount,

            [Description("Number of InputStreamFifoFull events.")]
            InputStreamFifoFullEventCount,

            [Description("Number of ImageHeaderError events.")]
            ImageHeaderErrorEventCount,

            [Description("Number of MigAxiWriteError events.")]
            MigAxiWriteErrorEventCount,

            [Description("Number of MigAxiReadError events.")]
            MigAxiReadErrorEventCount,

            [Description("Number of PacketWithUnexpectedTag events.")]
            PacketWithUnexpectedTagEventCount,

            [Description("Number of FillLevelAboveIlSosRejected events.")]
            FillLevelAboveIlSosRejectedEventCount,

            [Description("Number of FillLevelAboveAfEarlyEos events.")]
            FillLevelAboveAfEarlyEosEventCount,

            [Description("Number of ExternalTriggerReqsTooClose events.")]
            ExternalTriggerReqsTooCloseEventCount,
        }

        [Serializable]
        public enum EventNotificationContext2
        {
            [Description("Event-specific context information.")]
            EventSpecific,

            [Description("Low 32-bit part of LineStatusAll.")]
            LineStatusAll,

            [Description("High 32-bit part of LineStatusAll.")]
            LineStatusAllHi,

            [Description("Position of Quadrature Decoder Tool 1.")]
            QDC1Position,

            [Description("Position of Quadrature Decoder Tool 2.")]
            QDC2Position,

            [Description("Position of Quadrature Decoder Tool 3.")]
            QDC3Position,

            [Description("Position of Quadrature Decoder Tool 4.")]
            QDC4Position,

            [Description("Number of currently pending CIC triggers.")]
            PendingCicTriggerCount,

            [Description("Number of LIN1 events.")]
            LIN1EventCount,

            [Description("Number of LIN2 events.")]
            LIN2EventCount,

            [Description("Number of LIN3 events.")]
            LIN3EventCount,

            [Description("Number of LIN4 events.")]
            LIN4EventCount,

            [Description("Number of LIN5 events.")]
            LIN5EventCount,

            [Description("Number of LIN6 events.")]
            LIN6EventCount,

            [Description("Number of LIN7 events.")]
            LIN7EventCount,

            [Description("Number of LIN8 events.")]
            LIN8EventCount,

            [Description("Number of QDC1 events.")]
            QDC1EventCount,

            [Description("Number of QDC1Dir events.")]
            QDC1DirEventCount,

            [Description("Number of QDC2 events.")]
            QDC2EventCount,

            [Description("Number of QDC2Dir events.")]
            QDC2DirEventCount,

            [Description("Number of QDC3 events.")]
            QDC3EventCount,

            [Description("Number of QDC3Dir events.")]
            QDC3DirEventCount,

            [Description("Number of QDC4 events.")]
            QDC4EventCount,

            [Description("Number of QDC4Dir events.")]
            QDC4DirEventCount,

            [Description("Number of DIV1 events.")]
            DIV1EventCount,

            [Description("Number of DIV2 events.")]
            DIV2EventCount,

            [Description("Number of DIV3 events.")]
            DIV3EventCount,

            [Description("Number of DIV4 events.")]
            DIV4EventCount,

            [Description("Number of MDV1 events.")]
            MDV1EventCount,

            [Description("Number of MDV2 events.")]
            MDV2EventCount,

            [Description("Number of MDV3 events.")]
            MDV3EventCount,

            [Description("Number of MDV4 events.")]
            MDV4EventCount,

            [Description("Number of DEL11 events.")]
            DEL11EventCount,

            [Description("Number of DEL12 events.")]
            DEL12EventCount,

            [Description("Number of DEL21 events.")]
            DEL21EventCount,

            [Description("Number of DEL22 events.")]
            DEL22EventCount,

            [Description("Number of DEL31 events.")]
            DEL31EventCount,

            [Description("Number of DEL32 events.")]
            DEL32EventCount,

            [Description("Number of DEL41 events.")]
            DEL41EventCount,

            [Description("Number of DEL42 events.")]
            DEL42EventCount,

            [Description("Number of UserEvent1 events.")]
            UserEvent1EventCount,

            [Description("Number of UserEvent2 events.")]
            UserEvent2EventCount,

            [Description("Number of UserEvent3 events.")]
            UserEvent3EventCount,

            [Description("Number of UserEvent4 events.")]
            UserEvent4EventCount,

            [Description("Number of EIN1 events.")]
            EIN1EventCount,

            [Description("Number of EIN2 events.")]
            EIN2EventCount,

            [Description("Number of CrcErrorCxpA events.")]
            CrcErrorCxpAEventCount,

            [Description("Number of CrcErrorCxpB events.")]
            CrcErrorCxpBEventCount,

            [Description("Number of CrcErrorCxpC events.")]
            CrcErrorCxpCEventCount,

            [Description("Number of CrcErrorCxpD events.")]
            CrcErrorCxpDEventCount,

            [Description("Number of CrcErrorCxpE events.")]
            CrcErrorCxpEEventCount,

            [Description("Number of CrcErrorCxpF events.")]
            CrcErrorCxpFEventCount,

            [Description("Number of CrcErrorCxpG events.")]
            CrcErrorCxpGEventCount,

            [Description("Number of CrcErrorCxpH events.")]
            CrcErrorCxpHEventCount,

            [Description("Number of CameraTriggerRisingEdge events.")]
            CameraTriggerRisingEdgeEventCount,

            [Description("Number of CameraTriggerFallingEdge events.")]
            CameraTriggerFallingEdgeEventCount,

            [Description("Number of StrobeRisingEdge events.")]
            StrobeRisingEdgeEventCount,

            [Description("Number of StrobeFallingEdge events.")]
            StrobeFallingEdgeEventCount,

            [Description("Number of AllowNextCycle events.")]
            AllowNextCycleEventCount,

            [Description("Number of DiscardedCicTrigger events.")]
            DiscardedCicTriggerEventCount,

            [Description("Number of PendingCicTrigger events.")]
            PendingCicTriggerEventCount,

            [Description("Number of CxpTriggerAck events.")]
            CxpTriggerAckEventCount,

            [Description("Number of CxpTriggerResend events.")]
            CxpTriggerResendEventCount,

            [Description("Number of Trigger events.")]
            TriggerEventCount,

            [Description("Number of StreamPacketSizeError events.")]
            StreamPacketSizeErrorEventCount,

            [Description("Number of StreamPacketFifoOverflow events.")]
            StreamPacketFifoOverflowEventCount,

            [Description("Number of CameraTriggerOverrun events.")]
            CameraTriggerOverrunEventCount,

            [Description("Number of DidNotReceiveTriggerAck events.")]
            DidNotReceiveTriggerAckEventCount,

            [Description("Number of TriggerPacketRetryError events.")]
            TriggerPacketRetryErrorEventCount,

            [Description("Number of InputStreamFifoHalfFull events.")]
            InputStreamFifoHalfFullEventCount,

            [Description("Number of InputStreamFifoFull events.")]
            InputStreamFifoFullEventCount,

            [Description("Number of ImageHeaderError events.")]
            ImageHeaderErrorEventCount,

            [Description("Number of MigAxiWriteError events.")]
            MigAxiWriteErrorEventCount,

            [Description("Number of MigAxiReadError events.")]
            MigAxiReadErrorEventCount,

            [Description("Number of PacketWithUnexpectedTag events.")]
            PacketWithUnexpectedTagEventCount,

            [Description("Number of FillLevelAboveIlSosRejected events.")]
            FillLevelAboveIlSosRejectedEventCount,

            [Description("Number of FillLevelAboveAfEarlyEos events.")]
            FillLevelAboveAfEarlyEosEventCount,

            [Description("Number of ExternalTriggerReqsTooClose events.")]
            ExternalTriggerReqsTooCloseEventCount,
        }

        [Serializable]
        public enum EventNotificationContext3
        {
            [Description("Position of Quadrature Decoder Tool 1.")]
            QDC1Position,

            [Description("Position of Quadrature Decoder Tool 2.")]
            QDC2Position,

            [Description("Position of Quadrature Decoder Tool 3.")]
            QDC3Position,

            [Description("Position of Quadrature Decoder Tool 4.")]
            QDC4Position,

            [Description("Number of currently pending CIC triggers.")]
            PendingCicTriggerCount,

            [Description("Number of LIN1 events.")]
            LIN1EventCount,

            [Description("Number of LIN2 events.")]
            LIN2EventCount,

            [Description("Number of LIN3 events.")]
            LIN3EventCount,

            [Description("Number of LIN4 events.")]
            LIN4EventCount,

            [Description("Number of LIN5 events.")]
            LIN5EventCount,

            [Description("Number of LIN6 events.")]
            LIN6EventCount,

            [Description("Number of LIN7 events.")]
            LIN7EventCount,

            [Description("Number of LIN8 events.")]
            LIN8EventCount,

            [Description("Number of QDC1 events.")]
            QDC1EventCount,

            [Description("Number of QDC1Dir events.")]
            QDC1DirEventCount,

            [Description("Number of QDC2 events.")]
            QDC2EventCount,

            [Description("Number of QDC2Dir events.")]
            QDC2DirEventCount,

            [Description("Number of QDC3 events.")]
            QDC3EventCount,

            [Description("Number of QDC3Dir events.")]
            QDC3DirEventCount,

            [Description("Number of QDC4 events.")]
            QDC4EventCount,

            [Description("Number of QDC4Dir events.")]
            QDC4DirEventCount,

            [Description("Number of DIV1 events.")]
            DIV1EventCount,

            [Description("Number of DIV2 events.")]
            DIV2EventCount,

            [Description("Number of DIV3 events.")]
            DIV3EventCount,

            [Description("Number of DIV4 events.")]
            DIV4EventCount,

            [Description("Number of MDV1 events.")]
            MDV1EventCount,

            [Description("Number of MDV2 events.")]
            MDV2EventCount,

            [Description("Number of MDV3 events.")]
            MDV3EventCount,

            [Description("Number of MDV4 events.")]
            MDV4EventCount,

            [Description("Number of DEL11 events.")]
            DEL11EventCount,

            [Description("Number of DEL12 events.")]
            DEL12EventCount,

            [Description("Number of DEL21 events.")]
            DEL21EventCount,

            [Description("Number of DEL22 events.")]
            DEL22EventCount,

            [Description("Number of DEL31 events.")]
            DEL31EventCount,

            [Description("Number of DEL32 events.")]
            DEL32EventCount,

            [Description("Number of DEL41 events.")]
            DEL41EventCount,

            [Description("Number of DEL42 events.")]
            DEL42EventCount,

            [Description("Number of UserEvent1 events.")]
            UserEvent1EventCount,

            [Description("Number of UserEvent2 events.")]
            UserEvent2EventCount,

            [Description("Number of UserEvent3 events.")]
            UserEvent3EventCount,

            [Description("Number of UserEvent4 events.")]
            UserEvent4EventCount,

            [Description("Number of EIN1 events.")]
            EIN1EventCount,

            [Description("Number of EIN2 events.")]
            EIN2EventCount,

            [Description("Number of CrcErrorCxpA events.")]
            CrcErrorCxpAEventCount,

            [Description("Number of CrcErrorCxpB events.")]
            CrcErrorCxpBEventCount,

            [Description("Number of CrcErrorCxpC events.")]
            CrcErrorCxpCEventCount,

            [Description("Number of CrcErrorCxpD events.")]
            CrcErrorCxpDEventCount,

            [Description("Number of CrcErrorCxpE events.")]
            CrcErrorCxpEEventCount,

            [Description("Number of CrcErrorCxpF events.")]
            CrcErrorCxpFEventCount,

            [Description("Number of CrcErrorCxpG events.")]
            CrcErrorCxpGEventCount,

            [Description("Number of CrcErrorCxpH events.")]
            CrcErrorCxpHEventCount,

            [Description("Number of CameraTriggerRisingEdge events.")]
            CameraTriggerRisingEdgeEventCount,

            [Description("Number of CameraTriggerFallingEdge events.")]
            CameraTriggerFallingEdgeEventCount,

            [Description("Number of StrobeRisingEdge events.")]
            StrobeRisingEdgeEventCount,

            [Description("Number of StrobeFallingEdge events.")]
            StrobeFallingEdgeEventCount,

            [Description("Number of AllowNextCycle events.")]
            AllowNextCycleEventCount,

            [Description("Number of DiscardedCicTrigger events.")]
            DiscardedCicTriggerEventCount,

            [Description("Number of PendingCicTrigger events.")]
            PendingCicTriggerEventCount,

            [Description("Number of CxpTriggerAck events.")]
            CxpTriggerAckEventCount,

            [Description("Number of CxpTriggerResend events.")]
            CxpTriggerResendEventCount,

            [Description("Number of Trigger events.")]
            TriggerEventCount,

            [Description("Number of StreamPacketSizeError events.")]
            StreamPacketSizeErrorEventCount,

            [Description("Number of StreamPacketFifoOverflow events.")]
            StreamPacketFifoOverflowEventCount,

            [Description("Number of CameraTriggerOverrun events.")]
            CameraTriggerOverrunEventCount,

            [Description("Number of DidNotReceiveTriggerAck events.")]
            DidNotReceiveTriggerAckEventCount,

            [Description("Number of TriggerPacketRetryError events.")]
            TriggerPacketRetryErrorEventCount,

            [Description("Number of InputStreamFifoHalfFull events.")]
            InputStreamFifoHalfFullEventCount,

            [Description("Number of InputStreamFifoFull events.")]
            InputStreamFifoFullEventCount,

            [Description("Number of ImageHeaderError events.")]
            ImageHeaderErrorEventCount,

            [Description("Number of MigAxiWriteError events.")]
            MigAxiWriteErrorEventCount,

            [Description("Number of MigAxiReadError events.")]
            MigAxiReadErrorEventCount,

            [Description("Number of PacketWithUnexpectedTag events.")]
            PacketWithUnexpectedTagEventCount,

            [Description("Number of FillLevelAboveIlSosRejected events.")]
            FillLevelAboveIlSosRejectedEventCount,

            [Description("Number of FillLevelAboveAfEarlyEos events.")]
            FillLevelAboveAfEarlyEosEventCount,

            [Description("Number of ExternalTriggerReqsTooClose events.")]
            ExternalTriggerReqsTooCloseEventCount,
        }

        [Serializable]
        public enum ErrorSelector
        {
            [Description(" All errors.")]
            All,

            [Description(" Stream packet size error.")]
            StreamPacketSizeError,

            [Description(" Stream packet FIFO overflow.")]
            StreamPacketFifoOverflow,

            [Description(" New trigger sent to remote device even though readout of previous frame has not started yet.")]
            CameraTriggerOverrun,

            [Description(" Trigger ignored because ACK to previous trigger has not been received yet.")]
            DidNotReceiveTriggerAck,

            [Description(" Trigger packet resend not successful.")]
            TriggerPacketRetryError,

            [Description(" Input stream FIFO half full.")]
            InputStreamFifoHalfFull,

            [Description(" Input stream FIFO full.")]
            InputStreamFifoFull,

            [Description(" Image header error.")]
            ImageHeaderError,

            [Description(" MIG AXI write error.")]
            MigAxiWriteError,

            [Description(" MIG AXI read error.")]
            MigAxiReadError,

            [Description(" Received a CXP packet with unexpected tag.")]
            PacketWithUnexpectedTag,

            [Description(" Stream packet CRC error on connector A.")]
            StreamPacketCrcError0,

            [Description(" Stream packet CRC error on connector B.")]
            StreamPacketCrcError1,

            [Description(" Stream packet CRC error on connector C.")]
            StreamPacketCrcError2,

            [Description(" Stream packet CRC error on connector D.")]
            StreamPacketCrcError3,

            [Description(" Stream packet CRC error on connector E.")]
            StreamPacketCrcError4,

            [Description(" Stream packet CRC error on connector F.")]
            StreamPacketCrcError5,

            [Description(" Stream packet CRC error on connector G.")]
            StreamPacketCrcError6,

            [Description(" Stream packet CRC error on connector H.")]
            StreamPacketCrcError7,

            [Description(" Start of scan skipped (caused by internal exception")]
            StartOfScanSkipped,

            [Description(" End of scan (caused by internal exception")]
            PrematureEndOfScan,

            [Description(" External trigger requests too close together.")]
            ExternalTriggerReqsTooClose,

            [Description(" Unknown errors.")]
            Unknown,
        }

        [Serializable]
        public enum ExposureMode
        {
            [Description("TriggerWidth")]
            TriggerWidth,
        }


        [Serializable]
        public enum TriggerMode
        {
            [Description("On")]
            On,

            [Description("Off")]
            Off,
        }

        [Serializable]
        public enum AcquisitionFrameRateEnableType
        {
            On,
            Off,
        }

        [Serializable]
        public enum TiggerModeType
        {
            On,
            Off,
        }

        [Serializable]
        public enum StreamType
        {
            [Description("This enumeration value indicates CoaXPress transport layer technology.")]
            CXP,
        }

        [Serializable]
        public enum ErrorSelectorType
        {
            [Description(" All errors.")]
            All,

            [Description(" Stream packet size error.")]
            StreamPacketSizeError,

            [Description(" Stream packet FIFO overflow.")]
            StreamPacketFifoOverflow,

            [Description(" New trigger sent to remote device even though readout of previous frame has not started yet.")]
            CameraTriggerOverrun,

            [Description(" Trigger ignored because ACK to previous trigger has not been received yet.")]
            DidNotReceiveTriggerAck,

            [Description(" Trigger packet resend not successful.")]
            TriggerPacketRetryError,

            [Description(" Input stream FIFO half full.")]
            InputStreamFifoHalfFull,

            [Description(" Input stream FIFO full.")]
            InputStreamFifoFull,

            [Description(" Image header error.")]
            ImageHeaderError,

            [Description(" MIG AXI write error.")]
            MigAxiWriteError,

            [Description(" MIG AXI read error.")]
            MigAxiReadError,

            [Description(" Received a CXP packet with unexpected tag.")]
            PacketWithUnexpectedTag,

            [Description(" Stream packet CRC error on connector A.")]
            StreamPacketCrcError0,

            [Description(" Stream packet CRC error on connector B.")]
            StreamPacketCrcError1,

            [Description(" Stream packet CRC error on connector C.")]
            StreamPacketCrcError2,

            [Description(" Stream packet CRC error on connector D.")]
            StreamPacketCrcError3,

            [Description(" Stream packet CRC error on connector E.")]
            StreamPacketCrcError4,

            [Description(" Stream packet CRC error on connector F.")]
            StreamPacketCrcError5,

            [Description(" Stream packet CRC error on connector G.")]
            StreamPacketCrcError6,

            [Description(" Stream packet CRC error on connector H.")]
            StreamPacketCrcError7,

            [Description(" Start of scan skipped (caused by internal exception: frame store almost full).")]
            StartOfScanSkipped,

            [Description(" End of scan (caused by internal exception: frame store almost full).")]
            PrematureEndOfScan,

            [Description(" External trigger requests too close together.")]
            ExternalTriggerReqsTooClose,

            [Description(" Unknown errors.")]
            Unknown,
        }

        [Serializable]
        public enum PixelFormat
        {
            [Description("BayerBG10pmsb.")]
            BayerBG10pmsb,

            [Description("BayerBG12pmsb.")]
            BayerBG12pmsb,

            [Description("BayerBG14pmsb.")]
            BayerBG14pmsb,

            [Description("BayerGB10pmsb.")]
            BayerGB10pmsb,

            [Description("BayerGB12pmsb.")]
            BayerGB12pmsb,

            [Description("BayerGB14pmsb.")]
            BayerGB14pmsb,

            [Description("BayerGR10pmsb.")]
            BayerGR10pmsb,

            [Description("BayerGR12pmsb.")]
            BayerGR12pmsb,

            [Description("BayerGR14pmsb.")]
            BayerGR14pmsb,

            [Description("BayerRG10pmsb.")]
            BayerRG10pmsb,

            [Description("BayerRG12pmsb.")]
            BayerRG12pmsb,

            [Description("BayerRG14pmsb.")]
            BayerRG14pmsb,

            [Description("Mono10pmsb.")]
            Mono10pmsb,

            [Description("Mono12pmsb.")]
            Mono12pmsb,

            [Description("Mono14pmsb.")]
            Mono14pmsb,

            [Description("RGB10pmsb.")]
            RGB10pmsb,

            [Description("RGB12pmsb.")]
            RGB12pmsb,

            [Description("RGB14pmsb.")]
            RGB14pmsb,

            [Description("RGBa10pmsb.")]
            RGBa10pmsb,

            [Description("RGBa12pmsb.")]
            RGBa12pmsb,

            [Description("RGBa14pmsb.")]
            RGBa14pmsb,

            [Description("YCbCr601_10pmsb.")]
            YCbCr601_10pmsb,

            [Description("YCbCr601_12pmsb.")]
            YCbCr601_12pmsb,

            [Description("YCbCr601_14pmsb.")]
            YCbCr601_14pmsb,

            [Description("YCbCr601_16.")]
            YCbCr601_16,

            [Description("YCbCr601_411_10pmsb.")]
            YCbCr601_411_10pmsb,

            [Description("YCbCr601_411_12pmsb.")]
            YCbCr601_411_12pmsb,

            [Description("YCbCr601_411_14pmsb.")]
            YCbCr601_411_14pmsb,

            [Description("YCbCr601_411_16.")]
            YCbCr601_411_16,

            [Description("YCbCr601_411_8.")]
            YCbCr601_411_8,

            [Description("YCbCr601_422_10pmsb.")]
            YCbCr601_422_10pmsb,

            [Description("YCbCr601_422_12pmsb.")]
            YCbCr601_422_12pmsb,

            [Description("YCbCr601_422_14pmsb.")]
            YCbCr601_422_14pmsb,

            [Description("YCbCr601_422_16.")]
            YCbCr601_422_16,

            [Description("YCbCr601_8.")]
            YCbCr601_8,

            [Description("YCbCr709_10pmsb.")]
            YCbCr709_10pmsb,

            [Description("YCbCr709_12pmsb.")]
            YCbCr709_12pmsb,

            [Description("YCbCr709_14pmsb.")]
            YCbCr709_14pmsb,

            [Description("YCbCr709_16.")]
            YCbCr709_16,

            [Description("YCbCr709_411_10pmsb.")]
            YCbCr709_411_10pmsb,

            [Description("YCbCr709_411_12pmsb.")]
            YCbCr709_411_12pmsb,

            [Description("YCbCr709_411_14pmsb.")]
            YCbCr709_411_14pmsb,

            [Description("YCbCr709_411_16.")]
            YCbCr709_411_16,

            [Description("YCbCr709_411_8.")]
            YCbCr709_411_8,

            [Description("YCbCr709_422_10pmsb.")]
            YCbCr709_422_10pmsb,

            [Description("YCbCr709_422_12pmsb.")]
            YCbCr709_422_12pmsb,

            [Description("YCbCr709_422_14pmsb.")]
            YCbCr709_422_14pmsb,

            [Description("YCbCr709_422_16.")]
            YCbCr709_422_16,

            [Description("YCbCr709_8.")]
            YCbCr709_8,

            [Description("YUV10pmsb.")]
            YUV10pmsb,

            [Description("YUV12pmsb.")]
            YUV12pmsb,

            [Description("YUV14pmsb.")]
            YUV14pmsb,

            [Description("YUV16.")]
            YUV16,

            [Description("YUV411_10pmsb.")]
            YUV411_10pmsb,

            [Description("YUV411_12pmsb.")]
            YUV411_12pmsb,

            [Description("YUV411_14pmsb.")]
            YUV411_14pmsb,

            [Description("YUV411_16.")]
            YUV411_16,

            [Description("YUV411_8.")]
            YUV411_8,

            [Description("YUV422_10pmsb.")]
            YUV422_10pmsb,

            [Description("YUV422_12pmsb.")]
            YUV422_12pmsb,

            [Description("YUV422_14pmsb.")]
            YUV422_14pmsb,

            [Description("YUV422_16.")]
            YUV422_16,

            [Description("YUV8.")]
            YUV8,

            [Description("Blue 10-bit.")]
            B10,

            [Description("Blue 12-bit.")]
            B12,

            [Description("Blue 16-bit.")]
            B16,

            [Description("Blue 8-bit.")]
            B8,

            [Description("Bayer Blue-Green 10-bit unpacked.")]
            BayerBG10,

            [Description("Bayer Blue-Green 10-bit packed.")]
            BayerBG10p,

            [Description("Bayer Blue-Green 10-bit packed.")]
            BayerBG10Packed,

            [Description("Bayer Blue-Green 12-bit unpacked.")]
            BayerBG12,

            [Description("Bayer Blue-Green 12-bit packed.")]
            BayerBG12p,

            [Description("Bayer Blue-Green 12-bit packed.")]
            BayerBG12Packed,

            [Description("Bayer Blue-Green 14-bit.")]
            BayerBG14,

            [Description("Bayer Blue-Green 14-bit packed.")]
            BayerBG14p,

            [Description("Bayer Blue-Green 16-bit.")]
            BayerBG16,

            [Description("Bayer Blue-Green 4-bit packed.")]
            BayerBG4p,

            [Description("Bayer Blue-Green 8-bit.")]
            BayerBG8,

            [Description("Bayer Green-Blue 10-bit unpacked.")]
            BayerGB10,

            [Description("Bayer Green-Blue 10-bit packed.")]
            BayerGB10p,

            [Description("Bayer Green-Blue 10-bit packed.")]
            BayerGB10Packed,

            [Description("Bayer Green-Blue 12-bit unpacked.")]
            BayerGB12,

            [Description("Bayer Green-Blue 12-bit packed.")]
            BayerGB12p,

            [Description("Bayer Green-Blue 12-bit packed.")]
            BayerGB12Packed,

            [Description("Bayer Green-Blue 14-bit.")]
            BayerGB14,

            [Description("Bayer Green-Blue 14-bit packed.")]
            BayerGB14p,

            [Description("Bayer Green-Blue 16-bit.")]
            BayerGB16,

            [Description("Bayer Green-Blue 4-bit packed.")]
            BayerGB4p,

            [Description("Bayer Green-Blue 8-bit.")]
            BayerGB8,

            [Description("Bayer Green-Red 10-bit unpacked.")]
            BayerGR10,

            [Description("Bayer Green-Red 10-bit packed.")]
            BayerGR10p,

            [Description("Bayer Green-Red 10-bit packed.")]
            BayerGR10Packed,

            [Description("Bayer Green-Red 12-bit unpacked.")]
            BayerGR12,

            [Description("Bayer Green-Red 12-bit packed.")]
            BayerGR12p,

            [Description("Bayer Green-Red 12-bit packed.")]
            BayerGR12Packed,

            [Description("Bayer Green-Red 14-bit.")]
            BayerGR14,

            [Description("Bayer Green-Red 14-bit packed.")]
            BayerGR14p,

            [Description("Bayer Green-Red 16-bit.")]
            BayerGR16,

            [Description("Bayer Green-Red 4-bit packed.")]
            BayerGR4p,

            [Description("Bayer Green-Red 8-bit.")]
            BayerGR8,

            [Description("Bayer Red-Green 10-bit unpacked.")]
            BayerRG10,

            [Description("Bayer Red-Green 10-bit packed.")]
            BayerRG10p,

            [Description("Bayer Red-Green 10-bit packed.")]
            BayerRG10Packed,

            [Description("Bayer Red-Green 12-bit unpacked.")]
            BayerRG12,

            [Description("Bayer Red-Green 12-bit packed.")]
            BayerRG12p,

            [Description("Bayer Red-Green 12-bit packed.")]
            BayerRG12Packed,

            [Description("Bayer Red-Green 14-bit.")]
            BayerRG14,

            [Description("Bayer Red-Green 14-bit packed.")]
            BayerRG14p,

            [Description("Bayer Red-Green 16-bit.")]
            BayerRG16,

            [Description("Bayer Red-Green 4-bit packed.")]
            BayerRG4p,

            [Description("Bayer Red-Green 8-bit.")]
            BayerRG8,

            [Description("Blue-Green-Red 10-bit unpacked.")]
            BGR10,

            [Description("Blue-Green-Red 10-bit packed.")]
            BGR10p,

            [Description("Blue-Green-Red 12-bit unpacked.")]
            BGR12,

            [Description("Blue-Green-Red 12-bit packed.")]
            BGR12p,

            [Description("Blue-Green-Red 14-bit unpacked.")]
            BGR14,

            [Description("Blue-Green-Red 16-bit.")]
            BGR16,

            [Description("Blue-Green-Red 5/6/5-bit packed.")]
            BGR565p,

            [Description("Blue-Green-Red 8-bit.")]
            BGR8,

            [Description("BGR8a32.")]
            BGR8a32,

            [Description("Blue-Green-Red-alpha 10-bit unpacked.")]
            BGRa10,

            [Description("Blue-Green-Red-alpha 10-bit packed.")]
            BGRa10p,

            [Description("Blue-Green-Red-alpha 12-bit unpacked.")]
            BGRa12,

            [Description("Blue-Green-Red-alpha 12-bit packed.")]
            BGRa12p,

            [Description("Blue-Green-Red-alpha 14-bit unpacked.")]
            BGRa14,

            [Description("Blue-Green-Red-alpha 16-bit.")]
            BGRa16,

            [Description("Blue-Green-Red-alpha 8-bit.")]
            BGRa8,

            [Description("Bi-color Blue/Green - Red/Green 10-bit unpacked.")]
            BiColorBGRG10,

            [Description("Bi-color Blue/Green - Red/Green 10-bit packed.")]
            BiColorBGRG10p,

            [Description("Bi-color Blue/Green - Red/Green 12-bit unpacked.")]
            BiColorBGRG12,

            [Description("Bi-color Blue/Green - Red/Green 12-bit packed.")]
            BiColorBGRG12p,

            [Description("Bi-color Blue/Green - Red/Green 8-bit.")]
            BiColorBGRG8,

            [Description("Bi-color Red/Green - Blue/Green 10-bit unpacked.")]
            BiColorRGBG10,

            [Description("Bi-color Red/Green - Blue/Green 10-bit packed.")]
            BiColorRGBG10p,

            [Description("Bi-color Red/Green - Blue/Green 12-bit unpacked.")]
            BiColorRGBG12,

            [Description("Bi-color Red/Green - Blue/Green 12-bit packed.")]
            BiColorRGBG12p,

            [Description("Bi-color Red/Green - Blue/Green 8-bit.")]
            BiColorRGBG8,

            [Description("Confidence 1-bit unpacked.")]
            Confidence1,

            [Description("Confidence 16-bit.")]
            Confidence16,

            [Description("Confidence 1-bit packed.")]
            Confidence1p,

            [Description("Confidence 32-bit floating point.")]
            Confidence32f,

            [Description("Confidence 8-bit.")]
            Confidence8,

            [Description("3D coordinate A 10-bit packed.")]
            Coord3D_A10p,

            [Description("3D coordinate A 12-bit packed.")]
            Coord3D_A12p,

            [Description("3D coordinate A 16-bit.")]
            Coord3D_A16,

            [Description("3D coordinate A 32-bit floating point.")]
            Coord3D_A32f,

            [Description("3D coordinate A 8-bit.")]
            Coord3D_A8,

            [Description("3D coordinate A-B-C 10-bit packed.")]
            Coord3D_ABC10p,

            [Description("3D coordinate A-B-C 10-bit packed planar.")]
            Coord3D_ABC10p_Planar,

            [Description("3D coordinate A-B-C 12-bit packed.")]
            Coord3D_ABC12p,

            [Description("3D coordinate A-B-C 12-bit packed planar.")]
            Coord3D_ABC12p_Planar,

            [Description("3D coordinate A-B-C 16-bit.")]
            Coord3D_ABC16,

            [Description("3D coordinate A-B-C 16-bit planar.")]
            Coord3D_ABC16_Planar,

            [Description("3D coordinate A-B-C 32-bit floating point.")]
            Coord3D_ABC32f,

            [Description("3D coordinate A-B-C 32-bit floating point planar.")]
            Coord3D_ABC32f_Planar,

            [Description("3D coordinate A-B-C 8-bit.")]
            Coord3D_ABC8,

            [Description("3D coordinate A-B-C 8-bit planar.")]
            Coord3D_ABC8_Planar,

            [Description("3D coordinate A-C 10-bit packed.")]
            Coord3D_AC10p,

            [Description("3D coordinate A-C 10-bit packed planar.")]
            Coord3D_AC10p_Planar,

            [Description("3D coordinate A-C 12-bit packed.")]
            Coord3D_AC12p,

            [Description("3D coordinate A-C 12-bit packed planar.")]
            Coord3D_AC12p_Planar,

            [Description("3D coordinate A-C 16-bit.")]
            Coord3D_AC16,

            [Description("3D coordinate A-C 16-bit planar.")]
            Coord3D_AC16_Planar,

            [Description("3D coordinate A-C 32-bit floating point.")]
            Coord3D_AC32f,

            [Description("3D coordinate A-C 32-bit floating point planar.")]
            Coord3D_AC32f_Planar,

            [Description("3D coordinate A-C 8-bit.")]
            Coord3D_AC8,

            [Description("3D coordinate A-C 8-bit planar.")]
            Coord3D_AC8_Planar,

            [Description("3D coordinate B 10-bit packed.")]
            Coord3D_B10p,

            [Description("3D coordinate B 12-bit packed.")]
            Coord3D_B12p,

            [Description("3D coordinate B 16-bit.")]
            Coord3D_B16,

            [Description("3D coordinate B 32-bit floating point.")]
            Coord3D_B32f,

            [Description("3D coordinate B 8-bit.")]
            Coord3D_B8,

            [Description("3D coordinate C 10-bit packed.")]
            Coord3D_C10p,

            [Description("3D coordinate C 12-bit packed.")]
            Coord3D_C12p,

            [Description("3D coordinate C 16-bit.")]
            Coord3D_C16,

            [Description("3D coordinate C 32-bit floating point.")]
            Coord3D_C32f,

            [Description("3D coordinate C 8-bit.")]
            Coord3D_C8,

            [Description("CustomBayerBG14.")]
            CustomBayerBG14,

            [Description("CustomBayerGB14.")]
            CustomBayerGB14,

            [Description("CustomBayerGR14.")]
            CustomBayerGR14,

            [Description("CustomBayerRG14.")]
            CustomBayerRG14,

            [Description("CustomJFIF.")]
            CustomJFIF,

            [Description("Green 10-bit.")]
            G10,

            [Description("Green 12-bit.")]
            G12,

            [Description("Green 16-bit.")]
            G16,

            [Description("Green 8-bit.")]
            G8,

            [Description("Monochrome 10-bit unpacked.")]
            Mono10,

            [Description("Monochrome 10-bit packed.")]
            Mono10p,

            [Description("Monochrome 10-bit packed.")]
            Mono10Packed,

            [Description("Monochrome 12-bit unpacked.")]
            Mono12,

            [Description("Monochrome 12-bit packed.")]
            Mono12p,

            [Description("Monochrome 12-bit packed.")]
            Mono12Packed,

            [Description("Monochrome 14-bit unpacked.")]
            Mono14,

            [Description("Monochrome 14-bit packed.")]
            Mono14p,

            [Description("Monochrome 16-bit.")]
            Mono16,

            [Description("Monochrome 1-bit packed.")]
            Mono1p,

            [Description("Monochrome 2-bit packed.")]
            Mono2p,

            [Description("Monochrome 32-bit.")]
            Mono32,

            [Description("Monochrome 4-bit packed.")]
            Mono4p,

            [Description("Monochrome 8-bit.")]
            Mono8,

            [Description("Monochrome 8-bit signed.")]
            Mono8s,

            [Description("Red 10-bit.")]
            R10,

            [Description("Red 12-bit.")]
            R12,

            [Description("Red 16-bit.")]
            R16,

            [Description("Red 8-bit.")]
            R8,

            [Description("Red-Green-Blue 10-bit unpacked.")]
            RGB10,

            [Description("Red-Green-Blue 10-bit unpacked planar.")]
            RGB10_Planar,

            [Description("Red-Green-Blue 10-bit packed.")]
            RGB10p,

            [Description("Red-Green-Blue 10-bit packed into 32-bit.")]
            RGB10p32,

            [Description("Red-Green-Blue 10-bit packed - variant 1.")]
            RGB10V1Packed,

            [Description("Red-Green-Blue 12-bit unpacked.")]
            RGB12,

            [Description("Red-Green-Blue 12-bit unpacked planar.")]
            RGB12_Planar,

            [Description("Red-Green-Blue 12-bit packed.")]
            RGB12p,

            [Description("Red-Green-Blue 12-bit packed - variant 1.")]
            RGB12V1Packed,

            [Description("Red-Green-Blue 14-bit unpacked.")]
            RGB14,

            [Description("Red-Green-Blue 16-bit.")]
            RGB16,

            [Description("Red-Green-Blue 16-bit planar.")]
            RGB16_Planar,

            [Description("Red-Green-Blue 5/6/5-bit packed.")]
            RGB565p,

            [Description("Red-Green-Blue 8-bit.")]
            RGB8,

            [Description("Red-Green-Blue 8-bit planar.")]
            RGB8_Planar,

            [Description("RGB8a32.")]
            RGB8a32,

            [Description("Red-Green-Blue-alpha 10-bit unpacked.")]
            RGBa10,

            [Description("Red-Green-Blue-alpha 10-bit packed.")]
            RGBa10p,

            [Description("Red-Green-Blue-alpha 12-bit unpacked.")]
            RGBa12,

            [Description("Red-Green-Blue-alpha 12-bit packed.")]
            RGBa12p,

            [Description("Red-Green-Blue-alpha 14-bit unpacked.")]
            RGBa14,

            [Description("Red-Green-Blue-alpha 16-bit.")]
            RGBa16,

            [Description("Red-Green-Blue-alpha 8-bit.")]
            RGBa8,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 10-bit unpacked.")]
            SCF1WBWG10,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 10-bit packed.")]
            SCF1WBWG10p,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 12-bit unpacked.")]
            SCF1WBWG12,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 12-bit packed.")]
            SCF1WBWG12p,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 14-bit unpacked.")]
            SCF1WBWG14,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 16-bit unpacked.")]
            SCF1WBWG16,

            [Description("Sparse Color Filter #1 White-Blue-White-Green 8-bit.")]
            SCF1WBWG8,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 10-bit unpacked.")]
            SCF1WGWB10,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 10-bit packed.")]
            SCF1WGWB10p,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 12-bit unpacked.")]
            SCF1WGWB12,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 12-bit packed.")]
            SCF1WGWB12p,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 14-bit unpacked.")]
            SCF1WGWB14,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 16-bit.")]
            SCF1WGWB16,

            [Description("Sparse Color Filter #1 White-Green-White-Blue 8-bit.")]
            SCF1WGWB8,

            [Description("Sparse Color Filter #1 White-Green-White-Red 10-bit unpacked.")]
            SCF1WGWR10,

            [Description("Sparse Color Filter #1 White-Green-White-Red 10-bit packed.")]
            SCF1WGWR10p,

            [Description("Sparse Color Filter #1 White-Green-White-Red 12-bit unpacked.")]
            SCF1WGWR12,

            [Description("Sparse Color Filter #1 White-Green-White-Red 12-bit packed.")]
            SCF1WGWR12p,

            [Description("Sparse Color Filter #1 White-Green-White-Red 14-bit unpacked.")]
            SCF1WGWR14,

            [Description("Sparse Color Filter #1 White-Green-White-Red 16-bit.")]
            SCF1WGWR16,

            [Description("Sparse Color Filter #1 White-Green-White-Red 8-bit.")]
            SCF1WGWR8,

            [Description("Sparse Color Filter #1 White-Red-White-Green 10-bit unpacked.")]
            SCF1WRWG10,

            [Description("Sparse Color Filter #1 White-Red-White-Green 10-bit packed.")]
            SCF1WRWG10p,

            [Description("Sparse Color Filter #1 White-Red-White-Green 12-bit unpacked.")]
            SCF1WRWG12,

            [Description("Sparse Color Filter #1 White-Red-White-Green 12-bit packed.")]
            SCF1WRWG12p,

            [Description("Sparse Color Filter #1 White-Red-White-Green 14-bit unpacked.")]
            SCF1WRWG14,

            [Description("Sparse Color Filter #1 White-Red-White-Green 16-bit.")]
            SCF1WRWG16,

            [Description("Sparse Color Filter #1 White-Red-White-Green 8-bit.")]
            SCF1WRWG8,

            [Description("YCbCr 444 10-bit unpacked.")]
            YCbCr10_CbYCr,

            [Description("YCbCr 444 10-bit packed.")]
            YCbCr10p_CbYCr,

            [Description("YCbCr 444 12-bit unpacked.")]
            YCbCr12_CbYCr,

            [Description("YCbCr 444 12-bit packed.")]
            YCbCr12p_CbYCr,

            [Description("YCbCr 444 10-bit unpacked BT.2020.")]
            YCbCr2020_10_CbYCr,

            [Description("YCbCr 444 10-bit packed BT.2020.")]
            YCbCr2020_10p_CbYCr,

            [Description("YCbCr 444 12-bit unpacked BT.2020.")]
            YCbCr2020_12_CbYCr,

            [Description("YCbCr 444 12-bit packed BT.2020.")]
            YCbCr2020_12p_CbYCr,

            [Description("YCbCr 411 8-bit BT.2020.")]
            YCbCr2020_411_8_CbYYCrYY,

            [Description("YCbCr 422 10-bit unpacked BT.2020.")]
            YCbCr2020_422_10,

            [Description("YCbCr 422 10-bit unpacked BT.2020.")]
            YCbCr2020_422_10_CbYCrY,

            [Description("YCbCr 422 10-bit packed BT.2020.")]
            YCbCr2020_422_10p,

            [Description("YCbCr 422 10-bit packed BT.2020.")]
            YCbCr2020_422_10p_CbYCrY,

            [Description("YCbCr 422 12-bit unpacked BT.2020.")]
            YCbCr2020_422_12,

            [Description("YCbCr 422 12-bit unpacked BT.2020.")]
            YCbCr2020_422_12_CbYCrY,

            [Description("YCbCr 422 12-bit packed BT.2020.")]
            YCbCr2020_422_12p,

            [Description("YCbCr 422 12-bit packed BT.2020.")]
            YCbCr2020_422_12p_CbYCrY,

            [Description("YCbCr 422 8-bit BT.2020.")]
            YCbCr2020_422_8,

            [Description("YCbCr 422 8-bit BT.2020.")]
            YCbCr2020_422_8_CbYCrY,

            [Description("YCbCr 444 8-bit BT.2020.")]
            YCbCr2020_8_CbYCr,

            [Description("YCbCr 411 8-bit.")]
            YCbCr411_8,

            [Description("YCbCr 411 8-bit.")]
            YCbCr411_8_CbYYCrYY,

            [Description("YCbCr 420 8-bit YY/CbCr Semiplanar.")]
            YCbCr420_8_YY_CbCr_Semiplanar,

            [Description("YCbCr 420 8-bit YY/CrCb Semiplanar.")]
            YCbCr420_8_YY_CrCb_Semiplanar,

            [Description("YCbCr 422 10-bit unpacked.")]
            YCbCr422_10,

            [Description("YCbCr 422 10-bit unpacked.")]
            YCbCr422_10_CbYCrY,

            [Description("YCbCr 422 10-bit packed.")]
            YCbCr422_10p,

            [Description("YCbCr 422 10-bit packed.")]
            YCbCr422_10p_CbYCrY,

            [Description("YCbCr 422 12-bit unpacked.")]
            YCbCr422_12,

            [Description("YCbCr 422 12-bit unpacked.")]
            YCbCr422_12_CbYCrY,

            [Description("YCbCr 422 12-bit packed.")]
            YCbCr422_12p,

            [Description("YCbCr 422 12-bit packed.")]
            YCbCr422_12p_CbYCrY,

            [Description("YCbCr 422 8-bit.")]
            YCbCr422_8,

            [Description("YCbCr 422 8-bit.")]
            YCbCr422_8_CbYCrY,

            [Description("YCbCr 422 8-bit YY/CbCr Semiplanar.")]
            YCbCr422_8_YY_CbCr_Semiplanar,

            [Description("YCbCr 422 8-bit YY/CrCb Semiplanar.")]
            YCbCr422_8_YY_CrCb_Semiplanar,

            [Description("YCbCr 444 10-bit unpacked BT.601.")]
            YCbCr601_10_CbYCr,

            [Description("YCbCr 444 10-bit packed BT.601.")]
            YCbCr601_10p_CbYCr,

            [Description("YCbCr 444 12-bit unpacked BT.601.")]
            YCbCr601_12_CbYCr,

            [Description("YCbCr 444 12-bit packed BT.601.")]
            YCbCr601_12p_CbYCr,

            [Description("YCbCr 411 8-bit BT.601.")]
            YCbCr601_411_8_CbYYCrYY,

            [Description("YCbCr 422 10-bit unpacked BT.601.")]
            YCbCr601_422_10,

            [Description("YCbCr 422 10-bit unpacked BT.601.")]
            YCbCr601_422_10_CbYCrY,

            [Description("YCbCr 422 10-bit packed BT.601.")]
            YCbCr601_422_10p,

            [Description("YCbCr 422 10-bit packed BT.601.")]
            YCbCr601_422_10p_CbYCrY,

            [Description("YCbCr 422 12-bit unpacked BT.601.")]
            YCbCr601_422_12,

            [Description("YCbCr 422 12-bit unpacked BT.601.")]
            YCbCr601_422_12_CbYCrY,

            [Description("YCbCr 422 12-bit packed BT.601.")]
            YCbCr601_422_12p,

            [Description("YCbCr 422 12-bit packed BT.601.")]
            YCbCr601_422_12p_CbYCrY,

            [Description("YCbCr 422 8-bit BT.601.")]
            YCbCr601_422_8,

            [Description("YCbCr 422 8-bit BT.601.")]
            YCbCr601_422_8_CbYCrY,

            [Description("YCbCr 444 8-bit BT.601.")]
            YCbCr601_8_CbYCr,

            [Description("YCbCr 444 10-bit unpacked BT.709.")]
            YCbCr709_10_CbYCr,

            [Description("YCbCr 444 10-bit packed BT.709.")]
            YCbCr709_10p_CbYCr,

            [Description("YCbCr 444 12-bit unpacked BT.709.")]
            YCbCr709_12_CbYCr,

            [Description("YCbCr 444 12-bit packed BT.709.")]
            YCbCr709_12p_CbYCr,

            [Description("YCbCr 411 8-bit BT.709.")]
            YCbCr709_411_8_CbYYCrYY,

            [Description("YCbCr 422 10-bit unpacked BT.709.")]
            YCbCr709_422_10,

            [Description("YCbCr 422 10-bit unpacked BT.709.")]
            YCbCr709_422_10_CbYCrY,

            [Description("YCbCr 422 10-bit packed BT.709.")]
            YCbCr709_422_10p,

            [Description("YCbCr 422 10-bit packed BT.709.")]
            YCbCr709_422_10p_CbYCrY,

            [Description("YCbCr 422 12-bit unpacked BT.709.")]
            YCbCr709_422_12,

            [Description("YCbCr 422 12-bit unpacked BT.709.")]
            YCbCr709_422_12_CbYCrY,

            [Description("YCbCr 422 12-bit packed BT.709.")]
            YCbCr709_422_12p,

            [Description("YCbCr 422 12-bit packed BT.709.")]
            YCbCr709_422_12p_CbYCrY,

            [Description("YCbCr 422 8-bit BT.709.")]
            YCbCr709_422_8,

            [Description("YCbCr 422 8-bit BT.709.")]
            YCbCr709_422_8_CbYCrY,

            [Description("YCbCr 444 8-bit BT.709.")]
            YCbCr709_8_CbYCr,

            [Description("YCbCr 444 8-bit.")]
            YCbCr8,

            [Description("YCbCr 444 8-bit.")]
            YCbCr8_CbYCr,

            [Description("YUV 411 8-bit.")]
            YUV411_8_UYYVYY,

            [Description("YUV 422 8-bit.")]
            YUV422_8,

            [Description("YUV 422 8-bit.")]
            YUV422_8_UYVY,

            [Description("YUV 444 8-bit.")]
            YUV8_UYV,
        }

        [Serializable]
        public enum PixelFormatNamespace
        {
            [Description("Unknown.")]
            Unknown,

            [Description("GEV.")]
            GEV,

            [Description("IIDC.")]
            IIDC,

            [Description("PFNC_16BIT.")]
            PFNC_16BIT,

            [Description("PFNC_32BIT.")]
            PFNC_32BIT,
        }

        [Serializable]
        public enum UnpackingMode
        {
            [Description("Unpacking to lsb.")]
            Lsb,

            [Description("Unpacking to msb.")]
            Msb,

            [Description("No unpacking.")]
            Off,
        }

        [Serializable]
        public enum ImageScaling
        {
            [Description("No image scaling.")]
            Off,
            [Description("1:8 image down-scaling.")]
            Scaling_1_8,
        }

        [Serializable]
        public enum LUTConfiguration
        {
            [Description("Monochrome 8-bit to 8-bit.")]
            M_8x8,
            [Description("Monochrome 10-bit to 8-bit.")]
            M_10x8,
            [Description("Monochrome 10-bit to 10-bit.")]
            M_10x10,
            [Description("Monochrome 10-bit to 16-bit.")]
            M_10x16,
            [Description("Monochrome 12-bit to 8-bit.")]
            M_12x8,
            [Description("Monochrome 12-bit to 12-bit.")]
            M_12x12,
            [Description("Monochrome 12-bit to 16-bit.")]
            M_12x16,
        }

        [Serializable]
        public enum LUTSet
        {
            [Description("Select LUT set 1 for access.")]
            Set1,

            [Description("Select LUT set 2 for access.")]
            Set2,

            [Description("Select LUT set 3 for access.")]
            Set3,

            [Description("Select LUT set 4 for access.")]
            Set4,

            [Description("Select LUT set 5 for access.")]
            Set5,

            [Description("Select LUT set 6 for access.")]
            Set6,

            [Description("Select LUT set 7 for access.")]
            Set7,

            [Description("Select LUT set 8 for access.")]
            Set8,

            [Description("Select LUT set 9 for access.")]
            Set9,

            [Description("Select LUT set 10 for access.")]
            Set10,

            [Description("Select LUT set 11 for access.")]
            Set11,

            [Description("Select LUT set 12 for access.")]
            Set12,

            [Description("Select LUT set 13 for access.")]
            Set13,

            [Description("Select LUT set 14 for access.")]
            Set14,

            [Description("Select LUT set 15 for access.")]
            Set15,

            [Description("Select LUT set 16 for access.")]
            Set16,
        }

        [Serializable]
        public enum LUTEnable
        {
            Off,

            [Description("Enables the LUT processor with LUT set 1.")]
            Set1,

            [Description("Enables the LUT processor with LUT set 2.")]
            Set2,

            [Description("Enables the LUT processor with LUT set 3.")]
            Set3,

            [Description("Enables the LUT processor with LUT set 4.")]
            Set4,

            [Description("Enables the LUT processor with LUT set 5.")]
            Set5,

            [Description("Enables the LUT processor with LUT set 6.")]
            Set6,

            [Description("Enables the LUT processor with LUT set 7.")]
            Set7,

            [Description("Enables the LUT processor with LUT set 8.")]
            Set8,

            [Description("Enables the LUT processor with LUT set 9.")]
            Set9,

            [Description("Enables the LUT processor with LUT set 10.")]
            Set10,

            [Description("Enables the LUT processor with LUT set 11.")]
            Set11,

            [Description("Enables the LUT processor with LUT set 12.")]
            Set12,

            [Description("Enables the LUT processor with LUT set 13.")]
            Set13,

            [Description("Enables the LUT processor with LUT set 14.")]
            Set14,

            [Description("Enables the LUT processor with LUT set 15.")]
            Set15,

            [Description("Enables the LUT processor with LUT set 16.")]
            Set16,
        }

        [Serializable]
        public enum StreamBufferHandlingMode
        {
            [Description("Default Buffer Handling Mode.")]
            Default,
        }

        [Serializable]
        public enum StreamAcquisitionModeSelector
        {
            [Description("Default Buffer Handling Mode.")]
            Default,
        }

        [Serializable]
        public enum StartOfScanTriggerSource
        {
            [Description("Immediate")]
            Immediate,

            [Description("StartScan command.")]
            StartScan,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StartScan command.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2 or on execution of the StartScan command.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3 or on execution of the StartScan command.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4 or on execution of the StartScan command.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5 or on execution of the StartScan command.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6 or on execution of the StartScan command.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7 or on execution of the StartScan command.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8 or on execution of the StartScan command.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1 or on execution of the StartScan command.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2 or on execution of the StartScan command.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3 or on execution of the StartScan command.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4 or on execution of the StartScan command.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1 or on execution of the StartScan command.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2 or on execution of the StartScan command.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3 or on execution of the StartScan command.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4 or on execution of the StartScan command.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1 or on execution of the StartScan command.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2 or on execution of the StartScan command.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3 or on execution of the StartScan command.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4 or on execution of the StartScan command.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1 or on execution of the StartScan command.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2 or on execution of the StartScan command.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1 or on execution of the StartScan command.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2 or on execution of the StartScan command.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1 or on execution of the StartScan command.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2 or on execution of the StartScan command.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1 or on execution of the StartScan command.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2 or on execution of the StartScan command.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1 or on execution of the StartScan command.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2 or on execution of the StartScan command.")]
            EIN2,

            [Description("When an event occurs on User Event 1 or on execution of the StartScan command.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2 or on execution of the StartScan command.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3 or on execution of the StartScan command.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4 or on execution of the StartScan command.")]
            UserEvent4,
        }

        [Serializable]
        public enum EndOfScanTriggerSource
        {
            [Description("ScanLength")]
            ScanLength,

            [Description("StopScan command.")]
            StopScan,

            [Description("When an event occurs on Line Input Tool 1 or on execution of the StopScan  command.")]
            LIN1,

            [Description("When an event occurs on Line Input Tool 2 or on execution of the StopScan  command.")]
            LIN2,

            [Description("When an event occurs on Line Input Tool 3 or on execution of the StopScan  command.")]
            LIN3,

            [Description("When an event occurs on Line Input Tool 4 or on execution of the StopScan  command.")]
            LIN4,

            [Description("When an event occurs on Line Input Tool 5 or on execution of the StopScan  command.")]
            LIN5,

            [Description("When an event occurs on Line Input Tool 6 or on execution of the StopScan  command.")]
            LIN6,

            [Description("When an event occurs on Line Input Tool 7 or on execution of the StopScan  command.")]
            LIN7,

            [Description("When an event occurs on Line Input Tool 8 or on execution of the StopScan  command.")]
            LIN8,

            [Description("When an event occurs on Quadrature Decoder Tool 1 or on execution of the StopScan  command.")]
            QDC1,

            [Description("When an event occurs on Quadrature Decoder Tool 2 or on execution of the StopScan  command.")]
            QDC2,

            [Description("When an event occurs on Quadrature Decoder Tool 3 or on execution of the StopScan  command.")]
            QDC3,

            [Description("When an event occurs on Quadrature Decoder Tool 4 or on execution of the StopScan  command.")]
            QDC4,

            [Description("When an event occurs on Multiplier/Divider Tool 1 or on execution of the StopScan  command.")]
            MDV1,

            [Description("When an event occurs on Multiplier/Divider Tool 2 or on execution of the StopScan  command.")]
            MDV2,

            [Description("When an event occurs on Multiplier/Divider Tool 3 or on execution of the StopScan  command.")]
            MDV3,

            [Description("When an event occurs on Multiplier/Divider Tool 4 or on execution of the StopScan  command.")]
            MDV4,

            [Description("When an event occurs on Divider Tool 1 or on execution of the StopScan  command.")]
            DIV1,

            [Description("When an event occurs on Divider Tool 2 or on execution of the StopScan  command.")]
            DIV2,

            [Description("When an event occurs on Divider Tool 3 or on execution of the StopScan  command.")]
            DIV3,

            [Description("When an event occurs on Divider Tool 4 or on execution of the StopScan  command.")]
            DIV4,

            [Description("When an event occurs on Delay Tool 1 Output 1 or on execution of the StopScan  command.")]
            DEL1_1,

            [Description("When an event occurs on Delay Tool 1 Output 2 or on execution of the StopScan  command.")]
            DEL1_2,

            [Description("When an event occurs on Delay Tool 2 Output 1 or on execution of the StopScan  command.")]
            DEL2_1,

            [Description("When an event occurs on Delay Tool 2 Output 2 or on execution of the StopScan  command.")]
            DEL2_2,

            [Description("When an event occurs on Delay Tool 3 Output 1 or on execution of the StopScan  command.")]
            DEL3_1,

            [Description("When an event occurs on Delay Tool 3 Output 2 or on execution of the StopScan  command.")]
            DEL3_2,

            [Description("When an event occurs on Delay Tool 4 Output 1 or on execution of the StopScan  command.")]
            DEL4_1,

            [Description("When an event occurs on Delay Tool 4 Output 2 or on execution of the StopScan  command.")]
            DEL4_2,

            [Description("When an event occurs on Event Input Tool 1 or on execution of the StopScan  command.")]
            EIN1,

            [Description("When an event occurs on Event Input Tool 2 or on execution of the StopScan  command.")]
            EIN2,

            [Description("When an event occurs on User Event 1 or on execution of the StopScan  command.")]
            UserEvent1,

            [Description("When an event occurs on User Event 2 or on execution of the StopScan  command.")]
            UserEvent2,

            [Description("When an event occurs on User Event 3 or on execution of the StopScan  command.")]
            UserEvent3,

            [Description("When an event occurs on User Event 4 or on execution of the StopScan  command.")]
            UserEvent4,
        }

        [Serializable]
        public enum DmaEngineOptimization
        {
            [Description("DMA operations are optimized for low latency and maximum PCIe throughput.")]
            Default,

            [Description("DMA operations are optimized for low memory usage; this may lead to higher latency and reduced PCIe throughput.")]
            LowMemoryUsage,
        }

        [Serializable]
        public enum StripeArrangement
        {
            [Description("Regular (top-down) image.")]
            Geometry_1X_1Y,

            [Description("Vertically flipped (bottom-up) image.")]
            Geometry_1X_1YE,

            [Description("2 taps arranged top-down and bottom-up.")]
            Geometry_1X_2YE,

            [Description("2 taps arranged middle-up and middle-down.")]
            Geometry_1X_2YM,
        }

        [Serializable]
        public enum StatisticsSamplingSelector
        {
            [Description("During the last second.")]
            LastSecond,

            [Description("During the last 10 seconds.")]
            LastTenSeconds,

            [Description("For the last 2 buffers.")]
            Last2Buffers,

            [Description("For the last 10 buffers.")]
            Last10Buffers,

            [Description("For the last 100 buffers.")]
            Last100Buffers,

            [Description("For the last 1000 buffers.")]
            Last1000Buffers,

            [Description("During the last acquisition activity period. Namely since the last DSStartAcquisition() function call until now, if the acquisition is still active otherwise until the last DSStopAcquisition() function call.")]
            LastAcquisition,

            [Description("Custom sampling using StatisticsStartSampling and StatisticsStopSampling commands.")]
            Custom,
        }

        [Serializable]
        public enum LinearFilterControl
        {
            [Description("Disable")]
            Disable,

            [Description("Enable")]
            Enable,
        }

        [Serializable]
        public enum ThresholdControl
        {
            [Description("Disable")]
            Disable,

            [Description("Enable")]
            Enable,
        }

        [Serializable]
        public enum Scan3dExtractionMethod
        {
            [Description("Disable extraction")]
            Disable,

            [Description("Maximum detection, 8-bit integer coordinates.")]
            MaxDetection_8,

            [Description("Maximum detection, 16-bit integer coordinates.")]
            MaxDetection_16,

            [Description("Peak detection, UQ11.5 fixed-point coordinates (fx11.16).")]
            PeakDetection_11_5,

            [Description("Peak detection, UQ8.8 fixed-point coordinates (fx8.16).")]
            PeakDetection_8_8,

            [Description("Center of gravity, UQ11.5 fixed-point coordinates (fx11.16).")]
            CenterOfGravity_11_5,

            [Description("Center of gravity, UQ8.8 fixed-point coordinates (fx8.16).")]
            CenterOfGravity_8_8,
        }

        [Serializable]
        public enum Scan3dOutputMode
        {
            [Description("Uncalibrated 2.5D Depth map.")]
            UncalibratedC,
        }

        [Serializable]
        public enum BayerMethod
        {
            [Description("Disable.")]
            Disable,

            [Description("Legacy.")]
            Legacy,

            [Description("Advanced.")]
            Advanced,
        }

        [Serializable]
        public enum FfcControl
        {
            [Description("Disable.")]
            Disable,

            [Description("Enable.")]
            Enable,
        }

        [Serializable]
        public enum FfcBypass
        {
            [Description("Disable")]
            Disable,

            [Description("Enable")]
            Enable,
        }
        #endregion

        #region GetParameter
        [Serializable]
        public enum GetParameter
        {

            InterfaceInformation,
            DeviceEnumeration,
            CoaXPress,
            CoaXPressAdvanced,
            DigitalIOControl,
            IOExtensionModule,
            UserOutputRegister,
            IOToolbox,
            PCIExpress,
            InterfaceControl,
            InterfaceDetails,
            EventControl,
            OemSafetyKey,
            CustomLogic,
            OnboardMemory,
            InterfaceID,
            InterfaceType,
            ProductCode,
            SerialNumber,
            PartNumber,
            FirmwareRevision,
            FirmwareVariant,
            FirmwareStatus,
            FirmwareRecoverySwitch,
            DeviceUpdateList,
            DeviceSelector,
            DeviceID,
            DeviceVendorName,
            DeviceModelName,
            DeviceAccessStatus,
            CxpPoCxpHostConnectionSelector,
            CxpPoCxpConfigurationStatus,
            CxpPoCxpStatus,
            CxpPoCxpCurrent,
            CxpPoCxpVoltage,
            CxpPoCxpPowerInputStatus,
            CxpHostConnectionCount,
            CxpHostConnectionSelector,
            CxpConnectionState,
            CxpDownConnectionSpeed,
            CxpDeviceConnectionID,
            CXP1Supported,
            CXP2Supported,
            CXP3Supported,
            CXP5Supported,
            CXP6Supported,
            CXP10Supported,
            CXP12Supported,
            CxpHostConnectionTestMode,
            CxpHostConnectionTestErrorCount,
            CxpHostConnectionTestPacketCount,
            CxpHostConnectionTestInjectError,
            CxpRevisionSelector,
            CxpRevisionSupport,
            ShowCoaXPressAdvancedFeatures,
            CxpRateMask,
            CxpRateMaskCXP1,
            CxpRateMaskCXP2,
            CxpRateMaskCXP3,
            CxpRateMaskCXP5,
            CxpRateMaskCXP6,
            CxpRateMaskCXP10,
            CxpRateMaskCXP12,
            CxpDiscoveryTimingSelector,
            CxpDiscoveryTiming,
            CxpControlParameterSelector,
            CxpControlParameter,
            LineSelector,
            LineFormat,
            LineMode,
            LineInverter,
            LineFilterStrength,
            LineFilterDelay,
            LineStatus,
            LineStatusAll,
            LineSource,
            IOExtensionModuleLineSelector,
            IOExtensionModuleLineFormat,
            IOExtensionModuleLineMode,
            IOExtensionModuleLineStatus,
            IOExtensionModuleLineToRepair,
            IOExtensionModuleErrorCount,
            IOExtensionModuleInformation,
            IOExtensionModuleProductCode,
            IOExtensionModuleSerialNumber,
            IOExtensionModulePartNumber,
            IOExtensionModuleRevision,
            IOExtensionModuleVariant,
            UserOutputValueAll,
            UserActions,
            UserActionsSchedulerReference,
            ScheduledUserActionsPoolStatus,
            LineInputToolSelector,
            LineInputToolSource,
            LineInputToolActivation,
            MultiplierDividerToolSelector,
            MultiplierDividerToolSource,
            MultiplierDividerToolOutputControl,
            MultiplierDividerToolMultiplicationFactor,
            MultiplierDividerToolDivisionFactor,
            MultiplierDividerToolEffectiveMultiplicationFactor,
            MultiplierDividerToolEffectiveDivisionFactor,
            QuadratureDecoderToolSelector,
            QuadratureDecoderToolSources,
            QuadratureDecoderToolActivation,
            QuadratureDecoderToolForwardDirection,
            QuadratureDecoderToolOutputMode,
            QuadratureDecoderToolPosition,
            QuadratureDecoderToolDirection,
            DividerToolSelector,
            DividerToolSource,
            DividerToolEnableControl,
            DividerToolDivisionFactor,
            DividerToolInitialOffset,
            DelayToolSelector,
            DelayToolSource1,
            DelayToolSource2,
            DelayToolClockSource,
            DelayToolDelayValue,
            EventInputToolSelector,
            EventInputToolSource,
            EventInputToolActivation,
            InternalTime,
            PCIeMaxPayloadSizeSupported,
            PCIeMaxPayloadSize,
            PCIeMaxReadRequestSize,
            PCIeMaxLinkSpeed,
            PCIeCurrentLinkSpeed,
            PCIeMaximumLinkWidth,
            PCIeNegotiatedLinkWidth,
            PCIeLinkSpeed2500MTpsSupported,
            PCIeLinkSpeed5000MTpsSupported,
            PCIeLinkSpeed8000MTpsSupported,
            FanStatus,
            TemperatureSensorSelector,
            Temperature,
            AuxiliaryPowerInput,
            AuxiliaryPower12VInput,
            BoardCapabilities,
            FirmwareBoardID,
            CPLDRevision,
            PreviousBootBank,
            NextBootBank,
            CurrentBankSelect,
            CurrentBankSelectReadback,
            NextBankSelect,
            SpiBankStatus,
            PotBankStatus,
            EventSelector,
            EventNotification,
            EventNotificationContext1,
            EventNotificationContext2,
            EventNotificationContext3,
            EventCount,
            OemSafetyKeyVerification,
            EncryptedOemSafetyKey,
            MaximumOemKeyLength,
            CustomLogicControlAddress,
            CustomLogicControlData,
            OnboardMemoryBase,
            OnboardMemorySize,
            DeviceInformation,
            StreamEnumeration,
            CameraAndIlluminationControl,
            Errors,
            DeviceType,
            StreamSelector,
            StreamID,
            CxpLinkConfiguration,
            CxpLinkConfigurationOption,
            CxpHostConnectionBase,
            CxpTriggerMessageFormat,
            CxpTriggerLevel,
            CxpTriggerAckTimeout,
            CxpTriggerMaxResendCount,
            CxpPacketArbiterReset,
            CxpPortAlignment,
            CameraModel,
            CycleTiming,
            CycleControl,
            SequenceControl,
            DeviceReset,
            CameraAndIlluminationControllerStream,
            CameraControlMethod,
            C2CLinkConfiguration,
            ExposureReadoutOverlap,
            ExposureRecoveryTime,
            ExposureTimeMin,
            ExposureTimeMax,
            CycleMinimumPeriod,
            StrobeDelay,
            StrobeDuration,
            CycleTriggerSource,
            StartCycle,
            CycleMaxPendingTriggerCount,
            CyclePendingTriggerCount,
            CycleLostTriggerCount,
            CycleLostTriggerCountReset,
            StartOfSequenceTriggerSource,
            EndOfSequenceTriggerSource,
            SequenceLength,
            StartSequence,
            StopSequence,
            AbortSequence,
            ErrorSelector,
            ErrorCount,
            StreamInformation,
            ImageFormatControl,
            PixelProcessing,
            LUTControl,
            TransportLayerControl,
            BufferHandlingControl,
            LineScanAcquisitionControl,
            StreamControl,
            StreamStatistics,
            LinearFilter,
            Threshold,
            LaserLineExtractor,
            Bayer,
            FlatFieldCorrection,
            StreamType,
            PixelFormat,
            PixelFormatNamespace,
            PixelSize,
            PixelComponentCount,
            UnpackingMode,
            RedBlueSwap,
            ImageScaling,
            JpegQuality,
            LUTConfiguration,
            LUTLength,
            LUTMaxValue,
            LUTSet,
            LUTIndex,
            LUTValue,
            LUTReadBlockLength,
            LUTEnable,
            PayloadSize,
            StreamAnnouncedBufferCount,
            StreamBufferHandlingMode,
            StreamAnnounceBufferMinimum,
            StreamAcquisitionModeSelector,
            StartOfScanTriggerSource,
            EndOfScanTriggerSource,
            ScanLength,
            BufferHeight,
            StartScan,
            StopScan,
            StreamReset,
            DmaEngineOptimization,
            LineWidth,
            LinePitch,
            StripeHeight,
            StripePitch,
            BlockHeight,
            StripeOffset,
            StripeArrangement,
            SyncMarker,
            SyncMarkerBusAddress,
            SyncMarkerValue,
            SyncMarkerValueIncrement,
            ErrorCountReset,
            StatisticsSamplingSelector,
            StatisticsFrameRate,
            StatisticsLineRate,
            StatisticsDataRate,
            StatisticsStartSampling,
            StatisticsStopSampling,
            LinearFilterControl,
            LinearFilterCoefficientA,
            LinearFilterCoefficientB,
            LinearFilterCoefficientC,
            ThresholdControl,
            ThresholdLevel,
            Scan3dExtractionMethod,
            Scan3dOutputMode,
            Scan3dSecondLineROIOffsetY,
            BayerMethod,
            FfcCoefficientPartitionBase,
            FfcCoefficientPartitionSize,
            FfcControl,
            FfcBypass,
            FfcCoefficientsValid,
            SystemInformation,
            InterfaceEnumeration,
            TLVendorName,
            TLModelName,
            TLID,
            TLVersion,
            TLPath,
            TLType,
            GenTLVersionMajor,
            GenTLVersionMinor,
            InterfaceUpdateList,
            InterfaceSelector,
            ExposureTime,
            AcquisitionFrameRate,
            C2C,
            Width,
            Height,

        }
        #endregion

        #region Method
        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetInterfaceParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringInterfaceModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetDeviceParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringDeviceModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetDeviceParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringDeviceModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetDeviceParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringDeviceModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetRemoteParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringRemoteModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetRemoteParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringRemoteModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetRemoteParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringRemoteModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetStreamParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringStreamModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetStreamParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringStreamModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetStreamParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringStreamModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetDeviceParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatDeviceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetDeviceParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatDeviceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetDeviceParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatDeviceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetRemoteParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatRemoteModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetRemoteParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatRemoteModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetRemoteParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatRemoteModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetRemoteParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerRemoteModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetRemoteParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerRemoteModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetRemoteParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerRemoteModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetInterfaceParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringInterfaceModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetInterfaceParameter parameter, Enum value)
        {
            int ret = 0;

            grabber.setStringInterfaceModule(parameter.ToString(), value.ToString());

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetInterfaceParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatInterfaceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetInterfaceParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatInterfaceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetInterfaceParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatInterfaceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetInterfaceParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerInterfaceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetInterfaceParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerInterfaceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetInterfaceParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerInterfaceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetDeviceParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerDeviceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetDeviceParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerDeviceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetDeviceParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerDeviceModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetStreamParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatStreamModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetStreamParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatStreamModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetStreamParameter parameter, double value)
        {
            int ret = 0;

            grabber.setFloatStreamModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackOnDemand grabber, GenICam.SetStreamParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerStreamModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackSingleThread grabber, GenICam.SetStreamParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerStreamModule(parameter.ToString(), value);

            return ret;
        }

        internal static int SetParameter(Euresys.EGrabberCallbackMultiThread grabber, GenICam.SetStreamParameter parameter, int value)
        {
            int ret = 0;

            grabber.setIntegerStreamModule(parameter.ToString(), value);

            return ret;
        }
        #endregion

        #region ExecuteCommand

        [Serializable]
        public enum ExecuteInterfaceCommand
        {
            DeviceUpdateList,
            CxpPoCxpAuto,
            CxpPoCxpTurnOff,
            CxpPoCxpTripReset,
            CxpHostConnectionTestInjectError,
            ClearUserActions,
            ExecuteUserActions,
            DiscardScheduledUserActions,
            QuadratureDecoderToolPositionReset,
            EventCountReset,
            EventCountResetAll,
        }

        [Serializable]
        public enum ExecuteDeviceCommand
        {
            [Description("CoaXPress 데이터 패킷 조정기 재설정.")]
            CxpPacketArbiterReset,

            [Description("CIC를 재설정합니다.")]
            DeviceReset,

            [Description("카메라 주기를 시작합니다. CIC를 사용하는 경우에만 적용됩니다(CameraControlMethod가 RC 또는 RG인 경우).")]
            StartCycle,

            [Description("손실된 CIC주기 트리거 이벤트 수를 다시 설정합니다. CIC를 사용하는 경우에만 적용됩니다 (CameraControlMethod가 RC 또는 RG인 경우).")]
            CycleLostTriggerCountReset,

            [Description("CIC 시퀀스를 시작합니다. CIC를 사용하는 경우(CameraControlMethod가 RC 또는 RG인 경우) 및 StartOfSequenceTriggerSource가 Immediate로 설정되지 않은 경우에만 적용됩니다.")]
            StartSequence,

            [Description("CIC 시퀀스를 중지합니다. CIC를 사용하는 경우에만 적용됩니다.(CameraControlMethod가 RC 또는 RG인 경우)")]
            StopSequence,

            [Description("CIC 시퀀스를 중단합니다.CIC를 사용하는 경우(CameraControlMethod가 RC 또는 RG인 경우) 및 StartOfSequenceTriggerSource가 Immediate로 설정되지 않은 경우에만 적용됩니다.")]
            AbortSequence,

            [Description("Reset the selected EventCount.")]
            EventCountReset,

            [Description("Reset the selected ErrorCount.")]
            ErrorCountReset,
        }

        [Serializable]
        public enum ExecuteStreamCommand
        {
            [Description("Starts a scan.")]
            StartScan,

            [Description("Stops a scan.")]
            StopScan,

            [Description("Stream Reset.")]
            StreamReset,

            [Description("Reset the selected ErrorCount.")]
            ErrorCountReset,

            [Description("Start sampling the stream data. Applies only when StatisticsSamplingSelector = Custom.")]
            StatisticsStartSampling,

            [Description("Stop sampling the stream data. Applies only when StatisticsSamplingSelector = Custom.")]
            StatisticsStopSampling,
        }

        [Serializable]
        public enum ExecuteRemoteCommand
        {
            [Description("RemotoeAcquisition")]
            AcquisitionStop,
        }

        #region Execute
        internal static int Execute(Euresys.EGrabberCallbackOnDemand grabber, GenICam.ExecuteInterfaceCommand command)
        {
            int ret = 0;

            grabber.executeInterfaceModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackSingleThread grabber, GenICam.ExecuteInterfaceCommand command)
        {
            int ret = 0;

            grabber.executeInterfaceModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackMultiThread grabber, GenICam.ExecuteInterfaceCommand command)
        {
            int ret = 0;

            grabber.executeInterfaceModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackOnDemand grabber, GenICam.ExecuteDeviceCommand command)
        {
            int ret = 0;

            grabber.executeDeviceModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackSingleThread grabber, GenICam.ExecuteDeviceCommand command)
        {
            int ret = 0;

            grabber.executeDeviceModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackMultiThread grabber, GenICam.ExecuteDeviceCommand command)
        {
            int ret = 0;

            grabber.executeDeviceModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackOnDemand grabber, GenICam.ExecuteStreamCommand command)
        {
            int ret = 0;

            grabber.executeStreamModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackSingleThread grabber, GenICam.ExecuteStreamCommand command)
        {
            int ret = 0;

            grabber.executeStreamModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackMultiThread grabber, GenICam.ExecuteStreamCommand command)
        {
            int ret = 0;

            grabber.executeStreamModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackOnDemand grabber, GenICam.ExecuteRemoteCommand command)
        {
            int ret = 0;

            grabber.executeRemoteModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackSingleThread grabber, GenICam.ExecuteRemoteCommand command)
        {
            int ret = 0;

            grabber.executeRemoteModule(command.ToString());

            return ret;
        }

        internal static int Execute(Euresys.EGrabberCallbackMultiThread grabber, GenICam.ExecuteRemoteCommand command)
        {
            int ret = 0;

            grabber.executeRemoteModule(command.ToString());

            return ret;
        }
        #endregion
        #endregion

        internal static readonly Euresys.GenTL Instance = new Euresys.GenTL();
    }
    #endregion
}
