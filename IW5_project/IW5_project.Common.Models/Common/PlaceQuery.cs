using IW5_project.Common.Models.Enums;

namespace IW5_project.Common.Models.Common
{
    public class PlaceQuery : PagedQueryBase
    {
        public string? OwnerId { get; init; }
        public PlaceSortBy SortBy { get; init; } = PlaceSortBy.Name;
    }
}
