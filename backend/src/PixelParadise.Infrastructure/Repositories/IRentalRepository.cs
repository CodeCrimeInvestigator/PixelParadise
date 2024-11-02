using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
/// Represents a repository interface for managing <see cref="Rental"/> entities, with additional 
/// methods specific to the Rental entity type.
/// </summary>
public interface IRentalRepository : IRepository<Rental>
{
    /// <summary>
    /// Asynchronously retrieves rentals that fall within a specified price range.
    /// </summary>
    /// <param name="min">The minimum price of the rentals to retrieve.</param>
    /// <param name="max">The maximum price of the rentals to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of rentals within the specified price range.</returns>
    Task<List<Rental>> GetRentalsInPriceRangeAsync(int min, int max);

    /// <summary>
    /// Asynchronously retrieves bookings for a rental that overlap with a specified date range.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental.</param>
    /// <param name="checkIn">The start date of the date range to check.</param>
    /// <param name="checkOut">The end date of the date range to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of overlapping bookings, or an empty list if none are found.</returns>
    Task<List<Booking>> GetBookingsInDateRangeAsync(Guid rentalId, DateTimeOffset checkIn, DateTimeOffset checkOut);
}

/// <summary>
/// Represents a repository for managing <see cref="Rental"/> entities using Entity Framework, with methods for
/// retrieving rentals by price range and checking for overlapping bookings in a date range.
/// </summary>
public class RentalRepository(PixelParadiseContext ctx) : Repository<Rental>(ctx), IRentalRepository
{
    /// <inheritdoc />
    public async Task<List<Rental>> GetRentalsInPriceRangeAsync(int min, int max)
    {
        return await ctx.Rentals
            .Where(r => r.Price >= min && r.Price <= max)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<Booking>> GetBookingsInDateRangeAsync(Guid rentalId, DateTimeOffset checkIn,
        DateTimeOffset checkOut)
    {
        return await ctx.Rentals
            .Where(r => r.Id == rentalId)
            .SelectMany(r => r.Bookings)
            .Where(b => b.CheckIn < checkOut && b.CheckOut > checkIn)
            .ToListAsync();
    }
}