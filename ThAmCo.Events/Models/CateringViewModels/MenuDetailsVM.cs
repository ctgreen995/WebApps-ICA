using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models.CateringViewModels
{
    public class MenuDetailsVM
    {
        public int MenuID { get; set; }

        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }

        [Display(Name = "Menu Food Items")]
        public IEnumerable<SelectListItem> MenuFoodItems { get; set; }
    }
}
