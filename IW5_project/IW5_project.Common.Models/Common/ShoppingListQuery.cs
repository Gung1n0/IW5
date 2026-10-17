using IW5_project.Common.Models.Enums;
using IW5_project.Api.Common.Models.Common;

namespace IW5_project.Common.Models.Common
{
    public class ShoppingListQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public string? store { get; init; }
        public ShoppingListSortBy SortBy { get; init; } = ShoppingListSortBy.Name;
    }
}
