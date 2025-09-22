using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class OfferConfig : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(o => o.Des)
                   .HasMaxLength(500);

            builder.Property(o => o.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.StartDate)
                   .IsRequired();

            builder.Property(o => o.EndDate)
                   .IsRequired();

            // علاقة الصور
            builder.HasMany(o => o.Images)
                   .WithOne(io => io.Offer)
                   .HasForeignKey(io => io.OfferId)
                   .OnDelete(DeleteBehavior.Cascade);

            // علاقة العروض بالفروع
            builder.HasMany(o => o.OfferStocks)
                   .WithOne(os => os.Offer)
                   .HasForeignKey(os => os.OfferId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

