using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public abstract class BaseRecipe : ParamContainer
    {

        public virtual List<object> GetPositions()
        {
            return null;
        }

        public virtual IlluminationDataList GetIlluminationDatas()
        {
            return null;
        }
    }
}
