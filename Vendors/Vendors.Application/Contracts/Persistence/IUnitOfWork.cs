using System;
using System.Collections.Generic;
using System.Text;

namespace Vendors.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
