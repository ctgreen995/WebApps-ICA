using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using ThAmCo.Events.Models.CateringDTOs;
using ThAmCo.Events.Models.CateringViewModels;

namespace ThAmCo.Events.Services.Catering
{
    public class CateringController : Controller
    {
        private readonly HttpClient _client;

        public CateringController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7173/");
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        #region Food Bookings

        public async Task<IActionResult> FoodBookings()
        {
            HttpResponseMessage response = await _client.GetAsync("api/FoodBookings");
            if (response.IsSuccessStatusCode)
            {
                var foodBookings = await response.Content.ReadAsAsync<IEnumerable<FoodBookingDTO>>();

                return View(foodBookings);
            }
            else
            {
                Debug.WriteLine("Failed to retrieve valid response from the catering service.");
            }
            return View();
        }

        public async Task<FoodBookingDTO> GetFoodBooking(string clientReferenceId)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/FoodBookings/{clientReferenceId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<FoodBookingDTO>();
            }
            return null;
        }

        public async Task<FoodBookingDTO> CreateFoodBooking(FoodBookingDTO foodBookingDTO)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Foodbookings", foodBookingDTO);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<FoodBookingDTO>();
            }
            return null;
        }

        public async Task<IActionResult> FoodBookingDetails(string id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/FoodBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                var foodBooking = await response.Content.ReadAsAsync<FoodBookingDTO>();
                return View(foodBooking);
            }
            else
            {
                Debug.WriteLine("Failed to retrieve valid response from the caatering service.");
            }
            return View();
        }

        public async Task<HttpResponseMessage> DeleteFoodBooking(string clientReferenceId)
        {
            return await _client.DeleteAsync($"api/FoodBookings/{clientReferenceId}");
        }

        #endregion

        #region Food Items

        [HttpGet]
        public async Task<IActionResult> ListFoodItems()
        {
            HttpResponseMessage response = await _client.GetAsync("api/FoodItems");
            if (response.IsSuccessStatusCode)
            {
                var foodItems = await response.Content.ReadAsAsync<IEnumerable<FoodItemDTO>>();

                return View(foodItems);
            }
            else
            {
                Debug.WriteLine("Failed to retrieve valid response from the caatering service.");
            }
            return View();
        }

        private async Task<List<FoodItemDTO>> GetFoodItems()
        {
            HttpResponseMessage response = await _client.GetAsync("api/FoodItems");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<FoodItemDTO>>();
            }
            return null;
        }

        public IActionResult CreateFoodItem()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFoodItem(FoodItemDTO foodItemToAdd)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage request = await _client.PostAsJsonAsync("api/FoodItems", foodItemToAdd);
                return RedirectToAction(nameof(ListFoodItems));
            }
            else
            {
                Debug.WriteLine("Failed to create Food Item");
                return View(foodItemToAdd);
            }
        }

        public async Task<IActionResult> FoodItemDetails(int? id)
        {
            FoodItemDTO foodItem = await GetFoodItem(id);

            if (foodItem != null)
            {
                return View(foodItem);
            }
            return NotFound();
        }

        private async Task<FoodItemDTO> GetFoodItem(int? id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/FoodItems/{id}");
            if (response.IsSuccessStatusCode)
            {
                var foodItem = await response.Content.ReadAsAsync<FoodItemDTO>();

                return foodItem;
            }
            else
            {
                Debug.WriteLine("Failed to retrieve valid response from the caatering service.");
            }
            return null;
        }


        public async Task<IActionResult> EditFoodItem(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            FoodItemDTO foodItem = await GetFoodItem(id);

            if (foodItem != null)
            {
                return View(foodItem);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFoodItem(int id, FoodItemDTO foodItemDTO)
        {
            if (ModelState.IsValid)
            {

                HttpResponseMessage request = await _client.PutAsJsonAsync($"api/FoodItems/{id}", foodItemDTO);
                if (request.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(ListFoodItems));
                }
            }
            return View(foodItemDTO);
        }


        public async Task<IActionResult> DeleteFoodItem(string id)
        {
            HttpResponseMessage requst = await _client.DeleteAsync($"api/FoodItems/{id}");

            return RedirectToAction(nameof(ListFoodItems));
        }

        #endregion

        #region Menus

        [HttpGet]
        public async Task<IActionResult> ListMenus()
        {
            List<MenuDTO> menus = await GetAvailableMenus();

            if (menus != null)
            {
                return View(menus);
            }
            else
            {
                return BadRequest("Failed to receive a valid response from the Catering Service.");
            }
        }

        public async Task<List<MenuDTO>> GetAvailableMenus()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Menus");
            if (response.IsSuccessStatusCode)
            {
                var menus = await response.Content.ReadAsAsync<List<MenuDTO>>();

                return menus;
            }
            else
            {
                return null;
            }
        }

        private async Task<MenuDTO> GetMenu(int? id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Menus/{id}");

            if (response.IsSuccessStatusCode)
            {
                var menu = await response.Content.ReadAsAsync<MenuDTO>();

                return menu;
            }
            else
            {
                Debug.WriteLine("Failed to receive a valid response form the Catering Service.");
            }

            return null;
        }

        public async Task<IActionResult> MenuDetails(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MenuDTO menuDTO = await GetMenu(id);

            List<SelectListItem> menuFoodItemList = menuDTO.FoodItems.Select(f => new SelectListItem
            {
                Text = f.Description + ": £" + f.Price,
                Value = f.Id.ToString()
            }).ToList();

            var menuVM = new MenuDetailsVM
            {
                MenuID = menuDTO.Id,
                MenuName = menuDTO.MenuName,
                MenuFoodItems = menuFoodItemList,
            };

            if (menuDTO != null)
            {
                return View(menuVM);
            }
            return NotFound();
        }

        public async Task<IActionResult> CreateMenu()
        {
            var menuFoodItemList = new List<SelectListItem>();

            var foodItemDTOs = await GetFoodItems();

            List<SelectListItem> foodItemList = foodItemDTOs.Select(f => new SelectListItem
            {
                Text = f.Description + ": £" + f.Price,
                Value = f.Id.ToString()
            }).ToList();

            var menuVM = new CreateMenuVM
            {
                MenuFoodItems = menuFoodItemList,
                FoodItems = foodItemList
            };
            return View(menuVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu(CreateMenuVM menuVM, int[] menuFoodItems)
        {
            if (ModelState.IsValid)
            {
                List<FoodItemDTO> foodItems = menuFoodItems.Select(f => new FoodItemDTO
                {
                    Id = f
                }).ToList();

                var menuDTO = new MenuDTO
                {
                    MenuName = menuVM.MenuName,
                    FoodItems = foodItems
                };

                HttpResponseMessage request = await _client.PostAsJsonAsync($"api/Menus", menuDTO);
                if (request.IsSuccessStatusCode)
                {
                    return Ok();
                }
            }
            return RedirectToAction("EditMenu", "Catering");
        }

        public async Task<IActionResult> EditMenu(int id)
        {
            MenuDTO menuDTO = await GetMenu(id);
            List<SelectListItem> menuFoodItemList = SetMenuFoodItemList(menuDTO);

            List<SelectListItem> foodItemList = await SetFoodItemList(menuDTO);

            var menuVM = new EditMenuVM
            {
                MenuID = menuDTO.Id,
                MenuName = menuDTO.MenuName,
                MenuFoodItems = menuFoodItemList,
                FoodItems = foodItemList
            };

            if (menuDTO != null)
            {
                return View(menuVM);
            }
            return NotFound();
        }

        private async Task<List<SelectListItem>> SetFoodItemList(MenuDTO menuDTO)
        {
            var foodItemDTOs = await GetFoodItems();

            return foodItemDTOs
                .Where(f1 => !menuDTO.FoodItems.Any(f2 => f2.Id == f1.Id))
                .Select(f => new SelectListItem
                {
                    Text = f.Description + ": £" + f.Price,
                    Value = f.Id.ToString()
                }).ToList();
        }

        private List<SelectListItem> SetMenuFoodItemList(MenuDTO menuDTO)
        {
            return menuDTO.FoodItems.Select(f => new SelectListItem
            {
                Text = f.Description + ": £" + f.Price,
                Value = f.Id.ToString()
            }).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> EditMenu(EditMenuVM menuVM, int[] menuFoodItems)
        {
            if (ModelState.IsValid)
            {
                List<FoodItemDTO> foodItems = menuFoodItems.Select(f => new FoodItemDTO
                {
                    Id = f
                }).ToList();

                var menuDTO = new MenuDTO
                {
                    Id = menuVM.MenuID,
                    MenuName = menuVM.MenuName,
                    FoodItems = foodItems
                };

                HttpResponseMessage request = await _client.PutAsJsonAsync($"api/Menus/{menuVM.MenuID}", menuDTO);
                if (request.IsSuccessStatusCode)
                {
                    return Ok();
                }
            }
            return RedirectToAction("EditMenu", "Catering");
        }

        public async Task<IActionResult> DeleteMenu(int id)
        {
            HttpResponseMessage requst = await _client.DeleteAsync($"api/Menus/{id}");
            return RedirectToAction(nameof(ListMenus));
        }

        #endregion
    }
}
