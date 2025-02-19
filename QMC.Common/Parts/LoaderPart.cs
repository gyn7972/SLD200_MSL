using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Modules;

namespace QMC.Common.Parts
{
    public class LoaderPart : MotionPart
    {
        public enum MotionKey
        {
            Z0,
            Z1,
            TR_X,
            TR_Z,
            ALN_X,
            ALN_Y,
        }
        public LoaderConfig Config { get; set; }
        public XytStage Stage { get; set; }

        public LoaderPart(string strName) : base(strName)
        {
            Config = new LoaderConfig();
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
            m_dicAxisDisplayType.Add(MotionKey.Z0.ToString(), DisplayAxisType.Vertical);
            m_dicAxisDisplayType.Add(MotionKey.Z1.ToString(), DisplayAxisType.Vertical);
            m_dicAxisDisplayType.Add(MotionKey.TR_X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.TR_Z.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.ALN_X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.ALN_Y.ToString(), DisplayAxisType.CombinationVertical);

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
                string strKey = MotionKey.Z0.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.Z1.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.TR_X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.TR_Z.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.ALN_X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.ALN_Y.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        //public int MovePosition(XyCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.Stage_X.ToString()].Position = coordinate.X;
        //    dicMovingProjection[MotionKey.Stage_Y.ToString()].Position = coordinate.Y;
            
        //    return Move(dicMovingProjection);
        //}

        //public Task<int> BeginMovePosition(XyCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.Stage_X.ToString()].Position = coordinate.X;
        //    dicMovingProjection[MotionKey.Stage_Y.ToString()].Position = coordinate.Y;
            
        //    return BeginMove(dicMovingProjection);
        //}
    }
}
