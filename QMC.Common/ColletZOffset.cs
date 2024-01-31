using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class ColletZOffset
    {
        public int nColletIndex { get; set; }
        public double dOffsetZ { get; set; }
        public ColletZOffset()
        {
            nColletIndex = 0;
            dOffsetZ = 0;
        }
    }
}
