using FluentValidation;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Application.Validators;

/// <summary>
///     Validator class for validating rental entities using FluentValidation.
/// </summary>
public class AccommodationValidator : AbstractValidator<Accommodation>
{
    private readonly IUserRepository _userRepository;

    public AccommodationValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(accommodation => accommodation.Name)
            .NotEmpty().NotNull()
            .WithMessage("'Name' must not be empty.");

        RuleFor(accommodation => accommodation.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0.");

        RuleFor(accommodation => accommodation.OwnerId)
            .MustAsync(ValidateOwner)
            .WithMessage("User with specified Id '{PropertyValue}' does not exist.");
    }

    private async Task<bool> ValidateOwner(Accommodation accommodation, Guid ownerId, CancellationToken token = default)
    {
        var existingUser = await _userRepository.GetAsync(ownerId);
        if (existingUser != null)
            return existingUser.Id == accommodation.OwnerId;
        return false;
    }
}