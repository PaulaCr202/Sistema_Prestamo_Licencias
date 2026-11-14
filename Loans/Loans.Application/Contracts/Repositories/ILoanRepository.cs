using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.Repositories;
using Loans.Domain.Entities.Loan;

namespace Loans.Application.Contracts.Repositories
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<int> CountActiveByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}