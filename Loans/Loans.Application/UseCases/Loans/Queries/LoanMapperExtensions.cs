using LoanEntity = Loans.Domain.Entities.Loan.Loan;

namespace Loans.Application.UseCases.Loans.Queries
{
    internal static class LoanMapperExtensions
    {
        public static LoanDTO ToDto(this LoanEntity loan, DateTime nowUtc)
        {
            return new LoanDTO
            {
                Id = loan.Id,
                UserId = loan.UserId,
                SoftwareId = loan.SoftwareId,
                LicenseId = loan.LicenseId,
                StartDate = loan.StartDate,
                DueDate = loan.DueDate,
                ReturnedAt = loan.ReturnedAt,
                ExtensionsCount = loan.ExtensionsCount,
                Status = loan.Status.ToString(),
                IsOverdue = loan.IsOverdue(nowUtc)
            };
        }
    }
}