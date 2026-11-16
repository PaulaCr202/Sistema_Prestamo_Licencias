using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Contracts.Commands.CreateContract
{
    public sealed class CreateContractCommand
        : IRequest<Result<Guid>>
    {
        public Guid VendorId { get; set; }

        public string ContractNumber { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
