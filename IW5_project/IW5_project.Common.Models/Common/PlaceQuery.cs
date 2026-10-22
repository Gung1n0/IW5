using IW5_project.Common.Models.Enums;

namespace IW5_project.Common.Models.Common
{

    public class PlaceQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public PlaceSortBy SortBy { get; init; } = PlaceSortBy.Name;

        public PlaceQuery(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        SortDirection sortDirection = SortDirection.Ascending,
        string? ownerId = null,
        PlaceSortBy sortBy = PlaceSortBy.Name)
        : base(search, page, pageSize, sortDirection)
        {
            OwnerId = ownerId;
            SortBy = sortBy;
        }
    }
}
