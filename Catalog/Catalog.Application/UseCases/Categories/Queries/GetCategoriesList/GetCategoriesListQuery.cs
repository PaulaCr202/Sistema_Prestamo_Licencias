using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Text;


namespace Catalog.Application.UseCases.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListQuery : IRequest<PaginationResponse<GetCategoriesListDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
    }
}
