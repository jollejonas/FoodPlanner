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
using Microsoft.CodeAnalysis.CSharp;

namespace FoodPlanner.Controllers
{
    public class RecipesController : Controller
    {
        private readonly FoodPlannerContext _context;

        public RecipesController(FoodPlannerContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
              return _context.Recipe != null ? 
                          View(await _context.Recipe.ToListAsync()) :
                          Problem("Entity set 'FoodPlannerContext.Recipe'  is null.");
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.Include(r => r.RecipeIngredients).ThenInclude(i => i.Ingredient).FirstOrDefaultAsync(m => m.RecipeID == id);
            if (recipe == null)
            {
                return NotFound();
            }
            var viewModel = new CreateRecipeViewModel
            {
                RecipeID = recipe.RecipeID,
                RecipeName = recipe.RecipeName,
                Description = recipe.Description,
                ImgURL = recipe.ImgURL,
                Link = recipe.Link,
                Portions = recipe.Portions,
                IngredientText = GetIngredientName(recipe),
                Quantities = GetQuantities(recipe),
                Units = GetUnits(recipe)
            };


            return View(viewModel);
        }

        // GET: Recipes/Create

        public IActionResult Create()
        {
            var model = new CreateRecipeViewModel
            {
                UnitOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "dl", Text = "dl" },
                    new SelectListItem { Value = "ml", Text = "ml" },
                    new SelectListItem { Value = "g", Text = "Gram" },
                    new SelectListItem { Value = "stk", Text = "Styk" },
                    new SelectListItem { Value = "teske", Text = "Teske" },
                    new SelectListItem { Value = "spiseke", Text = "Spiseske" },
                    new SelectListItem { Value = "kg", Text = "Kilogram" }

                }
            };
            return View(model);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recipe = new Recipe
                {
                    RecipeName = model.RecipeName,
                    Description = model.Description,
                    ImgURL = model.ImgURL,
                    Link = model.Link,
                    Portions = model.Portions
                };

                _context.Recipe.Add(recipe);

                for (int i = 0; i < model.IngredientText.Count; i++)
                {
                    var ingredientName = model.IngredientText[i].Trim();
                    var ingredient = _context.Ingredient.FirstOrDefault(ing => ing.IngredientName == ingredientName) 
                                        ?? new Ingredient { IngredientName = ingredientName };

                    var recipeIngredient = new RecipeIngredient
                    {
                        Recipe = recipe,
                        Ingredient = ingredient,
                        Quantity = model.Quantities[i],
                        Unit = model.Units[i]
                    };

                    recipe.RecipeIngredients.Add(recipeIngredient);

                    if(ingredient.IngredientID == 0)
                    {
                        _context.Ingredient.Add(ingredient);
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
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
                return View(model);
            }
            return View(model);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeID,RecipeName,Description,ImgURL,Link,Portions")] Recipe recipe)
        {
            if (id != recipe.RecipeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeID))
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
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.RecipeID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'FoodPlannerContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipe?.Any(e => e.RecipeID == id)).GetValueOrDefault();
        }

        private List<string> GetIngredientName(Recipe recipe)
        {
            return recipe.RecipeIngredients.Where(ri => ri.Ingredient != null).Select(ri => ri.Ingredient.IngredientName).ToList();
        }
        private List<decimal> GetQuantities(Recipe recipe)
        {
            return recipe.RecipeIngredients.Select(ri => ri.Quantity).ToList();
        }
        private List<string> GetUnits(Recipe recipe)
        {
            return recipe.RecipeIngredients.Select(ri => ri.Unit).ToList();
        }
    }
}
