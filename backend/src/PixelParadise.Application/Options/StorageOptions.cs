namespace PixelParadise.Application.Options;

/// <summary>
///     Configuration settings for file storage paths related to user and accommodation images.
/// </summary>
public class StorageOptions
{
    public string AbsStoragePath { get; set; }
    public string RelDefaultUserImagePath { get; set; }
    public string RelDefaultAccommodationCoverImagePath { get; set; }
}