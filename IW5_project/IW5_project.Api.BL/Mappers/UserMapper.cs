using Riok.Mapperly.Abstractions;
using IW5_project.API.DAL.Common.Entities;
using IW5_project.Common.Models.User;

namespace IW5_project.API.BL.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapperIgnoreSource(nameof(UserEntity.OwnerId))]
        public partial UserListModel ToListModel(UserEntity entity);
        [MapperIgnoreSource(nameof(UserEntity.OwnerId))]
        public partial UserDetailModel ToDetailModel(UserEntity entity);
        public partial UserEntity ToEntity(UserDetailModel model, string? ownerId = null);
        public partial List<UserListModel> ToListModels(IEnumerable<UserEntity> entities);
    }
}