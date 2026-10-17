using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.ShoppingList
{
    public class ShoppingListItemModel
    {
        public Guid ItemId { get; init; }
        public double QuantityToBuy { get; init; }
        public bool IsBought { get; init; }

        // TODO: uncomment after implementing Item logic
        //public string ItemName { get; init; } = null!;
        //public string? ItemImageUrl { get; init; }
    }

}
