using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(b => b.City)
                     .WithMany()
                     .HasForeignKey(b => b.CityId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(b => b.Employees)
                .WithOne(e => e.Branch)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(b => b.Stock)
               .WithOne(s => s.Branch)
               .HasForeignKey<Stock>(s => s.BranchId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(b => b.CityId);
            builder.HasIndex(b => b.ManagerId);
            builder.HasIndex(b => new { b.CityId, b.Name }).IsUnique();

        }
    }
}
