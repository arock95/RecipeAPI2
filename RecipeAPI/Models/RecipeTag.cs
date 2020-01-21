namespace RecipeAPI.Models
{
    public class RecipeTag
    {//join entity for Many to Many relationship of recipes and tags
        public int RecipeId { get; set; }
        public int TagId { get; set; }
        public Recipe Recipe { get; set; }
        public Tag Tag { get; set; }
    }
}
