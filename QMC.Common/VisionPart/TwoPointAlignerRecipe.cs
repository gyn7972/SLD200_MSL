using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class TwoPointAlignerRecipe
    {
        [Serializable]
        public enum Direction
        {
            /// <summary>
            /// 반시계 방향.
            /// </summary>
            CounterClockWise,

            /// <summary>
            /// 시계 방향.
            /// </summary>
            ClockWise,
        }
        [Serializable]
        public enum PathType
        {
            Continuous,
            StepByStep,
        }

        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        public XyCoordinate ReferencePosition { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        public double UnitPitchX { set; get; }
        public double UnitPitchY { set; get; }
        public double AlignPointPitch { set; get; }
        public int DelayTimeBeforeScan { set; get; }

        public bool InvertX { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PatternMatchingParameters PatternMatchingParameter { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public IlluminationDataSet IlluminationDataSet { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PathGenerator pathGenerator { set; get; }

        public TwoPointAlignerRecipe(Part part)
        {
            UnitPitchX = 0;
            UnitPitchY = 0;
            AlignPointPitch = 0;
            DelayTimeBeforeScan = 0;
            Init(part);
        }

        public void Init(Part part)
        {
            if (ReferencePosition == null)
                ReferencePosition = new XyCoordinate();
            
            if (PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();

            if (TrainRoiStartLocation == null)
                TrainRoiStartLocation = new Point();

            if (TrainRoiEndLocation == null)
                TrainRoiEndLocation = new Point();

            if(InspectRoiStartLocation == null)
                InspectRoiStartLocation = new Point();

            if( InspectRoiEndLocation == null)
                InspectRoiEndLocation = new Point();

            if (IlluminationDataSet == null)
                IlluminationDataSet = new IlluminationDataSet(part.Name);

            if (pathGenerator == null)
                pathGenerator = new PathGenerator();
        }

        public XyCoordinate GetSecondPosition()
        {
            XyCoordinate secondPos = new XyCoordinate(ReferencePosition.X + AlignPointPitch, ReferencePosition.Y);
            return secondPos;
        }
    }

    [Serializable]
    [TypeConverter(typeof(NormalExpandableObjectConverter))]
    public class PathParameter
    {
        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        public XyCoordinate CenterCoordinate { set; get; }
        public TwoPointAlignerRecipe.Direction Direction { get; set; }
        public TwoPointAlignerRecipe.PathType PathType { get; set; }
        public SizeD PitchDistance { get; set; }
        public SizeD Area { get; set; }
        public Size PitchCount { get; set; }

        public bool InvertedX { set; get; }
        public bool InvertedY { set; get; }
        public PathParameter()
        {
            CenterCoordinate = new XyCoordinate(0, 0);
            Direction = TwoPointAlignerRecipe.Direction.ClockWise;
            PathType = TwoPointAlignerRecipe.PathType.StepByStep;
            PitchDistance = new SizeD(10, 10);
            Area = new SizeD(10, 10);
            PitchCount = new Size(10, 10);
            InvertedX = false;
            InvertedY = false;
        }
    }
    [Serializable]
    public class PathGenerator
    {
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PathParameter PathParameter { set; get; }


        public List<XyCoordinate> Paths { get; set; }
        public PathGenerator()
        {
            //     PathParameter = new PathParameter();
            Paths = new List<XyCoordinate>();
            PathParameter = new PathParameter();
        }

        protected int OnGenerate(PathParameter paramter)
        {
            return this.Run(paramter);
        }
        private int Run(PathParameter parameter)
        {//?
            int ret = 0;
            int directionX = 1;
            int directionY = 1;
            int loopCount = 0;
            List<XyCoordinate> paths = new List<XyCoordinate>(parameter.PitchCount.Width * parameter.PitchCount.Height);
            XyCoordinate currentCoordinate = new XyCoordinate(parameter.CenterCoordinate.X, parameter.CenterCoordinate.Y);
            int offsetX = 0;
            int offsetY = 0;
            paths.Add(currentCoordinate);
            #region Direction 및 시작 위치 지정
            // 시계방향이면서 N x M에서 N이 M보다 크거나 같을때
            if (parameter.Direction == TwoPointAlignerRecipe.Direction.ClockWise && parameter.PitchCount.Height <= parameter.PitchCount.Width)
            {
                // X : + , Y : +
                directionX = 1;
                directionY = 1;
                //시작위치는 왼쪽 상단.
                currentCoordinate = new XyCoordinate(currentCoordinate.X - Math.Truncate((parameter.PitchCount.Width - parameter.PitchCount.Height) / 2 * parameter.PitchDistance.Width), currentCoordinate.Y);
            }
            // 시계방향이면서 N x M에서 M이 N보다 클때
            else if (parameter.Direction == TwoPointAlignerRecipe.Direction.ClockWise && parameter.PitchCount.Width < parameter.PitchCount.Height)
            {
                // X : + , Y : -
                directionX = 1;
                directionY = -1;
                // 시작 위치는 왼쪽 하단.
                currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + Math.Truncate((parameter.PitchCount.Height - parameter.PitchCount.Width) / 2 * parameter.PitchDistance.Height));
            }
            // 반시계방향이면서 N x M에서 N이 M보다 크거나 같을때
            else if (parameter.Direction == TwoPointAlignerRecipe.Direction.CounterClockWise && parameter.PitchCount.Height <= parameter.PitchCount.Width)
            {
                // X : - , Y : +
                directionX = -1;
                directionY = 1;
                // 시작 위치는 오른쪽 상단
                currentCoordinate = new XyCoordinate(currentCoordinate.X + Math.Truncate((parameter.PitchCount.Width - parameter.PitchCount.Height) / 2 * parameter.PitchDistance.Width), currentCoordinate.Y);
            }
            // 반시계방향이면서 N x M에서 M이 N보다 클때
            else if (parameter.Direction == TwoPointAlignerRecipe.Direction.CounterClockWise && parameter.PitchCount.Width < parameter.PitchCount.Height)
            {
                // X : - , Y : -
                directionX = -1;
                directionY = -1;
                // 시작 위치는 오른쪽 하단
                currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + Math.Truncate((parameter.PitchCount.Height - parameter.PitchCount.Width) / 2 * parameter.PitchDistance.Height));
            }

            // 짝수일 경우 처리
            if (parameter.PitchCount.Width % 2 == 0)
                currentCoordinate -= new XyCoordinate(parameter.PitchDistance.Width / 2, 0);
            if (parameter.PitchCount.Height % 2 == 0)
                currentCoordinate -= new XyCoordinate(0, parameter.PitchDistance.Height / 2);

            paths.Add(currentCoordinate);
            #endregion

            #region Spiral 방식으로 Loop 시작
            // 가로의 횟수가 높을 경우.
            if (parameter.PitchCount.Height < parameter.PitchCount.Width)
            {

                loopCount = parameter.PitchCount.Height;

                for (int i = 1; i < loopCount + 1; i++)
                {
                    // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
                    if (i == loopCount && loopCount % 2 == 0) offsetX = 1;

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep)
                    {
                        // 가로의 횟수가 높기 때문에 가로부터 진행.
                        for (int j = 0; j < i - offsetX + (parameter.PitchCount.Width - (int)Math.Ceiling(parameter.PitchCount.Height / 2.0) * 2); j++)
                        {
                            currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
                            paths.Add(currentCoordinate);
                        }
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)
                    {
                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX + (parameter.PitchCount.Width - (int)Math.Round(parameter.PitchCount.Height / 2.0) * 2)), currentCoordinate.Y);
                        paths.Add(currentCoordinate);
                    }


                    // 마지막 루프에서는 height쪽으로 Path를 생성하지 않음.
                    if (i == loopCount && loopCount % 2 == 0) break;
                    if (i == loopCount && loopCount % 2 != 0) offsetY = 1;

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep)
                    {
                        for (int j = 0; j < i - offsetY; j++)
                        {
                            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
                            paths.Add(currentCoordinate);
                        }
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)
                    {
                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY));
                        paths.Add(currentCoordinate);
                    }

                    directionX *= -1;
                    directionY *= -1;
                }
            }
            // 세로의 횟수가 높을 경우.
            else if (parameter.PitchCount.Width < parameter.PitchCount.Height)
            {
                loopCount = parameter.PitchCount.Width;

                for (int i = 1; i < loopCount + 1; i++)
                {
                    // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
                    if (i == loopCount && loopCount % 2 == 0) offsetY = 1;

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep)
                    {
                        // 세로 횟수가 높기 때문에 세로부터 진행.
                        for (int j = 0; j < i - offsetY + (parameter.PitchCount.Height - (int)Math.Ceiling(parameter.PitchCount.Width / 2.0) * 2); j++)
                        {
                            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
                            paths.Add(currentCoordinate);
                        }
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)
                    {
                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY + (parameter.PitchCount.Height - (int)Math.Round(parameter.PitchCount.Width / 2.0) * 2)));
                        paths.Add(currentCoordinate);
                    }

                    // 마지막 루프에서는 Width쪽으로 Path를 생성하지 않음.
                    if (i == loopCount && loopCount % 2 == 0) break;
                    if (i == loopCount && loopCount % 2 != 0) offsetX = 1;

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep)
                    {
                        for (int j = 0; j < i - offsetX; j++)
                        {
                            currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
                            paths.Add(currentCoordinate);
                        }
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)
                    {
                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX), currentCoordinate.Y);
                        paths.Add(currentCoordinate);
                    }

                    directionX *= -1;
                    directionY *= -1;
                }
            }
            // 가로, 세로의 횟수가 같을 경우.
            else if (parameter.PitchCount.Width == parameter.PitchCount.Height)
            {
                loopCount = parameter.PitchCount.Width;

                for (int i = 1; i <= loopCount + 1; i++)
                {
                    // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
                    if (i == loopCount) offsetX = 1;

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep)
                    {
                        for (int j = 0; j < i - offsetX; j++)
                        {
                            currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
                            paths.Add(currentCoordinate);
                        }
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)
                    {
                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX), currentCoordinate.Y);
                        paths.Add(currentCoordinate);
                    }

                    // 마지막 루프에서는 height쪽으로 Path를 생성하지 않음.
                    if (i == loopCount) break;

                    if (parameter.PathType == TwoPointAlignerRecipe.PathType.StepByStep)
                    {
                        for (int j = 0; j < i - offsetY; j++)
                        {
                            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
                            paths.Add(currentCoordinate);
                        }
                    }
                    else if (parameter.PathType == TwoPointAlignerRecipe.PathType.Continuous)
                    {
                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY));
                        paths.Add(currentCoordinate);
                    }

                    directionX *= -1;
                    directionY *= -1;
                }
            }
            #endregion

            this.Paths = paths;

            return ret;
        }


        public PathParameter GetParameter()
        {
            return (PathParameter)CopyUtility.GetDeepCopy(PathParameter);
        }


        public int Generate(PathParameter paramter)
        {
            int ret = 0;

            this.PathParameter = paramter;

            if ((ret = this.OnGenerate(paramter)) != 0) return ret;

            return ret;
        }
    }
    public class PathGeneratorCollection : Collection<PathGenerator>
    {

    }
    public class PathParameterCollection : Collection<PathParameter>
    {

    }
}
