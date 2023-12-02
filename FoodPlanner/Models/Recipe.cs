namespace FoodPlanner.Models
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public string ImgURL { get; set; }
        public string Link { get; set; }
        public int Portions { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual ICollection<RecipeTag> RecipeTags { get; set; }

    }
}
