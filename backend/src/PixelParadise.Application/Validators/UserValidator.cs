using FluentValidation;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Application.Validators;

/// <summary>
///     Validator class for validating user entities using FluentValidation.
/// </summary>
public class UserValidator : AbstractValidator<User>
{
    private readonly IUserRepository _userRepository;

    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("'{PropertyValue}' is not a valid username. Username must not be empty")
            .MinimumLength(1)
            .WithMessage("'{PropertyValue}' does not meet the minimum length of at least 1 character")
            .MaximumLength(255)
            .WithMessage("'{PropertyValue}' exceeds the maximum length of 255 characters")
            .MustAsync(ValidateUsername)
            .WithMessage("'{PropertyValue}' is already taken");
        RuleFor(user => user.Nickname)
            .NotEmpty()
            .WithMessage("'{PropertyValue}' is not a valid nickname. Nickname must not be empty")
            .MaximumLength(255)
            .WithMessage("'{PropertyValue}' exceeds the maximum length of 255 characters");

        RuleFor(user => user.Age)
            .GreaterThanOrEqualTo(18)
            .WithMessage("'{PropertyValue}' is not valid. Age must be greater than or equal to 18");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("'{PropertyValue}' is not a valid email. Email must not be empty");
    }

    private async Task<bool> ValidateUsername(User user, string username, CancellationToken token = default)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);

        if (existingUser != null)
            return existingUser.Id == user.Id;
        return existingUser is null;
    }
}