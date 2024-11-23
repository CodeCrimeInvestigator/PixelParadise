using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Application.Logging;

/// <summary>
///     Provides extension methods for registering repositories in the application's dependency injection container.
/// </summary>
public static class RepositoryRegistrar
{
    /// <summary>
    ///     Registers the repositories required for the application into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which the repositories will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection" /> with repositories registered.</returns>
    /// <remarks>
    ///     This method registers the following repositories with singleton lifetimes:
    ///     - Generic repository for handling all entities (`IRepository)
    ///     - User-related repository (`IUserRepository`)
    ///     - Rental-related repository (`IRentalRepository`)
    ///     - Booking-related repository (`IBookingRepository`)
    /// </remarks>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IRentalRepository, RentalRepository>();
        services.AddSingleton<IBookingRepository, BookingRepository>();
        return services;
    }
}