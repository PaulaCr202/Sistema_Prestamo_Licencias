using Loans.Application.Contracts.Persistence;
using Loans.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Loans.Persistence.Extensions;

public static class PersistenceServicesRegistry
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(o => o.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();

        // La Persona 2 agrega aquí: ILoanRepository y el HttpClient de ICatalogClient

        return services;
    }
}
