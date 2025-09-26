using Restaurant_Chain_Management.Models.Enums;

namespace Restaurant_Chain_Management.DTOs
{
    public class RegisterCustomerDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public String PhoneNumber { get; set; }
        public int BranchId { get; set; }
    }
}
