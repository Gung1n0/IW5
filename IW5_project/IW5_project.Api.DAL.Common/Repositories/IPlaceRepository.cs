using IW5_project.Api.Common.Models.Common;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Repositories;

namespace IW5_project.Api.DAL.Common.Repositories
{
    public interface IPlaceRepository : IApiRepository<PlaceEntity>
    {
        PagedResult<PlaceEntity> GetPaged(PlaceQuery query);
    }
}
