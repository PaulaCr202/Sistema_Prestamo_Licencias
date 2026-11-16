
using FluentValidation;

namespace Vendors.Application.UseCases.Vendors.Commands.CreateVendor
{
    public sealed class CreateVendorCommandValidator
        : AbstractValidator<CreateVendorCommand>
    {
        public CreateVendorCommandValidator()
        {
            RuleFor(cmd => cmd.Name)
                .NotEmpty()
                .WithMessage("El nombre del proveedor es requerido.")
                .MinimumLength(3)
                .WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(150)
                .WithMessage("El nombre no debe superar los 150 caracteres.");

            RuleFor(cmd => cmd.Email)
                .NotEmpty()
                .WithMessage("El correo del proveedor es requerido.")
                .EmailAddress()
                .WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(200)
                .WithMessage("El correo no debe superar los 200 caracteres.");

            RuleFor(cmd => cmd.Phone)
                .MaximumLength(30)
                .WithMessage("El teléfono no debe superar los 30 caracteres.");
        }
    }
}
