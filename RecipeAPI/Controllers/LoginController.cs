using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<RecipeUser> _signInMgr;
        public LoginController(ILogger<LoginController> logger,
            SignInManager<RecipeUser> signInManager)
        {
            _logger = logger;
            _signInMgr = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]RecipeUserView recipe)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInMgr.PasswordSignInAsync(recipe.UserName, recipe.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return Unauthorized();
        }
    }
}