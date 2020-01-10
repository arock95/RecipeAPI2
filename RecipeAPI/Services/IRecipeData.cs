using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public interface IRecipeData
    {
        bool AddRecipe(Recipe recipe);
        IEnumerable<Recipe> GetAllRecipes();

        Recipe GetRecipeById(int id);

        IEnumerable<Recipe> GetRecipeByTag(string tag);
    }
}
