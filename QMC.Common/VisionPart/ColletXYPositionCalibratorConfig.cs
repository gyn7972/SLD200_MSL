using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class ColletXYPositionCalibratorConfig
    {
        public double AutoOffsetX { get; set; }
        public double AutoOffsetY { get; set; }
        public double UserOffsetX { get; set; }
        public int UserOffsetY { get; set; }
        public double BeforeDelay { get; set; }
        public double ColletSize { get; set; }
        public double Velocity { get; set; }
        public XyztCoordinate StartPosition { get; set; }

        public XyCoordinate CalibrationValue { get; set; }
    }
}
