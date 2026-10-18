using IW5_project.Api.DAL.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace IW5_project.Api.DAL.Common.Mappers
{
    [Mapper]
    public partial class ShoppingListMapper
    {
        [MapperIgnoreSource(nameof(ShoppingListEntity.Id))]
        [MapperIgnoreSource(nameof(ShoppingListEntity.OwnerId))]
        [MapperIgnoreSource(nameof(ShoppingListEntity.Items))]
        [MapperIgnoreTarget(nameof(ShoppingListEntity.OwnerId))]
        [MapperIgnoreTarget(nameof(ShoppingListEntity.Items))]
        public partial void UpdateEntity(ShoppingListEntity source, ShoppingListEntity destination);
    }
}