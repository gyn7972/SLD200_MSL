using QMC.Common.Hmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class RealTimeScannerRecipe
    {
        [Serializable]
        public enum Direction
        {
            ToTop,            
            ToBottom,            
            ToLeft,            
            ToRight,
        }

        public Direction PriorityDirection { get; set; }
        public Direction SecondarySearchDirection { get; set; }
        public SizeD WorkingArea { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        [ReadOnly(true)]
        public PatternMatchingParameters PatternMatchingParameter { get; set; }
        [Browsable(false)]
        public IlluminationDataSet IlluminationDataSet { set; get; }
        public IlluminationDataList IlluminationDataSets { set; get; }
        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        public RealTimeScannerRecipe(Part part)
        {
            PriorityDirection = Direction.ToBottom;
            SecondarySearchDirection = Direction.ToRight;
            WorkingArea = new SizeD();
            PatternMatchingParameter = new PatternMatchingParameters();
            IlluminationDataSet = new IlluminationDataSet(part.Name);
            IlluminationDataSets = new IlluminationDataList();

            InspectRoiStartLocation = new Point(0, 0);
            InspectRoiEndLocation = new Point(100, 100);
            TrainRoiStartLocation = new Point(0, 0);
            TrainRoiEndLocation = new Point(100, 100);
        }

    }
}
