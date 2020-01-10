using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Services;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagData _tags;
        public TagController(ITagData tags)
        {
            _tags = tags;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_tags.GetAllTags());
        }


    }
}