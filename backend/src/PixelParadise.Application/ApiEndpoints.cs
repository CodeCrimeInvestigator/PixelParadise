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
        public const string UpdateImage = $"{Base}/{{userId:guid}}/images";
    }

    public static class Accommodations
    {
        private const string Base = $"{ApiBase}/accommodation";
        public const string Create = Base;
        public const string Get = $"{Base}/{{accommodationId:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{accommodationId:guid}}";
        public const string Delete = $"{Base}/{{accommodationId:guid}}";
        public const string UpdateCoverImage = $"{Base}/{{accommodationId:guid}}/cover-image";
        public const string AddAccommodationImage = $"{Base}/{{accommodationId:guid}}/images";
        public const string RemoveAccommodationImage = $"{Base}/{{accommodationId:guid}}/images/{{imageId}}";
    }

    public static class Bookings
    {
        private const string Base = $"{ApiBase}/bookings";
        public const string Create = Base;
        public const string Get = $"{Base}/{{bookingId:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{bookingId:guid}}";
        public const string Delete = $"{Base}/{{bookingId:guid}}";
    }
}