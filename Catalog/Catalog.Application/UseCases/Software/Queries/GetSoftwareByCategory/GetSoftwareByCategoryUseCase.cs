using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Application.UseCases.Software.Queries.GetSoftwareByCategory
{
    public class GetSoftwareByCategoryUseCase : IRequestHandler<GetSoftwareByCategoryQuery, List<SoftwareDTO>>
    {
        private readonly ISoftwareRepository _repository;

        public GetSoftwareByCategoryUseCase(ISoftwareRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SoftwareDTO>> Handle(GetSoftwareByCategoryQuery request)
        {
            IEnumerable<SoftwareEntity> software = await _repository.GetByCategoryAsync(request.CategoryId);

            return software
                .OrderBy(s => s.Nombre.Valor)
                .Select(s => s.ToDto())
                .ToList();
        }
    }
}
