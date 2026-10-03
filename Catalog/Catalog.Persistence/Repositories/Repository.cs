using Microsoft.EntityFrameworkCore;
using Catalog.Application.Contracts.Repositories;

namespace Catalog.Persistence.Repositories;

public class Repository<TEntity>(DataContext context) : IRepository<TEntity> where TEntity : class
{
    protected readonly DataContext Context = context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Set.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Update(entity);
        return Task.FromResult(entity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await Set.FindAsync([id], cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null) Set.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        => await Set.AsNoTracking().ToListAsync(cancellationToken);
}