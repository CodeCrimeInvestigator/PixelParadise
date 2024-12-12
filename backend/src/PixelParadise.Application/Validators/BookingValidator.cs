using FluentValidation;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Infrastructure.Validators;

/// <summary>
///     Validator class for validating booking entities using FluentValidation.
/// </summary>
public class BookingValidator : AbstractValidator<Booking>
{
    private readonly IAccommodationRepository _accommodationRepository;
    private readonly IUserRepository _userRepository;

    public BookingValidator(IUserRepository userRepository, IAccommodationRepository accommodationRepository)
    {
        _userRepository = userRepository;
        _accommodationRepository = accommodationRepository;

        RuleFor(booking => booking.UserId).MustAsync(ValidateUser)
            .WithMessage("User with specified Id does not exist.");

        RuleFor(booking => booking.AccommodationId)
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
        var existingRental = await _accommodationRepository.GetAsync(rentalId);
        if (existingRental != null)
            return existingRental.Id == booking.AccommodationId;
        return false;
    }
}