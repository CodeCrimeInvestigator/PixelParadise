using FluentValidation;
using PixelParadise.Domain.Entities;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Infrastructure.Validators;

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
            .MaximumLength(255)
            .MustAsync(ValidateUsername)
            .WithMessage("This Username is already taken");
        RuleFor(user => user.Nickname)
            .NotEmpty()
            .MaximumLength(255);
        RuleFor(user => user.Age)
            .GreaterThanOrEqualTo(18);
        RuleFor(user => user.Email).NotEmpty();
    }

    private async Task<bool> ValidateUsername(User user, string username, CancellationToken token = default)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);

        if (existingUser != null)
            return existingUser.Id == user.Id;
        return existingUser is null;
    }
}