using IW5_project.Api.Common;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Common.BL.Facades;
using IW5_project.Common.Models.Place;
using IW5_project.Common.Models.ShoppingList;
using IW5_project.Common.Models.Common;

namespace IW5_project.Api.BL.Facades
{
    public interface IShoppingListFacade : IAppFacade
    {
        PagedResult<ShoppingListListModel> GetPaged(ShoppingListQuery query);
        ShoppingListDetailModel? GetById(Guid id);
        Guid CreateOrUpdate(ShoppingListDetailModel shoppingListModel, IList<string> userRoles, string? ownerId = null);
        Guid Create(ShoppingListDetailModel shoppingListModel, IList<string> userRoles, string? ownerId = null);
        Guid? Update(ShoppingListDetailModel shoppingListModel, IList<string> userRoles, string? ownerId = null);
        void Delete(Guid id, IList<string> userRoles, string? ownerId = null);
        
        Guid AddItem(Guid shoppingListId, AddShoppingListItemModel model);
        Guid UpdateItem(Guid shoppingListId, Guid itemId, UpdateShoppingListItemModel model);
        void RemoveItem(Guid shoppingListId, Guid itemId);
    }
}
