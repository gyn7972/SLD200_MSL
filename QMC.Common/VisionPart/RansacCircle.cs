using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QMC.Common.VisionPart
{
    // 원을 나타내는 구조체 (중심좌표와 반지름)
    public struct Circle
    {
        public float CenterX;
        public float CenterY;
        public float Radius;
        public float Score;
        public Circle(float x, float y, float radius)
        {
            CenterX = x;
            CenterY = y;
            Radius = radius;
            Score = 0;
        }
        public Circle(float x, float y, float radius, float score)
        {
            CenterX = x;
            CenterY = y;
            Radius = radius;
            Score = score;
        }
        public RectangleF GetBoundery()
        {
            return new RectangleF(CenterX - Radius, CenterY - Radius, Radius * 2, Radius * 2);
        }
        public override string ToString()
        {
            return $"Center: ({CenterX:F2}, {CenterY:F2}), Radius: {Radius:F2}";
        }
    }

    public class RansacCircleFitter
    {
        static double[,] distancedic = null;
        public RansacCircleFitter()
        {

            if (distancedic == null)
            {
                distancedic = new double[4000, 4000];
                for (int i = 0; i < 4000; i++)
                {
                    for (int j = 0; j < 4000; j++)
                    {
                        distancedic[i, j] = Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2));
                    }
                }
            }
        }
        /// <summary>
        /// RANSAC을 사용하여 원 피팅을 수행합니다.
        /// </summary>
        /// <param name="points">엣지 점들의 리스트</param>
        /// <param name="iterations">반복 횟수</param>
        /// <param name="threshold">각 점이 원 모델에 얼마나 근접해야 inlier로 판단할지 정의하는 허용 오차 (픽셀 단위)</param>
        /// <returns>최적의 원 모델</returns>
        public static Circle FitCircle(List<PointF> points, int iterations = 1000, double threshold = 5.0, int r = 0)
        {
            if (points.Count < 100)
            {
                return new Circle();
            }
            double dSamplingRate = 1;
            iterations = (int)(iterations * dSamplingRate);
            Circle bestCircle = new Circle();
            double bestInliers = 0;// (int)(r*2*Math.PI / 2) * dSamplingRate;
            Random rnd = new Random();
            int nStep = points.Count / 6;
            int nCount = points.Count;
            for (int i = 0; i < iterations; i++)
            {
                // 랜덤하게 3개의 서로 다른 점을 선택
                int idx1 = (i % nCount);
                if (idx1 == 0 && i != 0)
                {
                    nStep--;
                }
                int idx2 = (idx1 + nStep) % nCount;
                int idx3 = (idx2 + nStep) % nCount;
                if (idx1 < 0 || idx2 < 0 || idx3 < 0)
                    break;
                PointF p1 = points[idx1];
                PointF p2 = points[idx2];
                PointF p3 = points[idx3];

                // 3개의 점으로부터 원 후보 계산 (collinear한 경우라면 null 반환)
                Circle? circleCandidate = ComputeCircleFromPoints(p1, p2, p3);
                if (circleCandidate == null)
                {

                    continue;
                }
                Circle circle = circleCandidate.Value;

                if (r > 0)
                {
                    //if (r * 0.80 < circle.Radius && circle.Radius < r * 1.20)
                    //{

                    //}else
                    //{
                    //    continue;
                    //}

                }
                // 모든 점들에 대해 원의 경계(반지름)와의 오차를 계산하고 inlier 수를 센다.
                int inlierCount = 0;
                System.Threading.Tasks.Parallel.For(0, points.Count, iter =>
                {
                    var pt = points[iter];
                    int x = Math.Abs((int)(pt.X - circle.CenterX));
                    int y = Math.Abs((int)(pt.Y - circle.CenterY));
                    double distance = 0;

                    if (x < 4000 && y < 4000)
                    {
                        distance = distancedic[x, y];
                    }
                    else
                    {
                        distance = Math.Sqrt(x * x + y * y);
                    }

                    double error = Math.Abs(distance - circle.Radius);
                    if (error < threshold)
                    {
                        System.Threading.Interlocked.Increment(ref inlierCount);
                    }
                });

                // 지금까지의 모델보다 inlier가 많으면 최적 모델을 업데이트
                double dMin = Math.Min(circle.Radius, r);
                double dMax = Math.Max(circle.Radius, r);
                double dScore = inlierCount * dMin / dMax;
                if (dScore > bestInliers)
                {
                    bestInliers = dScore;
                    circle.Score = (float)dScore / points.Count;
                    bestCircle = circle;

                }
            }

            return bestCircle;
        }

        /// <summary>
        /// 3개의 점으로부터 원의 중심과 반지름(=외접원)을 구합니다.
        /// collinear한 경우 null을 반환합니다.
        /// </summary>
        private static Circle? ComputeCircleFromPoints(PointF p1, PointF p2, PointF p3)
        {
            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;
            double x3 = p3.X, y3 = p3.Y;

            // 세 점이 이루는 행렬식: 2*(x1(y2 - y3) + x2(y3 - y1) + x3(y1 - y2))
            double d = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            if (Math.Abs(d) < 1e-6)
                return null;  // 점들이 collinear하면 유효한 원을 만들 수 없음

            double sq1 = x1 * x1 + y1 * y1;
            double sq2 = x2 * x2 + y2 * y2;
            double sq3 = x3 * x3 + y3 * y3;

            double centerX = (sq1 * (y2 - y3) + sq2 * (y3 - y1) + sq3 * (y1 - y2)) / d;
            double centerY = (sq1 * (x3 - x2) + sq2 * (x1 - x3) + sq3 * (x2 - x1)) / d;
            double radius = Math.Sqrt((centerX - x1) * (centerX - x1) + (centerY - y1) * (centerY - y1));

            return new Circle((float)centerX, (float)centerY, (float)radius);
        }
    }

}


