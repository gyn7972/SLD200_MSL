using QMC.Common.Modules;
using QMC.Common.PathGenerators;
using QMC.Common.Vision;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Parts.ScannerCompensator;
using static QMC.Common.PathGenerators.PathGenerator;

using QMC.Common.Motion;
using QMC.Common.Motion.Ajin.Motions;
using static QMC.Common.Modules.WorkStage;
using Newtonsoft.Json.Linq;
using static QMC.Common.Vision.Tools.PatternMatchingResult;
using System.ServiceModel.Syndication;

namespace QMC.Common.Parts
{
    #region ScannerCompensator
    [Serializable]
    public class ScannerCompensator : PatternMatchingVisionPart
    {
        #region Define
        [Serializable]
        public enum GridXyMotionPositionKeys
        {
            StartPosition,
        }

        [Serializable]
        public enum SearchMethod
        {
            PatternMatching,
            Blob
        }
        #endregion

        #region Field
        private WorkStage m_Owner;

        // InterpolatorMotionFunction 인스턴스 추가
        public InterpolatorMotionFunction MC_Func = new InterpolatorMotionFunction();

        #endregion

        public override void UpdateRecipeData()
        {
            if(Owner is WorkStage ws)
            {
                this.Recipe = ws.Recipe.scannerCompensatorRecipe;
            }
            base.UpdateRecipeData();
        }

        #region Constructor
        public ScannerCompensator(string strName) : base(strName)
        {
            this.Recipe = new ScannerCompensatorRecipe(this);
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
        //public XyzLDzzxzULzzxzStage Stage { set; get; }

        private ScannerCompensatorRecipe _recipe;
        public XyzyStage Stage { set; get; }
        public ScannerCompensatorConfig Config { set; get; }
        public ScannerCompensatorRecipe Recipe
        {
            set
            {
                _recipe = value;
            }
            get
            {
                if (_recipe == null)
                {
                    _recipe = new ScannerCompensatorRecipe(this);
                }
                return _recipe;



            }
        }
        public TwoDimensionPathGenerator GridPathGenerator { get; set; }
        public RectangleZigzagTwoDimensionPathGeneratorParameter PathGeneratorParameter { get; set; }

        public BlobVisionTool BlobVisionTool { get; set; }

        private XyCoordinate _resultPosition;
        public XyCoordinate ResultPosition
        {
            get
            {
                return _resultPosition;
            }
            set
            {
                _resultPosition = value;
            }
        }

        private XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();         //  Stage XY Map Data 로 변환된 위치 이동 좌표

        CorrectionDataSaver correctionDataSaver = new CorrectionDataSaver();

        public Action<string> ActionSaveDone;
        public Action<QMCFindLenzCenter> ActionSaveDoneAllData;

        // CorrectionData 전달을 위한 이벤트 정의
        public event Action<List<CorrectionData>> CorrectionDataUpdated;

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

        private int SetPathGeneratorParameter()
        {
            int ret = 0;

            this.PathGeneratorParameter = new RectangleZigzagTwoDimensionPathGeneratorParameter();
            //this.PathGeneratorParameter.InvertedX = this.Config.Parameter.PathInvetedX;
            //this.PathGeneratorParameter.InvertedX = this.Config.Parameter.PathInvertedY;
            this.PathGeneratorParameter.PathType = PathType.StepByStep;
            this.PathGeneratorParameter.StartCoordinate = (XyCoordinate)this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
            this.PathGeneratorParameter.PitchCount = new Size(this.Config.Count);
            this.PathGeneratorParameter.Direction = this.Config.Direction;
            this.PathGeneratorParameter.StartLocation = this.Config.StartLocation;
            this.PathGeneratorParameter.AreaCalculationParameterType = ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes.PitchCount;
            this.PathGeneratorParameter.PitchDistance = new SizeD(Config.PitchDistanceX, Config.PitchDistanceY);

            return ret;
        }

        public int CreatePathGeneratorSync()
        {
            return this.CreatePathGeneratorProcedure();
        }

        private int CreatePathGeneratorProcedure()
        {
            int ret = 0;
            if ((ret = this.OnCreatePathGenerator()) != 0)
            {
                //this.DieUnloadPathGenerated = false;
                return ret;
            }
            //this.DieUnloadPathGenerated = true;
            return ret;
        }

        protected virtual int OnCreatePathGenerator()
        {
            int ret = 0;

            if ((ret = this.SetPathGeneratorParameter()) != 0) return ret;
            TwoDimensionPathGenerator generator = null;

            if ((ret = this.PathGeneratorParameter.CreatePathGenerator(out generator)) != 0) return ret;
            this.GridPathGenerator = generator;

            return ret;
        }

        public int Train()
        {
            int ret = 0;

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

        public BlobResult Blob()
        {
            int ret = 0;
            if ((ret = this.OnBlob(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.BlobParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return this.BlobVisionTool.Result;
        }

        private int OnBlob(Point startRoiPoint, Point endRoiPoint, BlobVisionToolParameter parameter, IlluminationDataSet illuminationData)
        {
            int ret = 0;

            VisionImage image;

            if (Simulated)
            {
                image = TestImage;
            }
            else
            {
                if (Illuminator != null)
                {
                    if ((ret = OnSetIllumination(illuminationData, true)) != 0) return ret;
                }
                if ((ret = this.Camera.GrabSync(Vision.Cameras.Purpose.Processing, out image)) != 0) return ret;
            }

            this.BlobVisionTool.Parameter.Polarity = parameter.Polarity;
            this.BlobVisionTool.Parameter.MinPixels = parameter.MinPixels;
            this.BlobVisionTool.Parameter.RepeatCount = parameter.RepeatCount;

            m_RoiInspect.Parameter.StartLocation = startRoiPoint;
            m_RoiInspect.Parameter.EndLocation = endRoiPoint;

            m_RoiTrain.Parameter.IsFull = true;

            m_RoiInspect.InputImage = image;
            m_RoiInspect.Parameter.IsFull = false;

            if ((ret = m_RoiInspect.Run()) != 0) return ret;
            this.BlobVisionTool.InputImage = m_RoiInspect.OutputImage;

            if ((ret = this.BlobVisionTool.Run()) != 0) return ret;

            if (this.BlobVisionTool.Result.PixelValues.Count <= 0)
            {
                Log.Write(this.Name, "Blob is Fail");
                return ret;
            }

            return ret;
        }

        public BlobResult GetBlobResult()
        {
            return this.BlobVisionTool.Result;
        }

        public int MoveStageXY(double x, double y)
        {
            int ret = 0;

            if (Stage != null)
            {
                XyzCoordinate currentPos = new XyzCoordinate();
                this.Stage.GetCommandPosition(ref currentPos);
                this.MoveStageXY(x, y, currentPos.Z);
            }

            return ret;
        }

        public int MoveStageXY(double x, double y, double z)
        {
            int ret = 0;

            //Get Motion Setting
            //Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();
            Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections(); ;

            if (dicMovingProjection != null)
            {
                dicMovingProjection[XyStage.MotionKey.X.ToString()].Position = x;
                dicMovingProjection[XyStage.MotionKey.Y.ToString()].Position = y;
                //dicMovingProjection[XyStage.MotionKey.Z.ToString()].Position = z;

                this.Stage.Move(dicMovingProjection);
                Thread.Sleep(Config.MoveToDelay);
            }

            return ret;
        }
        #endregion

        #region VisionPart Members
        public override int Create()
        {
            int ret = base.Create();

            if (this.BlobVisionTool == null)
            {
                this.BlobVisionTool = new VisionProBlobVisionTool("BlobVisionTool");
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

        public override int OnWork()
        {
            int ret = 0;
            m_Owner = this.Owner as WorkStage;
            if (m_Status == RunStatus.Stop) return 1;
            if (this.Stage == null) return -1;

            if ((ret = this.RunSearchGridXy()) != 0) return ret;

            return ret;
        }

        public override void UpdateConfigData() //참고 : Override
        {
            // Todo : 여기
            //if(Recipe.IlluminationDataSet == null)
            //{
            //    Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
            //    Recipe.IlluminationDataSet.SetIlluminationChannel(Owner.Config.)
            //}

            if (Owner is WorkStage)
            {
                WorkStage workStage = Owner as WorkStage;
                if (workStage != null)
                {
                    // Todo : 여기 풀어보자. 250421
                    this.Config = workStage.Config.ScannerCompensatorConfig;
                }
            }
        }

        public int RunSearchMark()
        {
            int ret = 0;

            //VisionProPatternMatchingVisionTool
            PatternMatchingResult patternMatchingResult = null;
            XyzCoordinate currentPos = new XyzCoordinate();
            XyCoordinate resultPosition = new XyCoordinate();

            patternMatchingResult = Search();

            if(patternMatchingResult == null)
            {
                MessageBox.Show("Can not Search Center Mark");

                return -1;
            }

            if (patternMatchingResult.Values.Count <= 0)
            {
                MessageBox.Show("Can not Search Center Mark");

                return -1;
            }

            this.Stage.GetCommandPosition(ref currentPos);

            if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
            {
                VisionScale m_TempScale = new VisionScale();
                m_TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                m_TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                m_TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                m_TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

                VisionScale.ConvertPosition<XyCoordinate>(m_TempScale, Camera.Resolution, patternMatchingResult.Values[0], out resultPosition);
            }
            else
            {
                VisionScale.ConvertPosition<XyCoordinate>(this.m_Owner.Scale, this.m_Owner.Camera_HighRes.Resolution, patternMatchingResult.Values[0], out resultPosition);
            }

            _resultPosition.X = currentPos.X + resultPosition.X;
            _resultPosition.Y = currentPos.Y + resultPosition.Y;

            ret = 1;
            return ret;
        }
        class ResultData
        {
            public ResultData(string fileName, PatternMatchingResult results, XyzCoordinate commandPosition)
            {
                FileName = fileName;
                Results = results;
                CommandPosition = commandPosition;
            }
            public string FileName { get; set; }
            public PatternMatchingResult Results { get; set; }
            public XyzCoordinate CommandPosition { get; set; }
            public int x;
            public int y;       
        }
        class RunData
        {
            public List<VisionImage> images;
            public XyzCoordinate CommandPosition;
            public bool bFirst;
            public int x;
            public int y;
        }
        private int SearchGridXy(out List<PositionOffset> results, out XyCoordinate CommandPosition)
        {
            int ret = 0;

            string fileName = "ScannerCompensatorData" + DateTime.Now.ToString("dd_HH_ss");
            string m_strScannerCompensatorDataPath = "";

            CycleTimer timer = new CycleTimer();
            PatternMatchingResult patternMatchingResult = null;
            BlobResult blobResult = null;
            XyzCoordinate position = new XyzCoordinate();
            XyzCoordinate currentPosition = new XyzCoordinate();
            XyCoordinate resultPosition = new XyCoordinate();
            PositionOffset result = new PositionOffset();
            XyzCoordinate movePosition = new XyzCoordinate();
            XyCoordinate centerPosition = new XyCoordinate();
            QMCFindLenzCenter findLenzCenter = new QMCFindLenzCenter();

            int defaultXIndex = (int)(((this.Config.Count.X - 1) * this.Config.PitchDistanceX) / 2);
            int defaultYIndex = -(int)(((this.Config.Count.Y - 1) * this.Config.PitchDistanceY) / 2);

            results = new List<PositionOffset>();
            CommandPosition = new XyCoordinate();

            if (m_Status == RunStatus.Stop) return 1;

            timer.Start();

            this.Stage.GetCommandPosition(ref currentPosition);

            movePosition = currentPosition;

            centerPosition.X = this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].X + (this.Config.PitchDistanceX * (this.Config.Count.X / 2));
            centerPosition.Y = this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Y + (this.Config.PitchDistanceY * (this.Config.Count.Y / 2));

            #region 이중 For문 사용 - 주석
            //for (int y = 0; y < this.Config.Count.Y; y++)
            //{
            //    for (int x = 0; x < this.Config.Count.X; x++)
            //    {
            //        position = new XyzCoordinate(movePosition.X + this.Config.PitchDistanceX * x, movePosition.Y + this.Config.PitchDistanceY * y, movePosition.Z);

            //        Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();

            //        dicMovingProjection[XyzyStage.MotionKey.X.ToString()].Position = position.X;
            //        dicMovingProjection[XyzyStage.MotionKey.Y.ToString()].Position = position.Y;
            //        dicMovingProjection[XyzyStage.MotionKey.Z.ToString()].Position = position.Z;

            //        if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
            //        Thread.Sleep(Config.MoveToDelay);

            //        XyzCoordinate currentPos = new XyzCoordinate();
            //        this.Stage.GetCommandPosition(ref currentPos);
            //        CommandPosition = (XyCoordinate)currentPos;

            //        patternMatchingResult = this.Search();

            //        if (patternMatchingResult.Values.Count <= 0)
            //        {
            //            PatternMatchingResult.PatternMatchingResultValue value = new PatternMatchingResult.PatternMatchingResultValue();
            //            value.X = this.Camera.Resolution.Width / 2;
            //            value.Y = this.Camera.Resolution.Height / 2;
            //            value.R = 0.0;

            //            patternMatchingResult.Values.Add(value);
            //        }

            //        if (this.Stage.GetCommandPosition(ref currentPosition) != 0) continue;

            //        VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, m_Owner.Camera.Resolution, patternMatchingResult.Values[0], out resultPosition);
            //        //resultPosition = new XyCoordinate((this.m_Owner.Camera.Resolution.Width / 2 - intersectPoint.X) * this.m_Owner.Scale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1), (intersectPoint.Y - this.m_Owner.Camera.Resolution.Height / 2) * this.m_Owner.Scale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));

            //        result = new PositionOffset((XyCoordinate)currentPosition, resultPosition);
            //        results.Add(result);

            //        int xIndex = (int)(defaultXIndex + this.Config.PitchDistanceX * x);
            //        int yIndex = (int)(defaultYIndex + this.Config.PitchDistanceY * y);

            //        double resultX = xIndex - result.Offset.X;
            //        double resultY = yIndex + result.Offset.Y;

            //        // data format : row, col, reference, measured
            //        LogManager.Instance.WriteTxt(fileName, string.Format($"{y}, {x} : {xIndex.ToString("0.000")}, {yIndex.ToString("0.000")}, {resultX.ToString("0.000")}, {resultY.ToString("0.000")}"));
            //    }
            //}
            #endregion 
            Task<ResultData> task = null;
            bool bFirst = true;
            #region 이중 For문 사용 - 90도 회전.
            for (int x = 0; x < this.Config.Count.X; x++)
            {
                for (int y = 0; y < this.Config.Count.Y; y++)
                {
                    if (m_Status == RunStatus.Stop)
                    {
                        return ret;
                    }

                    #region 이중 for문 내 - 주석
                    //Count Check하고 Center위치로가서 보정하고
                    //if (((x * this.Config.Count.Y) + y) % this.Config.CenterCheckCount == 0 && y != 0)
                    //{
                    //    this.MoveStageXY(centerPosition.X, centerPosition.Y);

                    //    //3. Search후 Center로 Move.
                    //    if (this.Config.SearchMethod == SearchMethod.PatternMatching)
                    //    {
                    //        patternMatchingResult = this.Search();

                    //        if (patternMatchingResult.Values.Count <= 0)
                    //        {
                    //            MessageBox.Show("Can not Search Center Mark");
                    //            return ret;
                    //        }

                    //        this.Stage.GetCommandPosition(ref currentPosition);

                    //        VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, m_Owner.Camera.Resolution, patternMatchingResult.Values[0], out resultPosition);
                    //    }
                    //    else
                    //    {
                    //        blobResult = this.Blob();

                    //        if (blobResult.PixelValues.Count <= 0)
                    //        {
                    //            MessageBox.Show("Can not Blob Center Mark");
                    //            return ret;
                    //        }

                    //        this.Stage.GetCommandPosition(ref currentPosition);

                    //        VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, m_Owner.Camera.Resolution, new PointD(blobResult.PixelValues[0][1].Value, blobResult.PixelValues[0][2].Value), out resultPosition);
                    //    }
                    //    centerPosition.X = centerPosition.X + resultPosition.X;
                    //    centerPosition.Y = centerPosition.Y + resultPosition.Y;

                    //    movePosition.X = movePosition.X + resultPosition.X;
                    //    movePosition.Y = movePosition.Y + resultPosition.Y;
                    //}
                    #endregion

                    position = new XyzCoordinate(movePosition.X + this.Config.PitchDistanceX * x, movePosition.Y + this.Config.PitchDistanceY * y, movePosition.Z);

                    double lfVelocity;
                    double lfAccDec;
                    //  속도 설정
                    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Speed_Coarse;
                    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

                    xyInterpolatedCoordinate.X = position.X; //stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X;
                    xyInterpolatedCoordinate.Y = position.Y; //stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y;

                    MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
                    int nWait = 0;
                    while (true)
                    {
                        if (MC_Func.MC_GetDone((int)WorkStage.nAxis.X) == true)
                        {
                            break;
                        }
                        Thread.Sleep(1);
                        nWait++;
                        if (nWait == 1000)
                        {
                            break;
                        }

                    }

                    nWait = 0;
                    while (true)
                    {
                        if (MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) == true)
                        {
                            break;
                        }
                        Thread.Sleep(1);
                        nWait++;
                        if (nWait == 1000)
                        {
                            break;
                        }

                    }
                    //Thread.Sleep(Config.MoveToDelay);
                    Thread.Sleep(500);

                    XyzCoordinate currentPos = new XyzCoordinate();
                    this.Stage.GetCommandPosition(ref currentPos);
                    CommandPosition = (XyCoordinate)currentPos;

                    if (this.Config.SearchMethod == SearchMethod.PatternMatching)
                    {
                        List<VisionImage> images = new List<VisionImage>();
                        DateTime dt = DateTime.Now;

                        for (int iter = 0; iter < 5; iter++)
                        {
                            VisionImage image = null;
                            this.Camera.GrabSync(Vision.Cameras.Purpose.Processing, out image);
                            images.Add(image);
                        }
                        TimeSpan ts = DateTime.Now - dt;
                        WaitNCalcData(results, fileName, ref resultPosition, ref result, findLenzCenter, defaultXIndex, defaultYIndex, task);
                        RunData runData = new RunData();
                        runData.images = images;
                        runData.bFirst = bFirst;
                        runData.x = x;
                        runData.y = y;
                        bFirst = false;
                        runData.CommandPosition = currentPosition;
                        task = Task.Factory.StartNew((obj) =>
                        {
                            RunData run = obj as RunData;
                            List<VisionImage> visionImages = (List<VisionImage>)run.images;
                            PatternMatchingResult pmrAll = new PatternMatchingResult();
                            PatternMatchingResult pmrAll_Circle = new PatternMatchingResult();

                            foreach (VisionImage imageGrabed in visionImages)
                            {
                                
                                int r = this.OnSearch(imageGrabed, Recipe.InspectRoiStartLocation
                                    , Recipe.InspectRoiEndLocation
                                    , Recipe.PatternMatchingParameter
                                    , Recipe.IlluminationDataSet, runData.bFirst);
                                PatternMatchingResult pmr = this.m_PatternMatchingTool.Result;
                                if (pmr.Values.Count > 0)
                                {
                                    PatternMatchingResultValue pmrv = new PatternMatchingResultValue();

                                    pmrv.X = pmr.Values[0].X; //빠진거겠지?
                                    pmrv.Y = pmr.Values[0].Y;
                                    pmrv.R = pmr.Values[0].R;
                                    pmrv.Score = pmr.Values[0].Score;

                                    pmrAll.Values.Add(pmrv);
                                }

                                if (Equipment.Scanner_Calibration_UseBlobVisionTool)
                                {
                                    //Circle 찾는 알고리듬 적용
                                    PatternMatchingResultValue pmCircle = new PatternMatchingResult.PatternMatchingResultValue();       //Blob                                                                                  
                                    bool bFind = false;
                                    QMC_ImageProcessFindAlign qip = new QMC_ImageProcessFindAlign();
                                    List<RectangleF> Fiducial_circlesResult = new List<RectangleF>();

                                    VisionScale m_TempScale = new VisionScale();
                                    m_TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                                    m_TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                                    m_TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                                    m_TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;
                                    
                                    double pixelR = (Equipment.Scanner_Calibration_CrossMarkLength/2) / (m_TempScale.X);

                                    qip.FindCirclesWidthCircleBoundary(Fiducial_circlesResult, Camera.LatestImage.RawData
                                            , Camera.LatestImage.Header.Width
                                            , Camera.LatestImage.Header.Height
                                            , (int)pixelR, 0.5, ref bFind, (int)pmr.Values[0].X, (int)pmr.Values[0].Y);
                                    //1000, 1 -> 엄청느린값 // 원의 반지름의 값이랑 오차범위
                                    //센터점 전달해서 찾기로, 센터 못찾으면 그냥 센터로. 


                                    if (Fiducial_circlesResult.Count > 0 && bFind == true)
                                    {
                                        double cx = Fiducial_circlesResult[0].X + (Fiducial_circlesResult[0].Width / 2);
                                        double cy = Fiducial_circlesResult[0].Y + (Fiducial_circlesResult[0].Height / 2);
                                        pmCircle.X = cx;
                                        pmCircle.Y = cy;

                                        pmrAll_Circle.Values.Add(pmCircle);
                                    }
                                }
                            }

                            ResultData resultData = new ResultData("", Equipment.Scanner_Calibration_UseBlobVisionTool? pmrAll_Circle:pmrAll, run.CommandPosition);
                            resultData.x = run.x;
                            resultData.y = run.y;

                            return resultData;
                        }, runData);
                        continue;
                    }
                    else
                    {
                        blobResult = this.Blob();
                        if (blobResult.PixelValues.Count <= 0)
                        {
                            if (this.Stage.GetCommandPosition(ref currentPosition) != 0) continue;

                            if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
                            {
                                VisionScale m_TempScale = new VisionScale();
                                m_TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                                m_TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                                m_TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                                m_TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

                                VisionScale.ConvertPosition<XyCoordinate>(m_TempScale, m_Owner.Camera_HighRes.Resolution, new PointD(this.Camera.Resolution.Width / 2, this.Camera.Resolution.Height / 2), out resultPosition);
                            }
                            else
                            {
                                VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, m_Owner.Camera_HighRes.Resolution, new PointD(this.Camera.Resolution.Width / 2, this.Camera.Resolution.Height / 2), out resultPosition);
                            }


                            //double xIndex = defaultXIndex - this.Config.PitchDistanceX * x;
                            double xIndex = defaultXIndex - this.Config.PitchDistanceX * x;
                            double yIndex = defaultYIndex + this.Config.PitchDistanceY * y;

                            PointD offset = new PointD(result.Offset.X, result.Offset.Y);

                            //double resultX = xIndex + offset.X;
                            //double resultY = yIndex + offset.Y;
                            double resultX = xIndex - offset.X;
                            double resultY = yIndex - offset.Y;

                            // data format : row, col, reference, measured
                            LogManager.Instance.WriteTxt(fileName, string.Format($"{x}, {y} : {yIndex.ToString("0.000")}, {xIndex.ToString("0.000")}, {resultY.ToString("0.00000")}, {resultX.ToString("0.00000")}"));
                            //LogManager.Instance.WriteTxt(fileName, string.Format($"{x}, {y} : {xIndex.ToString("0.000")}, {yIndex.ToString("0.000")}, {resultX.ToString("0.000")}, {resultY.ToString("0.000")}"));

                            //Motor <-> Scanner 좌표에 따른 x, y -> y, x 반전.
                            int nIndexX = task.Result.x;
                            int nIndexY = task.Result.y;
                            double dX = TruncateTo3DecimalPlacesAndZeroRest(yIndex);
                            double dY = TruncateTo3DecimalPlacesAndZeroRest(xIndex);
                            double dMeasureX = TruncateTo3DecimalPlacesAndZeroRest(resultY);
                            double dMeasureY = TruncateTo3DecimalPlacesAndZeroRest(resultX);

                            findLenzCenter.AddSLDMeasureData(new SLDMeasureData(nIndexX, nIndexY, dX, dY, dMeasureX, dMeasureY));
                            //findLenzCenter.AddSLDMeasureData(new SLDMeasureData(x, y, yIndex, xIndex, resultY, resultX));
                            //findLenzCenter.AddSLDMeasureData(new SLDMeasureData(x, y, yIndex, xIndex, offset.Y, offset.X));
                        }
                    }
                }
            }
            #endregion
            if (this.Config.SearchMethod == SearchMethod.PatternMatching)
            {
                WaitNCalcData(results, fileName, ref resultPosition, ref result, findLenzCenter, defaultXIndex, defaultYIndex, task);
            }

            // Convert 대기를 위한 변수 처리.
            Equipment.Scanner_Calibration_Convert = 0;

            //  폴더 없으면 만들기
            m_strScannerCompensatorDataPath = LogManager.Instance.GetLogPath() + "\\ScannerCalData";
            if (Directory.Exists( m_strScannerCompensatorDataPath ) == false)
            {
                Directory.CreateDirectory(m_strScannerCompensatorDataPath);
            }
            string fileName1 = findLenzCenter.SaveData(LogManager.Instance.GetLogPath() + "\\ScannerCalData");

            correctionDataSaver.SaveData(LogManager.Instance.GetLogPath() + "\\ScannerCalData");

            ActionSaveDone?.Invoke(fileName1);
            ActionSaveDoneAllData?.Invoke(findLenzCenter);

            timer.End();
            Log.Write("ScannerCompensator Time", string.Format($"{timer.Latest.Interval.TotalSeconds.ToString()}"));
            return ret;
        }

        private void WaitNCalcData(List<PositionOffset> results, string fileName, ref XyCoordinate resultPosition, ref PositionOffset result, QMCFindLenzCenter findLenzCenter, int defaultXIndex, int defaultYIndex, Task<ResultData> task)
        {
            if (task != null)
            {
                task.Wait();
                PatternMatchingResultValue avgValue = new PatternMatchingResult.PatternMatchingResultValue();   //Pattern
                
                try
                {
                    try
                    {
                        avgValue.X = task.Result.Results.Values.Average(t => t.X);
                        avgValue.Y = task.Result.Results.Values.Average(t => t.Y);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                        avgValue.X = 0;
                        avgValue.Y = 0;
                        Log.Write(ex);
                    }
                    //Log.Write("ScannerCompensator", string.Format($"PatternX : {avgValue.X}, PatternY : {avgValue.Y}"));

                    if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
                    {
                        VisionScale m_TempScale = new VisionScale();
                        m_TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                        m_TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                        m_TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                        m_TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;
                        VisionScale.ConvertPosition<XyCoordinate>(m_TempScale, Camera.Resolution, avgValue, out resultPosition);
                        //Log.Write("ScannerCompensator", string.Format($"PatternX : {avgValue.X}, PatternY : {avgValue.Y}"));
                    }
                    else
                    {
                        VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, Camera.Resolution, avgValue, out resultPosition);
                        //Log.Write("ScannerCompensator", string.Format($"PatternX : {avgValue.X}, PatternY : {avgValue.Y}"));
                    }

                    result = new PositionOffset((XyCoordinate)task.Result.CommandPosition, resultPosition);
                    results.Add(result);

                    //double xIndex = defaultXIndex - this.Config.PitchDistanceX * x;
                    double xIndex = defaultXIndex - this.Config.PitchDistanceX * task.Result.x;
                    double yIndex = defaultYIndex + this.Config.PitchDistanceY * task.Result.y;

                    PointD offset = new PointD(result.Offset.X, result.Offset.Y);

                    //기존
                    //double resultX = xIndex + offset.X;
                    //double resultY = yIndex + offset.Y;
                    //FormNew_Setup에서 수정하던 부분 옮김.
                    double resultX = xIndex - offset.X;
                    double resultY = yIndex - offset.Y;

                    // data format : row, col, reference, measured
                    LogManager.Instance.WriteTxt(fileName, string.Format($"{task.Result.x}, {task.Result.y} : {yIndex.ToString("0.000")}, {xIndex.ToString("0.000")}, {resultY.ToString("0.00000")}, {resultX.ToString("0.00000")}"));
                    //LogManager.Instance.WriteTxt(fileName, string.Format($"{x}, {y} : {xIndex.ToString("0.000")}, {yIndex.ToString("0.000")}, {resultX.ToString("0.000")}, {resultY.ToString("0.000")}"));

                    //Motor <-> Scanner 좌표에 따른 x, y -> y, x 반전.
                    int x = task.Result.x;// TruncateTo3DecimalPlaces()
                    int y = task.Result.y;
                    double dX = TruncateTo3DecimalPlacesAndZeroRest(yIndex);
                    double dY = TruncateTo3DecimalPlacesAndZeroRest(xIndex);
                    double dMeasureX = TruncateTo3DecimalPlacesAndZeroRest(resultY);
                    double dMeasureY = TruncateTo3DecimalPlacesAndZeroRest(resultX);

                    findLenzCenter.AddSLDMeasureData(new SLDMeasureData(x, y, dX, dY, dMeasureX, dMeasureY));
                    //findLenzCenter.AddSLDMeasureData(new SLDMeasureData(task.Result.x, task.Result.y, yIndex, xIndex, resultY, resultX));
                    //findLenzCenter.AddSLDMeasureData(new SLDMeasureData(x, y, yIndex, xIndex, offset.Y, offset.X));
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        private int RunSearchGridXy()
        {
            int ret = 0;
            List<PositionOffset> results = null;
            XyCoordinate position = new XyCoordinate();
            XyzCoordinate movePosition = new XyzCoordinate();
            XyzCoordinate currentPos = new XyzCoordinate();
            XyCoordinate resultPosition = new XyCoordinate();

            PatternMatchingResult patternMatchingResult = null;
            BlobResult blobResult = null;

            CycleTimer timer = new CycleTimer();

            if (m_Status == RunStatus.Stop) return 1;

            //1. StartPoint로 이동.
            Dictionary<string, MovingProjection> dicMovingProjection = this.Stage.GetDefaultMovingProjections();

            #region 밖에서 우측상단으로 이동한 후에 들어오기 때문에 아래 구문 필요없음 - 주석
            //밖에서 우측상단으로 이동한 후에 들어오기 때문에 아래 구문 필요없음.
            //movePosition.X = this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].X;
            //movePosition.Y = this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Y;
            //movePosition.Z = this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Z;

            //dicMovingProjection[XyzyStage.MotionKey.X.ToString()].Position = movePosition.X;
            //dicMovingProjection[XyzyStage.MotionKey.Y.ToString()].Position = movePosition.Y;
            //dicMovingProjection[XyzyStage.MotionKey.Z.ToString()].Position = movePosition.Z;

            //if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
            //Thread.Sleep(Config.MoveToDelay);

            ////2. CenterPoint로 이동. (Count와 Pitch 이용)
            //this.Stage.GetCommandPosition(ref currentPos);
            ////movePosition.X = currentPos.X - (this.Config.PitchDistanceX * (this.Config.Count.X / 2));
            ////movePosition.Y = currentPos.Y - (this.Config.PitchDistanceY * (this.Config.Count.Y / 2));
            //movePosition.X = currentPos.X + (this.Config.PitchDistanceX * (this.Config.Count.X / 2));
            //movePosition.Y = currentPos.Y + (this.Config.PitchDistanceY * (this.Config.Count.Y / 2));
            //dicMovingProjection[XyzyStage.MotionKey.X.ToString()].Position = movePosition.X;
            //dicMovingProjection[XyzyStage.MotionKey.Y.ToString()].Position = movePosition.Y;
            //dicMovingProjection[XyzyStage.MotionKey.Z.ToString()].Position = movePosition.Z;
            //if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
            //Thread.Sleep(Config.MoveToDelay);
            ////3. Search후 Center로 Move.
            //if (this.Config.SearchMethod == SearchMethod.PatternMatching)
            //{
            //    patternMatchingResult = this.Search();

            //    if (patternMatchingResult.Values.Count <= 0)
            //    {
            //        MessageBox.Show("Can not Search Center Mark");
            //        return ret;
            //    }

            //    this.Stage.GetCommandPosition(ref currentPos);

            //    if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
            //    {
            //        VisionScale m_TempScale = new VisionScale();
            //        m_TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
            //        m_TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
            //        m_TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
            //        m_TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

            //        VisionScale.ConvertPosition<XyCoordinate>(m_TempScale, m_Owner.Camera_HighRes.Resolution, patternMatchingResult.Values[0], out resultPosition);
            //    }
            //    else
            //    {
            //        VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, m_Owner.Camera_HighRes.Resolution, patternMatchingResult.Values[0], out resultPosition);
            //    }                
            //}
            //else
            //{
            //    blobResult = this.Blob();

            //    if (blobResult.PixelValues.Count <= 0)
            //    {
            //        MessageBox.Show("Can not Blob Center Mark");
            //        return ret;
            //    }

            //    this.Stage.GetCommandPosition(ref currentPos);

            //    if (((WorkStage)this.Owner).Config.ParamConfig.ManualScale_Usage)
            //    {
            //        VisionScale m_TempScale = new VisionScale();
            //        m_TempScale.X = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
            //        m_TempScale.Y = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
            //        m_TempScale.InvertedX = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
            //        m_TempScale.InvertedY = ((WorkStage)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

            //        VisionScale.ConvertPosition<XyCoordinate>(m_TempScale, m_Owner.Camera_HighRes.Resolution, new PointD(blobResult.PixelValues[0][1].Value, blobResult.PixelValues[0][2].Value), out resultPosition);
            //    }
            //    else
            //    {
            //        VisionScale.ConvertPosition<XyCoordinate>(m_Owner.Scale, m_Owner.Camera_HighRes.Resolution, new PointD(blobResult.PixelValues[0][1].Value, blobResult.PixelValues[0][2].Value), out resultPosition);
            //    }                
            //}
            //movePosition.X = currentPos.X + resultPosition.X;
            //movePosition.Y = currentPos.Y + resultPosition.Y;

            //dicMovingProjection[XyzyStage.MotionKey.X.ToString()].Position = movePosition.X;
            //dicMovingProjection[XyzyStage.MotionKey.Y.ToString()].Position = movePosition.Y;
            //dicMovingProjection[XyzyStage.MotionKey.Z.ToString()].Position = movePosition.Z;

            //if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
            //Thread.Sleep(Config.MoveToDelay);

            ////4. StartPoint로 이동. (Count와 Pitch 이동)
            //this.Stage.GetCommandPosition(ref currentPos);

            ////movePosition.X = currentPos.X + (this.Config.PitchDistanceX * (this.Config.Count.X / 2));
            ////movePosition.Y = currentPos.Y + (this.Config.PitchDistanceY * (this.Config.Count.Y / 2));
            //movePosition.X = currentPos.X - (this.Config.PitchDistanceX * (this.Config.Count.X / 2));
            //movePosition.Y = currentPos.Y - (this.Config.PitchDistanceY * (this.Config.Count.Y / 2));

            //dicMovingProjection[XyzyStage.MotionKey.X.ToString()].Position = movePosition.X;
            //dicMovingProjection[XyzyStage.MotionKey.Y.ToString()].Position = movePosition.Y;
            //dicMovingProjection[XyzyStage.MotionKey.Z.ToString()].Position = movePosition.Z;

            //if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
            //Thread.Sleep(Config.MoveToDelay);

            //5. 현재 위치 StartPoint 저장
            //this.Stage.GetCommandPosition(ref currentPos);
            //this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate = currentPos;
            //m_Owner.SaveConfigData();
            //Path Generator 생성 주석
            //timer.Start();
            //if ((ret = this.CreatePathGeneratorSync()) != 0) return ret;
            //timer.End();
            //Log.Write("MotionVisionCompensator", string.Format("Create Path Generator [{0}] Completed. Inverval: {1} ms", OperatorKeys.Measurement, timer.Latest.Interval));
            #endregion

            if ((ret = this.SearchGridXy(out results, out position)) != 0) return ret;

            //Log.Write("MotionVisionCompensator", string.Format("Search Grid [{0}] Complete. Inverval: {1} ms", OperatorKeys.Measurement, timer.Latest.Interval));
            Config.XyGridSearchResults = results;
            Config.Positions.Add(position);

            //if (this.Motion.XyPositionCompensator != null && this.Motion.XyPositionCompensator is GridPositionCompensator)
            //{
            //    compensator = this.Motion.XyPositionCompensator as GridPositionCompensator;
            //    compensator.SetOffset(result);
            //}

            return ret;
        }

        public static double TruncateTo3DecimalPlacesAndZeroRest(double value)
        {
            // 먼저 소수점 셋째 자리까지 자르기 (버림)
            double truncated = Math.Truncate(value * 1000) / 1000.0;

            // double은 소수점 자릿수 표현이 불확실하므로, string 포맷을 거쳐 보정 가능
            string fixedStr = truncated.ToString("F6");  // 항상 소수점 이하 6자리로 표현
            return double.Parse(fixedStr);               // 다시 double로 변환
        }
        #endregion
    }
    #endregion

    #region ScannerCompensatorParameter
    [Serializable]
    public class ScannerCompensatorParameter
    {
        [Browsable(false)]
        public XyzPositionDataCollection CrossPositions { set; get; }
        [Browsable(false)]
        public XyzPositionDataCollection GridPositions { set; get; }
        private ZigzagTwoDimensionPathGenerator.Direction m_Direction;
        private double m_PitchDistanceX;
        private double m_PitchDistanceY;
        private double m_VerificationPitchX;
        private double m_VerificationPitchY;
        private Point m_Count;
        private ZigzagTwoDimensionPathGenerator.StartLocation m_StartLocation;
        private double m_Velocity;
        private double m_Acc;
        private double m_Dcc;
        private bool m_InvertedX;
        private bool m_InvertedY;
        private int m_MoveToDelay;

        public ScannerCompensatorParameter()
        {
            Init();

            GridPositions.Clear();
            foreach (GridXyMotionPositionKeys key in Enum.GetValues(typeof(GridXyMotionPositionKeys)))
            {
                XyzPositionData positionBase = new XyzPositionData();
                positionBase.Name = key.ToString();
                GridPositions.Add(positionBase);

                XyzPositionData positionTarget = new XyzPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                GridPositions.Add(positionTarget);
            }
        }
        private void Init()
        {
            if (CrossPositions == null)
                CrossPositions = new XyzPositionDataCollection();
            if (GridPositions == null)
                GridPositions = new XyzPositionDataCollection();
            m_Direction = ZigzagTwoDimensionPathGenerator.Direction.Horizontal;
            m_PitchDistanceX = 0;
            m_PitchDistanceY = 0;
            m_VerificationPitchX = 0;
            m_VerificationPitchY = 0;
            m_Count = new Point();
            m_StartLocation = new ZigzagTwoDimensionPathGenerator.StartLocation();
            m_Velocity = 10;
            m_Acc = 100;
            m_Dcc = 100;
            m_InvertedX = false;
            m_InvertedY = false;
            m_MoveToDelay = 100;
        }
        [Category("GridXY")]
        public ZigzagTwoDimensionPathGenerator.Direction Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }
        [Browsable(false)]
        public double PitchDistanceX
        {
            get { return m_PitchDistanceX; }
            set { m_PitchDistanceX = value; }
        }
        [Browsable(false)]
        public double VerficationPitchDistanceX
        {
            get { return m_PitchDistanceY; }
            set { m_PitchDistanceY = value; }
        }
        [Browsable(false)]
        public double VerficationPitchDistanceY
        {
            get { return m_VerificationPitchX; }
            set { m_VerificationPitchX = value; }
        }
        [Browsable(false)]
        public double PitchDistanceY
        {
            get { return m_VerificationPitchY; }
            set { m_VerificationPitchY = value; }
        }

        [Category("GridXY")]
        public Point Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }
        [Category("GridXY")]
        public ZigzagTwoDimensionPathGenerator.StartLocation StartLocation
        {
            get { return this.m_StartLocation; }
            set { this.m_StartLocation = value; }
        }
        [Browsable(false)]
        public double Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }
        [Browsable(false)]
        public double Acc
        {
            get { return m_Acc; }
            set { m_Acc = value; }
        }
        [Browsable(false)]
        public double Dcc
        {
            get { return m_Dcc; }
            set { m_Dcc = value; }
        }
        [Browsable(false)]
        public bool InvertedX
        {
            get { return m_InvertedX; }
            set { m_InvertedX = value; }
        }
        [Browsable(false)]
        public bool InvertedY
        {
            get { return m_InvertedY; }
            set { m_InvertedY = value; }
        }
        [Category("기타")]
        public int MoveToDelay
        {
            get { return m_MoveToDelay; }
            set { m_MoveToDelay = value; }
        }
    }
    #endregion

    #region CompensationValue
    [Serializable]
    public class CompensationValue
    {
        #region Define

        #endregion

        #region Field
        private List<double> m_OffsetXAxisX;
        private List<double> m_OffsetXAxisY;
        private List<double> m_OffsetYAxisX;
        private List<double> m_OffsetYAxisY;
        #endregion

        #region Constructor
        public CompensationValue()
        {
            this.OffsetXAxisX = new List<double>();
            this.OffsetXAxisY = new List<double>();
            this.OffsetYAxisX = new List<double>();
            this.OffsetYAxisY = new List<double>();
        }
        #endregion

        #region Porperty

        public List<double> OffsetXAxisX
        {
            get { return this.m_OffsetXAxisX; }
            set { this.m_OffsetXAxisX = value; }
        }

        public List<double> OffsetXAxisY
        {
            get { return this.m_OffsetXAxisY; }
            set { this.m_OffsetXAxisY = value; }
        }

        public List<double> OffsetYAxisX
        {
            get { return this.m_OffsetYAxisX; }
            set { this.m_OffsetYAxisX = value; }
        }

        public List<double> OffsetYAxisY
        {
            get { return this.m_OffsetYAxisY; }
            set { this.m_OffsetYAxisY = value; }
        }
        #endregion
    }
    #endregion

    #region PositionOffset
    [Serializable]
    public struct PositionOffset/* : IComparable, IComparer*/
    {
        private XyCoordinate m_Position;
        private XyCoordinate m_Offset;

        public PositionOffset(XyCoordinate position, XyCoordinate offset)
        {
            this.m_Position = position;
            this.m_Offset = offset;
        }

        public XyCoordinate Position
        {
            get { return this.m_Position; }
            set { this.m_Position = value; }
        }

        public XyCoordinate Offset
        {
            get { return this.m_Offset; }
            set { this.m_Offset = value; }
        }

        public override string ToString()
        {
            return string.Format("Position = {0}, Offset = {1}", this.Position, this.Offset);
        }
    }
    #endregion
}