using Loans.Application.Contracts.Repositories;
using Loans.Application.Utilities.Mediator;
using Loans.Domain.Entities.Loan;

namespace Loans.Application.UseCases.Loans.Queries.GetAllLoans
{
    public sealed class GetAllLoansUseCase : IRequestHandler<GetAllLoansQuery, List<LoanDTO>>
    {
        private readonly ILoanRepository _repository;

        public GetAllLoansUseCase(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LoanDTO>> Handle(GetAllLoansQuery request)
        {
            IEnumerable<Loan> loans = await _repository.GetListAsync();
            DateTime now = DateTime.UtcNow;

            return loans.OrderByDescending(l => l.StartDate)
                        .Select(l => l.ToDTO(now))
                        .ToList();
        }
    }
}
