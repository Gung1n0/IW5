using FluentValidation;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Resources;

namespace IW5_project.Common.Models.ShoppingList
{
    public class ShoppingListQueryValidator : PagedQueryBaseValidator<ShoppingListQuery>
    {
        public ShoppingListQueryValidator()
        {
            RuleFor(q => q.SortBy)
                .IsInEnum().WithMessage(ShoppingListDetailModelResources.QueryInvalidSortFieldErrorMessage);

            RuleFor(q => q.store)
                .MaximumLength(Constants.MAX_NAME_LENGTH)
                .WithMessage(ShoppingListDetailModelResources.QueryStoreExceedsMaxLengthErrorMessage);

            RuleFor(q => q.OwnerId)
                .NotEmpty().WithMessage(ShoppingListDetailModelResources.QueryOwnerIdCannotBeEmptyErrorMessage)
                .When(q => q.OwnerId is not null);
        }
    }
}