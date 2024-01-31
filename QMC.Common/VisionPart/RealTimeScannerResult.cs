using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Vision.Tools;

namespace QMC.Common.VisionPart
{
    public class RealTimeScannerResult
    {
        private XytCoordinateCollection m_SearchPositions;

        public XytCoordinateCollection SearchPositions
        {
            get { return m_SearchPositions; }
            set { m_SearchPositions = value; }
        }
        public PatternMatchingResult.ResultOverlayCollection ResultOverlays
        {
            get;
            set;
        }

        public RealTimeScannerResult()
        {
            m_SearchPositions = new XytCoordinateCollection();
        }
    }
}
