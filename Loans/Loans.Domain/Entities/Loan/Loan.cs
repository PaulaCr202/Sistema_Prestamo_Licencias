using System;
using System.Collections.Generic;
using System.Text;

using Loans.Domain.Exceptions;

namespace Loans.Domain.Entities.Loan
{
    public sealed class Loan
    {
        private const int MinDays = 1;
        private const int MaxDays = 30;
        private const int MinExtraDays = 1;
        private const int MaxExtraDays = 7;

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid SoftwareId { get; private set; }
        public Guid LicenseId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnedAt { get; private set; }
        public int ExtensionsCount { get; private set; }
        public LoanStatus Status { get; private set; }

        private Loan() { } // para Entity Framework

        public Loan(Guid userId, Guid softwareId, Guid licenseId, int days)
        {
            if (userId == Guid.Empty)
                throw new BussinesRuleException("El Id del usuario no puede estar vacío.");

            if (softwareId == Guid.Empty)
                throw new BussinesRuleException("El Id del software no puede estar vacío.");

            if (licenseId == Guid.Empty)
                throw new BussinesRuleException("El Id de la licencia no puede estar vacío.");

            if (days < MinDays || days > MaxDays)
                throw new BussinesRuleException($"Los días de préstamo deben estar entre {MinDays} y {MaxDays}.");

            Id = Guid.CreateVersion7();
            UserId = userId;
            SoftwareId = softwareId;
            LicenseId = licenseId;
            StartDate = DateTime.UtcNow;
            DueDate = StartDate.AddDays(days);
            ExtensionsCount = 0;
            Status = LoanStatus.Active;
        }

        public void Return()
        {
            if (Status != LoanStatus.Active)
                throw new BussinesRuleException("Este préstamo ya fue devuelto.");

            Status = LoanStatus.Returned;
            ReturnedAt = DateTime.UtcNow;
        }

        public void Extend(int extraDays)
        {
            if (Status != LoanStatus.Active)
                throw new BussinesRuleException("Solo se puede extender un préstamo activo.");

            if (ExtensionsCount >= 1)
                throw new BussinesRuleException("Este préstamo ya fue extendido una vez.");

            if (extraDays < MinExtraDays || extraDays > MaxExtraDays)
                throw new BussinesRuleException($"La extensión debe estar entre {MinExtraDays} y {MaxExtraDays} días.");

            DueDate = DueDate.AddDays(extraDays);
            ExtensionsCount++;
        }

        public bool IsOverdue(DateTime nowUtc) => Status == LoanStatus.Active && nowUtc > DueDate;
    }
}