using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Persistence.Configurations;

public class SoftwareConfiguration : IEntityTypeConfiguration<SoftwareEntity>
{
    public void Configure(EntityTypeBuilder<SoftwareEntity> builder)
    {
        builder.ToTable("Softwares");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.OwnsOne(s => s.Nombre, n =>
            n.Property(x => x.Valor).HasColumnName("Nombre").HasMaxLength(100).IsRequired());

        builder.HasOne(s => s.Category)
               .WithMany()
               .HasForeignKey(s => s.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}