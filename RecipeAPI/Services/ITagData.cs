using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public interface ITagData
    {
        IEnumerable<Tag> GetAllTags();
        Tag FindTag(string tag);

        Tag AddTag(string tag);

    }
}
