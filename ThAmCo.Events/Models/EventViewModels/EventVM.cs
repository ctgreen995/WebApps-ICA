using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Models.GuestViewModels;
using ThAmCo.Events.Models.StaffViewModels;

namespace ThAmCo.Events.Models.EventViewModels
{
    public class EventVM
    {
        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("Event Title")]
        public string Title { get; set; }

        [DisplayName("Event Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        [DisplayName("Event Type")]
        public string EventType { get; set; }

        [DisplayName("Venue Reservation ID")]
        public string Reservation { get; set; }

        [DisplayName("FoodBooking ID")]
        public int? FoodBooking { get; set; }

        [DisplayName("Guests")]
        public List<GuestVM> Guests { get; set; }

        [DisplayName("Assigned Staff")]
        public List<EmployeeVM> AssignedStaff { get; set; }

        public bool FirstAiderAssigned { get; set; }
    }
}