using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.License.Commands.ReleaseLicense
{
    public record ReleaseLicenseCommand(Guid LicenseId) : IRequest;
}
