using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Rental.Requests;
using PixelParadise.Application.Contracts.Rental.Responses;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Services;
using ILogger = Serilog.ILogger;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing rental-related operations.
///     This controller handles HTTP requests related to rentals, such as creating, retrieving, updating, and deleting
///     rental
///     information.
/// </summary>
[ApiController]
public class RentalController(IRentalService rentalService, ILogger logger) : ControllerBase
{
    private ILogger Logger => logger.ForContext<RentalController>();

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
        Logger.Information("Received request to create rental with data: {@RequestData}", request);
        Logger.Debug("Request data: {@RequestData}", request);
        var rental = request.MapToRental();

        await rentalService.CreateRentalAsync(rental);
        Logger.Information("Rental successfully created with ID: {RentalId}", rental.Id);

        var rentalResponse = rental.MapToResponse();
        Logger.Debug("Response data prepared for created rental. Response: {@RentalResponse}", rentalResponse);

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
        Logger.Information("Retrieve rental request received. Rental ID: {RentalId}", rentalId);
        var rental = await rentalService.GetRentalAsync(rentalId);
        if (rental == null)
        {
            Logger.Warning("Rental not found with Rental ID: {RentalId}", rentalId);
            return NotFound();
        }

        Logger.Information("Rental found with Rental ID: {RentalId}", rentalId);
        var rentalResponse = rental.MapToResponse();

        Logger.Debug("Response data prepared for retrieved rental. Response: {@RentalResponse}", rentalResponse);
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
        Logger.Information("Retrieve all rentals request received. Filter Criteria: {@FilterCriteria}", request);
        var searchOptions = request.MapToOptions();
        var rentals = await rentalService.GetAllRentalsAsync(searchOptions);

        Logger.Information("Rentals retrieved successfully with RentalCount: {RentalCount}", rentals.TotalCount);
        var rentalsResponse = rentals.MapToResponse();

        Logger.Debug("Response data prepared for all rentals. Response: {@RentalsResponse}", rentalsResponse);
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
        Logger.Information("Update rental request received. Rental ID: {RentalId}, Request Data: {@RequestData}",
            rentalId, request);
        var rental = request.MapToRental(rentalId);
        var updatedRental = await rentalService.UpdateRentalAsync(rental);
        if (updatedRental == null)
        {
            Logger.Warning("Failed to update. Rental not found with Rental ID: {RentalId}", rentalId);
            return NotFound();
        }

        Logger.Information("Rental updated successfully with Rental ID: {RentalId}", rentalId);
        var response = updatedRental.MapToResponse();

        Logger.Debug("Response data prepared for updated rental. Response: {@RentalResponse}", response);
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
        Logger.Information("Delete rental request received with Rental ID: {RentalId}", rentalId);
        var deleted = await rentalService.DeleteRentalAsync(rentalId);
        if (!deleted)
        {
            Logger.Warning("Failed to delete. Rental not found with Rental ID: {RentalId}", rentalId);
            return NotFound();
        }

        Logger.Information("Rental deleted successfully with Rental ID: {RentalId}", rentalId);
        return Ok();
    }
}