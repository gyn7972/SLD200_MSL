using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class MaterialPitch
    {
        private double m_Pitch;
        private double m_Count;

        public double Pitch
        {
            get { return m_Pitch; }
            set { m_Pitch = value; }
        }
        public double Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }
        public MaterialPitch()
        {
            m_Pitch = 0;
            m_Count = 0;
        }
    }     
}
