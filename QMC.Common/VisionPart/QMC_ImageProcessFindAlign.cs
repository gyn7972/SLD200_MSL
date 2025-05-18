
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public class QMC_ImageProcessFindAlignResult
    {
        public List<Circle> Circles { get; set; }
        public List<double> ScoreCollection { get; set; }

        public QMC_ImageProcessFindAlignResult()
        {
            Circles = new List<Circle>();
            ScoreCollection = new List<double>();

        }
    }

    public class QMC_ImageProcessFindAlign
    {
        private bool IsImageSave = false;
        private List<Point> pointsCircle = new List<Point>();
        CirclePoints circlePoints1;
        //Random random = new Random((int)DateTime.Now.Ticks);
        Random random = new Random();
        RansacCircleFitter ransacCircleFitter = new RansacCircleFitter();
        public List<RectangleF> FindCircles(Bitmap bitmap)
        {
            List<RectangleF> circles = new List<RectangleF>();
            List<RectangleF> circlesResult = new List<RectangleF>();
            // Bitmap을 byte 배열로 변환
            byte[] pixelData = ConvertBitmapToByteArray(bitmap);
            // 히스토그램 평활화


            int w = bitmap.Width;
            int h = bitmap.Height;
            bool m_bFindCircle = false;

            var v = FindCirclesWidthCircleBoundary(circlesResult, pixelData, w, h,
                260, 0.05, ref m_bFindCircle, 0, 0, false);

            //var v = MatchCoordinates(listMetal, 3);
            // v의 좌표를 원점으로 하고 listMetal의 w,h 를 가지는 List < RectangleF > result를  생성
            //circlesResult = new List<RectangleF>();
            //for (int i = 0; i < v.Count; i++)
            //{
            //    RectangleF rect = new RectangleF(v[i].X - listMetal[0].Width / 2, v[i].Y - listMetal[0].Height / 2, listMetal[0].Width, listMetal[0].Height);
            //    circlesResult.Add(rect);
            //}

            return circlesResult;
        }
        public static List<PointF> MatchCoordinates(List<RectangleF> listMetal, int cols)
        {
            // 1. rows 계산
            int rows = CalculateRows(listMetal, cols);

            // 2. 회전 각도 계산
            double rotationAngle = CalculateRotationAngle(listMetal);

            // 3. listMetal 좌표 회전
            List<PointF> rotatedMetalCenters = RotateCoordinates(listMetal, rotationAngle);

            // 4. 이상적인 격자 생성
            List<PointF> idealPoints = GenerateIdealGrid(rows, cols, rotatedMetalCenters);

            // 5. 거리 기반 매칭
            List<PointF> matchedPoints = new List<PointF>();
            HashSet<int> usedIndices = new HashSet<int>();

            foreach (var metal in rotatedMetalCenters)
            {
                int closestIndex = -1;
                double minDistance = double.MaxValue;

                for (int i = 0; i < idealPoints.Count; i++)
                {
                    if (usedIndices.Contains(i)) continue;

                    double distance = GetDistance(metal, idealPoints[i]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestIndex = i;
                    }
                }

                if (closestIndex != -1)
                {
                    matchedPoints.Add(idealPoints[closestIndex]);
                    usedIndices.Add(closestIndex);
                }
            }

            // 6. 매칭된 좌표를 원래 각도로 되돌림
            return RotateCoordinatesBack(matchedPoints, -rotationAngle);
        }
        private static double GetDistance(PointF p1, PointF p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static List<PointF> GenerateIdealGrid(int rows, int cols, List<PointF> rotatedMetalCenters)
        {
            // 1. 중심 좌표 계산
            float avgX = rotatedMetalCenters.Average(p => p.X);
            float avgY = rotatedMetalCenters.Average(p => p.Y);

            // 2. 격자 간격 계산
            float spacingX = (rotatedMetalCenters.Max(p => p.X) - rotatedMetalCenters.Min(p => p.X)) / (cols - 1);
            float spacingY = (rotatedMetalCenters.Max(p => p.Y) - rotatedMetalCenters.Min(p => p.Y)) / (rows - 1);

            // 3. 이상적인 격자 생성
            List<PointF> grid = new List<PointF>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float x = avgX + (col - (cols - 1) / 2.0f) * spacingX;
                    float y = avgY + (row - (rows - 1) / 2.0f) * spacingY;
                    grid.Add(new PointF(x, y));
                }
            }

            return grid;
        }
        private static int CalculateRows(List<RectangleF> listMetal, int cols)
        {
            if (listMetal.Count > 1)
            {
                float minY = listMetal.Min(r => r.Y + r.Height / 2);
                float maxY = listMetal.Max(r => r.Y + r.Height / 2);
                float avgHeight = listMetal.Average(r => r.Height);
                return (int)Math.Round((maxY - minY) / avgHeight) + 1;

            }
            return 1;
            // Y축 범위를 기준으로 rows 계산


        }

        private static double CalculateRotationAngle(List<RectangleF> listMetal)
        {
            // 1. 중심 좌표 계산
            var centers = listMetal.Select(rect => new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)).ToList();

            // 2. X 좌표를 기준으로 그룹화 (같은 열에 있는 점들 찾기)
            var groupedByColumn = centers.GroupBy(p => Math.Floor(p.X / 100)) // X 좌표를 100픽셀 단위로 그룹화
                              .OrderBy(g => g.Key) // X 좌표 기준으로 정렬
                              .ToList();

            if (groupedByColumn.Count < 2)
            {
                throw new InvalidOperationException("열이 두 개 이상 필요합니다.");
            }

            // 3. 가장 왼쪽 열과 가장 오른쪽 열 선택
            var leftColumn = groupedByColumn.First().ToList();
            var rightColumn = groupedByColumn.Last().ToList();

            // 4. 각 열의 평균 좌표 계산 (결측 데이터 감안)
            var leftCenter = new PointF(
                leftColumn.Average(p => p.X),
                leftColumn.Any() ? leftColumn.Average(p => p.Y) : 0 // 데이터가 없으면 Y 좌표를 0으로 설정
            );

            var rightCenter = new PointF(
                rightColumn.Average(p => p.X),
                rightColumn.Any() ? rightColumn.Average(p => p.Y) : 0 // 데이터가 없으면 Y 좌표를 0으로 설정
            );

            // 5. 두 점 사이의 기울기를 이용해 회전 각도 계산
            double angle = Math.Atan2(rightCenter.Y - leftCenter.Y, rightCenter.X - leftCenter.X);
            return 0;
        }

        private static List<PointF> RotateCoordinates(List<RectangleF> listMetal, double angle)
        {
            List<PointF> rotatedPoints = new List<PointF>();
            foreach (var rect in listMetal)
            {
                PointF center = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                rotatedPoints.Add(RotatePoint(center, angle));
            }
            return rotatedPoints;
        }

        private static List<PointF> RotateCoordinatesBack(List<PointF> points, double angle)
        {
            List<PointF> rotatedBackPoints = new List<PointF>();
            foreach (var point in points)
            {
                rotatedBackPoints.Add(RotatePoint(point, angle));
            }
            return rotatedBackPoints;
        }

        private static PointF RotatePoint(PointF point, double angle)
        {
            double cosTheta = Math.Cos(angle);
            double sinTheta = Math.Sin(angle);

            float x = (float)(point.X * cosTheta - point.Y * sinTheta);
            float y = (float)(point.X * sinTheta + point.Y * cosTheta);

            return new PointF(x, y);
        }




        public void SaveOutLine(List<PointF> outline, int w, int h, string filename)
        {
            byte[] images = new byte[w * h];
            foreach (var point in outline)
            {
                //point의 좌표에 255를 넣어줌
                //point의 좌표 정합성을 체크 한다.
                if (point.X >= 0 && point.X < w && point.Y >= 0 && point.Y < h)
                {
                    int cx = (int)point.X;
                    int cy = (int)point.Y;
                    images[cy * w + cx] = 255;
                }


            }
            SaveImage(images, w, h, filename);
        }
        public QMC_ImageProcessFindAlignResult FindCirclesWidthCircleBoundary(List<RectangleF> circlesResult,
            byte[] pixelData, int w, int h, int radius, double dSpec, ref bool circleFound,
            int nCenterX = 0, int nCenterY = 0, bool bIsDarkCircleSearch = true
            , double miscellaneous_FiducialMarkSocre = 0.7
            , bool bSpiralSearch = true)
        {

            string strFileName = "d:\\Temp\\AlignOrg" + DateTime.Now.Ticks.ToString() + ".bmp";
            IsImageSave = true;
            SaveImage(pixelData, w, h, strFileName);
            if (bIsDarkCircleSearch == false)
            {
                //pixelData = InversImage(pixelData);
                //MeanFilter(pixelData, w, h, 10, 10);
                //SaveImage(pixelData, w, h, "polygonMeanFilter.bmp");
            }
            List<PointF> polygon = new List<PointF>();
            List<PointF> points = new List<PointF>();
            int nDivideCount = (int)(w / radius);
            int nStepX = (int)(radius / 2);
            int nStepY = (int)(radius / 2);
            int nDirectionX = 0;
            int nDirectionY = 0;
            bool bFindCircle = false;
            double dRadius = 0;
            float cx = 0;
            float cy = 0;
            double dErrorRatio = 0.2;
            int direction = 0; // 0: 오른쪽, 1: 위, 2: 왼쪽, 3: 아래
            int stepsInCurrentDirection = 1;
            int stepsTaken = 0;
            int directionChangeCount = 0;
            PointF currentPosition = new PointF
            {
                X = w / 2,
                Y = h / 2
            };
            if (bSpiralSearch == false)
            {
                nDivideCount = 3;
            }
            for (int y = 0; y < nDivideCount; y++)
            {
                if (bFindCircle)
                    break;
                int nShiftY = nDirectionY % 2 == 0 ? nStepY : -nStepY;
                nShiftY *= y;

                int nCy = h / 2 + nShiftY;
                nDirectionX = 0;
                for (int x = 0; x < nDivideCount; x++)
                {
                    int nShiftX = nDirectionX % 2 == 0 ? nStepX : -nStepX;
                    nShiftX *= x;
                    int nCx = w / 2 + nShiftX;

                    nCx = (int)currentPosition.X;
                    nCy = (int)currentPosition.Y;

                    int nMaxCircle = (int)(radius * (1 + dSpec));
                    int nMinCircle = (int)(radius * (1 - dSpec));

                    double dFirstSpec = dSpec * 3;
                    if (dFirstSpec > 0.5)
                    {
                        dFirstSpec = 0.5;
                    }
                    int nMaxCircleFirst = (int)(radius * 2);
                    int nMinCircleFirst = (int)(radius * (1 - dFirstSpec));

                    if (nMaxCircleFirst > 2000)
                    {
                        nMaxCircleFirst = 2000;
                    }
                    double dAngleStep = 2;
                    polygon = FindCircleBoundary(pixelData, w, h, nCx, nCy, (int)(radius / 2), (int)nMaxCircleFirst, dAngleStep, 10, bIsDarkCircleSearch);
                    points = polygon;
                    circlesResult.Clear();
                    FindCircleFitter(circlesResult, points, out dRadius, 5, radius, dSpec);

                    if (nCenterX == 0 || nCenterY == 0)
                    {
                        cx = circlesResult.Count > 0 ? circlesResult[0].X + circlesResult[0].Width / 2 : w / 2;
                        cy = circlesResult.Count > 0 ? circlesResult[0].Y + circlesResult[0].Height / 2 : h / 2;
                    }
                    else
                    {
                        cx = (float)nCenterX;
                        cy = (float)nCenterY;
                    }

                    if (dRadius < nMaxCircle && dRadius > nMinCircle && cx > 0 && cx < w
                        && cy < h && cy > 0)
                    {


                        cx = circlesResult.Count > 0 ? circlesResult[0].X + circlesResult[0].Width / 2 : w / 2;
                        cy = circlesResult.Count > 0 ? circlesResult[0].Y + circlesResult[0].Height / 2 : h / 2;
                        dErrorRatio = dSpec * 2;
                        if (dErrorRatio < 0.2)
                        {
                            dErrorRatio = 0.2;
                        }
                        polygon = FindCircleBoundary(pixelData, w, h, cx, cy, (int)(dRadius * (1 - dErrorRatio)), (int)(dRadius * (1 + dErrorRatio)), 1, 2, bIsDarkCircleSearch);


                        points = polygon;

                        circlesResult.Clear();
                        double dRadius2 = 0;
                        Circle center = FindCircleFitter(circlesResult, points, out dRadius2, 2, radius, dSpec);

                        if (Math.Abs((dRadius - dRadius2) / dRadius2) < 0.05)
                        {
                            //허상을 찾아는지 검사 한다.
                            double dScoreCheck = IsRealCircle(center, dRadius2, points, dSpec);
                            if (dScoreCheck > 0.8)
                            {
                                bFindCircle = true;
                                break;
                            }


                        }

                        //return circlesResult;
                    }
                    switch (direction)
                    {
                        case 0: // 오른쪽
                            currentPosition.X += nStepX;
                            break;
                        case 1: // 위
                            currentPosition.Y += nStepX;
                            break;
                        case 2: // 왼쪽
                            currentPosition.X -= nStepX;
                            break;
                        case 3: // 아래
                            currentPosition.Y -= nStepX;
                            break;
                    }

                    stepsTaken++;
                    if (stepsTaken == stepsInCurrentDirection)
                    {
                        stepsTaken = 0;
                        direction = (direction + 1) % 4; // 방향 전환
                        directionChangeCount++;

                        if (directionChangeCount % 2 == 0)
                        {
                            stepsInCurrentDirection++; // 두 번 방향 전환 후 이동 거리 증가
                        }
                    }




                }

            }

            //  원을 찾았는지 여부 Ref.
            circleFound = bFindCircle;

            if (bFindCircle == false)
            {
                circlesResult.Clear();
                strFileName = "d:\\Temp\\AlignFail" + DateTime.Now.Ticks.ToString() + ".bmp";
                IsImageSave = true;
                SaveImage(pixelData, w, h, strFileName);
                return new QMC_ImageProcessFindAlignResult();
            }

            cx = circlesResult.Count > 0 ? circlesResult[0].X + circlesResult[0].Width / 2 : w / 2;
            cy = circlesResult.Count > 0 ? circlesResult[0].Y + circlesResult[0].Height / 2 : h / 2;
            //SaveOutLine(polygon, w, h, "polygon.bmp");
            polygon = FindCircleBoundary(pixelData, w, h, cx, cy, (int)(dRadius * (1 - dErrorRatio)), (int)(dRadius * (1 + dErrorRatio)), 0.25, 1, bIsDarkCircleSearch);
            //(polygon, w, h, "polygon2.bmp");

            points = polygon;

            circlesResult.Clear();
            Circle resultCircle = FindCircleFitter(circlesResult, points, out dRadius, 4, radius, dSpec);
            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();

            double dScore = IsRealCircle(resultCircle, dRadius, points, dSpec);
            if (dScore > miscellaneous_FiducialMarkSocre)
            {
                bFindCircle = true;
                result.Circles.Add(resultCircle);
                result.ScoreCollection.Add(dScore);
            }
            else
            {
                circlesResult.Clear();
            }

            return result;
        }

        private double IsRealCircle(Circle center, double dRadius, List<PointF> points, double dSpec)
        {
            double dScore = 0;
            double dDistance = 0;
            int TotalCount = points.Count;
            int GoodCoount = 0;
            foreach (var point in points)
            {
                dDistance = Math.Pow(center.CenterX - point.X, 2) + Math.Pow(center.CenterY - point.Y, 2);
                dDistance = Math.Sqrt(dDistance);
                if (dRadius * (1 - dSpec) < dDistance && dDistance < dRadius * (1 + dSpec))
                {
                    GoodCoount++;
                }
            }
            dScore = (double)GoodCoount / (double)TotalCount;
            return dScore;
        }



        public List<Circle> FindGoldPowder(List<RectangleF> circlesResult, byte[] pixelData, int w, int h, ref bool circleFound, int Threshold = 75, double dScore = 0.7, int radius = 0, double dSpec = 0.1)
        {
            List<RectangleF> circlesResultLocal = new List<RectangleF>();
            List<PointF> polygon = new List<PointF>();
            List<PointF> points = new List<PointF>();
            int nStepX = w / 80;
            int nStepY = h / 80;

            double dErrorRatio = 0.3;
            List<List<Point>> blobs = new List<List<Point>>();

            List<List<Point>> list = FindBrightBlobs(pixelData, w, h, w, Threshold); // 영상 밝기 바뀌면 70 이게 쓰레스 홀드 입니다. 이거 변경 해야 됩니다.
            int MinArea = (int)(radius * radius * Math.PI * (1 - 0.2));
            int MaxArea = (int)(radius * radius * Math.PI * (1 + 0.2));
            blobs.AddRange(list.Where(t => t.Count() > MinArea && t.Count() < MaxArea).ToList());
            list.Clear();
            List<Circle> circles = new List<Circle>();
            foreach (List<Point> point in blobs)
            {
                if (point.Count == 0)
                    continue;

                // Width와 Height 계산
                int width = point.Max(p => p.X) - point.Min(p => p.X);
                int height = point.Max(p => p.Y) - point.Min(p => p.Y);

                // Width와 Height의 비율 계산
                float ratio = (float)width / height;
                float filter = 0.05f;
                // 비율이 0.9~1.1 사이인 경우만 처리
                if (ratio >= 1 - filter && ratio <= 1 + filter)
                {

                    // circlesResult에 추가
                    list.Add(point);
                }
            }
            blobs = list;


            double medianWidth = 0;
            double medianHeight = 0;
            // blobs 의 Width 와 Hight의 중간값을 도출 한다.
            // blobs 의 Width 와 Height의 중간값(평균)을 도출한다.
            if (blobs.Count > 0)
            {
                // 각 블랍의 Width와 Height를 계산
                var widths = blobs.Select(blob => blob.Max(p => p.X) - blob.Min(p => p.X)).OrderBy(t => t).ToList();
                var heights = blobs.Select(blob => blob.Max(p => p.Y) - blob.Min(p => p.Y)).OrderBy(t => t).ToList();

                // 중간값 계산
                medianWidth = widths.Count % 2 == 0
                    ? (widths[widths.Count / 2 - 1] + widths[widths.Count / 2]) / 2.0
                    : widths[widths.Count / 2];

                medianHeight = heights.Count % 2 == 0
                    ? (heights[heights.Count / 2 - 1] + heights[heights.Count / 2]) / 2.0
                    : heights[heights.Count / 2];

                Console.WriteLine($"Median Width: {medianWidth}, Median Height: {medianHeight}");
            }
            else
            {
                Console.WriteLine("No blobs found.");

                //  원을 찾았는지 여부 Ref.
                circleFound = false;

                circlesResult.Clear();
                return circles;
            }


            foreach (List<Point> point in blobs)
            {
                if (point.Count == 0)
                    continue;

                // Center 계산 (모든 Point의 평균)
                float centerX = (float)point.Average(p => p.X);
                float centerY = (float)point.Average(p => p.Y);
                int nTotalCount = point.Count;
                PointF center = new PointF(centerX, centerY);
                float Myradius = (float)((medianWidth + medianHeight) / 4);
                var pts = point.Where(p => GetDistance(p, center) < Myradius * 0.95).ToList();
                int nInCount = pts.Count();
                int nOutCount = nTotalCount - nInCount;
                double dGuessInCount = Math.PI * Myradius * Myradius;
                double Min = Math.Min(nTotalCount, dGuessInCount);
                double Max = Math.Max(nTotalCount, dGuessInCount);

                double dMyScore = ((nInCount - nOutCount) / dGuessInCount);
                if (dMyScore < dScore)
                    continue;
                centerX = (float)point.Average(p => p.X);
                centerY = (float)point.Average(p => p.Y);
                // medianWidth와 medianHeight를 사용하여 RectangleF 생성
                RectangleF rectangle = new RectangleF(
                    centerX - (float)medianWidth / 2,
                    centerY - (float)medianHeight / 2,
                    (float)medianWidth,
                    (float)medianHeight
                );
                if (radius * (1 - dSpec) < Myradius && Myradius < radius * (1 + dSpec))
                {
                    circles.Add(new Circle(centerX, centerY, Myradius));
                    //FindBestCircle()
                    // circlesResult에 추가
                    circlesResult.Add(rectangle);
                }
            }


            return circles;
        }

        private void Sobel(byte[,] image, byte[] imgForeign, int nWidth, int nHeight, int nStride, int nApertureSize)
        {
            //sobel 알고리즘을 구현 한다.

            int[,] m_nSobelX = new int[nApertureSize, nApertureSize];
            int[,] m_nSobelY = new int[nApertureSize, nApertureSize];

            int nValue = nApertureSize / 2;
            for (int x = -nValue; x <= nValue; x++)
            {
                for (int y = -nValue; y <= nValue; y++)
                {
                    m_nSobelX[x + nValue, y + nValue] = x;
                    m_nSobelY[x + nValue, y + nValue] = y;
                }
            }

            int ThreadCount = 5;
            int nStartY = nValue;
            int nYIncreament = nHeight / ThreadCount;
            System.Threading.Tasks.ParallelOptions opt = new System.Threading.Tasks.ParallelOptions();
            opt.MaxDegreeOfParallelism = ThreadCount;
            Parallel.Invoke(opt,
                        () =>
                        {
                            Sobel(image, imgForeign, nWidth, nHeight, nStride, nApertureSize, m_nSobelX, m_nSobelY, nValue
                                , nStartY, nStartY + nYIncreament);
                        },
                        () =>
                        {
                            Sobel(image, imgForeign, nWidth, nHeight, nStride, nApertureSize, m_nSobelX, m_nSobelY, nValue
                                , nStartY + nYIncreament * 1, nStartY + nYIncreament * 2);
                        }, () =>
                        {

                            Sobel(image, imgForeign, nWidth, nHeight, nStride, nApertureSize, m_nSobelX, m_nSobelY, nValue
                                , nStartY + nYIncreament * 2, nStartY + nYIncreament * 3);
                        }, () =>
                        {

                            Sobel(image, imgForeign, nWidth, nHeight, nStride, nApertureSize, m_nSobelX, m_nSobelY, nValue
                                , nStartY + nYIncreament * 3, nStartY + nYIncreament * 4);
                        }, () =>
                        {

                            Sobel(image, imgForeign, nWidth, nHeight, nStride, nApertureSize, m_nSobelX, m_nSobelY, nValue
                                , nStartY + nYIncreament * 4, nHeight - nValue);
                        });

        }
        private static void Sobel(byte[,] image, byte[] imgForeign, int nWidth, int nHeight, int nStride, int nApertureSize, int[,] m_nSobelX, int[,] m_nSobelY, int nValue
           , int StartY, int EndY)
        {
            for (int y = StartY; y < EndY; y++)
            {
                int nStrideY = y * nStride;

                for (int x = nValue; x < nWidth - nValue; x++)
                {
                    int myY = y;
                    int myStrideY = nStrideY;
                    int nGx = 0;
                    int nGy = 0;
                    for (int i = 0; i < nApertureSize; i++)
                    {
                        for (int j = 0; j < nApertureSize; j++)
                        {
                            nGx += image[x + i - nValue, myY + j - nValue] * m_nSobelX[i, j];
                            nGy += image[x + i - nValue, myY + j - nValue] * m_nSobelY[i, j];
                        }
                    }
                    int nG = Math.Abs(nGx) + Math.Abs(nGy);
                    if (nG > 255)
                    {
                        nG = 255;
                    }
                    imgForeign[x + myStrideY] = (byte)nG;
                }
            }
        }
        public QMC_ImageProcessFindAlignResult FindGoldPowderForAutoTreshold(List<RectangleF> circlesResult,
            byte[] pixelData, int w, int h, int radius, double dScore, double dSpec)
        {

            //List<RectangleF> result = new List<RectangleF>();
            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();
            List<Circle> BestCircle = new List<Circle>();
            double dMaxCount = 0;
            object obj = new object();
            int nThresholdMax = 0;
            Parallel.For(1, 20, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, threshold =>
            {
                int myThreshold = threshold * 7 + 50;
                bool bFound = false;
                var circles = FindGoldPowder(circlesResult, pixelData, w, h, ref bFound, myThreshold, dScore, radius, dSpec);
                int nCount = 0;
                foreach (var circle in circles)
                {
                    double dMin = Math.Min(circle.Radius, radius);
                    double dMax = Math.Max(circle.Radius, radius);
                    if (dMin / dMin > dScore)
                    {
                        nCount++;
                    }
                }
                lock (obj)
                {
                    if (dMaxCount < nCount)
                    {
                        dMaxCount = nCount;
                        BestCircle = circles;
                        nThresholdMax = myThreshold;


                    }
                    else if (dMaxCount == nCount)
                    {
                        if (nThresholdMax < myThreshold)
                        {
                            dMaxCount = nCount;
                            BestCircle = circles;
                            nThresholdMax = myThreshold;
                        }
                    }
                }

            });
            circlesResult.Clear();
            foreach (var circle in BestCircle)
            {
                circlesResult.Add(circle.GetBoundery());
                result.ScoreCollection.Add(circle.Score);
            }
            result.Circles.AddRange(BestCircle);

            return result;
        }


        public QMC_ImageProcessFindAlignResult FindCircleForFR4(byte[] pixelData, int w, int h, int r, double dSpec, double dScore)
        {
            List<RectangleF> circlesResult = new List<RectangleF>();
            QMC_ImageProcessFindAlignResult Qmcresult = new QMC_ImageProcessFindAlignResult();
            byte[,] image = new byte[w, h];
            int nSum = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    image[x, y] = pixelData[y * w + x];

                }
            }

            Sobel(image, pixelData, w, h, w, 3);
            int[] nHisto = new int[256];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    nHisto[pixelData[y * w + x]]++;
                }
            }
            int nCount = 0;
            int nThreshold = 0;
            int nNeedCount = (int)(r * r * Math.PI * 2);
            for (int i = 0; i < nHisto.Length; i++)
            {
                nCount += nHisto[255 - i];
                if (nCount > nNeedCount)
                {
                    nThreshold = 255 - i;
                    break;
                }
            }
            IsImageSave = true;
            SaveImage(pixelData, w, h, "d:\\test\\Sobel.bmp");
            List<PointF> polygons;
            IOrderedEnumerable<List<Point>> blobSorted;
            double dRadius;
            Circle result;
            bool bFind = false;
            while (true)
            {
                circlesResult.Clear();
                FindCircleFR4(pixelData, w, h, r, circlesResult, nThreshold, out polygons, out blobSorted, out dRadius, out result);

                if (r * 0.95 < result.Radius && result.Radius < r * 1.05)
                {
                    if (result.Score > dScore)
                    {
                        bFind = true;
                        Qmcresult.Circles.Add(result);
                        return Qmcresult;
                    }

                }

                nThreshold += 5;
                if (nThreshold > 255)
                    break;
            }


            //if(polygons.Count > 1000)
            //{
            //    FindCircleFitter(circlesResult, polygons, out dRadius,1,r);
            //}

            return Qmcresult;
        }

        private void FindCircleFR4(byte[] pixelData, int w, int h, int r, List<RectangleF> circlesResult, int nThreshold, out List<PointF> polygons, out IOrderedEnumerable<List<Point>> blobSorted, out double dRadius, out Circle result)
        {
            List<List<Point>> blobs = FindBrightBlobs(pixelData, w, h, w, (int)nThreshold);
            result = new Circle();
            polygons = new List<PointF>();
            blobSorted = blobs.OrderByDescending(t => t.Count);
            int No = 0;
            dRadius = 0;
            List<Point> Outline = GetOutLine(blobs);
            if (Outline != null)
            {
                foreach (var v in Outline)
                {
                    polygons.Add(new PointF(v.X, v.Y));
                }
            }

            if (polygons.Count > 2 * Math.PI * r / 5)
            {
                result = FindCircleFitter(circlesResult, polygons, out dRadius, 3, r, 0.1);

            }

        }

        private List<PointF> FindCircleBoundary(byte[] pixelData, int width, int height, float cx, float cy, int initialRadius = 50, int maxRadius = 1000, double angleStep = 0.11, int step = 10, bool bIsDarkCircleSearch = false)
        {
            List<PointF> boundaryPoints = new List<PointF>();

            if (pixelData == null) //pixelData가 null인 경우 프로그램 다운.
                return boundaryPoints;


            maxRadius = Math.Min(Math.Min(width, height) / 2, maxRadius);
            int pixelAverageCount = 20;

            for (double angle = 0; angle < 360; angle += angleStep)
            {
                double radian = angle * Math.PI / 180;
                double maxDifferenceD = 0;
                double maxDifferenceW = 0;

                double dSin = Math.Sin(radian);
                double dCos = Math.Cos(radian);
                PointF boundaryPointW = new PointF(cx, cy);
                PointF boundaryPointD = new PointF(cx, cy);


                // 병렬 처리
                object lockObject = new object();
                Parallel.For((int)initialRadius, (int)maxRadius, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, r =>
                {
                    float x = cx + (int)(r * dCos);
                    float y = cy + (int)(r * dSin);

                    if (x < 0 || x >= width || y < 0 || y >= height)
                        return;

                    double currentAverage = GetPixelAverage(pixelData, width, height, x, y, pixelAverageCount, radian, true);
                    double nextAverage = GetPixelAverage(pixelData, width, height, x, y, pixelAverageCount, radian, false);

                    double differenceD = 0;
                    double differenceW = 0;
                    //if (bIsDarkCircleSearch == false)
                    {
                        differenceW = (currentAverage - nextAverage) / nextAverage;

                    }
                    // else
                    {
                        differenceD = (nextAverage - currentAverage) / currentAverage;
                    }


                    lock (lockObject)
                    {
                        if (differenceD > maxDifferenceD)
                        {
                            maxDifferenceD = differenceD;
                            boundaryPointD = new PointF(x, y);
                        }
                        if (differenceW > maxDifferenceW)
                        {
                            maxDifferenceW = differenceW;
                            boundaryPointW = new PointF(x, y);
                        }
                    }
                });
                //double dW = GetDistance(boundaryPointW, new PointF(cx, cy));
                //double dD = GetDistance(boundaryPointD, new PointF(cx, cy));
                if (bIsDarkCircleSearch)
                {
                    boundaryPoints.Add(boundaryPointD);
                }
                else
                {
                    boundaryPoints.Add(boundaryPointW);

                }
            }

            return boundaryPoints;
        }

        private int GetPixelAverage(byte[] pixelData, int width, int height, float x, float y, int count, double radian, bool isCurrent)
        {
            int sum = 0;
            int step = isCurrent ? -1 : 1;

            double dSin = Math.Sin(radian);
            double dCos = Math.Cos(radian);
            for (int i = 0; i < count; i++)
            {
                int newX = (int)(x + i * step * dCos);
                int newY = (int)(y + i * step * dSin);

                if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                    continue;

                sum += pixelData[newY * width + newX];
            }

            return sum / count;
        }


        private bool IsSameCircle(List<RectangleF> circles, out double def)
        {
            def = 99999999;
            if (circles.Count < 2)
            {
                return false;
            }
            RectangleF circle1 = circles[0];

            for (int i = 1; i < circles.Count; i++)
            {
                RectangleF circle2 = circles[i];
                def = Math.Pow(circle1.X - circle2.X, 2) + Math.Pow(circle1.Y - circle2.Y, 2);
                def = Math.Sqrt(def);

                if (def > 10)
                {
                    return false;
                }
            }
            return true;
        }

        private byte[] HistogramEqualization(byte[] pixelData, int w, int h)
        {
            byte[] result = new byte[pixelData.Length];
            int[] histogram = new int[256];
            for (int i = 0; i < pixelData.Length; i++)
            {
                histogram[pixelData[i]]++;
            }
            int[] cumulativeHistogram = new int[256];
            cumulativeHistogram[0] = histogram[0];
            for (int i = 1; i < 256; i++)
            {
                cumulativeHistogram[i] = cumulativeHistogram[i - 1] + histogram[i];
            }
            for (int i = 0; i < pixelData.Length; i++)
            {
                result[i] = (byte)(cumulativeHistogram[pixelData[i]] * 255 / (w * h));
            }
            return result;
        }

        private byte[] InversImage(byte[] image)
        {
            byte[] imageInvers = new byte[image.Length];
            for (int i = 0; i < image.Length; i++)
            {
                imageInvers[i] = (byte)(255 - image[i]);
            }
            return imageInvers;
        }
        private byte[] AddImage(byte[] image1, byte[] image2)
        {
            byte[] image = new byte[image1.Length];
            for (int i = 0; i < image1.Length; i++)
            {
                int nValue = image1[i] + image2[i];
                if (nValue > 255)
                {
                    nValue = 255;
                }
                image[i] = (byte)nValue;
            }
            return image;
        }
        private void SaveImage(byte[] image, int w, int h, string fileName)
        {
            if (IsImageSave == false)
            {
                return;
            }
            try
            {
                // 디렉토리 없으면 생성
                string dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format8bppIndexed);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                //비트맵 8ibt Gray 파레트 추가
                ColorPalette grayPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    grayPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = grayPalette;
                System.Runtime.InteropServices.Marshal.Copy(image, 0, bitmapData.Scan0, image.Length);
                bitmap.UnlockBits(bitmapData);
                bitmap.Save(fileName);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

        }
        int m_nFarField = 10;
        double dCutoffFrequence = 0.5;
        private byte[] HighpassFilterHolizontal(byte[] image, int w, int h, int cx, int cy)
        {
            byte[] imageHighPass = new byte[w * h];

            for (int y = 0; y < h; y++)
            {
                for (int x = Math.Min(w / 2, cx) - m_nFarField; x > m_nFarField; x--)
                {
                    int nBeforeSum = 0;
                    int nAfterSum = 0;
                    for (int iter = m_nFarField - 2; iter < m_nFarField; iter++)
                    {
                        nBeforeSum += image[y * w + x - iter];
                        nAfterSum += image[y * w + x + iter];
                    }
                    double dBeforAverage = nBeforeSum / 2;
                    double dAfterAverage = nAfterSum / 2;
                    double dHighpassValue = dBeforAverage - image[y * w + x];
                    if (dHighpassValue > 40)
                    {
                        dHighpassValue = 255;
                    }
                    else
                    {
                        dHighpassValue = 0;
                    }
                    imageHighPass[y * w + x] = (byte)dHighpassValue;
                }
                for (int x = Math.Max(w / 2, cx) + m_nFarField; x < w - m_nFarField; x++)
                {
                    int nBeforeSum = 0;
                    int nAfterSum = 0;
                    for (int iter = m_nFarField - 2; iter < m_nFarField; iter++)
                    {
                        nBeforeSum += image[y * w + x + iter];
                        nAfterSum += image[y * w + x - iter];
                    }
                    double dBeforAverage = nBeforeSum / 2;
                    double dAfterAverage = nAfterSum / 2;
                    double dHighpassValue = dBeforAverage - image[y * w + x];
                    if (dHighpassValue > 40)
                    {
                        dHighpassValue = 255;
                    }
                    else
                    {
                        dHighpassValue = 0;
                    }
                    imageHighPass[y * w + x] = (byte)dHighpassValue;
                }
            }

            //imageHighPass를 image로 복사
            return imageHighPass;
        }
        private byte[] LowpassFilterHolizontal(byte[] image, int w, int h, int cx, int cy)
        {

            byte[] imageLowPass = new byte[w * h];
            for (int y = 0; y < h; y++)
            {
                double dxValue = image[y * w + cx];
                double PrevValue = dxValue;
                for (int x = Math.Min(w / 2, cx); x > 0; x--)
                {
                    double dValue = image[y * w + x];
                    double dLowpassValue = PrevValue + dCutoffFrequence * (dValue - PrevValue);
                    imageLowPass[y * w + x] = (byte)dLowpassValue;
                    PrevValue = dLowpassValue;
                }

                dxValue = image[y * w + cx];
                PrevValue = dxValue;
                for (int x = Math.Max(w / 2, cx); x < w; x++)
                {
                    double dValue = image[y * w + x];
                    double dLowpassValue = PrevValue + dCutoffFrequence * (dValue - PrevValue);
                    imageLowPass[y * w + x] = (byte)dLowpassValue;
                    PrevValue = dLowpassValue;
                }
            }
            //imageLowPass를 image로 복사
            return imageLowPass;
        }
        //버티컬 로패스 필터를 만든다.

        private byte[] LowpassFilterVertical(byte[] image, int w, int h, int cx, int cy)
        {

            byte[] imageLowPass = new byte[w * h];
            for (int x = 0; x < w; x++)
            {
                double dxValue = image[cy * w + x];
                double PrevValue = dxValue;
                for (int y = Math.Min(h / 2, cy); y > 0; y--)
                {
                    double dValue = image[y * w + x];
                    double dLowpassValue = PrevValue + dCutoffFrequence * (dValue - PrevValue);
                    imageLowPass[y * w + x] = (byte)dLowpassValue;
                    PrevValue = dLowpassValue;
                }
                dxValue = image[cy * w + x];
                PrevValue = dxValue;
                for (int y = Math.Max(h / 2, cy); y < h; y++)
                {
                    double dValue = image[y * w + x];
                    double dLowpassValue = PrevValue + dCutoffFrequence * (dValue - PrevValue);
                    imageLowPass[y * w + x] = (byte)dLowpassValue;
                    PrevValue = dLowpassValue;
                }
            }
            //imageLowPass를 image로 복사
            return imageLowPass;
        }

        //버티컬 하이패스 필터를 만든다.

        private byte[] HighpassFilterVertical(byte[] image, int w, int h, int cx, int cy)
        {

            byte[] imageHighPass = new byte[w * h];
            for (int x = 0; x < w; x++)
            {
                for (int y = Math.Min(h / 2, cy) - m_nFarField; y > m_nFarField; y--)
                {
                    int nBeforeSum = 0;
                    int nAfterSum = 0;
                    for (int iter = m_nFarField - 2; iter < m_nFarField; iter++)
                    {
                        nBeforeSum += image[(y - iter) * w + x];
                        nAfterSum += image[(y + iter) * w + x];
                    }
                    double dBeforAverage = nBeforeSum / 2;
                    double dAfterAverage = nAfterSum / 2;
                    double dHighpassValue = dBeforAverage - nAfterSum;
                    if (dHighpassValue > 40)
                    {
                        dHighpassValue = 255;
                    }
                    else
                    {
                        dHighpassValue = 0;
                    }
                    imageHighPass[y * w + x] = (byte)dHighpassValue;
                }
                for (int y = Math.Max(h / 2, cy) + m_nFarField; y < h - m_nFarField; y++)
                {
                    int nBeforeSum = 0;
                    int nAfterSum = 0;

                    for (int iter = m_nFarField - 2; iter < m_nFarField; iter++)
                    {
                        nBeforeSum += image[(y + iter) * w + x];
                        nAfterSum += image[(y - iter) * w + x];
                    }

                    double dBeforAverage = nBeforeSum / 2;
                    double dAfterAverage = nAfterSum / 2;

                    double dHighpassValue = dBeforAverage - nAfterSum;
                    if (dHighpassValue > 40)
                    {
                        dHighpassValue = 255;
                    }
                    else
                    {
                        dHighpassValue = 0;
                    }
                    imageHighPass[y * w + x] = (byte)dHighpassValue;
                }
            }
            //imageHighPass를 image로 복사
            return imageHighPass;
        }

        private void blob(byte[] pixelData, int w, int h, QMC_ImageProcessFindAlignRecipe recipe, List<RectangleF> circles)
        {

            // 어두운 부분의 블랍을 찾음
            List<List<Point>> blobs = FindDarkBlobs(pixelData, w, h, w, recipe.Threshold);
            FindCircleRule(recipe, circles, blobs);
        }

        private void FindCircleRule(QMC_ImageProcessFindAlignRecipe recipe, List<RectangleF> circles, List<List<Point>> blobs)
        {
            // 가장 큰 블랍을 선택
            var outline = GetOutLine(blobs);
            double radius = 0;
            FindCircleRule(recipe, circles, outline, out radius);
        }

        private List<Point> GetOutLine(List<List<Point>> blobs)
        {
            List<Point> largestBlob = null;
            double dBlobRatio = 0;
            if (blobs.Count() >= 2)
            {
                var v = blobs.OrderByDescending(t => t.Count()).Take(2);
                // 가로 세로 블랍의 비율이 1:1에 가까운 블랍을 선택

                foreach (var blob in v)
                {
                    double dWidth = blob.Max(t => t.X) - blob.Min(t => t.X);
                    double dHeight = blob.Max(t => t.Y) - blob.Min(t => t.Y);
                    double dMin = Math.Min(dWidth, dHeight);
                    double dMax = Math.Max(dWidth, dHeight);
                    double dRatio = dMin / dMax;
                    if (dRatio > dBlobRatio)
                    {
                        dBlobRatio = dRatio;
                        largestBlob = blob;
                    }
                }


            }

            //foreach (var blob in blobs)
            //{
            //    if (blob.Count > maxBlobSize)
            //    {
            //        maxBlobSize = blob.Count;
            //        largestBlob = blob;
            //    }
            //}
            List<Point> outline = null;
            if (largestBlob != null)
            {
                outline = GetBlobOutline(largestBlob);
                //
            }
            return outline;
        }

        private void FindCircleRule(QMC_ImageProcessFindAlignRecipe recipe, List<RectangleF> circles, List<Point> outline, out double radius)
        {
            double dCenterX, dCenterY;
            List<Point> top = FindCircleCenterTop(recipe, outline);
            List<Point> bottom = FindCircleCenterBottom(recipe, outline);
            List<Point> left = FindCircleCenterLeft(recipe, outline);
            List<Point> right = FindCircleCenterRight(recipe, outline);

            FindCenter(top, bottom, left, right, out dCenterX, out dCenterY);

            Point center = new Point((int)dCenterX, (int)dCenterY);

            radius = FindCircleRadius(center, outline);
            //center 로 RADIUS 만큼의 RectangleF을 구함
            RectangleF circle = new RectangleF(center.X - recipe.Radius, center.Y - recipe.Radius, 2 * recipe.Radius, 2 * recipe.Radius);
            circles.Add(circle);
        }

        private double FindCircleRadius(Point center, List<Point> outline)
        {
            double dRadius = 0;
            //distance를 int로 해서 가장 많은 빈도수가 나오는 r를 선택
            int nMultiple = 4;
            List<int> distances = new List<int>();
            foreach (var point in outline)
            {
                int distance = (int)(Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2)) / nMultiple);
                distances.Add(distance);
            }
            var distanceGroups = distances.GroupBy(t => t);
            int maxCount = distanceGroups.Max(t => t.Count());
            int maxDistance = distanceGroups.First(t => t.Count() == maxCount).Key;
            dRadius = maxDistance * nMultiple;
            return dRadius;
        }

        private void Hough(QMC_ImageProcessFindAlignRecipe recipe, byte[] pixelData, int w, int h, List<Point> points, List<RectangleF> circles)
        {
            double dCenterX = w / 2;
            double dCenterY = h / 2;
            if (circles.Count > 0)
            {
                dCenterX = circles.Average(t => t.X + t.Width / 2);
                dCenterY = circles.Average(t => t.Y + t.Height / 2);
            }

            Hough(pixelData, w, h, 80, points, dCenterX, dCenterY);

            dCenterX = points.Average(t => t.X);
            dCenterY = points.Average(t => t.Y);
            Point center = new Point((int)dCenterX, (int)dCenterY);

            //center 로 RADIUS 만큼의 RectangleF을 구함
            RectangleF circle = new RectangleF(center.X - recipe.Radius, center.Y - recipe.Radius, 2 * recipe.Radius, 2 * recipe.Radius);
            //circles.Clear();
            circles.Add(circle);
        }

        private void FindCenter(List<Point> top, List<Point> bottom, List<Point> left, List<Point> right, out double dCenterX, out double dCenterY)
        {
            List<List<Point>> allCenters = new List<List<Point>> { top, bottom, left, right };
            List<Point> selectedCenters = null;
            double minStdDev = double.MaxValue;

            foreach (var centers in allCenters)
            {
                if (centers.Count > 0)
                {
                    double stdDevX = CalculateStandardDeviation(centers.Select(p => p.X).ToList());
                    double stdDevY = CalculateStandardDeviation(centers.Select(p => p.Y).ToList());
                    double stdDev = (stdDevX + stdDevY) / 2;

                    if (stdDev < minStdDev)
                    {
                        minStdDev = stdDev;
                        selectedCenters = centers;
                    }
                }
            }

            if (selectedCenters != null && selectedCenters.Count > 0)
            {
                dCenterX = selectedCenters.Average(p => p.X);
                dCenterY = selectedCenters.Average(p => p.Y);
            }
            else
            {
                dCenterX = 0;
                dCenterY = 0;
            }
        }

        private double CalculateStandardDeviation(List<int> values)
        {
            double avg = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - avg) * (val - avg)).Sum();
            double stdDev = Math.Sqrt(sumOfSquaresOfDifferences / values.Count);
            return stdDev;
        }

        private List<Point> FindCircleCenterTop(QMC_ImageProcessFindAlignRecipe recipe, List<Point> outline)
        {
            double dXCenter = outline.Average(t => t.X);
            double dYCenter = outline.Average(t => t.Y);
            outline = outline.Where(t => t.X < dXCenter && t.Y < dYCenter).ToList();
            List<Point> centers = new List<Point>();
            outline = outline.OrderBy(t => t.X).ToList();
            for (int i = 0; i < outline.Count / 3; i++)
            {
                Point ptCenter = FindCircleCenter(outline, recipe.Radius, i);
                if (ptCenter.X > 0 && ptCenter.Y > 0)
                {
                    centers.Add(ptCenter);
                }
            }
            return centers;
        }






        private static Circle FindCircleFitter(List<RectangleF> circles, List<PointF> points, out double radius, double threshold = 10, int r = 0, double dSpec = 0.05)
        {
            int iter = points.Count;
            if (iter < 1000)
            {
                iter = 1000;
            }

            Circle fittedCircle = RansacCircleFitter.FitCircle(points, iter, threshold, r, dSpec);
            radius = fittedCircle.Radius;
            //center 로 RADIUS 만큼의 RectangleF을 구함
            RectangleF circle = new RectangleF((float)(fittedCircle.CenterX - radius), (float)(fittedCircle.CenterY - radius), (float)(2 * radius), (float)(2 * radius));

            circles.Add(circle);
            return fittedCircle;
        }

        //주석좀 달아줘라.
        private void Hough(byte[] image, int w, int h, int threshold, List<Point> points, double cx, double cy)
        {
            int rmax = 370;
            int[,] hough = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (image[y * w + x] > 0)
                    {
                        for (int r = 0; r < rmax; r++)
                        {
                            double theta = r * Math.PI / rmax;
                            int a = (int)(x - r * Math.Cos(theta));
                            int b = (int)(y - r * Math.Sin(theta));
                            if (a >= 0 && a < w && b >= 0 && b < h)
                            {
                                hough[a, b]++;
                            }
                        }
                    }
                }
            }
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (hough[x, y] > threshold)
                    {
                        points.Add(new Point(x, y));
                    }
                }
            }

        }

        private List<Point> FindCircleCenterBottom(QMC_ImageProcessFindAlignRecipe recipe, List<Point> outline)
        {
            double dXCenter = outline.Average(t => t.X);
            double dYCenter = outline.Average(t => t.Y);
            outline = outline.Where(t => t.X < dXCenter && t.Y > dYCenter).ToList();
            List<Point> centers = new List<Point>();
            outline = outline.OrderBy(t => t.X).ToList();
            for (int i = 0; i < outline.Count / 3; i++)
            {
                Point ptCenter = FindCircleCenter(outline, recipe.Radius, i);
                if (ptCenter.X > 0 && ptCenter.Y > 0)
                {
                    centers.Add(ptCenter);
                }
            }
            return centers;
        }

        private List<Point> FindCircleCenterLeft(QMC_ImageProcessFindAlignRecipe recipe, List<Point> outline)
        {
            double dXCenter = outline.Average(t => t.X);
            double dYCenter = outline.Average(t => t.Y);
            outline = outline.Where(t => t.X < dXCenter && t.Y < dYCenter).ToList();
            List<Point> centers = new List<Point>();
            outline = outline.OrderBy(t => t.Y).ToList();
            for (int i = 0; i < outline.Count / 3; i++)
            {
                Point ptCenter = FindCircleCenter(outline, recipe.Radius, i);
                if (ptCenter.X > 0 && ptCenter.Y > 0)
                {
                    centers.Add(ptCenter);
                }
            }
            return centers;
        }

        private List<Point> FindCircleCenterRight(QMC_ImageProcessFindAlignRecipe recipe, List<Point> outline)
        {
            double dXCenter = outline.Average(t => t.X);
            double dYCenter = outline.Average(t => t.Y);
            outline = outline.Where(t => t.X > dXCenter && t.Y < dYCenter).ToList();
            List<Point> centers = new List<Point>();
            outline = outline.OrderBy(t => t.Y).ToList();
            for (int i = 0; i < outline.Count / 3; i++)
            {
                Point ptCenter = FindCircleCenter(outline, recipe.Radius, i);
                if (ptCenter.X > 0 && ptCenter.Y > 0)
                {
                    centers.Add(ptCenter);
                }
            }
            return centers;
        }

        bool IsInCircle(int x, int y, int center_x, int center_y, int r)
        {
            //pointsCircle을 이용하여 원의 내부에 있는지 판단
            var v = pointsCircle.Where(t => t.X == x);
            if (v.Count() > 0)
            {
                // v의 최대 Y 값과 최소Y 값 사이에 Y가 있으면 true
                if (v.Max(t => t.Y) > y && v.Min(t => t.Y) < y)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        private List<Point> GetBlobOutline(List<Point> blob)
        {
            List<Point> outline = new List<Point>();
            {
                //blob의 경계를 찾음
                //blob을 GroupbyX 후 MaxY와 MinY의 2포인트를 outline 넣는다.
                //blob을 GroupbyY 후 MaxX와 MinX의 2포인트를 outline 넣는다.

                var groupByX = blob.GroupBy(t => t.X).Select(t => new { X = t.Key, MaxY = t.Max(p => p.Y), MinY = t.Min(p => p.Y) });
                var groupByY = blob.GroupBy(t => t.Y).Select(t => new { Y = t.Key, MaxX = t.Max(p => p.X), MinX = t.Min(p => p.X) });

                foreach (var item in groupByX)
                {
                    outline.Add(new Point(item.X, item.MaxY));
                    outline.Add(new Point(item.X, item.MinY));
                }
            }
            return outline;
        }

        private Point FindCircleCenter(List<Point> points, int r, int index)
        {
            //points의 랜덤한 3점을 선택하여 3점으로 원의 중점을 찾는다.

            List<Point> selectedPoints = new List<Point>();
            //int index = random.Next(points.Count / 3);
            int nStep = points.Count / 3;
            for (int iter = 0; iter < 3; iter++)
            {
                selectedPoints.Add(points[index + iter * nStep]);
            }

            Point center = FindCircleCenter(selectedPoints[0], selectedPoints[1], selectedPoints[2]);
            //center로 selectedPoints[0]의 거리와 r의 차가 r의 5% 이내이면 center를 반환

            double dDistance = Math.Sqrt(Math.Pow(selectedPoints[0].X - center.X, 2) + Math.Pow(selectedPoints[0].Y - center.Y, 2));
            return center;
            if (Math.Abs(dDistance - r) < r * 0.2)
            {
                return center;
            }
            else
            {
                return new Point(-1, -1);
            }
        }

        private Point FindCircleCenter(Point point1, Point point2, Point point3)
        {
            // point1, point2, point3을 이용하여 원의 중심을 찾는다.
            double x1 = point1.X, y1 = point1.Y;
            double x2 = point2.X, y2 = point2.Y;
            double x3 = point3.X, y3 = point3.Y;

            double a = x1 * (y2 - y3) - y1 * (x2 - x3) + x2 * y3 - x3 * y2;
            double b = (x1 * x1 + y1 * y1) * (y3 - y2) + (x2 * x2 + y2 * y2) * (y1 - y3) + (x3 * x3 + y3 * y3) * (y2 - y1);
            double c = (x1 * x1 + y1 * y1) * (x2 - x3) + (x2 * x2 + y2 * y2) * (x3 - x1) + (x3 * x3 + y3 * y3) * (x1 - x2);
            double d = (x1 * x1 + y1 * y1) * (x3 * y2 - x2 * y3) + (x2 * x2 + y2 * y2) * (x1 * y3 - x3 * y1) + (x3 * x3 + y3 * y3) * (x2 * y1 - x1 * y2);

            double centerX = -b / (2 * a);
            double centerY = -c / (2 * a);

            return new Point((int)centerX, (int)centerY);
        }

        private RectangleF FindCircles(List<Point> contours, int radius)
        {
            List<Point> circlePoints = new List<Point>();
            int radiusSquared = radius * radius;

            foreach (var point in contours)
            {
                foreach (var otherPoint in contours)
                {
                    if (point == otherPoint) continue;

                    int dx = point.X - otherPoint.X;
                    int dy = point.Y - otherPoint.Y;
                    int distanceSquared = dx * dx + dy * dy;

                    if (distanceSquared == radiusSquared)
                    {
                        circlePoints.Add(point);
                        break;
                    }
                }
            }

            if (circlePoints.Count == 0)
            {
                return RectangleF.Empty;
            }

            // 가장 큰 리스트의 원의 중심을 구함
            Point center = GetCircleCenter(circlePoints);

            // 원의 중심을 기준으로 경계 사각형을 반환
            return new RectangleF(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
        }

        private Point GetCircleCenter(List<Point> points)
        {
            int sumX = 0;
            int sumY = 0;

            foreach (var point in points)
            {
                sumX += point.X;
                sumY += point.Y;
            }

            int centerX = sumX / points.Count;
            int centerY = sumY / points.Count;

            return new Point(centerX, centerY);
        }

        public List<List<Point>> FindDarkBlobs(byte[] pixelData, int width, int height, int stride, int threshold)
        {
            List<List<Point>> blobs = new List<List<Point>>();

            bool[,] visited = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x; // 픽셀 데이터 인덱스 계산 (8비트 그레이스케일 형식)
                    byte pixelValue = pixelData[index];

                    if (pixelValue < threshold && !visited[x, y])
                    {
                        // 새로운 블랍을 찾음
                        List<Point> blob = FindBlob(pixelData, width, height, stride, x, y, threshold, visited, true);
                        blobs.Add(blob);
                    }
                }
            }

            return blobs;
        }

        public List<List<Point>> FindBrightBlobs(byte[] pixelData, int width, int height, int stride, int threshold)
        {
            List<List<Point>> blobs = new List<List<Point>>();

            bool[,] visited = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x; // 픽셀 데이터 인덱스 계산 (8비트 그레이스케일 형식)
                    byte pixelValue = pixelData[index];

                    if (pixelValue >= threshold && !visited[x, y])
                    {
                        // 새로운 블랍을 찾음
                        List<Point> blob = FindBlob(pixelData, width, height, stride, x, y, threshold, visited, false);
                        blobs.Add(blob);
                    }
                }
            }

            return blobs;
        }

        private List<Point> FindBlob(byte[] pixelData, int width, int height, int stride, int startX, int startY, int threshold, bool[,] visited, bool isDarkBlob)
        {
            List<Point> blob = new List<Point>();
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(new Point(startX, startY));

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();
                int x = point.X;
                int y = point.Y;

                if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y])
                    continue;

                int index = y * stride + x;
                byte pixelValue = pixelData[index];

                if ((isDarkBlob && pixelValue >= threshold) || (!isDarkBlob && pixelValue < threshold))
                    continue;

                visited[x, y] = true;
                blob.Add(point);

                queue.Enqueue(new Point(x - 1, y));
                queue.Enqueue(new Point(x + 1, y));
                queue.Enqueue(new Point(x, y - 1));
                queue.Enqueue(new Point(x, y + 1));
            }

            return blob;
        }

        public List<Point> FindContours(List<Point> points)
        {
            if (points == null || points.Count == 0)
                return new List<Point>();

            // 컨투어를 찾기 위해 Graham's scan 알고리즘을 사용
            points.Sort((p1, p2) => p1.X == p2.X ? p1.Y.CompareTo(p2.Y) : p1.X.CompareTo(p2.X));
            Point pivot = points[0];
            points.RemoveAt(0);

            points.Sort((p1, p2) =>
            {
                double angle1 = Math.Atan2(p1.Y - pivot.Y, p1.X - pivot.X);
                double angle2 = Math.Atan2(p2.Y - pivot.Y, p2.X - pivot.X);
                return angle1.CompareTo(angle2);
            });

            Stack<Point> hull = new Stack<Point>();
            hull.Push(pivot);
            hull.Push(points[0]);
            hull.Push(points[1]);

            try
            {
                for (int i = 2; i < points.Count; i++)
                {
                    Point top = hull.Pop();
                    while (IsCounterClockwise(hull.Peek(), top, points[i]) <= 0)
                    {
                        top = hull.Pop();
                    }
                    hull.Push(top);
                    hull.Push(points[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<Point>(hull);
        }

        private int IsCounterClockwise(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
        }

        private RectangleF GetBoundingBox(List<Point> points)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var point in points)
            {
                if (point.X < minX) minX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.X > maxX) maxX = point.X;
                if (point.Y > maxY) maxY = point.Y;
            }

            return new RectangleF(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        public byte[] ConvertBitmapToByteArray(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("8비트 그레이스케일 이미지만 지원됩니다.");
            }

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int bytes = bitmapData.Stride * bitmap.Height;
            byte[] pixelData = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, pixelData, 0, bytes);
            bitmap.UnlockBits(bitmapData);
            return pixelData;
        }
    }
}


