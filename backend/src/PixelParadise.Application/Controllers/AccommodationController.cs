using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PixelParadise.Application.Contracts.Accommodation.Requests;
using PixelParadise.Application.Contracts.Accommodation.Responses;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Options;
using PixelParadise.Application.Services;
using ILogger = Serilog.ILogger;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing accommodation-related operations.
///     This controller handles HTTP requests related to Accommodations, such as creating, retrieving, updating, and
///     deleting accommodation information.
/// </summary>
[ApiController]
public class AccommodationController(
    IAccommodationService accommodationService,
    IOptions<StorageOptions> options,
    ILogger logger) : ControllerBase
{
    private ILogger Logger => logger.ForContext<AccommodationController>();

    /// <summary>
    ///     Creates new accommodation based on the provided request data.
    /// </summary>
    /// <param name="request">The request containing details for the accommodation to accommodation.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 201 Created and the created accommodation data if successful,
    ///     or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost(ApiEndpoints.Accommodations.Create)]
    [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccommodationRequest request)
    {
        Logger.Information("Received request to create accommodation with data: {@RequestData}", request);
        Logger.Debug("Request data: {@RequestData}", request);
        var accommodation = request.MapToAccommodation(options);

        await accommodationService.CreateAccommodationAsync(accommodation);
        Logger.Information("Accommodation successfully created with ID: {accommodationId}", accommodation.Id);

        var accommodationResponse = accommodation.MapToResponse();
        Logger.Debug("Response data prepared for created accommodation. Response: {@AccommodationResponseResponse}",
            accommodationResponse);

        return CreatedAtAction(nameof(Get), new { accommodationId = accommodation.Id }, accommodationResponse);
    }

    /// <summary>
    ///     Retrieves a accommodation by its unique identifier.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to retrieve.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the accommodation data if found,
    ///     or 404 Not Found if the accommodation does not exist.
    /// </returns>
    [HttpGet(ApiEndpoints.Accommodations.Get)]
    [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid accommodationId)
    {
        Logger.Information("Retrieve accommodation request received. Accommodation ID: {accommodationId}",
            accommodationId);
        var accommodation = await accommodationService.GetAccommodationAsync(accommodationId);
        if (accommodation == null)
        {
            Logger.Warning("Accommodation not found with Accommodation ID: {accommodationId}", accommodationId);
            return NotFound();
        }

        Logger.Information("Accommodation found with Accommodation ID: {accommodationId}", accommodationId);
        var accommodationResponse = accommodation.MapToResponse();

        Logger.Debug("Response data prepared for retrieved accommodation. Response: {@AccommodationResponseResponse}",
            accommodationResponse);
        return Ok(accommodationResponse);
    }

    /// <summary>
    ///     Retrieves all accommodations with optional filtering and sorting.
    /// </summary>
    /// <param name="request">The request containing optional filtering and sorting criteria.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and a list of accommodations that match the criteria.
    /// </returns>
    [HttpGet(ApiEndpoints.Accommodations.GetAll)]
    [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAccommodationsRequest? request)
    {
        Logger.Information("Retrieve all accommodations request received. Filter Criteria: {@FilterCriteria}", request);
        var searchOptions = request.MapToOptions();
        var accommodations = await accommodationService.GetAllAccommodationsAsync(searchOptions);

        Logger.Information("Accommodations retrieved successfully with AccommodationCount: {AccommodationCount}",
            accommodations.TotalCount);
        var AccommodationsResponse = accommodations.MapToResponse();

        Logger.Debug("Response data prepared for all accommodations. Response: {@AccommodationsResponse}",
            AccommodationsResponse);
        return Ok(AccommodationsResponse);
    }

    /// <summary>
    ///     Updates an existing accommodations based on the provided accommodations ID and request data.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodations to update.</param>
    /// <param name="request">The request containing updated accommodations details.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the updated accommodations data if successful,
    ///     or 404 Not Found if the accommodations does not exist, or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut(ApiEndpoints.Accommodations.Update)]
    [ProducesResponseType(typeof(AccommodationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid accommodationId,
        [FromBody] UpdateAccommodationRequest request)
    {
        Logger.Information(
            "Update accommodation request received. Accommodations ID: {accommodationId}, Request Data: {@RequestData}",
            accommodationId, request);
        var accommodation = request.MapToAccommodation(accommodationId);
        var updatedAccommodation = await accommodationService.UpdateAccommodationAsync(accommodation);
        if (updatedAccommodation == null)
        {
            Logger.Warning("Failed to update. Accommodation not found with Accommodation ID: {accommodationId}",
                accommodationId);
            return NotFound();
        }

        Logger.Information("Accommodation updated successfully with Accommodation ID: {accommodationId}",
            accommodationId);
        var response = updatedAccommodation.MapToResponse();

        Logger.Debug("Response data prepared for updated accommodation. Response: {@AccommodationResponse}", response);
        return Ok(response);
    }

    /// <summary>
    ///     Deletes a accommodation by its unique identifier.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to delete.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the accommodation was successfully deleted,
    ///     or 404 Not Found if the accommodation does not exist.
    /// </returns>
    [HttpDelete(ApiEndpoints.Accommodations.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid accommodationId)
    {
        Logger.Information("Delete accommodation request received with Accommodation ID: {accommodationId}",
            accommodationId);
        var deleted = await accommodationService.DeleteAccommodationAsync(accommodationId);
        if (!deleted)
        {
            Logger.Warning("Failed to delete. Accommodation not found with Accommodation ID: {accommodationId}",
                accommodationId);
            return NotFound();
        }

        Logger.Information("Accommodation deleted successfully with Accommodation ID: {accommodationId}",
            accommodationId);
        return Ok();
    }

    /// <summary>
    ///     Uploads a new cover image for a specific accommodation.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to update the cover image for.</param>
    /// <param name="file">The image file to upload as the cover image.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the image was successfully uploaded,
    ///     or 404 Not Found if the accommodation does not exist.
    /// </returns>
    [HttpPost(ApiEndpoints.Accommodations.UpdateCoverImage)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCoverImage(Guid accommodationId, IFormFile? file)
    {
        var wasSuccess = await accommodationService.UploadCoverImageAsync(accommodationId, file);
        if (!wasSuccess)
            return NotFound("Accommodation not found.");
        return Ok();
    }

    /// <summary>
    ///     Adds a new image to the accommodation's image gallery.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to add the image to.</param>
    /// <param name="file">The image file to add to the accommodation's gallery.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the image was successfully added,
    ///     or 404 Not Found if the accommodation does not exist.
    /// </returns>
    [HttpPost(ApiEndpoints.Accommodations.AddAccommodationImage)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddAccommodationImage(Guid accommodationId, IFormFile file)
    {
        var wasSuccess = await accommodationService.AddAccommodationImageAsync(accommodationId, file);
        if (!wasSuccess)
            return NotFound("Accommodation not found.");
        return Ok();
    }

    /// <summary>
    ///     Removes an image from the accommodation's image gallery.
    /// </summary>
    /// <param name="accommodationId">The unique identifier of the accommodation to remove the image from.</param>
    /// <param name="imageName">The unique identifier (GUID) of the image to be removed.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the image was successfully removed,
    ///     or 404 Not Found if the accommodation or image does not exist.
    /// </returns>
    [HttpDelete(ApiEndpoints.Accommodations.RemoveAccommodationImage)]
    public async Task<IActionResult> RemoveAccommodationImage(Guid accommodationId, string imageName)
    {
        var wasSuccess = await accommodationService.RemoveAccommodationImageAsync(accommodationId, imageName);
        if (!wasSuccess)
            return NotFound("Accommodation or image not found.");
        return Ok();
    }
}