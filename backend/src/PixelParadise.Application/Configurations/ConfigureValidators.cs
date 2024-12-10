using FluentValidation;
using PixelParadise.Infrastructure.Validators;

namespace PixelParadise.Application.Logging;

/// <summary>
///     Provides an extension method for registering FluentValidation validators in the dependency injection container.
/// </summary>
public static class ValidatorRegistrar
{
    /// <summary>
    ///     Registers FluentValidation validators from the specified assemblies into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which the validators will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection" /> with the validators registered.</returns>
    /// <remarks>
    ///     This method scans the assemblies containing the validator classes (e.g., <see cref="UserValidator" />,
    ///     <see cref="RentalValidator" />, <see cref="BookingValidator" />)
    ///     and registers them with the specified <see cref="ServiceLifetime.Singleton" />.
    /// </remarks>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserValidator>(ServiceLifetime.Singleton);
        services.AddValidatorsFromAssemblyContaining<RentalValidator>(ServiceLifetime.Singleton);
        services.AddValidatorsFromAssemblyContaining<BookingValidator>(ServiceLifetime.Singleton);
        services.AddValidatorsFromAssemblyContaining<ImageValidator>(ServiceLifetime.Singleton);
        return services;
    }
}