using FluentValidation;

namespace Vendors.Application.UseCases.Contracts.Commands.CreateContract
{
    public sealed class CreateContractCommandValidator
        : AbstractValidator<CreateContractCommand>
    {
        public CreateContractCommandValidator()
        {
            RuleFor(cmd => cmd.VendorId)
                .NotEmpty()
                .WithMessage("El proveedor es requerido.");

            RuleFor(cmd => cmd.ContractNumber)
                .NotEmpty()
                .WithMessage("El número de contrato es requerido.")
                .MaximumLength(100)
                .WithMessage("El número de contrato no debe superar los 100 caracteres.");

            RuleFor(cmd => cmd.Cost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El costo no puede ser negativo.");

            RuleFor(cmd => cmd.StartDate)
                .NotEmpty()
                .WithMessage("La fecha de inicio es requerida.");

            RuleFor(cmd => cmd.EndDate)
                .NotEmpty()
                .WithMessage("La fecha de vencimiento es requerida.")
                .GreaterThan(cmd => cmd.StartDate)
                .WithMessage("La fecha de vencimiento debe ser posterior a la fecha de inicio.");
        }
    }
}
