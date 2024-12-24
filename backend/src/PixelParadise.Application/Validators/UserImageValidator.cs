using FluentValidation;

namespace PixelParadise.Application.Validators;

/// <summary>
///     Validator class for validating profile picture uploads.
/// </summary>
public class ImageValidator : AbstractValidator<IFormFile>
{
    private readonly int _maxFileSizeInBytes;

    //TODO: fix that propertyName is "", rework logic to show properties like "userProfPic"

    public ImageValidator(int maxFileSizeInBytes = 5 * 1024 * 1024)
    {
        _maxFileSizeInBytes = maxFileSizeInBytes;

        RuleFor(file => file)
            .NotNull().WithMessage("File is required.")
            .Must(BeAValidExtension).WithMessage("Invalid file type. Only jpg, jpeg, and png are allowed.")
            .Must(BeValidSize).WithMessage($"File size must be less than {_maxFileSizeInBytes / 1024 / 1024} MB.");
    }

    /// <summary>
    ///     Validates that the file extension is one of the allowed types.
    /// </summary>
    private bool BeAValidExtension(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
        return allowedExtensions.Contains(fileExtension);
    }

    /// <summary>
    ///     Validates that the file size is within the specified limit.
    /// </summary>
    private bool BeValidSize(IFormFile file)
    {
        return file.Length <= _maxFileSizeInBytes;
    }
}