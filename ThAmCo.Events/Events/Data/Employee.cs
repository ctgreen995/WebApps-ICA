namespace ThAmCo.Events.Events.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFirstAider { get; set; }
        public List<EventStaff> Events { get; set; }
    }
}
