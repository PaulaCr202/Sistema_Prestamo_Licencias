using Microsoft.EntityFrameworkCore;
using Vendors.Domain.Entities.Contracts;
using Vendors.Domain.Entities.Vendors;

namespace Vendors.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Contract> Contracts => Set<Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
}