using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTOs;

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public FoodItemsController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDTO>>> GetFoodItems()
        {
            try
            {
                var foodItems = await _context.FoodItems.ToListAsync();

                List<FoodItemDTO> foodItemDTOs = foodItems.Select(fi => new FoodItemDTO
                {
                    Id = fi.Id,
                    Description = fi.Description,
                    Price = fi.UnitPrice
                }).ToList();

                return Ok(foodItemDTOs);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to retrieve Food Items from the database." + e.Message);
                return NotFound();
            }
        }

        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDTO>> GetFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }
            return new FoodItemDTO
            {
                Id = foodItem.Id,
                Description = foodItem.Description,
                Price = foodItem.UnitPrice
            };
        }

        // PUT: api/FoodItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Description, UnitPrice")] FoodItemDTO foodItemDTO)
        {
            if (id != foodItemDTO.Id)
            {
                return BadRequest();
            }
            try
            {
                var foodItem = await _context.FoodItems.FindAsync(foodItemDTO.Id);

                foodItem.Description = foodItemDTO.Description;
                foodItem.UnitPrice = foodItemDTO.Price;

                _context.Entry(foodItem).State = EntityState.Modified;
                _context.SaveChanges();

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/FoodItems
        [HttpPost]
        public async Task<ActionResult<FoodItem>> PostFoodItem([Bind("Description, UnitPrice")] FoodItemDTO foodItemDTO)
        {
            try
            {
                var foodItemToAdd = new FoodItem
                {
                    Description = foodItemDTO.Description,
                    UnitPrice = foodItemDTO.Price
                };

                _context.FoodItems.Add(foodItemToAdd);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFoodItem", new { id = foodItemToAdd.Id }, foodItemToAdd);

            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to add Food Item to Database. " + e);
            }
            return NoContent();
        }

        // DELETE: api/FoodItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
