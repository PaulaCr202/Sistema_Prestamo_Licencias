using System;
using System.Collections.Generic;
using System.Text;
using Vendors.Domain.Entities.Vendors;

namespace Vendors.Application.Contracts.Repositories
{
    public interface IVendorsRepository : IRepository<Vendor>
    {
    }
}
