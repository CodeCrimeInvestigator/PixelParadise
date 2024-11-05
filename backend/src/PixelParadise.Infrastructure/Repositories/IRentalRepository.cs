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
    Task<List<Rental>> GetAllAsync(GetAllRentalOptions options);
}

/// <summary>
///     Represents a repository for managing <see cref="Rental" /> entities using Entity Framework, with methods for
///     retrieving rentals by price range and checking for overlapping bookings in a date range.
/// </summary>
public class RentalRepository(PixelParadiseContext ctx) : Repository<Rental>(ctx), IRentalRepository
{
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

        return await query.ToListAsync();
    }
}