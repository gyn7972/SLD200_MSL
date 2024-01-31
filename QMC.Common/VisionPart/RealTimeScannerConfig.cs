using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class RealTimeScannerConfig
    {
        public SizeD AreaMargin { set; get; }
        public double AreaPercent { set; get; }

        public bool AlignAngleEnabled { set; get; }

        public bool AngleFullRoiCheck { set; get; }

        public double RangeTolerance { set; get; }
        public double CheckTolerance { set; get; }

        public double AngleLimit { set; get; }

        public bool CheckArea { set; get; }
    }
}
