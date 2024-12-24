using PixelParadise.Application.Mapping;

namespace PixelParadise.Application.Logging;

public static class ConfigureMappers
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddSingleton<IUserMapper, UserMapper>();
        return services;
    }
}