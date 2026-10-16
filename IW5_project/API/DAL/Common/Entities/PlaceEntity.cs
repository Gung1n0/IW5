namespace IW5_project.API.DAL.Common.Entities
{
    public record PlaceEntity : EntityBase
    {
        public string name { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;

        public List<ItemEntity> items { get; set; } = new List<ItemEntity>();
        // TODO: add connection to "polozky"
        // TODO: add connection to "author"
    }
}
