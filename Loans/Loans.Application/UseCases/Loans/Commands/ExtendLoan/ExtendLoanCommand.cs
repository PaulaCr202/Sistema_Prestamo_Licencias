using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Commands.ExtendLoan
{
    public record ExtendLoanCommand(Guid LoanId, Guid UserId, bool IsAdmin, int ExtraDays) : IRequest;
}