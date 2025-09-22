using Microsoft.EntityFrameworkCore;
using Restaurant_Chain_Management.Models.Confing;

namespace Restaurant_Chain_Management.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }
       
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        public DbSet<StockProduct> StockProducts { get; set; }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferStock> OfferStocks { get; set; }
        public DbSet<ImageOffer> ImageOffers { get; set; }




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
