using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class RecipeParameters
    {

        public string Module { get; set; }
        public object RecipeData { get; set; }

        public RecipeParameters() : this("", null)
        {
        }

        public RecipeParameters(string module, object recipeData)
        {
            Module = module;

            RecipeData = recipeData;
        }

        public RecipeParameters DeepCopy()
        {
            RecipeParameters recipe = new RecipeParameters(this.Module, this.RecipeData);
            return recipe;
        }
    }
    [Serializable]
    public class RecipeParametersCollection : Collection<RecipeParameters>
    {
    	public RecipeParametersCollection DeepCopy()
        {
            RecipeParametersCollection parameters = new RecipeParametersCollection();

            foreach(RecipeParameters recipe in this)
            {
                parameters.Add(recipe.DeepCopy());
            }

            return parameters;
        }    

        public void SetRecipeParameter(string strModuleName, object recipeData)
        {
            foreach (RecipeParameters recipe in this)
            {
                if(recipe.Module == strModuleName)
                {
                    recipe.RecipeData = recipeData;
                    break;
                }
            }
        }
    }

}
