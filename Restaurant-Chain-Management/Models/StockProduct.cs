namespace Restaurant_Chain_Management.Models
{
    public class StockProduct
    {
        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
