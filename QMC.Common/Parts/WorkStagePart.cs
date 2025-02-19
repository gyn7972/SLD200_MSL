using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Modules;

namespace QMC.Common.Parts
{
    public class WorkStagePart : MotionPart
    {
        public enum MotionKey
        {
            Stage_X,
            Stage_Y,
            Stage_T,
            Stage_Z,
            Rva_X,
            Rva_Y,
        }
        public WorkStageConfig Config { get; set; }
        public XytStage Stage { get; set; }

        public WorkStagePart(string strName) : base(strName)
        {
            Config = new WorkStageConfig();
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
            m_dicAxisDisplayType.Add(MotionKey.Stage_X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.Stage_Y.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.Stage_T.ToString(), DisplayAxisType.Theta);
            m_dicAxisDisplayType.Add(MotionKey.Stage_Z.ToString(), DisplayAxisType.Vertical);

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
        #endregion

        protected override int OnBeforeMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;
            //Interlock

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
        protected Dictionary<string, MovingProjection> GetDefaultMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            {
                string strKey = MotionKey.Stage_X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.Stage_Y.ToString();
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

            dicMovingProjection[MotionKey.Stage_X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Stage_Y.ToString()].Position = coordinate.Y;
            
            return Move(dicMovingProjection);
        }

        public Task<int> BeginMovePosition(XyCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.Stage_X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Stage_Y.ToString()].Position = coordinate.Y;
            
            return BeginMove(dicMovingProjection);
        }
    }
}
