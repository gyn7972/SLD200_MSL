using System;
using System.Collections.Generic;

namespace SLD200.NewStyleForm.NewSubForm
{
    public class GridMeasurePoint
    {
        public int IndexX { get; set; }
        public int IndexY { get; set; }

        public float RefX { get; set; }
        public float RefY { get; set; }

        public float MeasuredX { get; set; }
        public float MeasuredY { get; set; }

        // 필요 시 점수 저장
        // XY 위치 오차 거리
        public float Score
        {
            get
            {
                float dx = MeasuredX - RefX;
                float dy = MeasuredY - RefY;
                return (float)Math.Sqrt((dx * dx) + (dy * dy));
            }
            set
            {
                // JSON 역직렬화 호환용 빈 setter
            }
        }
    }

    // 높이 1개 파일 (FormNew_Setup에서 저장)
    public class ZPlaneMeasureFile
    {
        public float Z { get; set; }          // 이 파일의 스캐너 Z(mm)
        public int Rows { get; set; }
        public int Cols { get; set; }
        public float PitchX { get; set; }
        public float PitchY { get; set; }

        public List<GridMeasurePoint> Points { get; set; } = new List<GridMeasurePoint>();
    }

    // 여러 높이 파일을 합쳐서 3D 생성 시 내부 사용
    public class FocusPointAggregate
    {
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public float X { get; set; }          // 기준 XY
        public float Y { get; set; }

        public float BestZ { get; set; }      // 최적 Z
        public float BestScore { get; set; }  // 최적 점수
    }
}