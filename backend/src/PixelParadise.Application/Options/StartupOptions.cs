namespace PixelParadise.Application.Options;

/// <summary>
/// Configurations only applied at startup.
/// </summary>
public class StartupOptions
{
    /// <summary>
    ///     Should the swagger ui be reachable with /swagger.
    /// </summary>
    public required bool EnableSwagger { get; init; }

    /// <summary>
    ///     Allow every origin to access the API.
    /// </summary>
    public required bool AllowCors { get; init; }
}