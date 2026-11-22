using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendors.Application.UseCases.Contracts.Commands.CreateContract;
using Vendors.Application.UseCases.Contracts.Queries.GetContracts;
using Vendors.Application.UseCases.Contracts.Queries.GetExpiringContracts;
using Vendors.Application.Utilities.Mediator;

namespace Vendors.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Catalog Manager")]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetContractsQuery());

        return result.IsSuccess ? Ok(result.Value) : ToProblem(result);
    }

    [HttpGet("expiring")]
    [Authorize(Roles = "Admin,Catalog Manager")]
    public async Task<IActionResult> GetExpiring([FromQuery] int days = 30)
    {
        var result = await mediator.Send(new GetExpiringContractsQuery { Days = days });

        return result.IsSuccess ? Ok(result.Value) : ToProblem(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateContractCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Created($"api/contracts/{result.Value}", new { id = result.Value })
            : ToProblem(result);
    }
}