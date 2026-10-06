using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Application.UseCases.Software.Queries
{
    internal static class SoftwareMapperExtensions
    {
        public static SoftwareDTO ToDto(this SoftwareEntity software)
        {
            return new SoftwareDTO
            {
                Id = software.Id,
                Nombre = software.Nombre.Valor,
                CategoryId = software.CategoryId
            };
        }
    }
}