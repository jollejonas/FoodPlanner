using System;
using System.Collections.Generic;
using System.Linq;
using FoodPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FoodPlanner.Data; // Replace with your actual namespace

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new FoodPlannerContext(
            serviceProvider.GetRequiredService<DbContextOptions<FoodPlannerContext>>()))
        {
            // Check if we have any MealPlans already in database.
            if (context.MealPlan.Any())
            {
                return; // DB has been seeded.
            }

            var flour = new Ingredient { IngredientName = "Flour" };
            var sugar = new Ingredient { IngredientName = "Sugar" };
            var cocoa = new Ingredient { IngredientName = "Cocoa Powder" };

            var sweetTag = new Tag { TagName = "Sweet" };
            var dessertTag = new Tag { TagName = "Dessert" };

            var chocolateCake = new Recipe
            {
                RecipeName = "Chocolate Cake",
                Description = "A rich chocolate cake",
                ImgURL = "https://example.com/chocolate-cake.jpg",
                Link = "https://example.com/chocolate-cake-recipe",
                Portions = 8,
                RecipeIngredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { Ingredient = flour, Quantity = 2, Unit = "Cups" },
                    new RecipeIngredient { Ingredient = sugar, Quantity = 1, Unit = "Cup" },
                    new RecipeIngredient { Ingredient = cocoa, Quantity = 1, Unit = "Cup" }
                },
                RecipeTags = new List<RecipeTag>
                {
                    new RecipeTag { Tag = sweetTag },
                    new RecipeTag { Tag = dessertTag }
                }
            };

            context.MealPlan.Add(new MealPlan
            {
                Date = DateTime.Parse("2023-11-29"),
                Purchased = "No",
                Recipe = chocolateCake
            });

            context.SaveChanges();
        }
    }
}