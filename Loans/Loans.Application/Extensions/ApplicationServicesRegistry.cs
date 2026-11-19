using Loans.Application.UseCases.Loans.Commands.ExtendLoan;
using Loans.Application.UseCases.Loans.Commands.RequestLoan;
using Loans.Application.UseCases.Loans.Commands.ReturnLoan;
using Loans.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Loans.Application.Extensions
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMediator, SimpleMediator>();

            // Comandos
            services.AddScoped<IRequestHandler<RequestLoanCommand, Guid>, RequestLoanUseCase>();
            services.AddScoped<IRequestHandler<ReturnLoanCommand>, ReturnLoanUseCase>();
            services.AddScoped<IRequestHandler<ExtendLoanCommand>, ExtendLoanUseCase>();

            return services;
        }
    }
}