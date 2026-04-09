using System.Collections.Generic;

namespace SLD200.NewStyleForm.NewSubForm
{
    public class GridMeasurePoint
    {
        public int IndexX { get; set; }
        public int IndexY { get; set; }

        // 기준 좌표 (장비 명령/참조)
        public float RefX { get; set; }
        public float RefY { get; set; }

        // 측정 좌표 (비전 결과)
        public float MeasuredX { get; set; }
        public float MeasuredY { get; set; }
    }

    public class ZPlaneMeasureFile
    {
        public float Z { get; set; }

        public int Rows { get; set; }
        public int Cols { get; set; }

        public float PitchX { get; set; } // col interval
        public float PitchY { get; set; } // row interval

        public List<GridMeasurePoint> Points { get; set; } = new List<GridMeasurePoint>();
    }
}