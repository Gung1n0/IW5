using FluentValidation;
using IW5_project.Common.Models.Resources;
namespace IW5_project.Common.Models.Place
{
    public class PlaceDetailModel
    {
        public Guid Id { get; init; }
        public string name { get; init; } = null!;
        public string? description { get; init; }
        public int itemCount { get; init; }
        public string imageUrl { get; set; } = string.Empty;
    }
    public class PlaceDetailModelValidator : AbstractValidator<PlaceDetailModel>
    {
        public PlaceDetailModelValidator()
        {
            RuleFor(p => p.name).NotEmpty().WithMessage(PlaceDetailModelResources.NameMissingErrorMessage);
            RuleFor(p => p.name).MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(PlaceDetailModelResources.MaximumLengthExceededErrorMessage);
            RuleFor(p => p.description).MaximumLength(Constants.MAX_DESCRIPTION_LENGTH).WithMessage(PlaceDetailModelResources.DescriptionMaximumLengthExceededErrorMessage);
            RuleFor(p => p.itemCount).GreaterThanOrEqualTo(0).WithMessage(PlaceDetailModelResources.ItemCountNegativeErrorMessage);
        }
    }
}
