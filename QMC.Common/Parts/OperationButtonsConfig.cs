using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class OperationButtonsConfig
    {
        public int OperationButtonResponseTimeOut { get; set; }
        public OperationButtonsConfig()
        {
            OperationButtonResponseTimeOut = 3000;
        }

    }
}
