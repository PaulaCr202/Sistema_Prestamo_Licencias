using Microsoft.EntityFrameworkCore;
using Vendors.Application.Contracts.Repositories;
using Vendors.Domain.Entities.Contracts;

namespace Vendors.Persistence.Repositories;

public class ContractsRepository(DataContext context)
    : Repository<Contract>(context), IContractsRepository
{
    public async Task<IEnumerable<Contract>> GetExpiringAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
                    .Where(c => c.EndDate >= fromDate && c.EndDate <= toDate)
                    .OrderBy(c => c.EndDate)
                    .ToListAsync(cancellationToken);
}