using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.UseCases.License.GetLicenseList
{
    public class GetLicenseListDTO
    {
        public Guid Id { get; init; }
        public Guid SoftwareId { get; init; }
        public bool IsAvailable { get; init; }
    }
}