using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace QMC.Common.Laser
{
    static class CepheusWrapDll
    {
        public const string FileName = "CEPHEUSctrlDLL.dll";

        #region Dll Imports 
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_GetRepRate@4")]
        public static extern int cp_GetRepRate(ref int reprate);

        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_GetPower@4")]
        public static extern int cp_GetPower(ref int power);

        /// <summary>
        /// This command initializes the laser system and the control library. All default values will be uploaded to the laser control board. The laser system is calculating the synchronization parameters. This procedure may takes up to 120s.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_InitCtrlDll@0")]        
        public static extern int cp_InitCtrlDll();

        /// <summary>
        /// This command activates the burst mode option. The option is only available if power value is set to 1000 per mille. Otherwise the burst mode is locked. The function returns with return code cBurstModeLocked
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_ActivateBurstMode@0")]
        public static extern int cp_ActivateBurstMode();

        /// <summary>
        /// This command deactivates the burst mode option. The option is only available if power value is set to 1000 per mille. Otherwise the burst mode is locked. the function returns with return code cBurstModeLocked
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_DeactivateBurstMode@0")]
        public static extern int cp_DeactivateBurstMode();

        /// <summary> 
        /// Sets the burst internal repetition rate. Burst mode must be unlocked and activated!!
        /// </summary>
        /// <param name="binreprate">21, 32, 65</param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_SetBinRepRate@4")]
        public static extern int cp_SetBinRepRate(int binreprate);

        /// <summary>
        /// Sets the number of pulses in between one burst. Burst mode must be unlocked and activated!!
        /// </summary>
        /// <param name="noofpulses"></param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_SetNoOfPulses@4")]
        public static extern int cp_SetNoOfPulses(int noofpulses);

        /// <summary>
        /// Sets the laser repetition rate (50 - 200 kHz).
        /// <param name="reprate"></param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_SetRepRate@4")]
        public static extern int cp_SetRepRate(int reprate);

        /// <summary>
        /// Sets the laser power in per mill (0 - 1000 ‰).
        /// </summary>
        /// <param name="power"></param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_SetPower@4")]
        public static extern int cp_SetPower(int power);

        /// <summary>
        /// This command arms the laser system and applies the current paramset. If the system is set to armmode emission can be started by activating the gate signal. Use cp_GetSystemStatus() to check if system is armed (sfLaserArmed).
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_SetArmLaser@0")]
        public static extern int cp_SetArmLaser();

        /// <summary>
        /// This command disarms the laser system and applies. No emission when GateSignal is activated.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_ResetArmLaser@0")]
        public static extern int cp_ResetArmLaser();

        /// <summary>
        /// This command activates the shutter connector output. After calling this command the System Status Flag sfShutterOpen is set. The shutter is an option.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_OpenShutter@0")]
        public static extern int cp_OpenShutter();

        /// <summary>
        /// This command deactivates the shutter connector output. After calling this command the System Status Flag sfShutterOpen is resetted. The shutter is an option.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_CloseShutter@0")]
        public static extern int cp_CloseShutter();

        /// <summary>
        /// Use this command for system state requests. See System Status Flag definition above.
        /// </summary>
        /// <param name="systemstatus">결과값은 (enum)SystemStatus를 확인하십시오.</param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_GetStatus@4")]
        public static extern int cp_GetStatus(ref int systemstatus);

        /// <summary>
        /// Use this command to load individual burst shapes.
        /// </summary>
        /// <param name="shapepath"></param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_LoadBurstShape@4")]
        public static extern int cp_LoadBurstShape(System.String shapepath);

        /// <summary>
        /// This command hides the graphical user interface of the CEPHEUS CONTROL IF software.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_HideUserInterface@0")]
        public static extern int cp_HideUserInterface();

        /// <summary>
        /// This command shows the graphical user interface of the CEPHEUS CONTROL IF software with full functionality.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_ShowUserInterface@0")]
        public static extern int cp_ShowUserInterface();

        /// <summary>
        /// Use this command for requesting Error state. See Error Status Flag definition above.
        /// </summary>
        /// <param name="errorStatus">결과값은 (enum)StatusFlags를 확인하십시오.</param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_GetErrorStatus@4")]
        public static extern int cp_GetErrorStatus(ref int errorStatus);

        /// <summary>
        /// Use this command before shutting down the laser system. All system components will be deactivated. The Com connection will be closed.
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "cp_ShutDown@0")]
        public static extern int cp_ShutDown();

        /// <summary>
        /// Use this command to check the temperature of the system components
        /// </summary>
        /// <param name="LD1Temp">temperature sensor at the laser diode respectively</param>
        /// <param name="LD2Temp">temperature sensor at the laser diode respectively</param>
        /// <param name="LD3Temp">temperature sensor at the laser diode respectively</param>
        /// <param name="PATemp">temperature at the power amplifier</param>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_GetTemperature@16")]
        public static extern int cp_GetTemperature(ref double LD1Temp, ref double LD2Temp, ref double LD3Temp, ref double PATemp);

        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_GetTemperature_State@16")]
        public static extern int cp_GetTemperature_State(ref int State1, ref int State2, ref int State3, ref int State4);

        /// <summary>
        /// Use this command create a PLL-Synchronisation Error. For testing purpose only !!!!
        /// </summary>
        /// <returns>결과값은 (enum)ReturnCode를 확인하십시오.</returns>
        [DllImport(FileName, CallingConvention = CallingConvention.StdCall, EntryPoint = "cp_ForceSyncError@0")]
        public static extern int cp_ForceSyncError();
        #endregion
    }

    public enum ReturnCode
    {
        Success             = 0,                //  function successfully called
        InitialFail         = 1,                //  library and laser system is not inited
        ComportConnectFail  = 2,                //  ComPort connection error
        UnKnowError         = 3,                //  unknown programmatic error (e.g. exception)
        BoardConnectionFail = 4,                //  no connection to laser control board
        ValueOutOfRange     = 6,                //  committed function value out of range
        FileDoesNotFind     = 7,                //  committed file path does not exist
        BurstModeLocked     = 8,                //  burst mode is locked (power value < 1000 per mille)
        BurstModeOff        = 9,                //  burst mode is switched off
        ParameterNotValid   = 10,               //  parameter set not valid / parameter file missing or invalid
        cWarmUpRunning      = 22,               //  laser system is not up to temp
        PhasingError        = 600,              //  The phasing error is an exclusive error code reserved for the
                                                //  function cp_InitCtrlDll. The initialization procedure calibrates the
                                                //  right phasing position corresponding to the master clock of the
                                                //  system. In some cases the phasing procedure fails due to some
                                                //  hardware damage or after an aborted firmware upgrade....etc.
                                                //  In that case the cp_InitCtrlDll returns (after 300s timeout) an
                                                //  error code >=600.
                                                //  This error code is a combination of two values which are added
                                                //  up.
                                                //  ----------------------------------------------------------------------
                                                //  value 1: 600    // error code indicates phasing failure
                                                //  value 2: XX     // number between 1 and 15 / some additional
                                                //                     information about the phasing position.
                                                //                     (important for a service call !!).
                                                //  ----------------------------------------------------------------------
                                                //  complete error code: value 1 + value 2
    }

    public enum SystemStatus
    {
        sfInited            = (0x01 << 0),      //  = 1 library and lasersystem is inited
        sfShutterOpen       = (0x01 << 1),      //  = 2 the shutter open command has been called
        sfGlobalError       = (0x01 << 2),      //  = 4 a system error has occured
        sfLaserReady        = (0x01 << 3),      //  = 8 laser system is ready to arm
        sfGateSignalOn      = (0x01 << 4),      //  = 16 gate signal is active
        sfLaserArmed        = (0x01 << 5),      //  = 32 laser system is armed
        sfBurstModeOn       = (0x01 << 6),      //  = 64 Burst mode is active
        sfSpotshiftRqurd    = (0x01 << 7),      //  = 128 Spotshift required
    }
   

    public class MyCepheusLaser
    {
        public bool disposed;
        public CepheusLaserState m_State;

        #region Constructor
        public MyCepheusLaser()
        {
            this.disposed = false;
            this.Status = new CepheusLaserState();
        }
        ~MyCepheusLaser()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        #endregion

        //bool IsConnected { get; }
        ///// <summary>
        ///// 셔터 Open/Close 여부
        ///// </summary>
        //bool IsShutterOpend { get; }
        ///// <summary>
        ///// GateSignal On/Off 여부
        ///// </summary>
        //bool IsGateSignalOn { get; }
        ///// <summary>
        ///// Armed 상태
        ///// </summary>
        //bool IsArmed { get; }
        ///// <summary>
        ///// BurstMode On/Off 여부
        ///// </summary>
        //bool IsBurstModeOn { get; }

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


        public int Connect()
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_InitCtrlDll()) != 0)
            {
                return (int)this.CheckReturnCode(nRet);
            }

            return nRet;
        }

        public int SetPower(int power)
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_SetPower(power)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int GetPower(ref int power)
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_GetPower(ref power)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int SetRepRate(int repRate)
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_SetRepRate(repRate)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int GetRepRate(ref int repRate)
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_GetRepRate(ref repRate)) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int SetArm()
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_SetArmLaser()) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int ResetArm()
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_ResetArmLaser()) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int ShutDown()
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_ShutDown()) != 0) return (int)this.CheckReturnCode(nRet);

            return nRet;
        }

        public int GetStatus(ref int state)
        {
            int nRet = 0;

            if ((nRet = CepheusWrapDll.cp_GetStatus(ref state)) != 0) return (int)this.CheckReturnCode(nRet);

            this.CheckStatus(state);

            return nRet;
        }

        private CepheusLaserState CheckStatus(int code)
        {
            this.Status.Armed           = (code & (int)SystemStatus.sfLaserArmed)   == (int)SystemStatus.sfLaserArmed;
            this.Status.BurstModeOn     = (code & (int)SystemStatus.sfBurstModeOn)  == (int)SystemStatus.sfBurstModeOn;
            this.Status.Connected       = (code & (int)SystemStatus.sfInited)       == (int)SystemStatus.sfInited;
            this.Status.Error           = (code & (int)SystemStatus.sfGlobalError)  == (int)SystemStatus.sfGlobalError;
            this.Status.GateSignalOn    = (code & (int)SystemStatus.sfGateSignalOn) == (int)SystemStatus.sfGateSignalOn;
            this.Status.Ready           = (code & (int)SystemStatus.sfLaserReady)   == (int)SystemStatus.sfLaserReady;
            this.Status.ShutterOpend    = (code & (int)SystemStatus.sfShutterOpen)  == (int)SystemStatus.sfShutterOpen;

            return this.Status;
        }

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

            this.disposed = true;
        }
    }
}
