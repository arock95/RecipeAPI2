using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<RecipeUser> _recipeUser;
        private readonly SignInManager<RecipeUser> _signIn;
        private readonly IConfiguration _config;

        public UserController(UserManager<RecipeUser> recipeUser, 
            SignInManager<RecipeUser> signin, IConfiguration config)
        {
            _recipeUser = recipeUser;
            _signIn = signin;
            _config = config;
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

        [HttpPost]
        [Route("/createtoken")]
        public async Task<IActionResult> CreateToken([FromBody]RecipeUserView userView)
        {
            if (ModelState.IsValid)
            {
                var user = await _recipeUser.FindByNameAsync(userView.UserName);
                if (user != null)
                {
                    var result = await _signIn.CheckPasswordSignInAsync(user, userView.Password, false);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"],
                            _config["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(60),
                            signingCredentials: creds
                            );
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Created("", results);
                    }
                }

            }
            return BadRequest();
        }
    }
}