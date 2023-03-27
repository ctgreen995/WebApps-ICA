using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTOs;

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodBookingsController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public FoodBookingsController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodBookingDTO>>> GetFoodBookings()
        {
            try
            {
                var foodBookings = await _context.FoodBookings
                    .Include(m => m.Menu)
                    .ThenInclude(mf => mf.MenuFoodItems)
                    .ThenInclude(f => f.FoodItem)
                    .ToListAsync();

                List<FoodBookingDTO> foodBookingDTOs = foodBookings.Select(fb => new FoodBookingDTO
                {
                    ClientReferenceId = fb.ClientReferenceId,
                    FoodBookingId = fb.FoodBookingId,
                    MenuId = fb.MenuId,
                    NumberOfGuests = fb.NumberOfGuests
                }).ToList();

                return Ok(foodBookingDTOs);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



        //GET: api/FoodBooking
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodBookingDTO>> GetFoodBooking(string clientReferenceId)
        {
            if (clientReferenceId == null)
            {
               return BadRequest();
            }
            try
            {
                var booking = await _context.FoodBookings
                    .Include(m => m.Menu)
                    .ThenInclude(mfi => mfi.MenuFoodItems)
                    .ThenInclude(fi => fi.FoodItem)
                    .FirstOrDefaultAsync(b => b.ClientReferenceId == clientReferenceId);

                if (booking == null)
                {
                    return NotFound();
                }

                return new FoodBookingDTO
                {
                    FoodBookingId = booking.FoodBookingId,
                    ClientReferenceId = booking.ClientReferenceId,
                    MenuId = booking.MenuId,
                    NumberOfGuests = booking.NumberOfGuests
                };
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: api/FoodBookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodBooking(int id, FoodBooking foodBooking)
        {
            if (id != foodBooking.FoodBookingId)
            {
                return BadRequest();
            }

            _context.Entry(foodBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodBookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/FoodBookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodBooking>> PostFoodBooking(FoodBookingDTO foodBookingDTO)
        {
            var foodBooking = new FoodBooking
            {
                ClientReferenceId = foodBookingDTO.ClientReferenceId,
                MenuId = foodBookingDTO.MenuId,
                NumberOfGuests = foodBookingDTO.NumberOfGuests
            };
            _context.FoodBookings.Add(foodBooking);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodBooking", new { id = foodBooking.FoodBookingId }, foodBooking);
        }

        // DELETE: api/FoodBookings/5
        [HttpDelete("{clientReferenceId}")]
        public async Task<IActionResult> DeleteFoodBooking(string clientReferenceId)
        {
            var foodBooking = await _context.FoodBookings
                .FirstOrDefaultAsync(f => f.ClientReferenceId == clientReferenceId);

            if (foodBooking == null)
            {
                return NotFound();
            }

            _context.FoodBookings.Remove(foodBooking);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool FoodBookingExists(int id)
        {
            return _context.FoodBookings.Any(e => e.FoodBookingId == id);
        }
    }
}
