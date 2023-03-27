using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Catering.Data
{
    public class FoodItem
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double UnitPrice { get; set; }

        public List<MenuFoodItem> Menus { get; set; }
    }
}
