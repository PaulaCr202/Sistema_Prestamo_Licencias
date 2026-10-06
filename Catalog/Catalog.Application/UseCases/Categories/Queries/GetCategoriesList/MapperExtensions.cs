using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Entities.Categories;


namespace Catalog.Application.UseCases.Categories.Queries.GetCategoriesList
{
    public static class MapperExtensions
    {
        public static GetCategoriesListDTO ToListItemDTO(
            this Category categoryEntity)
        {
            return new GetCategoriesListDTO
            {
                Id = categoryEntity.Id,
                Nombre = categoryEntity.Nombre.Valor
            };
        }
    }
}
