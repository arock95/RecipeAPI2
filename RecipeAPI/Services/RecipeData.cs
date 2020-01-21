using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _recipe.Recipes
                .Include(rt => rt.RecipeTags).ThenInclude(r => r.Recipe)
                .Include(rt => rt.RecipeTags).ThenInclude(t => t.Tag);
        }

        public Recipe GetRecipeById(int id)
        {
            return _recipe.Recipes
                .Include(rt => rt.RecipeTags).ThenInclude(r => r.Recipe)
                .Include(rt => rt.RecipeTags).ThenInclude(t => t.Tag)
                .FirstOrDefault(r => r.Id == id);

        }

        public Tag GetRecipeByTag(string tag)
        {
            //return _recipe.Recipes.Include(rt => rt.RecipeTags).ThenInclude(r => r.Recipe).ToArray();
            return _recipe.Tags.Include(rt => rt.RecipeTags)
                .ThenInclude(r => r.Recipe)
                .FirstOrDefault(t => t.Name.Contains(tag));
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
                //update recipes at some point
                _recipe.Recipes.Update(updatedRecipe);
                SaveChanges();
                return true;
            }
            else { return false; }

        }
    }
}
