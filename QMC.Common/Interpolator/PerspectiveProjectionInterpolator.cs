using QMC.Common.Vision.VisionAlign;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Interpolator
{
    public class PerspectiveProjectionInterpolator : BaseInterpolator
    {
        class CacheData
        {
            public XyCoordinate LeftTop { get; set; }
            public XyCoordinate RightBottom { get; set; }
            public List<PositionOffset> Around4Point { get; set; } = new List<PositionOffset>();
            public void AddAroundData(List<PositionOffset> around4Point)
            {
                Around4Point = around4Point;
                if (around4Point.Count == 4)
                {
                    LeftTop = around4Point[0].Position;
                    RightBottom = around4Point[2].Position;
                }
            }
            public bool IsHit(XyCoordinate source)
            {
                if (Around4Point.Count == 0)
                {
                    return false;
                }
                double MinX = LeftTop.X;
                double MinY = LeftTop.Y;
                double MaxX = RightBottom.X;
                double MaxY = RightBottom.Y;
                if (source.X >= MinX && source.X <= MaxX && source.Y >= MinY && source.Y <= MaxY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private CacheData cacheAround4Point = new CacheData();
        private CacheData cacheDestAround4Point = new CacheData();

        protected List<PositionOffset> GetAround4Point(XyCoordinate source)
        {
            if (cacheAround4Point.IsHit(source))
            {
                return cacheAround4Point.Around4Point;
            }
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
            cacheAround4Point.AddAroundData(list);
            return list;
        }


        protected List<PositionOffset> GetDestAround4Point(XyCoordinate dest)
        {
            if (cacheDestAround4Point.IsHit(dest))
            {
                return cacheDestAround4Point.Around4Point;
            }
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



        //public override int Interpolate(XyCoordinate source, ref XyCoordinate dest)
        //{
        //    int ret = 0;
        //    List<PositionOffset> list = GetAround4Point(source);
        //    if(list.Count == 4)
        //    {
        //        QMCMatrix CorrectionMatrix = null;
        //        PerspectiveProjection perspectiveProjection = new PerspectiveProjection();
        //        XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
        //        XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();
        //        int nIndex = 0;
        //        foreach (PositionOffset v in list)
        //        {
        //            coordinatesSource.Add(new XyCoordinate(v.Position.X, v.Position.Y));
        //            coordinatesTarget.Add(new XyCoordinate(v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));

        //        }
        //        CorrectionMatrix = perspectiveProjection.GetCorrectionMatrix(coordinatesSource, coordinatesTarget);
        //        XyCoordinate t = perspectiveProjection.GetPerspectiveProjectionPoint(source, CorrectionMatrix);

        //        dest = t;
        //    }
        //    else
        //    {
        //        dest = source;
        //    }


        //    return ret;
        //}

        //public override int ReverseInterpolate(XyCoordinate dest, ref XyCoordinate source)
        //{
        //    int ret = 0;
        //    List<PositionOffset> list = GetDestAround4Point(dest);
        //    if (list.Count == 4)
        //    {
        //        QMCMatrix CorrectionMatrix = null;
        //        PerspectiveProjection perspectiveProjection = new PerspectiveProjection();
        //        XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
        //        XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();
        //        int nIndex = 0;
        //        foreach (PositionOffset v in list)
        //        {
        //            coordinatesSource.Add(new XyCoordinate(v.Position.X, v.Position.Y));
        //            coordinatesTarget.Add(new XyCoordinate(v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));
        //        }
        //        CorrectionMatrix = perspectiveProjection.GetCorrectionMatrix(coordinatesTarget, coordinatesSource);
        //        XyCoordinate t = perspectiveProjection.GetPerspectiveProjectionPoint(dest, CorrectionMatrix);

        //        source = t;
        //    }
        //    else
        //    {
        //        source = dest;
        //    }

        //    return ret;
        //}



        // 스테이지 이상 - 리팩토링
        public override int Interpolate(XyCoordinate source, ref XyCoordinate dest)
        {
            try
            {
                List<PositionOffset> list = GetAround4Point(source);
                if (list == null || list.Count < 4)
                {
                    dest = source;
                    //Log.Write("Interpolate", $"[오류] 주변 보정 포인트 부족 (Count={list?.Count ?? 0}) - Source: X={source.X:F3}, Y={source.Y:F3}");
                    //dest = source;
                    //return -1;
                }
                else
                {
                    QMCMatrix CorrectionMatrix = null;
                    PerspectiveProjection perspectiveProjection = new PerspectiveProjection();
                    XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
                    XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();

                    foreach (PositionOffset v in list)
                    {
                        coordinatesSource.Add(new XyCoordinate(v.Position.X, v.Position.Y));
                        coordinatesTarget.Add(new XyCoordinate(v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));
                    }

                    CorrectionMatrix = perspectiveProjection.GetCorrectionMatrix(coordinatesSource, coordinatesTarget);
                    if (CorrectionMatrix == null)
                    {
                        Log.Write("Interpolate", "[오류] 보정 행렬 생성 실패");
                        //dest = source;
                        //return -2;
                    }

                    XyCoordinate t = perspectiveProjection.GetPerspectiveProjectionPoint(source, CorrectionMatrix);
                    if (double.IsNaN(t.X) || double.IsNaN(t.Y) || double.IsInfinity(t.X) || double.IsInfinity(t.Y))
                    {
                        Log.Write("Interpolate", $"[오류] 보간 결과 비정상 → X={t.X:F3}, Y={t.Y:F3}");
                        //dest = source;
                        //return -3;
                    }

                    dest = t;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Log.Write("Interpolate", $"[예외] {ex.Message}");
                dest = source;
                return -4;
            }
        }

        public override int ReverseInterpolate(XyCoordinate dest, ref XyCoordinate source)
        {
            try
            {
                List<PositionOffset> list = GetDestAround4Point(dest);
                if (list == null || list.Count < 4)
                {
                    source = dest;
                    //if(list.Count != 1)
                    //    Log.Write("ReverseInterpolate", $"[오류] 주변 역보정 포인트 부족 (Count={list?.Count ?? 0}) - Dest: X={dest.X:F3}, Y={dest.Y:F3}");
                    //source = dest;
                    //return -1;
                }
                else
                {
                    QMCMatrix CorrectionMatrix = null;
                    PerspectiveProjection perspectiveProjection = new PerspectiveProjection();
                    XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
                    XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();

                    foreach (PositionOffset v in list)
                    {
                        coordinatesSource.Add(new XyCoordinate(v.Position.X, v.Position.Y));
                        coordinatesTarget.Add(new XyCoordinate(v.Position.X + v.Offset.X, v.Position.Y - v.Offset.Y));
                    }

                    CorrectionMatrix = perspectiveProjection.GetCorrectionMatrix(coordinatesTarget, coordinatesSource);
                    if (CorrectionMatrix == null)
                    {
                        Log.Write("ReverseInterpolate", "[오류] 역보정 행렬 생성 실패");
                        //source = dest;
                        //return -2;
                    }

                    XyCoordinate t = perspectiveProjection.GetPerspectiveProjectionPoint(dest, CorrectionMatrix);
                    if (double.IsNaN(t.X) || double.IsNaN(t.Y) || double.IsInfinity(t.X) || double.IsInfinity(t.Y))
                    {
                        Log.Write("ReverseInterpolate", $"[오류] 역보간 결과 비정상 → X={t.X:F3}, Y={t.Y:F3}");
                        //source = dest;
                        //return -3;
                    }

                    source = t;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Log.Write("ReverseInterpolate", $"[예외] {ex.Message}");
                source = dest;
                return -4;
            }
        }



    }
}
