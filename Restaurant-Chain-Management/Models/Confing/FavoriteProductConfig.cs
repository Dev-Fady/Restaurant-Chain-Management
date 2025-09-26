using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class FavoriteProductConfig : IEntityTypeConfiguration<FavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavoriteProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(fp => fp.Product)
                   .WithMany(p => p.FavoriteProducts)
                   .HasForeignKey(fp => fp.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fp => fp.User)
                    .WithMany(u => u.FavoriteProducts)
                    .HasForeignKey(fp => fp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder
         .HasIndex(f => new { f.UserId, f.ProductId, f.stockId })
         .IsUnique();

        }
    }
}