using IW5_project.Common.Enums;

namespace IW5_project.API.DAL.Common.Entities
{
    public class ItemEntity
    {
        public string name { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public int quantity { get; set; } = 0;
        public DateTime? expirationDate { get; set; } = null;
        public ItemCategories category { get; set; } = ItemCategories.Potraviny;
        public string? description { get; set; } = string.Empty;
        
        // TODO: Add connection to "author"
    }
}