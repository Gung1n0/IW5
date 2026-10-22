using FluentValidation;
using IW5_project.Common;
using IW5_project.Common.Models.Resources;

namespace IW5_project.Common.Models.User;

public record UserDetailModel(Guid Id, string Name, string? PhotoUrl, string Role);

public class UserDetailModelValidator : AbstractValidator<UserDetailModel>
{
    public UserDetailModelValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage(UserDetailModelResources.UserNameMissingErrorMessage)
            .MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(UserDetailModelResources.UserNameMaximumLengthExceededErrorMessage);

        RuleFor(u => u.Role)
            .NotEmpty().WithMessage(UserDetailModelResources.UserRoleMissingErrorMessage)
            .Must(role => role == UserRoles.User || role == UserRoles.Admin)
            .WithMessage(UserDetailModelResources.UserRoleInvalidErrorMessage);

        RuleFor(u => u.PhotoUrl)
            .MaximumLength(Constants.MAX_IMAGE_URL_LENGTH)
            .WithMessage(UserDetailModelResources.UserPhotoUrlMaximumLengthExceededErrorMessage)
            .When(u => !string.IsNullOrEmpty(u.PhotoUrl));
    }
}