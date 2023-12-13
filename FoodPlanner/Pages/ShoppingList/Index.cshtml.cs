using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FoodPlanner.Pages.ShoppingList
{
    public class IndexModel : PageModel
    {
        public async Task OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = "https://localhost:7065/api/ShoppingListApi";
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Ingredients = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(content);
                }
            }
        }
        public List<Dictionary<string, object>> Ingredients { get; set; }
    }
}
