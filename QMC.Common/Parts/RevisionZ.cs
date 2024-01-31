using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class RevisionZ : MotionPart
    {
        public enum MotionKey
        {
            Z,
        }
        public RevisionZ(string strName) : base(strName)
        {

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
            m_dicAxisDisplayType.Add(MotionKey.Z.ToString(), DisplayAxisType.Vertical);
            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override int Initialize()
        {
            int ret = 0;
            ret = base.Initialize();

            return ret;
        }

        public int MovePosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.Z.ToString()].Position = dPosition;

            return Move(dicMovingProjection);
        }

        protected Dictionary<string, MovingProjection> GetDefaultMovingProjections()
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
