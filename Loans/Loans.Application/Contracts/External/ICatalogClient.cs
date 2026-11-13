using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Application.Contracts.External
{
    public interface ICatalogClient
    {
        Task<Guid> AssignLicenseAsync(Guid softwareId, CancellationToken cancellationToken = default);
        Task ReleaseLicenseAsync(Guid licenseId, CancellationToken cancellationToken = default);
    }
}