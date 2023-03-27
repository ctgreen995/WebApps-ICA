using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThAmCo.Events.Models.VenuesDTOs
{
    public class UpdatedAvailabilityDTO
    {
        public List<SelectListItem> SelectVenues { get; set; }
        public List<AvailabilityDTO> AvailabilityDTOs { get; set; }
    }
}