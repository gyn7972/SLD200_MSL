using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common
{
    public delegate int BeforeMove(MotionAxis axis, double dPosition);
    public delegate int MoveInterpolation(MotionAxis axis, double dPosition, int nVelPercent);
    public delegate int StopJogVelocity(MotionAxis axis, int nVelPercent);
    public delegate int GetAcutualInterpolationPosition(MotionAxis axis, ref double dPosition);
    public delegate int GetCommandInterpolationPosition(MotionAxis axis, ref double dPosition);

    [Serializable]
    public abstract class MotionAxis : IActor
    {
        #region Define
        public enum FunctionID
        {
            Home,
            MovePosition,
            MoveDistance,
            MoveVelocity,
            WaitMoveDone,
            Count
        }

        public event BeforeMove OnBeforeMove;
        public event MoveInterpolation OnMoveInterpolation;
        public event StopJogVelocity OnStopJogVelocity;
        public event GetAcutualInterpolationPosition OnGetAcutualInterpolationPosition;
        public event GetCommandInterpolationPosition OnGetCommandInterpolationPosition;

        #endregion

        #region Field
        private AxisState m_SimulatedAxisState;
        [NonSerialized]
        protected Thread m_MoniteringThread;
        [NonSerialized]
        protected MotorSimulator m_Simulator;
        protected bool m_bExit;
        #endregion

        #region Property
        public MotionBoard Board { set; get; }
        public int BoardNo => Board.Configuration.No;
        public HomingState HomingState { set; get; }
        public MotionAxisConfiguration Configuration { set; get; }
        public double Offset { set; get; }
        public string Name
        {
            set
            {
                Configuration.Name = value;
            }
            get
            {
                return Configuration.Name;
            }

        }
        public string Description => Configuration.Description;
        public bool Simulated => Configuration.Simulated;
        public int No => Configuration.No;
        //public int AxisNo => Configuration.AxisNo;
        public int Step { set; get; }
        public int Velocity { set; get; }
        public string Part
        {
            set
            {
                Configuration.PartUid = value;
            }
            get
            {
                return Configuration.PartUid;
            }

        }
        public string Module
        {
            set
            {
                Configuration.ModuleUid = value;
            }
            get
            {
                return Configuration.ModuleUid;
            }
        }

        public string Tag
        {
            set
            {
                Configuration.Tag = value;
            }
            get
            {
                return Configuration.Tag;
            }
        }
        public uint UID
        {
            set
            {
                Configuration.UID = value;
            }
            get
            {
                return Configuration.UID;
            }

        }

        public MotionDirection Direction { set; get; }

        public MotorState Motor { set; get; }

        public double Scale
        {
            get { return Configuration.PositionPerPulse; }
        }

        public bool IsDelete { set; get; }
        #endregion


        #region Constructor
        public MotionAxis()
        {
            m_Simulator = new MotorSimulator();
            if (Configuration == null)
                Configuration = new MotionAxisConfiguration();
            HomingState = HomingState.None;
            m_SimulatedAxisState = AxisState.Idle;
            Step = 1;
            Velocity = 10;
            Direction = MotionDirection.Forward;
        }
        #endregion

        #region Method
        public void Homing()
        {
            Thread thread = new Thread(() =>
            {
                HomingProcedure(this.Configuration.HomingSpecification);
            });
            thread.Start();

        }
        public int HomeSync()
        {
            return HomingProcedure(this.Configuration.HomingSpecification);
        }
        private int HomingProcedure(HomingSpecification specification)
        {
            int num = 0;
            double num2 = 0.0;
            HomingState = HomingState.Homing;
            if (Simulated)
            {
                num2 = Configuration.HomingSpecification.HomePosition;
                //PositionConverter를 써서 변환해줬음.
                if ((num = SetActualPosition(num2)) != 0)
                {
                    return num;
                }
                if ((num = SetCommandPosition(num2)) != 0)
                {
                    return num;
                }
                //if ((num = Motor.NegativePosition.Reset()) != 0)
                //{
                //	return num;
                //}
                //if ((num = Motor.PositivePosition.Reset()) != 0)
                //{
                //	return num;
                //}
                m_SimulatedAxisState = AxisState.Idle;
            }
            else
            {
                num = OnHoming(specification);
            }
            if (num == 0)
            {
                HomingState = HomingState.Completed;
            }
            else
            {
                HomingState = HomingState.None;
            }

            return num;
        }

        protected abstract int OnHoming(HomingSpecification specification);
        public abstract int SetActualPosition(double dPosition);
        public abstract int SetCommandPosition(double dPosition);

        public abstract int GetActualPosition(ref double pulse);
        public abstract int GetAxisState(ref AxisState axisState);
        public abstract int GetCommandPosition(ref double pulse);
        protected abstract int GetInPosition(ref bool value);
        protected abstract int GetMaxVelocity(ref double pulse);
        protected abstract int GetMotionDone(ref bool done);
        public abstract int GetAmpFault(ref bool bAmpFault);
        protected abstract int GetPositionError(ref double pulse);
        protected abstract int GetVelocity(ref double pulse);

        protected abstract int SetMaxVelocity(double pulse);
        public abstract int Stop(double dDeccel);
        public abstract int StopEmergency();
        public abstract int Stop();
        public abstract int Reset();
        public abstract int SetEnable(bool bEnable);
        public abstract int GetEnable(ref bool bEnable);
        public abstract int GetIsNegativeLimit(ref bool bEnable);
        public abstract int GetIsPositiveLimit(ref bool bEnable);
        public abstract int GetIsHomeSensor(ref bool bEnable);
        public abstract int MovePosition(double dPosition, double dVelocity, double dAccel, double dDeccel);
        public abstract int MoveVelocity(double dVelocity, double dAccel, double dDeccel);
        public abstract int Clear();

        public abstract int ModifyPosition(double position, double velocity, double acceleration, double deceleration);
        public abstract int ModifyVelocity(double velocity, double acceleration, double deceleration);

        public abstract int WaitMotionDone(double dTimeout);

        public virtual void Open()
        {


        }
        public int SetPosition(double dPosition)
        {
            int ret = 0;

            if ((ret = SetActualPosition(dPosition)) != 0) return ret;
            if ((ret = SetCommandPosition(dPosition)) != 0) return ret;

            return ret;
        }
        public int MovePosition(double dPosition)
        {
            if (OnBeforeMove != null)
            {
                int ret = OnBeforeMove(this, dPosition);
                if (ret != 0) return ret;
            }
            return MovePosition(dPosition, this.Configuration.Velocity, this.Configuration.Acceleration, this.Configuration.Deceleration);
        }

        public int MovePosition(double dPosition, int nVelPercent)
        {
            if (OnBeforeMove != null)
            {
                int ret = OnBeforeMove(this, dPosition);
                if (ret != 0) return ret;
            }

            if (nVelPercent == 0)
                nVelPercent = 1;

            return MovePosition(dPosition, this.Configuration.Velocity * nVelPercent / 100, this.Configuration.Acceleration * nVelPercent / 100, this.Configuration.Deceleration * nVelPercent / 100);
        }

        public int MoveVelocity(double dVelocity)
        {
            if (OnBeforeMove != null)
            {
                double dPosition = 0;
                GetActualPosition(ref dPosition);
                int ret = OnBeforeMove(this, dPosition);
                if (ret != 0) return ret;
            }

            return MoveVelocity(dVelocity, dVelocity * 5, dVelocity * 5);
        }

        public int MoveVelocity()
        {
            if (OnBeforeMove != null)
            {
                double dPosition = 0;
                GetActualPosition(ref dPosition);
                int ret = OnBeforeMove(this, dPosition);
                if (ret != 0) return ret;
            }

            return MoveVelocity(this.Configuration.Velocity, this.Configuration.Acceleration, this.Configuration.Deceleration);
        }

        public int MoveDistance(double dDistance, double dVelocity, double dAccel, double dDeccel)
        {
            double dCurrent = 0;
            GetCommandPosition(ref dCurrent);
            if (OnBeforeMove != null)
            {
                int ret = OnBeforeMove(this, dCurrent + dDistance);
                if (ret != 0) return ret;
            }

            return MovePosition(dCurrent + dDistance, dVelocity, dAccel, dDeccel);
        }

        public int MoveDistance(double dDistance)
        {
            double dCurrent = 0;
            GetCommandPosition(ref dCurrent);
            if (OnBeforeMove != null)
            {
                int ret = OnBeforeMove(this, dCurrent + dDistance);
                if (ret != 0) return ret;
            }
            return MovePosition(dCurrent + dDistance);
        }

        public int GetCurrentActualPosition(ref double dCurrent)
        {
            int ret = 0;

            if (OnGetAcutualInterpolationPosition != null)
            {
                OnGetAcutualInterpolationPosition(this, ref dCurrent);
            }

            return ret;
        }
        public int GetCurrentCommandPosition(ref double dCurrent)
        {
            int ret = 0;

            if (OnGetCommandInterpolationPosition != null)
            {
                OnGetCommandInterpolationPosition(this, ref dCurrent);
            }


            return ret;
        }

        public int StopJogVelocity(int nVelPercent)
        {
            int ret = 0;

            if (OnStopJogVelocity != null)
            {
                ret = OnStopJogVelocity(this, nVelPercent);
            }

            ret = Stop();

            return ret;
        }
        public int MoveJogDistance(double dDistance, int nVelPercent)
        {
            int ret = 0;
            if (OnMoveInterpolation != null)
            {
                double dPos = 0;
                GetCurrentActualPosition(ref dPos);
                //GetCurrentCommandPosition(ref dPos);			//	2023. 05. 24.  SCH : Actual 을 EncPos 가 아니라 CmdPos 로 해야 한다고 함. -> ACS 는 그런 듯. 아진은 걍 Actual 로 해도 될 듯
                ret = OnMoveInterpolation(this, dPos + dDistance, nVelPercent);
            }
            return ret;
        }

        public virtual void Close()
        {

        }


        public int Save(FileStream fs)
        {
            int ret = 0;

            ret = SaveManager.BinarySerialize(fs, this.Configuration);

            return ret;
        }

        public abstract int Load(FileStream fs);



        public int Execute(uint nID, SettingParameterCollection parameter)
        {
            int ret = 0;
            switch ((FunctionID)nID)
            {
                case FunctionID.Home:
                    HomeSync();
                    break;
                case FunctionID.MovePosition:
                    ret = MovePosition(parameter[0].DoubleValue + Offset, parameter[1].DoubleValue, parameter[2].DoubleValue, parameter[3].DoubleValue);
                    Console.WriteLine("{0} : MovePosition", Name);
                    break;
                case FunctionID.MoveDistance:
                    ret = MoveDistance(parameter[0].DoubleValue + Offset, parameter[1].DoubleValue, parameter[2].DoubleValue, parameter[3].DoubleValue);
                    break;
                case FunctionID.MoveVelocity:
                    ret = MoveVelocity(parameter[0].DoubleValue, parameter[1].DoubleValue, parameter[2].DoubleValue);
                    break;
                case FunctionID.WaitMoveDone:
                    ret = WaitMotionDone(parameter[0].DoubleValue);
                    Console.WriteLine("{0} : WaitMoveDone", Name);
                    break;
                default:
                    ret = -1;
                    break;
            }
            ret = 0;



            return ret;
        }

        public SettingParameterCollection GetParameters(uint nID)
        {
            SettingParameterCollection parameters = new SettingParameterCollection();
            switch ((FunctionID)nID)
            {
                case FunctionID.Home:
                    break;
                case FunctionID.MovePosition:
                    parameters.Add(new SettingParameter("Position", DataType.Double, ParameterType.Position, this));
                    parameters.Add(new SettingParameter("Velocity", DataType.Double, ParameterType.Velocity, this));
                    parameters.Add(new SettingParameter("Acceleration", DataType.Double, ParameterType.Acceleration, this));
                    parameters.Add(new SettingParameter("Deceleration", DataType.Double, ParameterType.Deceleration, this));
                    break;
                case FunctionID.MoveDistance:
                    parameters.Add(new SettingParameter("Position", DataType.Double, ParameterType.Position, this));
                    parameters.Add(new SettingParameter("Velocity", DataType.Double, ParameterType.Velocity, this));
                    parameters.Add(new SettingParameter("Acceleration", DataType.Double, ParameterType.Acceleration, this));
                    parameters.Add(new SettingParameter("Deceleration", DataType.Double, ParameterType.Deceleration, this));
                    break;
                case FunctionID.MoveVelocity:
                    parameters.Add(new SettingParameter("Velocity", DataType.Double, ParameterType.Velocity, this));
                    parameters.Add(new SettingParameter("Acceleration", DataType.Double, ParameterType.Acceleration, this));
                    parameters.Add(new SettingParameter("Deceleration", DataType.Double, ParameterType.Deceleration, this));
                    break;
                case FunctionID.WaitMoveDone:
                    parameters.Add(new SettingParameter("Timeout", DataType.Double, ParameterType.Time, this));
                    break;
                default:
                    break;
            }

            return parameters;
        }

        public SettingParameter GetActionParameter(uint nID, int nNo)
        {
            SettingParameter parameter = null;
            SettingParameterCollection parameters = GetParameters(nID);
            if (parameters != null && nNo < parameters.Count)
            {
                parameter = parameters[nNo];
            }

            return parameter;
        }

        public MovingProjection GetDefaultMovingProjection()
        {
            MovingProjection projection = new MovingProjection();
            double dPosition = 0;
            this.GetCommandPosition(ref dPosition);
            projection.Position = dPosition;
            projection.Velocity = Configuration.Velocity;
            projection.Acceleration = Configuration.Acceleration;
            projection.Deceleration = Configuration.Deceleration;
            projection.Timeout = Configuration.Timeout;
            return projection;
        }

        //public Function GetFunction(uint nID)
        //      {
        //	Function func = new Function();
        //	func.ID = nID;
        //	func.Name = ((FunctionID)nID).ToString();

        //	return func;
        //}

        public int GetFunctionCount()
        {
            return (int)FunctionID.Count;
        }

        public int Initialize()
        {
            int ret = 0;
            ret = HomeSync();
            return ret;
        }

        public int StopExecute()
        {
            return Stop();
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }

    [Serializable]
    public class MotionAxisConfiguration
    {
        public DisplayAxisType DisplayAxisType { set; get; }

        [ReadOnly(true)]
        public uint UID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string PartUid { set; get; }
        public bool Simulated { set; get; }
        public string Tag { set; get; }
        public string ModuleUid { set; get; }
        public int BoardNo { get; set; }

        public MotionBoardType BoardType { set; get; }
        /// <summary>
        /// Gets or sets the default velocity
        /// </summary>
        [Category("Trajectory")]
        public double Velocity
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the default acceleration
        /// </summary>
        [Category("Trajectory")]
        public double Acceleration
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the default deceleration
        /// </summary>
        [Category("Trajectory")]
        public double Deceleration
        {
            get; set;
        }

        /// <summary>
        /// 연결된 파트의 UID를 가져오거나 설정합니다.
        /// </summary>
        //[Browsable(false)]
        //public string PartUid
        //{
        //    get; set;
        //}

        /// <summary>
        /// Gets or Sets the amount of pulse per position.
        /// </summary>
        public Fraction PulsePerPosition
        {
            get; set;
        }

        /// <summary>
        /// Gets the amount of unit per pulse.
        /// </summary>
        [Browsable(false)]
        public Fraction PositionPerPulse => PulsePerPosition.Reverse();

        public int No
        { get; set; }

        public int Timeout
        { get; set; }
        //public int AxisNo
        //{ get; set; }

        [Category("Homing")]
        [TypeConverter(typeof(HomingSpecification))]
        public HomingSpecification HomingSpecification { set; get; }
    }

    [Serializable]
    public class MotionAxisConfigurationCollection : Collection<MotionAxisConfiguration>
    {

    }
}
