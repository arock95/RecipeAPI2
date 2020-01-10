using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public class RecipeData : IRecipeData
    {
        private readonly RecipeDbContext _recipe;
        public RecipeData(RecipeDbContext recipe)
        {
            _recipe = recipe;
        }

        public bool AddRecipe(Recipe recipe)
        {
            try
            {
                _recipe.Recipes.Add(recipe);
                foreach (Tag t in recipe.Tags)
                {
                    if (_recipe.Tags.FirstOrDefault(tag => tag.Name == t.Name)==null){ _recipe.Tags.Add(new Tag { Name = t.Name }); }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _recipe.Recipes.ToArray();
        }

        public Recipe GetRecipeById(int id)
        {
            return _recipe.Recipes.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Recipe> GetRecipeByTag(string tag)
        {
            //return _recipe.Recipes.Where(r => r.Tags.Contains(tag));
            throw new NotImplementedException();
        }
    }
}
