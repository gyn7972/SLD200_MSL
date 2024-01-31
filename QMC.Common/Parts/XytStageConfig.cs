using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class XytStageConfig
    {

        public string FileName { set; get; }


        public bool Use2DMap { set; get; }

        public XytStageConfig(string owner)
        {
            Init(owner);
        }

        public void Init(string owner)
        {
            this.Use2DMap = false;

        }
    }
}
