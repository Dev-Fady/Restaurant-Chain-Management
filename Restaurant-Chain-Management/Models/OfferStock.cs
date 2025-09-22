namespace Restaurant_Chain_Management.Models
{
    public class OfferStock
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int Quantity { get; set; }
    }
}
