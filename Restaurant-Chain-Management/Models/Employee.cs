using Microsoft.AspNetCore.Identity;
using Restaurant_Chain_Management.Models.Enums;

namespace Restaurant_Chain_Management.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public decimal Salary { get; set; }

        public EmployeeRole Role { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        // 
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
