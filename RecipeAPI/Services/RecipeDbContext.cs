using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public class RecipeDbContext:DbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options): base(options)
        { }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
