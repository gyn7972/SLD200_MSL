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
using System.ServiceModel.Syndication;
using static QMC.Common.Q_Sequence.Sequence_VerifyScannerCameraOffset;
using QMC.Core;
using System.Windows.Forms;



namespace QMC.Common.Parts
{
    //internal class SpiralLabScanner : Part
    public class SpiralLabScanner : Part
    {
        public class ScannerLaserSetting
        {
            // Laser Condition
            public float PowerPercent { get; set; } = 5f;  // 나중에 analog로 변환 가능
            public float Frequency { get; set; } = 5000.0f;    // kHz

            private float _pulseWidth = 1.0f;  // µs
            public float PulseWidth
            {
                get => _pulseWidth;
                set => _pulseWidth = value;
            }
            public float DutyCycle
            {
                get => Frequency > 0 ? (PulseWidth / (1_000_000f / Frequency)) * 100f : 0f;
                set
                {
                    if (Frequency > 0)
                    {
                        float periodSeconds = 1f / Frequency;
                        float pulseWidthSeconds = (value / 100f) * periodSeconds;

                        PulseWidth = pulseWidthSeconds * 1_000_000f;// * 10f; // μs + 보정 ×10
                    }
                }
            }
            // Delay
            public float LaserOnDelay { get; set; } = 0f;
            public float LaserOffDelay { get; set; } = 0f;
            public float MarkDelay { get; set; } = 0f;
            public float JumpDelay { get; set; } = 200f;
            public float PolygonDelay { get; set; } = 0f;

            // Speed
            public float JumpSpeed { get; set; } = 1000f;    // mm/s
            public float MarkSpeed { get; set; } = 100f;     // mm/s

            // 기타
            public bool EnableCrossCheck { get; set; } = false;
            public string CalibrationName { get; set; } = string.Empty;

            public int PowerMeterType {get; set;} = 0;
            public int MaskIndex { get; set;} = 4;
            public int BETIndex { get; set;} = 0;
            public int Duration { get; set; } = 0;

            public float PowerLimitMin_Top { get; set; } = 0f; // 레이저 출력 제한 (0 = 제한 없음)
            public float PowerLimitMax_Top { get; set; } = 0f; // 레이저 출력 제한 (0 = 제한 없음)
            public float PowerLimitMin_Stage { get; set; } = 0f; // 레이저 출력 제한 (0 = 제한 없음)
            public float PowerLimitMax_Stage { get; set; } = 0f; // 레이저 출력 제한 (0 = 제한 없음)


            public ScannerLaserSetting Clone()
            {
                return (ScannerLaserSetting)this.MemberwiseClone();
            }

            public void Initialize()
            {
                PowerPercent = 10f;
                Frequency = 5000.0f;    // kHz
                PulseWidth = 10.0f;   // µs
                DutyCycle = 0f; // 초기값 설정
                LaserOnDelay = 0f;
                LaserOffDelay = 0f;
                MarkDelay = 0f;
                JumpDelay = 200f;
                PolygonDelay = 0f;
                JumpSpeed = 1000f;    // mm/s
                MarkSpeed = 100f;     // mm/s
                EnableCrossCheck = false;
                CalibrationName = string.Empty;

                PowerMeterType = 0;
                MaskIndex = 4;
                BETIndex = 0;
                Duration = 5000;

                PowerLimitMin_Top = 0; // 레이저 출력 제한 (0 = 제한 없음)
                PowerLimitMax_Top = 0; // 레이저 출력 제한 (0 = 제한 없음)
                PowerLimitMin_Stage = 0; // 레이저 출력 제한 (0 = 제한 없음)
                PowerLimitMax_Stage = 0; // 레이저 출력 제한 (0 = 제한 없음)
            }

            public bool LoadPowerMeterConfig()
            {
                bool bRtn = true;

                string iniPath = ConfigManager.GetConfigPath() + "\\ConfigFile(Do not delete or modify).ini";
                StringBuilder temp = new StringBuilder(255);

                Initialize();              //초기화 후 Data Load.

                NativeMethods.GetPrivateProfileString("Laser", "TargetTypeIndex", "0", temp, 255, iniPath);
                PowerMeterType = Equipment.ToInt(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "Duration", "100000", temp, 255, iniPath);
                Duration = Equipment.ToInt(temp.ToString());

                if (Equipment.Machine_LaserType_CO2)
                {
                    NativeMethods.GetPrivateProfileString("Laser", "Frequency", "7000", temp, 255, iniPath);
                    Frequency = (float)Equipment.ToDouble(temp.ToString());

                    NativeMethods.GetPrivateProfileString("Laser", "PulseWidth", "1", temp, 255, iniPath);
                    PulseWidth = (float)Equipment.ToDouble(temp.ToString());

                    NativeMethods.GetPrivateProfileString("Laser", "DutyCycle", "1", temp, 255, iniPath);
                    DutyCycle = (float)Equipment.ToDouble(temp.ToString());

                    NativeMethods.GetPrivateProfileString("Laser", "MaskIndex", "1", temp, 255, iniPath);
                    MaskIndex = Equipment.ToInt(temp.ToString());

                    NativeMethods.GetPrivateProfileString("Laser", "BetIndex", "1", temp, 255, iniPath);
                    BETIndex = Equipment.ToInt(temp.ToString());
                }
                else
                {
                    NativeMethods.GetPrivateProfileString("Laser", "PowerPercent", "10", temp, 255, iniPath);
                    PowerPercent = (float)Equipment.ToDouble(temp.ToString());

                    NativeMethods.GetPrivateProfileString("Laser", "Frequency", "500000", temp, 255, iniPath);
                    Frequency = (float)Equipment.ToDouble(temp.ToString());

                    NativeMethods.GetPrivateProfileString("Laser", "PulseWidth", "1", temp, 255, iniPath);
                    PulseWidth = (float)Equipment.ToDouble(temp.ToString());
                }

                NativeMethods.GetPrivateProfileString("Laser", "PowerLimitMin_Top", "0", temp, 255, iniPath);
                PowerLimitMin_Top = (float)Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString("Laser", "PowerLimitMax_Top", "0", temp, 255, iniPath);
                PowerLimitMax_Top = (float)Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString("Laser", "PowerLimitMin_Stage", "0", temp, 255, iniPath);
                PowerLimitMin_Stage = (float)Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString("Laser", "PowerLimitMax_Stage", "0", temp, 255, iniPath);
                PowerLimitMax_Stage = (float)Equipment.ToDouble(temp.ToString());

                return bRtn;
            }

        }


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
        public LaserVirtual laser { set; get; }
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
                    //Dispose(true);
                    // 관리되는 리소스 해제
                    if (rtc != null && rtc is IDisposable dRtc)
                        dRtc.Dispose();

                    rtc = null;

                    // 비관리 리소스 해제 (필요시 여기에 작성)
                    IsInitialized = false;

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
        private const double logDeltaThreshold = 0.1; // 로그 갱신 임계값 (°C 단위) //1도 이상 바뀌면 로그 남김.

        public bool IsOverTemperatureWarning()
        {
            if (rtc == null)
                return false;

            bool isOverTemp = false;
            try
            {
                //var Rtc6 = rtc as rtc.RTC6Import.RTC6Wrap;

                var rtcMeasurement = rtc as IRtcMeasurement;
                if (rtcMeasurement == null)
                {
                    Log.Write("Rtc6", "RTC6 Measurement interface not implemented.");
                    return false;
                }
                // --- 온도 측정 명령어 ---
                // PCB_Temp
                RTC6Wrap.control_command(1, 1, 0x0514);
                Thread.Sleep(10);
                RTC6Wrap.control_command(1, 2, 0x0514);
                Thread.Sleep(10);
                int nPCBTemp1 = RTC6Wrap.get_value(1);
                int nPCBTemp2 = RTC6Wrap.get_value(2);
                double pcbTemp1 = (nPCBTemp1 >> 4) / 10.0; // °C
                double pcbTemp2 = (nPCBTemp2 >> 4) / 10.0;

                Thread.Sleep(10);

                // Galvo_Temp
                RTC6Wrap.control_command(1, 1, 0x0515);
                Thread.Sleep(10);
                RTC6Wrap.control_command(1, 2, 0x0515);
                Thread.Sleep(10); // 10 ms wait
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

        public bool LaserSetting()
        {
            if (rtc == null)
                return false;

            try
            {
                if (rtc.CtlGetStatus(RtcStatus.Busy))
                    return false;

                bool success = true;

                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 0f;
                if (fPulseWidth <= 0) fPulseWidth = 0f;

                if (!rtc.ListFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }

                float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
                float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
                float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
                float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
                float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
                if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
                if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
                if (fMarkDelay <= 0) fMarkDelay = 0;
                if (fJumpDelay <= 0) fJumpDelay = 200;
                if (fPolygonDelay <= 0) fPolygonDelay = 0;

                if (!rtc.ListDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                    return false;
                }

                float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
                float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
                if (fJumpSpeed <= 0) fJumpSpeed = 0;
                if (fMarkSpeed <= 0) fMarkSpeed = 0;

                if (!rtc.ListSpeed(fJumpSpeed, fMarkSpeed))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            return true;
        }

        public bool LaserOn(float durationMsec, ScannerLaserSetting setting)
        {
            if (rtc == null)
                return false;

            try
            {
                if (rtc.CtlGetStatus(RtcStatus.Busy))
                    return false;

                string strTemp = string.Empty;
                bool success = true;


                //int m_nSDC_Count = 0;
                //do
                //{
                //    // Spot Distance Control
                //    var alc = rtc as IRtcAutoLaserControl;
                //    success = alc.CtlAutoLaserControl<float>(AutoLaserControlSignal.Disabled, AutoLaserControlMode.Disabled, 0, 0, 0);
                //    if (!success)
                //    {
                //        strTemp = string.Format("CtlAutoLaserControl(null, null) 파라미터 적용 실패, ({0}/3)", m_nSDC_Count + 1);
                //        Log.Write("SLD-200", "Auto Run", strTemp);
                //    }
                //    else
                //    {
                //        strTemp = string.Format("CtlAutoLaserControl(null, null) 파라미터 적용 성공, ({0}/3)", m_nSDC_Count + 1);
                //        Log.Write("SLD-200", "Auto Run", strTemp);
                //    }

                //    m_nSDC_Count++;
                //} while (!success && (m_nSDC_Count < 3));
                //rtc.CtlFrequency(setting.Frequency, 2);
                //rtc.CtlLaserOn();

                rtc.CtlLaserMode(LaserMode.Co2);    //Laser On은... Co2가 맞겠지? Yag1

                success &= rtc.ListBegin(laser, ListType.Single);

                float fFrequency = (float)Math.Max(setting.Frequency, 0);
                //float fPulseWidth = (float)Math.Min(setting.PulseWidth, fFrequency / 2);
                float fPulseWidth = setting.PulseWidth;
                success &= rtc.ListFrequency(fFrequency, fPulseWidth);

                success &= rtc.ListDelay(
                            Math.Max(setting.LaserOnDelay, 0),
                            Math.Max(setting.LaserOffDelay, 0),
                            Math.Max(setting.MarkDelay, 0),
                            Math.Max(setting.JumpDelay, 200),
                            Math.Max(setting.PolygonDelay, 0));

                success &= rtc.ListSpeed(
                            Math.Max(setting.JumpSpeed, 200),
                            Math.Max(setting.MarkSpeed, 100));

                success &= rtc.ListJump(0, 0);
                success &= rtc.ListLaserOn(durationMsec); // 레이저 켜기:단위 msec

                if (!success)
                    return false;

                success &= rtc.ListEnd();
                if (success)
                    success &= rtc.ListExecute(false);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            return true;
        }

        public bool LaserOff()
        {
            if (rtc == null)
                return false;
            try
            {
                if (rtc.CtlGetStatus(RtcStatus.Busy))
                    return false;
                bool success = rtc.ListBegin(laser, ListType.Single);
                success &= rtc.ListLaserOff();
                success &= rtc.ListEnd();
                if (success)
                    success &= rtc.ListExecute(true);
                return success;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        public void LaserAbort()
        {
            rtc.CtlAbort();
            Thread.Sleep(2000);
            rtc.CtlReset();
        }


        // Todo : 아래 함수. SpiralLabScanner 정상 동작하면 지우고 옮기자.
        public bool DrawCalibrationCrosses(int rows, int cols, float pitchX, float pitchY, double markLength = 0.5)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            bool bRtn = false;

            float crossSize = (float)Equipment.Scanner_Calibration_CrossMarkLength; //1.0f; //(float)markLength; 

            //if (!Equipment.Machine_LaserType_CO2)
            //{
            //    float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
            //    float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

            //    if (fFrequency / 2 <= fPulseWidth)
            //        fPulseWidth = fFrequency / 2;
            //    if (fFrequency <= 0) fFrequency = 0f;
            //    if (fPulseWidth <= 0) fPulseWidth = 0f;

            //    if (!rtc.CtlFrequency(fFrequency, fPulseWidth))
            //    {
            //        Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
            //        return false;
            //    }
            //}

            //float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            //float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            //if (fJumpSpeed <= 0) fJumpSpeed = 0;
            //if (fMarkSpeed <= 0) fMarkSpeed = 0;

            //if (!rtc.CtlSpeed(fJumpSpeed, fMarkSpeed))
            //{
            //    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
            //    return false;
            //}

            //float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            //float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            //float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            //float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            //float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            //if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            //if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            //if (fMarkDelay <= 0) fMarkDelay = 0;
            //if (fJumpDelay <= 0) fJumpDelay = 200;
            //if (fPolygonDelay <= 0) fPolygonDelay = 0;

            //if (!rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            //{
            //    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
            //    return false;
            //}

            var rtcMode = rtc as IRtc;

            rtcMode.CtlLaserMode(LaserMode.Yag1);

            rtcMode.ListBegin(laser, ListType.Auto);

            //if (Equipment.Machine_LaserType_CO2)
            {
                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 0f;
                if (fPulseWidth <= 0) fPulseWidth = 0f;

                if (!rtcMode.ListFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }
            }

            float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            if (fMarkDelay <= 0) fMarkDelay = 0;
            if (fJumpDelay <= 0) fJumpDelay = 200;
            if (fPolygonDelay <= 0) fPolygonDelay = 0;

            if (!rtcMode.ListDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                return false;
            }

            float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            if (fJumpSpeed <= 0) fJumpSpeed = 0;
            if (fMarkSpeed <= 0) fMarkSpeed = 0;

            if (!rtcMode.ListSpeed(fJumpSpeed, fMarkSpeed))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                return false;
            }

            // 중심 기준 좌표로 시작점 계산
            float startX = -((cols - 1) * pitchX) / 2.0f;
            float startY = -((rows - 1) * pitchY) / 2.0f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = startX + col * pitchX;
                    float centerY = startY + row * pitchY;

                    DrawCross(centerX, centerY, crossSize);
                }
            }

            rtcMode.ListEnd();
            rtcMode.ListExecute();

            bRtn = true;
            return bRtn;
        }

        /// <summary>
        /// 주어진 중심 좌표에 1mm 크기의 십자가를 그리는 함수
        /// </summary>
        /// <param name="centerX">십자가 중심의 X 좌표</param>
        /// <param name="centerY">십자가 중심의 Y 좌표</param>
        /// <param name="size">십자가의 크기 (mm)</param>
        private void DrawCross(float centerX, float centerY, float size)
        {
            var rtcMode = rtc as IRtc;

            float halfSize = size / 2;

            // 가로선 그리기
            rtcMode.ListJump(centerX - halfSize, centerY);
            rtcMode.ListMark(centerX + halfSize, centerY);

            // 세로선 그리기
            rtcMode.ListJump(centerX, centerY - halfSize);
            rtcMode.ListMark(centerX, centerY + halfSize);
        }


        public bool DrawCalibrationArc(int rows, int cols, float pitchX, float pitchY)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            bool bRtn = false;

            float crossSize = (float)Equipment.Scanner_Calibration_CrossMarkLength; //1.0f; //(float)markLength; 

            rtc.CtlLaserMode(LaserMode.Co2);

            if (Equipment.Machine_LaserType_CO2)
            {
                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 0f;
                if (fPulseWidth <= 0) fPulseWidth = 0f;

                if (!rtc.CtlFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }
            }

            float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            if (fJumpSpeed <= 0) fJumpSpeed = 0;
            if (fMarkSpeed <= 0) fMarkSpeed = 0;

            if (!rtc.CtlSpeed(fJumpSpeed, fMarkSpeed))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                return false;
            }

            float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            if (fMarkDelay <= 0) fMarkDelay = 0;
            if (fJumpDelay <= 0) fJumpDelay = 200;
            if (fPolygonDelay <= 0) fPolygonDelay = 0;

            if (!rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                return false;
            }

            rtc.ListBegin(laser, ListType.Auto);
            // 중심 기준 좌표로 시작점 계산
            float startX = -((cols - 1) * pitchX) / 2.0f;
            float startY = -((rows - 1) * pitchY) / 2.0f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = startX + col * pitchX;
                    float centerY = startY + row * pitchY;

                    // DrawArc 호출: 중심 좌표와 반지름, 시작 각도, 끝 각도를 전달
                    float radius = crossSize / 2.0f; // 반지름은 crossSize의 절반으로 설정
                    float startAngle = 0.0f;         // 시작 각도 (0도)
                    float endAngle = 360.0f;         // 끝 각도 (360도, 완전한 원)

                    DrawArc(centerX, centerY, radius, startAngle, endAngle);

                    //radius만 가지고 구하기.
                    //PointD pointD = new PointD(centerX, centerY);
                    //PointD startPoint = new PointD(centerX - radius, centerY);
                    //PointD endPoint = new PointD(centerX + radius, centerY);
                    //var angles = CalculateAngles(pointD, startPoint, endPoint);
                    //// DrawArc 호출: 중심 좌표와 반지름, 시작 각도, 끝 각도를 전달
                    //DrawArc(centerX, centerY, radius, (float)angles.startAngle, (float)angles.endAngle);

                }
            }

            rtc.ListEnd();
            rtc.ListExecute();
            bRtn = true;
            return bRtn;
        }

        /// <summary>
        /// FOV기준으로 cal 진행시 사용 함수.
        /// <summary>
        public bool DrawCalibrationArc(float fovWidth, float fovHeight, int rows, int cols, out float pitchX, out float pitchY)
        {
            pitchX = 0;
            pitchY = 0;

            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            bool bRtn = false;
            float crossSize = (float)Equipment.Scanner_Calibration_CrossMarkLength;  //1.0f; // 각 원의 지름을 1mm로 설정

            // pitch 자동 계산
            pitchX = (cols > 1) ? fovWidth / (cols - 1) : 0;
            pitchY = (rows > 1) ? fovHeight / (rows - 1) : 0;

            rtc.CtlLaserMode(LaserMode.Co2);

            if (Equipment.Machine_LaserType_CO2)
            {
                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 5000;
                if (fPulseWidth <= 0) fPulseWidth = 2.6f;

                if (!rtc.CtlFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }
            }

            float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            if (fJumpSpeed <= 0) fJumpSpeed = 0;
            if (fMarkSpeed <= 0) fMarkSpeed = 0;

            if (!rtc.CtlSpeed(fJumpSpeed, fMarkSpeed))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                return false;
            }

            float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            if (fMarkDelay <= 0) fMarkDelay = 0;
            if (fJumpDelay <= 0) fJumpDelay = 200;
            if (fPolygonDelay <= 0) fPolygonDelay = 0;

            if (!rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                return false;
            }

            rtc.ListBegin(laser, ListType.Auto);

            float startX = -((cols - 1) * pitchX) / 2.0f;
            float startY = -((rows - 1) * pitchY) / 2.0f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = startX + col * pitchX;
                    float centerY = startY + row * pitchY;

                    float radius = crossSize / 2.0f;
                    DrawArc(centerX, centerY, radius, 0.0f, 360.0f);
                }
            }

            rtc.ListEnd();
            rtc.ListExecute();
            bRtn = true;
            return bRtn;
        }

        private void DrawArc(float centerX, float centerY, float radius, float startAngle, float endAngle)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius must be greater than zero.");

            // 원호의 sweepAngle 계산
            float sweepAngle = endAngle - startAngle;

            // 원호 시작점 계산.
            float startX = centerX + radius * (float)Math.Cos(startAngle * Math.PI / 180.0);
            float startY = centerY + radius * (float)Math.Sin(startAngle * Math.PI / 180.0);

            // 먼저 원호의 시작점으로 점프
            if (!rtc.ListJump(startX, startY))
            {
                throw new InvalidOperationException("Failed to jump to arc start position.");
            }

            // rtc.ListArc 호출
            if (!rtc.ListArc(centerX, centerY, sweepAngle))
            {
                throw new InvalidOperationException("Failed to draw arc using rtc.ListArc.");
            }
        }

        public static (double startAngle, double endAngle) CalculateAngles(PointD center, PointD startPoint, PointD endPoint)
        {
            // 시작 각도 계산
            double startAngle = Math.Atan2(startPoint.Y - center.Y, startPoint.X - center.X) * (180.0 / Math.PI);

            // 끝 각도 계산
            double endAngle = Math.Atan2(endPoint.Y - center.Y, endPoint.X - center.X) * (180.0 / Math.PI);

            // 각도를 0~360 범위로 변환
            if (startAngle < 0) startAngle += 360;
            if (endAngle < 0) endAngle += 360;

            return (startAngle, endAngle);
        }

        // Todo: 구영남 Cal파일 넣기 함수 만들것.
        public bool LoadCorrectionData(int tableIndex, string filePath)
        {
            // CorrectionData를 처리하는 메서드
            // 초기화를 해야 하냐 말아야 하냐.. 
            // 초기화를 안하고 cal파일이 들어가면 땡큐인데.. 될꺼같다.. select도 있으니깐..
            CorrectionTableIndex index = (CorrectionTableIndex)tableIndex;
            bool bRtn = rtc.CtlLoadCorrectionFile(index, filePath);   //  Sirius1
            if (bRtn == false)
            {
                //MessageBox.Show("파일을 적용하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Write("SLD-200", "LoadCorrectionData", "파일을 적용하지 못했습니다.");
                return false;
            }
            //cal 파일 선택하여 사용한다.
            rtc.CtlSelectCorrection(index, 0);
            return true;
        }

        /// <summary>
        /// 위치 좌표값 리스트로부터 배열의 Rows와 Columns를 계산합니다.
        /// </summary>
        public (int Rows, int Columns) CalculateArraySize(List<PointD> positions)
        {
            if (positions == null || positions.Count == 0)
            {
                MessageBox.Show("좌표값 리스트가 비어 있습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 허용 오차 설정 (예: 0.01)
            double tolerance = 0.01;

            // X, Y 좌표값을 정렬하여 고유한 값 추출 (허용 오차 적용)
            var uniqueX = positions.Select(p => Math.Round(p.X / tolerance) * tolerance).Distinct().OrderBy(x => x).ToList();
            var uniqueY = positions.Select(p => Math.Round(p.Y / tolerance) * tolerance).Distinct().OrderBy(y => y).ToList();

            // 가로(Columns)와 세로(Rows) 계산
            int columns = uniqueX.Count;
            int rows = uniqueY.Count;

            return (rows, columns);
        }
    }
}