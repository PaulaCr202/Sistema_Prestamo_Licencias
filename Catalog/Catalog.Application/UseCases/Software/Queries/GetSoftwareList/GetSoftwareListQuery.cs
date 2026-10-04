using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;

namespace Catalog.Application.UseCases.Software.Queries.GetSoftwareList
{
    public class GetSoftwareListQuery : IRequest<PaginationResponse<SoftwareDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
    }
}