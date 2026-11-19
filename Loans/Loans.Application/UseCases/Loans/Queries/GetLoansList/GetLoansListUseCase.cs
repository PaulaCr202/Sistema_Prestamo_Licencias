using Loans.Application.Contracts.Repositories;
using Loans.Application.Utilities.Mediator;
using Loans.Application.Utilities.Pagination;
using LoanEntity = Loans.Domain.Entities.Loan.Loan;

namespace Loans.Application.UseCases.Loans.Queries.GetLoansList
{
    public class GetLoansListUseCase : IRequestHandler<GetLoansListQuery, PaginationResponse<LoanDTO>>
    {
        private readonly ILoanRepository _repository;

        public GetLoansListUseCase(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<LoanDTO>> Handle(GetLoansListQuery request)
        {
            // Que solo el Admin llegue aquí lo controla el controller con [Authorize(Roles = "Admin")]
            IEnumerable<LoanEntity> loans = await _repository.GetListAsync();
            DateTime now = DateTime.UtcNow;

            if (request.OnlyOverdue)
            {
                loans = loans.Where(l => l.IsOverdue(now));
            }

            List<LoanEntity> ordered = loans
                .OrderByDescending(l => l.StartDate)
                .ToList();

            PaginationRequest pagination = request.Pagination;

            List<LoanDTO> items = ordered
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(l => l.ToDto(now))
                .ToList();

            return PaginationResponse<LoanDTO>.Create(items, ordered.Count, pagination);
        }
    }
}