using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.ShoppingList
{
    public class ShoppingListItemModel
    {
        public Guid itemId { get; init; }
        public int quantity { get; init; }
        public bool isBought { get; init; }

        // TODO: uncomment after implementing Item logic
        //public string ItemName { get; init; } = null!;
        //public string? ItemImageUrl { get; init; }
    }

}
