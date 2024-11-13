using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Requests;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Services;

namespace PixelParadise.Application.Controllers;

/// <summary>
///     Provides API endpoints for managing user-related operations.
///     This controller handles HTTP requests related to users, such as creating, retrieving, updating, and deleting user
///     information.
/// </summary>
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
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
        var user = request.MapToUser();
        await userService.CreateUserAsync(user);
        var userResponse = user.MapToResponse();
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
        var user = await userService.GetUserAsync(userId);
        if (user == null) return NotFound();
        var userResponse = user.MapToResponse();
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
        var searchOptions = request.MapToOptions();
        var users = await userService.GetAllUsersAsync(searchOptions);
        var usersResponse = users.MapToResponse();
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
        var user = request.MapToUser(userId);
        var updatedUser = await userService.UpdateUser(user);
        if (updatedUser == null) return NotFound();

        var response = user.MapToResponse();
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
        var deleted = await userService.DeleteUser(userId);
        if (!deleted) return NotFound();

        return Ok();
    }
}