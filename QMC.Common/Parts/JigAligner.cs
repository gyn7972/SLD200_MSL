using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Modules;
using QMC.Common.PathGenerators;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.PathGenerators.PathGenerator;
using System.Drawing;
using static QMC.Common.Vision.Tools.PatternMatchingResult;
using System.Net.Http.Headers;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Hmi;
using static QMC.Common.Equipment;

namespace QMC.Common.Parts
{
    [Serializable]
    public class JigAligner : PatternMatchingVisionPart
    {
        #region Define
        [Serializable]
        public enum SearchMethod
        {
            PatternMatching,
            Blob,
            Circle,
        }
        #endregion

        #region Field
        public WorkStage m_Owner;
        public XyCoordinate[] m_AlignPositions;
        public double[] m_dRadius;
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

            this.m_dRadius = new double[2];
            m_dRadius[0] = 0.0;
            m_dRadius[1] = 0.0;
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
        public bool UsePatternMatchingTool
        {
            get;
            set;
        }

        #region Method
        public InterpolatorMotionFunction MC_Func = new InterpolatorMotionFunction();
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


        public int OnSearchForRule(Point startRoiPoint, Point endRoiPoint, PatternMatchingParameters parameter, IlluminationDataSet illuminationData)
        {
            int ret = 0;

            

            return ret;
        }
        protected XyCoordinate GetCoordinate(double dX, double dY)
        {
            XyCoordinate coordinate = new XyCoordinate();
            //m_nVisionAligner_Type = (int)Aligner_Type.Aligner_PAK;
            if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
            {
                //if (((WorkStage)this.Owner).m_bLowerVision_Align)     //  하부 카메라
                if ((((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_Wafer) ||
                    (((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_Reticle_Lower) ||
                    (((WorkStage)this.Owner).m_nVisionAligner_Type == (int)WorkStage.Aligner_Type.Aligner_CoarseCam))     //  하부 카메라
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
                else  //  상부 카메라
                {
                    coordinate.X = (dX - this.Camera.Resolution.Width / 2) * ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_X * (((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X ? 1 : -1);

                    if (((WorkStage)this.Owner).Config.ParamConfig.AlignConcept_MyWaferAligner)  //  JigAligner 와 반대로 움직이길래... Invert 를 바꿔줌
                    {
                        coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_Y * (((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y ? -1 : 1);
                    }
                    else
                    {
                        coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_Y * (((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y ? 1 : -1);
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

        private XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();         //  Stage XY Map Data 로 변환된 위치 이동 좌표


        // PreAlign - 1번/2번 포인트 각각의 마지막 성공 마크 인덱스
        private int m_lastPreAlignMarkIndex1 = -1;   // 기본: 없으면 -1 → 0번부터
        private int m_lastPreAlignMarkIndex2 = -1;   // 기본: 없으면 -1 → 0번부터
        public override int OnWork()
        {
            int ret = 0;
            try
            {
                double dAngle = 0.0;
                m_Owner = this.Owner as WorkStage;
                if (m_Status == RunStatus.Stop) return 1;
                //if (this.Stage == null) return -1;

                if (m_AlignPositions == null) return -1;

                XyzCoordinate position = new XyzCoordinate();
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

                double lfVelocity = 0.0;
                double lfAccDec = 0.0;
                int nWait = 0;

                bool bWaitPosX = false;
                bool bWaitPosY = false;

                Task<bool> WaitPosX;
                Task<bool> WaitPosY;

                double dSpec = 0;
                double dScore = 0;
                double dRadius = 0;
                int nColor = 0;

                if (Owner is WorkStage workstage)
                {
                    if (m_Status == RunStatus.Stop) return 1;               //  마크 찾다가 중지 하면 빠져나가자

                    //무조건 2개 서치 - 소스 확인 하자.
                    m_Owner.m_nFindAlignMarkType = 0;
                    //첫번째 위치 Search
                    if (m_Owner.m_nFindAlignMarkType != (int)WorkStage.AlignMarkType.ALIGN_2NDMARK)                                                      //  2번 Align Mark 만 찾을 경우가 아닐 때만 1번 마크를 찾는다.
                    {
                        //위치만 살리면 된다...
                        //this.Stage.MovePosition(m_AlignPositions[0]);

                        //도면 좌표 불러옴 
                        position = new XyzCoordinate(m_AlignPositions[0].X, m_AlignPositions[0].Y, 0.0);
                        Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"positionX1: {position.X}, positionY1: {position.Y}"));

                        //  속도 설정
                        //lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Speed_Coarse;
                        //lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

                        position = workstage.ConvertPointCoarseCam(position);

                        xyInterpolatedCoordinate.X = position.X;
                        xyInterpolatedCoordinate.Y = position.Y;
                        Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"xyInterpolatedCoordinateX1:{xyInterpolatedCoordinate.X}, xyInterpolatedCoordinateY1:{xyInterpolatedCoordinate.Y}"));

                        //  속도 설정
                        if (false)
                        {
                            m_Owner.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Process);
                        }
                        else
                        {
                            m_Owner.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
                        }
                        //m_Owner.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);

                        Thread.Sleep(100);
                        Task<bool> resultX1 = m_Owner.WaitUntilInPositionAsync(WorkStage.nAxis.X, xyInterpolatedCoordinate.X);
                        Task<bool> resultY1 = m_Owner.WaitUntilInPositionAsync(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y);
                        resultX1.Wait();
                        resultY1.Wait();
                        if (!resultX1.Result || !resultY1.Result)
                        {
                            //  이동 실패 
                            if(!resultX1.Result)
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"X축 이동 실패"));
                                m_Owner.AlarmPost(AlarmKey.eStageMoveFail); //X,Y축 분할 필요?
                            }
                            if (!resultY1.Result)
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"Y축 이동 실패"));
                                m_Owner.AlarmPost(AlarmKey.eStageMoveFail); //X,Y축 분할 필요?
                            }
                        }
                        Thread.Sleep(100);

                        this.Recipe.pathGenerator.PathParameter.CenterCoordinate = (XyCoordinate)m_AlignPositions[0];

                        // 1. PreAlignMarkList가 있다면 반복, 아니면 기존 단일 구조 fallback
                        if (Equipment.stVisionRecipeSet.PreAlignMarkList != null && Equipment.stVisionRecipeSet.PreAlignMarkList.Count > 0)
                        {
                            m_Owner.SetLightingByChannel(LightingChannel.FineCamRed, 4000, true);
                            m_Owner.SetLightingByChannel(LightingChannel.FineCamIR, 0, false);

                            var marks = Equipment.stVisionRecipeSet.PreAlignMarkList;
                            if (marks == null || marks.Count == 0)
                            {
                                m_lastPreAlignMarkIndex1 = -1;
                                // 여기서 적절히 return/continue
                            }

                            // 이전 인덱스가 범위를 벗어나면 리셋
                            if (m_lastPreAlignMarkIndex1 >= marks.Count) m_lastPreAlignMarkIndex1 = -1;

                            if (marks != null && marks.Count > 0)
                            {
                                // 시작 인덱스 결정: 최근 성공 마크가 있으면 거기서, 없으면 기본 0
                                int defaultStart1 = 0;

                                int start = (m_lastPreAlignMarkIndex1 >= 0 && m_lastPreAlignMarkIndex1 < marks.Count)
                                            ? m_lastPreAlignMarkIndex1
                                            : (marks.Count > defaultStart1 ? defaultStart1 : 0);

                                var markListToSearch = marks.Skip(start).Concat(marks.Take(start)).ToList();

                                //foreach (var mark in Equipment.stVisionRecipeSet.PreAlignMarkList)
                                foreach (var mark in markListToSearch)
                                {
                                    //Illum 변경 필요.!!
                                    // 카메라 노출 설정
                                    Camera.SetExposureTime(mark.ExposureTime);
                                    m_Owner.SetLightingByChannel(LightingChannel.CoarseCamIR, mark.IllumIR);
                                    m_Owner.SetLightingByChannel(LightingChannel.CoarseCamRed, mark.IllumRed);
                                    Thread.Sleep(100);

                                    if (mark.AlgorithmType == Equipment.VisionAlgorithmType.PatternMatching)
                                    {
                                        this.FindFiducialMark(out firstPointSearchResult, out firstPointCoordinate);
                                    }
                                    else if (mark.AlgorithmType == Equipment.VisionAlgorithmType.CircleDetection)
                                    {
                                        dSpec = mark.CircleMarkSpec;
                                        dScore = mark.CircleMarkScore;
                                        dRadius = m_dRadius[0] != 0 ? m_dRadius[0] : mark.CircleMarkRadius;
                                        nColor = mark.CircleColor;
                                        // ROI, Illum 등도 필요시 마크별 값 사용

                                        //내부에서 Camera Grab 호출함.
                                        this.FindCircleDetection(dRadius, nColor, dSpec, dScore, out firstPointSearchResult, out firstPointCoordinate);
                                    }

                                    // 마크를 찾으면 break;
                                    if (firstPointSearchResult != null && firstPointSearchResult.Values.Count > 0)
                                    {
                                        m_lastPreAlignMarkIndex1 = marks.IndexOf(mark); // ★ 1번 포인트용 저장
                                        break;
                                    }
                                    else
                                    {
                                        m_lastPreAlignMarkIndex1 = -1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 여기 들어오면 안되는거징.
                            // 구버전 fallback (단일 마크)

                            if (Equipment.stVisionRecipeSet.ePreAlgorithmType == Equipment.VisionAlgorithmType.PatternMatching)
                            {
                                this.FindFiducialMark(out firstPointSearchResult, out firstPointCoordinate);
                            }
                            else if (Equipment.stVisionRecipeSet.ePreAlgorithmType == Equipment.VisionAlgorithmType.CircleDetection)
                            {
                                // 구버전 fallback (단일 마크)
                                dSpec = Equipment.stVisionRecipeSet.dPreCircleMarkSpec;
                                dScore = Equipment.stVisionRecipeSet.dPreCircleMarkScore;
                                dRadius = m_dRadius[0] != 0 ? m_dRadius[0] : Equipment.stVisionRecipeSet.dPreCircleMarkRadius;
                                nColor = Equipment.stVisionRecipeSet.nPreCircleColor;

                                this.FindCircleDetection(dRadius, nColor, dSpec, dScore, out firstPointSearchResult, out firstPointCoordinate);
                            }
                        }
                    }

                    if (m_Status == RunStatus.Stop) return 1; //  마크 찾다가 중지 하면 빠져나가자

                    if (m_Owner.m_nFindAlignMarkType == (int)WorkStage.AlignMarkType.ALIGN_1STMARK)                                                      //  1번 Align Mark 만 찾을 경우, 여기서 Out
                    {
                        //m_Owner.m_nFindAlignMarkType = (int)WorkStage.AlignMarkType.ALIGN_2POINT;

                        if (firstPointSearchResult != null)
                        {
                            FirstPosition_ImageCoord = GetCoordinate(firstPointSearchResult.Values[0].X, firstPointSearchResult.Values[0].Y);                            //  이미지 좌표
                                                                                                                                                                         //finalFirstPosition = firstPointCoordinate + GetCoordinate(firstPointSearchResult.Values[0].X, firstPointSearchResult.Values[0].Y);          //  이미지 좌표 + 모션 좌표
                            finalFirstPosition = firstPointCoordinate - FirstPosition_ImageCoord;                                                                        //  이미지 좌표 + 모션 좌표

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
                    position = new XyzCoordinate(m_AlignPositions[1].X, m_AlignPositions[1].Y, 0.0);
                    Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"positionX2: {position.X}, positionY2: {position.Y}"));

                    //  속도 설정
                    //lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Speed_Coarse;
                    //lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

                    position = workstage.ConvertPointCoarseCam(position);

                    xyInterpolatedCoordinate.X = position.X;
                    xyInterpolatedCoordinate.Y = position.Y;

                    Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"xyInterpolatedCoordinateX2:{xyInterpolatedCoordinate.X}, xyInterpolatedCoordinateY2:{xyInterpolatedCoordinate.Y}"));

                    //  속도 설정
                    if (false)
                    {
                        m_Owner.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Process);
                    }
                    else
                    {
                        m_Owner.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
                    }
                    //m_Owner.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);

                    Thread.Sleep(100);

                    // 비동기 대기 (UI에서 사용하면 안됨)
                    Task<bool> resultX = m_Owner.WaitUntilInPositionAsync(WorkStage.nAxis.X, xyInterpolatedCoordinate.X);
                    Task<bool> resultY = m_Owner.WaitUntilInPositionAsync(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y);
                    
                    resultX.Wait();
                    resultY.Wait();
                    if (!resultX.Result || !resultY.Result)
                    {
                        //  이동 실패 
                        if (!bWaitPosX)
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"X축 이동 실패"));
                            m_Owner.AlarmPost(AlarmKey.eStageMoveFail); //X,Y축 분할 필요?
                        }

                        if (!bWaitPosY)
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"Y축 이동 실패"));
                            m_Owner.AlarmPost(AlarmKey.eStageMoveFail); //X,Y축 분할 필요?
                        }
                    }
                    Thread.Sleep(100);


                    if (m_Status == RunStatus.Stop) return 1;               //  마크 찾다가 중지 하면 빠져나가자

                    this.Recipe.pathGenerator.PathParameter.CenterCoordinate = (XyCoordinate)m_AlignPositions[1];

                    // 1. PreAlignMarkList가 있다면 반복, 아니면 기존 단일 구조 fallback
                    if (Equipment.stVisionRecipeSet.PreAlignMarkList != null && Equipment.stVisionRecipeSet.PreAlignMarkList.Count > 0)
                    {
                        m_Owner.SetLightingByChannel(LightingChannel.FineCamRed, 4000, true);
                        m_Owner.SetLightingByChannel(LightingChannel.FineCamIR, 0, false);

                        var marks = Equipment.stVisionRecipeSet.PreAlignMarkList;

                        if (m_lastPreAlignMarkIndex2 >= marks.Count) m_lastPreAlignMarkIndex2 = -1;

                        if (marks != null && marks.Count > 0)
                        {
                            int defaultStart2 = 0;

                            int start = (m_lastPreAlignMarkIndex2 >= 0 && m_lastPreAlignMarkIndex2 < marks.Count)
                                        ? m_lastPreAlignMarkIndex2
                                        : (marks.Count > defaultStart2 ? defaultStart2 : 0);

                            var markListToSearch = marks.Skip(start).Concat(marks.Take(start)).ToList();

                            //foreach (var mark in Equipment.stVisionRecipeSet.PreAlignMarkList)
                            foreach (var mark in markListToSearch)
                            {
                                //Illum 변경 필요.!!
                                // 카메라 노출 설정
                                Camera.SetExposureTime(mark.ExposureTime);
                                m_Owner.SetLightingByChannel(LightingChannel.CoarseCamIR, mark.IllumIR);
                                m_Owner.SetLightingByChannel(LightingChannel.CoarseCamRed, mark.IllumRed);
                                Thread.Sleep(100);

                                if (mark.AlgorithmType == Equipment.VisionAlgorithmType.PatternMatching)
                                {
                                    this.FindFiducialMark(out secondPointSearchResult, out secondPointCoordinate);
                                }
                                else if (mark.AlgorithmType == Equipment.VisionAlgorithmType.CircleDetection)
                                {
                                    dSpec = mark.CircleMarkSpec;
                                    dScore = mark.CircleMarkScore;
                                    dRadius = m_dRadius[1] != 0 ? m_dRadius[1] : mark.CircleMarkRadius;
                                    nColor = mark.CircleColor;
                                    // ROI, Illum 등도 필요시 마크별 값 사용

                                    //내부에서 Camera Grab 호출함.
                                    this.FindCircleDetection(dRadius, nColor, dSpec, dScore, out secondPointSearchResult, out secondPointCoordinate);
                                }

                                // 마크를 찾으면 break;
                                if (secondPointSearchResult != null && secondPointSearchResult.Values.Count > 0)
                                {
                                    m_lastPreAlignMarkIndex2 = marks.IndexOf(mark); // ★ 2번 포인트용 저장
                                    break;
                                }
                                else
                                {
                                    m_lastPreAlignMarkIndex2 = -1;
                                }
                            }

                        }
                    }
                    else
                    {
                        // 여기 들어오면 안되는거징.
                        // 구버전 fallback (단일 마크)

                        if (Equipment.stVisionRecipeSet.ePreAlgorithmType == Equipment.VisionAlgorithmType.PatternMatching)
                        {
                            this.FindFiducialMark(out secondPointSearchResult, out secondPointCoordinate);
                        }
                        else if (Equipment.stVisionRecipeSet.ePreAlgorithmType == Equipment.VisionAlgorithmType.CircleDetection)
                        {
                            // 구버전 fallback (단일 마크)
                            dSpec = Equipment.stVisionRecipeSet.dPreCircleMarkSpec;
                            dScore = Equipment.stVisionRecipeSet.dPreCircleMarkScore;
                            dRadius = m_dRadius[1] != 0 ? m_dRadius[1] : Equipment.stVisionRecipeSet.dPreCircleMarkRadius;
                            nColor = Equipment.stVisionRecipeSet.nPreCircleColor;

                            this.FindCircleDetection(dRadius, nColor, dSpec, dScore, out secondPointSearchResult, out secondPointCoordinate);
                        }
                    }

                    if (m_Owner.m_nFindAlignMarkType == (int)WorkStage.AlignMarkType.ALIGN_2NDMARK)                                                      //  2번 Align Mark 만 찾을 경우, 여기서 Out
                    {
                        if (secondPointSearchResult != null)
                        {
                            FirstPosition_ImageCoord = GetCoordinate(secondPointSearchResult.Values[0].X, secondPointSearchResult.Values[0].Y);                              //  이미지 좌표
                                                                                                                                                                             //finalSecondPosition = secondPointCoordinate + GetCoordinate(secondPointSearchResult.Values[0].X, secondPointSearchResult.Values[0].Y);          //  이미지 좌표 + 모션 좌표
                            finalSecondPosition = secondPointCoordinate - FirstPosition_ImageCoord;                                                                          //  이미지 좌표 + 모션 좌표

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

                        Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"firstPointSearchResultX:{firstPointSearchResult.Values[0].X}, firstPointSearchResultY:{firstPointSearchResult.Values[0].Y}"));
                        Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", string.Format($"firstPointSearchResultX:{secondPointSearchResult.Values[0].X}, firstPointSearchResultY:{secondPointSearchResult.Values[0].Y}"));

                        if (((WorkStage)this.Owner).Config.ParamConfig.Align_ThetaCalcFunction_Atan)
                        {
                            dAngle = GetAngle_byAtan(finalFirstPosition, finalSecondPosition);
                        }
                        else
                        {
                            // 꼭확인
                            // 1, 2번 마크 위치가.. 좌우 바뀌었는데...
                            //XyzCoordinate position1 = new XyzCoordinate(m_AlignPositions[0].X, m_AlignPositions[0].Y, 0.0);
                            //XyzCoordinate position2 = new XyzCoordinate(m_AlignPositions[1].X, m_AlignPositions[1].Y, 0.0);
                            //double dRefAngle = GetAngle(new XyCoordinate(position2.X,position2.Y), new XyCoordinate(position1.X, position1.Y));
                            //position1.X -= finalFirstPosition.X;
                            //position1.Y -= finalFirstPosition.Y;
                            //position2.X -= finalSecondPosition.X;
                            //position2.Y -= finalSecondPosition.Y;
                            //dAngle = GetAngle(new XyCoordinate(position2.X, position2.Y), new XyCoordinate(position1.X, position1.Y));
                            //dAngle -= dRefAngle;

                            XyzCoordinate position1 = new XyzCoordinate(m_AlignPositions[0].X, m_AlignPositions[0].Y, 0.0);
                            XyzCoordinate position2 = new XyzCoordinate(m_AlignPositions[1].X, m_AlignPositions[1].Y, 0.0);
                            double dRefAngle = GetAngle(new XyCoordinate(position1.X, position1.Y), new XyCoordinate(position2.X, position2.Y));
                            position1.X += finalFirstPosition.X;
                            position1.Y -= finalFirstPosition.Y;
                            position2.X += finalSecondPosition.X;
                            position2.Y -= finalSecondPosition.Y;

                            dAngle = GetAngle(new XyCoordinate(position1.X, position1.Y), new XyCoordinate(position2.X, position2.Y));
                            dAngle -= dRefAngle;

                        }

                        if (m_Status == RunStatus.Stop) return 1;               //  마크 찾다가 중지 하면 빠져나가자

                        //Test ::
                        if (false) //구영남 - 1,2번 마크 위치에 따라 의심되면 TEST 해보자.
                        {
                            XyCoordinate base1 = new XyCoordinate(0,0);
                            XyCoordinate base2 = new XyCoordinate(0, 0);
                            XyCoordinate actual1 = new XyCoordinate(0, 0);
                            XyCoordinate actual2 = new XyCoordinate(0, 0);
                            
                            // 기준 마크 위치
                            base1 = m_AlignPositions[0];
                            base2 = m_AlignPositions[1];

                            // 정렬: 좌우/상하 순서 고정
                            if (Math.Abs(base2.X - base1.X) < Math.Abs(base2.Y - base1.Y))
                            {
                                // 세로 방향 기준 정렬
                                if (base1.Y > base2.Y)
                                {
                                    Swap(ref base1, ref base2);
                                }
                            }
                            else
                            {
                                // 가로 방향 기준 정렬
                                if (base1.X > base2.X)
                                {
                                    Swap(ref base1, ref base2);
                                }
                            }
                            double dRefAngle = GetAngle(new XyCoordinate(base1.X, base1.Y), new XyCoordinate(base2.X, base2.Y));

                            // 실측 마크 위치 (Offset 적용) : Y축이 반전이라는 전제인데.
                            actual1.X = base1.X + finalFirstPosition.X;
                            actual1.Y = base1.Y - finalFirstPosition.Y;
                            actual2.X = base2.X + finalSecondPosition.X;
                            actual2.Y = base2.Y - finalSecondPosition.Y;
                            double dActualAngle = GetAngle(new XyCoordinate(actual1.X, actual1.Y), new XyCoordinate(actual2.X, actual2.Y));
                            dAngle = dActualAngle - dRefAngle;

                            Log.Write("SLD-200", "JigAligner_OnWork_PreAlign", $"Base1: ({base1.X:F3}, {base1.Y:F3}), Base2: ({base2.X:F3}, {base2.Y:F3})");
                            Log.Write("SLD-200", "JigAligner_OnWork_PreAlign", $"Actual1: ({actual1.X:F3}, {actual1.Y:F3}), Actual2: ({actual2.X:F3}, {actual2.Y:F3})");
                            Log.Write("SLD-200", "JigAligner_OnWork_PreAlign", $"RefAngle: {dRefAngle:F3}, ActualAngle: {dActualAngle:F3}, dAngle: {dAngle:F3}");
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

                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            
            return ret;
        }

        private void Swap(ref XyCoordinate a, ref XyCoordinate b)
        {
            XyCoordinate temp = a;
            a = b;
            b = temp;
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

            //if (this.Stage == null)
            //{
            //    return ret;
            //}

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

        public int FindCircleDetection(double dRadius, int nalgorithmIndex, double dSpec, double dScore, out PatternMatchingResult searchResult, out XyCoordinate currentCoordinate)
        {
            int ret = 0;
            currentCoordinate = new XyCoordinate();
            searchResult = new PatternMatchingResult();

            QMC_ImageProcessFindAlign qip = new QMC_ImageProcessFindAlign();
            PatternMatchingResultValue pmCircle = new PatternMatchingResult.PatternMatchingResultValue();
            List<RectangleF> Fiducial_circlesResult = new List<RectangleF>();
            bool bFind = false;
            VisionImage image;
            VisionImage inputImage = null;
            VisionScale TempScale = new VisionScale();

            m_Owner = this.Owner as WorkStage;
            try
            {
                TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_X;
                TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_Scale_Y;
                TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_ScaleInvert_X;
                TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.LowerVision_ScaleInvert_Y;

                int nRadiusImageCount = (int)(dRadius / TempScale.X); // 찾고자 하는 circle size 
                //nRadiusImageCount /= 2;
                //Simulated = true;
                if (Simulated)
                {
                    image = TestImage;
                }
                else
                {
                    Camera.StopLive();
                    if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0)
                    {
                        searchResult = null;
                        currentCoordinate.X = 0.0;
                        currentCoordinate.Y = 0.0;
                        return ret;
                    }
                }

                {
                    //Circle 찾는 알고리듬 적용
                    //dSpec;
                    QMC_ImageProcessFindAlignResult result = null;
                    if(nalgorithmIndex < 2)
                    {
                        result = qip.FindCirclesWidthCircleBoundary(Fiducial_circlesResult,
                        Camera.LatestImage.RawData,
                        Camera.LatestImage.Header.Width,
                        Camera.LatestImage.Header.Height,
                        nRadiusImageCount, dSpec, ref bFind, 0, 0, nalgorithmIndex==0, dScore);
                        // 0.05 - Spec 
                    }
                    else
                    {
                        result = qip.FindCircleForFR4(Camera.LatestImage.RawData,
                        Camera.LatestImage.Header.Width,
                        Camera.LatestImage.Header.Height,
                        nRadiusImageCount, dSpec,  dScore);
                        // 0.05 - Spec 
                        foreach(var circle in result.Circles)
                        {
                            Fiducial_circlesResult.Add(circle.GetBoundery());
                            bFind = true;
                        }
                    }


                    if (result.Circles.Count > 0 && bFind == true)
                    {
                        double cx = result.Circles[0].CenterX ;
                        double cy = result.Circles[0].CenterY;
                        pmCircle.X = cx;
                        pmCircle.Y = cy;
                        pmCircle.Score = result.Circles[0].Score;

                        //currentCoordinate 안쓰는디...
                        currentCoordinate.X = 0.0;
                        currentCoordinate.Y = 0.0;
                        searchResult.Values.Add(pmCircle);
                    }
                    else
                    {
                        searchResult = null;
                        currentCoordinate.X = 0.0;
                        currentCoordinate.Y = 0.0;
                        return -1;
                    }

                    if (m_Owner.UpdateResultOveray != null)
                    {
                        try
                        {
                            m_Owner.CoarseCamResultOveray = new VisionImageViewer.OwnedOverlayCollection();
                            foreach (var v in Fiducial_circlesResult)
                            {
                                Point ptStart = new Point((int)v.Left, (int)v.Top);
                                Point ptEnd = new Point((int)v.Right, (int)v.Bottom);
                                var overayRect = new RectangleFrameVisionImageOverlay("Fine Align", ptStart, ptEnd);
                                overayRect.Visible = true;
                                overayRect.Color = Color.Lime;
                                overayRect.Thickness = 1;
                                m_Owner.CoarseCamResultOveray.Add(overayRect);
                                var overayEl = new EllipseFrameVisionImageOverlay("Fine Align", ptStart, ptEnd);
                                overayEl.Visible = true;
                                overayEl.Color = Color.Blue;
                                overayEl.Thickness = 1;
                                m_Owner.CoarseCamResultOveray.Add(overayEl);

                                int FontSize = 50;
                                string strScore = string.Format("Score : {0:0.00},Size:{1:0.00}  ", result.ScoreCollection[0], result.Circles[0].Radius*2* TempScale.X);
                                Font font = new Font("verdana", FontSize, FontStyle.Bold);
                                var textOveray = new TextVisionImageOverlay(strScore, new Point((int)v.Left, (int)v.Top - FontSize *3), font);
                                textOveray.Visible = true;
                                textOveray.Color = Color.Lime;
                                m_Owner.CoarseCamResultOveray.Add(textOveray);
                            }
                            m_Owner.UpdateResultOveray?.Invoke(this.Camera, null);
                        }
                        catch (Exception ex)
                        {

                            Log.Write(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
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
            //if ((ret = generator.Generate(parameter)) != 0) return ret;

            // Step 2 : 생성된 Path로 이동 시작.
            //if (Equipment.Vision_SpiralMove_Use)
            {
                //for (int i = 0; i < generator.Paths.Count; i++)
                {
                    //if (Equipment.MachineStop_byUser)
                    //{
                    //    m_Status = RunStatus.Stop;
                    //    return -1;
                    //}

                    //if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep) //parameter.PathType == PathType.StepByStep)
                    {
                        //if ((this.Stage.MovePosition(generator.Paths[i]) != 0)) return -1;

                        //Thread.Sleep(m_Owner.Recipe.jigAlignerRecipe_LowRes.MoveToDelay);
                        Thread.Sleep(200);
                        result = this.Search();

                        //currenCoordinate = (XyCoordinate)generator.Paths[i];

                        //if (result != null && result.Values.Count != 0)
                        //{
                        //    break;
                        //}

                        FireUpdateResult(result);
                    }
                    //else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)// parameter.PathType == PathType.Continuous)
                    {
                    }
                }
            }
            //else
            //{
            //    for (int i = 0; i < 1; i++)
            //    {
            //        if (Equipment.MachineStop_byUser)
            //        {
            //            m_Status = RunStatus.Stop;
            //            return -1;
            //        }

            //        if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep) //parameter.PathType == PathType.StepByStep)
            //        {
            //            //if ((this.Stage.MovePosition(generator.Paths[i]) != 0)) return -1;                //  테스트용 주석 : 모션 없이 테스트

            //            Thread.Sleep(m_Owner.Recipe.jigAlignerRecipe_HighRes.MoveToDelay);
            //            result = this.Search();

            //            currenCoordinate = (XyCoordinate)generator.Paths[i];

            //            if (result != null && result.Values.Count != 0)
            //            {
            //                break;
            //            }

            //            FireUpdateResult(result);
            //        }
            //        else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)// parameter.PathType == PathType.Continuous)
            //        {
            //        }
            //    }
            //}

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
