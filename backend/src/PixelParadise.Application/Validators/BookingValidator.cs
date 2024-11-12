using FluentValidation;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Infrastructure.Validators;

/// <summary>
///     Validator class for validating booking entities using FluentValidation.
/// </summary>
public class BookingValidator : AbstractValidator<Booking>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUserRepository _userRepository;

    public BookingValidator(IUserRepository userRepository, IRentalRepository rentalRepository)
    {
        _userRepository = userRepository;
        _rentalRepository = rentalRepository;

        RuleFor(booking => booking.UserId).MustAsync(ValidateUser)
            .WithMessage("User with specified Id does not exist.");

        RuleFor(booking => booking.RentalId)
            .MustAsync(ValidateRental).WithMessage("Rental with specified Id does not exist.");

        RuleFor(booking => booking.AmountPaid)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount paid must be greater than or equal to 0.");
    }

    private async Task<bool> ValidateUser(Booking booking, Guid userId, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetAsync(userId);
        if (existingUser != null)
            return existingUser.Id == booking.UserId;
        return false;
    }

    private async Task<bool> ValidateRental(Booking booking, Guid rentalId,
        CancellationToken cancellationToken = default)
    {
        var existingRental = await _rentalRepository.GetAsync(rentalId);
        if (existingRental != null)
            return existingRental.Id == booking.UserId;
        return false;
    }
}