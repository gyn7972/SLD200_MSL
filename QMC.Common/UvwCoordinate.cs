using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    #region UvwCoordinate
    [Serializable]
    public struct UvwCoordinate //: IXyCoordinate
    {
        #region Field
        private double m_U;
        private double m_V;
        private double m_W;
        #endregion

        #region Constructor
        public UvwCoordinate(double u, double v, double w)
        {
            this.m_U = u;
            this.m_V = v;
            this.m_W = w;
        }
        #endregion

        #region Property
        public double U
        {
            get { return this.m_U; }
            set { this.m_U = value; }
        }

        public double V
        {
            get { return this.m_V; }
            set { this.m_V = value; }
        }

        public double W
        {
            get { return this.m_W; }
            set { this.m_W = value; }
        }
        #endregion

        #region Method
        public bool Equals(UvwCoordinate value)
        {
            return this == value;
        }
        #endregion

        #region Static Members
        public static bool operator ==(UvwCoordinate a, UvwCoordinate b)
        {
            if (a.U == b.U && a.V == b.V && a.W == b.W)
                return true;
            else
                return false;
        }

        public static bool operator !=(UvwCoordinate a, UvwCoordinate b)
        {
            if (a.U == b.U && a.V == b.V && a.W == b.W)
                return false;
            else
                return true;
        }

        public static UvwCoordinate operator +(UvwCoordinate a, UvwCoordinate b)
        {
            return new UvwCoordinate(a.U + b.U, a.V + b.V, a.W + b.W);
        }

        //public static XyzCoordinate operator +(UvwCoordinate a, XyCoordinate b)
        //{
        //    return new XyzCoordinate(a.X + b.X, a.Y + b.Y, a.Z);
        //}

        //public static XyzCoordinate operator +(XyCoordinate a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X + b.X, a.Y + b.Y, b.Z);
        //}

        //public static XyzCoordinate operator +(XyzCoordinate a, SizeD b)
        //{
        //    return new XyzCoordinate(a.X + b.Width, a.Y + b.Height, a.Z);
        //}

        //public static XyzCoordinate operator +(SizeD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.Width + b.X, a.Height + b.Y, b.Z);
        //}

        //public static XyzCoordinate operator +(XyzCoordinate a, PointD b)
        //{
        //    return new XyzCoordinate(a.X + b.X, a.Y + b.Y, a.Z);
        //}

        //public static XyzCoordinate operator +(PointD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X + b.X, a.Y + b.Y, b.Z);
        //}

        //public static XyzCoordinate operator +(XyzCoordinate a, int b)
        //{
        //    return new XyzCoordinate(a.X + b, a.Y + b, a.Z + b);
        //}

        //public static XyzCoordinate operator +(int a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a + b.X, a + b.Y, a + b.Z);
        //}

        //public static XyzCoordinate operator +(XyzCoordinate a, double b)
        //{
        //    return new XyzCoordinate(a.X + b, a.Y + b, a.Z + b);
        //}

        //public static XyzCoordinate operator +(double a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a + b.X, a + b.Y, a + b.Z);
        //}

        public static UvwCoordinate operator -(UvwCoordinate a, UvwCoordinate b)
        {
            return new UvwCoordinate(a.U - b.U, a.V - b.V, a.W - b.W);
        }

        //public static XyzCoordinate operator -(XyzCoordinate a, XyCoordinate b)
        //{
        //    return new XyzCoordinate(a.X - b.X, a.Y - b.Y, a.Z);
        //}

        //public static XyzCoordinate operator -(XyCoordinate a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X - b.X, a.Y - b.Y, b.Z);
        //}

        //public static XyzCoordinate operator -(XyzCoordinate a, SizeD b)
        //{
        //    return new XyzCoordinate(a.X - b.Width, a.Y - b.Height, a.Z);
        //}

        //public static XyzCoordinate operator -(SizeD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.Width - b.X, a.Height - b.Y, b.Z);
        //}

        //public static XyzCoordinate operator -(XyzCoordinate a, PointD b)
        //{
        //    return new XyzCoordinate(a.X - b.X, a.Y - b.Y, a.Z);
        //}

        //public static XyzCoordinate operator -(PointD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X - b.X, a.Y - b.Y, b.Z);
        //}

        //public static XyzCoordinate operator -(XyzCoordinate a, int b)
        //{
        //    return new XyzCoordinate(a.X - b, a.Y - b, a.Z - b);
        //}

        //public static XyzCoordinate operator -(int a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a - b.X, a - b.Y, a - b.Z);
        //}

        //public static XyzCoordinate operator -(XyzCoordinate a, double b)
        //{
        //    return new XyzCoordinate(a.X - b, a.Y - b, a.Z - b);
        //}

        //public static XyzCoordinate operator -(double a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a - b.X, a - b.Y, a - b.Z);
        //}

        public static UvwCoordinate operator *(UvwCoordinate a, UvwCoordinate b)
        {
            return new UvwCoordinate(a.U * b.U, a.V * b.V, a.W * b.W);
        }

        //public static XyzCoordinate operator *(XyzCoordinate a, XyCoordinate b)
        //{
        //    return new XyzCoordinate(a.X * b.X, a.Y * b.Y, a.Z);
        //}

        //public static XyzCoordinate operator *(XyCoordinate a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X * b.X, a.Y * b.Y, b.Z);
        //}

        //public static XyzCoordinate operator *(XyzCoordinate a, SizeD b)
        //{
        //    return new XyzCoordinate(a.X * b.Width, a.Y * b.Height, a.Z);
        //}

        //public static XyzCoordinate operator *(SizeD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.Width * b.X, a.Height * b.Y, b.Z);
        //}

        //public static XyzCoordinate operator *(XyzCoordinate a, PointD b)
        //{
        //    return new XyzCoordinate(a.X * b.X, a.Y * b.Y, a.Z);
        //}

        //public static XyzCoordinate operator *(PointD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X * b.X, a.Y * b.Y, b.Z);
        //}

        //public static XyzCoordinate operator *(XyzCoordinate a, int b)
        //{
        //    return new XyzCoordinate(a.X * b, a.Y * b, a.Z * b);
        //}

        //public static XyzCoordinate operator *(int a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a * b.X, a * b.Y, a * b.Z);
        //}

        //public static XyzCoordinate operator *(XyzCoordinate a, double b)
        //{
        //    return new XyzCoordinate(a.X * b, a.Y * b, a.Z * b);
        //}

        //public static XyzCoordinate operator *(double a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a * b.X, a * b.Y, a * b.Z);
        //}

        public static UvwCoordinate operator /(UvwCoordinate a, UvwCoordinate b)
        {
            return new UvwCoordinate(a.U / b.U, a.V / b.V, a.W / b.W);
        }

        //public static XyzCoordinate operator /(XyzCoordinate a, XyCoordinate b)
        //{
        //    return new XyzCoordinate(a.X / b.X, a.Y / b.Y, a.Z);
        //}

        //public static XyzCoordinate operator /(XyCoordinate a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X / b.X, a.Y / b.Y, b.Z);
        //}

        //public static XyzCoordinate operator /(XyzCoordinate a, SizeD b)
        //{
        //    return new XyzCoordinate(a.X / b.Width, a.Y / b.Height, a.Z);
        //}

        //public static XyzCoordinate operator /(SizeD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.Width / b.X, a.Height / b.Y, b.Z);
        //}

        //public static XyzCoordinate operator /(XyzCoordinate a, PointD b)
        //{
        //    return new XyzCoordinate(a.X / b.X, a.Y / b.Y, a.Z);
        //}

        //public static XyzCoordinate operator /(PointD a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a.X / b.X, a.Y / b.Y, b.Z);
        //}

        //public static XyzCoordinate operator /(XyzCoordinate a, int b)
        //{
        //    return new XyzCoordinate(a.X / b, a.Y / b, a.Z / b);
        //}

        //public static XyzCoordinate operator /(int a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a / b.X, a / b.Y, a / b.Z);
        //}

        //public static XyzCoordinate operator /(XyzCoordinate a, double b)
        //{
        //    return new XyzCoordinate(a.X / b, a.Y / b, a.Z / b);
        //}

        //public static XyzCoordinate operator /(double a, XyzCoordinate b)
        //{
        //    return new XyzCoordinate(a / b.X, a / b.Y, a / b.Z);
        //}

        //public static explicit operator XyzCoordinate(XyCoordinate a)
        //{
        //    return new XyzCoordinate(a.X, a.Y, 0.0);
        //}

        //public static explicit operator XyzCoordinate(XytCoordinate a)
        //{
        //    return new XyzCoordinate(a.X, a.Y, 0.0);
        //}

        //public static explicit operator XyzCoordinate(XyztCoordinate a)
        //{
        //    return new XyzCoordinate(a.X, a.Y, a.Z);
        //}

        //public static explicit operator XyzCoordinate(PointD a)
        //{
        //    return new XyzCoordinate(a.X, a.Y, 0.0);
        //}
        #endregion

        #region Object Members
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is UvwCoordinate == false) return false;
            UvwCoordinate value = (UvwCoordinate)obj;

            return this.Equals(value);
        }

        public override int GetHashCode()
        {
            return this.U.GetHashCode() ^ this.V.GetHashCode() ^ this.W.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", this.U, this.V, this.W);
        }
        #endregion
    }
    #endregion

    #region UvwCoordinateCollection
    [Serializable]
    public class UvwCoordinateCollection : Collection<UvwCoordinate>
    {
        #region Constructor
        public UvwCoordinateCollection(IList<UvwCoordinate> list) : base(list) { }
        public UvwCoordinateCollection() : base() { }
        #endregion
    }
    #endregion
}
