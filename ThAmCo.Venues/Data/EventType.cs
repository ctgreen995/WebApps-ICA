using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Venues.Data
{
    public class EventType
    {

        [MinLength(3), MaxLength(3)]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        public List<Suitability> SuitableVenues { get; set; }
    }
}
