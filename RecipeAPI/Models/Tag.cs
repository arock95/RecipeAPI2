using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<RecipeTag> RecipeTags { get; set; }
    }
}
