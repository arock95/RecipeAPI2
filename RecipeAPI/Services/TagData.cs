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

        public Tag AddTag(string tag)
        {
            _tags.Tags.Add(new Tag { Name = tag });
            _tags.SaveChanges();
            return FindTag(tag);
        }

        public Tag FindTag(string tag)
        {
            return _tags.Tags.FirstOrDefault(t => t.Name == tag);
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _tags.Tags.ToArray();
        }
    }
}
