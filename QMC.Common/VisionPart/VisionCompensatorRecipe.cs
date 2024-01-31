using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Hmi;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class VisionCompensatorRecipe
    {
        public IlluminationDataSet IlluminationDataSet { set; get; }

        [Browsable(false)]
        public PatternMatchingParameters PatternMatchingParameters { set; get; }

        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }

        public VisionCompensatorRecipe(string strName)
        {
            InspectRoiStartLocation = new Point();
            InspectRoiEndLocation = new Point();
            TrainRoiStartLocation = new Point();
            TrainRoiEndLocation = new Point();
            Init(strName);
        }
        public void Init(string strName)
        {
            if (IlluminationDataSet == null)
                IlluminationDataSet = new IlluminationDataSet(strName);

            if (PatternMatchingParameters == null)
            {
                PatternMatchingParameters = new PatternMatchingParameters();
            }
            
        }
    }    
}
