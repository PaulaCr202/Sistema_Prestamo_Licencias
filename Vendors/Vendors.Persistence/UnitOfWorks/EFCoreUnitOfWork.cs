using Vendors.Application.Contracts.Persistence;

namespace Vendors.Persistence.UnitOfWorks;

public class EFCoreUnitOfWork(DataContext context) : IUnitOfWork
{
    public async Task CommitAsync() => await context.SaveChangesAsync();

    public Task RollbackAsync()
    {
        context.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}