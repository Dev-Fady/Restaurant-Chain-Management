namespace Restaurant_Chain_Management.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Des { get; set; }
        public decimal Price { get; set; }
        public bool IsFavorite { get; set; }

        //public int StockId { get; set; }
        //public Stock Stock { get; set; }

        public ICollection<ImageProduct> ImageProducts { get; set; } = new List<ImageProduct>();

        public ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
        public ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();

    }
}
