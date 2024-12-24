using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Application.Validators;
using User = PixelParadise.Domain.Entities.User;

namespace PixelParadise.Test;

public class BookingValidatorTests
{
    private readonly IAccommodationRepository _accommodationRepository = Substitute.For<IAccommodationRepository>();
    private readonly BookingValidator _bookingValidator;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    public BookingValidatorTests()
    {
        _bookingValidator = new BookingValidator(_userRepository, _accommodationRepository);
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnIsValid_WhenNoValidationErrorIsFound()
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

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = owner.Id,
            AccommodationId = accommodation.Id,
            CheckIn = new DateTime(2020, 1, 1),
            CheckOut = new DateTime(2020, 2, 1),
            AmountPaid = 100
        };

        _userRepository.GetAsync(owner.Id).Returns(owner);
        _accommodationRepository.GetAsync(accommodation.Id).Returns(accommodation);

        // Act
        var result = await _bookingValidator.TestValidateAsync(booking);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenUserDoesNotExist()
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
            Id = Guid.Parse("cd212506-f7c3-466c-9220-9df08237bba6"),
            Name = "Test Accommodation",
            Description = "Test Description",
            Price = 10,
            OwnerId = owner.Id
        };

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = owner.Id,
            AccommodationId = accommodation.Id,
            CheckIn = new DateTime(2020, 1, 1),
            CheckOut = new DateTime(2020, 2, 1),
            AmountPaid = 100
        };

        _userRepository.GetAsync(owner.Id).Returns((User)null);
        _accommodationRepository.GetAsync(accommodation.Id).Returns(accommodation);

        // Act
        var result = await _bookingValidator.TestValidateAsync(booking);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(booking => booking.UserId)
            .WithErrorMessage("User with specified Id '5b3cf79e-6f2d-4d84-8aed-7c55f50d8834' does not exist.");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenAccommodationDoesNotExist()
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
            Id = Guid.Parse("cd212506-f7c3-466c-9220-9df08237bba6"),
            Name = "Test Accommodation",
            Description = "Test Description",
            Price = 10,
            OwnerId = owner.Id
        };

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = owner.Id,
            AccommodationId = accommodation.Id,
            CheckIn = new DateTime(2020, 1, 1),
            CheckOut = new DateTime(2020, 2, 1),
            AmountPaid = 100
        };

        _userRepository.GetAsync(owner.Id).Returns(owner);
        _accommodationRepository.GetAsync(accommodation.Id).Returns((Accommodation)null);

        // Act
        var result = await _bookingValidator.TestValidateAsync(booking);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(accommodation => accommodation.AccommodationId)
            .WithErrorMessage("Accommodation with specified Id 'cd212506-f7c3-466c-9220-9df08237bba6' does not exist.");
    }

    [Fact]
    private async Task ValidateAsync_ShouldReturnError_WhenAmountPaidLessThanZero()
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
            Id = Guid.Parse("cd212506-f7c3-466c-9220-9df08237bba6"),
            Name = "Test Accommodation",
            Description = "Test Description",
            Price = 10,
            OwnerId = owner.Id
        };

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = owner.Id,
            AccommodationId = accommodation.Id,
            CheckIn = new DateTime(2020, 1, 1),
            CheckOut = new DateTime(2020, 2, 1),
            AmountPaid = -120
        };

        _userRepository.GetAsync(owner.Id).Returns(owner);
        _accommodationRepository.GetAsync(accommodation.Id).Returns(accommodation);

        // Act
        var result = await _bookingValidator.TestValidateAsync(booking);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(booking => booking.AmountPaid)
            .WithErrorMessage("Amount paid must be greater than or equal to 0.");
    }
}

/*
 private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IAccommodationRepository> _mockAccommodationRepository;
    private readonly BookingValidator _validator;

    public BookingValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockAccommodationRepository = new Mock<IAccommodationRepository>();
        _validator = new BookingValidator(_mockUserRepository.Object, _mockAccommodationRepository.Object);
    }

    [Fact]
    public async Task Validation_ShouldHaveError_WhenUserIdDoesNotExist()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.NewGuid(),
            AccommodationId = Guid.NewGuid(),
            AmountPaid = 100
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.UserId)
            .WithErrorMessage("User with specified Id does not exist.");
    }

    [Fact]
    public async Task Should_Have_Error_When_RentalId_Does_Not_Exist()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.NewGuid(),
            AccommodationId = Guid.NewGuid(),
        };

        _mockAccommodationRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Accommodation)null);

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.AccommodationId)
            .WithErrorMessage("Rental with specified Id does not exist.");
    }


    [Fact]
    public async Task Should_Have_Error_When_AmountPaid_Is_Less_Than_Zero()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.NewGuid(),
            AccommodationId = Guid.NewGuid(),
            AmountPaid = -1
        };

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.AmountPaid)
            .WithErrorMessage("Amount paid must be greater than or equal to 0.");
    }

    [Fact]
    public async Task Should_Pass_When_All_Fields_Are_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accommodationId = Guid.NewGuid();
        var booking = new Booking
        {
            UserId = userId,
            AccommodationId = accommodationId,
            AmountPaid = 100
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(userId))
            .ReturnsAsync(new User { Id = userId });
        _mockAccommodationRepository.Setup(repo => repo.GetAsync(accommodationId))
            .ReturnsAsync(new Accommodation { Id = accommodationId });

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldNotHaveValidationErrorFor(b => b.UserId);
        result.ShouldNotHaveValidationErrorFor(b => b.AccommodationId);
        result.ShouldNotHaveValidationErrorFor(b => b.AmountPaid);
    }

    [Fact]
    public async Task Should_Have_Error_When_UserId_Is_Empty()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.Empty,
            AccommodationId = Guid.NewGuid(),
            AmountPaid = 100
        };

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.UserId)
            .WithErrorMessage("User with specified Id does not exist.");
    }

    [Fact]
    public async Task Should_Have_Error_When_RentalId_Is_Empty()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.NewGuid(),
            AccommodationId = Guid.Empty,
            AmountPaid = 100
        };

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.AccommodationId)
            .WithErrorMessage("Rental with specified Id does not exist.");
    }
    */