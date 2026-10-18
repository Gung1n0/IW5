using FluentValidation;
using IW5_project.Common.Models.Resources;

namespace IW5_project.Common.Models.ShoppingList
{
    public class UpdateShoppingListItemModel
    {
        public int Quantity { get; init; }
        public bool IsBought { get; init; }
    }

    public class UpdateShoppingListItemModelValidator : AbstractValidator<UpdateShoppingListItemModel>
    {
        public UpdateShoppingListItemModelValidator()
        {
            RuleFor(m => m.Quantity)
                .GreaterThan(0)
                .WithMessage(ShoppingListDetailModelResources.ShoppingListQuantityNotWithinRangeErrorMessage);
        }
    }
}