using FluentValidation;
using IW5_project.Common.Models.Resources;
using IW5_project.Common;
using IW5_project.Common.Models.Common;

namespace IW5_project.Common.Models.Place  
{
    public class PlaceQueryValidator : PagedQueryBaseValidator<PlaceQuery>
    {
        public PlaceQueryValidator()
        {
            RuleFor(q => q.SortBy)
                .IsInEnum().WithMessage(PlaceDetailModelResources.QueryInvalidSortFieldErrorMessage);

            RuleFor(q => q.OwnerId)
                .NotEmpty().WithMessage(PlaceDetailModelResources.QueryOwnerIdCannotBeEmptyErrorMessage)
                .When(q => q.OwnerId is not null);
        }
    }
}