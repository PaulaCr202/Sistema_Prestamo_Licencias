using LicenseEntity = Catalog.Domain.Entities.License.License;

namespace Catalog.Application.UseCases.License.GetLicenseList
{
    internal static class MapperExtensions
    {
        public static GetLicenseListDTO ToListItemDTO(
            this LicenseEntity licenseEntity)
        {
            return new GetLicenseListDTO
            {
                Id = licenseEntity.Id,
                SoftwareId = licenseEntity.SoftwareId,
                IsAvailable = licenseEntity.IsAvailable
            };
        }
    }
}