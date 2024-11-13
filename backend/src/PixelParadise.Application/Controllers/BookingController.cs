using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Booking.Requests;
using PixelParadise.Application.Contracts.Booking.Responses;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Services;
using PixelParadise.Domain.Entities;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing booking-related operations.
///     This controller handles HTTP requests related to bookings, such as creating, retrieving, updating, and deleting
///     booking information.
/// </summary>
[ApiController]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    /// <summary>
    ///     Creates a new booking based on the provided request data.
    /// </summary>
    /// <param name="request">The request containing booking data for the new booking. Must include required fields.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 201 Created and the created booking data if successful,
    ///     or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost(ApiEndpoints.Bookings.Create)]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        var booking = request.MapToBooking();
        await bookingService.CreateBookingAsync(booking);
        var bookingResponse = booking.MapToResponse();
        return CreatedAtAction(nameof(Get), new { bookingId = booking.Id }, bookingResponse);
    }

    /// <summary>
    ///     Retrieves a booking by its unique identifier.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to retrieve.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the booking data if found,
    ///     or 404 Not Found if the booking does not exist.
    /// </returns>
    [HttpGet(ApiEndpoints.Bookings.Get)]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid bookingId)
    {
        var booking = await bookingService.GetBookingAsync(bookingId);
        if (booking == null) return NotFound();
        var bookingResponse = booking.MapToResponse();
        return Ok(bookingResponse);
    }

    /// <summary>
    ///     Retrieves all bookings based on the specified filter criteria.
    /// </summary>
    /// <param name="request">The optional request containing filter criteria for booking retrieval.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and a list of bookings that match the criteria.
    /// </returns>
    [HttpGet(ApiEndpoints.Bookings.GetAll)]
    [ProducesResponseType(typeof(BookingsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllBookingRequest request)
    {
        var bookingOptions = request.MapToOptions();
        var bookings = await bookingService.GetAllBookingsAsync(bookingOptions);
        var bookingsResponse = bookings.MapToResponse();
        return Ok(bookingsResponse);
    }

    /// <summary>
    ///     Updates an existing booking based on the provided booking ID and request data.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to update.</param>
    /// <param name="request">The request containing updated booking data, including the booking status.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the updated booking data if successful,
    ///     404 Not Found if the booking does not exist, or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut(ApiEndpoints.Bookings.Update)]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid bookingId, [FromBody] UpdateBookingRequest request)
    {
        //TODO: Make the update logic consistent across controllers!!!
        var booking = await bookingService.GetBookingAsync(bookingId);
        if (booking == null) return NotFound();

        booking.Status = Enum.Parse<BookingStatus>(request.Status);

        var updatedBooking = await bookingService.UpdateBookingAsync(booking);
        if (updatedBooking == null) return NotFound();
        var response = updatedBooking.MapToResponse();
        return Ok(response);
    }

    /// <summary>
    ///     Deletes a booking by its unique identifier.
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking to delete.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the booking was successfully deleted,
    ///     or 404 Not Found if the booking does not exist.
    /// </returns>
    [HttpDelete(ApiEndpoints.Bookings.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid bookingId)
    {
        var deleted = await bookingService.DeleteBookingAsync(bookingId);
        if (!deleted) return NotFound();
        return Ok();
    }
}