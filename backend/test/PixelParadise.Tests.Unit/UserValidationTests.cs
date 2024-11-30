using FluentValidation.TestHelper;
using Moq;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Infrastructure.Validators;

namespace PixelParadise.Test;

public class UserValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserValidator _validator;

    public UserValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new UserValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Username_Is_Empty()
    {
        // Arrange
        var user = new User
        {
            Username = "",
            Nickname = "Nick",
            Age = 20,
            Email = "test@example.com"
        };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Username)
            .WithErrorMessage("'' is not a valid username. Username must not be empty");
    }


    [Fact]
    public async Task Should_Have_Error_When_Username_Too_Short()
    {
        // Arrange
        var user = new User { Username = "", Nickname = "Nick", Age = 20, Email = "test@example.com" };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Username)
            .WithErrorMessage("'' does not meet the minimum length of at least 1 character");
    }

    [Fact]
    public async Task Should_Have_Error_When_Username_Taken()
    {
        // Arrange
        var user = new User { Username = "takenUsername", Nickname = "Nick", Age = 20, Email = "test@example.com" };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("takenUsername"))
            .ReturnsAsync(new User { Id = Guid.NewGuid(), Username = "takenUsername" });

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Username)
            .WithErrorMessage("'takenUsername' is already taken");
    }

    [Fact]
    public async Task Should_Pass_When_Username_Valid()
    {
        // Arrange
        var user = new User { Username = "validUsername", Nickname = "Nick", Age = 20, Email = "test@example.com" };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("validUsername"))
            .ReturnsAsync((User)null); // Simulating that username is not taken

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldNotHaveValidationErrorFor(u => u.Username);
    }

    [Fact]
    public async Task Should_Have_Error_When_Nickname_Too_Long()
    {
        // Arrange
        var user = new User
            { Username = "validUsername", Nickname = new string('a', 256), Age = 20, Email = "test@example.com" };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Nickname)
            .WithErrorMessage(
                "'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa' exceeds the maximum length of 255 characters");
    }

    [Fact]
    public async Task Should_Have_Error_When_Age_Less_Than_18()
    {
        // Arrange
        var user = new User { Username = "validUsername", Nickname = "Nick", Age = 17, Email = "test@example.com" };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Age)
            .WithErrorMessage("'17' is not valid. Age must be greater than or equal to 18");
    }

    [Fact]
    public async Task Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var user = new User { Username = "validUsername", Nickname = "Nick", Age = 20, Email = "" };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Email)
            .WithErrorMessage("'' is not a valid email. Email must not be empty");
    }

    [Fact]
    public async Task Should_Pass_When_All_Fields_Are_Valid()
    {
        // Arrange
        var user = new User { Username = "validUsername", Nickname = "Nick", Age = 20, Email = "test@example.com" };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("validUsername"))
            .ReturnsAsync((User)null);

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        result.ShouldNotHaveValidationErrorFor(u => u.Username);
        result.ShouldNotHaveValidationErrorFor(u => u.Nickname);
        result.ShouldNotHaveValidationErrorFor(u => u.Age);
        result.ShouldNotHaveValidationErrorFor(u => u.Email);
    }
}