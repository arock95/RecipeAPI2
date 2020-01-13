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
                var tags = recipe.Tags.Split(',');
                foreach (string t in tags)
                {
                    if (_recipe.Tags.FirstOrDefault(tag => tag.Name == t)==null){ _recipe.Tags.Add(new Tag { Name = t }); }
                }
                SaveChanges();
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
            //this needs fixed, will return sub-strings
            var new_tag = ", " + tag + ",";
            return _recipe.Recipes.Where(r => r.Tags.Contains(new_tag));
        }

        public int SaveChanges()
        {
            return _recipe.SaveChanges();
        }

        public bool DeleteRecipe(int id)
        {
            var deletedRecipe = _recipe.Recipes.FirstOrDefault(r => r.Id == id);
            if (deletedRecipe != null)
            {
                _recipe.Recipes.Remove(deletedRecipe);
                SaveChanges();
            }
            return true;
        }

        public bool UpdateRecipe(Recipe recipe)
        {
            var updatedRecipe = _recipe.Recipes.FirstOrDefault(r => r.Id == recipe.Id);
            if (updatedRecipe != null)
            {
                updatedRecipe.Name = recipe.Name;
                updatedRecipe.Description = recipe.Description;
                updatedRecipe.Tags = recipe.Tags;
                _recipe.Recipes.Update(updatedRecipe);
                SaveChanges();
                return true;
            }
            else { return false; }
            
        }
    }
}
