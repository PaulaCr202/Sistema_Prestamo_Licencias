using Loans.Application.Contracts.Repositories;
using Loans.Application.Exceptions;
using Loans.Application.Utilities.Mediator;
using Loans.Domain.Entities.Loan;

namespace Loans.Application.UseCases.Loans.Queries.GetLoanById
{
    public sealed class GetLoanByIdUseCase : IRequestHandler<GetLoanByIdQuery, LoanDTO>
    {
        private readonly ILoanRepository _repository;

        public GetLoanByIdUseCase(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<LoanDTO> Handle(GetLoanByIdQuery request)
        {
            Loan loan = await _repository.GetByIdAsync(request.Id)
                        ?? throw new NotFoundException("El préstamo no existe.");

            return loan.ToDTO(DateTime.UtcNow);
        }
    }
}
