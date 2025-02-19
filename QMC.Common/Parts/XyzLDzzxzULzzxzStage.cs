using ACS.SPiiPlusNET;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Parts.WorkStageParameter;

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

namespace QMC.Common.Parts
{
    public class XyzLDzzxzULzzxzStage : XyStage
    {
        #region Define

        [Serializable]
        public new enum MotionKey
        {
            X,
            Y,
            Z,
            MASK_Y,
            LD_SZ0,
            LD_SZ1,
            LD_TRX,
            LD_TRZ,
            ALN_X,
            ALN_Y,
            UL_SZ0,
            UL_SZ1,
            UL_TRX,
            UL_TRZ
        }

        [Serializable]
        public enum AlarmKeys
        {
            eOutOfInterLockRangeX = -10,
            eOutOfInterLockRangeY = -11,
        }

        #endregion

        #region Constructor
        public XyzLDzzxzULzzxzStage(string strName) : base(strName)
        {
            this.CenterCoordinate = new XyzLDzzxzULzzxzCoordinate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }
        #endregion

        #region Property
        public XyzLDzzxzULzzxzCoordinate CenterCoordinate { get; set; }
        public Camera Camera { get; set; }
        public XyzLDzzxzULzzxzStageConfig Config { get; set; }
        public NeedleBlock NeedleBlock { get; set; }
        #endregion

        #region MotionPart Members
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

        public override int Create()
        {
            int ret = 0;

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
            m_dicAxisDisplayType.Add(MotionKey.Z.ToString(), DisplayAxisType.Vertical);
            m_dicAxisDisplayType.Add(MotionKey.MASK_Y.ToString(), DisplayAxisType.Vertical);
            m_dicAxisDisplayType.Add(MotionKey.LD_SZ0.ToString(), DisplayAxisType.CombinationStacker0);
            m_dicAxisDisplayType.Add(MotionKey.LD_SZ1.ToString(), DisplayAxisType.CombinationStacker1);
            m_dicAxisDisplayType.Add(MotionKey.LD_TRX.ToString(), DisplayAxisType.CombinationHorizontal);            
            m_dicAxisDisplayType.Add(MotionKey.LD_TRZ.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.ALN_X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.ALN_Y.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.UL_SZ0.ToString(), DisplayAxisType.CombinationStacker0);
            m_dicAxisDisplayType.Add(MotionKey.UL_SZ1.ToString(), DisplayAxisType.CombinationStacker1);
            m_dicAxisDisplayType.Add(MotionKey.UL_TRX.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.UL_TRZ.ToString(), DisplayAxisType.CombinationVertical);

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

            //if((ret = base.Initialize())!=0)
            //{
            //    return ret;
            //}

            Axes[MotionKey.Z.ToString()].Initialize();
            Axes[MotionKey.LD_SZ0.ToString()].Initialize();
            Axes[MotionKey.LD_SZ1.ToString()].Initialize();
            Axes[MotionKey.LD_TRZ.ToString()].Initialize();
            Axes[MotionKey.UL_SZ0.ToString()].Initialize();
            Axes[MotionKey.UL_SZ1.ToString()].Initialize();
            Axes[MotionKey.UL_TRZ.ToString()].Initialize();

            Task<int> task = Task.Factory.StartNew(() =>
            {
                if ((ret = Axes[MotionKey.X.ToString()].Initialize()) != 0) return ret;
                if ((ret = Axes[MotionKey.Y.ToString()].Initialize()) != 0) return ret;
                if ((ret = Axes[MotionKey.LD_TRX.ToString()].Initialize()) != 0) return ret;
                if ((ret = Axes[MotionKey.UL_TRX.ToString()].Initialize()) != 0) return ret;
                return ret;
            });

            if (task != null)
                task.Wait();

            return ret;
        }
        #endregion

        #region Method
        public int MovePosition(XyzLDzzxzULzzxzCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            dicMovingProjection[MotionKey.Z.ToString()].Position = coordinate.Z;
            dicMovingProjection[MotionKey.MASK_Y.ToString()].Position = coordinate.Z;
            dicMovingProjection[MotionKey.LD_SZ0.ToString()].Position = coordinate.LD_SZ0;
            dicMovingProjection[MotionKey.LD_SZ1.ToString()].Position = coordinate.LD_SZ1;
            dicMovingProjection[MotionKey.LD_TRX.ToString()].Position = coordinate.LD_TRX;
            dicMovingProjection[MotionKey.LD_TRZ.ToString()].Position = coordinate.LD_TRZ;
            dicMovingProjection[MotionKey.UL_SZ0.ToString()].Position = coordinate.UL_SZ0;
            dicMovingProjection[MotionKey.UL_SZ1.ToString()].Position = coordinate.UL_SZ1;
            dicMovingProjection[MotionKey.UL_TRX.ToString()].Position = coordinate.UL_TRX;
            dicMovingProjection[MotionKey.UL_TRZ.ToString()].Position = coordinate.UL_TRZ;

            return Move(dicMovingProjection);
        }

        //public int MovePosition(UvwCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.U.ToString()].Position = coordinate.U;
        //    dicMovingProjection[MotionKey.V.ToString()].Position = coordinate.V;
        //    dicMovingProjection[MotionKey.W.ToString()].Position = coordinate.W;

        //    return Move(dicMovingProjection);
        //}

        //public int MovePosition(XyzCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
        //    dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
        //    dicMovingProjection[MotionKey.VZ.ToString()].Position = coordinate.Z;

        //    return Move(dicMovingProjection);
        //}

        //public int MovePosition(XyCoordinate coordinate)
        //{
        //    if (m_Status == RunStatus.Stop) return 1;

        //    XyCoordinate command = new XyCoordinate();
        //    if (Interpolator != null)
        //    {
        //        Interpolator.Interpolate(coordinate, ref command);
        //    }
        //    else
        //    {
        //        command = coordinate;
        //    }

        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
        //    dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
        //    //dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.X;
        //    //dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.Y;

        //    return Move(dicMovingProjection);
        //}

        public Task<int> BeginMovePosition(XyzCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            dicMovingProjection[MotionKey.Z.ToString()].Position = coordinate.Z;

            return BeginMove(dicMovingProjection);
        }

        public Task<int> BeginMovePosition(XyzLDzzxzULzzxzCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            dicMovingProjection[MotionKey.Z.ToString()].Position = coordinate.Z;
            dicMovingProjection[MotionKey.LD_SZ0.ToString()].Position = coordinate.LD_SZ0;
            dicMovingProjection[MotionKey.LD_SZ1.ToString()].Position = coordinate.LD_SZ1;
            dicMovingProjection[MotionKey.LD_TRX.ToString()].Position = coordinate.LD_TRX;
            dicMovingProjection[MotionKey.LD_TRZ.ToString()].Position = coordinate.LD_TRZ;
            dicMovingProjection[MotionKey.UL_SZ0.ToString()].Position = coordinate.UL_SZ0;
            dicMovingProjection[MotionKey.UL_SZ1.ToString()].Position = coordinate.UL_SZ1;
            dicMovingProjection[MotionKey.UL_TRX.ToString()].Position = coordinate.UL_TRX;
            dicMovingProjection[MotionKey.UL_TRZ.ToString()].Position = coordinate.UL_TRZ;

            return BeginMove(dicMovingProjection);
        }

        public int GetCommandPosition(ref XyzLDzzxzULzzxzCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XyzLDzzxzULzzxzCoordinate();

            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetCommandPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Y = dPos;
            m_dicAxes[MotionKey.Z.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Z = dPos;
            m_dicAxes[MotionKey.LD_SZ0.ToString()].GetCommandPosition(ref dPos);
            currentPosition.LD_SZ0 = dPos;
            m_dicAxes[MotionKey.LD_SZ1.ToString()].GetCommandPosition(ref dPos);
            currentPosition.LD_SZ1 = dPos;
            m_dicAxes[MotionKey.LD_TRX.ToString()].GetCommandPosition(ref dPos);
            currentPosition.LD_TRX = dPos;
            m_dicAxes[MotionKey.LD_TRZ.ToString()].GetCommandPosition(ref dPos);
            currentPosition.LD_TRZ = dPos;
            m_dicAxes[MotionKey.UL_SZ0.ToString()].GetCommandPosition(ref dPos);
            currentPosition.UL_SZ0 = dPos;
            m_dicAxes[MotionKey.UL_SZ1.ToString()].GetCommandPosition(ref dPos);
            currentPosition.UL_SZ1 = dPos;
            m_dicAxes[MotionKey.UL_TRX.ToString()].GetCommandPosition(ref dPos);
            currentPosition.UL_TRX = dPos;
            m_dicAxes[MotionKey.UL_TRZ.ToString()].GetCommandPosition(ref dPos);
            currentPosition.UL_TRZ = dPos;
            return ret;
        }

        public int GetCommandPosition(ref XyzCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XyzCoordinate();

            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetCommandPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Y = dPos;
            m_dicAxes[MotionKey.Z.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Z = dPos;

            return ret;
        }

        //public int GetCommandPosition(ref XyCoordinate currentPosition)
        //{
        //    int ret = 0;

        //    if (currentPosition == null)
        //        currentPosition = new XyCoordinate();

        //    double dPos = 0;
        //    m_dicAxes[MotionKey.X.ToString()].GetCommandPosition(ref dPos);
        //    currentPosition.X = dPos;
        //    m_dicAxes[MotionKey.Y.ToString()].GetCommandPosition(ref dPos);
        //    currentPosition.Y = dPos;

        //    return ret;
        //}

        public int GetActualPosition(ref XyzLDzzxzULzzxzCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XyzLDzzxzULzzxzCoordinate();

            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetActualPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetActualPosition(ref dPos);
            currentPosition.Y = dPos;
            m_dicAxes[MotionKey.Z.ToString()].GetActualPosition(ref dPos);
            currentPosition.Z = dPos;
            m_dicAxes[MotionKey.LD_SZ0.ToString()].GetActualPosition(ref dPos);
            currentPosition.LD_SZ0 = dPos;
            m_dicAxes[MotionKey.LD_SZ1.ToString()].GetActualPosition(ref dPos);
            currentPosition.LD_SZ1 = dPos;
            m_dicAxes[MotionKey.LD_TRX.ToString()].GetActualPosition(ref dPos);
            currentPosition.LD_TRX = dPos;
            m_dicAxes[MotionKey.LD_TRZ.ToString()].GetActualPosition(ref dPos);
            currentPosition.LD_TRZ = dPos;
            m_dicAxes[MotionKey.UL_SZ0.ToString()].GetActualPosition(ref dPos);
            currentPosition.UL_SZ0 = dPos;
            m_dicAxes[MotionKey.UL_SZ1.ToString()].GetActualPosition(ref dPos);
            currentPosition.UL_SZ1 = dPos;
            m_dicAxes[MotionKey.UL_TRX.ToString()].GetActualPosition(ref dPos);
            currentPosition.UL_TRX = dPos;
            m_dicAxes[MotionKey.UL_TRZ.ToString()].GetActualPosition(ref dPos);
            currentPosition.UL_TRZ = dPos;

            return ret;
        }

        public int GetActualPosition(ref XyzCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XyzCoordinate();
            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetActualPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetActualPosition(ref dPos);
            currentPosition.Y = dPos;
            m_dicAxes[MotionKey.Z.ToString()].GetActualPosition(ref dPos);
            currentPosition.Z = dPos;

            return ret;
        }

        public int GetAxisCount()
        {
            return m_dicAxes.Count;
        }

        public bool CheckLimit(MotionKey axis, double dPosition)
        {
            bool bRet = false;
            MotionAxis motion = m_dicAxes[axis.ToString()];
            if (motion != null)
            {
                bRet = motion.Motor.CheckLimit(dPosition);
            }

            return bRet;
        }

        public override void UpdateConfigData()
        {
            base.UpdateConfigData();

            if (m_dicAxes[MotionKey.X.ToString()] != null)
            {
                m_dicAxes[MotionKey.X.ToString()].Direction = MotionDirection.Backward;
            }
        }

        public void UpdateDirection()
        {
            //  2023. 08. 04.  SSY : Jog 버튼의 Z 축 이동 방향을 변경하기 위해 추가됨.

            //m_dicAxes[MotionKey.EZ.ToString()].Direction = MotionDirection.Backward;
            //m_dicAxes[MotionKey.VZ.ToString()].Direction = MotionDirection.Backward;
        }
        #endregion
    }
}