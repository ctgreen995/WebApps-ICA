namespace ThAmCo.Events.Events.Data
{
    public class EventStaff
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
