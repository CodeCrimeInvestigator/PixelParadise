using PixelParadise.Domain.Entities;
using PixelParadise.Domain.Options;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Application.Services;

/// <summary>
///     Defines the contract for booking-related operations.
/// </summary>
public interface IBookingService
{
    /// <summary>
    ///     Asynchronously creates a new booking.
    /// </summary>
    /// <param name="booking">The booking entity to create.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created booking.
    /// </returns>
    Task<Booking> CreateBookingAsync(Booking booking);

    /// <summary>
    ///     Asynchronously retrieves a booking by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the booking.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the booking if found; otherwise, null.
    /// </returns>
    Task<Booking?> GetBookingAsync(Guid id);

    /// <summary>
    ///     Asynchronously retrieves all bookings with optional filtering and sorting.
    /// </summary>
    /// <param name="options">The options for filtering and sorting the booking list.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of bookings matching the
    ///     specified criteria.
    /// </returns>
    Task<List<Booking>> GetAllBookingsAsync(GetAllBookingOptions options);

    /// <summary>
    ///     Asynchronously updates an existing booking.
    /// </summary>
    /// <param name="booking">The updated booking entity.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated booking if successful.
    /// </returns>
    Task<Booking> UpdateBookingAsync(Booking booking);

    /// <summary>
    ///     Asynchronously deletes a booking by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to delete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.
    /// </returns>
    Task<bool> DeleteBookingAsync(Guid id);
}

/// <summary>
///     Provides implementation for booking-related operations.
/// </summary>
public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    /// <inheritdoc />
    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        return await bookingRepository.CreateAsync(booking);
    }

    /// <inheritdoc />
    public async Task<Booking?> GetBookingAsync(Guid id)
    {
        return await bookingRepository.GetAsync(id);
    }

    /// <inheritdoc />
    public async Task<List<Booking>> GetAllBookingsAsync(GetAllBookingOptions options)
    {
        return await bookingRepository.GetAllAsync(options);
    }

    /// <inheritdoc />
    public async Task<Booking> UpdateBookingAsync(Booking booking)
    {
        return await bookingRepository.UpdateAsync(booking.Id, booking);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteBookingAsync(Guid id)
    {
        return await bookingRepository.DeleteAsync(id);
    }
}