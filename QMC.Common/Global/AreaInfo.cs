using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class AreaInfo
    {
        public int AreaIndex { get; set; }
        public bool IsProcessed { get; set; }
        public string Note { get; set; }

        public AreaInfo(int areaIndex)
        {
            AreaIndex = areaIndex;
            IsProcessed = false;
            Note = string.Empty;
        }

        public void Reset()
        {
            IsProcessed = false;
            Note = string.Empty;
        }
    }
}
