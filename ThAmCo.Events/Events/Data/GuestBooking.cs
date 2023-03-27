namespace ThAmCo.Events.Events.Data
{
    public class GuestBooking
    {
        public string EventBookingId { get; set; }
        public Event Event { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public bool Attended { get; set; } = false;
    }
}
