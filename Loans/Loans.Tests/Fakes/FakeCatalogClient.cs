using Loans.Application.Contracts.External;

namespace Loans.Tests.Fakes
{
    public class FakeCatalogClient : ICatalogClient
    {
        public Guid LicenseToAssign { get; set; } = Guid.NewGuid();
        public int AssignCalls { get; private set; }
        public List<Guid> ReleasedLicenses { get; } = new();

        public Task<Guid> AssignLicenseAsync(Guid softwareId, CancellationToken cancellationToken = default)
        {
            AssignCalls++;
            return Task.FromResult(LicenseToAssign);
        }

        public Task ReleaseLicenseAsync(Guid licenseId, CancellationToken cancellationToken = default)
        {
            ReleasedLicenses.Add(licenseId);
            return Task.CompletedTask;
        }
    }
}