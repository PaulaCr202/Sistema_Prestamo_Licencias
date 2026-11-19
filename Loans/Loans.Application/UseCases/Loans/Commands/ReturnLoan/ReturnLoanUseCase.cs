using Loans.Application.Contracts.External;
using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Application.Exceptions;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Commands.ReturnLoan
{
    public class ReturnLoanUseCase : IRequestHandler<ReturnLoanCommand>
    {
        private readonly ILoanRepository _repository;
        private readonly ICatalogClient _catalogClient;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnLoanUseCase(ILoanRepository repository, ICatalogClient catalogClient, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _catalogClient = catalogClient;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ReturnLoanCommand request)
        {
            var loan = await _repository.GetByIdAsync(request.LoanId)
                ?? throw new NotFoundException("El préstamo no existe.");

            if (loan.UserId != request.UserId && !request.IsAdmin)
            {
                throw new ForbiddenException("No puedes devolver un préstamo de otro usuario.");
            }

            loan.Return();

            
            await _catalogClient.ReleaseLicenseAsync(loan.LicenseId);

            await _repository.UpdateAsync(loan);
            await _unitOfWork.CommitAsync();
        }
    }
}