using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Runtime.InteropServices;


using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using SpiralLab.Sirius;
using LaserVirtual = SpiralLab.Sirius.LaserVirtual;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using QMC.Common.Modules;



namespace QMC.Common.Parts
{
    //internal class SpiralLabScanner : Part
    public class SpiralLabScanner : Part
    {
        #region Define
        private System.Timers.Timer _monitorTimer;
        private const uint OverTempWarningBit = 0x40u;
        private const uint OverTempErrorBit = 0x80u;
        private bool _disposed = false;
        public bool IsInitialized { get; private set; } = false;
        #endregion

        #region Property
        public string ScannerName { get; set; }
        public Rtc6 rtc { get; set; }
        #endregion

        public SpiralLabScanner(string name, IRtc rtcInstance) : base(name)
        {
            this.ScannerName = name;
            this.rtc = rtcInstance as Rtc6 ?? throw new InvalidCastException("IRtc 인스턴스는 Rtc6를 구현해야 합니다.");

            IsInitialized = true;
            //StartMonitoring();
        }

        ~SpiralLabScanner()
        {
            Dispose(false);
        }

        public override int Create()
        {
            //StartMonitoring();
            return base.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //StopMonitoring();
                }
                _disposed = true;
            }
        }

        private void StartMonitoring(int intervalMs = 1000)
        {
            _monitorTimer = new System.Timers.Timer(intervalMs);
            _monitorTimer.Elapsed += (s, e) => CheckAndLogTemperature();
            _monitorTimer.AutoReset = true;
            _monitorTimer.Start();
        }

        private void StopMonitoring()
        {
            _monitorTimer?.Stop();
            _monitorTimer?.Dispose();
            _monitorTimer = null;
        }
        
        public void CheckAndLogTemperature()
        {
            if (rtc == null)
                return;

            if (!rtc.CtlGetStatus(RtcStatus.TempOK))
            {
                Log.Write("Rtc6", "⚠️ Warning: Head over - temperature or temperature sensor abnormal.");
            }

            if (!rtc.CtlGetStatus(RtcStatus.PowerOK))
            {
                Log.Write("Rtc6", "❌ Error: Power supply abnormal.");
            }
        }

        public void CheckAndLogAllStatuses()
        {
            if (rtc == null)
                return;

            foreach (RtcStatus status in Enum.GetValues(typeof(RtcStatus)))
            {
                bool result = rtc.CtlGetStatus(status);
                string logMsg = "";

                switch (status)
                {
                    case RtcStatus.Busy:
                        logMsg = $"Busy (Status bits: 0x1, 0x80, 0x8000, 0x800000): {(result ? "✅ OK" : "❌ Not Busy")}";
                        break;
                    case RtcStatus.NotBusy:
                        logMsg = $"NotBusy (Inverse of Busy): {(result ? "✅ OK" : "❌ Busy")}";
                        break;
                    case RtcStatus.List1Busy:
                        logMsg = $"List1Busy (Status & 0x0F): {(result ? "✅ OK" : "❌ Idle")}";
                        break;
                    case RtcStatus.List2Busy:
                        logMsg = $"List2Busy (Status & 0x10): {(result ? "✅ OK" : "❌ Idle")}";
                        break;
                    case RtcStatus.NoError:
                        logMsg = $"NoError (No Abort + LastError == 0): {(result ? "✅ OK" : "❌ Error Detected")}";
                        break;
                    case RtcStatus.Aborted:
                        logMsg = $"Aborted (Manual Abort Flag): {(result ? "❌ Aborted" : "✅ Not Aborted")}";
                        break;
                    case RtcStatus.PositionAckOK:
                        logMsg = $"PositionAckOK (HeadStatus & 0x08, 0x10): {(result ? "✅ OK" : "❌ Not Acknowledged")}";
                        break;
                    case RtcStatus.PowerOK:
                        logMsg = $"PowerOK (HeadStatus & 0x80): {(result ? "✅ OK" : "❌ Power Fault")}";
                        break;
                    case RtcStatus.TempOK:
                        logMsg = $"TempOK (HeadStatus & 0x40): {(result ? "✅ OK" : "❌ Over Temp / Sensor Fault")}";
                        break;
                    case RtcStatus.MotfOutOfRange:
                        logMsg = $"MotfOutOfRange (MOF Overflow/Underflow flags): {(result ? "❌ Out of Range" : "✅ OK")}";
                        break;
                    default:
                        logMsg = $"{status}: {(result ? "✅ OK" : "❌ FAIL")}";
                        break;
                }

                Log.Write("Rtc6", logMsg);
            }
        }

        public bool GetScannerPosition(out double x_mm, out double y_mm)
        {
            x_mm = 0;
            y_mm = 0;

            if (rtc == null)
                return false;

            //try
            //{
            //    int x_raw, y_raw;
            //            // Rtc6 클래스 내부 cardId는 protected이거나 private일 수 있으므로 추가적으로 확인 필요
            //    var cardIdField = rtc.GetType().GetField("cardId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //    if (cardIdField == null)
            //        return false;

            //    int cardId = (int)cardIdField.GetValue(rtc);

            //    Rtc6Native.n_get_actual_position(cardId, out x_raw, out y_raw);

            //    x_mm = x_raw / 65536.0;
            //    y_mm = y_raw / 65536.0;
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Log.Write("Rtc6", $"⚠️ Failed to get scanner position: {ex.Message}");
            //    return false;
            //}

            return true;
        }

        // DrawCalibrationCrosses, DrawCross, DrawCalibrationArc, DrawArc 등 기존 메서드들은 그대로 유지됨
        // ... (기존 코드 생략)

    }
}


//internal static class Rtc6Native
//{
//    [DllImport("rtc6.dll", CallingConvention = CallingConvention.Cdecl)]
//    public static extern void n_get_actual_position(int cardNo, out int x, out int y);
//}