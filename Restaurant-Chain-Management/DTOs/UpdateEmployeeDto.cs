using Restaurant_Chain_Management.Models.Enums;

namespace Restaurant_Chain_Management.DTOs
{
    public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? BranchId { get; set; }
        public decimal? Salary { get; set; }
        public EmployeeRole Role { get; set; }
    }

}
