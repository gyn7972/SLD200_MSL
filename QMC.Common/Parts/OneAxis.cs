using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class OneAxis : MotionPart
    {
        #region Deifne
        public enum MotionKey
        {
            Z,
        }
        #endregion

        #region Constructor
        public OneAxis(string strName) : base(strName)
        {

        }
        #endregion

        #region MotionPart Members
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

        public Task<int> BeginMovePosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.Z.ToString()].Position = dPosition;

            return BeginMove(dicMovingProjection);
        }
        #endregion

    }
}
