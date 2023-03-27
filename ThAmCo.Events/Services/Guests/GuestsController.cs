using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Events.Data;
using ThAmCo.Events.Models.GuestViewModels;

namespace ThAmCo.Events.Services.Guests
{
    public class GuestsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestsController(EventsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListGuests()
        {
            List<GuestVM> guestVMs = await _context.Guests
                .Where(g => !g.IsDeleted)
                .Select(g => new GuestVM
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync();

            return View(guestVMs);
        }

        public async Task<IActionResult> GuestDetails(int? id)
        {
            var guest = await _context.Guests
                .Include(g => g.Bookings)
                .ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (guest == null)
            {
                return NotFound();
            }

            var guestDetailsVM = new GuestDetailsVM
            {
                Id = guest.Id,
                Name = guest.Name,
                Street = guest.Street,
                Town = guest.Town,
                Postcode = guest.Postcode,
                Telephone = guest.Telephone,
                Email = guest.Email,
                BookedEvents = guest.Bookings.Select(b => new GuestEventVM
                {
                    EventName = b.Event.Title,
                    Attended = b.Attended
                }).ToList()
            };

            return View(guestDetailsVM);
        }

        public IActionResult CreateGuest()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGuest(CreateGuestVM guest)
        {
            if (ModelState.IsValid)
            {
                var newGuest = new Guest
                {
                    Name = guest.Name,
                    Street = guest.Street,
                    Town = guest.Town,
                    Postcode = guest.Postcode,
                    Telephone = guest.Telephone,
                    Email = guest.Email
                };
                _context.Guests.Add(newGuest);
                await _context.SaveChangesAsync();

                _context.SaveChanges();

                return RedirectToAction(nameof(ListGuests));
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> EditGuest(int? id)
        {
            var guest = await _context.Guests
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

            if (guest == null)
            {
                return NotFound();
            }

            var editGuestVM = new EditGuestVM
            {
                Id = guest.Id,
                Name = guest.Name,
                Street = guest.Street,
                Town = guest.Town,
                Postcode = guest.Postcode,
                Telephone = guest.Telephone,
                Email = guest.Email,
            };

            return View(editGuestVM);
        }

        // POST: Guests/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditGuest(EditGuestVM editGuestVM)
        {
            if (ModelState.IsValid)
            {
                var guest = new Guest
                {
                    Id = editGuestVM.Id,
                    Name = editGuestVM.Name,
                    Street = editGuestVM.Street,
                    Town = editGuestVM.Town,
                    Postcode = editGuestVM.Postcode,
                    Telephone = editGuestVM.Telephone,
                    Email = editGuestVM.Email
                };
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (!GuestExists(guest.Id))
                    {
                        return NotFound(e);
                    }
                    else
                    {
                        return BadRequest(e);
                    }
                }
                return RedirectToAction(nameof(ListGuests));
            }
            return View(editGuestVM);
        }

        //GET: Guests/Delete/5
        public async Task<IActionResult> DeleteGuest(int? id)
        {
            var guest = await _context.Guests
                .Include(b => b.Bookings)
                .ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (guest == null)
            {
                return NotFound();
            }

            List<GuestEventVM> bookings = guest.Bookings.Select(e => new GuestEventVM
            {
                EventId = e.Event.Id,
                EventName = e.Event.Title,
                Attended = e.Attended
            }).ToList();

            var guestVM = new GuestDetailsVM
            {
                Id = guest.Id,
                Name = guest.Name,
                Street = guest.Street,
                Town = guest.Town,
                Postcode = guest.Postcode,
                Telephone = guest.Telephone,
                Email = guest.Email,
                BookedEvents = bookings
            };

            return View(guestVM);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("DeleteGuest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var guest = await _context.Guests
                    .Include(b => b.Bookings)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (guest == null)
                {
                    return NotFound();
                }

                guest.Name = "#####";
                guest.Street = "#####";
                guest.Town = "#####";
                guest.Postcode = "#####";
                guest.Telephone = "#####";
                guest.Email = "#####";
                guest.IsDeleted = true;

                _context.GuestBookings.RemoveRange(guest.Bookings);
                _context.Guests.Update(guest);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(ListGuests));
        }

        private bool GuestExists(int id)
        {
            return _context.Guests.Any(g => g.Id == id);
        }

        public async Task<List<Guest>> GetAvailableGuests(string id)
        {
            return await _context.Guests
                .Where(g => !g.IsDeleted && !g.Bookings.Any(e => e.EventBookingId == id))
                .ToListAsync();
        }

        public async Task UpdateGuestBooking(int[] selectedGuests, string eventId)
        {
            List<GuestBooking> eventGuests = selectedGuests.Select(g => new GuestBooking
            {
                GuestId = g,
                EventBookingId = eventId,
            }).ToList();

            _context.GuestBookings.RemoveRange(_context.GuestBookings.Where(g => g.EventBookingId == eventId));

            await _context.GuestBookings.AddRangeAsync(eventGuests);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Guest>> GetAttendingGuests(string eventId)
        {
            return await _context.Guests
                .Where(g => !g.IsDeleted && g.Bookings.Any(e => e.EventBookingId == eventId))
                .ToListAsync();
        }
    }
}
