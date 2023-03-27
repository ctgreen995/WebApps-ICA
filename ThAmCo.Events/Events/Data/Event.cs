using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Events.Data
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string EventType { get; set; }
        public string Reservation { get; set; }
        public int FoodBooking { get; set; }
        public int NumberOfGuests { get; set; }
        public int TotalAttendedGuests { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<EventStaff> Staff { get; set; }
        public List<GuestBooking> Guests { get; set; }
    }
}
