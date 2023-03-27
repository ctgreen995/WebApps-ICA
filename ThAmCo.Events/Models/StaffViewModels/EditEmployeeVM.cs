using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ThAmCo.Events.Models.StaffViewModels
{
    public class EditEmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFirstAider { get; set; }
        [DisplayName("Assigned Events: ")]
        public IEnumerable<SelectListItem> AssignedEvents { get; set; }
        [DisplayName("Available Events: ")]
        public IEnumerable<SelectListItem> AvailableEvents { get; set; }
    }
}
