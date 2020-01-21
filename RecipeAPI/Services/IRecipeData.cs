using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Services
{
    public interface IRecipeData
    {
        bool AddRecipe(Recipe recipe);
        IEnumerable<Recipe> GetAllRecipes();

        Recipe GetRecipeById(int id);

        Tag GetRecipeByTag(string tag);
        bool DeleteRecipe(int id);
        bool UpdateRecipe(RecipeView recipe);
    }
}
