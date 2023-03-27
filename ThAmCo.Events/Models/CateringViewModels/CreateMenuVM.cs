using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThAmCo.Events.Models.CateringViewModels
{
    public class CreateMenuVM
    {
        public string MenuName { get; set; }
        public IEnumerable<SelectListItem> MenuFoodItems { get; set; }
        public IEnumerable<SelectListItem> FoodItems { get; set; }
    }
}
