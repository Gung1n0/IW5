using IW5_project.Api.Common.Models.Common;
using IW5_project.Common.Enums;
using IW5_project.Common.Models.Enums;

namespace IW5_project.Common.Models.Common
{
    public class ShoppingListQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public string? store { get; init; }
        public ShoppingListSortBy SortBy { get; init; } = ShoppingListSortBy.Name;

        public ShoppingListQuery(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        SortDirection sortDirection = SortDirection.Ascending,
        string? ownerId = null,
        string? store = null,
        ShoppingListSortBy sortBy = ShoppingListSortBy.Name)
        : base(search, page, pageSize, sortDirection)
        {
            OwnerId = ownerId;
            this.store = store;
            SortBy = sortBy;
        }
    }
}
