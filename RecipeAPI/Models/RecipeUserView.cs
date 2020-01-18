using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models
{
    public class RecipeUserView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
