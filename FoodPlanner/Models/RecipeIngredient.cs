﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FoodPlanner.Models
{
    public class RecipeIngredient
    {
        public int RecipeIngredientID { get; set; }
        public int RecipeID { get; set; }
        public int IngredientID { get; set; }
        [Column(TypeName = "decimal(10, 1)")]
        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        [ForeignKey("RecipeID")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey("IngredientID")]
        public virtual Ingredient Ingredient { get; set; }
    }
}
