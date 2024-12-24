using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Application.Validators;

namespace PixelParadise.Test;

public class RentalValidatorTests
{
    private readonly AccommodationValidator _accommodationValidator;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    public RentalValidatorTests()
    {
        _accommodationValidator = new AccommodationValidator(_userRepository);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnIsValid_WhenNoValidationErrorIsFound()
    {
        // Arrange
        var owner = new User
        {
            Id = Guid.Parse("5b3cf79e-6f2d-4d84-8aed-7c55f50d8834"),
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        var accommodation = new Accommodation
        {
            Id = Guid.NewGuid(),
            Name = "Test Accommodation",
            Description = "Test Description",
            Price = 10,
            OwnerId = owner.Id
        };

        _userRepository.GetAsync(owner.Id).Returns(owner);

        // Act
        var result = await _accommodationValidator.TestValidateAsync(accommodation);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenOwnerCantBeFound()
    {
        var owner = new User
        {
            Id = Guid.Parse("5b3cf79e-6f2d-4d84-8aed-7c55f50d8834"),
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        var accommodation = new Accommodation
        {
            Id = Guid.NewGuid(),
            Name = "Test Accommodation",
            Description = "Test Description",
            Price = 10,
            OwnerId = owner.Id
        };

        _userRepository.GetAsync(owner.Id).Returns((User)null);

        // Act
        var result = await _accommodationValidator.TestValidateAsync(accommodation);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(accommodation => accommodation.OwnerId)
            .WithErrorMessage("User with specified Id '5b3cf79e-6f2d-4d84-8aed-7c55f50d8834' does not exist.");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenAccommodationNameIsEmpty()
    {
        // Arrange
        var owner = new User
        {
            Id = Guid.Parse("5b3cf79e-6f2d-4d84-8aed-7c55f50d8834"),
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        var accommodation = new Accommodation
        {
            Id = Guid.NewGuid(),
            Name = "",
            Description = "Test Description",
            Price = 10,
            OwnerId = owner.Id
        };

        _userRepository.GetAsync(owner.Id).Returns(owner);

        // Act
        var result = await _accommodationValidator.TestValidateAsync(accommodation);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(accommodation => accommodation.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenAccommodationPriceNegative()
    {
        var owner = new User
        {
            Id = Guid.Parse("5b3cf79e-6f2d-4d84-8aed-7c55f50d8834"),
            Username = "JohnDoe",
            Nickname = "Jonny",
            Age = 20,
            Email = "test@example.com"
        };

        var accommodation = new Accommodation
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Description = "Test Description",
            Price = -1,
            OwnerId = owner.Id
        };

        _userRepository.GetAsync(owner.Id).Returns(owner);

        var result = await _accommodationValidator.TestValidateAsync(accommodation);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(accommodation => accommodation.Price)
            .WithErrorMessage("Price must be greater than or equal to 0.");
    }
}