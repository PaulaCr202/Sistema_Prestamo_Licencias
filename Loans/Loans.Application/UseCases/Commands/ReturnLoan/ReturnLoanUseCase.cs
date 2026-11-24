using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.External;
using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Application.Exceptions;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Commands.ReturnLoan
{
    public class ReturnLoanUseCase : IRequestHandler<ReturnLoanCommand>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ICatalogClient _catalogClient;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnLoanUseCase(
            ILoanRepository loanRepository,
            ICatalogClient catalogClient,
            IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _catalogClient = catalogClient;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ReturnLoanCommand command)
        {
            var loan = await _loanRepository.GetByIdAsync(command.LoanId)
                ?? throw new NotFoundException("El préstamo no existe.");

            if (loan.UserId != command.UserId && !command.IsAdmin)
                throw new ForbiddenException("No puedes devolver un préstamo de otro usuario.");

            loan.Return();

            await _catalogClient.ReleaseLicenseAsync(loan.LicenseId);

            await _loanRepository.UpdateAsync(loan);
            await _unitOfWork.CommitAsync();
        }
    }
}
