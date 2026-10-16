using IW5_project.API.Common.Enums;

namespace IW5_project.API.Common
{
    public class PlaceQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public PlaceSortBy SortBy { get; init; } = PlaceSortBy.Name;
    }
}
