using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Application.Validators;

namespace PixelParadise.Test;

public class UserValidatorTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly UserValidator _userValidator;

    public UserValidatorTests()
    {
        _userValidator = new UserValidator(_userRepository);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnIsValid_WhenNoValidationErrorIsFound()
    {
        // Arrange
        var user = new User
        {
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        // Act
        var result = await _userValidator.TestValidateAsync(user);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenUserNameAlreadyTaken()
    {
        // Arrange
        var user = new User
        {
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        var existingUser = new User
        {
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        _userRepository.GetByUsernameAsync(user.Username).Returns(existingUser);

        // Act
        var result = await _userValidator.TestValidateAsync(user);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(user => user.Username)
            .WithErrorMessage("'JohnDoe' is already taken");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnErrors_WhenUserNameDoesNotMeetRequiredLength()
    {
        // Arrange
        var user = new User
        {
            Username = "",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        // Act
        var result = await _userValidator.TestValidateAsync(user);

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(2);
        result.ShouldHaveValidationErrorFor(user => user.Username)
            .WithErrorMessage("'' is not a valid username. Username must not be empty");
        result.ShouldHaveValidationErrorFor(user => user.Username)
            .WithErrorMessage("'' does not meet the minimum length of at least 1 character");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenNickNameIsEmpty()
    {
        // Arrange
        var user = new User
        {
            Username = "JohnDoe",
            Nickname = "",
            Age = 20,
            Email = "test@example.com"
        };

        // Act
        var result = await _userValidator.TestValidateAsync(user);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(user => user.Nickname)
            .WithErrorMessage("'' is not a valid nickname. Nickname must not be empty");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenEmailIsEmpty()
    {
        // Arrange
        var user = new User
        {
            Username = "JohnDoe",
            Nickname = "Johny",
            Age = 20,
            Email = ""
        };

        // Act
        var result = await _userValidator.TestValidateAsync(user);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(user => user.Email)
            .WithErrorMessage("'' is not a valid email. Email must not be empty");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenAgeIsNotValid()
    {
        var user = new User
        {
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 1,
            Email = "test@example.com"
        };

        // Act
        var result = await _userValidator.TestValidateAsync(user);

        // Arrange
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(user => user.Age)
            .WithErrorMessage("'1' is not valid. Age must be greater than or equal to 18");
    }
}