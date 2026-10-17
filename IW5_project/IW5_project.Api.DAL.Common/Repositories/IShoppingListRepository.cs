using IW5_project.API.Common;
using IW5_project.API.DAL.Common.Entities;
using IW5_project.API.DAL.Common.Repositories;

namespace IW5_project.API.DAL.Common.Repositories
{
    public interface IShoppingListRepository : IApiRepository<ShoppingListEntity>
    {
        PagedResult<ShoppingListEntity> GetPaged(ShoppingListQuery query);
    }
}
