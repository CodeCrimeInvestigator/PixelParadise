using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
/// Represents a repository for managing user-related data.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Asynchronously retrieves a list of rentals owned by the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of rentals owned by the user.</returns>
    Task<List<Rental>> GetUserOwnedRentals(Guid userId);

    /// <summary>
    /// Asynchronously retrieves a list of bookings made by the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of bookings made by the user.</returns>
    Task<List<Booking>> GetUserBookings(Guid userId);
}

/// <summary>
/// Provides methods for accessing user data from the database.
/// </summary>
public class UserRepository(PixelParadiseContext ctx) : Repository<User>(ctx), IUserRepository
{
    /// <inheritdoc />
    public async Task<List<Rental>> GetUserOwnedRentals(Guid userId)
    {
        return await ctx.Users
            .SelectMany(u => u.Rentals)
            .Where(r => r.Owner.Id == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<Booking>> GetUserBookings(Guid userId)
    {
        return await ctx.Users
            .SelectMany(u => u.Bookings)
            .Where(b => b.User.Id == userId)
            .ToListAsync();
    }
}