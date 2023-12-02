using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodPlanner.Models;

namespace FoodPlanner.Data
{
    public class FoodPlannerContext : DbContext
    {
        public FoodPlannerContext (DbContextOptions<FoodPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<FoodPlanner.Models.Ingredient> Ingredient { get; set; } = default!;

        public DbSet<FoodPlanner.Models.Tag> Tag { get; set; } = default!;

        public DbSet<FoodPlanner.Models.Recipe> Recipe { get; set; } = default!;

        public DbSet<FoodPlanner.Models.RecipeIngredient> RecipeIngredient { get; set; } = default!;

        public DbSet<FoodPlanner.Models.RecipeTag> RecipeTag { get; set; } = default!;

        public DbSet<FoodPlanner.Models.MealPlan> MealPlan { get; set; } = default!;
    }
}
