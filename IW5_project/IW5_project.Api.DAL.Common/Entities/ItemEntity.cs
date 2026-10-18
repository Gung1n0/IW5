using IW5_project.Common.Enums;

namespace IW5_project.Api.DAL.Common.Entities
{
    public record ItemEntity : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public DateTime? ExpirationDate { get; set; } = null;
        public ItemCategories Category { get; set; } = ItemCategories.Potraviny;
        public string? Description { get; set; } = string.Empty;
        
        // TODO: Add connection to "author"
    }
}