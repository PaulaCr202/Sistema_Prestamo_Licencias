using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LicenseEntity = Catalog.Domain.Entities.License.License;

namespace Catalog.Persistence.Configurations;

public class LicenseConfiguration : IEntityTypeConfiguration<LicenseEntity>
{
    public void Configure(EntityTypeBuilder<LicenseEntity> builder)
    {
        builder.ToTable("Licenses");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Property(l => l.IsAvailable).IsRequired();

        builder.HasOne(l => l.Software)
               .WithMany()
               .HasForeignKey(l => l.SoftwareId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => new { l.SoftwareId, l.IsAvailable });
    }
}