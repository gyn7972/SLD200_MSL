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
    public class DieFinderRecipe
    {
        [NonSerialized]
        protected DieFinder m_Owner;
        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        [Browsable(false)]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public IlluminationDataSet IlluminationDataSet { set; get; }
        [Browsable(false)]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
        public DieFinderRecipe(DieFinder dieFinder)
        {
            m_Owner = dieFinder;
            Init(m_Owner.Name);
        }

        public void Init(string strName)
        {
            InspectRoiStartLocation = new Point();
            InspectRoiEndLocation = new Point();
            TrainRoiStartLocation = new Point();
            TrainRoiEndLocation = new Point();
            IlluminationDataSet = new IlluminationDataSet(strName);
            if (PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();
        }
    }
}
