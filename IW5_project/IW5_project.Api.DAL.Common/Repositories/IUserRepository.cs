using IW5_project.API.Common;
using IW5_project.API.DAL.Common.Entities;

namespace IW5_project.API.DAL.Common.Repositories
{
    public interface IUserRepository : IApiRepository<UserEntity>
    {
        PagedResult<UserEntity> GetPaged(UserQuery query);
    }
}