using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

using Vendors.Application.UseCases.Vendors.Commands.CreateVendor;
using Vendors.Application.UseCases.Vendors.Queries.GetVendors;

using Vendors.Application.UseCases.Contracts.Commands.CreateContract;
using Vendors.Application.UseCases.Contracts.Queries.GetContracts;
using Vendors.Application.UseCases.Contracts.Queries.GetExpiringContracts;

using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediator, SimpleMediator>();

            // Vendor Use Cases
            services.AddScoped<
                IRequestHandler<CreateVendorCommand, Result<Guid>>,
                CreateVendorUseCase>();

            services.AddScoped<
                IRequestHandler<GetVendorsQuery,
                    Result<List<VendorListItemDTO>>>,
                GetVendorsUseCase>();

            // Contract Use Cases
            services.AddScoped<
                IRequestHandler<CreateContractCommand, Result<Guid>>,
                CreateContractUseCase>();

            services.AddScoped<
                IRequestHandler<GetContractsQuery,
                    Result<List<ContractListItemDTO>>>,
                GetContractsUseCase>();

            services.AddScoped<
                IRequestHandler<GetExpiringContractsQuery,
                    Result<List<ContractListItemDTO>>>,
                GetExpiringContractsUseCase>();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<
                CreateVendorCommandValidator>();

            return services;
        }
    }
}
