namespace IW5_project.Common.Models.Place
{
    public class PlaceListModel : IWithId
    {
        public Guid Id { get; init; }
        public string name { get; init; } = null!;
        public string? description { get; init; }
        public int itemCount { get; init; }
        public string imageUrl { get; set; } = string.Empty;
    }
}
