using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Application.UseCases.Software.Queries.GetSoftwareList
{
    public class GetSoftwareListUseCase : IRequestHandler<GetSoftwareListQuery, PaginationResponse<SoftwareDTO>>
    {
        private readonly ISoftwareRepository _repository;

        public GetSoftwareListUseCase(ISoftwareRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<SoftwareDTO>> Handle(GetSoftwareListQuery request)
        {
            IEnumerable<SoftwareEntity> software = await _repository.GetListAsync();

            List<SoftwareEntity> ordered = software
                .OrderBy(s => s.Nombre.Valor)
                .ToList();

            PaginationRequest pagination = request.Pagination;

            List<SoftwareDTO> items = ordered
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(s => s.ToDto())
                .ToList();

            return PaginationResponse<SoftwareDTO>.Create(items, ordered.Count, pagination);
        }
    }
}