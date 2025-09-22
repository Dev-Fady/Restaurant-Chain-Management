namespace Restaurant_Chain_Management.Models
{
    public class FavoriteProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation Property
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
