using Microsoft.AspNetCore.Identity;

namespace KaedyRestaurant.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
