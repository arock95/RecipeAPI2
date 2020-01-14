using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<RecipeUser> _recipeUser;
        public UserController(UserManager<RecipeUser> recipeUser)
        {
            _recipeUser = recipeUser;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]RecipeUserView userView)
        {
            if (ModelState.IsValid)
            {
                RecipeUser user = await _recipeUser.FindByEmailAsync(userView.Email);
                if (user == null)
                {
                    user = new RecipeUser
                    {
                        Email = userView.Email,
                        UserName = userView.UserName,
                        FirstName = userView.FirstName,
                        LastName = userView.LastName
                    };
                    var result = await _recipeUser.CreateAsync(user, userView.Password);
                    if (result == IdentityResult.Success)
                    {
                        return Ok();
                    } 
                }
            }
            return BadRequest();
        }
    }
}