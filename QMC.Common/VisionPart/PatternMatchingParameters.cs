using QMC.Common.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class PatternMatchingParameters
    {
        public VisionImage TrainImage { get; set; }
        public double MinTolerance { get; set; }
        public double MaxTolerance { get; set; }
        public bool DuplicateChecked { get; set; }
        public int MaxInstance { get; set; }
        public double MinScore { get; set; }

        public RectangleD MaskRegion { get; set; }
        public bool UseMaskImage { get; set; }

        public PatternMatchingParameters()
        {
            TrainImage = new VisionImage();
            MinTolerance = 0;
            MaxTolerance = 0;
            DuplicateChecked = false;
            MaxInstance = 1;
            MinScore = 0.5;
            MaskRegion = new RectangleD();
            UseMaskImage = false;
        }
    }
}
