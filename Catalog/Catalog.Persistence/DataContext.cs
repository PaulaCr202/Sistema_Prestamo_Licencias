using Microsoft.EntityFrameworkCore;
using Catalog.Domain.Entities.Categories;
using LicenseEntity = Catalog.Domain.Entities.License.License;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SoftwareEntity> Softwares => Set<SoftwareEntity>();
    public DbSet<LicenseEntity> Licenses => Set<LicenseEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
}