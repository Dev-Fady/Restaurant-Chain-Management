using Restaurant_Chain_Management.Models.Enums;

namespace Restaurant_Chain_Management.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public EmployeeRole Role { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
    }
}
