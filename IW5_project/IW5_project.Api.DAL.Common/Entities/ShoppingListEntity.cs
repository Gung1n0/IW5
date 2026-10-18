using IW5_project.Api.DAL.Common.Entities;

namespace IW5_project.Api.DAL.Common.Entities
{
    public record ShoppingListEntity : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Store { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public ICollection<ShoppingListItemEntity> Items { get; set; } = new List<ShoppingListItemEntity>();
        // TODO: add connection to "author"
    }
}
