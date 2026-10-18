namespace IW5_project.Common.Models.Place
{
    public class PlaceListModel : IWithId
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public int ItemCount { get; init; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
