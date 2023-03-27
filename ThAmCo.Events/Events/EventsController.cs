using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Services.Guests;
using ThAmCo.Events.Services.Catering;
using ThAmCo.Events.Services.Venues;
using ThAmCo.Events.Services.Staff;
using ThAmCo.Events.Events.Data;
using ThAmCo.Events.Models.EventViewModels;
using ThAmCo.Events.Models.CateringDTOs;
using ThAmCo.Events.Models.VenuesDTOs;
using ThAmCo.Events.Models.GuestViewModels;
using ThAmCo.Events.Models.StaffViewModels;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;
        private readonly CateringController _cateringController;
        private readonly VenuesController _venuesController;
        private readonly GuestsController _guestsController;
        private readonly StaffController _staffController;

        public EventsController(EventsDbContext context)
        {
            _context = context;
            _cateringController = new CateringController();
            _venuesController = new VenuesController();
            _guestsController = new GuestsController(_context);
            _staffController = new StaffController(_context);
        }

        #region CRUD

        // GET: Events
        public async Task<IActionResult> ListEvents()
        {
            var events = await _context.Events
                .Where(e => !e.IsDeleted)
                .Include(gb => gb.Guests)
                .ThenInclude(g => g.Guest)
                .Include(s => s.Staff)
                .ThenInclude(e => e.Employee)
                .ToListAsync();

            List<EventVM> eventsVM = events.Select(e => new EventVM
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                EventType = e.EventType,
                FoodBooking = e.FoodBooking,
                Reservation = e.Reservation,
                Guests = e.Guests.Select(g => new GuestVM
                {
                    Name = g.Guest.Name
                }).ToList(),
                AssignedStaff = e.Staff.Select(s => new EmployeeVM
                {
                    Name = s.Employee.Name,
                    IsFirstAider = s.Employee.IsFirstAider
                }).ToList(),
                FirstAiderAssigned = e.Staff.Any(f => f.Employee.IsFirstAider),
            }
            ).ToList();

            return View(eventsVM);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> EventDetails(string id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events
                .Where(e => !e.IsDeleted)
                 .Include(e => e.Guests)
                 .ThenInclude(g => g.Guest)
                 .Include(s => s.Staff)
                 .ThenInclude(e => e.Employee)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }
            ReservationGetDTO reservation = await _venuesController.RetrieveEventVenue(@event.Reservation);

            EventDetailsVM eventVM = new EventDetailsVM
            {
                Id = @event.Id,
                Title = @event.Title,
                Date = @event.Date,
                EventType = @event.EventType,
                FoodBooking = @event.FoodBooking,
                Guests = SetEventGuestsVM(@event),
                Staff = SetAssignedStaffVM(@event),
                TotalNumberOfGuests = @event.Guests.Count,
                AttendedGuests = @event.TotalAttendedGuests,
                StaffId = reservation.StaffId,
                VenueCode = reservation.VenueCode,
                WhenMade = reservation.WhenMade.ToShortDateString()
            };

            return View(eventVM);
        }

        public async Task<IActionResult> CreateEvent()
        {
            List<EventTypeDTO> eventTypeDTOs = await _venuesController.GetEventTypes();

            if (eventTypeDTOs == null || eventTypeDTOs.Count == 0)
            {
                return NotFound("Unable to retrieve event types from the venues service.");
            }
            List<SelectListItem> eventTypes = eventTypeDTOs.Select(e => new SelectListItem
            {
                Text = e.Title,
                Value = e.Id
            }).ToList();

            List<MenuDTO> menuDTOs = await _cateringController.GetAvailableMenus();

            var eventVM = new CreateEventVM
            {
                EventTypes = eventTypes,
                Availability = new List<SelectListItem>(),
                AvailableMenus = SetMenuSelector(menuDTOs),
                MenuDTOs = menuDTOs
            };

            return View(eventVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventVM eventVM, string eventDate)
        {
            if (ModelState.IsValid)
            {
                DateTime parsedDate = DateTime.Parse(eventDate);

                ReservationGetDTO reservation = await _venuesController.MakeReservation
                    (parsedDate, eventVM.SelectedAvailability.Substring(0, 5), "1");

                var newEvent = new Event
                {
                    Id = $"{reservation.WhenMade.ToString("yyyyMMddHHmmss")}{eventVM.SelectedEventType}",
                    Title = eventVM.Title,
                    Date = parsedDate,
                    EventType = eventVM.SelectedEventType,
                    Reservation = reservation.Reference,
                    NumberOfGuests = eventVM.NumberOfGuests
                };

                _context.Add(newEvent);
                _context.SaveChanges();

                UpdateEventFoodBooking(newEvent, eventVM);

                _context.SaveChanges();

                return RedirectToAction(nameof(ListEvents));
            }
            return View(eventVM);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> EditEvent(string id)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => !e.IsDeleted && e.Id == id);
            if (@event == null)
            {
                return NotFound("Could not find that event.");
            }

            List<MenuDTO> menuDTOs = await _cateringController.GetAvailableMenus();

            List<SelectListItem> menuSelect = SetMenuSelector(menuDTOs);

            List<SelectListItem> assignedStaffSelect = await SetAssignedStaffSelector(id);

            List<SelectListItem> availableStaffSelect = await SetAvailableStaffSelector(id);

            List<SelectListItem> attendingGuestsSelector = await SetAttendingGuestsSelector(id);

            List<SelectListItem> availableGuestsSelector = await SetAvailableGuestsSelector(id);

            var eventVM = new EditEventVM
            {
                EventId = id,
                Title = @event.Title,
                EventDate = @event.Date,
                EventType = @event.EventType,
                AvailableVenuesSelector = new List<SelectListItem>(),
                Reservation = @event.Reservation,
                FoodBooking = @event.FoodBooking,
                NumberOfGuests = @event.NumberOfGuests,
                AvailableMenusSelector = menuSelect,
                MenuDTOs = menuDTOs,
                AvailableStaff = availableStaffSelect,
                AssignedStaff = assignedStaffSelect,
                AvailableGuests = availableGuestsSelector,
                AttendingGuests = attendingGuestsSelector
            };

            return View(eventVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(EditEventVM editEventVM, int[] selectedStaff, int[] selectedGuests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid form data, please check values.");
            }
            string reservation = await SetVenue(editEventVM);

            int foodBooking = await CreateEventFoodBooking(editEventVM);

            try
            {
                await _staffController.UpdateEventStaffing(selectedStaff, editEventVM.EventId);
                await _guestsController.UpdateGuestBooking(selectedGuests, editEventVM.EventId);

                var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == editEventVM.EventId && !e.IsDeleted);

                @event.Date = editEventVM.EventDate;
                @event.EventType = editEventVM.EventType;
                @event.FoodBooking = foodBooking;
                @event.NumberOfGuests = editEventVM.NumberOfGuests;
                @event.Reservation = reservation;
                @event.Title = editEventVM.Title;

                _context.Entry(@event).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return RedirectToAction(nameof(ListEvents));
        }

        // GET: Events/Delete/5 
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var eventVM = await _context.Events.Where(i => !i.IsDeleted && i.Id == id).Select(e => new EventVM
            {
                Id = e.Id,
                Date = e.Date,
                EventType = e.EventType,
                FoodBooking = e.FoodBooking,
                Reservation = e.Reservation,
                Title = e.Title,
            }).FirstOrDefaultAsync();

            if (eventVM == null)
            {
                return NotFound("Failed to find Event Id in database.");
            }
            return View(eventVM);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("DeleteEvent")]
        public async Task<IActionResult> DeleteEventConfirmed(string id)
        {
            try
            {
                var @event = await _context.Events.FindAsync(id);
                if (@event == null)
                {
                    return NotFound("Event to delete not found in database.");
                }

                var venuesResponse = await _venuesController.DeleteReservation(@event.Reservation);
                if (@event.Reservation != null)
                {
                    if (!venuesResponse.IsSuccessStatusCode)
                    {
                        return BadRequest("Unable to delete reservation.");
                    }
                }

                if (@event.FoodBooking != 0)
                {
                    var cateringResponse = await _cateringController.DeleteFoodBooking(@event.Id);
                    if (!cateringResponse.IsSuccessStatusCode)
                    {
                        return BadRequest("Unable to delete food booking.");
                    }
                }

                @event.IsDeleted = true;
                _context.Events.Update(@event);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListEvents));
            }
            catch (Exception e)
            {
                return BadRequest("Failed to delete event: " + e.Message);
            }
        }
        #endregion

        #region Venues Service

        private async Task<string> SetVenue(EditEventVM editEventVM)
        {
            if (editEventVM.SelectedVenue != null)
            {
                var reservationDTO = await _venuesController.MakeReservation
                    (editEventVM.EventDate, editEventVM.SelectedVenue.Substring(0, 5), "1");

                return reservationDTO.Reference;
            }
            return editEventVM.Reservation;
        }

        public async Task<IActionResult> RetrieveAvailableVenues
            (string selectedEventType, string beginDate, string endDate)
        {
            return await _venuesController.GetAvailableVenues
                (selectedEventType, DateTime.Parse(beginDate), DateTime.Parse(endDate));
        }

        public async Task<IActionResult> DeleteVenueReservation(string reservation)
        {
            var reservationDeleted = await _venuesController.DeleteReservation(reservation);
            if (!reservationDeleted.IsSuccessStatusCode)
            {
                return BadRequest("Unable to delete Reservation. " + reservationDeleted.ReasonPhrase);
            }
            return Ok();
        }
        #endregion

        #region Guests Service

        public List<EventGuestVM> SetEventGuestsVM(Event @event)
        {
            return @event.Guests.Select(g => new EventGuestVM
            {
                GuestId = g.GuestId,
                GuestName = g.Guest.Name,
                Attended = @event.Guests
                    .Where(eg => eg.GuestId == g.GuestId)
                    .Select(a => a.Attended).FirstOrDefault()

            }).ToList();
        }
        /* This method is used to register if a guests has attended an event. It get from the view model
         * all guests which has attended, creates a list of guest booking objects and updates the attended 
         * column in the GuestBooking table for those guests. */
        [HttpPost]
        public async Task<IActionResult> RegisterAttendance(EventDetailsVM eventDetailsVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data submitted.");
            }
            var @event = new Event
            {
                Id = eventDetailsVM.Id,
                TotalAttendedGuests = eventDetailsVM.AttendedGuests,
            };

            _context.Events.Attach(@event);
            _context.Entry(@event).Property(e => e.TotalAttendedGuests).IsModified = true;

            if (eventDetailsVM.Guests != null)
            {

                var guests = eventDetailsVM.Guests.Where(g => g.Attended == true)
                    .Select(a => _context.Guests.Find(a.GuestId)).ToList();

                var addAttendedGuests = eventDetailsVM.Guests
                    .Where(g => g.Attended == true).Select(a => new GuestBooking
                    {
                        EventBookingId = eventDetailsVM.Id,
                        GuestId = guests.FirstOrDefault(g => g.Id == a.GuestId).Id,
                        Attended = a.Attended
                    }).ToList();

                _context.UpdateRange(addAttendedGuests);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListEvents));
        }

        private async Task<List<SelectListItem>> SetAvailableGuestsSelector(string id)
        {
            List<Guest> availableGuests = await _guestsController.GetAvailableGuests(id);

            return availableGuests.Select(s => new SelectListItem
            {
                Text = s.Name + ", " + s.Street + ", " + s.Postcode,
                Value = s.Id.ToString()
            }).ToList();
        }

        private async Task<List<SelectListItem>> SetAttendingGuestsSelector(string id)
        {
            List<Guest> attendingGuests = await _guestsController.GetAttendingGuests(id);

            return attendingGuests.Select(s => new SelectListItem
            {
                Text = s.Name + ", " + s.Street + ", " + s.Postcode,
                Value = s.Id.ToString()
            }).ToList();
        }

        #endregion

        #region Catering Service

        /* This method creates a new food booking for an event if a new menu has been selected in the create
         * event view. */
        private async Task<int> CreateEventFoodBooking(EditEventVM editEventVM)
        {
            if (editEventVM.SelectedMenu != 0)
            {
                var foodBookingDTO = await _cateringController.CreateFoodBooking(new FoodBookingDTO
                {
                    ClientReferenceId = editEventVM.EventId,
                    MenuId = editEventVM.SelectedMenu,
                    NumberOfGuests = editEventVM.NumberOfGuests
                });
                return foodBookingDTO.FoodBookingId;
            }
            return editEventVM.FoodBooking;
        }

        /* This method creates a new food booking for an event if a new menu has been selected in the edit
         * event view. */
        private async void UpdateEventFoodBooking(Event newEvent, CreateEventVM eventVM)
        {
            if (eventVM.SelectedMenu != 0)
            {
                var foodBooking = _cateringController.CreateFoodBooking(new FoodBookingDTO
                {
                    ClientReferenceId = newEvent.Id,
                    MenuId = eventVM.SelectedMenu,
                    NumberOfGuests = eventVM.NumberOfGuests
                });
                var result = await _context.Events.SingleOrDefaultAsync(e => e.Id == newEvent.Id);
                if (result != null)
                {
                    result.FoodBooking = foodBooking.Result.FoodBookingId;
                }
            }
        }

        /* This method populates a select list with menus the inserts a placeholder value at index 0
         * to signify no menu has been selected, for use when creating or editing an event. */
        private List<SelectListItem> SetMenuSelector(List<MenuDTO> menuDTOs)
        {
            List<SelectListItem> menuSelect = menuDTOs.Select(m => new SelectListItem
            {
                Text = m.MenuName,
                Value = m.Id.ToString()
            }).ToList();

            menuSelect.Insert(0, new SelectListItem
            {
                Text = "-- Select an Available Menu --",
                Value = "0"
            });
            return menuSelect;
        }

        public async Task<IActionResult> DeleteFoodBooking(string eventId)
        {
            var foodBookingDeleted = await _cateringController.DeleteFoodBooking(eventId);
            if (!foodBookingDeleted.IsSuccessStatusCode)
            {
                return BadRequest("Unable to delete foodbooking! " + foodBookingDeleted.ReasonPhrase);
            }
            return Ok();
        }

        #endregion

        #region Staff Service

        public List<EmployeeVM> SetAssignedStaffVM(Event @event)
        {
            return @event.Staff.Select(s => new EmployeeVM
            {
                Id = s.EmployeeId,
                Name = s.Employee.Name,
                IsFirstAider = _staffController.GetIsFirstAider(s.EmployeeId).Result
            }).ToList();
        }

        private async Task<List<SelectListItem>> SetAvailableStaffSelector(string id)
        {
            List<Employee> availableStaff = await _staffController.GetAvailableStaff(id);

            return availableStaff.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();
        }

        private async Task<List<SelectListItem>> SetAssignedStaffSelector(string eventId)
        {
            List<Employee> assignedStaff = await _staffController.GetAssignedStaff(eventId);

            return assignedStaff.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();
        }

        #endregion
    }
}
