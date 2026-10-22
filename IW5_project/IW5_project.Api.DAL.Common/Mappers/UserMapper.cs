using IW5_project.Api.DAL.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace IW5_project.Api.DAL.Common.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapperIgnoreSource(nameof(UserEntity.Id))]
        public partial void UpdateEntity(UserEntity source, UserEntity destination);
    }
}