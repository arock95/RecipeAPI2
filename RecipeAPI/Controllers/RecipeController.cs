using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using RecipeAPI.Services;

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

        [HttpPost]
        public IActionResult Recipe([FromBody]Recipe r) 
        {
            if (ModelState.IsValid)
            {
                if (_recipe.AddRecipe(r))
                {
                    var uri = "https://" +HttpContext.Request.Host + "api/Recipe" +"?id/"+r.Id;
                    return Created(uri, r);
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
        public IActionResult Recipe()
        {
            var recipes = _recipe.GetAllRecipes();
            return Ok(recipes);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public IActionResult Recipe(int id)
        {
            var recipe = _recipe.GetRecipeById(id);
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

        [HttpGet("{tag}")]
        public IActionResult Recipe(string tag)
        {
            var recipe = _recipe.GetRecipeByTag(tag);
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

    }
}