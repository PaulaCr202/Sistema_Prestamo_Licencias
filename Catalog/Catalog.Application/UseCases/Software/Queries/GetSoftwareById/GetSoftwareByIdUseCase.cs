using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Application.UseCases.Software.Queries.GetSoftwareById
{
    public class GetSoftwareByIdUseCase : IRequestHandler<GetSoftwareByIdQuery, SoftwareDTO?>
    {
        private readonly ISoftwareRepository _repository;

        public GetSoftwareByIdUseCase(ISoftwareRepository repository)
        {
            _repository = repository;
        }

        public async Task<SoftwareDTO?> Handle(GetSoftwareByIdQuery request)
        {
            SoftwareEntity? software = await _repository.GetByIdAsync(request.Id);

            return software?.ToDto();
        }
    }
}