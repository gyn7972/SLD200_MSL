using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public enum TargetType
    {
        Base,
        Offset,
    }


    [Serializable]
    public class UvwzxyzPositionData
    {
        protected UvwzxyzCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public UvwzxyzCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }
        public double U
        {
            set
            {
                m_coordinate.U = value;
            }
            get
            {
                return m_coordinate.U;
            }
        }
        public double V
        {
            set
            {
                m_coordinate.V = value;
            }
            get
            {
                return m_coordinate.V;
            }
        }
        public double W
        {
            set
            {
                m_coordinate.W = value;
            }
            get
            {
                return m_coordinate.W;
            }
        }
        public double EZ
        {
            set
            {
                m_coordinate.EZ = value;
            }
            get
            {
                return m_coordinate.EZ;
            }
        }
        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }
        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }        
        public double VZ
        {
            set
            {
                m_coordinate.VZ = value;
            }
            get
            {
                return m_coordinate.VZ;
            }
        }

        public UvwzxyzPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new UvwzxyzCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new UvwzxyzCoordinate();
            if (parameters != null && parameters.Count >= 7)
            {
                m_coordinate.U = parameters[0].DoubleValue;
                m_coordinate.V = parameters[1].DoubleValue;
                m_coordinate.W = parameters[2].DoubleValue;
                m_coordinate.EZ = parameters[3].DoubleValue;
                m_coordinate.X = parameters[4].DoubleValue;
                m_coordinate.Y = parameters[5].DoubleValue;
                m_coordinate.VZ = parameters[6].DoubleValue;
            }


        }
    }
    [Serializable]
    public class UvwzxyzPositionDataCollection : Collection<UvwzxyzPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (UvwzxyzPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<UvwzxyzPositionData> GetPositionDatas(string strName)
        {
            List<UvwzxyzPositionData> list = new List<UvwzxyzPositionData>();
            foreach (UvwzxyzPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public UvwzxyzCoordinate GetPositionCoordinate(string strName)
        {
            UvwzxyzCoordinate coord = new UvwzxyzCoordinate();
            List<UvwzxyzPositionData> list = GetPositionDatas(strName);
            if (list != null && list.Count > 0)
            {
                foreach (UvwzxyzPositionData data in list)
                {
                    coord += data.Coordinate;
                }
            }
            return coord;
        }

        public XyCoordinate GetPositionCoordinate_UVW(string strName)
        {
            XyCoordinate coord = new XyCoordinate();
            List<UvwzxyzPositionData> list = GetPositionDatas(strName);
            if (list != null && list.Count > 0)
            {
                foreach (UvwzxyzPositionData data in list)
                {
                    coord.X += data.Coordinate.U;
                    coord.Y += data.Coordinate.V;
                }
            }
            return coord;
        }
    }



    [Serializable]
    public class XyzztPositionData
    {
        protected XyzztCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XyzztCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }
        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }
        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }
        public double IZ
        {
            set
            {
                m_coordinate.IZ = value;
            }
            get
            {
                return m_coordinate.IZ;
            }
        }
        public double SZ
        {
            set
            {
                m_coordinate.SZ = value;
            }
            get
            {
                return m_coordinate.SZ;
            }
        }
        public double T
        {
            set
            {
                m_coordinate.T = value;
            }
            get
            {
                return m_coordinate.T;
            }
        }

        public XyzztPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XyzztCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XyzztCoordinate();
            if (parameters != null && parameters.Count >= 5)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.Y = parameters[1].DoubleValue;
                m_coordinate.IZ = parameters[2].DoubleValue;
                m_coordinate.SZ = parameters[3].DoubleValue;
                m_coordinate.T = parameters[4].DoubleValue;
            }


        }
    }
    [Serializable]
    public class XyzztPositionDataCollection : Collection<XyzztPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XyzztPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XyzztPositionData> GetPositionDatas(string strName)
        {
            List<XyzztPositionData> list = new List<XyzztPositionData>();
            foreach (XyzztPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public XyzztCoordinate GetPositionCoordinate(string strName)
        {
            XyzztCoordinate coord = new XyzztCoordinate();
            List<XyzztPositionData> list = GetPositionDatas(strName);
            if (list != null && list.Count > 0)
            {
                foreach (XyzztPositionData data in list)
                {
                    coord += data.Coordinate;
                }
            }
            return coord;
        }
    }


    [Serializable]
    public class XryztPositionData
    {
        protected XryztCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XryztCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }

        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }

        public double R
        {
            set
            {
                m_coordinate.R = value;
            }
            get
            {
                return m_coordinate.R;
            }
        }

        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }
        public double Z
        {
            set
            {
                m_coordinate.Z = value;
            }
            get
            {
                return m_coordinate.Z;
            }
        }
        public double T
        {
            set
            {
                m_coordinate.T = value;
            }
            get
            {
                return m_coordinate.T;
            }
        }

        public XryztPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XryztCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XryztCoordinate();
            if (parameters != null && parameters.Count >= 5)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.R = parameters[1].DoubleValue;
                m_coordinate.Y = parameters[2].DoubleValue;
                m_coordinate.Z = parameters[3].DoubleValue;
                m_coordinate.T = parameters[4].DoubleValue;
            }


        }
    }
    [Serializable]
    public class XryztPositionDataCollection : Collection<XryztPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XryztPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XryztPositionData> GetPositionDatas(string strName)
        {
            List<XryztPositionData> list = new List<XryztPositionData>();
            foreach (XryztPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }
    }

    [Serializable]
    public class XyztPositionData
    {
        protected XyztCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XyztCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }

        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }

        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }
        public double Z
        {
            set
            {
                m_coordinate.Z = value;
            }
            get
            {
                return m_coordinate.Z;
            }
        }
        public double T
        {
            set
            {
                m_coordinate.T = value;
            }
            get
            {
                return m_coordinate.T;
            }
        }

        public XyztPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XyztCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XyztCoordinate();
            if (parameters != null && parameters.Count >= 4)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.Y = parameters[1].DoubleValue;
                m_coordinate.T = parameters[2].DoubleValue;
                m_coordinate.Z = parameters[3].DoubleValue;
            }


        }
    }
    [Serializable]
    public class XyztPositionDataCollection : Collection<XyztPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XyztPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XyztPositionData> GetPositionDatas(string strName)
        {
            List<XyztPositionData> list = new List<XyztPositionData>();
            foreach (XyztPositionData data in this)
            {
                if(data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public XyztCoordinate GetPositionCoordinate(string strName)
        {
            XyztCoordinate coord = new XyztCoordinate();
            List<XyztPositionData> list = GetPositionDatas(strName);
            if (list != null && list.Count > 0)
            {
                foreach (XyztPositionData data in list)
                {
                    coord += data.Coordinate;
                }
            }
            return coord;
        }
    }
    [Serializable]
    public class XytPositionData
    {
        protected XytCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XytCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }

        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }

        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }

        public double T
        {
            set
            {
                m_coordinate.T = value;
            }
            get
            {
                return m_coordinate.T;
            }
        }
        public XytPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XytCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XytCoordinate();
            if (parameters != null && parameters.Count >= 3)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.Y = parameters[1].DoubleValue;
                m_coordinate.T = parameters[2].DoubleValue;
            }
        }
    }
    [Serializable]
    public class XytPositionDataCollection : Collection<XytPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XytPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XytPositionData> GetPositionDatas(string strName)
        {
            List<XytPositionData> list = new List<XytPositionData>();
            foreach (XytPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public XytCoordinate GetPositionCoordinate(string strName)
        {
            XytCoordinate coord = new XytCoordinate();
            List<XytPositionData> list = GetPositionDatas(strName);
            if(list != null && list.Count > 0)
            {
                foreach(XytPositionData data in list)
                {
                    coord += data.Coordinate;
                }
            }
            return coord;
        }
    }
    [Serializable]
    public class XyzPositionData
    {
        protected XyzCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XyzCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }

        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }

        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }

        public double Z
        {
            set
            {
                m_coordinate.Z = value;
            }
            get
            {
                return m_coordinate.Z;
            }
        }
        public XyzPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XyzCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XyzCoordinate();
            if (parameters != null && parameters.Count >= 3)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.Y = parameters[1].DoubleValue;
                m_coordinate.Z = parameters[2].DoubleValue;
            }

        }
    }
    [Serializable]
    public class XytzPositionData
    {
        protected XyztCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XyztCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }

        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }

        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }

        public double Z
        {
            set
            {
                m_coordinate.Z = value;
            }
            get
            {
                return m_coordinate.Z;
            }
        }

        public double T
        {
            set
            {
                m_coordinate.T = value;
            }
            get
            {
                return m_coordinate.T;
            }
        }
        public XytzPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XyztCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XyztCoordinate();
            if (parameters != null && parameters.Count >= 4)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.Y = parameters[1].DoubleValue;
                m_coordinate.T = parameters[2].DoubleValue;
                m_coordinate.Z = parameters[3].DoubleValue;
            }

        }
    }
    [Serializable]
    public class XyzPositionDataCollection : Collection<XyzPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XyzPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XyzPositionData> GetPositionDatas(string strName)
        {
            List<XyzPositionData> list = new List<XyzPositionData>();
            foreach (XyzPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }
    }
    [Serializable]
    public class XytzPositionDataCollection : Collection<XytzPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XytzPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XytzPositionData> GetPositionDatas(string strName)
        {
            List<XytzPositionData> list = new List<XytzPositionData>();
            foreach (XytzPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }
    }
    [Serializable]
    public class XyPositionData
    {
        protected XyCoordinate m_coordinate;
        public string Name { get; set; }
        public TargetType Type { set; get; }
        [Browsable(false)]
        public XyCoordinate Coordinate
        {
            set
            {
                m_coordinate = value;
            }
            get
            {
                return m_coordinate;
            }
        }

        public double X
        {
            set
            {
                m_coordinate.X = value;
            }
            get
            {
                return m_coordinate.X;
            }
        }

        public double Y
        {
            set
            {
                m_coordinate.Y = value;
            }
            get
            {
                return m_coordinate.Y;
            }
        }

        public XyPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Coordinate = new XyCoordinate();
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (m_coordinate == null)
                m_coordinate = new XyCoordinate();
            if (parameters != null && parameters.Count >= 2)
            {
                m_coordinate.X = parameters[0].DoubleValue;
                m_coordinate.Y = parameters[1].DoubleValue;
            }
        }
    }
    [Serializable]
    public class XyPositionDataCollection : Collection<XyPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (XyPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<XyPositionData> GetPositionDatas(string strName)
        {
            List<XyPositionData> list = new List<XyPositionData>();
            foreach (XyPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }
    }
    [Serializable]
    public class ZPositionData
    {
        protected double m_dPosition;
        public string Name { get; set; }
        public TargetType Type { set; get; }

        public double Z
        {
            set
            {
                m_dPosition = value;
            }
            get
            {
                return m_dPosition;
            }
        }

        public ZPositionData()
        {
            Name = "";
            Type = TargetType.Base;
            Z = 0;
        }

        public void SetData(SettingParameterCollection parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                Z = parameters[0].DoubleValue;
            }


        }
    }
    [Serializable]
    public class ZPositionDataCollection : Collection<ZPositionData>
    {
        public List<string> GetPositionList()
        {
            List<string> list = new List<string>();
            foreach (ZPositionData data in this)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }
            return list;
        }

        public List<ZPositionData> GetPositionDatas(string strName)
        {
            List<ZPositionData> list = new List<ZPositionData>();
            foreach (ZPositionData data in this)
            {
                if (data.Name == strName)
                {
                    list.Add(data);
                }
            }
            return list;
        }
    }
}
