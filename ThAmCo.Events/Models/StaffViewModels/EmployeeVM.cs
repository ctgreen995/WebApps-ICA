using System.ComponentModel;
using ThAmCo.Events.Events.Data;

namespace ThAmCo.Events.Models.StaffViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("First Aider")]
        public bool IsFirstAider { get; set; }
        public List<EventStaff> Events { get; set; }
    }
}
