using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
/// Represents a repository for managing booking-related data.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    /// Asynchronously retrieves a list of users who have booked the specified rental.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of users who have booked the rental.</returns>
    Task<List<User>> GetAllUsersWhoBookedRentalAsync(Guid rentalId);
    
    /// <summary>
    /// Asynchronously retrieves a list of users who have booked the specified rental within a given time period.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental.</param>
    /// <param name="startDate">The start date of the time period.</param>
    /// <param name="endDate">The end date of the time period.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of users who have booked the rental within the specified time period.</returns>
    Task<List<User>> GetAllUsersWhoBookedRentalInTimePeriodAsync(Guid rentalId, DateTime startDate, DateTime endDate);
}

/// <summary>
/// Provides methods for accessing booking data from the database.
/// </summary>
public class BookingRepository(PixelParadiseContext ctx) : Repository<Booking>(ctx), IBookingRepository
{
    /// <inheritdoc />
    public async Task<List<User>> GetAllUsersWhoBookedRentalAsync(Guid rentalId)
    {
        return await ctx.Bookings
            .Where(b => b.RentalId == rentalId)
            .Select(b => b.User)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<User>> GetAllUsersWhoBookedRentalInTimePeriodAsync(Guid rentalId, DateTime startDate,
        DateTime endDate)
    {
        return await ctx.Bookings
            .Where(b => b.RentalId == rentalId && b.CheckIn >= startDate && b.CheckIn <= endDate)
            .Select(b => b.User)
            .ToListAsync();
    }
}