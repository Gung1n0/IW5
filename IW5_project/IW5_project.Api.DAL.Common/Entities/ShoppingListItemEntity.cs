using IW5_project.API.DAL.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Api.DAL.Common.Entities
{
    public record ShoppingListItemEntity : EntityBase
    {
        public required int quantity { get; set; }
        public required bool isBought { get; set; }
        public required Guid shoppingListId { get; set; }
        public ShoppingListEntity? shoppingList { get; set; }

        public required Guid itemId { get; set; }
        public ItemEntity? item { get; set; }
    }
}
