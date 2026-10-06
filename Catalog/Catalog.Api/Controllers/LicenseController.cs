using Catalog.Application.UseCases.License.GetLicenseList;
using Catalog.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LicenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public LicenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetLicenseListQuery());
        return Ok(result);
    }
}