using Loans.Application.Contracts.Repositories;
using Loans.Application.Utilities.Mediator;
using Loans.Domain.Entities.Loan;

namespace Loans.Application.UseCases.Loans.Queries.GetMyLoans
{
    public sealed class GetMyLoansUseCase : IRequestHandler<GetMyLoansQuery, List<LoanDTO>>
    {
        private readonly ILoanRepository _repository;

        public GetMyLoansUseCase(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LoanDTO>> Handle(GetMyLoansQuery request)
        {
            IEnumerable<Loan> loans = await _repository.GetByUserAsync(request.UserId);
            DateTime now = DateTime.UtcNow;

            return loans.OrderByDescending(l => l.StartDate)
                        .Select(l => l.ToDTO(now))
                        .ToList();
        }
    }
}
