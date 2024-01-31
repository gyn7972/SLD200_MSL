using QMC.Common.Modules;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public class DieFinder : PatternMatchingVisionPart
    {
        public DieFinderRecipe Recipe { get; set; }
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

        public DieFinder(string strName) : base(strName)
        {
            Recipe = new DieFinderRecipe(this);
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

            if ((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;


        }
        public override void UpdateRecipeData()
        {
            DieUnloader dieUnloader = Owner as DieUnloader;
            if (dieUnloader != null)
            {
                if (dieUnloader.Recipe.DieFinderRecipe != null)
                {
                    this.Recipe = dieUnloader.Recipe.DieFinderRecipe;
                    if (this.Recipe.IlluminationDataSet != null)
                    {
                        this.Recipe = dieUnloader.Recipe.DieFinderRecipe;
                        IlluminationData = this.Recipe.IlluminationDataSet;
                    }
                    else
                    {
                        dieUnloader.Recipe.DieFinderRecipe = this.Recipe;
                        this.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                    }
                }
                else
                {
                    dieUnloader.Recipe.DieFinderRecipe = this.Recipe;
                }
            }
        }
    }
}
