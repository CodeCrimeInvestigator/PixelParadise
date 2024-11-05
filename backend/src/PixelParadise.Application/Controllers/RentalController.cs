using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Rental.Requests;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Services;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing rental-related operations.
///     This controller handles HTTP requests related to users, such as creating, retrieving, updating, and deleting user
///     information.
/// </summary>
[ApiController]
public class RentalController(IRentalService rentalService) : ControllerBase
{
    /// <summary>
    ///     Creates a new rental.
    /// </summary>
    /// <param name="request">The request containing details for the rental to create.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an <see cref="IActionResult" />
    ///     that indicates the result of the operation.
    /// </returns>
    [HttpPost(ApiEndpoints.Rentals.Create)]
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
    ///     A task that represents the asynchronous operation. The task result is an <see cref="IActionResult" />
    ///     that contains the rental if found; otherwise, a <see cref="NotFoundResult" />.
    /// </returns>
    [HttpGet(ApiEndpoints.Rentals.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid rentalId)
    {
        var rental = await rentalService.GetRentalAsync(rentalId);
        if (rental == null) return NotFound();
        var rentalResponse = rental.MapToResponse();
        return Ok(rentalResponse);
    }
    
    /// <summary>
    ///     Retrieves all rentals, with optional filtering and sorting.
    /// </summary>
    /// <param name="request">The request containing optional filtering and sorting criteria.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an <see cref="IActionResult" />
    ///     that contains a list of rentals.
    /// </returns>
    [HttpGet(ApiEndpoints.Rentals.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRentalsRequest? request)
    {
        var searchOptions = request.MapToOptions();
        var rentals = await rentalService.GetAllRentalsAsync(searchOptions);
        var rentalsResponse = rentals.MapToResponse();
        return Ok(rentalsResponse);
    }

    /// <summary>
    ///     Updates an existing rental by its unique identifier.
    /// </summary>
    /// <param name="rentalId">The unique identifier of the rental to update.</param>
    /// <param name="request">The request containing updated rental details.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an <see cref="IActionResult" />
    ///     that contains the updated rental if successful; otherwise, a <see cref="NotFoundResult" />.
    /// </returns>
    [HttpPut(ApiEndpoints.Rentals.Update)]
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
    ///     A task that represents the asynchronous operation. The task result is an <see cref="IActionResult" />
    ///     that indicates whether the deletion was successful.
    /// </returns>
    [HttpDelete(ApiEndpoints.Rentals.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid rentalId)
    {
        var deleted = await rentalService.DeleteRentalAsync(rentalId);
        if (!deleted) return NotFound();
        return Ok();
    }
}