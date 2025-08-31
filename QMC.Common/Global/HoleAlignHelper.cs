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
        public class HoleDeviation
        {
            public AlignPoint Nominal { get; set; }     // 도면 기준
            public AlignPoint Detected { get; set; }    // Vision 검출
            public double OffsetX => Detected.X - Nominal.X;
            public double OffsetY => Detected.Y - Nominal.Y;
            public double Distance => Math.Sqrt(OffsetX * OffsetX + OffsetY * OffsetY);
        }


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

        public class AlignResult
        {
            public Dictionary<int, AlignPoint> CornerPoints { get; set; } = new Dictionary<int, AlignPoint>();
            public List<AlignPoint> AllPoints { get; set; } = new List<AlignPoint>();
        }

        public static AlignResult CalculateAlignmentPoints(
                    int nSocketNum,
                    stDividedRegion_GroupData[] stDividedRegion_GroupData,
                    int sampleCount = 20)
        {
            List<AlignPoint> allPoints = new List<AlignPoint>();

            if (nSocketNum < 0 || nSocketNum >= stDividedRegion_GroupData.Length)
                return null;

            var group = stDividedRegion_GroupData[nSocketNum];
            //int regionCount = group.nGroup_Num;
            int regionCount = group.nGroup_Num;

            if (group.m_stDividedRegion_RegionData.Length == 0)
            {
                if (group.m_stDividedRegion_RegionData.Length > 0)
                {
                    // fallback 안전 처리
                    var region = group.m_stDividedRegion_RegionData[0];
                    var objects = region.m_stDividedRegion_ObjectData;

                    for (int j = 0; j < objects.Length; j++)
                    {
                        var pt = objects[j].dEdgePoint[0];
                        var pt1 = objects[j].dEdgePoint[1];
                        float x = (float)pt.X;
                        float y = (float)pt.Y;
                        float radius = (float)pt1.X;

                        allPoints.Add(new AlignPoint(x, y, radius));
                    }
                }
            }
            else
            {
                for (int i = 0; i < group.m_stDividedRegion_RegionData.Length; i++)
                {
                    var region = group.m_stDividedRegion_RegionData[i];
                    var objects = region.m_stDividedRegion_ObjectData;

                    for (int j = 0; j < objects.Length; j++)
                    {
                        var pt = objects[j].dEdgePoint[0];
                        var pt1 = objects[j].dEdgePoint[1];
                        float x = (float)pt.X;
                        float y = (float)pt.Y;
                        float radius = (float)pt1.X;

                        allPoints.Add(new AlignPoint(x, y, radius));
                    }
                }
            }

            if(Equipment.Machine_LaserType_CO2)
            {
                return GetFourCornerAlignmentCentersWithAll_CO2(allPoints, sampleCount);
            }
            else
            {
                return GetFourCornerAlignmentCentersWithAll(allPoints, sampleCount);
            }
                //return GetFourCornerAlignmentCentersWithAll(allPoints, sampleCount);
        }

        public static AlignResult GetFourCornerAlignmentCentersWithAll(List<AlignPoint> allPoints, int sampleCount = 20)
        {
            AlignResult result = new AlignResult();
            var cornerPoints = result.CornerPoints;

            if (allPoints == null || allPoints.Count == 0)
                return result;

            result.AllPoints = allPoints;  // 전체 데이터 저장

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

            cornerPoints[0] = GetAverage(bottomLeft);   // BL
            cornerPoints[1] = GetAverage(topLeft);      // TL
            cornerPoints[2] = GetAverage(topRight);     // TR
            cornerPoints[3] = GetAverage(bottomRight);  // BR
            cornerPoints[4] = GetAverage(allPoints);    // 전체 평균

            return result;
        }

        public static AlignResult GetFourCornerAlignmentCentersWithAll_CO2(List<AlignPoint> allPoints, int sampleCount = 20)
        {
            AlignResult result = new AlignResult();
            var cornerPoints = result.CornerPoints;

            if (allPoints == null || allPoints.Count == 0)
                return result;

            result.AllPoints = allPoints;

            // 공통 헬퍼
            AlignPoint GetAverage(IReadOnlyList<AlignPoint> pts)
            {
                if (pts == null || pts.Count == 0) return new AlignPoint(0, 0, 0);
                double sx = 0, sy = 0, sr = 0;
                foreach (var p in pts) { sx += p.X; sy += p.Y; sr += p.Radius; }
                double n = pts.Count;
                return new AlignPoint((float)(sx / n), (float)(sy / n), (float)(sr / n));
            }
            List<AlignPoint> TakeNearest(IReadOnlyList<AlignPoint> src, AlignPoint anchor, int k)
            {
                k = Math.Max(1, Math.Min(k, src.Count));
                return src.OrderBy(p =>
                {
                    double dx = p.X - anchor.X;
                    double dy = p.Y - anchor.Y;
                    return dx * dx + dy * dy;
                }).Take(k).ToList();
            }

            // 1) 우선 극값 기반 코너 앵커 산출 (사분면 분포에 덜 민감)
            var tlAnchor = allPoints.OrderBy(p => (double)p.X + p.Y).First();      // Top-Left  : X+Y 최소
            var brAnchor = allPoints.OrderByDescending(p => (double)p.X + p.Y).First(); // Bottom-Right: X+Y 최대
            var trAnchor = allPoints.OrderByDescending(p => (double)p.X - p.Y).First(); // Top-Right : X−Y 최대
            var blAnchor = allPoints.OrderBy(p => (double)p.X - p.Y).First();      // Bottom-Left: X−Y 최소

            // 2) 각 앵커 주변에서 sampleCount개 샘플링 → 평균
            var tlGroup = TakeNearest(allPoints, tlAnchor, sampleCount);
            var trGroup = TakeNearest(allPoints, trAnchor, sampleCount);
            var blGroup = TakeNearest(allPoints, blAnchor, sampleCount);
            var brGroup = TakeNearest(allPoints, brAnchor, sampleCount);

            // 3) 결과 매핑 (기존 인덱스 유지)
            cornerPoints[0] = GetAverage(blGroup);            // BL
            cornerPoints[1] = GetAverage(tlGroup);            // TL
            cornerPoints[2] = GetAverage(trGroup);            // TR
            cornerPoints[3] = GetAverage(brGroup);            // BR
            cornerPoints[4] = GetAverage(allPoints);          // 전체 평균

            return result;
        }


        public static List<HoleDeviation> MatchClosestHoles(List<AlignPoint> nominalHoles, List<AlignPoint> detectedHoles)
        {
            List<HoleDeviation> matched = new List<HoleDeviation>();
            List<AlignPoint> remainingDetected = new List<AlignPoint>(detectedHoles);

            foreach (var nominal in nominalHoles)
            {
                if (remainingDetected.Count == 0)
                    break;

                // 가장 가까운 홀을 찾음
                var closest = remainingDetected
                    .OrderBy(d => GetDistance(nominal, d))
                    .First();

                matched.Add(new HoleDeviation
                {
                    Nominal = nominal,
                    Detected = closest
                });

                // 중복 매칭 방지
                remainingDetected.Remove(closest);
            }

            return matched;
        }

        private static double GetDistance(AlignPoint a, AlignPoint b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
