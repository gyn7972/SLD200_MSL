using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace QMC.Common.PathGenerators
{
    #region TwoDimensionPathGenerator
    public abstract class TwoDimensionPathGenerator : PathGenerator
    {
        #region Field
        private PointCollection m_PointAddressCollection;
        #endregion

        #region Consturctor
        public TwoDimensionPathGenerator(string name) : base(name)
        {
            this.PointAddressCollection = new PointCollection();
        }
        public TwoDimensionPathGenerator() : this("") { }
        #endregion

        #region Property
        public PointCollection PointAddressCollection
        {
            get { return this.m_PointAddressCollection; }
            protected set { this.m_PointAddressCollection = value; }
        }
        #endregion

        #region PathGenerator Members
        public class TwoDimensionPathReadOnlyCollection : PathReadOnlyCollection<XyCoordinate>
        {
            private Queue<int> listMissIndex = new Queue<int>();
            private int m_nLastIndex = 0;
            #region Constructor
            public TwoDimensionPathReadOnlyCollection(IList<XyCoordinate> list) : base(list)
            {
            }
            public TwoDimensionPathReadOnlyCollection(XyCoordinate position) : this(new XyCoordinate[] { position }) { }
            public TwoDimensionPathReadOnlyCollection() : this(new XyCoordinate[0]) { }

            public override XyCoordinate MoveNext()
            {
                if (listMissIndex.Count > 0)
                {
                    int nNextIndex = listMissIndex.Dequeue();

                    if (nNextIndex >= 0 && nNextIndex < Count)
                    {
                        m_nLastIndex = CurrentPathIndex;
                        if (nNextIndex < CurrentPathIndex)
                        {
                            while (nNextIndex < CurrentPathIndex)
                            {
                                base.MovePrevious();
                            }
                        }
                        return this[nNextIndex];
                    }
                }
                if (m_nLastIndex > CurrentPathIndex)
                {
                    while (m_nLastIndex > CurrentPathIndex)
                    {
                        base.MoveNext();
                    }
                }

                return base.MoveNext();
            }
            public void Missing(int nIndex)
            {
                if (nIndex >= 0 && nIndex < Count)
                {
                    Queue<int> que = (Queue<int>)listMissIndex.Select(t => t.Equals(nIndex));
                    if (que.Count == 0)
                        listMissIndex.Enqueue(nIndex);
                }
            }

            #endregion
        }

        /// <summary>
        /// Path 생성시 사용할 Paramter를 가져온다.
        /// </summary>
        public new TwoDimensionPathGeneratorParameter GetParameter()
        {
            return CopyUtility.GetDeepCopy(this.Parameter) as TwoDimensionPathGeneratorParameter;
        }



        /// <summary>
        /// 생성된 Path들을 가져온다.
        /// </summary>
        public new TwoDimensionPathReadOnlyCollection Paths
        {
            get { return base.Paths as TwoDimensionPathReadOnlyCollection; }
            protected set { base.Paths = value; }
        }

        protected new TwoDimensionPathGeneratorParameter Parameter
        {
            get { return base.Parameter as TwoDimensionPathGeneratorParameter; }
            set { base.Parameter = value; }
        }
        #endregion
    }
    #endregion

    #region TwoDimensionPathGeneratorParameter
    [Serializable]
    public abstract class TwoDimensionPathGeneratorParameter : PathGeneratorParameter
    {
        #region Field
        private XyCoordinate m_CenterCoordinate;
        #endregion

        #region Constructor
        public TwoDimensionPathGeneratorParameter() : base()
        {
            this.CenterCoordinate = new XyCoordinate();
        }
        #endregion

        #region Property
        public XyCoordinate CenterCoordinate
        {
            get { return this.m_CenterCoordinate; }
            set { this.m_CenterCoordinate = value; }
        }
        #endregion

        #region PathGeneratorParameter Members
        public int CreatePathGenerator(string name, out TwoDimensionPathGenerator generator)
        {
            int ret = 0;

            generator = Activator.CreateInstance(this.OnGetPathGeneratorType(), name) as TwoDimensionPathGenerator;
            // To Do: parameter가 완료되지 않은 상태에서 호출하면 generate를 할 수 없어서 실패를 리턴한다.
            generator.Generate(this);

            return ret;
        }

        public int CreatePathGenerator(out TwoDimensionPathGenerator generator)
        {
            return this.CreatePathGenerator(PathGeneratorParameter.DefaultName, out generator);
        }
        #endregion

        #region Object Members
        public override string ToString()
        {
            StringWriter sw = new StringWriter();

            sw.Write(base.ToString());
            sw.WriteLine("\tCenterCoordinate = {0}", this.CenterCoordinate);

            return sw.ToString();
        }
        #endregion
    }
    #endregion
    public class ZigzagTwoDimensionPathGenerator : TwoDimensionPathGenerator
    {
        #region Define
        private const int DecimalPoint = 7;

        [Serializable]
        public enum Direction
        {
            /// <summary>
            /// 가로 방향.
            /// </summary> 
            Horizontal,

            /// <summary>
            /// 세로 방향.
            /// </summary>
            Vertical,
        }

        [Serializable]
        public enum StartLocation
        {
            /// <summary>
            /// 좌측 상단
            /// </summary> 
            LeftTop,

            /// <summary>
            /// 좌측 하단
            /// </summary>
            LeftBottom,

            /// <summary>
            /// 우측 상단
            /// </summary> 
            RightTop,

            /// <summary>
            /// 우측 하단
            /// </summary>
            RightBottom,
        }

        [Serializable]
        public enum StartAddressingPoint
        {
            /// <summary>
            /// 좌측 상단
            /// </summary> 
            LeftTop,

            /// <summary>
            /// 좌측 하단
            /// </summary>
            LeftBottom,

            /// <summary>
            /// 우측 상단
            /// </summary> 
            RightTop,

            /// <summary>
            /// 우측 하단
            /// </summary>
            RightBottom,
        }

        [Serializable]
        public enum TargetShape
        {
            Rectangle,
            Point,
        }

        [Serializable]
        public enum SetPositionType
        {
            Center,
            Start,
        }

        [Serializable]
        public enum AreaCalculationParameterTypes
        {
            Area,
            PitchCount,
        }

        [Serializable]
        public class LineResult
        {
            #region Field
            private XyCoordinate m_StartCoordinate;
            private XyCoordinate m_EndCoordinate;
            private XyCoordinateCollection m_LineCoordinates;
            #endregion

            #region Constructor
            public LineResult()
            {
                this.StartCoordinate = new XyCoordinate(0, 0);
                this.EndCoordinate = new XyCoordinate(0, 0);
                this.LineCoordinates = new XyCoordinateCollection();
            }
            #endregion

            #region Property
            public XyCoordinate StartCoordinate
            {
                get { return this.m_StartCoordinate; }
                set { this.m_StartCoordinate = value; }
            }

            public XyCoordinate EndCoordinate
            {
                get { return this.m_EndCoordinate; }
                set { this.m_EndCoordinate = value; }
            }

            public XyCoordinateCollection LineCoordinates
            {
                get { return this.m_LineCoordinates; }
                set { this.m_LineCoordinates = value; }
            }
            #endregion
        }

        [Serializable]
        public class LineResultCollection : Collection<LineResult>
        {
            public LineResultCollection()
            {
            }
        }
        #endregion

        #region Field
        private LineResultCollection m_Result;
        #endregion

        #region Constructor
        public ZigzagTwoDimensionPathGenerator(string name) : base(name)
        {
            this.Parameter = new RectangleZigzagTwoDimensionPathGeneratorParameter();
        }
        public ZigzagTwoDimensionPathGenerator() : this("") { }
        #endregion

        #region Property
        public LineResultCollection Result
        {
            get { return this.m_Result; }
            private set { this.m_Result = value; }
        }
        #endregion

        #region Method
        //private int Run(CircleZigzagTwoDimensionPathGeneratorParameter parameter)
        //{
        //    int ret = 0;
        //    int directionX = 1;
        //    int directionY = 1;
        //    int switchDirection = -1;
        //    List<XyCoordinate> paths = new List<XyCoordinate>(parameter.PitchCount.Width * parameter.PitchCount.Height);
        //    XyCoordinate currentCoordinate = new XyCoordinate(parameter.CenterCoordinate.X, parameter.CenterCoordinate.Y);

        //    #region Direction 지정
        //    // +1 : Plus 방향, -1 : Minus 방향
        //    // 시작 위치 : 좌측 상단
        //    if (parameter.StartLocation == StartLocation.LeftTop)
        //    {
        //        directionX = 1;
        //        directionY = -1;
        //    }
        //    // 시작 위치 : 좌측 하단
        //    else if (parameter.StartLocation == StartLocation.LeftBottom)
        //    {
        //        directionX = 1;
        //        directionY = 1;
        //    }
        //    // 시작 위치 : 우측 상단
        //    else if (parameter.StartLocation == StartLocation.RightTop)
        //    {
        //        directionX = -1;
        //        directionY = -1;
        //    }
        //    // 시작 위치 : 우측 하단
        //    else if (parameter.StartLocation == StartLocation.RightBottom)
        //    {
        //        directionX = -1;
        //        directionY = 1;
        //    }
        //    #endregion

        //    #region 시작 위치 지정
        //    // Horizontal
        //    if (parameter.Direction == Direction.Horizontal)
        //    {
        //        // 가운데 정렬 : 마지막 Pitch에서 너무 적은 범위를 Image Grab하는 것을 막기 위함 
        //        //if (parameter.Radius * 2 % parameter.PitchDistance.Height != 0)
        //        //{
        //        //    currentCoordinate.Y += -directionY * ((parameter.PitchCount.Height * parameter.PitchDistance.Height - parameter.Radius * 2) / 2);
        //        //}
        //        // 원점에서 반지름만큼 이동 후 ObjectSize.Height만큼 반대로 이동
        //        currentCoordinate.Y += directionY * (-parameter.Radius + parameter.ObjectSize.Height / 2);
        //        currentCoordinate = parameter.GetStartingPoint(currentCoordinate.X, currentCoordinate.Y, directionX, directionY, 0);
        //    }
        //    // Vertical
        //    else
        //    {
        //        //if (parameter.Radius * 2 % parameter.PitchDistance.Width != 0)
        //        //{
        //        //    currentCoordinate.X += -directionX * (parameter.PitchCount.Width * parameter.PitchDistance.Width - parameter.Radius * 2) / 2;
        //        //}
        //        // 원점에서 반지름만큼 이동 후 ObjectSize.Width만큼 반대로 이동
        //        currentCoordinate.X += directionX * (-parameter.Radius + parameter.ObjectSize.Width / 2);
        //        currentCoordinate = parameter.GetStartingPoint(currentCoordinate.X, currentCoordinate.Y, directionX, directionY, 0);
        //    }
        //    currentCoordinate.X = Math.Round(currentCoordinate.X, DecimalPoint);
        //    currentCoordinate.Y = Math.Round(currentCoordinate.Y, DecimalPoint);
        //    paths.Add(currentCoordinate);
        //    #endregion

        //    #region Zigzag 방식으로 Loop 시작
        //    // 가로 방향 진행
        //    if (parameter.Direction == Direction.Horizontal)
        //    {
        //        for (int i = 0; i < parameter.PitchCount.Height; i++)
        //        {
        //            if (parameter.PathType == PathType.StepByStep)
        //            {
        //                for (int j = 1; j < parameter.PitchCount.Width; j++)
        //                {
        //                    currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
        //                    paths.Add(currentCoordinate);
        //                }
        //            }
        //            else if (parameter.PathType == PathType.Continuous)
        //            {
        //                currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * parameter.PitchCount.Width, currentCoordinate.Y);
        //                paths.Add(currentCoordinate);
        //            }

        //            if (i == parameter.PitchCount.Height - 1) break;
        //            // 한 줄 상/하로 이동
        //            currentCoordinate.Y += parameter.PitchDistance.Height * directionY;
        //            currentCoordinate = parameter.GetStartingPoint(currentCoordinate.X, currentCoordinate.Y, directionX * switchDirection, directionY, i + 1);
        //            paths.Add(currentCoordinate);
        //            directionX *= switchDirection;
        //        }
        //    }
        //    // 세로 방향 진행
        //    else if (parameter.Direction == Direction.Vertical)
        //    {
        //        for (int i = 0; i < parameter.PitchCount.Width; i++)
        //        {
        //            if (parameter.PathType == PathType.StepByStep)
        //            {
        //                for (int j = 1; j < parameter.PitchCount.Height; j++)
        //                {
        //                    currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
        //                    paths.Add(currentCoordinate);
        //                }
        //            }
        //            else if (parameter.PathType == PathType.Continuous)
        //            {
        //                currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * parameter.PitchCount.Height);
        //                paths.Add(currentCoordinate);
        //            }

        //            if (i == parameter.PitchCount.Width - 1) break;
        //            // 한 줄 좌/우로 이동
        //            currentCoordinate.X += parameter.PitchDistance.Width * directionX;
        //            currentCoordinate = parameter.GetStartingPoint(currentCoordinate.X, currentCoordinate.Y, directionX, directionY * switchDirection, i + 1);
        //            paths.Add(currentCoordinate);
        //            directionY *= switchDirection;
        //        }
        //    }
        //    #endregion

        //    this.Paths = new TwoDimensionPathReadOnlyCollection(paths);
        //    return ret;
        //}

        private int Run(RectangleZigzagTwoDimensionPathGeneratorParameter parameter)
        {
            //Direction
            //Sequence
            //LeftTop
            //→
            //↓↑↓↑
            //LeftBottom
            //→
            //↑↓↑↓
            //RightTop
            //←
            //↓↑↓↑
            //RightBottom
            //←
            //↑↓↑↓
            int ret = 0;
            int maxValue = 0;
            CycleTimer timer = new CycleTimer();
            List<Point> points = new List<Point>();
            PointCollection tempPoints = new PointCollection();
            timer.Start();
            if (parameter.Direction == Direction.Horizontal)
                maxValue = parameter.PitchCount.Height;
            else
                maxValue = parameter.PitchCount.Width;

            #region 주석
            //if (parameter.StartLocation == StartLocation.LeftTop || parameter.StartLocation == StartLocation.LeftBottom)
            //{
            //    for (int i = 0; i < maxValue; i++)
            //    {
            //        points.AddRange(this.GetIndex(parameter, i, this.CheckReverse(parameter.StartLocation, i)));
            //    }
            //}
            //else if (parameter.StartLocation == StartLocation.RightTop || parameter.StartLocation == StartLocation.RightBottom)
            //{
            //    for (int i = maxValue - 1; -1 < i; i--)
            //    {
            //        points.AddRange(this.GetIndex(parameter, i, this.CheckReverse(parameter.StartLocation, i)));
            //    }
            //}
            #endregion

            //if (parameter.StartLocation == StartLocation.LeftTop || parameter.StartLocation == StartLocation.LeftBottom)
            //{
            //    for (int i = 0; i < maxValue; i++)
            //        points.AddRange(this.GetIndex(parameter, i, this.CheckReverse(parameter.StartLocation, i)));
            //}
            //if (parameter.StartLocation == StartLocation.RightTop || parameter.StartLocation == StartLocation.RightBottom)
            //{
            //    for (int i = maxValue - 1; -1 < i; i--)
            //        points.AddRange(this.GetIndex(parameter, i, this.CheckReverse(parameter.StartLocation, i)));
            //}

            for (int i = 0; i < maxValue; i++)
                points.AddRange(this.GetIndex(parameter, i, this.CheckReverse(parameter.StartLocation, i)));

            this.PointAddressCollection = new PointCollection(points);
            this.Paths = new TwoDimensionPathReadOnlyCollection(points.Select(t => this.ConvertToPosition(parameter, t)).ToArray());
            timer.End();

            return ret;
        }

        private int PathFileSave(string path, CycleTimer timer)
        {
            int ret = 0;
            StringBuilder builder = new StringBuilder();
            if (this.PointAddressCollection == null)
                this.PointAddressCollection = new PointCollection();
            if (this.Paths == null)
                this.Paths = new TwoDimensionPathReadOnlyCollection(new XyCoordinate[] { });

            if (this.PointAddressCollection.Count != this.Paths.Count)
                //return ErrorManager.Register("PathCount and IndexCount are different.");

            if (timer != null)
                builder.AppendLine(string.Format("BuildTime: {0}ms, TotalCount: {1}ea", timer.Latest.Interval.TotalMilliseconds, this.PointAddressCollection.Count));

            builder.AppendLine("Index_X,Index_Y,Position_X,Position_Y");
            for (int i = 0; i < this.PointAddressCollection.Count; i++)
                builder.AppendLine(string.Format("{0},{1},{2},{3}", this.PointAddressCollection[i].X, this.PointAddressCollection[i].Y, this.Paths[i].X, this.Paths[i].Y));


            try
            {
                File.WriteAllText(path, builder.ToString());
            }
            catch (Exception ex)
            {
                //ret = ErrorManager.Register(ex.Message);
                ret = -1;
            }

            return ret;
        }

        private bool CheckReverse(ZigzagTwoDimensionPathGenerator.StartLocation startLocaion, int index)
        {
            bool reverse = false;
            //if(startLocaion == StartLocation.LeftTop || startLocaion == StartLocation.RightTop)
            //{
            //    if ((index + 1) % 2 == 0)
            //        reverse = true;
            //    else
            //        reverse = false;
            //}
            //else if (startLocaion == StartLocation.LeftBottom || startLocaion == StartLocation.RightBottom)
            //{
            //    if ((index + 1) % 2 == 0)
            //        reverse = false;
            //    else
            //        reverse = true;
            //}

            if ((index + 1) % 2 == 0)
                reverse = true;
            else
                reverse = false;

            return reverse;
        }

        private PointCollection GetIndex(RectangleZigzagTwoDimensionPathGeneratorParameter parameter, int index, bool reverse)
        {
            PointCollection points = new PointCollection();

            if (parameter.Direction == Direction.Horizontal)
            {
                if (reverse == true)
                {
                    if (parameter.PathType == PathType.StepByStep)
                    {
                        for (int i = parameter.PitchCount.Width - 1; -1 < i; i--)
                            points.Add(new Point(i, index));
                    }
                    else
                        return new PointCollection(new Point[] { new Point(parameter.PitchCount.Width - 1, index) });
                }
                else
                {
                    if (parameter.PathType == PathType.StepByStep)
                    {
                        for (int i = 0; i < parameter.PitchCount.Width; i++)
                            points.Add(new Point(i, index));
                    }
                    else
                        return new PointCollection(new Point[] { new Point(0, index) });
                }
            }
            else
            {
                if (reverse == true)
                {
                    if (parameter.PathType == PathType.StepByStep)
                    {
                        for (int i = parameter.PitchCount.Height - 1; -1 < i; i--)
                            points.Add(new Point(index, i));
                    }
                    else
                        return new PointCollection(new Point[] { new Point(index, parameter.PitchCount.Height - 1) });
                }
                else
                {
                    if (parameter.PathType == PathType.StepByStep)
                    {
                        for (int i = 0; i < parameter.PitchCount.Height; i++)
                            points.Add(new Point(index, i));
                    }
                    else
                        return new PointCollection(new Point[] { new Point(index, 0) });
                }
            }

            return points;
        }

        protected virtual XyCoordinate ConvertToPosition(RectangleZigzagTwoDimensionPathGeneratorParameter parameter, Point point)
        {
            double x = 0.0, y = 0.0, xOffset = 0.0, yOffset = 0.0;
            xOffset = parameter.PitchDistance.Width * point.X;
            yOffset = parameter.PitchDistance.Height * point.Y;

            //if (parameter.StartLocation == StartLocation.LeftTop || parameter.StartLocation == StartLocation.LeftBottom)
            //{
            //    xOffset = xOffset * 1;
            //    yOffset = yOffset * -1;
            //}
            //else if (parameter.StartLocation == StartLocation.RightTop || parameter.StartLocation == StartLocation.RightBottom)
            //{
            //    xOffset = xOffset * 1;
            //    yOffset = yOffset * -1;
            //}
            if (parameter.StartLocation == StartLocation.LeftTop)
            {
                xOffset = xOffset * 1;
                yOffset = yOffset * -1;
            }
            else if (parameter.StartLocation == StartLocation.LeftBottom)
            {
                xOffset = xOffset * 1;
                yOffset = yOffset * 1;
            }
            else if (parameter.StartLocation == StartLocation.RightTop)
            {
                xOffset = xOffset * -1;
                yOffset = yOffset * -1;
            }
            else if (parameter.StartLocation == StartLocation.RightBottom)
            {
                xOffset = xOffset * -1;
                yOffset = yOffset * 1;
            }

            x = parameter.StartCoordinate.X + xOffset;
            y = parameter.StartCoordinate.Y + yOffset;

            return new XyCoordinate(x, y);
        }

        //private int Run(PolygonZigzagTwoDimensionPathGeneratorParameter parameter)
        //{
        //    int ret = 0;

        //    int directionX = 1;
        //    int directionY = 1;
        //    int switchDirection = -1;
        //    XyCoordinate currentCoordinate = new XyCoordinate(0, 0);
        //    XyCoordinateCollection paths = new XyCoordinateCollection();
        //    XyCoordinateCollection bothCoordinates = new XyCoordinateCollection();
        //    LineResult lineResult = null;
        //    RangeD rangeX = new RangeD();
        //    RangeD rangeY = new RangeD();
        //    int pitchCountWidth = 0;
        //    int pitchCountHeight = 0;

        //    #region PitchCount 및 Area 설정
        //    parameter.GetMinMaxValue(parameter.Coordinates, out rangeX, out rangeY);
        //    parameter.RangeX = rangeX;
        //    parameter.RangeY = rangeY;
        //    pitchCountWidth = (int)Math.Truncate((parameter.RangeX.Maximum - parameter.RangeX.Minimum) / parameter.PitchDistance.Width + 1);
        //    pitchCountHeight = (int)Math.Truncate((parameter.RangeY.Maximum - parameter.RangeY.Minimum) / parameter.PitchDistance.Height + 1);
        //    parameter.PitchCount = new Size(pitchCountWidth, pitchCountHeight);
        //    #endregion

        //    #region Direction 지정 및 시작 위치 지정
        //    // +1 : Plus 방향, -1 : Minus 방향
        //    // 시작 위치 : 좌측 상단
        //    if (parameter.StartLocation == StartLocation.LeftTop)
        //    {
        //        directionX = 1;
        //        directionY = -1;
        //        currentCoordinate = new XyCoordinate(parameter.RangeX.Minimum, parameter.RangeY.Maximum);
        //    }
        //    // 시작 위치 : 좌측 하단
        //    else if (parameter.StartLocation == StartLocation.LeftBottom)
        //    {
        //        directionX = 1;
        //        directionY = 1;
        //        currentCoordinate = new XyCoordinate(parameter.RangeX.Minimum, parameter.RangeY.Minimum);
        //    }
        //    // 시작 위치 : 우측 상단
        //    else if (parameter.StartLocation == StartLocation.RightTop)
        //    {
        //        directionX = -1;
        //        directionY = -1;
        //        currentCoordinate = new XyCoordinate(parameter.RangeX.Maximum, parameter.RangeY.Maximum);
        //    }
        //    // 시작 위치 : 우측 하단
        //    else if (parameter.StartLocation == StartLocation.RightBottom)
        //    {
        //        directionX = -1;
        //        directionY = 1;
        //        currentCoordinate = new XyCoordinate(parameter.RangeX.Maximum, parameter.RangeY.Minimum);
        //    }

        //    currentCoordinate.X = Math.Round(currentCoordinate.X, DecimalPoint);
        //    currentCoordinate.Y = Math.Round(currentCoordinate.Y, DecimalPoint);
        //    #endregion

        //    #region Loop 시작
        //    // Horizontal
        //    if (parameter.Direction == Direction.Horizontal)
        //    {
        //        currentCoordinate.Y += directionY * parameter.ObjectSize.Height / 2;
        //        currentCoordinate.Y = Math.Round(currentCoordinate.Y, DecimalPoint);

        //        for (int i = 0; i < parameter.PitchCount.Height; i++)
        //        {
        //            lineResult = new LineResult();
        //            if (parameter.GetCoordinates(currentCoordinate.Y, directionX, out bothCoordinates) != 0) continue;

        //            lineResult.StartCoordinate = new XyCoordinate(bothCoordinates[0].X, bothCoordinates[0].Y);
        //            lineResult.EndCoordinate = new XyCoordinate(bothCoordinates[1].X, bothCoordinates[1].Y);

        //            currentCoordinate.X = bothCoordinates[0].X + directionX * parameter.ObjectSize.Width / 2;
        //            currentCoordinate.X = Math.Round(currentCoordinate.X, DecimalPoint);

        //            if (parameter.PathType == PathType.StepByStep)
        //            {
        //                for (int j = 0; j < parameter.PitchCount.Width; j++)
        //                {
        //                    lineResult.LineCoordinates.Add(currentCoordinate);
        //                    currentCoordinate.X += parameter.PitchDistance.Width * directionX;
        //                    currentCoordinate.X = Math.Round(currentCoordinate.X, DecimalPoint);
        //                }
        //            }
        //            // 한 줄 상/하로 이동
        //            currentCoordinate.Y += parameter.PitchDistance.Height * directionY;
        //            currentCoordinate.Y = Math.Round(currentCoordinate.Y, DecimalPoint);
        //            directionX *= switchDirection;
        //            this.Result.Add(lineResult);
        //        }
        //    }
        //    // Vertical
        //    else
        //    {
        //        currentCoordinate.X += directionX * parameter.ObjectSize.Width / 2;
        //        currentCoordinate.X = Math.Round(currentCoordinate.X, DecimalPoint);

        //        for (int i = 0; i < parameter.PitchCount.Width; i++)
        //        {
        //            lineResult = new LineResult();
        //            if (parameter.GetCoordinates(currentCoordinate.X, directionY, out bothCoordinates) != 0) continue;
        //            lineResult.StartCoordinate = new XyCoordinate(bothCoordinates[0].X, bothCoordinates[0].Y);
        //            lineResult.EndCoordinate = new XyCoordinate(bothCoordinates[1].X, bothCoordinates[1].Y);

        //            currentCoordinate.Y = bothCoordinates[0].Y + directionY * parameter.ObjectSize.Height / 2;
        //            currentCoordinate.Y = Math.Round(currentCoordinate.Y, DecimalPoint);

        //            if (parameter.PathType == PathType.StepByStep)
        //            {
        //                for (int j = 0; j < parameter.PitchCount.Height; j++)
        //                {
        //                    lineResult.LineCoordinates.Add(currentCoordinate);
        //                    currentCoordinate.Y += parameter.PitchDistance.Height * directionY;
        //                    currentCoordinate.Y = Math.Round(currentCoordinate.Y, DecimalPoint);
        //                }
        //            }
        //            // 한 줄 상/하로 이동
        //            currentCoordinate.X += parameter.PitchDistance.Width * directionX;
        //            currentCoordinate.X = Math.Round(currentCoordinate.X, DecimalPoint);
        //            directionY *= switchDirection;
        //            this.Result.Add(lineResult);
        //        }
        //    }

        //    for (int i = 0; i < this.Result.Count; i++)
        //    {
        //        if (parameter.PathType == PathType.StepByStep)
        //        {
        //            for (int j = 0; j < this.Result[i].LineCoordinates.Count; j++)
        //            {
        //                paths.Add(this.Result[i].LineCoordinates[j]);
        //            }
        //        }
        //        else if (parameter.PathType == PathType.Continuous)
        //        {
        //            paths.Add(this.Result[i].StartCoordinate);
        //            paths.Add(this.Result[i].EndCoordinate);
        //        }
        //    }
        //    this.Paths = new TwoDimensionPathReadOnlyCollection(paths);
        //    #endregion

        //    return ret;
        //}

        private int Run(ZigzagTwoDimensionPathGeneratorParameter parameter)
        {
            int ret = 0;

            //if (parameter is CircleZigzagTwoDimensionPathGeneratorParameter)
            //{
            //    if ((ret = this.Run((CircleZigzagTwoDimensionPathGeneratorParameter)parameter)) != 0) return ret;
            //}
            //if (parameter is RectangleZigzagTwoDimensionPathGeneratorParameter)
            //{
                //this.Result = new LineResultCollection();
                if ((ret = this.Run((RectangleZigzagTwoDimensionPathGeneratorParameter)parameter)) != 0) return ret;
            //}
            //else if (parameter is PolygonZigzagTwoDimensionPathGeneratorParameter)
            //{
            //    this.Result = new LineResultCollection();
            //    if ((ret = this.Run((PolygonZigzagTwoDimensionPathGeneratorParameter)parameter)) != 0) return ret;
            //}

            return ret;
        }

        protected virtual int GetAddressingPoint(int Index, RectangleZigzagTwoDimensionPathGeneratorParameter parameter, ref PointCollection addressingStartPointCollection)
        {
            int ret = 0;

            Size pitchCount = parameter.PitchCount;
            Point addressingPoint = new Point();
            addressingStartPointCollection = new PointCollection();

            if (Index < 0 || pitchCount.Width * pitchCount.Height <= Index)
                return -1; /*ErrorManager.Register(new ArgumentOutOfRangeException("Index"))*/;

            #region LocationStartPoint(LeftTop)
            if (parameter.StartAddressingPoint == StartAddressingPoint.LeftTop)
            {
                if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
            }
            #endregion

            #region LocationStartPoint(RightTop)
            else if (parameter.StartAddressingPoint == StartAddressingPoint.RightTop)
            {
                if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
            }
            #endregion

            #region LocationStartPoint(LeftBottom)
            else if (parameter.StartAddressingPoint == StartAddressingPoint.LeftBottom)
            {
                if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
            }
            #endregion

            #region LocationStartPoint(RightBottom)
            else if (parameter.StartAddressingPoint == StartAddressingPoint.RightBottom)
            {
                if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)

                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightTop && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = pitchCount.Height - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = pitchCount.Width - 1; 0 <= i; i--)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.LeftBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Vertical)
                {
                    for (int i = 0; i < pitchCount.Width; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Height; j++)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Height - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = i;
                                addressingPoint.Y = j;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
                else if (parameter.StartLocation == StartLocation.RightBottom && parameter.Direction == Direction.Horizontal)
                {
                    for (int i = 0; i < pitchCount.Height; i++)
                    {
                        addressingPoint = new Point();
                        if (i == 0 || i % 2 == 0)
                        {
                            for (int j = 0; j < pitchCount.Width; j++)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                        else
                        {
                            for (int j = pitchCount.Width - 1; 0 <= j; j--)
                            {
                                addressingPoint.X = j;
                                addressingPoint.Y = i;

                                addressingStartPointCollection.Add(addressingPoint);
                            }
                        }
                    }
                }
            }
            #endregion

            return ret;
        }
        #endregion

        #region PathGenerator Members
        protected override int OnGenerate(PathGeneratorParameter parameter)
        {
            return this.Run(parameter as ZigzagTwoDimensionPathGeneratorParameter);
        }

        protected new ZigzagTwoDimensionPathGeneratorParameter Parameter
        {
            get { return base.Parameter as ZigzagTwoDimensionPathGeneratorParameter; }
            set { base.Parameter = value; }
        }
        #endregion
    }
    public abstract class ZigzagTwoDimensionPathGeneratorParameter : TwoDimensionPathGeneratorParameter
    {
        #region Field
        private ZigzagTwoDimensionPathGenerator.Direction m_Direction;
        private ZigzagTwoDimensionPathGenerator.StartLocation m_StartLocation;
        private SizeD m_PitchDistance;
        private Size m_PitchCount;
        #endregion

        #region Constructor
        public ZigzagTwoDimensionPathGeneratorParameter() : base()
        {
            this.PathType = PathGenerator.PathType.StepByStep;
            this.Direction = ZigzagTwoDimensionPathGenerator.Direction.Horizontal;
            this.StartLocation = ZigzagTwoDimensionPathGenerator.StartLocation.LeftTop;
            this.PitchCount = new Size(10, 10);
            this.PitchDistance = new SizeD(1, 1);
        }
        #endregion

        #region Property
        /// <summary>
        /// 지그재그의 첫 방향을 가져오거나 설정한다.
        /// </summary>
        public ZigzagTwoDimensionPathGenerator.Direction Direction
        {
            get { return this.m_Direction; }
            set { this.m_Direction = value; }
        }

        /// <summary>
        /// 지그재그의 시작 위치를 가져오거나 설정한다.
        /// </summary>
        public ZigzagTwoDimensionPathGenerator.StartLocation StartLocation
        {
            get { return this.m_StartLocation; }
            set { this.m_StartLocation = value; }
        }

        /// <summary>
        /// 다음 경로까지의 이동 거리를 가져오거나 설정한다.
        /// </summary>
        public SizeD PitchDistance
        {
            get { return this.m_PitchDistance; }
            set { this.m_PitchDistance = value; }
        }

        /// <summary>
        /// 생성될 경로의 가로, 세로의 횟수를 가져오거나 설정한다.
        /// </summary>
        public Size PitchCount
        {
            get { return this.m_PitchCount; }
            set
            {
                if (this.m_PitchCount == value) return;
                this.m_PitchCount = value;
            }
        }
        #endregion

        #region Method
        public virtual void SetDirection(ref int directionX, ref int directionY)
        {
            // +1 : Plus 방향, -1 : Minus 방향
            // 시작 위치 : 좌측 상단
            if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftTop)
            {
                directionX = 1;
                directionY = -1;
            }
            // 시작 위치 : 좌측 하단
            else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftBottom)
            {
                directionX = 1;
                directionY = 1;
            }
            // 시작 위치 : 우측 상단
            else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightTop)
            {
                directionX = -1;
                directionY = -1;
            }
            // 시작 위치 : 우측 하단
            else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightBottom)
            {
                directionX = -1;
                directionY = 1;
            }
        }
        #endregion

        #region PathGeneratorParameter Members
        protected override Type OnGetPathGeneratorType()
        {
            return typeof(ZigzagTwoDimensionPathGenerator);
        }

        public int CreatePathGenerator(string name, out ZigzagTwoDimensionPathGenerator generator)
        {
            int ret = 0;

            generator = Activator.CreateInstance(this.OnGetPathGeneratorType(), name) as ZigzagTwoDimensionPathGenerator;

            if ((ret = generator.Generate(this)) != 0) return ret;

            return ret;
        }

        public int CreatePathGenerator(out ZigzagTwoDimensionPathGenerator generator)
        {
            return this.CreatePathGenerator(PathGeneratorParameter.DefaultName, out generator);
        }
        #endregion

        #region Object Members
        public override string ToString()
        {
            StringWriter sw = new StringWriter();

            sw.Write(base.ToString());
            sw.WriteLine("\tDirection = {0}", this.Direction);
            sw.WriteLine("\tStartLocation = {0}", this.StartLocation);
            sw.WriteLine("\tPitchDistance = {0}", this.PitchDistance);
            sw.WriteLine("\tPitchCount = {0}", this.PitchCount);

            return sw.ToString();
        }
        #endregion
    }
    [Serializable]
    public class RectangleZigzagTwoDimensionPathGeneratorParameter : ZigzagTwoDimensionPathGeneratorParameter
    {
        #region Field
        private SizeD m_Area;
        private XyCoordinate m_StartCoordinate;
        private XyCoordinate m_CenterCoordinate;
        private ZigzagTwoDimensionPathGenerator.StartAddressingPoint m_StartAddressingPoint;
        private ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes m_AreaCalculationParameterType;
        #endregion

        #region Constructor
        public RectangleZigzagTwoDimensionPathGeneratorParameter() : base()
        {
            this.Area = new SizeD();
            this.StartCoordinate = new XyCoordinate();
            this.CenterCoordinate = new XyCoordinate();
            this.StartAddressingPoint = ZigzagTwoDimensionPathGenerator.StartAddressingPoint.LeftBottom;
            this.AreaCalculationParameterType = ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes.Area;
        }
        #endregion

        #region Property
        /// <summary>
        /// 생성될 경로의 면적을 가져오거나 설정한다.
        /// </summary>
        public SizeD Area
        {
            get { return this.m_Area; }
            set
            {
                if (this.m_Area == value) return;
                this.m_Area = value;
            }
        }

        /// <summary>
        /// Area의 중심좌표를 가져오거나 설정한다.
        /// </summary>
        public new XyCoordinate CenterCoordinate
        {
            get { return this.m_CenterCoordinate; }
            set
            {
                this.m_CenterCoordinate = value;

                if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftTop)
                {
                    this.m_StartCoordinate = new XyCoordinate(this.CenterCoordinate.X - this.Area.Width / 2, this.CenterCoordinate.Y + this.Area.Height / 2);
                }
                else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftBottom)
                {
                    this.m_StartCoordinate = new XyCoordinate(this.CenterCoordinate.X - this.Area.Width / 2, this.CenterCoordinate.Y - this.Area.Height / 2);
                }
                else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightTop)
                {
                    this.m_StartCoordinate = new XyCoordinate(this.CenterCoordinate.X + this.Area.Width / 2, this.CenterCoordinate.Y + this.Area.Height / 2);
                }
                else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightBottom)
                {
                    this.m_StartCoordinate = new XyCoordinate(this.CenterCoordinate.X + this.Area.Width / 2, this.CenterCoordinate.Y - this.Area.Height / 2);
                }
            }
        }

        /// <summary>
        /// 모션이 최초로 이동할 위치를 가져오거나 설정한다.
        /// </summary>
        public XyCoordinate StartCoordinate
        {
            get { return this.m_StartCoordinate; }
            set
            {
                this.m_StartCoordinate = value;

                if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftTop)
                {
                    this.m_CenterCoordinate = new XyCoordinate(this.StartCoordinate.X + this.Area.Width / 2, this.StartCoordinate.Y - this.Area.Height / 2);
                }
                else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftBottom)
                {
                    this.m_CenterCoordinate = new XyCoordinate(this.StartCoordinate.X + this.Area.Width / 2, this.StartCoordinate.Y + this.Area.Height / 2);
                }
                else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightTop)
                {
                    this.m_CenterCoordinate = new XyCoordinate(this.StartCoordinate.X - this.Area.Width / 2, this.StartCoordinate.Y - this.Area.Height / 2);
                }
                else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightBottom)
                {
                    this.m_CenterCoordinate = new XyCoordinate(this.StartCoordinate.X - this.Area.Width / 2, this.StartCoordinate.Y + this.Area.Height / 2);
                }
            }
        }

        public ZigzagTwoDimensionPathGenerator.StartAddressingPoint StartAddressingPoint
        {
            get { return this.m_StartAddressingPoint; }
            set { this.m_StartAddressingPoint = value; }
        }

        /// <summary>
        /// 영역 계산을 하기위해 입력할 파라미터를 가져오거나 설정한다.
        /// </summary>
        public ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes AreaCalculationParameterType
        {
            get { return this.m_AreaCalculationParameterType; }
            set { this.m_AreaCalculationParameterType = value; }
        }
        #endregion

        #region Method
        public int GetCoordinates(double axisValue, int direction, out XyCoordinateCollection intersectionCollection)
        {
            int ret = 0;
            XyCoordinateCollection twoPointCollection = new XyCoordinateCollection();
            intersectionCollection = new XyCoordinateCollection();
            RectangleD rectangle = RectangleD.Empty;

            if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftTop)
            {
                if (this.InvertedX == true && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(0, this.Area.Height), this.Area);
                }
                else if (this.InvertedX == true && this.InvertedY == false)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, 0), this.Area);
                }
                else if (this.InvertedX == false && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, this.Area.Height), this.Area);
                }
                else
                {
                    rectangle = new RectangleD(this.StartCoordinate, this.Area);
                }
            }
            else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.LeftBottom)
            {
                if (this.InvertedX == true && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, 0), this.Area);
                }
                else if (this.InvertedX == true && this.InvertedY == false)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, this.Area.Height), this.Area);
                }
                else if (this.InvertedX == false && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate, this.Area);
                }
                else
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(0, this.Area.Height), this.Area);
                }
            }
            else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightTop)
            {
                if (this.InvertedX == true && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(0, this.Area.Height), this.Area);
                }
                else if (this.InvertedX == true && this.InvertedY == false)
                {
                    rectangle = new RectangleD(this.StartCoordinate, this.Area);
                }
                else if (this.InvertedX == false && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, this.Area.Height), this.Area);
                }
                else
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, 0), this.Area);
                }
            }
            else if (this.StartLocation == ZigzagTwoDimensionPathGenerator.StartLocation.RightBottom)
            {

                if (this.InvertedX == true && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate, this.Area);
                }
                else if (this.InvertedX == true && this.InvertedY == false)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(0, this.Area.Height), this.Area);
                }
                else if (this.InvertedX == false && this.InvertedY == true)
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, 0), this.Area);
                }
                else
                {
                    rectangle = new RectangleD(this.StartCoordinate + new XyCoordinate(-this.Area.Width, this.Area.Height), this.Area);
                }
            }

            // 가로방향 진행
            if (this.Direction == ZigzagTwoDimensionPathGenerator.Direction.Horizontal)
            {
                twoPointCollection = RectangleD.FindLineRectangleIntersection(rectangle, new XyCoordinate(0, axisValue), 0);

                if (twoPointCollection.Count < 2)
                {
                    return -1;
                }

                if (direction == 1)
                {
                    intersectionCollection.Add(twoPointCollection[0]);
                    intersectionCollection.Add(twoPointCollection[1]);
                }
                else
                {
                    intersectionCollection.Add(twoPointCollection[1]);
                    intersectionCollection.Add(twoPointCollection[0]);
                }

                //lineWidth = Math.Abs(twoPointCollection[1].X - twoPointCollection[0].X);
                //this.PitchCount = new Size((int)(lineWidth / this.PitchDistance.Width + 1), this.PitchCount.Height);
            }
            //세로방향 진행
            else if (this.Direction == ZigzagTwoDimensionPathGenerator.Direction.Vertical)
            {
                twoPointCollection = RectangleD.FindLineRectangleIntersection(rectangle, new XyCoordinate(axisValue, 0), 90);

                if (twoPointCollection.Count < 2)
                {
                    return -1;
                }

                if (direction == 1)
                {
                    intersectionCollection.Add(twoPointCollection[0]);
                    intersectionCollection.Add(twoPointCollection[1]);
                }
                else
                {
                    intersectionCollection.Add(twoPointCollection[1]);
                    intersectionCollection.Add(twoPointCollection[0]);
                }

                //lineHeight = Math.Abs(twoPointCollection[1].Y - twoPointCollection[0].Y);
                //this.PitchCount = new Size(this.PitchCount.Width, (int)(lineHeight / this.PitchDistance.Height + 1));
            }

            return ret;
        }

        public void SetAreaAndPitchCount()
        {
            int pitchCountWidth = 0;
            int pitchCountHeight = 0;

            if (this.AreaCalculationParameterType == ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes.PitchCount)
            {
                this.Area = new SizeD(this.PitchDistance.Width * (this.PitchCount.Width - 1), this.PitchDistance.Height * (this.PitchCount.Height - 1));
            }
            else
            {
                pitchCountWidth = (int)Math.Truncate(this.Area.Width / this.PitchDistance.Width + 1);
                pitchCountHeight = (int)Math.Truncate(this.Area.Height / this.PitchDistance.Height + 1);
                this.PitchCount = new Size(pitchCountWidth, pitchCountHeight);
            }
        }
        #endregion

        //#region PathGeneratorParameter Members
        //protected override PathGeneratorControl OnGetControl(PathGeneratorParameter parameter)
        //{
        //    return new RectangleZigzagTwoDimensionPathGeneratorControl(parameter as RectangleZigzagTwoDimensionPathGeneratorParameter);
        //}
        //#endregion

        #region Object Members
        public override string ToString()
        {
            StringWriter sw = new StringWriter();

            sw.Write(base.ToString());
            sw.WriteLine("\tArea = {0}", this.Area);
            sw.WriteLine("\tStartCoordinate = {0}", this.StartCoordinate);
            sw.WriteLine("\tStartAddressingPoint = {0}", this.StartAddressingPoint);
            sw.WriteLine("\tAreaCalculationParameterType = {0}", this.AreaCalculationParameterType);

            return sw.ToString();
        }
        #endregion
    }
}
