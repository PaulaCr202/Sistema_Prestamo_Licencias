using Catalog.Application.Contracts;
using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Exceptions;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Entities.License;

namespace Catalog.Application.UseCases.Software.Commands.RegisterLicenses
{
    public class RegisterLicensesUseCase : IRequestHandler<RegisterLicensesCommand, List<Guid>>
    {
        private readonly ISoftwareRepository _softwareRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterLicensesUseCase(
            ISoftwareRepository softwareRepository,
            ILicenseRepository licenseRepository,
            IUnitOfWork unitOfWork)
        {
            _softwareRepository = softwareRepository;
            _licenseRepository = licenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Guid>> Handle(RegisterLicensesCommand command)
        {
            if (command.Quantity <= 0)
                throw new BussinesRuleException("La cantidad de licencias debe ser mayor a cero.");

            var software = await _softwareRepository.GetByIdAsync(command.SoftwareId)
                ?? throw new BussinesRuleException("El software no existe.");

            var ids = new List<Guid>();

            for (var i = 0; i < command.Quantity; i++)
            {
                var license = new License(software.Id); // el Dominio valida el SoftwareId
                await _licenseRepository.CreateAsync(license);
                ids.Add(license.Id);
            }

            await _unitOfWork.CommitAsync();

            return ids;
        }
    }
}
