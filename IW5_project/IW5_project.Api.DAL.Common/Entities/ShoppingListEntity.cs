using IW5_project.Api.DAL.Common.Entities;

namespace IW5_project.Api.DAL.Common.Entities
{
    public record ShoppingListEntity : EntityBase
    {
        public string name { get; set; } = string.Empty;
        public string store { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public ICollection<ShoppingListItemEntity> items { get; set; } = new List<ShoppingListItemEntity>();
        // TODO: add connection to "author"
    }
}
