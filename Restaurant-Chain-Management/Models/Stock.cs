namespace Restaurant_Chain_Management.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public String Name { get; set; }
        
        //public int Quantity { get; set; }
        //public int ProductId { get; set; }
        //public Product Product { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();

    }
}
