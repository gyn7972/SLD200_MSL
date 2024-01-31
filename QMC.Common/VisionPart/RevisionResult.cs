using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    #region RevisionResult
    [Serializable]
    public class RevisionResult
    {
        #region Field
        private bool m_Found;
        private XytCoordinate m_Position;
        private PointF m_PixelPos;
        #endregion

        #region Constructor
        public RevisionResult(XytCoordinate position)
        {
            this.Position = position;
            this.Found = false;
        }
        public RevisionResult() : this(new XytCoordinate()) { }
        #endregion

        #region Property
        public bool Found
        {
            get { return m_Found; }
            set { m_Found = value; }
        }

        public XytCoordinate Position
        {
            get { return this.m_Position; }
            set { this.m_Position = value; }
        }

        public PointF PixelPosition
        {
            get { return this.m_PixelPos; }
            set { this.m_PixelPos = value; }
        }
        #endregion

    }
    #endregion
}
