using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public delegate void UpdateAutoFocusConfigEvent(ref AutoFocuserConfig config);
    public class AutoFocuser : VisionPart
    {
        public UpdateAutoFocusConfigEvent UpdateAutoFocusEvent;

        protected VisionProSharpnessVisionTool m_SharpnessVisionTool;
        public AutoFocuserConfig Config { get; set; }
        public ScoreCollection ScoreCollection { get; set; }

        public MotionPart Motion { set; get; }
        public MotionPart ZAxis { set; get; }
        public IlluminationDataSet IlluminationDataSet { get; set; }
        public List<AutoFocusResult> Results { get; set; }
        public AutoFocusResult Result
        {
            get;
            protected set;
        }
        public AutoFocuser(string strName) : base(strName)
        {
            Config = new AutoFocuserConfig();
            ScoreCollection = new ScoreCollection();
            m_SharpnessVisionTool = new VisionProSharpnessVisionTool();
            IlluminationDataSet = new IlluminationDataSet(strName);

            if (Results == null)
            {
                Results = new List<AutoFocusResult>();
                for (int i = 0; i < 6; i++)
                {
                    AutoFocusResult result = new AutoFocusResult(0, 0, null, 0);
                    Results.Add(result);
                }
            }
        }

        public override void UpdateConfigData()
        {
            WaferProbeAlign waferProbeAlign = Owner as WaferProbeAlign;
            if (Owner != null)
            {
                //if (dieTransfer.Config.AutoFocuerConfig != null && dieTransfer.Config.AutoFocuerConfig.FocusPosition == null)
                //{
                //    dieTransfer.Config.AutoFocuerConfig.Init();
                //}
                Config = waferProbeAlign.Config.AutoFocuserConfig_Upper;

                if (Config == null)
                {
                    Config = new AutoFocuserConfig();
                    waferProbeAlign.Config.AutoFocuserConfig_Upper = Config;
                }
            }
        }

        public override int Create()
        {
            int ret = 0;

            ret = base.Create();

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Stop()
        {
            base.Stop();
            Motion.SetRunStatus(RunStatus.Stop);
            Motion.Stop();
        }
        public override int OnWork()
        {
            int ret = 0;

            int index = 0;
            int count = 0;
            double pitch = 0.0;
            VisionImage image = null;
            double position = 0;
            DateTime StartTime = DateTime.Now;
            WaferProbeAlign waferProbeAlign = this.Owner as WaferProbeAlign;

            position = Config.FocusStartPosition;

            try
            {
                if (m_Status == RunStatus.Stop) return 1;

                // Step 1 : 첫번째 시작 위치 및 Pitch 거리, Count를 설정.
                count = Config.FocusCount;

                if (waferProbeAlign.m_nProductAlign_CameraType == (int)WaferProbeAlign.CameraType.CAMERA_HIGH)
                {
                    pitch = 0.01;               //  왜 FocusMargin 이 안먹는지 알 수가 없다...                    
                }
                else
                {
                    pitch = (Config.FocusMargin * 2) / (count - 1);
                }

                if ((ret = this.GetFocusValues(position, count, pitch)) != 0) return ret;

                // Step 3 : 첫번째 Scan의 결과 중 가장 높은 ScorePair를 가져와 두번째 시작 위치 및 Pitch 거리, Count를 설정.
                index = this.ScoreCollection.GetMaxScoreIndex();

                Log.Write(Name, string.Format("FineAutoFocus First Result[Value] : {0}", this.ScoreCollection[index].Score));
                Log.Write(Name, string.Format("FineAutoFocus First Result[Position] : {0}", this.ScoreCollection[index].Position));

                // Step 4 : 두번째 Scan 시작.
                //if (this.Config.EnabledSecondStep == true)
                //{
                //    if (this.Config.Direction == FocusingDirection.Plus)
                //    {
                //        //position = this.ScoreCollection[index].Position - (Math.Abs(this.Config.MinimumPitch * count / 2));
                //        position = this.ScoreCollection[index].Position - pitch;
                //    }
                //    else if (this.Config.Direction == FocusingDirection.Minus)
                //    {
                //        //position = this.ScoreCollection[index].Position + (Math.Abs(this.Config.MinimumPitch * count / 2));
                //        position = this.ScoreCollection[index].Position + pitch;
                //    }
                //    count = (int)((pitch / this.Config.MinimumPitch) * 2 + 1);
                //    //if ((ret = this.GetFocusValues(position, count, this.Config.MinimumPitch, out image)) != 0) return ret;
                //    if ((ret = this.GetFocusValues(position, count, this.Config.MinimumPitch)) != 0) return ret;

                //    index = this.ScoreCollection.GetMaxScoreIndex();

                //    Log.Write(Name, string.Format("Last Result[Value] : {0}", this.ScoreCollection[index].Score));
                //    Log.Write(Name, string.Format("Last Result[Position] : {0}", this.ScoreCollection[index].Position));
                //}

                // Step 5 : 가장 높은 ScorePair의 결과의 위치로 Move 후 저장.
                if (this.Config.EnabledMoveBestFocusPosition == true)
                {
                    if ((ret = this.Motion.Move(this.ScoreCollection[index].Position)) != 0) return ret;
                }
            }
            finally
            {
                TimeSpan processTime = DateTime.Now - StartTime;
                //DieTransfer dieTransfer = this.Owner as DieTransfer;
                if (this.ScoreCollection.Count > 0)
                {
                    this.Result = new AutoFocusResult(this.ScoreCollection[index].Score, this.ScoreCollection[index].Position, image, processTime.TotalMilliseconds);
                    //Config.FocusPosition[collet.ArmIndex] = Result.BestFocusPosition;
                    Config.FocusPosition = Result.BestFocusPosition;
                }
                //Results[ColletIndex - 1] = Result;
                //if (Config.Results.Count < 0 && Config.Results == null)
                //{
                //    Config.Results = new List<AutoFocusResult>();
                //    for (int i = 0; i < 6; i++)
                //    {
                //        AutoFocusResult result = new AutoFocusResult(0, 0, null, 0);
                //        Config.Results.Add(result);
                //    }
                //}
                //Config.Results[ColletIndex - 1] = Result;

            }
            Camera.StartLive();
            return ret;
        }

        public double ScoreFocus(byte[] buffer, int w, int h, int nThreadBack, int nThreadCollet)
        {
            double dScore = 0;
            int nLastX = 0;
            int nLastY = 0;
            try
            {
                byte[] buffer2 = new byte[w * h];
                int[] buffer3 = new int[w * h];
                buffer.CopyTo(buffer2, 0);

                int nStartX = Math.Max((int)4, 0);
                int nStartY = Math.Max((int)4, 0);
                int nEndX = Math.Min((int)w - 4, w);
                int nEndY = Math.Min((int)h - 4, h);

                Queue<int> que = new Queue<int>();
                int nPixelGab = 3;
                int w2 = w * nPixelGab;
                for (int y = nPixelGab; y < h - nPixelGab; y++)
                {
                    for (int x = nPixelGab; x < w - nPixelGab; x++)
                    {
                        int nSum = 0;
                        int nY = y * w;
                        if (buffer2[x + nY] > nThreadCollet)
                        {
                            nSum += buffer[x + nY] * 8;

                            nSum -= buffer[x - nPixelGab + nY - w2];
                            nSum -= buffer[x - nPixelGab + nY - 0];
                            nSum -= buffer[x - nPixelGab + nY + w2];
                            nSum -= buffer[x + nPixelGab + nY - w2];
                            nSum -= buffer[x + nPixelGab + nY - 0];
                            nSum -= buffer[x + nPixelGab + nY + w2];
                            nSum -= buffer[x + nY - w2];
                            nSum -= buffer[x + nY + w2];

                            if (nSum > 0)
                            {
                                buffer3[x + y * w] = (int)Math.Min(4096, Math.Abs(nSum));
                            }
                            else
                            {
                                buffer3[x + y * w] = 0;
                            }

                        }
                        else
                        {
                            buffer3[x + y * w] = 0;
                        }
                    }
                }

                dScore = buffer3.OrderByDescending(t => t).Take(5000).Average(t => t);
                //dScore = buffer3.OrderBy(t => t).Take(50).Average(t => t);

                buffer3.CopyTo(buffer, 0);
            }
            catch (Exception ex)
            {
                Log.Write(this, ex.Message);
            }
            return dScore;

        }
        private int GetFocusValues(double position, int count, double minimumPitch)
        {
            int ret = 0;
            VisionImage image = null;
            ScoreSet pair;
            this.ScoreCollection.Clear();

            for (int i = 0; i < count; i++)
            {
                if (m_Status == RunStatus.Stop)
                    return 1;
                this.Motion.SetRunStatus(m_Status);
                if ((ret = this.Motion.Move(position)) != 0)
                {
                    //Alarm Move Failed
                    Alarm alarm = new Alarm();
                    alarm.Title = "Motion Move Failed.";
                    alarm.Code = -1;
                    alarm.Source = Name;
                    alarm.Grade = "Error";
                    alarm.Cause = "[" + Motion.Name + "] Move Failed.";
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }

                if ((ret = OnSetIllumination(IlluminationDataSet, true)) != 0) return ret;
                Thread.Sleep(1);
                if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0) return ret;
                double dScore = this.ScoreFocus(image.RawData, image.Header.Width, image.Header.Height, Config.BackGroundThreshold, Config.Threshold);
                if (dScore <= 0)
                {
                    //Alarm Focus Value Failed
                    Alarm alarm = new Alarm();
                    alarm.Title = "Focus Value Failed.";
                    alarm.Code = -1;
                    alarm.Source = Name;
                    alarm.Grade = "Error";
                    alarm.Cause = "Focus Value Failed.";
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }

                pair = new ScoreSet(dScore, position);

                Log.Write(Name, string.Format("FineAutoFocus [Position] : {0}", pair.Position));
                Log.Write(Name, string.Format("FineAutoFocus [Value] : {0}", pair.Score));

                this.ScoreCollection.Add(pair);

                if (i == count - 1) continue;
                if (Config.Direction == FocusingDirection.Plus)
                {
                    position += minimumPitch;
                }
                else if (Config.Direction == FocusingDirection.Minus)
                {
                    position -= minimumPitch;
                }
            }

            return ret;
        }
        private int GetFocusValues(double position, int count, double minimumPitch, out VisionImage image)
        {
            int ret = 0;
            SharpnessResult result = null;
            ScoreSet pair;
            image = null;
            this.ScoreCollection.Clear();

            for (int i = 0; i < count; i++)
            {
                if (m_Status == RunStatus.Stop)
                    return 1;

                if ((ret = this.Motion.Move(position)) != 0)
                {
                    //Alarm Move Failed
                    Alarm alarm = new Alarm();
                    alarm.Title = "Motion Move Failed.";
                    alarm.Code = -1;
                    alarm.Source = Name;
                    alarm.Grade = "Error";
                    alarm.Cause = "[" + Motion.Name + "] Move Failed.";
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = this.GetFocusValue(out result)) != 0)
                {
                    //Alarm Focus Value Failed
                    Alarm alarm = new Alarm();
                    alarm.Title = "Focus Value Failed.";
                    alarm.Code = -1;
                    alarm.Source = Name;
                    alarm.Grade = "Error";
                    alarm.Cause = "Focus Value Failed.";
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }

                pair = new ScoreSet(result.SharpnessScore, position);

                if (this.ScoreCollection.GetMaxScore() < pair.Score)
                    image = result.OutputImage;

                Log.Write(Name, string.Format("FineAutoFocus [Position] : {0}", pair.Position));
                Log.Write(Name, string.Format("FineAutoFocus [Value] : {0}", pair.Score));

                this.ScoreCollection.Add(pair);

                if (i == count - 1) continue;
                if (Config.Direction == FocusingDirection.Plus)
                {
                    position += minimumPitch;
                }
                else if (Config.Direction == FocusingDirection.Minus)
                {
                    position -= minimumPitch;
                }
            }
            return ret;
        }

        private int GetFocusValue(out SharpnessResult result)
        {
            int ret = 0;
            result = null;
            VisionImage image = null;

            //if ((ret = OnSetIllumination(IlluminationDataSet, true)) != 0) return ret;
            if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0) return ret;
            m_SharpnessVisionTool.InputImage = image;
            if ((ret = m_SharpnessVisionTool.Run()) != 0) return ret;
            result = m_SharpnessVisionTool.Result;

            return ret;
        }
    }

    [Serializable]
    public class AutoFocusResult
    {
        #region Field
        private double m_Score;
        private double m_BestFocusPosition;
        private double m_ProcessingTime;
        private VisionImage m_BestFocusImage;
        #endregion

        #region Constructor
        public AutoFocusResult(double score, double bestFocusPosition, VisionImage bestFocusImage, double processingTime)
        {
            this.Score = score;
            this.BestFocusImage = bestFocusImage;
            this.BestFocusPosition = bestFocusPosition;
            this.ProcessingTime = processingTime;
        }
        #endregion

        #region Property
        public double Score
        {
            get { return this.m_Score; }
            private set { this.m_Score = value; }
        }

        public double BestFocusPosition
        {
            get { return this.m_BestFocusPosition; }
            private set { this.m_BestFocusPosition = value; }
        }

        public double ProcessingTime
        {
            get { return this.m_ProcessingTime; }
            private set { this.m_ProcessingTime = value; }
        }

        public VisionImage BestFocusImage
        {
            get { return this.m_BestFocusImage; }
            private set { this.m_BestFocusImage = value; }
        }
        #endregion
    }

}
