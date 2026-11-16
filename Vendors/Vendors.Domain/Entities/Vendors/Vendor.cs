using System;
using System.Collections.Generic;
using System.Text;


using Vendors.Domain.Exceptions;

namespace Vendors.Domain.Entities.Vendors
{
    public sealed class Vendor
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;

        private Vendor() { }

        public Vendor(string name, string email, string phone)
        {
            ApplyNameRules(name);
            ApplyEmailRules(email);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
            Email = email.Trim();
            Phone = phone?.Trim() ?? string.Empty;
        }

        public void Update(
            string name,
            string email,
            string phone)
        {
            ApplyNameRules(name);
            ApplyEmailRules(email);

            Name = name.Trim();
            Email = email.Trim();
            Phone = phone?.Trim() ?? string.Empty;
        }


        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BussinesRuleException(
                    "El nombre del proveedor es requerido.");

            if (name.Trim().Length < 3)
                throw new BussinesRuleException(
                    "El nombre debe tener al menos 3 caracteres.");

            if (name.Trim().Length > 150)
                throw new BussinesRuleException(
                    "El nombre no debe superar los 150 caracteres.");
        }



        private static void ApplyEmailRules(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BussinesRuleException(
                    "El correo del proveedor es requerido.");

            if (email.Trim().Length > 200)
                throw new BussinesRuleException(
                    "El correo no debe superar los 200 caracteres.");
        }

    }
}

