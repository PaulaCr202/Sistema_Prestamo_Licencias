using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Exceptions;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Domain.Entities.License
{
    public sealed class License
    {
        public Guid Id { get; private set; }

        public Guid SoftwareId { get; private set; }

        public SoftwareEntity Software { get; private set; } = null!;

        public bool IsAvailable { get; private set; }

        private License()
        {
        }

        public License(Guid softwareId)
        {
            ApplySoftwareRules(softwareId);

            Id = Guid.CreateVersion7();
            SoftwareId = softwareId;
            IsAvailable = true;
        }

        public void Assign()
        {
            if (!IsAvailable)
            {
                throw new BussinesRuleException(
                    "La licencia no está disponible.");
            }

            IsAvailable = false;
        }

        public void Release()
        {
            IsAvailable = true;
        }

        private void ApplySoftwareRules(Guid softwareId)
        {
            if (softwareId == Guid.Empty)
            {
                throw new BussinesRuleException(
                    "El software es requerido.");
            }
        }
    }
}