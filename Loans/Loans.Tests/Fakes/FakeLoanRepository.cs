using Loans.Application.Contracts.Repositories;
using Loans.Domain.Entities.Loan;

namespace Loans.Tests.Fakes
{
    public class FakeLoanRepository : ILoanRepository
    {
        public List<Loan> Loans { get; } = new();

        // Permite simular un número de préstamos activos sin tener que crearlos
        public int? ActiveCountOverride { get; set; }

        public Task<Loan> CreateAsync(Loan entity, CancellationToken cancellationToken = default)
        {
            Loans.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<Loan> UpdateAsync(Loan entity, CancellationToken cancellationToken = default)
            => Task.FromResult(entity);

        public Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Loans.FirstOrDefault(l => l.Id == id));

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Loans.RemoveAll(l => l.Id == id);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Loan>> GetListAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IEnumerable<Loan>>(Loans.ToList());

        public Task<int> CountActiveByUserAsync(Guid userId, CancellationToken cancellationToken = default)
            => Task.FromResult(ActiveCountOverride
                ?? Loans.Count(l => l.UserId == userId && l.Status == LoanStatus.Active));

        public Task<IEnumerable<Loan>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
            => Task.FromResult<IEnumerable<Loan>>(Loans.Where(l => l.UserId == userId).ToList());
    }
}