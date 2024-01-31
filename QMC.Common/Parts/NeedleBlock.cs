using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class NeedleBlock : MotionPart
    {
        [Serializable]
        public enum MotionKey
        {
            X,
            Y,
            Cap,
            Needle,
        }
        [Serializable]
        public enum DioPointKey
        {
            Input_NeedleVacuum,
            Output_NeedleVacuum,
        }
        [Serializable]
        public enum AlarmKeys
        {
            ePinOutOfRange = -7,
            eCapOutOfRange = -8,
        }
        public NeedleBlockConfig Config { get; set; }
        //public XytStage Stage { get; set; }
        //public XyztStage Stage { get; set; }
        //public XyzztStage Stage { get; set; }
        public UvwzxyzStage Stage { get; set; }
        public Camera Camera { get; set; }
        public bool Simulated { set; get; }
        public VisionImage TestImage { set; get; }
        public VisionImage TrainImage { set; get; }
        public int ResponseTimeout { set; get; }

        public double Radius
        {
            set
            {
                Config.Radius = value;
            }
            get
            {
                return Config.Radius;
            }
        }

        public NeedleBlock(string strName) : base(strName)
        {
            if (Config == null)
                Config = new NeedleBlockConfig();
            TestImage = new VisionImage();
            TrainImage = new VisionImage();
            ResponseTimeout = 0;
        }

        protected override void InitAlarm()
        {
            base.InitAlarm();

            Alarm alarm = new Alarm();
            alarm.Code = (int)AlarmKeys.ePinOutOfRange;
            alarm.Title = "Out Of Range Error";
            alarm.Cause = "Needle Movable OutOfRange";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKeys.eCapOutOfRange;
            alarm.Title = "Out Of Range Error";
            alarm.Cause = "Cap Movable OutOfRange";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);
        }
        #region Part
        public override int Create()
        {
            int ret = base.Create();

            if (m_dicAxes == null)
                m_dicAxes = new Dictionary<string, MotionAxis>();

            m_dicAxes.Clear();
            foreach (MotionKey key in Enum.GetValues(typeof(MotionKey)))
            {
                m_dicAxes.Add(key.ToString(), null);
            }

            m_dicAxisDisplayType.Clear();
            m_dicAxisDisplayType.Add(MotionKey.X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.Y.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.Cap.ToString(), DisplayAxisType.Vertical);
            m_dicAxisDisplayType.Add(MotionKey.Needle.ToString(), DisplayAxisType.Vertical);

            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();
            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }

            return ret;
        }

        public override void Stop()
        {
            base.Stop();
            foreach (string key in m_dicAxes.Keys)
            {
                MotionAxis axis = m_dicAxes[key];
                if (axis != null)
                    axis.Stop();
            }
        }

        public override int Initialize()
        {
            int ret = 0;
            if (m_Status == RunStatus.Stop) return 1;
            if ((ret = m_dicAxes[MotionKey.Needle.ToString()].Initialize()) != 0) return ret;
            if ((ret = m_dicAxes[MotionKey.Cap.ToString()].Initialize()) != 0) return ret;
            if ((ret = m_dicAxes[MotionKey.X.ToString()].Initialize()) != 0) return ret;
            if ((ret = m_dicAxes[MotionKey.Y.ToString()].Initialize()) != 0) return ret;

            return ret;
        }
        #endregion

        protected override int OnBeforeMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;
            //Interlock

            //  없는 부분이라 일단 주석 처리
            /*
            if (Owner is DieLoader)
            {
                DieLoader dieLoader = Owner as DieLoader;
                LoaderInterLockPosition interlock = dieLoader.Config.InterlockPosition;
                //double needlePos = this.GetCurrentActualPosition("Cap");
                double PositionX = dieLoader.Stage.GetCurrentActualPosition(XytStage.MotionKey.X.ToString());
                double PositionY = dieLoader.Stage.GetCurrentActualPosition(XytStage.MotionKey.Y.ToString());
                RangeD rangeX = new RangeD(interlock.SecondPos.X, interlock.FirstPos.X);
                RangeD rangeY = new RangeD(interlock.FirstPos.Y, interlock.SecondPos.Y);
                XyCoordinate tourchSensorPos = new XyCoordinate();
                //int nindex = (int)PositionKeys.TouchSensor;
                tourchSensorPos.X = dieLoader.Config.GetPositionData("TouchSensor", TargetType.Base).X;
                tourchSensorPos.Y = dieLoader.Config.GetPositionData("TouchSensor", TargetType.Base).Y;
                RangeD tourchRangeX = new RangeD(tourchSensorPos.X - 3, tourchSensorPos.X + 3);
                RangeD tourchRangeY = new RangeD(tourchSensorPos.Y - 3, tourchSensorPos.Y + 3);
                if (tourchRangeX.Contains(PositionX) && tourchRangeY.Contains(PositionY))
                {
                    if (dicMovingProjection.ContainsKey("Cap"))
                    {
                        double needlePos = dicMovingProjection["Cap"].Position;
                        if (interlock.NeedlePos + 2 <= needlePos)
                        {
                            Alarm alarm = this.GetAlarm((int)AlarmKeys.eCapOutOfRange);
                            AlarmManager.Instance.ShowAlarm(alarm);
                            return (int)AlarmKeys.eCapOutOfRange;
                        }

                    }
                }
                else
                {
                    if (!rangeX.Contains(PositionX) || !rangeY.Contains(PositionY))
                    {

                        if (dicMovingProjection.ContainsKey("Cap"))
                        {
                            double needlePos = dicMovingProjection["Cap"].Position;
                            if (interlock.NeedlePos <= needlePos)
                            {
                                Alarm alarm = this.GetAlarm((int)AlarmKeys.eCapOutOfRange);
                                AlarmManager.Instance.ShowAlarm(alarm);
                                return (int)AlarmKeys.eCapOutOfRange;
                            }

                        }
                    }
                }
            }
            */
            return ret;
        }
        protected MovingProjection GetDefaultMovingProjections(string strKey)
        {
            MovingProjection movingProjection = null;
            MotionAxis axis = m_dicAxes[strKey];
            if (axis != null)
            {
                movingProjection = axis.GetDefaultMovingProjection();
            }

            return movingProjection;
        }
        public Dictionary<string, MovingProjection> GetDefaultMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            {
                string strKey = MotionKey.X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.Y.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        public Dictionary<string, MovingProjection> GetDefaultCapMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            {
                string strKey = MotionKey.Cap.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        public Dictionary<string, MovingProjection> GetDefaultNeedleMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            {
                string strKey = MotionKey.Needle.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        public int MovePosition(XyCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;

            return Move(dicMovingProjection);
        }

        public Task<int> BeginMovePosition(XyCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;

            return BeginMove(dicMovingProjection);
        }

        public int MoveCapPosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultCapMovingProjections();

            dicMovingProjection[MotionKey.Cap.ToString()].Position = dPosition;

            return Move(dicMovingProjection);
        }

        public Task<int> BeginMoveCapPosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultCapMovingProjections();

            dicMovingProjection[MotionKey.Cap.ToString()].Position = dPosition;

            return BeginMove(dicMovingProjection);
        }

        public int MoveNeedlePosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultNeedleMovingProjections();

            dicMovingProjection[MotionKey.Needle.ToString()].Position = dPosition;

            return Move(dicMovingProjection);
        }

        public Task<int> BeginMoveNeedlePosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultNeedleMovingProjections();

            dicMovingProjection[MotionKey.Needle.ToString()].Position = dPosition;
            return BeginMove(dicMovingProjection);

        }

        public Task<int> BeginNeedleGoBackPosition()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultNeedleMovingProjections();
            dicMovingProjection[MotionKey.Needle.ToString()].Position = Config.GetNeedlePosition("Z", TargetType.Base);
            return BeginMove(dicMovingProjection);
        }
        public double GetCurrentCapPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.Cap.ToString()];
            if (axis != null)
            {
                //dPosition = axis.Motor.ActualPosition;
                axis.GetActualPosition(ref dPosition);
            }
            return dPosition;
        }

        public double GetCurrentNeedlePosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.Needle.ToString()];
            if (axis != null)
            {
                //dPosition = axis.Motor.ActualPosition;
                axis.GetActualPosition(ref dPosition);
            }
            return dPosition;
        }
        public double GetCurrentNeedlePosition(ref double dPosition)
        {
            int ret = 0;
            dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.Needle.ToString()];
            if (axis != null)
            {
                //dPosition = axis.Motor.ActualPosition;
                ret = axis.GetActualPosition(ref dPosition);
            }
            return ret;
        }

        public XyCoordinate GetCurrentNeedleBlockPosition()
        {
            XyCoordinate current = new XyCoordinate();
            MotionAxis axisX = m_dicAxes[MotionKey.X.ToString()];
            MotionAxis axisY = m_dicAxes[MotionKey.Y.ToString()];

            if (axisX != null && axisY != null)
            {
                double dPos = 0;
                axisX.GetActualPosition(ref dPos);
                current.X = dPos;
                axisY.GetActualPosition(ref dPos);
                current.Y = dPos;
            }

            return current;
        }

        public int WaitMoveDone(MotionKey motion)
        {
            int ret = 0;
            ret = m_dicAxes[motion.ToString()].WaitMotionDone(0);

            return ret;
        }


        public int GetCommandPosition(ref XyCoordinate capCurrentCoordinate)
        {
            int ret = 0;

            if (capCurrentCoordinate == null)
                capCurrentCoordinate = new XyCoordinate();


            return ret;
        }
        public Task<int> BeginMoveTwoStepAccDec(MovingProjection first, MovingProjection second)
        {

            return Task.Factory.StartNew(() =>
            {
                return OnMoveTwoStepAccDec(first, second);
            });
        }
        public virtual int OnMoveTwoStepAccDec(MovingProjection first, MovingProjection second)
        {
            int ret = 0;
            double dPosition = GetCurrentActualPosition("Z");
            AjinAxlAxis axis = this.Axes["Z"] as AjinAxlAxis;
            if (axis != null)
            {
                Task<int> ar = this.BeginMoveNeedlePosition(dPosition + first.Position);
                dPosition = GetCurrentActualPosition("Z");
                double dOverridePos = first.Position;
                first.Position += second.Position;
                //Task<int> ar = Axes["Z"].MoveDistance(first.Position - dPosition, first.Velocity, first.Acceleration, first.Deceleration);

                while (true)
                {
                    double dCurrentPos = 0;
                    if (Math.Abs(dCurrentPos - dPosition) >= Math.Abs(dOverridePos))
                    {

                        axis.MoveVelocity(second.Velocity, second.Acceleration, second.Deceleration);
                        break;
                    }
                    if (ar.IsCompleted)
                        break;
                    Thread.Sleep(0);
                }
                ar.Wait();
            }
            else
            {
                first.Position += second.Position;
                dPosition += first.Position;
                this.BeginMoveNeedlePosition(dPosition);
            }
            return ret;
        }

        public virtual bool IsHold()
        {
            bool bRet = false;
            DioPoint point = m_dicDioPoints[DioPointKey.Input_NeedleVacuum.ToString()];
            if (point == null)
            {
                return bRet;
            }


            //DioValue off = DioValue.Off;
            DioValue value = DioValue.Off;
            if (m_dicDioPoints[DioPointKey.Input_NeedleVacuum.ToString()] != null)
            {
                value = point.GetValue();
            }


            if (value == DioValue.On)
            {
                bRet = true;
            }


            return bRet;
        }

        public virtual bool IsRelease()
        {
            bool bRet = false;

            DioPoint point = m_dicDioPoints[DioPointKey.Input_NeedleVacuum.ToString()];
            if (point == null)
            {
                return bRet;
            }


            //DioValue off = DioValue.Off;
            DioValue value = DioValue.Off;
            if (m_dicDioPoints[DioPointKey.Input_NeedleVacuum.ToString()] != null)
            {
                value = point.GetValue();
            }


            if (value == DioValue.Off)
            {
                bRet = true;
            }


            return bRet;
        }
        public virtual int Hold()
        {
            int ret = 0;
            /*
            DioPoint point = m_dicDioPoints[DioPointKey.Output_NeedleVacuum.ToString()];
            if (point == null)
            {
                return ret;
            }
            if (point.IoType != IoType.Input)
            {
                if (point.GetValue() == DioValue.Off)
                {
                    point.Write(DioValue.On);
                }
                else if ((point.GetValue() == DioValue.On))
                {
                    //point.Write(DioValue.Off);
                }
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsHold())
                    break;
                TimeSpan processTime = DateTime.Now - startTime;
                if (ResponseTimeout != 0 && processTime.TotalSeconds >= ResponseTimeout)
                {
                    //Timeout
                    Alarm alarm = this.GetAlarm((int)Gripper.AlarmKeys.eTimeOut);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    ret = -1;
                    break;
                }
                Thread.Sleep(1);
            }
            */
            return ret;
        }

        public virtual Task<int> BeginHold()
        {
            return Task.Factory.StartNew(() =>
            {
                return Hold();
            });
        }

        public virtual int Release()
        {
            int ret = 0;

            /*
            //if ((ret = m_dicDioPoints[DioPointKey.Output_NeedleVacuum.ToString()].Write(DioValue.Off)) != 0) return ret;
            DioPoint point = m_dicDioPoints[DioPointKey.Output_NeedleVacuum.ToString()];
            if (point == null)
            {
                return ret;
            }
            if (point.IoType != IoType.Input)
            {
                if (point.GetValue() == DioValue.Off)
                {
                    //point.Write(DioValue.On);
                }
                else if ((point.GetValue() == DioValue.On))
                {
                    point.Write(DioValue.Off);
                }
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsRelease())
                    break;
                TimeSpan processTime = DateTime.Now - startTime;
                if (ResponseTimeout != 0 && processTime.TotalSeconds >= ResponseTimeout)
                {
                    //Timeout
                    Alarm alarm = this.GetAlarm((int)Gripper.AlarmKeys.eTimeOut);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    ret = -1;
                    break;
                }
                Thread.Sleep(1);
            }
            */

            return ret;
        }

        public virtual Task<int> BeginRelease()
        {
            return Task.Factory.StartNew(() =>
            {
                return Release();
            });
        }

        public override void UpdateConfigData() //참고 : 오버라이드,, 파트 콜
        {
            /*
            if (Owner is DieLoader)
            {
                DieLoader dieLoader = Owner as DieLoader;
                Config = dieLoader.Config.NeedleBlockConfig;
            }
            */
        }
    }
}
