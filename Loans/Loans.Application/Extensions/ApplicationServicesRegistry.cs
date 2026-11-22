using Loans.Application.UseCases.Loans.Commands.ExtendLoan;
using Loans.Application.UseCases.Loans.Commands.RequestLoan;
using Loans.Application.UseCases.Loans.Commands.ReturnLoan;
using Loans.Application.UseCases.Loans.Queries;
using Loans.Application.UseCases.Loans.Queries.GetLoanById;
using Loans.Application.UseCases.Loans.Queries.GetLoansList;
using Loans.Application.UseCases.Loans.Queries.GetMyLoans;
using Loans.Application.Utilities.Mediator;
using Loans.Application.Utilities.Pagination;
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

            // Consultas
            services.AddScoped<IRequestHandler<GetMyLoansQuery, PaginationResponse<LoanDTO>>, GetMyLoansUseCase>();
            services.AddScoped<IRequestHandler<GetLoanByIdQuery, LoanDTO>, GetLoanByIdUseCase>();
            services.AddScoped<IRequestHandler<GetLoansListQuery, PaginationResponse<LoanDTO>>, GetLoansListUseCase>();

            return services;
        }
    }
}