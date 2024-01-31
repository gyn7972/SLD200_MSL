using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    // 제품을 나타내는 작은 단위
    public class Unit
    {
        private XytCoordinate m_InspectionPositionOnLoadSubstrate;
        private XytCoordinate m_CompensationOffsetOnRevision;
        public XytCoordinate InspectionPositionOnLoadSubstrate 
        {
            set { m_InspectionPositionOnLoadSubstrate = value; } 
            get { return m_InspectionPositionOnLoadSubstrate; }
        }

        public XytCoordinate CompensationOffsetOnRevision
        {
            set { m_CompensationOffsetOnRevision = value; }
            get { return m_CompensationOffsetOnRevision; }
        }
        public double X 
        {
            get
            {
                return m_InspectionPositionOnLoadSubstrate.X;
            }
            set
            {
                m_InspectionPositionOnLoadSubstrate.X = value;
            }
        }

        public double Y
        {
            get
            {
                return m_InspectionPositionOnLoadSubstrate.Y;
            }
            set
            {
                m_InspectionPositionOnLoadSubstrate.Y = value;
            }
        }

        public double T
        {
            get
            {
                return m_InspectionPositionOnLoadSubstrate.T;
            }
            set
            {
                m_InspectionPositionOnLoadSubstrate.T = value;
            }
        }

        public SizeD Size { set; get; }
        public string ObjId { set; get; }
        public bool IsFind { set; get; }
        //public Collet Collet { set; get; }
        public Unit() : this("")
        {
        }
        public Unit(string strObjId)
        {
            ObjId = strObjId;
            InspectionPositionOnLoadSubstrate = new XytCoordinate();
            CompensationOffsetOnRevision = new XytCoordinate();
            IsFind = false;
            Size = new SizeD();
        }
    }
}
