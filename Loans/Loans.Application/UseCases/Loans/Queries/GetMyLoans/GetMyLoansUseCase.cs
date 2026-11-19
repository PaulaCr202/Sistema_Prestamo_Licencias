using Loans.Application.Contracts.Repositories;
using Loans.Application.Utilities.Mediator;
using Loans.Application.Utilities.Pagination;
using LoanEntity = Loans.Domain.Entities.Loan.Loan;

namespace Loans.Application.UseCases.Loans.Queries.GetMyLoans
{
    public class GetMyLoansUseCase : IRequestHandler<GetMyLoansQuery, PaginationResponse<LoanDTO>>
    {
        private readonly ILoanRepository _repository;

        public GetMyLoansUseCase(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<LoanDTO>> Handle(GetMyLoansQuery request)
        {
            IEnumerable<LoanEntity> loans = await _repository.GetByUserAsync(request.UserId);

            // Los más recientes primero; un orden fijo evita que las páginas repitan o salten elementos
            List<LoanEntity> ordered = loans
                .OrderByDescending(l => l.StartDate)
                .ToList();

            PaginationRequest pagination = request.Pagination;
            DateTime now = DateTime.UtcNow;

            List<LoanDTO> items = ordered
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(l => l.ToDto(now))
                .ToList();

            return PaginationResponse<LoanDTO>.Create(items, ordered.Count, pagination);
        }
    }
}