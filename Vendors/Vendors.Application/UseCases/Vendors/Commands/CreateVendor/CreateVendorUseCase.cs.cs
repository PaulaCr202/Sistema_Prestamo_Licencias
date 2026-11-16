using Vendors.Application.Contracts.Persistence;
using Vendors.Application.Contracts.Repositories;
using Vendors.Application.UseCases.Common;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;
using Vendors.Domain.Entities.Vendors;

namespace Vendors.Application.UseCases.Vendors.Commands.CreateVendor
{
    public sealed class CreateVendorUseCase
        : BaseHandler,
          IRequestHandler<CreateVendorCommand, Result<Guid>>
    {
        private readonly IVendorsRepository _vendorsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVendorUseCase(
            IVendorsRepository vendorsRepository,
            IUnitOfWork unitOfWork)
        {
            _vendorsRepository = vendorsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateVendorCommand request)
        {
            return await ExecuteAsync(async () =>
            {
                var vendor = new Vendor(
                    request.Name,
                    request.Email,
                    request.Phone);

                await _vendorsRepository.CreateAsync(vendor);

                await _unitOfWork.CommitAsync();

                return vendor.Id;
            });
        }
    }
}
