using System;
using System.Collections.Generic;
using System.Text;
using Loans.Domain.Entities.Loan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loans.Persistence.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Status)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(l => l.UserId).IsRequired();
        builder.Property(l => l.SoftwareId).IsRequired();
        builder.Property(l => l.LicenseId).IsRequired();
        builder.Property(l => l.StartDate).IsRequired();
        builder.Property(l => l.DueDate).IsRequired();

        builder.HasIndex(l => l.UserId);
    }
}
