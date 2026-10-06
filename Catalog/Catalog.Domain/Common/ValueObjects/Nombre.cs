using System;
using System.Collections.Generic;
using System.Text;

using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Common.ValueObjects
{
    public sealed record Nombre
    {
        public string Valor { get; private set; }

        private Nombre()
        {
            Valor = null!;
        }

        public Nombre(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new BussinesRuleException(
                    "El nombre es requerido.");
            }

            if (valor.Length > 100)
            {
                throw new BussinesRuleException(
                    "El nombre no puede superar los 100 caracteres.");
            }

            Valor = valor;
        }
    }
}
