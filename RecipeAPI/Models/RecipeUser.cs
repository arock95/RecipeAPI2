using Microsoft.AspNetCore.Identity;

namespace RecipeAPI.Models
{
    public class RecipeUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
