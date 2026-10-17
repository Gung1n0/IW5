using FluentValidation;
using IW5_project.Common;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.Resources;
using IW5_project.Common.Models.ShoppingList;

namespace IW5_project.Common.Models.ShoppingList
{
    public class ShoppingListDetailModel
    {
        public Guid Id { get; init; }
        public string name { get; init; } = null!;
        public string? description { get; init; }
        public string store { get; init; } = null!;
        public IList<ShoppingListItemModel> items { get; init; } = new List<ShoppingListItemModel>();
    }
    
    public class ShoppingListDetailModelValidator : AbstractValidator<ShoppingListDetailModel>
    {
        public ShoppingListDetailModelValidator()
        {
            RuleFor(sl => sl.name).NotEmpty().WithMessage(ShoppingListDetailModelResources.ShoppingListNameMissingErrorMessage);
            RuleFor(sl => sl.name).MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(ShoppingListDetailModelResources.ShoppingListNameMaximumLengthExceededErrorMessage);

            RuleFor(sl => sl.store).NotEmpty().WithMessage(ShoppingListDetailModelResources.ShoppingListStoreMissingErrorMessage);
            RuleFor(sl => sl.store).MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(ShoppingListDetailModelResources.ShoppingListStoreMaximumLengthExceededErrorMessage);

            RuleFor(sl => sl.description).MaximumLength(Constants.MAX_DESCRIPTION_LENGTH).WithMessage(ShoppingListDetailModelResources.ShoppingListDescriptionMaximumLengthExceededErrorMessage);
        }
    }
}