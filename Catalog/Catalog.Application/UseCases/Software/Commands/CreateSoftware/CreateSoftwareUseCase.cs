using Catalog.Application.Contracts.Persistence;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Application.UseCases.Software.Commands.CreateSoftware
{
    public class CreateSoftwareUseCase : IRequestHandler<CreateSoftwareCommand, Guid>
    {
        private readonly ISoftwareRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSoftwareUseCase(ISoftwareRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSoftwareCommand command)
        {
            var software = new SoftwareEntity(command.CategoryId, command.Nombre);

            await _repository.CreateAsync(software);
            await _unitOfWork.CommitAsync();

            return software.Id;
        }
    }
}
