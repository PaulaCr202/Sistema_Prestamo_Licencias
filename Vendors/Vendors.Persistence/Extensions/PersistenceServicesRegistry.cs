using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vendors.Application.Contracts.Persistence;
using Vendors.Application.Contracts.Repositories;
using Vendors.Persistence.Repositories;
using Vendors.Persistence.UnitOfWorks;

namespace Vendors.Persistence.Extensions;

public static class PersistenceServicesRegistry
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DataContext>(o => o.UseSqlServer(connectionString));

        services.AddScoped<IVendorsRepository, VendorsRepository>();
        services.AddScoped<IContractsRepository, ContractsRepository>();
        services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();

        return services;
    }
}