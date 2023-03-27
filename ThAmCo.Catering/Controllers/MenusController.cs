using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.DTOs;

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public MenusController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDTO>>> GetMenus()
        {
            var menus = await _context.Menus.Include(mf => mf.MenuFoodItems).ThenInclude(f => f.FoodItem).ToListAsync();

            if (menus == null)
            {
                return NotFound();
            }

            List<MenuDTO> menuDTOs = menus.Select(m => new MenuDTO
            {
                Id = m.Id,
                MenuName = m.Name,
                FoodItems = m.MenuFoodItems.Select(f => new FoodItemDTO
                {
                    Id = f.FoodItemId,
                    Description = f.FoodItem.Description,
                    Price = f.FoodItem.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(menuDTOs);

        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDTO>> GetMenu(int id)
        {
            var menu = await _context.Menus
                .Include(mf => mf.MenuFoodItems)
                .ThenInclude(fi => fi.FoodItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            List<FoodItemDTO> foodItems = menu.MenuFoodItems
                .Select(mf => mf.FoodItem)
                .Select(fi => new FoodItemDTO
            {
                Id = fi.Id,
                Price = fi.UnitPrice,
                Description = fi.Description
            }).ToList();

            var menuDTO = new MenuDTO
            {
                Id = menu.Id,
                MenuName = menu.Name,
                FoodItems = foodItems
            };

            return menuDTO;
        }

        // PUT: api/Menus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("MenuName")] MenuDTO menuDTO)
        {
            if (id != menuDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                var menu = await _context.Menus.FindAsync(menuDTO.Id);

                menu.Name = menuDTO.MenuName;

                _context.Entry(menu).State = EntityState.Modified;

                _context.MenuFoodItems.RemoveRange(_context.MenuFoodItems.Where(m => m.MenuId == menuDTO.Id));

                List<MenuFoodItem> menuFood = menuDTO.FoodItems.Select(f => new MenuFoodItem
                {
                    MenuId = menuDTO.Id,
                    FoodItemId = f.Id,
                }).ToList();

                _context.MenuFoodItems.AddRange(menuFood);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
            return Ok();
        }

        // POST: api/Menus
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(MenuDTO menuDTO)
        {
            try
            {
                var menu = new Menu
                {
                    Name = menuDTO.MenuName
                };

                _context.Menus.Add(menu);
                await _context.SaveChangesAsync();

                int id = menu.Id;

                List<MenuFoodItem> menuFood = menuDTO.FoodItems.Select(f => new MenuFoodItem
                {
                    MenuId = menuDTO.Id,
                    FoodItemId = f.Id,
                }).ToList();

                _context.MenuFoodItems.AddRange(menuFood);

                return CreatedAtAction("GetMenu", new { id = menu.Id }, menu);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to add Food Item to Database. " + e);
            }
            return NoContent();
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}
