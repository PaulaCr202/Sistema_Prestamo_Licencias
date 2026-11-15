using Loans.Application.Contracts.External;
using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Persistence.ExternalServices;
using Loans.Persistence.Repositories;
using Loans.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Persistence.Extensions;

public static class PersistenceServicesRegistry
{
    public static IServiceCollection AddPersistenceServices(
    this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(o => o.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
        services.AddScoped<ILoanRepository, LoanRepository>();

        var catalogBaseUrl = configuration["Services:Catalog:BaseUrl"]
        ?? throw new InvalidOperationException("Falta la configuración Services:Catalog:BaseUrl.");

        services.AddHttpClient<ICatalogClient, CatalogClient>(client =>
        {
            client.BaseAddress = new Uri(catalogBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        return services;
    }

}

