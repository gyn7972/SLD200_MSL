//using QMC.Common.PathGenerators;
//using System;
//using System.Collections.Generic;
//using System.Drawing;

//namespace QMC.Common.PathGenerators
//{
//    #region SpiralTwoDimensionPathGenerator
//    public class SpiralTwoDimensionPathGenerator : TwoDimensionPathGenerator
//    {
//        #region Define
//        [Serializable]
//        public enum Direction
//        {
//            /// <summary>
//            /// 반시계 방향.
//            /// </summary>
//            CounterClockWise,

//            /// <summary>
//            /// 시계 방향.
//            /// </summary>
//            ClockWise,
//        }
//        #endregion

//        #region Constructor
//        public SpiralTwoDimensionPathGenerator(string name) : base(name)
//        {
//            this.Parameter = new SpiralTwoDimensionPathGeneratorParameter();
//        }
//        public SpiralTwoDimensionPathGenerator() : this("") { }
//        #endregion

//        #region Method
//        private int Run(SpiralTwoDimensionPathGeneratorParameter parameter)
//        {//?
//            int ret = 0;
//            int directionX = 1;
//            int directionY = 1;
//            int loopCount = 0;
//            List<XyCoordinate> paths = new List<XyCoordinate>(parameter.PitchCount.Width * parameter.PitchCount.Height);
//            XyCoordinate currentCoordinate = new XyCoordinate(parameter.CenterCoordinate.X, parameter.CenterCoordinate.Y);
//            int offsetX = 0;
//            int offsetY = 0;

//            #region Direction 및 시작 위치 지정
//            // 시계방향이면서 N x M에서 N이 M보다 크거나 같을때
//            if (parameter.Direction == Direction.ClockWise && parameter.PitchCount.Height <= parameter.PitchCount.Width)
//            {
//                // X : + , Y : +
//                directionX = 1;
//                directionY = 1;
//                //시작위치는 왼쪽 상단.
//                currentCoordinate = new XyCoordinate(currentCoordinate.X - Math.Truncate((parameter.PitchCount.Width - parameter.PitchCount.Height) / 2 * parameter.PitchDistance.Width), currentCoordinate.Y);
//            }
//            // 시계방향이면서 N x M에서 M이 N보다 클때
//            else if (parameter.Direction == Direction.ClockWise && parameter.PitchCount.Width < parameter.PitchCount.Height)
//            {
//                // X : + , Y : -
//                directionX = 1;
//                directionY = -1;
//                // 시작 위치는 왼쪽 하단.
//                currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + Math.Truncate((parameter.PitchCount.Height - parameter.PitchCount.Width) / 2 * parameter.PitchDistance.Height));
//            }
//            // 반시계방향이면서 N x M에서 N이 M보다 크거나 같을때
//            else if (parameter.Direction == Direction.CounterClockWise && parameter.PitchCount.Height <= parameter.PitchCount.Width)
//            {
//                // X : - , Y : +
//                directionX = -1;
//                directionY = 1;
//                // 시작 위치는 오른쪽 상단
//                currentCoordinate = new XyCoordinate(currentCoordinate.X + Math.Truncate((parameter.PitchCount.Width - parameter.PitchCount.Height) / 2 * parameter.PitchDistance.Width), currentCoordinate.Y);
//            }
//            // 반시계방향이면서 N x M에서 M이 N보다 클때
//            else if (parameter.Direction == Direction.CounterClockWise && parameter.PitchCount.Width < parameter.PitchCount.Height)
//            {
//                // X : - , Y : -
//                directionX = -1;
//                directionY = -1;
//                // 시작 위치는 오른쪽 하단
//                currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + Math.Truncate((parameter.PitchCount.Height - parameter.PitchCount.Width) / 2 * parameter.PitchDistance.Height));
//            }

//            // 짝수일 경우 처리
//            if (parameter.PitchCount.Width % 2 == 0)
//                currentCoordinate -= new XyCoordinate(parameter.PitchDistance.Width / 2, 0);
//            if (parameter.PitchCount.Height % 2 == 0)
//                currentCoordinate -= new XyCoordinate(0, parameter.PitchDistance.Height / 2);

//            paths.Add(currentCoordinate);
//            #endregion

//            #region Spiral 방식으로 Loop 시작
//            // 가로의 횟수가 높을 경우.
//            if (parameter.PitchCount.Height < parameter.PitchCount.Width)
//            {

//                loopCount = parameter.PitchCount.Height;

//                for (int i = 1; i < loopCount + 1; i++)
//                {
//                    // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
//                    if (i == loopCount && loopCount % 2 == 0) offsetX = 1;

//                    if (parameter.PathType == PathType.StepByStep)
//                    {
//                        // 가로의 횟수가 높기 때문에 가로부터 진행.
//                        for (int j = 0; j < i - offsetX + (parameter.PitchCount.Width - (int)Math.Ceiling(parameter.PitchCount.Height / 2.0) * 2); j++)
//                        {
//                            currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
//                            paths.Add(currentCoordinate);
//                        }
//                    }
//                    else if (parameter.PathType == PathType.Continuous)
//                    {
//                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX + (parameter.PitchCount.Width - (int)Math.Round(parameter.PitchCount.Height / 2.0) * 2)), currentCoordinate.Y);
//                        paths.Add(currentCoordinate);
//                    }

//                    // 마지막 루프에서는 height쪽으로 Path를 생성하지 않음.
//                    if (i == loopCount && loopCount % 2 == 0) break;
//                    if (i == loopCount && loopCount % 2 != 0) offsetY = 1;

//                    if (parameter.PathType == PathType.StepByStep)
//                    {
//                        for (int j = 0; j < i - offsetY; j++)
//                        {
//                            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
//                            paths.Add(currentCoordinate);
//                        }
//                    }
//                    else if (parameter.PathType == PathType.Continuous)
//                    {
//                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY));
//                        paths.Add(currentCoordinate);
//                    }

//                    directionX *= -1;
//                    directionY *= -1;
//                }
//            }
//            // 세로의 횟수가 높을 경우.
//            else if (parameter.PitchCount.Width < parameter.PitchCount.Height)
//            {
//                loopCount = parameter.PitchCount.Width;

//                for (int i = 1; i < loopCount + 1; i++)
//                {
//                    // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
//                    if (i == loopCount && loopCount % 2 == 0) offsetY = 1;

//                    if (parameter.PathType == PathType.StepByStep)
//                    {
//                        // 세로 횟수가 높기 때문에 세로부터 진행.
//                        for (int j = 0; j < i - offsetY + (parameter.PitchCount.Height - (int)Math.Ceiling(parameter.PitchCount.Width / 2.0) * 2); j++)
//                        {
//                            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
//                            paths.Add(currentCoordinate);
//                        }
//                    }
//                    else if (parameter.PathType == PathType.Continuous)
//                    {
//                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY + (parameter.PitchCount.Height - (int)Math.Round(parameter.PitchCount.Width / 2.0) * 2)));
//                        paths.Add(currentCoordinate);
//                    }

//                    // 마지막 루프에서는 Width쪽으로 Path를 생성하지 않음.
//                    if (i == loopCount && loopCount % 2 == 0) break;
//                    if (i == loopCount && loopCount % 2 != 0) offsetX = 1;

//                    if (parameter.PathType == PathType.StepByStep)
//                    {
//                        for (int j = 0; j < i - offsetX; j++)
//                        {
//                            currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
//                            paths.Add(currentCoordinate);
//                        }
//                    }
//                    else if (parameter.PathType == PathType.Continuous)
//                    {
//                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX), currentCoordinate.Y);
//                        paths.Add(currentCoordinate);
//                    }

//                    directionX *= -1;
//                    directionY *= -1;
//                }
//            }
//            // 가로, 세로의 횟수가 같을 경우.
//            else if (parameter.PitchCount.Width == parameter.PitchCount.Height)
//            {
//                loopCount = parameter.PitchCount.Width;

//                for (int i = 1; i <= loopCount + 1; i++)
//                {
//                    // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
//                    if (i == loopCount) offsetX = 1;

//                    if (parameter.PathType == PathType.StepByStep)
//                    {
//                        for (int j = 0; j < i - offsetX; j++)
//                        {
//                            currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
//                            paths.Add(currentCoordinate);
//                        }
//                    }
//                    else if (parameter.PathType == PathType.Continuous)
//                    {
//                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX), currentCoordinate.Y);
//                        paths.Add(currentCoordinate);
//                    }

//                    // 마지막 루프에서는 height쪽으로 Path를 생성하지 않음.
//                    if (i == loopCount) break;

//                    if (parameter.PathType == PathType.StepByStep)
//                    {
//                        for (int j = 0; j < i - offsetY; j++)
//                        {
//                            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
//                            paths.Add(currentCoordinate);
//                        }
//                    }
//                    else if (parameter.PathType == PathType.Continuous)
//                    {
//                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY));
//                        paths.Add(currentCoordinate);
//                    }

//                    directionX *= -1;
//                    directionY *= -1;
//                }
//            }
//            #endregion

//            this.Paths = new TwoDimensionPathReadOnlyCollection(paths);

//            return ret;
//        }
//        #endregion

//        #region PathGenerator Members
//        protected override int OnGenerate(PathGeneratorParameter paramter)
//        {
//            return this.Run(paramter as SpiralTwoDimensionPathGeneratorParameter);
//        }

//        protected new SpiralTwoDimensionPathGeneratorParameter Parameter
//        {
//            get { return base.Parameter as SpiralTwoDimensionPathGeneratorParameter; }
//            set { base.Parameter = value; }
//        }
//        #endregion
//    }
//    #endregion

//    #region SpiralTwoDimensionPathGeneratorParameter
//    [Serializable]
//    public class SpiralTwoDimensionPathGeneratorParameter : TwoDimensionPathGeneratorParameter
//    {
//        #region Field
//        private SpiralTwoDimensionPathGenerator.Direction m_Direction;
//        private SizeD m_PitchDistance;
//        private SizeD m_Area;
//        private Size m_PitchCount;
//        #endregion

//        #region Constructor
//        public SpiralTwoDimensionPathGeneratorParameter() : base()
//        {
//            this.Direction = SpiralTwoDimensionPathGenerator.Direction.ClockWise;
//            this.CenterCoordinate = new XyCoordinate();
//            this.PitchDistance = new SizeD(10, 10);
//            this.PitchCount = new Size(10, 10);
//        }
//        #endregion

//        #region Property
//        /// <summary>
//        /// 스파이럴의 방향을 가져오거나 설정한다.
//        /// </summary>
//        public SpiralTwoDimensionPathGenerator.Direction Direction
//        {
//            get { return this.m_Direction; }
//            set { this.m_Direction = value; }
//        }

//        /// <summary>
//        /// 다음 경로까지의 이동 거리를 가져오거나 설정한다.
//        /// </summary>
//        public SizeD PitchDistance
//        {
//            get { return this.m_PitchDistance; }
//            set
//            {
//                if (this.m_PitchDistance == value) return;
//                this.m_PitchDistance = value;

//                this.m_Area = new SizeD(value.Width * this.m_PitchCount.Width, value.Height * this.m_PitchCount.Height);
//            }
//        }

//        /// <summary>
//        /// 생성될 경로의 면적을 가져오거나 설정한다.
//        /// </summary>
//        public SizeD Area
//        {
//            get { return this.m_Area; }
//            set
//            {
//                if (this.Area == value) return;
//                this.m_Area = value;

//                this.m_PitchDistance = new SizeD(value.Width / this.m_PitchCount.Width, value.Height / this.m_PitchCount.Height);
//            }
//        }

//        /// <summary>
//        /// 생성될 경로의 가로, 세로의 횟수를 가져오거나 설정한다.
//        /// </summary>
//        public Size PitchCount
//        {
//            get { return this.m_PitchCount; }
//            set
//            {
//                if (this.m_PitchCount == value) return;
//                this.m_PitchCount = value;

//                this.m_Area = new SizeD(this.m_PitchDistance.Width * value.Width, this.m_PitchDistance.Height * value.Height);
//            }
//        }
//        #endregion

//        #region PathGeneratorParameter Members
//        protected override Type OnGetPathGeneratorType()
//        {
//            return typeof(SpiralTwoDimensionPathGenerator);
//        }

//        public int CreatePathGenerator(string name, out SpiralTwoDimensionPathGenerator generator)
//        {
//            int ret = 0;

//            generator = Activator.CreateInstance(this.OnGetPathGeneratorType(), name) as SpiralTwoDimensionPathGenerator;

//            if ((ret = generator.Generate(this)) != 0) return ret;

//            return ret;
//        }

//        public int CreatePathGenerator(out SpiralTwoDimensionPathGenerator generator)
//        {
//            return this.CreatePathGenerator(PathGeneratorParameter.DefaultName, out generator);
//        }

//        //protected override PathGeneratorControl OnGetControl(PathGeneratorParameter parameter)
//        //{
//        //    return new SpiralTwoDimensionPathGeneratorControl(parameter as SpiralTwoDimensionPathGeneratorParameter);
//        //}
//        #endregion
//    }
//    #endregion
//}
