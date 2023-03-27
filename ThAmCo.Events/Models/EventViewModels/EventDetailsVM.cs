using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Models.StaffViewModels;

namespace ThAmCo.Events.Models.EventViewModels
{
    public class EventDetailsVM
    {
        public string Id { get; set; }
        public string Title { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string VenueCode { get; set; }
        public string WhenMade { get; set; }
        public string StaffId { get; set; }
        public string EventType { get; set; }
        public int? FoodBooking { get; set; }
        public int TotalNumberOfGuests { get; set; }
        public int AttendedGuests { get; set; }
        public List<EmployeeVM> Staff { get; set; }
        public List<EventGuestVM> Guests { get; set; }
    }
}
