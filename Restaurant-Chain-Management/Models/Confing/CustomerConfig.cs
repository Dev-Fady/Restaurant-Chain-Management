using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Address)
                   .HasMaxLength(50);

            builder.Property(c => c.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.Email)
                   .HasMaxLength(100);

            builder.HasOne(c => c.Branch)
                   .WithMany(b => b.Customers)
                   .HasForeignKey(c => c.BranchId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

