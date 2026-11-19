using Microsoft.AspNetCore.Mvc;
using Vendors.Application.Utilities.Results;

namespace Vendors.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ToProblem(Result result)
    {
        var errors = result.Errors
            .Select(e => new { code = e.Code, description = e.Description })
            .ToList();

        int statusCode = result.Errors.First().Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Business => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return StatusCode(statusCode, new { errors });
    }
}