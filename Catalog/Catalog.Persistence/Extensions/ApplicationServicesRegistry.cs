using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.UseCases.Software.Commands.CreateSoftware;
using Catalog.Application.UseCases.Software.Commands.RegisterLicenses;
using Catalog.Application.UseCases.Software.Queries;
using Catalog.Application.UseCases.Software.Queries.GetSoftwareByCategory;
using Catalog.Application.UseCases.Software.Queries.GetSoftwareById;
using Catalog.Application.UseCases.Software.Queries.GetSoftwareList;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application.Extensions;

public static class ApplicationServicesRegistry
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, SimpleMediator>();

        services.AddScoped<IRequestHandler<CreateSoftwareCommand, Guid>, CreateSoftwareUseCase>();
        services.AddScoped<IRequestHandler<RegisterLicensesCommand, List<Guid>>, RegisterLicensesUseCase>();
        services.AddScoped<IRequestHandler<GetSoftwareListQuery, PaginationResponse<SoftwareDTO>>, GetSoftwareListUseCase>();
        services.AddScoped<IRequestHandler<GetSoftwareByIdQuery, SoftwareDTO?>, GetSoftwareByIdUseCase>();
        services.AddScoped<IRequestHandler<GetSoftwareByCategoryQuery, List<SoftwareDTO>>, GetSoftwareByCategoryUseCase>();

        return services;
    }
}
