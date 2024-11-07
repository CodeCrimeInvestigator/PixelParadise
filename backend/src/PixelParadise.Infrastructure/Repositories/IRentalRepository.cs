using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
///     Represents a repository interface for managing <see cref="Rental" /> entities, with additional
///     methods specific to the Rental entity type.
/// </summary>
public interface IRentalRepository : IRepository<Rental>
{
    /// <summary>
    ///     Retrieves a list of all <see cref="Rental" /> entities that match the specified filtering options.
    /// </summary>
    /// <param name="options">The filtering and sorting options for retrieving rentals.</param>
    /// <returns>A list of <see cref="Rental" /> entities matching the provided options.</returns>
    Task<List<Rental>> GetAllAsync(GetAllRentalOptions options);
}

/// <summary>
///     Represents a repository for managing <see cref="Rental" /> entities using Entity Framework, with methods for
///     retrieving rentals by price range and checking for overlapping bookings in a date range.
/// </summary>
public class RentalRepository(PixelParadiseContext ctx) : Repository<Rental>(ctx), IRentalRepository
{
    /// <inheritdoc />
    public async Task<List<Rental>> GetAllAsync(GetAllRentalOptions options)
    {
        var query = ctx.Rentals.AsQueryable();

        if (!string.IsNullOrEmpty(options.Name))
            query = query.Where(r => r.Name.Contains(options.Name));

        if (!string.IsNullOrEmpty(options.PriceLowerLimit.ToString()))
            query = query.Where(r => r.Price >= Convert.ToDecimal(options.PriceLowerLimit));

        if (!string.IsNullOrEmpty(options.PriceUpperLimit.ToString()))
            query = query.Where(r => r.Price <= Convert.ToDecimal(options.PriceUpperLimit));

        if (!string.IsNullOrEmpty(options.OwnerUsername))
            query = query.Where(r => r.Owner.Username.Equals(options.OwnerUsername));

        if (!string.IsNullOrEmpty(options.SortField))
            query = options.SortOrder == SortOrder.Descending
                ? query.OrderByDescending(u => EF.Property<object>(u, options.SortField))
                : query.OrderBy(u => EF.Property<object>(u, options.SortField));

        return await query.ToListAsync();
    }
}