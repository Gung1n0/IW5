using IW5_project.Api.DAL.Common.Entities;
using IW5_project.IW5_project.Api.DAL.Common.Entities;
using Riok.Mapperly.Abstractions;

namespace IW5_project.IW5_project.Api.DAL.Common.Mappers
{
    [Mapper]
    public partial class ShoppingListMapper
    {
        [MapperIgnoreSource(nameof(ShoppingListEntity.Id))]
        [MapperIgnoreSource(nameof(ShoppingListEntity.OwnerId))]
        [MapperIgnoreSource(nameof(ShoppingListEntity.Lines))]
        public partial void UpdateEntity(ShoppingListEntity source, ShoppingListEntity destination);
    }
}1