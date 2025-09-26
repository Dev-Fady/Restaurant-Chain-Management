using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
           
            builder.Property(x => x.GlobalCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => x.GlobalCode)
                   .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
           
            builder.Property(x => x.Des)
                .HasMaxLength(500);
           
            //builder.Property(x => x.IsFavorite)
            //    .IsRequired()
            //    .HasDefaultValue(false);

            // Images relationship
            builder.HasMany(p => p.ImageProducts)
                .WithOne(ip => ip.Product)
                .HasForeignKey(ip => ip.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

