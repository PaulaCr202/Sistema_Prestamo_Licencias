using System;
using System.Collections.Generic;
using System.Text;
using Vendors.Application.Utilities.Mediator;
using Vendors.Application.Utilities.Results;

namespace Vendors.Application.UseCases.Vendors.Commands.CreateVendor
{
    public sealed class CreateVendorCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
