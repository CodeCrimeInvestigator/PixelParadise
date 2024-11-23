using Microsoft.EntityFrameworkCore;
using PixelParadise.Application.Options;
using PixelParadise.Infrastructure;

namespace PixelParadise.Application.Logging;

/// <summary>
///     Provides extension methods to configure database-related services.
/// </summary>
public static class ConfigureDatabase
{
    /// <summary>
    ///     Adds and configures the database context for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the DbContext to.</param>
    /// <param name="postgreSqlOptions">The <see cref="PostgreSqlOptions" /> containing PostgreSQL configuration settings.</param>
    /// <returns>The updated <see cref="IServiceCollection" /> with the DbContext registered.</returns>
    /// <remarks>
    ///     This method registers the <see cref="PixelParadiseContext" /> as a service with the dependency injection container
    ///     using PostgreSQL as the database provider. The connection string is retrieved from the
    ///     <paramref name="postgreSqlOptions" />.
    /// </remarks>
    public static IServiceCollection AddDbContext(this IServiceCollection services, PostgreSqlOptions postgreSqlOptions)
    {
        services.AddDbContext<PixelParadiseContext>(options =>
            options.UseNpgsql(postgreSqlOptions.GetConnectionString), ServiceLifetime.Singleton);
        return services;
    }
}