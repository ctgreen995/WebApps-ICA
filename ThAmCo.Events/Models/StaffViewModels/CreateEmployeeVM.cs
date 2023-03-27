using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ThAmCo.Events.Models.StaffViewModels
{
    public class CreateEmployeeVM
    {
        public string Name { get; set; }
        public bool IsFirstAider { get; set; }

        [AllowNull]
        [DisplayName("Assigned Events: ")]
        public IEnumerable<SelectListItem> AssignedEvents { get; set; }

        [AllowNull]
        [DisplayName("Available Events: ")]
        public IEnumerable<SelectListItem> AvailableEvents { get; set; }
    }
}
