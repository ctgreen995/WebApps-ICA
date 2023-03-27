using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThAmCo.Events.Models.VenuesDTOs;

namespace ThAmCo.Events.Services.Venues
{
    public class VenuesController : Controller
    {
        private readonly HttpClient _client;

        public VenuesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7088/");
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        public async Task<List<EventTypeDTO>> GetEventTypes()
        {
            HttpResponseMessage eventTypesResponse =
                await _client.GetAsync("api/EventTypes");

            if (!eventTypesResponse.IsSuccessStatusCode)
            {
                return null;
            }

            return await eventTypesResponse.Content.ReadAsAsync<List<EventTypeDTO>>();
        }

        public async Task<IActionResult> GetAvailableVenues(string selectedEventType, DateTime beginDate, DateTime endDate)
        {
            string query = $"api/Availability?" +
                $"eventType={selectedEventType}&" +
                $"beginDate={beginDate.ToString("yyyy-MM-dd")}&" +
                $"endDate={endDate.ToString("yyyy-MM-dd")}";

            HttpResponseMessage response = await _client.GetAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error - Failed to retrieve available venues.");
            }
            var availabilityDTOs = await response.Content.ReadAsAsync<List<AvailabilityDTO>>();
            if (availabilityDTOs == null || availabilityDTOs.Count == 0)
            {
                return NotFound("No available venues.");
            }

            var availabilityDetails = new UpdatedAvailabilityDTO
            {
                SelectVenues = SetVenuesSelector(availabilityDTOs),
                AvailabilityDTOs = availabilityDTOs
            };

            return Ok(availabilityDetails);
        }

        public async Task<ReservationGetDTO> MakeReservation
            (DateTime eventDate, string venueCode, string staffId)
        {
            HttpResponseMessage reserveVenue =
                    await _client.PostAsJsonAsync("api/Reservations/", new ReservationPostDTO
                    {
                        EventDate = eventDate,
                        VenueCode = venueCode,
                        StaffId = staffId
                    });

            if (reserveVenue.IsSuccessStatusCode)
            {
                return await reserveVenue.Content.ReadAsAsync<ReservationGetDTO>();
            }
            return null;
        }

        public async Task<HttpResponseMessage> DeleteReservation(string reference)
        {
            return await _client.DeleteAsync($"/api/Reservations/{reference}");
        }

        public async Task<ReservationGetDTO> RetrieveEventVenue(string reference)
        {
            HttpResponseMessage getReservation = await _client.GetAsync($"/api/Reservations/{reference}");

            if (getReservation.IsSuccessStatusCode)
            {
                return await getReservation.Content.ReadAsAsync<ReservationGetDTO>();
            }
            return null;
        }

        private List<SelectListItem> SetVenuesSelector(List<AvailabilityDTO> availabilityDTOs)
        {
            return availabilityDTOs.Select(a => new SelectListItem
            {
                Text = "Name: " + a.Name +
                ", Capacity: " + a.Capacity +
                ",\n Date: " + a.Date.ToString("dd/MM/yyyy") +
                ",\n £" + a.CostPerHour,

                Value = a.Code + a.Date.ToString("dd-MM-yyy")
            }).ToList();
        }
    }
}
