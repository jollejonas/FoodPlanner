using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodPlanner.Models
{
    public class MealPlan
    {
        public int MealPlanID { get; set; }
        public int RecipeID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Purchased { get; set; }

        [ForeignKey("RecipeID")]
        public virtual Recipe Recipe { get; set; }
    }
}
