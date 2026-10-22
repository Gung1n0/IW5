using IW5_project.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.Item
{
    public class ItemDetailModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public float Quantity { get; set; } = float.MinValue;
        public DateTime? ExpirationDate { get; set; } = DateTime.MinValue;
        public ItemCategories Category { get; set; } = ItemCategories.Potraviny;
        public Guid PlaceId { get; set; } = Guid.Empty;
    }
}
