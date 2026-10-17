using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.Common;

namespace IW5_project.Api.DAL.Common.Repositories
{
    public interface IPlaceRepository : IApiRepository<PlaceEntity>
    {
        PagedResult<PlaceEntity> GetPaged(PlaceQuery query);
    }
}
