using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class ColletCountData
    {
        public int ColletIndex { protected set; get; }
        public int MissCount { set; get; }
        public int TotalMissCount { set; get; }
        public int TotalCount { set; get; }

        public ColletCountData(int nColletIndex)
        {
            ColletIndex = nColletIndex;
            MissCount = 0;
            TotalMissCount = 0;
            TotalCount = 0;
        }

        public void AllClear()
        {
            MissCount = 0;
            TotalMissCount = 0;
            TotalCount = 0;
        }

        public void ClearMissCount()
        {
            MissCount = 0;
            TotalMissCount = 0;
        }
    }

    public class ColletCountDataList : List<ColletCountData>
    {
        public void AllClear(int nColletIndex)
        {
            foreach(ColletCountData data in this)
            {
                if(data.ColletIndex == nColletIndex)
                {
                    data.AllClear();
                    break;
                }
            }
        }

        public void ClearMissCount(int nColletIndex)
        {
            foreach (ColletCountData data in this)
            {
                if (data.ColletIndex == nColletIndex)
                {
                    data.ClearMissCount();
                    break;
                }
            }
        }
    }
}
