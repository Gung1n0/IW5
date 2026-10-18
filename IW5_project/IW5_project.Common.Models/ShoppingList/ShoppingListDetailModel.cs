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
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public string Store { get; init; } = null!;
        public IList<ShoppingListItemModel> Items { get; init; } = new List<ShoppingListItemModel>();
    }
    
    public class ShoppingListDetailModelValidator : AbstractValidator<ShoppingListDetailModel>
    {
        public ShoppingListDetailModelValidator()
        {
            RuleFor(sl => sl.Name).NotEmpty().WithMessage(ShoppingListDetailModelResources.ShoppingListNameMissingErrorMessage);
            RuleFor(sl => sl.Name).MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(ShoppingListDetailModelResources.ShoppingListNameMaximumLengthExceededErrorMessage);

            RuleFor(sl => sl.Store).NotEmpty().WithMessage(ShoppingListDetailModelResources.ShoppingListStoreMissingErrorMessage);
            RuleFor(sl => sl.Store).MaximumLength(Constants.MAX_NAME_LENGTH).WithMessage(ShoppingListDetailModelResources.ShoppingListStoreMaximumLengthExceededErrorMessage);

            RuleFor(sl => sl.Description).MaximumLength(Constants.MAX_DESCRIPTION_LENGTH).WithMessage(ShoppingListDetailModelResources.ShoppingListDescriptionMaximumLengthExceededErrorMessage);
        }
    }
}