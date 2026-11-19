using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Queries.GetLoanById
{
    public record GetLoanByIdQuery(Guid LoanId, Guid UserId, bool IsAdmin) : IRequest<LoanDTO>;
}