using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models.VenuesDTOs
{
    public class ReservationPostDTO
    {
        [Required, DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required, MinLength(5), MaxLength(5)]
        public string VenueCode { get; set; }

        [Required]
        public string StaffId { get; set; }
    }
}
