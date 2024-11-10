using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Infrastructure.Repositories.Results;

namespace PixelParadise.Application.Services;

/// <summary>
///     Defines the contract for rental-related operations.
/// </summary>
public interface IRentalService
{
    /// <summary>
    ///     Asynchronously creates a new rental.
    /// </summary>
    /// <param name="rental">The rental entity to create.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created rental.
    /// </returns>
    Task<Rental> CreateRentalAsync(Rental rental);

    /// <summary>
    ///     Asynchronously retrieves a rental by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the rental.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the rental if found; otherwise, null.
    /// </returns>
    Task<Rental?> GetRentalAsync(Guid id);

    /// <summary>
    ///     Asynchronously retrieves a paginated result of rentals with optional filtering and sorting criteria.
    /// </summary>
    /// <param name="options">The options for filtering, sorting, and pagination of the rentals list.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResult{T}" />
    ///     of rentals that match the specified criteria.
    /// </returns>
    Task<PaginatedResult<Rental>> GetAllRentalsAsync(GetAllRentalOptions options);

    /// <summary>
    ///     Asynchronously updates an existing rental.
    /// </summary>
    /// <param name="rental">The updated rental entity.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated rental if successful.
    /// </returns>
    Task<Rental> UpdateRentalAsync(Rental rental);

    /// <summary>
    ///     Asynchronously deletes a rental by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the rental to delete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.
    /// </returns>
    Task<bool> DeleteRentalAsync(Guid id);
}

/// <summary>
///     Provides implementation for rental-related operations.
/// </summary>
public class RentalService(IRentalRepository repository) : IRentalService
{
    /// <inheritdoc />
    public async Task<Rental> CreateRentalAsync(Rental rental)
    {
        return await repository.CreateAsync(rental);
    }

    /// <inheritdoc />
    public async Task<Rental?> GetRentalAsync(Guid id)
    {
        return await repository.GetAsync(id);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Rental>> GetAllRentalsAsync(GetAllRentalOptions options)
    {
        return await repository.GetAllAsync(options);
    }

    /// <inheritdoc />
    public async Task<Rental> UpdateRentalAsync(Rental rental)
    {
        return await repository.UpdateAsync(rental.Id, rental);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteRentalAsync(Guid id)
    {
        return await repository.DeleteAsync(id);
    }
}