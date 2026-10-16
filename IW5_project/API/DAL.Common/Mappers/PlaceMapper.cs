using Riok.Mapperly.Abstractions;
using IW5_project.API.DAL.Common.Entities;

namespace IW5_project.API.DAL.Common.Mappers
{
    [Mapper]
    public partial class PlaceMapper
    {
        [MapperIgnoreSource(nameof(PlaceEntity.Id))]
        public partial void UpdateEntity(PlaceEntity source, PlaceEntity destination);
    }
}
