using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class OfferStockConfig : IEntityTypeConfiguration<OfferStock>
    {
        public void Configure(EntityTypeBuilder<OfferStock> builder)
        {
            builder.HasKey(os => os.Id);

            builder.Property(os => os.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(os => os.Quantity)
                   .IsRequired();

            // علاقة بالعرض
            builder.HasOne(os => os.Offer)
                   .WithMany(o => o.OfferStocks)
                   .HasForeignKey(os => os.OfferId)
                   .OnDelete(DeleteBehavior.Cascade);

            // علاقة بالفرع
            builder.HasOne(os => os.Branch)
                   .WithMany(b => b.OfferStocks)
                   .HasForeignKey(os => os.BranchId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(os => new { os.OfferId, os.BranchId });
        }
    }
}

