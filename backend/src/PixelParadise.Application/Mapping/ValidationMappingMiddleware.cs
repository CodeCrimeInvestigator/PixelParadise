using Newtonsoft.Json;
using PixelParadise.Application.Contracts.Responses;
using ValidationException = FluentValidation.ValidationException;
using ILogger = Serilog.ILogger;

namespace PixelParadise.Application.Mapping;

/// <summary>
///     Middleware for mapping validation exceptions to a standardized error response.
/// </summary>
public class ValidationMappingMiddleware(RequestDelegate next, ILogger logger)
{
    private ILogger Logger => logger.ForContext<ValidationMappingMiddleware>();
    /// <summary>
    ///     Invokes the middleware and catches any <see cref="ValidationException" /> thrown during request processing.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException e)
        {
            context.Response.StatusCode = 400;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = e.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };
            
            Logger.Error("Validation failure:{@ValidationResponse}", validationFailureResponse);

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}