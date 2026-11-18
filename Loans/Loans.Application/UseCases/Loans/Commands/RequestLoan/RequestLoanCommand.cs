using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Loans.Commands.RequestLoan
{
    public record RequestLoanCommand(Guid UserId, Guid SoftwareId, int Days) : IRequest<Guid>;
}