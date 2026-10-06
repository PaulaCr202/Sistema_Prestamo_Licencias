using System;
using System.Collections.Generic;
using System.Text;

using Catalog.Domain.Common.ValueObjects;

namespace Catalog.Domain.Entities.Categories
{
    public sealed class Category
    {
        public Guid Id { get; private set; }

        public Nombre Nombre { get; private set; } = null!;

        // Constructor para Entity Framework
        private Category()
        {
        }

        public Category(string nombre)
        {
            Id = Guid.CreateVersion7();
            Nombre = new Nombre(nombre);
        }
    }
}
