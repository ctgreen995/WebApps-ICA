namespace ThAmCo.Events.Events.Data
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public List<GuestBooking> Bookings { get; set; }
        public string Town { get; set; }
        public bool IsDeleted { get; set; }
    }
}
