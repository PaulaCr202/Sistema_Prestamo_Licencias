using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Queries.GetMyLoans
{
    public sealed record GetMyLoansQuery(Guid UserId) : IRequest<List<LoanDTO>>;
}
