using FluentValidation;
using IW5_project.Common.Models.Resources;
namespace IW5_project.Common.Models.ShoppingList
{
    public class AddShoppingListItemModel
    {
        public Guid ItemId { get; init; }
        public int Quantity { get; init; }
    }
    public class AddShoppingListItemModelValidator : AbstractValidator<AddShoppingListItemModel>
    {
        public AddShoppingListItemModelValidator()
        {
            RuleFor(m => m.ItemId)
                .NotEmpty().WithMessage(ShoppingListDetailModelResources.ItemIdRequiredErrorMessage);

            RuleFor(m => m.Quantity)
                .GreaterThan(0).WithMessage(ShoppingListDetailModelResources.ShoppingListQuantityNotWithinRangeErrorMessage);
        }
    }
}