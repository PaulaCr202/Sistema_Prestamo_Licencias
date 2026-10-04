namespace Catalog.Application.UseCases.Software.Queries
{
    public class SoftwareDTO
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; } = null!;
        public Guid CategoryId { get; init; }
    }
}