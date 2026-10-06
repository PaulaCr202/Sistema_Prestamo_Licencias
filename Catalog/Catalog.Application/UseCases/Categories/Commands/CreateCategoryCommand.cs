using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(string Nombre) : IRequest<Guid>;
}