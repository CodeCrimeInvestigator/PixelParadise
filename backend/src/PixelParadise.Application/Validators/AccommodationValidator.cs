using FluentValidation;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Infrastructure.Validators;

/// <summary>
///     Validator class for validating rental entities using FluentValidation.
/// </summary>
public class AccommodationValidator : AbstractValidator<Accommodation>
{
    private readonly IUserRepository _userRepository;

    public AccommodationValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(rental => rental.Name)
            .NotEmpty().NotNull()
            .WithMessage("Name cannot be empty.");

        RuleFor(rental => rental.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0.");

        RuleFor(rental => rental.OwnerId)
            .MustAsync(ValidateOwner)
            .WithMessage("User with specified Id does not exist.");
    }

    private async Task<bool> ValidateOwner(Accommodation rental, Guid ownerId, CancellationToken token = default)
    {
        var existingUser = await _userRepository.GetAsync(ownerId);
        if (existingUser != null)
            return existingUser.Id == rental.OwnerId;
        return false;
    }
}