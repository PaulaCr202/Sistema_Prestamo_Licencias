using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Utilities.Mediator;

namespace Loans.Application.UseCases.Commands.ReturnLoan
{
    public record ReturnLoanCommand(Guid LoanId, Guid UserId, bool IsAdmin) : IRequest;
}