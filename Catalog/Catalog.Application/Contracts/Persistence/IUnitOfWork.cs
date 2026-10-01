using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}