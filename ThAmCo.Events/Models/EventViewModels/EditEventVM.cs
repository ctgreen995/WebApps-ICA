using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ThAmCo.Events.Models.CateringDTOs;
using ThAmCo.Events.Models.VenuesDTOs;

namespace ThAmCo.Events.Models.EventViewModels
{
    public class EditEventVM
    {
        public string EventId { get; set; }

        public string Title { get; set; }

        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime EventDate { get; set; }

        public string EventType { get; set; }

        [AllowNull]
        public string Reservation { get; set; }

        [AllowNull]
        public string SelectedVenue { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AvailableVenuesSelector { get; set; }

        [ValidateNever]
        public List<AvailabilityDTO> VenuesDTOs { get; set; }

        [AllowNull]
        [Display(Name = "Number Of Guests:")]
        public int NumberOfGuests { get; set; }

        [ValidateNever]
        public int SelectedMenu { get; set; }

        [AllowNull]
        public int FoodBooking { get; set; }

        [ValidateNever]
        public List<SelectListItem> AvailableMenusSelector { get; set; }

        [ValidateNever]
        public List<MenuDTO> MenuDTOs { get; set; }

        [ValidateNever]
        public List<SelectListItem> AvailableStaff { get; set; }

        [ValidateNever]
        public List<SelectListItem> AssignedStaff { get; set; }
        [ValidateNever]
        public List<SelectListItem> AvailableGuests { get; set; }

        [ValidateNever]
        public List<SelectListItem> AttendingGuests { get; set; }
    }
}