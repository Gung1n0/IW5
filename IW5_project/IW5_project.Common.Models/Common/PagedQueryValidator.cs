using FluentValidation;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Common.Models.Resources;

namespace IW5_project.Common.Models.Common
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
}
