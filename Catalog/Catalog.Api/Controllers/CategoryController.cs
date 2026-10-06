using Catalog.Application.UseCases.Categories.Commands.CreateCategory;
using Catalog.Application.UseCases.Categories.Queries.GetCategoriesList;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetCategoriesListQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return Created($"api/category/{id}", new { id });
        }
        catch (BussinesRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}