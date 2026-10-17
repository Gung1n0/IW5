using FluentValidation;
using IW5_project.Common.Models.Resources;

namespace IW5_project.Common.Models.ShoppingList
{
    public class UpdateShoppingListItemModel
    {
        public int quantity { get; init; }
        public bool isBought { get; init; }
    }

    public class UpdateShoppingListItemModelValidator : AbstractValidator<UpdateShoppingListItemModel>
    {
        public UpdateShoppingListItemModelValidator()
        {
            RuleFor(m => m.quantity)
                .GreaterThan(0)
                .WithMessage(ShoppingListDetailModelResources.ShoppingListQuantityNotWithinRangeErrorMessage);
        }
    }
}