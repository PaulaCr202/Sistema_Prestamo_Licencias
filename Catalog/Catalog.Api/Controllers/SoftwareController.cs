using Catalog.Application.UseCases.Software.Commands.CreateSoftware;
using Catalog.Application.UseCases.Software.Commands.RegisterLicenses;
using Catalog.Application.UseCases.Software.Queries.GetSoftwareByCategory;
using Catalog.Application.UseCases.Software.Queries.GetSoftwareById;
using Catalog.Application.UseCases.Software.Queries.GetSoftwareList;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

public record RegisterLicensesRequest(int Quantity);

[ApiController]
[Route("api/[controller]")]
public class SoftwareController : ControllerBase
{
    private readonly IMediator _mediator;

    public SoftwareController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetSoftwareListQuery());
        return Ok(result);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var software = await _mediator.Send(new GetSoftwareByIdQuery { Id = id });

        return software is null
            ? NotFound(new { error = "El software no existe." })
            : Ok(software);
    }


    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId)
    {
        var result = await _mediator.Send(new GetSoftwareByCategoryQuery { CategoryId = categoryId });
        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSoftwareCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return Created($"api/software/{id}", new { id });
        }
        catch (BussinesRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpPost("{softwareId:guid}/licenses")]
    public async Task<IActionResult> RegisterLicenses(Guid softwareId, [FromBody] RegisterLicensesRequest request)
    {
        try
        {
            var licenseIds = await _mediator.Send(new RegisterLicensesCommand(softwareId, request.Quantity));
            return Ok(new { softwareId, licenseIds });
        }
        catch (BussinesRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}