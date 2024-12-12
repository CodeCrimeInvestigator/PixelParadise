using FluentValidation;
using Microsoft.Extensions.Options;
using PixelParadise.Application.Options;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Services;

/// <summary>
///     Defines the contract for Accommodation-related operations.
/// </summary>
public interface IAccommodationService
{
    /// <summary>
    ///     Asynchronously creates a new Accommodation.
    /// </summary>
    /// <param name="Accommodation">The Accommodation entity to create.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created Accommodation.
    /// </returns>
    Task<Accommodation> CreateAccommodationAsync(Accommodation Accommodation);

    /// <summary>
    ///     Asynchronously retrieves a Accommodation by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Accommodation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the Accommodation if found; otherwise,
    ///     null.
    /// </returns>
    Task<Accommodation?> GetAccommodationAsync(Guid id);

    /// <summary>
    ///     Asynchronously retrieves a paginated result of Accommodations with optional filtering and sorting criteria.
    /// </summary>
    /// <param name="options">The options for filtering, sorting, and pagination of the Accommodations list.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResult{T}" />
    ///     of Accommodations that match the specified criteria.
    /// </returns>
    Task<PaginatedResult<Accommodation>> GetAllAccommodationsAsync(GetAllAccommodationOptions options);

    /// <summary>
    ///     Asynchronously updates an existing Accommodation.
    /// </summary>
    /// <param name="Accommodation">The updated Accommodation entity.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated Accommodation if
    ///     successful.
    /// </returns>
    Task<Accommodation> UpdateAccommodationAsync(Accommodation Accommodation);

    /// <summary>
    ///     Asynchronously deletes a Accommodation by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Accommodation to delete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.
    /// </returns>
    Task<bool> DeleteAccommodationAsync(Guid id);

    /// <summary>
    ///     Asynchronously uploads a cover image for accommodation.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to upload the cover image for.</param>
    /// <param name="coverImage">
    ///     The cover image file to upload. If null, the existing cover image will be reset to the default image.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result is true if the upload or reset was successful,
    ///     or false if the accommodation was not found.
    /// </returns>
    Task<bool> UploadCoverImageAsync(Guid accommodationId, IFormFile? coverImage);

    /// <summary>
    ///     Asynchronously adds a new image to the accommodation's gallery.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to add the image to.</param>
    /// <param name="image">The image file to add to the gallery.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result is true if the image was successfully added,
    ///     or false if the accommodation was not found.
    /// </returns>
    Task<bool> AddAccommodationImageAsync(Guid accommodationId, IFormFile image);

    /// <summary>
    ///     Asynchronously removes an image from the accommodation's gallery.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to remove the image from.</param>
    /// <param name="imagePath">
    ///     The path or identifier of the image to be removed. The path must match an existing entry in the accommodation's
    ///     image list.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result is true if the image was successfully removed,
    ///     or false if the accommodation or image was not found.
    /// </returns>
    Task<bool> RemoveAccommodationImageAsync(Guid accommodationId, string imagePath);
}

/// <summary>
///     Provides implementation for Accommodation-related operations.
/// </summary>
public class AccommodationService(
    IAccommodationRepository accommodationRepository,
    IValidator<Accommodation> accommodationValidator,
    IValidator<IFormFile> imageValidator,
    IOptions<StorageOptions> storageOptions) : IAccommodationService
{
    /// <inheritdoc />
    public async Task<Accommodation> CreateAccommodationAsync(Accommodation Accommodation)
    {
        await accommodationValidator.ValidateAndThrowAsync(Accommodation);
        return await accommodationRepository.CreateAsync(Accommodation);
        ;
    }

    /// <inheritdoc />
    /// SS
    public async Task<Accommodation?> GetAccommodationAsync(Guid id)
    {
        return await accommodationRepository.GetAsync(id);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Accommodation>> GetAllAccommodationsAsync(GetAllAccommodationOptions options)
    {
        return await accommodationRepository.GetAllAsync(options);
    }

    /// <inheritdoc />
    public async Task<Accommodation> UpdateAccommodationAsync(Accommodation Accommodation)
    {
        await accommodationValidator.ValidateAndThrowAsync(Accommodation);
        return await accommodationRepository.UpdateAsync(Accommodation.Id, Accommodation);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAccommodationAsync(Guid id)
    {
        return await accommodationRepository.DeleteAsync(id);
    }

    /// <inheritdoc />
    public async Task<bool> UploadCoverImageAsync(Guid accommodationId, IFormFile? coverImage)
    {
        var accommodation = await accommodationRepository.GetAsync(accommodationId);
        if (accommodation == null) return false;

        if (coverImage == null || coverImage.Length == 0)
        {
            if (!string.IsNullOrEmpty(accommodation.CoverImage) && File.Exists(accommodation.CoverImage))
                File.Delete(accommodation.CoverImage);

            accommodation.CoverImage = storageOptions.Value.DefaultAccommodationCoverImagePath;
            await accommodationRepository.UpdateAsync(accommodation.Id, accommodation);
            return true;
        }

        await imageValidator.ValidateAndThrowAsync(coverImage);

        var fileName = $"{accommodationId}.png";
        var filePath = Path.Combine(storageOptions.Value.StorageFolderPath, "accommodation-images", fileName);

        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await coverImage.CopyToAsync(stream);
        }

        accommodation.CoverImage = filePath;
        await accommodationRepository.UpdateAsync(accommodation.Id, accommodation);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> AddAccommodationImageAsync(Guid accommodationId, IFormFile image)
    {
        var accommodation = await accommodationRepository.GetAsync(accommodationId);
        if (accommodation == null) return false;

        await imageValidator.ValidateAndThrowAsync(image);

        var fileName = $"{accommodationId}.png";
        var filePath = Path.Combine(storageOptions.Value.StorageFolderPath, "accommodation-images", fileName);

        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        accommodation.Images.Add(filePath);
        await accommodationRepository.UpdateAsync(accommodation.Id, accommodation);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> RemoveAccommodationImageAsync(Guid accommodationId, string imageId)
    {
        var accommodation = await accommodationRepository.GetAsync(accommodationId);
        if (accommodation == null) return false;

        var imageName = $"{imageId}.png";
        var imagePath = Path.Combine(storageOptions.Value.StorageFolderPath, "accommodation-images", imageName);


        if (!accommodation.Images.Contains(imagePath))
            return false;

        if (File.Exists(imagePath))
            File.Delete(imagePath);

        accommodation.Images.Remove(imagePath);
        await accommodationRepository.UpdateAsync(accommodation.Id, accommodation);

        return true;
    }
}