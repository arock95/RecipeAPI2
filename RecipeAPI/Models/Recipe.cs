using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecipeTag> RecipeTags { get; set; }


    }
}
