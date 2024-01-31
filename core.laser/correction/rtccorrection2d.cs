using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using QMC.Core;

namespace QMC.Core.Laser
{
    /// <summary>
    /// scanner correction for 2D plane (Z=0)
    /// </summary>
    public class RtcCorrection2D 
        : ICorrection
    {
        #region 공개 속성
        /// <summary> 
        /// 변환 결과에 대한 이벤트 핸들러
        /// </summary>
        public event ResultEventHandler OnResult;
        /// <summary>
        /// 입력 데이타의 행 개수
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 입력 데이타의 열 개수
        /// </summary>
        public int Cols { get; set; }
        /// <summary>
        /// 입력 보정 파일
        /// </summary>
        public string SourceCorrectionFile { get; set; }
        /// <summary>
        /// 출력 보정 파일
        /// </summary>
        public string TargetCorrectionFile { get; set; }
        /// <summary>
        /// 변환 결과 로그 메시지
        /// </summary>
        public string ResultMessage { get; protected set; } 
        #endregion

        /// <summary>
        /// bits/mm
        /// </summary>
        private double kFactor;

        private double Interval;
        /// <summary>
        /// 입력 데이타를 각각의 행과 열의 위치에 저장하기위한 2차원 배열
        /// </summary>
        protected CorrectionData2D[ , ] Data { get; set; }
        /// <summary>
        /// SCANLAB 의 보정 유틸리티 실행파일 경로
        /// </summary>
        //private readonly string exeFileName = Path.Combine(Define.CorrectionRootPath, "correXionPro.exe");
		private readonly string exeFileName = Path.Combine(Define.CorrectionRootPath, "CorreXion5.exe");
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="kFactor">bits/mm</param>
        /// <param name="rows">행 개수</param>
        /// <param name="cols">열 개수</param>
        /// <param name="interval">간격</param>
        /// <param name="srcCtbFile">입력 보정 파일</param>
        /// <param name="targetCtbFile">출력 보정 파일</param>
        public RtcCorrection2D(double kFactor, int rows, int cols, double interval, string srcCtbFile, string targetCtbFile)
        {
            Debug.Assert(kFactor > 0);
            Debug.Assert(rows > 0);
            Debug.Assert(cols > 0);
            Debug.Assert(interval > 0);
            Debug.Assert(!string.IsNullOrEmpty(srcCtbFile));
            Debug.Assert(!string.IsNullOrEmpty(targetCtbFile));

            this.Rows = rows;
            this.Cols = cols;
            this.Interval = interval;
            this.SourceCorrectionFile = srcCtbFile;
            this.TargetCorrectionFile = targetCtbFile;
            this.kFactor = kFactor;
            this.Data = new CorrectionData2D[Rows, Cols];
        }

        /// <summary>
        /// 측정 데이타 입력 (절대 좌표 값)
        /// 좌상단부터 우상단 방향으로 순서 
        /// 예 :
        /// 1 2 3
        /// 4 5 6
        /// 7 8 9
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">기준 좌표(mm)</param>
        /// <param name="absoulte">측정 절대 좌표 (mm)</param>
        /// <returns></returns>
        public bool AddAbsolute(int row, int col, Vector2 reference, Vector2 absoulte)
        {
            Debug.Assert(this.Data != null);
            this.Data[row, col] = new CorrectionData2D(reference, absoulte);
			//this.Data.Add(absoulte); //-> 변경
            return true;
        }
        /// <summary>
        /// 측정 데이타 입력 (상대 좌표값) 
        /// ex) 상대 좌표값 = 비전 오차량 만큼만 입력
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">기준 좌표값 (mm)</param>
        /// <param name="relative">측정 상대 좌표 (mm)</param>
        /// <returns></returns>
        public bool AddRelative(int row, int col, Vector2 reference, Vector2 relative)
        {
            return this.AddAbsolute(row, col, reference, reference + relative);
        }
        /// <summary>
        /// 입력 데이타 모두 제거
        /// </summary>
        public void Clear()
        {
            this.Data = new CorrectionData2D[Rows, Cols];
        }
        /// <summary>
        /// 변환 
        /// </summary>
        /// <returns></returns>
        public bool Convert()
        {
            #region .dat 파일 생성
            string datFileName = String.Format($"{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.dat");
            string datFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", datFileName);

            using (System.IO.StreamWriter stream = new System.IO.StreamWriter(datFileFullPath))
            {
                stream.WriteLine($"[INPUT]\t= {Path.GetFileNameWithoutExtension(this.SourceCorrectionFile)} ; input table filename");
                stream.WriteLine($"[OUTPUT]\t= {Path.GetFileNameWithoutExtension(this.TargetCorrectionFile)} ; output table filename");
                stream.WriteLine($"[CALIBRATION]\t= {this.kFactor:F8}");
                stream.WriteLine("[RTC4]\t\t= 0");
                stream.WriteLine("[FITORDER]\t= 0");
                stream.WriteLine("[SMOOTHING]\t= -1");
                stream.WriteLine("[AUTO_FIT]\t= 0");
                stream.WriteLine("[TOLERANCE]\t= 0");
                stream.WriteLine("[APPLY_OFFSET]\t= 0");
                stream.WriteLine("");
                stream.WriteLine("[Limit(Bits)]\t= 524288");
                stream.WriteLine($"[Limit(mm)]\t= {Math.Pow(2.0, 19)/this.kFactor}");
                stream.WriteLine($"[OffsetX]\t= 0");
                stream.WriteLine($"[OffsetY]\t= 0");
                stream.WriteLine($"[Deviation]\t= 0");
                stream.WriteLine("");
                stream.WriteLine($"[GRIDNUMBERS]\t= {(int)(this.Rows/2)} {(int)(this.Cols/2)}");

                double left = this.Interval * (int)(this.Cols / 2);
                double top = this.Interval * (int)(this.Rows / 2);
                for (int i = 0; i < this.Cols; ++i)
                {
                    double val = (-left + (this.Interval * i)) * 1000;
                    double fVal = (int)val * 0.001;
                    stream.WriteLine($"[GRIDVALUES_X]=\t{fVal}");
                }
                stream.WriteLine($"");
                for (int i = 0; i < this.Rows; ++i)
                {
                    double val = (-top + (this.Interval * i)) * 1000;
                    double fVal = (int)val * 0.001;
                    stream.WriteLine($"[GRIDVALUES_Y]=\t{fVal}");
                }
                stream.WriteLine($"");
                stream.WriteLine("\tXn\tYn\tX mm\tY mm");
                stream.WriteLine($"");

                int index = 0;
                for (int row = 0; row < this.Rows; row++)
                {
                    for (int col = 0; col < this.Cols; col++)
                    {
                        //stream.WriteLine($"\t{(int)(-this.Cols / 2) + col}\t{(int)(this.Rows / 2) - row}\t{this.Data[index].X}\t{this.Data[index].Y}");
                        stream.WriteLine($"\t{(int)(-this.Cols / 2) + col}\t{(int)(this.Rows / 2) - row}\t{this.Data[row, col].Measured.X}\t{this.Data[row, col].Measured.Y}");
                        index++;
                    }
                }

            }
            #endregion

            #region correXionPro.exe 프로세스 생성및 dat 파일 인자로 전달
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction");
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = exeFileName;   
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = datFileName;
            try
            {
                using (Process proc = Process.Start(startInfo))
                {
                    if (!proc.WaitForExit(5 * 1000))
                    {
                        //Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"correction 2d timed out: {exeFileName}");                        
                        return false;
                    }

                    //switch (proc.ExitCode)
                    //{
                    //    case 10: // old correction file not found. incorrect path or name : {exeFileName}, {datFileName}"); break;
                    //    case 11: // could not open old correction file. access denied : {exeFileName}, {datFileName}"); break;
                    //    case 12: // ct5/ctb file size invalid. invalid file foramt : {exeFileName}, {datFileName}"); break;
                    //    case 14: // could not create new correction file. incorrect path or access denied : {exeFileName}, { datFileName}"); break;
                    //    case 15: // saving ct5/ctb file failed. unable to save : {exeFileName}, {datFileName}"); break;
                    //    case 16: // datafile not found. file access denied : {exeFileName}, {datFileName}"); break;
                    //    case 17: // open datafile failed : {exeFileName}, {datFileName}"); break;
                    //    case 18: // old correction file command missing. oldctfile command missing or incorrect : {exeFileName}, {datFileName}"); break;
                    //    case 20: // invalid file extension. incorrect file extension for original or new correction file : {exeFileName}, {datFileName}"); break;
                    //    case 21: // no fit possible. test point data is possibly incorrect : {exeFileName}, {datFileName}"); break;
                    //    case 23: // not enough memory : {exeFileName}, {datFileName}"); break;
                    //    case 26: // autocalibration failed. try manually specifying a calibration factor : {exeFileName}, {datFileName}"); break;
                    //    case 27: // computing new correction file failed. incorrect calibration or too few test points : {exeFileName}, {datFileName}"); break;
                    //        break;
                    //}
                    //if (0 != proc.ExitCode)
                    //{
                    //    Trace.Write($"ExitCode : {proc.ExitCode}");
                    //    Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"correction 2d abnormal exit code= {proc.ExitCode}");
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Module.Laser, ex);
                return false;
            }
            #endregion

            #region  변환 결과에 대한 출력 로그 접근
            //String resultLogFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", this.TargetCorrectionFile + ".log");
            //if (!File.Exists(resultLogFileFullPath))
            //    return false;

            //this.ResultMessage = string.Empty;
            //this.ResultMessage = File.ReadAllText(resultLogFileFullPath);
            #endregion
           
            // 성공 여부를 특정 문자열 시그니처로 처리함 (temporary)
            //bool success = this.ResultMessage.Contains("NewCTFile:");
			//bool success = this.ResultMessage.Contains("written successfully");
            //this.OnResult?.Invoke(this, success, this.ResultMessage);

            // 임시 생성된 파일 정리
            //if (success)
            //{
            //    File.Delete(datFileFullPath);
            //    //File.Delete(resultLogFileFullPath);
            //}
            return true;
        }
    }
}