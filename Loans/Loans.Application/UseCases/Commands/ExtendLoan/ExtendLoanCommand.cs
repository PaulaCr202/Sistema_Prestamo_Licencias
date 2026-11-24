using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Commands.ExtendLoan
{
    public record ExtendLoanCommand(Guid LoanId, Guid UserId, bool IsAdmin, int ExtraDays)
        : IRequest<ExtendLoanResponse>;

    public record ExtendLoanResponse(DateTime DueDate);
}
