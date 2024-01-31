using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class ColletOffset
    {
        private XyCoordinate m_Offset;
        public string Name { set; get; }
        [Browsable(false)]
        public XyCoordinate Offset 
        { 
            set
            {
                m_Offset = value;
            }
            get
            {
                return m_Offset;
            }
        }

        public double OffsetX
        {
            set { m_Offset.X = value; }
            get { return m_Offset.X; }
        }

        public double OffsetY
        {
            set { m_Offset.Y = value; }
            get { return m_Offset.Y; }
        }

        public ColletOffset()
        {
            m_Offset = new XyCoordinate();
            Name = "";
        }

        public ColletOffset(string strName, double dOffsetX, double dOffsetY)
        {
            m_Offset = new XyCoordinate();
            Name = strName;
            OffsetX = dOffsetX;
            OffsetY = dOffsetY;
        }
        public ColletOffset(string strName, XyCoordinate offset) : this(strName, offset.X, offset.Y)
        {
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_Offset == null)
                m_Offset = new XyCoordinate();
            if (parameters != null && parameters.Count >= 2)
            {
                m_Offset.X = parameters[0].DoubleValue;
                m_Offset.Y = parameters[1].DoubleValue;
            }
        }
    }
}
