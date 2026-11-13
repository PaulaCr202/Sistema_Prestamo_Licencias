using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.Persistence;

namespace Loans.Persistence.UnitOfWorks;

public class EFCoreUnitOfWork(DataContext context) : IUnitOfWork
{
    public async Task CommitAsync() => await context.SaveChangesAsync();

    public Task RollbackAsync()
    {
        context.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}