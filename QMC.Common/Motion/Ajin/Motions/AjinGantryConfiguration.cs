using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Motion.Ajin.AXM;

namespace QMC.Common.Motion.Ajin.Motions
{
    [Serializable]
    public class AjinGantryConfiguration
    {
        public GantryHomingMethods HomingMethod { set; get; }
        public int MasterAxisNo { set; get; }
        public int SlaveAxisNo { set; get; }
        public double SlaveRatio { set; get; }
        public double SlaveHomeOffset { set; get; }

        public AjinGantryConfiguration()
        {
            this.MasterAxisNo = 0;
            this.SlaveAxisNo = 0;
            this.SlaveRatio = 0.0;
            this.HomingMethod = GantryHomingMethods.OnlyMaster;
            this.SlaveHomeOffset = 0.0;
        }
    }
}
