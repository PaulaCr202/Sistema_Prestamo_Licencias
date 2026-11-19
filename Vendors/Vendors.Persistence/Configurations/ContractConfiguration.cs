using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendors.Domain.Entities.Contracts;

namespace Vendors.Persistence.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.ContractNumber).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Cost).HasPrecision(18, 2).IsRequired();
        builder.Property(c => c.StartDate).IsRequired();
        builder.Property(c => c.EndDate).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();

        builder.HasOne(c => c.Vendor)
               .WithMany()
               .HasForeignKey(c => c.VendorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.VendorId);
        builder.HasIndex(c => c.EndDate);   // acelera la consulta de contratos por vencer
    }
}