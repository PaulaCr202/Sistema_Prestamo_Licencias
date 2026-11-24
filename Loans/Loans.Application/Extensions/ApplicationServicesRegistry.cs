using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Loans.Application.UseCases.Commands.CreateLoan;
using Loans.Application.UseCases.Commands.ReturnLoan;
using Loans.Application.UseCases.Commands.ExtendLoan;

namespace Loans.Application.Extensions
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMediator, SimpleMediator>();
            services.AddScoped<IRequestHandler<CreateLoanCommand, CreateLoanResponse>, CreateLoanUseCase>();
            services.AddScoped<IRequestHandler<ReturnLoanCommand>, ReturnLoanUseCase>();
            services.AddScoped<IRequestHandler<ExtendLoanCommand, ExtendLoanResponse>, ExtendLoanUseCase>();

            return services;
        }
    }
}
