using FoodPlanner.Data;
using FoodPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListApiController : ControllerBase
    {
        private readonly FoodPlannerContext _context;

        public ShoppingListApiController(FoodPlannerContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredientsToPuchase()
        {
            var ingredientsToPurchase = await _context.MealPlan
                .Where(mp => mp.Purchased == "No")
                .SelectMany(mp => mp.Recipe.RecipeIngredients)
                .GroupBy(ri => ri.Ingredient.IngredientName)
                .Select(i => new
                {
                    Name = i.Key,
                    TotalQuantity = i.Sum(ri => (double)ri.Quantity),
                    Unit = i.First().Unit
                })
                .ToListAsync();

            return Ok(ingredientsToPurchase);
        }
    }
}
