using IW5_project.IW5_project.Common.Enums;

namespace IW5_project.IW5_project.Common.Paging
{
    public class PlaceQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public PlaceSortBy SortBy { get; init; } = PlaceSortBy.Name;
    }
}
