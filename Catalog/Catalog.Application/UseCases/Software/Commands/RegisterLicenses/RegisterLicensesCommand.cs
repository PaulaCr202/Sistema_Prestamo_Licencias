using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.Software.Commands.RegisterLicenses
{
    public record RegisterLicensesCommand(
    Guid SoftwareId,
    int Quantity
) : IRequest<List<Guid>>;
}
