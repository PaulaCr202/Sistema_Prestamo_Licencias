using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Exceptions;

namespace Catalog.Application.UseCases.License.Commands.ReleaseLicense
{
    public class ReleaseLicenseUseCase : IRequestHandler<ReleaseLicenseCommand>
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReleaseLicenseUseCase(ILicenseRepository licenseRepository, IUnitOfWork unitOfWork)
        {
            _licenseRepository = licenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ReleaseLicenseCommand command)
        {
            var license = await _licenseRepository.GetByIdAsync(command.LicenseId)
                ?? throw new BussinesRuleException("La licencia no existe.");

            license.Release();
            await _licenseRepository.UpdateAsync(license);
            await _unitOfWork.CommitAsync();
        }
    }
}
