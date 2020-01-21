using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class RecipeView
    {
        public RecipeView()
        {
            Tags = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
