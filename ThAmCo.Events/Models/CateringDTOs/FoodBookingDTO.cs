namespace ThAmCo.Events.Models.CateringDTOs
{
    public class FoodBookingDTO
    {
        public int FoodBookingId { get; set; }
        public string ClientReferenceId { get; set; }
        public int NumberOfGuests { get; set; }
        public int MenuId { get; set; }
    }
}
