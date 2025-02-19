using QMC.Common.Vision;
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
    public class VisionCalibratorRecipe
    {

        public PatternMatchingParameters PatternMatchingParameters { get; set; }

        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        [Browsable(false)]
        public IlluminationDataSet IlluminationDataSet { set; get; }
        //public IlluminationDataList IlluminationDataSets { set; get; }

        public VisionCalibratorRecipe(Part part)
        {
            //PatternMatchingParameters = new PatternMatchingParameters();

            //InspectRoiStartLocation = new Point(0, 0);
            //InspectRoiEndLocation = new Point(100, 100);
            //TrainRoiStartLocation = new Point(0, 0);
            //TrainRoiEndLocation = new Point(100, 100);

            ////IlluminationDataSets = new IlluminationDataList();
            //IlluminationDataSet = new IlluminationDataSet(part.Name);

            Init(part);
        }

        #region Method
        public void Init(Part part)
        {
            if (PatternMatchingParameters == null)
                PatternMatchingParameters = new PatternMatchingParameters();

            if (TrainRoiStartLocation == null)
                TrainRoiStartLocation = new Point();

            if (TrainRoiEndLocation == null)
                TrainRoiEndLocation = new Point();

            if (InspectRoiStartLocation == null)
                InspectRoiStartLocation = new Point();

            if (InspectRoiEndLocation == null)
                InspectRoiEndLocation = new Point();

            //if (IlluminationDataSet == null)
            //    IlluminationDataSet = new IlluminationDataSet(part.Name);
        }
        #endregion

    }
}
