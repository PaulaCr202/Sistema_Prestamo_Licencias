using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Persistence.Repositories;

public class CategoryRepository(DataContext context)
    : Repository<Category>(context), ICategoryRepository
{ }