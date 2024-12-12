using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
///     Represents a repository for managing booking-related data.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    ///     Retrieves a list of bookings asynchronously based on given filtering and sorting options.
    /// </summary>
    /// <param name="options">Criteria for filtering and sorting bookings.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, returning a paginated list of matching bookings.
    /// </returns>
    Task<PaginatedResult<Booking>> GetAllAsync(GetAllBookingOptions options);
}

/// <summary>
///     Provides methods for accessing booking data from the database.
/// </summary>
public class BookingRepository(PixelParadiseContext ctx) : Repository<Booking>(ctx), IBookingRepository
{
    /// <inheritdoc />
    public async Task<PaginatedResult<Booking>> GetAllAsync(GetAllBookingOptions options)
    {
        var query = ctx.Bookings.AsQueryable();

        if (options.AccommodationId != null)
            query = query.Where(b => b.AccommodationId.Equals(options.AccommodationId));

        if (options.UserId != null)
            query = query.Where(b => b.UserId.Equals(options.UserId));

        if (options.CheckIn != null)
            query = query.Where(r => r.CheckIn >= options.CheckIn);

        if (options.CheckOut != null)
            query = query.Where(r => r.CheckOut <= options.CheckOut);

        if (!options.Status.Equals(BookingStatus.All))
            query = query.Where(r => r.Status.Equals(options.Status));

        if (!string.IsNullOrEmpty(options.SortField))
            query = options.SortOrder == SortOrder.Descending
                ? query.OrderByDescending(u => EF.Property<object>(u, options.SortField))
                : query.OrderBy(u => EF.Property<object>(u, options.SortField));

        var totalCount = await query.CountAsync();
        var skip = (options.Page - 1) * options.PageSize;
        query = query.Skip(skip).Take(options.PageSize);

        return new PaginatedResult<Booking>
        {
            Items = await query.ToListAsync(),
            TotalCount = totalCount,
            Page = options.Page,
            PageSize = options.PageSize
        };
    }
}