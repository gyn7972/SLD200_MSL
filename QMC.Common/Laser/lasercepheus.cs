using SpiralLab.Sirius;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;

//namespace QMC.Core.Laser
namespace QMC.Common.Laser
{
    /// <summary>
    /// 레이저 Photon Laser (CEPHEUS:32bit) 소스
    /// </summary>
    //public class LaserCepheus : ILaser
    //{
    //    /// <summary>
    //    /// Sets the laser power in per mill (0 - 1000 ‰).
    //    /// 0, 200, 400, 600, 800, 1000 (values in between are clipped)
    //    /// 0 – 1000 (stepless, if system is equipped with attenuator option)
    //    /// </summary>
    //    public enum PowerParam
    //    {
    //        P_0 = 0,
    //        P_200 = 200,
    //        P_400 = 400,
    //        P_600 = 600,
    //        P_800 = 800,
    //        P_1000 = 1000,
    //    }

    //    //1. Repetition rate(50 / 100 / 150 / 200 kHz) / Gate Signal modulation
    //    //2. Burst internal repetition rate(21 / 32 / 65 MHz)
    //    //3. Number of pulses(2 – 10 pulses)
    //    //
    //    //          1. Repetition rate
    //    //      |<------------------------>|
    //    //
    //    //       _________________           _________________           _________________           _________________
    //    //      |  _   _   _   _  |         |  _   _   _   _  |         |  _   _   _   _  |         |  _   _   _   _  |
    //    //      | | | | | | | | | |         | | | | | | | | | |         | | | | | | | | | |         | | | | | | | | | |
    //    //      | | | | | | | | | |         | | | | | | | | | |         | | | | | | | | | |         | | | | | | | | | |
    //    //      | | | | | | | | | |         | | | | | | | | | |         | | | | | | | | | |         | | | | | | | | | |
    //    //______| | | | | | | | | |_________| | | | | | | | | |_________| | | | | | | | | |_________| | | | | | | | | |_________
    //    //        |<----------->|               ->|-|<-
    //    //         3. pulse count             2. Burst internal repetition rate

    //    /// <summary>
    //    /// Sets the laser repetition rate (20 - 500 kHz).
    //    /// 20, 50, 100, 150, 200, 250, 400, 500 (values in between are clipped) => standard frequency range other frequency settings optional available(see laser specification sheet)
    //    /// </summary>
    //    public enum RepRateParam
    //    {
    //        kHz_20 = 20,
    //        kHz_50 = 50,
    //        kHz_100 = 100,
    //        kHz_150 = 150,
    //        kHz_200 = 200,
    //        kHz_250 = 250,
    //        kHz_400 = 400,
    //        kHz_500 = 500,
    //    }

    //    /// <summary>
    //    /// Sets the burst internal repetition rate. Burst mode must be unlocked and activated!!
    //    /// 21, 32, 65 (MHz)
    //    /// </summary>
    //    public enum BinRepRateParam
    //    {
    //        MHz_21 = 21,
    //        MHz_32 = 32,
    //        MHz_65 = 65,
    //    }

    //    /// <summary>
    //    /// Sets the number of pulses in between one burst. Burst mode must be unlocked and activated!! 
    //    /// </summary>
    //    public enum PulsesParam
    //    {
    //        PULSE2 = 2,
    //        PULSE3,
    //        PULSE4,
    //        PULSE5,
    //        PULSE6,
    //        PULSE7,
    //        PULSE8,
    //        PULSE9,
    //        PULSE10,
    //    }

    //    [Flags]
    //    public enum Status : uint
    //    {
    //        Init = 1 << 0, //library and laser system is init
    //        ShutterOpen = 1 << 1, //the shutter open command has been called
    //        GlobalErr = 1 << 2, //a system error has occurred
    //        LaserReady = 1 << 3, //laser system is ready to arm
    //        GateSignalOn = 1 << 4, //gate signal is active
    //        LaserArm = 1 << 5, //laser system is armed
    //        BurstModeOn = 1 << 6, //Burst mode is active
    //    }

    //    [Flags]
    //    public enum ErrorStatus : uint
    //    {
    //        TEMP = 1 << 0, //temperature error / LSM board
    //        LD3 = 1 << 1, //general or temperature error in laser driver 3
    //        LD2 = 1 << 2, //general or temperature error in laser driver 2
    //        LD1 = 1 << 3, //general or temperature error in laser driver 1
    //        QSD = 1 << 4, //q-switching error has occurred
    //        PLL = 1 << 5, //laser system is out of synchronization
    //        SHUTTER_IL = 1 << 6, //shutter has an error
    //        MC_RESET = 1 << 7, //µController Reset happened
    //    }

    //    public enum TemperatureState
    //    {
    //        Warmup = 0,
    //        Normal,
    //        Warning,
    //        Knockout,
    //        Deactivated,
    //    }

    //    public enum FuncState
    //    {
    //        Success = 0,
    //        NotInit = 1,
    //        ComError = 2,
    //        UnknownError = 3,
    //        BoardConnectionError = 4,
    //        ValueOutOfRange = 6,
    //        FileNotFound = 7,
    //        BurstModeLocked = 8,
    //        BurstModeOff = 9,
    //        ParameterNotValid = 10,
    //        WarmUpRunning = 22,
    //    }

    //    private ChannelFactory<IMyContract> factory = new ChannelFactory<IMyContract>();
    //    private IMyContract channel;
    //    public int Index { get; private set; }
    //    public string Name { get; private set; }

    //    public IRtc Rtc { get; private set; }
    //    public float MaxPowerWatt { get; }
    //    public PowerXFactor PowerXFactor { get; set; }
    //    public bool IsReady
    //    {
    //        get
    //        {
    //            //if (true == GetState())
    //                return !this.IsError;
    //            //else
    //            //    return false;
    //        }
    //    }
    //    public bool IsBusy
    //    {
    //        get
    //        {
    //            return this.isbusy;
    //        }
    //    }
    //    private bool isbusy;
    //    public bool IsError { get; set; }

    //    public object Tag { get; set; }

    //    private bool disposed = false;

    //    public LaserCepheus(int index, string name, IRtc rtc, PowerXFactor powerXFactor, float maxPowerWatt)
    //    {
    //        this.Index = index;
    //        this.Name = name;
    //        this.Rtc = rtc;
    //        this.PowerXFactor = powerXFactor;
    //        this.MaxPowerWatt = maxPowerWatt;
    //    }
    //    ~LaserCepheus()
    //    {
    //        if (this.disposed)
    //            return;
    //        this.Dispose(false);
    //    }
    //    public void Dispose()
    //    {
    //        this.Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }
    //    private void Dispose(bool disposing)
    //    {
    //        if (this.disposed)
    //            return;

    //        if (disposing)
    //        {
    //            //if (channel != null)
    //            //{
    //            //    ((ICommunicationObject)channel).Close();
    //            //    channel.Terminate();
    //            //    // Close Channel
    //            //}
    //        }
    //        this.disposed = true;
    //    }

    //    public bool Initialize()
    //    {
    //        // Address
    //        string address = "net.tcp://localhost:8080/myAddress";
    //        factory.Endpoint.Address = new EndpointAddress(address);
    //        // Binding : TCP 사용
    //        factory.Endpoint.Binding = new NetTcpBinding();
    //        // Contract 설정
    //        factory.Endpoint.Contract.ContractType = typeof(IMyContract);
    //        // Channel Factory 만들기
    //        channel = factory.CreateChannel();

    //        //if (!CtlReset())
    //        //{
    //        //    channel.Terminate();
    //        //    channel = null;
    //        //    return false;
    //        //}

    //        return true;
    //    }

    //    [ServiceContract]
    //    public interface IMyContract
    //    {
    //        [OperationContract]
    //        int LaserOn();

    //        [OperationContract]
    //        int LaserOff();

    //        [OperationContract]
    //        int SetRepRate(int rate);

    //        [OperationContract]
    //        int SetPower(double watt);

    //        [OperationContract]
    //        int GetPower(out int power);

    //        [OperationContract]
    //        int GetState(out int status);

    //        [OperationContract]
    //        int GetTemperature(out double ld1Temp, out double ld2Temp, out double ld3Temp, out double paTemp);

    //        [OperationContract]
    //        int GetTemperatureState(out int ld1, out int ld2, out int ld3, out int pa);

    //        [OperationContract]
    //        int Init();

    //        [OperationContract]
    //        void Terminate();
    //    }

    //    public bool CtlReset()
    //    {
    //        int nRet = channel.Init();
    //        if ((int)FuncState.Success != nRet)
    //        {
    //            Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"Cepheus Dll open Fail!(Error Code:{nRet})");
    //            IsError = true;
    //            return false;
    //        }
    //        nRet = channel.SetRepRate((int)RepRateParam.kHz_20);
    //        if ((int)FuncState.Success != nRet)
    //        {
    //            Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"Cepheus SetRepRate Fail!(Error Code:{nRet})");
    //            IsError = true;
    //            return false;
    //        }
    //        IsError = false;
    //        return true;
    //    }

    //    /// <summary>
    //    /// 0~100%
    //    /// </summary>
    //    /// <param name="power"></param>
    //    /// <returns></returns>
    //    public bool CtlPower(double power)
    //    {
    //        if (this.IsBusy)
    //            return false;
    //        if (this.IsError)
    //            return false;

    //        int nRet = channel.SetPower((double)power * 10.0);
    //        if ((int)FuncState.Success != nRet)
    //        {
    //            Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"Cepheus SetPower Command Fail!(Error Code:{nRet})");
    //            return false;
    //        }

    //        Logger.Log(Logger.Module.Laser, Logger.Type.Warn, $"laser [{this.Index}]: set laser power to {power:F3}‰");
    //        return true;
    //    }

    //    public bool CtlShutterOpen()
    //    {
    //        bool success = true;
    //        return success;
    //    }

    //    public bool CtlShutterClose()
    //    {
    //        bool success = true;
    //        return success;
    //    }

    //    public bool CtlLaserShot()
    //    {
    //        return this.Rtc.CtlLaserOn();
    //    }

    //    public bool CtlLaserStop()
    //    {
    //        return this.Rtc.CtlLaserOff();
    //    }

    //    public bool CtlLaserOn()
    //    {
    //        bool success = true;
    //        if (GetState())
    //        {
    //            //if(!this.Rtc.CtlLaserOn())
    //            //    return false;

    //            int nRet = channel.LaserOn();
    //            if ((int)FuncState.Success != nRet)
    //            {
    //                Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"Cepheus LaserOn Command Fail!(Error Code:{nRet})");
    //                return false;
    //            }
    //        }
    //        else
    //            success = false;

    //        return success;
    //    }

    //    public bool CtlLaserOff()
    //    {
    //        bool success = true;

    //        //if (!this.Rtc.CtlLaserOff())
    //        //    return false;

    //        int nRet = channel.LaserOff();
    //        if ((int)FuncState.Success != nRet)
    //        {
    //            Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"Cepheus LaserOff Command Fail!(Error Code:{nRet})");
    //            return false;
    //        }
    //        return success;
    //    }


    //    public bool GetState()
    //    {
    //        if (channel == null) return false;

    //        int nRet = channel.GetState(out int state);
    //        if ((int)FuncState.Success != nRet)
    //        {
    //            Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"Cepheus GetState Command Fail!(Error Code:{nRet})");
    //            return false;
    //        }
    //        bool init = (state & (int)Status.Init) != 0 ? true : false;
    //        bool shutterOpen = (state & (int)Status.BdsShutterOpen) != 0 ? true : false;
    //        bool globalErr = (state & (int)Status.GlobalErr) != 0 ? true : false;
    //        bool laserReady = (state & (int)Status.LaserReady) != 0 ? true : false;
    //        bool GateSignalOn = (state & (int)Status.GateSignalOn) != 0 ? true : false;
    //        bool LaserArm = (state & (int)Status.LaserArm) != 0 ? true : false;
    //        bool BurstModeOn = (state & (int)Status.BurstModeOn) != 0 ? true : false;

    //        IsError &= init;
    //        IsError &= !laserReady;

    //        return true;
    //    }

    //    public void GetTemperature()
    //    {
    //        channel.GetTemperature(out double ld1Temp, out double ld2, out double ld3, out double pa);
    //    }

    //    public void GetTemperatureState()
    //    {
    //        channel.GetTemperatureState(out int ld1, out int ld2, out int ld3, out int pa);
    //        if (ld1 <= 0 || ld2 <= 0 || ld3 <= 0 || pa <= 0)
    //            this.IsError = true;
    //    }
    //}

    //public class LaserCepheus : ILaser
    public class LaserCepheus : DPSSLaser
    {
        #region Define

        #region Service
        [ServiceContract]
        public interface IMyContract
        {
            [OperationContract]
            int SetArm();

            [OperationContract]
            int ResetArm();

            [OperationContract]
            int SetRepRate(int repRate);

            [OperationContract]
            int GetRepRate(ref int repRate);

            [OperationContract]
            int SetPower(int power);

            [OperationContract]
            int GetPower(ref int power);

            [OperationContract]
            int GetStatus(ref int status);

            [OperationContract]
            int Initialize();

            [OperationContract]
            void ShutDown();
        }
        #endregion

        private enum ReturnCode
        {
            InitialFail = 1,
            ComportConnectFail = 2,
            UnKnowError = 3,
            BoardConnectionFail = 4,
            ValueOutOfRange = 6,
            FileDoesNotFind = 7,
            BurstModeLocked = 8,
            BurstModeOff = 9,
            PhasingError = 600,
        }

        public enum SystemStatus
        {
            sfInited = (0x01 << 0),
            sfShutterOpen = (0x01 << 1),
            sfGlobalError = (0x01 << 2),
            sfLaserReady = (0x01 << 3),
            sfGateSignalOn = (0x01 << 4),
            sfLaserArmed = (0x01 << 5),
            sfBurstModeOn = (0x01 << 6),
        }

        public enum StatusFlags
        {
            /// <summary>
            /// temperature error / LSM board
            /// </summary>
            efTEMP = 1,
            /// <summary>
            /// general or temperature error in laser driver 3
            /// </summary>
            efLD3 = 2,
            /// <summary>
            /// general or temperature error in laser driver 2
            /// </summary>
            efLD2 = 4,
            /// <summary>
            /// general or temperature error in laser driver 1
            /// </summary>
            efLD1 = 8,
            /// <summary>
            /// q-switching error has occurred
            /// </summary>
            efQSD = 16,
            /// <summary>
            /// laser system is out of synchronization
            /// </summary>
            efPLL = 32,
        }
        #endregion

        #region Field
        public ChannelFactory<IMyContract> factory = new ChannelFactory<IMyContract>();
        public IMyContract channel;
        public bool disposed;
        public CepheusLaserState m_State;
        #endregion

        #region Constructor
        public LaserCepheus()
        {
            this.disposed = false;
            this.Status = new CepheusLaserState();
        }
        ~LaserCepheus()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        #endregion

        #region Property
        public int Index { get; private set; }
        public string Name { get; private set; }

        public float MaxPowerWatt { get; }
        public PowerXFactor PowerXFactor { get; set; }
        public bool IsBusy
        {
            get
            {
                return this.isbusy;
            }
        }
        private bool isbusy;

        #region Status Flag
        public CepheusLaserState Status
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public bool IsConnected
        {
            get { return this.Status.Connected; }
        }

        public bool IsReady
        {
            get { return this.Status.Ready; }
        }

        public bool IsShutterOpend
        {
            get { return this.Status.ShutterOpend; }
        }

        public bool IsError
        {
            get { return this.Status.Error; }
        }

        public bool IsGateSignalOn
        {
            get { return this.Status.GateSignalOn; }
        }

        public bool IsArmed
        {
            get { return this.Status.Armed; }
        }

        public bool IsBurstModeOn
        {
            get { return this.Status.BurstModeOn; }
        }

        public object Tag { get; set; }

        #endregion

        #endregion

        #region Method
        private ReturnCode CheckReturnCode(int code)
        {
            ReturnCode key = ReturnCode.UnKnowError;

            switch (code)
            {
                case (int)ReturnCode.InitialFail:
                case (int)ReturnCode.ComportConnectFail:
                case (int)ReturnCode.UnKnowError:
                case (int)ReturnCode.BoardConnectionFail:
                case (int)ReturnCode.ValueOutOfRange:
                case (int)ReturnCode.FileDoesNotFind:
                case (int)ReturnCode.BurstModeLocked:
                case (int)ReturnCode.BurstModeOff:
                case (int)ReturnCode.PhasingError:
                    key = (ReturnCode)code;
                    //Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"{key.ToString()}(Error Code:{code})");
                    break;
                default:
                    break;
            }

            return key;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            //if (disposing)
            //{
            if (channel != null)
            {
                //channel.ShutDown();
                //((ICommunicationObject)channel).Close();
                // Close Channel
            }
            //}
            this.disposed = true;
        }

        public int Initialize()
        {
            int nRet = 0;
            
            if ((nRet = channel.Initialize()) != 0) return (int)this.CheckReturnCode(nRet);
            if ((nRet = channel.SetRepRate(36)) != 0) return (int)this.CheckReturnCode(nRet);
            if ((nRet = channel.SetPower(1)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }
        
        public int Connect()
        {
            int nRet = 0;

            // 실행 시 Laser 프로그램 살아 있는 경우 닫아주고 재 실행.
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("conhost"))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit(); // possibly with a timeout
                }
                catch (InvalidOperationException invalidException)
                {

                }
            }

            // Address
            string address = "net.tcp://localhost:8080/myAddress";
            factory.Endpoint.Address = new EndpointAddress(address);
            // Binding : TCP 사용
            factory.Endpoint.Binding = new NetTcpBinding();
            // Contract 설정
            factory.Endpoint.Contract.ContractType = typeof(IMyContract);
            factory.Endpoint.Binding.SendTimeout = new TimeSpan(0, 5, 0);
            factory.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            // Channel Factory 만들기
            channel = factory.CreateChannel();

            return nRet;
        }

        /// <summary>
        /// 0~100%
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public int SetPower(int power)
        {
            int nRet = 0;

            if ((nRet = channel.SetPower(power)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int GetPower(ref int power)
        {
            int nRet = 0;

            if ((nRet = channel.GetPower(ref power)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int SetRepRate(int repRate)
        {
            int nRet = 0;

            if ((nRet = channel.SetRepRate(repRate)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int GetRepRate(ref int repRate)
        {
            int nRet = 0;

            if ((nRet = channel.GetRepRate(ref repRate)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int SetArm()
        {
            int nRet = 0;

            if ((nRet = channel.SetArm()) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int ResetArm()
        {
            int nRet = 0;

            if ((nRet = channel.ResetArm()) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }
        
        public int ShutDown()
        {
            int nRet = 0;
            channel.ShutDown();
            return nRet;
        }
        
        public int GetStatus(ref int status)
        {
            int nRet = 0;
            
            if ((nRet = channel.GetStatus(ref status)) != 0) return (int)this.CheckReturnCode(nRet);
            this.CheckStatus(status);
            
            return nRet;
        }

        private CepheusLaserState CheckStatus(int code)
        {
            this.Status.Armed = (code & (int)SystemStatus.sfLaserArmed) == (int)SystemStatus.sfLaserArmed;
            this.Status.BurstModeOn = (code & (int)SystemStatus.sfBurstModeOn) == (int)SystemStatus.sfBurstModeOn;
            this.Status.Connected = (code & (int)SystemStatus.sfInited) == (int)SystemStatus.sfInited;
            this.Status.Error = (code & (int)SystemStatus.sfGlobalError) == (int)SystemStatus.sfGlobalError;
            this.Status.GateSignalOn = (code & (int)SystemStatus.sfGateSignalOn) == (int)SystemStatus.sfGateSignalOn;
            this.Status.Ready = (code & (int)SystemStatus.sfLaserReady) == (int)SystemStatus.sfLaserReady;
            this.Status.ShutterOpend = (code & (int)SystemStatus.sfShutterOpen) == (int)SystemStatus.sfShutterOpen;

            return this.Status;
        }

        //public void GetTemperature()
        //{
        //    channel.GetTemperature(out double ld1Temp, out double ld2, out double ld3, out double pa);
        //}

        //public void GetTemperatureState()
        //{
        //    channel.GetTemperatureState(out int ld1, out int ld2, out int ld3, out int pa);
        //    if (ld1 <= 0 || ld2 <= 0 || ld3 <= 0 || pa <= 0)
        //        this.IsError = true;
        //}

        public bool CtlReset()
        {
            return true;
        }

        /// <summary>
        /// 0~100%
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public bool CtlPower(double power)
        {
            return true;
        }
        //public bool Initialize()
        //{
        //    return true;
        //}
        #endregion

    }

    #region CepheusLaserState
    [Serializable]
    public class CepheusLaserState
    {
        #region Field
        private bool m_Connected;
        private bool m_ShutterOpend;
        private bool m_Error;
        private bool m_Ready;
        private bool m_GateSignalOn;
        private bool m_Armed;
        private bool m_BurstModeOn;
        #endregion

        #region Constructor
        public CepheusLaserState()
        {
            this.Connected = false;
            this.ShutterOpend = false;
            this.Error = false;
            this.Ready = false;
            this.GateSignalOn = false;
            this.Armed = false;
            this.BurstModeOn = false;
        }
        #endregion

        #region Property
        public bool BurstModeOn
        {
            get { return this.m_BurstModeOn; }
            set { this.m_BurstModeOn = value; }
        }
        public bool Armed
        {
            get { return this.m_Armed; }
            set { this.m_Armed = value; }
        }
        public bool GateSignalOn
        {
            get { return this.m_GateSignalOn; }
            set { this.m_GateSignalOn = value; }
        }
        public bool Ready
        {
            get { return this.m_Ready; }
            set { this.m_Ready = value; }
        }
        public bool Error
        {
            get { return this.m_Error; }
            set { this.m_Error = value; }
        }
        public bool ShutterOpend
        {
            get { return this.m_ShutterOpend; }
            set { this.m_ShutterOpend = value; }
        }
        public bool Connected
        {
            get { return this.m_Connected; }
            set { this.m_Connected = value; }
        }
        #endregion
    }
    #endregion
}
