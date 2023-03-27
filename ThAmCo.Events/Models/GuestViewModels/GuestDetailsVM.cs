namespace ThAmCo.Events.Models.GuestViewModels
{
    public class GuestDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public List<GuestEventVM> BookedEvents { get; set; }
    }
}