using Catalog.Application.Contracts.Persistence;

namespace Catalog.Persistence.UnitOfWorks;

public class EFCoreUnitOfWork(DataContext context) : IUnitOfWork
{
    public async Task CommitAsync() => await context.SaveChangesAsync();

    public Task RollbackAsync()
    {
        context.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}