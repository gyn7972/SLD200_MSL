using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLD200.NewStyleForm.NewSubForm
{
    public class ScannerFocusCalibrationBuilder
    {
        public class BuildResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public Calibration3DReturnCode ReturnCode { get; set; }
            public List<FocusPointAggregate> Aggregates { get; set; } = new List<FocusPointAggregate>();
        }

        public BuildResult Build(
            List<ZPlaneMeasureFile> planeFiles,
            string inputCtFileName,
            string outputCtFileName,
            float kFactor,
            string inputCtReadMeFileName = null)
        {
            BuildResult result = new BuildResult();

            try
            {
                ValidateInput(planeFiles, inputCtFileName, outputCtFileName, kFactor);

                ValidatePlaneCompatibility(planeFiles);

                List<FocusPointAggregate> aggregates = AggregateBestFocusPoints(planeFiles);
                if (aggregates.Count == 0)
                    throw new Exception("FocusCalibration용 집계 데이터가 없습니다.");

                Tuple<float, float, float>[] focusData = aggregates
                    .OrderBy(p => p.IndexY)
                    .ThenBy(p => p.IndexX)
                    .Select(p => Tuple.Create(p.X, p.Y, p.BestZ))
                    .ToArray();

                Calibration3DReturnCode returnCode;
                bool ok = Correction3DRtc.FocusCalibration(
                    focusData,
                    inputCtFileName,
                    inputCtReadMeFileName,
                    kFactor,
                    outputCtFileName,
                    out returnCode);

                result.Success = ok;
                result.ReturnCode = returnCode;
                result.Aggregates = aggregates;

                if (ok)
                {
                    result.Message = $"FocusCalibration 성공\r\n" +
                                     $"Input CT5 : {inputCtFileName}\r\n" +
                                     $"Output CT5 : {outputCtFileName}\r\n" +
                                     $"Point Count : {focusData.Length}";
                }
                else
                {
                    result.Message = $"FocusCalibration 실패\r\n" +
                                     $"ReturnCode : {returnCode}";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        private void ValidateInput(
            List<ZPlaneMeasureFile> planeFiles,
            string inputCtFileName,
            string outputCtFileName,
            float kFactor)
        {
            if (planeFiles == null || planeFiles.Count < 2)
                throw new Exception("최소 2개 이상의 Z plane 파일이 필요합니다.");

            if (string.IsNullOrWhiteSpace(inputCtFileName))
                throw new Exception("입력 CT5 파일이 비어 있습니다.");

            if (!File.Exists(inputCtFileName))
                throw new Exception($"입력 CT5 파일이 없습니다: {inputCtFileName}");

            if (string.IsNullOrWhiteSpace(outputCtFileName))
                throw new Exception("출력 CT5 파일 경로가 비어 있습니다.");

            if (kFactor <= 0)
                throw new Exception("KFactor 값이 0 이하입니다.");
        }

        private void ValidatePlaneCompatibility(List<ZPlaneMeasureFile> planeFiles)
        {
            ZPlaneMeasureFile first = planeFiles[0];

            if (first.Points == null || first.Points.Count == 0)
                throw new Exception("기준 plane의 포인트가 비어 있습니다.");

            foreach (ZPlaneMeasureFile plane in planeFiles)
            {
                if (plane == null)
                    throw new Exception("plane 데이터가 null 입니다.");

                if (plane.Points == null || plane.Points.Count == 0)
                    throw new Exception($"Z={plane.Z:0.000} plane 포인트가 비어 있습니다.");

                if (plane.Rows != first.Rows)
                    throw new Exception($"Rows 불일치: 기준={first.Rows}, 현재={plane.Rows}, Z={plane.Z:0.000}");

                if (plane.Cols != first.Cols)
                    throw new Exception($"Cols 불일치: 기준={first.Cols}, 현재={plane.Cols}, Z={plane.Z:0.000}");

                if (Math.Abs(plane.PitchX - first.PitchX) > 0.000001f)
                    throw new Exception($"PitchX 불일치: 기준={first.PitchX}, 현재={plane.PitchX}, Z={plane.Z:0.000}");

                if (Math.Abs(plane.PitchY - first.PitchY) > 0.000001f)
                    throw new Exception($"PitchY 불일치: 기준={first.PitchY}, 현재={plane.PitchY}, Z={plane.Z:0.000}");

                if (plane.Points.Count != first.Points.Count)
                    throw new Exception($"PointCount 불일치: 기준={first.Points.Count}, 현재={plane.Points.Count}, Z={plane.Z:0.000}");
            }

            Dictionary<string, GridMeasurePoint> refMap = first.Points
                .ToDictionary(p => GetKey(p.IndexX, p.IndexY), p => p);

            foreach (ZPlaneMeasureFile plane in planeFiles.Skip(1))
            {
                foreach (GridMeasurePoint pt in plane.Points)
                {
                    string key = GetKey(pt.IndexX, pt.IndexY);

                    if (!refMap.TryGetValue(key, out GridMeasurePoint refPt))
                        throw new Exception($"Index mismatch: ({pt.IndexX},{pt.IndexY}), Z={plane.Z:0.000}");

                    if (Math.Abs(refPt.RefX - pt.RefX) > 0.000001f ||
                        Math.Abs(refPt.RefY - pt.RefY) > 0.000001f)
                    {
                        throw new Exception(
                            $"RefXY mismatch: Index=({pt.IndexX},{pt.IndexY}), " +
                            $"기준=({refPt.RefX:0.000},{refPt.RefY:0.000}), " +
                            $"현재=({pt.RefX:0.000},{pt.RefY:0.000}), Z={plane.Z:0.000}");
                    }
                }
            }
        }

        private List<FocusPointAggregate> AggregateBestFocusPoints(List<ZPlaneMeasureFile> planeFiles)
        {
            Dictionary<string, FocusPointAggregate> map = new Dictionary<string, FocusPointAggregate>();

            foreach (ZPlaneMeasureFile plane in planeFiles)
            {
                foreach (GridMeasurePoint pt in plane.Points)
                {
                    string key = GetKey(pt.IndexX, pt.IndexY);

                    if (!map.TryGetValue(key, out FocusPointAggregate agg))
                    {
                        agg = new FocusPointAggregate
                        {
                            IndexX = pt.IndexX,
                            IndexY = pt.IndexY,
                            X = pt.RefX,
                            Y = pt.RefY,
                            BestZ = plane.Z,
                            BestScore = pt.Score
                        };
                        map[key] = agg;
                    }
                    else
                    {
                        // Score = XY 거리 오차, 작을수록 좋음
                        if (pt.Score < agg.BestScore)
                        {
                            agg.BestScore = pt.Score;
                            agg.BestZ = plane.Z;
                        }
                        else if (Math.Abs(pt.Score - agg.BestScore) < 0.000001f)
                        {
                            // 동점이면 Z=0에 더 가까운 값 우선
                            if (Math.Abs(plane.Z) < Math.Abs(agg.BestZ))
                            {
                                agg.BestScore = pt.Score;
                                agg.BestZ = plane.Z;
                            }
                        }
                    }
                }
            }

            return map.Values
                .OrderBy(p => p.IndexY)
                .ThenBy(p => p.IndexX)
                .ToList();
        }

        private string GetKey(int ix, int iy)
        {
            return $"{ix}_{iy}";
        }
    }
}