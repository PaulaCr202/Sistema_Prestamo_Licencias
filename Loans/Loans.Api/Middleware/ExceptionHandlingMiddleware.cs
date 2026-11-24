using Loans.Application.Exceptions;
using Loans.Domain.Exceptions;
using System.Text.Json;

namespace Loans.Api.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    BussinesRuleException => StatusCodes.Status400BadRequest,
                    ForbiddenException => StatusCodes.Status403Forbidden,
                    NotFoundException => StatusCodes.Status404NotFound,
                    ExternalServiceException => StatusCodes.Status502BadGateway, // antes 503
                    _ => StatusCodes.Status500InternalServerError,
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
        }
    }
}
