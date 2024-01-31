using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class StageThetaAgingTester : PatternMatchingVisionPart
    {
        protected bool m_bExit;
        public StageThetaAgingTester(string strName) : base(strName)
        {
            m_Results = new StageThetaAgingTestResultCollection();
            m_bExit = false;
            Recipe = new StageThetaAgingTesterRecipe(this);
        }

        #region Property
        public VisionScale Scale { set; get; }
        public StageThetaAgingTesterRecipe Recipe { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }

        private StageThetaAgingTestResultCollection m_Results;

        public IlluminationDataSet IlluminationData
        {
            set
            {
                Recipe.IlluminationDataSet = value;
            }
            get
            {
                return Recipe.IlluminationDataSet;
            }
        }
        #endregion

        #region Method

        public override int Create()
        {
            return base.Create();
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Stop()
        {
            m_bExit = true;
            this.Stage.Stop();
        }
        public XytCoordinate GetCurrentPosition()
        {
            XytCoordinate current = new XytCoordinate();
            if (Stage != null)
            {
                Stage.GetActualPosition(ref current);
            }

            return current;
        }
        public int Train(PatternMatchingParameters parameter)
        {
            int ret = 0;

            if ((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, parameter, IlluminationData)) != 0)
            {
                return ret;
            }

            if (Recipe != null)
            {
                if (parameter == null)
                {
                    PatternMatchingParameters newParameter = new PatternMatchingParameters();
                    parameter = newParameter;
                }
                parameter.TrainImage = TrainImage;
                Owner.SaveRecipeData();
            }
            return ret;
        }

        public PatternMatchingResult Search()
        {
            int ret = 0;

            if ((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;
        }
        #endregion

        public void SetParameter(double dTolerance, int nMaxInstance, double dMinScore, bool bDuplicateChecked, bool bUseMaskImage)
        {
            this.Recipe.PatternMatchingParameter.MaxTolerance = dTolerance;
            this.Recipe.PatternMatchingParameter.MinTolerance = dTolerance * -1;
            this.Recipe.PatternMatchingParameter.MaxInstance = nMaxInstance;
            this.Recipe.PatternMatchingParameter.MinScore = dMinScore;
            this.Recipe.PatternMatchingParameter.DuplicateChecked = bDuplicateChecked;
            this.Recipe.PatternMatchingParameter.UseMaskImage = bUseMaskImage;
        }

        public override void UpdateConfigData() //참고 : 오버라이드,, 파트 콜
        {
            //DieUnloader dieUnloader = Owner as DieUnloader;
            //if (dieUnloader != null)
            //{
            //    if (Recipe.IlluminationDataSet == null)
            //        Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
            //    Recipe.IlluminationDataSet.SetIlluminationChannel(dieUnloader.Config.ListIlluminationChannel);
            //}
        }

        public override void UpdateRecipeData()
        {
            //DieUnloader dieUnloader = Owner as DieUnloader;
            //if (dieUnloader != null)
            //{
            //    if (dieUnloader.Recipe.DieFinderRecipe != null)
            //    {
            //        this.Recipe = dieUnloader.Recipe.StageThetaAgingTesterRecipe;
            //        if (this.Recipe.IlluminationDataSet != null)
            //        {
            //            this.Recipe = dieUnloader.Recipe.StageThetaAgingTesterRecipe;
            //            IlluminationData = this.Recipe.IlluminationDataSet;
            //        }
            //        else
            //        {
            //            dieUnloader.Recipe.StageThetaAgingTesterRecipe = this.Recipe;
            //            this.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
            //        }
            //    }
            //    else
            //    {
            //        dieUnloader.Recipe.StageThetaAgingTesterRecipe = this.Recipe;
            //    }


            //}
            Recipe.Init(this);
            base.UpdateRecipeData();

            m_RoiTrain.Parameter.StartLocation = Recipe.TrainRoiStartLocation;
            m_RoiTrain.Parameter.EndLocation = Recipe.TrainRoiEndLocation;
            m_RoiInspect.Parameter.StartLocation = Recipe.InspectRoiStartLocation;
            m_RoiInspect.Parameter.EndLocation = Recipe.InspectRoiEndLocation;
        }

        public void GetParameter(out PatternMatchingParameters parameter)
        {
            parameter = new PatternMatchingParameters();
            parameter.MaxTolerance = Recipe.PatternMatchingParameter.MaxTolerance;
            parameter.MinTolerance = Recipe.PatternMatchingParameter.MinTolerance;
            parameter.MaxInstance = Recipe.PatternMatchingParameter.MaxInstance;
            parameter.MinScore = Recipe.PatternMatchingParameter.MinScore;
            parameter.DuplicateChecked = Recipe.PatternMatchingParameter.DuplicateChecked;
            parameter.UseMaskImage = Recipe.PatternMatchingParameter.UseMaskImage;
        }
        public int OnRun()
        {
            int ret = 0;
            m_bExit = false;

            if (this.Stage != null)
            {
                this.Stage.SetRunStatus(RunStatus.Run);
                XytCoordinate firstPosition = Recipe.GetPositionData(StageThetaAgingTesterRecipe.PositionKeys.First.ToString());
                XytCoordinate secondPosition = Recipe.GetPositionData(StageThetaAgingTesterRecipe.PositionKeys.Second.ToString());

                Dictionary<string, MovingProjection> firstProjection = GetMovingProjection(firstPosition);
                Dictionary<string, MovingProjection> secondProjection = GetMovingProjection(secondPosition);

                PatternMatchingResult result = new PatternMatchingResult();

                int nindex = 0;

                //if (Recipe.RepeatCount > 0)
                {
                    while (true)
                    {
                        string strResult = string.Empty;
                        if((ret = PositionSearch(firstProjection, out result))!=0)
                        {
                            return ret;
                        }
                        //UpdateResultCollection(result);
                        strResult = GetResultString(result);


                        if ((ret = PositionSearch(secondProjection, out result)) != 0)
                        {
                            return ret;
                        }
                        //UpdateResultCollection(result);
                        strResult += GetResultString(result);

                        WriteFile(strResult);

                        

                        if(Recipe.RepeatCount > 0)
                        {
                            nindex++;
                            if (nindex >= Recipe.RepeatCount)
                            {
                                break;
                            }
                        }

                        if (m_bExit)
                            break;
                        
                    }
                }

            }

            return ret;
        }

        public Task<int> OnRunAsync()
        {
            Task<int> result = Task.Factory.StartNew(() =>
            {
                int ret = 0;

                ret = OnRun();

                return ret;
            });

            return result;
        }

        private void WriteFile(string strData)
        {
            string strFileName = Recipe.ResultFilePath;

            using (StreamWriter writer = new StreamWriter(strFileName, true))
            {
                writer.WriteLine(strData);
                writer.Close();
            }
        }

        private string GetResultString(PatternMatchingResult result)
        {
            StringBuilder builder = new StringBuilder();
            XytCoordinate position = new XytCoordinate();
            XytCoordinate actualPosition = new XytCoordinate();
            this.Stage.GetCommandPosition(ref position);
            this.Stage.GetActualPosition(ref actualPosition);
            builder.AppendFormat("{0}, {1}, {2},", position.X, position.Y, position.T);
            if(result != null)
            {
                //DieUnloader dieUnloader = this.Owner as DieUnloader;
                XytCoordinate converted = new XytCoordinate((result.Values[0].X - this.Camera.Resolution.Width / 2) * Scale.X * (Scale.InvertedX ? 1 : -1),
                                                       (result.Values[0].Y - this.Camera.Resolution.Height / 2) * Scale.Y * (Scale.InvertedY ? 1 : -1), result.Values[0].R);
                builder.AppendFormat("{0}, {1}, {2},", actualPosition.X + converted.X, actualPosition.Y + converted.Y, converted.T);
                builder.AppendFormat("{0}, {1}, {2},", position.X - actualPosition.X + converted.X, position.Y - actualPosition.Y + converted.Y, position.T - actualPosition.T + converted.T);
            }

            return builder.ToString();
        }
        private void UpdateResultCollection(PatternMatchingResult result)
        {


            if(m_Results == null)
            {
                m_Results = new StageThetaAgingTestResultCollection();
            }

            XytCoordinate position = new XytCoordinate();
            StageThetaAgingTestResult agingResult = new StageThetaAgingTestResult();

            this.Stage.GetActualPosition(ref position);
            agingResult.ActualPositionX = position.X;
            agingResult.ActualPositionY = position.Y;
            agingResult.ActualPositionT = position.T;

            agingResult.MesurePositionX = result.Values[0].X;
            agingResult.MesurePositionY = result.Values[0].Y;
            agingResult.MesurePositionT = result.Values[0].R;

            agingResult.OffsetX = position.X - result.Values[0].X;
            agingResult.OffsetY = position.Y - result.Values[0].Y;
            agingResult.OffsetT = position.T - result.Values[0].R;

            this.m_Results.Add(agingResult);
        }

        private int PositionSearch(Dictionary<string, MovingProjection> projection, out PatternMatchingResult result)
        {
            int ret = 0;
            result = new PatternMatchingResult();

            // Position Move
            if ((ret = Stage.Move(projection)) != 0)
            {
                return ret;
            }

            // Delay
            if (this.Recipe.MoveDelay > 0)
            {
                Thread.Sleep(this.Recipe.MoveDelay);
            }

            // Search
            result = Search();

            if (result != null && result.Values.Count <= 0)
            {
                Alarm alarm = new Alarm();
                alarm.Source = this.Name;
                alarm.Grade = "Error";
                alarm.Code = -1;
                alarm.Title = "Pattern Matching Failed.";
                alarm.Cause = "Pattern Matching Failed.";
                AlarmManager.Instance.ShowAlarm(alarm);
            }

            return ret;
        }

        private Dictionary<string, MovingProjection> GetMovingProjection(XytCoordinate position)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();
            dicMovingProjection[XytStage.MotionKey.X.ToString()].Velocity = Recipe.Velovity_X;
            dicMovingProjection[XytStage.MotionKey.X.ToString()].Acceleration = Recipe.Accelelation_X;
            dicMovingProjection[XytStage.MotionKey.X.ToString()].Deceleration = Recipe.Deceleration_X;


            dicMovingProjection[XytStage.MotionKey.Y.ToString()].Velocity = Recipe.Velovity_Y;
            dicMovingProjection[XytStage.MotionKey.Y.ToString()].Acceleration = Recipe.Accelelation_Y;
            dicMovingProjection[XytStage.MotionKey.Y.ToString()].Deceleration = Recipe.Deceleration_Y;

            dicMovingProjection[XytStage.MotionKey.T.ToString()].Velocity = Recipe.Velovity_T;
            dicMovingProjection[XytStage.MotionKey.T.ToString()].Acceleration = Recipe.Accelelation_T;
            dicMovingProjection[XytStage.MotionKey.T.ToString()].Deceleration = Recipe.Deceleration_T;

            dicMovingProjection[XytStage.MotionKey.X.ToString()].Position = position.X;
            dicMovingProjection[XytStage.MotionKey.Y.ToString()].Position = position.Y;
            dicMovingProjection[XytStage.MotionKey.T.ToString()].Position = position.T;

            return dicMovingProjection;
        }

        
    }
    [Serializable]
    public class StageThetaAgingTestResult
    {
        public double ActualPositionX { set; get; }
        public double ActualPositionY { set; get; }
        public double ActualPositionT { set; get; }

        public double MesurePositionX { set; get; }
        public double MesurePositionY { set; get; }
        public double MesurePositionT { set; get; }

        public double OffsetX { set; get; }
        public double OffsetY { set; get; }
        public double OffsetT { set; get; }

        public StageThetaAgingTestResult()
        {
            ActualPositionX = 0;
            ActualPositionY = 0;
            ActualPositionT = 0;

            MesurePositionX = 0;
            MesurePositionY = 0;
            MesurePositionT = 0;

            OffsetX = 0;
            OffsetY = 0;
            OffsetT = 0;
        }
    }

    [Serializable]
    public class StageThetaAgingTestResultCollection : Collection<StageThetaAgingTestResult>
    {
        
    }
}
