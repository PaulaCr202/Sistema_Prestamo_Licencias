using Loans.Application.UseCases.Commands.CreateLoan;
using Loans.Application.UseCases.Commands.ExtendLoan;
using Loans.Application.UseCases.Commands.ReturnLoan;
using Loans.Application.UseCases.Loans.Queries;
using Loans.Application.UseCases.Loans.Queries.GetAllLoans;
using Loans.Application.UseCases.Loans.Queries.GetLoanById;
using Loans.Application.UseCases.Loans.Queries.GetMyLoans;
using Loans.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
            services.AddScoped<IRequestHandler<GetMyLoansQuery, List<LoanDTO>>, GetMyLoansUseCase>();
            services.AddScoped<IRequestHandler<GetAllLoansQuery, List<LoanDTO>>, GetAllLoansUseCase>();
            services.AddScoped<IRequestHandler<GetLoanByIdQuery, LoanDTO>, GetLoanByIdUseCase>();

            return services;
        }
    }
}
