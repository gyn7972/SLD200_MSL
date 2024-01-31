using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class LoadZConfig
    {
        [Serializable]
        public enum PositionLoadZ
        {
            GoBack,
            Pick,
        }
        protected ZPositionDataCollection m_LoadZPosition;
        public ZPositionDataCollection LoadZPosition
        {
            get { return m_LoadZPosition; }
            set { m_LoadZPosition = value; }
        }
        public List<CalibrationPitch> m_listCalibrationpitch { get; set; }
        public List<CalibrationPitch> listCalibrationpitch
        {
            get { return m_listCalibrationpitch; }
            set { m_listCalibrationpitch = value; }
        }

        public List<ColletZOffset> m_ListOffsetZ { get; set; }
        public List<ColletZOffset> listOffsetZ
        {
            get
            {
                return m_ListOffsetZ;
            }
            set
            {
                m_ListOffsetZ = value;
            }
        }

        public LoadZConfig()
        {
            m_LoadZPosition = new ZPositionDataCollection();
            m_listCalibrationpitch = new List<CalibrationPitch>();
            Init();
        }
        protected void Init()
        {
            m_LoadZPosition.Clear();

            foreach (PositionLoadZ key in Enum.GetValues(typeof(PositionLoadZ)))
            {
                ZPositionData positionBase = new ZPositionData();
                positionBase.Name = key.ToString();
                m_LoadZPosition.Add(positionBase);

                ZPositionData positionTarget = new ZPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                m_LoadZPosition.Add(positionTarget);
            }
        }
        public List<string> GetLoadZPositionList()
        {
            List<string> ret = new List<string>();
            foreach (ZPositionData position in m_LoadZPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }
        public void SetCapPosition(string strPositionKey, TargetType targetType, double dPosition)
        {
            foreach (ZPositionData position in m_LoadZPosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.Z = dPosition;
                    break;
                }
            }
        }
        public double GetCapPosition(string strPositionKey, TargetType targetType)
        {
            double dPosition = 0;
            foreach (ZPositionData position in m_LoadZPosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    dPosition = position.Z;
                    break;
                }
            }
            return dPosition;
        }
        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            foreach (ZPositionData position in m_LoadZPosition)
            {
                if (i >= parameters.Count)
                    return i;
                position.Z = parameters[i++].DoubleValue;
            }
            return i;
        }

    }
}
