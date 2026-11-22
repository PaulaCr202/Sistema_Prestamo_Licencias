using Loans.Application.Exceptions;
using Loans.Domain.Exceptions;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Loans.Api.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        // Deja las tildes legibles en el JSON ("está" en lugar de "est\u00E1")
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

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
                    NotFoundException => StatusCodes.Status404NotFound,
                    ForbiddenException => StatusCodes.Status403Forbidden,
                    ExternalServiceException => StatusCodes.Status503ServiceUnavailable,
                    _ => StatusCodes.Status500InternalServerError,
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }, JsonOptions));
            }
        }
    }
}