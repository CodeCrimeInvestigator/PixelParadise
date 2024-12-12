using Microsoft.EntityFrameworkCore;
using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
///     Represents a repository interface for managing <see cref="Accommodation" /> entities, with additional
///     methods specific to the Accommodation entity type.
/// </summary>
public interface IAccommodationRepository : IRepository<Accommodation>
{
    /// <summary>
    ///     Retrieves a list of Accommodations asynchronously based on given filtering and sorting options.
    /// </summary>
    /// <param name="options">Criteria for filtering and sorting Accommodations.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, returning a paginated list of matching Accommodations.
    /// </returns>
    Task<PaginatedResult<Accommodation>> GetAllAsync(GetAllAccommodationOptions options);

    /// <summary>
    ///     Updates a Accommodation asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the Accommodation.</param>
    /// <param name="Accommodation">The updated Accommodation entity.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Accommodation> UpdateAsync(Guid id, Accommodation Accommodation);
}

/// <summary>
///     Represents a repository for managing <see cref="Accommodation" /> entities using Entity Framework, with methods for
///     retrieving Accommodations by price range and checking for overlapping bookings in a date range.
/// </summary>
public class AccommodationRepository(PixelParadiseContext ctx)
    : Repository<Accommodation>(ctx), IAccommodationRepository
{
    /// <inheritdoc />
    public async Task<PaginatedResult<Accommodation>> GetAllAsync(GetAllAccommodationOptions options)
    {
        var query = ctx.Accommodations.AsQueryable();

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

        var totalCount = await query.CountAsync();
        var skip = (options.Page - 1) * options.PageSize;
        query = query.Skip(skip).Take(options.PageSize);

        return new PaginatedResult<Accommodation>
        {
            Items = await query.ToListAsync(),
            TotalCount = totalCount,
            Page = options.Page,
            PageSize = options.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<Accommodation> UpdateAsync(Guid id, Accommodation Accommodation)
    {
        var existingAccommodation = await ctx.Accommodations.FindAsync(id);

        if (existingAccommodation == null)
            throw new Exception($"Accommodation with ID {id} not found.");

        ctx.Entry(existingAccommodation).CurrentValues.SetValues(Accommodation);
        await ctx.SaveChangesAsync();

        return existingAccommodation;
    }
}