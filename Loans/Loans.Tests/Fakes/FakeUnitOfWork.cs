using Loans.Application.Contracts.Persistence;

namespace Loans.Tests.Fakes
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        // Si está en true, CommitAsync falla como si la base de datos no respondiera
        public bool FailOnCommit { get; set; }
        public int Commits { get; private set; }

        public Task CommitAsync()
        {
            if (FailOnCommit)
                throw new InvalidOperationException("Fallo simulado al guardar.");

            Commits++;
            return Task.CompletedTask;
        }

        public Task RollbackAsync() => Task.CompletedTask;
    }
}
