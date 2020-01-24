using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using RecipeAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeData _recipe;
        private readonly ITagData _tags;
        private readonly RecipeDbContext _context;
        public RecipeController(IRecipeData recipe, ITagData tags, RecipeDbContext context)
        {
            _recipe = recipe;
            _tags = tags;
            _context = context;
        }

        [HttpPost]
        public IActionResult Recipe([FromBody]RecipeView r)
        {
            if (ModelState.IsValid)
            {
                //automapper
                var recipe = new Recipe
                {
                    Name = r.Name,
                    Description = r.Description
                };

                if (_recipe.AddRecipe(recipe))
                {
                    // see if tags exist, if not add them
                    // then add many-many mapping via recipetag
                    foreach (string t in r.Tags)
                    {
                        var result = _tags.FindTag(t);
                        if (result == null)
                        {
                            result = _tags.AddTag(t);
                        }
                        //add the 'join' between recipe and tag
                        _context.Add(new RecipeTag { RecipeId = recipe.Id, TagId = result.Id });
                        _context.SaveChanges();
                    }
                    var uri = Url.Link("RecipeById", new { recipe.Id });

                    RecipeView rv = RecipeToView(recipe);
                    return Created(uri, rv);
                }
                else
                {
                    return BadRequest("Invalid Recipe - cannot add to database");
                }
            }
            else //model state is invalid
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Recipe([FromQuery]string tag)
        {
            List<RecipeView> recipes = new List<RecipeView>();

            if (!string.IsNullOrEmpty(tag))
            {
                var result = _recipe.GetRecipeByTag(tag);
                foreach (RecipeTag rt in result.RecipeTags) {
                    recipes.Add(new RecipeView { 
                        Name = rt.Recipe.Name,
                        Description = rt.Recipe.Description
                    });//leaving tags out, need to fix the search
                } 
            }
            else
            {
                var result = _recipe.GetAllRecipes().ToList();
                foreach (Recipe r in result)
                {
                    var temp = RecipeToView(r);
                    recipes.Add(temp);
                }
            }

            return Ok(recipes);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}", Name = "RecipeByID")]
        public IActionResult RecipeById(int id)
        {
            var recipe = _recipe.GetRecipeById(id);
            if (recipe != null)
            {
                RecipeView rv = RecipeToView(recipe);
                return Ok(rv);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteRecipe(int id)
        {
            _recipe.DeleteRecipe(id);
            return NoContent();
        }

        [HttpPatch]
        //[Route("{id:int}")]
        public IActionResult UpdateRecipe([FromBody]RecipeView r)
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

        private RecipeView RecipeToView(Recipe r)
        {
            var return_me = new RecipeView
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            };
            foreach (RecipeTag rt in r.RecipeTags)
            {
                return_me.Tags.Add(rt.Tag.Name);
            }
            return return_me;
        }
    }
}