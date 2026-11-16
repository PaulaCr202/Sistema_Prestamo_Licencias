using FluentValidation;

namespace Vendors.Application.UseCases.Contracts.Queries.GetExpiringContracts
{
    public sealed class GetExpiringContractsQueryValidator
        : AbstractValidator<GetExpiringContractsQuery>
    {
        public GetExpiringContractsQueryValidator()
        {
            RuleFor(query => query.Days)
                .InclusiveBetween(1, 365)
                .WithMessage(
                    "La cantidad de días debe estar entre 1 y 365.");
        }
    }
}
