using System.Security.Claims;
using Loans.Application.UseCases.Loans.Commands.ExtendLoan;
using Loans.Application.UseCases.Loans.Commands.RequestLoan;
using Loans.Application.UseCases.Loans.Commands.ReturnLoan;
using Loans.Application.UseCases.Loans.Queries.GetLoanById;
using Loans.Application.UseCases.Loans.Queries.GetLoansList;
using Loans.Application.UseCases.Loans.Queries.GetMyLoans;
using Loans.Application.Utilities.Mediator;
using Loans.Application.Utilities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loans.Api.Controllers
{
    public record RequestLoanRequest(Guid SoftwareId, int Days);

    public record ExtendLoanRequest(int ExtraDays);

    [ApiController]
    [Route("api/loans")]
    [Authorize]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator mediator) => _mediator = mediator;

        // El Id del usuario sale del token emitido por Identity
        protected Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        protected bool IsAdmin => User.IsInRole("Admin");

        [HttpPost]
        public async Task<IActionResult> RequestLoan([FromBody] RequestLoanRequest body)
        {
            var id = await _mediator.Send(new RequestLoanCommand(CurrentUserId, body.SoftwareId, body.Days));
            return Created($"api/loans/{id}", new { id });
        }

        [HttpPost("{id:guid}/return")]
        public async Task<IActionResult> ReturnLoan(Guid id)
        {
            await _mediator.Send(new ReturnLoanCommand(id, CurrentUserId, IsAdmin));
            return NoContent();
        }

        [HttpPost("{id:guid}/extend")]
        public async Task<IActionResult> ExtendLoan(Guid id, [FromBody] ExtendLoanRequest body)
        {
            await _mediator.Send(new ExtendLoanCommand(id, CurrentUserId, IsAdmin, body.ExtraDays));
            return NoContent();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyLoans(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE)
        {
            var result = await _mediator.Send(
                new GetMyLoansQuery(CurrentUserId, new PaginationRequest(pageNumber, pageSize)));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetLoanById(Guid id)
        {
            var result = await _mediator.Send(new GetLoanByIdQuery(id, CurrentUserId, IsAdmin));
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLoansList(
            [FromQuery] bool onlyOverdue = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE)
        {
            var result = await _mediator.Send(
                new GetLoansListQuery(new PaginationRequest(pageNumber, pageSize), onlyOverdue));
            return Ok(result);
        }
    }
}