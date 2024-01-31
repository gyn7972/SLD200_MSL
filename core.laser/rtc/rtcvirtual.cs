using System;
using System.Diagnostics;
using RTC5Import;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    /// <summary>
    /// rtc virtual class (used for mark path simulation)
    /// </summary>
    public class RtcVirtual 
        : IRtc
    {
        /// <summary>
        /// 식별번호
        /// </summary>
        public uint Index { get; protected set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// bits/mm
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

        protected StreamWriter stream;
        protected string outputFileName;
        protected bool isAborted;
        bool disposed = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="index">RTC카드의 식별자 (0,1,2,...)</param>
        /// <param name="name">이름(alias name)</param>
        /// <param name="outputFileName">시뮬레이션 출력 텍스트 파일 이름 </param>
        public RtcVirtual(uint index, string name, string outputFileName)
        {
            this.Index = index;
            this.Name = name;
            this.CorrectionFile = string.Empty;
            this.MatrixStack = new MatrixStack();
            this.outputFileName = outputFileName;
        }
        ~RtcVirtual()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
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
            if (disposing)
            {
            }
            this.disposed = true;
        }
        public bool Initialize(double kFactor, string correctionFileName, LaserMode laserMode = LaserMode.Yag1, SignalLevel signalLevel = SignalLevel.ActiveHigh)
        {
            Debug.Assert(kFactor > 0);
            return true;
        }
        #region 컨트롤 명령
        public bool CtlLoadCorrectionFile(string correctionFileName)
        {
            Debug.Assert(!string.IsNullOrEmpty(correctionFileName));
            if (this.CtlGetStatus(RtcStatus.Busy))
                return false;
            this.CorrectionFile = correctionFileName;
            return true;
        }
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
           
            return true;
        }                                       
        public bool CtlWriteData<T>(ExtensionChannel ch, T value)
        {          
            return true;
        }
        public bool CtlLaserControl(PowerXFactor powerXFactor, double powerXValue)
        {
            return true;
        }
        /// <summary>
        /// RTC 카드 상태 조회
        /// </summary>
        /// <param name="status">RtcStatus 열거형 타입</param>
        /// <returns></returns>
        public bool CtlGetStatus(RtcStatus status)
        {
            bool result = false;
            uint busy = 0, position = 0;
            switch (status)
            {
                case RtcStatus.Busy:
                case RtcStatus.List1Busy:
                case RtcStatus.List2Busy:
                    result = false;
                    break;
                case RtcStatus.NotBusy:
                case RtcStatus.NoError:
                    result = true;
                    break;
                case RtcStatus.Aborted:
                    result = this.isAborted;
                    break;
                case RtcStatus.PositionAckOK:
                case RtcStatus.PowerOK:
                case RtcStatus.TempOK:
                    result = true;
                    break;
            }
            return result;
        }        
        /// <summary>
        /// 리스트 명령이 끝날때 까지 대기 (blocking)
        /// </summary>
        /// <returns></returns>
        protected bool CtlBusyWait()
        {
            System.Threading.Thread.Sleep(500);            
            return true;
        }
        /// <summary>
        /// 실행중인 리스트 명령을 중단
        /// </summary>
        /// <returns></returns>
        public bool CtlAbort()
        {
            this.isAborted = true;

            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = null;
            return true;
        }
        /// <summary>
        /// 중단된 상태를 해제
        /// </summary>
        /// <returns></returns>
        public bool CtlReset()
        {
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

            if (!string.IsNullOrEmpty(this.outputFileName))
            {
                this.stream?.Dispose();
                this.stream = new StreamWriter(this.outputFileName);
            }
            stream?.WriteLine($"; LIST HAS BEGAN : {DateTime.Now.ToString()}");
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

            stream?.WriteLine($"FREQUENCY_HZ = {frequency:F3}");
            stream?.WriteLine($"PULSE_WIDTH_US = {pulseWidth:F3}");
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
            stream?.WriteLine($"LASER_ON_DELAY_US = {laserOn:F3}");
            stream?.WriteLine($"LASER_OFF_DELAY_US = {laserOff:F3}");
            stream?.WriteLine($"SCANNER_JUMP_DELAY_US = {scannerJump:F3}");
            stream?.WriteLine($"SCANNER_MARK_DELAY_US = {scannerMark:F3}");
            stream?.WriteLine($"SCANNER_POLYGON_DELAY_US = {scannerPolygon:F3}");
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
            stream?.WriteLine($"SCANNER_JUMP_SPEED_MM_S = {jump:F3}");
            stream?.WriteLine($"SCANNER_MARK_SPEED_MM_S = {mark:F3}");
            return true;
        }
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정위치로 점프
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        public bool ListJumpTo(double x, double y, double weight = 1.0)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            MatrixStack.CalculateVector((float)x, (float)y, out float xOut, out float yOut);
            stream?.WriteLine($"JUMP_TO = {xOut:F3}, {yOut:F3}");
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
            stream?.WriteLine($"MARK_TO = {xOut:F3}, {yOut:F3}");
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
            MatrixStack.CalculateVector((float)cx, (float)cy, out float cxOut, out float cyOut);
            stream?.WriteLine($"ARC_BY_CENTER = {cxOut:F3}, {cxOut:F3}, SWEEP_ANGLE = {sweepAngle:F3}");
            return true;
        }
        public bool ListPixelLine(double usec, ExtensionChannel ext, double dx, double dy, uint pixelCount)
        {
            Debug.Assert(usec < 65535 * 10);
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            MatrixStack.CalculateVector((float)dx, (float)dy, out float dxOut, out float dyOut);
            stream?.WriteLine($"PIXEL_LINE = {usec:F1} usec, ext= {ext.ToString()}, dx= {dxOut:F3}, dy= {dyOut:F3} mm");
            return true;
        }
        public bool ListPixel(double usec, double voltage = 0.0f)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            //stream?.WriteLine($"PIXEL = {usec:F1} usec, {voltage:F3} v");
            return true;
        }
        public bool ListWriteData<T>(ExtensionChannel ch, T value)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            stream?.WriteLine($"WRITE_DATA = ext= {ch.ToString()}, value= {value.ToString()}");
            return true;
        }

        public bool ListLaserControl(PowerXFactor powerXFactor, double powerXValue)
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            stream?.WriteLine($"LASER_CONTROL = factor= {powerXFactor.ToString()}, value= {powerXValue:F3}");
            return true;
        }
        /// <summary>
        /// 리스트 버퍼 명령 기록 완료
        /// </summary>
        /// <returns></returns>
        public bool ListEnd()
        {
            if (this.CtlGetStatus(RtcStatus.Aborted))
                return false;
            stream?.WriteLine($"; LIST ENDED");
            stream?.WriteLine(Environment.NewLine);
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
            stream?.WriteLine($"; LIST EXECUTE : {DateTime.Now.ToString()}");
            stream?.WriteLine(Environment.NewLine);
            if (busyWait)
                this.CtlBusyWait();

            this.stream?.Flush();
            this.stream?.Dispose();
            this.stream = null;
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
