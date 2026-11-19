using Loans.Application.Contracts.Repositories;
using Loans.Application.Exceptions;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Queries.GetLoanById
{
    public class GetLoanByIdUseCase : IRequestHandler<GetLoanByIdQuery, LoanDTO>
    {
        private readonly ILoanRepository _repository;

        public GetLoanByIdUseCase(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<LoanDTO> Handle(GetLoanByIdQuery request)
        {
            var loan = await _repository.GetByIdAsync(request.LoanId)
                ?? throw new NotFoundException("El préstamo no existe.");

            if (loan.UserId != request.UserId && !request.IsAdmin)
            {
                throw new ForbiddenException("No puedes ver un préstamo de otro usuario.");
            }

            return loan.ToDto(DateTime.UtcNow);
        }
    }
}