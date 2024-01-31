using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Motion.Ajin.Motions
{
    #region AjinAxlMotionType
    [Serializable]
    public enum AjinAxlMotionType
    {
        Trapezoidal,
        SCurve
    }
    #endregion

    #region AjinAxlAxisConfiguration
    [Serializable]
    public class AjinAxlAxisConfiguration : MotionAxisConfiguration
    {
        private AjinAxlMotionType m_MotionType;
        //private AjinAxlGantrySpecification m_GantrySpecification;
        private double m_AccelerationJerk;
        private double m_DecelerationJerk;

        public AjinAxlAxisConfiguration()
            : base()
        {
            this.m_MotionType = AjinAxlMotionType.Trapezoidal;
            SetDefaultValues();
        }

        public AjinAxlMotionType MotionType
        {
            get { return this.m_MotionType; }
            set { this.m_MotionType = value; }
        }
        //[Category("Gantry")]
        //[TypeConverter(typeof(AjinAxlGantrySpecification))]
        //public AjinAxlGantrySpecification GantrySpecification
        //{
        //    get { return this.m_GantrySpecification; }
        //    set { this.m_GantrySpecification = value; }
        //}

        public double AccelerationJerk
        {
            get { return this.m_AccelerationJerk; }
            set
            {
                if (value < 0 || 100.0 < value)
                    throw new ArgumentOutOfRangeException("AccelerationJerk");

                this.m_AccelerationJerk = value;
            }
        }

        public double DecelerationJerk
        {
            get { return this.m_DecelerationJerk; }
            set
            {
                if (value < 0 || 100.0 < value)
                    throw new ArgumentOutOfRangeException("DecelerationJerk");

                this.m_DecelerationJerk = value;
            }
        }

        /// <summary>
        /// UnitPerSec2 = 0,
        /// Second = 1
        /// </summary>
        private uint m_AccelUnit;
        public uint AccelUnit
        {
            get { return this.m_AccelUnit; }
            set { this.m_AccelUnit = value; }
        }

        /// <summary>
		/// 모션 보드의 NO를 가져오거나 설정합니다.
		/// </summary>
        public int BoardNo
        {
            get; set;
        }



        /// <summary>
        /// Gets or Sets the maximum velocity
        /// </summary>
        public double MaxVelocity
        {
            get; set;
        }

        /// <summary>
        /// 읽은 펄스 레벨의 실제 위치에 적용할 승수를 가져오거나 설정합니다.
        /// </summary>
        public Fraction ActualPositionMultiplier
        {
            get; set;
        }

        /// <summary>
        /// 명령 위치를 실제 위치로 사용할지 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool ActualPositionByCommandPosition
        {
            get; set;
        }


        /// <summary>
        /// 축의 위치 오프셋을 가져오거나 설정한다
        /// 절대 encoder를 사용하는 경우 활용할 수 있다.
        /// 단 encoder위치를 리셋하는 경우 다시 값을 변경하여야 한다
        /// </summary>
        public double PositionOffset
        {
            get; set;
        }

        /// <summary>
        /// 위치 보상에 대한 사양을 가져오거나 설정한다.
        /// </summary>
        //public CompensatingSpecification CompensatingSpecification
        //{
        //    get; set;
        //}

        /// <summary>
        /// 축 메뉴얼이동시 사용할 이동 거리에 대한 리스트를 가져오거나 설정한다.
        /// </summary>
        [Category("Trajectory")]
        public DoubleCollection StepList
        {
            get; set;
        }

        /// <summary>
        /// 축 메뉴얼 이동시 사용할 이동 속도에 대한 리스트를 가져오거나 설정한다.
        /// </summary>
        [Category("Trajectory")]
        public DoubleCollection VelocityList
        {
            get; set;
        }

        /// <summary>
        /// 빠른 응답을 요구하는 고속 축인지 여부를 가져오거나 설정한다.
        /// </summary>
        [Category("Trajectory")]
        [DefaultValue(false)]
        public bool HighSpeed
        {
            get; set;
        }

        /// <summary>
        /// SubPulse 계산없이 제어할지 여부를 가져온다.
        /// </summary>
        [Category("Trajectory")]
        [DefaultValue(false)]
        public bool WithoutSubPulse
        {
            get; set;
        }

        /// <summary>
        /// 가속도를 이동거리 기준으로 자동 변경할지 여부를 가져오거나 설정한다.
        /// </summary>
        [Category("Trajectory")]
        public bool AutomaticChangeAccelerationBasedOnDistance
        {
            get; set;
        }

        /// <summary>
        /// 이동거리 기준으로 변경할 가속도 테이블을 가져오거나 설정한다.
        /// </summary>
        //[Category("Trajectory")]
        //public TargetValueCollection AccelerationTableBasedOnDistance
        //{
        //    get; set;
        //}

        /// <summary>
        /// 감속도를 이동거리 기준으로 자동 변경할지 여부를 가져오거나 설정한다.
        /// </summary>
        [Category("Trajectory")]
        public bool AutomaticChangeDecelerationBasedOnDistance
        {
            get; set;
        }

        /// <summary>
        /// 이동거리 기준으로 변경할 감속도 테이블을 가져오거나 설정한다.
        /// </summary>
        //[Category("Trajectory")]
        //public TargetValueCollection DecelerationTableBaseOnDistance
        //{
        //    get; set;
        //}

        /// <summary>
        /// 속도를 이동거리 기준으로 자동 변경할지 여부를 가져오거나 설정한다.
        /// </summary>
        [Category("Trajectory")]
        public bool AutomaticChangeVelocityBasedOnDistance
        {
            get; set;
        }

        /// <summary>
        /// 이동거리 기준으로 변경할 속도 테이블을 가져오거나 설정한다.
        /// </summary>
        //[Category("Trajectory")]
        //public TargetValueCollection VelocityTableBaseOnDistance
        //{
        //    get
        //    {
        //        return m_VelocityTableBaseOnDistance;
        //    }
        //    set
        //    {
        //        m_VelocityTableBaseOnDistance = value;
        //    }
        //}

        /// <summary>
        ///
        /// </summary>
        //[Category("Antivibration")]
        //public AxisAntivibrationSpecification AntivibrationSpecification
        //{
        //    get
        //    {
        //        return m_AntivibrationSpecification;
        //    }
        //    set
        //    {
        //        m_AntivibrationSpecification = value;
        //    }
        //}

        /// <summary>
        /// 화면에 표시되는 JoyStick의 이동 방향을 반대로 할지 여부를 가져오거나 설정한다.
        /// 실제 구동 방향은 바뀌지 않는다.
        /// </summary>
        [DefaultValue(false)]
        [Category("Trajectory")]
        public bool InvertedJoyStick
        {
            get; set;
        }

        //public BacklashSpecification BacklashSpecification
        //{
        //    get
        //    {
        //        return m_BacklashSpecification;
        //    }
        //    set
        //    {
        //        m_BacklashSpecification = value;
        //    }
        //}

        #region AxisConfiguration Member
        //public new AjinAxlAxisConfigurationBody
        #endregion
        protected void SetDefaultValues()
        {
            if (HomingSpecification == null)
                this.HomingSpecification = new AjinAxlHomingSpecification();
            //this.GantrySpecification = new AjinAxlGantrySpecification();

            this.AccelerationJerk = 50;
            this.DecelerationJerk = 50;

            this.AccelUnit = (uint)AXM.AccelUnit.UnitPerSec2;
            this.PulsePerPosition = new Fraction(1000, 1);
            this.MotionType = AjinAxlMotionType.Trapezoidal;
            this.MaxVelocity = 100;
            this.Acceleration = 250;
            this.Deceleration = 250;
            this.Velocity = 50;
            this.Timeout = 5000;
        }

        public void Init()
        {
            if (this.PulsePerPosition == null)
                this.PulsePerPosition = new Fraction(1000, 1);
        }
    }
    #endregion

    #region AjinAxlGantrySpecification
    [Serializable]
    public class AjinAxlGantrySpecification : ExpandableObjectConverter
    {
        private bool m_Enabled;
        private int m_SlaveAxisNo;
        private bool m_SlaveOffsetVerified;
        private double m_SlaveOffset;
        private double m_SlaveOffsetRange;

        public AjinAxlGantrySpecification()
        {
            this.Enabled = false;
            this.SlaveAxisNo = 1;

            this.SlaveOffsetVerified = false;
            this.SlaveOffset = 0.0;
            this.SlaveOffsetRange = 10;
        }

        /// <summary>
        /// Gantry를 사용할지 여부를 가져오거나 설정한다.
        /// </summary>
        public bool Enabled
        {
            get { return this.m_Enabled; }
            set { this.m_Enabled = value; }
        }

        /// <summary>
        /// slave 축의 번호를 가져오거나 설정한다.
        /// </summary>
        public int SlaveAxisNo
        {
            get { return this.m_SlaveAxisNo; }
            set { this.m_SlaveAxisNo = value; }
        }

        /// <summary>
        /// 슬레이브 축의 오프셋이 확인되었는지 여부를 가져온다.
        /// </summary>
        public bool SlaveOffsetVerified
        {
            get { return this.m_SlaveOffsetVerified; }
            set { this.m_SlaveOffsetVerified = value; }
        }

        /// <summary>
        /// 슬레이브 축의 오프셋을 가져온다.
        /// </summary>
        public double SlaveOffset
        {
            get { return this.m_SlaveOffset; }
            set { this.m_SlaveOffset = value; }
        }

        /// <summary>
        /// 슬레이브 축의 허용할 최대 오프셋을 가져오거나 설정한다.
        /// </summary>
        public double SlaveOffsetRange
        {
            get { return this.m_SlaveOffsetRange; }
            set { this.m_SlaveOffsetRange = value; }
        }
    }
    #endregion

    #region AjinAxlAxisConfigurationCollection
    public class AjinAxlAxisConfigurationCollection : Collection<AjinAxlAxisConfiguration>
    {

    }
    #endregion
}
