using Cognex.VisionPro;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public class VisionCalibrator : PatternMatchingVisionPart
    {
        #region Define
        public enum MoveDirection
        {
            Start,
            Top,
            Bottom,
            Left,
            Right,
        }

        public enum MotionPositionKeys
        {
            Start,
        }
        #endregion

        #region Property
        //protected List<XytCoordinate> m_listDies;
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
        //public XyztStage XyztStage { set; get; }
        //public XyzztStage XyzztStage { set; get; }
        public UvwzxyzStage UvwzxyzStage { set; get; }
        public VisionCalibratorConfig Config { set; get; }
        public VisionCalibratorRecipe Recipe { set; get; }
        #endregion

        public VisionCalibrator(string strName) : base(strName)
        {
            Config = new VisionCalibratorConfig();
            Recipe = new VisionCalibratorRecipe(this);
            IlluminationData = new IlluminationDataSet(Name);
        }

        public override int Create()
        {
            int ret = base.Create();

            return ret;
        }
        public override void Close()
        {
            base.Close();
        }

        public int Train()
        {
            int ret = 0;

            if ((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, Recipe.PatternMatchingParameters, IlluminationData)) != 0)
            {
                return ret;
            }

            if (Recipe != null)
            {
                if (Recipe.PatternMatchingParameters == null)
                {
                    PatternMatchingParameters newParameter = new PatternMatchingParameters();
                    Recipe.PatternMatchingParameters = newParameter;
                }
                Recipe.PatternMatchingParameters.TrainImage = TrainImage;
                Owner.SaveRecipeData();
            }
            return ret;
        }

        public PatternMatchingResult Search()
        {
            int ret = 0;
            if ((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameters, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;
        }

        //public XyztCoordinate GetCurrentPosition()
        //{
        //    XyztCoordinate current = new XyztCoordinate();
        //    if (XyztStage != null)
        //    {
        //        XyztStage.GetActualPosition(ref current);
        //    }

        //    return current;
        //}

        public UvwzxyzCoordinate GetCurrentPosition()
        {
            UvwzxyzCoordinate current = new UvwzxyzCoordinate();
            if (UvwzxyzStage != null)
            {
                UvwzxyzStage.GetActualPosition(ref current);
            }

            return current;
        }

        public override int OnWork()
        {
            int ret = 0;
            bool isCompensation = false;

            PatternMatchingResult patternMatchingResult = null;
            //XyztCoordinate targetCoordinate = new XyztCoordinate();
            //XyzztCoordinate targetCoordinate = new XyzztCoordinate();
            UvwzxyzCoordinate targetCoordinate = new UvwzxyzCoordinate();
            XyCoordinate coodinate = new XyCoordinate();
            DirectionResultKeyedCollection directionResult = new DirectionResultKeyedCollection();
            double temp = 0.0;
            VisionScale scale = new VisionScale();
            XyCoordinate MillimeterValue = new XyCoordinate();
            XyCoordinate PixelValue = new XyCoordinate();


            try
            {
                MoveDirection[] moveDirection = GetMoveDirections();
                for (int i = 0; i < moveDirection.Length; i++)
                {
                    // 시작 위치 설정
                    targetCoordinate = GetTargetCoordinate(moveDirection[i]);

                    if ((ret = this.PatternMatchingAfterMove(targetCoordinate, out patternMatchingResult)) != 0) return ret;

                    if (patternMatchingResult == null)
                        return -1;
                    directionResult.Add(new DirectionResult(new PointD(patternMatchingResult.Values[0].X, patternMatchingResult.Values[0].Y), moveDirection[i]));
                }

                // Scale Inverted 계산
                if (directionResult[MoveDirection.Start].PixelLocation.X < directionResult[MoveDirection.Right].PixelLocation.X)
                    scale.InvertedX = false;
                else
                    scale.InvertedX = true;


                if (directionResult[MoveDirection.Start].PixelLocation.Y < directionResult[MoveDirection.Top].PixelLocation.Y)
                    scale.InvertedY = false;
                else
                    scale.InvertedY = true;


                // Scale 계산
                scale.X = (this.Config.MoveDistance * 2) / qGeometry.GetDistanceBetweenTwoPoints(directionResult[MoveDirection.Left].PixelLocation, directionResult[MoveDirection.Right].PixelLocation);
                scale.Y = (this.Config.MoveDistance * 2) / qGeometry.GetDistanceBetweenTwoPoints(directionResult[MoveDirection.Top].PixelLocation, directionResult[MoveDirection.Bottom].PixelLocation);

                if (qGeometry.GetAngle(directionResult[MoveDirection.Left].PixelLocation, directionResult[MoveDirection.Right].PixelLocation, ref temp) == false) return -1;
                scale.XAxisT = temp;

                if (qGeometry.GetAngle(directionResult[MoveDirection.Bottom].PixelLocation, directionResult[MoveDirection.Top].PixelLocation, ref temp) == false) return -1;
                scale.YAxisT = temp;


                if ((ret = VisionScale.ConvertPosition<XyCoordinate>(scale, Camera.Resolution, patternMatchingResult.Values[0], out coodinate)) != 0) return ret;



                this.Config.Scale = scale;

                this.Owner.SetModuleScale(scale.X, scale.Y, scale.XAxisT, scale.YAxisT, scale.InvertedX, scale.InvertedY);
            }
            finally
            {

            }

            return ret;
        }

        protected virtual MoveDirection[] GetMoveDirections()
        {
            MoveDirection[] directions = null;

            directions = (MoveDirection[])Enum.GetValues(typeof(MoveDirection));

            return directions;
        }

        //protected virtual XyztCoordinate GetTargetCoordinate(MoveDirection direction)
        //{
        //    XyztCoordinate coordinate = new XyztCoordinate();

        //    if (direction == MoveDirection.Start)
        //        coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate;
        //    else if (direction == MoveDirection.Top)
        //        coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(0, this.Config.MoveDistance , 0.0, 0.0);
        //    else if (direction == MoveDirection.Bottom)
        //        coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(0, this.Config.MoveDistance * -1, 0.0, 0.0);
        //    else if (direction == MoveDirection.Left)
        //        coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(this.Config.MoveDistance * -1, 0, 0.0, 0.0);
        //    else if (direction == MoveDirection.Right)
        //        coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(this.Config.MoveDistance, 0, 0.0, 0.0);

        //    return coordinate;
        //}

        protected virtual UvwzxyzCoordinate GetTargetCoordinate(MoveDirection direction)
        {
            UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();

            //if (direction == MoveDirection.Start)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate;
            //else if (direction == MoveDirection.Top)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(0, this.Config.MoveDistance, 0.0, 0.0);
            //else if (direction == MoveDirection.Bottom)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(0, this.Config.MoveDistance * -1, 0.0, 0.0);
            //else if (direction == MoveDirection.Left)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(this.Config.MoveDistance * -1, 0, 0.0, 0.0);
            //else if (direction == MoveDirection.Right)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyztCoordinate(this.Config.MoveDistance, 0, 0.0, 0.0);

            //if (direction == MoveDirection.Start)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate;
            //else if (direction == MoveDirection.Top)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyzztCoordinate(0, this.Config.MoveDistance, 0.0, 0.0, 0.0);
            //else if (direction == MoveDirection.Bottom)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyzztCoordinate(0, this.Config.MoveDistance * -1, 0.0, 0.0, 0.0);
            //else if (direction == MoveDirection.Left)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyzztCoordinate(this.Config.MoveDistance * -1, 0, 0.0, 0.0, 0.0);
            //else if (direction == MoveDirection.Right)
            //    coordinate = this.Config.VisionCalPositions[(int)VisionCalibratorConfig.PositionVisionCal.Start].Coordinate + new XyzztCoordinate(this.Config.MoveDistance, 0, 0.0, 0.0, 0.0);

            return coordinate;
        }

        //protected virtual int PatternMatchingAfterMove(XyztCoordinate coordinate, out PatternMatchingResult result)
        //{
        //    int ret = 0;
        //    result = null;

        //    if ((ret = this.XyztStage.MovePosition(coordinate)) != 0) return ret;

        //    Thread.Sleep(1000);

        //    for (int i = 0; i < 5; i++)
        //    {
        //        result = this.Search();

        //        if (result.Values.Count > 0)
        //        {
        //            break;
        //        }
        //    }

        //    if (result.Values.Count < 1)
        //    {
        //        //Matching 실패.
        //        ret = -1;
        //    }

        //    FireUpdateResult(result);

        //    return ret;
        //}

        protected virtual int PatternMatchingAfterMove(UvwzxyzCoordinate coordinate, out PatternMatchingResult result)
        {
            int ret = 0;
            result = null;

            if ((ret = this.UvwzxyzStage.MovePosition(coordinate)) != 0) return ret;

            Thread.Sleep(1000);

            for (int i = 0; i < 5; i++)
            {
                result = this.Search();

                if (result.Values.Count > 0)
                {
                    break;
                }
            }

            if (result.Values.Count < 1)
            {
                //Matching 실패.
                ret = -1;
            }

            FireUpdateResult(result);

            return ret;
        }

        private int OnPatternMatch(out PatternMatchingResult result)
        {
            int ret = 0;
            result = GetResult();
            return ret;
        }

        public override void UpdateConfigData() //참고 : 오버라이드,, 파트 콜
        {
            WaferProbeAlign waferProbeAlign = Owner as WaferProbeAlign;
            if (waferProbeAlign != null)
            {
                Config = waferProbeAlign.Config.VisonCalibratorConfig_Upper;
                Config.Init();
                if (Recipe.IlluminationDataSet == null)
                    Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                Recipe.IlluminationDataSet.SetIlluminationChannel(waferProbeAlign.Config.ListIlluminationChannel);
            }
        }

        public override void UpdateRecipeData()
        {

        }

        [Serializable]
        public class DirectionResult
        {
            #region Field
            private PointD m_PixelLocation;
            private MoveDirection m_Direction;
            #endregion

            #region Constructor
            public DirectionResult(PointD pixelLocation, MoveDirection direction)
            {
                this.PixelLocation = pixelLocation;
                this.Direction = direction;
            }

            public DirectionResult() : this(PointD.Empty, MoveDirection.Start) { }
            #endregion

            #region Property
            public MoveDirection Direction
            {
                get { return this.m_Direction; }
                set { this.m_Direction = value; }
            }

            public PointD PixelLocation
            {
                get { return this.m_PixelLocation; }
                set { this.m_PixelLocation = value; }
            }
            #endregion
        }
        [Serializable]
        public class DirectionResultKeyedCollection : KeyedCollection<MoveDirection, DirectionResult>
        {
            protected override MoveDirection GetKeyForItem(DirectionResult item)
            {
                return item.Direction;
            }
        }
    }
}
