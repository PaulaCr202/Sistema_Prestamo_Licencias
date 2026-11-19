using Loans.Application.Contracts.External;
using Loans.Application.Contracts.Persistence;
using Loans.Application.Contracts.Repositories;
using Loans.Application.Utilities.Mediator;
using Loans.Domain.Exceptions;
using LoanEntity = Loans.Domain.Entities.Loan.Loan;

namespace Loans.Application.UseCases.Loans.Commands.RequestLoan
{
    public class RequestLoanUseCase : IRequestHandler<RequestLoanCommand, Guid>
    {
        private const int MaxActiveLoans = 3;

        private readonly ILoanRepository _repository;
        private readonly ICatalogClient _catalogClient;
        private readonly IUnitOfWork _unitOfWork;

        public RequestLoanUseCase(ILoanRepository repository, ICatalogClient catalogClient, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _catalogClient = catalogClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(RequestLoanCommand request)
        {
        
            int activeLoans = await _repository.CountActiveByUserAsync(request.UserId);
            if (activeLoans >= MaxActiveLoans)
            {
                throw new BussinesRuleException($"Ya tienes {MaxActiveLoans} préstamos activos.");
            }

            Guid licenseId = await _catalogClient.AssignLicenseAsync(request.SoftwareId);

            try
            {
                var loan = new LoanEntity(request.UserId, request.SoftwareId, licenseId, request.Days);
                await _repository.CreateAsync(loan);
                await _unitOfWork.CommitAsync();
                return loan.Id;
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