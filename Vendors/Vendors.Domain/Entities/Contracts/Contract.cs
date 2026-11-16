using System;
using System.Collections.Generic;
using System.Text;


using Vendors.Domain.Entities.Vendors;
using Vendors.Domain.Exceptions;

namespace Vendors.Domain.Entities.Contracts
{
    public sealed class Contract
    {
        public Guid Id { get; private set; }
        public Guid VendorId { get; private set; }
        public Vendor Vendor { get; private set; } = null!;
        public string ContractNumber { get; private set; } = null!;
        public decimal Cost { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Contract() { }

        public Contract(
            Guid vendorId,
            string contractNumber,
            decimal cost,
            DateTime startDate,
            DateTime endDate)
        {
            ApplyVendorIdRules(vendorId);
            ApplyContractNumberRules(contractNumber);
            ApplyCostRules(cost);
            ApplyDateRules(startDate, endDate);

            Id = Guid.CreateVersion7();
            VendorId = vendorId;
            ContractNumber = contractNumber.Trim();
            Cost = cost;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateCost(decimal cost)
        {
            ApplyCostRules(cost);
            Cost = cost;
        }

        public void UpdateDates(
            DateTime startDate,
            DateTime endDate)
        {
            ApplyDateRules(startDate, endDate);
            StartDate = startDate;
            EndDate = endDate;
        }

        private static void ApplyVendorIdRules(Guid vendorId)
        {
            if (vendorId == Guid.Empty)
                throw new BussinesRuleException(
                    "El proveedor es requerido.");
        }


        private static void ApplyContractNumberRules(
            string contractNumber)
        {
            if (string.IsNullOrWhiteSpace(contractNumber))
            {
                throw new BussinesRuleException(
                    "El número de contrato es requerido.");
            }

            if (contractNumber.Length > 100)
            {
                throw new BussinesRuleException(
                    "El número de contrato no debe superar los 100 caracteres.");
            }
        }


        private static void ApplyCostRules(decimal cost)
        {
            if (cost < 0)
                throw new BussinesRuleException(
                    "El costo no puede ser negativo.");
        }


        private static void ApplyDateRules(
            DateTime startDate,
            DateTime endDate)
        {
            if (startDate == default)
            {
                throw new BussinesRuleException(
                    "La fecha de inicio es requerida.");
            }

            if (endDate == default)
            {
                throw new BussinesRuleException(
                    "La fecha de vencimiento es requerida.");
            }

            if (endDate <= startDate)
            {
                throw new BussinesRuleException(
                    "La fecha de vencimiento debe ser posterior a la fecha de inicio.");
            }
        }

    }
}
