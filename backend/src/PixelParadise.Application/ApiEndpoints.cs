namespace PixelParadise.Application;

/// <summary>
///     Provides endpoint definitions for the PixelParadise API, organized by resource.
///     This static class centralizes API route definitions to ensure consistent routing throughout the application.
/// </summary>
public static class ApiEndpoints
{
    private const string ApiBase = "";

    public static class Users
    {
        private const string Base = $"{ApiBase}/users";
        public const string Create = Base;
        public const string Get = $"{Base}/{{userId:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{userId:guid}}";
        public const string Delete = $"{Base}/{{userId:guid}}";
    }
}