namespace Restaurant_Chain_Management.DTOs
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }

        //public Guid Id { get; set; }
        public string GlobalCode { get; set; }
        public string Name { get; set; }
        public string Des { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Images { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsCart { get; set; }
        public int CartQuantity { get; set; }
    }
}
