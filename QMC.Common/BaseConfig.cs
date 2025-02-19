using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public abstract class BaseConfig : ParamContainer
    {
        public virtual bool IsConfigForm()
        {
            return true;
        }

        public abstract List<object> GetPositions();
        public virtual List<IlluminationChannel> GetIlluminationChannels()
        {
            return null;
        }

        public virtual void SetIlluminationChannels(List<IlluminationChannel> illuminationChannels)
        {

        }
    }
}
