using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public abstract class PositionConverter
    {
        #region Field
        protected PositionOffsetCollection m_positionOffsets;
        #endregion

        #region Property

        #endregion
        #region Constructor
        public PositionConverter()
        {
            m_positionOffsets = new PositionOffsetCollection();
        }
        #endregion

        public int Load(string strFileName)
        {
            return m_positionOffsets.Load(strFileName);
        }

        public int Save(string strFileName)
        {
            return m_positionOffsets.Save(strFileName);
        }

        public abstract XyCoordinate ToCompensationCoordinate(XyCoordinate coordinate);
    }

    public class LinearPositionConverter : PositionConverter
    {
        public LinearPositionConverter() : base()
        {
        }

        public override XyCoordinate ToCompensationCoordinate(XyCoordinate coordinate)
        {
            XyCoordinate result = new XyCoordinate();

            PositionOffset src1;
            PositionOffset src2;

            if (GetNeighboring(coordinate, out src1, out src2) != 0) return result;

            result.X = InterpolationX(src1, src2, coordinate.X);
            result.Y = InterpolationY(src1, src2, coordinate.Y);

            return result;
        }

        protected int GetNeighboring(XyCoordinate target, out PositionOffset src1, out PositionOffset src2)
        {
            int ret = 0;

            src1 = m_positionOffsets.Where(x => target.X - x.Position.X > 0 && target.Y - x.Position.Y > 0).OrderBy(x => target.X - x.Position.X).ThenBy(x => target.Y - x.Position.Y).First();
            src2 = m_positionOffsets.Where(x => target.X - x.Position.X < 0 && target.Y - x.Position.Y < 0).OrderBy(x => target.X - x.Position.X).ThenBy(x => target.Y - x.Position.Y).Last();

            return ret;
        }

        protected double InterpolationX(PositionOffset src1, PositionOffset src2, double dDesX)
        {
            double dResult = 0;
            double dRatio = GetInterpolationRatio(src1.Position.X, src2.Position.X, dDesX);
            dResult = dDesX + src1.Offset.X + (src2.Offset.X - src1.Offset.X) * dRatio;

            return dResult;
        }

        protected double InterpolationY(PositionOffset src1, PositionOffset src2, double dDesY)
        {
            double dResult = 0;
            double dRatio = GetInterpolationRatio(src1.Position.Y, src2.Position.Y, dDesY);
            dResult = dDesY + src1.Offset.Y + (src2.Offset.Y - src1.Offset.Y) * dRatio;

            return dResult;
        }

        protected double GetInterpolationRatio(double dSrc1, double dSrc2, double dDes)
        {
            double dResult = 0;
            dResult = (dDes - dSrc1) / (dSrc2 - dSrc1);
            return dResult;
        }

    }
}
