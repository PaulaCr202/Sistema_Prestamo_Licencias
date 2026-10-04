using Catalog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.UseCases.Software.Commands.CreateSoftware
{
    public record CreateSoftwareCommand(
    Guid CategoryId,
    string Nombre
) : IRequest<Guid>;

}
