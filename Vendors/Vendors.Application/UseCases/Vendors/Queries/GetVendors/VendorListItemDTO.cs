using System;
using System.Collections.Generic;
using System.Text;


namespace Vendors.Application.UseCases.Vendors.Queries.GetVendors
{
    public sealed class VendorListItemDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
    }
}
