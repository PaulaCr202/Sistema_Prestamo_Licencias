using Loans.Api.Extensions;
using Loans.Application.UseCases.Commands.CreateLoan;
using Loans.Application.UseCases.Commands.ExtendLoan;
using Loans.Application.UseCases.Commands.ReturnLoan;
using Loans.Application.UseCases.Commands;     // <-- ajustar al namespace real de Persona 1
using Loans.Application.UseCases.Loans.Queries;
using Loans.Application.UseCases.Loans.Queries.GetAllLoans;
using Loans.Application.UseCases.Loans.Queries.GetLoanById;
using Loans.Application.UseCases.Loans.Queries.GetMyLoans;
using Loans.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loans.Api.Controllers;

public record CreateLoanRequest(Guid SoftwareId, int Days);
public record ExtendLoanRequest(int ExtraDays);

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/loans
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoanRequest request)
    {
        Guid? userId = User.GetUserId();
        if (userId is null) return Unauthorized();

        CreateLoanResponse loan = await _mediator.Send(
            new CreateLoanCommand(userId.Value, request.SoftwareId, request.Days));

        return Created($"api/loans/{loan.Id}", new
        {
            id = loan.Id,
            licenseId = loan.LicenseId,
            dueDate = loan.DueDate
        });
    }

    // GET /api/loans/mine
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        Guid? userId = User.GetUserId();
        if (userId is null) return Unauthorized();

        List<LoanDTO> loans = await _mediator.Send(new GetMyLoansQuery(userId.Value));
        return Ok(loans);
    }

    // GET /api/loans   (solo Admin)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        List<LoanDTO> loans = await _mediator.Send(new GetAllLoansQuery());
        return Ok(loans);
    }

    // POST /api/loans/{id}/return
    [HttpPost("{id:guid}/return")]
    public async Task<IActionResult> Return(Guid id)
    {
        Guid? userId = User.GetUserId();
        if (userId is null) return Unauthorized();

        // AJUSTAR: ReturnLoanCommand(loanId, userId, isAdmin) : IRequest
        await _mediator.Send(new ReturnLoanCommand(id, userId.Value, User.IsInRole("Admin")));
        return Ok();
    }

    // POST /api/loans/{id}/extend
    [HttpPost("{id:guid}/extend")]
    public async Task<IActionResult> Extend(Guid id, [FromBody] ExtendLoanRequest request)
    {
        Guid? userId = User.GetUserId();
        if (userId is null) return Unauthorized();

        // AJUSTAR: ExtendLoanCommand(loanId, userId, isAdmin, extraDays) : IRequest
        await _mediator.Send(new ExtendLoanCommand(id, userId.Value, User.IsInRole("Admin"), request.ExtraDays));

        LoanDTO loan = await _mediator.Send(new GetLoanByIdQuery(id));
        return Ok(new { dueDate = loan.DueDate });
    }
}
