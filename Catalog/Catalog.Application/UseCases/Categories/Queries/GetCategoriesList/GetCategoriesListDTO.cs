using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.UseCases.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListDTO
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; } = null!;
    }
}