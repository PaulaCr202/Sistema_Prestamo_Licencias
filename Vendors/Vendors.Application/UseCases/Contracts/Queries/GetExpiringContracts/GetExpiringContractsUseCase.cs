using Vendors.Application.Contracts.Repositories;
using Vendors.Application.UseCases.Common;
using Vendors.Application.UseCases.Contracts.Queries.GetContracts;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Contracts.Queries.GetExpiringContracts
{
    public sealed class GetExpiringContractsUseCase
        : BaseHandler,
          IRequestHandler<GetExpiringContractsQuery,
              Result<List<ContractListItemDTO>>>
    {
        private readonly IContractsRepository _contractsRepository;

        public GetExpiringContractsUseCase(
            IContractsRepository contractsRepository)
        {
            _contractsRepository = contractsRepository;
        }

        public async Task<Result<List<ContractListItemDTO>>> Handle(
            GetExpiringContractsQuery request)
        {
            return await ExecuteAsync(async () =>
            {
                var fromDate = DateTime.UtcNow;
                var toDate = fromDate.AddDays(request.Days);

                var contracts = await _contractsRepository.GetExpiringAsync(
                    fromDate,
                    toDate);

                return contracts
                    .Select(contract => contract.ToListItemDTO())
                    .ToList();
            });
        }
    }
}
