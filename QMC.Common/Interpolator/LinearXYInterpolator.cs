using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Interpolator
{
    public class LinearXYInterpolator : BaseInterpolator
    {
        
        public override int Interpolate(XyCoordinate source, ref XyCoordinate dest)
        {
            int ret = 0;
            /*
            var result = (from data in m_listMapData
                          where source.X - data.Position.X < 0 && data.Position.Y - source.Y < 0
                          orderby data.Position.Y descending, data.Position.X
                          select data);

            PositionOffset pos1 = result.First();

            result = (from data in m_listMapData
                      where source.X - data.Position.X + dPitchX < 0 && data.Position.Y - (source.Y + dPitchY) < 0
                      orderby data.Position.Y descending, data.Position.X
                      select data);

            PositionOffset pos2 = result.First();
            */
            PositionOffset posLT = GetLeftTop(source);
            //PositionOffset posLB = GetLeftBottom(source);
            //PositionOffset posRT = GetRightTop(source);
            PositionOffset posRB = GetRightBottom(source);

            XyCoordinate dOffset = posRB.Offset - posLT.Offset;
            XyCoordinate dRatio = posRB.Position - source;

            dest = source + (dOffset * dRatio);

            return ret;
        }

        public override int ReverseInterpolate(XyCoordinate dest, ref XyCoordinate source)
        {
            int ret = 0;



            return ret;
        }
    }
}
