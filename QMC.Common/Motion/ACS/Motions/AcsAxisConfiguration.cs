using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Motion.ACS.Motion
{
    public class AcsAxisConfiguration : MotionAxisConfiguration
    {
        public int m_nAxisCount;
        public List<AxisParam> ListAxisParam { get; set; }
        public AcsAxisConfiguration() : base()
        {
            m_nAxisCount = 2; // X, Y
            ListAxisParam = new List<AxisParam>(m_nAxisCount);


        }

        public int LoadAxisParam()
        {
            int ret = 0;
            
            return ret;
        }
    }
}
