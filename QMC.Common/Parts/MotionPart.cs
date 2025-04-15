using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class MotionPart : Part
    {
        public BaseInterpolator Interpolator { set; get; }
        public InterpolatorMotionFunction MC_Func = null;
        public MotionPart(string strName) : base(strName)
        {
            Interpolator = null;
            MC_Func = new InterpolatorMotionFunction();
        }

        public override int Initialize()
        {
            int ret = 0;
            if (m_Status == RunStatus.Stop) return 1;
            if((ret = base.Initialize()) != 0 ) return ret;
            foreach(DioPoint point in DioPoints.Values)
            {
                if (m_Status == RunStatus.Stop) return 1;
                if (point != null)
                    point.Initialize();
            }

            foreach(MotionAxis axis in Axes.Values)
            {
                if(axis != null)
                {
                    if (m_Status == RunStatus.Stop) return 1;
                    ret = axis.Initialize();
                    if (ret != 0)
                        return ret;
                }
                    
            }

            return ret;
        }

        public override List<MotionAxis> GetAxisList()
        {
            List<MotionAxis> listAxis = new List<MotionAxis>();
            foreach (string strKey in m_dicAxes.Keys)
            {
                MotionAxis axis = m_dicAxes[strKey];
                if (axis != null)
                {
                    axis.Configuration.DisplayAxisType = m_dicAxisDisplayType[strKey];
                    axis.OnBeforeMove -= Axis_OnBeforeMove;
                    axis.OnBeforeMove += Axis_OnBeforeMove;
                    listAxis.Add(axis);
                }
            }

            return listAxis;
        }

        private int Axis_OnBeforeMove(MotionAxis axis, double dPosition)
        {
            int ret = 0;
            Dictionary<string, MovingProjection> dicProjection = GetDefaultMovingProjections();
            if(dicProjection.ContainsKey(axis.Tag))
            {
                dicProjection[axis.Tag].Position = dPosition;
            }
            else
            {
                dicProjection.Add(axis.Tag, axis.GetDefaultMovingProjection()); 
                dicProjection[axis.Tag].Position = dPosition;
            }

            ret = OnBeforeMove(dicProjection);

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Stop()
        {
            base.Stop();
            foreach (MotionAxis axis in Axes.Values)
            {
                if (axis != null)
                    axis.Stop();
            }
        }
        public Task<int> BeginMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = Move(dicMovingProjection);
                return 0;
            });

            return task;
        }

        public int Move(double dPosition)
        {
            if (m_Status == RunStatus.Stop) return 1;

            int ret = 0;
            MovingProjection movingProjection = null;
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            foreach (MotionAxis axis in Axes.Values)
            {
                if (axis.Name != "STAGE_Z")
                    continue;

                if (!axis.Motor.CheckLimit(dPosition))
                {
                    ret = -1;
                    return ret;
                }

                movingProjection = axis.GetDefaultMovingProjection();
                movingProjection.Position = dPosition * axis.Scale;
                dicMovingProjection.Add(axis.Tag, movingProjection);
                break;
            }
            if(dicMovingProjection.Count > 0)
            {
                if ((ret = OnBeforeMove(dicMovingProjection)) != 0) return ret;
                if ((ret = OnMove(dicMovingProjection)) != 0) return ret;
                if ((ret = OnAfterMove(dicMovingProjection)) != 0) return ret;
            }
            
            return ret;
        }

        protected override int OnMoveInterpolation(MotionAxis axis, double dPosition, int nVelPercent)
        {
            int ret = 0;

            if (Interpolator != null && Interpolator.IsInterpolationAxis(axis))
            {
                ret = OnInterpolation(axis, dPosition, nVelPercent);
            }
            else
            {
                ret = axis.MovePosition(dPosition, nVelPercent);
            }

            return ret;
        }

        protected override int OnStopJogVelocity(MotionAxis axis, int nVelPercent)
        {
            int ret = 0;

            if (Interpolator != null && Interpolator.IsInterpolationAxis(axis))
            {

            }

            return ret;
        }

        protected override int OnGetAcutualInterpolationPosition(MotionAxis axis, ref double dPosition)
        {
            int ret = 0;

            if (Interpolator != null && Interpolator.IsInterpolationAxis(axis))
            {
                ret = Interpolator.GetInterpolationActualPosition(axis, ref dPosition);
            }
            else
            {
                ret = axis.GetActualPosition(ref dPosition);
            }

            return ret;
        }

        protected override int OnGetCommandInterpolationPosition(MotionAxis axis, ref double dPosition)
        {
            int ret = 0;

            if (Interpolator != null && Interpolator.IsInterpolationAxis(axis))
            {
                ret = Interpolator.GetInterpolationCommandPosition(axis, ref dPosition);
            }
            else
            {
                ret = axis.GetCommandPosition(ref dPosition);
            }

            return ret;
        }

        protected virtual int OnInterpolation(MotionAxis axis, double dPosition, int nVelPercent)
        {
            int ret = 0;
            XyCoordinate coordinate = new XyCoordinate();
            if ((ret = Interpolator.Interpolate(axis, dPosition, ref coordinate)) != 0)
            {
                return ret;
            }

            if ((ret = Interpolator.Move(coordinate, nVelPercent)) != 0)
            {
                return ret;
            }

            return ret;
        }

        protected virtual int OnReverseInterpolate(MotionAxis axis)
        {
            int ret = 0;

            return ret;
        }

        public int Move(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop) return 1;
            foreach (string axisKey in dicMovingProjection.Keys)
            {
                if(Axes.ContainsKey(axisKey))
                {
                    MovingProjection movingProjection = dicMovingProjection[axisKey];
                    MotionAxis axis = Axes[axisKey];

                    if (axis.Simulated == true) continue;

                    if(axis != null)
                    {
                        //  2023. 07. 31 임시 주석 (Axis 정보에... Homing 파라미터 쪽에 Neg, Pos Limit 을 설정하게 되어 있음. default 는 0 이니 꼭 수정해야 함.)
                        //Limit 확인.
                        //if (!axis.Motor.CheckLimit(movingProjection.Position))
                        //{
                        //    ret = -1;
                        //    return ret;
                        //}
                    }
                }
            }

            if ((ret = OnBeforeMove(dicMovingProjection)) != 0) return ret;
            if ((ret = OnMove(dicMovingProjection)) != 0) return ret;
            if ((ret = OnAfterMove(dicMovingProjection)) != 0) return ret;

            return ret;
        }

        protected virtual int OnBeforeMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;            

            return ret;
        }
        protected virtual int OnMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;
            
            //if (m_Status == RunStatus.Stop) return 1;
            foreach (string axisKey in dicMovingProjection.Keys)
            {
                if (Axes.ContainsKey(axisKey))
                {
                    MovingProjection movingProjection = dicMovingProjection[axisKey];
                    MotionAxis axis = Axes[axisKey];
                    if (axis != null)
                    {
                        if(axis.No == (int)WorkStage.nAxis.X ||(axis.No == (int)WorkStage.nAxis.Y)  )
                        {
                            MC_Func.MC_MovePosition(axis.No, movingProjection.Position, movingProjection.Velocity, movingProjection.Acceleration, movingProjection.Deceleration);

                        }
                        else
                        {

                            if ((ret = axis.MovePosition(movingProjection.Position, movingProjection.Velocity, movingProjection.Acceleration, movingProjection.Deceleration)) != 0) return ret;
                        }
                    }
                }
            }

            foreach(string axisKey in dicMovingProjection.Keys)
            {
                if (Axes.ContainsKey(axisKey))
                {
                    MotionAxis axis = Axes[axisKey];
                    if (axis != null)
                    {
                        axis.WaitMotionDone(dicMovingProjection[axisKey].Timeout);
                    }
                }
            }
            
            return ret;
        }

        public double GetCurrentCommandPosition(string strAxis)
        {
            double dValue = 0;
            if (Axes.ContainsKey(strAxis))
            {
                MotionAxis axis = Axes[strAxis];
                if (axis != null)
                {
                    //axis.GetCommandPosition(ref dValue);
                    dValue = GetCurrentActualPosition(axis);                //  2023. 05. 22.  SCH : 2D Mapping 에 이걸로 되어 있음.
                }
            }
            
            return dValue;
        }

        public int SetPosition(double dPosition, string strAxis)
        {
            int ret = 0;

            if (Axes.ContainsKey(strAxis))
            {
                MotionAxis axis = Axes[strAxis];
                if (axis != null)
                {
                    axis.SetPosition(dPosition);
                }
            }

            return ret;
        }

        public double GetCurrentActualPosition(MotionAxis axis)
        {
            double dValue = 0;

            if (Interpolator != null && Interpolator.IsInterpolationAxis(axis))
            {
                OnReverseInterpolate(axis);
            }
            else
            {
                axis.GetActualPosition(ref dValue);
            }

            return dValue;
        }

        public double GetCurrentActualPosition(string strAxis)
        {
            double dValue = 0;
            if (Axes.ContainsKey(strAxis))
            {
                MotionAxis axis = Axes[strAxis];
                if (axis != null)
                {
                    //dValue = axis.Motor.ActualPosition;
                    axis.GetActualPosition(ref dValue);
                }
            }

            return dValue;
        }
        public double GetCurrentActualPosition(string strAxis, ref double dPositon)
        {
            int ret = 0;
            dPositon = 0;
            if (Axes.ContainsKey(strAxis))
            {
                MotionAxis axis = Axes[strAxis];
                if (axis != null)
                {
                    //dValue = axis.Motor.ActualPosition;
                    ret = axis.GetActualPosition(ref dPositon);

                }
            }
            else
            {
                return -1;
            }

            return ret;
        }


        protected virtual int OnAfterMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;

            return ret;
        }

        public MovingProjection GetDefaultMovingProjection(string strAxis)
        {
            MovingProjection movingProjection = null;

            if (m_dicAxes.ContainsKey(strAxis)/* && m_dicAxes[strAxis] != null*/)
            {
                if (m_dicAxes[strAxis] == null)
                {
                    MovingProjection projection = new MovingProjection();
                    double dPosition = 0;
                    projection.Position = dPosition;
                    projection.Velocity = 10;
                    projection.Acceleration = 100;
                    projection.Deceleration = 100;
                    projection.Timeout = 1000;

                    movingProjection = projection;
                }
                else
                {
                    movingProjection = m_dicAxes[strAxis].GetDefaultMovingProjection();
                }
            }
            return movingProjection;
        }

        public Dictionary<string, MovingProjection> GetDefaultMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            foreach (string key in m_dicAxes.Keys)
            {
                MotionAxis axis = m_dicAxes[key];
                if (axis != null)
                {
                    MovingProjection movingProjection = axis.GetDefaultMovingProjection();
                    if (movingProjection != null)
                    {
                        dicMovingProjection.Add(key, movingProjection);
                    }
                }
            }

            return dicMovingProjection;
        }

    }
}
