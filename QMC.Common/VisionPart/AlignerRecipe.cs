using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Hmi;

using static QMC.Common.VisionPart.Aligner;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class AlignerRecipe
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
        [Serializable]
        public enum PositionAligns
        {
            Reference,
            First,
            Second,
            Third,
            Forth
        }

        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PatternMatchingParameters PatternMatchingParameter { set; get; }

        //    public PathParameter PathParameter { set; get; }
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PathGenerator pathGenerator { set; get; }

        [Category("FiducialMark")]
        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        [Browsable(false)]
        public XyCoordinate FirstFiducialmark { set; get; }
        [Category("FiducialMark")]
        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        [Browsable(false)]
        public XyCoordinate SecondFiducialmark { set; get; }
        [Category("FiducialMark")]
        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        [Browsable(false)]
        public XyCoordinate ThirdFiducialmark { set; get; }
        [Category("FiducialMark")]
        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        [Browsable(false)]
        public XyCoordinate FourthFiducialmark { set; get; }

        [Category("Alignment Parameter")]
        public bool VerificationEnable { get; set; }
        [Category("Alignment Parameter")]
        public double AngleTolerance { set; get; }
        [Category("Alignment Parameter")]
        public int RetryCount { set; get; }
        [Category("Theta Correction")]
        public double ThetaCorrectionAngleTolerance { set; get; }
        [Category("Theta Correction")]
        public double ThetaCorrectionUnitAngle { set; get; }
        [Category("Theta Correction")]
        public bool EnableThetaCorrection { set; get; }

        [Category("Theta Correction")]
        public bool UseSaveThetaCorrectionData { set; get; }

        [Category("Theta Correction Range")]
        public double MaxTolerance { set; get; }

        [Category("Theta Correction Range")]
        public double MinTolerance { set; get; }

        [Category("Four Point Align")]
        public bool UseFourPointAlign { set; get; }
        [Category("Four Point Align")]
        public bool UseFourPointAlignNotTheta { set; get; }
        [Category("Two Point Align")]
        public bool UseTwoPointAlign { get; set; }

        //public Aligner Aligner;
        [Category("FiducialMark")]
        //[TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        //public XytPositionDataCollection AlignPositions { set; get; }
        //public XyztPositionDataCollection AlignPositions { set; get; }
        //public XyzztPositionDataCollection AlignPositions { set; get; }
        public UvwzxyzPositionDataCollection AlignPositions { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public IlluminationDataSet IlluminationDataSet { set; get; }

        [Category("FiducialMark")]
        public double FiducialXDistance { set; get; }

        [Category("FiducialMark")]
        public double FiducialYDistance { set; get; }

        public AlignerRecipe(Part part)
        {
            Init(part);

            FirstFiducialmark = new XyCoordinate(0, 0);
            SecondFiducialmark = new XyCoordinate(10, 1);
            ThirdFiducialmark = new XyCoordinate(10, 11);
            FourthFiducialmark = new XyCoordinate(0, 10);
            FiducialXDistance = 0.0;
            FiducialYDistance = 0.0;
            RetryCount = 1;
            AngleTolerance = 1;

            this.VerificationEnable = false;
            ThetaCorrectionAngleTolerance = 3;
            ThetaCorrectionUnitAngle = 1;
            EnableThetaCorrection = false;
            UseFourPointAlign = false;
            UseFourPointAlignNotTheta = false;
            UseTwoPointAlign = true;

            InspectRoiStartLocation = new Point();
            InspectRoiEndLocation = new Point();
            TrainRoiStartLocation = new Point();
            TrainRoiEndLocation = new Point();

            UseSaveThetaCorrectionData = false;
            MaxTolerance = 0;
            MinTolerance = 0;
        }

        public void Init(Part part)
        {
            if (PatternMatchingParameter == null)
            {
                PatternMatchingParameter = new PatternMatchingParameters();
            }

            if (pathGenerator == null)
                pathGenerator = new PathGenerator();
            if (IlluminationDataSet == null)
                IlluminationDataSet = new IlluminationDataSet(part.Name);

            if (AlignPositions == null)
            {
                AlignPositions = new UvwzxyzPositionDataCollection();
                foreach (PositionAligns key in Enum.GetValues(typeof(PositionAligns)))
                {
                    UvwzxyzPositionData positionBase = new UvwzxyzPositionData();
                    positionBase.Name = key.ToString();
                    //positionBase.Coordinate = 
                    AlignPositions.Add(positionBase);

                    UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();
                    positionTarget.Name = key.ToString();
                    positionTarget.Type = TargetType.Offset;
                    AlignPositions.Add(positionTarget);
                }
            }
        }

        public List<string> GetAlignsPositionList()
        {
            List<string> ret = new List<string>();
            foreach (UvwzxyzPositionData position in AlignPositions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public void SetAlignPositionData(string strPosition, TargetType targetType, UvwCoordinate coordinate)
        {
            foreach (UvwzxyzPositionData position in AlignPositions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.U = coordinate.U;
                    position.V = coordinate.V;
                    position.W = coordinate.W;
                    break;
                }
            }
        }


        //  2023. 05. 19.  SCH : 이거 대신
        //public XytCoordinate GetAlignPositionData(string strPosition)
        //{
        //    XytCoordinate coordinate = new XytCoordinate();

        //    foreach (XytPositionData position in AlignPositions)
        //    {
        //        if (position.Name == strPosition)
        //        {
        //            coordinate += position.Coordinate;
        //            break;
        //        }
        //    }

        //    return coordinate;
        //}

        //  2023. 05. 19.  SCH : 이거 쓰자 --> 또 이거 대신
        //public XyztCoordinate GetAlignPositionData(string strPosition)
        //{
        //    XyztCoordinate coordinate = new XyztCoordinate();

        //    foreach (XyztPositionData position in AlignPositions)
        //    {
        //        if (position.Name == strPosition)
        //        {
        //            coordinate += position.Coordinate;
        //            break;
        //        }
        //    }

        //    return coordinate;
        //}

        //  2023. 07. 13.  SCH : 이거 쓰자 --> 또 이거 대신
        //public XyzztCoordinate GetAlignPositionData(string strPosition)
        //{
        //    XyzztCoordinate coordinate = new XyzztCoordinate();

        //    foreach (XyzztPositionData position in AlignPositions)
        //    {
        //        if (position.Name == strPosition)
        //        {
        //            coordinate += position.Coordinate;
        //            break;
        //        }
        //    }

        //    return coordinate;
        //}

        //  2023. 11. 23.  SCH : 이거 쓰자
        public UvwzxyzCoordinate GetAlignPositionData(string strPosition)
        {
            UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();

            foreach (UvwzxyzPositionData position in AlignPositions)
            {
                if (position.Name == strPosition)
                {
                    coordinate += position.Coordinate;
                    break;
                }
            }

            return coordinate;
        }

        public XyCoordinate GetAlignPositionData_UVW(string strPosition)
        {
            XyCoordinate coordinate = new XyCoordinate();

            foreach (UvwzxyzPositionData position in AlignPositions)
            {
                if (position.Name == strPosition)
                {
                    coordinate.X += position.Coordinate.U;
                    coordinate.Y += position.Coordinate.V;
                    break;
                }
            }

            return coordinate;
        }

        public void UpdateFiducialMark()
        {
            if (AlignPositions != null && AlignPositions.Count != 0)
            {
                this.FirstFiducialmark = (XyCoordinate)AlignPositions.GetPositionCoordinate_UVW(PositionAligns.First.ToString());
                this.SecondFiducialmark = (XyCoordinate)AlignPositions.GetPositionCoordinate_UVW(PositionAligns.Second.ToString());
                this.ThirdFiducialmark = (XyCoordinate)AlignPositions.GetPositionCoordinate_UVW(PositionAligns.Third.ToString());
                this.FourthFiducialmark = (XyCoordinate)AlignPositions.GetPositionCoordinate_UVW(PositionAligns.Forth.ToString());
            }
        }
    }

    //  2023. 05. 19.  SCH : TwoPointAlignerRecipe.cs 에 정의되어 있음.

    //[Serializable]
    //[TypeConverter(typeof(NormalExpandableObjectConverter))]
    //public class PathParameter
    //{
    //    [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
    //    public XyCoordinate CenterCoordinate { set; get; }
    //    public AlignerRecipe.Direction Direction { get; set; }
    //    public AlignerRecipe.PathType PathType { get; set; }
    //    public SizeD PitchDistance { get; set; }
    //    public SizeD Area { get; set; }
    //    public Size PitchCount { get; set; }

    //    public bool InvertedX { set; get; }
    //    public bool InvertedY { set; get; }
    //    public PathParameter()
    //    {
    //        CenterCoordinate = new XyCoordinate(0, 0);
    //        Direction = AlignerRecipe.Direction.ClockWise;
    //        PathType = AlignerRecipe.PathType.StepByStep;
    //        PitchDistance = new SizeD(10, 10);
    //        Area = new SizeD(10, 10);
    //        PitchCount = new Size(10, 10);
    //        InvertedX = false;
    //        InvertedY = false;
    //    }
    //}
    //[Serializable]
    //public class PathGenerator
    //{
    //    [TypeConverter(typeof(NormalExpandableObjectConverter))]
    //    public PathParameter PathParameter { set; get; }


    //    public List<XyCoordinate> Paths { get; set; }
    //    public PathGenerator()
    //    {
    //        //     PathParameter = new PathParameter();
    //        Paths = new List<XyCoordinate>();
    //        PathParameter = new PathParameter();
    //    }

    //    protected int OnGenerate(PathParameter paramter)
    //    {
    //        return this.Run(paramter);
    //    }
    //    private int Run(PathParameter parameter)
    //    {//?
    //        int ret = 0;
    //        int directionX = 1;
    //        int directionY = 1;
    //        int loopCount = 0;
    //        List<XyCoordinate> paths = new List<XyCoordinate>(parameter.PitchCount.Width * parameter.PitchCount.Height);
    //        XyCoordinate currentCoordinate = new XyCoordinate(parameter.CenterCoordinate.X, parameter.CenterCoordinate.Y);
    //        int offsetX = 0;
    //        int offsetY = 0;
    //        paths.Add(currentCoordinate);
    //        #region Direction 및 시작 위치 지정
    //        // 시계방향이면서 N x M에서 N이 M보다 크거나 같을때
    //        if (parameter.Direction == AlignerRecipe.Direction.ClockWise && parameter.PitchCount.Height <= parameter.PitchCount.Width)
    //        {
    //            // X : + , Y : +
    //            directionX = 1;
    //            directionY = 1;
    //            //시작위치는 왼쪽 상단.
    //            currentCoordinate = new XyCoordinate(currentCoordinate.X - Math.Truncate((parameter.PitchCount.Width - parameter.PitchCount.Height) / 2 * parameter.PitchDistance.Width), currentCoordinate.Y);
    //        }
    //        // 시계방향이면서 N x M에서 M이 N보다 클때
    //        else if (parameter.Direction == AlignerRecipe.Direction.ClockWise && parameter.PitchCount.Width < parameter.PitchCount.Height)
    //        {
    //            // X : + , Y : -
    //            directionX = 1;
    //            directionY = -1;
    //            // 시작 위치는 왼쪽 하단.
    //            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + Math.Truncate((parameter.PitchCount.Height - parameter.PitchCount.Width) / 2 * parameter.PitchDistance.Height));
    //        }
    //        // 반시계방향이면서 N x M에서 N이 M보다 크거나 같을때
    //        else if (parameter.Direction == AlignerRecipe.Direction.CounterClockWise && parameter.PitchCount.Height <= parameter.PitchCount.Width)
    //        {
    //            // X : - , Y : +
    //            directionX = -1;
    //            directionY = 1;
    //            // 시작 위치는 오른쪽 상단
    //            currentCoordinate = new XyCoordinate(currentCoordinate.X + Math.Truncate((parameter.PitchCount.Width - parameter.PitchCount.Height) / 2 * parameter.PitchDistance.Width), currentCoordinate.Y);
    //        }
    //        // 반시계방향이면서 N x M에서 M이 N보다 클때
    //        else if (parameter.Direction == AlignerRecipe.Direction.CounterClockWise && parameter.PitchCount.Width < parameter.PitchCount.Height)
    //        {
    //            // X : - , Y : -
    //            directionX = -1;
    //            directionY = -1;
    //            // 시작 위치는 오른쪽 하단
    //            currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + Math.Truncate((parameter.PitchCount.Height - parameter.PitchCount.Width) / 2 * parameter.PitchDistance.Height));
    //        }

    //        // 짝수일 경우 처리
    //        if (parameter.PitchCount.Width % 2 == 0)
    //            currentCoordinate -= new XyCoordinate(parameter.PitchDistance.Width / 2, 0);
    //        if (parameter.PitchCount.Height % 2 == 0)
    //            currentCoordinate -= new XyCoordinate(0, parameter.PitchDistance.Height / 2);

    //        paths.Add(currentCoordinate);
    //        #endregion

    //        #region Spiral 방식으로 Loop 시작
    //        // 가로의 횟수가 높을 경우.
    //        if (parameter.PitchCount.Height < parameter.PitchCount.Width)
    //        {

    //            loopCount = parameter.PitchCount.Height;

    //            for (int i = 1; i < loopCount + 1; i++)
    //            {
    //                // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
    //                if (i == loopCount && loopCount % 2 == 0) offsetX = 1;

    //                if (parameter.PathType == AlignerRecipe.PathType.StepByStep)
    //                {
    //                    // 가로의 횟수가 높기 때문에 가로부터 진행.
    //                    for (int j = 0; j < i - offsetX + (parameter.PitchCount.Width - (int)Math.Ceiling(parameter.PitchCount.Height / 2.0) * 2); j++)
    //                    {
    //                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
    //                        paths.Add(currentCoordinate);
    //                    }
    //                }
    //                else if (parameter.PathType == AlignerRecipe.PathType.Continuous)
    //                {
    //                    currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX + (parameter.PitchCount.Width - (int)Math.Round(parameter.PitchCount.Height / 2.0) * 2)), currentCoordinate.Y);
    //                    paths.Add(currentCoordinate);
    //                }

    //                // 마지막 루프에서는 height쪽으로 Path를 생성하지 않음.
    //                if (i == loopCount && loopCount % 2 == 0) break;
    //                if (i == loopCount && loopCount % 2 != 0) offsetY = 1;

    //                if (parameter.PathType == AlignerRecipe.PathType.StepByStep)
    //                {
    //                    for (int j = 0; j < i - offsetY; j++)
    //                    {
    //                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
    //                        paths.Add(currentCoordinate);
    //                    }
    //                }
    //                else if (parameter.PathType == AlignerRecipe.PathType.Continuous)
    //                {
    //                    currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY));
    //                    paths.Add(currentCoordinate);
    //                }

    //                directionX *= -1;
    //                directionY *= -1;
    //            }
    //        }
    //        // 세로의 횟수가 높을 경우.
    //        else if (parameter.PitchCount.Width < parameter.PitchCount.Height)
    //        {
    //            loopCount = parameter.PitchCount.Width;

    //            for (int i = 1; i < loopCount + 1; i++)
    //            {
    //                // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
    //                if (i == loopCount && loopCount % 2 == 0) offsetY = 1;

    //                if (parameter.PathType == AlignerRecipe.PathType.StepByStep)
    //                {
    //                    // 세로 횟수가 높기 때문에 세로부터 진행.
    //                    for (int j = 0; j < i - offsetY + (parameter.PitchCount.Height - (int)Math.Ceiling(parameter.PitchCount.Width / 2.0) * 2); j++)
    //                    {
    //                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
    //                        paths.Add(currentCoordinate);
    //                    }
    //                }
    //                else if (parameter.PathType == AlignerRecipe.PathType.Continuous)
    //                {
    //                    currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY + (parameter.PitchCount.Height - (int)Math.Round(parameter.PitchCount.Width / 2.0) * 2)));
    //                    paths.Add(currentCoordinate);
    //                }

    //                // 마지막 루프에서는 Width쪽으로 Path를 생성하지 않음.
    //                if (i == loopCount && loopCount % 2 == 0) break;
    //                if (i == loopCount && loopCount % 2 != 0) offsetX = 1;

    //                if (parameter.PathType == AlignerRecipe.PathType.StepByStep)
    //                {
    //                    for (int j = 0; j < i - offsetX; j++)
    //                    {
    //                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
    //                        paths.Add(currentCoordinate);
    //                    }
    //                }
    //                else if (parameter.PathType == AlignerRecipe.PathType.Continuous)
    //                {
    //                    currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX), currentCoordinate.Y);
    //                    paths.Add(currentCoordinate);
    //                }

    //                directionX *= -1;
    //                directionY *= -1;
    //            }
    //        }
    //        // 가로, 세로의 횟수가 같을 경우.
    //        else if (parameter.PitchCount.Width == parameter.PitchCount.Height)
    //        {
    //            loopCount = parameter.PitchCount.Width;

    //            for (int i = 1; i <= loopCount + 1; i++)
    //            {
    //                // 마지막 루프에서는 loopCount를 1회 낮추기 위해 Offset값을 1로 변경.
    //                if (i == loopCount) offsetX = 1;

    //                if (parameter.PathType == AlignerRecipe.PathType.StepByStep)
    //                {
    //                    for (int j = 0; j < i - offsetX; j++)
    //                    {
    //                        currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX, currentCoordinate.Y);
    //                        paths.Add(currentCoordinate);
    //                    }
    //                }
    //                else if (parameter.PathType == AlignerRecipe.PathType.Continuous)
    //                {
    //                    currentCoordinate = new XyCoordinate(currentCoordinate.X + parameter.PitchDistance.Width * directionX * (i - offsetX), currentCoordinate.Y);
    //                    paths.Add(currentCoordinate);
    //                }

    //                // 마지막 루프에서는 height쪽으로 Path를 생성하지 않음.
    //                if (i == loopCount) break;

    //                if (parameter.PathType == AlignerRecipe.PathType.StepByStep)
    //                {
    //                    for (int j = 0; j < i - offsetY; j++)
    //                    {
    //                        currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY);
    //                        paths.Add(currentCoordinate);
    //                    }
    //                }
    //                else if (parameter.PathType == AlignerRecipe.PathType.Continuous)
    //                {
    //                    currentCoordinate = new XyCoordinate(currentCoordinate.X, currentCoordinate.Y + parameter.PitchDistance.Height * directionY * (i - offsetY));
    //                    paths.Add(currentCoordinate);
    //                }

    //                directionX *= -1;
    //                directionY *= -1;
    //            }
    //        }
    //        #endregion

    //        this.Paths = paths;

    //        return ret;
    //    }


    //    public PathParameter GetParameter()
    //    {
    //        return (PathParameter)CopyUtility.GetDeepCopy(PathParameter);
    //    }


    //    public int Generate(PathParameter paramter)
    //    {
    //        int ret = 0;

    //        this.PathParameter = paramter;

    //        if ((ret = this.OnGenerate(paramter)) != 0) return ret;

    //        return ret;
    //    }


    //}
    //public class PathGeneratorCollection : Collection<PathGenerator>
    //{

    //}
    //public class PathParameterCollection : Collection<PathParameter>
    //{

    //}
}
