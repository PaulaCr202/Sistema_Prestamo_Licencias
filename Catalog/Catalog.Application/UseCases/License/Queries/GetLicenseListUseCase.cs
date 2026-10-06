using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using LicenseEntity = Catalog.Domain.Entities.License.License;

namespace Catalog.Application.UseCases.License.GetLicenseList
{
    public class GetLicenseListUseCase : IRequestHandler<GetLicenseListQuery, PaginationResponse<GetLicenseListDTO>>
    {
        private readonly ILicenseRepository _repository;

        public GetLicenseListUseCase(ILicenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<GetLicenseListDTO>> Handle(GetLicenseListQuery request)
        {
            IEnumerable<LicenseEntity> licenses = await _repository.GetListAsync();

            List<LicenseEntity> ordered = licenses.OrderBy(l => l.SoftwareId).ToList();

            PaginationRequest pagination = request.Pagination;

            List<GetLicenseListDTO> items = ordered
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(l => l.ToListItemDTO())
                .ToList();

            return PaginationResponse<GetLicenseListDTO>.Create(items, ordered.Count, pagination);
        }
    }
}