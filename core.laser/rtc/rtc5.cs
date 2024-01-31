using System;
using System.Diagnostics;
using RTC5Import;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    /// <summary>
    /// 스캔랩 RTC5 스캐너 객체
    /// </summary>
    public class Rtc5
        : IRtc
    {
        #region 공개 속성
        /// <summary>
        /// RTC 카드의 식별자 (0,1,2...)
        /// </summary>
        public uint Index { get; protected set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// KFactor = bits/mm
        /// </summary>
        public double KFactor { get; protected set; }
        /// <summary>
        /// 스캐너의 논리적인 FOV 크기
        /// </summary>
        public double Fov
        {
            get { return this.KFactor / Math.Pow(2, 20); }
        }
        /// <summary>
        /// 보정 파일 이름 (correction 폴더에서의 상대적 경로)
        /// </summary>
        public string CorrectionFile { get; protected set; }
        /// <summary>
        /// 3x3 행렬 스택
        /// </summary>
        public MatrixStack MatrixStack { get; protected set; }
        #endregion

        #region 비공개 속성
        /// <summary>
        /// 강제 종료 여부
        /// </summary>
        protected bool isAborted;
        /// <summary>
        /// 현재 동작중인 리스트 번호 (1,2)
        /// </summary>
        protected uint listIndex;
        /// <summary>
        /// 리스트 내의 명령 개수 (임시 변수)
        /// </summary>
        protected uint listCount;
        /// <summary>
        /// 리스트당 명령을 허용할 개수 제한값
        /// </summary>
        protected const uint RTC5_LIST_BUFFER_MAX = 4000;

        protected double frequency; //hz
        protected double pulseWidth;//usec
        protected PowerXFactor powerXFactor;
        protected double powerXValue;

        /// <summary>
        /// IDisposable 처리용
        /// </summary>
        bool disposed = false;
        static int rtc5Counts = 0;
        #endregion

        #region 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="index">RTC카드의 식별자 (0,1,2,...)</param>
        /// <param name="name">이름(alias name)</param>
        public Rtc5(uint index, string name)
        {
            this.Index = index;
            this.Name = name;
            this.KFactor = 0.0;
            this.CorrectionFile = string.Empty;
            this.MatrixStack = new MatrixStack();
            this.isAborted = false;
            this.listIndex = 1;
            this.listCount = 0;
            Rtc5.rtc5Counts++;
        }
        #endregion

        #region 소멸자및 자원 해제
        /// <summary>
        /// 소멸자
        /// </summary>
        ~Rtc5()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        /// <summary>
        /// IDisposable 인터페이스 구현
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// IDisposable 인터페이스 구현
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                Rtc5.rtc5Counts--;
                if (0 == Rtc5.rtc5Counts)
                    RTC5Wrap.free_rtc5_dll();
            }
            this.disposed = true;
        }
        #endregion

        /// <summary>
        /// RTC5 카드 초기화
        /// </summary>
        /// <param name="kFactor">bits/mm</param>        
        /// <param name="correctionFileName">스캐너 보정 파일 이름 (correction 디렉토리가 루트)</param>        
        /// <param name="laserMode">레이저 출력 핀 동작 방식에 대한 레이저 모드 (Co2, Yag ,...)설정</param>
        /// <param name="signalLevel">레이저 출력 핀의 신호 레벨 설정</param>
        /// <returns></returns>
        public bool Initialize(double kFactor, string correctionFileName, LaserMode laserMode = LaserMode.Yag1, SignalLevel signalLevel= SignalLevel.ActiveHigh)
        {
            Debug.Assert(kFactor > 0);

            uint result = RTC5Wrap.init_rtc5_dll();
            RTC5Wrap.n_stop_execution(this.Index + 1);
            uint error = RTC5Wrap.n_load_program_file(this.Index + 1, string.Empty);
            if (0 != error)
                return false;
            uint lastError = RTC5Wrap.n_get_last_error(this.Index + 1);
            if (0 != lastError)
                RTC5Wrap.n_reset_error(this.Index + 1, lastError);

            uint cardCnt = RTC5Wrap.rtc5_count_cards();
            uint dllVersion = RTC5Wrap.get_dll_version();
            uint hexVersion = RTC5Wrap.get_hex_version();
            uint rtcVersion = RTC5Wrap.get_rtc_version();            
            uint serialNo = RTC5Wrap.n_get_serial_number(this.Index + 1);

            if (!this.CtlLoadCorrectionFile(correctionFileName))
                return false;

            // signal level : laser on : bit #03
            // signal level : laser1/2 : bit #04
            switch (signalLevel)
            {
                case SignalLevel.ActiveHigh:
                    RTC5Wrap.n_set_laser_control(this.Index + 1, 0);
                    break;
                case SignalLevel.ActiveLow:
                    RTC5Wrap.n_set_laser_control(this.Index + 1, (0x01 << 3) | (0x01 << 4));
                    break;
            }
            // laser mode
            RTC5Wrap.n_set_laser_mode(this.Index + 1, (uint)laserMode);
            // fpk to 0
            RTC5Wrap.n_set_firstpulse_killer(this.Index + 1, 0);
            // stand by timing to 0
            RTC5Wrap.n_set_standby(this.Index + 1, 0, 0);
            // maximum size of list 1 and 2
            RTC5Wrap.n_config_list(this.Index + 1, RTC5_LIST_BUFFER_MAX * 2, RTC5_LIST_BUFFER_MAX * 2);

            this.KFactor = kFactor;
            return true;
        }
        
        #region 컨트롤 명령
        /// <summary>
        /// 스캐너 보정 파일 로드(변경)
        /// </summary>
        /// <param name="correctionFileName">보정 파일 이름 (correction 폴더에서의 상대 경로)</param>
        /// <returns></returns>
        public bool CtlLoadCorrectionFile(string correctionFileName)
        {
            Debug.Assert(!string.IsNullOrEmpty(correctionFileName));
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;

            string fullPathName = Path.Combine(Define.CorrectionRootPath, correctionFileName);
            uint error = RTC5Wrap.n_load_correction_file(this.Index + 1, fullPathName, 1, 2);
            if (0 != error)
            {
                switch (error)
                {
                    //case 1:
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: file error (file corrupt or incomplete)"); break;
                    //case 2:                                
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: memory error (DLL-internal, WINDOWS system memory)"); break;
                    //case 3:                                
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: File open error (empty string summitted or file not found)"); break;
                    //case 4:                                
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: DSP memory error"); break;
                    //case 5:                                
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: PCI download error (driver error)"); break;
                    //case 8:                                
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: System driver not found"); break;
                    //case 10:                               
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: Parameter error (incorrect No.)"); break;
                    //case 11:                               
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: Access error : board reserved for another application"); break;
                    //case 12:                               
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: Warning : 3D table or D3 selected but 3D option is not enabled."); break;
                    //case 13:                               
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: Busy error: no download, board is BUSY or INTERNAL BUSY"); break;
                    //case 14:                               
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: PCI upload error (driver error, only applicable for download verification)"); break;
                    //case 15:                               
                    //    Logger.Log(Logger.Type.Error, $"rtc5 [{this.Index}]: Verify error (only applicable for download verification)"); break;
                    default:
                        break;
                }
                return false;
            }
            RTC5Wrap.n_select_cor_table(this.Index + 1, 1, 0);
            this.CorrectionFile = correctionFileName;
            return true;
        }
        /// <summary>
        /// 수동 레이저 출사 시작
        /// </summary>
        /// <returns></returns>
        public bool CtlLaserOn()
        {
            RTC5Wrap.n_laser_signal_on(this.Index + 1);
            return true;
        }
        /// <summary>
        /// 수동 레이저 출사 정지
        /// </summary>
        /// <returns></returns>
        public bool CtlLaserOff()
        {
            RTC5Wrap.n_laser_signal_off(this.Index + 1);
            return true;
        }
        /// <summary>
        /// 스캐너 위치 이동
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        public bool CtlMove(double x, double y)
        {
            this.MatrixStack.CalculateVector((float)x, (float)y, out float xOut, out float yOut);
            int xBits = (int)(xOut * this.KFactor);
            int yBits = (int)(yOut * this.KFactor);
            RTC5Wrap.n_goto_xy(this.Index + 1, xBits, yBits);
            return true;
        }
        /// <summary>
        /// 레이저 타이밍(주파수, 펄스폭) 설정
        /// </summary>
        /// <param name="frequency">Hz</param>
        /// <param name="pulseWidth">usec</param>
        /// <returns></returns>
        public bool CtlFrequency(double frequency, double pulseWidth)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            double period = 1.0 / frequency * (double)1.0e6;
            double halfPeriod = period / 2.0;
            RTC5Wrap.n_set_start_list(this.Index + 1, 1);
            RTC5Wrap.n_set_laser_timing(this.Index + 1,
                (uint)(halfPeriod * 64.0),
                (uint)(pulseWidth * 64.0),
                0,
                0);
            RTC5Wrap.n_set_end_of_list(this.Index + 1);
            RTC5Wrap.n_execute_list(this.Index + 1, 1);
            this.frequency = (float)frequency; //hz
            this.pulseWidth = (float)pulseWidth; //usec
            this.CtlBusyWait();
            return true;
        }
        /// <summary>
        /// 레이저및 스캐너의 지연시간 설정
        /// </summary>
        /// <param name="laserOn">usec</param>
        /// <param name="laserOff">usec</param>
        /// <param name="scannerJump">usec</param>
        /// <param name="scannerMark">usec</param>
        /// <param name="scannerPolygon">usec</param>
        /// <returns></returns>
        public bool CtlDelay(double laserOn, double laserOff, double scannerJump, double scannerMark, double scannerPolygon)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            RTC5Wrap.n_set_start_list(this.Index + 1, 1);
            RTC5Wrap.n_set_scanner_delays(this.Index + 1,
                (uint)(scannerJump / 10.0),
                (uint)(scannerMark / 10.0),
                (uint)(scannerPolygon / 10.0)
                );
            RTC5Wrap.n_set_laser_delays(this.Index + 1,
                (int)(laserOn * 2.0),
                (uint)(laserOff * 2.0)
                );
            RTC5Wrap.n_set_end_of_list(this.Index + 1);
            RTC5Wrap.n_execute_list(this.Index + 1, 1);
            this.CtlBusyWait();
            return true;
        }
        /// <summary>
        /// 스캐너 속도 설정
        /// </summary>
        /// <param name="jump">mm/s</param>
        /// <param name="mark">mm/s</param>
        /// <returns></returns>
        public bool CtlSpeed(double jump, double mark)
        {
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            double jump_bitperms = jump / (double)1.0e3 * this.KFactor;
            double mark_bitperms = mark / (double)1.0e3 * this.KFactor;
            RTC5Wrap.n_set_jump_speed_ctrl(this.Index + 1, jump_bitperms);
            RTC5Wrap.n_set_mark_speed_ctrl(this.Index + 1, mark_bitperms);
            return true;
        }
        /// <summary>
        /// RTC 확장채널에 데이타 쓰기
        /// </summary>
        /// <typeparam name="T">데이타 타입(ExtDO16, ExtDI8O8, ExtDI2O2 : uint, ExtAO1, ExtAO2 : double)</typeparam>
        /// <param name="ch">확장 채널</param>
        /// <param name="value">값</param>
        /// <returns></returns>
        public bool CtlWriteData<T>(ExtensionChannel ch, T value)
        {
            switch (ch)
            {
                case ExtensionChannel.ExtDO16:
                    uint d16Out = (uint)Convert.ChangeType(value, typeof(uint));
                    Debug.Assert(d16Out >= 0x00 && d16Out <= 0xFFFF);
                    RTC5Wrap.n_write_io_port(this.Index + 1, d16Out);        //16bits
                    break;
                case ExtensionChannel.ExtDI8O8:
                    uint d8Out = (uint)Convert.ChangeType(value, typeof(uint));
                    Debug.Assert(d8Out >= 0x00 && d8Out <= 0xFF);
                    RTC5Wrap.n_write_8bit_port(this.Index + 1, d8Out);        //16bits
                    break;
                case ExtensionChannel.ExtDI2O2:
                    uint d2Out = (uint)Convert.ChangeType(value, typeof(uint));
                    Debug.Assert(d2Out >= 0x00 && d2Out <= 0x03);
                    RTC5Wrap.n_set_laser_pin_out(this.Index + 1, d2Out);        //16bits
                    break;
                case ExtensionChannel.ExtAO1:
                    float ao1 = (float)Convert.ChangeType(value, typeof(double));
                    Debug.Assert(ao1 >= 0.0 && ao1 <= 10.0);
                    uint a1Value = (uint)((Math.Pow(2, 12) - 1.0) * ao1 / 10.0);
                    RTC5Wrap.n_write_da_x(this.Index + 1, 1, a1Value);
                    break;
                case ExtensionChannel.ExtAO2:
                    float ao2 = (float)Convert.ChangeType(value, typeof(double));
                    Debug.Assert(ao2 >= 0.0 && ao2 <= 10.0);
                    uint a2Value = (uint)((Math.Pow(2, 12) - 1.0) * ao2 / 10.0);
                    RTC5Wrap.n_write_da_x(this.Index + 1, 2, a2Value);
                    break;
                default:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 레이저 소스측의 파워 출력을 변경
        /// </summary>
        /// <param name="powerXFactor">파워 제어에 사용하는 RTC 출력 인터페이스</param>
        /// <param name="powerXValue">출력 값</param>
        /// <returns></returns>
        public bool CtlLaserControl(PowerXFactor powerXFactor, double powerXValue)
        {
            bool success = true;
            switch (powerXFactor)
            {
                case PowerXFactor.Custom:
                    break;
                case PowerXFactor.Analog1:
                    Debug.Assert(powerXValue >= 0 && powerXValue <= 10);
                    success &= this.CtlWriteData<double>(ExtensionChannel.ExtAO1, powerXValue);
                    break;
                case PowerXFactor.Analog2:
                    Debug.Assert(powerXValue >= 0 && powerXValue <= 10);
                    success &= this.CtlWriteData<double>(ExtensionChannel.ExtAO2, powerXValue);
                    break;
                case PowerXFactor.ExtDO8Bit:
                    Debug.Assert(powerXValue >= 0 && powerXValue <= 255);
                    success &= this.CtlWriteData<uint>(ExtensionChannel.ExtDI8O8, (uint)powerXValue);
                    break;
                case PowerXFactor.PulseWidth:
                    Debug.Assert(this.frequency > 0);
                    Debug.Assert(powerXValue > 0);
                    success &= this.CtlFrequency(this.frequency, powerXValue);
                    break;
                case PowerXFactor.Frequency:
                    Debug.Assert(this.pulseWidth > 0);
                    Debug.Assert(powerXValue > 0);
                    success &= this.CtlFrequency(powerXValue, this.pulseWidth);
                    break;
                case PowerXFactor.ExtDO16:
                    Debug.Assert(powerXValue >= 0 && powerXValue <= 65536);
                    success &= this.CtlWriteData<uint>(ExtensionChannel.ExtDI8O8, (uint)powerXValue);
                    break;
                case PowerXFactor.FocusShift:
                    //Debug.Assert(powerXValue >= -5 && powerXValue <= 5); z-stroke
                    success = false;
                    break;
            }
            this.powerXFactor = powerXFactor;
            this.powerXValue = powerXValue;
            return success;
        }
        /// <summary>
        /// RTC 카드 상태 조회
        /// </summary>
        /// <param name="status">RtcStatus 열거형 타입</param>
        /// <returns></returns>
        public bool CtlGetStatus(RtcStatus status)
        {
            bool result = false;
            uint busy=0, position=0;
            switch (status)
            {
                case RtcStatus.Busy:
                    RTC5Wrap.n_get_status(this.Index + 1, out busy, out position);
                    result = Convert.ToBoolean(busy > 0);
                    break;
                case RtcStatus.NotBusy:
                    result = !this.CtlGetStatus(RtcStatus.Busy);
                    break;
                case RtcStatus.List1Busy:
                    uint l1Status = RTC5Wrap.n_read_status(this.Index + 1);
                    result = Convert.ToBoolean(l1Status & 0x0F);
                    break;
                case RtcStatus.List2Busy:
                    uint l2Status = RTC5Wrap.n_read_status(this.Index + 1);
                    result = Convert.ToBoolean(l2Status & 0x10);
                    break;
                case RtcStatus.NoError:
                    bool aborted = this.CtlGetStatus(RtcStatus.Aborted);
                    uint lastError = RTC5Wrap.n_get_last_error(this.Index + 1);
                    bool error = (0 != lastError);
                    result = !aborted && !error;
                    break;
                case RtcStatus.Aborted:
                    result = this.isAborted;
                    break;
                case RtcStatus.PositionAckOK:
                    uint posAckStatus = RTC5Wrap.n_get_head_status(this.Index + 1, 1);
                    bool xAcked = Convert.ToBoolean(posAckStatus & (0x01 << 3));
                    bool yAcked = Convert.ToBoolean(posAckStatus & (0x01 << 4));
                    result = xAcked && yAcked;
                    break;
                case RtcStatus.PowerOK:
                    uint powStatus = RTC5Wrap.n_get_head_status(this.Index + 1, 1);
                    result = Convert.ToBoolean(powStatus & (0x01 << 7));
                    break;
                case RtcStatus.TempOK:
                    uint tempStatus = RTC5Wrap.n_get_head_status(this.Index + 1, 1);
                    result = Convert.ToBoolean(tempStatus & (0x01 << 6));
                    break;
            }
            return result;
        }
        /// <summary>
        /// RTC 카드의 에러코드 조회용
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        protected string CtlGetErrMsg(uint errorCode)
        {
            if (0 == errorCode)
                return string.Empty;
            uint code = RTC5Wrap.n_get_error(this.Index + 1);
            if (Convert.ToBoolean(code & (0x01 << 0)))
                return ("no rtc board founded via init_rtc_dll");
            if (Convert.ToBoolean(code & (0x01 << 1)))
                return ("access denied via init_rtc_dll, select, acquire_rtc");
            if (Convert.ToBoolean(code & (0x01 << 2)))
                return ("command not forwarded. PCI or driver error");
            if (Convert.ToBoolean(code & (0x01 << 3)))
                return ("rtc timed out. no response from board");
            if (Convert.ToBoolean(code & (0x01 << 4)))
                return ("invalid parameter");
            if (Convert.ToBoolean(code & (0x01 << 5)))
                return ("list processing is (not) active");
            if (Convert.ToBoolean(code & (0x01 << 6)))
                return ("list command rejected, illegal input pointer");
            if (Convert.ToBoolean(code & (0x01 << 7)))
                return ("list command wad converted to a List_mop");
            if (Convert.ToBoolean(code & (0x01 << 8)))
                return ("dll, rtc or hex version error");
            if (Convert.ToBoolean(code & (0x01 << 9)))
                return ("download verification error. load_program_file ?");
            if (Convert.ToBoolean(code & (0x01 << 10)))
                return ("DSP version is too old");
            if (Convert.ToBoolean(code & (0x01 << 11)))
                return ("out of memeory. dll internal windows memory request failed");
            if (Convert.ToBoolean(code & (0x01 << 12)))
                return ("EEPROM read or write error");
            if (Convert.ToBoolean(code & (0x01 << 16)))
                return ("error reading PCI configuration reqister druing init_rtc_dll");
            return ($"unknown error code : {errorCode}");
        }
        /// <summary>
        /// 리스트 명령이 끝날때 까지 대기 (blocking)
        /// </summary>
        /// <returns></returns>
        protected bool CtlBusyWait()
        {
            uint busy=0, position=0;
            do {
                RTC5Wrap.n_get_status(this.Index + 1, out busy, out position);
                System.Threading.Thread.Sleep(1);
            } while (0 != busy);
            return true;
        }
        /// <summary>
        /// 실행중인 리스트 명령을 중단
        /// </summary>
        /// <returns></returns>
        public bool CtlAbort()
        {
            RTC5Wrap.n_stop_execution(this.Index + 1);
            this.isAborted = true;
            return this.CtlGetStatus(RtcStatus.NotBusy);
        }
        /// <summary>
        /// 중단된 상태를 해제
        /// </summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            uint lastError = RTC5Wrap.n_get_last_error(this.Index + 1);
            if (0 != lastError)
                RTC5Wrap.n_reset_error(this.Index + 1, lastError);

            this.isAborted = false;
            return true;
        } 
        #endregion

        #region 리스트 명령
        /// <summary>
        /// 리스트 명령 버퍼 초기화
        /// </summary>
        /// <returns></returns>
        public bool ListBegin()
        {
            Debug.Assert(this.CtlGetStatus(RtcStatus.NotBusy));
            this.CtlReset();
            this.listIndex = 1;
            this.listCount = 0;
            RTC5Wrap.n_set_start_list(this.Index + 1, this.listIndex);
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 대한 더블 버퍼링 처리 로직
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool IsListBufferReady(uint count)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            if ((this.listCount + count) >= RTC5_LIST_BUFFER_MAX)
            {
                uint busy = 0, position = 0;
                RTC5Wrap.n_get_status(this.Index + 1, out busy, out position);
                if (0 != busy)
                {
                    RTC5Wrap.n_set_end_of_list(this.Index + 1);
                    RTC5Wrap.n_execute_list(this.Index + 1, this.listIndex);
                    //Logger.Log(Logger.Module.Laser, Logger.Type.Debug, $"rtc list has executed and switched");
                    this.listIndex = this.listIndex ^ 0x03;
                    RTC5Wrap.n_set_start_list(this.Index + 1, this.listIndex);
                }
                else
                {
                    RTC5Wrap.n_set_end_of_list(this.Index + 1);
                    if (this.CtlGetStatus(RtcStatus.Aborted))
                        return false;
                    //Logger.Log(Logger.Module.Laser, Logger.Type.Debug, $"rtc list has switched by automatically ...");
                    RTC5Wrap.n_auto_change(this.Index + 1);
                    uint readStatus = 0;
                    switch (this.listIndex)
                    {
                        case 1:
                            do {
                                readStatus = RTC5Wrap.n_read_status(this.Index + 1);
                                System.Threading.Thread.Sleep(1);
                            } while (Convert.ToBoolean(readStatus & 0x20));
                            break;
                        case 2:
                            do {
                                readStatus = RTC5Wrap.n_read_status(this.Index + 1);
                                System.Threading.Thread.Sleep(1);
                            } while (Convert.ToBoolean(readStatus & 0x10));
                            break;
                    }
                    if (this.CtlGetStatus(RtcStatus.Aborted))
                        return false;
                    this.listIndex = this.listIndex ^ 0x03;
                    RTC5Wrap.n_set_start_list(this.Index + 1, this.listIndex);
                }
                this.listCount = count;
            }
            else
                this.listCount += count;
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 레이저 주파수, 펄스폭 명령
        /// </summary>
        /// <param name="frequency">Hz</param>
        /// <param name="pulseWidth">usec</param>
        /// <returns></returns>
        public bool ListFrequency(double frequency, double pulseWidth)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double period = 1.0 / frequency * (double)1.0e6;
            double halfPeriod = period / 2.0;
            //if (!this.IsListBufferReady(2))
            //    return false;
            //RTC5Wrap.n_list_nop(this.Index + 1);
            RTC5Wrap.n_set_laser_timing(this.Index + 1,
                (uint)(halfPeriod * 64.0),
                (uint)(pulseWidth * 64.0),
                0, 0);
            this.frequency = (float)frequency; //hz
            this.pulseWidth = (float)pulseWidth; //usec
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 레이저, 스캐너 지연값 명령
        /// </summary>
        /// <param name="laserOn">usec</param>
        /// <param name="laserOff">usec</param>
        /// <param name="scannerJump">usec</param>
        /// <param name="scannerMark">usec</param>
        /// <param name="scannerPolygon">usec</param>
        /// <returns></returns>
        public bool ListDelay(double laserOn, double laserOff, double scannerJump, double scannerMark, double scannerPolygon)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            //if (!this.IsListBufferReady(3))
            //    return false;
            //RTC5Wrap.n_list_nop(this.Index + 1);
            RTC5Wrap.n_set_scanner_delays(this.Index + 1,
                (uint)(scannerJump / 10.0),
                (uint)(scannerMark / 10.0),
                (uint)(scannerPolygon / 10.0)
                );
            RTC5Wrap.n_set_laser_delays(this.Index + 1,
                (int)(laserOn * 2.0),
                (uint)(laserOff * 2.0)
                );
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 스캐너 속도 명령
        /// </summary>
        /// <param name="jump">mm/s</param>
        /// <param name="mark">mm/s</param>
        /// <returns></returns>
        public bool ListSpeed(double jump, double mark)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            double jump_bitperms = jump / (double)1.0e3 * this.KFactor;
            double mark_bitperms = mark / (double)1.0e3 * this.KFactor;
            //if (!this.IsListBufferReady(3))
            //    return false;
            //RTC5Wrap.n_list_nop(this.Index + 1);
            RTC5Wrap.n_set_jump_speed(this.Index + 1, jump_bitperms);
            RTC5Wrap.n_set_mark_speed(this.Index + 1, mark_bitperms);
            return true;
        }      
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정위치로 점프
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        public bool ListJumpTo(double x, double y, double weight=1.0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            MatrixStack.CalculateVector((float)x, (float)y, out float xOut, out float yOut);
            int xBits = (int)(xOut * this.KFactor);
            int yBits = (int)(yOut * this.KFactor);
            //if (!this.IsListBufferReady(1))
            //    return false;
            //RTC5Wrap.n_jump_abs(this.Index + 1, (int)xBits, (int)yBits);
            RTC5Wrap.n_para_jump_abs(this.Index + 1, xBits, yBits, (uint)(weight * this.powerXValue));
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정위치로 마크
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        public bool ListMarkTo(double x, double y, double weight = 1.0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            MatrixStack.CalculateVector((float)x, (float)y, out float xOut, out float yOut);
            int xBits = (int)(xOut * this.KFactor);
            int yBits = (int)(yOut * this.KFactor);
            //if (!this.IsListBufferReady(1))
            //    return false;
            //RTC5Wrap.n_mark_abs(this.Index + 1, (int)xBitsOut, (int)yBitsOut);
            RTC5Wrap.n_para_mark_abs(this.Index + 1, xBits, yBits, (uint)(weight * this.powerXValue));
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정 위치를 중심으로 지정된 회전각도만큼 회전
        /// (호의 시작위치는 마지막 위치가 기준임)
        /// </summary>
        /// <param name="cx">rotate center x (mm)</param>
        /// <param name="cy">rotate center y (mm)</param>
        /// <param name="sweepAngle">degree ( CCW:+, CW:-) </param>
        /// <returns></returns>
        public bool ListArc(double cx, double cy, double sweepAngle, double weight = 1.0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.MatrixStack.CalculateVector((float)cx, (float)cy, out float cxOut, out float cyOut);
            int xBits = (int)(cxOut * this.KFactor);
            int yBits = (int)(cyOut * this.KFactor);
            //if (!this.IsListBufferReady(1))
            //    return false;
            RTC5Wrap.n_arc_abs(this.Index + 1, xBits, yBits, -sweepAngle);
            return true; 
        }
        /// <summary>
        /// Pixel Raster Operation
        /// 시작점은 현재 스캐너 위치 (ListJump 를 호출한후에 사용 한다던지)
        /// </summary>
        /// <param name="usec">매 픽셀당 주기 (usec)</param>
        /// <param name="ext">매 픽셀당 출력강도를 제어할 채널 (None, ExtAO1, ExtAO2 중 선택) </param>
        /// <param name="dx">매 픽셀당 가로 방향 이동거리 (mm)</param>
        /// <param name="dy">매 픽셀당 세로 방향 이동거리 (mm)</param>
        /// <param name="pixelCount">pixel counts per line</param>
        /// <returns></returns>
        public bool ListPixelLine(double usec, ExtensionChannel ext, double dx, double dy, uint pixelCount)
        {
            Debug.Assert(usec < 65535 * 10);
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            this.MatrixStack.CalculateVector((float)dx, (float)dy, out float dxOut, out float dyOut);
            int xBits = (int)(dxOut * this.KFactor);
            int yBits = (int)(dyOut * this.KFactor);
            //if (!this.IsListBufferReady((uint)(2 + pixelCount) + 1))
            //    return false;
            //RTC5Wrap.n_list_nop(this.Index + 1);
            switch (ext)
            {
                case ExtensionChannel.ExtAO1:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1, 1, (uint)(usec / 2.0 * 64.0), xBits, yBits);
                    break;
                case ExtensionChannel.ExtAO2:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1, 2, (uint)(usec / 2.0 * 64.0), xBits, yBits);
                    break;
                default:
                    RTC5Wrap.n_set_pixel_line(this.Index + 1, 0, (uint)(usec / 2.0 * 64.0), xBits, yBits);
                    break;
            }
            return true;
        }
        /// <summary>
        /// Pixel Raster Operation 
        /// 하나의 픽셀에 대한 출력 리스트 명령
        /// </summary>
        /// <param name="usec">한 픽셀에 대한 펄스 출력 시간(ListPixelLine 에서 지정한 주기보다는 당연히 작아야 한다) </param>
        /// <param name="voltage">출력 채널중 아날로그를 사용할 경우 Voltage 값 (0~10V) </param>
        /// <returns></returns>
        public bool ListPixel(double usec, double voltage = 0.0f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            uint value = (uint)((Math.Pow(2, 12) - 1) * voltage / 10.0);
            RTC5Wrap.n_set_pixel(this.Index + 1, (uint)(usec * 64.0), value);
            return true;
        }
        /// <summary>
        /// RTC 확장채널에 데이타 쓰기
        /// </summary>
        /// <typeparam name="T">데이타 타입(ExtDO16, ExtDI8O8, ExtDI2O2 : uint, ExtAO1, ExtAO2 : double)</typeparam>
        /// <param name="ch">확장 채널</param>
        /// <param name="value">값</param>
        /// <returns></returns>
        public bool ListWriteData<T>(ExtensionChannel ch, T value)
        {
            //if (!this.IsListBufferReady(2))
            //    return false;
            //RTC5Wrap.n_list_nop(this.Index + 1);
            switch (ch)
            {
                case ExtensionChannel.ExtDO16:
                    uint d16Out = (uint)Convert.ChangeType(value, typeof(uint));
                    RTC5Wrap.n_write_io_port_list(this.Index, d16Out);
                    break;
                case ExtensionChannel.ExtDI8O8:
                    uint d8Out = (uint)Convert.ChangeType(value, typeof(uint));
                    RTC5Wrap.n_write_8bit_port_list(this.Index, d8Out);
                    break;
                case ExtensionChannel.ExtDI2O2:
                    uint d2Out = (uint)Convert.ChangeType(value, typeof(uint));
                    RTC5Wrap.n_set_laser_pin_out_list(this.Index, d2Out);
                    break;
                case ExtensionChannel.ExtAO1:
                    double ao1 = (double)Convert.ChangeType(value, typeof(double));
                    double ao1_bits = (Math.Pow(2, 12) - 1) * ao1 / 10.0;
                    RTC5Wrap.n_write_da_x_list(this.Index + 1, 1, (uint)ao1_bits);
                    break;
                case ExtensionChannel.ExtAO2:
                    double ao2 = (double)Convert.ChangeType(value, typeof(double));
                    double ao2_bits = (Math.Pow(2, 12) - 1) * ao2 / 10.0;
                    RTC5Wrap.n_write_da_x_list(this.Index + 1, 2, (uint)ao2_bits);
                    break;
                default:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 리스트 명령으로 레이저 파워 제어용 명령쓰기
        /// </summary>
        /// <param name="powerXFactor">파워 변경 방법</param>
        /// <param name="powerXValue">파워 값</param>
        /// <returns></returns>
        public bool ListLaserControl(PowerXFactor powerXFactor, double powerXValue)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            //if (!this.IsListBufferReady(2))
            //    return false;
            //RTC5Wrap.n_list_nop(this.Index + 1);
            bool success = true;
            switch (powerXFactor)
            {
                default:
                case PowerXFactor.Custom:
                    this.powerXValue = powerXValue;
                    break;
                case PowerXFactor.Analog1:
                    success &= this.ListWriteData<double>(ExtensionChannel.ExtAO1, powerXValue);
                    double a1Ovalue = powerXValue / 10.0 * 4095.0; //2^12 bits 
                    this.powerXValue = a1Ovalue;
                    break;
                case PowerXFactor.Analog2:
                    success &= this.ListWriteData<double>(ExtensionChannel.ExtAO2, powerXValue);
                    double a2Ovalue = powerXValue / 10.0 * 4095.0; //2^12 bits 
                    this.powerXValue = a2Ovalue;
                    break;
                case PowerXFactor.ExtDO8Bit:
                    success &= this.ListWriteData<uint>(ExtensionChannel.ExtDI8O8, (uint)powerXValue);
                    double d8Out = powerXValue;//0~255
                    this.powerXValue = d8Out;
                    break;
                case PowerXFactor.PulseWidth:
                    success &= this.ListFrequency(this.frequency, powerXValue);
                    double pw_1_64 = powerXValue / 64.0; // 1/64usec
                    this.powerXValue = pw_1_64;
                    break;
                case PowerXFactor.Frequency:
                    success &= this.ListFrequency(powerXValue, this.pulseWidth);  //hz
                    double period = 1.0f / powerXValue * 1.0e6; //peroid (usec)
                    double halfperiod = period / 2.0;
                    double halfperiod_1_16 = halfperiod / 64.0;
                    this.powerXValue = halfperiod_1_16;
                    break;
                case PowerXFactor.ExtDO16:
                    success &= this.ListWriteData<uint>(ExtensionChannel.ExtDI8O8, (uint)powerXValue);
                    double d16Out = powerXValue;//0~65535
                    this.powerXValue = d16Out;
                    break;
                case PowerXFactor.FocusShift:
                    //    double zDistanceBits = powerXValue * this.KFactor + 32768;    // mm -> bit
                    //    RTC5Wrap.n_set_vector_control(this.Index + 1, (uint)laser.PowerXFactor, (uint)zDistanceBits);
                    //    //none ? success &= ListJump(0,0, powerX); //offset ?
                    //    laserPowerXValue = (float)zDistanceBits;
                    success = false;
                    break;
            }
            this.powerXFactor = powerXFactor;
            return success;
        }
        /// <summary>
        /// 리스트 버퍼 명령 기록 완료
        /// </summary>
        /// <returns></returns>
        public bool ListEnd()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            RTC5Wrap.n_set_end_of_list(this.Index + 1);
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 있는 모든 명령 실행
        /// </summary>
        /// <param name="busyWait">완료될때까지 대기 여부 (blocking)</param>
        /// <returns></returns>
        public bool ListExecute(bool busyWait = true)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            uint busy, position;
            busy = position = 0;
            RTC5Wrap.n_get_status(this.Index + 1, out busy, out position);
            if (busy > 0)
                RTC5Wrap.n_auto_change(this.Index + 1);
            else
                RTC5Wrap.n_execute_list(this.Index + 1, this.listIndex);
            if (busyWait)
                this.CtlBusyWait();
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            return true;
        }
        #endregion

        #region => ScannerCal Draw


        public bool DrawRec(double lengthX, double lengthY)
        {
            this.CtlBusyWait();

            var t = Task.Run(() =>
            {
                this.CtlReset();
                this.ListBegin();
                //this.ListSettings();
                this.ListJumpTo(0, 0);

                this.ListJumpTo(lengthX / 2, lengthY / 2);
                this.ListMarkTo(-lengthX / 2, lengthY / 2);
                this.ListMarkTo(-lengthX / 2, -lengthY / 2);
                this.ListMarkTo(lengthX / 2, -lengthY / 2);
                this.ListPixel(250);

                this.ListJumpTo(0, 0);
                this.ListEnd();
                this.ListExecute();
            }
           );

            return true;
        }


        public bool DrawCalGrid(int cntx, int cnty)
        {
            double pitchX = Fov / (cntx - 1);
            double pitchY = Fov / (cnty - 1);

            this.CtlBusyWait();

            var t = Task.Run(() =>
            {
                this.CtlReset();
                this.ListBegin();
                //this.ListSettings();

                ListJumpTo(0, 0);

                for (int i = 0; i < cntx; i++)
                {
                    ListJumpTo(-Fov / 2, -(pitchY * (cnty - 1) / 2) + pitchY * i);
                    //ListJumpTo(-(pitchX * (cntx - 1) / 2) + pitchX * i, -(pitchY * (cnty - 1) / 2));
                    ListMarkTo(Fov / 2, -(pitchY * (cnty - 1) / 2) + pitchY * i);
                }

                for (int i = 0; i < cnty; i++)
                {
                    ListJumpTo(-(pitchX * (cntx - 1) / 2) + pitchX * i, -Fov / 2);
                    ListMarkTo(-(pitchX * (cntx - 1) / 2) + pitchX * i, Fov / 2);
                }
                //ListPixel(250);

                for (int i = 0; i < cntx; i++)
                {
                    for (int j = 0; j < cnty; j++)
                    {
                        DrawMarkX(-Fov + (pitchX * j), Fov + (pitchY * i), 300);
                    }
                }

                this.ListPixel(250);

                this.ListJumpTo(0, 0);
                this.ListEnd();
                this.ListExecute();
            }
           );

            return true;
        }


        /// <summary>
        ///  Draw X-mark in calibration grid.
        /// </summary>
        public bool DrawMarkX(double coordx, double coordy, double size)
        {
            this.CtlBusyWait();

            var t = Task.Run(() =>
            {
                this.CtlReset();
                this.ListBegin();
                //this.ListSettings();
                this.ListJumpTo(0, 0);

                this.ListJumpTo(coordx + (size / 2), coordy + (size / 2));
                this.ListMarkTo(coordx - (size / 2), coordy - (size / 2));

                this.ListJumpTo(coordx - (size / 2), coordy + (size / 2));
                this.ListMarkTo(coordx + (size / 2), coordy - (size / 2));
                this.ListPixel(250);

                this.ListJumpTo(0, 0);
                this.ListEnd();
                this.ListExecute();
            }
           );

            return true;
        }


        public bool DrawTestLine()
        {
            this.CtlBusyWait();

            var t = Task.Run(() =>
            {
                this.CtlReset();
                this.ListBegin();
                //this.ListSettings();

                this.ListJumpTo(0, 0);
                ListPixelLine(500, ExtensionChannel.ExtAO1, 0.1, 0, 100);
                // --> 가감구간 간견 불일치.
                for (int i = 0; i < 100; i++)
                    ListPixel(100);

                ListJumpTo(0 - 10 * 0.1, 0 + 0.2); //1mm 뒤에서 시작
                ListPixelLine(500, ExtensionChannel.ExtAO1, 0.1, 0, 100 + 10);

                for (int i = 0; i < 100 + 10; i++)
                { //--> 가감구간 간격 일치를 위해서.
                    if (i < 10)
                        ListPixel(0);
                    else
                        ListPixel(100);
                }

                this.ListJumpTo(0, 0);
                this.ListEnd();
                this.ListExecute();
            }
           );

            return true;

        }

        /// <summary>
        /// List Frequence, Delay, Speed
        /// using dm? or fixed value,
        /// </summary>
        private void ListSettings()
        {
            double freq = 20000;
            double pulsewidth = 20;
            double laseron = 30;  // 200;
            double laseroff = 60; // 250;
            double scannerjump = 99.5; //200;
            double scannermark = 49.5; // 200;
            double scannerplygon = 25; // 400;
            double jump = 400; //200;
            double mark = 400; // 200;

            //--> list working setting
            ListFrequency(freq, pulsewidth);
            ListDelay(laseron, laseroff, scannerjump, scannermark, scannerplygon);
            ListSpeed(jump, mark);
        }

        #endregion
    }
}
