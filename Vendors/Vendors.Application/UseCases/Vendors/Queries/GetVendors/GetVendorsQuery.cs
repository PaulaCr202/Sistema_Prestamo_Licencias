using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Vendors.Queries.GetVendors
{
    public sealed class GetVendorsQuery
        : IRequest<Result<List<VendorListItemDTO>>>
    {
    }
}
