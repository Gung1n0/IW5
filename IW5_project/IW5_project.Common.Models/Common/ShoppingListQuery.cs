using IW5_project.Common.Enums;

namespace IW5_project.Common.Models.Common
{
    public class ShoppingListQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public string? StoreId { get; init; }
        public ShoppingListSortby SortBy { get; init; } = ShoppingListSortby.Name;
    }
}
