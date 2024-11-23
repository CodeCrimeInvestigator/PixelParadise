using Microsoft.OpenApi.Models;
using PixelParadise.Application.Options;

namespace PixelParadise.Application.Logging;

/// <summary>
///     Provides extension methods for registering Swagger configuration in the dependency injection container.
/// </summary>
public static class SwaggerRegistrar
{
    /// <summary>
    ///     Registers Swagger services and configurations into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which the Swagger services will be added.</param>
    /// <param name="startupOptions">
    ///     The <see cref="StartupOptions" /> that contain configuration settings for the application,
    ///     including whether Swagger should be enabled.
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection" /> with Swagger services registered.</returns>
    /// <remarks>
    ///     This method adds the Swagger services conditionally based on the <see cref="StartupOptions.EnableSwagger" /> value.
    ///     If Swagger is enabled, it adds the necessary services for generating the Swagger documentation.
    /// </remarks>
    public static IServiceCollection AddSwagger(this IServiceCollection services, StartupOptions startupOptions)
    {
        if (startupOptions.EnableSwagger)
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PixelParadise API", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, "PixelParadise.xml");
                c.IncludeXmlComments(filePath);
            });
        return services;
    }
}