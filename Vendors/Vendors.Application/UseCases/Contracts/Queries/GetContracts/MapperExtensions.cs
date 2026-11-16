using Vendors.Domain.Entities.Contracts;

namespace Vendors.Application.UseCases.Contracts.Queries.GetContracts
{
    internal static class MapperExtensions
    {
        public static ContractListItemDTO ToListItemDTO(
            this Contract contract)
        {
            return new ContractListItemDTO
            {
                Id = contract.Id,
                VendorId = contract.VendorId,
                ContractNumber = contract.ContractNumber,
                Cost = contract.Cost,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                CreatedAt = contract.CreatedAt
            };
        }
    }
}
