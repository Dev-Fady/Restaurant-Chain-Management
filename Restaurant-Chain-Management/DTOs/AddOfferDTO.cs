namespace Restaurant_Chain_Management.DTOs
{
    public class AddOfferDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // صور العرض
        public List<IFormFile> Images { get; set; }

        // هنا هنستقبل الـ JSON كـ string
        public OfferStockDto OfferStocksJson { get; set; }
    }
}
