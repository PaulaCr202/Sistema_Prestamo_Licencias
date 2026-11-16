using System;
using System.Collections.Generic;
using System.Text;
using Vendors.Domain.Entities.Contracts;

namespace Vendors.Application.Contracts.Repositories
{
    public interface IContractsRepository : IRepository<Contract>
    {
        Task<IEnumerable<Contract>> GetExpiringAsync(
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default);
    }
}
