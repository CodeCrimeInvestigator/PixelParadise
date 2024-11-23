using Serilog;

namespace PixelParadise.Application.Logging;

/// <summary>
///     Provides extension methods for configuring logging in the application.
/// </summary>
public static class LoggingRegister
{
    /// <summary>
    ///     Configures Serilog as the logging provider for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which the logging services will be added.</param>
    /// <param name="configuration">The <see cref="IConfiguration" /> used to configure the Serilog logger.</param>
    /// <returns>The updated <see cref="IServiceCollection" /> with logging services registered.</returns>
    /// <remarks>
    ///     This method sets up Serilog as the logging provider, replacing any default logging providers.
    ///     It reads configuration settings from the application's configuration (e.g., appsettings.json) and
    ///     registers the Serilog logger as a singleton in the dependency injection container.
    /// </remarks>
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        return services;
    }
}