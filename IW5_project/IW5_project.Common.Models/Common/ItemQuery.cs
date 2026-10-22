using IW5_project.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.Common
{
    public class ItemQuery : PagedQueryBase
    {
            public Guid? PlaceId { get; set; }
            public ItemCategories? Category { get; set; } = ItemCategories.Drogerie;
            public ItemSortBy? Sort { get; set; } = ItemSortBy.Name;
    }
}
