using PixelParadise.Application.Services;

namespace PixelParadise.Application.Logging;

/// <summary>
///     Provides extension methods for registering application services in the dependency injection container.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Registers the application services required for business logic into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection" /> with services registered.</returns>
    /// <remarks>
    ///     This method registers the following application services with singleton lifetimes:
    ///     - User-related service (`IUserService`)
    ///     - Rental-related service (`IRentalService`)
    ///     - Booking-related service (`IBookingService`)
    /// </remarks>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IRentalService, RentalService>();
        services.AddSingleton<IBookingService, BookingService>();
        return services;
    }
}