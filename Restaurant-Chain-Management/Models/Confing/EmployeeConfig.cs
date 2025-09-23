using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant_Chain_Management.Models.Confing
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Salary)
                 .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Role)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(e => e.Role);
            builder.HasIndex(e => e.BranchId);

        }
    }
}

