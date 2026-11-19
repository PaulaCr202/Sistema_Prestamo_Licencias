using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.Repositories;
using Loans.Domain.Entities.Loan;
using Microsoft.EntityFrameworkCore;

namespace Loans.Persistence.Repositories;

public class LoanRepository(DataContext context)
    : Repository<Loan>(context), ILoanRepository
{
    public async Task<int> CountActiveByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await Set.CountAsync(
            l => l.UserId == userId && l.Status == LoanStatus.Active, cancellationToken);

    public async Task<IEnumerable<Loan>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
                    .Where(l => l.UserId == userId)
                    .ToListAsync(cancellationToken);
}
