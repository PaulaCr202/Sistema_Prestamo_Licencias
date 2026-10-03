using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Contracts.Repositories;
using Catalog.Persistence.Repositories;
using Catalog.Persistence.UnitOfWorks;

namespace Catalog.Persistence.Extensions;

public static class PersistenceServicesRegistry
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DataContext>(o => o.UseSqlServer(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISoftwareRepository, SoftwareRepository>();
        services.AddScoped<ILicenseRepository, LicenseRepository>();
        services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();

        return services;
    }
}