using Catalog.Application.UseCases.License.Commands.ReleaseLicense;
using Catalog.Application.UseCases.License.GetLicenseList;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpPost("{licenseId:guid}/release")]
    public async Task<IActionResult> Release(Guid licenseId)
    {
        try
        {
            await _mediator.Send(new ReleaseLicenseCommand(licenseId));
            return Ok();
        }
        catch (BussinesRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}