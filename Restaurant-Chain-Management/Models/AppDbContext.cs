using Microsoft.EntityFrameworkCore;
using Restaurant_Chain_Management.Models.Confing;

namespace Restaurant_Chain_Management.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext() : base()
        { 
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
             typeof(CityConfig).Assembly);
        }
    }
}
