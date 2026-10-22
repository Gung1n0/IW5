using IW5_project.Common.Models.Enums;

namespace IW5_project.Common.Models.Item
{
    public class ItemListModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public float Quantity { get; set; } = float.MinValue;
        public DateTime? ExpirationDate { get; set; } = DateTime.MinValue;
        public ItemCategories Category { get; set; } = ItemCategories.Potraviny;
        public string PlaceName { get; set; } = string.Empty;
    }
}
