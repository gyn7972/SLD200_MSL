using QMC.Common.Modules;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class XytStage : XyStage
    {
        [Serializable]
        public new enum MotionKey
        {
            X,
            Y,
            T,
        }
        [Serializable]
        public enum AlarmKeys
        {
            eOutOfInterLockRangeX = -10,
            eOutOfInterLockRangeY = -11,
        }
        public XytCoordinate CenterCoordinate { get; set; }
        //public MaterialCompansator m_Compansator;
        public NeedleBlock  NeedleBlock { get; set; }

        //public Gripper Gripper { get; set; }

        //public MaterialCompansator Compansator
        //{
        //    get { return m_Compansator; }
        //    set { m_Compansator = value; }
        //}
        public XytStage(string strName) : base(strName)
        {
            CenterCoordinate = new XytCoordinate(5, 5, 0);//? 회전의 중심
        }
        protected override void InitAlarm()
        {
            base.InitAlarm();
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKeys.eOutOfInterLockRangeX;
                alarm.Title = "InterLock";
                alarm.Cause = "Out Of Range Error";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKeys.eOutOfInterLockRangeY;
                alarm.Title = "InterLock";
                alarm.Cause = "Out Of Range Error";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }

        }

        #region Part
        public override int Create()
        {
            int ret = 0;

            if(m_dicAxes == null)
                m_dicAxes = new Dictionary<string, MotionAxis>();

            m_dicAxes.Clear();
            foreach(MotionKey key in Enum.GetValues(typeof(MotionKey)))
            {
                m_dicAxes.Add(key.ToString(), null);
            }

            m_dicAxisDisplayType.Clear();
            m_dicAxisDisplayType.Add(MotionKey.X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.Y.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.T.ToString(), DisplayAxisType.Theta);

            return ret;
        }

        public override void Stop()
        {
            base.Stop();
            foreach(string key in m_dicAxes.Keys)
            {
                MotionAxis axis = m_dicAxes[key];
                if(axis != null)
                    axis.Stop();
            }
        }
        #endregion
        
        public int MovePosition(XytCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            dicMovingProjection[MotionKey.T.ToString()].Position = coordinate.T;

            return Move(dicMovingProjection);
        }
        public new int MovePosition(XyCoordinate coordinate)
        {
            if (m_Status == RunStatus.Stop) return 1;
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;

            return Move(dicMovingProjection);
        }

        public Task<int> BeginMovePosition(XytCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            dicMovingProjection[MotionKey.T.ToString()].Position = coordinate.T;

            return BeginMove(dicMovingProjection);
        }

        protected override int OnBeforeMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;
            /*
            if (Owner is DieLoader)
            {
                DieLoader dieLoader = Owner as DieLoader;
                double needlePos = dieLoader.NeedleBlock.GetCurrentCapPosition();
                LoaderInterLockPosition interlock = dieLoader.Config.InterlockPosition;
                double PositionX = dicMovingProjection["X"].Position;
                double PositionY = dicMovingProjection["Y"].Position;
                XyCoordinate tourchSensorPos = new XyCoordinate();
                tourchSensorPos.X = dieLoader.Config.GetPositionData("TouchSensor", TargetType.Base).X;
                tourchSensorPos.Y = dieLoader.Config.GetPositionData("TouchSensor", TargetType.Base).Y;
                RangeD tourchRangeX = new RangeD(tourchSensorPos.X - 3, tourchSensorPos.X + 3);
                RangeD tourchRangeY = new RangeD(tourchSensorPos.Y - 3, tourchSensorPos.Y + 3);
                if (tourchRangeX.Contains(PositionX) && tourchRangeY.Contains(PositionY))
                {
                    if (interlock.NeedlePos + 2 <= needlePos)
                    {
                        Alarm alarm = this.GetAlarm((int)NeedleBlock.AlarmKeys.eCapOutOfRange);
                        AlarmManager.Instance.ShowAlarm(alarm);
                        return (int)NeedleBlock.AlarmKeys.eCapOutOfRange;
                    }
                }
                else
                {
                    if (interlock.IsValidValues())
                    {
                        if (interlock.NeedlePos <= needlePos)
                        {

                            RangeD rangeX = new RangeD(interlock.SecondPos.X, interlock.FirstPos.X);
                            RangeD rangeY = new RangeD(interlock.FirstPos.Y, interlock.SecondPos.Y);
                            if (dicMovingProjection.ContainsKey("X"))
                            {
                                if (!rangeX.Contains(PositionX))
                                //if(interlock.FirstPos.X > PositionX && interlock.SecondPos.X < PositionX)
                                {
                                    Alarm alarm = this.GetAlarm((int)AlarmKeys.eOutOfInterLockRangeX);
                                    AlarmManager.Instance.ShowAlarm(alarm);
                                    return (int)AlarmKeys.eOutOfInterLockRangeX;
                                }

                            }
                            if (dicMovingProjection.ContainsKey("Y"))
                            {
                                if (!rangeY.Contains(PositionY))
                                //if (interlock.FirstPos.Y < PositionX && interlock.SecondPos.Y > PositionX)
                                {
                                    Alarm alarm = this.GetAlarm((int)AlarmKeys.eOutOfInterLockRangeY);
                                    AlarmManager.Instance.ShowAlarm(alarm);
                                    return (int)AlarmKeys.eOutOfInterLockRangeY;
                                }
                            }
                        }
                    }
                }
            }
            */
            return ret;
        }

        public new int GetCommandPosition(ref XytCoordinate currentPosition)
        {
            int ret = 0;

            if(currentPosition == null)
                currentPosition = new XytCoordinate();

            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetCommandPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Y = dPos;
            m_dicAxes[MotionKey.T.ToString()].GetCommandPosition(ref dPos);
            currentPosition.T = dPos;

            return ret;
        }
        public int GetCommandPosition(ref XyCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XyCoordinate();

            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetCommandPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Y = dPos;

            return ret;
        }

        public new int GetActualPosition(ref XytCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XytCoordinate();
            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetActualPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetActualPosition(ref dPos);
            currentPosition.Y = dPos;
            m_dicAxes[MotionKey.T.ToString()].GetActualPosition(ref dPos);
            currentPosition.T = dPos;

            return ret;
        }

        public new int GetAxisCount()
        {
            return m_dicAxes.Count;
        }

        public bool CheckLimit(MotionKey axis, double dPosition)
        {
            bool bRet = false;
            MotionAxis motion = m_dicAxes[axis.ToString()];
            if(motion != null)
            {
                bRet = motion.Motor.CheckLimit(dPosition);
            }

            return bRet;
        }
    }
}
