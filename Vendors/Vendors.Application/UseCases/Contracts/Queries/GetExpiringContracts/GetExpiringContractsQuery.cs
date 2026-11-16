using Vendors.Application.UseCases.Contracts.Queries.GetContracts;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Contracts.Queries.GetExpiringContracts
{
    public sealed class GetExpiringContractsQuery
        : IRequest<Result<List<ContractListItemDTO>>>
    {
        public int Days { get; set; } = 30;
    }
}
