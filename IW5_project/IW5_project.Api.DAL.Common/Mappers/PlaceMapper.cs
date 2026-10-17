using Riok.Mapperly.Abstractions;
using IW5_project.Api.DAL.Common.Entities;

namespace IW5_project.Api.DAL.Common.Mappers
{
    [Mapper]
    public partial class PlaceMapper
    {
        [MapperIgnoreSource(nameof(PlaceEntity.Id))]
        [MapperIgnoreSource(nameof(PlaceEntity.OwnerId))]
        //[MapperIgnoreSource(nameof(PlaceEntity.Items))] => ked budu implementovane polozky, tak sa to tu bude ignorovat, lebo sa to bude mapovat cez ineho mappera
        public partial void UpdateEntity(PlaceEntity source, PlaceEntity destination);
    }
}
