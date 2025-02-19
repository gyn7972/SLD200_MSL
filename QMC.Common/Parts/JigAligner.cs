using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Modules;
using QMC.Common.PathGenerators;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.PathGenerators.PathGenerator;

namespace QMC.Common.Parts
{
    [Serializable]
    public class JigAligner : PatternMatchingVisionPart
    {
        #region Field
        public WorkStage m_Owner;
        public XyCoordinate[] m_AlignPositions;

        #endregion

        #region Constructor
        public JigAligner(string strName) : base(strName)
        {
            this.Recipe = new JigAlignerRecipe(this);
            this.Config = new JigAlignerConfig();
            this.Result = 0.0;
            this.m_AlignPositions = null;

            this.PathGenerators = new VisionPart.PathGeneratorCollection();
            this.PathParameters = new PathParameterCollection();

            this.m_AlignPositions = new XyCoordinate[2];
            m_AlignPositions[0].X = 0.0;
            m_AlignPositions[0].Y = 0.0;
            m_AlignPositions[1].X = 0.0;
            m_AlignPositions[1].Y = 0.0;
        }
        #endregion

        #region Property
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
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        //public UvwzxyzStage Stage { set; get; }
        public XyzLDzzxzULzzxzStage Stage { set; get; }
        public XyzyStage XyzyStage { set; get; }
        public JigAlignerConfig Config { set; get; }
        public JigAlignerRecipe Recipe { set; get; }
        public XyCoordinate FirstPosition { protected set; get; }
        public XyCoordinate FirstPosition_ImageCoord { protected set; get; }             //  이미지 좌표
        public double Result { set; get; }
        public VisionPart.PathGeneratorCollection PathGenerators { set; get; }
        public PathParameterCollection PathParameters { set; get; }
        #endregion

        #region Method
        protected int GetMotionLimit(MotionAxis axis, out RangeD range)
        {
            int ret = 0;
            double maxValue = 0.0;
            double minValue = 0.0;
            range = new RangeD(double.NaN, double.NaN);

            if (axis == null) return ret;

            if ((ret = GetPosition(axis, ref minValue, ref maxValue)) != 0) return ret;
            //if ((ret = axis.Motor.PositivePosition.GetPosition(ref maxValue)) != 0) return ret;

            range = new RangeD(minValue, maxValue);

            return ret;
        }

        private int GetPosition(MotionAxis axis, ref double minValue, ref double MaxValue)
        {
            int ret = 0;
            minValue = axis.Motor.NegativePosition;
            MaxValue = axis.Motor.PositivePosition;
            return ret;
        }

        public int Train()
        {
            int ret = 0;

            //if (!Equipment.User_AdminMode && ((((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_Reticle_Lower) || 
            //                                (((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_Reticle_Upper)))
            //{
            //    MessageBox.Show("관리자 모드가 아닙니다.", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    return ret;
            //}

            if ((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return ret;
            }

            if (Recipe != null)
            {
                if (Recipe.PatternMatchingParameter == null)
                {
                    PatternMatchingParameters newParameter = new PatternMatchingParameters();
                    Recipe.PatternMatchingParameter = newParameter;
                }
                Recipe.PatternMatchingParameter.TrainImage = TrainImage;
                //Owner.SaveRecipeData();
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

        protected XyCoordinate GetCoordinate(double dX, double dY)
        {
            XyCoordinate coordinate = new XyCoordinate();
            //m_nVisionAligner_Type = (int)Aligner_Type.Aligner_PAK;
            if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
            {
                //if (((WorkStage)this.Owner).m_bLowerVision_Align)     //  하부 카메라
                if ((((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_Wafer) ||
                    (((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_Reticle_Lower))     //  하부 카메라
                {
                    coordinate.X = (dX - this.Camera.Resolution.Width / 2) * ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_X * (((WorkStage)this.Owner).Config.ParamConfig.LowerVision_ScaleInvert_X ? 1 : -1);

                    if (((WorkStage)this.Owner).Config.ParamConfig.AlignConcept_MyWaferAligner)  //  JigAligner 와 반대로 움직이길래... Invert 를 바꿔줌
                    {
                        coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_Y * (((WorkStage)this.Owner).Config.ParamConfig.LowerVision_ScaleInvert_Y ? -1 : 1);
                    }
                    else
                    {
                        coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_Y * (((WorkStage)this.Owner).Config.ParamConfig.LowerVision_ScaleInvert_Y ? 1 : -1);
                    }
                }
                else                                                        //  상부 카메라
                {
                    coordinate.X = (dX - this.Camera.Resolution.Width / 2) * ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X * (((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X ? 1 : -1);

                    if (((WorkStage)this.Owner).Config.ParamConfig.AlignConcept_MyWaferAligner)  //  JigAligner 와 반대로 움직이길래... Invert 를 바꿔줌
                    {
                        coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y * (((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y ? -1 : 1);
                    }
                    else
                    {
                        coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y * (((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y ? 1 : -1);
                    }
                }
            }
            else
            {
                coordinate.X = (dX - this.Camera.Resolution.Width / 2) * ((WorkStage)this.Owner).Scale.X * (((WorkStage)this.Owner).Scale.InvertedX ? 1 : -1);
                coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Scale.Y * (((WorkStage)this.Owner).Scale.InvertedY ? 1 : -1);
            }
            return coordinate;
        }

        protected double GetAngle(XyCoordinate first, XyCoordinate second)
        {
            XyCoordinate pos = second - first;

            return Math.Atan2(pos.Y, pos.X) * 180 / Math.PI;
        }

        protected double GetAngle_byAtan(XyCoordinate first, XyCoordinate second)
        {
            XyCoordinate pos = second - first;

            double DeltaX = first.X - second.X;
            double DeltaY = first.Y - second.Y;
            double Theta = 0.0;
            int invertAngle = 1;

            if (((WorkStage)this.Owner).Config.ParamConfig.Align_AngleInvert)
            {
                invertAngle *= -1;
            }

            if (Math.Abs(DeltaX) > Math.Abs(DeltaY))
                Theta = Math.Atan(DeltaY / DeltaX) * (180 / Math.PI) * invertAngle;
            else
                Theta = -Math.Atan(DeltaX / DeltaY) * (180 / Math.PI) * invertAngle;
            //Theta = Math.Truncate(Theta * 10000) / 10000;
            //Theta *= 10;

            //if ((m_Owner.m_nWaferAlign_MainStep >= (int)WorkStage.WaferAlign_Step.WaferAlign_LowVision_AlignStart) &&
            //    (m_Owner.m_nWaferAlign_MainStep <= (int)WorkStage.WaferAlign_Step.__WaferAlign_LowVisionCycle_Complete))
            //{
            //    Theta = Math.Truncate(Theta * 100000) / 100000;
            //}
            //else if ((m_Owner.m_nWaferAlign_MainStep >= (int)WorkStage.WaferAlign_Step.WaferAlign_HighVision_AlignStart) &&
            //    (m_Owner.m_nWaferAlign_MainStep <= (int)WorkStage.WaferAlign_Step.__WaferAlign_HighVisionCycle_Complete))
            //{
            //    Theta = Math.Truncate(Theta * 1000000) / 1000000;
            //    //Theta *= -1.0;
            //}
            //else
            //{
            //    Theta = Math.Truncate(Theta * 10000) / 10000;
            //}

            return Theta;
        }

        public double GetJigAlignResult()
        {
            if (double.IsNaN(Result) == true) return 0;

            return Result;
        }
        #endregion

        #region VisionPart Members
        public override int Create()
        {
            int ret = base.Create();

            //if (m_PatternMatchingTool == null)
            //    m_PatternMatchingTool = new VisionProPatternMatchingVisionTool();
            //if (m_RoiTrain == null)
            //    m_RoiTrain = new VisionProRoiVisionTool();
            //if (m_RoiInspect == null)
            //    m_RoiInspect = new VisionProRoiVisionTool();
            //m_PatternMatchingTool.SubTools.Clear();
            //m_PatternMatchingTool.SubTools.Add(m_RoiTrain);

            if (this.PathGenerators.Count == 0)
            {
                this.PathGenerators.Add(Recipe.pathGenerator);
            }

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Stop()
        {
            base.Stop();
            Stage.Stop();
        }

        public override int OnPrepareToWork()
        {
            int ret = 0;

            if ((ret = base.OnPrepareToWork()) != 0) return ret;

            if (this.m_AlignPositions == null)
                this.m_AlignPositions = new XyCoordinate[2] { Recipe.ReferencePosition, Recipe.GetSecondPosition() };

            return ret;
        }

        public override int OnWork()
        {
            int ret = 0;
            double dAngle = 0.0;
            m_Owner = this.Owner as WorkStage;
            if (m_Status == RunStatus.Stop) return 1;
            if (this.Stage == null) return -1;

            if (m_AlignPositions == null) return -1;

            //XyCoordinate center = (m_AlignPositions[0] + m_AlignPositions[1]) / 2;

            #region 주석
            //List<PatternMatchingResult> listResults = new List<PatternMatchingResult>();
            //foreach (XyCoordinate position in m_AlignPositions)
            //{
            //    if ((ret = Stage.MovePosition(position)) != 0) return ret;
            //    if (Recipe.MoveToDelay > 0)
            //        Thread.Sleep(Recipe.MoveToDelay);

            //    listResults.Add(Search());
            //}

            //if (listResults.Count == 2)
            //{
            //    if (listResults[0] != null && listResults[0].Values.Count > 0 &&
            //        listResults[1] != null && listResults[1].Values.Count > 0)
            //    {
            //        XyCoordinate first = new XyCoordinate();
            //        XyCoordinate second = new XyCoordinate();
            //        first = m_AlignPositions[0] + GetCoordinate(listResults[0].Values[0].X, listResults[0].Values[0].Y);
            //        second = m_AlignPositions[1] + GetCoordinate(listResults[1].Values[0].X, listResults[1].Values[0].Y);
            //        double dAngle = GetAngle(first, second);
            //        XyCoordinate result = new XyCoordinate();
            //        result = ((first + second) / 2);

            //        //m_Result.X = result.X;
            //        //m_Result.Y = result.Y;
            //        //reu.T = dAngle;
            //        this.Result = dAngle;
            //        ret = 0;
            //    }
            //    else
            //    {
            //        ret = -1;
            //    }
            //}
            //else
            //{
            //    ret = -1;
            //}
            #endregion

            PatternMatchingResult firstPointSearchResult = null;
            PatternMatchingResult secondPointSearchResult = null;
            XyCoordinate firstPointCoordinate = new XyCoordinate();
            XyCoordinate secondPointCoordinate = new XyCoordinate();

            XyCoordinate finalFirstPosition = new XyCoordinate();
            XyCoordinate finalSecondPosition = new XyCoordinate();

            //첫번째 위치 Search
            if (m_Owner.m_nFindAlignMarkType != (int)WorkStage.AlignMarkType.ALIGN_2NDMARK)                                                      //  2번 Align Mark 만 찾을 경우가 아닐 때만 1번 마크를 찾는다.
            {
                //this.Stage.MovePosition(m_AlignPositions[0]);
                this.Recipe.pathGenerator.PathParameter.CenterCoordinate = (XyCoordinate)m_AlignPositions[0];
                this.FindFiducialMark(out firstPointSearchResult, out firstPointCoordinate);
            }

            if (m_Status == RunStatus.Stop) return 1;               //  마크 찾다가 중지 하면 빠져나가자

            if (m_Owner.m_nFindAlignMarkType == (int)WorkStage.AlignMarkType.ALIGN_1STMARK )                                                      //  1번 Align Mark 만 찾을 경우, 여기서 Out
            {
                //m_Owner.m_nFindAlignMarkType = (int)WorkStage.AlignMarkType.ALIGN_2POINT;

                if (firstPointSearchResult != null)
                {
                    FirstPosition_ImageCoord = GetCoordinate(firstPointSearchResult.Values[0].X, firstPointSearchResult.Values[0].Y);                            //  이미지 좌표
                    //finalFirstPosition = firstPointCoordinate + GetCoordinate(firstPointSearchResult.Values[0].X, firstPointSearchResult.Values[0].Y);          //  이미지 좌표 + 모션 좌표
                    finalFirstPosition = firstPointCoordinate + FirstPosition_ImageCoord;                                                                        //  이미지 좌표 + 모션 좌표

                    FirstPosition = finalFirstPosition;
                    this.Result = 0.0;
                }
                //  마크 찾기에 실패하면? --> 싹 다 0 으로 Set
                else
                {
                    FirstPosition = new XyCoordinate(0.0, 0.0);
                    FirstPosition_ImageCoord = new XyCoordinate(0.0, 0.0);
                    this.Result = 0.0;
                }
                return ret;
            }

            if (m_Status == RunStatus.Stop) return 1;               //  마크 찾다가 중지 하면 빠져나가자

            //두번째 위치 Search
            //this.Stage.MovePosition(m_AlignPositions[1]);
            this.Recipe.pathGenerator.PathParameter.CenterCoordinate = (XyCoordinate)m_AlignPositions[1];
            this.FindFiducialMark(out secondPointSearchResult, out secondPointCoordinate);

            if (m_Owner.m_nFindAlignMarkType == (int)WorkStage.AlignMarkType.ALIGN_2NDMARK)                                                      //  2번 Align Mark 만 찾을 경우, 여기서 Out
            {
                //m_Owner.m_nFindAlignMarkType = (int)WorkStage.AlignMarkType.ALIGN_2POINT;

                if (secondPointSearchResult != null)
                {
                    FirstPosition_ImageCoord = GetCoordinate(secondPointSearchResult.Values[0].X, secondPointSearchResult.Values[0].Y);                              //  이미지 좌표
                    //finalSecondPosition = secondPointCoordinate + GetCoordinate(secondPointSearchResult.Values[0].X, secondPointSearchResult.Values[0].Y);          //  이미지 좌표 + 모션 좌표
                    finalSecondPosition = secondPointCoordinate + FirstPosition_ImageCoord;                                                                          //  이미지 좌표 + 모션 좌표

                    FirstPosition = finalSecondPosition;
                    this.Result = 0.0;
                }
                //  마크 찾기에 실패하면? --> 싹 다 0 으로 Set
                else
                {
                    FirstPosition = new XyCoordinate(0.0, 0.0);
                    FirstPosition_ImageCoord = new XyCoordinate(0.0, 0.0);
                    this.Result = 0.0;
                }
                return ret;
            }

            if (m_Status == RunStatus.Stop) return 1;               //  마크 찾다가 중지 하면 빠져나가자

            if ((firstPointSearchResult != null) && (secondPointSearchResult != null))
            {
                finalFirstPosition = firstPointCoordinate + GetCoordinate(firstPointSearchResult.Values[0].X, firstPointSearchResult.Values[0].Y);
                finalSecondPosition = secondPointCoordinate + GetCoordinate(secondPointSearchResult.Values[0].X, secondPointSearchResult.Values[0].Y);

                if(((WorkStage)this.Owner).Config.ParamConfig.Align_ThetaCalcFunction_Atan)
                {
                    dAngle = GetAngle_byAtan(finalFirstPosition, finalSecondPosition);
                }
                else
                {
                    dAngle = GetAngle(finalFirstPosition, finalSecondPosition);
                }

                //true면 NaN
                if (double.IsNaN(dAngle))
                {
                    FirstPosition = new XyCoordinate(0.0, 0.0);
                    this.Result = 0.0;
                }
                else
                {
                    FirstPosition = finalFirstPosition;
                    this.Result = dAngle;
                }
            }
            //  마크 찾기에 실패하면? --> 싹 다 0 으로 Set
            else
            {
                FirstPosition = new XyCoordinate(0.0, 0.0);
                this.Result = 0.0;
            }
            return ret;
        }

        /// <summary>
        /// searchResult : 현재 Vision에서 찾은 대상체의 좌표, CurrentCoordinate : Vision이 찾은 위치에서의 Motion Value X,Y
        /// </summary>
        /// <param name="searchResult"></param>
        /// <param name="currentCoordinate"></param>
        /// <returns></returns>
        public int FindFiducialMark(out PatternMatchingResult searchResult, out XyCoordinate currentCoordinate)
        {
            int ret = 0;
            currentCoordinate = new XyCoordinate();
            searchResult = new PatternMatchingResult();

            if (this.Stage == null)
            {
                return ret;
            }

            if (this.PathParameters == null || this.PathParameters.Count == 0)
            {
                this.PathParameters = new PathParameterCollection();
                this.PathParameters.Add(this.Recipe.pathGenerator.PathParameter);
            }

            m_Owner = this.Owner as WorkStage;

            if ((ret = this.Scan(this.PathGenerators[0], this.PathParameters[0], out searchResult, out currentCoordinate)) != 0)
            {
                return ret;
            }

            return ret;
        }

        private int Scan(VisionPart.PathGenerator generator, VisionPart.PathParameter parameter, out PatternMatchingResult findResult, out XyCoordinate currentPosition)
        {
            int ret = 0;

            PatternMatchingResult result = null;
            XyCoordinate currenCoordinate = new XyCoordinate();
            currentPosition = new XyCoordinate();
            findResult = null;
            //Step 1 : 설정되어 있는 Parameter를 이용하여 Path들을 생성.
            if ((ret = generator.Generate(parameter)) != 0) return ret;

            // Step 2 : 생성된 Path로 이동 시작.
            if (Equipment.Vision_SpiralMove_Use)
            {
                for (int i = 0; i < generator.Paths.Count; i++)
                {
                    if (Equipment.MachineStop_byUser)
                    {
                        m_Status = RunStatus.Stop;
                        return -1;
                    }

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep) //parameter.PathType == PathType.StepByStep)
                    {
                        if ((this.Stage.MovePosition(generator.Paths[i]) != 0)) return -1;

                        Thread.Sleep(m_Owner.Recipe.jigAlignerRecipe_HighRes.MoveToDelay);
                        result = this.Search();

                        currenCoordinate = (XyCoordinate)generator.Paths[i];

                        if (result != null && result.Values.Count != 0)
                        {
                            break;
                        }

                        FireUpdateResult(result);
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)// parameter.PathType == PathType.Continuous)
                    {
                    }
                }
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    if (Equipment.MachineStop_byUser)
                    {
                        m_Status = RunStatus.Stop;
                        return -1;
                    }

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep) //parameter.PathType == PathType.StepByStep)
                    {
                        //if ((this.Stage.MovePosition(generator.Paths[i]) != 0)) return -1;                //  테스트용 주석 : 모션 없이 테스트

                        Thread.Sleep(m_Owner.Recipe.jigAlignerRecipe_HighRes.MoveToDelay);
                        result = this.Search();

                        currenCoordinate = (XyCoordinate)generator.Paths[i];

                        if (result != null && result.Values.Count != 0)
                        {
                            break;
                        }

                        FireUpdateResult(result);
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)// parameter.PathType == PathType.Continuous)
                    {
                    }
                }
            }

            //if ((ret = this.Stage.MovePosition(currenCoordinate)) != 0) return ret;               //  테스트용 주석 : 모션 없이 테스트

            if (result == null || result.Values.Count == 0)
                return -1;

            findResult = result;
            currentPosition = currenCoordinate;

            return ret;
        }

        public override void UpdateConfigData() //참고 : Override
        {
            //TO DO : 문제있음. Serialize문제 생김.
            if (Owner is WorkStage)
            {
                WorkStage workStage = Owner as WorkStage;
                if (workStage != null)
                {
                    // this.Config = workStage.Config.JigAlignerConfig;
                }
            }

        }

        public override void UpdateRecipeData()
        {
            if (this.Recipe.IlluminationDataSet != null)
            {
                IlluminationData = this.Recipe.IlluminationDataSet;
            }
            else
            {
                this.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
            }
            Recipe.Init(this);

            if (PathGenerators != null)
            {
                PathGenerators.Clear();
                this.PathGenerators.Add(Recipe.pathGenerator);
            }

            base.UpdateRecipeData();
        }
        #endregion
    }
}
