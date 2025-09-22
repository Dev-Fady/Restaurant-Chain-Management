namespace Restaurant_Chain_Management.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Des { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }   // بداية العرض
        public DateTime EndDate { get; set; }     // نهاية العرض

        public ICollection<ImageOffer> Images { get; set; } = new List<ImageOffer>();
        public ICollection<OfferStock> OfferStocks { get; set; } = new List<OfferStock>();
    }
}
