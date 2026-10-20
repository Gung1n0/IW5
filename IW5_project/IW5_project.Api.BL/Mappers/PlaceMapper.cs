using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.Place;
using Riok.Mapperly.Abstractions;

namespace IW5_project.Api.BL.Mappers
{
    [Mapper]
    public partial class PlaceMapper
    {
        [MapProperty("Items.Count", nameof(PlaceListModel.ItemCount))]
        //[MapperIgnoreSource(nameof(PlaceEntity.description))]
        [MapperIgnoreSource(nameof(PlaceEntity.OwnerId))]
        public partial PlaceListModel ToListModel(PlaceEntity entity);

        [MapProperty("Items.Count", nameof(PlaceDetailModel.ItemCount))]
        [MapperIgnoreSource(nameof(PlaceEntity.OwnerId))]
        public partial PlaceDetailModel ToDetailModel(PlaceEntity entity);

        [MapperIgnoreSource(nameof(PlaceDetailModel.ItemCount))]
        [MapperIgnoreTarget(nameof(PlaceEntity.Items))]
        public partial PlaceEntity ToEntity(PlaceDetailModel model, string? ownerId = null);

        public partial List<PlaceListModel> ToListModels(IEnumerable<PlaceEntity> entities);
    }
}