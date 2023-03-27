using System.ComponentModel;

namespace ThAmCo.Events.Models.StaffViewModels
{
    public class EmployeeDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFirstAider { get; set; }

        [DisplayName("Assigned Events")]
        public List<EmployeeEventVM> AssignedEvents { get; set; }
    }
}
