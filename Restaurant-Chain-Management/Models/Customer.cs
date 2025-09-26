namespace Restaurant_Chain_Management.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
