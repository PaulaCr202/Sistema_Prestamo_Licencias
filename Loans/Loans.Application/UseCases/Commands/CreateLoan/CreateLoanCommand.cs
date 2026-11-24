using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Commands.CreateLoan
{
    public record CreateLoanCommand(Guid UserId, Guid SoftwareId, int Days)
        : IRequest<CreateLoanResponse>;

    public record CreateLoanResponse(Guid Id, Guid LicenseId, DateTime DueDate);
}
