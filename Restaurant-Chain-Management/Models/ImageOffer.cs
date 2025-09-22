namespace Restaurant_Chain_Management.Models
{
    public class ImageOffer
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
