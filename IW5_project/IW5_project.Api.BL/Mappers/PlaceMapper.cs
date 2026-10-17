using IW5_project.IW5_project.Api.DAL.Common.Entities;
using IW5_project.IW5_project.Common.Models.Place;
using Riok.Mapperly.Abstractions;

namespace IW5_project.IW5_project.Api.BL.Mappers
{
    [Mapper]
    public partial class PlaceMapper
    {
        [MapProperty("items.Count", nameof(PlaceListModel.itemCount))]
        [MapperIgnoreSource(nameof(PlaceEntity.description))]
        [MapperIgnoreSource(nameof(PlaceEntity.OwnerId))]
        public partial PlaceListModel ToListModel(PlaceEntity entity);

        [MapProperty("items.Count", nameof(PlaceDetailModel.itemCount))]
        [MapperIgnoreSource(nameof(PlaceEntity.OwnerId))]
        public partial PlaceDetailModel ToDetailModel(PlaceEntity entity);

        [MapperIgnoreSource(nameof(PlaceDetailModel.itemCount))]
        [MapperIgnoreTarget(nameof(PlaceEntity.items))]
        public partial PlaceEntity ToEntity(PlaceDetailModel model, string? ownerId = null);

        public partial List<PlaceListModel> ToListModels(IEnumerable<PlaceEntity> entities);
    }
}