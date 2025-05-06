using QMC.Common.Vision.VisionAlign;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Interpolator
{
    public class PerspectiveProjectionInterpolator : BaseInterpolator
    {
        protected List<PositionOffset> GetAround4Point(XyCoordinate source)
        {
            List<PositionOffset> list = new List<PositionOffset>();
            PositionOffset posLT = GetLeftTop(source);
            if (posLT.Position.X != 0 || posLT.Position.Y != 0)
            {
                list.Add(posLT);
            }
            PositionOffset posLB = GetLeftBottom(source);
            if (posLB.Position.X != 0 || posLB.Position.Y != 0)
            {
                list.Add(posLB);
            }
            PositionOffset posRT = GetRightTop(source);
            if (posRT.Position.X != 0 || posRT.Position.Y != 0)
            {
                list.Add(posRT);
            }
            PositionOffset posRB = GetRightBottom(source);
            if (posRB.Position.X != 0 || posRB.Position.Y != 0)
            {
                list.Add(posRB);
            }
            return list;
        }

        protected List<PositionOffset> GetDestAround4Point(XyCoordinate dest)
        {
            List<PositionOffset> list = new List<PositionOffset>();
            PositionOffset posLT = GetMeasureLeftTop(dest);
            if (posLT.Position.X != 0 || posLT.Position.Y != 0)
            {
                list.Add(posLT);
            }
            PositionOffset posLB = GetMeasureLeftBottom(dest);
            if (posLB.Position.X != 0 || posLB.Position.Y != 0) 
            {
                list.Add(posLB);
            }
            PositionOffset posRT = GetMeasureRightTop(dest);
            if (posRT.Position.X != 0 || posRT.Position.Y != 0)
            {
                list.Add(posRT);
            }
            PositionOffset posRB = GetMeasureRightBottom(dest);
            if (posRB.Position.X != 0 || posRB.Position.Y != 0)
            {
                list.Add(posRB);
            }
            return list;
        }

        public override int Interpolate(XyCoordinate source, ref XyCoordinate dest)
        {
            int ret = 0;
            List<PositionOffset> list = GetAround4Point(source);
            if(list.Count == 4)
            {
                QMCMatrix CorrectionMatrix = null;
                PerspectiveProjection perspectiveProjection = new PerspectiveProjection();
                XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
                XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();
                int nIndex = 0;
                foreach (PositionOffset v in list)
                {
                    coordinatesSource.Add(new XyCoordinate(v.Position.X, v.Position.Y));
                    coordinatesTarget.Add(new XyCoordinate(v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));

                    //  2025. 04. 17.  SCH : 로그파일 용량이 너무 커서..
                    //Log.Write("PerspectiveProjectionInterpolator", String.Format("{0}, {1}, {2},{3},{4}",
                    //    nIndex++, v.Position.X, v.Position.Y, v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));
                }
                CorrectionMatrix = perspectiveProjection.projection_matrix(coordinatesSource, coordinatesTarget);
                XyCoordinate t = perspectiveProjection.GetPerspectiveProjectionPoint(source, CorrectionMatrix);
                
                dest = t;
            }
            else
            {
                dest = source;
            }

            //  2025. 04. 17.  SCH : 로그파일 용량이 너무 커서..
            //Log.Write("PerspectiveProjectionInterpolator", String.Format("list.Count = {0} : {1}, {2}, {3}, {4}"
            //            , list.Count, source.X, source.Y, dest.X, dest.Y));

            return ret;
        }

        public override int ReverseInterpolate(XyCoordinate dest, ref XyCoordinate source)
        {
            int ret = 0;
            List<PositionOffset> list = GetDestAround4Point(dest);
            if (list.Count == 4)
            {
                QMCMatrix CorrectionMatrix = null;
                PerspectiveProjection perspectiveProjection = new PerspectiveProjection();
                XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
                XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();
                int nIndex = 0;
                foreach (PositionOffset v in list)
                {
                    coordinatesSource.Add(new XyCoordinate(v.Position.X, v.Position.Y));
                    coordinatesTarget.Add(new XyCoordinate(v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));

                    //  2025. 04. 17.  SCH : 로그파일 용량이 너무 커서..
                    //Log.Write("PerspectiveProjectionInterpolator", String.Format("{0}, {1}, {2},{3},{4}",
                    //    nIndex++, v.Position.X, v.Position.Y, v.Position.X + v.Offset.X, v.Position.Y + v.Offset.Y));
                }
                CorrectionMatrix = perspectiveProjection.projection_matrix(coordinatesTarget, coordinatesSource);
                XyCoordinate t = perspectiveProjection.GetPerspectiveProjectionPoint(dest, CorrectionMatrix);

                source = t;
            }
            else
            {
                source = dest;
            }

            return ret;
        }
    }
}
