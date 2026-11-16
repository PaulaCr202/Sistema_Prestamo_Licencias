using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Contracts.Queries.GetContracts
{
    public sealed class GetContractsQuery
        : IRequest<Result<List<ContractListItemDTO>>>
    {
    }
}
