namespace IW5_project.API.DAL.Common.Entities
{
    public record PlaceEntity : EntityBase
    {
        public string name { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;

        // TODO: add connection to "polozky"
        // TODO: add connection to "author"

    }
}
