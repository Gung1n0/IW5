using IW5_project.IW5_project.Api.DAL.Common.Entities;
using IW5_project.IW5_project.Common.Paging;

namespace IW5_project.IW5_project.Api.DAL.Common.Repositories
{
    public interface IPlaceRepository : IApiRepository<PlaceEntity>
    {
        PagedResult<PlaceEntity> GetPaged(PlaceQuery query);
    }
}
