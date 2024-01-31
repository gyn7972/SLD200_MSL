using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CEPHEUSImport
{
    public class CepheusWrap
    {
        //private const string FileName = "CEPHEUSctrlDLL.dll";
        private const string FileName = "CEPHEUSctrlDLL.64.dll";

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
}
