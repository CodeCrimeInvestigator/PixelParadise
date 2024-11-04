﻿using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
///     Represents a repository for managing user-related data.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    ///     Asynchronously retrieves all users based on the specified options for filtering and sorting.
    /// </summary>
    /// <param name="options">The options for filtering and sorting the user list.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of users matching the criteria.
    /// </returns>
    Task<List<User>> GetAllAsync(GetAllUsersOptions options);

    /// <summary>
    ///     Asynchronously retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve. This must be a unique identifier for the user.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.
    /// </returns>
    /// <remarks>
    ///     If no user exists with the provided username, the method will return null.
    /// </remarks>
    Task<User> GetByUsernameAsync(string username);
}

/// <summary>
///     Provides methods for accessing user data from the database.
/// </summary>
public class UserRepository(PixelParadiseContext ctx) : Repository<User>(ctx), IUserRepository
{
    /// <inheritdoc />
    public async Task<List<User>> GetAllAsync(GetAllUsersOptions options)
    {
        var query = ctx.Users.AsQueryable();

        if (!string.IsNullOrEmpty(options.Username))
            query = query.Where(u => u.Username.Contains(options.Username));

        if (!string.IsNullOrEmpty(options.Nickname))
            query = query.Where(u => u.Nickname != null && u.Nickname.Contains(options.Nickname));

        if (!string.IsNullOrEmpty(options.Email))
            query = query.Where(u => u.Email != null && u.Email.Contains(options.Email));

        if (!string.IsNullOrEmpty(options.SortField))
            query = options.SortOrder == SortOrder.Descending
                ? query.OrderByDescending(u => EF.Property<object>(u, options.SortField))
                : query.OrderBy(u => EF.Property<object>(u, options.SortField));

        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<User> GetByUsernameAsync(string username)
    {
        return await ctx.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));
    }
}