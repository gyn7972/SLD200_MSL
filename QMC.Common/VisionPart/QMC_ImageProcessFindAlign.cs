
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    // 결과 진단용 열거형 (Result 클래스 위쪽에 추가)
    public enum CircleSearchFailReason
    {
        None = 0,
        NotExecuted,
        InvalidInput,
        ImageNullOrEmpty,
        EdgePointInsufficient,
        RadiusOutOfTolerance,
        RansacUnstable,
        ScoreTooLow,
        ColorModeMismatch,
        NotFound
    }

    public class QMC_ImageProcessFindAlignResult
    {
        public List<Circle> Circles { get; set; }
        public List<double> ScoreCollection { get; set; }
        
        // === Diagnostics 추가 ===
        public CircleSearchFailReason FailReason { get; set; } = CircleSearchFailReason.None;
        public string FailMessage { get; set; } = "";
        public string Recommendation { get; set; } = "";
        public double ExpectedRadius { get; set; }          // 요청된 (입력) 반경
        public double MeasuredRadius { get; set; }          // 실제 피팅 반경
        public double Score { get; set; }                   // 단일 서클 시 대표 점수
        public int EdgePointCount { get; set; }             // 경계 후보 점 개수
        public double EdgeInlierRatio { get; set; }         // 유효(원 둘레 내) 점 비율
        public double AvgBrightness { get; set; }           // 영상 평균 밝기
        public double Contrast { get; set; }                // (표준편차)
        public bool Success => Circles.Count > 0 && FailReason == CircleSearchFailReason.None;

        // 사용자 안내용 최종 가이드 (BuildUserGuide 결과)
        public string UserGuide { get; set; } = "";


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

        private byte[] ExtractROI(byte[] src, int srcWidth, int srcHeight, int roiX, int roiY, int roiWidth, int roiHeight)
        {
            byte[] roi = new byte[roiWidth * roiHeight];
            for (int y = 0; y < roiHeight; y++)
            {
                for (int x = 0; x < roiWidth; x++)
                {
                    int srcIndex = (roiY + y) * srcWidth + (roiX + x);
                    int dstIndex = y * roiWidth + x;
                    roi[dstIndex] = src[srcIndex];
                }
            }
            return roi;
        }

        public QMC_ImageProcessFindAlignResult FindCirclesWidthCircleBoundary(
                                                List<RectangleF> circlesResult,
                                                byte[] pixelData, int w, int h, int radius, double dSpec, ref bool circleFound,
                                                int nCenterX = 0, int nCenterY = 0, bool bIsDarkCircleSearch = true,
                                                double miscellaneous_FiducialMarkSocre = 0.7,
                                                bool bSpiralSearch = true,
                                                Rectangle? roiRect = null, 
                                                double dResolution = 0.00137789) // ROI 파라미터 추가(옵션)
        {
            // ===========================================================
            // [공통 가이드 빌더] - 실패 시 자동으로 순차 점검 안내 생성
            // ===========================================================
            void AttachAdvice(QMC_ImageProcessFindAlignResult r)
            {
                if (r == null) return;
                r.UserGuide = BuildSequentialAdvice(
                    r, dSpec, miscellaneous_FiducialMarkSocre, bIsDarkCircleSearch, radius, dResolution);
            }

            string BuildSequentialAdvice(QMC_ImageProcessFindAlignResult r,
                                         double spec, double scoreTh,
                                         bool isDarkMode, int reqRadiusPx, double res)
            {
                if (r == null) return "";

                double expPx = Math.Max(0, r.ExpectedRadius);
                double meaPx = Math.Max(0, r.MeasuredRadius);
                double expMm = expPx * res;
                double meaMm = meaPx * res;
                double diffPx = (expPx > 0 && meaPx > 0) ? Math.Abs(meaPx - expPx) : 0.0;
                double diffRatio = (expPx > 0 && meaPx > 0) ? diffPx / Math.Max(1.0, expPx) : 0.0;
                double specPct = Math.Round(spec * 100.0, 1);
                double diffPct = Math.Round(diffRatio * 100.0, 1);

                bool lowContrast = r.Contrast < 15.0;
                bool lowBrightness = r.AvgBrightness < 50.0;
                bool polaritySuspect =
                    (r.FailReason == CircleSearchFailReason.NotFound ||
                     r.FailReason == CircleSearchFailReason.EdgePointInsufficient) &&
                    (lowContrast || lowBrightness);

                double suggestedSpec = (diffRatio > spec)
                    ? Math.Min(0.40, Math.Max(spec, Math.Round(diffRatio + 0.05, 3)))
                    : spec;
                double suggestedScoreTh = (r.Score < scoreTh)
                    ? Math.Max(0.55, Math.Round(scoreTh - 0.10, 2))
                    : scoreTh;

                var sb = new System.Text.StringBuilder();
                sb.AppendLine("[Operator Guide – 순차 점검]");

                // ① 색상 극성
                if (polaritySuspect)
                    sb.AppendLine($"1) 색상 극성: 현재 {(isDarkMode ? "DarkCircle(어두운 원)" : "BrightCircle(밝은 원)")} 탐색 → {(isDarkMode ? "밝은 원(BrightCircle)" : "어두운 원(DarkCircle)")} 전환 권장");
                else
                    sb.AppendLine($"1) 색상 극성: 유지 (Avg={r.AvgBrightness:0.0}, Contrast={r.Contrast:0.0})");

                // ② 사이즈(Spec)
                if (expPx > 0 && meaPx > 0)
                {
                    sb.AppendLine($"2) 사이즈(Spec): 기대 {expPx:0.0}px({expMm:0.000}mm) vs 측정 {meaPx:0.0}px({meaMm:0.000}mm)");
                    if (diffRatio > spec)
                        sb.AppendLine($"   - 편차 {diffPct:0.0}% > Spec {specPct:0.0}% → dSpec을 {specPct:0.0}% → {suggestedSpec * 100.0:0.0}% 로 완화 권장");
                    else
                        sb.AppendLine($"   - 편차 {diffPct:0.0}% ≤ Spec {specPct:0.0}% → 유지 가능");
                }
                else
                    sb.AppendLine("2) 사이즈(Spec): 유효 반경 없음 → 극성/조명/Score 항목 우선 점검");

                // ③ Score
                if (r.Score > 0)
                {
                    if (r.Score < scoreTh)
                        sb.AppendLine($"3) Score: {r.Score:0.00} < 기준 {scoreTh:0.00} → 기준을 {scoreTh:0.00} → {suggestedScoreTh:0.00} 완화 또는 대비 향상 권장");
                    else
                        sb.AppendLine($"3) Score: {r.Score:0.00} ≥ 기준 {scoreTh:0.00} → 유지");
                }
                else
                    sb.AppendLine("3) Score: 유효 점수 없음 → 극성/사이즈 항목 우선 점검");

                // 요약
                sb.Append("→ 액션: ");
                bool any = false;
                if (polaritySuspect) { sb.Append("극성 전환 "); any = true; }
                if (diffRatio > spec) { sb.Append(any ? "/ " : ""); sb.Append("dSpec 완화 "); any = true; }
                if (r.Score < scoreTh) { sb.Append(any ? "/ " : ""); sb.Append("Score 기준 완화 또는 대비 향상 "); any = true; }
                if (!any) sb.Append("변경 불필요(유지)");
                return sb.ToString();
            }
            // ===========================================================


            // ROI 파라미터 정규화
            int roiX = 0, roiY = 0, roiWidth = w, roiHeight = h;
            if (roiRect.HasValue)
            {
                Rectangle rr = roiRect.Value;
                if (rr.Width > 0 && rr.Height > 0)
                {
                    int left = Math.Max(0, rr.Left);
                    int top = Math.Max(0, rr.Top);
                    int right = Math.Min(w, rr.Right);
                    int bottom = Math.Min(h, rr.Bottom);
                    roiX = left;
                    roiY = top;
                    roiWidth = Math.Max(0, right - left);
                    roiHeight = Math.Max(0, bottom - top);
                }
            }
            bool bRoiUse = !(roiX == 0 && roiY == 0 && roiWidth == w && roiHeight == h);

            // ROI 픽셀 데이터 추출 (ROI 사용 시에만)
            byte[] roiPixelData = bRoiUse
                ? ExtractROI(pixelData, w, h, roiX, roiY, roiWidth, roiHeight)
                : pixelData;

            // 1. 폴더 생성 (날짜 기준)
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string baseDir = Path.Combine("d:\\TempAlign", dateFolder);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            // 2. 초기 원본 이미지 저장
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string rawImagePath = Path.Combine(baseDir, $"AlignRaw_{timestamp}.bmp");
            IsImageSave = false;
            SaveImage(pixelData, w, h, rawImagePath);

            if (bIsDarkCircleSearch == false)
            {
                // 필요 시 전처리
                //pixelData = InversImage(pixelData);
                //MeanFilter(pixelData, w, h, 10, 10);
            }

            List<PointF> polygon = new List<PointF>();
            List<PointF> points = new List<PointF>();
            int nDivideCount = (int)((bRoiUse ? roiWidth : w) / radius);
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

            // 최고 후보 추적용
            Circle bestCandidate = new Circle();
            double bestCandidateRadius = 0;
            double bestCandidateScore = -1.0;

            PointF currentPosition;
            if (bRoiUse)
            {
                currentPosition = new PointF
                {
                    X = roiWidth / 2,
                    Y = roiHeight / 2
                };
            }
            else
            {
                currentPosition = new PointF
                {
                    X = w / 2,
                    Y = h / 2
                };
            }

            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();

            if (bRoiUse)
            {
                // ROI 내에서 Spiral 탐색
                for (int y = 0; y < nDivideCount; y++)
                {
                    if (bFindCircle) 
                        break;

                    for (int x = 0; x < nDivideCount; x++)
                    {
                        int nCx = (int)currentPosition.X;
                        int nCy = (int)currentPosition.Y;

                        int nMaxCircle = (int)(radius * (1 + dSpec));
                        int nMinCircle = (int)(radius * (1 - dSpec));

                        double dFirstSpec = Math.Min(dSpec * 3, 0.5);
                        int nMaxCircleFirst = Math.Min((int)(radius * 2), 2000);
                        int nMinCircleFirst = (int)(radius * (1 - dFirstSpec));

                        double dAngleStep = 2;

                        // ROI 기반 원 경계 탐색
                        polygon = FindCircleBoundary(roiPixelData, roiWidth, roiHeight, nCx, nCy,
                            (int)(radius / 2), nMaxCircleFirst, dAngleStep, 10, bIsDarkCircleSearch);

                        points = polygon;
                        circlesResult.Clear();
                        FindCircleFitter(circlesResult, points, out dRadius, 5, radius, dSpec);

                        if (nCenterX == 0 || nCenterY == 0)
                        {
                            cx = circlesResult.Count > 0
                                ? circlesResult[0].X + circlesResult[0].Width / 2
                                : roiWidth / 2;
                            cy = circlesResult.Count > 0
                                ? circlesResult[0].Y + circlesResult[0].Height / 2
                                : roiHeight / 2;
                        }
                        else
                        {
                            cx = (float)nCenterX;
                            cy = (float)nCenterY;
                        }

                        if (dRadius < nMaxCircle && dRadius > nMinCircle && cx > 0 && cx < roiWidth
                            && cy < roiHeight && cy > 0)
                        {
                            dErrorRatio = Math.Max(dSpec * 2, 0.2);

                            // 정밀 탐색 (ROI)
                            polygon = FindCircleBoundary(roiPixelData, roiWidth, roiHeight,
                                (int)cx, (int)cy,
                                (int)(dRadius * (1 - dErrorRatio)), (int)(dRadius * (1 + dErrorRatio)),
                                1, 2, bIsDarkCircleSearch);

                            points = polygon;

                            circlesResult.Clear();
                            double dRadius2 = 0;
                            Circle center = FindCircleFitter(circlesResult, points, out dRadius2, 2, radius, dSpec);

                            // 최고 후보 갱신
                            if (dRadius2 > 0)
                            {
                                double tmpScore = IsRealCircle(center, dRadius2, points, dSpec);
                                if (tmpScore > bestCandidateScore)
                                {
                                    bestCandidateScore = tmpScore;
                                    bestCandidateRadius = dRadius2;
                                    bestCandidate = center;
                                }
                            }

                            if (Math.Abs((dRadius - dRadius2) / Math.Max(1e-6, dRadius2)) < 0.05)
                            {
                                double dScoreCheck = IsRealCircle(center, dRadius2, points, dSpec);
                                if (dScoreCheck > 0.8)
                                {
                                    bFindCircle = true;
                                    break;
                                }
                            }
                        }

                        // Spiral 이동
                        switch (direction)
                        {
                            case 0: currentPosition.X += nStepX; break; // 오른쪽
                            case 1: currentPosition.Y -= nStepX; break; // 위
                            case 2: currentPosition.X -= nStepX; break; // 왼쪽
                            case 3: currentPosition.Y += nStepX; break; // 아래
                        }

                        stepsTaken++;
                        if (stepsTaken == stepsInCurrentDirection)
                        {
                            stepsTaken = 0;
                            direction = (direction + 1) % 4;
                            directionChangeCount++;
                            if (directionChangeCount % 2 == 0) stepsInCurrentDirection++;
                        }
                    }
                }

                // ROI에서 찾은 좌표를 원본 기준으로 보정
                cx += roiX;
                cy += roiY;

                circleFound = bFindCircle;
                //if (!bFindCircle)
                //{
                //    // 실패 통계 및 최고 후보 기반 분류
                //    var statsRoiFail = ComputeBrightnessStats(pixelData);
                //    result.AvgBrightness = statsRoiFail.avg;
                //    result.Contrast = statsRoiFail.std;
                //    result.ExpectedRadius = radius;
                //    result.MeasuredRadius = bestCandidateRadius;
                //    result.Score = bestCandidateScore;

                //    if (bestCandidateRadius > 0 && bestCandidateRadius < radius)
                //    {
                //        double diffPx = radius - bestCandidateRadius;
                //        double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;

                //        bestCandidateRadius *= dResolution;
                //        diffPx *= dResolution;
                //        diffPct *= dResolution;

                //        FillFailure(result, CircleSearchFailReason.RadiusOutOfTolerance,
                //            $"설정한 사이즈보다 작아 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이={diffPx:0.0}px, {diffPct:0.0}%)"
                //            , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                //        //"Circle Size 재확인 또는 Spec 완화, 조명/포커스 조정 권장.");
                //        if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
                //        {
                //            FillFailure(result, CircleSearchFailReason.ScoreTooLow,
                //                $"설정한 Score({miscellaneous_FiducialMarkSocre:0.00})보다 낮아 미검출 (측정Score={bestCandidateScore:0.00})"
                //                , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                //            //"Score 기준을 약간 낮추거나 조명/노출로 대비 향상 후 재시도.");
                //        }
                //    }
                //    else if (bestCandidateRadius > 0 && bestCandidateRadius > radius)
                //    {
                //        double diffPx = bestCandidateRadius - radius;
                //        double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;

                //        double dRadiusR = 0.0;
                //        dRadiusR = radius * dResolution;
                //        bestCandidateRadius *= dResolution;
                //        diffPx *= dResolution;
                //        diffPct *= dResolution;

                //        var msg = new StringBuilder();
                //        msg.AppendFormat("설정한 사이즈보다 커서 미검출 (요청반경={0}mm, 측정반경={1:0.000}mm, 차이=+{2:0.0000}mm, {3:0.000}%)",
                //            dRadiusR, bestCandidateRadius, diffPx, diffPct);

                //        // Score 미달이면 동일 메시지에 추가
                //        if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
                //        {
                //            msg.AppendLine();
                //            msg.AppendFormat("설정한 Score({0:0.00})보다 낮아 미검출 (측정Score={1:0.00})",
                //                miscellaneous_FiducialMarkSocre, bestCandidateScore);
                //        }

                //        FillFailure(result,
                //            CircleSearchFailReason.RadiusOutOfTolerance,
                //            msg.ToString(),
                //            "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");


                //        //double dRadiusR = 0.0;
                //        //dRadiusR = radius * dResolution;
                //        //bestCandidateRadius *= dResolution;
                //        //diffPx *= dResolution;
                //        //diffPct *= dResolution;

                //        //FillFailure(result, CircleSearchFailReason.RadiusOutOfTolerance,
                //        //    $"설정한 사이즈보다 커서 미검출 (요청반경={dRadiusR}, 측정반경={bestCandidateRadius:0.0}, 차이=+{diffPx:0.0}px, {diffPct:0.0}%)"
                //        //    , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                //        ////"Circle Size 재확인 또는 Spec 완화, 조명/포커스 조정 권장.");
                //        //if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
                //        //{
                //        //    FillFailure(result, CircleSearchFailReason.ScoreTooLow,
                //        //        $"설정한 Score({miscellaneous_FiducialMarkSocre:0.00})보다 낮아 미검출 (측정Score={bestCandidateScore:0.00})"
                //        //        , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                //        //    //"Score 기준을 약간 낮추거나 조명/노출로 대비 향상 후 재시도.");
                //        //}
                //    }
                //    else if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
                //    {
                //        FillFailure(result, CircleSearchFailReason.ScoreTooLow,
                //            $"설정한 Score({miscellaneous_FiducialMarkSocre:0.00})보다 낮아 미검출 (측정Score={bestCandidateScore:0.00})"
                //            , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                //        //"Score 기준을 약간 낮추거나 조명/노출로 대비 향상 후 재시도.");
                //    }
                //    else
                //    {
                //        FillFailure(result, CircleSearchFailReason.NotFound,
                //            "원 미검출. 마크 색상/극성 구분 또는 조명 변경 필요.", "");
                //            //"Black/White(극성) 전환, 조명(RED/IR)/Exposure 조정 후 재시도.");
                //    }

                //    circlesResult.Clear();
                //    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                //    rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                //    IsImageSave = true;
                //    SaveImage(pixelData, w, h, rawImagePath);
                //    return result;
                //}

                // 마지막 정밀 탐색 (ROI 좌표로 계산 후 원본으로 보정)
                polygon = FindCircleBoundary(roiPixelData, roiWidth, roiHeight,
                    (int)(cx - roiX), (int)(cy - roiY),
                    (int)(dRadius * (1 - dErrorRatio)), (int)(dRadius * (1 + dErrorRatio)),
                    0.25, 1, bIsDarkCircleSearch);

                points = polygon;
                circlesResult.Clear();
                Circle resultCircle = FindCircleFitter(circlesResult, points, out dRadius, 4, radius, dSpec);

                double dScore = IsRealCircle(resultCircle, dRadius, points, dSpec);
                if (dScore > miscellaneous_FiducialMarkSocre)
                {
                    bFindCircle = true;

                    // ROI → 원본 좌표계로 보정
                    resultCircle.CenterX += roiX;
                    resultCircle.CenterY += roiY;
                    resultCircle.Radius = (float)dRadius;
                    resultCircle.Score = (float)dScore;
                    for (int i = 0; i < circlesResult.Count; i++)
                    {
                        RectangleF rc = circlesResult[i];
                        circlesResult[i] = new RectangleF(
                            rc.X + roiX,
                            rc.Y + roiY,
                            rc.Width,
                            rc.Height);
                    }

                    result.Circles.Add(resultCircle);
                    result.ScoreCollection.Add(dScore);
                    result.Score = dScore;
                }
                else
                {
                    // 실패 시에도 최고 후보 갱신
                    if (dScore > bestCandidateScore)
                    {
                        bestCandidateScore = dScore;
                        bestCandidateRadius = dRadius;
                        bestCandidate = resultCircle;
                    }

                    circlesResult.Clear();
                }
            }
            else
            {
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

                            // 최고 후보 갱신
                            if (dRadius2 > 0)
                            {
                                double tmpScore = IsRealCircle(center, dRadius2, points, dSpec);
                                if (tmpScore > bestCandidateScore)
                                {
                                    bestCandidateScore = tmpScore;
                                    bestCandidateRadius = dRadius2;
                                    bestCandidate = center;
                                }
                            }

                            if (Math.Abs((dRadius - dRadius2) / Math.Max(1e-6, dRadius2)) < 0.05)
                            {
                                double dScoreCheck = IsRealCircle(center, dRadius2, points, dSpec);
                                if (dScoreCheck > 0.8)
                                {
                                    bFindCircle = true;
                                    break;
                                }
                            }
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
                    // 실패 진단/로그 (원본 기준)
                    var stats = ComputeBrightnessStats(pixelData);
                    result.AvgBrightness = stats.avg;
                    result.Contrast = stats.std;
                    result.ExpectedRadius = radius;
                    result.MeasuredRadius = bestCandidateRadius;
                    result.Score = bestCandidateScore;

                    if (pixelData == null || pixelData.Length == 0)
                    {
                        FillFailure(result, CircleSearchFailReason.ImageNullOrEmpty,
                            "이미지 데이터가 비어 있음",
                            "카메라 Live / 이미지 캡처 상태 및 케이블, 노출 확인.");

                        circlesResult.Clear();
                        timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                        rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                        IsImageSave = true;
                        SaveImage(pixelData, w, h, rawImagePath);
                        return result;
                    }

                    if (radius <= 0)
                    {
                        FillFailure(result, CircleSearchFailReason.InvalidInput,
                            $"입력 반경이 0 이하 (radius={radius})",
                            "레시피 Fiducial Circle Size 설정을 확인.");

                        circlesResult.Clear();
                        timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                        rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                        IsImageSave = true;
                        SaveImage(pixelData, w, h, rawImagePath);
                        return result;
                    }

                    if (points != null && points.Count < 50)
                    {
                        FillFailure(result, CircleSearchFailReason.EdgePointInsufficient,
                            $"경계 후보 점 부족 (count={points.Count})",
                            "Spec 확장 / 조명 또는 노출 증가");
                        result.EdgePointCount = points.Count;
                        result.UserGuide = BuildUserGuide(result, dSpec * 100.0, miscellaneous_FiducialMarkSocre * 100.0);

                        circlesResult.Clear();
                        timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                        rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                        IsImageSave = true;
                        SaveImage(pixelData, w, h, rawImagePath);
                        return result;
                    }

                    // 최종 분류: Score 미달 / 사이즈 작음 / 사이즈 큼 / 기타
                    if (bestCandidateRadius > 0 && bestCandidateRadius < radius)
                    {
                        double diffPx = radius - bestCandidateRadius;
                        double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;
                        FillFailure(result, CircleSearchFailReason.RadiusOutOfTolerance,
                            $"설정한 사이즈보다 작아 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이={diffPx:0.0}px, {diffPct:0.0}%)"
                            , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                        //"Circle Size 재확인 또는 Spec 완화, 조명/포커스 조정 권장.");
                    }
                    else if (bestCandidateRadius > 0 && bestCandidateRadius > radius)
                    {
                        double diffPx = bestCandidateRadius - radius;
                        double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;
                        FillFailure(result, CircleSearchFailReason.RadiusOutOfTolerance,
                            $"설정한 사이즈보다 커서 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이=+{diffPx:0.0}px, {diffPct:0.0}%)"
                            , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                        //"Circle Size 재확인 또는 Spec 완화, 조명/포커스 조정 권장.");
                    }
                    else if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
                    {
                        FillFailure(result, CircleSearchFailReason.ScoreTooLow,
                            $"설정한 Score({miscellaneous_FiducialMarkSocre:0.00})보다 낮아 미검출 (측정Score={bestCandidateScore:0.00})"
                            , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                        //"Score 기준을 약간 낮추거나 조명/노출로 대비 향상 후 재시도.");
                    }
                    
                    else
                    {
                        FillFailure(result, CircleSearchFailReason.NotFound,
                            "원 미검출",
                            BuildGenericRecommendation(result, dSpec));
                    }
                    result.UserGuide = BuildUserGuide(result, dSpec * 100.0, miscellaneous_FiducialMarkSocre * 100.0);

                    circlesResult.Clear();
                    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                    rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                    IsImageSave = true;
                    SaveImage(pixelData, w, h, rawImagePath);
                    return result;
                }

                cx = circlesResult.Count > 0 ? circlesResult[0].X + circlesResult[0].Width / 2 : w / 2;
                cy = circlesResult.Count > 0 ? circlesResult[0].Y + circlesResult[0].Height / 2 : h / 2;

                polygon = FindCircleBoundary(pixelData, w, h, cx, cy, (int)(dRadius * (1 - dErrorRatio)), (int)(dRadius * (1 + dErrorRatio)), 0.25, 1, bIsDarkCircleSearch);

                points = polygon;

                circlesResult.Clear();
                Circle resultCircle2 = FindCircleFitter(circlesResult, points, out dRadius, 4, radius, dSpec);

                double dScore2 = IsRealCircle(resultCircle2, dRadius, points, dSpec);
                resultCircle2.Score = (float)dScore2;

                // 최고 후보 갱신
                if (dScore2 > bestCandidateScore)
                {
                    bestCandidateScore = dScore2;
                    bestCandidateRadius = dRadius;
                    bestCandidate = resultCircle2;
                }

                if (dScore2 > miscellaneous_FiducialMarkSocre)
                {
                    bFindCircle = true;
                    result.Circles.Add(resultCircle2);
                    result.ScoreCollection.Add(dScore2);
                    result.Score = dScore2;

                    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                    rawImagePath = Path.Combine(baseDir, $"Align_OK_{timestamp}.bmp");
                }
                else
                {
                    circlesResult.Clear();
                }
            }


            // ── 공통 통계/실패 처리 (교체 버전) ─────────────────────────────────────────────
            var stats1 = ComputeBrightnessStats(pixelData);
            result.AvgBrightness = stats1.avg;
            result.Contrast = stats1.std;
            result.ExpectedRadius = radius;

            if (result.Success)
            {
                result.MeasuredRadius = result.Circles[0].Radius;
                result.Score = result.ScoreCollection.Count > 0 ? result.ScoreCollection[0] : 0;

                // 성공이어도 Spec 초과면 가이드에서 알려주기 위해 플래그만 남김
                if (Math.Abs(result.MeasuredRadius - radius) / Math.Max(1, radius) > dSpec)
                    result.FailReason = CircleSearchFailReason.RadiusOutOfTolerance;

                // 일원화된 안내 생성
                AttachAdvice(result);
                return result;
            }

            // 실패 공통 가드
            if (pixelData == null || pixelData.Length == 0)
            {
                result.FailReason = CircleSearchFailReason.ImageNullOrEmpty;

                circlesResult.Clear();
                timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                IsImageSave = true;
                SaveImage(pixelData, w, h, rawImagePath);

                AttachAdvice(result);
                return result;
            }

            if (radius <= 0)
            {
                result.FailReason = CircleSearchFailReason.InvalidInput;

                circlesResult.Clear();
                timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                IsImageSave = true;
                SaveImage(pixelData, w, h, rawImagePath);

                AttachAdvice(result);
                return result;
            }

            if (points != null && points.Count < 50)
            {
                result.FailReason = CircleSearchFailReason.EdgePointInsufficient;
                result.EdgePointCount = points.Count;

                circlesResult.Clear();
                timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
                IsImageSave = true;
                SaveImage(pixelData, w, h, rawImagePath);

                AttachAdvice(result);
                return result;
            }

            // 최종 분류 (미검출 도달 시) — 사유 플래그만 세팅
            result.MeasuredRadius = bestCandidateRadius;
            result.Score = bestCandidateScore;

            if (bestCandidateRadius > 0 && bestCandidateRadius < radius)
            {
                result.FailReason = CircleSearchFailReason.RadiusOutOfTolerance;
            }
            else if (bestCandidateRadius > 0 && bestCandidateRadius > radius)
            {
                result.FailReason = CircleSearchFailReason.RadiusOutOfTolerance;
            }
            else if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
            {
                result.FailReason = CircleSearchFailReason.ScoreTooLow;
            }
            else
            {
                result.FailReason = CircleSearchFailReason.NotFound;
            }

            circlesResult.Clear();
            timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
            IsImageSave = true;
            SaveImage(pixelData, w, h, rawImagePath);

            // 일원화된 안내 생성
            AttachAdvice(result);
            return result;

            //// 공통 통계/실패 처리
            //var stats1 = ComputeBrightnessStats(pixelData);
            //result.AvgBrightness = stats1.avg;
            //result.Contrast = stats1.std;
            //result.ExpectedRadius = radius;
            //if (result.Success)
            //{
            //    result.MeasuredRadius = result.Circles[0].Radius;
            //    result.Score = result.ScoreCollection.Count > 0 ? result.ScoreCollection[0] : 0;
            //    if (Math.Abs(result.MeasuredRadius - radius) / Math.Max(1, radius) > dSpec)
            //    {
            //        FillFailure(result,
            //            CircleSearchFailReason.RadiusOutOfTolerance,
            //            $"반경 편차 초과 (요청:{radius}, 측정:{result.MeasuredRadius:0.0})", "");
            //            //"Circle Size(Spec) 값을 키우거나 ROI/조명 재조정.");
            //    }
            //    return result;
            //}

            //// 실패 공통 가드
            //if (pixelData == null || pixelData.Length == 0)
            //{
            //    FillFailure(result,
            //        CircleSearchFailReason.ImageNullOrEmpty,
            //        "이미지 데이터가 비어 있음",
            //        "카메라 Live / 이미지 캡처 상태 및 케이블, 노출 확인.");

            //    circlesResult.Clear();
            //    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            //    rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
            //    IsImageSave = true;
            //    SaveImage(pixelData, w, h, rawImagePath);

            //    return result;
            //}

            //if (radius <= 0)
            //{
            //    FillFailure(result,
            //        CircleSearchFailReason.InvalidInput,
            //        $"입력 반경이 0 이하 (radius={radius})",
            //        "레시피 Fiducial Circle Size 설정을 확인.");

            //    circlesResult.Clear();
            //    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            //    rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
            //    IsImageSave = true;
            //    SaveImage(pixelData, w, h, rawImagePath);

            //    return result;
            //}

            //if (points != null && points.Count < 50)
            //{
            //    FillFailure(result,
            //        CircleSearchFailReason.EdgePointInsufficient,
            //        $"경계 후보 점 부족 (count={points.Count})",
            //        "Spec 확장 / 조명 또는 노출 증가");
            //    result.EdgePointCount = points.Count;
            //    result.UserGuide = BuildUserGuide(result, dSpec * 100.0, miscellaneous_FiducialMarkSocre * 100.0);

            //    circlesResult.Clear();
            //    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            //    rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
            //    IsImageSave = true;
            //    SaveImage(pixelData, w, h, rawImagePath);

            //    return result;
            //}

            //// 최종 분류 (미검출 도달 시)
            //result.MeasuredRadius = bestCandidateRadius;
            //result.Score = bestCandidateScore;

            //if (bestCandidateRadius > 0 && bestCandidateRadius < radius)
            //{
            //    double diffPx = radius - bestCandidateRadius;
            //    double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;
            //    FillFailure(result,
            //        CircleSearchFailReason.RadiusOutOfTolerance,
            //        $"설정한 사이즈보다 작아 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이={diffPx:0.0}px, {diffPct:0.0}%)"
            //        , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
            //    //"Circle Size 재확인 또는 Spec 완화, 조명/포커스 조정 권장.");
            //}
            //else if (bestCandidateRadius > 0 && bestCandidateRadius > radius)
            //{
            //    double diffPx = bestCandidateRadius - radius;
            //    double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;
            //    FillFailure(result,
            //        CircleSearchFailReason.RadiusOutOfTolerance,
            //        $"설정한 사이즈보다 커서 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이=+{diffPx:0.0}px, {diffPct:0.0}%)"
            //        , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
            //    //"Circle Size 재확인 또는 Spec 완화, 조명/포커스 조정 권장.");
            //}
            //else if (bestCandidateScore >= 0 && bestCandidateScore < miscellaneous_FiducialMarkSocre)
            //{
            //    FillFailure(result,
            //        CircleSearchFailReason.ScoreTooLow,
            //        $"설정한 Score({miscellaneous_FiducialMarkSocre:0.00})보다 낮아 미검출 (측정Score={bestCandidateScore:0.00})"
            //        , "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
            //    //"Score 기준을 약간 낮추거나 조명/노출로 대비 향상 후 재시도.");
            //}
            //else
            //{
            //    FillFailure(result,
            //        CircleSearchFailReason.NotFound,
            //        "원 미검출. 마크 색상/극성 구분 또는 조명 변경 필요.",
            //        "Black/White(극성) 전환, 조명(RED/IR)/Exposure 조정 후 재시도.");
            //}

            //result.UserGuide = BuildUserGuide(result, dSpec * 100.0, miscellaneous_FiducialMarkSocre * 100.0);

            //circlesResult.Clear();
            //timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            //rawImagePath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
            //IsImageSave = true;
            //SaveImage(pixelData, w, h, rawImagePath);

            //return result;
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
            int MinArea = (int)(radius * radius * Math.PI * (1 - 0.5));
            int MaxArea = (int)(radius * radius * Math.PI * (1 + 0.5));
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
                float filter = 0.2f;
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
                    circles.Add(new Circle(centerX, centerY, Myradius, (float)dMyScore));
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
            byte[] pixelData, int w, int h, int radius, double dScore, double dSpec, int nMaxInstance =20)
        {
            //List<RectangleF> result = new List<RectangleF>();
            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();
            List<Circle> BestCircle = new List<Circle>();
            double dMaxCount = 0;
            object obj = new object();
            int nThresholdMax = 0;

            // [1] 날짜 기반 폴더 생성
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string baseDir = Path.Combine("d:\\TempGoldpowder", dateFolder);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            // [2] 초기 원본 이미지 저장
            //string rawImagePath = Path.Combine(baseDir, $"AlignRaw_{DateTime.Now.Ticks}.bmp");
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");  // ex: 20250612_154512_123
            string rawImagePath = Path.Combine(baseDir, $"AlignRaw_{timestamp}.bmp");
            IsImageSave = true;
            SaveImage(pixelData, w, h, rawImagePath);

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
            var orderbyCircle = BestCircle.OrderByDescending(t => t.Score);
            int nResultCount = 0;
            foreach (var circle in orderbyCircle)
            {
                if (nResultCount >= nMaxInstance)
                    break;
                circlesResult.Add(circle.GetBoundery());
                result.ScoreCollection.Add(circle.Score);
                result.Circles.Add(circle);
                nResultCount++;
            }
            //result.Circles.AddRange(BestCircle);

            // [4] 성공/실패 여부 판단 및 저장
            timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");  // ex: 20250612_154512_123
            string fileName = (result.Circles.Count > 0)
                ? $"AlignSuccess_{timestamp}.bmp"
                : $"AlignFail_{timestamp}.bmp";
            string fullPath = Path.Combine(baseDir, fileName);

            //여기 다시 확인하자.
            //SaveImageWithOverlay(pixelData, w, h, BestCircle, fullPath);

            return result;
        }

        public QMC_ImageProcessFindAlignResult FindCircleForFR4(
    byte[] imageRaw, int w, int h, int r, double dSpec, double dScore,
    Rectangle? roiRect = null) // ROI 파라미터 추가(옵션)
        {
            // 결과 후보 추적
            Circle bestCircle = new Circle(0, 0, 0);
            double bestScore = -1.0;
            double bestRadius = 0.0;
            int bestThreshold = -1;
            int bestEdgeCount = 0;

            List<RectangleF> circlesResult = new List<RectangleF>();
            QMC_ImageProcessFindAlignResult Qmcresult = new QMC_ImageProcessFindAlignResult();

            // ROI 정규화
            int roiX = 0, roiY = 0, roiW = w, roiH = h;
            if (roiRect.HasValue)
            {
                var rr = roiRect.Value;
                if (rr.Width > 0 && rr.Height > 0)
                {
                    int left = Math.Max(0, rr.Left);
                    int top = Math.Max(0, rr.Top);
                    int right = Math.Min(w, rr.Right);
                    int bottom = Math.Min(h, rr.Bottom);
                    roiX = left;
                    roiY = top;
                    roiW = Math.Max(0, right - left);
                    roiH = Math.Max(0, bottom - top);
                }
            }
            bool useRoi = !(roiX == 0 && roiY == 0 && roiW == w && roiH == h);

            // ROI 소스 선택
            byte[] src = useRoi ? ExtractROI(imageRaw, w, h, roiX, roiY, roiW, roiH) : imageRaw;

            // 1. 폴더 생성 (날짜 기준)
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string baseDir = Path.Combine("d:\\TempAlign", dateFolder);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            // 2. 초기 원본 이미지 저장 (IsImageSave=false 이므로 저장되지 않음)
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string rawImagePath = Path.Combine(baseDir, $"AlignRaw_{timestamp}.bmp");
            IsImageSave = false;
            SaveImage(imageRaw, w, h, rawImagePath);

            // 2D 이미지 구성 (ROI 기준)
            byte[,] image2D = new byte[roiW, roiH];
            for (int yy = 0; yy < roiH; yy++)
                for (int xx = 0; xx < roiW; xx++)
                    image2D[xx, yy] = src[yy * roiW + xx];

            // Sobel(ROI 기준) -> sobelEdge
            byte[] sobelEdge = new byte[src.Length];
            Sobel(image2D, sobelEdge, roiW, roiH, roiW, 5);

            // 밝기/대비 통계 (원본 기준)
            var stats = ComputeBrightnessStats(imageRaw);
            Qmcresult.AvgBrightness = stats.avg;
            Qmcresult.Contrast = stats.std;
            Qmcresult.ExpectedRadius = r;

            // 히스토그램 기반 초기 Threshold (Sobel-ROI 기반)
            int[] hist = new int[256];
            for (int i = 0; i < sobelEdge.Length; i++) hist[sobelEdge[i]]++;
            int nNeedCount = (int)(r * r * Math.PI * 2); // 대략적 필요 엣지 개수 추정
            int accum = 0;
            int initialThreshold = 0;
            for (int i = 0; i < 256; i++)
            {
                accum += hist[255 - i];
                if (accum > nNeedCount)
                {
                    initialThreshold = 255 - i;
                    break;
                }
            }

            IsImageSave = true;
            SaveImage(sobelEdge, roiW, roiH, Path.Combine(baseDir, $"Sobel_{timestamp}.bmp"));

            // 탐색 루프 (ROI 기준)
            int nThreshold = initialThreshold;
            int safetyIter = 0;
            const int MAX_THRESHOLD = 255;

            while (nThreshold <= MAX_THRESHOLD)
            {
                safetyIter++;
                if (safetyIter > 100) // 무한 루프 방지
                    break;

                circlesResult.Clear();

                List<PointF> polygons;
                IOrderedEnumerable<List<Point>> blobSorted;
                double dRadius;
                Circle resultCircle;

                // ROI 기준 배열/크기 전달
                FindCircleFR4(sobelEdge, roiW, roiH, r, circlesResult, nThreshold,
                              out polygons, out blobSorted, out dRadius, out resultCircle);

                int edgeCount = polygons != null ? polygons.Count : 0;

                if (resultCircle.Radius > 0)
                {
                    // 1) ROI(로컬) 완전 포함성 검사
                    bool insideLocal = !useRoi ||
                                       (resultCircle.CenterX - resultCircle.Radius >= 0 &&
                                        resultCircle.CenterX + resultCircle.Radius <= roiW &&
                                        resultCircle.CenterY - resultCircle.Radius >= 0 &&
                                        resultCircle.CenterY + resultCircle.Radius <= roiH);
                    if (!insideLocal)
                    {
                        nThreshold += 5;
                        continue;
                    }

                    bool radiusInRange = (r * (1 - dSpec) <= resultCircle.Radius) &&
                                         (resultCircle.Radius <= r * (1 + dSpec));

                    double score = resultCircle.Score;

                    if (radiusInRange && score > dScore)
                    {
                        // 좌표 복원: ROI -> 원본
                        if (useRoi)
                        {
                            resultCircle.CenterX += roiX;
                            resultCircle.CenterY += roiY;
                        }

                        // 2) ROI(글로벌) 완전 포함성 재검사
                        if (useRoi)
                        {
                            if (resultCircle.CenterX - resultCircle.Radius < roiX ||
                                resultCircle.CenterX + resultCircle.Radius > roiX + roiW ||
                                resultCircle.CenterY - resultCircle.Radius < roiY ||
                                resultCircle.CenterY + resultCircle.Radius > roiY + roiH)
                            {
                                nThreshold += 5;
                                continue;
                            }
                        }

                        Qmcresult.Circles.Add(resultCircle);
                        Qmcresult.ScoreCollection.Add(score);
                        Qmcresult.MeasuredRadius = resultCircle.Radius;
                        Qmcresult.Score = score;
                        Qmcresult.EdgePointCount = edgeCount;
                        Qmcresult.EdgeInlierRatio = score;
                        return Qmcresult; // 성공 종료
                    }

                    // 최고 후보 갱신 (실패 시 진단용)
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestCircle = resultCircle;
                        bestRadius = resultCircle.Radius;
                        bestThreshold = nThreshold;
                        bestEdgeCount = edgeCount;
                    }
                }

                nThreshold += 5; // 임계값 증가
            }

            // 미검출 (최종 실패 진단: FindCirclesWidthCircleBoundary와 동일한 분류)
            Qmcresult.MeasuredRadius = bestRadius;
            Qmcresult.Score = bestScore;
            Qmcresult.EdgePointCount = bestEdgeCount;
            Qmcresult.EdgeInlierRatio = bestScore; // inlier ratio 성격으로 사용

            // 최종 분류: 사이즈 작음/큼/Score 미달/기타
            if (bestRadius > 0 && bestRadius < r)
            {
                double diffPx = r - bestRadius;
                double diffPct = (diffPx / Math.Max(1, r)) * 100.0;
                FillFailure(Qmcresult,
                    CircleSearchFailReason.RadiusOutOfTolerance,
                    $"설정한 사이즈보다 작아 미검출 (요청반경={r}, 측정반경={bestRadius:0.0}, 차이={diffPx:0.0}px, {diffPct:0.0}%)",
                    "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
            }
            else if (bestRadius > 0 && bestRadius > r)
            {
                double diffPx = bestRadius - r;
                double diffPct = (diffPx / Math.Max(1, r)) * 100.0;
                FillFailure(Qmcresult,
                    CircleSearchFailReason.RadiusOutOfTolerance,
                    $"설정한 사이즈보다 커서 미검출 (요청반경={r}, 측정반경={bestRadius:0.0}, 차이=+{diffPx:0.0}px, {diffPct:0.0}%)",
                    "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
            }
            else if (bestScore >= 0 && bestScore < dScore)
            {
                FillFailure(Qmcresult,
                    CircleSearchFailReason.ScoreTooLow,
                    $"설정한 Score({dScore:0.00})보다 낮아 미검출 (측정Score={bestScore:0.00}, Threshold:{bestThreshold})",
                    "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
            }
            else
            {
                FillFailure(Qmcresult,
                    CircleSearchFailReason.NotFound,
                    "원 미검출",
                    BuildGenericRecommendation(Qmcresult, dSpec));
            }

            Qmcresult.UserGuide = BuildUserGuide(Qmcresult, dSpec * 100.0, dScore * 100.0);

            // NG 저장
            timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string ngPath = Path.Combine(baseDir, $"Align_NG_{timestamp}.bmp");
            IsImageSave = true;
            SaveImage(imageRaw, w, h, ngPath);

            return Qmcresult;
        }

        //public QMC_ImageProcessFindAlignResult FindCircleForFR4(
        //                                    byte[] imageRaw, int w, int h, int r, double dSpec, double dScore,
        //                                    Rectangle? roiRect = null) // ROI 파라미터 추가(옵션)
        //{
        //    // 결과 후보 추적
        //    Circle bestCircle = new Circle(0, 0, 0);
        //    double bestScore = 0.0;
        //    double bestRadius = 0.0;
        //    int bestThreshold = -1;
        //    int bestEdgeCount = 0;

        //    List<RectangleF> circlesResult = new List<RectangleF>();
        //    QMC_ImageProcessFindAlignResult Qmcresult = new QMC_ImageProcessFindAlignResult();

        //    // ROI 정규화
        //    int roiX = 0, roiY = 0, roiW = w, roiH = h;
        //    if (roiRect.HasValue)
        //    {
        //        var rr = roiRect.Value;
        //        if (rr.Width > 0 && rr.Height > 0)
        //        {
        //            int left = Math.Max(0, rr.Left);
        //            int top = Math.Max(0, rr.Top);
        //            int right = Math.Min(w, rr.Right);
        //            int bottom = Math.Min(h, rr.Bottom);
        //            roiX = left;
        //            roiY = top;
        //            roiW = Math.Max(0, right - left);
        //            roiH = Math.Max(0, bottom - top);
        //        }
        //    }
        //    bool useRoi = !(roiX == 0 && roiY == 0 && roiW == w && roiH == h);

        //    // ROI 소스 선택
        //    byte[] src = useRoi ? ExtractROI(imageRaw, w, h, roiX, roiY, roiW, roiH) : imageRaw;

        //    // 1. 폴더 생성 (날짜 기준)
        //    string dateFolder = DateTime.Now.ToString("yyyyMMdd");
        //    string baseDir = Path.Combine("d:\\TempAlign", dateFolder);
        //    if (!Directory.Exists(baseDir))
        //        Directory.CreateDirectory(baseDir);

        //    // 2. 초기 원본 이미지 저장 (IsImageSave=false 이므로 저장되지 않음)
        //    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        //    string rawImagePath = Path.Combine(baseDir, $"AlignRaw_{timestamp}.bmp");
        //    IsImageSave = false;
        //    SaveImage(imageRaw, w, h, rawImagePath);

        //    // 2D 이미지 구성 (ROI 기준)
        //    byte[,] image2D = new byte[roiW, roiH];
        //    for (int yy = 0; yy < roiH; yy++)
        //        for (int xx = 0; xx < roiW; xx++)
        //            image2D[xx, yy] = src[yy * roiW + xx];

        //    // Sobel(ROI 기준) -> sobelEdge
        //    byte[] sobelEdge = new byte[src.Length];
        //    Sobel(image2D, sobelEdge, roiW, roiH, roiW, 5);

        //    // 밝기/대비 통계 (원본 기준)
        //    var stats = ComputeBrightnessStats(imageRaw);
        //    Qmcresult.AvgBrightness = stats.avg;
        //    Qmcresult.Contrast = stats.std;

        //    // 히스토그램 기반 초기 Threshold (Sobel-ROI 기반)
        //    int[] hist = new int[256];
        //    for (int i = 0; i < sobelEdge.Length; i++) hist[sobelEdge[i]]++;
        //    int nNeedCount = (int)(r * r * Math.PI * 2); // 대략적 필요 엣지 개수 추정
        //    int accum = 0;
        //    int initialThreshold = 0;
        //    for (int i = 0; i < 256; i++)
        //    {
        //        accum += hist[255 - i];
        //        if (accum > nNeedCount)
        //        {
        //            initialThreshold = 255 - i;
        //            break;
        //        }
        //    }

        //    IsImageSave = true;
        //    SaveImage(sobelEdge, roiW, roiH, Path.Combine(baseDir, $"Sobel_{timestamp}.bmp"));

        //    // 탐색 루프 (ROI 기준)
        //    int nThreshold = initialThreshold;
        //    int safetyIter = 0;
        //    const int MAX_THRESHOLD = 255;

        //    while (nThreshold <= MAX_THRESHOLD)
        //    {
        //        safetyIter++;
        //        if (safetyIter > 100) // 무한 루프 방지
        //            break;

        //        circlesResult.Clear();

        //        List<PointF> polygons;
        //        IOrderedEnumerable<List<Point>> blobSorted;
        //        double dRadius;
        //        Circle resultCircle;

        //        // ROI 기준 배열/크기 전달
        //        FindCircleFR4(sobelEdge, roiW, roiH, r, circlesResult, nThreshold,
        //                      out polygons, out blobSorted, out dRadius, out resultCircle);

        //        int edgeCount = polygons != null ? polygons.Count : 0;

        //        if (resultCircle.Radius > 0)
        //        {
        //            // 1) ROI(로컬) 완전 포함성 검사 추가
        //            bool insideLocal = !useRoi ||
        //                               (resultCircle.CenterX - resultCircle.Radius >= 0 &&
        //                                resultCircle.CenterX + resultCircle.Radius <= roiW &&
        //                                resultCircle.CenterY - resultCircle.Radius >= 0 &&
        //                                resultCircle.CenterY + resultCircle.Radius <= roiH);
        //            if (!insideLocal)
        //            {
        //                nThreshold += 5;
        //                continue;
        //            }

        //            bool radiusInRange = (r * (1 - dSpec) <= resultCircle.Radius) &&
        //                                 (resultCircle.Radius <= r * (1 + dSpec));

        //            double score = resultCircle.Score;

        //            if (radiusInRange && score > dScore)
        //            {
        //                // 좌표 복원: ROI -> 원본
        //                if (useRoi)
        //                {
        //                    resultCircle.CenterX += roiX;
        //                    resultCircle.CenterY += roiY;
        //                }

        //                // 2) ROI(글로벌) 완전 포함성 재검사
        //                if (useRoi)
        //                {
        //                    if (resultCircle.CenterX - resultCircle.Radius < roiX ||
        //                        resultCircle.CenterX + resultCircle.Radius > roiX + roiW ||
        //                        resultCircle.CenterY - resultCircle.Radius < roiY ||
        //                        resultCircle.CenterY + resultCircle.Radius > roiY + roiH)
        //                    {
        //                        nThreshold += 5;
        //                        continue;
        //                    }
        //                }

        //                Qmcresult.Circles.Add(resultCircle);
        //                Qmcresult.ScoreCollection.Add(score);
        //                Qmcresult.MeasuredRadius = resultCircle.Radius;
        //                Qmcresult.Score = score;
        //                Qmcresult.EdgePointCount = edgeCount;
        //                Qmcresult.EdgeInlierRatio = score;
        //                return Qmcresult; // 성공 종료
        //            }

        //            // 최고 후보 갱신 (실패 시 진단용)
        //            if (score > bestScore)
        //            {
        //                bestScore = score;
        //                bestCircle = resultCircle;
        //                bestRadius = resultCircle.Radius;
        //                bestThreshold = nThreshold;
        //                bestEdgeCount = edgeCount;
        //            }
        //        }

        //        nThreshold += 5; // 임계값 증가
        //    }

        //    // 미검출 (최종 실패 진단)
        //    Qmcresult.MeasuredRadius = bestRadius;
        //    Qmcresult.Score = bestScore;
        //    Qmcresult.EdgePointCount = bestEdgeCount;
        //    Qmcresult.EdgeInlierRatio = bestScore;

        //    if (Math.Abs(bestRadius - r) / Math.Max(1, r) > dSpec)
        //    {
        //        FillFailure(Qmcresult,
        //            CircleSearchFailReason.RadiusOutOfTolerance,
        //            $"반경 편차 초과 (요청:{r}, 측정:{bestRadius:0.0})", "");
        //            //"Circle Size 정확도 재확인 또는 Spec(dSpec) 완화, 조명/포커스 조정.");
        //        return Qmcresult;
        //    }

        //    FillFailure(Qmcresult,
        //        CircleSearchFailReason.ScoreTooLow,
        //        $"Score 부족 (기준:{dScore:0.00}, 측정:{bestScore:0.00}, Threshold:{bestThreshold})", "");
        //        //"조명(RED/IR) 또는 Exposure 조정 → 대비 향상\nSpec(dSpec) 완화 또는 Score 기준 하향(예: 0.80→0.60)\nFiducial 표면 청소/포커스 재조정 후 재시도.");
        //    return Qmcresult;
        //}


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
                result.Score = (float)Math.Sqrt(result.Score);
            }

        }

        private List<PointF> FindCircleBoundary(byte[] pixelData, 
                                                int width, 
                                                int height, 
                                                float cx, 
                                                float cy, 
                                                int initialRadius = 50, 
                                                int maxRadius = 1000, 
                                                double angleStep = 0.11, 
                                                int step = 10, 
                                                bool bIsDarkCircleSearch = false)
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


        private void SaveImageWithOverlay(byte[] pixelData, int w, int h, List<PointF> polygon, Circle? circle, string filename)
        {
            using (Bitmap bmp = new Bitmap(w, h, PixelFormat.Format8bppIndexed))
            {
                // 그레이스케일 팔레트 적용
                ColorPalette pal = bmp.Palette;
                for (int i = 0; i < 256; i++) pal.Entries[i] = Color.FromArgb(i, i, i);
                bmp.Palette = pal;

                // 이미지 데이터 복사
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, bmp.PixelFormat);
                int stride = bmpData.Stride;
                for (int y = 0; y < h; y++)
                {
                    Marshal.Copy(pixelData, y * w, bmpData.Scan0 + y * stride, w);
                }
                bmp.UnlockBits(bmpData);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Polygon 외곽선
                    if (polygon != null && polygon.Count > 1)
                    {
                        using (Pen pen = new Pen(Color.Lime, 1))
                        {
                            for (int i = 0; i < polygon.Count - 1; i++)
                                g.DrawLine(pen, polygon[i], polygon[i + 1]);
                            g.DrawLine(pen, polygon[polygon.Count - 1], polygon[0]); // 닫기
                        }
                    }

                    // 원 중심 및 반지름
                    if (circle != null)
                    {
                        using (Pen pen = new Pen(Color.Red, 2))
                        {
                            float cx = circle.Value.CenterX;
                            float cy = circle.Value.CenterY;
                            float r = circle.Value.Radius;
                            g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
                            g.DrawEllipse(Pens.Yellow, cx - 2, cy - 2, 4, 4); // 중심점 표시
                        }
                    }
                }

                bmp.Save(filename, ImageFormat.Bmp);
            }
        }

        private void SaveImageWithOverlay(byte[] pixelData, int w, int h, List<Circle> circles, string filename)
        {
            using (Bitmap bmp = new Bitmap(w, h, PixelFormat.Format8bppIndexed))
            {
                // 그레이스케일 팔레트 설정
                ColorPalette pal = bmp.Palette;
                for (int i = 0; i < 256; i++)
                    pal.Entries[i] = Color.FromArgb(i, i, i);
                bmp.Palette = pal;

                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, bmp.PixelFormat);
                int stride = bmpData.Stride;
                for (int y = 0; y < h; y++)
                {
                    Marshal.Copy(pixelData, y * w, bmpData.Scan0 + y * stride, w);
                }
                bmp.UnlockBits(bmpData);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (Pen pen = new Pen(Color.Red, 2))
                    {
                        foreach (var circle in circles)
                        {
                            float cx = circle.CenterX;
                            float cy = circle.CenterY;
                            float r = circle.Radius;
                            g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
                            g.DrawEllipse(Pens.Yellow, cx - 2, cy - 2, 4, 4); // 중심점
                        }
                    }
                }

                bmp.Save(filename, ImageFormat.Bmp);
            }
        }

        //public QMC_ImageProcessFindAlignResult FindCirclesWidthCircleBoundaryMultipleCircles(
        //                                        List<RectangleF> circlesResult,
        //                                        byte[] pixelData, int w, int h, int radius, double dSpec,
        //                                        int maxCircleCount = 20,
        //                                        bool bIsDarkCircleSearch = true,
        //                                        double scoreThreshold = 0.7,
        //                                        bool bSpiralSearch = true,
        //                                        Rectangle? roiRect = null)
        //{
        //    QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();

        //    // [1] 날짜 기반 폴더 생성
        //    string dateFolder = DateTime.Now.ToString("yyyyMMdd");
        //    string baseDir = Path.Combine("d:\\TempGoldpowder", dateFolder);
        //    if (!Directory.Exists(baseDir))
        //        Directory.CreateDirectory(baseDir);

        //    // [2] 초기 원본 이미지 저장
        //    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");  // ex: 20250612_154512_123
        //    string rawImagePath = Path.Combine(baseDir, $"Goldpowder_Raw_{timestamp}.bmp");
        //    IsImageSave = true;
        //    SaveImage(pixelData, w, h, rawImagePath);

        //    circlesResult.Clear();
        //    List<PointF> polygon = new List<PointF>();
        //    List<PointF> points = new List<PointF>();
        //    int step = (int)(radius * 0.8);
        //    int foundCount = 0;

        //    // 1) ROI 정규화 및 픽셀 버퍼 추출
        //    Rectangle roiIn = roiRect ?? new Rectangle(0, 0, w, h);
        //    int roiX = Math.Max(0, roiIn.Left);
        //    int roiY = Math.Max(0, roiIn.Top);
        //    int roiRight = Math.Min(w, roiIn.Right);
        //    int roiBottom = Math.Min(h, roiIn.Bottom);
        //    int roiW = Math.Max(0, roiRight - roiX);
        //    int roiH = Math.Max(0, roiBottom - roiY);

        //    bool useRoiBuffer = !(roiX == 0 && roiY == 0 && roiW == 0 && roiH == 0);
        //    byte[] src = useRoiBuffer ? ExtractROI(pixelData, w, h, roiX, roiY, roiW, roiH) : pixelData;

        //    if(useRoiBuffer == false)
        //    {
        //        if (roiW == 0)
        //            roiW = w;
        //        if (roiH == 0)
        //            roiH = h;
        //    }

        //    // 2) ROI 로컬 좌표계에서 스캔 (ROI 외부로는 절대 접근하지 않음)
        //    int xStartLocal = Math.Max(radius, 0);
        //    int yStartLocal = Math.Max(radius, 0);
        //    int xEndLocal = Math.Max(0, roiW - radius);
        //    int yEndLocal = Math.Max(0, roiH - radius);

        //    for (int y = yStartLocal; y < yEndLocal; y += step)
        //    {
        //        for (int x = xStartLocal; x < xEndLocal; x += step)
        //        {
        //            if (foundCount >= maxCircleCount)
        //                break;

        //            int nMaxCircle = (int)(radius * (1 + dSpec));
        //            int nMinCircle = (int)(radius * (1 - dSpec));

        //            double dFirstSpec = Math.Min(dSpec * 3, 0.5);
        //            int nMaxCircleFirst = Math.Min((int)(radius * 2), 2000);
        //            int nMinCircleFirst = (int)(radius * (1 - dFirstSpec));

        //            // ROI 버퍼만 사용
        //            polygon = FindCircleBoundary(src, roiW, roiH, x, y,
        //                                         (int)(radius / 2), nMaxCircleFirst,
        //                                         2, 10, bIsDarkCircleSearch);
        //            points = polygon;

        //            double dRadius1 = 0;
        //            List<RectangleF> tempCircleList = new List<RectangleF>();
        //            FindCircleFitter(tempCircleList, points, out dRadius1, 5, radius, dSpec);

        //            if (dRadius1 > nMinCircle && dRadius1 < nMaxCircle)
        //            {
        //                float cxLocal = tempCircleList[0].X + tempCircleList[0].Width / 2;
        //                float cyLocal = tempCircleList[0].Y + tempCircleList[0].Height / 2;
        //                double dErrorRatio = Math.Max(dSpec * 2, 0.2);

        //                polygon = FindCircleBoundary(src, roiW, roiH, cxLocal, cyLocal,
        //                                             (int)(dRadius1 * (1 - dErrorRatio)),
        //                                             (int)(dRadius1 * (1 + dErrorRatio)),
        //                                             1, 2, bIsDarkCircleSearch);
        //                points = polygon;

        //                double dRadius2 = 0;
        //                Circle finalCircleLocal = FindCircleFitter(tempCircleList, points, out dRadius2, 2, radius, dSpec);

        //                if (Math.Abs((dRadius1 - dRadius2) / Math.Max(1e-6, dRadius2)) < 0.05)
        //                {
        //                    double dScore = IsRealCircle(finalCircleLocal, dRadius2, points, dSpec);
        //                    finalCircleLocal.Score = (float)dScore;

        //                    if (dScore >= scoreThreshold)
        //                    {
        //                        // 3) ROI 로컬 → 원본 좌표계 변환
        //                        float gx = finalCircleLocal.CenterX + (useRoiBuffer ? roiX : 0);
        //                        float gy = finalCircleLocal.CenterY + (useRoiBuffer ? roiY : 0);
        //                        float rr = (float)dRadius2;

        //                        // 4) 최종 ROI 포함성 검증 (바운더리 전체가 ROI 내부인지)
        //                        if (gx - rr < roiX || gx + rr > roiX + roiW || gy - rr < roiY || gy + rr > roiY + roiH)
        //                        {
        //                            continue; // ROI 밖으로 나가면 폐기
        //                        }

        //                        // 좌표 업데이트 후 결과 반영
        //                        finalCircleLocal.CenterX = gx;
        //                        finalCircleLocal.CenterY = gy;

        //                        circlesResult.Add(new RectangleF(gx - rr, gy - rr, rr * 2, rr * 2));
        //                        result.Circles.Add(finalCircleLocal);
        //                        result.ScoreCollection.Add(dScore);
        //                        foundCount++;

        //                        if (foundCount >= maxCircleCount)
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // 5) 중복 제거 + 점수 기준 필터링 (그대로 유지)
        //    double minCenterDistance = radius * 0.75;
        //    List<Circle> filteredCircles = new List<Circle>();
        //    List<double> filteredScores = new List<double>();

        //    for (int i = 0; i < result.Circles.Count; i++)
        //    {
        //        var circleA = result.Circles[i];
        //        var scoreA = result.ScoreCollection[i];
        //        PointF centerA = new PointF(circleA.CenterX, circleA.CenterY);

        //        bool isOverlapping = false;
        //        foreach (var circleB in filteredCircles)
        //        {
        //            PointF centerB = new PointF(circleB.CenterX, circleB.CenterY);
        //            double dist = Math.Sqrt(Math.Pow(centerA.X - centerB.X, 2) + Math.Pow(centerA.Y - centerB.Y, 2));
        //            if (dist < minCenterDistance)
        //            {
        //                isOverlapping = true;
        //                break;
        //            }
        //        }

        //        // ROI 포함성 재확인(안전장치)
        //        float r = result.Circles[i].Radius;
        //        if (!isOverlapping && scoreA >= scoreThreshold &&
        //            circleA.CenterX - r >= roiX && circleA.CenterX + r <= roiX + roiW &&
        //            circleA.CenterY - r >= roiY && circleA.CenterY + r <= roiY + roiH)
        //        {
        //            filteredCircles.Add(circleA);
        //            filteredScores.Add(scoreA);
        //        }
        //    }

        //    result.Circles = filteredCircles;
        //    result.ScoreCollection = filteredScores;

        //    // 🔹 결과 Rect 목록 반영
        //    circlesResult.Clear();
        //    foreach (var c in filteredCircles)
        //    {
        //        float r = (float)c.Radius;
        //        circlesResult.Add(new RectangleF(c.CenterX - r, c.CenterY - r, r * 2, r * 2));
        //    }

        //    return result;
        //}


        public QMC_ImageProcessFindAlignResult FindCirclesWidthCircleBoundaryMultipleCircles(
                                                List<RectangleF> circlesResult,
                                                byte[] pixelData, int w, int h, int radius, double dSpec,
                                                int maxCircleCount = 20,
                                                bool bIsDarkCircleSearch = true,
                                                double scoreThreshold = 0.7,
                                                bool bSpiralSearch = true,
                                                Rectangle? roiRect = null)
        {
            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();

            // [1] 날짜 기반 폴더 생성
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string baseDir = Path.Combine("d:\\TempGoldpowder", dateFolder);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            // [2] 초기 원본 이미지 저장
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");  // ex: 20250612_154512_123
            string rawImagePath = Path.Combine(baseDir, $"Goldpowder_Raw_{timestamp}.bmp");
            IsImageSave = true;
            SaveImage(pixelData, w, h, rawImagePath);

            circlesResult.Clear();
            List<PointF> polygon = new List<PointF>();
            List<PointF> points = new List<PointF>();
            int step = (int)(radius * 0.8);
            int foundCount = 0;

            // 실패 진단용 추적값
            double bestCandidateRadius = 0.0;
            double bestCandidateScore = -1.0;
            int maxEdgePointCount = 0;

            // 1) ROI 정규화 및 픽셀 버퍼 추출
            Rectangle roiIn = roiRect ?? new Rectangle(0, 0, w, h);
            int roiX = Math.Max(0, roiIn.Left);
            int roiY = Math.Max(0, roiIn.Top);
            int roiRight = Math.Min(w, roiIn.Right);
            int roiBottom = Math.Min(h, roiIn.Bottom);
            int roiW = Math.Max(0, roiRight - roiX);
            int roiH = Math.Max(0, roiBottom - roiY);

            bool useRoiBuffer = !(roiX == 0 && roiY == 0 && roiW == 0 && roiH == 0);
            byte[] src = useRoiBuffer ? ExtractROI(pixelData, w, h, roiX, roiY, roiW, roiH) : pixelData;

            if (useRoiBuffer == false)
            {
                if (roiW == 0)
                    roiW = w;
                if (roiH == 0)
                    roiH = h;
            }

            // 2) ROI 로컬 좌표계에서 스캔 (ROI 외부로는 절대 접근하지 않음)
            int xStartLocal = Math.Max(radius, 0);
            int yStartLocal = Math.Max(radius, 0);
            int xEndLocal = Math.Max(0, roiW - radius);
            int yEndLocal = Math.Max(0, roiH - radius);

            for (int y = yStartLocal; y < yEndLocal; y += step)
            {
                for (int x = xStartLocal; x < xEndLocal; x += step)
                {
                    if (foundCount >= maxCircleCount)
                        break;

                    int nMaxCircle = (int)(radius * (1 + dSpec));
                    int nMinCircle = (int)(radius * (1 - dSpec));

                    double dFirstSpec = Math.Min(dSpec * 3, 0.5);
                    int nMaxCircleFirst = Math.Min((int)(radius * 2), 2000);
                    int nMinCircleFirst = (int)(radius * (1 - dFirstSpec));

                    // ROI 버퍼만 사용
                    polygon = FindCircleBoundary(src, roiW, roiH, x, y,
                                                 (int)(radius / 2), nMaxCircleFirst,
                                                 2, 10, bIsDarkCircleSearch);
                    points = polygon;
                    if (points != null) maxEdgePointCount = Math.Max(maxEdgePointCount, points.Count);

                    double dRadius1 = 0;
                    List<RectangleF> tempCircleList = new List<RectangleF>();
                    FindCircleFitter(tempCircleList, points, out dRadius1, 5, radius, dSpec);

                    if (dRadius1 > nMinCircle && dRadius1 < nMaxCircle)
                    {
                        float cxLocal = tempCircleList[0].X + tempCircleList[0].Width / 2;
                        float cyLocal = tempCircleList[0].Y + tempCircleList[0].Height / 2;
                        double dErrorRatio = Math.Max(dSpec * 2, 0.2);

                        polygon = FindCircleBoundary(src, roiW, roiH, cxLocal, cyLocal,
                                                     (int)(dRadius1 * (1 - dErrorRatio)),
                                                     (int)(dRadius1 * (1 + dErrorRatio)),
                                                     1, 2, bIsDarkCircleSearch);
                        points = polygon;
                        if (points != null) maxEdgePointCount = Math.Max(maxEdgePointCount, points.Count);

                        double dRadius2 = 0;
                        Circle finalCircleLocal = FindCircleFitter(tempCircleList, points, out dRadius2, 2, radius, dSpec);

                        if (Math.Abs((dRadius1 - dRadius2) / Math.Max(1e-6, dRadius2)) < 0.05)
                        {
                            double dScore = IsRealCircle(finalCircleLocal, dRadius2, points, dSpec);
                            finalCircleLocal.Score = (float)dScore;

                            // 최고 후보 갱신 (성공여부와 무관하게 항상 추적)
                            if (dRadius2 > 0 && dScore > bestCandidateScore)
                            {
                                bestCandidateScore = dScore;
                                bestCandidateRadius = dRadius2;
                            }

                            if (dScore >= scoreThreshold)
                            {
                                // 3) ROI 로컬 → 원본 좌표계 변환
                                float gx = finalCircleLocal.CenterX + (useRoiBuffer ? roiX : 0);
                                float gy = finalCircleLocal.CenterY + (useRoiBuffer ? roiY : 0);
                                float rr = (float)dRadius2;

                                // 4) 최종 ROI 포함성 검증 (바운더리 전체가 ROI 내부인지)
                                if (gx - rr < roiX || gx + rr > roiX + roiW || gy - rr < roiY || gy + rr > roiY + roiH)
                                {
                                    continue; // ROI 밖으로 나가면 폐기
                                }

                                // 좌표 업데이트 후 결과 반영
                                finalCircleLocal.CenterX = gx;
                                finalCircleLocal.CenterY = gy;

                                circlesResult.Add(new RectangleF(gx - rr, gy - rr, rr * 2, rr * 2));
                                result.Circles.Add(finalCircleLocal);
                                result.ScoreCollection.Add(dScore);
                                foundCount++;

                                if (foundCount >= maxCircleCount)
                                    break;
                            }
                        }
                    }
                }
            }

            // 5) 중복 제거 + 점수 기준 필터링 (그대로 유지)
            double minCenterDistance = radius * 0.75;
            List<Circle> filteredCircles = new List<Circle>();
            List<double> filteredScores = new List<double>();

            for (int i = 0; i < result.Circles.Count; i++)
            {
                var circleA = result.Circles[i];
                var scoreA = result.ScoreCollection[i];
                PointF centerA = new PointF(circleA.CenterX, circleA.CenterY);

                bool isOverlapping = false;
                foreach (var circleB in filteredCircles)
                {
                    PointF centerB = new PointF(circleB.CenterX, circleB.CenterY);
                    double dist = Math.Sqrt(Math.Pow(centerA.X - centerB.X, 2) + Math.Pow(centerA.Y - centerB.Y, 2));
                    if (dist < minCenterDistance)
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                // ROI 포함성 재확인(안전장치)
                float r = result.Circles[i].Radius;
                if (!isOverlapping && scoreA >= scoreThreshold &&
                    circleA.CenterX - r >= roiX && circleA.CenterX + r <= roiX + roiW &&
                    circleA.CenterY - r >= roiY && circleA.CenterY + r <= roiY + roiH)
                {
                    filteredCircles.Add(circleA);
                    filteredScores.Add(scoreA);
                }
            }

            result.Circles = filteredCircles;
            result.ScoreCollection = filteredScores;

            // 🔹 결과 Rect 목록 반영
            circlesResult.Clear();
            foreach (var c in filteredCircles)
            {
                float r = (float)c.Radius;
                circlesResult.Add(new RectangleF(c.CenterX - r, c.CenterY - r, r * 2, r * 2));
            }

            // 🔹 미검출 시 실패 사유 진단 (FindCirclesWidthCircleBoundary 와 동일한 톤)
            if (result.Circles.Count == 0)
            {
                var stats1 = ComputeBrightnessStats(pixelData);
                result.AvgBrightness = stats1.avg;
                result.Contrast = stats1.std;
                result.ExpectedRadius = radius;
                result.MeasuredRadius = bestCandidateRadius;
                result.Score = bestCandidateScore;
                result.EdgePointCount = maxEdgePointCount;

                if (pixelData == null || pixelData.Length == 0)
                {
                    FillFailure(result, CircleSearchFailReason.ImageNullOrEmpty,
                        "이미지 데이터가 비어 있음",
                        "카메라 Live / 이미지 캡처 상태 및 케이블, 노출 확인.");

                    string ngPath = Path.Combine(baseDir, $"Goldpowder_NG_{DateTime.Now:yyyyMMdd_HHmmss_fff}.bmp");
                    IsImageSave = true;
                    SaveImage(pixelData, w, h, ngPath);
                    return result;
                }

                if (radius <= 0)
                {
                    FillFailure(result, CircleSearchFailReason.InvalidInput,
                        $"입력 반경이 0 이하 (radius={radius})",
                        "레시피 Fiducial Circle Size 설정을 확인.");

                    string ngPath = Path.Combine(baseDir, $"Goldpowder_NG_{DateTime.Now:yyyyMMdd_HHmmss_fff}.bmp");
                    IsImageSave = true;
                    SaveImage(pixelData, w, h, ngPath);
                    return result;
                }

                if (maxEdgePointCount < 50)
                {
                    FillFailure(result, CircleSearchFailReason.EdgePointInsufficient,
                        $"경계 후보 점 부족 (count={maxEdgePointCount})",
                        "Spec 확장 / 조명 또는 노출 증가");
                    result.UserGuide = BuildUserGuide(result, dSpec * 100.0, scoreThreshold * 100.0);

                    string ngPath = Path.Combine(baseDir, $"Goldpowder_NG_{DateTime.Now:yyyyMMdd_HHmmss_fff}.bmp");
                    IsImageSave = true;
                    SaveImage(pixelData, w, h, ngPath);
                    return result;
                }

                // 최종 분류: 사이즈 작음/큼/Score 미달/기타
                if (bestCandidateRadius > 0 && bestCandidateRadius < radius)
                {
                    double diffPx = radius - bestCandidateRadius;
                    double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;
                    FillFailure(result, CircleSearchFailReason.RadiusOutOfTolerance,
                        $"설정한 사이즈보다 작아 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이={diffPx:0.0}px, {diffPct:0.0}%)",
                        "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                }
                else if (bestCandidateRadius > 0 && bestCandidateRadius > radius)
                {
                    double diffPx = bestCandidateRadius - radius;
                    double diffPct = (diffPx / Math.Max(1, radius)) * 100.0;
                    FillFailure(result, CircleSearchFailReason.RadiusOutOfTolerance,
                        $"설정한 사이즈보다 커서 미검출 (요청반경={radius}, 측정반경={bestCandidateRadius:0.0}, 차이=+{diffPx:0.0}px, {diffPct:0.0}%)",
                        "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                }
                else if (bestCandidateScore >= 0 && bestCandidateScore < scoreThreshold)
                {
                    FillFailure(result, CircleSearchFailReason.ScoreTooLow,
                        $"설정한 Score({scoreThreshold:0.00})보다 낮아 미검출 (측정Score={bestCandidateScore:0.00})",
                        "Circle Spec 완화: 20~40% 사이 조정 | Score는 70% 이상 설정 권고");
                }
                else
                {
                    FillFailure(result, CircleSearchFailReason.NotFound,
                        "원 미검출",
                        BuildGenericRecommendation(result, dSpec));
                }

                result.UserGuide = BuildUserGuide(result, dSpec * 100.0, scoreThreshold * 100.0);

                string ngPath2 = Path.Combine(baseDir, $"Goldpowder_NG_{DateTime.Now:yyyyMMdd_HHmmss_fff}.bmp");
                IsImageSave = true;
                SaveImage(pixelData, w, h, ngPath2);
            }

            return result;
        }


        private (double avg, double std) ComputeBrightnessStats(byte[] data, int lengthSampleLimit = 50000)
        {
            if (data == null || data.Length == 0) return (0, 0);
            int step = Math.Max(1, data.Length / Math.Min(lengthSampleLimit, data.Length));
            long sum = 0;
            long count = 0;
            for (int i = 0; i < data.Length; i += step)
            {
                sum += data[i];
                count++;
            }
            double avg = (count > 0) ? (double)sum / count : 0;
            // 표준편차
            double varSum = 0;
            for (int i = 0; i < data.Length; i += step)
            {
                double d = data[i] - avg;
                varSum += d * d;
            }
            double std = (count > 1) ? Math.Sqrt(varSum / (count - 1)) : 0;
            return (avg, std);
        }

        private void FillFailure(QMC_ImageProcessFindAlignResult r,
                                 CircleSearchFailReason reason,
                                 string msg,
                                 string recommend)
        {
            if (r.Success) return;
            r.FailReason = reason;
            r.FailMessage = msg;
            r.Recommendation = recommend;
        }

        private string BuildGenericRecommendation(QMC_ImageProcessFindAlignResult r, double dSpec)
        {
            var sb = new StringBuilder();
            if (r.AvgBrightness < 30)
                sb.AppendLine(" - 영상이 너무 어둡습니다: 조명(RED/IR) 증가 또는 Exposure 상승.");
            else if (r.AvgBrightness > 200)
                sb.AppendLine(" - 영상이 과다노출: 조명 또는 노출 감소.");

            if (r.Contrast < 10)
                sb.AppendLine(" - 콘트라스트 낮음: 조명 방향/밝기 조절, Gain 조정.");
            if (dSpec < 0.05)
                sb.AppendLine(" - Spec이 너무 타이트: Spec(dSpec) 값 증가 시도 (예: 5 -> 8).");
            sb.AppendLine(" - Circle Size가 맞는지 확인.");
            sb.AppendLine(" - Score 기준을 낮춰 재시도 (예: 80 → 60).");
            return sb.ToString();
        }

        private string BuildUserGuide(QMC_ImageProcessFindAlignResult r, double uiSpecPercent, double uiScorePercent)
        {
            var sb = new StringBuilder();
            //switch (r.FailReason)
            //{
            //    case CircleSearchFailReason.EdgePointInsufficient:
            //        sb.AppendLine($"경계 포인트 부족 (수집 {r.EdgePointCount}개).");
            //        sb.AppendLine("1) Circle Spec(%)를 높여 범위를 넓혀보세요.");
            //        sb.AppendLine("2) Circle Size 를 ±10% 조정 후 재시도.");
            //        sb.AppendLine("3) Black/White 전환 또는 Ignore(FR4).");
            //        sb.AppendLine("4) 조명(RED/IR)/Exposure ↑ 로 대비 확보.");
            //        break;
            //    case CircleSearchFailReason.NotFound:
            //        sb.AppendLine("원 미검출.");
            //        sb.AppendLine("1) Circle Size 재확인 (실제 반경과 오차?).");
            //        sb.AppendLine("2) Circle Spec(%) 확대.");
            //        sb.AppendLine("3) Mark Color 변경 또는 Ignore 사용.");
            //        sb.AppendLine("4) 조명/노출 재조정.");
            //        break;
            //    case CircleSearchFailReason.RadiusOutOfTolerance:
            //        sb.AppendLine($"반경 편차 초과 (요청 {r.ExpectedRadius}, 측정 {r.MeasuredRadius:0.0}).");
            //        sb.AppendLine("1) Circle Size를 측정값에 맞게 수정.");
            //        sb.AppendLine("2) Spec(%) 소폭 증가.");
            //        sb.AppendLine("3) 필요 시 Score 기준 약간 낮춤.");
            //        break;
            //    case CircleSearchFailReason.ScoreTooLow:
            //        sb.AppendLine($"Score 부족 (기준 {uiScorePercent:0.0}%, 측정 {r.Score * 100:0.0}%).");
            //        sb.AppendLine("1) Spec(%) 증가 (정밀도 vs 안정성).");
            //        sb.AppendLine("2) Score(%) 임계값 5~10p 낮춤.");
            //        sb.AppendLine("3) Circle Size ±5% 조정.");
            //        sb.AppendLine("4) 조명/포커스 개선.");
            //        break;
            //    case CircleSearchFailReason.ImageNullOrEmpty:
            //        sb.AppendLine("이미지 없음/잘못됨. 카메라 Live/케이블/노출 확인.");
            //        break;
            //    case CircleSearchFailReason.InvalidInput:
            //        sb.AppendLine("입력 파라미터 오류 (반경/Spec). UI 값 확인.");
            //        break;
            //    default:
            //        sb.AppendLine("세부 원인 분석 불가. Spec/Size/Score/Color 순서로 변경 후 재시도.");
            //        break;
            //}
            return sb.ToString();
        }

    }
}


