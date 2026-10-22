using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.Common;

namespace IW5_project.Api.DAL.Common.Repositories
{
   public interface IItemRepository : IApiRepository<ItemEntity>
    {
        PagedResult<ItemEntity> GetPaged(ItemQuery query);
    }
}