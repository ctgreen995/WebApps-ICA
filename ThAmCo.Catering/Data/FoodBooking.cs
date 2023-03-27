namespace ThAmCo.Catering.Data
{
    public class FoodBooking
    {
        public int FoodBookingId { get; set; }
        public string ClientReferenceId { get; set; }
        public int NumberOfGuests { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
