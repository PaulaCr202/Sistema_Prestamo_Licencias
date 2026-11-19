using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendors.Domain.Entities.Vendors;

namespace Vendors.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).ValueGeneratedNever();

        builder.Property(v => v.Name).IsRequired().HasMaxLength(150);
        builder.Property(v => v.Email).IsRequired().HasMaxLength(200);
        builder.Property(v => v.Phone).IsRequired().HasMaxLength(30);
    }
}