using Restaurant_Chain_Management.Models.Enums;

namespace Restaurant_Chain_Management.DTOs
{
    public class ManagerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public string RoleName => RoleId.HasValue
       ? Enum.GetName(typeof(EmployeeRole), RoleId.Value)
       : "Unknown";
    }
}
