using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Modules.WorkStage;

namespace QMC.Common.Global
{
    public class HoleAlignHelper
    {
        public class AlignPoint
        {
            public float X;
            public float Y;
            public float Radius;   // dEdgePoint[1].X 또는 Y 값으로 간주

            public AlignPoint(float x, float y, float radius)
            {
                X = x;
                Y = y;
                Radius = radius;
            }

            public PointF ToPointF() => new PointF(X, Y);

            public override string ToString()
            {
                return $"X={X:F3}, Y={Y:F3}, R={Radius:F3}";
            }
        }

        public static Dictionary<int, AlignPoint> CalculateAlignmentPoints(
                    int nSocketNum,
                    stDividedRegion_GroupData[] stDividedRegion_GroupData,
                    int sampleCount = 20)
        {
            List<AlignPoint> allPoints = new List<AlignPoint>();

            if (nSocketNum < 0 || nSocketNum >= stDividedRegion_GroupData.Length)
                return null;

            var group = stDividedRegion_GroupData[nSocketNum];
            int regionCount = group.nGroup_Num;

            //for (int i = 0; i < regionCount; i++)
            {
                var region = group.m_stDividedRegion_RegionData[0];
                var objects = region.m_stDividedRegion_ObjectData;

                for (int j = 0; j < objects.Length; j++)
                {
                    var pt = objects[j].dEdgePoint[0];
                    var pt1 = objects[j].dEdgePoint[1]; // radius 정보

                    float x = (float)pt.X;
                    float y = (float)pt.Y;
                    float radius = (float)pt1.X; // 또는 pt1.Y

                    allPoints.Add(new AlignPoint(x, y, radius));
                }
            }

            return GetFourCornerAlignmentCenters(allPoints, sampleCount);
        }

        public static Dictionary<int, AlignPoint> GetFourCornerAlignmentCenters(List<AlignPoint> allPoints, int sampleCount = 20)
        {
            Dictionary<int, AlignPoint> result = new Dictionary<int, AlignPoint>();

            if (allPoints == null || allPoints.Count == 0)
                return result;

            float centerX = allPoints.Average(p => p.X);
            float centerY = allPoints.Average(p => p.Y);

            var bottomLeft = allPoints.Where(p => p.X < centerX && p.Y > centerY)
                                      .OrderByDescending(p => p.Y).Take(sampleCount).ToList();
            var topLeft = allPoints.Where(p => p.X < centerX && p.Y < centerY)
                                   .OrderBy(p => p.Y).Take(sampleCount).ToList();
            var topRight = allPoints.Where(p => p.X > centerX && p.Y < centerY)
                                    .OrderBy(p => p.Y).Take(sampleCount).ToList();
            var bottomRight = allPoints.Where(p => p.X > centerX && p.Y > centerY)
                                       .OrderByDescending(p => p.Y).Take(sampleCount).ToList();

            AlignPoint GetAverage(List<AlignPoint> pts)
            {
                if (pts.Count == 0) return new AlignPoint(0, 0, 0);
                return new AlignPoint(
                    pts.Average(p => p.X),
                    pts.Average(p => p.Y),
                    pts.Average(p => p.Radius)
                );
            }

            // 인덱스 매핑
            result[0] = GetAverage(bottomLeft);   // BL
            result[1] = GetAverage(topLeft);      // TL
            result[2] = GetAverage(topRight);     // TR
            result[3] = GetAverage(bottomRight);  // BR

            return result;
        }
    }
}
