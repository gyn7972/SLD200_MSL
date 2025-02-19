using QMC.Common;
using QMC.Common.Parts;
using QMC.Process.WorkStage.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Process.WorkStage.Modules
{
    [Serializable]
    public class WorkStageProcessorRecipe
    {
        [NonSerialized]
        protected WorkStageProcessor m_DrillingProcessor;

        //public TwoPointAlignerRecipe TwoPointAlignerRecipe { set; get; }

        public ScannerParameterRecipe ScannerRecipe { set; get; }

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

        public WorkStageProcessorRecipe(Module module)
        {
            m_DrillingProcessor = module as WorkStageProcessor;
            Init(m_DrillingProcessor);
        }

        public void Init(WorkStageProcessor drillingProcessor)
        {
            if (drillingProcessor == null)
                return;

            m_DrillingProcessor = drillingProcessor;

            /*if (TwoPointAlignerRecipe == null)
                TwoPointAlignerRecipe = new TwoPointAlignerRecipe(drillingProcessor.TwoPointAligner);*/

            if (ScannerRecipe == null)
                ScannerRecipe = new ScannerParameterRecipe();

        }
    }
}
