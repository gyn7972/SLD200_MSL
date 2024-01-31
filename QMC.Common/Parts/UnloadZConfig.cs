using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class UnloadZConfig
    {
       
        
        [Serializable]
        public enum PositionUnloadZ
        {
            GoBack,
            Place,
        }
        protected ZPositionDataCollection m_UnloadZPosition;
        public ZPositionDataCollection UnloadZPosition
        {
            get { return m_UnloadZPosition; }
            set { m_UnloadZPosition = value; }
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

        public UnloadZConfig()
        {
            m_UnloadZPosition = new ZPositionDataCollection();
            m_listCalibrationpitch = new List<CalibrationPitch>();
            Init();
        }
        protected void Init()
        {
            m_UnloadZPosition.Clear();

            foreach (PositionUnloadZ key in Enum.GetValues(typeof(PositionUnloadZ)))
            {
                ZPositionData positionBase = new ZPositionData();
                positionBase.Name = key.ToString();
                m_UnloadZPosition.Add(positionBase);

                ZPositionData positionTarget = new ZPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                m_UnloadZPosition.Add(positionTarget);
            }
        }
        public List<string> GetLoadZPositionList()
        {
            List<string> ret = new List<string>();
            foreach (ZPositionData position in m_UnloadZPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }
        public void SetCapPosition(string strPositionKey, TargetType targetType, double dPosition)
        {
            foreach (ZPositionData position in m_UnloadZPosition)
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
            foreach (ZPositionData position in m_UnloadZPosition)
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
            foreach (ZPositionData position in m_UnloadZPosition)
            {
                if (i >= parameters.Count)
                    return i;
                position.Z = parameters[i++].DoubleValue;
            }
            return i;
        }

    }
}
