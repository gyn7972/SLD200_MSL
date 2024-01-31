using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class ProductionData
    {
        #region Constructor
        public ProductionData()
        {
            TotalCount = 0;
            OKCount = 0;
            NGCount = 0;
        }
        #endregion

        #region Property
        public int TotalCount { get; set; }
        public int OKCount { get; set; }
        public int NGCount { get; set; }

        public double TactTime { set; get; }

        public DateTime StartTime { set; get; }
        #endregion

        public virtual void Reset()
        {
            TotalCount = 0;
            OKCount = 0;
            NGCount = 0;
            TactTime = 0;
        }
    }
}
