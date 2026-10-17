using Riok.Mapperly.Abstractions;
using IW5_project.API.DAL.Common.Entities;

namespace IW5_project.API.DAL.Common.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapperIgnoreSource(nameof(UserEntity.Id))]
        public partial void UpdateEntity(UserEntity source, UserEntity destination);
    }
}