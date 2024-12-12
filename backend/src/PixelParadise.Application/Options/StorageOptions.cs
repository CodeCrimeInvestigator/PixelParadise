namespace PixelParadise.Application.Options;

/// <summary>
///     Configuration settings for file storage paths related to user and accommodation images.
/// </summary>
public class StorageOptions
{
    /// <summary>
    ///     The root directory path where all user and accommodation-related images are stored.
    /// </summary>
    public string StorageFolderPath { get; set; }

    /// <summary>
    ///     The file path to the default user image used when a user has not uploaded a profile picture.
    /// </summary>
    public string DefaultUserImagePath { get; set; }

    /// <summary>
    ///     The file path to the default cover image for accommodations that do not have a user-uploaded image.
    /// </summary>
    public string DefaultAccommodationCoverImagePath { get; set; }
}