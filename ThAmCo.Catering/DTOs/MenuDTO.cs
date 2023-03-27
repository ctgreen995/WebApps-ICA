namespace ThAmCo.Catering.DTOs
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public List<FoodItemDTO> FoodItems { get; set; }
    }
}