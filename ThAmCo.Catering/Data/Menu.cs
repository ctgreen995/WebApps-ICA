using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Catering.Data
{
    public class Menu
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<FoodBooking> FoodBookings { get; set; }

        public List<MenuFoodItem> MenuFoodItems { get; set; }
    }
}
