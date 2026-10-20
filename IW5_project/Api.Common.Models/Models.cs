
using IW5_project.Api.DAL.Common.Entities;

namespace IW5_project.API.Common.Models
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public float Amount { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public ItemCategories Category { get; set; }
        public string? Text { get; set; }
        public int UserId { get; set; } //connection to user
        public int PlaceId { get; set; } // connection to place
    }
    public class CreateItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public float Amount { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public ItemCategories Category { get; set; }
        public string? Text { get; set; }
        public int PlaceId { get; set; } // connection to place
        public int UserId { get; set; }
    }
}
