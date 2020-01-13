using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using RecipeAPI.Services;
using System.Collections.Generic;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeData _recipe;
        public RecipeController(IRecipeData recipe)
        {
            _recipe = recipe;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Recipe([FromBody]RecipeView r)
        {
            if (ModelState.IsValid)
            {
                //automapper
                var recipe = new Recipe
                {
                    Name = r.Name,
                    Description = r.Description,
                    Tags = r.Tags
                };

                if (_recipe.AddRecipe(recipe))
                {
                    var uri = Url.Link("RecipeById", new { recipe.Id });
                    return Created(uri, recipe);
                }
                else
                {
                    return BadRequest("bad recipe add");
                }

            }
            else //model state is invalid
            {
                return BadRequest("this stinks bad model");
            }
        }

        [HttpGet]
        public IActionResult Recipe([FromQuery]string tag)
        {
            IEnumerable<Recipe> recipes;
            if (!string.IsNullOrEmpty(tag))
            {
                recipes = _recipe.GetRecipeByTag(tag);
            }
            else
            {
                recipes = _recipe.GetAllRecipes();
            }

            return Ok(recipes);
        }

        [HttpGet]
        [Route("{id:int}", Name = "RecipeByID")]
        public IActionResult RecipeById(int id)
        {
            var recipe = _recipe.GetRecipeById(id);
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteRecipe(int id)
        {
            _recipe.DeleteRecipe(id);
            return NoContent();
        }

        [Authorize]
        [HttpPatch]
        //[Route("{id:int}")]
        public IActionResult UpdateRecipe(Recipe r)
        {
            if (ModelState.IsValid)
            {
                if (_recipe.UpdateRecipe(r))
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}