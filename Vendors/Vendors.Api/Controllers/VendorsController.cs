using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendors.Application.UseCases.Vendors.Commands.CreateVendor;
using Vendors.Application.UseCases.Vendors.Queries.GetVendors;
using Vendors.Application.Utilities.Mediator;

namespace Vendors.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Catalog Manager")]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetVendorsQuery());

        return result.IsSuccess ? Ok(result.Value) : ToProblem(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateVendorCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Created($"api/vendors/{result.Value}", new { id = result.Value })
            : ToProblem(result);
    }
}