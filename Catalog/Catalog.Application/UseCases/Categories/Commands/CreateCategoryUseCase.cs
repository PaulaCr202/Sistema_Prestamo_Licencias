using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Application.UseCases.Categories.Commands.CreateCategory
{
    public class CreateCategoryUseCase : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryUseCase(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCategoryCommand command)
        {
            var category = new Category(command.Nombre);

            await _repository.CreateAsync(category);
            await _unitOfWork.CommitAsync();

            return category.Id;
        }
    }
}