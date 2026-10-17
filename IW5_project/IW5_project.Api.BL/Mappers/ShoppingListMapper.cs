using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.ShoppingList;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Api.BL.Mappers
{
    [Mapper]
    public partial class ShoppingListMapper
    {
        [MapProperty("items.Count", nameof(ShoppingListListModel.quantity))]
        //[MapperIgnoreSource(nameof(ShoppingListEntity.description))]
        [MapperIgnoreSource(nameof(ShoppingListEntity.OwnerId))]
        public partial ShoppingListListModel ToListModel(ShoppingListEntity entity);

        [MapperIgnoreSource(nameof(ShoppingListEntity.OwnerId))]
        public partial ShoppingListDetailModel ToDetailModel(ShoppingListEntity entity);

        [MapperIgnoreSource(nameof(ShoppingListItemEntity.Id))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.OwnerId))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.shoppingListId))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.shoppingList))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.item))]      // TODO: remove when itemName/imageUrl are enabled
        public partial ShoppingListItemModel ToItemModel(ShoppingListItemEntity entity);

        [MapperIgnoreSource(nameof(ShoppingListDetailModel.items))]
        [MapperIgnoreTarget(nameof(ShoppingListEntity.items))]
        public partial ShoppingListEntity ToEntity(ShoppingListDetailModel model, string? ownerId = null);

        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.OwnerId))]
        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.isBought))]
        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.shoppingList))]
        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.item))]
        public partial ShoppingListItemEntity ToLineEntity(AddShoppingListItemModel model, Guid id, Guid shoppingListId);

        public partial List<ShoppingListListModel> ToListModels(IEnumerable<ShoppingListEntity> entities);
    }
}
