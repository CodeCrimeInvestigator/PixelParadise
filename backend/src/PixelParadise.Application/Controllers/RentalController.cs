using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Rental.Requests;
using PixelParadise.Application.Contracts.Rental.Responses;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Services;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing rental-related operations.
///     This controller handles HTTP requests related to rentals, such as creating, retrieving, updating, and deleting
///     rental
///     information.
/// </summary>
[ApiController]
public class RentalController(IRentalService rentalService) : ControllerBase
{
    /// <summary>
    ///     Creates a new rental based on the provided request data.
    /// </summary>
    /// <param name="request">The request containing details for the rental to create.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 201 Created and the created rental data if successful,
    ///     or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost(ApiEndpoints.Rentals.Create)]
    [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRentalRequest request)
    {
        var rental = request.MapToRental();
        await rentalService.CreateRentalAsync(rental);
        var rentalResponse = rental.MapToResponse();
        return CreatedAtAction(nameof(Get), new { rentalId = rental.Id }, rentalResponse);
    }

    /// <summary>
    ///     Retrieves a rental by its unique identifier.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental to retrieve.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the rental data if found,
    ///     or 404 Not Found if the rental does not exist.
    /// </returns>
    [HttpGet(ApiEndpoints.Rentals.Get)]
    [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid rentalId)
    {
        var rental = await rentalService.GetRentalAsync(rentalId);
        if (rental == null) return NotFound();
        var rentalResponse = rental.MapToResponse();
        return Ok(rentalResponse);
    }

    /// <summary>
    ///     Retrieves all rentals with optional filtering and sorting.
    /// </summary>
    /// <param name="request">The request containing optional filtering and sorting criteria.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and a list of rentals that match the criteria.
    /// </returns>
    [HttpGet(ApiEndpoints.Rentals.GetAll)]
    [ProducesResponseType(typeof(RentalsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRentalsRequest? request)
    {
        var searchOptions = request.MapToOptions();
        var rentals = await rentalService.GetAllRentalsAsync(searchOptions);
        var rentalsResponse = rentals.MapToResponse();
        return Ok(rentalsResponse);
    }


    /// <summary>
    ///     Updates an existing rental based on the provided rental ID and request data.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental to update.</param>
    /// <param name="request">The request containing updated rental details.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the updated rental data if successful,
    ///     or 404 Not Found if the rental does not exist, or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut(ApiEndpoints.Rentals.Update)]
    [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid rentalId, [FromBody] UpdateRentalRequest request)
    {
        var rental = request.MapToRental(rentalId);
        var updatedRental = await rentalService.UpdateRentalAsync(rental);
        if (updatedRental == null) return NotFound();
        var response = updatedRental.MapToResponse();
        return Ok(response);
    }


    /// <summary>
    ///     Deletes a rental by its unique identifier.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental to delete.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the rental was successfully deleted,
    ///     or 404 Not Found if the rental does not exist.
    /// </returns>
    [HttpDelete(ApiEndpoints.Rentals.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid rentalId)
    {
        var deleted = await rentalService.DeleteRentalAsync(rentalId);
        if (!deleted) return NotFound();
        return Ok();
    }
}