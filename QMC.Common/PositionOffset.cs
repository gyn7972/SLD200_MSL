using QMC.Common.VisionPart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    //[Serializable]
    //public struct PositionOffset : IComparable, IComparer
    //{
    //    private XyCoordinate m_Position;
    //    private XyCoordinate m_Offset;

    //    public PositionOffset(XyCoordinate position, XyCoordinate offset)
    //    {
    //        this.m_Position = position;
    //        this.m_Offset = offset;
    //    }

    //    public XyCoordinate Position
    //    {
    //        get { return this.m_Position; }
    //        set { this.m_Position = value; }
    //    }

    //    public XyCoordinate Offset
    //    {
    //        get { return this.m_Offset; }
    //        set { this.m_Offset = value; }
    //    }

    //    public override string ToString()
    //    {
    //        return string.Format("Position = {0}, Offset = {1}", this.Position, this.Offset);
    //    }

    //    #region IComparable Members
    //    public int CompareTo(object obj)
    //    {
    //        PositionOffset b = (PositionOffset)obj;

    //        if (this.Position.X == b.Position.X && this.Position.Y == b.Position.Y) return 0;

    //        else if (this.Position.X < b.Position.X)
    //            return -1;
    //        else if (this.Position.X > b.Position.X)
    //            return 1;
    //        else if (this.Position.Y < b.Position.Y)
    //            return -1;
    //        else
    //            return 1;
    //    }
    //    #endregion

    //    #region IComparer Members
    //    public int Compare(object x, object y)
    //    {
    //        PositionOffset a = (PositionOffset)x;
    //        PositionOffset b = (PositionOffset)y;

    //        return a.CompareTo(b.Position);
    //    }
    //    #endregion

    //    public int Load(string strData)
    //    {
    //        int ret = 0;
    //        string[] strValues = strData.Split(',');
            
    //        if(strValues.Length == 4)
    //        {
    //            try
    //            {
    //                m_Position.X = double.Parse(strValues[0]);
    //                m_Position.Y = double.Parse(strValues[1]);
    //                m_Offset.X = double.Parse(strValues[2]);
    //                m_Offset.Y = double.Parse(strValues[3]);
    //            }
    //            catch(Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                ret = -1;
    //            }                
    //        }
    //        else
    //        {
    //            ret = -1;
    //        }

    //        return ret;
    //    }

    //    public int Save(out string strData)
    //    {
    //        int ret = 0;

    //        strData = string.Format("{0}, {1}, {2}, {3}", m_Position.X, m_Position.Y, m_Offset.X, m_Offset.Y);

    //        return ret;
    //    }
    //}

    [Serializable]
    public class PositionOffsetCollection : List<PositionOffset>
    {
        public PositionOffsetCollection(IList<PositionOffset> list) : base(list) { }
        public PositionOffsetCollection() : base() { }

        public int Load(string strFileName)
        {
            int ret = 0;

            Clear();
            using (StreamReader sr = new StreamReader(strFileName))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    PositionOffset positionOffset = new PositionOffset();
                    //if ((ret = positionOffset.Load(line)) != 0) return ret;
                    positionOffset.SetValue(line);
                    Add(positionOffset);
                }
            }

            return ret;
        }

        public int Save(string strFileName)
        {
            int ret = 0;

            using (StreamWriter sw = new StreamWriter(strFileName))
            {
                string strData = "";
                foreach(PositionOffset positionOffset in this)
                {
                    //if ((ret = positionOffset.Save(out strData)) != 0) return ret;
                    strData = positionOffset.ToString();
                    sw.WriteLine(strData);
                }

                sw.Flush();
            }

            return ret;
        }
    }
}
