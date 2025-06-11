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
using RTC6Import;
using System.Threading;



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


        private Dictionary<RtcStatus, bool> previousStatus = new Dictionary<RtcStatus, bool>();
        public bool IsRtcBusy { get; private set; } = false;

        public void CheckAndLogAllStatuses()
        {
            if (rtc == null)
                return;

            foreach (RtcStatus status in Enum.GetValues(typeof(RtcStatus)))
            {
                bool current = rtc.CtlGetStatus(status);
                bool hasChanged = !previousStatus.ContainsKey(status) || previousStatus[status] != current;

                if (!hasChanged)
                    continue;

                // 상태 업데이트
                previousStatus[status] = current;

                string logMsg = "";

                switch (status)
                {
                    case RtcStatus.Busy:
                        IsRtcBusy = current;  // 상태 캐싱
                        logMsg = $"Busy (0x1, 0x80, 0x8000, 0x800000): {(current ? "✅ OK" : "❌ Not Busy")}";
                        break;
                    //case RtcStatus.NotBusy:
                    //    logMsg = $"NotBusy (Inverse of Busy): {(current ? "✅ OK" : "❌ Busy")}";
                    //    break;
                    //case RtcStatus.List1Busy:
                    //    logMsg = $"List1Busy (Status & 0x0F): {(current ? "✅ OK" : "❌ Idle")}";
                    //    break;
                    //case RtcStatus.List2Busy:
                    //    logMsg = $"List2Busy (Status & 0x10): {(current ? "✅ OK" : "❌ Idle")}";
                    //    break;
                    //case RtcStatus.NoError:
                    //    logMsg = $"NoError (No Abort + LastError == 0): {(current ? "✅ OK" : "❌ Error Detected")}";
                    //    break;
                    //case RtcStatus.Aborted:
                    //    logMsg = $"Aborted (Manual Abort Flag): {(current ? "❌ Aborted" : "✅ Not Aborted")}";
                    //    break;
                    //case RtcStatus.PositionAckOK:
                    //    logMsg = $"PositionAckOK (HeadStatus & 0x08, 0x10): {(current ? "✅ OK" : "❌ Not Acknowledged")}";
                    //    break;
                    case RtcStatus.PowerOK:
                        logMsg = $"PowerOK (HeadStatus & 0x80): {(current ? "✅ OK" : "❌ Power Fault")}";
                        break;
                    case RtcStatus.TempOK:
                        logMsg = $"TempOK (HeadStatus & 0x40): {(current ? "✅ OK" : "❌ Over Temp / Sensor Fault")}";
                        break;
                    //case RtcStatus.MotfOutOfRange:
                    //    logMsg = $"MotfOutOfRange (MOF Overflow/Underflow): {(current ? "❌ Out of Range" : "✅ OK")}";
                    //    break;
                    default:
                        //logMsg = $"{status}: {(current ? "✅ OK" : "❌ FAIL")}";
                        break;
                }

                Log.Write("Rtc6", logMsg);
            }
        }

        // 클래스 멤버로 이전 온도값 저장
        private double _prevPcbTemp1 = -1;
        private double _prevPcbTemp2 = -1;
        private double _prevGalvoTemp1 = -1;
        private double _prevGalvoTemp2 = -1;
        private const double logDeltaThreshold = 1; // 로그 갱신 임계값 (°C 단위) //1도 이상 바뀌면 로그 남김.

        public bool IsOverTemperatureWarning()
        {
            if (rtc == null)
                return false;

            bool isOverTemp = false;
            try
            {
                RTC6Wrap.control_command(1, 1, 0x0514);
                RTC6Wrap.control_command(1, 2, 0x0514);
                Thread.Sleep(5);
                int nPCBTemp1 = RTC6Wrap.get_value(1);
                int nPCBTemp2 = RTC6Wrap.get_value(2);
                double pcbTemp1 = (nPCBTemp1 >> 4) / 10.0; // °C
                double pcbTemp2 = (nPCBTemp2 >> 4) / 10.0;

                Thread.Sleep(1);

                // Galvo_Temp
                RTC6Wrap.control_command(1, 1, 0x0515);
                RTC6Wrap.control_command(1, 2, 0x0515);
                Thread.Sleep(5); // 10 ms wait
                int nGalvoTemp1 = RTC6Wrap.get_value(1);
                int nGalvoTemp2 = RTC6Wrap.get_value(2);
                double galvoTemp1 = (nGalvoTemp1 >> 4) / 10.0;
                double galvoTemp2 = (nGalvoTemp2 >> 4) / 10.0;

                // --- 온도 임계값 ---
                const double warningThreshold = 75.0; // 임계값 설정 (필요 시 변경)

                isOverTemp =
                    pcbTemp1 >= warningThreshold || pcbTemp2 >= warningThreshold ||
                    galvoTemp1 >= warningThreshold || galvoTemp2 >= warningThreshold;

                // 로그 남길 조건
                bool shouldLog =
                    Math.Abs(pcbTemp1 - _prevPcbTemp1) >= logDeltaThreshold ||
                    Math.Abs(pcbTemp2 - _prevPcbTemp2) >= logDeltaThreshold ||
                    Math.Abs(galvoTemp1 - _prevGalvoTemp1) >= logDeltaThreshold ||
                    Math.Abs(galvoTemp2 - _prevGalvoTemp2) >= logDeltaThreshold;

                if (shouldLog)
                {
                    string logMsg = string.Format(
                        "[RTC6 Temp Check] PCB1: {0:F1}°C, PCB2: {1:F1}°C | Galvo1: {2:F1}°C, Galvo2: {3:F1}°C => {4}",
                        pcbTemp1, pcbTemp2, galvoTemp1, galvoTemp2,
                        isOverTemp ? "Over Temp Detected!" : "Normal");

                    Log.Write("Rtc6", logMsg);

                    // 온도 갱신
                    _prevPcbTemp1 = pcbTemp1;
                    _prevPcbTemp2 = pcbTemp2;
                    _prevGalvoTemp1 = galvoTemp1;
                    _prevGalvoTemp2 = galvoTemp2;
                }

                //// --- 로그 메시지 구성 ---
                //string logMsg = string.Format(
                //    "[RTC6 Temp Check] PCB1: {0:F1}°C, PCB2: {1:F1}°C | Galvo1: {2:F1}°C, Galvo2: {3:F1}°C => {4}",
                //    pcbTemp1, pcbTemp2, galvoTemp1, galvoTemp2,
                //    isOverTemp ? "Over Temp Detected!" : "Normal");
                //Log.Write("Rtc6", logMsg);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            
            return isOverTemp;
        }

        public bool GetScannerPosition(out double x_mm, out double y_mm)
        {
            x_mm = 0;
            y_mm = 0;

            if (rtc == null)
                return false;

            try
            {
                // 파일럿 호출: X, Y 실제 위치 읽기
                RTC6Wrap.control_command(1, 1, 0x0530); // X축
                RTC6Wrap.control_command(1, 2, 0x0530);
                Thread.Sleep(2);
                int rawX = RTC6Wrap.get_value(1);

                RTC6Wrap.control_command(1, 1, 0x0531); // Y축
                RTC6Wrap.control_command(1, 2, 0x0531);
                Thread.Sleep(2);
                int rawY = RTC6Wrap.get_value(2);

                // 20비트 수치 → mm 단위 변환 (1 unit = 1/65536 mm)
                x_mm = rawX / 65536.0;
                y_mm = rawY / 65536.0;

                //Log.Write("Rtc6", $"Scanner Position → X: {x_mm:F3} mm, Y: {y_mm:F3} mm");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            return true;
        }

        // DrawCalibrationCrosses, DrawCross, DrawCalibrationArc, DrawArc 등 기존 메서드들은 그대로 유지됨
        // ... (기존 코드 생략)

    }
}