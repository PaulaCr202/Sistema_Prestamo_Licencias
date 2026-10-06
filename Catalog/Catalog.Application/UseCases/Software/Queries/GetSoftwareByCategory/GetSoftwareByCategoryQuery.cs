using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.Software.Queries.GetSoftwareByCategory
{
    public class GetSoftwareByCategoryQuery : IRequest<List<SoftwareDTO>>
    {
        public Guid CategoryId { get; set; }
    }
}