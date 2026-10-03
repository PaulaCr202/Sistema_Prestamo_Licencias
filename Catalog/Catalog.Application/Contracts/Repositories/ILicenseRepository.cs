using Catalog.Domain.Entities.License;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Repositories
{
    public interface ILicenseRepository : IRepository<License>
    {
        Task<IEnumerable<License>> GetBySoftwareAsync(Guid softwareId, CancellationToken cancellationToken = default);
        Task<int> CountAvailableAsync(Guid softwareId, CancellationToken cancellationToken = default);
    }
}