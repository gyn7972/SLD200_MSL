using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;

namespace QMC.Common.Parts
{
    [Serializable]
    public class ScannerCompensatorRecipe
    {
        #region Field
        private ScannerCompensator m_Owner;
        #endregion

        #region Property
        public PatternMatchingParameters PatternMatchingParameter { get; set; }

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

        public BlobVisionToolParameter BlobParameter { get; set; }
        #endregion

        #region Constructor
        public ScannerCompensatorRecipe(Part part)
        {
            if(part != null && part is ScannerCompensator)
            {
                m_Owner = part as ScannerCompensator;
            }

            Init(part);
        }
        #endregion

        #region Method
        public void Init(Part part)
        {
            if (PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();

            if (TrainRoiStartLocation == null)
                TrainRoiStartLocation = new Point();

            if (TrainRoiEndLocation == null)
                TrainRoiEndLocation = new Point();

            if (InspectRoiStartLocation == null)
                InspectRoiStartLocation = new Point();

            if (InspectRoiEndLocation == null)
                InspectRoiEndLocation = new Point();

            if (IlluminationDataSet == null)
                IlluminationDataSet = new IlluminationDataSet(part.Name);

            if (BlobParameter == null)
                BlobParameter = new BlobVisionToolParameter();
        }
        #endregion
    }
}
