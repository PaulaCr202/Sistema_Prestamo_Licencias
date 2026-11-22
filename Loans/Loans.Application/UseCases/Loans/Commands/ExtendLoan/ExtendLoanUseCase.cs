using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Application.Exceptions;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Commands.ExtendLoan
{
    public class ExtendLoanUseCase : IRequestHandler<ExtendLoanCommand>
    {
        private readonly ILoanRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ExtendLoanUseCase(ILoanRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ExtendLoanCommand request)
        {
            var loan = await _repository.GetByIdAsync(request.LoanId)
                ?? throw new NotFoundException("El préstamo no existe.");

            if (loan.UserId != request.UserId && !request.IsAdmin)
            {
                throw new ForbiddenException("No puedes extender un préstamo de otro usuario.");
            }

            loan.Extend(request.ExtraDays);

            await _repository.UpdateAsync(loan);
            await _unitOfWork.CommitAsync();
        }
    }
}