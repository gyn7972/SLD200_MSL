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

        /// <summary>
        /// 0: 자재 있음, 1: 작업 중, 2: 완료
        /// </summary>
        public int ProcessStatus { get; set; } = 0;

        public string Note { get; set; } = string.Empty;

        public AreaInfo(int areaIndex)
        {
            AreaIndex = areaIndex;
            ProcessStatus = 0;
            Note = string.Empty;
        }

        public void Reset()
        {
            ProcessStatus = 0;
            Note = string.Empty;
        }

        public bool IsProcessed => ProcessStatus == 2;
    }
}
