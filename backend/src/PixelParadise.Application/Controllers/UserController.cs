using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Requests;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Services;
using ILogger = Serilog.ILogger;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing user-related operations.
///     This controller handles HTTP requests related to users, such as creating, retrieving, updating, and deleting user
///     information.
/// </summary>
[ApiController]
public class UserController(IUserService userService, ILogger logger) : ControllerBase
{
    private ILogger Logger => logger.ForContext<UserController>();

    /// <summary>
    ///     Creates a new user based on the provided request data.
    /// </summary>
    /// <param name="request">The request containing user data for the new user. Must include required fields.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 201 Created and the created user data if successful,
    ///     or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPost(ApiEndpoints.Users.Create)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        Logger.Information("Received request to create user with data: {@RequestData}", request);
        Logger.Debug("Request data: {@RequestData}", request);
        var user = request.MapToUser();

        await userService.CreateUserAsync(user);
        Logger.Information("User successfully created with ID: {UserId}", user.Id);

        var userResponse = user.MapToResponse();
        Logger.Debug("Response data prepared for created user. Response: {@UserResponse}", userResponse);

        return CreatedAtAction(nameof(Get), new { userId = user.Id }, userResponse);
    }

    /// <summary>
    ///     Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the user data if found,
    ///     or 404 Not Found if the user does not exist.
    /// </returns>
    [HttpGet(ApiEndpoints.Users.Get)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid userId)
    {
        Logger.Information("Retrieve user request received. User ID: {UserId}", userId);
        var user = await userService.GetUserAsync(userId);
        if (user == null)
        {
            Logger.Warning("User not found with User ID: {UserId}", userId);
            return NotFound();
        }

        Logger.Information("User found with User ID: {UserId}", userId);
        var userResponse = user.MapToResponse();

        Logger.Debug("Response data prepared for retrieved user. Response: {@UserResponse}", userResponse);
        return Ok(userResponse);
    }

    /// <summary>
    ///     Retrieves all users in the system, with optional filtering criteria.
    /// </summary>
    /// <param name="request">The optional request containing filter criteria for user retrieval.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and a list of users that match the criteria.
    /// </returns>
    [HttpGet(ApiEndpoints.Users.GetAll)]
    [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersRequest? request)
    {
        Logger.Information("Retrieve all users request received. Filter Criteria: {@FilterCriteria}", request);
        var searchOptions = request.MapToOptions();
        var users = await userService.GetAllUsersAsync(searchOptions);

        Logger.Information("Users retrieved successfully with UserCount: {UserCount}", users.TotalCount);
        var usersResponse = users.MapToResponse();

        Logger.Debug("Response data prepared for all users. Response: {@UsersResponse}", usersResponse);
        return Ok(usersResponse);
    }

    /// <summary>
    ///     Updates an existing user based on the provided user ID and request data.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to update.</param>
    /// <param name="request">The request containing updated user data.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK and the updated user data if successful,
    ///     or 404 Not Found if the user does not exist, or 400 Bad Request if validation fails.
    /// </returns>
    [HttpPut(ApiEndpoints.Users.Update)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        Logger.Information("Update user request received. User ID: {UserId}, Request Data: {@RequestData}", userId,
            request);
        var user = request.MapToUser(userId);
        var updatedUser = await userService.UpdateUser(user);
        if (updatedUser == null)
        {
            Logger.Warning("Failed to update. User not found with User ID: {UserId}", userId);
            return NotFound();
        }

        Logger.Information("User updated successfully with User ID: {UserId}", userId);
        var response = user.MapToResponse();

        Logger.Debug("Response data prepared for updated user. Response: {@UserResponse}", response);
        return Ok(response);
    }

    /// <summary>
    ///     Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> with status 200 OK if the user was successfully deleted,
    ///     or 404 Not Found if the user does not exist.
    /// </returns>
    [HttpDelete(ApiEndpoints.Users.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        Logger.Information("Delete user request received with User ID: {UserId}", userId);
        var deleted = await userService.DeleteUser(userId);
        if (!deleted)
        {
            Logger.Warning("Failed to delete. User not found with User ID: {UserId}", userId);
            return NotFound();
        }

        Logger.Information("User deleted successfully with User ID: {UserId}", userId);
        return Ok();
    }
}