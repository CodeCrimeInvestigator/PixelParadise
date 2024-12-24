using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using PixelParadise.Application.Contracts.Requests;
using PixelParadise.Application.Contracts.Responses;
using PixelParadise.Application.Controllers;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Options;
using PixelParadise.Application.Services;
using PixelParadise.Domain.Entities;
using ILogger = Serilog.ILogger;

namespace PixelParadise.Test.Controllers;

public class UserControllerTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly IOptions<StorageOptions> _options = Substitute.For<IOptions<StorageOptions>>();
    private readonly UserController _userController;
    private readonly IUserMapper _userMapping = Substitute.For<IUserMapper>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserControllerTests()
    {
        _logger.ForContext<UserController>().Returns(_logger);
        _userController = new UserController(_userMapping, _userService, _options, _logger);
    }

    private CreateUserRequest CreateUserRequestExample()
    {
        return new CreateUserRequest
        {
            UserName = "JohnDoe",
            NickName = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };
    }

    private User UserExample(CreateUserRequest createUserRequest)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = createUserRequest.UserName,
            Nickname = createUserRequest.NickName,
            Age = createUserRequest.Age,
            Email = createUserRequest.Email,
            ProfileImageUrl = "/images/default.png"
        };
    }

    private UserResponse UserResponseExample(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.Username,
            NickName = user.Nickname,
            Age = user.Age,
            Email = user.Email,
            ProfileImageUrl = user.ProfileImageUrl
        };
    }

    [Fact]
    private async Task Create_ShouldReturnStatus201Created_WhenNoErrorIsThrown()
    {
        var createUserRequest = CreateUserRequestExample();
        var user = UserExample(createUserRequest);
        var userResponse = UserResponseExample(user);

        _userMapping.MapToUser(createUserRequest, _options).Returns(user);
        _userService.CreateUserAsync(Arg.Any<User>()).Returns(Task.FromResult(user));
        _userMapping.MapToResponse(user).Returns(userResponse);

        // Act
        var result = await _userController.Create(createUserRequest);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(nameof(UserController.Get), createdResult.ActionName);
        Assert.Equal(user.Id, ((UserResponse)createdResult.Value).Id);

        await _userService.Received(1).CreateUserAsync(Arg.Is<User>(u =>
            u.Username == createUserRequest.UserName &&
            u.Nickname == createUserRequest.NickName &&
            u.Email == createUserRequest.Email &&
            u.Age == createUserRequest.Age &&
            u.ProfileImageUrl == "/images/default.png"));

        _logger.Received(1).Information(
            "Received request to create user with data: {@RequestData}",
            Arg.Is<CreateUserRequest>(r =>
                r.UserName == createUserRequest.UserName &&
                r.NickName == createUserRequest.NickName &&
                r.Email == createUserRequest.Email &&
                r.Age == createUserRequest.Age
            )
        );

        _logger.Received(1).Information(
            "User successfully created with ID: {UserId}",
            user.Id
        );
    }

    [Fact]
    private async Task Create_ShouldReturnStatus400BadRequest_WhenValidationErrorIsThrown()
    {
        var createUserRequest = CreateUserRequestExample();
        createUserRequest.Age = 1;

        _userService
            .When(x => x.CreateUserAsync(Arg.Any<User>()))
            .Do(_ => throw new ValidationException("Validation failed", new List<ValidationFailure>
            {
                new("Age", "'1' is not valid. Age must be greater than or equal to 18")
            }));

        IActionResult result;

        // Act
        try
        {
            result = await _userController.Create(createUserRequest);
        }
        catch (ValidationException ex)
        {
            result = new BadRequestObjectResult(new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(e => new ValidationResponse
                {
                    PropertyName = e.PropertyName,
                    Message = e.ErrorMessage
                }).ToList()
            });
        }

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

        var validationResponse = Assert.IsType<ValidationFailureResponse>(badRequestResult.Value);
        Assert.Contains(validationResponse.Errors, e =>
            e is { PropertyName: "Age", Message: "'1' is not valid. Age must be greater than or equal to 18" });
    }
}