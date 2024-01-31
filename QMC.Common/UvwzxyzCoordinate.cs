using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    #region UvwxyzzCoordinate

    [Serializable]
    public struct UvwzxyzCoordinate : IXyCoordinate
    {
        #region Field
        private double m_U;
        private double m_V;
        private double m_W;
        private double m_EZ;
        private double m_X;
        private double m_Y;
        private double m_VZ;
        #endregion

        #region Constructor
        public UvwzxyzCoordinate(double u, double v, double w, double z1, double x, double y, double z2)
        {
            this.m_U = u;
            this.m_V = v;
            this.m_W = w;
            this.m_EZ = z1;
            this.m_X = x;
            this.m_Y = y;
            this.m_VZ = z2;
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

        public double EZ
        {
            get { return this.m_EZ; }
            set { this.m_EZ = value; }
        }

        public double X
        {
            get { return this.m_X; }
            set { this.m_X = value; }
        }

        public double Y
        {
            get { return this.m_Y; }
            set { this.m_Y = value; }
        }

        public double VZ
        {
            get { return this.m_VZ; }
            set { this.m_VZ = value; }
        }

        #endregion

        #region Method
        public bool Equals(UvwzxyzCoordinate value)
        {
            return this == value;
        }
        #endregion

        #region Static Members
        public static bool operator ==(UvwzxyzCoordinate a, UvwzxyzCoordinate b)
        {
            if (a.U == b.U && a.V == b.V && a.W == b.W && a.EZ == b.EZ && a.X == b.X && a.Y == b.Y && a.VZ == b.VZ)
                return true;
            else
                return false;
        }

        public static bool operator !=(UvwzxyzCoordinate a, UvwzxyzCoordinate b)
        {
            if (a.U == b.U && a.V == b.V && a.W == b.W && a.EZ == b.EZ && a.X == b.X && a.Y == b.Y && a.VZ == b.VZ)
                return false;
            else
                return true;
        }

        public static UvwzxyzCoordinate operator +(UvwzxyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U + b.U, a.V + b.V, a.W + b.W, a.EZ + b.EZ, a.X + b.X, a.Y + b.Y, a.VZ + b.VZ);
        }

        public static UvwzxyzCoordinate operator +(UvwzxyzCoordinate a, XyCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X + b.X, a.Y + b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator +(XyCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X + b.X, a.Y + b.Y, b.VZ);
        }

        //public static UvwxyzzCoordinate operator +(UvwxyzzCoordinate a, XytCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X + b.X, a.Y + b.Y, a.IZ, a.SZ, a.T + b.T);
        //}

        //public static UvwxyzzCoordinate operator +(XytCoordinate a, UvwxyzzCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X + b.X, a.Y + b.Y, b.IZ, b.SZ, a.T + b.T);
        //}

        public static UvwzxyzCoordinate operator +(UvwzxyzCoordinate a, XyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X + b.X, a.Y + b.Y, a.VZ + b.Z);
        }

        public static UvwzxyzCoordinate operator +(XyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X + b.X, a.Y + b.Y, a.Z + b.VZ);
        }

        public static UvwzxyzCoordinate operator +(int a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a + b.U, a + b.V, a + b.W, a + b.EZ, a + b.X, a + b.Y, a + b.VZ);
        }

        public static UvwzxyzCoordinate operator +(double a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a + b.U, a + b.V, a + b.W, a + b.EZ, a + b.X, a + b.Y, a + b.VZ);
        }

        public static UvwzxyzCoordinate operator +(UvwzxyzCoordinate a, SizeD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X + b.Width, a.Y + b.Height, a.VZ);
        }

        public static UvwzxyzCoordinate operator +(SizeD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.Width + b.X, a.Height + b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator +(UvwzxyzCoordinate a, PointD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X + b.X, a.Y + b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator +(PointD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X + b.X, a.Y + b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator -(UvwzxyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U - b.U, a.V - b.V, a.W - b.W, a.EZ - b.EZ, a.X - b.X, a.Y - b.Y, a.VZ - b.VZ);
        }

        public static UvwzxyzCoordinate operator -(UvwzxyzCoordinate a, XyCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X - b.X, a.Y - b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator -(XyCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X - b.X, a.Y - b.Y, b.VZ);
        }

        //public static UvwxyzzCoordinate operator -(UvwxyzzCoordinate a, XytCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X - b.X, a.Y - b.Y, a.IZ, a.SZ, a.T - b.T);
        //}

        //public static UvwxyzzCoordinate operator -(XytCoordinate a, UvwxyzzCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X - b.X, a.Y - b.Y, b.IZ, b.SZ, a.T - b.T);
        //}

        public static UvwzxyzCoordinate operator -(UvwzxyzCoordinate a, XyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X - b.X, a.Y - b.Y, a.VZ - b.Z);
        }

        public static UvwzxyzCoordinate operator -(XyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X - b.X, a.Y - b.Y, a.Z - b.VZ);
        }

        public static UvwzxyzCoordinate operator -(UvwzxyzCoordinate a, SizeD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X - b.Width, a.Y - b.Height, a.VZ);
        }

        public static UvwzxyzCoordinate operator -(SizeD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.Width - b.X, a.Height - b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator -(UvwzxyzCoordinate a, PointD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X - b.X, a.Y - b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator -(PointD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X - b.X, a.Y - b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator *(UvwzxyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U * b.U, a.V * b.V, a.W * b.W, a.EZ * b.EZ, a.X * b.X, a.Y * b.Y, a.VZ * b.VZ);
        }

        public static UvwzxyzCoordinate operator *(UvwzxyzCoordinate a, XyCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X * b.X, a.Y * b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator *(XyCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X * b.X, a.Y * b.Y, b.VZ);
        }

        //public static UvwxyzzCoordinate operator *(UvwxyzzCoordinate a, XytCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X * b.X, a.Y * b.Y, a.IZ, a.SZ, a.T * b.T);
        //}

        //public static UvwxyzzCoordinate operator *(XytCoordinate a, UvwxyzzCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X * b.X, a.Y * b.Y, b.IZ, b.SZ, a.T * b.T);
        //}

        public static UvwzxyzCoordinate operator *(UvwzxyzCoordinate a, XyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X * b.X, a.Y * b.Y, a.VZ * b.Z);
        }

        public static UvwzxyzCoordinate operator *(XyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X * b.X, a.Y * b.Y, a.Z * b.VZ);
        }

        public static UvwzxyzCoordinate operator *(UvwzxyzCoordinate a, SizeD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X * b.Width, a.Y * b.Height, a.VZ);
        }

        public static UvwzxyzCoordinate operator *(SizeD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.Width * b.X, a.Height * b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator *(UvwzxyzCoordinate a, PointD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X * b.X, a.Y * b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator *(PointD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X * b.X, a.Y * b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator /(UvwzxyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U / b.U, a.V / b.V, a.W / b.W, a.EZ / b.EZ, a.X / b.X, a.Y / b.Y, a.VZ / b.VZ);
        }

        public static UvwzxyzCoordinate operator /(UvwzxyzCoordinate a, XyCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X / b.X, a.Y / b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator /(XyCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X / b.X, a.Y / b.Y, b.VZ);
        }

        //public static UvwxyzzCoordinate operator /(UvwxyzzCoordinate a, XytCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X / b.X, a.Y / b.Y, a.IZ, a.SZ, a.T / b.T);
        //}

        //public static UvwxyzzCoordinate operator /(XytCoordinate a, UvwxyzzCoordinate b)
        //{
        //    return new UvwxyzzCoordinate(a.X / b.X, a.Y / b.Y, b.IZ, b.SZ, a.T / b.T);
        //}

        public static UvwzxyzCoordinate operator /(UvwzxyzCoordinate a, XyzCoordinate b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X / b.X, a.Y / b.Y, a.VZ / b.Z);
        }

        public static UvwzxyzCoordinate operator /(XyzCoordinate a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X / b.X, a.Y / b.Y, a.Z / b.VZ);
        }

        public static UvwzxyzCoordinate operator /(UvwzxyzCoordinate a, SizeD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X / b.Width, a.Y / b.Height, a.VZ);
        }

        public static UvwzxyzCoordinate operator /(SizeD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.Width / b.X, a.Height / b.Y, b.VZ);
        }

        public static UvwzxyzCoordinate operator /(UvwzxyzCoordinate a, PointD b)
        {
            return new UvwzxyzCoordinate(a.U, a.V, a.W, a.EZ, a.X / b.X, a.Y / b.Y, a.VZ);
        }

        public static UvwzxyzCoordinate operator /(PointD a, UvwzxyzCoordinate b)
        {
            return new UvwzxyzCoordinate(b.U, b.V, b.W, b.EZ, a.X / b.X, a.Y / b.Y, b.VZ);
        }

        public static explicit operator UvwzxyzCoordinate(XyCoordinate a)
        {
            return new UvwzxyzCoordinate(0.0, 0.0, 0.0, 0.0, a.X, a.Y, 0.0);
        }

        public static explicit operator UvwzxyzCoordinate(XyzCoordinate a)
        {
            return new UvwzxyzCoordinate(0.0, 0.0, 0.0, 0.0, a.X, a.Y, a.Z);
        }

        //public static explicit operator UvwxyzzCoordinate(XytCoordinate a)
        //{
        //    return new UvwxyzzCoordinate(a.X, a.Y, 0.0, 0.0, a.T);
        //}

        public static explicit operator UvwzxyzCoordinate(PointD a)
        {
            return new UvwzxyzCoordinate(0.0, 0.0, 0.0, 0.0, a.X, a.Y, 0.0);
        }
        #endregion

        #region Object Members
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is UvwzxyzCoordinate == false) return false;
            UvwzxyzCoordinate value = (UvwzxyzCoordinate)obj;

            return this.Equals(value);
        }

        public override int GetHashCode()
        {
            return this.U.GetHashCode() ^ this.V.GetHashCode() ^ this.W.GetHashCode() ^ this.EZ.GetHashCode() ^ this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.VZ.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}, {3}, {4}, {5}]", this.U, this.V, this.W, this.EZ, this.X, this.Y, this.VZ);
        }
        #endregion
    }
    #endregion

    #region UvwxyzzCoordinateCollection
    [Serializable]
    public class UvwzxyzCoordinateCollection : Collection<UvwzxyzCoordinate>
    {
        #region Constructor
        public UvwzxyzCoordinateCollection(IList<UvwzxyzCoordinate> list) : base(list) { }
        public UvwzxyzCoordinateCollection() : base() { }
        #endregion
    }
    #endregion
}
