using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.Software.Queries.GetSoftwareById
{
    public class GetSoftwareByIdQuery : IRequest<SoftwareDTO?>
    {
        public Guid Id { get; set; }
    }
}