using Catalog.Domain.Entities.Software;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Repositories
{
    public interface ISoftwareRepository : IRepository<Software>
    {
        Task<IEnumerable<Software>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    }
}