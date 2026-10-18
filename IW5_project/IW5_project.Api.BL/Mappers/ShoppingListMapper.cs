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
        [MapProperty("Items.Count", nameof(ShoppingListListModel.ItemCount))]
        //[MapperIgnoreSource(nameof(ShoppingListEntity.description))]
        [MapperIgnoreSource(nameof(ShoppingListEntity.OwnerId))]
        public partial ShoppingListListModel ToListModel(ShoppingListEntity entity);

        [MapperIgnoreSource(nameof(ShoppingListEntity.OwnerId))]
        public partial ShoppingListDetailModel ToDetailModel(ShoppingListEntity entity);

        [MapperIgnoreSource(nameof(ShoppingListItemEntity.Id))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.OwnerId))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.ShoppingListId))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.ShoppingList))]
        [MapperIgnoreSource(nameof(ShoppingListItemEntity.Item))]      // TODO: remove when itemName/imageUrl are enabled
        public partial ShoppingListItemModel ToItemModel(ShoppingListItemEntity entity);

        [MapperIgnoreSource(nameof(ShoppingListDetailModel.Items))]
        [MapperIgnoreTarget(nameof(ShoppingListEntity.Items))]
        public partial ShoppingListEntity ToEntity(ShoppingListDetailModel model, string? ownerId = null);

        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.OwnerId))]
        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.IsBought))]
        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.ShoppingList))]
        [MapperIgnoreTarget(nameof(ShoppingListItemEntity.Item))]
        public partial ShoppingListItemEntity ToLineEntity(AddShoppingListItemModel model, Guid id, Guid shoppingListId);

        public partial List<ShoppingListListModel> ToListModels(IEnumerable<ShoppingListEntity> entities);
    }
}
