namespace Loans.Application.UseCases.Loans.Queries
{
    public class LoanDTO
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid SoftwareId { get; init; }
        public Guid LicenseId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? ReturnedAt { get; init; }
        public int ExtensionsCount { get; init; }
        public string Status { get; init; } = null!;
        public bool IsOverdue { get; init; }
    }
}