using QMC.Common.Hmi;
using QMC.Common.PathGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Parts.LaserPitchMoveShotter;
using static QMC.Common.PathGenerators.ZigzagTwoDimensionPathGenerator;

namespace QMC.Common.Parts
{
    #region LaserPitchMoveShotterConfig
    [Serializable]
    public class LaserPitchMoveShotterConfig
    {
        #region Field
        private double m_PitchDistanceX;
        private double m_PitchDistanceY;
        private Point m_Count;
        private ZigzagTwoDimensionPathGenerator.StartLocation m_StartLocation;
        private ZigzagTwoDimensionPathGenerator.Direction m_Direction;
        #endregion

        #region Property
        [Browsable(false)]
        public XyzPositionDataCollection GridPositions { set; get; }

        [Category("GridXY")]
        [Browsable(true)]
        public double PitchDistanceX
        {
            get { return m_PitchDistanceX; }
            set { m_PitchDistanceX = value; }
        }
        [Category("GridXY")]
        [Browsable(true)]
        public double PitchDistanceY
        {
            get { return m_PitchDistanceY; }
            set { m_PitchDistanceY = value; }
        }

        [Category("GridXY")]
        [Browsable(true)]
        public Point Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }
        [Category("GridXY")]
        [Browsable(true)]
        public ZigzagTwoDimensionPathGenerator.StartLocation StartLocation
        {
            get { return this.m_StartLocation; }
            set { this.m_StartLocation = value; }
        }

        [Category("GridXY")]
        [Browsable(true)]
        public ZigzagTwoDimensionPathGenerator.Direction Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }
        [Browsable(true)]
        public int MoveToDelay { get; set; }


        [Category("LaserParameter")]
        [TypeConverter(typeof(LaserParameterConverter))]
        [Browsable(true)]
        public LaserParameterForPitchMoveShotter LaserParameter { get; set; }
        #endregion

        #region Method
        public void Init()
        {
            if (GridPositions == null)
                GridPositions = new XyzPositionDataCollection();

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

            if (this.LaserParameter == null)
                this.LaserParameter = new LaserParameterForPitchMoveShotter();

            m_Direction = ZigzagTwoDimensionPathGenerator.Direction.Horizontal;
            m_PitchDistanceX = 0;
            m_PitchDistanceY = 0;
            m_Count = new Point();
            m_StartLocation = new ZigzagTwoDimensionPathGenerator.StartLocation();
            StartLocation = StartLocation.LeftBottom;
            this.MoveToDelay = 100;
        }
        #endregion
    }
    #endregion

    #region LaserParameter
    [Serializable]
    public class LaserParameterForPitchMoveShotter
    {
        #region Field
        private Equipment.RtcMode m_RtcMode;
        private double m_LaserOnTime;
        private double m_LaserOffTime;
        private double m_JumpSpeed;
        private SizeD m_ShotSize;
        private ShotShape m_Shape;
        private double m_MarkSpeed;
        #endregion

        #region Constructor
        public LaserParameterForPitchMoveShotter()
        {
            Init();
        }
        #endregion

        #region Property
        public Equipment.RtcMode RtcMode
        {
            get { return this.m_RtcMode; }
            set { this.m_RtcMode = value; }
        }

        public double LaserOnTime
        {
            get { return this.m_LaserOnTime; }
            set { this.m_LaserOnTime = value; }
        }

        public double LaserOffTime
        {
            get { return this.m_LaserOffTime; }
            set { this.m_LaserOffTime = value; }
        }

        public double JumpSpeed
        {
            get { return this.m_JumpSpeed; }
            set { this.m_JumpSpeed = value; }
        }

        public SizeD ShotSize
        {
            get { return this.m_ShotSize; }
            set { this.m_ShotSize = value; }
        }

        public ShotShape Shape
        {
            get { return this.m_Shape; }
            set { this.m_Shape = value; }
        }

        public double MarkSpeed
        {
            get { return this.m_MarkSpeed; }
            set { this.m_MarkSpeed = value; }
        }
        #endregion

        #region Method
        public void Init()
        {
            this.RtcMode = Equipment.RtcMode.RTC_RTC6;
            this.Shape = ShotShape.Cross;
            this.ShotSize = new SizeD(1.0, 1.0);
            this.LaserOnTime = 20.0;
            this.LaserOffTime = 20.0;
            this.JumpSpeed = 50.0;
            this.MarkSpeed = 50.0;
        }
        #endregion
    }
    #endregion

    #region LaserParameterConverter
    [Serializable]
    internal class LaserParameterConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = value as string;
                string[] token;
                token = s.Split(',');
                LaserParameterForPitchMoveShotter specification = new LaserParameterForPitchMoveShotter();
                specification.RtcMode = (Equipment.RtcMode)Enum.Parse(typeof(Equipment.RtcMode), token[0]);
                specification.LaserOnTime = double.Parse(token[1]);
                specification.LaserOffTime = double.Parse(token[2]);
                specification.JumpSpeed = double.Parse(token[3]);
                specification.ShotSize = SizeD.Parse(token[4]);
                specification.Shape = (ShotShape)Enum.Parse(typeof(ShotShape), token[5]);
                specification.MarkSpeed = double.Parse(token[6]);

                return specification;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is LaserParameterForPitchMoveShotter)
            {
                LaserParameterForPitchMoveShotter specification = value as LaserParameterForPitchMoveShotter;
                //return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                //    specification.Velocity, specification.Acceleration, specification.Deceleration,
                //    specification.NegativePosition, specification.PositivePosition, specification.EscapeDistance,
                //    specification.HomePosition, specification.Method,
                //    specification.EnablePreciseSearch, specification.PreciseSearchVelocityPercent, specification.EnableIndexSearch, specification.UseAjinAxlFunction);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion
}
