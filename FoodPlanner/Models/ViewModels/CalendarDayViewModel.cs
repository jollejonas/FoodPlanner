namespace FoodPlanner.Models.ViewModels
{
    public class CalendarDayViewModel
    {
        public DateTime Date { get; set; }
        public MealPlan ?MealPlan { get; set; }
    }
}
