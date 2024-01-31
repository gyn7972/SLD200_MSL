using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class XyStage : MotionPart
    {
        public enum MotionKey
        {
            X,
            Y,
        }

        public XyStage(string strName) : base(strName)
        {
        }

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
            ret = base.Initialize();

            return ret;
        }

        public override void Close()
        {
            base.Close();
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

        public int GetCommandPosition(ref XytCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XytCoordinate();

            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetCommandPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetCommandPosition(ref dPos);
            currentPosition.Y = dPos;

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

        public int GetActualPosition(ref XytCoordinate currentPosition)
        {
            int ret = 0;

            if (currentPosition == null)
                currentPosition = new XytCoordinate();
            double dPos = 0;
            m_dicAxes[MotionKey.X.ToString()].GetActualPosition(ref dPos);
            currentPosition.X = dPos;
            m_dicAxes[MotionKey.Y.ToString()].GetActualPosition(ref dPos);
            currentPosition.Y = dPos;

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
    }
}
