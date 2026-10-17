using IW5_project.API.Common;
using IW5_project.API.DAL.Common.Entities;
using IW5_project.API.DAL.Common.Repositories;

namespace IW5_project.API.DAL.Common.Repositories
{
    public interface IPlaceRepository : IApiRepository<PlaceEntity>
    {
        PagedResult<PlaceEntity> GetPaged(PlaceQuery query);
    }
}
