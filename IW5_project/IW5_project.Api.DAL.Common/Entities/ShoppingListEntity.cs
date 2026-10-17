using IW5_project.API.DAL.Common.Entities;

namespace IW5_project.Api.DAL.Common.Entities
{
    public record ShoppingListEntity : EntityBase
    {
        public string name { get; set; } = string.Empty;
        public string store { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public List<ItemEntity> items { get; set; } = new List<ItemEntity>();

        // TODO: add connection to "author"
    }
}
