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
    public class IngredientsController : ControllerBase
    {
        private readonly FoodPlannerContext _context;

        public IngredientsController(FoodPlannerContext context)
        {
            _context = context;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredient()
        {
          if (_context.Ingredient == null)
          {
              return NotFound();
          }
            return await _context.Ingredient.ToListAsync();
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
          if (_context.Ingredient == null)
          {
              return NotFound();
          }
            var ingredient = await _context.Ingredient.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient;
        }

        // PUT: api/Ingredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(int id, Ingredient ingredient)
        {
            if (id != ingredient.IngredientID)
            {
                return BadRequest();
            }

            _context.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: api/Ingredients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient)
        {
          if (_context.Ingredient == null)
          {
              return Problem("Entity set 'FoodPlannerContext.Ingredient'  is null.");
          }
            _context.Ingredient.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredient", new { id = ingredient.IngredientID }, ingredient);
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            if (_context.Ingredient == null)
            {
                return NotFound();
            }
            var ingredient = await _context.Ingredient.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredient.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientExists(int id)
        {
            return (_context.Ingredient?.Any(e => e.IngredientID == id)).GetValueOrDefault();
        }
    }
}
