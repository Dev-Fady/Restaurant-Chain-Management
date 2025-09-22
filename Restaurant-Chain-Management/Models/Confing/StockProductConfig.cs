using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class StockProductConfig : IEntityTypeConfiguration<StockProduct>
    {
        public void Configure(EntityTypeBuilder<StockProduct> builder)
        {
            builder.HasKey(sp => new { sp.StockId, sp.ProductId }); // Composite Key

            builder.Property(sp => sp.Quantity)
                   .IsRequired();

            builder.HasOne(sp => sp.Stock)
                   .WithMany(s => s.StockProducts)
                   .HasForeignKey(sp => sp.StockId);

            builder.HasOne(sp => sp.Product)
                   .WithMany(p => p.StockProducts)
                   .HasForeignKey(sp => sp.ProductId);
        }
    }
}

