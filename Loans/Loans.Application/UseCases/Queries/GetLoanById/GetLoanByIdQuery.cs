using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Queries.GetLoanById
{
    public sealed record GetLoanByIdQuery(Guid Id) : IRequest<LoanDTO>;
}
