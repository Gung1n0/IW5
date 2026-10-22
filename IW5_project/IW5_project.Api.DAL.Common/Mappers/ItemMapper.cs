using IW5_project.Api.DAL.Common.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Api.DAL.Common.Mappers
{
        [Mapper]
        public partial class ItemMapper
        {
        [MapperIgnoreTarget(nameof(ItemEntity.Id))]
        [MapperIgnoreTarget(nameof(ItemEntity.OwnerId))]
        [MapperIgnoreSource(nameof(ItemEntity.Id))]
        [MapperIgnoreSource(nameof(ItemEntity.OwnerId))]
        public partial void UpdateEntity(ItemEntity source, ItemEntity destination);
        }
}
