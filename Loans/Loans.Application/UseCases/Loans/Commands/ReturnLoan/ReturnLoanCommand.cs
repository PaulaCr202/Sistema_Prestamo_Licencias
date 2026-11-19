using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Commands.ReturnLoan
{
    public record ReturnLoanCommand(Guid LoanId, Guid UserId, bool IsAdmin) : IRequest;
}