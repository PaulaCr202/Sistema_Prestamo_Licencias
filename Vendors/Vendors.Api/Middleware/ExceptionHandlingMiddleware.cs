using System.Text.Json;
using Vendors.Application.Exceptions;
using Vendors.Domain.Exceptions;

namespace Vendors.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CustomValidationException ex)
        {
            IEnumerable<object> errors = ex.Errors.Select(e => (object)new
            {
                code = $"Validation.{e.PropertyName}",
                description = e.ErrorMessage
            });

            await WriteAsync(context, StatusCodes.Status400BadRequest, errors);
        }
        catch (BussinesRuleException ex)
        {
            await WriteAsync(context, StatusCodes.Status400BadRequest,
                [new { code = "BusinessRule.Violation", description = ex.Message }]);
        }
        catch (NotFoundException ex)
        {
            await WriteAsync(context, StatusCodes.Status404NotFound,
                [new { code = "BusinessRule.NotFound", description = ex.Message }]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);

            await WriteAsync(context, StatusCodes.Status500InternalServerError,
                [new { code = "Unexpected", description = "Ocurrió un error inesperado." }]);
        }
    }

    private static async Task WriteAsync(
        HttpContext context,
        int statusCode,
        IEnumerable<object> errors)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(
            new { errors },
            new JsonSerializerOptions(JsonSerializerDefaults.Web)));
    }
}