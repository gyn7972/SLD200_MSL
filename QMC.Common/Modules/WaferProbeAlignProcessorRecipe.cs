using QMC.Common;
using QMC.Process.WaferProbeAlign.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Process.WaferProbeAlign.Modules
{
    [Serializable]
    public class WaferProbeAlignProcessorRecipe
    {
        [NonSerialized]
        protected WaferProbeAlignProcessor m_DrillingProcessor;

        //public TwoPointAlignerRecipe TwoPointAlignerRecipe { set; get; }

        public ScannerRecipe ScannerRecipe { set; get; }

        /*public double UnitPitchX 
        { 
            set
            {
                TwoPointAlignerRecipe.UnitPitchX = value;
            }
            get
            {
                return TwoPointAlignerRecipe.UnitPitchX;
            }
        }
        public double UnitPitchY 
        {
            set
            {
                TwoPointAlignerRecipe.UnitPitchY = value;
            }
            get
            {
                return TwoPointAlignerRecipe.UnitPitchY;
            }
        }*/

        public WaferProbeAlignProcessorRecipe(Module module)
        {
            m_DrillingProcessor = module as WaferProbeAlignProcessor;
            Init(m_DrillingProcessor);
        }

        public void Init(WaferProbeAlignProcessor drillingProcessor)
        {
            if (drillingProcessor == null)
                return;

            m_DrillingProcessor = drillingProcessor;

            /*if (TwoPointAlignerRecipe == null)
                TwoPointAlignerRecipe = new TwoPointAlignerRecipe(drillingProcessor.TwoPointAligner);*/

            if (ScannerRecipe == null)
                ScannerRecipe = new ScannerRecipe();

        }
    }
}
