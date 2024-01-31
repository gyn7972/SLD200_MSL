using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class DieSearcher : PatternMatchingVisionPart
    {

        public DieSearcherRecipe Recipe { get; set; }
        public Turret Turret { set; get; }
        public IlluminationDataSet IlluminationData
        {
            set
            {
                Recipe.IlluminationDataSet = value;
            }
            get
            {
                return Recipe.IlluminationDataSet;
            }
        }
        public VisionScale Scale
        {
            get
            {
                DieTransfer dieTransfer = Owner as DieTransfer;
                if( dieTransfer != null )
                    return dieTransfer.Scale;
                else
                    return null;
            }
        }
        public DieSearcher(string strName) : base(strName)
        {
            Recipe = new DieSearcherRecipe(this);
        }

        public override int Create()
        {
            int ret = base.Create();
            
            
            return ret;
        }
        public override void Close()
        {
            base.Close();
        }

        public int Train()
        {
            int ret = 0;

            if ((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return ret;
            }

            if (Recipe != null)
            {
                if (Recipe.PatternMatchingParameter == null)
                {
                    PatternMatchingParameters newParameter = new PatternMatchingParameters();
                    Recipe.PatternMatchingParameter = newParameter;
                }
                Recipe.PatternMatchingParameter.TrainImage = TrainImage;
                Owner.SaveRecipeData();
            }
            return ret;
        }

        public PatternMatchingResult Search()
        {
            int ret = 0;
            
            if((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;
            

        }
        public override void UpdateRecipeData()
        {
            DieTransfer dieTransfer = Owner as DieTransfer;
            if (dieTransfer != null)
            {
                if (dieTransfer.Recipe.DieSearcherRecipe != null)
                {
                    this.Recipe = dieTransfer.Recipe.DieSearcherRecipe;
                    if (this.Recipe.IlluminationDataSet != null)
                    {
                        this.Recipe = dieTransfer.Recipe.DieSearcherRecipe;
                        IlluminationData = this.Recipe.IlluminationDataSet;
                    }
                    else
                    {
                        dieTransfer.Recipe.DieSearcherRecipe = this.Recipe;
                        this.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                    }
                }
                else
                {
                    dieTransfer.Recipe.DieSearcherRecipe = this.Recipe;
                }
            }
        }

        public override int OnWork()
        {
            int ret = 0;



            return ret;
        }
    }
}
