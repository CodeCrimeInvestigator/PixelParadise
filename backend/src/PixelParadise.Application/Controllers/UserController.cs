using Microsoft.AspNetCore.Mvc;
using PixelParadise.Application.Contracts.Requests;
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
    ///     An <see cref="IActionResult" /> representing the result of the creation operation.
    ///     Returns 201 Created if successful, along with the created user data.
    /// </returns>
    [HttpPost(ApiEndpoints.Users.Create)]
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
    ///     An <see cref="IActionResult" /> containing the user data if found,
    ///     or a 404 Not Found result if the user does not exist.
    [HttpGet(ApiEndpoints.Users.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid userId)
    {
        var user = await userService.GetUserAsync(userId);
        if (user == null) return NotFound();
        var userResponse = user.MapToResponse();
        return Ok(userResponse);
    }

    /// <summary>
    ///     Retrieves all users in the system.
    /// </summary>
    /// <param name="request">The optional request containing filter criteria for user retrieval.</param>
    /// <returns>
    ///     An <see cref="IActionResult" /> containing a list of all users.
    ///     Returns 200 OK with the list of users if successful.
    /// </returns>
    [HttpGet(ApiEndpoints.Users.GetAll)]
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
    ///     An <see cref="IActionResult" /> representing the result of the update operation.
    ///     Returns 200 OK with the updated user data if successful,
    ///     or 404 Not Found if the user does not exist.
    /// </returns>
    [HttpPut(ApiEndpoints.Users.Update)]
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
    ///     An <see cref="IActionResult" /> indicating the success or failure of the deletion operation.
    ///     Returns 200 OK if the user was successfully deleted,
    ///     or 404 Not Found if the user does not exist.
    /// </returns>
    [HttpDelete(ApiEndpoints.Users.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var deleted = await userService.DeleteUser(userId);
        if (!deleted) return NotFound();

        return Ok();
    }
}