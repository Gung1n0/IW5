using FluentValidation;
using IW5_project.Common.Models.Resources;

namespace IW5_project.Common.Models.Item
{
    public class ItemDetailModelValidator : AbstractValidator<ItemDetailModel>
    {
        public ItemDetailModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ItemDetailModelResources.ItemNameMissingErrorMessage)
                .MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(ItemDetailModelResources.ItemNameMaximumLengthExceededErrorMessage);

            RuleFor(x => x.Description)
                .MaximumLength(Constants.MAX_DESCRIPTION_LENGTH).WithMessage(ItemDetailModelResources.ItemDescriptionMaximumLengthExceededErrorMessage);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(Constants.MAX_IMAGE_URL_LENGTH).WithMessage(ItemDetailModelResources.ItemUrlMaximumLenghtExceededErrorMessage);

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage(ItemDetailModelResources.ItemQuantityPositiveErrorMessage);

            RuleFor(x => x.PlaceId)
                .NotEqual(Guid.Empty).WithMessage(ItemDetailModelResources.ItemIdEmptyErrorMessage);
        }
    }
}
