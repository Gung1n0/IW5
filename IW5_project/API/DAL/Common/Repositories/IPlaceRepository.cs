using IW5_project.API.Common;
using IW5_project.API.DAL.Common.Entities;
using static IW5_project.API.DAL.Common.Repositories.IApiRepository;

namespace IW5_project.API.DAL.Common.Repositories
{
    public interface IRecipeRepository : IApiRepository<PlaceEntity>
    {
        PagedResult<PlaceEntity> GetPaged(PlaceQuery query);
    }
}
