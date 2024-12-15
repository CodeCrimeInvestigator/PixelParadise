using FluentValidation;
using Microsoft.Extensions.Options;
using PixelParadise.Application.Options;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Services;

/// <summary>
///     Defines the contract for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    ///     Asynchronously creates a new user.
    /// </summary>
    /// <param name="user">
    ///     The user entity to create. This object must contain valid and unique user data, including a username
    ///     that does not already exist in the system.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created user entity if
    ///     successful; otherwise, an exception may be thrown if validation fails.
    /// </returns>
    /// <remarks>
    ///     If the provided username already exists in the system, a validation exception will be thrown.
    /// </remarks>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    ///     Asynchronously retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the user if found; otherwise,
    ///     null.
    /// </returns>
    Task<User> GetUserAsync(Guid id);

    /// <summary>
    ///     Retrieves all users asynchronously with options for filtering and sorting.
    /// </summary>
    /// <param name="options">The criteria used for filtering and sorting the list of users.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, returning a paginated list of users
    ///     that match the specified options.
    /// </returns>
    Task<PaginatedResult<User>> GetAllUsersAsync(GetAllUsersOptions options);

    /// <summary>
    ///     Asynchronously updates an existing user.
    /// </summary>
    /// <param name="user">The updated user entity.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated user if successful;
    ///     otherwise, null.
    /// </returns>
    Task<User> UpdateUser(User user);

    /// <summary>
    ///     Asynchronously deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result indicates whether the deletion was
    ///     successful.
    /// </returns>
    Task<bool> DeleteUser(Guid id);

    /// <summary>
    ///     Asynchronously uploads a profile picture for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to upload the profile picture for.</param>
    /// <param name="profilePicture">The profile picture file to upload. Can be null to reset the image.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result is true if the upload or reset was successful,
    ///     or false if the user was not found or there was an error during the process.
    /// </returns>
    Task<bool> UploadCoverImageAsync(Guid userId, IFormFile? coverImage);
}

/// <summary>
///     Provides implementation for user-related operations.
/// </summary>
public class UserService(
    IUserRepository userRepository,
    IValidator<User> userValidator,
    IValidator<IFormFile> imageValidator,
    IOptions<StorageOptions> storageOptions) : IUserService
{
    /// <inheritdoc />
    public async Task<User> CreateUserAsync(User user)
    {
        await userValidator.ValidateAndThrowAsync(user);
        return await userRepository.CreateAsync(user);
    }

    /// <inheritdoc />
    public async Task<User> GetUserAsync(Guid id)
    {
        return await userRepository.GetAsync(id);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<User>> GetAllUsersAsync(GetAllUsersOptions options)
    {
        return await userRepository.GetAllAsync(options);
    }

    /// <inheritdoc />
    public async Task<User> UpdateUser(User user)
    {
        await userValidator.ValidateAndThrowAsync(user);
        var userExist = await userRepository.GetAsync(user.Id);
        if (userExist == null) return null;

        await userRepository.UpdateAsync(user.Id, user);
        return user;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteUser(Guid id)
    {
        return await userRepository.DeleteAsync(id);
    }

    /// <inheritdoc />
    public async Task<bool> UploadCoverImageAsync(Guid userId, IFormFile? coverImage)
    {
        var user = await userRepository.GetAsync(userId);
        if (user == null) return false;

        if (coverImage == null || coverImage.Length == 0)
        {
            var imagePath = Path.Combine(storageOptions.Value.AbsStoragePath, user.ProfileImageUrl).Replace("/", "\\");
            if (!string.IsNullOrEmpty(user.ProfileImageUrl) && File.Exists(imagePath))
                File.Delete(imagePath);

            user.ProfileImageUrl = storageOptions.Value.RelDefaultUserImagePath;
            await userRepository.UpdateAsync(user.Id, user);
            return true;
        }

        await imageValidator.ValidateAndThrowAsync(coverImage);

        var fileName = $"{userId}.png";
        var filePath = Path.Combine(storageOptions.Value.AbsStoragePath, "images/user", fileName);

        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await coverImage.CopyToAsync(stream);
        }
        var relativePath = Path.Combine("images/user", fileName).Replace("\\", "/");

        user.ProfileImageUrl = relativePath;
        await userRepository.UpdateAsync(user.Id, user);

        return true;
    }
}