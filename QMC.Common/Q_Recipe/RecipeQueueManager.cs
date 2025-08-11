using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Q_Recipe
{
    public class RecipeQueueManager
    {
        private Queue<string> recipeQueue = new Queue<string>();

        public void AddRecipe(string recipePath)
        {
            if (!string.IsNullOrEmpty(recipePath))
                recipeQueue.Enqueue(recipePath);
        }

        public string GetNextRecipe()
        {
            return recipeQueue.Count > 0 ? recipeQueue.Dequeue() : null;
        }

        public bool HasNextRecipe()
        {
            return recipeQueue.Count > 0;
        }

        public void Clear()
        {
            recipeQueue.Clear();
        }

        public int Count => recipeQueue.Count;
    }
}
