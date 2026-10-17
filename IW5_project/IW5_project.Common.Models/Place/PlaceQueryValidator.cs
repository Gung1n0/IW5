using FluentValidation;
using IW5_project.IW5_project.Common.Paging;
using IW5_project.IW5_project.Common;
using IW5_project.IW5_project.Common.Models.Resources;

namespace IW5_project.IW5_project.Common.Models.Place  
{
    public class PagedQueryBaseValidator<T> : AbstractValidator<T>
        where T : PagedQueryBase
    {
        public PagedQueryBaseValidator()
        {
            RuleFor(q => q.page)
                .GreaterThanOrEqualTo(1).WithMessage(PlaceDetailModelResources.QueryPageGreaterThanZeroErrorMessage);

            RuleFor(q => q.pageSize)
                .InclusiveBetween(1, Constants.MAX_PAGE_SIZE)
                .WithMessage(PlaceDetailModelResources.QueryPageSizeNotWithinRangeErrorMessage);

            RuleFor(q => q.sortDirection)
                .IsInEnum().WithMessage(PlaceDetailModelResources.QueryInvalidSortByErrorMessage);

            RuleFor(q => q.search)
                .MaximumLength(Constants.MAX_SEARCH_LENGTH)
                .WithMessage(PlaceDetailModelResources.QuerySearchTextExceedsMaxLengthErrorMessage);
        }
    }

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