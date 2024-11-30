using FluentValidation.TestHelper;
using Moq;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;
using PixelParadise.Infrastructure.Validators;

namespace PixelParadise.Test;

public class BookingValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IRentalRepository> _mockRentalRepository;
    private readonly BookingValidator _validator;

    public BookingValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockRentalRepository = new Mock<IRentalRepository>();
        _validator = new BookingValidator(_mockUserRepository.Object, _mockRentalRepository.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_UserId_Does_Not_Exist()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.NewGuid(), 
            RentalId = Guid.NewGuid(),
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
            RentalId = Guid.NewGuid(), 
        };

        _mockRentalRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Rental)null); 
        
        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.RentalId)
            .WithErrorMessage("Rental with specified Id does not exist.");
    }


    [Fact]
    public async Task Should_Have_Error_When_AmountPaid_Is_Less_Than_Zero()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.NewGuid(),
            RentalId = Guid.NewGuid(),
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
        var rentalId = Guid.NewGuid(); 
        var booking = new Booking
        {
            UserId = userId,
            RentalId = rentalId,
            AmountPaid = 100
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(userId))
            .ReturnsAsync(new User { Id = userId }); 
        _mockRentalRepository.Setup(repo => repo.GetAsync(rentalId))
            .ReturnsAsync(new Rental { Id = rentalId }); 

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldNotHaveValidationErrorFor(b => b.UserId);
        result.ShouldNotHaveValidationErrorFor(b => b.RentalId);
        result.ShouldNotHaveValidationErrorFor(b => b.AmountPaid);
    }
    
    [Fact]
    public async Task Should_Have_Error_When_UserId_Is_Empty()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = Guid.Empty,
            RentalId = Guid.NewGuid(),
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
            RentalId = Guid.Empty,
            AmountPaid = 100
        };

        // Act
        var result = await _validator.TestValidateAsync(booking);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.RentalId)
            .WithErrorMessage("Rental with specified Id does not exist.");
    }
}
