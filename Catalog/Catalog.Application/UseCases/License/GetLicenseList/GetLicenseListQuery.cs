using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;

namespace Catalog.Application.UseCases.License.GetLicenseList
{
    public class GetLicenseListQuery
        : IRequest<PaginationResponse<GetLicenseListDTO>>
    {
        public PaginationRequest Pagination { get; set; }
            = PaginationRequest.Standart();
    }
}