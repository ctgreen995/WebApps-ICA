using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models.VenuesDTOs
{
    public class AvailabilityDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        public double CostPerHour { get; set; }
    }
}
