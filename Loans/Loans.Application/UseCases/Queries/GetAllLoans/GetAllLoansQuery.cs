using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Queries.GetAllLoans
{
    public sealed record GetAllLoansQuery : IRequest<List<LoanDTO>>;
}
