using Microsoft.AspNetCore.Identity;

namespace Restaurant_Chain_Management.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
