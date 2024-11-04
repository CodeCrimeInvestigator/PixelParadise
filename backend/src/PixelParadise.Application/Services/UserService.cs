﻿using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories;

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
    ///     A task that represents the asynchronous operation. The task result contains an
    ///     <see cref="EntityCreationResult{User}" /> object, which provides the result of the creation attempt:
    ///     - <c>Success</c>: Indicates whether the user creation was successful.
    ///     - <c>Message</c>: Provides a message regarding the outcome, e.g., success or error message if the username already
    ///     exists.
    ///     - <c>Entity</c>: The created user entity if successful; otherwise, null.
    /// </returns>
    /// <remarks>
    ///     If the provided username already exists in the system, the method will return an unsuccessful result with a message
    ///     indicating that the username is already taken.
    /// </remarks>
    Task<EntityCreationResult<User>> CreateUserAsync(User user);

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
    ///     Asynchronously retrieves all users with optional filtering and sorting.
    /// </summary>
    /// <param name="options">The options for filtering and sorting the user list.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a collection of users
    ///     matching the specified criteria.
    /// </returns>
    Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersOptions options);

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
}

/// <summary>
///     Provides implementation for user-related operations.
/// </summary>
public class UserService(IUserRepository userRepository) : IUserService
{
    /// <inheritdoc />
    public async Task<EntityCreationResult<User>> CreateUserAsync(User user)
    {
        var userExist = await userRepository.GetByUsernameAsync(user.Username);
        if (userExist != null)
            return new EntityCreationResult<User>
            {
                Success = false,
                Message = "User with that username already exists!",
                Entity = null
            };

        var newUser = await userRepository.CreateAsync(user);
        return new EntityCreationResult<User>
        {
            Success = true,
            Message = "User created!",
            Entity = newUser
        };
    }

    /// <inheritdoc />
    public async Task<User> GetUserAsync(Guid id)
    {
        return await userRepository.GetAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersOptions options)
    {
        return await userRepository.GetAllAsync(options);
    }

    /// <inheritdoc />
    public async Task<User> UpdateUser(User user)
    {
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
}