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
    public class RecipeTagsController : ControllerBase
    {
        private readonly FoodPlannerContext _context;

        public RecipeTagsController(FoodPlannerContext context)
        {
            _context = context;
        }

        // GET: api/RecipeTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeTag>>> GetRecipeTag()
        {
          if (_context.RecipeTag == null)
          {
              return NotFound();
          }
            return await _context.RecipeTag.ToListAsync();
        }

        // GET: api/RecipeTags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeTag>> GetRecipeTag(int id)
        {
          if (_context.RecipeTag == null)
          {
              return NotFound();
          }
            var recipeTag = await _context.RecipeTag.FindAsync(id);

            if (recipeTag == null)
            {
                return NotFound();
            }

            return recipeTag;
        }

        // PUT: api/RecipeTags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeTag(int id, RecipeTag recipeTag)
        {
            if (id != recipeTag.RecipeTagID)
            {
                return BadRequest();
            }

            _context.Entry(recipeTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeTagExists(id))
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

        // POST: api/RecipeTags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipeTag>> PostRecipeTag(RecipeTag recipeTag)
        {
          if (_context.RecipeTag == null)
          {
              return Problem("Entity set 'FoodPlannerContext.RecipeTag'  is null.");
          }
            _context.RecipeTag.Add(recipeTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipeTag", new { id = recipeTag.RecipeTagID }, recipeTag);
        }

        // DELETE: api/RecipeTags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeTag(int id)
        {
            if (_context.RecipeTag == null)
            {
                return NotFound();
            }
            var recipeTag = await _context.RecipeTag.FindAsync(id);
            if (recipeTag == null)
            {
                return NotFound();
            }

            _context.RecipeTag.Remove(recipeTag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeTagExists(int id)
        {
            return (_context.RecipeTag?.Any(e => e.RecipeTagID == id)).GetValueOrDefault();
        }
    }
}
