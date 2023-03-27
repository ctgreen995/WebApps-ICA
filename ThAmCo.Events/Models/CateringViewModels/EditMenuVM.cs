using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThAmCo.Events.Models.CateringViewModels
{
    public class EditMenuVM
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public IEnumerable<SelectListItem> MenuFoodItems { get; set; }
        public IEnumerable<SelectListItem> FoodItems { get; set; }
    }
}