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
        private readonly ITagData _tags;
        public RecipeData(RecipeDbContext recipe, ITagData tags)
        {
            _recipe = recipe;
            _tags = tags;
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
            return _recipe.Tags
                .Include(rt => rt.RecipeTags).ThenInclude(r => r.Recipe)
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

        public bool UpdateRecipe(RecipeView recipe)
        {
            var updatedRecipe = GetRecipeById(recipe.Id);
            if (updatedRecipe != null)
            {
                updatedRecipe.Name = recipe.Name;
                updatedRecipe.Description = recipe.Description;
                updatedRecipe.RecipeTags.RemoveAll(r => r.RecipeId == recipe.Id);
                SaveChanges();
                //update tags at some point
                foreach (string t in recipe.Tags)
                {
                    var result = _tags.FindTag(t);
                    if (result == null)
                    {
                        result = _tags.AddTag(t);
                    }
                    //add the 'join' between recipe and tag
                    _recipe.Add(new RecipeTag { RecipeId = recipe.Id, TagId = result.Id });
                }
                _recipe.Recipes.Update(updatedRecipe);
                SaveChanges();
                return true;
            }
            else { return false; }

        }
    }
}
