using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Chain_Management.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation Property
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;

        public int stockId { get; set; }
        public Stock stock { get; set; }
    }
}
