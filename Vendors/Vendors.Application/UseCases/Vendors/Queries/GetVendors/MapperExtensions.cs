using System;
using System.Collections.Generic;
using System.Text;


using Vendors.Domain.Entities.Vendors;

namespace Vendors.Application.UseCases.Vendors.Queries.GetVendors
{
    internal static class MapperExtensions
    {
        public static VendorListItemDTO ToListItemDTO(
            this Vendor vendor)
        {
            return new VendorListItemDTO
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Email = vendor.Email,
                Phone = vendor.Phone
            };
        }
    }
}
