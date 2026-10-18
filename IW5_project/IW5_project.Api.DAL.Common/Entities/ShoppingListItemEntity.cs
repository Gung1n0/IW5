using IW5_project.Api.DAL.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Api.DAL.Common.Entities
{
    public record ShoppingListItemEntity : EntityBase
    {
        public required int Quantity { get; set; }
        public bool IsBought { get; set; }
        public required Guid ShoppingListId { get; set; }
        public ShoppingListEntity? ShoppingList { get; set; }

        public required Guid ItemId { get; set; }
        public ItemEntity? Item { get; set; }
    }
}
