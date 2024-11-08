using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
///     Represents a repository for managing booking-related data.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    Task<List<Booking>> GetAllAsync(GetAllBookingOptions options);
}

/// <summary>
///     Provides methods for accessing booking data from the database.
/// </summary>
public class BookingRepository(PixelParadiseContext ctx) : Repository<Booking>(ctx), IBookingRepository
{
    public async Task<List<Booking>> GetAllAsync(GetAllBookingOptions options)
    {
        var query = ctx.Bookings.AsQueryable();

        if (options.RentalId != null)
            query = query.Where(b => b.RentalId.Equals(options.RentalId));

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

        return await query.ToListAsync();
    }
}