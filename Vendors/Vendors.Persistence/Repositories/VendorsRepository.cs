using Vendors.Application.Contracts.Repositories;
using Vendors.Domain.Entities.Vendors;

namespace Vendors.Persistence.Repositories;

public class VendorsRepository(DataContext context)
    : Repository<Vendor>(context), IVendorsRepository
{
}