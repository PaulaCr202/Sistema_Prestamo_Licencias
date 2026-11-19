using Loans.Application.Utilities.Mediator;
using Loans.Application.Utilities.Pagination;

namespace Loans.Application.UseCases.Loans.Queries.GetMyLoans
{
    public record GetMyLoansQuery(Guid UserId, PaginationRequest Pagination) : IRequest<PaginationResponse<LoanDTO>>;
}