using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Application.Exceptions;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Commands.ExtendLoan
{
    public class ExtendLoanUseCase : IRequestHandler<ExtendLoanCommand, ExtendLoanResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExtendLoanUseCase(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ExtendLoanResponse> Handle(ExtendLoanCommand command)
        {
            var loan = await _loanRepository.GetByIdAsync(command.LoanId)
                ?? throw new NotFoundException("El préstamo no existe.");

            if (loan.UserId != command.UserId && !command.IsAdmin)
                throw new ForbiddenException("No puedes extender un préstamo de otro usuario.");

            loan.Extend(command.ExtraDays); 

            await _loanRepository.UpdateAsync(loan);
            await _unitOfWork.CommitAsync();

            return new ExtendLoanResponse(loan.DueDate);
        }
    }
}
