using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public  class MaterialCompensatorResult
    {
        private XytCoordinateCollection m_SearchPositions;

        public XytCoordinateCollection SearchPositions
        {
            get { return m_SearchPositions; }
            set { m_SearchPositions = value; }
        }

        public MaterialCompensatorResult()
        {
            m_SearchPositions = new XytCoordinateCollection();
        }
    }
}
