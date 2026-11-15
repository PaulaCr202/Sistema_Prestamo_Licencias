using Catalog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.License.Commands.AssignLicense
{
    public record AssignLicenseCommand(Guid SoftwareId) : IRequest<Guid>;
}
