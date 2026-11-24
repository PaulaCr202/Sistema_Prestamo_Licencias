using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.External;
using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Application.Utilities.Mediator;
using Loans.Domain.Entities.Loan;

namespace Loans.Application.UseCases.Commands.CreateLoan
{
    public class CreateLoanUseCase : IRequestHandler<CreateLoanCommand, CreateLoanResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ICatalogClient _catalogClient;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLoanUseCase(
            ILoanRepository loanRepository,
            ICatalogClient catalogClient,
            IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _catalogClient = catalogClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateLoanResponse> Handle(CreateLoanCommand command)
        {
            Guid licenseId = await _catalogClient.AssignLicenseAsync(command.SoftwareId);

            try
            {
                var loan = new Loan(command.UserId, command.SoftwareId, licenseId, command.Days);

                await _loanRepository.CreateAsync(loan);
                await _unitOfWork.CommitAsync();

                return new CreateLoanResponse(loan.Id, loan.LicenseId, loan.DueDate);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                await _catalogClient.ReleaseLicenseAsync(licenseId);
                throw;
            }
        }
    }
}