/*
 * Purpose
 * 
 * Revision
 * 
 */

using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QMC.Common.Motion.Ajin.Motions
{
    #region AjinAxlAxis
    [Serializable]
    public class AjinAxlAxis : MotionAxis
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
        public AjinAxlAxis() : base()
        {
            this.Configuration = new AjinAxlAxisConfiguration();
            this.Motor = new MotorState(this.No.ToString());
        }
        #endregion

        #region Property
        
        #endregion

        #region Method

        public override void Open()
        {
            base.Open();
            
            if (Configuration.Simulated == true)
            {
                if (m_Simulator == null)
                    m_Simulator = new MotorSimulator();
                m_Simulator.MotorState = Motor;
            }
            else
            {
                //m_MoniteringThread = new Thread(new ParameterizedThreadStart(DoMoniter));
                //m_MoniteringThread.Start(this);
                //m_bExit = false;
            }
        }

        public override void Close()
        {
            base.Close();

            if (m_MoniteringThread != null)
            {
                m_bExit = true;
                m_MoniteringThread.Join();
            }
        }

        protected static void DoMoniter(object obj)
        {
            AjinAxlAxis owner = obj as AjinAxlAxis;
            if (owner != null)
            {
                owner.OnMoniterProc();
            }
        }

        protected void OnMoniterProc()
        {
            AxisState state = AxisState.Idle;
            double dValue = 0;
            bool bValue = false;

            while (true)
            {
                if (m_bExit)
                    break;

                //if (Board != null && Board.IsOpen)
                //{

                //    if (GetEnable(ref bValue) != 0) continue;
                //    this.Motor.IsAmpEnable = bValue;

                //    if (GetAxisState(ref state) != 0) continue;
                //    this.Motor.MotionState = state;

                //    if (GetMotionDone(ref bValue) != 0) continue;
                //    this.Motor.IsMotionDone = bValue;

                //    if (GetInPosition(ref bValue) != 0) continue;
                //    this.Motor.IsInPosition = bValue;

                //    if (GetCommandPosition(ref dValue) != 0) continue;
                //    this.Motor.CommandPosition = dValue;

                //    if (GetActualPosition(ref dValue) != 0) continue;
                //    this.Motor.ActualPosition = Math.Round(dValue * this.Scale, 3);

                //    if (GetAmpFault(ref bValue) != 0) continue;
                //    this.Motor.IsAmpFault = bValue;

                //    if (GetIsNegativeLimit(ref bValue) != 0) continue;
                //    this.Motor.IsNegativeLimit = bValue;

                //    if (GetIsPositiveLimit(ref bValue) != 0) continue;
                //    this.Motor.IsPositiveLimit = bValue;

                //    if (GetIsHomeSensor(ref bValue) != 0) continue;
                //    this.Motor.IsHome = bValue;

                //}

                Thread.Sleep(100);
            }
        }

        private void SetStepState(StepState state)
        {
            this.m_PreviousStepState = this.m_StepState;
            this.m_StepState = state;

            //AXL.WriteLog(new LogEntry(LogLevel.Normal, string.Format("StepChanged {0} -> {1}", this.m_PreviousStepState, this.m_StepState)));
        }

        public override int WaitMotionDone(double dTimeOut)
        {
            return this.WaitMotionDone(true, dTimeOut);
        }

        private int WaitMotionDone(bool checkStatus, double dTimeOut)
        {
            int ret = 0;
            bool done = false;
            bool ampFaultValue = false;
            DateTime startTime = DateTime.Now;
            TimeSpan current;
            while (true)
            {
                Thread.Sleep(1);
                if(Simulated)
                {
                    if (Motor.ActualPosition == Motor.CommandPosition)
                    {
                        break;
                    }
                }
                else
                {
                    if ((ret = this.GetMotionDone(ref done)) != 0) return ret;
                    if (checkStatus == true)
                    {
                        if ((ret = this.GetAmpFault(ref ampFaultValue)) != 0) return ret;
                        if (ampFaultValue)
                        {
                            //this.WriteLog(LogLevel.Highest, "AjinAxis.WaitMotionDone() AmpFaultDetected");
                            Console.WriteLine("AjinAxis.WaitMotionDone() AmpFaultDetected");
                            return -1;
                        }
                    }
                    if (done) break;
                }
                

                current = DateTime.Now - startTime;
                if (dTimeOut > 0 && current.TotalMilliseconds > dTimeOut)
                {
                    Console.WriteLine("AjinAxis.WaitMotionDone() Timeout");
                    return -1;
                }
            }

            return ret;
        }
        public void GetAccelDecelTime(double currentPosition, double dPosition, ref TimeSpan accelTime, ref TimeSpan decelTime, ref TimeSpan velocityTime)
        {
            double num = Math.Abs(dPosition - currentPosition);
            double num2 = 0.0;
            double num3 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;

            double dVelocity = this.Configuration.Velocity;
            double dAcceleration = this.Configuration.Acceleration;
            double dDeceleration = this.Configuration.Deceleration;

            int num9;
            if (dVelocity == 0.0 && dAcceleration != 0.0)
            {
                num9 = ((dDeceleration == 0.0) ? 1 : 0);
            }
            else
            {
                num9 = 1;
            }
            if (num9 != 0)
            {
                throw new ArgumentOutOfRangeException("Velocity, Acceleration, Deceleration");
             
            }
            if (num == 0.0)
            {
                accelTime = TimeSpan.FromSeconds(0.0);
                decelTime = TimeSpan.FromSeconds(0.0);
                return;
            }
            num5 = dVelocity / dAcceleration;
            num6 = dVelocity / dDeceleration;
            num2 = dVelocity * num5 / 2.0;
            num3 = dVelocity * num6 / 2.0;
            if (num < num2 + num3)
            {
                num2 = num * (dDeceleration / (dAcceleration + dDeceleration));
                num3 = num * (dAcceleration / (dAcceleration + dDeceleration));
                num5 = Math.Sqrt(num2 * 2.0 / dAcceleration);
                num6 = Math.Sqrt(num3 * 2.0 / dDeceleration);
                num7 = 0.0;
            }
            else
            {
                num7 = (num - (num2 + num3)) / dVelocity;
            }
            accelTime = TimeSpan.FromSeconds(num5);
            decelTime = TimeSpan.FromSeconds(num6);
            velocityTime = TimeSpan.FromSeconds(num7);
        }

        #region homing related
        protected override int OnHoming(HomingSpecification specification)
        {
            int ret = 0;
            AjinAxlHomingSpecification specialized = specification as AjinAxlHomingSpecification;

            this.SetStepState(StepState.None);

            if (specialized.UseAjinAxlFunction == true)
            {
                AXT_MOTION_HOME_RESULT result = AXT_MOTION_HOME_RESULT.HOME_SUCCESS;

                #region 임시 Homing
                AXM.SetAccelerationJerk(this.No, 5000.0);
                AXM.SetDecelerationJerk(this.No, 5000.0);
                
                AXM.SetHomeStart(this.No);

                while (true)
                {
                    Thread.Sleep(10);
                    if ((ret = AXM.GetHomeResult(this.No, ref result)) != 0) return ret;

                    if (result == AXT_MOTION_HOME_RESULT.HOME_SUCCESS) break;
                    else if (result == AXT_MOTION_HOME_RESULT.HOME_SEARCHING) continue;
                    else if (result == AXT_MOTION_HOME_RESULT.HOME_ERR_USER_BREAK) return 1;
                    else return -1;

                    //this.WriteLog(LogLevel.Normal, string.Format("ExecuteAjinHomingLib() return {0}", result.ToString()));
                    Console.WriteLine("ExecuteAjinHomingLib() return {0}", result.ToString());
                    //if ((ret = ErrorManager.Register(result.ToString())) != 0) return ret;
                }
                #endregion

                //if ((ret = this.ExecuteAjinHomingLib(specialized)) != 0) return ret;
            }
            else
            {
                while (this.m_StepState != StepState.Complete)
                {
                    switch (this.m_StepState)
                    {
                        case StepState.None:
                            if ((ret = this.OnNone(specification)) != 0) return ret;
                            break;
                        case StepState.Search:
                            if ((ret = this.OnSearch(specification)) != 0) return ret;
                            break;
                        case StepState.Escape:
                            if ((ret = this.OnEscape(specification)) != 0) return ret;
                            break;
                        case StepState.SearchPrecisely:
                            if ((ret = this.OnSearchPrecisely(specification)) != 0) return ret;
                            break;
                        case StepState.SearchIndex:
                            if ((ret = this.OnSearchIndex(specification)) != 0) return ret;
                            break;
                        case StepState.MoveToHome:
                            if ((ret = this.OnMoveToHome(specification)) != 0) return ret;
                            break;
                        default:
                            break;
                    }
                }
            }

            return ret;
        }
        private int OnNone(HomingSpecification specification)
        {
            int ret = 0;
            bool bValue = false;
            DioValue sensorDetected = DioValue.Off;
            double position = 0.0;

            // Revison 6
            if ((ret = this.Reset()) != 0) return ret;
            if ((ret = this.GetAmpFault(ref bValue)) != 0) return ret;
            if (bValue)
            {
                if ((ret = this.Reset()) != 0) return ret;
            }
            
            if ((ret = this.Reset()) != 0) return ret;

            //// network type이기 때문에 off이후에 대기하여야 한다.
            //if (this.Board.Configuration.BoardType == AjinAxlMotionBoardTypes.PcieMechatrolinkIII)
            //    SafeThread.Sleep(1000);
            // AmpEnable에 설정된 delay는 AmpEnable.SetEnabled(true)인 경우만 유효하다

            //Thread.Sleep(this.Configuration.AmpEnableDelay);

            if ((ret = this.Reset()) != 0) return ret;

            // Homing시 이동 범위를 설정한다. 범위 안에서 찾지 못한 경우 다음 step으로 진행된다.
            // 위 내용도 수정이 필요하다. Search이후 완료 여부 확인하는 방법 아진과 협의 필요
            this.Motor.NegativePosition = specification.NegativePosition;
            this.Motor.PositivePosition = specification.PositivePosition;
            
            position = specification.HomePosition;
            if ((ret = this.SetActualPosition(position)) != 0) return ret;
            if ((ret = this.SetCommandPosition(position)) != 0) return ret;
            

            if ((ret = this.SetEnable(true)) != 0) return ret;

            switch (specification.Method)
            {
                case HomingMethod.None:
                    if ((ret = this.OnComplete(specification)) != 0) return ret;
                    break;

                case HomingMethod.HomeSensor:
                    sensorDetected = this.Motor.IsHome ? DioValue.On : DioValue.Off;
                    break;

                case HomingMethod.NegativeSensor:
                    sensorDetected = this.Motor.IsNegativeLimit ? DioValue.On : DioValue.Off;
                    break;

                case HomingMethod.PositiveSensor:
                    sensorDetected = this.Motor.IsPositiveLimit ? DioValue.On : DioValue.Off;
                    break;

                case HomingMethod.Index:
                    //if ((ret = this.Motor.IndexSensor.GetValue(ref sensorDetected)) != 0) return ret;
                    break;

                default:
                    break;
            }

            // next step
            if (specification.Method != HomingMethod.None)
            {
                if (sensorDetected == DioValue.Off)
                    this.SetStepState(StepState.Search);
                else
                    this.SetStepState(StepState.Escape);
            }

            return ret;
        }

        private int OnSearch(HomingSpecification specification)
        {
            int ret = 0;
            //ActiveLevel level = ActiveLevel.High;
            //double velocity = this.Position2Pulse(specification.Velocity);
            //double acceleration = this.Position2Pulse(specification.Acceleration);

            //switch (specification.Method)
            //{
            //    case HomingMethod.None:
            //        if ((ret = this.OnComplete(specification)) != 0) return ret;
            //        break;

            //    case HomingMethod.HomeSensor:
            //        if ((ret = this.Motor.HomeSensor.GetLevel(ref level)) != 0) return ret;
            //        //if (level == ActiveLevel.High)
            //        //{
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.HomeSensor,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        //}
            //        //else
            //        //{
            //        //    if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //        //        AXT_MOTION_HOME_DETECT_SIGNAL.HomeSensor,
            //        //        AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
            //        //        AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        //}
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.NegativeSensor:
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.NegEndLimit,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.PositiveSensor:
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.PosEndLimit,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.Index:
            //        if ((ret = this.Motor.IndexSensor.GetLevel(ref level)) != 0) return ret;
            //        if (level == ActiveLevel.High)
            //        {
            //            if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //                AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
            //                AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //                AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        }
            //        else
            //        {
            //            if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //                AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
            //                AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
            //                AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        }
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    default:
            //        break;
            //}

            //// next step
            //if (specification.Method != HomingMethod.None)
            //{
            //    if (specification.EnablePreciseSearch == true)
            //        this.SetStepState(StepState.Escape);
            //    else
            //    {
            //        if (specification.EnableIndexSearch == true)
            //            this.SetStepState(StepState.SearchIndex);
            //        else
            //            this.SetStepState(StepState.MoveToHome);
            //    }
            //}

            return ret;
        }

        private int OnEscape(HomingSpecification specification)
        {
            int ret = 0;
            //ActiveLevel level = ActiveLevel.High;
            //double velocity = this.Position2Pulse(specification.Velocity);
            //double acceleration = this.Position2Pulse(specification.Acceleration);

            //switch (specification.Method)
            //{
            //    case HomingMethod.None:
            //        if ((ret = this.OnComplete(specification)) != 0) return ret;
            //        break;

            //    case HomingMethod.HomeSensor:
            //        if ((ret = this.Motor.HomeSensor.GetLevel(ref level)) != 0) return ret;
            //        // 아진 보드에서 이렇게 처리하도록 라이브러리가 구성되어 있다.
            //        //if (level == ActiveLevel.High)
            //        //{
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.HomeSensor,
            //            AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        //}
            //        //else
            //        //{
            //        //if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //        //    AXT_MOTION_HOME_DETECT_SIGNAL.HomeSensor,
            //        //    AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //        //    AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        //}
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.NegativeSensor:
            //        // 아진 보드 오류
            //        level = this.Motor.NegativeSensor.Level;
            //        if ((ret = this.Motor.NegativeSensor.SetLevel(level == ActiveLevel.High ? ActiveLevel.Low : ActiveLevel.High)) != 0) return ret;
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.NegEndLimit,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        // 원위치
            //        if ((ret = this.Motor.NegativeSensor.SetLevel(level)) != 0) return ret;
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.PositiveSensor:
            //        // 아진 보드 오류
            //        level = this.Motor.PositiveSensor.Level;
            //        if ((ret = this.Motor.PositiveSensor.SetLevel(level == ActiveLevel.High ? ActiveLevel.Low : ActiveLevel.High)) != 0) return ret;
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.PosEndLimit,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        // 원위치
            //        if ((ret = this.Motor.PositiveSensor.SetLevel(level)) != 0) return ret;
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.Index:
            //        if ((ret = this.Motor.IndexSensor.GetLevel(ref level)) != 0) return ret;
            //        if (level == ActiveLevel.High)
            //        {
            //            if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //                AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
            //                AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
            //                AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        }
            //        else
            //        {
            //            if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //                AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
            //                AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //                AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        }

            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    default:
            //        break;
            //}

            //// next step
            //if (specification.Method != HomingMethod.None)
            //{
            //    if (specification.EnablePreciseSearch == true)
            //        this.SetStepState(StepState.SearchPrecisely);
            //    else
            //        this.SetStepState(StepState.Search);
            //}

            return ret;
        }

        private int OnSearchPrecisely(HomingSpecification specification)
        {
            int ret = 0;
            //ActiveLevel level = ActiveLevel.High;
            //double velocity = this.Position2Pulse(specification.Velocity * specification.PreciseSearchVelocityPercent / 100);
            //double acceleration = this.Position2Pulse(specification.Acceleration);

            //switch (specification.Method)
            //{
            //    case HomingMethod.None:
            //        if ((ret = this.OnComplete(specification)) != 0) return ret;
            //        break;

            //    case HomingMethod.HomeSensor:
            //        if ((ret = this.Motor.HomeSensor.GetLevel(ref level)) != 0) return ret;
            //        //if (level == ActiveLevel.High)
            //        //{
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.HomeSensor,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        //}
            //        //else
            //        //{
            //        //    if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //        //        AXT_MOTION_HOME_DETECT_SIGNAL.HomeSensor,
            //        //        AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
            //        //        AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        //}
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.NegativeSensor:
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, -velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.NegEndLimit,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.PositiveSensor:
            //        if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //            AXT_MOTION_HOME_DETECT_SIGNAL.PosEndLimit,
            //            AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //            AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    case HomingMethod.Index:
            //        if ((ret = this.Motor.IndexSensor.GetLevel(ref level)) != 0) return ret;
            //        if (level == ActiveLevel.High)
            //        {
            //            if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //                AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
            //                AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
            //                AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        }
            //        else
            //        {
            //            if ((ret = AXM.SearchSignal(this.Configuration.No, velocity, acceleration,
            //                AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
            //                AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
            //                AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            //        }
            //        if ((ret = this.WaitMotionDone()) != 0) return ret;
            //        break;

            //    default:
            //        break;
            //}

            //// next step
            //if (specification.Method != HomingMethod.None)
            //{
            //    if (specification.EnableIndexSearch == true)
            //        this.SetStepState(StepState.SearchIndex);
            //    else
            //        this.SetStepState(StepState.MoveToHome);
            //}

            return ret;
        }

        private int OnSearchIndex(HomingSpecification specification)
        {
            int ret = 0;
            ActiveLevel level = ActiveLevel.High;
            double velocity = this.Position2Pulse(specification.Velocity * specification.PreciseSearchVelocityPercent / 100);
            double acceleration = this.Position2Pulse(specification.Acceleration);

            //if ((ret = this.Motor.IndexSensor.GetLevel(ref level)) != 0) return ret;
            if (level == ActiveLevel.High)
            {
                if ((ret = AXM.SearchSignal(this.No, velocity, acceleration,
                    AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
                    AXT_MOTION_EDGE.SIGNAL_UP_EDGE,
                    AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            }
            else
            {
                if ((ret = AXM.SearchSignal(this.No, velocity, acceleration,
                    AXT_MOTION_HOME_DETECT_SIGNAL.EncodZPhase,
                    AXT_MOTION_EDGE.SIGNAL_DOWN_EDGE,
                    AXT_MOTION_STOPMODE.SLOWDOWN_STOP)) != 0) return ret;
            }
            if ((ret = this.WaitMotionDone(30000)) != 0) return ret;

            this.SetStepState(StepState.MoveToHome);

            return ret;
        }

        private int OnMove(double dPosition)
        {
            int ret = 0;
            double currentPosition = 0;
            AXT_MOTION_PROFILE_MODE mode = AXT_MOTION_PROFILE_MODE.ASYM_TRAPEZOIDE_MODE;

            if (this.Configuration != null)
            {
                if (this.Configuration.MotionType == AjinAxlMotionType.SCurve)
                    mode = AXT_MOTION_PROFILE_MODE.ASYM_S_CURVE_MODE;
            }

            if ((ret = AXM.SetProfileMode(this.No, mode)) != 0) return ret;
            if (this.Configuration != null && this.Configuration.MotionType == AjinAxlMotionType.SCurve)
            {
                if ((ret = AXM.SetAccelerationJerk(this.No, this.Configuration.AccelerationJerk)) != 0) return ret;
                if ((ret = AXM.SetDecelerationJerk(this.No, this.Configuration.DecelerationJerk)) != 0) return ret;
            }

            if ((AXM.AccelUnit)this.Configuration.AccelUnit != AXM.AccelUnit.Second)
            {
                if ((ret = AXM.MovePosition(this.No, dPosition, this.Configuration.Velocity, this.Configuration.Acceleration, this.Configuration.Deceleration)) != 0) return ret;
            }
            else
            {
                TimeSpan accelTime = new TimeSpan();
                TimeSpan decelTime = new TimeSpan();
                TimeSpan velotictyTime = new TimeSpan();

                if ((ret = this.GetCommandPosition(ref currentPosition)) != 0) return ret;
                currentPosition = this.Position2Pulse(currentPosition);
                //Log.Write("AjinTest", string.Format("CurrentPosition before GetAccelDecelTime = {0} {1}", currentPosition, trajectory.ToString()));
                this.GetAccelDecelTime(currentPosition, dPosition, ref accelTime, ref decelTime, ref velotictyTime);
                if ((ret = AXM.MovePosition(this.No, dPosition, this.Configuration.Velocity, accelTime, decelTime)) != 0) return ret;
            }
            return ret;
        }
        private int OnMoveToHome(HomingSpecification specification)
        {
            int ret = 0;
            double movingDistance = 0.0;
            double dPosition = 0;
            double dTimeout = 30000;

            switch (specification.Method)
            {
                case HomingMethod.None:
                    if ((ret = this.OnComplete(specification)) != 0) return ret;
                    break;

                case HomingMethod.HomeSensor:
                    movingDistance = specification.EscapeDistance;
                    
                    if ((ret = this.SetActualPosition(0.0)) != 0) return ret;
                    if ((ret = this.SetCommandPosition(0.0)) != 0) return ret;

                    dPosition = this.Position2Pulse(movingDistance);
                    
                    if ((ret = this.OnMove(dPosition)) != 0) return ret;
                    if ((ret = this.WaitMotionDone(dTimeout)) != 0) return ret;

                    if ((ret = this.OnComplete(specification)) != 0) return ret;
                    break;

                case HomingMethod.NegativeSensor:
                case HomingMethod.PositiveSensor:
                case HomingMethod.Index:
                    if ((ret = this.SetActualPosition(0.0)) != 0) return ret;
                    if ((ret = this.SetCommandPosition(0.0)) != 0) return ret;

                    movingDistance = specification.EscapeDistance;

                    dPosition = this.Position2Pulse(movingDistance);
                    if ((ret = this.OnMove(dPosition)) != 0) return ret;
                    if ((ret = this.WaitMotionDone(dTimeout)) != 0) return ret;

                    if ((ret = this.OnComplete(specification)) != 0) return ret;
                    break;

                default:
                    break;
            }
            return ret;
        }

        private int OnComplete(HomingSpecification specification)
        {
            int ret = 0;
            double homePosition = specification.HomePosition;

            if ((ret = this.Reset()) != 0) return ret;

            Thread.Sleep(500); // actual position이 변경되지 않는 경우가 발생한다

            if ((ret = this.SetActualPosition(homePosition)) != 0) return ret;
            if ((ret = this.SetCommandPosition(homePosition)) != 0) return ret;
            

            this.SetStepState(StepState.Complete);
           
            return ret;
        }

        public double Position2Pulse(double position)
        {
            return position * (double)Configuration.PulsePerPosition;
        }
        private int ExecuteAjinHomingLib(AjinAxlHomingSpecification specification)
        {
            int ret = 0;
            AXT_MOTION_HOME_RESULT result = AXT_MOTION_HOME_RESULT.HOME_SUCCESS;
            try
            {
                // AccelUnit.Seconds로 하는 경우 예상하는 속도보다 낮은 속도로 동작하기 때문에 변경함.
                // 아진에 문의 후 수정 예정
                if ((ret = AXM.SetAccelerationUnit(this.No, AXM.AccelUnit.UnitPerSec2)) != 0) return ret;

                if ((ret = AXM.SetHomeMethod(this.No, specification.AjinAxlHomingParameter.Direction, specification.AjinAxlHomingParameter.HomeSignal, specification.AjinAxlHomingParameter.ZPhaseMethod,
                        specification.AjinAxlHomingParameter.HomeClearTime, this.Position2Pulse(specification.EscapeDistance))) != 0) return ret;

                if ((ret = AXM.SetHomeVelocity(this.No,
                    this.Position2Pulse(specification.AjinAxlHomingParameter.FirstSearchVelocity),
                    this.Position2Pulse(specification.AjinAxlHomingParameter.SecondSearchVelocity),
                    this.Position2Pulse(specification.AjinAxlHomingParameter.LastVelocity),
                    this.Position2Pulse(specification.AjinAxlHomingParameter.IndexSearchVelocity),
                    this.Position2Pulse(specification.AjinAxlHomingParameter.FirstSearchAcc),
                    this.Position2Pulse(specification.AjinAxlHomingParameter.SecondSearchAcc))) != 0) return ret;
                
                if ((ret = AXM.SetHomeStart(this.No)) != 0) return ret;

                while (true)
                {
                    Thread.Sleep(10);
                    if ((ret = AXM.GetHomeResult(this.No, ref result)) != 0) return ret;

                    if (result == AXT_MOTION_HOME_RESULT.HOME_SUCCESS) break;
                    else if (result == AXT_MOTION_HOME_RESULT.HOME_SEARCHING) continue;
                    else if (result == AXT_MOTION_HOME_RESULT.HOME_ERR_USER_BREAK) return 1;
                    else return -1;

                    //this.WriteLog(LogLevel.Normal, string.Format("ExecuteAjinHomingLib() return {0}", result.ToString()));
                    Console.WriteLine("ExecuteAjinHomingLib() return {0}", result.ToString());
                    //if ((ret = ErrorManager.Register(result.ToString())) != 0) return ret;
                }
            }
            finally
            {
                AXM.SetAccelerationUnit(this.No, (AXM.AccelUnit)this.Configuration.AccelUnit);
            }
            
            if ((ret = this.OnComplete(specification)) != 0) return ret;

            return ret;
        }
        #endregion
        #endregion

        #region Axis Members
        public new AjinAxlAxisConfiguration Configuration
        {
            get
            {
                return base.Configuration as AjinAxlAxisConfiguration;
            }
            set
            {
                base.Configuration = value as MotionAxisConfiguration;
            }
        }


        protected int Abort()
        {
            int ret = 0;
            if ((ret = AXM.SetAmpEnabled(this.No, false)) != 0) return ret;
            if ((ret = AXM.StopEmergency(this.No)) != 0) return ret;
            return ret;
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
                if ((ret = AXM.GetActualPosition(this.No, ref pulse)) != 0) return ret;

                pulse = Math.Round(pulse * this.Scale, 3);
            }
            
            return ret;
        }

        public override int GetAxisState(ref AxisState axisState)
        {
            return AXM.GetAxisState(this.No, ref axisState);
        }

        public override int GetCommandPosition(ref double pulse)
        {
            int ret = 0;

            if(Simulated)
            {
                pulse = Motor.CommandPosition;
            }
            else
            {
                if ((ret = AXM.GetCommandPosition(this.No, ref pulse)) != 0) return ret;

                pulse = Math.Round(pulse * this.Scale, 6);
            }

            
            return ret;
        }

        protected override int GetInPosition(ref bool value)
        {
            return AXM.GetInPositionValue(this.No, ref value);
        }

        protected override int GetMaxVelocity(ref double pulse)
        {
            return AXM.GetMaxVelocity(this.No, ref pulse);
        }

        public void GetMotionState(ref bool done)
        {
            done = false;
            GetMotionDone(ref done);
        }
        protected override int GetMotionDone(ref bool done)
        {
            int ret = 0;
            bool value = done ? false : true;
            if ((ret = AXM.GetInMotion(this.No, ref value)) != 0) return ret;
            done = value == true ? false : true;
            return ret;
        }
        public override int GetAmpFault(ref bool bAmpFault)
        {
            int ret = 0;
            ret = AXM.GetAmpFaultValue(this.No, ref bAmpFault);
            return ret;
        }

        protected override int GetPositionError(ref double pulse)
        {
            return AXM.GetPositionError(this.No, ref pulse);
        }

        protected override int GetVelocity(ref double pulse)
        {
            return AXM.GetVelocity(this.No, ref pulse);
        }

        public override int SetActualPosition(double pulse)
        {
            if (Simulated)
            {
                Motor.ActualPosition = pulse;
                return 0;
            }
            else
            {
                return AXM.SetActualPosition(this.No, pulse);
            }
            
        }

        public override int SetCommandPosition(double pulse)
        {
            if (Simulated)
            {
                Motor.CommandPosition = pulse;
                return 0;
            }
            else
            {
                return AXM.SetCommandPosition(this.No, pulse);
            }
            
        }

        protected override int SetMaxVelocity(double pulse)
        {
            int ret = 0;

            if ((ret = AXM.SetMaxVelocity(this.No, pulse)) != 0) return ret;

            return ret;
        }

        public override int Stop(double dDeccel)
        {
            if (Simulated)
            {
                return m_Simulator.Stop();
            }
            else
            {
                return AXM.Stop(this.No, dDeccel);
            }
        }

        public override int Stop()
        {
            if (Simulated)
            {
                return m_Simulator.Stop();
            }
            else
            {
                return AXM.StopSlowly(this.No);
            }
        }
        public override int StopEmergency()
        {
            return AXM.StopEmergency(this.No);
        }


        public override int Reset()
        {
            int ret = 0;

            if (this.Simulated == false)
            {
                if ((ret = AXM.SetAccelerationUnit(this.No, (AXM.AccelUnit)this.Configuration.AccelUnit)) != 0) return ret;
            }

            if ((ret = AXM.Reset(this.No)) != 0) return ret;

            return ret;
        }

        public override int SetEnable(bool bEnable)
        {
            int ret = 0;

            ret = AXM.SetAmpEnabled(this.No, bEnable);

            return ret;
        }

        public override int GetEnable(ref bool bEnable)
        {
            int ret = 0;

            ret = AXM.GetAmpEnabled(this.No, ref bEnable);

            return ret;
        }

        public override int GetIsNegativeLimit(ref bool bEnable)
        {
            int ret = 0;

            ret = AXM.GetNegativeLimitValue(this.No, ref bEnable);

            return ret;
        }
        public override int GetIsPositiveLimit(ref bool bEnable)
        {
            int ret = 0;

            ret = AXM.GetPositiveLimitValue(this.No, ref bEnable);

            return ret;
        }

        public override int GetIsHomeSensor(ref bool bEnable)
        {
            int ret = 0;

            ret = AXM.GetHomeSensorValue(this.No, ref bEnable);

            return ret;
        }

        public override int MovePosition(double dPosition, double dVelocity, double dAccel, double dDeccel)
        {
            int ret = 0;
            if(Simulated)
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
                ret = AXM.MovePosition(this.No, dPosition / Scale, dVelocity / Scale, dAccel / Scale, dDeccel / Scale);
            }
            

            return ret;
        }

        public override int MoveVelocity(double dVelocity, double dAccel, double dDeccel)
        {
            int ret = 0;

            if (Simulated)
            {
                if (m_Simulator == null)
                {
                    m_Simulator = new MotorSimulator();
                    
                }
                m_Simulator.MotorState = Motor;
                ret = m_Simulator.MoveVelocity(dVelocity, dAccel, dDeccel);
            }
            else
            {
                ret = AXM.MoveVelocity(this.No, dVelocity, dAccel, dDeccel);
            }

            return ret;
        }
        public override int Clear()
        {
            int ret = 0;

            ret = (int)AXM.AxmSignalWriteOutputBit(this.No, 1, 1);

            return ret;
        }

        public override int ModifyPosition(double position, double velocity, double acceleration, double deceleration)
        {
            int ret = 0;

            ret = (int)AXM.ModifyPosition(this.No, position, velocity, acceleration, deceleration);

            return ret;
        }

        public override int ModifyVelocity(double velocity, double acceleration, double deceleration)
        {
            int ret = 0;

            ret = (int)AXM.ModifyVelocity(this.No, velocity, acceleration, deceleration);

            return ret;
        }
        #endregion

        public override int Load(FileStream fs)
        {
            int ret = 0;

            AjinAxlAxisConfiguration configuration = new AjinAxlAxisConfiguration();
            ret = SaveManager.BinaryDeserialize<AjinAxlAxisConfiguration>(fs, out configuration);
            configuration.Init();
            this.Configuration = configuration;

            if(configuration != null)
            {
                this.Motor.PositivePosition = Configuration.HomingSpecification.PositivePosition;
                this.Motor.NegativePosition = Configuration.HomingSpecification.NegativePosition;
            }
            return ret;
        }
    }
    #endregion

    

    #region AjinAxlTrajectory =>주석
    //[Serializable]
    //public class AjinAxlTrajectory : Trajectory
    //{
    //    public AjinAxlMotionType MotionType;

    //    public AjinAxlTrajectory()
    //        : base()
    //    {
    //        this.MotionType = AjinAxlMotionType.Trapezoidal;
    //    }

    //    public void CopyTo(AjinAxlTrajectory target)
    //    {
    //        base.CopyTo(target);
    //        target.MotionType = this.MotionType;
    //    }

    //    public override void CopyTo(Trajectory target)
    //    {
    //        this.CopyTo(target as AjinAxlTrajectory);
    //    }
    //}
    #endregion

    

    #region AjinAxlHomingSpecification
    /// <summary>
    /// AJIN에서 제공하는 Homing 함수를 사용하기 위해 프로퍼티를 추가함.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(AjinAxlHomingSpecificationConverter))]

    public class AjinAxlHomingSpecification : HomingSpecification
    {
        #region Field
        private bool m_UseAjinAxlFunction;
        private AjinAxlHomingParameter m_AjinAxlHomingParameter;
        #endregion

        #region Constructor
        public AjinAxlHomingSpecification() : base()
        {
            this.UseAjinAxlFunction = false;
            this.AjinAxlHomingParameter = new AjinAxlHomingParameter();
            SetDefaultValues();
        }
        #endregion

        #region Property
        /// <summary>
        /// Homing을 Ajin AXL 함수를 사용할지 여부를 가져오거나 설정한다.
        /// </summary>
        public bool UseAjinAxlFunction
        {
            get { return this.m_UseAjinAxlFunction; }
            set { this.m_UseAjinAxlFunction = value; }
        }

        public AjinAxlHomingParameter AjinAxlHomingParameter
        {
            get { return this.m_AjinAxlHomingParameter; }
            set { this.m_AjinAxlHomingParameter = value; }
        }
        #endregion
        #region Method
        protected void SetDefaultValues()
        {
            this.UseAjinAxlFunction = true;
            this.AjinAxlHomingParameter.Direction = Directions.Ccw;
            this.AjinAxlHomingParameter.FirstSearchVelocity = 30;
            this.AjinAxlHomingParameter.FirstSearchAcc = 300;
            this.AjinAxlHomingParameter.HomeClearTime = 1500;
            this.AjinAxlHomingParameter.HomeSignal = HomeSignals.NegativeLimit;
            this.AjinAxlHomingParameter.IndexSearchVelocity = 1;
            this.AjinAxlHomingParameter.LastVelocity = 10;
            this.AjinAxlHomingParameter.SecondSearchVelocity = 10;
            this.AjinAxlHomingParameter.SecondSearchAcc = 100;
            this.AjinAxlHomingParameter.ZPhaseMethod = ZPhaseMethods.None;

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

    #region AjinAxlHomingParameter
    /// <summary>
    /// AJIN에서 제공하는 Homing 함수를 사용하기 위해 프로퍼티를 추가함.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(AjinAxlHomingParameterConverter))]
    public class AjinAxlHomingParameter
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
        public AjinAxlHomingParameter()
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

    #region AjinAxlHomingParameterConverter
    [Serializable]
    internal class AjinAxlHomingParameterConverter : ExpandableObjectConverter
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
                AjinAxlHomingParameter parameter = new AjinAxlHomingParameter();
                parameter.Direction = (Directions) Enum.Parse(typeof(Directions), token[0]);
                parameter.ZPhaseMethod = (ZPhaseMethods) Enum.Parse(typeof(ZPhaseMethods), token[1]);
                parameter.HomeSignal = (HomeSignals) Enum.Parse(typeof(HomeSignals), token[2]);
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
            if (destinationType == typeof(String) && value is AjinAxlHomingParameter)
            {
                AjinAxlHomingParameter parameter = value as AjinAxlHomingParameter;
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

    #region AjinAxlHomingSpecificationConverter
    [Serializable]
    internal class AjinAxlHomingSpecificationConverter : ExpandableObjectConverter
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
                AjinAxlHomingSpecification specification = new AjinAxlHomingSpecification();
                specification.Velocity = double.Parse(token[0]);
                specification.Acceleration = double.Parse(token[1]);
                specification.Deceleration = double.Parse(token[2]);
                specification.NegativePosition = double.Parse(token[3]);
                specification.PositivePosition = double.Parse(token[4]);
                specification.EscapeDistance = double.Parse(token[5]);
                specification.HomePosition = double.Parse(token[6]);
                specification.Method = (HomingMethod) Enum.Parse(typeof(HomingMethod), token[7]);
                specification.EnablePreciseSearch = bool.Parse(token[8]);
                specification.PreciseSearchVelocityPercent = int.Parse(token[9]);
                specification.EnableIndexSearch = bool.Parse(token[10]);
                specification.UseAjinAxlFunction = bool.Parse(token[11]);

                return specification;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is AjinAxlHomingSpecification)
            {
                AjinAxlHomingSpecification specification = value as AjinAxlHomingSpecification;
                return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                    specification.Velocity, specification.Acceleration, specification.Deceleration,
                    specification.NegativePosition, specification.PositivePosition, specification.EscapeDistance,
                    specification.HomePosition, specification.Method,
                    specification.EnablePreciseSearch, specification.PreciseSearchVelocityPercent, specification.EnableIndexSearch, specification.UseAjinAxlFunction);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion

    #region AjinAxlGantrySpecification
    /// <summary>
    /// 아진 AXL Library에서 제공하는 gantry 설정을 정의한다.
    /// </summary>
    //[Serializable]
    //[TypeConverter(typeof(AjinAxlGantrySpecificationConverter))]
    //public class AjinAxlGantrySpecification
    //{
    //    private bool m_Enabled;
    //    private int m_SlaveAxisNo;
    //    private bool m_SlaveOffsetVerified;
    //    private double m_SlaveOffset;
    //    private double m_SlaveOffsetRange;

    //    public AjinAxlGantrySpecification()
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

    #region AjinAxlGantrySpecificationConverter
    [Serializable]
    internal class AjinAxlGantrySpecificationConverter : ExpandableObjectConverter
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
                AjinAxlGantrySpecification specification = new AjinAxlGantrySpecification();
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
            if (destinationType == typeof(String) && value is AjinAxlGantrySpecification)
            {
                AjinAxlGantrySpecification specification = value as AjinAxlGantrySpecification;
                return string.Format("{0}, {1}, {2}, {3}, {4}",
                    specification.Enabled, specification.SlaveAxisNo, specification.SlaveOffsetVerified, specification.SlaveOffset, specification.SlaveOffsetRange);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion
}