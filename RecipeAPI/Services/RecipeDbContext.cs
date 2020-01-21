using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public class RecipeDbContext:IdentityDbContext<RecipeUser>
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options): base(options)
        { }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RecipeTag>().HasKey(s => new { s.RecipeId, s.TagId });
        }
    }
}
