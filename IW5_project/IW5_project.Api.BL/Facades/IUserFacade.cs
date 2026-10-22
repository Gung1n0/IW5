using IW5_project.Common.Models;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.User;

namespace IW5_project.Api.BL.Facades;

public interface IUserFacade : IAppFacade
{
    PagedResult<UserListModel> GetPaged(UserQuery query);
    UserDetailModel? GetById(Guid id);
    Guid Create(UserDetailModel userModel, IList<string> userRoles, string? currentUserId = null);
    Guid? Update(UserDetailModel userModel, IList<string> userRoles, string? currentUserId = null);
    void Delete(Guid id, IList<string> userRoles, string? currentUserId = null);
}