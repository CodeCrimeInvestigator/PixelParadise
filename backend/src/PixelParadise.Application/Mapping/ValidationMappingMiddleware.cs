using PixelParadise.Application.Contracts.Responses;
using ValidationException = FluentValidation.ValidationException;

namespace PixelParadise.Application.Mapping;

/// <summary>
///     Middleware for mapping validation exceptions to a standardized error response.
/// </summary>
public class ValidationMappingMiddleware(RequestDelegate next)
{
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
            var ValidationFailureResponse = new ValidationFailureResponse
            {
                Errors = e.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };
            await context.Response.WriteAsJsonAsync(ValidationFailureResponse);
        }
    }
}