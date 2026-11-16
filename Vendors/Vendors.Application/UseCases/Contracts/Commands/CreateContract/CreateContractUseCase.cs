using Vendors.Application.Contracts.Persistence;
using Vendors.Application.Contracts.Repositories;
using Vendors.Application.Exceptions;
using Vendors.Application.UseCases.Common;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;
using Vendors.Domain.Entities.Contracts;

namespace Vendors.Application.UseCases.Contracts.Commands.CreateContract
{
    public sealed class CreateContractUseCase
        : BaseHandler,
          IRequestHandler<CreateContractCommand, Result<Guid>>
    {
        private readonly IContractsRepository _contractsRepository;
        private readonly IVendorsRepository _vendorsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateContractUseCase(
            IContractsRepository contractsRepository,
            IVendorsRepository vendorsRepository,
            IUnitOfWork unitOfWork)
        {
            _contractsRepository = contractsRepository;
            _vendorsRepository = vendorsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateContractCommand request)
        {
            return await ExecuteAsync(async () =>
            {
                var vendor = await _vendorsRepository.GetByIdAsync(
                    request.VendorId);

                if (vendor is null)
                {
                    throw new NotFoundException(
                        "El proveedor indicado no existe.");
                }

                var contract = new Contract(
                    request.VendorId,
                    request.ContractNumber,
                    request.Cost,
                    request.StartDate,
                    request.EndDate);

                await _contractsRepository.CreateAsync(contract);

                await _unitOfWork.CommitAsync();

                return contract.Id;
            });
        }
    }
}
