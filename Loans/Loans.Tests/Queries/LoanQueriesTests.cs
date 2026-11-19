using Loans.Application.Exceptions;
using Loans.Application.UseCases.Loans.Queries.GetLoanById;
using Loans.Application.UseCases.Loans.Queries.GetLoansList;
using Loans.Application.UseCases.Loans.Queries.GetMyLoans;
using Loans.Application.Utilities.Pagination;
using Loans.Domain.Entities.Loan;
using Loans.Tests.Fakes;

namespace Loans.Tests.Queries
{
    [TestClass]
    public class LoanQueriesTests
    {
        private FakeLoanRepository _repository = null!;

        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _otherUserId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            _repository = new FakeLoanRepository();
        }

        private Loan AddLoan(Guid userId)
        {
            var loan = new Loan(userId, Guid.NewGuid(), Guid.NewGuid(), 7);
            _repository.Loans.Add(loan);
            return loan;
        }

        private static void MakeOverdue(Loan loan)
        {
            typeof(Loan).GetProperty(nameof(Loan.DueDate))!
                .SetValue(loan, DateTime.UtcNow.AddDays(-1));
        }

        [TestMethod]
        public async Task GetMyLoans_SoloDevuelveLosDelUsuario()
        {
            AddLoan(_userId);
            AddLoan(_userId);
            AddLoan(_otherUserId);
            var useCase = new GetMyLoansUseCase(_repository);

            var result = await useCase.Handle(new GetMyLoansQuery(_userId, PaginationRequest.Standart()));

            Assert.AreEqual(2, result.TotalCount);
            Assert.IsTrue(result.Items.All(l => l.UserId == _userId));
        }

        [TestMethod]
        public async Task GetLoanById_DeOtroUsuario_DaForbidden()
        {
            Loan loan = AddLoan(_otherUserId);
            var useCase = new GetLoanByIdUseCase(_repository);

            await Assert.ThrowsExactlyAsync<ForbiddenException>(
                () => useCase.Handle(new GetLoanByIdQuery(loan.Id, _userId, IsAdmin: false)));
        }

        [TestMethod]
        public async Task GetLoanById_AdminPuedeVerElDeOtro()
        {
            Loan loan = AddLoan(_otherUserId);
            var useCase = new GetLoanByIdUseCase(_repository);

            var result = await useCase.Handle(new GetLoanByIdQuery(loan.Id, _userId, IsAdmin: true));

            Assert.AreEqual(loan.Id, result.Id);
        }

        [TestMethod]
        public async Task GetLoanById_QueNoExiste_DaNotFound()
        {
            var useCase = new GetLoanByIdUseCase(_repository);

            await Assert.ThrowsExactlyAsync<NotFoundException>(
                () => useCase.Handle(new GetLoanByIdQuery(Guid.NewGuid(), _userId, IsAdmin: false)));
        }

        [TestMethod]
        public async Task GetLoansList_OnlyOverdue_SoloDevuelveLosVencidos()
        {
            AddLoan(_userId);
            Loan overdue = AddLoan(_otherUserId);
            MakeOverdue(overdue);
            var useCase = new GetLoansListUseCase(_repository);

            var all = await useCase.Handle(new GetLoansListQuery(PaginationRequest.Standart(), OnlyOverdue: false));
            var onlyOverdue = await useCase.Handle(new GetLoansListQuery(PaginationRequest.Standart(), OnlyOverdue: true));

            Assert.AreEqual(2, all.TotalCount);
            Assert.AreEqual(1, onlyOverdue.TotalCount);
            Assert.AreEqual(overdue.Id, onlyOverdue.Items.Single().Id);
            Assert.IsTrue(onlyOverdue.Items.Single().IsOverdue);
        }
    }
}