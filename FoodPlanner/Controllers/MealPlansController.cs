using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodPlanner.Data;
using FoodPlanner.Models;
using FoodPlanner.Models.ViewModels;

namespace FoodPlanner.Controllers
{
    public class MealPlansController : Controller
    {
        private readonly FoodPlannerContext _context;

        public MealPlansController(FoodPlannerContext context)
        {
            _context = context;
        }

        // GET: MealPlans
        public IActionResult Index()
        {
            var calendar = new CalendarViewModel();
            var mealPlans = _context.MealPlan.Include(mp => mp.Recipe).ToList();
            calendar.AssignMealPlans(mealPlans);

            return View(calendar);
        }

        // GET: Show all MealPlans

        public async Task<IActionResult> ShowAll()
        {
            return _context.MealPlan != null ?
                        View(await _context.MealPlan.Include(mp => mp.Recipe).ToListAsync()) :
                        Problem("Entity set 'FoodPlannerContext.MealPlan'  is null.");
        }

        // GET: MealPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MealPlan == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan
                .Include(m => m.Recipe)
                .FirstOrDefaultAsync(m => m.MealPlanID == id);
            if (mealPlan == null)
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // GET: MealPlans/Create
        public IActionResult Create(int? id)
        {
            var purchaseOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes" },
                new SelectListItem { Text = "No" }
            };

            ViewData["Purchased"] = new SelectList(purchaseOptions, "Text", "Text");
            var mealPlan = new MealPlan
            {
                Date = DateTime.Today
            };

            if (id.HasValue)
            {
                mealPlan.RecipeID = (int)id;
                ViewData["RecipeID"] = new SelectList(_context.Recipe, "RecipeID", "RecipeName", mealPlan.RecipeID);
                Console.WriteLine("Something");
            }
            else
            {
                ViewData["RecipeID"] = new SelectList(_context.Recipe, "RecipeID", "RecipeName");
                Console.WriteLine("No Value");
            }
            return View(mealPlan);
        }

        // POST: MealPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealPlanID,RecipeID,Date,Purchased")] MealPlan mealPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mealPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeID"] = new SelectList(_context.Recipe, "RecipeID", "RecipeID", mealPlan.RecipeID);
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        // 'entry.Key' holds the name of the field
                        // 'entry.Value.Errors' holds the collection of errors
                        foreach (var error in entry.Value.Errors)
                        {
                            // 'error.ErrorMessage' contains the error message
                            Console.WriteLine($"{entry.Key}: {error.ErrorMessage}");
                        }
                    }
                }

                // Optionally, return the same view to display the errors
                return View(mealPlan);
            }
            return View(mealPlan);

        }

        // GET: MealPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var purchaseOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes" },
                new SelectListItem { Text = "No" }
            };

            ViewData["Purchased"] = new SelectList(purchaseOptions, "Text", "Text");
            if (id == null || _context.MealPlan == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan.FindAsync(id);
            if (mealPlan == null)
            {
                return NotFound();
            }
            ViewData["RecipeID"] = new SelectList(_context.Recipe, "RecipeID", "RecipeName", mealPlan.RecipeID);
            return View(mealPlan);
        }

        // POST: MealPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MealPlanID,RecipeID,Date,Purchased")] MealPlan mealPlan)
        {
            if (id != mealPlan.MealPlanID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealPlanExists(mealPlan.MealPlanID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeID"] = new SelectList(_context.Recipe, "RecipeID", "RecipeID", mealPlan.RecipeID);
            return View(mealPlan);
        }

        // GET: MealPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MealPlan == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan
                .Include(m => m.Recipe)
                .FirstOrDefaultAsync(m => m.MealPlanID == id);
            if (mealPlan == null)
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // POST: MealPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MealPlan == null)
            {
                return Problem("Entity set 'FoodPlannerContext.MealPlan'  is null.");
            }
            var mealPlan = await _context.MealPlan.FindAsync(id);
            if (mealPlan != null)
            {
                _context.MealPlan.Remove(mealPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAll));
        }

        private bool MealPlanExists(int id)
        {
          return (_context.MealPlan?.Any(e => e.MealPlanID == id)).GetValueOrDefault();
        }
    }
}
