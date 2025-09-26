namespace Restaurant_Chain_Management.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Des { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public int Quantity { get; set; }

        //public bool IsAvailable { get; set; }

        public List<IFormFile> Images { get; set; }

    }
}
