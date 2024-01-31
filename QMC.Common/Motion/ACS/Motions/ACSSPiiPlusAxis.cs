using ACS.SPiiPlusNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Motion.ACS.Motions
{
    #region ACSSPiiPlusAxis
    public class ACSSPiiPlusAxis : MotionAxis
    {
        #region Define
        private enum StepState
        {
            None,
            Search,
            Escape,
            SearchPrecisely,
            SearchIndex,
            MoveToHome,
            Complete,
        }
        #endregion

        #region Field
        private StepState m_StepState;
        private StepState m_PreviousStepState;
        #endregion

        #region Constructor
        public ACSSPiiPlusAxis()
        {
            this.Motor = new MotorState(this.No.ToString());
        }
        #endregion

        #region MotionAxis Members
        protected override int OnHoming(HomingSpecification specification)
        {
            int ret = 0;
            ProgramStates m_nProgramState;

            ACSSPiiPlusMotionBoard.Api.RunBuffer((ProgramBuffer)this.No, null);
            StopWatch sw = StopWatch.StartNew();

            while (true)
            {
                m_nProgramState = ACSSPiiPlusMotionBoard.Api.GetProgramState((ProgramBuffer)this.No);

                if ((m_nProgramState & ProgramStates.ACSC_PST_RUN) == 0)
                {
                    break;
                }
                else if (sw.Elapsed.TotalMilliseconds > 60000)
                {
                    MessageBox.Show("Stage XY 축 초기화 실패", "Error");
                    break;
                }
            }

            return ret;
        }

        public override int SetActualPosition(double dPosition)
        {
            if (Simulated)
            {
                Motor.ActualPosition = dPosition;
                return 0;
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.SetRPosition((Axis)this.No, dPosition);
                return 0;
                //return AXM.SetActualPosition(this.No, dPosition);
            }
        }

        public override int SetCommandPosition(double dPosition)
        {
            if (Simulated)
            {
                Motor.CommandPosition = dPosition;
                return 0;
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.SetFPosition((Axis)this.No, dPosition);
                return 0;
                //return AXM.SetCommandPosition(this.No, dPosition);
            }
        }

        public override int GetActualPosition(ref double pulse)
        {
            int ret = 0;
            if (Simulated)
            {
                pulse = Motor.CommandPosition;
            }
            else
            {
                pulse = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)this.No);

                //if ((ret = AXM.GetActualPosition(this.No, ref pulse)) != 0) return ret;

                //pulse = Math.Round(pulse * this.Scale, 3);
            }

            return ret;
        }

        public override int GetAxisState(ref AxisState axisState)
        {
            int ret = 0;
            AxisStates state = AxisStates.ACSC_NONE;
            state = ACSSPiiPlusMotionBoard.Api.GetAxisState((Axis)this.No);

            switch (state)
            {
                case AxisStates.ACSC_NONE:
                    axisState = AxisState.Idle;
                    break;
                case AxisStates.ACSC_AST_LEAD:
                    break;
                case AxisStates.ACSC_AST_DC:
                    break;
                case AxisStates.ACSC_AST_PEG:
                    break;
                case AxisStates.ACSC_AST_PEGREADY:
                    break;
                case AxisStates.ACSC_AST_MOVE:
                    axisState = AxisState.Moving;
                    break;
                case AxisStates.ACSC_AST_ACC:
                    break;
                case AxisStates.ACSC_AST_SEGMENT:
                    break;
                case AxisStates.ACSC_AST_VELLOCK:
                    break;
                case AxisStates.ACSC_AST_POSLOCK:
                    break;
                case AxisStates.ACSC_ALL:
                    break;
                default:
                    break;
            }

            return ret;
        }

        public override int GetCommandPosition(ref double pulse)
        {
            int ret = 0;

            if (Simulated)
            {
                pulse = Motor.CommandPosition;
            }
            else
            {
                pulse = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)this.No);
            }

            return ret;
        }

        protected override int GetInPosition(ref bool value)
        {
            int ret = 0;

            MotorStates m_nMotorState;
            m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)this.No);

            // Returned value is integer, you need to use bitmaks 
            //if ((m_nMotorState & MotorStates.ACSC_MST_MOVE) != 0) lblMoving.Image = Properties.Resources.On; else lblMoving.Image = Properties.Resources.Off;
            if ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0) value = true ; else value = false;
            //if ((m_nMotorState & MotorStates.ACSC_MST_ACC) != 0) lblAcc.Image = Properties.Resources.On; else lblAcc.Image = Properties.Resources.Off;
            //if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0) bEnable = true; else bEnable = false;

            return ret;
        }

        protected override int GetMaxVelocity(ref double pulse)
        {
            int ret = 0;
            return ret;
        }

        protected override int GetMotionDone(ref bool done)
        {
            int ret = 0;
            return ret;
        }

        public override int GetAmpFault(ref bool bAmpFault)
        {
            int ret = 0;
            return ret;
        }

        protected override int GetPositionError(ref double pulse)
        {
            int ret = 0;
            return ret;
        }

        protected override int GetVelocity(ref double pulse)
        {
            int ret = 0;
            return ret;
        }

        protected override int SetMaxVelocity(double pulse)
        {
            int ret = 0;
            return ret;
        }

        public override int Stop(double dDeccel)
        {
            int ret = 0;
            return ret;
        }

        public override int StopEmergency()
        {
            int ret = 0;
            return ret;
        }

        public override int Stop()
        {
            int ret = 0;
            return ret;
        }

        public override int Reset()
        {
            int ret = 0;
            return ret;
        }

        public override int SetEnable(bool bEnable)
        {
            int ret = 0;

            //if ( bEnable)
            //{
            //    ACSSPiiPlusMotionBoard.Api.Enable((Axis)this.No);
            //}
            //else
            //{
            //    ACSSPiiPlusMotionBoard.Api.Disable((Axis)this.No);
            //}

            return ret;
        }

        public override int GetEnable(ref bool bEnable)
        {
            int ret = 0;

            MotorStates m_nMotorState;
            m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)this.No);

            // Returned value is integer, you need to use bitmaks 
            //if ((m_nMotorState & MotorStates.ACSC_MST_MOVE) != 0) lblMoving.Image = Properties.Resources.On; else lblMoving.Image = Properties.Resources.Off;
            //if ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0) lblInPos.Image = Properties.Resources.On; else lblInPos.Image = Properties.Resources.Off;
            //if ((m_nMotorState & MotorStates.ACSC_MST_ACC) != 0) lblAcc.Image = Properties.Resources.On; else lblAcc.Image = Properties.Resources.Off;
            if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)     bEnable = true ;    else bEnable = false ;

            return ret;
        }

        public override int GetIsNegativeLimit(ref bool bEnable)
        {
            int ret = 0;



            return ret;
        }

        public override int GetIsPositiveLimit(ref bool bEnable)
        {
            int ret = 0;
            return ret;
        }

        public override int GetIsHomeSensor(ref bool bEnable)
        {
            int ret = 0;
            return ret;
        }

        public override int MovePosition(double dPosition, double dVelocity, double dAccel, double dDeccel)
        {
            int ret = 0;
            if (Simulated)
            {
                if (m_Simulator == null)
                {
                    m_Simulator = new MotorSimulator();
                }
                m_Simulator.MotorState = Motor;
                ret = m_Simulator.MovePosition(dPosition, dVelocity, dAccel, dDeccel);
            }
            else
            {
                ACSSPiiPlusMotionBoard.Api.ToPoint(MotionFlags.ACSC_NONE, (Axis)this.No, dPosition);
                return ret;
            }


            return ret;
        }

        public override int MoveVelocity(double dVelocity, double dAccel, double dDeccel)
        {
            int ret = 0;
            return ret;
        }

        public override int Clear()
        {
            int ret = 0;
            return ret;
        }

        public override int ModifyPosition(double position, double velocity, double acceleration, double deceleration)
        {
            int ret = 0;
            return ret;
        }

        public override int ModifyVelocity(double velocity, double acceleration, double deceleration)
        {
            int ret = 0;
            return ret;
        }

        public override int WaitMotionDone(double dTimeout)
        {
            int ret = 0;

            DateTime start = DateTime.Now;
            Log.Write("MotionTime", string.Format($"Move WaitDone Start! Motion No : {this.No}"));
            if (Simulated == true)
            {

            }
            else
            {
                while (true)
                {
                    MotorStates m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)this.No);

                    // Returned value is integer, you need to use bitmaks 
                    if (((m_nMotorState & MotorStates.ACSC_MST_MOVE) == 0) && ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0))
                    {
                        Log.Write("MotionTime", string.Format($"Move WaitDone! Motion No : {this.No}"));
                        break;
                    }

                    if ((DateTime.Now - start).TotalMilliseconds > dTimeout)
                    {
                        break;
                    }
                }
            }
            Log.Write("MotionTime", string.Format($"Move WaitDone End! Motion No : {this.No}"));
            return ret;
        }

        public bool PosTolerance(int nAxis, double m_dTargetPos)
        {
            bool m_bRet = false;
            double m_dCurPos = 0.0;
            double m_dTol = 0.5;            //  Tolerance : +- 0.02mm
            
            m_dCurPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)nAxis);

            if ((m_dCurPos >= (m_dTargetPos - m_dTol)) &&
                (m_dCurPos <= (m_dTargetPos + m_dTol)))
                m_bRet = true;

            return m_bRet;
        }

        public override int Load(FileStream fs)
        {
            int ret = 0;

            Ajin.Motions.AjinAxlAxisConfiguration configuration = new Ajin.Motions.AjinAxlAxisConfiguration();
            ret = SaveManager.BinaryDeserialize<Ajin.Motions.AjinAxlAxisConfiguration>(fs, out configuration);
            configuration.Init();
            this.Configuration = configuration;

            if (configuration != null)
            {
                this.Motor.PositivePosition = Configuration.HomingSpecification.PositivePosition;
                this.Motor.NegativePosition = Configuration.HomingSpecification.NegativePosition;
                if (configuration.InvertedJoyStick == true && this.Direction == MotionDirection.Forward)
                    this.Direction = MotionDirection.Backward;
                else if (configuration.InvertedJoyStick == true && this.Direction == MotionDirection.Backward)
                    this.Direction = MotionDirection.Forward;
            }
            return ret;
        }
        #endregion

        #region Property

        #endregion
    }
    #endregion

    #region ACSSPiiPlusHomingSpecification
    /// <summary>
    /// AJIN에서 제공하는 Homing 함수를 사용하기 위해 프로퍼티를 추가함.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ACSSPiiPlusHomingSpecificationConverter))]

    public class ACSSPiiPlusHomingSpecification : HomingSpecification
    {
        #region Field
        private bool m_UseACSSPiiPlusFunction;
        private ACSSPiiPlusHomingParameter m_ACSSPiiPlusHomingParameter;
        #endregion

        #region Constructor
        public ACSSPiiPlusHomingSpecification() : base()
        {
            this.UseACSSPiiPlusFunction = false;
            this.ACSSPiiPlusHomingParameter = new ACSSPiiPlusHomingParameter();
            SetDefaultValues();
        }
        #endregion

        #region Property
        /// <summary>
        /// Homing을 Ajin AXL 함수를 사용할지 여부를 가져오거나 설정한다.
        /// </summary>
        public bool UseACSSPiiPlusFunction
        {
            get { return this.m_UseACSSPiiPlusFunction; }
            set { this.m_UseACSSPiiPlusFunction = value; }
        }

        public ACSSPiiPlusHomingParameter ACSSPiiPlusHomingParameter
        {
            get { return this.m_ACSSPiiPlusHomingParameter; }
            set { this.m_ACSSPiiPlusHomingParameter = value; }
        }
        #endregion

        #region Method
        protected void SetDefaultValues()
        {
            this.UseACSSPiiPlusFunction = true;
            this.ACSSPiiPlusHomingParameter.Direction = Directions.Ccw;
            this.ACSSPiiPlusHomingParameter.FirstSearchVelocity = 30;
            this.ACSSPiiPlusHomingParameter.FirstSearchAcc = 300;
            this.ACSSPiiPlusHomingParameter.HomeClearTime = 1500;
            this.ACSSPiiPlusHomingParameter.HomeSignal = HomeSignals.NegativeLimit;
            this.ACSSPiiPlusHomingParameter.IndexSearchVelocity = 1;
            this.ACSSPiiPlusHomingParameter.LastVelocity = 10;
            this.ACSSPiiPlusHomingParameter.SecondSearchVelocity = 10;
            this.ACSSPiiPlusHomingParameter.SecondSearchAcc = 100;
            this.ACSSPiiPlusHomingParameter.ZPhaseMethod = ZPhaseMethods.None;

            this.Acceleration = 100;
            this.Deceleration = 100;
            this.EnableIndexSearch = false;
            this.EnablePreciseSearch = false;
            this.EscapeDistance = 0.5;
            this.HomePosition = 0;
            this.Method = HomingMethod.NegativeSensor;
            this.NegativePosition = 0;
            this.PreciseSearchVelocityPercent = 10;
            this.Velocity = 10;
        }
        #endregion
    }
    #endregion

    #region ACSSPiiPlusHomingParameter
    /// <summary>
    /// AJIN에서 제공하는 Homing 함수를 사용하기 위해 프로퍼티를 추가함.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ACSSPiiPlusHomingParameterConverter))]
    public class ACSSPiiPlusHomingParameter
    {
        #region Define
        //[Serializable]
        //public enum Directions : int
        //{
        //    Ccw = 0,
        //    Cw = 1,
        //}

        //[Serializable]
        //public enum ZPhaseMethods : uint
        //{
        //    None,
        //    Cw = 1,
        //    Ccw = 2,
        //}

        //[Serializable]
        //public enum HomeSignals : uint
        //{
        //    PositiveLimit = 0,
        //    NegativeLimit = 1,
        //    HomeSensor = 4,
        //    ZPhase = 5,
        //}
        #endregion

        #region Field
        private Directions m_Direction;
        private ZPhaseMethods m_ZPhaseMethod;
        private HomeSignals m_HomeSignal;
        private double m_HomeClearTime;
        private double m_FirstSearchVelocity;
        private double m_SecondSearchVelocity;
        private double m_LastVelocity;
        private double m_IndexSearchVelocity;
        private double m_FirstSearchAcc;
        private double m_SecondSearchAcc;
        #endregion

        #region Constructor
        public ACSSPiiPlusHomingParameter()
        {
            this.Direction = Directions.Ccw;
            this.ZPhaseMethod = ZPhaseMethods.None;
            this.HomeSignal = HomeSignals.NegativeLimit;
            this.HomeClearTime = 100;
            this.FirstSearchVelocity = 100;
            this.SecondSearchVelocity = 30;
            this.LastVelocity = 10;
            this.IndexSearchVelocity = 1;
            this.FirstSearchAcc = 1000;
            this.SecondSearchAcc = 300;
        }
        #endregion

        #region Property
        public Directions Direction
        {
            get { return this.m_Direction; }
            set { this.m_Direction = value; }
        }

        public ZPhaseMethods ZPhaseMethod
        {
            get { return this.m_ZPhaseMethod; }
            set { this.m_ZPhaseMethod = value; }
        }

        public HomeSignals HomeSignal
        {
            get { return this.m_HomeSignal; }
            set { this.m_HomeSignal = value; }
        }

        public double HomeClearTime
        {
            get { return this.m_HomeClearTime; }
            set { this.m_HomeClearTime = value; }
        }

        public double FirstSearchVelocity
        {
            get { return this.m_FirstSearchVelocity; }
            set { this.m_FirstSearchVelocity = value; }
        }

        public double SecondSearchVelocity
        {
            get { return this.m_SecondSearchVelocity; }
            set { this.m_SecondSearchVelocity = value; }
        }

        public double LastVelocity
        {
            get { return this.m_LastVelocity; }
            set { this.m_LastVelocity = value; }
        }

        public double IndexSearchVelocity
        {
            get { return this.m_IndexSearchVelocity; }
            set { this.m_IndexSearchVelocity = value; }
        }

        public double FirstSearchAcc
        {
            get { return this.m_FirstSearchAcc; }
            set { this.m_FirstSearchAcc = value; }
        }

        public double SecondSearchAcc
        {
            get { return this.m_SecondSearchAcc; }
            set { this.m_SecondSearchAcc = value; }
        }
        #endregion
    }
    #endregion

    #region ACSSPiiPlusHomingParameterConverter
    [Serializable]
    internal class ACSSPiiPlusHomingParameterConverter : ExpandableObjectConverter
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
                ACSSPiiPlusHomingParameter parameter = new ACSSPiiPlusHomingParameter();
                parameter.Direction = (Directions)Enum.Parse(typeof(Directions), token[0]);
                parameter.ZPhaseMethod = (ZPhaseMethods)Enum.Parse(typeof(ZPhaseMethods), token[1]);
                parameter.HomeSignal = (HomeSignals)Enum.Parse(typeof(HomeSignals), token[2]);
                parameter.HomeClearTime = double.Parse(token[3]);
                parameter.FirstSearchVelocity = double.Parse(token[4]);
                parameter.SecondSearchVelocity = double.Parse(token[5]);
                parameter.LastVelocity = double.Parse(token[6]);
                parameter.IndexSearchVelocity = double.Parse(token[7]);
                parameter.FirstSearchAcc = double.Parse(token[8]);
                parameter.SecondSearchAcc = double.Parse(token[9]);

                return parameter;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is ACSSPiiPlusHomingParameter)
            {
                ACSSPiiPlusHomingParameter parameter = value as ACSSPiiPlusHomingParameter;
                return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    parameter.Direction, parameter.ZPhaseMethod, parameter.HomeSignal,
                    parameter.HomeClearTime, parameter.FirstSearchVelocity, parameter.SecondSearchVelocity,
                    parameter.LastVelocity, parameter.IndexSearchVelocity,
                    parameter.FirstSearchAcc, parameter.SecondSearchAcc);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion

    #region ACSSPiiPlusHomingSpecificationConverter
    [Serializable]
    internal class ACSSPiiPlusHomingSpecificationConverter : ExpandableObjectConverter
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
                ACSSPiiPlusHomingSpecification specification = new ACSSPiiPlusHomingSpecification();
                specification.Velocity = double.Parse(token[0]);
                specification.Acceleration = double.Parse(token[1]);
                specification.Deceleration = double.Parse(token[2]);
                specification.NegativePosition = double.Parse(token[3]);
                specification.PositivePosition = double.Parse(token[4]);
                specification.EscapeDistance = double.Parse(token[5]);
                specification.HomePosition = double.Parse(token[6]);
                specification.Method = (HomingMethod)Enum.Parse(typeof(HomingMethod), token[7]);
                specification.EnablePreciseSearch = bool.Parse(token[8]);
                specification.PreciseSearchVelocityPercent = int.Parse(token[9]);
                specification.EnableIndexSearch = bool.Parse(token[10]);
                specification.UseACSSPiiPlusFunction = bool.Parse(token[11]);

                return specification;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is ACSSPiiPlusHomingSpecification)
            {
                ACSSPiiPlusHomingSpecification specification = value as ACSSPiiPlusHomingSpecification;
                return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                    specification.Velocity, specification.Acceleration, specification.Deceleration,
                    specification.NegativePosition, specification.PositivePosition, specification.EscapeDistance,
                    specification.HomePosition, specification.Method,
                    specification.EnablePreciseSearch, specification.PreciseSearchVelocityPercent, specification.EnableIndexSearch, specification.UseACSSPiiPlusFunction);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion

    #region ACSSPiiPlusGantrySpecification
    /// <summary>
    /// 아진 AXL Library에서 제공하는 gantry 설정을 정의한다.
    /// </summary>
    //[Serializable]
    //[TypeConverter(typeof(ACSSPiiPlusGantrySpecificationConverter))]
    //public class ACSSPiiPlusGantrySpecification
    //{
    //    private bool m_Enabled;
    //    private int m_SlaveAxisNo;
    //    private bool m_SlaveOffsetVerified;
    //    private double m_SlaveOffset;
    //    private double m_SlaveOffsetRange;

    //    public ACSSPiiPlusGantrySpecification()
    //    {
    //        this.Enabled = false;
    //        this.SlaveAxisNo = 1;

    //        this.SlaveOffsetVerified = false;
    //        this.SlaveOffset = 0.0;
    //        this.SlaveOffsetRange = 10;
    //    }

    //    /// <summary>
    //    /// Gantry를 사용할지 여부를 가져오거나 설정한다.
    //    /// </summary>
    //    public bool Enabled
    //    {
    //        get { return this.m_Enabled; }
    //        set { this.m_Enabled = value; }
    //    }

    //    /// <summary>
    //    /// slave 축의 번호를 가져오거나 설정한다.
    //    /// </summary>
    //    public int SlaveAxisNo
    //    {
    //        get { return this.m_SlaveAxisNo; }
    //        set { this.m_SlaveAxisNo = value; }
    //    }

    //    /// <summary>
    //    /// 슬레이브 축의 오프셋이 확인되었는지 여부를 가져온다.
    //    /// </summary>
    //    public bool SlaveOffsetVerified
    //    {
    //        get { return this.m_SlaveOffsetVerified; }
    //        set { this.m_SlaveOffsetVerified = value; }
    //    }

    //    /// <summary>
    //    /// 슬레이브 축의 오프셋을 가져온다.
    //    /// </summary>
    //    public double SlaveOffset
    //    {
    //        get { return this.m_SlaveOffset; }
    //        set { this.m_SlaveOffset = value; }
    //    }

    //    /// <summary>
    //    /// 슬레이브 축의 허용할 최대 오프셋을 가져오거나 설정한다.
    //    /// </summary>
    //    public double SlaveOffsetRange
    //    {
    //        get { return this.m_SlaveOffsetRange; }
    //        set { this.m_SlaveOffsetRange = value; }
    //    }
    //}
    #endregion

    #region ACSSPiiPlusGantrySpecificationConverter
    [Serializable]
    internal class ACSSPiiPlusGantrySpecificationConverter : ExpandableObjectConverter
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
                ACSSPiiPlusGantrySpecification specification = new ACSSPiiPlusGantrySpecification();
                specification.Enabled = bool.Parse(token[0]);
                specification.SlaveAxisNo = int.Parse(token[1]);
                specification.SlaveOffsetVerified = bool.Parse(token[2]);
                specification.SlaveOffset = double.Parse(token[3]);
                specification.SlaveOffsetRange = double.Parse(token[4]);

                return specification;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is ACSSPiiPlusGantrySpecification)
            {
                ACSSPiiPlusGantrySpecification specification = value as ACSSPiiPlusGantrySpecification;
                return string.Format("{0}, {1}, {2}, {3}, {4}",
                    specification.Enabled, specification.SlaveAxisNo, specification.SlaveOffsetVerified, specification.SlaveOffset, specification.SlaveOffsetRange);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion
}
