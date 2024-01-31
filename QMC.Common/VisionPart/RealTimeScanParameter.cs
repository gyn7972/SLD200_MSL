using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public class RealTimeScanParameter
    {
        public SizeD Pitch
        {
            get;
            set;
        }

        public PointD Reference
        {
            get;
            set;
        }

        public XytCoordinate CurrentPosition { set; get; }

        public PointD Resolution { set; get; }

        public bool SearchNextRow { set; get; }
        public RealTimeScanParameter()
        {
            Pitch = new SizeD();
            Reference = new PointD();
            CurrentPosition = new XytCoordinate();
            Resolution = new PointD();
            SearchNextRow = false;
        }
    }
}
