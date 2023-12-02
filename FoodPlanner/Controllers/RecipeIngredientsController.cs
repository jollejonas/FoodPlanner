using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodPlanner.Data;
using FoodPlanner.Models;

namespace FoodPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientsController : ControllerBase
    {
        private readonly FoodPlannerContext _context;

        public RecipeIngredientsController(FoodPlannerContext context)
        {
            _context = context;
        }

        // GET: api/RecipeIngredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeIngredient>>> GetRecipeIngredient()
        {
          if (_context.RecipeIngredient == null)
          {
              return NotFound();
          }
            return await _context.RecipeIngredient.ToListAsync();
        }

        // GET: api/RecipeIngredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeIngredient>> GetRecipeIngredient(int id)
        {
          if (_context.RecipeIngredient == null)
          {
              return NotFound();
          }
            var recipeIngredient = await _context.RecipeIngredient.FindAsync(id);

            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return recipeIngredient;
        }

        // PUT: api/RecipeIngredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeIngredient(int id, RecipeIngredient recipeIngredient)
        {
            if (id != recipeIngredient.RecipeIngredientID)
            {
                return BadRequest();
            }

            _context.Entry(recipeIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeIngredientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RecipeIngredients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipeIngredient>> PostRecipeIngredient(RecipeIngredient recipeIngredient)
        {
          if (_context.RecipeIngredient == null)
          {
              return Problem("Entity set 'FoodPlannerContext.RecipeIngredient'  is null.");
          }
            _context.RecipeIngredient.Add(recipeIngredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipeIngredient", new { id = recipeIngredient.RecipeIngredientID }, recipeIngredient);
        }

        // DELETE: api/RecipeIngredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeIngredient(int id)
        {
            if (_context.RecipeIngredient == null)
            {
                return NotFound();
            }
            var recipeIngredient = await _context.RecipeIngredient.FindAsync(id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            _context.RecipeIngredient.Remove(recipeIngredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeIngredientExists(int id)
        {
            return (_context.RecipeIngredient?.Any(e => e.RecipeIngredientID == id)).GetValueOrDefault();
        }
    }
}
