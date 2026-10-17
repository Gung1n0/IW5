using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.User;
using Riok.Mapperly.Abstractions;

namespace IW5_project.Api.BL.Mappers
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