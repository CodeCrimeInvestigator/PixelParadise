using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Application.Services;

/// <summary>
/// Defines the contract for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously creates a new user.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateUserAsync(User user);

    /// <summary>
    /// Asynchronously retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    Task<User> GetUserAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves all users.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of users.</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Asynchronously updates an existing user.
    /// </summary>
    /// <param name="user">The updated user entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user if successful; otherwise, null.</returns>
    Task<User> UpdateUser(User user);

    /// <summary>
    /// Asynchronously deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
    Task<bool> DeleteUser(Guid id);
}

/// <summary>
/// Provides implementation for user-related operations.
/// </summary>
public class UserService(IUserRepository userRepository) : IUserService
{
    /// <inheritdoc />
    public async Task CreateUserAsync(User user)
    {
        await userRepository.CreateAsync(user);
    }

    /// <inheritdoc />
    public async Task<User> GetUserAsync(Guid id)
    {
        return await userRepository.GetAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllAsync();
    }

    /// <inheritdoc />
    public async Task<User> UpdateUser(User user)
    {
        var userExist = await userRepository.GetAsync(user.Id);
        if (userExist == null)
        {
            return null;
        }

        await userRepository.UpdateAsync(user.Id, user);
        return user;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteUser(Guid id)
    {
        return await userRepository.DeleteAsync(id);
    }
}