using FluentValidation.TestHelper;
using Moq;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Infrastructure.Validators;

namespace PixelParadise.Test;

public class RentalValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly RentalValidator _validator;

    public RentalValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new RentalValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var rental = new Rental
        {
            Name = "",
            Price = 100,
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = await _validator.TestValidateAsync(rental);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Fact]
    public async Task Should_Have_Error_When_Price_Is_Less_Than_Zero()
    {
        // Arrange
        var rental = new Rental
        {
            Name = "Valid Rental",
            Price = -1,
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = await _validator.TestValidateAsync(rental);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Price)
            .WithErrorMessage("Price must be greater than or equal to 0.");
    }

    [Fact]
    public async Task Should_Have_Error_When_Owner_Does_Not_Exist()
    {
        // Arrange
        var rental = new Rental
        {
            Name = "Valid Rental",
            Price = 100,
            OwnerId = Guid.NewGuid()
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _validator.TestValidateAsync(rental);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.OwnerId)
            .WithErrorMessage("User with specified Id does not exist.");
    }

    [Fact]
    public async Task Should_Pass_When_All_Fields_Are_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var rental = new Rental
        {
            Name = "Valid Rental",
            Price = 100,
            OwnerId = userId
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(userId))
            .ReturnsAsync(new User { Id = userId });

        // Act
        var result = await _validator.TestValidateAsync(rental);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Name);
        result.ShouldNotHaveValidationErrorFor(r => r.Price);
        result.ShouldNotHaveValidationErrorFor(r => r.OwnerId);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Null()
    {
        // Arrange
        var rental = new Rental
        {
            Name = null,
            Price = 100,
            OwnerId = Guid.NewGuid()
        };

        // Act
        var result = await _validator.TestValidateAsync(rental);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Name cannot be empty.");
    }
}