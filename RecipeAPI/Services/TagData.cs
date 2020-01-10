using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public class TagData : ITagData
    {
        private readonly RecipeDbContext _tags;
        public TagData(RecipeDbContext tags)
        {
            _tags = tags;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _tags.Tags.ToArray();
        }
    }
}
