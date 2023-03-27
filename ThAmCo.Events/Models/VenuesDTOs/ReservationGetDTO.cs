using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models.VenuesDTOs
{
    public class ReservationGetDTO
    {
        public string Reference { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        public string VenueCode { get; set; }

        public DateTime WhenMade { get; set; }

        public string StaffId { get; set; }
    }
}
