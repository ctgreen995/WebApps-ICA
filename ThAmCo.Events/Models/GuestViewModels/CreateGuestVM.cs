using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ThAmCo.Events.Models.GuestViewModels
{
    public class CreateGuestVM
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Town { get; set; }
    }
}
