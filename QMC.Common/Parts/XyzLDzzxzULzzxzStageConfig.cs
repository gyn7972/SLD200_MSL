using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class XyzLDzzxzULzzxzStageConfig
    {

        public string FileName { set; get; }


        public bool Use2DMap { set; get; }

        //public XyztStageConfig(string owner)
        public XyzLDzzxzULzzxzStageConfig()
        {
            //Init(owner);
            Init();
        }

        //public void Init(string owner)
        public void Init()
        {
            this.Use2DMap = false;

        }
    }
}
