using Microsoft.EntityFrameworkCore;
using Catalog.Application.Contracts.Repositories;
using LicenseEntity = Catalog.Domain.Entities.License.License;

namespace Catalog.Persistence.Repositories;

public class LicenseRepository(DataContext context)
    : Repository<LicenseEntity>(context), ILicenseRepository
{
    public async Task<IEnumerable<LicenseEntity>> GetBySoftwareAsync(
        Guid softwareId, CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
                    .Where(l => l.SoftwareId == softwareId)
                    .ToListAsync(cancellationToken);

    public async Task<int> CountAvailableAsync(
        Guid softwareId, CancellationToken cancellationToken = default)
        => await Set.CountAsync(
            l => l.SoftwareId == softwareId && l.IsAvailable, cancellationToken);
}