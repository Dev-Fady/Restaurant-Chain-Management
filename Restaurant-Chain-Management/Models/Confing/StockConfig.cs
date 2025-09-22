using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class StockConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(s => s.Branch)
               .WithOne(b => b.Stock)
               .HasForeignKey<Stock>(s => s.BranchId);

            //builder.HasMany(s => s.Products)
            //       .WithOne(p => p.Stock)
            //       .HasForeignKey(p => p.StockId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

