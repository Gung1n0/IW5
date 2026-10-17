using FluentValidation;
using IW5_project.Common.Models.Resources;
namespace IW5_project.Common.Models.ShoppingList
{
    public class AddShoppingListItemModel
    {
        public Guid itemId { get; init; }
        public int quantity { get; init; }
    }
    public class AddShoppingListItemModelValidator : AbstractValidator<AddShoppingListItemModel>
    {
        public AddShoppingListItemModelValidator()
        {
            RuleFor(m => m.itemId)
                .NotEmpty().WithMessage(ShoppingListDetailModelResources.ItemIdRequiredErrorMessage);

            RuleFor(m => m.quantity)
                .GreaterThan(0).WithMessage(ShoppingListDetailModelResources.ShoppingListQuantityNotWithinRangeErrorMessage);
        }
    }
}