using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class LoadZ : MotionPart
    {

        protected List<ZPositionData> m_ZPosition;

        [Serializable]
        public enum MotionKey
        {
            Z,
        }
        public LoadZConfig LoadZConfig { get; set; }

        public ColletZCalibrator ColletZCalibrator { get; set; }
        public int PitchCount { get; set; }
        public LoadZ(string strName) : base(strName)
        {
            m_ZPosition = new List<ZPositionData>();
            LoadZConfig = new LoadZConfig();
            PitchCount = 3;
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
            m_dicAxisDisplayType.Add(MotionKey.Z.ToString(), DisplayAxisType.Vertical);

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void UpdateConfigData() //참고 : Override
        {
            DieTransfer dieTransfer = Owner as DieTransfer;
            if (Owner != null) // 참고 : Loader인지 Unloader인지 parsing
            {
                LoadZConfig = dieTransfer.Config.LoadZConfig;

                if (LoadZConfig.m_listCalibrationpitch == null) //참고 : 저장안될때.. Data null인지 꼭 확인
                    LoadZConfig.m_listCalibrationpitch = new List<CalibrationPitch>();

                if (LoadZConfig.m_ListOffsetZ != null)
                    LoadZConfig.m_ListOffsetZ = new List<ColletZOffset>();
            }
        }
        #endregion

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
