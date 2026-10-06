using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Application.UseCases.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListUseCase : IRequestHandler<GetCategoriesListQuery, PaginationResponse<GetCategoriesListDTO>>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoriesListUseCase(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<GetCategoriesListDTO>> Handle(GetCategoriesListQuery request)
        {
            IEnumerable<Category> categories = await _repository.GetListAsync();

            List<Category> ordered = categories.OrderBy(c => c.Nombre.Valor).ToList();

            PaginationRequest pagination = request.Pagination;

            List<GetCategoriesListDTO> items = ordered
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(c => c.ToListItemDTO())
                .ToList();

            return PaginationResponse<GetCategoriesListDTO>.Create(items, ordered.Count, pagination);
        }
    }
}