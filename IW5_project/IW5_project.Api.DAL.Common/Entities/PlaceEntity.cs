namespace IW5_project.Api.DAL.Common.Entities
{
    public record PlaceEntity : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public List<ItemEntity> Items { get; set; } = new List<ItemEntity>();
        // TODO: add connection to "author"
    }
}
