using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Models.CateringDTOs;
using ThAmCo.Events.Models.VenuesDTOs;

namespace ThAmCo.Events.Models.EventViewModels
{
    public class CreateEventVM
    {
        public string EventId { get; set; }
        public string Title { get; set; }

        [Display(Name = "Begin Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BeginDate { get; set; } = DateTime.Now.AddDays(-60);

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public string EventDate { get; set; }

        public string VenueCode { get; set; }

        public string SelectedEventType { get; set; }

        [Display(Name = "Event Type")]
        public IEnumerable<SelectListItem> EventTypes { get; set; }

        public string SelectedAvailability { get; set; }

        public IEnumerable<SelectListItem> Availability { get; set; }

        public List<AvailabilityDTO> AvailableVenues { get; set; }

        [Display(Name = "Number Of Guests:")]
        public int NumberOfGuests { get; set; }

        public int SelectedMenu { get; set; }

        public IEnumerable<SelectListItem> AvailableMenus { get; set; }

        public List<MenuDTO> MenuDTOs { get; set; }
    }
}
