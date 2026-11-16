using Vendors.Application.Contracts.Repositories;
using Vendors.Application.UseCases.Common;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Contracts.Queries.GetContracts
{
    public sealed class GetContractsUseCase
        : BaseHandler,
          IRequestHandler<GetContractsQuery,
              Result<List<ContractListItemDTO>>>
    {
        private readonly IContractsRepository _contractsRepository;

        public GetContractsUseCase(
            IContractsRepository contractsRepository)
        {
            _contractsRepository = contractsRepository;
        }

        public async Task<Result<List<ContractListItemDTO>>> Handle(
            GetContractsQuery request)
        {
            return await ExecuteAsync(async () =>
            {
                var contracts = await _contractsRepository.GetListAsync();

                return contracts
                    .Select(contract => contract.ToListItemDTO())
                    .ToList();
            });
        }
    }
}
