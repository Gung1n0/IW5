using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Common.Models.Common;
using IW5_project.Api.DAL.Common.Repositories;


namespace IW5_project.Api.DAL.Common.Repositories
{
    public interface IShoppingListRepository : IApiRepository<ShoppingListEntity>
    {
        PagedResult<ShoppingListEntity> GetPaged(ShoppingListQuery query);
        bool ItemExists(Guid itemId);
        ShoppingListItemEntity? GetLine(Guid shoppingListId, Guid itemId);
        Guid InsertLine(ShoppingListItemEntity line);
        Guid? UpdateLine(Guid shoppingListId, Guid itemId, int quantity, bool isBought);
        bool RemoveLine(Guid shoppingListId, Guid itemId);
    }
}
