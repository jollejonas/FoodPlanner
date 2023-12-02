using System.ComponentModel.DataAnnotations.Schema;

namespace FoodPlanner.Models
{
    public class RecipeTag
    {
        public int RecipeTagID { get; set; }
        public int RecipeID { get; set; }
        public int TagID { get; set; }

        [ForeignKey("RecipeID")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey("TagID")]
        public virtual Tag Tag { get; set; }
    }
}
