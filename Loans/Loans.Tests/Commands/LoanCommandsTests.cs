using Loans.Application.Exceptions;
using Loans.Application.UseCases.Loans.Commands.ExtendLoan;
using Loans.Application.UseCases.Loans.Commands.RequestLoan;
using Loans.Application.UseCases.Loans.Commands.ReturnLoan;
using Loans.Domain.Entities.Loan;
using Loans.Domain.Exceptions;
using Loans.Tests.Fakes;

namespace Loans.Tests.Commands
{
    [TestClass]
    public class LoanCommandsTests
    {
        private FakeLoanRepository _repository = null!;
        private FakeCatalogClient _catalog = null!;
        private FakeUnitOfWork _unitOfWork = null!;

        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _softwareId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            _repository = new FakeLoanRepository();
            _catalog = new FakeCatalogClient();
            _unitOfWork = new FakeUnitOfWork();
        }

        private Loan AddActiveLoan(Guid userId)
        {
            var loan = new Loan(userId, _softwareId, Guid.NewGuid(), 7);
            _repository.Loans.Add(loan);
            return loan;
        }

        [TestMethod]
        public async Task RequestLoan_Valido_GuardaElPrestamoConLaLicenciaAsignada()
        {
            var useCase = new RequestLoanUseCase(_repository, _catalog, _unitOfWork);

            Guid id = await useCase.Handle(new RequestLoanCommand(_userId, _softwareId, 7));

            Loan saved = _repository.Loans.Single();
            Assert.AreEqual(id, saved.Id);
            Assert.AreEqual(_catalog.LicenseToAssign, saved.LicenseId);
            Assert.AreEqual(1, _unitOfWork.Commits);
        }

        [TestMethod]
        public async Task RequestLoan_ConTresActivos_FallaYNoLlamaACatalog()
        {
            _repository.ActiveCountOverride = 3;
            var useCase = new RequestLoanUseCase(_repository, _catalog, _unitOfWork);

            await Assert.ThrowsExactlyAsync<BussinesRuleException>(
                () => useCase.Handle(new RequestLoanCommand(_userId, _softwareId, 7)));

            Assert.AreEqual(0, _catalog.AssignCalls);
        }

        [TestMethod]
        public async Task RequestLoan_SiFallaElGuardado_LiberaLaLicencia()
        {
            _unitOfWork.FailOnCommit = true;
            var useCase = new RequestLoanUseCase(_repository, _catalog, _unitOfWork);

            await Assert.ThrowsExactlyAsync<InvalidOperationException>(
                () => useCase.Handle(new RequestLoanCommand(_userId, _softwareId, 7)));

            CollectionAssert.Contains(_catalog.ReleasedLicenses, _catalog.LicenseToAssign);
        }

        [TestMethod]
        public async Task ReturnLoan_DeOtroUsuario_DaForbidden()
        {
            Loan loan = AddActiveLoan(Guid.NewGuid());
            var useCase = new ReturnLoanUseCase(_repository, _catalog, _unitOfWork);

            await Assert.ThrowsExactlyAsync<ForbiddenException>(
                () => useCase.Handle(new ReturnLoanCommand(loan.Id, _userId, IsAdmin: false)));
        }

        [TestMethod]
        public async Task ReturnLoan_Valido_LiberaLaLicenciaYQuedaDevuelto()
        {
            Loan loan = AddActiveLoan(_userId);
            var useCase = new ReturnLoanUseCase(_repository, _catalog, _unitOfWork);

            await useCase.Handle(new ReturnLoanCommand(loan.Id, _userId, IsAdmin: false));

            CollectionAssert.Contains(_catalog.ReleasedLicenses, loan.LicenseId);
            Assert.AreEqual(LoanStatus.Returned, loan.Status);
        }

        [TestMethod]
        public async Task ReturnLoan_QueNoExiste_DaNotFound()
        {
            var useCase = new ReturnLoanUseCase(_repository, _catalog, _unitOfWork);

            await Assert.ThrowsExactlyAsync<NotFoundException>(
                () => useCase.Handle(new ReturnLoanCommand(Guid.NewGuid(), _userId, IsAdmin: false)));
        }

        [TestMethod]
        public async Task ExtendLoan_DosVeces_Falla()
        {
            Loan loan = AddActiveLoan(_userId);
            var useCase = new ExtendLoanUseCase(_repository, _unitOfWork);

            await useCase.Handle(new ExtendLoanCommand(loan.Id, _userId, IsAdmin: false, ExtraDays: 3));

            await Assert.ThrowsExactlyAsync<BussinesRuleException>(
                () => useCase.Handle(new ExtendLoanCommand(loan.Id, _userId, IsAdmin: false, ExtraDays: 3)));
        }
    }
}