using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

//namespace QMC.Core.Laser
namespace QMC.Common.Laser
{
    /// <summary>
    /// 레이저 가상 소스
    /// </summary>
    //public class LaserVirtual : ILaser
    //{
    //    public int Index { get; private set; }
    //    public string Name { get; private set; }

    //    public IRtc Rtc { get; private set; }
    //    public float MaxPowerWatt { get; }
    //    public PowerXFactor PowerXFactor { get; set; }
    //    public bool IsReady { get { return !this.IsError; } }
    //    public bool IsBusy  { get { return false;  } }
    //    public bool IsError { get; set; }

    //    public object Tag { get; set; }

    //    private bool disposed = false;

    //    public LaserVirtual(int index, string name, IRtc rtc, PowerXFactor powerXFactor, float maxPowerWatt)
    //    {
    //        this.Index = index;
    //        this.Name = name;
    //        this.Rtc = rtc;
    //        this.PowerXFactor = powerXFactor;
    //        this.MaxPowerWatt = maxPowerWatt;
    //    }
    //    ~LaserVirtual()
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
    //        }
    //        this.disposed = true;
    //    }

    //    public bool Initialize()
    //    {
    //        return true;
    //    }

    //    public bool CtlReset()
    //    {
    //        IsError = false;
    //        return true;
    //    }

    //    public bool CtlPower(double watt)
    //    {
    //        if (this.IsBusy)
    //            return false;
    //        if (this.IsError)
    //            return false;
    //        bool success = true;
    //        float powerXValue = 10; //  pen.Power 파워를 출력하기 위한 입력값 연산
    //        success = this.Rtc.CtlLaserControl(this.PowerXFactor, powerXValue);
    //        if (success)
    //            Logger.Log(Logger.Module.Laser, Logger.Type.Warn, $"laser [{this.Index}]: set laser power to {watt:F3}W");
    //        return success;
    //    }

    //    public bool ListPower(double watt)
    //    {
    //        if (this.IsError)
    //            return false;
    //        bool success = true;
    //        float powerXValue = 10; //  pen.Power 파워를 출력하기 위한 입력값 연산
    //        success &= this.Rtc.ListLaserControl(this.PowerXFactor, powerXValue);
    //        return success;
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
    //        bool success = true;
    //        return success;
    //    }

    //    public bool CtlLaserStop()
    //    {
    //        bool success = true;
    //        return success;
    //    }

    //    public bool CtlLaserOn()
    //    {
    //        bool success = true;
    //        return success;
    //    }

    //    public bool CtlLaserOff()
    //    {
    //        bool success = true;
    //        return success;
    //    }

    //}

    //public class LaserVirtual : ILaser
    public class LaserVirtual : DPSSLaser
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

            //[OperationContract]
            //int GetTemperature(out double ld1Temp, out double ld2Temp, out double ld3Temp, out double paTemp);

            //[OperationContract]
            //int GetTemperatureState(out int ld1, out int ld2, out int ld3, out int pa);

            [OperationContract]
            int Initialize();

            [OperationContract]
            void ShutDown();
        }
        #endregion

        #endregion

        #region Field
        private ChannelFactory<IMyContract> factory = new ChannelFactory<IMyContract>();
        private IMyContract channel;
        private bool disposed;
        private CepheusLaserState m_State;
        public float MaxPowerWatt { get; }
        public PowerXFactor PowerXFactor { get; set; }
        public bool IsBusy { get { return false; } }

        public object Tag { get; set; }
        #endregion

        #region Constructor
        public LaserVirtual()
        {
            this.disposed = false;
            this.Status = new CepheusLaserState();
        }
        ~LaserVirtual()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        #endregion

        #region Property
        public int Index { get; private set; }
        public string Name { get; private set; }

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
        #endregion

        #endregion

        #region Method
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (channel != null)
            {
                ((ICommunicationObject)channel).Close();
                channel.ShutDown();
            }

            this.disposed = true;
        }

        public int Initialize()
        {
            int nRet = 0;

            return nRet;
        }

        public int Connect()
        {
            int nRet = 0;

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
            return nRet;
        }

        public int GetPower(ref int power)
        {
            int nRet = 0;
            return nRet;
        }

        public int SetRepRate(int repRate)
        {
            int nRet = 0;
            return nRet;
        }

        public int GetRepRate(ref int repRate)
        {
            int nRet = 0;
            return nRet;
        }
        
        public int SetArm()
        {
            int nRet = 0;

            return nRet;
        }

        public int ResetArm()
        {
            int nRet = 0;

            return nRet;
        }

        public int ShutDown()
        {
            int nRet = 0;
            return nRet;
        }

        public int GetStatus(ref int status)
        {
            int nRet = 0;

            return nRet;
        }

        //int ILaser.Initialize()
        int DPSSLaser.Initialize()
        {
            int ret = 0;
            return ret;
        }

        public bool CtlReset()
        {
            return true;
        }

        public bool CtlPower(double watt)
        {
            return true;
        }
        #endregion

    }
}
