using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Loans.Application.Extensions
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMediator, SimpleMediator>();

            return services;
        }
    }
}
