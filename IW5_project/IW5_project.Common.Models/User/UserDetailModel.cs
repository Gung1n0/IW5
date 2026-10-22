using FluentValidation;
using IW5_project.Common;

namespace IW5_project.Common.Models.User;

public record UserDetailModel(Guid Id, string Name, string? PhotoUrl, string Role);

public class UserDetailModelValidator : AbstractValidator<UserDetailModel>
{
    public UserDetailModelValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("User name is required.")
            .MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage($"Name cannot exceed {Constants.MAX_NAME_LENGTH} characters.");

        RuleFor(u => u.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => role == UserRoles.User || role == UserRoles.Admin)
            .WithMessage("Role must be either 'user' or 'admin'.");

        RuleFor(u => u.PhotoUrl)
            .MaximumLength(Constants.MAX_IMAGE_URL_LENGTH)
            .WithMessage($"PhotoUrl cannot exceed {Constants.MAX_IMAGE_URL_LENGTH} characters.")
            .When(u => !string.IsNullOrEmpty(u.PhotoUrl));
    }
}