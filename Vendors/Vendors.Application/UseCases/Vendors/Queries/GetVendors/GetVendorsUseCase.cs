using Vendors.Application.Contracts.Repositories;
using Vendors.Application.UseCases.Common;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Vendors.Queries.GetVendors
{
    public sealed class GetVendorsUseCase
        : BaseHandler,
          IRequestHandler<GetVendorsQuery,
              Result<List<VendorListItemDTO>>>
    {
        private readonly IVendorsRepository _vendorsRepository;

        public GetVendorsUseCase(
            IVendorsRepository vendorsRepository)
        {
            _vendorsRepository = vendorsRepository;
        }

        public async Task<Result<List<VendorListItemDTO>>> Handle(
            GetVendorsQuery request)
        {
            return await ExecuteAsync(async () =>
            {
                var vendors = await _vendorsRepository.GetListAsync();

                return vendors
                    .Select(vendor => vendor.ToListItemDTO())
                    .ToList();
            });
        }
    }
}
