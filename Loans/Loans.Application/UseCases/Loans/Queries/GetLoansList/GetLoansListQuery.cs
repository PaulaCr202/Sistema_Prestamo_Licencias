using Loans.Application.Utilities.Mediator;
using Loans.Application.Utilities.Pagination;

namespace Loans.Application.UseCases.Loans.Queries.GetLoansList
{
    public record GetLoansListQuery(PaginationRequest Pagination, bool OnlyOverdue) : IRequest<PaginationResponse<LoanDTO>>;
}