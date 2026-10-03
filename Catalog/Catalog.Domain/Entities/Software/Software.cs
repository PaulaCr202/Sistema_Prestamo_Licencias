using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Common.ValueObjects;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.Software
{
    public sealed class Software
    {
        public Guid Id { get; private set; }

        public Nombre Nombre { get; private set; } = null!;

        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; } = null!;

        private Software()
        {
        }

        public Software(
            Guid categoryId,
            string nombre)
        {
            ApplyCategoryRules(categoryId);

            Id = Guid.CreateVersion7();
            CategoryId = categoryId;
            Nombre = new Nombre(nombre);
        }

        private void ApplyCategoryRules(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BussinesRuleException(
                    "La categoría es requerida.");
            }
        }
    }
}