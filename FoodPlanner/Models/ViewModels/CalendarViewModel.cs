using System.Globalization;

namespace FoodPlanner.Models.ViewModels
{
    public class CalendarViewModel
    {
        public List<CalendarDayViewModel> Days { get; set; }
        public MealPlan MealPlan { get; set; }

        public CalendarViewModel()
        {
            Days = new List<CalendarDayViewModel>();
            var startDate = DateTime.Today.Date;

            for(int i = 0;i < 14; i++)
            {
                Days.Add(new CalendarDayViewModel {Date = startDate.AddDays(i)});
            }

        }

        public void AssignMealPlans(List<MealPlan> mealPlans)
        {
            foreach (var day in Days)
            {
                day.MealPlan = mealPlans?.FirstOrDefault(mp => mp.Date.Date == day.Date.Date);
                Console.WriteLine(day.MealPlan);
            }
        }
    }
}
