using Microsoft.EntityFrameworkCore;

namespace PixelParadise.Infrastructure.Repositories;

/// <summary>
/// Represents a generic repository interface for managing entities.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    Task<T?> GetAsync(Guid id);

    /// <summary>
    /// Asynchronously creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created entity.</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Asynchronously updates an existing entity.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <param name="entity">The updated entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity if successful; otherwise, null.</returns>
    Task<T?> UpdateAsync(Guid id, T entity);

    /// <summary>
    /// Asynchronously deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(Guid id);
}

/// <summary>
/// Represents a generic repository for managing entities using Entity Framework.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public class Repository<T>(PixelParadiseContext ctx) : IRepository<T> where T : class
{
    private DbSet<T> _set => ctx.Set<T>();

    /// <inheritdoc />
    public async Task<T?> GetAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<T> CreateAsync(T entity)
    {
        await _set.AddAsync(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<T?> UpdateAsync(Guid id, T entity)
    {
        var existing = await GetAsync(id);
        if (existing == null)
            return null;

        // Update properties as needed, using current values
        ctx.Entry(existing).CurrentValues.SetValues(entity);
        await ctx.SaveChangesAsync();
        return existing;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await GetAsync(id);
        if (existing is null)
            return false;

        _set.Remove(existing);
        await ctx.SaveChangesAsync();
        return true;
    }
}