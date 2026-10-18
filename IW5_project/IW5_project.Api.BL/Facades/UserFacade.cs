using IW5_project.Api.BL.Mappers;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Common.Models;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.User;

namespace IW5_project.Api.BL.Facades;

public class UserFacade(
    IUserRepository userRepository,
    UserMapper mapper)
    : FacadeBase<IUserRepository, UserEntity>(userRepository), IUserFacade
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly UserMapper mapper = mapper;

    public PagedResult<UserListModel> GetPaged(UserQuery query)
    {
        // Returns a paginated list from the database and remaps the entities to UserListModel
        return userRepository.GetPaged(query).Map(mapper.ToListModel);
    }

    public UserDetailModel? GetById(Guid id)
    {
        var userEntity = userRepository.GetById(id);
        return userEntity is null ? null : mapper.ToDetailModel(userEntity);
    }

    public Guid Create(UserDetailModel userModel, IList<string> userRoles, string? currentUserId = null)
    {
        var isAdmin = userRoles.Contains(UserRoles.Admin);

        // A regular user is not allowed to create a user with the administrator role
        if (userModel.Role == UserRoles.Admin && !isAdmin)
        {
            throw new UnauthorizedAccessException("Only administrators can create admin accounts.");
        }

        var userEntity = mapper.ToEntity(userModel, currentUserId);
        return userRepository.Insert(userEntity);
    }

    public Guid? Update(UserDetailModel userModel, IList<string> userRoles, string? currentUserId = null)
    {
        var existingUser = userRepository.GetById(userModel.Id);
        if (existingUser is null)
        {
            return null;
        }

        var isAdmin = userRoles.Contains(UserRoles.Admin);

        // If the caller is not an admin, they may edit ONLY their own profile
        if (!isAdmin && currentUserId != userModel.Id.ToString())
        {
            throw new UnauthorizedAccessException("Users can only modify their own profile.");
        }

        // A regular user may not change their role to admin (or to anything else)
        if (!isAdmin && userModel.Role != existingUser.Role)
        {
            throw new UnauthorizedAccessException("Users cannot change their assigned role.");
        }

        var userEntity = mapper.ToEntity(userModel);
        return userRepository.Update(userEntity);
    }

    public void Delete(Guid id, IList<string> userRoles, string? currentUserId = null)
    {
        var existingUser = userRepository.GetById(id)
            ?? throw new NotFoundException($"User {id} does not exist.");

        // Only admin can delete users
        if (!userRoles.Contains(UserRoles.Admin))
        {
            throw new UnauthorizedAccessException("Only administrators can delete users.");
        }

        userRepository.Remove(id);
    }
}