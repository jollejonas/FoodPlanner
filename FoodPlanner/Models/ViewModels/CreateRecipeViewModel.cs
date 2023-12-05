using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodPlanner.Models.ViewModels
{
    public class CreateRecipeViewModel
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public string ImgURL { get; set; }
        public string Link {  get; set; }
        public int Portions { get; set; }

        public List<string> IngredientText { get; set; }
        public List<decimal> Quantities { get; set; }
        public List<string> Units { get; set; }
        public List<SelectListItem> ?UnitOptions { get; set; }


    }
}
