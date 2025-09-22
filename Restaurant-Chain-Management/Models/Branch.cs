namespace Restaurant_Chain_Management.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
