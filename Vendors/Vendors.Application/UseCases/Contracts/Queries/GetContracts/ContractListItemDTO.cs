namespace Vendors.Application.UseCases.Contracts.Queries.GetContracts
{
    public sealed class ContractListItemDTO
    {
        public Guid Id { get; init; }
        public Guid VendorId { get; init; }
        public string ContractNumber { get; init; } = string.Empty;
        public decimal Cost { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

