using FluentValidation;
using IW5_project.Common.Models.Resources;
namespace IW5_project.Common.Models.Place
{
    public class PlaceDetailModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public int ItemCount { get; init; }
        public string ImageUrl { get; set; } = string.Empty;
    }
    public class PlaceDetailModelValidator : AbstractValidator<PlaceDetailModel>
    {
        public PlaceDetailModelValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(PlaceDetailModelResources.PlaceNameMissingErrorMessage);
            RuleFor(p => p.Name).MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(PlaceDetailModelResources.PlaceNameMaximumLengthExceededErrorMessage);
            RuleFor(p => p.Description).MaximumLength(Constants.MAX_DESCRIPTION_LENGTH).WithMessage(PlaceDetailModelResources.PlaceDescriptionMaximumLengthExceededErrorMessage);
            RuleFor(p => p.ItemCount).GreaterThanOrEqualTo(0).WithMessage(PlaceDetailModelResources.PlaceItemCountNegativeErrorMessage);
        }
    }
}
