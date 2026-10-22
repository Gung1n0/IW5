using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models.Item;
using Riok.Mapperly.Abstractions;

namespace IW5_project.Api.BL.Mappers
{
    [Mapper]
    public partial class ItemMapper
    {
        [MapperIgnoreSource(nameof(ItemEntity.OwnerId))]
        [MapperIgnoreSource(nameof(ItemEntity.Place))]
        public partial ItemDetailModel ToDetailModel(ItemEntity entity);

        [MapProperty("Place.Name", nameof(ItemListModel.PlaceName))]
        [MapperIgnoreSource(nameof(ItemEntity.OwnerId))]
        public partial ItemListModel ToListModel(ItemEntity entity);

        [MapperIgnoreTarget(nameof(ItemEntity.Place))]
        public partial ItemEntity ToEntity(ItemDetailModel model, string? ownerId = null);

        public partial List<ItemListModel> ToListModels(IEnumerable<ItemEntity> entities);
    }
}