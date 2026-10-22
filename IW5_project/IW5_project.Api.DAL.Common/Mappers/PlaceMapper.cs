using IW5_project.Api.DAL.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace IW5_project.Api.DAL.Common.Mappers
{
    [Mapper]
    public partial class PlaceMapper
    {
        [MapperIgnoreSource(nameof(PlaceEntity.Id))]
        [MapperIgnoreTarget(nameof(PlaceEntity.OwnerId))]
        [MapperIgnoreSource(nameof(PlaceEntity.OwnerId))]
        [MapperIgnoreTarget(nameof(PlaceEntity.OwnerId))]
        //[MapperIgnoreSource(nameof(PlaceEntity.Items))] => ked budu implementovane polozky, tak sa to tu bude ignorovat, lebo sa to bude mapovat cez ineho mappera
        public partial void UpdateEntity(PlaceEntity source, PlaceEntity destination);
    }
}
