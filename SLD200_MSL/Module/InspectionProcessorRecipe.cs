using QMC.Common;
using QMC.Common.Keithley.SourceMeter;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Vision
{
    [Serializable]
    public class InspectionProcessorRecipe
    {
        #region Field
        [NonSerialized]
        private InspectionProcessor m_Owner;
        public AlignerRecipe AlignerRecipe { set; get; }
        public VisionCompensatorRecipe VisionCompensatorRecipe { set; get; }

        public VisionCalibratorRecipe VisionCalibratorRecipe { set; get; }

        public StageThetaAgingTesterRecipe StageThetaAgingTesterRecipe { set; get; }
        #endregion

        #region Constructor
        public InspectionProcessorRecipe(Module module)
        {
            if (module is InspectionProcessor)
            {
                m_Owner = module as InspectionProcessor;
            }
            Init(m_Owner);
        }
        #endregion

        #region Property
        //public VisionCompensatorRecipe VisionCompensatorRecipe;
        //public StageRecipe StageRecipe;
        #endregion

        #region Method
        public void Init(InspectionProcessor owner)
        {
            m_Owner = owner;
            if (AlignerRecipe == null)
                AlignerRecipe = new AlignerRecipe(owner.Aligner);

            if (VisionCompensatorRecipe == null)
                VisionCompensatorRecipe = new VisionCompensatorRecipe(owner.VisionCompensator.Name);

            AlignerRecipe.Init(owner.Aligner);
            VisionCompensatorRecipe.Init(owner.VisionCompensator.Name);

            if(VisionCalibratorRecipe == null)
            {
                VisionCalibratorRecipe = new VisionCalibratorRecipe(owner.VisionCalibrator);
            }

            if(StageThetaAgingTesterRecipe == null)
            {
                StageThetaAgingTesterRecipe = new StageThetaAgingTesterRecipe(owner.StageThetaAgingTester);
            }

        }
        #endregion
    }
}
