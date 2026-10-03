using Microsoft.EntityFrameworkCore;
using Catalog.Application.Contracts.Repositories;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Persistence.Repositories;

public class SoftwareRepository(DataContext context)
    : Repository<SoftwareEntity>(context), ISoftwareRepository
{
    public async Task<IEnumerable<SoftwareEntity>> GetByCategoryAsync(
        Guid categoryId, CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
                    .Where(s => s.CategoryId == categoryId)
                    .ToListAsync(cancellationToken);
}