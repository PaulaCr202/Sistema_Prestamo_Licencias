using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Exceptions;

namespace Catalog.Application.UseCases.License.Commands.AssignLicense
{
    public class AssignLicenseUseCase : IRequestHandler<AssignLicenseCommand, Guid>
    {
        private readonly ISoftwareRepository _softwareRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignLicenseUseCase(
            ISoftwareRepository softwareRepository,
            ILicenseRepository licenseRepository,
            IUnitOfWork unitOfWork)
        {
            _softwareRepository = softwareRepository;
            _licenseRepository = licenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AssignLicenseCommand command)
        {
            var software = await _softwareRepository.GetByIdAsync(command.SoftwareId)
                ?? throw new BussinesRuleException("El software no existe.");

            var license = await _licenseRepository.GetFirstAvailableAsync(software.Id)
                ?? throw new BussinesRuleException("No hay licencias disponibles para este software.");

            license.Assign();
            await _licenseRepository.UpdateAsync(license);
            await _unitOfWork.CommitAsync();

            return license.Id;
        }
    }
}
